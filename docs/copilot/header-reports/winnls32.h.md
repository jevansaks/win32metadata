# Header Report: winnls32.h

## Partitions
`FileHistory`, `WinProg`

## Ownership audit (producer-site-only policy)
- `IMPGetIMEA/W`/`IMPQueryIMEA/W`/`IMPSetIMEA/W`/`WINNLSGetIMEHotkey`/`WINNLSEnableIME`/`WINNLSGetEnableStatus` all take `HWND` as a caller-supplied input (existing window) only. The `IMEPRO`/`LPIMEPROA`/`LPIMEPROW` structs are caller-allocated and passed by pointer to be filled/read — not created/destroyed via a handle mechanism. No `HANDLE` production.

## Conclusion
`accepted-normalized` — no ownership annotation required (HWND is an input; IME info struct is caller-allocated, not a handle).
