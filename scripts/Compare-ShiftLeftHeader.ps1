[CmdletBinding()]
param(
    [Parameter(Mandatory = $true)]
    [string]$Header,

    [Parameter(Mandatory = $true)]
    [string]$Partition,

    [Parameter(Mandatory = $true)]
    [string]$ReferenceWinmd,

    [Parameter(Mandatory = $true)]
    [string]$GeneratedWinmd,

    [Parameter(Mandatory = $true)]
    [string]$GeneratedRdl,

    [string]$RootRdl,

    [Parameter(Mandatory = $true)]
    [string]$OutputDirectory,

    [string]$ProgressManifest = "$PSScriptRoot\..\generation\WinSDK\patches\header-progress.json",

    [string]$WinmdUtils = "$PSScriptRoot\..\bin\Release\net8.0\WinmdUtils.dll",

    [switch]$FullHeader
)

$ErrorActionPreference = "Stop"

foreach ($path in @($ReferenceWinmd, $GeneratedWinmd, $GeneratedRdl, $ProgressManifest, $WinmdUtils)) {
    if (!(Test-Path $path)) {
        throw "Required input was not found: $path"
    }
    if (!$RootRdl) {
        $RootRdl = $GeneratedRdl
    }
    if (!(Test-Path $RootRdl)) {
        throw "Required input was not found: $RootRdl"
    }
}

$entry = (Get-Content $ProgressManifest -Raw | ConvertFrom-Json).headers |
    Where-Object { $_.header -ieq $Header -and $_.partition -ieq $Partition } |
    Select-Object -First 1
if (!$entry) {
    $plan = Join-Path (Split-Path $ProgressManifest) "header-plan\partition-header-queue.csv"
    if (Test-Path $plan) {
        $entry = Import-Csv $plan |
            Where-Object { $_.header -ieq $Header -and $_.partition -ieq $Partition } |
            Select-Object -First 1
    }
}
if (!$entry) {
    throw "No progress or all-SDK plan entry exists for $Header / $Partition."
}
if (!$FullHeader -and (!$entry.targetSymbols -or $entry.targetSymbols.Count -eq 0)) {
    throw "$Header / $Partition does not yet define targetSymbols in the progress manifest."
}

New-Item -ItemType Directory -Force -Path $OutputDirectory | Out-Null
$referenceDump = Join-Path $OutputDirectory "reference.cs"
$generatedDump = Join-Path $OutputDirectory "generated.cs"

& dotnet $WinmdUtils dump --winmd $ReferenceWinmd --output $referenceDump
if ($LASTEXITCODE -ne 0) {
    throw "Failed to dump reference winmd."
}
& dotnet $WinmdUtils dump --winmd $GeneratedWinmd --output $generatedDump
if ($LASTEXITCODE -ne 0) {
    throw "Failed to dump generated winmd."
}

function Get-RdlInventory([string]$path) {
    $items = foreach ($line in [System.IO.File]::ReadLines($path)) {
        if ($line -match '^\s*extern "system" fn\s+([A-Za-z_][A-Za-z0-9_]*)\b') {
            $Matches[1]
        }
        elseif ($line -match '^\s*(?:struct|enum|interface|type)\s+([A-Za-z_][A-Za-z0-9_]*)\b') {
            $Matches[1]
        }
        elseif ($line -match '^\s*(?:const|static)\s+([A-Za-z_][A-Za-z0-9_]*)\b') {
            $Matches[1]
        }
    }
    return @($items | Sort-Object -Unique)
}

