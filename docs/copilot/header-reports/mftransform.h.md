# mftransform.h — accepted-normalized

**Partition:** Mf
**Size:** ~74238 bytes

## Analysis

`mftransform.h` declares `MFCreateTransformActivate` (`_Out_
IMFActivate** ppActivate`) — outputs a COM interface pointer, out of
scope per blocker-class 5. Remainder of header is GUID constants and
COM interfaces.

## Conclusion

Clean. No patch required.
