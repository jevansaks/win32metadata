# mfcontentdecryptionmodule.h — accepted-normalized

**Partition:** Mf
**Size:** ~32215 bytes

## Analysis

Declares GUID constants and one free function,
`MFCreateEncryptedMediaExtensionsStoreActivate`, whose output
(`_Outptr_ IMFActivate** activate`) is a **COM interface pointer**
(IUnknown-derived, ref-counted), not a raw owned handle — out of
scope for handle-typedef producer-site annotation regardless of what
it returns, per blocker-class 5 (COM objects use ref-counting, not
`CloseApi`/`InvalidHandleValues` metadata).

## Conclusion

Clean. No patch required.
