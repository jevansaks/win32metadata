# Header Report: wiatwcmp.h

## Partitions
`Wia`

## Ownership audit (producer-site-only policy)
- Declares only the `TWAIN_CAPABILITY` data struct and `ESC_TWAIN_*`/`WiaItemTypeTwainCapabilityPassThrough` constants. No functions, no handles.

## Conclusion
`accepted-normalized` — no ownership annotation required (data struct + constants only, no functions).
