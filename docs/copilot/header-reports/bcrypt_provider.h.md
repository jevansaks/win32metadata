# bcrypt_provider.h — accepted-normalized

**Partition:** Security.Cryptography
**Size:** ~25100 bytes

## Analysis

`bcrypt_provider.h` (`um\cpdk\bcrypt_provider.h`, CNG BCrypt provider
DDI) declares only function-pointer typedefs for the provider
dispatch table (consistent with the sibling `ncrypt_provider.h`
pattern). Confirmed 0 `STDAPI`/`WINAPI` free-function matches — out of
scope per the function-pointer-typedef convention (blocker-class 4).

## Conclusion

Clean. No scrapable functions, no opaque handle production. No patch
required.
