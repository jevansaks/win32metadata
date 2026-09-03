# mfvirtualcamera.h — accepted-normalized

**Partition:** Mf
**Size:** ~32270 bytes

## Analysis

Declares `IMFVirtualCamera` COM interface plus two free functions:
`MFCreateVirtualCamera` (outputs `_COM_Outptr_ IMFVirtualCamera**` —
COM interface pointer) and `MFIsVirtualCameraTypeSupported` (outputs
`_Out_ BOOL*` — plain boolean, not a handle). Neither produces a raw
owned handle; out of scope per blocker-class 5 (COM) / not a handle
at all (BOOL*).

## Conclusion

Clean. No patch required.
