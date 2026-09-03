# Header Report: TpmVscAttestation.h

## Partitions
`Security.Tpm`

## Scrape validation
- Re-scraped `Security.Tpm` partition (`ScanArch=x86`) after touching `main.cpp`.
- Result: `Build succeeded. 0 Error(s).` (2 pre-existing unrelated cross-partition remap warnings for `_CERT_CONTEXT`/`SecPkgContext_IssuerListInfoEx`, not touched by this header).

## Ownership audit (producer-site-only policy)
- MIDL-generated header declaring only the `TPMVSC_ATTESTATION_TYPE` enum and standard MIDL boilerplate (`RPC_IF_HANDLE` ifspec externs). No functions, no handles.

## Conclusion
`accepted-normalized` — no ownership annotation required (enum + MIDL boilerplate only, no functions).
