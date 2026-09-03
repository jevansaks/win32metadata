# Header Report: rpc.h

## Partitions
`Rpc`

## Redirect-only-header safety test
Master RPC umbrella header including `rpcdce.h` (pending), `rpcnsi.h` (pending), `rpcnterr.h` (pending), `rpcasync.h` (pending) — every included sub-header is already a tracked ledger item.

## Ownership audit (producer-site-only policy)
- Declares only `I_RPC_HANDLE`/`RPC_STATUS` typedefs and RPC exception-handling macros (`RpcTryExcept`/`RpcExcept`/etc.) directly.
- The one real function declaration, `RpcMacSetYieldInfo`, is guarded by `__RPC_MAC__` (classic Mac OS RPC support), never compiled for Windows partitions — not reachable/scraped on this platform.

## Conclusion
`accepted-normalized` — pure umbrella/typedef header; all sub-headers already individually tracked ledger items; no reachable ownership-relevant declarations on Windows.
