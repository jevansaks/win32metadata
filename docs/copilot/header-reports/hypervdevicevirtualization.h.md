# Header Report: hypervdevicevirtualization.h

## Partitions
`Hypervisor`

## Ownership audit (producer-site-only policy) — genuine fix applied
- `HDV_HOST` (`typedef void*`, distinctly named) is produced via `HdvInitializeDeviceHost`/`HdvInitializeDeviceHostEx` (`_Out_ HDV_HOST* deviceHostHandle`), closed via the single canonical `HdvTeardownDeviceHost(_In_ HDV_HOST deviceHostHandle)`. `autoTypes.json` had **no entry at all** for this type. Added a new entry: `Namespace: Windows.Win32.System.Hypervisor` (matches the `Hypervisor` partition's namespace), `ValueType: IntPtr`, `CloseApi: HdvTeardownDeviceHost`, `InvalidHandleValues: [0]`. Confirmed via grep that `HDV_HOST` is declared only in this one header (no cross-namespace conflict). Re-scraped `Hypervisor` partition (0 errors).
- `HDV_DEVICE` (also `typedef void*`) is produced via `HdvCreateDeviceInstance` (`_Out_ HDV_DEVICE* deviceHandle`), but no corresponding `HdvDestroyDeviceInstance`/close function exists anywhere in this header — device instances appear to be implicitly scoped to (and torn down with) their owning `HDV_HOST`. Since no representable close API exists for this type, it is not annotated (no gap to fix — nothing to free independently).
- `HdvCreateGuestMemoryAperture`'s `_Out_ PVOID* mappedAddress` is a generic mapped-memory address (like `MapViewOfFile`), not a `HANDLE`-family type — out of scope. `HdvRegisterDoorbell`'s `HANDLE DoorbellEvent` is a pre-owned caller input.

## Conclusion
`accepted-normalized` — genuine gap fixed via new `autoTypes.json` entry for `HDV_HOST` (`CloseApi: HdvTeardownDeviceHost`); `HDV_DEVICE` has no representable close API and is left unannotated.