function Get-DeclarationSnippet([string[]]$lines, [string]$symbol) {
    $escaped = [regex]::Escape($symbol)
    $index = -1
    for ($i = 0; $i -lt $lines.Count; $i++) {
        if ($lines[$i] -match "^\s*public\s+.*\b(class|struct|interface|enum|delegate)\s+$escaped\b" -or
            $lines[$i] -match "^\s*public\s+.*\bextern\s+.*\s+$escaped\s*\(") {
            $index = $i
            break
        }
    }
    if ($index -lt 0) {
        return $null
    }

    $start = $index
    while ($start -gt 0 -and $lines[$start - 1].TrimStart().StartsWith("[")) {
        $start--
    }

    $end = $index
    if ($lines[$index] -match "^\s*public\s+(?:unsafe\s+)?(?:readonly\s+)?(?:struct|class|interface|enum|delegate)\b") {
        $depth = 0
        $opened = $false
        for ($i = $index; $i -lt $lines.Count; $i++) {
            $depth += ([regex]::Matches($lines[$i], "\{")).Count
            if ($depth -gt 0) {
                $opened = $true
            }
            $depth -= ([regex]::Matches($lines[$i], "\}")).Count
            $end = $i
            if ($opened -and $depth -eq 0) {
                break
            }
        }
    }

    return ($lines[$start..$end] -join "`n").Trim()
}

function Normalize-Snippet([string]$snippet) {
    if (!$snippet) {
        return $null
    }
    return (($snippet -split "`r?`n") |
        Where-Object {
            $_ -notmatch "^\s*\[(Documentation|Ansi|Unicode)\b"
        } |
        ForEach-Object {
            ($_ -replace "\bWindows\.Win32(?:\.[A-Za-z_][A-Za-z0-9_]*)*\.", "") -replace "\s+", " "
        }) -join "`n"
}

function Get-EnumMembers([string]$snippet) {
    $members = @{}
    $matches = [regex]::Matches(
        $snippet,
        "(?m)^\s*([A-Za-z_][A-Za-z0-9_]*)\s*=\s*(-?(?:0x[0-9A-Fa-f]+|[0-9]+))[uUlL]*\s*,?\s*$")
    foreach ($match in $matches) {
        $literal = $match.Groups[2].Value
        if ($literal.StartsWith("0x", [System.StringComparison]::OrdinalIgnoreCase)) {
            $value = [Convert]::ToUInt64($literal.Substring(2), 16)
        }
        else {
            $value = [Convert]::ToInt64($literal, 10)
        }
        $members[$match.Groups[1].Value] = $value
    }
    return $members
}

function Test-ExpectedEnumMembers([string]$snippet, $expectedMembers) {
    if (!$expectedMembers) {
        return $false
    }
    $actual = Get-EnumMembers $snippet
    $expected = @($expectedMembers.PSObject.Properties)
    if ($actual.Count -ne $expected.Count) {
        return $false
    }
    foreach ($member in $expected) {
        if (!$actual.ContainsKey($member.Name) -or
            $actual[$member.Name] -ne [long]$member.Value) {
            return $false
        }
    }
    return $true
}

