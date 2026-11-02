param
(
    [switch]
    $SkipInstallTools
)

. "$PSScriptRoot\CommonUtils.ps1"

if (!$SkipInstallTools.IsPresent)
{
    Install-BuildTools
}

Write-Host "*** Running tests on .winmd" -ForegroundColor Blue

$windowsWin32TestsDir = "$rootDir\tests\Windows.Win32.Tests"
$resultsDirectory = "$binDir\winmd-tests"
New-Item -ItemType Directory -Force $resultsDirectory | Out-Null
$trx = "$resultsDirectory\Windows.Win32.Tests.trx"
if (Test-Path $trx) { Remove-Item $trx }
dotnet test $windowsWin32TestsDir -c:Release --logger 'trx;LogFileName=Windows.Win32.Tests.trx' --results-directory $resultsDirectory
if ($LASTEXITCODE -ne 0) { throw "Winmd tests failed (exit $LASTEXITCODE)." }
Assert-TestResults $trx

Write-Host "`n`e[32mTesting .winmd succeeded`e[0m"

Write-Host "*** Comparing .winmd to last release (informational)" -ForegroundColor Blue

# Run the comparison for informational purposes — differences are expected
# and will be reviewed via the PR diff comment posted by CI.
& "$PSScriptRoot\CompareBinToLastRelease.ps1" -SkipInstallTools

if ($LastExitCode -lt 0)
{
    Write-Host "`e[33mAPI differences detected (see above). These will be posted as a PR comment for review.`e[0m"
}
else
{
    Write-Host "`n`e[32mNo API differences from last release.`e[0m"
}

exit 0
