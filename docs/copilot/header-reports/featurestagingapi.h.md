# Header Report: featurestagingapi.h

## Partitions
`FileHistory`, `WinProg`

## Ownership audit (producer-site-only policy)
- `DECLARE_HANDLE(FEATURE_STATE_CHANGE_SUBSCRIPTION)` is a genuine opaque handle, produced via
  `SubscribeFeatureStateChangeNotification(_Outptr_ FEATURE_STATE_CHANGE_SUBSCRIPTION* subscription, ...)`
  and released via `UnsubscribeFeatureStateChangeNotification(_In_ _Post_invalid_ FEATURE_STATE_CHANGE_SUBSCRIPTION subscription)`
  — the same shape as the fixable gap in `appnotify.h` (batch `scraping-investigation-39`).
- **Checked `autoTypes.json` first** (per the established methodology from `swdevice.h`, batch
  `scraping-investigation-45`): `FEATURE_STATE_CHANGE_SUBSCRIPTION` **already has a complete, correct
  entry** — `"ValueType": "DECLARE_HANDLE", "CloseApi": "UnsubscribeFeatureStateChangeNotification",
  "InvalidHandleValues": [-1, 0]` — matching this header exactly. No new inline annotation needed;
  existing legacy typedef metadata is already correct and complete.
- Other functions (`GetFeatureEnabledState`, `RecordFeatureUsage`, `RecordFeatureError`,
  `GetFeatureVariant`) take/output only plain enums/integers/strings, no `HANDLE` involved.

## Conclusion
`accepted-normalized` — `FEATURE_STATE_CHANGE_SUBSCRIPTION` ownership is already correctly captured via the existing `autoTypes.json` entry; no gap to fix.
