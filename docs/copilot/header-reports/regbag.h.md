# Header Report: regbag.h

## Partitions
`MsTv`

## Ownership audit (producer-site-only policy)
- Declares only the COM interface `ICreatePropBagOnRegKey` (single method `Create(_In_ HKEY hkey, ..., REFIID iid, _Out_ LPVOID *ppBag)` — a standard `QueryInterface`-style COM output, `hkey` is a pre-owned caller input). COM vtable method — out of scope; no `extern`/`DllImport` functions.

## Conclusion
`accepted-normalized` — no ownership annotation required (COM interface method only, out of scope).
