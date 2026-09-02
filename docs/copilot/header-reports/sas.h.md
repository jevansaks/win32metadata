# Header Report: sas.h

## Partitions
`Identity`

## Ownership audit (producer-site-only policy)
- Single function: `VOID WINAPI SendSAS(_In_ BOOL AsUser)`. No `HANDLE` parameters or return value.

## Conclusion
`accepted-normalized` — no ownership annotation required (no handle involved at all).
