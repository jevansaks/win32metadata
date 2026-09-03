# tnef.h — accepted-normalized

**Partition:** Tapi3
**Size:** ~11949 bytes

## Analysis

`tnef.h` (MAPI Transport Neutral Encapsulation Format) declares only
function-pointer typedefs (`LPOPENTNEFSTREAM`, `LPOPENTNEFSTREAMEX`,
`LPGETTNEFSTREAMCODEPAGE`) — dynamically-dispatched callback pointer
types, not `extern`/`DllImport` declarations, out of scope per the
function-pointer-typedef convention (blocker-class 4). It contains
**no free function declarations**.

## Conclusion

Clean. No scrapable functions, no opaque handle production. No patch
required.
