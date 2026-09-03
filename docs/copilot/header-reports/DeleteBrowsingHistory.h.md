# Header Report: DeleteBrowsingHistory.h

## Partitions
`WinProg`

## Ownership audit (producer-site-only policy)
- Declares only the COM interface `IDeleteBrowsingHistory` (single method `DeleteBrowsingHistory(DWORD dwFlags)`, no outputs) plus `CATID_DeleteBrowsingHistory` GUID and `DELETE_BROWSING_HISTORY_*` flag constants. COM vtable method — out of scope; no `extern`/`DllImport` functions.

## Conclusion
`accepted-normalized` — no ownership annotation required (COM interface + GUID/flag constants only, no extern functions).
