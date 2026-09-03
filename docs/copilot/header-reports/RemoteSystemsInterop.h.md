# Header Report: RemoteSystemsInterop.h

## Partitions
`WinRT`

## Ownership audit (producer-site-only policy)
- Declares only the COM/WinRT interface `ICorrelationVectorInformation` (derives `IInspectable`; `get_LastCorrelationVectorForThread`/`get_NextCorrelationVectorForThread` output `HSTRING`, already correctly covered via existing `autoTypes.json` entry; `put_NextCorrelationVectorForThread` is an input). COM vtable method — out of scope; no `extern`/`DllImport` functions.

## Conclusion
`accepted-normalized` — no ownership annotation required (COM/WinRT interface method only, out of scope).
