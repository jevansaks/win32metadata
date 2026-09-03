# Header Report: guiddef.h

## Partitions
`TransactionServer`

## Ownership audit (producer-site-only policy)
- Pure GUID type definition (`GUID`/`IID`/`CLSID`/`FMTID`), `DEFINE_GUID`/`REFGUID`/`REFIID`-style macros, and inline GUID-comparison helpers (`IsEqualGUID`, `operator==`/`operator!=`). No handles, no `extern`/`DllImport` functions (only `__inline` helpers).

## Conclusion
`accepted-normalized` — no ownership annotation required (GUID type/macros/inline helpers only, no extern functions).
