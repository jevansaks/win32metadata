# msdasc.h — accepted-normalized

**Partition:** Search
**Size:** ~29674 bytes

## Analysis

MIDL-generated COM header (`IService`, `IDBPromptInitialize`,
`IDataInitialize`, `IDataSourceLocator` interfaces). Confirmed via
grep that there are **no free function declarations** — all methods
are COM vtable methods, out of scope per blocker-class 5.

## Conclusion

Clean. No patch required.
