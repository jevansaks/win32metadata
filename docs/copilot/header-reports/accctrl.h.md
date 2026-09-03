# accctrl.h — accepted-normalized

**Partition:** Authorization, Authorization.UI, Security, TransactionServer
**Size:** ~28225 bytes

## Analysis

`accctrl.h` declares only enums (`SE_OBJECT_TYPE`, `TRUSTEE_TYPE`,
`TRUSTEE_FORM`, etc.), transparent structs (`TRUSTEE`,
`EXPLICIT_ACCESS`), and one ownership-convention macro:
`#define AccFree LocalFree`. It contains **no function
declarations** of its own — the macro documents that ACL-related
producer functions elsewhere (e.g. `GetExplicitEntriesFromAclW` in
`aclapi.h`) return `LocalFree`-owned buffers.

`aclapi.h` (the sibling header containing the actual producer
functions) is already `accepted-normalized` in the ledger — this
defers cleanly per the established deferred-to-sibling-header
pattern (blocker-class 8); no re-audit gap found.

## Conclusion

Clean. No patch required.
