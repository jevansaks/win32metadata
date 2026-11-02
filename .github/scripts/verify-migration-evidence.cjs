const fs = require('node:fs');
const path = require('node:path');
const { verdict, compare, sha256 } = require('./migration-qualification.cjs');

function verify(directory) {
  const receipt = JSON.parse(fs.readFileSync(path.join(directory, 'receipt.json'), 'utf8'));
  const result = verdict(receipt);
  if (!receipt.passed || !result.passed) throw new Error(`Qualification failed: ${result.errors.join('; ')}`);
  if (process.env.GITHUB_SHA && receipt.candidateSha !== process.env.GITHUB_SHA) {
    throw new Error('Evidence belongs to a different candidate commit');
  }
  for (const side of ['reference', 'candidate']) {
    for (const [file, hash] of [['inputs.sha256', 'inputHash'], ['toolchain.txt', 'toolchainHash']]) {
      if (sha256(fs.readFileSync(path.join(directory, `${side}-${file}`))) !== receipt[side][hash]) {
        throw new Error(`Evidence hash mismatch: ${side}-${file}`);
      }
    }
  }
  for (const [index, pair] of receipt.pairs.entries()) {
    const prefix = `pair-${index + 1}`;
    const first = fs.readFileSync(path.join(directory, `${prefix}-reference.winmd`));
    const second = fs.readFileSync(path.join(directory, `${prefix}-candidate.winmd`));
    const drift = compare(first, second);
    if (!drift.equal || drift.reference.sha256 !== pair.reference.outputHash ||
        drift.candidate.sha256 !== pair.candidate.outputHash) throw new Error(`Output binding mismatch: ${prefix}`);
    for (const side of ['reference', 'candidate']) {
      for (const [suffix, key] of [['.binlog', 'binlogHash'], ['.log', 'logHash'], ['-memory.csv', 'memoryLogHash']]) {
        const file = path.join(directory, `${prefix}-${side}${suffix}`);
        if (!fs.statSync(file).isFile() || fs.statSync(file).size === 0) throw new Error(`Missing evidence: ${file}`);
        if (sha256(fs.readFileSync(file)) !== pair[side][key]) throw new Error(`Evidence hash mismatch: ${file}`);
      }
      const sample = JSON.parse(fs.readFileSync(path.join(directory, `${prefix}-${side}-sample.json`), 'utf8'));
      for (const key of Object.keys(pair[side])) {
        if (sample[key] !== pair[side][key]) throw new Error(`Sample binding mismatch: ${prefix}-${side} ${key}`);
      }
    }
  }
  return result;
}

module.exports = { verify };
if (require.main === module) {
  try { console.log(verify(process.argv[2])); }
  catch (error) { console.error(error.stack); process.exitCode = 1; }
}
