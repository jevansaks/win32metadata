# Header Report: amparse.h

## Partitions
`Media.DShow`

## Ownership audit (producer-site-only policy)
- Declares `DEFINE_GUID(IID_IAMParse, ...)` and the `IAMParse` COM interface (`GetParseTime(_Out_ REFERENCE_TIME*)`, `SetParseTime(REFERENCE_TIME)`, `Flush()`). All outputs are plain `REFERENCE_TIME` (64-bit integer) values. No `HANDLE` involved.

## Conclusion
`accepted-normalized` — no ownership annotation required (COM interface with plain integer output, no handle).
