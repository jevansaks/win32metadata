const assert = require('node:assert/strict');
const { execFileSync, spawnSync } = require('node:child_process');
const fs = require('node:fs');
const os = require('node:os');
const path = require('node:path');
const { test } = require('node:test');
const { referenceSha, verdict, aggregate } = require('../scripts/migration-qualification.cjs');
const { compare, sha256 } = require('../scripts/migration-qualification.cjs');
const { verify } = require('../scripts/verify-migration-evidence.cjs');
const root = path.resolve(__dirname, '..', '..');
const workflow = fs.readFileSync(path.join(root, '.github', 'workflows', 'pr-validation.yml'), 'utf8').replace(/\r\n/g, '\n');
const jobs = new Map(workflow.split(/^  ([a-z][a-z-]*):\n/gm).slice(1).reduce((pairs, item, index, all) => {
  if (index % 2 === 0) pairs.push([item, all[index + 1]]);
  return pairs;
}, []));
const hash = 'a'.repeat(64);
const sha = 'b'.repeat(40);
const passingDependencies = Object.fromEntries(
  ['changes', 'workflow-tests', 'tooling-tests', 'scrape', 'build-test', 'qualification'].map(name => [name, 'success']));
function receipt() {
  const sample = {
    exitCode: 0, seconds: 100, peakWorkingSetBytes: 1000, memorySamples: 10,
    outputHash: hash, startedAt: '2026-11-01T00:00:00Z', endedAt: '2026-11-01T00:01:40Z'
  };
  return {
    referenceSha, candidateSha: sha, sdkVersion: '10.0.28000.2705', clangSharpVersion: '21.1.8.4',
    reference: { inputHash: hash, toolchainHash: hash },
    candidate: { inputHash: hash, toolchainHash: hash },
    pairs: [{
      reference: { ...sample }, candidate: { ...sample },
      drift: { equal: true, reference: { sha256: hash }, candidate: { sha256: hash } }
    }]
  };
}

test('required checks cover every PR base without path-based omission', () => {
  assert.match(workflow, /on:\n  pull_request:\n  push:\n/);
  assert.match(workflow, /branches: \[main, feature\/header-annotations\]/);
  assert.doesNotMatch(workflow, /paths-ignore:/);
  for (const [job, name] of [
    ['tooling-tests', 'Tooling tests'], ['migration', 'Migration qualification'],
    ['workflow-tests', 'Workflow regression tests'], ['build-test', "'Build, test, package'"]
  ]) assert.match(jobs.get(job), new RegExp(`^    name: ${name}$`, 'm'));
  assert.doesNotMatch(jobs.get('tooling-tests'), /^    (needs|if):/m);
  assert.match(jobs.get('tooling-tests'), /fetch-depth: 0/);
  assert.match(jobs.get('tooling-tests'), /lfs: true/);
  assert.match(jobs.get('changes'), /context\.repo\.owner === 'jevansaks'/);
  assert.match(jobs.get('workflow-tests'), /node --test \.github\/tests\/\*\.test\.cjs/);
});

test('aggregate is always scheduled and requires every exact dependency', () => {
  assert.match(jobs.get('migration'), /needs: \[changes, workflow-tests, tooling-tests, scrape, build-test, qualification\]/);
  assert.match(jobs.get('migration'), /if: always\(\) && github\.repository == 'jevansaks\/win32metadata'/);
  assert.match(jobs.get('migration'), /Require every qualification dependency/);
  assert.match(jobs.get('migration'), /verify-migration-evidence\.cjs evidence/);
  assert.match(jobs.get('build-test'), /if: always\(\)/);
  assert.match(jobs.get('build-test'), /needs: \[changes, scrape, qualification\]/);
  assert.match(jobs.get('build-test'), /needs\.changes\.result != 'success'/);
  assert.match(jobs.get('build-test'), /needs\.scrape\.result != 'success'/);
  assert.match(jobs.get('build-test'), /needs\.qualification\.result != 'success'/);
  assert.deepEqual(aggregate(passingDependencies), { passed: true, errors: [] });
  for (const dependency of Object.keys(passingDependencies)) {
    for (const result of ['skipped', 'failure', 'cancelled', undefined]) {
      assert.equal(aggregate({ ...passingDependencies, [dependency]: result }).passed, false, `${dependency} ${result}`);
    }
  }
});

