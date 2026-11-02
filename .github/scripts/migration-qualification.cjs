const fs = require('node:fs');
const crypto = require('node:crypto');

const referenceSha = '86ed37b2846440297afb087c4718acd1a1fb4ad8';
const sha256 = bytes => crypto.createHash('sha256').update(bytes).digest('hex');

// Compare the complete PE, not a decompiler's lossy view of the API.
// Only the emitter's random Module MVID and PE creation timestamp are normalized.
function normalizeWinmd(input) {
  const bytes = Buffer.from(input);
  function range(offset, size) {
    if (!Number.isSafeInteger(offset) || offset < 0 || offset + size > bytes.length) {
      throw new Error('Truncated or invalid winmd');
    }
    return offset;
  }
  const u16 = offset => bytes.readUInt16LE(range(offset, 2));
  const u32 = offset => bytes.readUInt32LE(range(offset, 4));
  if (u16(0) !== 0x5a4d) throw new Error('Missing DOS header');
  const pe = u32(0x3c);
  if (u32(pe) !== 0x4550) throw new Error('Missing PE header');
  const sections = u16(pe + 6);
  const optional = pe + 24;
  const magic = u16(optional);
  if (magic !== 0x10b && magic !== 0x20b) throw new Error('Invalid optional header');
  const directories = optional + (magic === 0x10b ? 96 : 112);
  function rva(value, size) {
    const table = optional + u16(pe + 20);
    for (let i = 0; i < sections; i++) {
      const section = table + i * 40;
      const address = u32(section + 12);
      const length = u32(section + 16);
      if (value >= address && value + size <= address + length) {
        return range(u32(section + 20) + value - address, size);
      }
    }
    throw new Error('Unmapped metadata RVA');
  }
  const cli = rva(u32(directories + 14 * 8), 16);
  const root = rva(u32(cli + 8), u32(cli + 12));
  if (u32(root) !== 0x424a5342) throw new Error('Missing metadata root');
  let cursor = root + 16 + Math.ceil(u32(root + 12) / 4) * 4;
  const streamCount = u16(cursor + 2);
  cursor += 4;
  const streams = new Map();
  for (let i = 0; i < streamCount; i++) {
    const offset = u32(cursor);
    const size = u32(cursor + 4);
    const start = cursor + 8;
    let end = start;
    while (bytes[range(end, 1)] !== 0) end++;
    const name = bytes.toString('ascii', start, end);
    if (streams.has(name)) throw new Error('Duplicate metadata stream');
    streams.set(name, { offset: range(root + offset, size), size });
    cursor = start + Math.ceil((end - start + 1) / 4) * 4;
  }
  const tables = streams.get('#~');
  const guids = streams.get('#GUID');
  if (!tables || !guids) throw new Error('Missing metadata tables or GUID heap');
  const heapSizes = bytes[range(tables.offset + 6, 1)];
  const valid = bytes.readBigUInt64LE(range(tables.offset + 8, 8));
  if (!(valid & 1n)) throw new Error('Missing Module row');
  cursor = tables.offset + 24;
  for (let i = 0; i < 64; i++) {
    if (valid & (1n << BigInt(i))) {
      const rows = u32(cursor);
      if (i === 0 && rows !== 1) throw new Error('Expected exactly one Module');
      cursor += 4;
    }
  }
  const stringSize = heapSizes & 1 ? 4 : 2;
  const guidSize = heapSizes & 2 ? 4 : 2;
  const readIndex = offset => guidSize === 4 ? u32(offset) : u16(offset);
  const mvidIndex = readIndex(cursor + 2 + stringSize);
  if (u16(cursor) !== 0 || mvidIndex < 1 ||
      readIndex(cursor + 2 + stringSize + guidSize) !== 0 ||
      readIndex(cursor + 2 + stringSize + 2 * guidSize) !== 0 ||
      mvidIndex * 16 > guids.size) {
    throw new Error('Unexpected Module generation or MVID');
  }
  const mvidOffset = guids.offset + (mvidIndex - 1) * 16;
  const normalization = {
    mvidOffset, mvid: bytes.subarray(mvidOffset, mvidOffset + 16).toString('hex'),
    timestampOffset: pe + 8, timestamp: u32(pe + 8)
  };
  bytes.fill(0, mvidOffset, mvidOffset + 16);
  bytes.fill(0, pe + 8, pe + 12);
  return { bytes, normalization };
}

