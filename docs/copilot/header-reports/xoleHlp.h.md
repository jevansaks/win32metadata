# Header Report: xoleHlp.h

## Partitions
`DTC`

## Ownership audit (producer-site-only policy)
- `DtcGetTransactionManager`/`DtcGetTransactionManagerC`/`DtcGetTransactionManagerExA`/`DtcGetTransactionManagerExW` all output `void** o_ppvObject` alongside a `REFIID i_riid` parameter — the standard COM `QueryInterface`-style pattern, released via `IUnknown::Release`, out of scope for the `HANDLE`-family annotation mechanism.
- Remaining declarations are `const DWORD` flag constants, enums, transparent structs, and `EXTERN_GUID`/CLSID constants.

## Conclusion
`accepted-normalized` — no ownership annotation required (all outputs are standard COM QueryInterface-style interface pointers).
