# Header Report: ioringapi.h

## Partitions
`Fs`

## Ownership audit (producer-site-only policy)
- `HIORING` (`DECLARE_HANDLE`, explicitly documented "Release resources held by this using CloseIoRing(); [NOT CloseHandle()!]") is produced via `CreateIoRing`, closed via `CloseIoRing`. `autoTypes.json` already has a complete entry (`CloseApi: CloseIoRing`) — already correctly covered.
- `QueryIoRingCapabilities`/`GetIoRingInfo`/`PopIoRingCompletion` output transparent structs (`IORING_CAPABILITIES`/`IORING_INFO`/`IORING_CQE`), out of scope. `SetIoRingCompletionEvent`'s `HANDLE hEvent` is a pre-owned caller input.

## Conclusion
`accepted-normalized` — no ownership annotation required (`HIORING` already correctly covered via existing `autoTypes.json` entry; all other outputs are transparent structs or pre-owned inputs).
