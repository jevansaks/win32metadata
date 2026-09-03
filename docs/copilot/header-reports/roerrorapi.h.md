# roerrorapi.h — accepted-normalized

**Partition:** WinRT
**Size:** ~13102 bytes

## Analysis

`roerrorapi.h` declares the WinRT error-reporting API:
`RoGetErrorReportingFlags`/`RoSetErrorReportingFlags` (plain `DWORD`
flags, no handles), `RoOriginateError`/`RoOriginateErrorW`,
`RoTransformError`/`RoTransformErrorW` (all `BOOL`-returning, no
handle production), and
`RoResolveRestrictedErrorInfoReference(_In_ PCWSTR reference, _Outptr_
IRestrictedErrorInfo** ppRestrictedErrorInfo)` — a standard COM
interface pointer output, out of scope per the COM-interface-pointer
convention.

## Conclusion

Clean. No opaque handle production. No patch required.
