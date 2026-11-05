param
(
    [Parameter(Mandatory = $true)]
    [string]
    $inputFileName,

    [Parameter(Mandatory = $true)]
    [string]
    $outputFileName
)

$ErrorActionPreference = "Stop"

if (!(Test-Path $inputFileName)) {
    Write-Error "Couldn't find $inputFileName."
    exit -1
}

if (Test-Path $outputFileName) {
    Remove-Item $outputFileName
}

# Function to parse and validate size/length expressions from MIDL attributes
function SALParseSize {
    param(
        [string]$attribute,
        [string]$prefix
    )
    
    $extracted = $attribute.Substring("${prefix}(".Length);
    $extracted = $extracted.Substring(0, $extracted.Length - 1);
    $extracted = $extracted.Trim(", ")
    
    # If the string still has an internal comma, that means it's more complex than SAL can handle, omit it.
    # Also if there are parens but they're not at the front, that's someone using a macro inside the annotation
    # which the winmd tooling cannot currently handle. Skip.
    if ($extracted.Contains(",")) { 
        return $null 
    } elseif ($extracted.IndexOf("(") -gt 0) {
        return $null
    } elseif ($extracted.IndexOf("(") -eq 0) {
        # Strip any cast prefix like (ULONG) off the front since the tooling can't parse that.
        $castPrefixEnd = $extracted.IndexOf(")") + 1
        $extracted = $extracted.Substring($castPrefixEnd)
    }
    
    return $extracted
}

# Some SAL annotations can have parens in them like size_is((ULONG)cMembers). Allow strings like "(...)" or "((type)...)"
$parenBlob = "\((?:\([^()]*\)[^()]*|[^()]*)\)"
$attributeListPattern = [Regex]::new("\[(((?:,\s*)?(in|out|string|retval|unique|defaultvalue$parenBlob|size_is$parenBlob|iid_is$parenBlob|length_is$parenBlob))+)\]")
$attributePattern = [Regex]::new("(in|out|string|retval|unique|defaultvalue$parenBlob|size_is$parenBlob|iid_is$parenBlob|length_is$parenBlob)")

function Get-SalAnnotation([string]$attributeList) {
        $in = $false
        $out = $false
        $isComPtr = $false
        $optional = $false
        $size_is = $null
        $length_is = $null
        $parameters = $null

        Write-Verbose "Found attribute: $attributeList"

        # Process each attribute.
        foreach ($match in $attributePattern.Matches($attributeList)) {
            $attribute = $match.Groups[1].Value
            if ($attribute -eq "in") {
                $in = $true
            } elseif ($attribute -eq "out") {
                $out = $true
            } elseif ($attribute.StartsWith("iid_is")) {
                $isComPtr = $true
            } elseif ($attribute -eq "unique") {
                $optional = $true
            } elseif ($attribute.StartsWith("size_is")) {
                $size_is = SALParseSize $attribute "size_is"
            } elseif ($attribute.StartsWith("length_is")) {
                $length_is = SALParseSize $attribute "length_is"
            }
        }

        # Skip matches that aren't parameters.
        if (!$in -and !$out) {
            return $null
        }
        $annotation = $null

        $pointerOperation = $null
        if ($in -and $out) {
            $annotation = "_Inout_"
            $pointerOperation = "updates_"
        } elseif ($in) {
            $annotation = "_In_"
            $pointerOperation = "reads_"
        } elseif ($out) {
            $annotation = "_Out_"
            $pointerOperation = "writes_"
        }

        if ($size_is) {
            $annotation += $pointerOperation
            $parameters = "($size_is"
            if ($length_is -and $out) {
                $annotation += "to_"
                $parameters += ",$length_is"
            }
            $parameters += ")"
        }

        if ($optional) {
            $annotation += "opt_"
        }

        if ($parameters) {
            $annotation += $parameters
        }

        # Retval stays in MIDL's native comment; derive all other facts using stock SAL.
        if ($out -and $isComPtr) {
            $annotation = if ($optional) { "_COM_Outptr_opt_" } else { "_COM_Outptr_" }
        }

        return $annotation
}

$lines = @(Get-Content $inputFileName)
$text = $lines -join "`n"
# Keep source offsets while excluding literals and comments, including block comments spanning lines.
$nonCodePattern = '(?s:/\*.*?\*/)|//[^\r\n]*|(?s:"(?:\\.|[^"\\])*"|''(?:\\.|[^''\\])*'')'
$code = [regex]::Replace($text, $nonCodePattern, {
    param($match)
    [regex]::Replace($match.Value, '[^\r\n]', ' ')
})
$code = [regex]::Replace($code, '(?m)^[^\r\n]*\bcpp_quote[ \t]*\([^\r\n]*|^[ \t]*#[ \t]*pragma\b[^\r\n]*', {
    param($match)
    [regex]::Replace($match.Value, '[^\r\n]', ' ')
})
$attributeBlockPattern = '\[(?>[^\[\]]+|\[(?<nested>)|\](?<-nested>))*(?(nested)(?!))\]'
$attributeGroupPattern = [regex]::new("$attributeBlockPattern(?:\s*$attributeBlockPattern)*")
$stream = [System.IO.StreamWriter] $outputFileName

$groups = $attributeGroupPattern.Matches($code)
for ($groupIndex = $groups.Count - 1; $groupIndex -ge 0; $groupIndex--) {
    $group = $groups[$groupIndex]
    # An authored annotation owns its adjacent attribute lists, not other parameters.
    if ($group.Value -match '\bannotation\s*\(') {
        continue
    }

    $groupCode = $group.Value
    $groupText = $text.Substring($group.Index, $group.Length)
    if ($attributeListPattern.Matches($groupCode).Count -gt 1) {
        $blocks = [regex]::Matches($groupCode, $attributeBlockPattern)
        for ($boundaryIndex = $blocks.Count - 2; $boundaryIndex -ge 0; $boundaryIndex--) {
            $start = $blocks[$boundaryIndex].Index + $blocks[$boundaryIndex].Length - 1
            $length = $blocks[$boundaryIndex + 1].Index - $start + 1
            $gap = $groupText.Substring($start + 1, $length - 2)
            if ($gap -match '^\s*$') {
                $replacement = ', '
                $codeReplacement = ', '
            } else {
                $replacement = ',' + $gap
                $codeReplacement = ',' + $groupCode.Substring($start + 1, $length - 2)
            }
            $groupText = $groupText.Remove($start, $length).Insert($start, $replacement)
            $groupCode = $groupCode.Remove($start, $length).Insert($start, $codeReplacement)
        }
    }

    $attributes = $attributeListPattern.Matches($groupCode)
    for ($attributeIndex = $attributes.Count - 1; $attributeIndex -ge 0; $attributeIndex--) {
        $attribute = $attributes[$attributeIndex]
        $annotation = Get-SalAnnotation $attribute.Groups[1].Value
        if ($annotation) {
            $groupText = $groupText.Insert($attribute.Index + $attribute.Length, "[annotation(""$annotation"")]")
        }
    }
    $text = $text.Remove($group.Index, $group.Length).Insert($group.Index, $groupText)
}

if ($lines.Count -gt 0) {
    foreach ($line in $text.Split("`n")) {
        $stream.WriteLine($line)
    }
}

$stream.Close()
