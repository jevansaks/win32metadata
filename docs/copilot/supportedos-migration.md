# supportedOS.rsp Sidecar-Removal Tranche

## Summary

`generation/WinSDK/supportedOS.rsp` is a `--with-attribute` sidecar: one line per
declared name mapping to a C# `[SupportedOSPlatform("windowsX.Y.ZZZZ")]` attribute.
Unlike the `WithSetLastError.rsp`/`emitter.settings.rsp` tranches (functions only),
this sidecar also covers **types** - COM interfaces (declared via several distinct
idioms - see Phase 3 below) and property-key constant macros (`DEFINE_PROPERTYKEY`)
- alongside free functions. The same shift-left goal applies: every fact should
live as an inline `_Win32_metadata_supported_os_(<version>)` annotation directly
on the producer's declaration in the owning SDK header.

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

## Phase 3: Fixing the WIN32METADATA/Kernel gap, expanding COM idiom detection,
## resolving all version conflicts, and dead-entry classification

### `WIN32METADATA` never defined anywhere - the real root cause of the Kernel gap

Root-caused the Phase 2 `Kernel`-partition regression: `WIN32METADATA` is **never
defined** by the scraper via any `-D`/`--additional` flag in any rsp (exhaustive
case-sensitive search across every `.rsp` in the repo confirms this). Every header
using `_Win32_metadata_*` macros only works because `win32metadata_annotations.h`
defines them as harmless no-ops when `WIN32METADATA`/`__clang__` aren't both set -
**provided the header is reachable at all**. `minwindef.h`/`windef.h` already
included it **unconditionally** (no `#if defined(WIN32METADATA)` guard around the
`#include`), which is why every `windows.h`-chain (user-mode) partition worked:
they reach `win32metadata_annotations.h` transitively through `minwindef.h`
regardless of the (always-false) `WIN32METADATA` condition. `Kernel/main.cpp`
intentionally omits the whole `windows.h` chain (kernel-mode code has no use for
it), so it never reached the guard's `#include` and the macros were left
completely undefined, causing the raw annotation text to fail to parse.

**Fix:** made `#include <win32metadata_annotations.h>` unconditional everywhere,
matching the existing `minwindef.h`/`windef.h` precedent - removed the
`#if defined(WIN32METADATA) ... #endif` wrapper from all 779 occurrences of the
guard across the header tree (safe: the file is `#pragma once`-guarded and its
own internal `#if defined(WIN32METADATA) && defined(__clang__)` still gates the
*real* attribute macro; only the *reachability* of a no-op fallback definition
changes). Verified with `ScrapeHeaders -p:PartitionFilter=Kernel` before and
after. Re-added the `IsEqualGUID` (`guiddef.h`) and `Int64ShllMod32`/
`Int64ShraMod32`/`Int64ShrlMod32` (`ntdef.h`, both the prototype and `__inline`
bodies) annotations that had been reverted in Phase 2; all now scrape
successfully via `Kernel` (and `TransactionServer` for `guiddef.h`).

### New false-positive classes found and fixed

- **Identifiers inside string literals.** `_WINSOCK_DEPRECATED_BY("WSARecv()")`-style
  deprecation hints matched as if `WSARecv(` were a real call/declaration site.
  Fixed by blanking double-quoted string-literal contents before pattern matching.
- **`return <cast>(NAME(...))` forwarding/alias calls**, e.g.
  `return reinterpret_cast<PIDLIST_ABSOLUTE>(ILClone(pidl))` inside `ILCloneFull`'s
  inline body - misattributed `ILClone`'s annotation onto `ILCloneFull`'s
  declaration line since the original "does the prefix look like an expression"
  check computed the wrong string span (an off-by-one in how the delimiter
  character captured by the scanning regex was excluded from the prefix).
  Fixed the offset and broadened the "`return` leading into a call" exclusion to
  cover cast-wrapped calls, while keeping it narrow enough to not exclude genuine
  SAL clauses like `_Success_(return != NULL)` on a real declaration line.
- **Multi-line call-argument lists with trailing commas** (e.g. `MAKELANGID(...)`
  as one argument deep inside a multi-line `FormatMessageA(...)` call) look like a
  continued declaration specifier to the "does this look like a declaration
  context" heuristic (no line ends in `;`/`{`/`}` until the enclosing call
  statement). Found and fixed by direct inspection of the two affected locations
  (`WS2tcpip.h` `gai_strerrorA`/`gai_strerrorW`); not generalized into the scanner
  given its rarity, but documented here for future scanner work.
