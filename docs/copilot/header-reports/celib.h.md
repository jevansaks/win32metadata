# celib.h — accepted-normalized

**Partition:** Security
**Size:** ~12340 bytes

## Analysis

`celib.h` (Certificate Enrollment helper library) declares utility
functions: `ceDbgPrintf`, `ceHLastError`, `ceGetBStrBuffer`, and an
encode/decode function outputting `OUT BYTE **ppbEncoded, OUT DWORD
*pcbEncoded`. `ceGetBStrBuffer` outputs `_Outptr_result_buffer_
(*pcbOut) PBYTE *ppbOut` — a transparent, generically-allocated byte
buffer (`fCoTaskMemAlloc` flag controls the allocator used), not an
opaque handle. No distinctly-named handle types are produced.

## Conclusion

Clean. Transparent buffer outputs only. No patch required.
