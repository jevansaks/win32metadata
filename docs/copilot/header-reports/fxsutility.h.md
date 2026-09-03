# Header Report: fxsutility.h

## Partitions
`Fax`

## Ownership audit (producer-site-only policy)
- Two functions: `CanSendToFaxRecipient()` (no params) and `SendToFaxRecipient(SendToMode sndMode, LPCWSTR lpFileName)` (string input only). No `HANDLE` involved.

## Conclusion
`accepted-normalized` — no ownership annotation required (no handle involved).
