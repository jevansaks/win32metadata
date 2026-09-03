# icu.h

**Classification:** blocked (ownership-ambiguity — needs dedicated follow-up)

## Summary
Core ICU (International Components for Unicode) C API, vendored
wholesale into the Windows SDK (`icu.dll`). Contains **45+ distinct
opaque forward-declared struct-pointer types** already present in
`autoTypes.json` (namespace `Windows.Win32.Globalization`) with
`ValueType`/`NativeTypedef` set but no `CloseApi` — e.g. `UBiDi`,
`UBreakIterator`, `UCollator`, `UConverter`, `UResourceBundle`,
`UNumberFormatter`, `USpoofChecker`, etc.

A systematic scan found unambiguous single `_close` functions for
~30 of these types (e.g. `ubidi_close`, `ubrk_close`, `ucol_close`,
`ucnv_close`, `ures_close`), which superficially looks like a large,
mechanical producer-site fix opportunity similar to `gl/glu.h`/
`webservices.h`.

**However, this pattern differs critically**: several ICU types are
returned by **both** an owned-instance constructor (`_open`/`_clone`/
`_safeClone`) **and** a borrowed-reference singleton accessor
(`_getInstance`/`_getNFCInstance`/etc.) from the *same* typedef, with
only the owned-instance path requiring `_close`. Confirmed example:
`UNormalizer2` is returned both by `unorm2_openFiltered` (owned, must
close) and by `unorm2_getNFCInstance`/`unorm2_getNFDInstance`/etc.
(borrowed ICU-managed singletons — calling `unorm2_close` on these is
documented ICU misuse). The same owned-vs-singleton split applies to
`UCollator` (`ucol_open` vs `ucol_safeClone` reference-counting nuances)
and likely others across the 30+ candidate types.

Putting a blanket `CloseApi` directly on these typedefs would
incorrectly assert universal ownership across every function that
returns the type — the exact anti-pattern the corrected policy (commits
165b5f09/7335ddc4) removed from `windef.h`'s shared handle typedefs
(`HGLRC`, `HDESK`, etc.). Correctly resolving this requires **per-
function** `emitter.settings.rsp` inline `Function::return=[RAIIFree(...)]`
annotations (only on the owned-instance-returning functions), verified
individually across all 45+ types and their full set of producer
functions — a large, dedicated investigation beyond a single batch.

Additionally, at least one related type, `UDateTimePatternGenerator`
(`udatpg_open`/`udatpg_close`), was found to have **no** existing
`autoTypes.json` entry at all, indicating the ICU type inventory itself
is incomplete and needs a full audit before any annotation pass.

## Ownership Analysis
Not resolved in this batch. No `autoTypes.json`/`emitter.settings.rsp`
changes made, to avoid introducing an incorrect blanket-ownership
regression of the kind the corrected policy specifically targets.

## Action
Blocked with actionable evidence. Recommended follow-up: enumerate every
ICU `_open`/`_clone`/`_safeClone`/`_getInstance` function per opaque
type, classify each as owned vs. borrowed, and apply per-function inline
`RAIIFree` annotations only to genuinely owned-instance producers.
