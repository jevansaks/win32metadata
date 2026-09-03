# ntldap.h — accepted-normalized

**Partition:** Ldap
**Size:** ~19780 bytes

## Analysis

`ntldap.h` (NT-specific server LDAP extensions) declares only control
constants and transparent structs consumed by the standard LDAP API
(declared elsewhere). It contains **no function declarations** (0
`WINAPI`/`STDAPI` matches).

## Conclusion

Clean. No functions, no opaque handle production. No patch required.
