# Header Report: sliddefs.h

## Partitions
`Identity`

## Ownership audit (producer-site-only policy)
- Declares only a single `DEFINE_GUID(WINDOWS_SLID, ...)` constant. No functions, no handles.

## Conclusion
`accepted-normalized` — no ownership annotation required (GUID constant only, no functions).
