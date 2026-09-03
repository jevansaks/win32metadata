# certbcli.h — accepted-normalized

**Partition:** Certificates
**Size:** ~23376 bytes

## Analysis

`certbcli.h` (Cert Server backup client APIs) declares only
function-pointer typedefs (`FNCERTSRVISSERVERONLINEW`,
`FNCERTSRVBACKUPGETDYNAMICFILELISTW`, `FNCERTSRVBACKUPPREPAREW`,
`FNCERTSRVBACKUPOPENFILEW`, `FNCERTSRVBACKUPREAD`,
`FNCERTSRVBACKUPCLOSE`, `FNCERTSRVBACKUPFREE`, etc.) — dynamically
loaded via `GetProcAddress`, not `extern`/`DllImport` declarations, out
of scope per the function-pointer-typedef convention (blocker-class
4).

## Conclusion

Clean. No scrapable functions, no opaque handle production. No patch
required.
