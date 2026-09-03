# Header Report: adshlp.h

## Partitions
`ActiveDirectory`

## Ownership audit (producer-site-only policy)
- `ADsGetObject`/`ADsOpenObject` are COM QueryInterface-style factories (`VOID**`/`void FAR* FAR*` per caller-supplied `riid`). `ADsBuildEnumerator`/`ADsFreeEnumerator`/`ADsEnumerateNext` operate on `IEnumVARIANT*` (standard COM refcounting).
- `AllocADsMem`/`FreeADsMem`/`ReallocADsMem` allocate/free a generic `LPVOID` memory block; `AllocADsStr`/`FreeADsStr`/`ReallocADsStr` allocate/free a generic `LPWSTR` string; `ADsEncodeBinaryData`'s `_Outptr_ LPWSTR* ppszDestData` follows the same generic string-allocation convention — all out of scope for the `HANDLE`-specific mechanism (same class as `resourceindexer.h`'s `PVOID`/`PWSTR` outputs).
- All other functions (`ADsGetLastError`, `ADsSetLastError`, `PropVariantToAdsType`, `AdsTypeToPropVariant`, `BinarySDToSecurityDescriptor`, `SecurityDescriptorToBinarySD`, etc.) operate on caller-allocated buffers/structs or plain values. No opaque `HANDLE`-family type is produced anywhere in this header.

## Conclusion
`accepted-normalized` — no ownership annotation required (COM refcounting + generic memory/string-allocation conventions only, no opaque HANDLE-family gap).
