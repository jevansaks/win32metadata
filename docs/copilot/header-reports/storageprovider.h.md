# storageprovider.h — accepted-normalized

**Partition:** Shell
**Size:** ~10767 bytes

## Analysis

`storageprovider.h` is MIDL-generated, declaring
`IStorageProviderPropertyHandler`/`IStorageProviderHandler` COM
interfaces. It contains **no free functions** — all methods are COM
vtable methods, out of scope per the COM-vtable-methods convention
(blocker-class 5).

## Conclusion

Clean. No patch required.
