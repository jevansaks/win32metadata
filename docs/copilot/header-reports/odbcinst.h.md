# odbcinst.h — accepted-normalized

**Partition:** Search
**Size:** ~16929 bytes

## Analysis

`odbcinst.h` (ODBCCP32.DLL prototypes) declares the
`SQLInstallODBC`/`SQLConfigDataSource`/`SQLInstallDriver*`/`SQLWriteDSNToIni`/
`ConfigDSN`/etc. family. All parameters are generic `HWND`
(caller-owned window handle, input only), strings, or plain
`LPDWORD`/`UWORD*` scalars. No opaque handle types are produced.

## Conclusion

Clean. No opaque handle production. No patch required.
