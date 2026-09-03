# Header Report: dciman.h

## Partitions
`FileHistory`, `WinProg`

## Ownership audit (producer-site-only policy)
Two distinct findings — one genuine fix applied, one remaining genuine gap:

1. **`HWINWATCH` (fixed)** — `WinWatchOpen(HWND hwnd)` returns `HWINWATCH` (a distinctly-named, single-purpose `DECLARE_HANDLE` opaque type) directly as its C return value, closed via `WinWatchClose(HWINWATCH hWW)`. `autoTypes.json` already had an entry for `HWINWATCH` (`InvalidHandleValues: [-1, 0]`) but it was **missing `CloseApi`** — the same "entry existed but incomplete" gap pattern previously found for `appnotify.h`/`packagevirtualizationcontext.h`. Added `"CloseApi": "WinWatchClose"` to the existing `autoTypes.json` entry. Since this is a type-level (not per-producer-site) annotation mechanism, it correctly covers `WinWatchOpen`'s return-value production (the return-value-handle-ownership restriction applies to *inline* per-declaration C attributes, not to this global type-level `autoTypes.json`/`NativeTypedefStructsCreator` mechanism). Verified namespace consistency: both `HWINWATCH` and `WinWatchClose` are declared only in `dciman.h`, scraped under partition `WinProg`'s namespace `Windows.Win32.System.WindowsProgramming` (matching the `autoTypes.json` entry's `Namespace` field) — satisfies `NativeTypedefStructsCreator`'s same-namespace `CloseApi` constraint. Re-scraped `WinProg`/`FileHistory` partitions (0 errors). Full `EmitWinmd` validation is blocked by the pre-existing, already-documented AllJoyn/`__builtin_verbose_trap` toolchain incompatibility (unrelated to this change) — confirmed no other partition declares `HWINWATCH` or `WinWatchClose`, so no cross-namespace conflict is possible.
2. **`DCIOpenProvider()`/`DCICloseProvider()` (genuine remaining gap)** — `DCIOpenProvider(void)` returns a generic `HDC` directly as its C return value, closed via `DCICloseProvider(HDC hdc)`. `HDC` is an extremely generic, shared-everywhere type (used by hundreds of different GDI APIs with different creation/destruction semantics) — the already-established **return-value handle ownership** blocker class applies (annotating `HDC` at the type level would incorrectly apply ownership metadata to every `HDC` value anywhere in the metadata).

## Conclusion
`blocked` — `HWINWATCH`/`WinWatchOpen`/`WinWatchClose` gap fixed via `autoTypes.json`; `DCIOpenProvider`/`DCICloseProvider`'s generic `HDC` return-value remains an unrepresentable gap (reuses established blocker class).
