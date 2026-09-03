# Header Report: lmat.h

## Partitions
`NetMgmt`

## Ownership audit (producer-site-only policy)
- `NetScheduleJobAdd`/`NetScheduleJobDel`/`NetScheduleJobEnum`/`NetScheduleJobGetInfo` operate on `LPBYTE`/`LPBYTE*` buffers following the established Net API buffer-allocation convention (`NetApiBufferFree`, same as `lmconfig.h`/`lmmsg.h`/`lmapibuf.h`/`lmremutl.h`), not `HANDLE`-family. `AT_INFO`/`AT_ENUM` are plain data structs.

## Conclusion
`accepted-normalized` — no ownership annotation required (Net API buffer-allocation convention, not HANDLE-family).
