# msacmdrv.h — accepted-normalized

**Partition:** Audio
**Size:** ~10510 bytes

## Analysis

`msacmdrv.h` is the Audio Compression Manager (ACM) *driver-side*
public header — it defines driver message codes (`ACMDM_*`) and
transparent parameter-block structs consumed by a driver's
`DriverProc` entry point (which is implemented by the driver, not
declared/exported here). It contains no `STDAPI`/`WINAPI`-decorated
function declarations at all — every symbol is a `#define` constant or
a transparent struct type.

## Conclusion

Clean. No functions, no opaque handle production. No patch required.
