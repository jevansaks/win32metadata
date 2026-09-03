# dxva2api.h — accepted-normalized

**Partition:** Media.DShow, Mf
**Size:** ~75779 bytes

## Analysis

`dxva2api.h` declares `DXVA2CreateDirect3DDeviceManager9`/
`DXVA2CreateVideoService` — both output COM interface pointers
(`IDirect3DDeviceManager9**`/`void** ppService` typed by `REFIID`),
out of scope per blocker-class 5.

## Conclusion

Clean. No patch required.
