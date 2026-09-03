# rpcasync.h — accepted-normalized

**Partition:** Rpc
**Size:** ~19693 bytes

## Analysis

`rpcasync.h` (async RPC runtime API) declares
`RpcAsyncInitializeHandle(_Out_writes_bytes_(Size) PRPC_ASYNC_STATE
pAsync, ...)`. `RPC_ASYNC_STATE` is a **caller-allocated, fully
transparent struct** — `RpcAsyncInitializeHandle` initializes a
struct the caller already owns in-place, it does not allocate/return a
new opaque handle. Out of scope per the transparent-struct convention
(blocker-class 6).

## Conclusion

Clean. No opaque handle production. No patch required.
