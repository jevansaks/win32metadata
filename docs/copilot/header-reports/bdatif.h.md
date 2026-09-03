# bdatif.h — accepted-normalized

**Partition:** MsTv
**Size:** ~61508 bytes

## Analysis

MIDL-generated COM header (BDA tuning interfaces). Confirmed via
multi-line-aware grep that there are **no free function
declarations** — all methods are COM vtable methods, out of scope
per blocker-class 5.

## Conclusion

Clean. No patch required.
