# Header Report: datetimeapi.h

## Partitions
`Intl`

## Ownership audit (producer-site-only policy)
- `GetDateFormat(A/W/Ex)`/`GetTimeFormat(A/W/Ex)`/`GetDurationFormatEx` all output caller-allocated string buffers. No `HANDLE` involved.

## Conclusion
`accepted-normalized` — no ownership annotation required (string/buffer formatting API only, no handle).
