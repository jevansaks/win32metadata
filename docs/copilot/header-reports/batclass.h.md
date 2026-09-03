# batclass.h — accepted-normalized

**Partition:** Power
**Size:** ~18998 bytes

## Analysis

`batclass.h` (battery class driver interfaces) declares only
`DEFINE_GUID`-based WMI GUID constants and transparent kernel IOCTL
structs. It contains **no function declarations** (0
`WINAPI`/`STDAPI` matches).

## Conclusion

Clean. No functions, no opaque handle production. No patch required.
