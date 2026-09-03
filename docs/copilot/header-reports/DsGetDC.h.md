# DsGetDC.h — accepted-normalized

**Partition:** ActiveDirectory
**Size:** ~18792 bytes

## Analysis

`DsGetDC.h` (`DsGetDcName` API) declares
`DsGetDcNameA(..., _Outptr_result_nullonfailure_
PDOMAIN_CONTROLLER_INFOA *DomainControllerInfo)`. The output is a
transparent struct freed via `NetApiBufferFree` (documented
convention) — matches the established Net API buffer-allocation
pattern (blocker-class 7).

## Conclusion

Clean. No opaque handle production. No patch required.
