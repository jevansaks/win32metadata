# mfplay.h — accepted-normalized

**Partition:** Mf
**Size:** ~48110 bytes

## Analysis

`mfplay.h` declares `MFPCreateMediaPlayer` (`_Out_opt_
IMFPMediaPlayer** ppMediaPlayer`) — outputs a **COM interface
pointer**, out of scope for producer-site handle annotation
regardless of what it returns, per blocker-class 5. The rest of the
header is COM vtable interfaces.

## Conclusion

Clean. No patch required.
