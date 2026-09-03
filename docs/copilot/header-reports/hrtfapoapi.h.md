# Header Report: hrtfapoapi.h

## Partitions
`Xaudio2`

## Ownership audit (producer-site-only policy)
- `CreateHrtfApo(_In_ const HrtfApoInit* pInit, _Outptr_ IXAPO** ppXapo)` outputs a standard COM interface pointer, out of scope. Remaining declarations are `HRTF_*` constants and transparent structs/enums (`HrtfPosition`, `HrtfOrientation`, `HrtfDirectivityType`, `HrtfEnvironment`).

## Conclusion
`accepted-normalized` — no ownership annotation required (standard COM factory function + constants/structs only).
