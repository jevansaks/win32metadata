[CmdletBinding()]
param(
    [Parameter(Mandatory = $true)]
    [string]$Header,

    [Parameter(Mandatory = $true)]
    [string]$Partition,

    [string]$WindowsRsRoot = $env:WINDOWS_RS_SHIFT_LEFT_ROOT,

    [string]$ReferenceWinmd = $env:WIN32METADATA_REFERENCE_WINMD,

    [ValidateSet("x64", "x86", "arm64")]
    [string]$Architecture = "x64",

    [string]$OutputRoot = "$PSScriptRoot\..\artifacts\shift-left-headers"
)

$ErrorActionPreference = "Stop"
$repoRoot = (Resolve-Path "$PSScriptRoot\..").Path

if (!$WindowsRsRoot -or !(Test-Path (Join-Path $WindowsRsRoot "crates\tools\win32\Cargo.toml"))) {
    throw "Pass -WindowsRsRoot or set WINDOWS_RS_SHIFT_LEFT_ROOT to the windows-rs implementation checkout."
}
if (!$ReferenceWinmd) {
    $ReferenceWinmd = "C:\.tools\.nuget\packages\microsoft.windows.sdk.win32metadata\70.0.11-preview\Windows.Win32.winmd"
}
if (!(Test-Path $ReferenceWinmd)) {
    throw "Reference winmd was not found: $ReferenceWinmd"
}

$stem = [System.IO.Path]::GetFileNameWithoutExtension($Header)
$manifest = Get-Content (Join-Path $repoRoot "generation\WinSDK\patches\header-progress.json") -Raw |
    ConvertFrom-Json
$entry = $manifest.headers |
    Where-Object { $_.header -ieq $Header -and $_.partition -ieq $Partition } |
    Select-Object -First 1
if (!$entry -or !$entry.targetSymbols) {
    throw "The progress manifest must define targetSymbols for $Header / $Partition."
}
$patch = Join-Path $repoRoot "generation\WinSDK\patches\post-midl\$Header.win32metadata.patch"
if (!(Test-Path $patch)) {
    throw "The one-patch-per-header artifact is missing: $patch"
}

$output = Join-Path $OutputRoot "$stem\$Partition\$Architecture"
New-Item -ItemType Directory -Force -Path $output | Out-Null

$winmdUtils = Join-Path $repoRoot "bin\Release\net8.0\WinmdUtils.dll"
if (!(Test-Path $winmdUtils)) {
    & dotnet build (Join-Path $repoRoot "sources\WinmdUtils\WinmdUtils.csproj") -c Release --nologo
    if ($LASTEXITCODE -ne 0) {
        throw "WinmdUtils build failed."
    }
}

$saved = @{
    WIN32METADATA_ROOT = $env:WIN32METADATA_ROOT
    WIN32METADATA_PARTITION_FILTER = $env:WIN32METADATA_PARTITION_FILTER
    WIN32METADATA_HEADER_FILTER = $env:WIN32METADATA_HEADER_FILTER
    WIN32METADATA_PARTITION_OUTPUT = $env:WIN32METADATA_PARTITION_OUTPUT
    WIN32METADATA_PARTITION_ARCHS = $env:WIN32METADATA_PARTITION_ARCHS
    WIN32METADATA_PARTITION_SMOKE = $env:WIN32METADATA_PARTITION_SMOKE
    WIN32METADATA_SEQUENTIAL = $env:WIN32METADATA_SEQUENTIAL
    WIN32METADATA_REFERENCE_WINMD = $env:WIN32METADATA_REFERENCE_WINMD
    WIN32METADATA_DEPENDENCY_WINMD = $env:WIN32METADATA_DEPENDENCY_WINMD
    WIN32METADATA_SYMBOL_FILTER = $env:WIN32METADATA_SYMBOL_FILTER
}

try {
    $env:WIN32METADATA_ROOT = $repoRoot
    $env:WIN32METADATA_PARTITION_FILTER = $Partition
    $env:WIN32METADATA_HEADER_FILTER = $Header
    $env:WIN32METADATA_PARTITION_OUTPUT = $output
    $env:WIN32METADATA_PARTITION_ARCHS = $Architecture
    $env:WIN32METADATA_PARTITION_SMOKE = "1"
    $env:WIN32METADATA_SEQUENTIAL = "1"
    $env:WIN32METADATA_REFERENCE_WINMD = $ReferenceWinmd
    $env:WIN32METADATA_DEPENDENCY_WINMD = Join-Path $WindowsRsRoot "crates\libs\default\Windows.Win32.winmd"
    $env:WIN32METADATA_SYMBOL_FILTER = $entry.targetSymbols -join ","

    Push-Location $WindowsRsRoot
    try {
        & cargo run -p tool_win32
        if ($LASTEXITCODE -ne 0) {
            throw "windows-rs header generation failed."
        }
    }
    finally {
        Pop-Location
    }
}
finally {
    foreach ($name in $saved.Keys) {
        if ($null -eq $saved[$name]) {
            Remove-Item -Path "Env:$name" -ErrorAction SilentlyContinue
        }
        else {
            Set-Item -Path "Env:$name" -Value $saved[$name]
        }
    }
}

$generatedWinmd = Join-Path $output "Windows.Win32.winmd"
$generatedRdl = Join-Path $output "rdl\$stem.rdl"
& "$PSScriptRoot\Compare-ShiftLeftHeader.ps1" `
    -Header $Header `
    -Partition $Partition `
    -ReferenceWinmd $ReferenceWinmd `
    -GeneratedWinmd $generatedWinmd `
    -GeneratedRdl $generatedRdl `
    -OutputDirectory (Join-Path $output "comparison") `
    -WinmdUtils $winmdUtils
exit $LASTEXITCODE
