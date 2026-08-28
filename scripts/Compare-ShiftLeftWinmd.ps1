[CmdletBinding()]
param(
    [Parameter(Mandatory = $true)]
    [string]$ReferenceWinmd,

    [Parameter(Mandatory = $true)]
    [string]$GeneratedWinmd,

    [Parameter(Mandatory = $true)]
    [string]$OutputDirectory,

    [string]$WinmdUtils = "$PSScriptRoot\..\bin\Release\net8.0\WinmdUtils.dll"
)

$ErrorActionPreference = "Stop"

New-Item -ItemType Directory -Force -Path $OutputDirectory | Out-Null

$referenceDump = Join-Path $OutputDirectory "reference.cs"
$generatedDump = Join-Path $OutputDirectory "generated.cs"

if (!(Test-Path $WinmdUtils)) {
    throw "WinmdUtils was not found at '$WinmdUtils'. Build sources\WinmdUtils first."
}

& dotnet $WinmdUtils dump --winmd $ReferenceWinmd --output $referenceDump
if ($LASTEXITCODE -ne 0) {
    throw "Failed to dump reference winmd."
}

& dotnet $WinmdUtils dump --winmd $GeneratedWinmd --output $generatedDump
if ($LASTEXITCODE -ne 0) {
    throw "Failed to dump generated winmd."
}

function Normalize-Declaration([string]$line) {
    $line = $line.Trim()
    $line = $line -replace "\s+", " "
    $line = $line -replace "\bWindows\.Win32(?:\.[A-Za-z_][A-Za-z0-9_]*)*\.", ""
    return $line
}

function Get-Surface([string]$path) {
    $types = [System.Collections.Generic.HashSet[string]]::new()
    $typeNames = [System.Collections.Generic.HashSet[string]]::new()
    $pinvokes = [System.Collections.Generic.HashSet[string]]::new()
    $pinvokeNames = [System.Collections.Generic.HashSet[string]]::new()
    $members = [System.Collections.Generic.HashSet[string]]::new()
    $memberNames = [System.Collections.Generic.HashSet[string]]::new()
    $attributes = @{}

    $currentType = $null
    $depth = 0
    $pendingAttributes = [System.Collections.Generic.List[string]]::new()

    foreach ($rawLine in [System.IO.File]::ReadLines($path)) {
        $line = $rawLine.Trim()

        if ($line.StartsWith("[") -and $line.EndsWith("]")) {
            if ($line -notmatch "^\[Documentation\b") {
                $pendingAttributes.Add((Normalize-Declaration $line))
            }
            continue
        }

        if ($line -match "^public\s+(?:(?:unsafe|static|sealed|abstract)\s+)*(class|struct|interface|enum|delegate)\s+([A-Za-z_][A-Za-z0-9_]*)") {
            $kind = $Matches[1]
            $name = $Matches[2]
            $currentType = $name
            $depth = 1
            $types.Add("$kind`:$name") | Out-Null
            $typeNames.Add($name) | Out-Null
            foreach ($attribute in $pendingAttributes) {
                $attributes[$attribute] = 1 + ($attributes[$attribute] ?? 0)
            }
            $pendingAttributes.Clear()
        }
        elseif ($line -match "^public\s+(?:(?:unsafe|static)\s+)*extern\s+.+?\s+([A-Za-z_][A-Za-z0-9_]*)\s*\(") {
            $name = $Matches[1]
            $signature = Normalize-Declaration $line
            $pinvokes.Add("$name|$signature") | Out-Null
            $pinvokeNames.Add($name) | Out-Null
            foreach ($attribute in $pendingAttributes) {
                $attributes[$attribute] = 1 + ($attributes[$attribute] ?? 0)
            }
            $pendingAttributes.Clear()
        }
        elseif ($currentType -and $line -match "^public\b.*\b([A-Za-z_][A-Za-z0-9_]*)\s*(?:[;{=(]|=>)") {
            $name = $Matches[1]
            $signature = Normalize-Declaration $line
            $members.Add("$currentType.$name|$signature") | Out-Null
            $memberNames.Add("$currentType.$name") | Out-Null
            foreach ($attribute in $pendingAttributes) {
                $attributes[$attribute] = 1 + ($attributes[$attribute] ?? 0)
            }
            $pendingAttributes.Clear()
        }
        elseif ($currentType -and $line -match "^([A-Za-z_][A-Za-z0-9_]*)\s*=\s*([^,]+),?$") {
            $name = $Matches[1]
            $value = ($Matches[2] -replace "\s+", "")
            $members.Add("$currentType.$name|$value") | Out-Null
            $memberNames.Add("$currentType.$name") | Out-Null
            $pendingAttributes.Clear()
        }
        elseif ($line.Length -gt 0 -and !$line.StartsWith("//") -and !$line.StartsWith("using ") -and !$line.StartsWith("namespace ")) {
            $pendingAttributes.Clear()
        }

        $depth += ([regex]::Matches($rawLine, "\{")).Count
        $depth -= ([regex]::Matches($rawLine, "\}")).Count
        if ($currentType -and $line -eq "}" -and $depth -eq 1) {
            $currentType = $null
            $depth = 0
        }
    }

    return @{
        Types = $types
        TypeNames = $typeNames
        PInvokes = $pinvokes
        PInvokeNames = $pinvokeNames
        Members = $members
        MemberNames = $memberNames
        Attributes = $attributes
    }
}

