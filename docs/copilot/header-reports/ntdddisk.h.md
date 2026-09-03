# ntdddisk.h — accepted-normalized

**Partition:** Fs
**Size:** ~62041 bytes

## Analysis

`ntdddisk.h` (Disk device access) declares only `IOCTL_DISK_*`
constants and structs used via the generic `DeviceIoControl`
mechanism. It contains **no direct function declarations**.

## Conclusion

Clean. No patch required.
