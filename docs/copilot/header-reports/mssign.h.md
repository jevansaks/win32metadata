# mssign.h — accepted-normalized

**Partition:** Security.Cryptography
**Size:** ~10508 bytes

## Analysis

`mssign.h` declares the Authenticode signing API:
`SignerSign`, `SignerSignEx`, `SignerSignEx2`, `SignerSignEx3`,
`SignerTimeStamp`, `SignerTimeStampEx`, `SignerTimeStampEx2`,
`SignerTimeStampEx3`, `SignerFreeSignerContext`, `SignError`.

`SignerSignEx`/`SignerSignEx2`/`SignerSignEx3` produce a
`SIGNER_CONTEXT **ppSignerContext`, consumed/freed via
`SignerFreeSignerContext(_In_ SIGNER_CONTEXT *pSignerContext)`. However,
`SIGNER_CONTEXT` is a fully-defined transparent struct:

```c
typedef struct _SIGNER_CONTEXT {
    DWORD cbSize;
    DWORD cbBlob;
    BYTE  *pbBlob;
} SIGNER_CONTEXT, *PSIGNER_CONTEXT;
```

All fields are visible/documented (a size-prefixed byte blob). Per the
producer-site-only ownership policy, transparent structs are out of
scope for `[RAIIFree]` metadata even when a genuine dedicated free
function exists (blocker-class 6: transparent-struct alloc/free pairs,
established via `dmort.h`/`keycredmgr.h`/`certpoleng.h`/`tokenbinding.h`/
`MrmResourceIndexer.h`). `SIGNER_CONTEXT` is directly analogous.

Function-pointer typedefs (`PFN_AUTHENTICODE_DIGEST_SIGN*`) used as
struct fields in `SIGNER_DIGEST_SIGN_INFO` are architecturally out of
scope (blocker-class 4).

Other input params (`PCCERT_CONTEXT`, `HCERTSTORE`) are tracked
elsewhere and consumed only, not produced, by this header.

## Conclusion

Clean. No metadata gap representable under current policy. No patch
required.
