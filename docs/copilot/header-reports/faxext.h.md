# Header Report: faxext.h

## Partitions
`Fax`

## Scrape validation
- Re-scraped `Fax` partition (x64) after touching `main.cpp`.
- Result: `Build succeeded. 0 Error(s).`

## Ownership audit (producer-site-only policy)
- `FaxExtRegisterForEvents` returns a generic `HANDLE` directly as its C return value (closed via `FaxExtUnregisterForEvents(HANDLE hNotification)`). This is the already-established **return-value handle ownership** blocker class (originated `getprocesshandlefromhwnd.h`): no function anywhere in the published baseline winmd annotates a bare return-value handle.
- `FaxExtGetData`'s `LPBYTE *ppData` out-param is a generic byte buffer freed via `FaxExtFreeBuffer(LPVOID)` — generic buffer convention, out of scope (not a distinctly-named opaque handle).

## Conclusion
`blocked` — genuine gap, reuses the established return-value-handle blocker class (`FaxExtRegisterForEvents`/`FaxExtUnregisterForEvents`).