test('all architectures, frozen inputs, finite timeouts and retained artifacts remain wired', () => {
  assert.match(jobs.get('scrape'), /arch: \[x64, x86, arm64\]/);
  for (const arch of ['x64', 'x86', 'arm64']) {
    assert.match(jobs.get('build-test'), new RegExp(`name: generated_${arch}`));
  }
  assert.match(jobs.get('qualification'), new RegExp(`ref: ${referenceSha}`));
  assert.match(jobs.get('qualification'), /lfs: true/);
  assert.match(jobs.get('qualification'), /TestMigrationQualification\.ps1 -ReferenceDirectory \.qualification\\reference/);
  for (const job of ['tooling-tests', 'qualification']) {
    assert.match(jobs.get(job), /name: Retain [^\n]+\n        if: always\(\)/);
    assert.match(jobs.get(job), /retention-days: 30/);
    assert.doesNotMatch(jobs.get(job), /continue-on-error:/);
  }
  for (const name of ['workflow-tests', 'tooling-tests', 'qualification', 'migration', 'changes', 'scrape', 'build-test']) {
    assert.match(jobs.get(name), /timeout-minutes: \d+/);
  }
  const upload = fs.readFileSync(path.join(root, '.github', 'actions', 'upload-artifact-with-retry', 'action.yml'), 'utf8');
  assert.equal((upload.match(/if-no-files-found: error/g) || []).length, 2);
  const owners = fs.readFileSync(path.join(root, '.github', 'CODEOWNERS'), 'utf8');
  assert.match(owners, /\/\.github\/ @jevansaks/);
  for (const script of ['TestTooling', 'TestMigrationQualification', 'DoTests']) {
    assert.ok(owners.includes(`/scripts/${script}.ps1 @jevansaks`));
  }
});

test('valid zero-drift receipt passes, with exact budgets rather than rounded ratios', () => {
  const data = receipt();
  data.pairs[0].candidate.seconds = 110;
  data.pairs[0].candidate.peakWorkingSetBytes = 1150;
  assert.equal(verdict(data).passed, true);
  for (const [metric, value] of [['seconds', 110.001], ['peakWorkingSetBytes', 1150.001]]) {
    const exceeded = receipt();
    exceeded.pairs[0].candidate[metric] = value;
    assert.equal(verdict(exceeded).passed, false);
    exceeded.pairs.push(structuredClone(exceeded.pairs[0]));
    assert.equal(verdict(exceeded).passed, false);
    exceeded.pairs.push(structuredClone(exceeded.pairs[0]));
    assert.equal(verdict(exceeded).passed, false);
  }
});

for (const [name, change] of [
  ['wrong reference commit', data => { data.referenceSha = sha; }],
  ['missing reference', data => { delete data.reference; }],
  ['mismatched reference inputs', data => { data.reference.inputHash = 'c'.repeat(64); }],
  ['mismatched toolchain', data => { data.candidate.toolchainHash = 'c'.repeat(64); }],
  ['wrong SDK', data => { data.sdkVersion = 'latest'; }],
  ['wrong ClangSharp', data => { data.clangSharpVersion = 'latest'; }],
  ['generation failed', data => { data.pairs[0].candidate.exitCode = 1; }],
  ['missing output', data => { delete data.pairs[0].candidate.outputHash; }],
  ['mismatched output', data => { data.pairs[0].candidate.outputHash = 'c'.repeat(64); }],
  ['comparison failed', data => { delete data.pairs[0].drift; }],
  ['final metadata drift', data => { data.pairs[0].drift.equal = false; }],
  ['missing measurement', data => { delete data.pairs[0].reference.seconds; }],
  ['no memory samples', data => { data.pairs[0].reference.memorySamples = 0; }],
  ['empty run', data => { data.pairs = []; }]
]) {
  test(`qualification fails closed: ${name}`, () => {
    const data = receipt();
    change(data);
    assert.equal(verdict(data).passed, false);
  });
}

