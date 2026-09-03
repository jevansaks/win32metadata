# softpub.h — accepted-normalized

**Partition:** Security.WinTrust
**Size:** ~14956 bytes

## Analysis

`softpub.h` (Authenticode Policy Provider) declares only GUIDs
(`WINTRUST_ACTION_GENERIC_VERIFY_V2`, etc.) and transparent structs
consumed by `WinVerifyTrust` (declared in the separate `wintrust.h`
header). It contains **no function declarations** in this file (0
`WINAPI`/`STDAPI` matches).

## Conclusion

Clean. No functions, no opaque handle production. No patch required.
