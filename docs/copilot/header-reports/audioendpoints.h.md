# Header Report: audioendpoints.h

## Partitions
`Audio.Endpoints`

## Ownership audit (producer-site-only policy)
- Declares only the COM interface `IAudioEndpointFormatControl` (single method `ResetToDefault(_In_ DWORD)`, no outputs). COM vtable method — out of scope; no `extern`/`DllImport` functions.

## Conclusion
`accepted-normalized` — no ownership annotation required (COM interface method only, no extern functions).
