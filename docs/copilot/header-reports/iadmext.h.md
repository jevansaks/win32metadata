# Header Report: iadmext.h

## Partitions
`Iis`

## Scrape validation
- Re-scraped `Iis` partition (`ScanArch=x86`) after touching `main.cpp`.
- Result: `Build succeeded. 0 Error(s).` (1 pre-existing unrelated cross-partition remap warning for `_CERT_CONTEXT`, not touched by this header).

## Ownership audit (producer-site-only policy)
- Declares the `IADMEXT` COM interface (`Initialize`/`EnumDcomCLSIDs`/`Terminate`, standard `IUnknown`-derived refcounting) and `IID_IADMEXT`/registry-key-path constants. No raw `HANDLE` involved.

## Conclusion
`accepted-normalized` — no ownership annotation required (COM interface, not HANDLE-family).
