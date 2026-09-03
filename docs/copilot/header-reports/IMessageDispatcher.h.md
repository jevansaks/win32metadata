# Header Report: IMessageDispatcher.h

## Partitions
`TransactionServer`

## Ownership audit (producer-site-only policy)
- Declares only the COM/WinRT interface `IMessageDispatcher` (derives `IInspectable`; single method `PumpMessages()`, no outputs). COM vtable method — out of scope; no `extern`/`DllImport` functions.

## Conclusion
`accepted-normalized` — no ownership annotation required (COM/WinRT interface method only, out of scope).
