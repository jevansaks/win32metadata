# Header Report: mswsockdef.h

## Partitions
`WinSock`

## Scrape validation
- Re-scraped `WinSock` partition (`ScanArch=x86`) after touching `main.cpp`.
- Result: `Build succeeded. 0 Error(s).` (3 pre-existing unrelated cross-partition remap warnings for `sockaddr`/`timeval`/`_CERT_CONTEXT`, not touched by this header).

## Ownership audit (producer-site-only policy)
- Declares `RIO_BUFFERID`/`RIO_CQ`/`RIO_RQ` (opaque pointer-to-incomplete-struct types with `RIO_INVALID_*` sentinel constants) plus `WSA_COMPATIBILITY_MODE`/`RIORESULT`/`RIO_BUF`/`RIO_CMSG_BUFFER` data structs, and two inline helper functions (`SOCKADDR_SIZE`, `ScopeLevel`) that do not touch the RIO types. **No functions producing/consuming `RIO_BUFFERID`/`RIO_CQ`/`RIO_RQ` are declared anywhere as `extern`/`DllImport` functions.**
- Investigated the actual RIO producer/consumer API (`RIORegisterBuffer`/`RIODeregisterBuffer`, `RIOCreateCompletionQueue`/`RIOCloseCompletionQueue`, `RIOCreateRequestQueue`) in the already-`accepted-normalized` `MSWSock.h`: these are **function-pointer typedef fields inside the `RIO_EXTENSION_FUNCTION_TABLE` struct** (retrieved dynamically at runtime via `WSAIoctl(SIO_GET_MULTIPLE_EXTENSION_FUNCTION_POINTER)`), not directly-linked `extern`/`DllImport` function declarations. This is architecturally out of scope for the producer-site annotation mechanism for the same reason COM vtable methods are (no scrapable function declaration exists to annotate at all — confirmed via `GameInput.h`, batch `scraping-investigation-13`, and `scclient.h`, batch `scraping-investigation-43`).

## Conclusion
`accepted-normalized` — no ownership annotation required. `RIO_BUFFERID`/`RIO_CQ`/`RIO_RQ` are genuine opaque handles, but their producer/consumer functions are accessed only via runtime function-pointer retrieval (not `extern`/`DllImport` declarations), architecturally out of scope for the annotation mechanism.
