# dpapi.h — accepted-normalized

**Partitions:** Security.Cryptography, Security.Cryptography.UI
**Size:** ~10565 bytes

## Analysis

`dpapi.h` declares the DPAPI (Data Protection API) surface:
`CryptProtectData`, `CryptUnprotectData`, `CryptProtectDataNoUI`,
`CryptUnprotectDataNoUI`, and `CryptUpdateProtectedState`.

All output parameters are transparent, fully-visible-field structs:
- `DATA_BLOB *pDataOut` — `{ DWORD cbData; BYTE *pbData; }`, freed by the
  caller via `LocalFree(pDataOut->pbData)` per the documented DPAPI
  convention.
- `CRYPTPROTECT_PROMPTSTRUCT` — input-only prompt configuration struct,
  no ownership transfer.
- Various `CRYPT_*` blob structs used as inputs (entropy, optional
  entropy) — transparent, caller-owned.

No opaque handle types are produced. Per the producer-site-only
ownership policy, transparent structs freed via a generic allocator
(`LocalFree`) are out of scope for `[RAIIFree]`/`[InvalidHandleValue]`
metadata (blocker-class 6: transparent-struct alloc/free pairs).

## Conclusion

Clean. No metadata gap. No patch required.
