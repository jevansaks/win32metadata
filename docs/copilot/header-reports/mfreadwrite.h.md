# mfreadwrite.h — accepted-normalized

**Partition:** Mf
**Size:** ~75324 bytes

## Analysis

`mfreadwrite.h` declares `MFCreateSourceReaderFrom*`/
`MFCreateSinkWriterFrom*` — all output COM interface pointers
(`IMFSourceReader**`/`IMFSinkWriter**`), out of scope for producer-
site handle annotation regardless of what they return, per
blocker-class 5.

## Conclusion

Clean. No patch required.
