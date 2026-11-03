#Requires -Version 7.0
param(
    [Parameter(Mandatory)][string]$ReferenceDirectory,
    [Parameter(Mandatory)][string]$ControlDirectory,
    [string]$CurrentHeaderRoot = "$PSScriptRoot\..\generation\WinSDK\RecompiledIdlHeaders",
    [string]$CurrentIncludesRoot = "$PSScriptRoot\..\generation\WinSDK\inc",
    [string]$ResultsDirectory = "$PSScriptRoot\..\bin\migration-qualification"
)

$ErrorActionPreference = 'Stop'
$root = [IO.Path]::GetFullPath("$PSScriptRoot\..")
$ReferenceDirectory = [IO.Path]::GetFullPath($ReferenceDirectory)
$ControlDirectory = [IO.Path]::GetFullPath($ControlDirectory)
$CurrentHeaderRoot = [IO.Path]::GetFullPath($CurrentHeaderRoot)
$CurrentIncludesRoot = [IO.Path]::GetFullPath($CurrentIncludesRoot)
$ResultsDirectory = [IO.Path]::GetFullPath($ResultsDirectory)
$judge = "$root\bin\Release\net10.0\WinmdUtils.dll"
$referenceSha = '86ed37b2846440297afb087c4718acd1a1fb4ad8'
New-Item -ItemType Directory -Force $ResultsDirectory | Out-Null

function Invoke-Native([string]$File, [string[]]$Arguments) {
    & $File @Arguments
    if ($LASTEXITCODE -ne 0) { throw "$File failed (exit $LASTEXITCODE): $Arguments" }
}

function Get-GenerationArguments([hashtable]$Build, [string]$Obj, [string]$Output) {
    return @($Build.Project, '-p:Configuration=Release', '-p:ScanArch=crossarch',
        '-p:WinmdVersion=1.0.0.0', '-p:MaxProcessors=2', '-m:1', '-nr:false',
        "-p:TaskBinDir=$($Build.Tools)", "-p:ToolsBinDir=$($Build.Tools)",
        "-p:ScriptsDir=$($Build.Directory)\scripts", "-p:LibToolsBinDir=$($Build.Directory)\tools",
        "-p:Win32MetadataAssetsDir=$($Build.Directory)\sources\GeneratorSdk\tools\assets",
        "-p:TargetPlatformSdkRootOverride=$($Build.SdkRoot)",
        "-p:SdkIncRoot=$($Build.Headers)", "-p:AdditionalIncludes=$($Build.Includes)",
        "-p:ObjDir=$Obj", "-p:OutputWinmd=$Output")
}

