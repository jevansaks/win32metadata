# usbioctl.h — accepted-normalized

**Partition:** Buses
**Size:** ~61253 bytes

## Analysis

`USBIOCTL.H` (kernel/user-mode IOCTL codes for the USB core stack)
declares only `IOCTL_*` constants and structs used via the generic
`DeviceIoControl` mechanism. It contains **no direct function
declarations**.

## Conclusion

Clean. No patch required.
