# Header Report: dispatcherqueue.h

## Partitions
`WinRT`

## Ownership audit (producer-site-only policy)
- `CreateDispatcherQueueController(_In_ DispatcherQueueOptions, _Deref_out_ PDISPATCHERQUEUECONTROLLER*)` is a COM factory pattern (outputs an `IDispatcherQueueController*`, standard COM refcounting). No raw `HANDLE` involved.

## Conclusion
`accepted-normalized` — no ownership annotation required (COM factory pattern).
