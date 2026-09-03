# printoem.h — accepted-normalized

**Partition:** Printing
**Size:** ~42432 bytes

## Analysis

`printoem.h` declares legacy OEM print-driver plugin contract
functions (`OEMGetInfo`, `OEMDriverDMS`, `OEMDevMode`,
`OEMEnableDriver`, `OEMDisableDriver` — each marked
`__drv_preferredFunction(..., "Obsolete")`) that an OEM plugin DLL
must export, called by the print spooler/driver, not exported by a
system DLL for applications to P/Invoke. No `_Out_` handle-producing
parameters found — plain buffer/size outputs
(`_Out_writes_bytes_`/`_Out_ PDWORD`) only. No representable
ownership pattern.

## Conclusion

Clean. No patch required — legacy plugin-DLL contract functions, no
ownership gap.
