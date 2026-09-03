# Header Report: lmconfig.h

## Partitions
`NetMgmt`

## Ownership audit (producer-site-only policy)
- `NetConfigGet`/`NetConfigGetAll` output `LPBYTE *bufptr` — a buffer allocated by the Net API runtime, conventionally freed via `NetApiBufferFree` (standard `lm*.h`-family allocation convention, not a `HANDLE`-family resource). `NetConfigSet` takes only input parameters. `CONFIG_INFO_0` is a plain data struct. No `HANDLE` involved.

## Conclusion
`accepted-normalized` — no ownership annotation required (Net API buffer-allocation convention, not HANDLE-family; part of the `lm.h` umbrella already noted as individually-tracked in batch `scraping-investigation-27`).
