# mediaobj.h — accepted-normalized

**Partition:** Media.DShow, Media.DxMediaObjects
**Size:** ~40582 bytes

## Analysis

MIDL-generated COM header (DirectX Media Object `IMediaObject`
interfaces). Confirmed via multi-line-aware grep that there are **no
free function declarations** — all methods are COM vtable methods,
out of scope per blocker-class 5.

## Conclusion

Clean. No patch required.
