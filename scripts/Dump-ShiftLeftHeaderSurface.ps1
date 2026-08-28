[CmdletBinding()]
param(
    [Parameter(Mandatory = $true)]
    [string]$Rdl,

    [Parameter(Mandatory = $true)]
    [string]$Namespace,

    [string]$PreexistingWindowsRsNamespace = "Windows.Win32",

    [Parameter(Mandatory = $true)]
    [string]$PreexistingWin32MetadataDump,

    [Parameter(Mandatory = $true)]
    [string]$PreexistingWindowsRsDump,

    [Parameter(Mandatory = $true)]
    [string]$ShiftLeftDump,

    [Parameter(Mandatory = $true)]
    [string]$OutputDirectory
)

$ErrorActionPreference = "Stop"

foreach ($path in @(
    $Rdl,
    $PreexistingWin32MetadataDump,
    $PreexistingWindowsRsDump,
    $ShiftLeftDump)) {
    if (!(Test-Path $path)) {
        throw "Required input was not found: $path"
    }
}

function Get-RdlInventory([string]$path) {
    $items = foreach ($line in [System.IO.File]::ReadLines($path)) {
        if ($line -match '^\s*extern "system" fn\s+([A-Za-z_][A-Za-z0-9_]*)\b') {
            [pscustomobject]@{ Category = "function"; Symbol = $Matches[1] }
        }
        elseif ($line -match '^\s*struct\s+([A-Za-z_][A-Za-z0-9_]*)\b') {
            [pscustomobject]@{ Category = "struct"; Symbol = $Matches[1] }
        }
        elseif ($line -match '^\s*enum\s+([A-Za-z_][A-Za-z0-9_]*)\b') {
            [pscustomobject]@{ Category = "enum"; Symbol = $Matches[1] }
        }
        elseif ($line -match '^\s*interface\s+([A-Za-z_][A-Za-z0-9_]*)\b') {
            [pscustomobject]@{ Category = "interface"; Symbol = $Matches[1] }
        }
        elseif ($line -match '^\s*type\s+([A-Za-z_][A-Za-z0-9_]*)\b') {
            [pscustomobject]@{ Category = "type"; Symbol = $Matches[1] }
        }
        elseif ($line -match '^\s*(?:const|static)\s+([A-Za-z_][A-Za-z0-9_]*)\b') {
            [pscustomobject]@{ Category = "constant"; Symbol = $Matches[1] }
        }
    }
    return @($items | Sort-Object Category, Symbol -Unique)
}

function New-DumpIndex([string]$path, [string]$targetNamespace) {
    $lines = [System.IO.File]::ReadAllLines($path)
    $namespaces = [string[]]::new($lines.Count)
    $declarations = @{}
    $currentNamespace = $null
    for ($i = 0; $i -lt $lines.Count; $i++) {
        if ($lines[$i] -match '^namespace\s+([^;]+);') {
            $currentNamespace = $Matches[1]
        }
        $namespaces[$i] = $currentNamespace
        if ($currentNamespace -ne $targetNamespace) {
            continue
        }

        $category = $null
        $symbol = $null
        if ($lines[$i] -match '^\s*public\s+.*\bextern\s+.+?\s+([A-Za-z_][A-Za-z0-9_]*)\s*\(') {
            $category = "function"
            $symbol = $Matches[1]
        }
        elseif ($lines[$i] -match '^\s*public\s+(?:const|static)\s+.*\b([A-Za-z_][A-Za-z0-9_]*)\s*(?:=|;)') {
            $category = "constant"
            $symbol = $Matches[1]
        }
        elseif ($lines[$i] -match '^\s*public\s+.*\b(struct|enum|interface)\s+([A-Za-z_][A-Za-z0-9_]*)\b') {
            $category = $Matches[1]
            $symbol = $Matches[2]
        }
        elseif ($lines[$i] -match '^\s*public\s+.*\bdelegate\s+.*\b([A-Za-z_][A-Za-z0-9_]*)\s*\(') {
            $category = "type"
            $symbol = $Matches[1]
        }
        if ($category -and !$declarations.ContainsKey("$category|$symbol")) {
            $declarations["$category|$symbol"] = $i
        }
        if ($category -eq "struct" -and
            $i -gt 0 -and
            $lines[$i - 1].Trim() -eq "[NativeTypedef]" -and
            !$declarations.ContainsKey("type|$symbol")) {
            $declarations["type|$symbol"] = $i
        }
    }
    return @{
        Lines = $lines
        Namespaces = $namespaces
        Declarations = $declarations
    }
}

