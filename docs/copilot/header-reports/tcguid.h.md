# Header Report: tcguid.h

## Partitions
`Qos`

## Ownership audit (producer-site-only policy)
- Declares only `GUID_QOS_*` GUID constants (via `DEFINE_GUID`). No functions, no handles.

## Conclusion
`accepted-normalized` — no ownership annotation required (GUID constants only, no functions).
