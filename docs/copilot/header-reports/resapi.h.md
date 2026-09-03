# resapi.h — accepted-normalized

**Partition:** MsCs
**Size:** ~84879 bytes

## Analysis

`resapi.h` (Failover Cluster Resource API) declares
`ClusWorkerCreate`/`ClusWorkerTerminate`, which operate on a
**caller-allocated** `CLUS_WORKER` struct (`OUT PCLUS_WORKER
lpWorker` — the caller provides the struct memory; `ClusWorkerCreate`
populates it and starts a thread, `ClusWorkerTerminate` joins/signals
termination but does not free the struct). This is a "populate
caller-provided struct" pattern, not a producer/consumer opaque-
handle pair — matches the transparent-struct blocker-class 6, out of
scope for handle-typedef annotation.

## Conclusion

Clean. No patch required — caller-owned struct, not an opaque handle
produced/freed by the API.
