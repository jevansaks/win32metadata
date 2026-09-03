# usb.h — accepted-normalized

**Partition:** Buses
**Size:** ~32017 bytes

## Analysis

`usb.h` (structures and APIs for USB drivers) declares only
transparent structs and constants used by kernel-mode drivers via
IOCTLs. It contains **no direct callable function declarations**.

## Conclusion

Clean. No patch required.
