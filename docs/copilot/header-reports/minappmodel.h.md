# Header Report: minappmodel.h

## Partitions
`AppxPackaging`

## Ownership audit (producer-site-only policy)
- Declares only `PACKAGE_*`/`APPLICATION_USER_MODEL_ID_*` length/bound constants. No functions, no handles.

## Conclusion
`accepted-normalized` — no ownership annotation required (constants only, no functions).
