# lmjoin.h — accepted-normalized

**Partition:** NetMgmt
**Size:** ~15515 bytes

## Analysis

`lmjoin.h` (NetSetup domain-join API) declares `NetJoinDomain` (all
string inputs, no handle production) and `NetGetJoinInformation`,
which follows the canonical `NET_API_STATUS`/
`NetApiBufferAllocate`/`NetApiBufferFree` convention (blocker-class
7), already established clean across many `lm*.h` headers.

## Conclusion

Clean. No opaque handle production. No patch required.
