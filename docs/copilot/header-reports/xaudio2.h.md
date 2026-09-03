# xaudio2.h — accepted-normalized

**Partition:** Xaudio2
**Size:** ~68581 bytes

## Analysis

`xaudio2.h` declares `XAudio2Create`/`XAudio2CreateWithVersionInfo`
— both output `_Outptr_ IXAudio2**` (COM interface pointer), out of
scope for producer-site handle annotation regardless of what they
return, per blocker-class 5. Remainder of header is COM vtable
interfaces.

## Conclusion

Clean. No patch required.