test('a noisy two-sample budget remains failing until a third pair resolves it', () => {
  const data = receipt();
  data.pairs[0].candidate.seconds = 111;
  data.pairs.push(receipt().pairs[0]);
  assert.equal(verdict(data).passed, false);
  data.pairs.push(receipt().pairs[0]);
  assert.equal(verdict(data).passed, true);
});

test('comparison and missing evidence commands fail rather than echoing success', () => {
  const directory = fs.mkdtempSync(path.join(os.tmpdir(), 'migration-qualification-'));
  try {
    const output = path.join(directory, 'invalid.winmd');
    fs.writeFileSync(output, 'not metadata');
    assert.notEqual(spawnSync(process.execPath, [
      path.join(root, '.github', 'scripts', 'migration-qualification.cjs'), 'compare',
      output, output, path.join(directory, 'drift.json')
    ]).status, 0);
    assert.notEqual(spawnSync(process.execPath, [
      path.join(root, '.github', 'scripts', 'verify-migration-evidence.cjs'), directory
    ]).status, 0);
  } finally { fs.rmSync(directory, { recursive: true, force: true }); }
});

function winmdFixture(mvidByte) {
  const bytes = Buffer.alloc(1024);
  bytes.writeUInt16LE(0x5a4d, 0);
  bytes.writeUInt32LE(128, 0x3c);
  bytes.writeUInt32LE(0x4550, 128);
  bytes.writeUInt16LE(1, 134);
  bytes.writeUInt16LE(224, 148);
  bytes.writeUInt16LE(0x10b, 152);
  bytes.writeUInt32LE(0x2000, 152 + 96 + 14 * 8);
  const section = 152 + 224;
  bytes.writeUInt32LE(0x2000, section + 12);
  bytes.writeUInt32LE(512, section + 16);
  bytes.writeUInt32LE(512, section + 20);
  bytes.writeUInt32LE(0x2040, 512 + 8);
  bytes.writeUInt32LE(256, 512 + 12);
  const root = 576;
  bytes.writeUInt32LE(0x424a5342, root);
  bytes.writeUInt32LE(4, root + 12);
  bytes.writeUInt16LE(2, root + 22);
  bytes.writeUInt32LE(64, root + 24);
  bytes.writeUInt32LE(48, root + 28);
  bytes.write('#~\0', root + 32, 'ascii');
  bytes.writeUInt32LE(128, root + 36);
  bytes.writeUInt32LE(16, root + 40);
  bytes.write('#GUID\0', root + 44, 'ascii');
  const tables = root + 64;
  bytes.writeBigUInt64LE(1n, tables + 8);
  bytes.writeUInt32LE(1, tables + 24);
  bytes.writeUInt16LE(1, tables + 32);
  bytes.fill(mvidByte, root + 128, root + 144);
  return bytes;
}

