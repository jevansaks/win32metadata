# Header Report: capi.h

## Partitions
`FileHistory`, `WinProg`

## Scrape validation
- `WinProg` partition previously re-scraped this session (batch `scraping-investigation-23`); result `0 Error(s)`.

## Ownership audit (producer-site-only policy)
- `capi.h` redirects to `wincrypt.h` (already `accepted-normalized`), `sipbase.h` (not itself a tracked
  ledger item, but confirmed to be a trivial redirect to `mssip.h`, which is already
  `accepted-normalized` — verified directly since `sipbase.h` is untracked, per the `wincon.h` lesson),
  `mscat.h` (already tracked, `pending`), `mssip.h` (already `accepted-normalized`), and `wintrust.h`
  (already tracked, `pending`).
- `capi.h` itself directly declares one data struct, `JAVA_TRUST` (Java/ActiveX trust-policy data —
  `cbSize`/flags/permission-blob fields). No functions, no handles.

## Conclusion
`accepted-normalized` — no ownership annotation required (`JAVA_TRUST` is a plain data struct; all
redirected headers are either already accepted or already separately tracked as pending; `sipbase.h`,
though untracked, was verified to be a trivial redirect to the already-accepted `mssip.h`).
