#Requires -Version 7.0
param(
    [Parameter(Mandatory)][string]$ReferenceDirectory,
    [string]$ResultsDirectory = "$PSScriptRoot\..\bin\migration-qualification"
)

$ErrorActionPreference = 'Stop'
$root = [IO.Path]::GetFullPath("$PSScriptRoot\..")
$ReferenceDirectory = [IO.Path]::GetFullPath($ReferenceDirectory)
$ResultsDirectory = [IO.Path]::GetFullPath($ResultsDirectory)
$judge = "$root\bin\Release\net10.0\WinmdUtils.dll"
$referenceSha = '86ed37b2846440297afb087c4718acd1a1fb4ad8'
New-Item -ItemType Directory -Force $ResultsDirectory | Out-Null

function Invoke-Native([string]$File, [string[]]$Arguments) {
    & $File @Arguments
    if ($LASTEXITCODE -ne 0) { throw "$File failed (exit $LASTEXITCODE): $Arguments" }
}

function Get-Receipt([string]$Directory, [string]$Name) {
    Push-Location $Directory
    try {
        $sha = (& git rev-parse HEAD).Trim()
        if ($LASTEXITCODE -ne 0) { throw 'Cannot identify source commit.' }
        $files = & git ls-files generation/WinSDK sources/GeneratorSdk/sdk sources/GeneratorSdk/tools/assets
        if ($LASTEXITCODE -ne 0 -or !$files) { throw 'Cannot enumerate legacy inputs.' }
        $entries = @($files | Sort-Object | ForEach-Object {
            "$_ $((Get-FileHash -Algorithm SHA256 -LiteralPath $_).Hash)"
        })
        $assets = Get-Content -Raw "$Directory\obj\BuildTools\project.assets.json" | ConvertFrom-Json -AsHashtable
        if (!$assets.libraries.ContainsKey('Microsoft.Windows.SDK.CPP/10.0.28000.2705') -or
            !$assets.libraries.ContainsKey('Microsoft.Windows.SDK.CPP.x64/10.0.28000.2705')) {
            throw 'BuildTools did not resolve the pinned SDK.'
        }
        # Bind package contents, not just a version label. Hash both restored SDK
        # packages; checked-in recompiled headers and configuration are above.
        foreach ($key in 'Microsoft.Windows.SDK.CPP/10.0.28000.2705', 'Microsoft.Windows.SDK.CPP.x64/10.0.28000.2705') {
            $library = $assets.libraries[$key]
            $package = @($assets.packageFolders.Keys | ForEach-Object {
                Join-Path $_ $library.path
            } | Where-Object { Test-Path $_ -PathType Container })
            if ($package.Count -ne 1) { throw "Missing/ambiguous restored SDK: $key" }
            foreach ($file in $library.files | Sort-Object) {
                $entries += "$key/$file $((Get-FileHash -Algorithm SHA256 -LiteralPath (Join-Path $package[0] $file)).Hash)"
            }
        }
        $clangAssets = Get-Content -Raw "$Directory\obj\Win32MetadataScraper\project.assets.json" | ConvertFrom-Json -AsHashtable
        if (!$clangAssets.libraries.ContainsKey('ClangSharp/21.1.8.4') -or
            !$clangAssets.libraries.ContainsKey('ClangSharp.PInvokeGenerator/21.1.8.4')) {
            throw 'Scraper did not resolve pinned ClangSharp 21.1.8.4.'
        }
        $inputFile = "$ResultsDirectory\$Name-inputs.sha256"
        $entries | Set-Content $inputFile
        $toolchain = @(& dotnet --list-sdks)
        if ($LASTEXITCODE -ne 0) { throw 'Cannot identify dotnet toolchain.' }
        $toolchain += "VSINSTALLDIR=$env:VSINSTALLDIR"
        $toolchain += "VCToolsVersion=$env:VCToolsVersion"
        $toolchain += 'MaxProcessors=2; MSBuild nodes=1; WinmdVersion=1.0.0.0; ScanArch=crossarch'
        $toolchainFile = "$ResultsDirectory\$Name-toolchain.txt"
        $toolchain | Set-Content $toolchainFile
        return @{
            sourceSha = $sha
            inputHash = (Get-FileHash $inputFile -Algorithm SHA256).Hash.ToLowerInvariant()
            toolchainHash = (Get-FileHash $toolchainFile -Algorithm SHA256).Hash.ToLowerInvariant()
        }
    } finally { Pop-Location }
}

