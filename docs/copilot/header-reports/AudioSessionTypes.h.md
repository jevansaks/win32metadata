# Header Report: AudioSessionTypes.h

## Partitions
`Audio`

## Ownership audit (producer-site-only policy)
- Pure enums (`AUDCLNT_SHAREMODE`, `AUDIO_STREAM_CATEGORY`) for the audio session manager RPC/COM interfaces. No functions.

## Conclusion
`accepted-normalized` — no ownership annotation required (enums only, no functions).
