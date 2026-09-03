# Header Report: lm.h

## Partitions
`NetMgmt`

## Scrape validation
- Re-scraped `NetMgmt` partition (`ScanArch=x86`) after touching `main.cpp`.
- Result: `Build succeeded. 0 Error(s).` (1 pre-existing unrelated cross-partition remap warning for `_CERT_CONTEXT`, not touched by this header).

## Ownership audit (producer-site-only policy)
- `lm.h` is a pure umbrella redirect header (LAN Manager API) pulling in 18 sub-headers
  (`lmcons.h`, `lmerr.h`, `lmaccess.h`, `lmalert.h`, `lmshare.h`, `lmmsg.h`, `lmremutl.h`, `lmrepl.h`,
  `lmserver.h`, `lmsvc.h`, `lmuse.h`, `lmwksta.h`, `lmapibuf.h`, `lmerrlog.h`, `lmconfig.h`, `lmstats.h`,
  `lmaudit.h`, `lmjoin.h`); it declares nothing itself.
- Unlike `wincon.h` (batch `scraping-investigation-22`), where the redirected sub-headers were **not**
  present in the ledger at all (a genuine audit gap), every one of `lm.h`'s 18 sub-headers **is already
  a tracked ledger item** (`lmaccess.h`/`lmserver.h` already `accepted-normalized`; the remaining 16 —
  `lmcons.h`, `lmerr.h`, `lmalert.h`, `lmshare.h`, `lmmsg.h`, `lmremutl.h`, `lmrepl.h`, `lmsvc.h`,
  `lmuse.h`, `lmwksta.h`, `lmapibuf.h`, `lmerrlog.h`, `lmconfig.h`, `lmstats.h`, `lmaudit.h`, `lmjoin.h`
  — still `pending`). Any genuine ownership work in this API surface (e.g. `NetUseAdd`/`NetUseDel`
  handle-like connection semantics in `lmuse.h`) will be caught by those already-queued individual
  audits — it is not silently invisible to future processing.

## Conclusion
`accepted-normalized` — pure umbrella redirect header with zero declarations of its own; all 18 included sub-headers are already represented as individually-tracked ledger items (most still pending), so no audit coverage is lost by closing `lm.h` itself.
