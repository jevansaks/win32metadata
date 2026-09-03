# Header Report: vpnotify.h

## Partitions
`Media.DShow`

## Ownership audit (producer-site-only policy)
- Declares the `IVPBaseNotify`/`IVPNotify`/`IVPNotify2`/`IVPVBINotify` COM interfaces. All active methods (`RenegotiateVPParameters`, `SetDeinterlaceMode`/`GetDeinterlaceMode`, `SetVPSyncMaster`/`GetVPSyncMaster`) take/output only plain `AMVP_MODE`/`BOOL` values. (Commented-out `SetDirectDrawSurface`/`GetDirectDrawSurface`/`SetVPColorControls`/`GetVPColorControls` methods are dead code, not compiled.) No `HANDLE` involved.

## Conclusion
`accepted-normalized` — no ownership annotation required (COM interfaces with plain enum/BOOL parameters only, no handle).
