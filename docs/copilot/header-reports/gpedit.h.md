# gpedit.h — accepted-normalized

**Partition:** Policy
**Size:** ~27871 bytes

## Analysis

`gpedit.h` (Group Policy Editor) declares only COM interfaces
(`IGroupPolicyObject`, `IGPEInformation`). Confirmed via
multi-line-aware grep that there are **no free function
declarations** — all methods are COM vtable methods, out of scope
per blocker-class 5.

## Conclusion

Clean. No patch required.
