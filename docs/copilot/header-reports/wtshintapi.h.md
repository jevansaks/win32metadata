# Header Report: wtshintapi.h

## Partitions
`TermServ`

## Scrape validation
- Re-scraped `TermServ` partition (`ScanArch=x86`) after touching `main.cpp`.
- Result: `Build succeeded. 0 Error(s).` (3 pre-existing unrelated cross-partition remap warnings for `in_addr`/`sockaddr`/`timeval`, not touched by this header).

## Ownership audit (producer-site-only policy)
- `WTSSetRenderHint(_Inout_ UINT64* pRenderHintID, _In_ HWND hwndOwner, ...)`. `HWND` is a caller-supplied input (existing window). `pRenderHintID` is a plain `UINT64` application-managed identifier, not a `HANDLE`. No `HANDLE` production.

## Conclusion
`accepted-normalized` — no ownership annotation required (HWND is an input; render hint ID is a plain UINT64, not a HANDLE).
