# Header Report: audevcod.h

## Partitions
`Media.DShow`

## Ownership audit (producer-site-only policy)
- Declares only the `SNDDEV_ERR` enum and `EC_SNDDEV_*`/`EC_SND_DEVICE_ERROR_BASE` constants (audio device error event codes). No functions, no handles.

## Conclusion
`accepted-normalized` — no ownership annotation required (error-code enum/constants only, no functions).
