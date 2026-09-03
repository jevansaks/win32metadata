# alljoyn_c\Init.h

**Classification:** blocked (parser/toolchain limitation - reused evidence)

## Summary
Part of the AllJoyn C-binding SDK, reachable only via the AllJoyn
partition. The AllJoyn partition fails to scrape entirely: 'use of
undeclared identifier `__builtin_verbose_trap`' in MSVC 14.51.36231's
`<xmemory>`, pulled in transitively by the AllJoyn C++ SDK headers
included by the partition - a genuine Clang/MSVC toolset version
mismatch unrelated to any Win32 header content, not fixable via a header
patch (would need a ClangSharp/Clang upgrade or MSVC toolset pin
change). This is the same parser/toolchain-limitation blocker class
already documented for MSAJTransport.h (see
docs/copilot/header-reports/MSAJTransport.h.md).

No other partition includes this header (confirmed pattern: AllJoyn C
bindings are exclusive to the AllJoyn partition), so there is no
alternate live-scrape validation path.

## Ownership Analysis
Not evaluable — no live-scrape validation is possible for this header
without a toolchain fix. AllJoyn C bindings typically use opaque
`typedef struct { ... }` handle-pointer types created via
`alljoyn_*_create` and destroyed via `alljoyn_*_destroy` functions,
which would be representable via the standard producer-site
`autoTypes.json`/`emitter.settings.rsp` mechanism if the partition
could be scraped and validated; this cannot be confirmed without a
successful build.

## Action
Blocked. Reused parser/toolchain-limitation blocker class (same root
cause as MSAJTransport.h). No patch artifact possible until the
toolchain mismatch is resolved.
