# Header Report: avrt.h

## Partitions
`Threading`

## Scrape validation
- Re-scraped `Threading` partition (x64) after touching `main.cpp`.
- Result: `Build succeeded. 0 Error(s).`

## Ownership audit (producer-site-only policy)
Two distinct genuine ownership gaps found, both falling into already-established blocker classes (concise reuse, no new class needed):

1. **Return-value handle ownership** (class established `getprocesshandlefromhwnd.h`): `AvSetMmThreadCharacteristicsA`/`W` and `AvSetMmMaxThreadCharacteristicsA`/`W` return a generic `HANDLE` directly as the C return value (closed via `AvRevertMmThreadCharacteristics`). No function anywhere in the published baseline winmd annotates a bare return-value handle; annotating these would require metadata capability the mechanism doesn't support at producer-return sites.
2. **Generic-type direct-out-param** (class established `wslapi.h`/`ratings.h`): `AvRtCreateThreadOrderingGroup`/`Ex A`/`Ex W`/`AvRtJoinThreadOrderingGroup` all take `_Out_ PHANDLE Context` — a generic `HANDLE` (not a distinctly-named type) produced via a direct out-parameter, closed via `AvRtDeleteThreadOrderingGroup`. Annotating a generic `HANDLE`/`PHANDLE` out-param would incorrectly apply ownership metadata to every `HANDLE` value anywhere in the metadata.

## Conclusion
`blocked` — two genuine gaps, both instances of already-established blocker classes (return-value handle; generic-type direct-out-param). No new patch representable without over-broad annotation of the generic `HANDLE` type.
