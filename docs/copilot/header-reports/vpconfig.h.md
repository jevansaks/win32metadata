# Header Report: vpconfig.h

## Partitions
`Media.DShow`

## Ownership audit (producer-site-only policy)
- Declares only `DECLARE_INTERFACE_`-style COM interfaces (`IVPBaseConfig`, `IVPConfig`, `IVPVBIConfig`) — pure vtable methods, out of scope for the annotation mechanism (no `extern "C"`/`DllImport` linkage). No standalone extern functions.

## Conclusion
`accepted-normalized` — no ownership annotation required (COM-style interface methods only, no extern functions).
