# mshtmhst.h — accepted-normalized

**Partition:** MsHtml
**Size:** ~50814 bytes

## Analysis

MIDL-generated COM header (Trident/MSHTML hosting interfaces,
`IDocHostUIHandler`, etc.). Confirmed via multi-line-aware grep that
there are **no free function declarations** — all methods are COM
vtable methods, out of scope per blocker-class 5.

## Conclusion

Clean. No patch required.
