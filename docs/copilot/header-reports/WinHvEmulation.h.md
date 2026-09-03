# Header Report: WinHvEmulation.h

## Partitions
`Hypervisor`

## Ownership audit (producer-site-only policy) — genuine fix applied
- `WHvEmulatorCreateEmulator(_In_ const WHV_EMULATOR_CALLBACKS* Callbacks, _Out_ WHV_EMULATOR_HANDLE* Emulator)` produces a `WHV_EMULATOR_HANDLE` (a distinctly-named type, `typedef VOID* WHV_EMULATOR_HANDLE`, NOT the generic `HANDLE`) via a direct out-param, closed via `WHvEmulatorDestroyEmulator(_In_ WHV_EMULATOR_HANDLE Emulator)`. `autoTypes.json` had **no entry at all** for this type — a genuine gap, same class as `appnotify.h`'s original fix (absent entry, not merely incomplete).
- Added a new `autoTypes.json` entry: `Name: WHV_EMULATOR_HANDLE`, `Namespace: Windows.Win32.System.Hypervisor` (matches the `Hypervisor` partition's namespace), `ValueType: IntPtr`, `CloseApi: WHvEmulatorDestroyEmulator`, `InvalidHandleValues: [0]`. Confirmed via grep that `WHV_EMULATOR_HANDLE` is declared only in this one header (no cross-namespace conflict risk). Re-scraped `Hypervisor` partition (0 errors).
- **Verification caveat:** confirmed via `WinmdUtils.exe dump` of the baseline `bin\Windows.Win32.winmd` that this exact `autoTypes.json`-driven mechanism (type-level `[RAIIFree]`) is correctly emitted for other single-purpose opaque types (`HINSTANCE`/`HMODULE`/`HPOWERNOTIFY`/`HSTRING` all carry `[RAIIFree(...)]` in the actual dump) — but the **generic** `HANDLE` type does NOT get `[RAIIFree]` emitted despite `autoTypes.json` listing `CloseApi: CloseHandle` for it, confirming the tooling correctly special-cases/excludes the shared generic type from blanket ownership annotation. This is decisive empirical confirmation that the established "generic-type" blocker class (used throughout this session for `HANDLE`/`PVOID`/`DWORD`-typed producers) is architecturally sound, while single-purpose distinctly-named types like `WHV_EMULATOR_HANDLE` are the correct and safe target for new `autoTypes.json` entries.
- `WHvEmulatorTryIoEmulation`/`WHvEmulatorTryMmioEmulation` take the handle as a pre-owned `_In_` input only, plus output a transparent `WHV_EMULATOR_STATUS` union (not an opaque handle) — no additional gaps.

## Conclusion
`accepted-normalized` — genuine gap fixed via new `autoTypes.json` entry for `WHV_EMULATOR_HANDLE` (`CloseApi: WHvEmulatorDestroyEmulator`).