function Measure-Generation([string]$Directory, [string]$Name, [int]$Pair) {
    $prefix = "$ResultsDirectory\pair-$Pair-$Name"
    $output = "$Directory\bin\Windows.Win32.winmd"
    # Never certify marker-driven incremental outputs from a previous sample.
    foreach ($path in "$Directory\generation\WinSDK\obj\generated", "$Directory\generation\WinSDK\obj\scratch") {
        if (Test-Path $path) { Remove-Item -LiteralPath $path -Recurse -Force }
    }
    if (Test-Path $output) { Remove-Item -LiteralPath $output }
    $info = [Diagnostics.ProcessStartInfo]::new('dotnet')
    $info.WorkingDirectory = $Directory
    $info.UseShellExecute = $false
    $info.RedirectStandardOutput = $true
    $info.RedirectStandardError = $true
    foreach ($argument in @('build', 'generation\WinSDK', '-c', 'Release', '--no-restore',
        '-t:EmitWinmd', '-p:ScanArch=crossarch', '-p:WinmdVersion=1.0.0.0',
        '-p:MaxProcessors=2', '-m:1', '-nr:false', "-bl:$prefix.binlog")) {
        $info.ArgumentList.Add($argument)
    }
    $process = [Diagnostics.Process]::new()
    $process.StartInfo = $info
    $sample = @{
        startedAt = [DateTime]::UtcNow.ToString('o')
        peakWorkingSetBytes = 0L; memorySamples = 0; exitCode = $null; outputHash = $null
        memoryMetric = '500ms sampled sum of live generation-process-tree working sets'
        phase = 'ScrapeHeaders -> ScrapeConstants -> EmitWinmd (target boundaries in binlog)'
    }
    $watch = [Diagnostics.Stopwatch]::StartNew()
    $memoryLog = [IO.StreamWriter]::new("$prefix-memory.csv")
    $memoryLog.WriteLine('elapsedSeconds,workingSetBytes,processCount')
    $started = $false
    try {
        if (!$process.Start()) { throw 'Generation process did not start.' }
        $started = $true
        $stdout = $process.StandardOutput.ReadToEndAsync()
        $stderr = $process.StandardError.ReadToEndAsync()
        while (!$process.HasExited) {
            if ($watch.Elapsed.TotalSeconds -gt 3600) {
                $process.Kill($true)
                throw 'Generation exceeded the one-hour sample timeout.'
            }
            $processes = @(Get-CimInstance Win32_Process -Property ProcessId, ParentProcessId, WorkingSetSize)
            $ids = [Collections.Generic.HashSet[uint32]]::new()
            [void]$ids.Add([uint32]$process.Id)
            do {
                $added = $false
                foreach ($child in $processes) {
                    if ($ids.Contains([uint32]$child.ParentProcessId) -and $ids.Add([uint32]$child.ProcessId)) {
                        $added = $true
                    }
                }
            } while ($added)
            $live = @($processes | Where-Object { $ids.Contains([uint32]$_.ProcessId) })
            $bytes = [long](($live | Measure-Object -Property WorkingSetSize -Sum).Sum)
            $sample.peakWorkingSetBytes = [Math]::Max($sample.peakWorkingSetBytes, $bytes)
            $sample.memorySamples++
            $memoryLog.WriteLine("$($watch.Elapsed.TotalSeconds.ToString('F3', [Globalization.CultureInfo]::InvariantCulture)),$bytes,$($live.Count)")
            Start-Sleep -Milliseconds 500
        }
        $watch.Stop()
        $sample.exitCode = $process.ExitCode
        if ($sample.exitCode -ne 0) { throw "Generation failed: $Name pair $Pair (exit $($sample.exitCode))." }
        if (!(Test-Path $output -PathType Leaf) -or (Get-Item $output).Length -eq 0) {
            throw "Generation did not create a winmd: $Name pair $Pair."
        }
        $sample.outputHash = (Get-FileHash $output -Algorithm SHA256).Hash.ToLowerInvariant()
        Copy-Item $output "$prefix.winmd"
    } finally {
        $watch.Stop()
        $sample.seconds = $watch.Elapsed.TotalSeconds
        $sample.endedAt = [DateTime]::UtcNow.ToString('o')
        $memoryLog.Dispose()
        if ($started) {
            if (!$process.HasExited) { $process.Kill($true) }
            $process.WaitForExit()
            $stdout.GetAwaiter().GetResult() | Set-Content "$prefix.log"
            $stderr.GetAwaiter().GetResult() | Set-Content "$prefix.stderr.log"
        }
        $process.Dispose()
        foreach ($entry in @{ logHash = '.log'; memoryLogHash = '-memory.csv'; binlogHash = '.binlog' }.GetEnumerator()) {
            if (Test-Path "$prefix$($entry.Value)" -PathType Leaf) {
                $sample[$entry.Key] = (Get-FileHash "$prefix$($entry.Value)" -Algorithm SHA256).Hash.ToLowerInvariant()
            }
        }
        $sample | ConvertTo-Json -Depth 10 | Set-Content "$prefix-sample.json"
    }
    return $sample
}

