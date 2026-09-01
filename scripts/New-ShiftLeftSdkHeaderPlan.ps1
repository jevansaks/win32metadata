[CmdletBinding()]
param(
    [string]$OutputDirectory = "$PSScriptRoot\..\generation\WinSDK\patches\header-plan",

    [string]$ProgressManifest = "$PSScriptRoot\..\generation\WinSDK\patches\header-progress.json"
)

$ErrorActionPreference = "Stop"
$repoRoot = (Resolve-Path "$PSScriptRoot\..").Path
$headersRoot = Join-Path $repoRoot "generation\WinSDK\RecompiledIdlHeaders"
$partitionsRoot = Join-Path $repoRoot "generation\WinSDK\Partitions"
$OutputDirectory = [System.IO.Path]::GetFullPath($OutputDirectory)

New-Item -ItemType Directory -Force -Path $OutputDirectory | Out-Null

$progress = if (Test-Path $ProgressManifest) {
    Get-Content $ProgressManifest -Raw | ConvertFrom-Json
}
else {
    $null
}
$statusByHeaderPartition = @{}
if ($progress) {
    foreach ($entry in $progress.headers) {
        $statusByHeaderPartition["$($entry.header.ToLowerInvariant())|$($entry.partition.ToLowerInvariant())"] = $entry.status
    }
}

$allHeaders = Get-ChildItem $headersRoot -Recurse -Filter *.h |
    Sort-Object FullName |
    ForEach-Object {
        $relative = [System.IO.Path]::GetRelativePath($headersRoot, $_.FullName)
        $segments = $relative -split '[\\/]'
        [pscustomobject]@{
            area = $segments[0]
            header = $_.Name
            sdkPath = "/" + (($segments -join "/"))
            localPath = [System.IO.Path]::GetRelativePath($repoRoot, $_.FullName)
        }
    }

$partitionRows = foreach ($settings in Get-ChildItem $partitionsRoot -Recurse -Filter settings.rsp) {
    $partition = [System.IO.Path]::GetRelativePath($partitionsRoot, $settings.Directory.FullName) -replace "\\", "."
    $lines = [System.IO.File]::ReadAllLines($settings.FullName)
    $namespace = $null
    for ($i = 0; $i -lt $lines.Count; $i++) {
        if ($lines[$i].Trim() -eq "--namespace" -and $i + 1 -lt $lines.Count) {
            $namespace = $lines[$i + 1].Trim()
        }
    }
    foreach ($line in $lines) {
        if ($line -match '<IncludeRoot>\s*(/[^<>\s]+\.h)') {
            $sdkPath = $Matches[1]
            $header = [System.IO.Path]::GetFileName($sdkPath)
            $key = "$($header.ToLowerInvariant())|$($partition.ToLowerInvariant())"
            [pscustomobject]@{
                order = 0
                header = $header
                sdkPath = $sdkPath
                partition = $partition
                namespace = $namespace
                status = if ($statusByHeaderPartition.ContainsKey($key)) { $statusByHeaderPartition[$key] } else { "queued" }
            }
        }
    }
}

$partitionRows = @($partitionRows |
    Sort-Object partition, sdkPath |
    ForEach-Object -Begin { $i = 0 } -Process {
        $i++
        $_.order = $i
        $_
    })

$allHeaderOutput = Join-Path $OutputDirectory "sdk-headers.csv"
$partitionOutput = Join-Path $OutputDirectory "partition-header-queue.csv"
$allHeaders | Export-Csv $allHeaderOutput -NoTypeInformation
$partitionRows | Export-Csv $partitionOutput -NoTypeInformation

$byAreaRows = $allHeaders |
    Group-Object area |
    Sort-Object Name |
    ForEach-Object { "| $($_.Name) | $($_.Count) |" }
$byStatusRows = $partitionRows |
    Group-Object status |
    Sort-Object Name |
    ForEach-Object { "| $($_.Name) | $($_.Count) |" }
$byPartitionRows = $partitionRows |
    Group-Object partition |
    Sort-Object Name |
    ForEach-Object { "| $($_.Name) | $($_.Count) |" }

$unrootedCount = @($allHeaders | Where-Object {
    $header = $_.header
    -not ($partitionRows | Where-Object { $_.header -ieq $header })
}).Count

$summary = @"
# All-SDK shift-left header plan

Generated from `generation/WinSDK/RecompiledIdlHeaders` and partition
`<IncludeRoot>` entries.

| Inventory | Count |
| --- | ---: |
| SDK headers | $($allHeaders.Count) |
| Partition/header roots | $($partitionRows.Count) |
| SDK headers without a direct partition root | $unrootedCount |

## Partition/header status

| Status | Count |
| --- | ---: |
$($byStatusRows -join "`r`n")

## SDK headers by area

| Area | Headers |
| --- | ---: |
$($byAreaRows -join "`r`n")

## Partition roots

| Partition | Header roots |
| --- | ---: |
$($byPartitionRows -join "`r`n")

## Files

- `sdk-headers.csv` lists every recompiled SDK header.
- `partition-header-queue.csv` lists every partition/header root that can be generated and compared directly.
"@

$summaryPath = Join-Path $OutputDirectory "README.md"
[System.IO.File]::WriteAllText($summaryPath, $summary, [System.Text.UTF8Encoding]::new($false))
Write-Host "Wrote $($allHeaders.Count) SDK headers and $($partitionRows.Count) partition/header roots to $OutputDirectory"
