const assert = require('node:assert/strict');
const { execFileSync } = require('node:child_process');
const fs = require('node:fs');
const path = require('node:path');
const { test } = require('node:test');
const os = require('node:os');
const { compare, normalizeWinmd } = require('../scripts/migration-qualification.cjs');

test('repeated real emission differs only in Module MVID and PE timestamp', () => {
  const root = path.resolve(__dirname, '..', '..');
  const directory = path.join(root, 'bin', 'emission-evidence');
  const source = path.join(directory, 'source');
  fs.mkdirSync(path.join(source, 'x64'), { recursive: true });
  fs.writeFileSync(path.join(source, 'x64', 'Test.cs'), `
using System;
using System.Runtime.InteropServices;
namespace Test {
  [Flags] public enum Mode { None = 0, First = 1, Second = 2 }
  [StructLayout(LayoutKind.Sequential, Pack = 8)]
  public struct Value { public int Number; public Mode Mode; }
  public static class Apis {
    [DllImport("kernel32.dll", ExactSpelling = true, SetLastError = true)]
    public static extern int Example(int value);
  }
}`);
  const outputs = [];
  for (let index = 0; index < 3; index++) {
    const outputDirectory = path.join(directory, `emission-${index}`);
    fs.mkdirSync(outputDirectory, { recursive: true });
    const output = path.join(outputDirectory, 'Windows.Win32.winmd');
    if (fs.existsSync(output)) fs.unlinkSync(output);
    const log = execFileSync('dotnet', [
      path.join(root, 'bin', 'Release', 'net10.0', 'ClangSharpSourceToWinmd.dll'),
      '--sourceDir', source, '--arch', 'x64', '--version', '1.0.0.0',
      '--outputFileName', output
    ], { cwd: root, encoding: 'utf8', timeout: 60000 });
    fs.writeFileSync(path.join(outputDirectory, 'emission.log'), log);
    outputs.push(fs.readFileSync(output));
  }
  const proof = outputs.slice(1).map(output => compare(outputs[0], output));
  fs.writeFileSync(path.join(directory, 'repeat-emission.json'), JSON.stringify(proof, null, 2));
  assert.ok(proof.every(result => result.equal), JSON.stringify(proof));
  assert.notEqual(proof[0].reference.mvid, proof[0].candidate.mvid, 'Expected random emitter MVIDs');

  // Every other byte remains observable, including native GUID attribute blobs,
  // import flags, enum values, signatures, namespace strings and layout tables.
  const { normalization } = normalizeWinmd(outputs[0]);
  const protectedOffsets = new Set([
    ...Array.from({ length: 16 }, (_, i) => normalization.mvidOffset + i),
    ...Array.from({ length: 4 }, (_, i) => normalization.timestampOffset + i)
  ]);
  const normalized = normalizeWinmd(outputs[0]).bytes;
  for (let offset = 0; offset < outputs[0].length; offset++) {
    if (protectedOffsets.has(offset)) continue;
    const changed = Buffer.from(outputs[0]);
    changed[offset] ^= 1;
    try {
      assert.notDeepEqual(normalizeWinmd(changed).bytes, normalized, `Hidden byte at ${offset}`);
    } catch (error) {
      if (error.code === 'ERR_ASSERTION') throw error;
      // Corrupting a structural header is rejected rather than normalized away.
    }
  }
});

test('real failing generation deletes stale outputs and retains a failing sample and logs', () => {
  const root = path.resolve(__dirname, '..', '..');
  const directory = fs.mkdtempSync(path.join(os.tmpdir(), 'migration-generation-failure-'));
  try {
    fs.mkdirSync(path.join(directory, 'generation', 'WinSDK'), { recursive: true });
    fs.mkdirSync(path.join(directory, 'generation', 'WinSDK', 'obj', 'generated'), { recursive: true });
    fs.mkdirSync(path.join(directory, 'bin'), { recursive: true });
    fs.mkdirSync(path.join(directory, 'evidence'), { recursive: true });
    fs.writeFileSync(path.join(directory, 'generation', 'WinSDK', 'Failure.proj'),
      '<Project><Target Name="EmitWinmd"><Error Text="Intentional qualification generation failure" /></Target></Project>');
    const output = path.join(directory, 'bin', 'Windows.Win32.winmd');
    fs.writeFileSync(output, 'stale output');
    const script = `
$ErrorActionPreference = 'Stop'
$errors = $null
$ast = [Management.Automation.Language.Parser]::ParseFile('${path.join(root, 'scripts', 'TestMigrationQualification.ps1').replace(/'/g, "''")}', [ref]$null, [ref]$errors)
$definition = $ast.Find({ param($node) $node -is [Management.Automation.Language.FunctionDefinitionAst] -and $node.Name -eq 'Measure-Generation' }, $true)
Invoke-Expression $definition.Extent.Text
$ResultsDirectory = '${path.join(directory, 'evidence').replace(/'/g, "''")}'
Measure-Generation '${directory.replace(/'/g, "''")}' reference 1
`;
    assert.throws(() => execFileSync('pwsh', ['-NoProfile', '-NonInteractive', '-Command', script],
      { encoding: 'utf8', timeout: 60000 }));
    assert.equal(fs.existsSync(output), false, 'Stale output must not be certified');
    const sample = JSON.parse(fs.readFileSync(path.join(directory, 'evidence', 'pair-1-reference-sample.json')));
    assert.notEqual(sample.exitCode, 0);
    assert.equal(sample.outputHash, null);
    const log = fs.readFileSync(path.join(directory, 'evidence', 'pair-1-reference.log'), 'utf8');
    assert.match(log, /Intentional qualification generation failure/);
  } finally { fs.rmSync(directory, { recursive: true, force: true }); }
});
