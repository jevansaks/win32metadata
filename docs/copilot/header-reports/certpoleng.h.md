# Header Report: certpoleng.h

## Partitions
`Certificates`

## Scrape validation
- Re-scraped `Certificates` partition (`ScanArch=x86`) after touching `main.cpp`.
- Result: `Build succeeded. 0 Error(s).` (2 pre-existing unrelated cross-partition remap warnings for `_CERT_CONTEXT`/`SecPkgContext_IssuerListInfoEx`, not touched by this header).

## Ownership audit (producer-site-only policy)
- `PstGetTrustAnchors`/`PstGetTrustAnchorsEx` output `_Outptr_ PSecPkgContext_IssuerListInfoEx*` — a `SecPkgContext_*`-family **transparent struct** (standard SSPI convention, freed via the generic `FreeContextBuffer`, not an opaque handle-specific close function) — out of scope for the `HANDLE`-specific mechanism (same class as `dmort.h`'s `DMO_MEDIA_TYPE`).
- `PstGetCertificateChain`/`PstGetCertificates` output `PCCERT_CHAIN_CONTEXT`(`*`) — also a transparent struct (defined in `wincrypt.h`), freed via `CertFreeCertificateChain`; no `autoTypes.json` entry exists for it (checked), consistent with it being a transparent-struct pattern, not an opaque handle.
- `PstMapCertificate`'s `_Outptr_ PVOID* ppTokenInformation` is a generic untyped `PVOID` output, out of scope (same class as `resourceindexer.h`).
- All other parameters (`PCCERT_CONTEXT`, `PUNICODE_STRING`, `HCERTSTORE*`) are caller-supplied inputs, not produced here.

## Conclusion
`accepted-normalized` — no ownership annotation required (all outputs are either transparent structs with generic free conventions, or generic untyped `PVOID`, both out of scope for the `HANDLE`-specific mechanism).
