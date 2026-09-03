# dsound.h — accepted-normalized

**Partition:** Audio.DirectSound, FileHistory, WinProg
**Size:** ~114097 bytes

## Analysis

`dsound.h` declares `DirectSoundCreate`/`DirectSoundCreate8` — both
output COM interface pointers (`LPDIRECTSOUND*`/`LPDIRECTSOUND8*`),
out of scope per blocker-class 5.

## Conclusion

Clean. No patch required.
