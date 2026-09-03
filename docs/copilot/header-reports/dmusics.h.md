# Header Report: dmusics.h

## Partitions
`Audio.DirectMusic`

## Ownership audit (producer-site-only policy)
- Declares only the `DECLARE_INTERFACE_`-style COM interfaces `IDirectMusicSynth`/`IDirectMusicSynthSink` plus constants and a transparent `DMUS_VOICE_STATE` struct. No `extern`/`DllImport` functions.

## Conclusion
`accepted-normalized` — no ownership annotation required (COM-style interfaces + constants/structs only, no extern functions).
