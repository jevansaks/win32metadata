# cor.h — accepted-normalized

**Partition:** ClrProfiling
**Size:** ~123066 bytes

## Analysis

`cor.h` (CLR core loader) declares `_CorValidateImage`/
`_CorImageUnloading`/`CoInitializeEE`/etc. — internal CLR loader
functions operating on in-out image-base pointers
(`PVOID *ImageBase`), not a producer/consumer handle pair. No
representable ownership pattern.

## Conclusion

Clean. No patch required.
