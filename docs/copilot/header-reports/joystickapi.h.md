# joystickapi.h — accepted-normalized

**Partition:** Multimedia
**Size:** ~17546 bytes

## Analysis

`joystickapi.h` (ApiSet `api-ms-win-mm-joystick-l1-1-0`) declares
`joyGetDevCaps`, `joyGetNumDevs`, `joyGetPos`/`joyGetPosEx`,
`joySetCapture`/`joyReleaseCapture`, `joySetThreshold`, etc. All
functions take a plain `UINT uJoyID` (joystick index, not a handle)
and transparent structs (`JOYCAPS`, `JOYINFO`, `JOYINFOEX`). No
opaque handle types are produced.

## Conclusion

Clean. No opaque handle production. No patch required.
