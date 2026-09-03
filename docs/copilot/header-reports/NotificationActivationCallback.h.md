# Header Report: NotificationActivationCallback.h

## Partitions
`Win32_Tile_Badge_Notif`

## Ownership audit (producer-site-only policy)
- Declares only the COM interface `INotificationActivationCallback` (single method `Activate`, all `_In_` inputs) plus a transparent `NOTIFICATION_USER_INPUT_DATA` struct. COM vtable method — out of scope; no `extern`/`DllImport` functions.

## Conclusion
`accepted-normalized` — no ownership annotation required (COM interface method only, out of scope).
