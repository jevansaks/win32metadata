param(
    [Parameter(Mandatory = $true)]
    [string]$SdkPackageRoot,

    [Parameter(Mandatory = $true)]
    [string]$OutputPath,

    [string]$HeaderRoot
)

$ErrorActionPreference = 'Stop'
$repo = Split-Path (Split-Path (Split-Path $PSScriptRoot -Parent) -Parent) -Parent
if (!$HeaderRoot) {
    $HeaderRoot = Join-Path $SdkPackageRoot 'c\include\10.0.28000.0'
}
$HeaderRoot = (Resolve-Path -LiteralPath $HeaderRoot).Path
$midl = Join-Path $SdkPackageRoot 'c\bin\10.0.28000.0\x64\midl.exe'
if (!(Test-Path -LiteralPath $midl)) {
    throw "Missing selected SDK MIDL: $midl"
}
if (Test-Path -LiteralPath $OutputPath) {
    throw "Use a new output directory to avoid mixing native-MIDL evidence: $OutputPath"
}
$OutputPath = (New-Item -ItemType Directory -Path $OutputPath).FullName
$records = @()
foreach ($mode in 'native', 'converted') {
    $output = (New-Item -ItemType Directory -Path (Join-Path $OutputPath $mode)).FullName
    foreach ($owner in 'dispex', 'docobj', 'bdaiface') {
        $inputFile = Join-Path $HeaderRoot "um\$owner.idl"
        if ($mode -eq 'converted') {
            $convertedFile = Join-Path $output "$owner.idl"
            & (Join-Path $repo 'scripts\ConvertMidlAttributesToSalAnnotations.ps1') $inputFile $convertedFile
            $inputFile = $convertedFile
        }
        $arguments = @(
            $inputFile, '/out', $output, '/header', (Join-Path $output "$owner.h"),
            '/no_warn', '/DUNICODE', '/D_UNICODE', '/DWINVER=0x0A00',
            '/D_APISET_MINWIN_VERSION=0x010F', '/DNTDDI_VERSION=0x0A00000C',
            '/DBUILD_UMS_ENABLED=0', '/DBUILD_WOW64_ENABLED=0', '/DBUILD_ARM64X_ENABLED=0',
            '/DEXECUTABLE_WRITES_SUPPORT=0', '/D_USE_DECLSPECS_FOR_SAL=1',
            '/D_CONTROL_FLOW_GUARD_SVCTAB=1', '/DMIDL_PASS=1', '/D_AMD64_', '/D_WIN64',
            '/D_WCHAR_T_DEFINED', '/no_stamp', '/nologo', '/no_settings_comment',
            '/lcid', '1033', '/sal', '/sal_local', '/amd64', '/target', 'NT100', '/Zp8',
            '/I', (Join-Path $HeaderRoot 'um'), '/I', (Join-Path $HeaderRoot 'shared'),
            '/I', (Join-Path $HeaderRoot 'winrt'), '/I', (Join-Path $repo 'scripts\inc')
        )
        $arguments | ConvertTo-Json | Set-Content -LiteralPath (Join-Path $output "$owner.arguments.json")
        & $midl @arguments *> (Join-Path $output "$owner.log")
        if ($LASTEXITCODE -ne 0) {
            throw "MIDL failed for ${mode}/${owner}; see $(Join-Path $output "$owner.log")"
        }
    }
}

$cases = @(
    @{ Owner = 'dispex'; Method = 'GetNameSpaceParent' },
    @{ Owner = 'dispex'; Method = 'QueryErrorInfo' },
    @{ Owner = 'dispex'; Method = 'GetNext' },
    @{ Owner = 'docobj'; Method = 'QueryStatus' },
    @{ Owner = 'bdaiface'; Method = 'get_FrequencyMapping' },
    @{ Owner = 'bdaiface'; Method = 'get_DefaultFrequencyMapping' }
)
foreach ($case in $cases) {
    $record = [ordered]@{ owner = $case.Owner; method = $case.Method }
    foreach ($mode in 'native', 'converted') {
        $text = Get-Content -LiteralPath (Join-Path $OutputPath "$mode\$($case.Owner).h") -Raw
        $match = [regex]::Match($text, 'virtual[^;]*?\b' + $case.Method + '\s*\([^;]+?= 0;')
        if (!$match.Success) {
            throw "Missing actual native method $($case.Owner)::$($case.Method) in $mode"
        }
        $record[$mode] = $match.Value
    }
    $records += [pscustomobject]$record
}
$records | ConvertTo-Json -Depth 4 | Set-Content -LiteralPath (Join-Path $OutputPath 'native-sal-comparison.json')
Write-Host "Native and converted headers retained in $OutputPath; compilation success is not SAL equivalence."
