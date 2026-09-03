# Header Report: shcore.h

## Partitions
`WinRT`

## Ownership audit (producer-site-only policy)
- `CreateRandomAccessStreamOnFile`/`CreateRandomAccessStreamOverStream`/`CreateStreamOverRandomAccessStream` are all clean COM QueryInterface-style factory functions (`_In_ REFIID riid, _COM_Outptr_ void** ppv`), consistent with precedent established throughout this session. No raw `HANDLE` involved.

## Conclusion
`accepted-normalized` — no ownership annotation required (COM factory pattern).