function Compare-Set($reference, $generated) {
    $matched = 0
    $missing = [System.Collections.Generic.List[string]]::new()
    foreach ($item in $reference) {
        if ($generated.Contains($item)) {
            $matched++
        }
        else {
            $missing.Add($item)
        }
    }

    $extra = [System.Collections.Generic.List[string]]::new()
    foreach ($item in $generated) {
        if (!$reference.Contains($item)) {
            $extra.Add($item)
        }
    }

    $coverage = if ($reference.Count -eq 0) { 100 } else { 100 * $matched / $reference.Count }
    return @{
        Reference = $reference.Count
        Generated = $generated.Count
        Matched = $matched
        Missing = $missing
        Extra = $extra
        Coverage = $coverage
    }
}

$reference = Get-Surface $referenceDump
$generated = Get-Surface $generatedDump

$typeDelta = Compare-Set $reference.Types $generated.Types
$typeNameDelta = Compare-Set $reference.TypeNames $generated.TypeNames
$pinvokeDelta = Compare-Set $reference.PInvokes $generated.PInvokes
$pinvokeNameDelta = Compare-Set $reference.PInvokeNames $generated.PInvokeNames
$memberDelta = Compare-Set $reference.Members $generated.Members
$memberNameDelta = Compare-Set $reference.MemberNames $generated.MemberNames

$weightedReference = $typeNameDelta.Reference + $pinvokeNameDelta.Reference + $memberNameDelta.Reference
$weightedMatched = $typeNameDelta.Matched + $pinvokeNameDelta.Matched + $memberNameDelta.Matched
$logicalSimilarity = if ($weightedReference -eq 0) { 100 } else { 100 * $weightedMatched / $weightedReference }

