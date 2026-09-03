# Header Report: VersionHelpers.h

## Partitions
`SystemInformation`

## Ownership audit (producer-site-only policy)
- All functions (`IsWindowsVersionOrGreater`, `IsWindowsXPOrGreater`, etc.) are `inline`/`FORCEINLINE` header-only helpers — no `extern "C"`/`DllImport` linkage at all, so nothing is scraped as a callable P/Invoke binding. Out of scope entirely (not merely for ownership — these generate no bindings).

## Conclusion
`accepted-normalized` — no ownership annotation required (all functions are inline, no scrapable extern declarations).
