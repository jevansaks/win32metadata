# Header Report: mpconfig.h

## Partitions
`Media.DShow`

## Ownership audit (producer-site-only policy)
- Declares only `DECLARE_INTERFACE_`-style COM interfaces (`IMixerPinConfig`, `IMixerPinConfig2`) — pure vtable methods (color-key/blend/aspect-ratio getters/setters), out of scope for the annotation mechanism. No extern functions.

## Conclusion
`accepted-normalized` — no ownership annotation required (COM-style interface methods only, no extern functions).