$typeDelta.Missing | Sort-Object | Set-Content (Join-Path $OutputDirectory "missing-types.txt")
$typeDelta.Extra | Sort-Object | Set-Content (Join-Path $OutputDirectory "extra-types.txt")
$typeNameDelta.Missing | Sort-Object | Set-Content (Join-Path $OutputDirectory "missing-type-names.txt")
$typeNameDelta.Extra | Sort-Object | Set-Content (Join-Path $OutputDirectory "extra-type-names.txt")
$pinvokeDelta.Missing | Sort-Object | Set-Content (Join-Path $OutputDirectory "missing-pinvokes.txt")
$pinvokeDelta.Extra | Sort-Object | Set-Content (Join-Path $OutputDirectory "extra-pinvokes.txt")
$pinvokeNameDelta.Missing | Sort-Object | Set-Content (Join-Path $OutputDirectory "missing-pinvoke-names.txt")
$pinvokeNameDelta.Extra | Sort-Object | Set-Content (Join-Path $OutputDirectory "extra-pinvoke-names.txt")
$memberDelta.Missing | Sort-Object | Set-Content (Join-Path $OutputDirectory "missing-members.txt")
$memberDelta.Extra | Sort-Object | Set-Content (Join-Path $OutputDirectory "extra-members.txt")
$memberNameDelta.Missing | Sort-Object | Set-Content (Join-Path $OutputDirectory "missing-member-names.txt")
$memberNameDelta.Extra | Sort-Object | Set-Content (Join-Path $OutputDirectory "extra-member-names.txt")

$report = @"
# Shift-Left Winmd Logical Delta

## Summary

| Identity surface | Reference | Generated | Matches | Coverage |
| --- | ---: | ---: | ---: | ---: |
| Type simple names | $($typeNameDelta.Reference) | $($typeNameDelta.Generated) | $($typeNameDelta.Matched) | $($typeNameDelta.Coverage.ToString("F2"))% |
| P/Invoke names | $($pinvokeNameDelta.Reference) | $($pinvokeNameDelta.Generated) | $($pinvokeNameDelta.Matched) | $($pinvokeNameDelta.Coverage.ToString("F2"))% |
| Type/member names | $($memberNameDelta.Reference) | $($memberNameDelta.Generated) | $($memberNameDelta.Matched) | $($memberNameDelta.Coverage.ToString("F2"))% |
| **Weighted identity similarity** | $weightedReference |  | $weightedMatched | **$($logicalSimilarity.ToString("F2"))%** |

## Exact declaration agreement

| Exact surface | Reference | Generated | Exact matches | Coverage |
| --- | ---: | ---: | ---: | ---: |
| Types by kind and simple name | $($typeDelta.Reference) | $($typeDelta.Generated) | $($typeDelta.Matched) | $($typeDelta.Coverage.ToString("F2"))% |
| P/Invoke signatures | $($pinvokeDelta.Reference) | $($pinvokeDelta.Generated) | $($pinvokeDelta.Matched) | $($pinvokeDelta.Coverage.ToString("F2"))% |
| Member signatures and enum values | $($memberDelta.Reference) | $($memberDelta.Generated) | $($memberDelta.Matched) | $($memberDelta.Coverage.ToString("F2"))% |

Identity comparison ignores namespace partitioning, declaration kind, signature spelling,
and documentation attributes. Exact comparison retains normalized signatures. The gap
between identity and exact coverage is the primary annotation/type-remap delta.

## Detailed files

- missing-types.txt / extra-types.txt
- missing-type-names.txt / extra-type-names.txt
- missing-pinvokes.txt / extra-pinvokes.txt
- missing-pinvoke-names.txt / extra-pinvoke-names.txt
- missing-members.txt / extra-members.txt
- missing-member-names.txt / extra-member-names.txt
- reference.cs / generated.cs

## Required classification

Review unmatched entries in this order:

1. Ownership, invalid values, calling conventions, DLL mappings, parameter direction,
   pointer depth, arrays/sizes, enum values, and layout differences.
2. Missing current-SDK versus legacy declarations.
3. Type aliases and namespace/toolchain spelling differences.
4. Documentation-only and ordering differences.
"@

$reportPath = Join-Path $OutputDirectory "winmd-delta.md"
[System.IO.File]::WriteAllText($reportPath, $report, [System.Text.UTF8Encoding]::new($false))
Write-Host "Logical similarity: $($logicalSimilarity.ToString("F2"))%"
Write-Host "Report: $reportPath"