function compare(first, second) {
  const a = normalizeWinmd(first);
  const b = normalizeWinmd(second);
  const equal = a.bytes.equals(b.bytes);
  let firstDifference = null;
  if (!equal) {
    firstDifference = 0;
    while (firstDifference < Math.min(a.bytes.length, b.bytes.length) &&
           a.bytes[firstDifference] === b.bytes[firstDifference]) firstDifference++;
  }
  return {
    equal, firstDifference,
    reference: { sha256: sha256(first), normalizedSha256: sha256(a.bytes), ...a.normalization },
    candidate: { sha256: sha256(second), normalizedSha256: sha256(b.bytes), ...b.normalization }
  };
}

function verdict(receipt) {
  const errors = [];
  if (receipt.referenceSha !== referenceSha) errors.push('Reference SHA does not match the frozen snapshot');
  if (!/^[0-9a-f]{40}$/.test(receipt.candidateSha || '')) errors.push('Missing candidate SHA');
  if (receipt.sdkVersion !== '10.0.28000.2705' || receipt.clangSharpVersion !== '21.1.8.4') {
    errors.push('SDK or ClangSharp version does not match qualification inputs');
  }
  for (const name of ['inputHash', 'toolchainHash']) {
    if (!/^[0-9a-f]{64}$/.test(receipt.reference?.[name] || '') ||
        receipt.reference[name] !== receipt.candidate?.[name]) errors.push(`Missing/mismatched ${name}`);
  }
  const pairs = receipt.pairs;
  if (!Array.isArray(pairs) || pairs.length < 1 || pairs.length > 3) {
    errors.push('Expected one to three full paired samples');
    return { passed: false, errors };
  }
  const breaches = [];
  for (const pair of pairs) {
    let valid = true;
    for (const side of ['reference', 'candidate']) {
      const sample = pair[side];
      if (!sample || sample.exitCode !== 0 || !/^[0-9a-f]{64}$/.test(sample.outputHash || '') ||
          !Number.isFinite(sample.seconds) || sample.seconds <= 0 ||
          !Number.isFinite(sample.peakWorkingSetBytes) || sample.peakWorkingSetBytes <= 0 ||
          !Number.isFinite(sample.memorySamples) || sample.memorySamples < 1 ||
          !sample.startedAt || !sample.endedAt) {
        errors.push(`Missing/failed generation or output: ${side}`);
        valid = false;
      }
    }
    if (!pair.drift?.equal ||
        pair.drift.reference?.sha256 !== pair.reference?.outputHash ||
        pair.drift.candidate?.sha256 !== pair.candidate?.outputHash) {
      errors.push('Missing, mismatched or differing final metadata');
    }
    if (valid) breaches.push({
      time: pair.candidate.seconds > pair.reference.seconds * 1.10,
      memory: pair.candidate.peakWorkingSetBytes > pair.reference.peakWorkingSetBytes * 1.15
    });
  }
  // A breach needs another paired sample. A mixed outcome remains non-green;
  // a third sample resolves a majority, rather than accepting a noisy reference.
  for (const metric of ['time', 'memory']) {
    const count = breaches.filter(pair => pair[metric]).length;
    if (count && pairs.length === 1) errors.push(`${metric} budget needs confirmation`);
    else if (count && pairs.length === 2) errors.push(`${metric} budget breached or unresolved`);
    else if (count >= 2) errors.push(`Confirmed ${metric} budget breach`);
  }
  return { passed: errors.length === 0, errors, breaches };
}

function aggregate(results) {
  const required = ['changes', 'workflow-tests', 'tooling-tests', 'scrape', 'build-test', 'qualification'];
  const errors = required.filter(name => results[name] !== 'success')
    .map(name => `${name}: ${results[name] || 'missing'}`);
  return { passed: errors.length === 0, errors };
}

module.exports = { referenceSha, sha256, normalizeWinmd, compare, verdict, aggregate };

if (require.main === module) {
  try {
    const [command, ...args] = process.argv.slice(2);
    let result;
    if (command === 'compare') {
      result = compare(fs.readFileSync(args[0]), fs.readFileSync(args[1]));
      fs.writeFileSync(args[2], JSON.stringify(result, null, 2));
      process.exitCode = result.equal ? 0 : 1;
    } else if (command === 'verdict') {
      result = verdict(JSON.parse(fs.readFileSync(args[0], 'utf8')));
      fs.writeFileSync(args[1], JSON.stringify(result, null, 2));
      process.exitCode = result.passed ? 0 : 1;
    } else if (command === 'aggregate') {
      result = aggregate(JSON.parse(args[0]));
      process.exitCode = result.passed ? 0 : 1;
    } else throw new Error('Unknown qualification command');
    console.log(JSON.stringify(result, null, 2));
  } catch (error) {
    console.error(error.stack);
    process.exitCode = 1;
  }
}
