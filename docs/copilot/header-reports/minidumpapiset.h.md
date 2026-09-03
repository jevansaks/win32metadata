# minidumpapiset.h — accepted-normalized

**Partition:** Base, Debug
**Size:** ~48351 bytes

## Analysis

`minidumpapiset.h` declares `MiniDumpWriteDump`/`MiniDumpReadDumpStream`
— both operate on caller-provided `HANDLE hProcess`/`hFile` (already
open, passed in), and `MiniDumpReadDumpStream` outputs a raw pointer
into caller-provided mapped memory (not an allocated/owned resource).
No handle production found.

## Conclusion

Clean. No patch required.
