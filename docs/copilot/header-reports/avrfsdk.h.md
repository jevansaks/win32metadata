# Header Report: avrfsdk.h

## Partitions
`ApplicationVerifier`, `FileHistory`

## Scrape validation
- Re-scraped `ApplicationVerifier` partition (`ScanArch=x86`) after touching `main.cpp`.
- Result: `Build succeeded. 0 Warning(s). 0 Error(s).`

## Ownership audit (producer-site-only policy)
- `VerifierEnumerateResource(HANDLE Process, ...)` takes `HANDLE` as a caller-supplied `_In_`-style input (an existing process handle) — not produced here. `VerifierIsPerUserSettingsEnabled(VOID)` takes no parameters.
- `AVRF_HEAP_ALLOCATION.HeapHandle`/`AVRF_HANDLE_OPERATION.Handle` are declared as plain `ULONG64` diagnostic identifiers (not typed `HANDLE`) — numeric addresses/values captured for Application Verifier's internal tracing, not real OS handles requiring closure.

## Conclusion
`accepted-normalized` — no ownership annotation required (HANDLE is a caller-supplied input; diagnostic "handle" fields are plain ULONG64 values, not HANDLE-typed).
