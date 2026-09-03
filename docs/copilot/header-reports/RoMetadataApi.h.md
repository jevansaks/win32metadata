# RoMetadataApi.h — accepted-normalized

**Partition:** WinRT.Metadata
**Size:** ~165295 bytes

## Analysis

MIDL-generated COM header (WinRT Metadata Resolver `IMetaDataDispenserEx`
interfaces). Confirmed via multi-line-aware grep that there are **no
free function declarations** — all methods are COM vtable methods,
out of scope per blocker-class 5.

## Conclusion

Clean. No patch required.
