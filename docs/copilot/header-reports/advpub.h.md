# advpub.h — accepted-normalized

**Partition:** FileHistory, WinProg
**Size:** ~41206 bytes

## Analysis

`advpub.h` (Advanced INF/Setup functions: `TranslateInfString`,
`LaunchINFSectionEx`, `RegRestoreAll`, `FileSaveRestore`,
`FileSaveMarkNotExist`) declares `HRESULT`-returning functions.
Confirmed via grep for `_Out_.*HANDLE`/`_Out_.*PHANDLE` that **none
produce a handle** — no ownership pattern present.

## Conclusion

Clean. No patch required.
