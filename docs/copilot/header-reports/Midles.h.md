# Header Report: Midles.h

## Partitions
`Rpc`

## Ownership audit (producer-site-only policy)
- Declares only enums (`MIDL_ES_CODE`, `MIDL_ES_HANDLE_STYLE`) and function-pointer typedefs (`MIDL_ES_ALLOC`/`MIDL_ES_WRITE`/etc.) for RPC encoding/decoding ("pickling") support. No `extern`/`DllImport` functions.

## Conclusion
`accepted-normalized` — no ownership annotation required (enums/function-pointer typedefs only, no extern functions).