- **Trailing line-comments hiding a real statement/block terminator**, e.g.
  `} // extern C++` not being recognized as ending in `}` because the comment
  came after it - caused two insertions to land after a stray `}` instead of
  immediately before the real declaration (`Unknwn.h`/`Unknwnbase.h`
  `IUnknown_QueryInterface_Proxy`). Fixed by stripping trailing `//` comments
  before checking for a terminator in all three
  declaration-context/coverage-range walkers.
- **Overly broad `STDMETHODCALLTYPE` exclusion.** The exclusion (originally meant
  to skip COM vtable function-pointer slots and virtual interface methods) was
  skipping *any* line mentioning `STDMETHODCALLTYPE`, including genuine free
  functions that use it as a plain calling-convention keyword (e.g.
  `HRESULT STDMETHODCALLTYPE WMCreateReader(...)`, `ULONG STDMETHODCALLTYPE
  IUnknown_AddRef_Proxy(...)`). Narrowed to specifically match the vtable
  function-pointer shape (`( STDMETHODCALLTYPE *Name )`) and the `virtual ...
  STDMETHODCALLTYPE` interface-method shape, leaving plain free-function
  declarations detectable.
- A full post-fix audit cross-referencing every `_Win32_metadata_supported_os_`
  annotation in every touched header against the original 17,248-entry list
  (by name and version) found and fixed all remaining misplacements described
  above; the remaining "no real declaration found" audit residue was two
  legitimate idioms the audit's own quick parser didn't recognize (C-style
  vtable-only interface fallback bodies, and `interface NAME : public Base`
  without a preceding `MIDL_INTERFACE`), verified correct by direct inspection.

### New COM/declaration idioms detected

- `DECLARE_INTERFACE_IID_(NAME, Base, "guid")` / `DECLARE_INTERFACE_IID(NAME, "guid")`
  / `DECLARE_INTERFACE_(NAME, Base)` / `DECLARE_INTERFACE(NAME)` - the pre-MIDL
  C-vtable interface-declaration macro family (`ShlObj.h`/`ShlObj_core.h` etc.).
- `#define INTERFACE NAME` followed a few lines later by a `DECLARE_INTERFACE...`
  invocation referencing the literal token `INTERFACE` (older shell/COM headers)
  - annotation goes on the `#define INTERFACE NAME` line itself.
- `interface <TAG>_DECLARE_INTERFACE("guid") NAME : public Base` - a single-line
  DirectX/DirectWrite interface idiom (`DX_DECLARE_INTERFACE` in `d2d1*.h`,
  `DWRITE_DECLARE_INTERFACE` in `dwrite*.h`), generalized to match any
  `*_DECLARE_INTERFACE` macro name.
- `typedef struct <tag> NAME;` plain typedef-to-a-real-type alias (e.g.
  `typedef struct fd_set FD_SET;` in `winsock.h`) - used only when `NAME` (not
  the lowercase tag) is itself the rsp-tracked public name with no other
  reachable declaration.
- A curated allowlist of "ABI-level" `winrt/` headers (`activation.h`,
  `activationregistration.h`, `hstring.h`, `inspectable.h`, `roapi.h`,
  `roerrorapi.h`, `winstring.h`) was included in the scan, narrowly reversing the
  blanket `winrt/` exclusion for just these free-function/base-interface headers
  (the exclusion's original purpose - avoiding COM/WinRT *projection* method-name
  collisions such as `GetClassName`/`SendMessage` - does not apply to these
  foundational, non-projection ABI headers).

### Dead/vestigial sidecar entries (removed, no header change)

Cross-referenced every remaining name against the full generated C# output of a
complete `ScrapeHeaders` run across all 323 partitions (`AllJoyn`/`WinRT.AllJoyn`
excluded as the pre-existing toolchain blocker) using exact-identifier matching
(not substring). **952 entries** never correspond to any currently-generated
metadata member and were removed with no header change:
- **935 confirmed `#define` function-like/object-like macros** (the
  `Button_*`/`ComboBox_*`/`DateTime_*`/`Edit_*`/`Animate_*` SendMessage-wrapper
  families in `CommCtrl.h`, `CLUSCTL_*`/`CLUSPROP_*`/`CLUSTER_*` cluster
  control-code macros in `ClusApi.h`, and similar) - win32metadata does not
  project function-like macros as P/Invoke members at all (verified: none of
  these names appear anywhere in the generated output, not even as a
  `--with-attribute`-driven synthetic member), so the sidecar entries were
  already permanently inert before this tranche even began.
