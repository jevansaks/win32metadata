# One-for-one parameter metadata pilot

This grouped layer follows the WinHttp last-error pilot. It keeps the single
production Clang translation unit, its existing AST walk, PInvokeGenerator's
annotation carriers, and the existing SAL/emission boundary. The shared typed
decoder handles only `RetVal`, `Retained`, `ContainsInteriorPointers`, and an
`AssociatedEnum` use of an existing native enum. Ordinary C/C++ builds see empty
helpers; no Optional, Reserved, ComOutPtr, or count macros are introduced.

## Native owning inputs

| Phase | Owning patch | Facts |
|---|---|---|
| post-MIDL | `combaseapi.h` | Reserved optional pointers, COM output pointers, five existing-enum associations |
| post-MIDL | `objbase.h` | Reserved optional pointers and COM output pointers |
| post-MIDL | `winhttp.h` | Existing last-error moves and `WinHttpQueryHeadersEx` interior-buffer marker |
| post-MIDL | `fileapi.h` | Retained `ReadFileEx`/`WriteFileEx` overlapped storage |
| pre-MIDL | `ObjIdlbase.Idl` | COM output pointers, IStream and IClientSecurity enum associations |
| pre-MIDL | `ObjIdl.Idl` | IMoniker COM output pointers and IStorage enum association |
| pre-MIDL | `dxgi.idl` | IDXGIAdapter GetDesc output-parameter RetVal |

Pre-MIDL changes survive the existing IDL-to-SAL converter and MIDL recompilation.
`ObjIdl.h`, `ObjIdlbase.h`, and `dxgi.h` are reingested outputs, not independent
hand-authored patches. Converted imported IDLs must remain available when
recompiling an owner; otherwise inherited method SAL is lost.
The seven-owner replay classifies against the complete package-original SDK
input tree: ObjIdl, ObjIdlbase, and dxgi are queued MIDL outputs, so their
facts stay in pre-MIDL IDL patches. WinHttp, combaseapi, objbase, and fileapi
are direct-header inputs. Subset replay passes the complete MIDL input root
to the existing guard; missing sibling IDLs cannot bypass source ownership.

For example, `CoGetDefaultContext` moves from `_Outptr_ void** ppv` plus
`CoGetDefaultContext::ppv=[ComOutPtr]` to `_COM_Outptr_ void** ppv` without that
selector. `IStream::Commit` uses
`annotation("_In_ _Win32_AssociatedEnum_(STGC)")`; it associates an existing enum
without replacing the native DWORD storage or constructing an enum.

## Review-only retired selectors

This layer retires these 38 exact `emitter.settings.rsp` selectors.
No wildcard, global type replacement, field, callback, or result-semantics
selector is retired. Complete production emission with these selectors removed
is strictly byte-identical to the legacy-header/legacy-selector output.

```text
CoGetDefaultContext::ppv
IAddrExclusionControl::GetCurrentAddrExclusionList::ppEnumerator
IAsyncManager::GetCallContext::pInterface
IGlobalInterfaceTable::GetInterfaceFromGlobal::ppv
IInternalUnknown::QueryInternalInterface::ppv
IMarshal::UnmarshalInterface::ppv
IRpcChannelBuffer3::GetCallContext::pInterface
CreateDataCache::ppv
BindMoniker::ppvResult
CoGetObject::ppv
CoGetObjectContext::ppv
CoGetClassObject::ppv
CoGetCallContext::ppInterface
CoGetCancelObject::ppUnk
IMoniker::BindToObject::ppvResult
IMoniker::BindToStorage::ppvObj
CoAllowSetForegroundWindow::lpvReserved
CoDisableCallCancellation::pReserved
CoEnableCallCancellation::pReserved
CoInitialize::pvReserved
CoInitializeEx::pvReserved
CoInitializeSecurity::pReserved1
CoInitializeSecurity::pReserved3
CoGetClassObject::dwClsContext
CoInitializeEx::dwCoInit
CoInitializeSecurity::dwCapabilities
CoRegisterClassObject::flags
CoSetProxyBlanket::dwCapabilities
IClientSecurity::QueryBlanket::pCapabilites
IClientSecurity::SetBlanket::dwCapabilities
IStorage::MoveElementTo::grfFlags
IStream::Commit::grfCommitFlags
IStream::LockRegion::dwLockType
IStream::Stat::grfStatFlag
IDXGIAdapter::GetDesc::pDesc
ReadFileEx::lpOverlapped
WriteFileEx::lpOverlapped
WinHttpQueryHeadersEx::pBuffer
```

