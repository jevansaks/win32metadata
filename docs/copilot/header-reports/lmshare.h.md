# lmshare.h — accepted-normalized

**Partition:** Fs
**Size:** ~21610 bytes

## Analysis

`lmshare.h` (NetShare/NetSession/NetFile/NetConnection API) declares
functions using the canonical `NET_API_STATUS NET_API_FUNCTION`
convention with `NetApiBufferAllocate`/`NetApiBufferFree`-managed
output buffers (blocker-class 7), already established clean across
many `lm*.h` headers.

## Conclusion

Clean. Net API buffer-allocation convention. No patch required.
