# Header Report: sporder.h

## Partitions
`WinSock`

## Ownership audit (producer-site-only policy)
- `WSCWriteProviderOrder`/`WSCWriteNameSpaceOrder` (and their function-pointer typedefs / `*32` variants) operate on `LPDWORD`/`LPGUID` arrays only. No `HANDLE` involved.

## Conclusion
`accepted-normalized` — no ownership annotation required (DWORD/GUID array API only, no handle).
