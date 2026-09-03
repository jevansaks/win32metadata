# Header Report: adssts.h

## Partitions
`ActiveDirectory`

## Ownership audit (producer-site-only policy)
- Declares only `ADS_PRINTER_*`/`ADS_JOB_*`/`ADS_SERVICE_*` status-code constants. No functions, no handles.

## Conclusion
`accepted-normalized` — no ownership annotation required (status-code constants only, no functions).
