# infocard.h — accepted-normalized

**Partitions:** Security.Cryptography, Security.Cryptography.UI
**Size:** ~13788 bytes

## Analysis

`infocard.h` (Windows CardSpace crypto helper library) declares
`GetToken`, `Encrypt`, `Decrypt`, `SignHash`, `VerifyHash`,
`GetCryptoTransform`, `GetKeyedHash`, `TransformBlock`,
`TransformFinalBlock`, `HashCore`, `HashFinal`, `FreeToken`,
`CloseCryptoHandle`, `GenerateDerivedKey`, `GetBrowserToken`,
`ManageCardSpace`.

`CloseCryptoHandle(__in PINFORMATIONCARD_CRYPTO_HANDLE hCrypto)` is a
genuine dedicated close function, but `INFORMATIONCARD_CRYPTO_HANDLE`
is a **fully transparent struct**:
```c
typedef struct _INFORMATIONCARD_CRYPTO_HANDLE
{
    HandleType type;
    LONGLONG   expiration;
    // points to the appropriate parameter struct above depending on the type.
    ...
} INFORMATIONCARD_CRYPTO_HANDLE, *PINFORMATIONCARD_CRYPTO_HANDLE;
```
Out of scope per the transparent-struct-alloc/free-pairs convention
(blocker-class 6), same as `SIGNER_CONTEXT`/`MrmResourceIndexerHandle`.
Similarly, `FreeToken(__in PGENERIC_XML_TOKEN pAllocMemory)` frees a
`GENERIC_XML_TOKEN` — also a fully transparent struct
(`FILETIME createDate; FILETIME expiryDate; LPWSTR xmlToken; ...`).

## Conclusion

Clean — both handle-shaped free functions target transparent structs,
out of scope under the current policy. No patch required.
