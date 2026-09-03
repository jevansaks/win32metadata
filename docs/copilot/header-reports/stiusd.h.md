# Header Report: stiusd.h

## Partitions
`ImagingDevice`

## Ownership audit (producer-site-only policy)
- Declares only the COM interface `IStiUSD` (Still-Image Driver interface) plus GUID constants. No `extern`/`DllImport` functions.

## Conclusion
`accepted-normalized` — no ownership annotation required (COM interface + GUID constants only, no extern functions).
