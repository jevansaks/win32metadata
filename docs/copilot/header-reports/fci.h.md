# fci.h — accepted-normalized (patch applied)

**Partition:** Cabinets (`Windows.Win32.Storage.Cabinets`)
**Size:** ~19767 bytes

## Analysis

`fci.h` (File Compression Interface) declares
`typedef void * HFCI;` — an opaque handle to an FCI context. Produced
by `FCICreate(...)` (return value), freed via
`FCIDestroy(_In_ HFCI hfci)`. Confirmed used only in this one file
(repo-wide grep). No prior `autoTypes.json` entry existed.

## Patch

Edited `generation/WinSDK/autoTypes.json` — added a new entry for
`HFCI` (`ValueType: IntPtr`, `CloseApi: FCIDestroy`) under namespace
`Windows.Win32.Storage.Cabinets`.

## Validation

Re-scraped the `Cabinets` partition (x64, not `ExcludeFromCrossarch`)
after the `main.cpp` touch: build succeeded, 0 warnings, 0 errors.

## Conclusion

Genuine producer-site fix applied.
