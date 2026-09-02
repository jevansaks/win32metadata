# Header Report: wab.h

## Partitions
`Wab`

## Scrape validation
- Re-scraped `Wab` partition (`ScanArch=x86` default) after touching `main.cpp`.
- Result: `Build succeeded. 0 Warning(s). 0 Error(s).` — 77 `DllImport` functions, 1387 auto-remaps, 24 fn-ptr excludes written.

## Ownership audit (producer-site-only policy) — BLOCKED

- `wab.h` is a redirect-only header pulling in `wabdefs.h`, `wabcode.h`, `wabtags.h`, `wabutil.h`, `wabiab.h`, `wabapi.h`, `wabmem.h`, `wabnot.h` (MAPI/WAB — Windows Address Book — API).
- No `DECLARE_HANDLE` found in any of these sub-headers.
- Almost all 77 functions are COM-interface-pointer producers (`_Outptr_ LPADRBOOK*` via `WABOpen`/`WABOpenEx`, `IMalloc*`, `IMAPITable*`, etc.) or pure data/struct-copy helpers (`ScCopyProps`, `PropCopyMore`, etc.) — consistent with the clean COM-interface pattern established throughout this session.
- **One genuine exception:** `FtgRegisterIdleRoutine(PFNIDLE* lpfnIdle, LPVOID lpvIdleParam, ...)` returns an opaque `FTG` handle (`void*`) **directly as the C function return value** (not via out-param). This handle is later passed to `DeregisterIdleRoutine(FTG ftg)` (release), `EnableIdleRoutine(FTG ftg, BOOL fEnable)`, and `ChangeIdleRoutine(FTG ftg, ...)` (mutate) — a genuine, well-defined producer/consumer ownership relationship exists.
- This is the **same class of blocker already recorded for `getprocesshandlefromhwnd.h`** in batch `scraping-investigation-14`: the annotation mechanism (both the legacy `autoTypes.json` type-level path and the newer inline producer-site path) has no precedent anywhere in this repository — or in the currently published baseline `Windows.Win32.winmd` (verified via `WinmdUtils.exe dump`, see `getprocesshandlefromhwnd.h.md` for the full investigation) — for annotating a bare function return-value handle's ownership. `FtgRegisterIdleRoutine`'s `FTG` return value cannot be fixed without the same dedicated policy decision.

## Conclusion
`blocked` — genuine `FTG`-handle-producing function (`FtgRegisterIdleRoutine`, return value, not out-param), same unresolved return-value-handle-ownership class as `getprocesshandlefromhwnd.h`. All other 76 functions in this header are clean (COM-interface/data-only).
