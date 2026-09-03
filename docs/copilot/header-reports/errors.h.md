# Header Report: errors.h

## Partitions
`Media.DShow`

## Ownership audit (producer-site-only policy)
- Two functions: `AMGetErrorTextA`/`AMGetErrorTextW(HRESULT, _Out_writes_(MaxLen) LPSTR/LPWSTR pbuffer, DWORD MaxLen)`. Outputs are caller-allocated string buffers. No `HANDLE` involved.

## Conclusion
`accepted-normalized` — no ownership annotation required (caller-allocated string buffer output only).
