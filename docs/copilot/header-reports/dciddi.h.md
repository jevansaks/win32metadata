# dciddi.h — accepted-normalized

**Partition:** Direct2D
**Size:** ~11933 bytes

## Analysis

`dciddi.h` (shared\dciddi.h, MS/Intel Display Control Interface)
declares only transparent structs (`DCICMD`, `DCICREATEINPUT`,
`DCISURFACEINFO`, `DCIENUMINPUT`, `DCIOFFSCREEN`, `DCIOVERLAY`) whose
fields include `CALLBACK`-convention function pointers
(`BeginAccess`, `EndAccess`, `DestroySurface`, `Draw`,
`SetClipList`, `SetDestination`, `EnumCallback`) — struct-field
function pointers, out of scope per the function-pointer-typedef
convention (blocker-class 4). It contains **no free/extern function
declarations**.

## Conclusion

Clean. No scrapable functions, no opaque handle production. No patch
required.
