# Header Report: lmmsg.h

## Partitions
`NetMgmt`

## Ownership audit (producer-site-only policy)
- `NetMessageNameAdd`/`NetMessageNameEnum`/`NetMessageNameGetInfo`/`NetMessageNameDel`/`NetMessageBufferSend` operate on `LPCWSTR` strings and `LPBYTE *bufptr` buffers — the Net API buffer-allocation convention (freed via `NetApiBufferFree`, same as `lmconfig.h` precedent in batch `scraping-investigation-35`), not `HANDLE`-family ownership. `MSG_INFO_0`/`MSG_INFO_1` are plain data structs. No `HANDLE` involved.

## Conclusion
`accepted-normalized` — no ownership annotation required (Net API buffer-allocation convention, not HANDLE-family; part of the `lm.h` umbrella already noted as individually tracked).
