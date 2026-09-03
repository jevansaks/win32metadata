# Header Report: realtimeapiset.h

## Partitions
`Base`

## Ownership audit (producer-site-only policy)
- `QueryThreadCycleTime(_In_ HANDLE ThreadHandle, ...)`/`QueryProcessCycleTime(_In_ HANDLE ProcessHandle, ...)` take `HANDLE` as caller-supplied inputs (existing thread/process handles) — not produced here. All other functions (`QueryIdleProcessorCycleTime*`, `QueryInterruptTime*`, `QueryAuxiliaryCounterFrequency`, `ConvertAuxiliaryCounterToPerformanceCounter`, etc.) output only plain `PULONG64`/`PULONGLONG` values. No `HANDLE` production.

## Conclusion
`accepted-normalized` — no ownership annotation required (HANDLE is a caller-supplied input, not produced here; all other outputs are plain integers).
