# Header Report: physicalmonitorenumerationapi.h

## Partitions
`Monitor`

## Scrape validation
- `Monitor` partition previously re-scraped this session (batch `scraping-investigation-30`); result `0 Warning(s), 0 Error(s)`.

## Ownership audit (producer-site-only policy) — BLOCKED (variant of a known blocker class)

This is the canonical producer/consumer pair already deferred from `lowlevelmonitorconfigurationapi.h`
(batch `scraping-investigation-30`):
- `GetPhysicalMonitorsFromHMONITOR`/`GetPhysicalMonitorsFromIDirect3DDevice9` fill a caller-allocated
  array (`_Out_writes_(dwPhysicalMonitorArraySize) LPPHYSICAL_MONITOR pPhysicalMonitorArray`) of
  `PHYSICAL_MONITOR` structs, each containing a `HANDLE hPhysicalMonitor` field.
- `DestroyPhysicalMonitor(_In_ HANDLE hMonitor)`/`DestroyPhysicalMonitors(...)` release those handles.

This is a variant of the already-documented **generic/shared-type blocker class** first recorded for
`resourceindexer.h` (batch `scraping-investigation-23`): the handle field's type is the plain, generic
system `HANDLE` — not a distinctly-named opaque handle type — so annotating it would incorrectly apply
ownership metadata to *every* `HANDLE` value anywhere in the published metadata (`HANDLE` is used for
thousands of unrelated resource kinds). This case additionally differs structurally from every other
fixable case: the handle is **nested inside a struct field inside a caller-allocated array output**,
not a direct `_Out_ HANDLE*`/out-param on the function itself — there is no single parameter to attach
an annotation to at all, even setting the generic-type problem aside.

## Conclusion
`blocked` — genuine `HANDLE` ownership relationship (`GetPhysicalMonitorsFromHMONITOR`/
`GetPhysicalMonitorsFromIDirect3DDevice9` → `DestroyPhysicalMonitor`/`DestroyPhysicalMonitors`), but
expressed via a generic `HANDLE` field nested inside a struct-array output rather than a directly
annotatable out-parameter of a distinctly-named type — same root blocker class as `resourceindexer.h`
(generic/shared type precludes producer-site annotation), with an added structural wrinkle. Needs the
same dedicated policy decision.
