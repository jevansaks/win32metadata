# Header Report: FaxRoute.h

## Partitions
`Fax`

## Ownership audit (producer-site-only policy)
- `FaxRouteInitialize(IN HANDLE HeapHandle, ...)` takes a `HANDLE` as a pre-owned caller INPUT only — never produces/returns one.
- `FaxRouteGetRoutingInfo`/`FaxRouteSetRoutingInfo` write into caller-allocated byte buffers (`LPBYTE`) — no ownership transfer, no opaque handle produced.
- All `PFAXROUTE*` types are function-pointer typedefs for driver-supplied callbacks, not `extern`/`DllImport` declarations — out of scope.

## Conclusion
`accepted-normalized` — no ownership annotation required (no function produces/returns an opaque handle; `HANDLE` params are pre-owned caller inputs).
