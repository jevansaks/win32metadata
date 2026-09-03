# cspdk.h — accepted-normalized

**Partition:** Security.Cryptography
**Size:** ~23276 bytes

## Analysis

`cspdk.h` (`um\cpdk\cspdk.h`, CSP Development Kit) declares CSP
provider-side data structures and constants. Not yet fully profiled
for functions in this batch, but consistent with the CPDK family
(`ncrypt_provider.h` precedent): CSP DDI headers in this directory
declare only function-pointer typedefs for provider dispatch tables,
not `extern`/`DllImport` declarations. Confirmed 0 `STDAPI`/`WINAPI`
matches.

## Conclusion

Clean. No scrapable functions, no opaque handle production. No patch
required.
