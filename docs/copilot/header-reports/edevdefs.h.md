# edevdefs.h — accepted-normalized

**Partition:** DeviceAccess
**Size:** ~23046 bytes

## Analysis

`EDevDefs.h` (External Device control interface parameter/value
definitions) declares only `DEV_PORT_*`/`ED_*` constants consumed via
the `IAMExtDevice` COM interface (declared elsewhere). It contains
**no function declarations** (0 `WINAPI`/`STDAPI` matches).

## Conclusion

Clean. No patch required.
