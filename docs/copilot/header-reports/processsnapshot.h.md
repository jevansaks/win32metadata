# processsnapshot.h — accepted-normalized (patch applied)

**Partition:** Proc_Snap (`Windows.Win32.System.Diagnostics.ProcessSnapshotting`)
**Size:** ~13111 bytes

## Analysis

`processsnapshot.h` declares the Process Snapshot API with two
distinct `DECLARE_HANDLE`-based types:

| Type | Producer | Closer | Prior autoTypes.json state |
|---|---|---|---|
| `HPSS` | `PssCaptureSnapshot` (`_Out_ HPSS* SnapshotHandle`) | `PssFreeSnapshot(_In_ HANDLE ProcessHandle, _In_ HPSS SnapshotHandle)` | Entry existed, **missing `CloseApi`** |
| `HPSSWALK` | `PssWalkMarkerCreate` | `PssWalkMarkerFree(_In_ HPSSWALK WalkMarkerHandle)` | Entry already complete (`CloseApi: PssWalkMarkerFree`) |

`HPSS` is used only in this one file (repo-wide grep confirmed).
`PssFreeSnapshot` is a two-parameter close function (it also needs the
originating `ProcessHandle`), but the `autoTypes.json`/`RAIIFree`
mechanism only records the API name — it performs no signature
validation (confirmed in
`ClangSharpSourceToWinmd/NativeTypedefStructsCreator.cs`) — so this is
mechanically safe to add, consistent with the sibling `HPSSWALK` entry
already present in the same file.

## Patch

Edited `generation/WinSDK/autoTypes.json` — added
`"CloseApi": "PssFreeSnapshot"` to the existing `HPSS` entry (was
previously missing).

## Validation

Re-scraped the `Proc_Snap` partition (x64, not `ExcludeFromCrossarch`)
after the `main.cpp` touch: build succeeded, 0 warnings, 0 errors.

## Conclusion

Genuine producer-site fix applied — completed a pre-existing
incomplete `autoTypes.json` entry.
