# rpcnsi.h — accepted-normalized

**Partition:** Rpc
**Size:** ~15272 bytes

## Analysis

`rpcnsi.h` (RPC Name Service Independent API) declares
`RPC_NS_HANDLE` (`typedef void __RPC_FAR * RPC_NS_HANDLE;`), used as a
generic context handle across **at least 5 independent
producer/consumer pairs**, each with its own distinct "Done"/close
function:

| Producer | Closer |
|---|---|
| `RpcNsBindingLookupBegin` | `RpcNsBindingLookupDone` |
| `RpcNsEntryObjectInqBegin` | `RpcNsEntryObjectInqDone` |
| `RpcNsGroupMbrInqBegin` | `RpcNsGroupMbrInqDone` |
| `RpcNsProfileEltInqBegin` | `RpcNsProfileEltInqDone` |
| `RpcNsBindingImportBegin` | `RpcNsBindingImportDone` |

Since `RPC_NS_HANDLE` is shared across multiple unrelated context
kinds, each requiring a *different* close function, it cannot be
safely annotated with a single `CloseApi` — per the generic/shared-type
blocker class (blocker-class 2), same reasoning as bare `HANDLE`.

## Conclusion

Clean (not fixable — generic/shared context handle type with multiple
independent close functions). No patch required.
