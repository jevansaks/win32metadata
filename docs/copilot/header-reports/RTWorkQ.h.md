# Header Report: RTWorkQ.h

## Partitions
`Threading`

## Ownership audit (producer-site-only policy)
Genuine gaps found, both reusing the already-established **generic-type direct-out-param** blocker class (`wslapi.h`/`ratings.h`/`avrt.h`/`powersetting.h`/`ondemandconnroutehelper.h`/`davclnt.h`):

- `RtwqJoinWorkQueue(DWORD workQueueId, HANDLE hFile, _Out_ HANDLE* out)` produces a generic `HANDLE` via a direct out-param; the exact close site is ambiguous from this header alone (`RtwqUnjoinWorkQueue` takes the original `hFile`, not the produced `out` value), reinforcing that this is a generic, not distinctly-named, type.
- `RtwqSetDeadline`/`RtwqSetDeadline2(..., _Out_ HANDLE* pRequest)` produce a generic `HANDLE` via a direct out-param, closed via `RtwqCancelDeadline(_In_ HANDLE pRequest)`.

Other functions (`RtwqAllocateWorkQueue`, `RtwqLockSharedWorkQueue`, `RtwqAddPeriodicCallback`, etc.) produce plain `DWORD` IDs (not `HANDLE`-family types, out of scope entirely) or standard COM interface pointers (`IRtwqAsyncResult**`, out of scope).

## Conclusion
`blocked` — genuine gaps in `RtwqJoinWorkQueue` and `RtwqSetDeadline`/`RtwqSetDeadline2`/`RtwqCancelDeadline` (generic `HANDLE` direct-out-param, reuses established blocker class).
