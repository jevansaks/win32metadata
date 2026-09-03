# tsattrs.h — accepted-normalized

**Partition:** Tsf
**Size:** ~10955 bytes

## Analysis

`tsattrs.h` declares only `DEFINE_GUID`-based `ITextStore` standard
attribute GUID constants (`TSATTRID_*`). It contains **no function
declarations, no structs, no handle typedefs**.

## Conclusion

Clean. No functions, no opaque handle production. No patch required.
