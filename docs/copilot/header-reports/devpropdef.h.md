# Header Report: devpropdef.h

## Partitions
`Display`, `Nfc`, `Nfp`, `WpdSdk`

## Ownership audit (producer-site-only policy)
- Pure Plug-and-Play device-property type/modifier `#define` constants (`DEVPROP_TYPE_*`/`DEVPROP_TYPEMOD_*`) and the `DEVPROPTYPE` typedef. No functions.

## Conclusion
`accepted-normalized` — no ownership annotation required (constants/typedef only, no functions).
