# Header Report: robuffer.h

## Partitions
`WinRT`

## Scrape validation
- `WinRT` partition previously re-scraped this session (batch `scraping-investigation-18`); result `0 Warning(s), 0 Error(s)`.

## Ownership audit (producer-site-only policy)
- `RoGetBufferMarshaler(_Outptr_ IMarshal** bufferMarshaler)` is a clean COM-factory pattern (COM interface pointer output).
- `IBufferByteAccess::Buffer(_Outptr_result_buffer_(...) byte** value)` is a COM interface method returning a buffer *view* into memory owned by the underlying `IBuffer` WinRT object — not a separately-owned `HANDLE`-family resource (same reasoning as `memorybuffer.h`'s `IMemoryBufferByteAccess`, already accepted).

## Conclusion
`accepted-normalized` — no ownership annotation required (COM-factory + buffer-view pattern).
