# Header Report: UserConsentVerifierInterop.h

## Partitions
`WinRT`

## Ownership audit (producer-site-only policy)
- Declares only the COM/WinRT interop interface `IUserConsentVerifierInterop` (derives `IInspectable`; single method `RequestVerificationForWindowAsync`, a standard `QueryInterface`-style COM output `void** asyncOperation`; `appWindow`/`message` are `_In_` inputs). COM vtable method — out of scope; no `extern`/`DllImport` functions.

## Conclusion
`accepted-normalized` — no ownership annotation required (COM/WinRT interop interface method only, out of scope).
