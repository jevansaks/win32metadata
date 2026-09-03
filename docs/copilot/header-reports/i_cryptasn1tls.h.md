# Header Report: i_cryptasn1tls.h

## Partitions
`Security.Cryptography`, `Security.Cryptography.UI`

## Ownership audit (producer-site-only policy) — BLOCKED (known blocker class)

- `typedef DWORD HCRYPTASN1MODULE` — a genuine "handle to an installed Asn1 module" (per the header's
  own comment), produced by `I_CryptInstallAsn1Module(...)` **returned directly as the function return
  value** (not via out-param), and released via `I_CryptUninstallAsn1Module(IN HCRYPTASN1MODULE hAsn1Module)`.
- This is the same **return-value-handle-ownership blocker class** already documented in depth for
  `getprocesshandlefromhwnd.h` (batch `scraping-investigation-14`) and reused concisely for `wab.h`,
  `wincon.h`, `winppi.h`, `libloaderapi2.h`, and `MSAJTransport.h`: no precedent anywhere in this repo
  or the published baseline winmd for annotating a bare return-value handle.
- `I_CryptGetAsn1Encoder`/`I_CryptGetAsn1Decoder` return `ASN1encoding_t`/`ASN1decoding_t` (`void*`
  typedefs) — these are per-thread cached pointers owned/managed internally by the Asn1 module
  (lifetime tied to the module, not independently freed by the caller), so no additional ownership
  concern beyond the module handle itself.

## Conclusion
`blocked` — genuine `HCRYPTASN1MODULE` ownership relationship via return value, same already-documented
return-value-handle-ownership class; needs the same dedicated policy decision.
