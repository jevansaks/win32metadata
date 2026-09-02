[CmdletBinding()]
param(
    [string]$ProgressPath = "$PSScriptRoot\..\generation\WinSDK\patches\header-progress.json",
    [string]$QueuePath = "$PSScriptRoot\..\generation\WinSDK\patches\ralph-loop-queue.json",
    [string]$QueueDocumentPath = "$PSScriptRoot\..\docs\copilot\ralph-loop-sdk-queue.md"
)

$ErrorActionPreference = "Stop"

$progress = Get-Content $ProgressPath -Raw | ConvertFrom-Json
$queue = foreach ($item in $progress.items) {
    $status = switch ($item.status) {
        { $_ -in "matched", "accepted-normalized" } { "matched"; break }
        { $_ -in "in-progress", "in_progress" } { "in-progress"; break }
        "blocked" { "blocked"; break }
        default { "remaining" }
    }

    $partitions = @($item.partitions) -join ", "
    [ordered]@{
        header = $item.header
        partition = $partitions
        status = $status
        owner = $item.assignee
        last_updated = $item.last_updated
        notes = $item.notes
        _source = "header-progress"
    }
}

$queue | ConvertTo-Json -Depth 5 | Set-Content $QueuePath -Encoding utf8NoBOM

$counts = @{
    matched = @($queue | Where-Object status -eq "matched").Count
    in_progress = @($queue | Where-Object status -eq "in-progress").Count
    blocked = @($queue | Where-Object status -eq "blocked").Count
    remaining = @($queue | Where-Object status -eq "remaining").Count
}
$generated = (Get-Date).ToUniversalTime().ToString("yyyy-MM-ddTHH:mm:ssZ")

$lines = [System.Collections.Generic.List[string]]::new()
$lines.Add("# Ralph Loop SDK Header Queue")
$lines.Add("")
$lines.Add("- Generated: $generated")
$lines.Add("- Source: ``generation/WinSDK/patches/header-progress.json`` (authoritative, one row per unique header)")
$lines.Add("- Total headers: $($queue.Count)")
$lines.Add("- Matched: $($counts.matched)")
$lines.Add("- In progress: $($counts.in_progress)")
$lines.Add("- Blocked: $($counts.blocked)")
$lines.Add("- Remaining: $($counts.remaining)")
$lines.Add("")
$lines.Add("| Header | Partition(s) | Status | Owner | Last Updated | Notes |")
$lines.Add("|---|---|---|---|---|---|")
foreach ($item in $queue) {
    $notes = ([string]$item.notes).Replace("|", "\|")
    $partitions = ([string]$item.partition).Replace("|", "\|")
    $lines.Add("| ``$($item.header)`` | $partitions | $($item.status) | $($item.owner) | $($item.last_updated) | $notes |")
}

[System.IO.File]::WriteAllLines(
    $QueueDocumentPath,
    $lines,
    [System.Text.UTF8Encoding]::new($false)
)

Write-Host "Queue reconciled: $($queue.Count) unique headers; $($counts.matched) matched, $($counts.in_progress) in progress, $($counts.blocked) blocked, $($counts.remaining) remaining."
