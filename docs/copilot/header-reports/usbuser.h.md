# usbuser.h — accepted-normalized

**Partition:** Buses
**Size:** ~18041 bytes

## Analysis

`USBUSER.H` (USB Host Controller user-mode IOCTLs) declares only
`IOCTL_USB_USER_REQUEST`-family constants and transparent structs
consumed via `DeviceIoControl` (declared elsewhere). It contains **no
function declarations** (0 `WINAPI`/`STDAPI` matches).

## Conclusion

Clean. No functions, no opaque handle production. No patch required.
