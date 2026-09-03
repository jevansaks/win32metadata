# Header Report: msdaosp.h

## Partitions
`Search`

## Ownership audit (producer-site-only policy)
- Declares only the `DataSourceObject : public IDispatch` COM/automation interface. COM/IDispatch vtable method — out of scope; no `extern`/`DllImport` functions.

## Conclusion
`accepted-normalized` — no ownership annotation required (COM/IDispatch interface method only, out of scope).
