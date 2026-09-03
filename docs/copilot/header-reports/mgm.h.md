# mgm.h — accepted-normalized

**Partition:** RRas
**Size:** ~14885 bytes

## Analysis

`mgm.h` (Multicast Group Manager) declares only transparent structs
(`MGM_IF_ENTRY`) and callback function-pointer typedefs
(`PMGM_CREATION_ALERT_CALLBACK`, `PMGM_PRUNE_ALERT_CALLBACK`,
`PMGM_JOIN_ALERT_CALLBACK`, `PMGM_WRONG_IF_CALLBACK`) — out of scope
per the function-pointer-typedef convention (blocker-class 4). It
contains **no `extern`/`DllImport` function declarations**.

## Conclusion

Clean. No scrapable functions, no opaque handle production. No patch
required.
