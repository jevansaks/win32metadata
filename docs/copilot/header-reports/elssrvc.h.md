# Header Report: elssrvc.h

## Partitions
`Intl`

## Ownership audit (producer-site-only policy)
- Declares only `static const GUID ELS_GUID_*` constants (Extended Linguistic Services identifiers). No functions, no handles.

## Conclusion
`accepted-normalized` — no ownership annotation required (GUID constants only, no functions).