test('aggregate evidence verifier rejects missing, mismatched and stale retained evidence', () => {
  const directory = fs.mkdtempSync(path.join(os.tmpdir(), 'migration-evidence-'));
  const originalSha = process.env.GITHUB_SHA;
  try {
    delete process.env.GITHUB_SHA;
    const data = receipt();
    data.passed = true;
    const first = winmdFixture(1);
    const second = winmdFixture(2);
    data.pairs[0].drift = compare(first, second);
    assert.equal(data.pairs[0].drift.equal, true);
    fs.writeFileSync(path.join(directory, 'pair-1-reference.winmd'), first);
    fs.writeFileSync(path.join(directory, 'pair-1-candidate.winmd'), second);
    for (const side of ['reference', 'candidate']) {
      for (const [file, key] of [['inputs.sha256', 'inputHash'], ['toolchain.txt', 'toolchainHash']]) {
        fs.writeFileSync(path.join(directory, `${side}-${file}`), 'bound inputs');
        data[side][key] = sha256(Buffer.from('bound inputs'));
      }
      data.pairs[0][side].outputHash = data.pairs[0].drift[side].sha256;
      for (const [suffix, key] of [['.binlog', 'binlogHash'], ['.log', 'logHash'], ['-memory.csv', 'memoryLogHash']]) {
        fs.writeFileSync(path.join(directory, `pair-1-${side}${suffix}`), 'raw evidence');
        data.pairs[0][side][key] = sha256(Buffer.from('raw evidence'));
      }
      fs.writeFileSync(path.join(directory, `pair-1-${side}-sample.json`), JSON.stringify(data.pairs[0][side]));
    }
    const receiptFile = path.join(directory, 'receipt.json');
    fs.writeFileSync(receiptFile, JSON.stringify(data));
    assert.equal(verify(directory).passed, true);
    process.env.GITHUB_SHA = sha;
    assert.equal(verify(directory).passed, true);
    process.env.GITHUB_SHA = 'c'.repeat(40);
    assert.throws(() => verify(directory), /different candidate/);
    delete process.env.GITHUB_SHA;
    const candidateFile = path.join(directory, 'pair-1-candidate.winmd');
    fs.unlinkSync(candidateFile);
    assert.throws(() => verify(directory), /ENOENT/);
    const changed = Buffer.from(second);
    changed[900] ^= 1;
    fs.writeFileSync(candidateFile, changed);
    assert.throws(() => verify(directory), /Output binding/);
    fs.writeFileSync(candidateFile, second);
    fs.writeFileSync(path.join(directory, 'pair-1-candidate-memory.csv'), 'tampered');
    assert.throws(() => verify(directory), /Evidence hash mismatch/);
  } finally {
    if (originalSha === undefined) delete process.env.GITHUB_SHA;
    else process.env.GITHUB_SHA = originalSha;
    fs.rmSync(directory, { recursive: true, force: true });
  }
});

for (const [name, code, executed, failed, passed] of [
  ['intentional command failure', 1, 1, 0, 1],
  ['empty TRX', 0, 0, 0, 0],
  ['failed tests', 0, 2, 1, 1],
  ['unexecuted or skipped tests', 0, 2, 0, 1],
  ['nonempty passing suites', 0, 2, 0, 2]
]) {
  test(`local tooling entry point rejects ${name}`, () => {
    const directory = fs.mkdtempSync(path.join(os.tmpdir(), 'tooling-verdict-'));
    try {
      const script = `
function dotnet {
  $results = $args[([Array]::IndexOf($args, '--results-directory') + 1)]
  $logger = $args[([Array]::IndexOf($args, '--logger') + 1)]
  $file = $logger.Replace('trx;LogFileName=', '')
  '<TestRun><ResultSummary outcome="Completed"><Counters executed="${executed}" failed="${failed}" passed="${passed}" /></ResultSummary></TestRun>' | Set-Content (Join-Path $results $file)
  $global:LASTEXITCODE = ${code}
}
& '${path.join(root, 'scripts', 'TestTooling.ps1').replace(/'/g, "''")}' -ResultsDirectory '${directory.replace(/'/g, "''")}'
`;
      const result = spawnSync('pwsh', ['-NoProfile', '-NonInteractive', '-Command', script], { encoding: 'utf8' });
      assert.equal(result.status === 0, name === 'nonempty passing suites', result.stdout + result.stderr);
      assert.ok(fs.statSync(path.join(directory, 'tooling.log')).size > 0, 'Failures must retain a transcript');
    } finally { fs.rmSync(directory, { recursive: true, force: true }); }
  });
}
