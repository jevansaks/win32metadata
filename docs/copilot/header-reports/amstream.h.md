# amstream.h — accepted-normalized

**Partition:** Media.DShow, Media.DShow.Xml
**Size:** ~61183 bytes

## Analysis

MIDL-generated COM header (`IAMMultiMediaStream`/DirectShow streaming
interfaces). Confirmed via multi-line-aware grep that there are **no
free function declarations** — all methods are COM vtable methods,
out of scope per blocker-class 5.

## Conclusion

Clean. No patch required.
