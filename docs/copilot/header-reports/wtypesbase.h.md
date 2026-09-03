# wtypesbase.h — accepted-normalized

**Partitions:** TransactionServer, WinSock
**Size:** ~14196 bytes

## Analysis

`wtypesbase.h` is MIDL-generated (base OLE/COM type definitions, e.g.
`CLIPFORMAT`, `HGLOBAL`-family typedefs). It contains **no free
functions** (`STDAPI` count: 0) — all methods (if any) are COM vtable
methods, out of scope per the COM-vtable-methods convention
(blocker-class 5).

## Conclusion

Clean. No patch required.
