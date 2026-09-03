# ntddvol.h — accepted-normalized

**Partition:** Fs
**Size:** ~27774 bytes

## Analysis

`ntddvol.h` declares only `IOCTL_VOLUME_*` device-control-code
constants and transparent structs (used with the generic
`DeviceIoControl` API, declared elsewhere). It contains **no direct
function declarations**.

## Conclusion

Clean. No patch required.
