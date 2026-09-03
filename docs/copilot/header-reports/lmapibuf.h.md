# Header Report: lmapibuf.h

## Partitions
`NetMgmt`

## Ownership audit (producer-site-only policy)
- This is the canonical home of the Net API buffer-allocation convention already cited as precedent in
  `lmconfig.h`/`lmmsg.h` (batches `scraping-investigation-35`/`-41`): `NetApiBufferAllocate`
  (`_Outptr_result_bytebuffer_`) / `NetApiBufferFree` (`_Frees_ptr_opt_`) / `NetApiBufferReallocate` /
  `NetApiBufferSize` operate on a generic `LPVOID` memory buffer — a plain heap-allocation pattern (SAL
  already documents `_Frees_ptr_opt_`), not an opaque `HANDLE`-family resource. Consistent with the
  established distinction (opaque named handle types vs. generic memory buffers) from batch
  `scraping-investigation-23`/`-39`.

## Conclusion
`accepted-normalized` — no ownership annotation required (generic memory-buffer allocation, not HANDLE-family; this is the canonical NetApiBufferFree convention already cited by sibling `lm*.h` headers).
