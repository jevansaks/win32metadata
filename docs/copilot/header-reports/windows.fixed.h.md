# windows.fixed.h

**Classification:** accepted-normalized (no producer-site change needed)

## Summary
Internal build-fixup header (`generation/WinSDK/inc/windows.fixed.h`)
that sets `WIN32_LEAN_AND_MEAN`, includes `<windows.h>`, then `#undef`s a
long list of macro-redefined Win32 API names (`GetUserName`,
`SendMessage`, `DrawText`, etc.) so the A/W-suffixed real symbols are
visible to the scraper, followed by `#include <combaseapi.h>`. Contains
no direct function/typedef declarations of its own — purely a
preprocessor compatibility shim consumed by the scraping pipeline.

## Ownership Analysis
No functions or typedefs are declared directly in this header. Any
ownership-relevant content is attributed to the underlying
`windows.h`/`combaseapi.h` headers already classified elsewhere. No
`autoTypes.json`/`emitter.settings.rsp` changes required.

## Action
None. Classified accepted-normalized with no patch artifact.
