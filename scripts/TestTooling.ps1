#Requires -Version 7.0
param(
    [string]$ResultsDirectory = "$PSScriptRoot\..\bin\tooling-tests"
)

. "$PSScriptRoot\CommonUtils.ps1"
$ResultsDirectory = [IO.Path]::GetFullPath($ResultsDirectory)
New-Item -ItemType Directory -Force $ResultsDirectory | Out-Null

Start-Transcript -Path "$ResultsDirectory\tooling.log" -Force | Out-Null
try {
    foreach ($suite in 'MetadataUtils.Tests', 'Win32MetadataScraperTests', 'ClangSharpSourceToWinmdTests') {
        $trx = Join-Path $ResultsDirectory "$suite.trx"
        if (Test-Path $trx) { Remove-Item $trx }
        & dotnet test "$PSScriptRoot\..\tests\$suite" -c Release --logger "trx;LogFileName=$suite.trx" --results-directory $ResultsDirectory
        if ($LASTEXITCODE -ne 0) { throw "$suite failed (exit $LASTEXITCODE)." }
        Assert-TestResults $trx
    }
} finally { Stop-Transcript | Out-Null }
