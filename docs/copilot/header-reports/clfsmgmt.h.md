# clfsmgmt.h — accepted-normalized (patch applied)

**Partition:** Fs (`Windows.Win32.Storage.FileSystem`)
**Size:** ~13542 bytes

## Analysis

`clfsmgmt.h` (CLFS log management API) declares
`CLFS_MGMT_CLIENT` (`typedef PVOID CLFS_MGMT_CLIENT, *PCLFS_MGMT_CLIENT;`)
— a distinctly-named, opaque `PVOID` handle produced by
`ClfsMgmtRegisterManagedClient(..., _Out_ PCLFS_MGMT_CLIENT
ClientCookie)` and freed via
`ClfsMgmtDeregisterManagedClient(_In_ CLFS_MGMT_CLIENT ClientCookie)`.
Confirmed used only in this one file (repo-wide grep). No prior
`autoTypes.json` entry existed.

## Patch

Edited `generation/WinSDK/autoTypes.json` — added a new entry for
`CLFS_MGMT_CLIENT` (`ValueType: IntPtr`, matching the established
`typedef void*`/`typedef PVOID` convention seen for `NDFHANDLE`,
`CloseApi: ClfsMgmtDeregisterManagedClient`) under namespace
`Windows.Win32.Storage.FileSystem`.

## Validation

Re-scraped the `Fs` partition (x64, not `ExcludeFromCrossarch`) after
the `main.cpp` touch: build succeeded, 0 warnings, 0 errors.

## Conclusion

Genuine producer-site fix applied.
