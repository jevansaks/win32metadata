# xaudio2fx.h — accepted-normalized

**Partition:** Xaudio2
**Size:** ~19247 bytes

## Analysis

`xaudio2fx.h` (XAudio2 audio effects) declares
`CreateAudioVolumeMeter(_Outptr_ IUnknown** ppApo)` and
`CreateAudioReverb(_Outptr_ IUnknown** ppApo)`. Both output standard
COM interface pointers, out of scope per the COM-interface-pointer
convention.

## Conclusion

Clean. No opaque handle production. No patch required.
