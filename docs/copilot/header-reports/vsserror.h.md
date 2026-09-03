# vsserror.h — accepted-normalized

**Partition:** VSS
**Size:** ~23216 bytes

## Analysis

`errors.h` (VSS common error message definitions, a subset of
`vssadmin.exe` message definitions) declares only `VSS_E_*`
`HRESULT` error-code constants. It contains **no function
declarations, no structs, no handle typedefs**.

## Conclusion

Clean. No patch required.
