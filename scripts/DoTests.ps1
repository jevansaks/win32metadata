param
(
    [switch]$SkipInstallTools
)

. $PSScriptRoot\CommonUtils.ps1

if (!$skipInstallTools)
{
    Install-BuildTools
}

& $PSScriptRoot\TestTooling.ps1

& $PSScriptRoot\TestWinmdBinary.ps1 -SkipInstallTools
