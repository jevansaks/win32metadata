# NTQuery.h — accepted-normalized

**Partitions:** IndexSrv, Shell
**Size:** ~10471 bytes

## Analysis

`NTQuery.h` declares the Indexing Service query API:
`LoadIFilter`, `LoadIFilterEx`, `BindIFilterFromStorage`,
`BindIFilterFromStream`, `SetCatalogState`.

- `LoadIFilter`/`LoadIFilterEx` produce `void **ppIUnk` — a generic
  COM interface pointer (`IUnknown`/`IFilter` via `IID`), out of scope
  per the COM-interface-pointer convention.
- `BindIFilterFromStorage`/`BindIFilterFromStream` likewise produce
  `void **ppIUnk`.
- `SetCatalogState` outputs `DWORD *pdwOldState` — a plain scalar, not
  a handle.

No opaque handle types are produced.

## Conclusion

Clean. No metadata gap. No patch required.