function Get-GenerationConfiguration([hashtable]$Build, [string]$Name) {
    $obj = "$ResultsDirectory\configuration-$Name\obj"
    $output = "$ResultsDirectory\configuration-$Name\Windows.Win32.winmd"
    $arguments = Get-GenerationArguments $Build $obj $output
    $properties = 'TaskBinDir,ToolsBinDir,ScriptsDir,LibToolsBinDir,Win32MetadataAssetsDir,SdkIncRoot,AdditionalIncludes,ObjDir,GeneratedDir,ScratchOutputDir,OutputWinmd,ScanArch,WinmdVersion,MaxProcessors,UseWinSDKAssets,SkipScraping,PartitionFilter,TargetPlatformSdkRootOverride,EnumsJson,ScraperHeader,ConstantsScraperHeader'
    $json = & dotnet msbuild @arguments "-getProperty:$properties" '-getItem:ScraperRsp,ConstantsScraperRsp,EmitterRsp,ResponseFile,AutoType,FunctionPointerFixup,ManualCs,PartitionFiles'
    if ($LASTEXITCODE -ne 0) { throw "Cannot evaluate generation configuration: $Name" }
    $configuration = ($json -join "`n") | ConvertFrom-Json -AsHashtable
    $expected = @{
        TaskBinDir = $Build.Tools; ToolsBinDir = $Build.Tools
        ScriptsDir = "$($Build.Directory)\scripts"; LibToolsBinDir = "$($Build.Directory)\tools"
        Win32MetadataAssetsDir = "$($Build.Directory)\sources\GeneratorSdk\tools\assets"
        SdkIncRoot = $Build.Headers; AdditionalIncludes = $Build.Includes
        ObjDir = $obj; GeneratedDir = "$obj\generated"; ScratchOutputDir = "$obj\scratch"; OutputWinmd = $output
        ScanArch = 'crossarch'; WinmdVersion = '1.0.0.0'; MaxProcessors = '2'; UseWinSDKAssets = 'false'
        SkipScraping = ''; PartitionFilter = ''
        TargetPlatformSdkRootOverride = $Build.SdkRoot
    }
    foreach ($entry in $expected.GetEnumerator()) {
        if ($configuration.Properties[$entry.Key].TrimEnd('\') -ne $entry.Value.TrimEnd('\')) {
            throw "Generation override not effective: $Name $($entry.Key)"
        }
    }
    $configuration | ConvertTo-Json -Depth 20 | Set-Content "$ResultsDirectory\$Name-configuration.json"
    return $configuration
}

function Get-ActiveInputEntries([hashtable]$Build, [hashtable]$Configuration) {
    # Reviewed input surface of Windows.Win32.proj/sdk.targets. Do not hash
    # dormant patch assets, AdditionalHeaders, schemas, or test fixtures.
    $files = [Collections.Generic.Dictionary[string,string]]::new([StringComparer]::Ordinal)
    foreach ($itemGroup in $Configuration.Items.GetEnumerator()) {
        foreach ($item in $itemGroup.Value) {
            $path = $item.FullPath
            if ($path.StartsWith($Configuration.Properties.ObjDir + '\', [StringComparison]::OrdinalIgnoreCase)) { continue }
            $label = [IO.Path]::GetRelativePath($Build.Directory, $path).Replace('\', '/')
            $files[$label] = $path
        }
    }
    foreach ($relative in 'generation\WinSDK\Windows.Win32.proj', 'sources\GeneratorSdk\sdk\sdk.props',
        'sources\GeneratorSdk\sdk\sdk.targets', 'Directory.Build.props', 'Directory.Build.targets') {
        $files[$relative.Replace('\', '/')] = Join-Path $Build.Directory $relative
    }
    foreach ($name in 'EnumsJson', 'ScraperHeader', 'ConstantsScraperHeader') {
        $path = $Configuration.Properties[$name]
        $files[[IO.Path]::GetRelativePath($Build.Directory, $path).Replace('\', '/')] = $path
    }
    $trees = @{
        'active-headers' = $Build.Headers
        'active-includes' = $Build.Includes
        'generation/WinSDK/Partitions' = "$($Build.Directory)\generation\WinSDK\Partitions"
        'scraper-assets' = "$($Build.Directory)\sources\GeneratorSdk\tools\assets\scraper"
    }
    foreach ($tree in $trees.GetEnumerator()) {
        if (!(Test-Path -LiteralPath $tree.Value -PathType Container)) { throw "Missing active input tree: $($tree.Value)" }
        foreach ($path in [IO.Directory]::EnumerateFiles($tree.Value, '*', [IO.SearchOption]::AllDirectories)) {
            $files["$($tree.Key)/$([IO.Path]::GetRelativePath($tree.Value, $path).Replace('\', '/'))"] = $path
        }
    }
    foreach ($label in $files.Keys | Sort-Object -CaseSensitive) {
        "$label $((Get-FileHash -Algorithm SHA256 -LiteralPath $files[$label]).Hash)"
    }
}

function Get-Receipt([hashtable]$Build, [string]$Name) {
    $Directory = $Build.Directory
    Push-Location $Directory
    try {
        $sha = (& git rev-parse HEAD).Trim()
        if ($LASTEXITCODE -ne 0) { throw 'Cannot identify source commit.' }
        Invoke-Native git @('diff', '--quiet', 'HEAD')
        if ($sha -eq $referenceSha) {
            $untracked = & git ls-files --others --exclude-standard -- generation/WinSDK/RecompiledIdlHeaders generation/WinSDK/inc generation/WinSDK/manual generation/WinSDK/Partitions sources/GeneratorSdk/tools/assets/scraper
            if ($LASTEXITCODE -ne 0 -or $untracked) { throw 'Frozen active input trees contain uncommitted files.' }
        }
        $assets = Get-Content -Raw "$Directory\obj\BuildTools\project.assets.json" | ConvertFrom-Json -AsHashtable
        if (!$assets.libraries.ContainsKey('Microsoft.Windows.SDK.CPP/10.0.28000.2705') -or
            !$assets.libraries.ContainsKey('Microsoft.Windows.SDK.CPP.x64/10.0.28000.2705')) {
            throw 'BuildTools did not resolve the pinned SDK.'
        }
        $sdkPackage = @($assets.packageFolders.Keys | ForEach-Object {
            Join-Path $_ $assets.libraries['Microsoft.Windows.SDK.CPP/10.0.28000.2705'].path
        } | Where-Object { Test-Path $_ -PathType Container })
        if ($sdkPackage.Count -ne 1) { throw 'Missing/ambiguous pinned SDK origin.' }
        $Build.SdkRoot = "$($sdkPackage[0])\c"
        $configuration = Get-GenerationConfiguration $Build $Name
        $entries = @(Get-ActiveInputEntries $Build $configuration)
        $sdkEntries = @()
        # Bind package contents, not just a version label. Hash both restored SDK
        # packages; checked-in recompiled headers and configuration are above.
        foreach ($key in 'Microsoft.Windows.SDK.CPP/10.0.28000.2705', 'Microsoft.Windows.SDK.CPP.x64/10.0.28000.2705') {
            $library = $assets.libraries[$key]
            $package = @($assets.packageFolders.Keys | ForEach-Object {
                Join-Path $_ $library.path
            } | Where-Object { Test-Path $_ -PathType Container })
            if ($package.Count -ne 1) { throw "Missing/ambiguous restored SDK: $key" }
            foreach ($file in $library.files | Sort-Object) {
                $sdkEntries += "$key/$file $((Get-FileHash -Algorithm SHA256 -LiteralPath (Join-Path $package[0] $file)).Hash)"
            }
        }
        $clangAssets = Get-Content -Raw "$($Build.ToolDirectory)\obj\Win32MetadataScraper\project.assets.json" | ConvertFrom-Json -AsHashtable
        if (!$clangAssets.libraries.ContainsKey('ClangSharp/21.1.8.4') -or
            !$clangAssets.libraries.ContainsKey('ClangSharp.PInvokeGenerator/21.1.8.4') -or
            !$clangAssets.libraries.ContainsKey('libclang/21.1.8') -or
            !$clangAssets.libraries.ContainsKey('libClangSharp/21.1.8.2')) {
            throw 'Scraper did not resolve pinned ClangSharp 21.1.8.4.'
        }
        $inputFile = "$ResultsDirectory\$Name-inputs.sha256"
        $entries | Set-Content $inputFile
        $sdkFile = "$ResultsDirectory\$Name-sdk.sha256"
        $sdkEntries | Set-Content $sdkFile
        # Bind the complete runnable tool directory, including deps/runtimeconfig,
        # RID scraper builds, and native clang libraries, rather than version labels.
        $toolsFile = "$ResultsDirectory\$Name-tools.sha256"
        [IO.Directory]::EnumerateFiles($Build.Tools, '*', [IO.SearchOption]::AllDirectories) |
            Sort-Object -CaseSensitive | ForEach-Object {
                "$([IO.Path]::GetRelativePath($Build.Tools, $_).Replace('\', '/')) $((Get-FileHash -Algorithm SHA256 -LiteralPath $_).Hash)"
            } | Set-Content $toolsFile
        $toolchain = @(& dotnet --list-sdks)
        if ($LASTEXITCODE -ne 0) { throw 'Cannot identify dotnet toolchain.' }
        $toolchain += "VSINSTALLDIR=$env:VSINSTALLDIR"
        $toolchain += "VCToolsVersion=$env:VCToolsVersion"
        $toolchain += 'MaxProcessors=2; MSBuild nodes=1; WinmdVersion=1.0.0.0; ScanArch=crossarch'
        $toolchainFile = "$ResultsDirectory\$Name-toolchain.txt"
        $toolchain | Set-Content $toolchainFile
        return @{
            sourceSha = $sha
            toolSourceSha = (& git -C $Build.ToolDirectory rev-parse HEAD).Trim()
            toolHash = (Get-FileHash $toolsFile -Algorithm SHA256).Hash.ToLowerInvariant()
            sdkHash = (Get-FileHash $sdkFile -Algorithm SHA256).Hash.ToLowerInvariant()
            inputHash = (Get-FileHash $inputFile -Algorithm SHA256).Hash.ToLowerInvariant()
            toolchainHash = (Get-FileHash $toolchainFile -Algorithm SHA256).Hash.ToLowerInvariant()
        }
    } finally { Pop-Location }
}

function Measure-Generation([hashtable]$Build, [string]$Name, [int]$Pair) {
    $Directory = $Build.Directory
    $prefix = "$ResultsDirectory\pair-$Pair-$Name"
    $buildDirectory = "$ResultsDirectory\build-$Pair-$Name"
    $obj = "$buildDirectory\obj"
    $output = "$buildDirectory\Windows.Win32.winmd"
    # Never certify marker-driven incremental outputs from a previous sample.
    if (Test-Path $obj) { Remove-Item -LiteralPath $obj -Recurse -Force }
    if (Test-Path $output) { Remove-Item -LiteralPath $output }
    New-Item -ItemType Directory -Force $buildDirectory | Out-Null
    $info = [Diagnostics.ProcessStartInfo]::new('dotnet')
    $info.WorkingDirectory = $Directory
    $info.UseShellExecute = $false
    $info.RedirectStandardOutput = $true
    $info.RedirectStandardError = $true
    foreach ($argument in @('msbuild') + (Get-GenerationArguments $Build $obj $output) +
        @('-t:EmitWinmd', "-bl:$prefix.binlog")) {
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
        $log = $stdout.GetAwaiter().GetResult()
        foreach ($tool in 'ConstantsScraper.dll', 'ClangSharpSourceToWinmd.dll') {
            if (!$log.Contains("$($Build.Tools)\$tool", [StringComparison]::OrdinalIgnoreCase)) {
                throw "Generation did not invoke the selected tool: $Name $tool"
            }
        }
        $scraper = "$($Build.Tools)\$([Runtime.InteropServices.RuntimeInformation]::RuntimeIdentifier)\Win32MetadataScraper.dll"
        if (!(Test-Path $scraper -PathType Leaf)) { $scraper = "$($Build.Tools)\Win32MetadataScraper.dll" }
        if (!$log.Contains("Using Win32MetadataScraper from $scraper", [StringComparison]::OrdinalIgnoreCase)) {
            throw "Generation did not invoke the selected scraper: $Name"
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
    if ((& git -C $ControlDirectory rev-parse HEAD).Trim() -ne $referenceSha -or $LASTEXITCODE -ne 0) {
        throw 'Control checkout does not match the pinned snapshot.'
    }
    if (@($ReferenceDirectory, $ControlDirectory, $root | Select-Object -Unique).Count -ne 3) {
        throw 'A/B/C require three separate writable checkouts.'
    }
    $processors = @(Get-CimInstance Win32_Processor -Property Name, NumberOfLogicalProcessors)
    $memory = Get-CimInstance Win32_ComputerSystem -Property TotalPhysicalMemory
    $hardware = @(
        $processors | ForEach-Object { "CPU: $($_.Name); logical cores: $($_.NumberOfLogicalProcessors)" }
        "Total RAM bytes: $($memory.TotalPhysicalMemory)"
        "Runner image OS: $env:ImageOS; image version: $env:ImageVersion"
        'Effective qualification scraper workers: 2; MSBuild nodes: 1; ScanArch: crossarch'
    )
    $hardware | Set-Content "$ResultsDirectory\runner-hardware.txt"
    $hardware | ForEach-Object { Write-Host $_ }
    # Preparation and restores are deliberately outside the measured region.
    foreach ($directory in $ReferenceDirectory, $root) {
        Push-Location $directory
        try {
            . "$directory\scripts\CommonUtils.ps1"
            Install-BuildTools
            Invoke-Native dotnet @('restore', 'generation\WinSDK', '--configfile', 'nuget.config')
        } finally { Pop-Location }
    }
    Invoke-Native dotnet @('restore', "$ControlDirectory\generation\WinSDK\Windows.Win32.proj",
        '--configfile', "$ControlDirectory\nuget.config")
    Invoke-Native dotnet @('restore', "$ControlDirectory\BuildTools", '--configfile', "$ControlDirectory\nuget.config")
    $builds = @{}
    foreach ($side in 'reference', 'control', 'candidate') {
        $directory = switch ($side) { reference { $ReferenceDirectory } control { $ControlDirectory } candidate { $root } }
        $toolDirectory = if ($side -eq 'reference') { $ReferenceDirectory } else { $root }
        $builds[$side] = @{
            Directory = $directory; ToolDirectory = $toolDirectory
            Project = "$directory\generation\WinSDK\Windows.Win32.proj"
            Tools = "$toolDirectory\bin\Release\net10.0"
            Headers = if ($side -eq 'candidate') { $CurrentHeaderRoot } else { "$directory\generation\WinSDK\RecompiledIdlHeaders" }
            Includes = if ($side -eq 'candidate') { $CurrentIncludesRoot } else { "$directory\generation\WinSDK\inc" }
        }
        $receipt[$side] = Get-Receipt $builds[$side] $side
    }
    $receipt.candidateSha = $receipt.candidate.sourceSha
    if ($receipt.reference.inputHash -ne $receipt.control.inputHash -or
        $receipt.control.toolHash -ne $receipt.candidate.toolHash -or
        $receipt.reference.sdkHash -ne $receipt.control.sdkHash -or $receipt.reference.sdkHash -ne $receipt.candidate.sdkHash -or
        $receipt.reference.toolchainHash -ne $receipt.control.toolchainHash -or
        $receipt.reference.toolchainHash -ne $receipt.candidate.toolchainHash) {
        throw 'Frozen inputs, candidate tools, SDK origin or toolchains do not match A/B/C controls.'
    }
    for ($pair = 1; $pair -le 3; $pair++) {
        # Alternate order on confirmation to avoid always charging cache warmth
        # to the reference. No two builds run concurrently in either checkout.
        $samples = @{}
        $order = switch ($pair) {
            1 { @('reference', 'control', 'candidate') }
            2 { @('candidate', 'reference', 'control') }
            3 { @('control', 'candidate', 'reference') }
        }
        foreach ($side in $order) {
            $samples[$side] = Measure-Generation $builds[$side] $side $pair
        }
        $driftFile = "$ResultsDirectory\pair-$pair-drift.json"
        $receipt.pairs += $samples
        Invoke-Native dotnet @($judge, 'compareBinary', '--first', "$ResultsDirectory\pair-$pair-reference.winmd",
            '--second', "$ResultsDirectory\pair-$pair-control.winmd", '--output', $driftFile)
        $samples.drift = Get-Content -Raw $driftFile | ConvertFrom-Json -AsHashtable
        $migrationDriftFile = "$ResultsDirectory\pair-$pair-migration-drift.json"
        Invoke-Native dotnet @($judge, 'compareBinary', '--first', "$ResultsDirectory\pair-$pair-control.winmd",
            '--second', "$ResultsDirectory\pair-$pair-candidate.winmd", '--output', $migrationDriftFile)
        $samples.migrationDrift = Get-Content -Raw $migrationDriftFile | ConvertFrom-Json -AsHashtable
        $receipt | ConvertTo-Json -Depth 20 | Set-Content "$ResultsDirectory\receipt.json"
        & dotnet $judge qualifyMigration --receipt "$ResultsDirectory\receipt.json" --output "$ResultsDirectory\verdict.json"
        if ($LASTEXITCODE -eq 0) {
            foreach ($side in 'reference', 'control', 'candidate') {
                $after = Get-Receipt $builds[$side] "$side-after"
                foreach ($field in 'sourceSha', 'toolSourceSha', 'toolHash', 'inputHash', 'sdkHash', 'toolchainHash') {
                    if ($receipt[$side][$field] -ne $after[$field]) { throw "Input/tool changed during qualification: $side $field" }
                }
            }
            $receipt.passed = $true
            break
        }
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
Full A/B/C crossarch generation through emission, two scraper workers and one MSBuild node.
A: frozen tools/legacy inputs. B: candidate tools/frozen legacy inputs. C: same candidate tools/current active inputs.
Frozen A/B inputs must match; B/C input relocations are permitted only with identical final metadata.
SDK origin/content is separate from the active header/config hash.
Blocking budgets: confirmed wall time >10%, sampled process-tree working set >15%.
Budgets apply to A/B, B/C, and cumulative A/C independently.
No metadata differences are approved. Only the random Module MVID and PE timestamp are normalized.

[Receipts, winmds, raw memory/timing samples, drift verdicts and target-phase binlogs]($env:GITHUB_SERVER_URL/$env:GITHUB_REPOSITORY/actions/runs/$env:GITHUB_RUN_ID#artifacts)

$($receipt.pilot)

| Pair | A seconds | B seconds | C seconds | A peak bytes | B peak bytes | C peak bytes |
|---|---:|---:|---:|---:|---:|---:|
"@ | Add-Content $env:GITHUB_STEP_SUMMARY
        foreach ($sample in $receipt.pairs) {
            "| $($receipt.pairs.IndexOf($sample) + 1) | $($sample.reference.seconds) | $($sample.control.seconds) | $($sample.candidate.seconds) | $($sample.reference.peakWorkingSetBytes) | $($sample.control.peakWorkingSetBytes) | $($sample.candidate.peakWorkingSetBytes) |" |
                Add-Content $env:GITHUB_STEP_SUMMARY
        }
    }
}
