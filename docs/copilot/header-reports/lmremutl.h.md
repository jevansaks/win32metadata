# Header Report: lmremutl.h

## Partitions
`NetMgmt`

## Ownership audit (producer-site-only policy)
- `NetRemoteTOD`'s `_Outptr_result_bytebuffer_ LPBYTE *BufferPtr` follows the established Net API buffer-allocation convention (`NetApiBufferFree`, same as `lmconfig.h`/`lmmsg.h`/`lmapibuf.h`), not `HANDLE`-family. `NetRemoteComputerSupports`/`RxRemoteApi` take only strings/`DWORD` flags. `TIME_OF_DAY_INFO` is a plain data struct.

## Conclusion
`accepted-normalized` — no ownership annotation required (Net API buffer-allocation convention, not HANDLE-family).
