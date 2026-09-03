# mmiscapi.h — accepted-normalized (patch applied)

**Partition:** Multimedia (`Windows.Win32.Media.Multimedia`)
**Size:** ~18502 bytes

## Analysis

`mmiscapi.h` (ApiSet `api-ms-win-mm-misc-l1-1`) declares
`OpenDriver(_In_ LPCWSTR szDriverName, _In_ LPCWSTR szSectionName,
_In_ LPARAM lParam2)` returning `HDRVR`, and
`CloseDriver(_In_ HDRVR hDriver, ...)`.

`HDRVR` (`DECLARE_HANDLE(HDRVR)` in the sibling `mmsyscom.h`, already
`accepted-normalized`) had an existing `autoTypes.json` entry
**missing `CloseApi`** — this was noted as a follow-up item in
`mmsyscom.h`'s report (batch 107) pending investigation of this file.

## Patch

Edited `generation/WinSDK/autoTypes.json` — added
`"CloseApi": "CloseDriver"` to the existing `HDRVR` entry.

## Validation

Re-scraped the `Multimedia` partition (x64, not
`ExcludeFromCrossarch`) after the `main.cpp` touch: build succeeded, 0
warnings, 0 errors.

## Conclusion

Genuine producer-site fix applied — resolved the `mmsyscom.h`
follow-up, completing a pre-existing incomplete `autoTypes.json`
entry.
