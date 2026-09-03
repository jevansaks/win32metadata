# Header Report: audiomediatype.h

## Partitions
`Audio.Apo`

## Ownership audit (producer-site-only policy)
- Declares the COM interface `IAudioMediaType` (out of scope) plus `CreateAudioMediaType`/`CreateAudioMediaTypeFromUncompressedAudioFormat`, which output standard COM interface pointers (`IAudioMediaType**`), out of scope for the `HANDLE`-family annotation mechanism.

## Conclusion
`accepted-normalized` — no ownership annotation required (COM interface + standard COM factory functions only).
