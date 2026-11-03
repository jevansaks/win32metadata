#Requires -Version 7.0
param(
    [Parameter(Mandatory)][string]$HeaderRoot,
    [switch]$IncludeUnqueued
)

$ErrorActionPreference = 'Stop'
$HeaderRoot = (Resolve-Path -LiteralPath $HeaderRoot).Path
if ((Get-Item -LiteralPath $HeaderRoot -Force).Attributes -band [IO.FileAttributes]::ReparsePoint) {
    throw "MIDL inputs cannot follow reparse points: $HeaderRoot"
}
$excluded = "cellularapi_oem", "certbcli", "dxgicommon", "dxgitype", "microsoft.diagnostics.appanalysis", "PortableDeviceConnectImports", "wincrypt" |
    ForEach-Object { "$_.idl" }
foreach ($directory in 'um', 'shared') {
    $folder = Join-Path $HeaderRoot $directory
    if (!(Test-Path -LiteralPath $folder -PathType Container)) { continue }
    if ((Get-Item -LiteralPath $folder -Force).Attributes -band [IO.FileAttributes]::ReparsePoint) {
        throw "MIDL inputs cannot follow reparse points: $folder"
    }
    Get-ChildItem -LiteralPath $folder -File | Where-Object { $_.Extension -ieq '.idl' -and $_.Name -notin $excluded } | ForEach-Object {
        $header = [IO.Path]::ChangeExtension($_.FullName, 'h')
        foreach ($file in @($_.FullName, $header)) {
            if ((Test-Path -LiteralPath $file) -and
                ((Get-Item -LiteralPath $file -Force).Attributes -band [IO.FileAttributes]::ReparsePoint)) {
                throw "MIDL inputs cannot follow reparse points: $file"
            }
        }
        $queued = (Test-Path -LiteralPath $header -PathType Leaf) -and
            (Select-String -LiteralPath $header -Pattern 'File created by MIDL compiler' -Quiet -SimpleMatch)
        if ($queued -or $IncludeUnqueued) {
            [pscustomobject]@{
                Idl = "$directory/$($_.Name)"
                Header = if ($queued) { "$directory/$([IO.Path]::GetFileName($header))" } else { $null }
            }
        }
    }
}
