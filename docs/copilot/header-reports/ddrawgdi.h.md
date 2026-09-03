# Header Report: ddrawgdi.h

## Partitions
`FileHistory`, `WinProg`

## Ownership audit (producer-site-only policy)
- Legacy, private GDI/DirectDraw kernel-mode interop entry points (renamed via `#define` to `GdiEntry1..17` when `NODDRAWGDI` is not defined).
- `DdGetDxHandle` returns a generic `HANDLE` directly as its C return value — the already-established **return-value handle ownership** blocker class (`getprocesshandlefromhwnd.h`).
- `DdCreateDIBSection` returns `HBITMAP` directly as its C return value — same return-value-handle class (GDI object handles follow the same "no bare return-value handle annotation anywhere in baseline metadata" rule).

## Conclusion
`blocked` — genuine gaps in `DdGetDxHandle` and `DdCreateDIBSection` (return-value handle ownership, reuses established blocker class).
