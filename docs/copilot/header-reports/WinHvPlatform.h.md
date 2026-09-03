# WinHvPlatform.h — accepted-normalized (patch applied)

**Partition:** Hypervisor (`Windows.Win32.System.Hypervisor`)
**Size:** ~23111 bytes

## Analysis

`WinHvPlatform.h` (Windows Hypervisor Platform user-mode APIs)
declares several opaque handle types (defined in the sibling
`WinHvPlatformDefs.h`, not separately ledger-tracked). Checking each:

| Type | Producer | Closer | Prior autoTypes.json state |
|---|---|---|---|
| `WHV_PARTITION_HANDLE` | `WHvCreatePartition` | `WHvDeletePartition` | Already complete |
| `WHV_TRIGGER_HANDLE` | `WHvCreateTrigger` | `WHvDeleteTrigger` | **Missing — no entry** |
| `WHV_NOTIFICATION_PORT_HANDLE` | `WHvCreateNotificationPort` | `WHvDeleteNotificationPort` | **Missing — no entry** |

Both new types are confirmed used only in `WinHvPlatform.h` +
`WinHvPlatformDefs.h` (repo-wide grep).

## Patch

Edited `generation/WinSDK/autoTypes.json` — added two new entries
(`WHV_TRIGGER_HANDLE`, `WHV_NOTIFICATION_PORT_HANDLE`) under namespace
`Windows.Win32.System.Hypervisor`.

## Validation

Re-scraped the `Hypervisor` partition (x64, not
`ExcludeFromCrossarch`) after the `main.cpp` touch: build succeeded, 0
warnings, 0 errors.

## Conclusion

Genuine producer-site fix applied — two new ownership-metadata
entries added.