function Classify-Delta(
    [string]$symbol,
    [string]$reference,
    [string]$generated,
    $progressEntry) {
    if (!$reference) {
        return "unresolved:missing-reference"
    }
    if (!$generated) {
        return "unresolved:missing-generated"
    }
    if ((Normalize-Snippet $reference) -ceq (Normalize-Snippet $generated)) {
        return "exact"
    }
    $classifications = [System.Collections.Generic.List[string]]::new()
    if ($progressEntry.acceptedNormalizations -contains "rawHandleOwnership" -and
        $reference -match "\bPRINTER_HANDLE\b" -and
        $generated -match "\bHANDLE\b") {
        $lifecycle = $progressEntry.expectedLifecycle
        if ($lifecycle.producerSymbols -contains $symbol) {
            $freeFunction = [regex]::Escape($lifecycle.freeFunction)
            $hasExpectedLifecycle = $generated -match "\[RAIIFree\s*\(`"$freeFunction`"\)\]"
            foreach ($invalidValue in $lifecycle.invalidValues) {
                $value = [regex]::Escape([string]$invalidValue)
                $hasExpectedLifecycle = $hasExpectedLifecycle -and
                    $generated -match "\[InvalidHandleValue\s*\($value(?:L)?\)\]"
            }
            if (!$hasExpectedLifecycle) {
                $classifications.Add("unresolved:producerLifecycle")
            }
            else {
                $classifications.Add("accepted:rawHandleOwnership")
            }
        }
        else {
            $classifications.Add("accepted:rawHandleOwnership")
        }
    }
    if ($progressEntry.acceptedNormalizations -contains "associatedEnum" -and
        $reference -match "\bFONT_RESOURCE_CHARACTERISTICS\b" -and
        $generated -match '\[AssociatedEnum\s*\("FONT_RESOURCE_CHARACTERISTICS"\)\]' -and
        $generated -match "\buint\s+fl\b") {
        $classifications.Add("accepted:associatedEnum")
    }
    if ($progressEntry.acceptedNormalizations -contains "associatedEnum") {
        foreach ($match in [regex]::Matches($generated, '\[AssociatedEnum\s*\("([^"]+)"\)\]\s+uint\s+([A-Za-z_][A-Za-z0-9_]*)\b')) {
            $enum = [regex]::Escape($match.Groups[1].Value)
            $param = [regex]::Escape($match.Groups[2].Value)
            if ($reference -match "\b$enum\s+$param\b") {
                $classifications.Add("accepted:associatedEnum")
                break
            }
        }
    }
    if ($progressEntry.acceptedNormalizations -contains "scopedEnum" -and
        $reference -match "public enum FONT_RESOURCE_CHARACTERISTICS : uint" -and
        $generated -match "public enum FONT_RESOURCE_CHARACTERISTICS : uint" -and
        $generated -match "\[ScopedEnum\]" -and
        (Test-ExpectedEnumMembers $reference $progressEntry.expectedEnumMembers) -and
        (Test-ExpectedEnumMembers $generated $progressEntry.expectedEnumMembers)) {
        $classifications.Add("accepted:scopedEnum")
    }
    if ($reference -match "\[Const\]" -and $generated -notmatch "\[Const\]") {
        $classifications.Add("unresolved:nativeConst")
    }
    if ($reference -match "\[Optional\]\[Reserved\]" -and
        $generated -match "\[Reserved\]" -and
        $generated -notmatch "\[Optional\]\[Reserved\]") {
        $classifications.Add("unresolved:reservedOptional")
    }
    if ($classifications.Count -gt 0) {
        return $classifications -join "; "
    }
    return "unresolved:signature"
}

$referenceLines = [System.IO.File]::ReadAllLines($referenceDump)
$generatedLines = [System.IO.File]::ReadAllLines($generatedDump)
$targetSymbols = if ($FullHeader) {
    Get-RdlInventory $RootRdl
}
else {
    @($entry.targetSymbols)
}
$results = foreach ($symbol in $targetSymbols) {
    $reference = Get-DeclarationSnippet $referenceLines $symbol
    $generated = Get-DeclarationSnippet $generatedLines $symbol
    [pscustomobject]@{
        symbol = $symbol
        classification = Classify-Delta $symbol $reference $generated $entry
        reference = $reference
        generated = $generated
    }
}

$resultPath = Join-Path $OutputDirectory "result.json"
$results | ConvertTo-Json -Depth 5 | Set-Content -Path $resultPath -Encoding utf8NoBOM

$rows = foreach ($result in $results) {
    "| ``$($result.symbol)`` | ``$($result.classification)`` |"
}
$details = foreach ($result in $results) {
@"
### $($result.symbol)

**Classification:** ``$($result.classification)``

Reference:

``````csharp
$($result.reference ?? "<missing>")
``````

Generated:

``````csharp
$($result.generated ?? "<missing>")
``````
"@
}

$report = @"
# $Header / $Partition comparison

- Namespace: ``$($entry.namespace)``
- Generated RDL: ``$GeneratedRdl``
- Reference winmd: ``$ReferenceWinmd``
- Generated winmd: ``$GeneratedWinmd``

| Symbol | Classification |
| --- | --- |
$($rows -join "`n")

$($details -join "`n")
"@

$reportPath = Join-Path $OutputDirectory "comparison.md"
[System.IO.File]::WriteAllText($reportPath, $report, [System.Text.UTF8Encoding]::new($false))

$unresolved = @($results | Where-Object { $_.classification -match "(^|; )unresolved:" })
Write-Host "$Header / ${Partition}: $($results.Count - $unresolved.Count) classified, $($unresolved.Count) unresolved"
Write-Host "Report: $reportPath"
if ($unresolved.Count -gt 0) {
    exit 2
}
