# ddrawgdi.h

**Classification:** accepted-normalized (no code change needed - not a genuine gap)

## Summary
Re-audit found neither producer previously listed is actually an
unaddressed ownership gap:
- `DdCreateDIBSection` returns `HBITMAP`, which already carries
  `CloseApi=DeleteObject` at the type level via `autoTypes.json`
  (`Windows.Win32.Graphics.Gdi.HBITMAP`) - automatically covers every
  `HBITMAP`-returning function, including this one.
- `DdGetDxHandle(..., BOOL bRelease)` is self-releasing: calling the same
  function again with `bRelease=TRUE` releases the handle. It has no separate
  consumer function to name in a `RAIIFree` annotation, so it is not an
  ownership gap in the RAIIFree sense at all (this is an internal/private GDI
  entry point renamed `GdiEntry14`, not a public documented API).

## Correction to prior investigation
Prior report blocked this header claiming both functions were unaddressed
return-value `HANDLE`/`HBITMAP` ownership gaps. Neither actually needs (or
can meaningfully use) a `RAIIFree` annotation.

## Ownership Analysis
No `emitter.settings.rsp`/`autoTypes.json` change required.

## Validation
Confirmed by inspection of `generation/WinSDK/autoTypes.json` (HBITMAP entry,
`CloseApi: "DeleteObject"`) and the `ddrawgdi.h` source (`DdGetDxHandle`
signature/comment describing `bRelease` self-release semantics).
