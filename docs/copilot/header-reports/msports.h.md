# Header Report: msports.h

## Partitions
`SerPorts`

## Ownership audit (producer-site-only policy)
- `HCOMDB` (`DECLARE_HANDLE`) is produced via `ComDBOpen(_Out_ PHCOMDB PHComDB)`, closed via `ComDBClose(_In_ HCOMDB HComDB)`. `autoTypes.json` already has a complete entry (`CloseApi: ComDBClose`) — already correctly covered at the type level.
- `SerialDisplayAdvancedSettings` takes `HDEVINFO`/`PSP_DEVINFO_DATA` as pre-owned caller inputs only.
- `ComDBGetCurrentPortUsage`/`ComDBClaimNextFreePort`/`ComDBClaimPort`/`ComDBReleasePort`/`ComDBResizeDatabase` all take `HCOMDB` as input and output plain port numbers/`BOOL`, no additional gaps.

## Conclusion
`accepted-normalized` — no ownership annotation required (`HCOMDB` already correctly covered via existing `autoTypes.json` entry).
