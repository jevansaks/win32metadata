# Header Report: ntioring_x.h

## Partitions
`Fs`

## Ownership audit (producer-site-only policy)
- Declares only IoRing version/feature-flag enums (`IORING_VERSION`, `IORING_FEATURE_FLAGS`) and related transparent structs/constants. No functions (the actual IoRing API functions producing `HIORING` live in a separate header, out of scope here).

## Conclusion
`accepted-normalized` — no ownership annotation required (enums/constants only, no functions in this "types" header).
