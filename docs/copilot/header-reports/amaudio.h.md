# Header Report: amaudio.h

## Partitions
`Media.DShow`

## Ownership audit (producer-site-only policy)
- `IAMDirectSound` COM interface: `GetDirectSoundInterface`/`GetPrimaryBufferInterface`/`GetSecondaryBufferInterface` output COM interface pointers via `_Outptr_`-style parameters (standard COM refcounting pattern); `ReleaseDirectSoundInterface`/`ReleasePrimaryBufferInterface`/`ReleaseSecondaryBufferInterface` take an existing interface pointer to release its extra reference (COM `Release()`-style, not `HANDLE`). `SetFocusWindow`/`GetFocusWindow` take/output `HWND` (input/query only).
- `AMValidateAndFixWaveFormatEx` is an `__inline` helper operating on a caller-allocated `WAVEFORMATEX` buffer.
- No `HANDLE`-family type involved.

## Conclusion
`accepted-normalized` — no ownership annotation required (COM interface-pointer refcounting pattern, not HANDLE-family).