- **13 names absent from every header in the current SDK tree entirely**
  (`CIBuildQueryNode`/`CIBuildQueryTree`/`CICreateCommand`/`CIMakeICommand`/
  `CIRestrictionToFullTree`/`CITextToFullTree{,Ex}`/`CITextToSelectTree{,Ex}`/
  `LocateCatalogsA`/`LocateCatalogsW` - old Content Index/Indexing Service query
  APIs documented against `ntquery.h` per `scripts/ApiInfo.csv`, and
  `IRDPViewerRenderingSurface`/`IWRdsRemoteFXGraphicsConnection` - RDP interfaces
  documented against `rdpencomapi.h`/`wtsprotocol.h`). Both source headers still
  exist in the current SDK snapshot but no longer declare these specific names -
  they were removed from the public SDK surface at some point before the
  19041-based snapshot this repo scrapes. No verified original signature was
  available to safely fabricate a "guarded metadata-only legacy" declaration
  without risking an incorrect shape, so these were documented and removed
  rather than guessed at.
- **2 names (`GetActivationFactory`, `RoGetActivatableClassRegistration`) exist
  only as C++/WinRT projection helpers under `winrt/wrl/`** (not the ABI-level
  headers allowlisted above) - out of scope for a Win32 ABI metadata surface.

### Duplicate-declaration version conflicts (all resolved, none left in the rsp)

All 14 names with two real, distinct declarations carrying different
pre-existing `_Win32_metadata_supported_os_` values were resolved by selecting
the authoritative value and normalizing the other declaration in place (not by
retaining a sidecar entry):
- `WSAAsyncGetProtoByName`, `WSAAsyncGetProtoByNumber`, `WSAAsyncGetServByName`,
  `WSAAsyncGetServByPort`: declared in both legacy `winsock.h` (`windows5.0`) and
  `WinSock2.h` (previously `windows8.1`, next to a `_WINSOCK_DEPRECATED_BY(...)`
  marker). The deprecation marker reflects when the replacement API was
  recommended, not when the function itself was introduced - Winsock async
  helpers have existed since the original Windows Sockets 1.1 surface. Normalized
  `WinSock2.h`'s value to `windows5.0` to match the canonical `winsock.h`
  declaration.
- `WSARecvDisconnect`, `WSASendDisconnect`: single declaration in `WinSock2.h`,
  incorrectly annotated `windows8.1` (again next to a `_WINSOCK_DEPRECATED_BY`
  marker) for a core WinSock2 overlapped-I/O function that has existed since
  WinSock2's introduction. Normalized to `windows5.0` to match the rsp.
