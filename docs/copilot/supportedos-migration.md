# supportedOS.rsp Sidecar-Removal Tranche

## Summary

`generation/WinSDK/supportedOS.rsp` is a `--with-attribute` sidecar: one line per
declared name mapping to a C# `[SupportedOSPlatform("windowsX.Y.ZZZZ")]` attribute.
Unlike the `WithSetLastError.rsp`/`emitter.settings.rsp` tranches (functions only),
this sidecar also covers **types** - COM interfaces (`MIDL_INTERFACE` declarations)
and property-key constant macros (`DEFINE_PROPERTYKEY`) - alongside free functions.
The same shift-left goal applies: every fact should live as an inline
`_Win32_metadata_supported_os_(<version>)` annotation directly on the producer's
declaration in the owning SDK header.

The rsp originally contained **17,248** API-specific entries. 106 headers already
carried 3,776 pre-existing `_Win32_metadata_supported_os_` annotations from prior
(pre-ralph-loop) migration work.

## Phase 1: Initial reconciliation (prior commit)

A first declaration-anchored scanner (function-only, no COM-interface detection)
found **3,196** entries already fully represented by an existing header annotation
and removed them as pure reconciliation. Residual after phase 1: 14,052 entries.

## Phase 2: Comprehensive scan, migration, and second reconciliation (this tranche)

### Declaration-index scanner rebuild

Extended the declaration-finding scanner with a new pattern for COM interfaces:
`MIDL_INTERFACE("guid")` immediately followed by `NAME : public Base` is recognized
as a real interface declaration (the annotation goes immediately above the
`MIDL_INTERFACE(...)` line, matching the placement convention already used by 106
pre-existing headers). This alone raised the number of locatable names from 12,338
to 15,903 of 17,247 unique names - COM interfaces make up the large majority of the
names this sidecar covers that the RAIIFree/SetLastError tranches never had to deal
with.

### Two new false-positive classes discovered and fixed at this larger scale

1. **Bare call-site pollution for ubiquitous APIs.** Many of the ~17k names here
   (`CoTaskMemFree`, `CoCreateInstance`, etc.) are called from inline helper-function
   bodies throughout hundreds of headers, not just declared once. A naive `NAME(`
   text match treats every call site as a possible declaration location, wildly
   inflating apparent location counts (`CoTaskMemFree`: 59 apparent locations vs. 1
   real declaration). Fixed with two combined checks: (a) an expression-context
   check on the immediate prefix (assignment, ternary, `&&`/`||`/`->`, a `::`-scoped
   call, or a prior statement terminator) using careful operator-boundary regexes
   (learned from a prior tranche's bug where a naive `=` check false-triggered on
   `!=` inside a SAL clause - this tranche's version explicitly excludes `==`, `!=`,
   `<=`, `>=`); (b) for a bare (no-prefix) candidate, requiring the immediately
   preceding non-blank line to *not* end in `;`/`{`/`}` (i.e., to look like a
   continued declaration specifier such as a calling-convention keyword, not a
   completed prior statement or block boundary).
