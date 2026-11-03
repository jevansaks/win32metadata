#Requires -Version 7.0
[CmdletBinding()]
param(
    [Parameter(Mandatory)][ValidateSet('pre-midl', 'post-midl')][string]$Phase,
    [Parameter(Mandatory)][string]$HeaderRoot,
    [Parameter(Mandatory)][string]$SdkVersion,
    [string[]]$SelectedPatches = @(),
    [string]$PatchRoot = "$PSScriptRoot\..\generation\WinSDK\patches",
    [string]$MidlInputRoot = $HeaderRoot,
    [object[]]$MidlOutputs
)

$ErrorActionPreference = 'Stop'
if ($SelectedPatches.Count -eq 0) { return }

function Assert-RegularPath([string]$Root, [string]$RelativePath) {
    $current = $Root
    foreach ($segment in @('') + ($RelativePath -split '/')) {
        if ($segment) { $current = Join-Path $current $segment }
        $item = Get-Item -LiteralPath $current -Force
        if ($item.Attributes -band [IO.FileAttributes]::ReparsePoint) {
            throw "SDK patches cannot follow reparse points: $current"
        }
    }
    if (!(Test-Path -LiteralPath $current -PathType Leaf)) { throw "Missing patch target/file: $current" }
    return $current
}

function Invoke-GitApply([string[]]$Arguments) {
    $output = & git -c core.autocrlf=false -c apply.ignoreWhitespace=no -C $staging apply -p4 --whitespace=nowarn @Arguments 2>&1
    if ($LASTEXITCODE -ne 0) { throw "SDK $Phase patch failed. Regenerate against SDK $SdkVersion.`n$($output -join "`n")" }
}

function Assert-Hunks([string]$Text, [string]$Path) {
    $escaped = [regex]::Escape($Path)
    $header = [regex]::Match($Text, "\Adiff --git a/$escaped b/$escaped`n(index [0-9a-f]+\.\.[0-9a-f]+ 100644`n)?--- a/$escaped`n\+\+\+ b/$escaped`n")
    if (!$header.Success -or !$Text.EndsWith("`n")) { throw "Invalid SDK patch header: $Path" }
    $body = $Text.Substring($header.Length)
    $old = 0
    $new = 0
    $hunks = 0
    foreach ($line in $body.Substring(0, $body.Length - 1).Split("`n")) {
        if ($line -match '^@@ -[0-9]+(,([0-9]+))? \+[0-9]+(,([0-9]+))? @@.*$') {
            if ($old -ne 0 -or $new -ne 0) { throw "Incomplete SDK patch hunk: $Path" }
            $old = if ($Matches[1]) { [int]$Matches[2] } else { 1 }
            $new = if ($Matches[3]) { [int]$Matches[4] } else { 1 }
            $hunks++
        } elseif ($hunks -gt 0 -and $line -eq '\ No newline at end of file') {
            continue
        } elseif ($hunks -gt 0 -and $line.StartsWith(' ')) {
            $old--; $new--
        } elseif ($hunks -gt 0 -and $line.StartsWith('-')) {
            $old--
        } elseif ($hunks -gt 0 -and $line.StartsWith('+')) {
            $new--
        } else { throw "Invalid/unconsumed SDK patch line: $Path" }
        if ($old -lt 0 -or $new -lt 0) { throw "SDK patch hunk length mismatch: $Path" }
    }
    if ($hunks -eq 0 -or $old -ne 0 -or $new -ne 0) { throw "Incomplete SDK patch: $Path" }
}

$HeaderRoot = (Resolve-Path -LiteralPath $HeaderRoot).Path
$PatchRoot = (Resolve-Path -LiteralPath $PatchRoot).Path
if (!$PSBoundParameters.ContainsKey('MidlOutputs')) {
    $MidlOutputs = @(& "$PSScriptRoot\Get-SDKMidlOutputs.ps1" -HeaderRoot $MidlInputRoot)
}
$generatedHeaders = [Collections.Generic.Dictionary[string,string]]::new([StringComparer]::OrdinalIgnoreCase)
foreach ($output in $MidlOutputs) {
    $generatedHeaders[$output.Header] = $output.Idl
}
$seen = [Collections.Generic.HashSet[string]]::new([StringComparer]::OrdinalIgnoreCase)
$targets = [Collections.Generic.HashSet[string]]::new([StringComparer]::OrdinalIgnoreCase)
$patches = @()
foreach ($selection in $SelectedPatches) {
    if ($selection -notmatch '^(pre-midl|post-midl)/([A-Za-z0-9_.-]+\.(idl|h))\.patch$' -or
        !$seen.Add($selection)) { throw "Invalid or duplicate SDK patch selection: $selection" }
    $patchPhase = $Matches[1]
    $owner = $Matches[2]
    if ($patchPhase -eq 'post-midl' -and $Matches[3] -ne 'h') { throw "Post-MIDL patches must own a header: $selection" }
    $file = Assert-RegularPath $PatchRoot $selection
    # Latin-1 is a byte-preserving transport, not a conversion of legacy SDK encodings.
    $text = [Text.Encoding]::Latin1.GetString([IO.File]::ReadAllBytes($file)).Replace("`r`n", "`n")
    if ($text -notmatch '\A# SDK-Version: ([0-9]+\.[0-9]+\.[0-9]+\.[0-9]+)\n' -or
        $Matches[1] -cne $SdkVersion) { throw "SDK version mismatch or missing SDK-Version in $selection (SDK $SdkVersion)." }
    $text = $text.Substring($text.IndexOf("`n") + 1)
    $prefix = 'generation/WinSDK/RecompiledIdlHeaders/'
    $headers = [regex]::Matches($text, '(?m)^diff --git a/(\S+) b/(\S+)$')
    if ($headers.Count -ne 1 -or $headers[0].Groups[1].Value -cne $headers[0].Groups[2].Value) {
        throw "Patch must modify exactly one existing owning file: $selection"
    }
    $path = $headers[0].Groups[1].Value
    if (!$path.StartsWith($prefix, [StringComparison]::Ordinal)) { throw "Invalid SDK target: $path" }
    $relative = $path.Substring($prefix.Length)
    if ($relative -notmatch '^(um|shared|winrt|ucrt)/([A-Za-z0-9_-]+/)*[A-Za-z0-9_.-]+$' -or
        [IO.Path]::GetFileName($relative) -cne $owner -or !$targets.Add("$patchPhase/$relative") -or
        $text -match '(?m)^(new file mode|deleted file mode|old mode|new mode|rename |copy |GIT binary patch|Binary files)' -or
        [regex]::Matches($text, '(?m)^--- ').Count -ne 1 -or
        [regex]::Matches($text, '(?m)^\+\+\+ ').Count -ne 1 -or
        !$text.Contains("--- a/$path`n+++ b/$path`n")) {
        throw "Invalid SDK owning file or patch operation: $selection"
    }
    Assert-Hunks $text $path
    $target = Assert-RegularPath $HeaderRoot $relative
    if ($generatedHeaders.ContainsKey($relative)) {
        throw "SDK patch $selection targets regenerated header $relative. Patch authoritative IDL $($generatedHeaders[$relative]) in pre-midl instead."
    }
    if ($patchPhase -eq $Phase) {
        $patches += @{ Selection = $selection; Relative = $relative; Target = $target; Text = $text }
    }
}

if ($patches.Count -eq 0) { return }
$staging = Join-Path ([IO.Path]::GetTempPath()) ("win32metadata-sdk-patches-" + [guid]::NewGuid().ToString('N'))
New-Item -ItemType Directory -Path $staging | Out-Null
try {
    foreach ($patch in $patches) {
        $destination = Join-Path $staging $patch.Relative
        New-Item -ItemType Directory -Force (Split-Path $destination) | Out-Null
        Copy-Item -LiteralPath $patch.Target -Destination $destination
        $bytes = [IO.File]::ReadAllBytes($destination)
        $content = [Text.Encoding]::Latin1.GetString($bytes)
        if ($content.Contains([char]0)) { throw "Unsupported SDK header encoding: $($patch.Target)" }
        $crlf = $content.Contains("`r`n")
        if ($content.Replace("`r`n", '').Contains("`r") -or
            ($crlf -and $content.Replace("`r`n", '').Contains("`n"))) {
            throw "Mixed/unsupported SDK line endings: $($patch.Target)"
        }
        $text = $patch.Text
        if ($crlf) { $text = $text.Replace("`n", "`r`n") }
        $patchFile = Join-Path $staging 'input.patch'
        [IO.File]::WriteAllBytes($patchFile, [Text.Encoding]::Latin1.GetBytes($text))
        Invoke-GitApply @('--check', $patchFile)
        Invoke-GitApply @($patchFile)
        $patch.Original = $bytes
    }
    # No failed hunk may leave a partially applied phase in the authoritative tree.
    foreach ($patch in $patches) {
        if (![Linq.Enumerable]::SequenceEqual[byte]($patch.Original, [IO.File]::ReadAllBytes($patch.Target))) {
            throw "SDK target changed during patch application: $($patch.Target)"
        }
    }
    foreach ($patch in $patches) {
        Copy-Item -LiteralPath (Join-Path $staging $patch.Relative) -Destination $patch.Target
        Write-Host "Applied SDK $Phase patch: $($patch.Selection)"
    }
} finally {
    Remove-Item -LiteralPath $staging -Recurse -Force
}
