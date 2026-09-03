# sdkddkver.h — accepted-normalized

**Partitions:** Antimalware, ApplicationVerifier, AppxPackaging, Audio, and many others (widely-included version-check header)
**Size:** ~11565 bytes

## Analysis

`sdkddkver.h` (shared\sdkddkver.h) declares only `NTDDI_*`/`_WIN32_WINNT_*`
version-comparison macros. It contains **no function declarations, no
structs, no handle typedefs** — confirmed via grep (0 `typedef struct`,
0 `typedef enum`, 0 `WINAPI` matches).

## Conclusion

Clean. No functions, no opaque handle production. No patch required.
