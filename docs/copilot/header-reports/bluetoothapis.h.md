# `bluetoothapis.h`

- **Status:** accepted-normalized
- **Batch:** `existing-patches-02`
- **Partitions:** Bluetooth
- **Delta:** Retained artifact adds `_Win32_metadata_set_last_error_` to the Bluetooth
  radio/device enumeration and pairing functions (`BluetoothFindFirstRadio`,
  `BluetoothFindNextRadio`, `BluetoothFindRadioClose`, `BluetoothGetRadioInfo`,
  `BluetoothFindFirstDevice`, `BluetoothFindNextDevice`, `BluetoothFindDeviceClose`,
  `BluetoothSelectDevices`, `BluetoothDisplayDeviceProperties`,
  `BluetoothRegisterForAuthenticationEx`, `BluetoothUnregisterAuthentication`,
  `BluetoothSdpEnumAttributes`), plus the `win32metadata_annotations.h` include guard.
- **Resolution:** No further changes needed; retained as-is.
- **Artifact:** `generation/WinSDK/patches/post-midl/bluetoothapis.h.zzz-set-last-error.patch`
- **Evidence:** `git apply --check --reverse` passes for the retained patch. Prior sequential
  x64, arm64, and x86 generation validated the annotation form (per
  `shift-left-metadata-worklog.md`).
- **Normalization:** ABI-neutral declaration annotation.
