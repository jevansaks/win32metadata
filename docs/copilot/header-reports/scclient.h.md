# Header Report: scclient.h

## Partitions
`Wmdm`

## Scrape validation
- Re-scraped `Wmdm` partition (`ScanArch=x86`) after touching `main.cpp`.
- Result: `Build succeeded. 0 Warning(s). 0 Error(s).` — 0 `DllImport` functions contributed by this header.

## Ownership audit (producer-site-only policy)
- `scclient.h` declares a pure C++ class, `CSecureChannelClient` (not `extern "C"`, no `DllImport`-style
  free functions) — the producer-site ownership-annotation mechanism (confirmed throughout this session
  to apply only to `extern`/`DllImport` function declarations, e.g. `GameInput.h`'s COM vtable methods
  in batch `scraping-investigation-13`) does not apply to C++ class member methods at all.
- Its `MACInit(HMAC *phMAC)`/`MACUpdate(HMAC hMAC, ...)`/`MACFinal(HMAC hMAC, ...)` methods appear to
  manage a MAC-computation context, but there is no `DllImport` surface here to annotate — this is
  private implementation detail of a C++ class, out of scope for header-level annotation.

## Conclusion
`accepted-normalized` — no ownership annotation required (pure C++ class with no `extern "C"`/`DllImport` functions; out of scope for the annotation mechanism).
