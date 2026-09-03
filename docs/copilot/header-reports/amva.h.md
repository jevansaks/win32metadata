# Header Report: amva.h

## Partitions
`Media.DShow`

## Ownership audit (producer-site-only policy)
- Declares only `AMVA_*` constants and plain data structs (`AMVAUncompBufferInfo`, `AMVAUncompDataInfo`, `AMVAInternalMemInfo`, `AMVACompBufferInfo`, `AMVABeginFrameInfo`, `AMVAEndFrameInfo`, `AMVABUFFERINFO`). No functions, no handles.

## Conclusion
`accepted-normalized` — no ownership annotation required (constants + data structs only, no functions).
