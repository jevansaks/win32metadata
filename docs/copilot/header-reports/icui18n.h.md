# icui18n.h

**Classification:** blocked (ownership-ambiguity — needs dedicated follow-up)

## Summary
ICU internationalization (i18n) C API extension header (collation,
number/date formatting, regex, spoof-check, etc.), sharing the same
opaque struct-pointer type family as `icu.h` (`UCollator`,
`UNumberFormatter`, `URegularExpression`, `USpoofChecker`, etc.) — most
of the relevant types are actually declared in `icu.h` and merely used
here.

Same blocker as `icu.h`: several of these types have both an
owned-instance constructor and a borrowed-reference singleton accessor
sharing the same typedef (e.g. `UCollator` via `ucol_open` vs.
`ucol_safeClone`/locale-default singleton retrieval paths), so a
blanket per-typedef `CloseApi` would be an unsafe, incorrect
generalization — the same anti-pattern the corrected producer-site
policy (commits 165b5f09/7335ddc4) already removed from `windef.h`'s
shared handle typedefs. See `icu.h`'s report for the full evidence and
the `UNormalizer2`/`UDateTimePatternGenerator` findings that motivated
this decision.

## Ownership Analysis
Not resolved in this batch — deferred to the same dedicated follow-up
investigation as `icu.h` (per-function ownership classification across
the shared ICU opaque-type family).

## Action
Blocked with actionable evidence, same root cause and remediation plan
as `icu.h`.
