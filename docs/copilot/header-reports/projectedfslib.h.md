# projectedfslib.h — accepted-normalized (patch applied)

**Partition:** ProjFs (`Windows.Win32.Storage.ProjectedFileSystem`)
**Size:** ~15565 bytes

## Analysis

`projectedfslib.h` (Projected File System user-mode API) declares
`DECLARE_HANDLE(PRJ_NAMESPACE_VIRTUALIZATION_CONTEXT);`, produced by
`PrjStartVirtualizing(..., _Outptr_
PRJ_NAMESPACE_VIRTUALIZATION_CONTEXT* namespaceVirtualizationContext)`
and freed via `PrjStopVirtualizing(_In_
PRJ_NAMESPACE_VIRTUALIZATION_CONTEXT namespaceVirtualizationContext)`.

An `autoTypes.json` entry for `PRJ_NAMESPACE_VIRTUALIZATION_CONTEXT`
already existed but was **missing `CloseApi`**. Confirmed used only
in this one file (repo-wide grep).

## Patch

Edited `generation/WinSDK/autoTypes.json` — added
`"CloseApi": "PrjStopVirtualizing"` to the existing entry.

## Validation

Re-scraped the `ProjFs` partition (x64, not `ExcludeFromCrossarch`)
after the `main.cpp` touch: build succeeded, 0 warnings, 0 errors.

## Conclusion

Genuine producer-site fix applied — completed a pre-existing
incomplete `autoTypes.json` entry.
