# stdint.h

**Classification:** accepted-normalized (no producer-site change needed)

## Summary
Standard C99 `<stdint.h>` (fixed-width integer typedefs: `int32_t`,
`uint64_t`, etc.), included only by the `AllJoyn` partition's `main.cpp`
(line 6). This is compiler/CRT-provided content, not a Windows SDK
header — it defines only scalar integer typedefs, no functions, and is
universally known standard-library content independent of any live
scrape. Even though the AllJoyn partition itself fails to scrape
(documented parser/toolchain limitation, same as MSAJTransport.h and the
28 `alljoyn_c\*.h` headers), this specific header's content can be
classified with full confidence directly from the C99 standard: it
contains zero functions and therefore zero ownership-relevant surface,
regardless of the partition's build status.

## Ownership Analysis
No functions or handle-like typedefs exist (only fixed-width integer
scalar typedefs). No `autoTypes.json`/`emitter.settings.rsp` changes
required.

## Action
None. Classified accepted-normalized with no patch artifact (content
verified by standard-library knowledge, not live-scrape, since the only
referencing partition cannot currently build).
