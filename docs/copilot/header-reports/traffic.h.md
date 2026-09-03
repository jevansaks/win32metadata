# traffic.h — accepted-normalized

**Partition:** Qos
**Size:** ~11294 bytes

## Analysis

`traffic.h` (shared\traffic.h) is the Traffic Control (TC) interface
header. It defines the `APIENTRY` macro override and transparent
QoS/flow-spec structs/constants (via `<qosobjs.h>`). It contains **no
function declarations** — the actual `Tc*` API functions
(`TcOpenInterface`, `TcCloseInterface`, `TcAddFlow`, etc.) are declared
in a separate header (`traffic.h` here provides only shared type
definitions consumed by that header).

## Conclusion

Clean. No functions, no opaque handle production. No patch required.
