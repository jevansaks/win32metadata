# Header Report: ObjSafe.h

## Partitions
`Debug`

## Ownership audit (producer-site-only policy)
- Declares only the COM interface `IObjectSafety` (`SetInterfaceSafetyOptions`/`GetInterfaceSafetyOptions`, plain `DWORD` in/out). COM vtable method — out of scope; no `extern`/`DllImport` functions.

## Conclusion
`accepted-normalized` — no ownership annotation required (COM interface method only, out of scope).