2. **`return !EXPR(...)`/`return !!EXPR(...)` forwarding-shim calls.** The existing
   `return NAME(...)` shim-exclusion regex required "return" to be followed
   immediately by the name; a leading negation (`return !!IsEqualGUID(...)` in
   `guiddef.h`'s `operator==`) slipped through both filters above and produced an
   annotation inserted *inside a function body* (syntactically broken). A
   post-insertion sweep for `_Win32_metadata_supported_os_(...)` immediately
   followed by `return`/`if`/`else`/`{`/`}` found **103** such misplaced insertions
   across 26 files; all were reverted. In every case but one (`IsEqualGUID`, see
   Blocker 3 below) the same name's *real* declaration elsewhere already carried a
   correct annotation, so reverting the bogus duplicate caused no coverage loss.

### Migration

- **12,682 entries removed** from the rsp in this phase: partly newly-migrated
  inline annotations, partly additional pre-existing coverage this tranche's
  more-capable scanner could locate that phase 1's simpler scanner could not
  (chiefly COM interfaces), partly non-representable macros (below).
- **660 headers touched** (a `DEFINE_PROPERTYKEY` macro-invocation sweep - see
  below - added 3 headers not otherwise reached by a real function/interface
  declaration: `audioenginebaseapo.h`, `audioengineextensionapo.h`, `winconp.h`).
- `#if defined(WIN32METADATA) #include <win32metadata_annotations.h> #endif` was
  added to every touched header lacking it (after `<winapifamily.h>` where present;
  manually placed after the file's own include-guard `#define` for 8 headers with
  no `<winapifamily.h>` include).

### `DEFINE_PROPERTYKEY` global consistency pass

`DEFINE_PROPERTYKEY` is a single rsp entry (`windows6.0.6000`) that the scraper's
`--with-attribute` mechanism applies to **every** expansion of the
`DEFINE_PROPERTYKEY(name, ...)` macro across the whole header tree, regardless of
which file invokes it - not just `propkey.h`. Migrating only `propkey.h`'s ~1083
invocations and removing the rsp entry would have silently dropped the attribute
from every other file's property-key constants (a real behavior regression). Found
and annotated the remaining ~510 invocations across 14 more headers
(`functiondiscoverykeys.h`, `functiondiscoverykeys_devpkey.h`, `PortableDevice.h`,
`sensors.h`, `sensorsdef.h`, `mmdeviceapi.h`, `AudioEngineEndpoint.h`,
`audioenginebaseapo.h`, `audioengineextensionapo.h`, `ehstorextensions.h`,
`PhotoAcquireProperties.h`, `syncregistration.h`, `WcnFunctionDiscoveryKeys.h`,
`winconp.h`, `WindowsSideShow.h`, `WpdMtpExtensions.h`, `WpdShellExtension.h`)
before removing the sidecar entry.

## Blockers (documented, left in the rsp)

1. **1,355 names not locatable by the current scanner.** Predominantly COM
   interfaces declared via an idiom other than `MIDL_INTERFACE(...) / NAME : public
   Base` (e.g. template/typedef-based DirectX interfaces), plus `SendMessage`-wrapper
   macros (CommCtrl.h's `Button_*`/`ComboBox_*`/`DateTime_*`/`Edit_*` families) and
   cluster/manifest constant macros (`CLUSCTL_*`, `CLUSPROP_*`). Left in the rsp
   rather than guessed at; needs a follow-up scanner extension (additional interface
   idioms) and per-macro classification (alias vs. genuinely non-representable)
   before further reduction.
2. **14 names with a genuine pre-existing multi-declaration version conflict**
   (`ILClone`, `TraceLoggingWriteActivity`, `MAKELANGID`, `WSAAsyncGetProtoByName`,
   `WSAAsyncGetProtoByNumber`, `WSAAsyncGetServByName`, `WSAAsyncGetServByPort`,
   `WSAEventSelect`, `WSARecv`, `WSARecvDisconnect`, `WSASendDisconnect`,
   `GetAddrInfoExW`, `getnameinfo`, `GetNameInfoW`) - two distinct declarations of
   the same name (e.g. `WinSock2.h` vs. `MSWSock.h`, or two historical overloads)
   already carry two *different* pre-existing `_Win32_metadata_supported_os_`
   values, neither of which was introduced by this tranche. Left as-is rather than
   picking one value without product-owner input on which declaration the rsp's
   recorded version was actually meant for.
3. **`IsEqualGUID` (`guiddef.h`) - Kernel-mode scrape context lacks
   `WIN32METADATA`.** `ntdef.h` and (transitively, via `Kernel/main.cpp`'s direct
   `#include <ntdef.h>`) `guiddef.h` are reached by the `Kernel` partition, whose
   `main.cpp` intentionally omits `windows.h`/`sdkddkver.h` (kernel-mode code has no
   use for the user-mode API surface those pull in). Whatever currently causes
   `WIN32METADATA` to be defined during scraping is reached only through that
   `windows.h`-family inclusion chain; this could not be identified via static
   search of the header tree in the time available for this tranche and needs a
   `sources/Win32MetadataScraper`/build-config-level investigation, not a header
   patch. Confirmed by direct reproduction: adding
   `_Win32_metadata_supported_os_(...)` to `ntdef.h`'s `Int64Shl{l,ra,rl}Mod32` and
   to `guiddef.h`'s `IsEqualGUID` caused `ClangSharpPInvokeGenerator` to fail
   scraping the `Kernel` partition with `use of undeclared identifier 'windows5'`
   (i.e. the macro was never expanded, leaving its raw invocation as unparsable
   source text). Reverted both; `Int64Shl{l,ra,rl}Mod32` also have a *second*,
   user-mode-reachable declaration in `winnt.h` that already carries the correct
   annotation (so those three names were still removed from the rsp - the
   Kernel-only duplicate in `ntdef.h` is accepted as an unannotated, functionally
   inert copy); `IsEqualGUID` has no such alternate reachable declaration, so its
   rsp entry remains, blocked on this infrastructure gap.

## Validation

- Zero adjacent-duplicate `_Win32_metadata_supported_os_` lines and zero
  misplaced-annotation matches (annotation immediately followed by `return`/`if`/
  `else`/`{`/`}`) across all 660 touched headers, checked after every insertion/
  revert pass.
- All 660 consolidated `<header>.metadata.patch` files (581 new, 79 updated;
  0 superseded per-reason patches remained to remove - all had already been
  consolidated by the prior RAIIFree/SetLastError tranches) replay byte-for-byte
  from a pristine `d154186c` checkout of their header (52 files showed a pure
  CRLF/LF line-ending artifact through the checkout/apply round-trip with zero
  content difference once normalized; the committed files use their original,
  correct encoding).
- `ScrapeHeaders -p:ScanArch=crossarch` run across all 240 partitions reachable
  from the 660 touched headers (`AllJoyn` excluded as the pre-existing, unrelated
  `__builtin_verbose_trap` toolchain blocker) in 5 batches of ~48 partitions each -
  all succeeded with 0 errors after the `Kernel`-partition revert described above.

## Residual state

`supportedOS.rsp` now contains **1,370** entries (down from 17,248): 1,355
not-yet-locatable names, 14 genuine pre-existing version conflicts, and 1
Kernel-mode-scrape-context blocker - all documented above rather than silently
dropped. No entries were removed without either (a) confirming an exact-version
match at every real declaration location, (b) confirming a partial match where the
only uncovered "locations" were non-declaration artifacts, or (c) confirming the
name is a pure macro incapable of carrying a declaration-level annotation.
