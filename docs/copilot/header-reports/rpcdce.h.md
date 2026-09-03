# rpcdce.h — accepted-normalized (1 item identified, unrepresentable)

**Partition:** Rpc, TransactionServer
**Size:** ~89430 bytes

## Analysis

`rpcdce.h` (DCE RPC runtime APIs) declares numerous
`RPC_BINDING_HANDLE`-producing functions (`RpcBindingCopy`,
`RpcBindingFromStringBindingW/A`, `RpcServerInqBindings`, etc.),
consumed by `RpcBindingFree`. However, `RpcBindingFree`'s signature is
`RpcBindingFree(_Inout_ RPC_BINDING_HANDLE __RPC_FAR * Binding)` —
it takes the handle **by pointer** (in-out, nulling the caller's
variable after freeing), not by value like every established
`CloseApi`/`RAIIFree` precedent in this codebase (`CloseHandle(HANDLE)`,
`MsiCloseHandle(MSIHANDLE)`, etc.). This by-reference/null-out
signature does not match the single-value-parameter convention the
metadata system's `CloseApi` mechanism assumes, and no existing
precedent for a by-pointer close function was found. Treated as
unrepresentable pending further tooling confirmation.

## Conclusion

No representable producer-site fix identified for `RPC_BINDING_HANDLE`
(by-pointer/in-out close signature, structurally distinct from every
established `CloseApi` precedent). No patch required; evidence
recorded for future reconsideration.
