# xinput.h — accepted-normalized

**Partition:** Xinput
**Size:** ~11491 bytes

## Analysis

`xinput.h` declares the Xbox 360 controller API
(`XInputGetState`, `XInputSetState`, `XInputGetCapabilities`,
`XInputEnable`, `XInputGetAudioDeviceIds`,
`XInputGetBatteryInformation`, `XInputGetKeystroke`,
`XInputGetDSoundAudioDeviceGuids`). All functions take a `DWORD
dwUserIndex` (controller slot number, not a handle) and output
transparent structs (`XINPUT_STATE`, `XINPUT_CAPABILITIES`,
`XINPUT_KEYSTROKE`) or plain GUIDs/scalars. No opaque handle types are
produced.

## Conclusion

Clean. No ownership metadata gap. No patch required.
