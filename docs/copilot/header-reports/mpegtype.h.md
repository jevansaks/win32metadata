# Header Report: mpegtype.h

## Partitions
`Media.DShow`

## Ownership audit (producer-site-only policy)
- Declares `AM_MPEGSTREAMTYPE`/`AM_MPEGSYSTEMTYPE` data structs, helper macros, and the `IMpegAudioDecoder` COM interface — all `get_*`/`put_*` methods take/output plain `unsigned long`/struct values. No raw `HANDLE` involved.

## Conclusion
`accepted-normalized` — no ownership annotation required (COM interface with plain-value parameters only, no handle).
