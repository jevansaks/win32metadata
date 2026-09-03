# rpcndr.h — accepted-normalized

**Partition:** FileHistory, Rpc, WinProg
**Size:** ~81583 bytes

## Analysis

`rpcndr.h` ("Definitions for stub data structures and prototypes of
helper functions") is RPC/MIDL infrastructure header declaring only
macros, type definitions, and version constants used by
MIDL-generated stub code. It contains **no directly P/Invokable
function declarations with ownership semantics**.

## Conclusion

Clean. No patch required.
