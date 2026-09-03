# Header Report: appnotify.h

## Partitions
`Shell`

## Scrape validation
- Re-scraped `Shell` partition (`ScanArch=x86`) after touching `main.cpp`.
- Result: `Build succeeded. 0 Error(s).` (2 pre-existing unrelated cross-partition remap warnings for `in6_addr`/`in_addr`, not touched by this header).

## Ownership audit (producer-site-only policy) — GENUINE GAP FOUND AND FIXED

Found a real, fixable ownership gap:
- `RegisterAppStateChangeNotification(..., _Out_ PAPPSTATE_REGISTRATION* Registration)` produces an
  opaque `PAPPSTATE_REGISTRATION` handle, released via `UnregisterAppStateChangeNotification(_Inout_ PAPPSTATE_REGISTRATION Registration)`.
- `RegisterAppConstrainedChangeNotification(..., _Out_ PAPPCONSTRAIN_REGISTRATION* Registration)`
  likewise produces an opaque `PAPPCONSTRAIN_REGISTRATION` handle, released via
  `UnregisterAppConstrainedChangeNotification`.
- Both `PAPPSTATE_REGISTRATION` (`typedef struct _APPSTATE_REGISTRATION *PAPPSTATE_REGISTRATION;`) and
  `PAPPCONSTRAIN_REGISTRATION` are genuine **opaque** pointer-to-incomplete-struct handle types (the
  `_APPSTATE_REGISTRATION`/`_APPCONSTRAIN_REGISTRATION` structs are never defined, only forward-declared
  as pointer targets) — unlike `resourceindexer.h`'s generic untyped `PVOID` (batch
  `scraping-investigation-23`), these are distinctly-named handle types, so the producer-site annotation
  mechanism correctly applies here.
- Checked `autoTypes.json`: both types already have `NativeTypedef: true` entries (for C# struct-wrapper
  generation) but **no** `CloseApi`/`InvalidHandleValues` — confirming no existing ownership metadata
  anywhere (neither the legacy typedef-based path nor an inline annotation) that this new annotation
  would duplicate or conflict with.
- **Fix applied**: added `#if defined(WIN32METADATA) #include <win32metadata_annotations.h> #endif` and,
  on each `_Out_` out-parameter, `_Win32_metadata_invalid_handle_(0)` +
  `_Win32_metadata_raii_free_(UnregisterAppStateChangeNotification)` /
  `_Win32_metadata_raii_free_(UnregisterAppConstrainedChangeNotification)` respectively — directly on
  the producer function's out-parameter, never on the typedef, per the corrected policy.
- Verified via live re-scrape of the `Shell` partition: `Build succeeded. 0 Error(s)` — the new
  annotation syntax parses cleanly.
- Patch artifact: `generation/WinSDK/patches/post-midl/appnotify.h.appstate-registration-ownership.patch`;
  verified with `git apply --check --reverse` (exit code 0).

## Conclusion
`accepted-normalized` — genuine ownership gap identified and fixed at the producer site (out-parameters
of `RegisterAppStateChangeNotification`/`RegisterAppConstrainedChangeNotification`), consistent with the
corrected policy from commits `165b5f09`/`7335ddc4`.
