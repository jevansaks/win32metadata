# Header Report: rpcnsip.h

## Partitions
`Rpc`

## Scrape validation
- Re-scraped `Rpc` partition (`ScanArch=x86`) after touching `main.cpp`.
- Result: `Build succeeded. 0 Error(s).` (1 pre-existing unrelated cross-partition remap warning for `_CERT_CONTEXT`, not touched by this header).

## Ownership audit (producer-site-only policy)
- Declares the `RPC_IMPORT_CONTEXT_P` data struct and several `I_`-prefixed internal RPC runtime
  "stub auto-binding" routines (per the header's own doc comment: *"types and function definitions to
  ... implement the autohandle features of the runtime"*). These `I_`-prefixed functions are Microsoft's
  documented-internal RPC runtime API, not public surface.
- `I_RpcNsSendReceive(IN PRPC_MESSAGE Message, OUT RPC_BINDING_HANDLE __RPC_FAR * Handle)` does output an
  `RPC_BINDING_HANDLE` — a real, widely-used RPC type — but this is an internal stub-binding-search
  routine, not the canonical public producer site for this type. `RPC_BINDING_HANDLE` has no existing
  `autoTypes.json` entry, and its canonical public producer/consumer API (`RpcBindingFromStringBinding`/
  `RpcBindingFree`, etc.) lives in `rpcdce.h`, which is already separately tracked in the ledger (still
  `pending`) — that is where any future ownership-annotation work for `RPC_BINDING_HANDLE` belongs, not
  in this internal stub-routine header.

## Conclusion
`accepted-normalized` — internal ("I_"-prefixed) RPC runtime stub routines; the `RPC_BINDING_HANDLE`
type's canonical producer/consumer API is tracked separately under `rpcdce.h` (still pending).
