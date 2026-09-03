# aclapi.h — accepted-normalized

**Partitions:** Authorization, Authorization.UI
**Size:** ~21344 bytes

## Analysis

`aclapi.h` (Win32 Access Control APIs) declares
`GetNamedSecurityInfoA`/`W`, `SetNamedSecurityInfo`,
`TreeSetNamedSecurityInfo`, etc. Outputs (`PSID*`, `PACL*`,
`PSECURITY_DESCRIPTOR*`) all point into a single allocated security
descriptor buffer, freed via `LocalFree` (documented convention) —
transparent structs, out of scope per the transparent-struct
convention (blocker-class 6). `FN_PROGRESS` is a callback
function-pointer typedef (blocker-class 4).

## Conclusion

Clean. No opaque handle production. No patch required.
