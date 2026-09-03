# Header Report: utilapiset.h

## Partitions
`Base`, `Debug`

## Scrape validation
- Re-scraped `Debug` partition (`ScanArch=x86`) after touching `main.cpp`.
- Result: `Build succeeded. 0 Error(s).` (1 pre-existing unrelated cross-partition remap warning for `_CERT_CONTEXT`, not touched by this header).

## Ownership audit (producer-site-only policy)
- `EncodePointer`/`DecodePointer`/`EncodeSystemPointer`/`DecodeSystemPointer` obfuscate/de-obfuscate a raw `PVOID` value (no resource ownership, just bit-scrambling) — not a `HANDLE`-producing pattern.
- `EncodeRemotePointer`/`DecodeRemotePointer` take `HANDLE ProcessHandle` as an `_In_` (caller-supplied, existing process handle) — not produced here.
- `Beep(DWORD, DWORD)` has no handle-typed parameters.

## Conclusion
`accepted-normalized` — no ownership annotation required (pointer obfuscation, not resource ownership; HANDLE is an input only).
