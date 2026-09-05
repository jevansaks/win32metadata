# Header Report: MSAJTransport.h

## Partitions
`AllJoyn`, `WinRT`

## Scrape validation — BLOCKED (new blocker class: parser/toolchain limitation)

Attempted to re-scrape the `AllJoyn` partition (`ScanArch=x86`) after touching `main.cpp` — this is the
only partition that actually enables the `WINAPI_PARTITION_APP`/`WINAPI_PARTITION_DESKTOP`-guarded
declarations in this header (confirmed: a fresh re-scrape of the `WinRT` partition, which also
`#include`s `msajtransport.h`, produces **zero** `AllJoynConnectToBus`/`AllJoynCreateBus`/
`AllJoynCloseBusHandle` declarations in its generated output — the partition's active
`WINAPI_FAMILY_PARTITION`/`WINVER` compile context does not satisfy this header's guards, so `WinRT`
cannot be used as a substitute validation path for this header).

The `AllJoyn` partition scrape **fails outright** with a genuine, unrelated compiler/toolchain
incompatibility, not a Win32 header content issue:

```
Diagnostics for 'C:\repos\win32metadata\generation\WinSDK\partitions\AllJoyn\main.cpp':
    C:\Program Files\Microsoft Visual Studio\18\Enterprise\VC\Tools\MSVC\14.51.36231\include\xmemory:219:5:
    error: use of undeclared identifier '__builtin_verbose_trap'
Skipping 'C:\repos\win32metadata\generation\WinSDK\partitions\AllJoyn\main.cpp' due to one or more errors listed above.
```

This is the C++ standard library header `<xmemory>` (part of the installed MSVC 14.51.36231 toolset,
pulled in transitively by the AllJoyn C++ SDK headers this partition also includes — `alljoyn_c\*.h`),
using a Clang/MSVC compiler builtin (`__builtin_verbose_trap`) that the pinned ClangSharp/libclang
version used by `Win32MetadataScraper` does not recognize. This is a genuine parser/toolchain version
mismatch between the installed MSVC toolset and the repo's pinned Clang version — **not** anything in
`MSAJTransport.h` or any other Win32 SDK header content. Fixing it would require either upgrading the
pinned ClangSharp/Clang version or pinning an older MSVC toolset for this partition — out of scope for
a header-content patch.

## Ownership audit (producer-site-only policy) — also a known blocker class

Even without a clean live-scrape, direct source inspection of `MSAJTransport.h` shows a real `HANDLE`
ownership relationship, **in addition to** the parser-limitation blocker above:
- `HANDLE WINAPI AllJoynConnectToBus(_In_opt_ PCWSTR)` — returns a bus `HANDLE` directly as the function
  return value (not out-param).
- `HANDLE WINAPI AllJoynCreateBus(DWORD, DWORD, LPSECURITY_ATTRIBUTES)` — likewise returns a bus
  `HANDLE` directly.
- `BOOL WINAPI AllJoynCloseBusHandle(_In_ HANDLE busHandle)` — the corresponding release function.
- All other functions (`AllJoynSendToBus`, `AllJoynReceiveFromBus`, `AllJoynEventSelect`,
  `AllJoynEnumEvents`, `AllJoynAcceptBusConnection`) take the bus `HANDLE` as an `_In_`-style consumer
  parameter only.

### EVIDENCE CORRECTED (this batch): the ownership pattern is representable
This is the same return-value-handle-ownership shape previously documented for
`getprocesshandlefromhwnd.h`, `wab.h`, `wincon.h`, `winppi.h`, `wnvapi.h`, and `i_cryptasn1tls.h` — all
of which were confirmed FIXABLE and FIXED in this batch via the established per-function
`Function::return=[RAIIFree(...)]` `emitter.settings.rsp` mechanism (68 existing precedents such as
`WTSOpenServerA::return=[RAIIFree("WTSCloseServer")]`). The prior claim that this pattern had "no
annotation precedent" is now known to be incorrect — it never blocked those other headers, and it does
not genuinely block this one either.

The correct, sole remaining blocker for `MSAJTransport.h` is the pre-existing `AllJoyn`
parser/toolchain failure above, which prevents scraping and validating **any** change to this
partition — including this now-known-correct ownership fix. Once the toolchain issue is resolved, apply:
```
AllJoynConnectToBus::return=[RAIIFree("AllJoynCloseBusHandle")]
AllJoynCreateBus::return=[RAIIFree("AllJoynCloseBusHandle")]
```
(function names to be reconfirmed against current header source at that time).

## Conclusion
`blocked` — sole blocker is the genuine parser/toolchain version mismatch (`__builtin_verbose_trap`
unsupported by the pinned Clang version), which prevents live-scrape validation of the only partition
(`AllJoyn`) where this header's declarations are actually compiled. The ownership pattern itself is
representable and not a blocker (see correction above) — not fixable via a header-content patch;
requires a toolchain upgrade.