## Exact coverage and boundaries

Native SAL supplies In/Out/Inout, Optional, Reserved, ComOutPtr, constness,
element counts, byte capacities, and parameter double-NUL termination. Existing
SDK authoring and the applicable SAL defaults remain authoritative. There is
no universal scalar-In or unannotated-pointer-Inout rule and no blanket
native direction rewrite. `ReadFileEx`/`WriteFileEx` already had Inout and
only gained Retained in this pilot; it made no native direction edits to
`ADsBuildVarArrayInt`, `NdrClientCall2`, or `UCharIterator`. Fileapi's
`GetVolumePathNamesForVolumeNameW` already carries parameter double-NUL SAL.
`_Out_writes_bytes_to_` preserves its byte-capacity parameter; this layer does
not claim a separately decoded initialized extent.

Fixtures run real headers through the production scraper and final emitter.
They decode direction/optional flags, pointer depth and native widths,
attribute blobs and count units, COM methods, return association at parameter
sequence zero, matching legacy/header transport, absence, and native diagnostics
on all three targets. They do not certify unselected SDK declarations.
Owned facts must be on the first native declaration: a minimized late-only
parameter annotation is not inherited by PInvokeGenerator and now fails with
a source diagnostic instead of disappearing. Repeated/transitive inclusion and
redeclarations whose first declaration carries the fact remain supported.

The following reviewed native facts are not unchanged-output migrations:

| Retained record/case | Exact reason |
|---|---|
| `CreateStreamOnHGlobal::hGlobal=[Optional]` | `_In_opt_` additionally emits In; the old Optional-only flags suppress the implicit In default |
| CoDisconnectObject/CoFreeUnusedLibrariesEx scalar Reserved selectors | `_Reserved_` additionally emits Optional; retain the existing scalar In and Reserved output rather than hide that correction |
| `IAgileReference::Resolve::ppvObjectReference=[ComOutPtr]` | Its hand-written C++ `[retval]` comment also needs RetVal; the existing metadata omits it |
| `EnumPrinterDriversW::pDriverInfo` | Shipped `_Out_writes_bytes_opt_(cbBuf)` buffer really contains interior pointers, but no old selector exists; adding the marker is an additive semantic change, not a move |

Callback/scalar-typedef recovery (including LPFNGETCLASSOBJECT and
FARPROC/NEARPROC/PROC), native type-changing enum replacements, enum construction
and flags, namespaces, ownership/fictional handles, availability, import
overrides, and result-semantics consolidation remain separate mechanisms.
Field-array projection and StructSizeField are not parameter/return claims.
The subsequent SAL/MIDL reader layer recovers structured native `[retval]`
comments on exact method and callback parameters, including generated C-vtable
slots. It also reads double-NUL SAL on returns, fields and native typedef uses.
`IDXGIAdapter::GetDesc` now authors `[out, retval]` in the authoritative
pre-MIDL `dxgi.idl` owner rather than using a custom annotation that replaces
generated SAL. `_Win32_Retval_` remains the C/C++ fallback for declarations
without an authoritative native marker; its payload is canonical `RetVal`.

Partial counts fit the existing final schema only: element capacity
`capacity` supplies `NativeArrayInfo.CountParamIndex`, a constant element count
supplies `CountConst`, and byte capacity `byteCapacity` supplies
`MemorySize.BytesParamIndex`. Native initialized expressions such as `*written`
and `*byteWritten` remain in the native preservation proof; there is no
independently represented initialized extent, new schema field or helper
vocabulary. Field-relative array projection/`CountFieldName` remains deferred.

