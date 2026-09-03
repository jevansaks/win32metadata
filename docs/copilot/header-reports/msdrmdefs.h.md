# Header Report: msdrmdefs.h

## Partitions
`Rm`

## Ownership audit (producer-site-only policy)
- Declares only DRM handle-family type definitions (`DRMHANDLE`/`DRMQUERYHANDLE`/`DRMENVHANDLE`/`DRMHSESSION`/`DRMPUBHANDLE`, all `typedef ULONG`), invalid-value constants, and the transparent `_DRMID` struct. No functions are declared in this "defs" header — the actual producer/consumer DRM functions live in the separate, still-`pending` `msdrm.h`. Ownership audit for these handle types is deferred there, per the same deferral pattern established for `HSTRING`/`winstring.h` and `DIAGNOSTIC_DATA_QUERY_SESSION`/`DiagnosticDataQueryTypes.h`.

## Conclusion
`accepted-normalized` — no ownership annotation required in this header (types/constants only, no functions); DRM handle ownership will be audited when `msdrm.h` is classified.
