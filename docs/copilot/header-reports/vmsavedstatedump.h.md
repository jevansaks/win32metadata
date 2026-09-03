# vmsavedstatedump.h — accepted-normalized (producer-site fix applied)

**Partition:** Hypervisor
**Size:** ~34610 bytes

## Analysis

`vmsavedstatedump.h` (VmSavedState Dump Provider APIs) declares
`LoadSavedStateFile`/`LoadSavedStateFiles` (producers of
`_Out_ VM_SAVED_STATE_DUMP_HANDLE*`) and `ReleaseSavedStateFiles`
(destroyer, single-arg). `VM_SAVED_STATE_DUMP_HANDLE`
(`typedef VOID* VM_SAVED_STATE_DUMP_HANDLE;`) is declared in the
already `accepted-normalized` sibling `vmsavedstatedumpdefs.h`, whose
report deferred ownership here (though it referenced a slightly
incorrect sibling filename — the actual file is this one,
`vmsavedstatedump.h`).

Checked `autoTypes.json` first: **no entry existed**. Confirmed
single-file usage (`vmsavedstatedump.h` + `vmsavedstatedumpdefs.h`
only), `Hypervisor` partition (not `ExcludeFromCrossarch`), namespace
`Windows.Win32.System.Hypervisor` (from partition `settings.rsp`).
Added new `autoTypes.json` entry (`ValueType: DECLARE_HANDLE` —
pointer-sized `VOID*` — `CloseApi: ReleaseSavedStateFiles`,
`InvalidHandleValues: [0]`). Validated via `dotnet build
generation/WinSDK -c Release -p:ScanArch=x64 -t:ScrapeHeaders
-p:PartitionFilter=Hypervisor` → 0 errors.

## Conclusion

Producer-site fix applied — new `VM_SAVED_STATE_DUMP_HANDLE`
`autoTypes.json` entry added at the producer/close-function level.