$receipt = @{
    referenceSha = $referenceSha; candidateSha = $null
    sdkVersion = '10.0.28000.2705'; clangSharpVersion = '21.1.8.4'
    pairs = @(); passed = $false
    pilot = 'Not implemented: full paired generation/emission is the initial blocking measurement.'
}
try {
    if ((& git -C $ReferenceDirectory rev-parse HEAD).Trim() -ne $referenceSha -or $LASTEXITCODE -ne 0) {
        throw 'Reference checkout does not match the pinned snapshot.'
    }
    if ($ReferenceDirectory -eq $root) { throw 'Reference must be a separate checkout.' }
    # Preparation and restores are deliberately outside the measured region.
    foreach ($directory in $ReferenceDirectory, $root) {
        Push-Location $directory
        try {
            . "$directory\scripts\CommonUtils.ps1"
            Install-BuildTools
            Invoke-Native dotnet @('restore', 'generation\WinSDK', '--configfile', 'nuget.config')
        } finally { Pop-Location }
    }
    $receipt.reference = Get-Receipt $ReferenceDirectory reference
    $receipt.candidate = Get-Receipt $root candidate
    $receipt.candidateSha = $receipt.candidate.sourceSha
    if ($receipt.reference.inputHash -ne $receipt.candidate.inputHash -or
        $receipt.reference.toolchainHash -ne $receipt.candidate.toolchainHash) {
        throw 'Reference/candidate inputs or toolchains do not match.'
    }
    for ($pair = 1; $pair -le 3; $pair++) {
        # Alternate order on confirmation to avoid always charging cache warmth
        # to the reference. No two builds run concurrently in either checkout.
        $samples = @{}
        $order = if ($pair % 2) { @('reference', 'candidate') } else { @('candidate', 'reference') }
        foreach ($side in $order) {
            $directory = if ($side -eq 'reference') { $ReferenceDirectory } else { $root }
            $samples[$side] = Measure-Generation $directory $side $pair
        }
        $driftFile = "$ResultsDirectory\pair-$pair-drift.json"
        $receipt.pairs += $samples
        Invoke-Native dotnet @($judge, 'compareBinary', '--first', "$ResultsDirectory\pair-$pair-reference.winmd",
            '--second', "$ResultsDirectory\pair-$pair-candidate.winmd", '--output', $driftFile)
        $samples.drift = Get-Content -Raw $driftFile | ConvertFrom-Json -AsHashtable
        $receipt | ConvertTo-Json -Depth 20 | Set-Content "$ResultsDirectory\receipt.json"
        & dotnet $judge qualifyMigration --receipt "$ResultsDirectory\receipt.json" --output "$ResultsDirectory\verdict.json"
        if ($LASTEXITCODE -eq 0) { $receipt.passed = $true; break }
        if ($LASTEXITCODE -ne 1) { throw 'Qualification judge failed.' }
    }
    if (!$receipt.passed) { throw 'Full-generation performance budget breached or confirmation remained unresolved.' }
} catch {
    $_ | Out-String | Set-Content "$ResultsDirectory\failure.txt"
    throw
} finally {
    $receipt | ConvertTo-Json -Depth 20 | Set-Content "$ResultsDirectory\receipt.json"
    if ($env:GITHUB_STEP_SUMMARY) {
        @"
## Migration qualification

Passed: **$($receipt.passed)**

Reference: ``$referenceSha``; candidate: ``$($receipt.candidateSha)``

SDK: ``10.0.28000.2705``; ClangSharp: ``21.1.8.4``; fixed winmd version: ``1.0.0.0``.
Full crossarch generation through emission, two scraper workers and one MSBuild node.
Blocking budgets: confirmed wall time >10%, sampled process-tree working set >15%.
No metadata differences are approved. Only the random Module MVID and PE timestamp are normalized.

[Receipts, winmds, raw memory/timing samples, drift verdicts and target-phase binlogs]($env:GITHUB_SERVER_URL/$env:GITHUB_REPOSITORY/actions/runs/$env:GITHUB_RUN_ID#artifacts)

$($receipt.pilot)

| Pair | Reference seconds | Candidate seconds | Reference sampled peak bytes | Candidate sampled peak bytes |
|---|---:|---:|---:|---:|
"@ | Add-Content $env:GITHUB_STEP_SUMMARY
        foreach ($sample in $receipt.pairs) {
            "| $($receipt.pairs.IndexOf($sample) + 1) | $($sample.reference.seconds) | $($sample.candidate.seconds) | $($sample.reference.peakWorkingSetBytes) | $($sample.candidate.peakWorkingSetBytes) |" |
                Add-Content $env:GITHUB_STEP_SUMMARY
        }
    }
}
