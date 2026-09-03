# Header Report: expandedresources.h

## Partitions
`GameMode`

## Scrape validation
- `GameMode` partition previously re-scraped this session (batch `scraping-investigation-20`); result `0 Warning(s), 0 Error(s)`.

## Ownership audit (producer-site-only policy)
- Three functions: `HasExpandedResources(_Out_ BOOL*)`, `GetExpandedResourceExclusiveCpuCount(_Out_ ULONG*)`, `ReleaseExclusiveCpuSets()`. All outputs are plain `BOOL`/`ULONG` values; `ReleaseExclusiveCpuSets` takes no parameters (releases process-wide state implicitly acquired via `GetExpandedResourceExclusiveCpuCount`'s side effect, not a `HANDLE`). No `HANDLE`-family type involved.

## Conclusion
`accepted-normalized` — no ownership annotation required (no handle-typed values anywhere in this header).