function Get-DeclarationSnippet($index, [string]$namespace, [string]$category, [string]$symbol) {
    $lines = $index.Lines
    $namespaces = $index.Namespaces
    $key = "$category|$symbol"
    if (!$index.Declarations.ContainsKey($key)) {
        return $null
    }
    $declaration = $index.Declarations[$key]

    $start = $declaration
    while ($start -gt 0 -and
        $namespaces[$start - 1] -eq $namespace -and
        $lines[$start - 1].TrimStart().StartsWith("[")) {
        $start--
    }

    $end = $declaration
    if ($category -in @("struct", "enum", "interface") -or
        ($category -eq "type" -and $lines[$declaration] -notmatch "\bdelegate\b")) {
        $depth = 0
        $opened = $false
        for ($i = $declaration; $i -lt $lines.Count; $i++) {
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

    return ($lines[$start..$end] -join "`r`n").Trim()
}

New-Item -ItemType Directory -Force -Path $OutputDirectory | Out-Null
$inventory = Get-RdlInventory $Rdl
$sources = @(
    @{
        Name = "Pre-existing win32metadata"
        File = "preexisting-win32metadata.txt"
        Namespace = $Namespace
        Index = New-DumpIndex $PreexistingWin32MetadataDump $Namespace
    },
    @{
        Name = "Pre-existing windows-rs"
        File = "preexisting-windows-rs.txt"
        Namespace = $PreexistingWindowsRsNamespace
        Index = New-DumpIndex $PreexistingWindowsRsDump $PreexistingWindowsRsNamespace
    },
    @{
        Name = "Shift-left conversion"
        File = "shift-left-conversion.txt"
        Namespace = $Namespace
        Index = New-DumpIndex $ShiftLeftDump $Namespace
    }
)

$rows = foreach ($item in $inventory) {
    $row = [ordered]@{
        Category = $item.Category
        Symbol = $item.Symbol
    }
    foreach ($source in $sources) {
        $snippet = Get-DeclarationSnippet `
            $source.Index $source.Namespace $item.Category $item.Symbol
        $row[$source.Name] = if ($snippet) { "present" } else { "missing" }
        $row["$($source.Name) snippet"] = $snippet
    }
    [pscustomobject]$row
}

foreach ($source in $sources) {
    $sections = foreach ($category in @(
        "function", "struct", "enum", "constant", "interface", "type")) {
        $snippets = foreach ($row in $rows | Where-Object Category -eq $category) {
            $snippet = $row."$($source.Name) snippet"
            @"
// --------------------------------------------------------------------------
// $($row.Symbol)
// --------------------------------------------------------------------------
$($snippet ?? "<missing>")
"@
        }
        @"
// ==========================================================================
// $($category.ToUpperInvariant())S
// ==========================================================================
$($snippets -join "`r`n`r`n")
"@
    }
    [System.IO.File]::WriteAllText(
        (Join-Path $OutputDirectory $source.File),
        ($sections -join "`r`n`r`n"),
        [System.Text.UTF8Encoding]::new($false))
}

$matrix = $rows | Select-Object `
    Category,
    Symbol,
    "Pre-existing win32metadata",
    "Pre-existing windows-rs",
    "Shift-left conversion"
$matrix | Export-Csv (Join-Path $OutputDirectory "inventory.csv") -NoTypeInformation
$rows | Select-Object `
    Category,
    Symbol,
    "Pre-existing win32metadata snippet",
    "Pre-existing windows-rs snippet",
    "Shift-left conversion snippet" |
    Export-Csv (Join-Path $OutputDirectory "declarations.csv") -NoTypeInformation

$categoryRows = foreach ($category in @(
    "function", "struct", "enum", "constant", "interface", "type")) {
    $categoryItems = @($rows | Where-Object Category -eq $category)
    "| $category | $($categoryItems.Count) |"
}
$presenceRows = foreach ($source in $sources) {
    $present = @($rows | Where-Object { $_."$($source.Name)" -eq "present" }).Count
    "| $($source.Name) | $present | $($rows.Count - $present) |"
}

$summary = @'
# `winspool.h` surface comparison

The inventory is derived from every declaration emitted from the patched
`winspool.h` RDL. Each symbol is then looked up in the existing win32metadata
winmd, the existing windows-rs winmd, and the new shift-left conversion.
The existing windows-rs metadata uses its historical flat `Windows.Win32`
namespace; the other two use the partition namespace.
'@ + @"

| Category | Symbols |
| --- | ---: |
$($categoryRows -join "`r`n")
| **Total** | **$($rows.Count)** |

| Source | Present | Missing |
| --- | ---: | ---: |
$($presenceRows -join "`r`n")

Files:

- `inventory.csv`: symbol-by-symbol presence matrix.
- `declarations.csv`: all three declarations side-by-side, including multiline bodies.
- `preexisting-win32metadata.txt`: complete matching declarations.
- `preexisting-windows-rs.txt`: complete matching declarations.
- `shift-left-conversion.txt`: complete matching declarations.
- `winspool.rdl`: native-header-derived RDL used as the inventory source.

The inventory intentionally excludes unrelated declarations from other headers
that share `Windows.Win32.Graphics.Printing`. Synthetic sidecar types that do
not correspond to a native `winspool.h` declaration appear through signatures
that use them, such as the existing `PRINTER_HANDLE` parameters.

RDL `type` aliases are listed in the inventory even when they collapse into
their underlying metadata representation and therefore have no standalone
winmd declaration.
"@
[System.IO.File]::WriteAllText(
    (Join-Path $OutputDirectory "README.md"),
    $summary,
    [System.Text.UTF8Encoding]::new($false))

Copy-Item $Rdl (Join-Path $OutputDirectory "winspool.rdl") -Force
Write-Host "Wrote $($rows.Count) symbols to $OutputDirectory"
