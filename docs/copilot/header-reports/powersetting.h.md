# Header Report: powersetting.h

## Partitions
`Power`

## Ownership audit (producer-site-only policy)
Two distinct ownership-relevant findings:

1. **`HPOWERNOTIFY` (already covered)** — `PowerSettingRegisterNotification(..., _Out_ PHPOWERNOTIFY RegistrationHandle)` / `PowerSettingUnregisterNotification(_Inout_ HPOWERNOTIFY RegistrationHandle)` produce/consume the `HPOWERNOTIFY` type. `autoTypes.json` already has a complete entry (`Name: HPOWERNOTIFY`, `CloseApi: UnregisterPowerSettingNotification`, `InvalidHandleValues: [-1, 0]`) — this attaches ownership metadata to the type itself (via `NativeTypedefStructsCreator`'s `[RAIIFree]` on the auto-generated struct), which is a distinct, pre-existing, single-purpose-type mechanism (not the newer per-producer inline `_Win32_metadata_raii_free_` annotation the corrected policy targets — that policy applies to *shared/generic* types like `windef.h`'s `HWND`, not to single-CloseApi opaque types like this one). `RegisterPowerSettingNotification`/`UnregisterPowerSettingNotification` (the actual matching close pair, in `WinUser.h`) already use this same type consistently; `PowerSettingUnregisterNotification` in this header is a semantically-equivalent close for the same type. No new annotation needed.
2. **`PowerRegisterForEffectivePowerModeNotifications`/`PowerUnregisterFromEffectivePowerModeNotifications` (genuine gap)** — `_Outptr_ PVOID* RegistrationHandle` is a raw, generic `PVOID` (not a distinctly-named type), produced via a direct out-param. This is the already-established **generic-type direct-out-param** blocker class (`wslapi.h`/`ratings.h`/`avrt.h`): annotating a bare `PVOID` out-param would incorrectly apply ownership metadata to every `PVOID` value anywhere in the metadata.
3. `PowerGetActiveScheme`'s `_Outptr_ GUID** ActivePolicyGuid` is a transparent `GUID` struct output, out of scope (same as other transparent-struct patterns).

## Conclusion
`blocked` — genuine gap in `PowerRegisterForEffectivePowerModeNotifications`/`PowerUnregisterFromEffectivePowerModeNotifications` (generic `PVOID` direct-out-param, reuses established blocker class). `HPOWERNOTIFY` portion is already correctly covered via `autoTypes.json`.
