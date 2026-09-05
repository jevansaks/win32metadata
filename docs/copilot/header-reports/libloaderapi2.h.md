# libloaderapi2.h

**Classification:** accepted-normalized (no code change needed - already covered)

## Summary
`LoadPackagedLibrary` returns `HMODULE` directly as the function return value.

## Correction to prior investigation
Prior report concluded this was unrepresentable ("same root cause as
getprocesshandlefromhwnd.h"). This is incorrect: `HMODULE` already carries
`CloseApi=FreeLibrary` at the TYPE level via `autoTypes.json`
(`Windows.Win32.Foundation.HMODULE`), which the `NativeTypedefStructsCreator`
emits as a `[RAIIFree("FreeLibrary")]` attribute on the `HMODULE` struct
declaration itself - applied automatically to every function that returns or
takes `HMODULE`, including `LoadPackagedLibrary`. No additional annotation is
needed or possible (adding a redundant per-function entry would be
duplicative). The distinction from the generic-`HANDLE` cases fixed elsewhere
in this batch: `HMODULE` is a *distinctly-named* handle typedef with a single,
unambiguous close API, so it qualifies for type-level `autoTypes.json`
coverage, unlike bare `HANDLE`/`PVOID` which is shared across thousands of
unrelated APIs.

## Ownership Analysis
No `emitter.settings.rsp`/`autoTypes.json` change required - already covered.

## Validation
Confirmed by inspection of `generation/WinSDK/autoTypes.json` (HMODULE entry,
`CloseApi: "FreeLibrary"`) and `NativeTypedefStructsCreator.cs` (emits
`[RAIIFree(...)]` for any autoType with a non-empty `CloseApi`).