The prepared reader layer is not an unchanged-output SDK migration:
authoritative native retval comments and raw RPC/local SAL can expose missing
facts. Its bounded follow-up uses production `/sal /sal_local` and actual
MIDL outputs for the authorized 32 headers plus the existing DXGI owner.
The original complete generated-header migration proofs remain explicit
historical tests, deriving their snapshot from the unique reachable cutover
introduction's parent rather than a pinned SHA. Native IDL, header-only
owners and current selected-site/absence assertions stay live, and ordinary
SDK CI strictly byte-compares every current generated owner with production
MIDL. The old red reports and native inputs remain evidence, not waived guards.
Notably, Audioclient's existing native `[out]`
on `GetAudioEffects::numEffects` changes final In|Out to Out only on the actual
parent; the difference is removal of In, not merely addition of Out. Local
MIDL default-input and optionality facts also cause measured input corrections
in D3D12, WMSDK, DirectShow and Media Foundation. The Imaging fixture measures
six native `riid` parameters changing In|Out to In and retains full final-byte
comparison against the explicitly corrected expected output. WMSDK's raw
`__RPC__in_ecount_full(cbLicenseRequestMsg)` supplies the existing element
`CountParamIndex = 2`, not a byte count or independent initialized extent.
These effects require
explicit review; the retained focused proofs do not approve them or substitute
for ordinary production qualification. See the prepared-layer boundary in
[`sdk_patches.md`](sdk_patches.md).

Validation uses the normally generated CI candidate and an already-built target
CI/main/release winmd. This layer does not require additional full-SDK scrapes,
paired generation, performance budgets, or generation receipts. Relevant native
header/ABI/byte fixtures and ordinary CI remain required.

## Archived local production evidence before predecessor sync

Full cross-architecture generation with candidate tools produced identical
complete winmd files for legacy headers/legacy selectors and patched
headers/matching selectors. Emission in a fresh output root using the complete
patched generated sources and all 38 selectors actually removed also passed
strict `fc.exe /b` equality against that legacy output. All three files have
SHA256 `6A85E41B5F09C976CE049C3B020FA2C30C01E4A2B553C774E06F540A74D876E2`.
The header-only emission took 272.362 seconds.

These archived local preflights are not certification of a later head and are
not instructions to repeat full-SDK generation or benchmark runs.
The final synced tooling run passed 195 tests; its header/emitter suite took
79 seconds, so the 30-second warm tooling target is not claimed as achieved.
The final targeted header/SDK replay run passed 84 tests. A production-integrity
test also verifies all 38 selected parameters, their exact flags, singleton
attribute blobs, and existing-enum associations in the complete header-only
winmd. A fresh selected-owner scrape with the final canonical-declaration guard
passed in 30.615 seconds: five normal common-lane partitions and WinHttp on
x86, x64, and arm64. These selected-owner results do not replace full hosted CI.

The test project explicitly declares the same pinned SDK package used by the
production build, with a generated path property and no imported build/runtime
assets. Its shared SDK-path helper reuses the existing repository-root locator
and NuGet-property reader. SDK replay therefore needs only the test project's
restore, not aggregate `BuildTools.proj` properties or a workflow setup
workaround. The SDK lane remains parallel and selective; native replay and ABI
assertions are independent of the removed receipt machinery.
With aggregate NuGet properties deliberately absent, the parent tooling suites
passed 195 tests and the separate SDK replay suite passed all 24 tests. The
header/emitter suite took 79 seconds and SDK replay took 36 seconds.

After replaying this layer onto PR5
`02c2674df78907d149cada750810385d8ff38651`, local tooling passed 199 tests
(132 header/emitter tests in 92 seconds) and SDK replay passed all 24 tests in
35 seconds. All 38 selector deletions, seven owning patches, shared SDK
fixture, and native byte/ABI checks remain intact. Those test results precede
the comparison simplification; ordinary exact-head CI and the already-built
target comparison must be followed independently.

After replay onto simplified PR5
`2b1d46349bc652b2d91a3f6842c23fe5e92d964e`, tooling passed 164 tests
(97 header/emitter tests in 72 seconds) and SDK replay passed all 24 tests in
36 seconds. The measurement runner and its hooks remain removed. The 38
selector deletions, seven owning patches, parameter capability sources, and
native/ABI fixtures are unchanged from the previous layer. The inherited
post-MIDL patch wildcard is preserved, with the three pre-MIDL owners added.
Ordinary exact-head CI and the already-built target comparison remain separate
from these local fixture results.

The source-classification sync onto PR5
`cda1fa94b96013ba4a130826f8efac2dd70c7efc` passed tooling 164/164 and
SDK replay 33/33. The seven-owner replay uses the complete SDK MIDL input
tree for both phase guards and explicitly checks the three generated owners
and four direct headers. The private data-only SDK dependency is inherited
once from the predecessor. No production annotations, selector deletions,
owning patch bytes, native fixtures, or documented semantic gaps changed.
