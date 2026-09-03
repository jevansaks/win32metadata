# d3d9helper.h — accepted-normalized

**Partition:** Media.DShow
**Size:** ~99174 bytes

## Analysis

`d3d9helper.h` declares `Direct3DCreate9` which returns `IDirect3D9*`
— a COM interface pointer, out of scope per blocker-class 5.

## Conclusion

Clean. No patch required.
