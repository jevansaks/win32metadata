# audiopolicy.h — accepted-normalized

**Partition:** Audio
**Size:** ~46606 bytes

## Analysis

MIDL-generated COM header (`IAudioSessionManager`/`ISimpleAudioVolume`
interfaces). Confirmed via multi-line-aware grep that there are **no
free function declarations** — all methods are COM vtable methods,
out of scope per blocker-class 5.

## Conclusion

Clean. No patch required.
