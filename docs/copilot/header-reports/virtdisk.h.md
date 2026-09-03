# virtdisk.h — accepted-normalized

**Partition:** Vhd
**Size:** ~49872 bytes

## Analysis

`virtdisk.h` (Virtual Disk API) declares `OpenVirtualDisk`/
`CreateVirtualDisk` (both output `_Out_ PHANDLE Handle`) — the
generic `HANDLE` type directly (not a distinct typedef), closed via
the generic `CloseHandle`. Matches the established Generic/shared-
type blocker (blocker-class 2), out of scope for typedef-level
producer-site annotation.

## Conclusion

Clean. No patch required — generic `HANDLE` output, blocker-class 2.
