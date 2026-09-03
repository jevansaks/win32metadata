# lmwksta.h — accepted-normalized

**Partition:** NetMgmt
**Size:** ~17762 bytes

## Analysis

`lmwksta.h` (NetWorkstation/NetWkstaTransport API) declares functions
using the canonical `NET_API_STATUS NET_API_FUNCTION` convention with
`NetApiBufferAllocate`/`NetApiBufferFree`-managed output buffers
(blocker-class 7), already established clean across many `lm*.h`
headers.

## Conclusion

Clean. Net API buffer-allocation convention. No patch required.
