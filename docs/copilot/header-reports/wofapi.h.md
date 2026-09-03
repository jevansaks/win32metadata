# Header Report: wofapi.h

## Partitions
`Fs`

## Ownership audit (producer-site-only policy)
- All `Wof*`/`WofWim*`/`WofFile*` functions take `HANDLE FileHandle`/`FileOrVolumeHandle` as pre-owned caller INPUT only — never produce/return one.
- Outputs are plain `DWORD`/`ULONG`/`LARGE_INTEGER`/`BOOL` values or transparent structs (`WIM_ENTRY_INFO`, `WOF_FILE_COMPRESSION_INFO`) — no opaque handles.

## Conclusion
`accepted-normalized` — no ownership annotation required (no function produces an opaque handle; `HANDLE` params are pre-owned caller inputs).