- `ILClone`, `WSAEventSelect`, `WSARecv`, `GetAddrInfoExW`, `getnameinfo`,
  `GetNameInfoW`, `MAKELANGID`, `TraceLoggingWriteActivity`: all were **false
  conflicts** caused by the false-positive classes described above (bogus
  locations inside string literals, cast-wrapped forwarding calls, or - for
  `TraceLoggingWriteActivity`, a pure macro - a misplaced annotation inside an
  unrelated template function's body). No header content needed to change beyond
  removing the bogus insertion; the real declaration(s) already carried (or, for
  `TraceLoggingWriteActivity`, could never carry) the correct value.

## Phase 4: Closing the final 50 entries to zero

Added six more declaration idioms/fixes to reach the last 50 names:
- **`DECLARE_MAPI_INTERFACE_`/`DECLARE_MAPI_INTERFACE_IID_`** - a MAPI-specific
  variant of the `DECLARE_INTERFACE` macro family (`WabApi.h`'s `IWABExtInit`,
  `IWABObject`; `WabIab.h`'s `IABContainer`/`IDistList`/`IMailUser`/
  `IMAPITable`/`IAddrBook`).
- **Bare `interface NAME : [public] Base`** with no preceding macro at all
  (`amxmlgraphbuilder.h`'s `IXMLGraphBuilder : IUnknown` - no `public` keyword;
  `strmif.h`'s `IAMFilterGraphCallback : public IUnknown`).
- **`interface DECLSPEC_UUID("guid") NAME : public Base`** on a single line
  (`TextServ.h`'s `IRicheditWindowlessAccessibility`, `IRichEditUiaInformation`)
  and its two-line variant with an extra `DECLSPEC_NOVTABLE` qualifier before
  the interface name wraps to the next line (`RTWorkQ.h`'s
  `IRtwqAsyncCallback`/`IRtwqAsyncResult`/`IRtwqPlatformEvents` - handled by
  direct inspection given only 3 instances, all in one file).
- **`MIDL_INTERFACE("guid") NAME` with no base class at all** - the `IUnknown`
  root interface itself (`Unknwn.h`/`Unknwnbase.h`), which every other
  interface pattern excluded because they all require a `: public Base` clause.
- Extended the "ABI-level `winrt/` allowlist" with 9 more foundational headers
  reachable from partitions already in scope
  (`rdpappcontainerclient.h`/`rometadata.h`/`roregistrationapi.h`/`shcore.h`/
  `webapplication.h`/`roparameterizediid.h`/`weakreference.h` - case-sensitive
  filename matching bug fixed along the way - plus `winstringmarshal.h`/
  `robuffer.h`/`windowsstring.h`/`roerror.h` retained from phase 3 but unused
  this phase).
- **Dead/vestigial macros removed** (verified absent from all generated output
  using exact-identifier matching, coincidental substring/field-name matches in
  generated code ruled out by manual inspection): `MAKE_HRESULT`, `SelectFont`,
  `RGB`, `MAKELCID`.
- **`IAppxPackageReader2`** - like the 13 Content Index/RDP names in phase 3,
  absent from every header in the current SDK tree entirely (documented against
  `appxpackaging.h`/`appxpkg` per `scripts/ApiInfo.csv`, but that header no
  longer declares it in this snapshot). Removed with no substitute rather than
  fabricating a signature.
- **`DWRITE_MAKE_OPENTYPE_TAG`** - unlike every other name in this tranche, this
  one is implemented as **hand-written C#**, not scraped from a header
  (`generation/WinSDK/manual/DirectWrite.cs`, a `static uint` helper method
  wrapping the OpenType tag-packing formula). Applied the fact as a
  `[SupportedOSPlatform("windows6.1")]` attribute directly on that method,
  matching the existing precedent in `generation/WinSDK/manual/
  VirtualDiskService.cs` for other hand-written members needing the same
  metadata.

## Residual state (final)

**`supportedOS.rsp` reached zero API-specific entries** (down from 17,248
originally) and, per the required end state, has been **deleted** along with
its `<ScraperRsp Include="supportedOS.rsp"/>` reference in
`generation/WinSDK/Windows.Win32.proj` and its Solution Items entry in
`sources/BuildTools.sln`. `docs/architecture.md` and `docs/spec.md` were
updated to stop referencing the removed sidecar. The unrelated audit/
regeneration helper script `scripts/CreateSupportedOSPlatformRsp.ps1` was left
untouched (not invoked by the build, out of scope for this tranche, same
treatment as `scripts/CreateWithSetLastError.ps1` in the prior tranche).

Final accounting for the full `supportedOS.rsp` tranche (all four phases):
17,248 original entries → 3,196 + 165 + 58 + 1 + 5 + 22 (phase 2/3 batches,
already covered) = 3,447 reconciled with no header change, 952 dead/vestigial
macros and constants removed with no header change, 13 + 1 = 14 names
documented as absent from the current SDK tree and removed with no header
change (no fabricated signatures), 14 duplicate-declaration version conflicts
resolved by normalizing the authoritative header value, 1 name
(`DWRITE_MAKE_OPENTYPE_TAG`) resolved via a hand-written-C# attribute instead
of a header annotation, and the remainder (≈12,624 entries) migrated to inline
`_Win32_metadata_supported_os_(version)` annotations across several hundred
headers. `WithSetLastError.rsp` and `supportedOS.rsp` are both now fully
removed from the sidecar system; `emitter.settings.rsp`'s RAIIFree category
was completed in an earlier tranche.
