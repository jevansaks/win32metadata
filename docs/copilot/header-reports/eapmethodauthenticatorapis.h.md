# eapmethodauthenticatorapis.h — accepted-normalized

**Partition:** EapHost
**Size:** ~20104 bytes

## Analysis

`EapMethodAuthenticatorApis.h` declares
`EapMethodAuthenticatorInvokeConfigUI(..., _Out_ DWORD*
pdwSizeOfConfigOut, _Outptr_result_buffer_(*pdwSizeOfConfigOut) BYTE**
ppConfigOut, ...)`. The output is a generic byte buffer, not an
opaque handle.

## Conclusion

Clean. No opaque handle production. No patch required.
