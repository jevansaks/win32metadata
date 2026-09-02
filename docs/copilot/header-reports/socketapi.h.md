# Header Report: socketapi.h

## Partitions
`WinSock`

## Scrape validation
- Re-scraped `WinSock` partition (`ScanArch=x86`, `ExcludeFromCrossarch`) after touching `main.cpp`. Result: `Build succeeded. 0 Error(s)` (same pre-existing cross-partition remap warnings as `transportsettingcommon.h`, unrelated to this header's content).

## Ownership audit (producer-site-only policy)
- Single function: `HRESULT WINAPI SetSocketMediaStreamingMode(_In_ BOOL value)`. No `HANDLE` parameters or return value.

## Conclusion
`accepted-normalized` — no ownership annotation required (no handle involved).
