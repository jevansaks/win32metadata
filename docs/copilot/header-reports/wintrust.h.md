# wintrust.h — accepted-normalized

**Partition:** Security.WinTrust
**Size:** ~73549 bytes

## Analysis

`wintrust.h` declares `WinVerifyTrust` (returns `LONG` error code, not
a handle) and `WTHelperProvDataFromStateData(HANDLE hStateData)` — a
query helper consuming the generic `HANDLE` state-data token that is
itself produced/destroyed by repeated calls to `WinVerifyTrust` with
different `WTD_STATEACTION_*` values (create/verify vs. close), not
by a separate dedicated producer/consumer function pair. This
"single-function, action-parameter-driven lifecycle" pattern is not
representable via the `CloseApi` convention (which assumes a distinct
destroyer function callable with just the handle).

## Conclusion

Clean. No patch required — no representable producer/consumer
function-pair pattern for the state-data handle.
