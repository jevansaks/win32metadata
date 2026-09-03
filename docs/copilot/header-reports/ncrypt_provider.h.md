# ncrypt_provider.h — accepted-normalized

**Partition:** Security.Cryptography
**Size:** ~12407 bytes

## Analysis

`ncrypt_provider.h` (`um\cpdk\ncrypt_provider.h`) declares the CNG Key
Storage/Key Isolation Provider DDI (provider-side function-table
structs — `NCRYPT_KEY_STORAGE_FUNCTION_TABLE`, etc.) consisting of
struct-embedded function-pointer fields dynamically dispatched by the
CNG framework — not `extern`/`DllImport` declarations. Out of scope
per the function-pointer-typedef convention (blocker-class 4).

## Conclusion

Clean. No scrapable functions, no opaque handle production. No patch
required.
