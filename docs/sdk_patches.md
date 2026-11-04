# SDK header patches

## Media Foundation COM outputs

This layer audits all **144** parent `ComOutPtr` records against every shipped
owner in the selected `Mf` traversal. It retires **30 complete records**,
leaving **114** active records. All 28 MF/IMF factory, service, resource and
reference leads are covered, together with the two video-service outputs
physically owned by the selected `dxva2api` input. There are no new partial
records or exact-byte boundaries in this family.

| Owning native input | Migrated existing output facts |
|---|---|
| `mfapi.h` | `MFCreateAttributes::ppMFAttributes`, `MFCreateMediaExtensionActivate::ppvObject`, `MFCreateVideoSampleAllocatorEx::ppSampleAllocator` |
| `mfcaptureengine.idl` | `IMFCaptureEngineClassFactory::CreateInstance::ppvObject` |
| `mfsharingengine.idl` | `IMFImageSharingEngineClassFactory::CreateInstanceFromUDN::ppEngine` |
| `mfreadwrite.idl` | `IMFReadWriteClassFactory::CreateInstanceFromURL::ppvObject`, `IMFReadWriteClassFactory::CreateInstanceFromObject::ppvObject`, `IMFSinkWriter::GetServiceForStream::ppvObject`, `IMFSourceReader::GetServiceForStream::ppvObject` |
| `Mfobjects.idl` | `IMFDXGIBuffer::GetResource::ppvObject`, `IMFDXGIBuffer::GetUnknown::ppvObject`, `IMFDXGIDeviceManager::GetVideoService::ppService` |
| `Mfidl.idl` | `IMFInputTrustAuthority::GetDecrypter::ppv`, `IMFObjectReferenceStream::LoadReference::ppv`, `IMFRemoteProxy::GetRemoteHost::ppv`, `IMFRemoteProxy::GetRemoteObject::ppv`, `MFCreateNetSchemePlugin::ppvHandler`, `MFCreatePropertiesFromMediaType::ppv`, `MFCreateStreamOnMFByteStreamEx::ppv`, `MFGetService::ppvObject` |
| `mfmediaengine.idl` | `IMFMediaEngineClassFactory4::CreateContentDecryptionModuleFactory::ppvObject` |
| `mfd3d12.idl` | `MFCreateD3D12SynchronizationObject::ppvSyncObject` |
| `evr.idl` | `MFCreateVideoMixer::ppv`, both `MFCreateVideoMixerAndPresenter` outputs, `MFCreateVideoPresenter::ppVideoPresenter`, `MFCreateVideoRenderer::ppVideoRenderer`, `MFCreateVideoSampleAllocator::ppSampleAllocator` |
| `dxva2api.idl` | `DXVA2CreateVideoService::ppService`, `IDirect3DDeviceManager9::GetVideoService::ppService` |

Nine per-file pre-MIDL patches update authoritative IDL annotations or
`cpp_quote` SAL. The one post-MIDL patch updates the genuinely header-only
`mfapi.h`; the selected SDK ships no `mfapi.idl`. Normal conversion and MIDL
regenerate the corresponding C++ methods, C vtables and inherited
`IMFSourceReaderEx` / `IMFSinkWriterEx` service entries. Native declarations,
including local methods, ordinary and nullable outputs, export macros and
version guards, keep their ABI. `MFCreateAttributes` and the image-sharing
factory keep their native typed outputs, not `void**`.

`MediaFoundationComOutputHeaderTests` extracts the actual original and patched
declarations and compares legacy-sidecar versus header-only final winmd bytes
on x86, x64 and arm64. It checks final output attributes, flags, pointer depth,
typed targets and retval absence; adding redundant sidecars also stays
byte-identical. Its inventory includes every selected shipped traversal owner,
not merely MF-prefixed names. Original inputs come from the reachable parent
of this fixture's introduction, resolved from branch history rather than a
pinned rebasable commit. That parent remains ancestry-guarded. Original owner
bytes must equal test-owned SDK `10.0.28000.2705` inputs before replay.
The byte-stream helper preserves the legacy `0x92` comment byte in
`mfd3d12.idl`; line endings, trailing blank lines and unrelated native bytes
are unchanged. Native diff checks allow only the selected SAL/annotation edits.

`SdkPatchTests` retains the complete test-owned SDK/D3D source-ownership inventory
as `MidlInputRoot`, including unqueued and imported IDL inputs; the selected
patch set does not infer ownership from a partial fixture directory.
It applies the actual selected patches, converts the bounded
owner/import set, runs production MIDL options, compares complete generated
owner bytes and passes the replayed declarations through final winmd for
all three architectures. Native C/C++ fixtures use the same paths for original,
patched and `WIN32METADATA` builds on x86/x64/arm64 in both include orders,
call every selected factory/method and the inherited service variants, and
compare full object bytes except the COFF timestamp. Existing parent no-op
fixtures and independent availability, result, import and SAL facts remain
in place.

DirectShow, WIC, Controls, HLink, automation, storage and unrelated UI are not
part of this family. No removed SDK header/API is restored. The DWrite
`void**` representation override, five graphics callback records and
`IPresentationManager::GetPresentRetiringFence` custom-attribute row-order
boundary remain active and unchanged. The presentation boundary is not a
semantic retval change. Production `CanReturn*` sidecars remain active and all
17 proposed PreserveResult replacements stay inactive. No global attribute
canonicalization or new production annotation mechanism is introduced.
Ordinary CI still builds one candidate and compares against an already-produced
exact-target winmd, with the existing explicitly disclosed main/release fallback;
this layer adds no full-SDK generation or comparison workflow.

## Graphics COM factory and resource outputs

This layer audits all 169 parent `ComOutPtr` records against the actual shipped
traversal owners for Direct2D, Direct3D 9/10/11/12 and interop, FXC, DXC,
DirectWrite, DXGI, DirectComposition, presentation and WinRT composition.
It finds 31 graphics-owned sites. It retires **25 attribute facts: 24 complete
records and one partial DWrite record**, leaving 144 active `ComOutPtr`
records. The remaining five graphics callbacks are a separate mechanism;
one presentation method is an exact-byte boundary described below.
The already absent `dciddi.h` traversal entry is not restored.

| Owning native input | Migrated existing output facts |
|---|---|
| `d2d1.h` | `D2D1CreateFactory::ppIFactory`, `ID2D1Resource::GetFactory::factory` |
| `d3d10.idl` | `ID3D10Device::OpenSharedResource::ppResource` |
| `d3d12.idl` (D3D12 package `1.619.3`) | `D3D12CreateRootSignatureDeserializer::ppRootSignatureDeserializer`, `D3D12CreateVersionedRootSignatureDeserializer::ppRootSignatureDeserializer` |
| `d3dcompiler.h` | `D3DReflect::ppReflector`, `D3DReflectLibrary::ppReflector` |
| `d3d9on12.h` | `IDirect3DDevice9On12::GetD3D12Device::ppvDevice`, `IDirect3DDevice9On12::UnwrapUnderlyingResource::ppvResource12` |
| `shared/d3d9.h` | `IDirect3DSurface9::GetContainer::ppContainer`, `IDirect3DVolume9::GetContainer::ppContainer` |
| `dcomp.h` | `DCompositionCreateDevice::dcompositionDevice`, `DCompositionCreateDevice2::dcompositionDevice`, `DCompositionCreateDevice3::dcompositionDevice`, `IDCompositionTexture::GetAvailableFence::availableFence`, `IDCompositionSurface::BeginDraw::updateObject` |
| `Presentation.idl` | `CreatePresentationFactory::presentationFactory` |
| `dxcapi.h` | `DxcCreateInstance::ppv`, `DxcCreateInstance2::ppv`, `IDxcContainerReflection::GetPartReflection::ppvObject`, `IDxcUtils::CreateReflection::ppvReflection`, `IDxcCompiler3::Compile::ppResult`, `IDxcCompiler3::Disassemble::ppResult` |
| `winrt/windows.ui.composition.interop.h` | `ICompositionTextureInterop::GetAvailableFence::availableFence` |
| `dwrite.h` (already authoritative `_COM_Outptr_`, no native edit) | `DWriteCreateFactory::factory` attribute only |

Only standard `_COM_Outptr_` SAL variants are used. MIDL-backed owners are patched
before conversion and compilation; genuinely header-only owners are patched
after MIDL. The generated `d3d10_1.h` inherited C-vtable copy is updated by
recompiling its unchanged IDL against the patched `d3d10.idl`. Native version
guards, pointer depths, optional storage and nullable results are retained.
The D2D C++ convenience overload and both templates remain unchanged: they have
different output selectors and no independently configured facts. No proxy,
forward declaration or duplicate complete definition gains a new API fact.

`DWriteCreateFactory` returns native `IUnknown**`. Its existing combined
`[ComOutPtr]void**` record encodes two independent facts. The already-present
native SAL supplies the attribute, but not the representation override.
The record therefore becomes exactly `DWriteCreateFactory::factory=void**`;
the native declaration stays `IUnknown**`. Final-winmd fixtures prove that
removing the type override would change the output. `ID2D1Resource::GetFactory`
stays typed `ID2D1Factory**`; adding COM-output SAL does not erase its type.

`IPresentationManager::GetPresentRetiringFence::fence` remains unchanged in
both native IDL/header and sidecar. Replacing its `_Out_retval_` with
`_COM_Outptr_retval_` preserves the custom-attribute values, parameter flags,
method/implementation flags and projected API shape, but reverses the
`RetValAttribute` / `ComOutPtrAttribute` metadata row order. That is an
**exact-byte custom-attribute row-order boundary**, not a semantic attribute
or retval change. The small final-winmd fixture proves this distinction on
all three architectures. No global attribute canonicalization is introduced.
The three `PFN_D3D12_*DESERIALIZER*` records and two `DxcCreateInstance*Proc`
records remain active and their native callback declarations are unchanged.
Media, WIC, Controls image lists, hyperlinks and unrelated UI remain out of
this family. Existing `CanReturn*` sidecars remain active; all 17
non-equivalent PreserveResult replacements remain inactive.

`DirectXComOutputHeaderTests` compares actual original native declarations plus
legacy facts with the patched declarations plus only the independent DWrite
override and unchanged presentation boundary. It checks final custom-attribute
payloads, direction, optionality, retval, pointer depth/type and complete winmd
bytes on x86, x64 and arm64. Original fixtures resolve the parent of the unique
reachable introduction of `DirectXComOutputHeaderTests.cs`, with an ancestry
guard and exact 169-record legacy corpus. No mutable or obsolete commit is
pinned. Selected original inputs must match the test-owned SDK/D3D12 packages.
`SdkPatchTests` applies the actual per-file patches, runs normal conversion and
production MIDL options, compares complete regenerated owner bytes, and replays
them through final winmd. Its full MIDL input root includes every SDK `um`/`shared`
input and the production D3D12 package overlay; source ownership is not inferred
from the small set of copied patch targets. Every selected generated owner must
be queued by the production MIDL inventory. Native C/C++ fixtures use the same
paths for original, patched and `WIN32METADATA` builds on all three architectures and both applicable
include orders, comparing complete object bytes apart from the COFF timestamp.
C++-only DirectWrite, DirectComposition, DXC, namespaced composition interop
and the D2D resource interface are exercised only in C++; the C fixture covers
every C-compatible selected declaration and inherited vtable. Existing parent
native fixtures are unchanged.
Ordinary CI still makes one candidate and compares against an already-produced
exact-target winmd, with the existing explicitly disclosed main/release fallback.
No new full-SDK generation or comparison workflow is added.

## Remaining shell COM outputs

This shell slice audits all 185 remaining `ComOutPtr` records against the
physical declarations in every selected Shell traversal owner. It retires
16 exact records, leaving 169 active records; it changes no representation,
import, namespace, availability, enum, callback, or lifetime mechanism.

| Owning input | Exact retired selectors |
|---|---|
| `ShObjIdl_core.idl` | `SHCreateAssociationRegistration::ppv`, `SHCreateDefaultExtractIcon::ppv`, `SHAssocEnumHandlersForProtocolByApplication::enumHandlers`, `IShellMenu::GetShellFolder::ppv` |
| `ShObjIdl.idl` | `INameSpaceTreeControlEvents::OnBeforeContextMenu::ppv`, `INameSpaceTreeControlEvents::OnAfterContextMenu::ppv` |
| `ShlObj_core.h` | `SHCoCreateInstance::ppv`, `SHCreateDataObject::ppv`, `SHCreateDefaultContextMenu::ppv`, `SHCreateFileExtractIconW::ppv` |
| `shellapi.h` | `AssocCreateForClasses::ppv`, `SHGetImageList::ppvObj` |
| `Shlwapi.h` | `AssocCreate::ppv`, `IQueryAssociations::GetEnum::ppvOut` |
| `ShlObj.h` | `IDockingWindowFrame::FindToolbar::ppv` |
| `shdeprecated.h` | `IBrowserService2::CreateBrowserPropSheetExt::ppv` |

Authoritative IDL edits use existing `_COM_Outptr_` SAL in `cpp_quote`
declarations or the existing MIDL `annotation` attribute, preserving `out`,
`iid_is`, other SAL and native guards. The five header-only owners have no
shipped IDL in SDK `10.0.28000.2705`; their edits are post-MIDL patches.
`AssocCreateForClasses` already carries `_COM_Outptr_` in the shipped SDK:
its final-winmd proof permits removing the redundant sidecar without a native
edit. The unbacked ANSI `SHCreateFileExtractIconA` variant remains unchanged.
The three inherited browser C vtables and `ITrackShellMenu`'s inherited
`GetShellFolder` vtable copy in `shdeprecated.h` carry the same facts as their
owning C++ methods. No proxy, template, or inline output declaration is added.

The audit explicitly retains the shell-owned storage operation
`StgMakeUniqueName`, the generic `IUnknown_GetSite`, `IUnknown_QueryService`
and `QISearch` helpers, and the seven HLink hyperlink operations. Controls'
image-list operations and unrelated generic COM, graphics, media, MSHTML,
automation and storage remain outside this logical shell family. No removed
SDK APIs or headers are restored. Existing `CanReturn*` sidecars remain active;
the 17 non-equivalent PreserveResult replacements remain inactive.

`ShellComOutputHeaderTests` compares actual old native declarations plus legacy
sidecars against patched-header-only declarations through the final winmd on
x86, x64 and arm64. It checks every selected final custom-attribute payload,
direction, optionality, retval absence and pointer depth as well as complete
output bytes. The SDK owner fixture applies the real per-file patches,
converts imports and runs production MIDL options for both shell IDL owners,
then compares complete checked-in header bytes and replays all selected
declarations to final winmd. A focused same-path native fixture calls all
selected functions/methods and inherited vtable copies in C and C++, in both
include orders and all three architectures; original, patched and
`WIN32METADATA` object bytes must match apart from the COFF timestamp.
The earlier native no-op fixtures are unchanged. Expected old declarations
come from the unique reachable introduction commit's parent for this fixture,
not a pinned mutable stack commit. The selected SDK patch replay passes the
complete package include root as `MidlInputRoot`, so source ownership is checked
against all shipped IDL inputs rather than only selected copies.
Ordinary CI still generates one normal candidate and compares against
an already-produced target winmd, with its existing disclosed fallback.

## Patch ingestion

SDK ingestion supports PR2248's per-file unified diffs and `git apply` mechanism.
`generation/WinSDK/patches/pre-midl/<owning-file>.patch` modifies copied IDL
or included headers **before** IDL-to-SAL conversion and MIDL compilation.
`post-midl/<owning-file>.patch` modifies the final header copies **after**
MIDL output and the existing compatibility-header restores.

Ingestion defaults to the evaluated `SdkHeaderPatch` items in
`generation/WinSDK/Windows.Win32.proj` for SDK `10.0.28000.2705`.
The grouped [parameter pilot](sdk_parameter_pilot.md) selects four owning
headers and three owning IDLs. Generation uses the checked-in result in
`RecompiledIdlHeaders`; it does not apply patches during scraping. Explicit
selections replace the default list, so retain the WinHttp patch when adding others:

```powershell
.\scripts\RecompileIdlFilesForScraping.ps1 -SelectedPatches @(
    'post-midl/winhttp.h.patch',
    'pre-midl/example.idl.patch',
    'post-midl/example.h.patch'
)
# UpdateSDK.ps1 accepts the same selection when updating the package version.
```

The test-only IDL proof reuses PR2248's `cpp_quote` comment next to `OUTPUT_STATE`;
MIDL emits that comment in `amstream.h`. The header proof adds only a comment
at PR2248's Uxtheme hook context. These fixtures live under the existing emitter
test project, are applied only to temporary SDK roots, and are not production
generation inputs. Neither changes declarations. PR2248's
Uxtheme enum/signature changes and the rest of PR2365's WinHttp annotation corpus
are deliberately not enabled.

The production WinHttp patch adds the shared helper include and prefixes only
`WinHttpReadData`, `WinHttpReceiveResponse`, and `WinHttpSendRequest` with
`_Win32_SetLastError_`. These are the only entries removed from
`WithSetLastError.rsp`. The helper emits `Win32Metadata.SetLastError` under
`WIN32METADATA` and Clang, and is empty during ordinary compilation. Ingestion
preserves the reviewed helper before replacing the generated header tree.
The scraper validates owned annotations during its existing AST walk, then
unions the discovered names with the legacy generator `WithSetLastErrors` list.
It still parses one translation unit and passes that same unit to the generator.
Unknown/malformed owned payloads, arguments, non-import targets, and non-prefix
placement fail with native source locations; unrelated annotations are left alone.
The grouped parameter layer reuses these boundaries for native SAL and four
small owned facts; its exact coverage and deliberate gaps are documented in
[the parameter pilot](sdk_parameter_pilot.md). It does not enable the broad
annotation recipe, enum construction, typedef recovery, or resource ownership.
The focused WinHttp fixture replays the owning-header patch and checks the exact
three sidecar deletions. Header-to-winmd fixtures compare complete output bytes
between legacy and header annotations, without a measurement or receipt framework.
The grouped fixture replays the owning patches against package-original
SDK files and checks their complete bytes against the reingested owners.
Full validation uses ordinary CI generation and comparison to an existing target
winmd; this migration does not add SDK scrapes or paired benchmark builds.

## Explicit supported-OS availability

The explicit-availability layer reuses the specification's fixed
`_Win32_SupportedOS_*_` menu, with no scoped defaults or new representation.
It moves 178 distinct `supportedOS.rsp` facts to 182 native declarations:
38 WinHTTP functions, all 16 sidecar-backed error-handling functions, all five
handle functions, 82 `combaseapi.h` functions, 35 `objbase.h` functions, and
the `IMalloc`, `ISequentialStream`, and `IStream` interface identities in both
`ObjIdl.h` and `ObjIdlbase.h`, annotated through IDL-generated forward declarations.
`CoFileTimeNow` is declared in both COM function owners and carries the same fact
at both declarations. The three interfaces share definition guards across their
two header owners, so both must receive the fact regardless of include order.
Only the exact matching sidecar lines are retired.
Functions with no existing availability fact remain unannotated; native version
guards alone do not authorize an inferred minimum version.

The selected owners contain no sidecar-backed native enum, plain record, callback
typedef, or individual COM-method availability to retire. No unrelated API fact is
invented to fill those gaps. Small native-header-to-final-winmd fixtures cover each
of these declaration kinds, including the actual interface method and callback
delegate type use sites, on x86, x64, and arm64. The reader validates the canonical
version payload, declaration kind, arguments, and conflicting redeclarations.
The existing `NativeAnnotation` transport is consumed by the ordinary emitter;
equal header/legacy values emit one attribute, conflicting legacy attributes fail.
Ordinary function facts also merge into the existing generator `WithAttributes`
configuration after native-name remapping. This preserves a fact when another
header supplies an earlier unannotated declaration (as with `PropVariantCopy`),
without requiring a second SDK parse or annotation patches on every redeclaration.
Ordinary MSVC C/C++ builds, including `WIN32METADATA` without Clang, see empty helpers.

COM availability is authored only in `pre-midl/ObjIdlbase.Idl.patch` alongside
the existing pre-MIDL parameter edits. Its existing `cpp_quote` block emits
C++-only annotated forward declarations of the three existing interface identities;
Clang propagates the attributes to their definitions. `ObjIdl.Idl` textually includes
`objidlbase.idl`, so the same authoritative facts survive into both header owners.
Neither generated header has a post-MIDL patch. Keeping the declarations in the
existing quote block also avoids introducing additional MIDL RPC interface handles.
Focused fixtures replay each selected SDK IDL, its converted imports, and the
production MIDL options, then compare the complete regenerated header bytes
without postprocessing. Those fresh MIDL outputs feed final-winmd fixtures with
the complete selected interface bodies on x86, x64, and arm64. Both include orders
preserve the exact legacy metadata, interface identities and method counts, with
one type-level floor and no method-level leakage.
Native fixtures compile the selected SDK owners as C and C++ on all three
architectures and compare object bytes apart from the COFF timestamp.

Concrete pre-existing projection boundaries remain explicit:

- Scalar typedefs, record/enum aliases, and aliases of callback typedefs have no
  independent projected availability use site. Annotating them fails; annotate the
  owning record/enum or the direct callback-pointer typedef instead. This layer
  does not add scalar identity or change callback alias recovery.
- An ordinary function annotation can appear after an unannotated declaration;
  final-winmd tests cover an earlier header and remapped public names. A method
  annotation must be on its first native method declaration; late-only method
  annotations are not reliably transported by PInvokeGenerator and fail at the
  native source location instead of being silently dropped.
- Ordinary legacy `SupportedOSPlatform` cleanup still removes attributes from
  interface methods. An explicitly authored native method annotation uses the
  owned transport and survives at the emitted method, not its return parameter.
- Scoped defaults remain later work; AST acceptance alone is not final-winmd
  coverage or authorization to add a new availability floor.

## Result and lifetime marker support

`_Win32_Retained_` and `_Win32_ContainsInteriorPointers_` already have the
parameter pilot's reader-to-final-winmd support. The core-owner inventory is
exhausted: WinHTTP's `WinHttpQueryHeadersEx::pBuffer` interior-buffer fact and
the adjacent File I/O `ReadFileEx::lpOverlapped` / `WriteFileEx::lpOverlapped`
retention facts are already in the headers. No remaining matching lifetime
selector (including wildcard supplies) exists in WinHTTP, Foundation, or COM
core owners. Other partitions' lifetime sidecars remain untouched.

The support-only result layer adds `_Win32_PreserveResult_`, transporting the
exact valueless `Win32Metadata.PreserveResult` payload through the existing
`NativeAnnotation` carrier into `System.Runtime.InteropServices.PreserveSig`.
The existing emitter encodes it as `MethodImplAttributes.PreserveSig`, not a
custom attribute or a return-parameter annotation. It neither transforms the
native HRESULT nor changes native/MIDL retval handling. Equal legacy and header
facts emit the same complete winmd bytes.

The marker is accepted only on function/method declarations without bodies,
before their return type and calling convention. MIDL's standard
`[annotation("_Win32_PreserveResult_")]` method attribute emits the marker after
`virtual` but before the return type; that native spelling is also accepted.
Typedefs, callbacks, records, fields, parameters, valued/malformed payloads,
expression arguments, duplicate markers, and late-only redeclarations fail
with native source locations. Ordinary compilation still sees an empty macro.

There are **no production API changes or sidecar retirements** in this layer.
The result inventory must include the differently named existing facts:
prototype [microsoft/win32metadata#2365](https://github.com/microsoft/win32metadata/pull/2365)'s
14 COM methods plus CoInitialize, CoInitializeEx, and CoGetTreatAsClass all have
`CanReturnMultipleSuccessValues` sidecars. Other existing selectors include
`IUnknown::QueryInterface=[CanReturnErrorsAsSuccess]`, inherited IsDirty sites,
and the adjacent `IEnumVARIANT::Next` / `Skip` family. These are existing facts,
not new semantic additions or an exhausted result category.

The current emitter writes those two legacy contracts as distinct custom
attributes, without setting PreserveSig. Replacing either with PreserveResult
removes that custom attribute and adds the implementation flag: it is **not**
an exact final-winmd migration. CsWin32's `UsePreserveSigForComMethod` accepts
either custom attribute or that flag for COM projection, but that does not make
the metadata shapes identical or establish equivalent behavior in all consumers.
The spec's valueless PreserveResult transport cannot recover which legacy
attribute was present. A focused final-winmd test proves the non-equivalence
and that retaining a legacy sidecar alongside the marker preserves its custom
attribute. All legacy selectors remain unchanged; all 17 prototype production
annotations remain inactive. No prototype patch slices are selected.
Test-only annotations on the shipped CoInitialize declaration and
ISequentialStream::Read prove the capability without changing those SDK owners.
The IDL fixture authors only a temporary copy, runs normal conversion and MIDL,
then verifies the actual generated method through the final emitter.

## Explicit import-module overrides

`_Win32_ImportLibrary_("module")` uses the specification's existing spelling
and exact `Win32Metadata.ImportLibrary("module")` payload. It is valid only
before an ordinary imported function declaration without a body. The reader
rejects malformed/unquoted/empty module names, paths, expression arguments,
invalid targets, misplaced annotations, and conflicting redeclarations with
native source locations. Equal repeated annotations are idempotent.
The helper is empty in ordinary C/C++ and in MSVC builds with `WIN32METADATA`.

The existing single AST walk collects overrides and merges them into
PInvokeGenerator's `WithLibraryPaths` **after native-name remapping**. Header
facts take precedence over configured/manual or scan-derived per-function
mappings, which still take precedence over the configured default. This also
handles an earlier unannotated declaration in another header. The normal
emitter consumes the resulting `DllImport`; no new loader, module inference,
entrypoint transformation, or alternate emission path is introduced.
The exact filename, including a non-DLL suffix, is preserved.

The production slice migrates all 27 existing `webauthnplugin.h` manual
overrides to `webauthn.dll`, plus both `HtmlHelp.h` entrypoints to `hhctrl.ocx`.
WebAuthnPlugin.dll/lib does not ship; the header retains the reason for the
override rather than deriving the module from the misleading library name.
The HtmlHelp patch reuses only PR2365's two import annotations, not its enum
or parameter changes. No WebAuthn plugin patch exists in that prototype.
Both owners are shipped in SDK `10.0.28000.2705`, use post-MIDL patches, and
retire exactly 29 matching `libMappingsManual.rsp` lines. Other manual
overrides and ordinary global library/export inference remain unchanged;
scan-derived mappings are not API-specific header annotations.

Focused `ImportLibraryHeaderTests` check final P/Invoke module and entrypoint
metadata, complete legacy/header output equivalence, precedence, remaps,
earlier declarations, diagnostics, and absence of hidden mappings.
The shipped-owner fixtures use the existing test project's independently
restored SDK package and the real native declarations on all architectures.
The isolated HtmlHelp fixture selects both declarations; the plugin fixture
includes its actual SDK WebAuthn type dependencies. The fixtures map the
external SDK HWND tag to pointer storage, without adding an API-specific type.
Ordinary CI's final-winmd integrity tests assert every migrated production
import. Existing SDK patch tests replay both full owners byte-for-byte, and
compile the patched owners as C/C++ on x86/x64/arm64, comparing original SDK,
ordinary patched, and `WIN32METADATA` object bytes apart from COFF timestamps.
This layer adds no workflow, full-SDK local scrape, reference build, or
comparison framework; ordinary CI remains the normal candidate producer.

## Core COM output pointers

The next parameter slice retires exactly twelve existing `=[ComOutPtr]`
records, using native `_COM_Outptr_` SAL and the existing SAL-to-winmd path.
It adds no custom COM annotation or reader, and retains the single production
translation-unit parse. SDK `10.0.28000.2705` establishes these actual owners:

| Owning input / partition | Exact retired selectors |
|---|---|
| `coml2api.h` / Com.StructuredStorage | `StgCreateStorageEx::ppObjectOpen`, `StgOpenStorageEx::ppObjectOpen` |
| `Ole2.h` / ComOle | `OleCreateDefaultHandler::lplpObj`, `OleCreateEmbeddingHelper::lplpObj`, `OleLoadFromStream::ppvObj` |
| `OleCtl.h` / ComOle | `OleCreateFontIndirect::lplpvObj`, `OleCreatePictureIndirect::lplpvObj`, `OleLoadPicture::lplpvObj`, `OleLoadPicturePath::ppvRet` |
| `ocidl.Idl` / ComOle | `IClassFactory2::CreateInstanceLic::ppvObj` |
| `ServProv.Idl` / Com | `IServiceProvider::QueryService::ppvObject` |
| `Unknwnbase.Idl` / Com | `IUnknown_QueryInterface_Proxy::ppvObject` |

The first three are post-MIDL patches. The last three are pre-MIDL patches;
checked-in headers are regenerated, not independently patched. `Unknwnbase.Idl`
supplies both physical `Unknwn.h` and `Unknwnbase.h` variants. `ServProv.Idl`
also contains a `cpp_quote` C++ alternative that must be annotated alongside
the ordinary MIDL method; its unrelated template helper and remote proxy are
unchanged. The IClassFactory2 native source already says `[out, iid_is(riid)]`;
its explicit RPC annotation previously prevented the converter from adding COM
SAL. Replacing that explicit annotation preserves the caller-required output
storage and nullable result distinction, with no optional-parameter change.
PR2365's `OleCreatePictureIndirect` SAL hunk is reused without its unrelated
input-direction or availability changes. Its comment-only ComOutPtr facts are
not a supported transport and are not copied.

The complete sidecar count falls from 237 to 225. No wildcard supplies the
retired facts. The separate `OleCreatePictureIndirect::lplpvObj=[Out]`
record stays intact. The preceding parameter pilot's sixteen COM moves are
not reintroduced. `IAgileReference::Resolve` remains active because native
retval recovery would change metadata shape; `LPFNGETCLASSOBJECT` is a
separate callback mechanism. IClassFactoryEx belongs to MsHtml/MsHtmHst.Idl,
not the core activation owner family. IBindHost references in ocidl are not
definitions: its MonikerBindToObject/Storage methods belong to urlmon.
The remaining graphics, media, shell, CLR and other owner families are
out of scope, not unavailable SDK APIs. No selected declaration was unavailable.
OleLoadPictureEx has no existing ComOutPtr selector and is not changed.

`ComOutputHeaderTests` emit final winmd for every selected declaration on
x86/x64/ARM64, compare all bytes with the reachable parent's actual declarations
plus the twelve old sidecars, and inspect singleton attribute blobs and flags.
SDK-dependent aggregate types in those bounded fixtures are only dependency
stubs; complete SDK C/C++ compilation independently checks all native signatures
and vtable calls against shipped/ordinary/metadata-on object bytes.
Real selected-SDK IDL conversion and MIDL output replay every physical owner
byte-for-byte and feed those fresh declarations through final emission.
Negative fixtures distinguish borrowed/unmarked/ordinary outputs, nullable
results from optional storage, deep pointers and wrong-site SAL; malformed SAL
remains a compiler diagnostic. Existing native SAL composition is preserved:
an explicit input plus COM output yields In|Out, rather than a new conflict
policy. Matching legacy COM facts deduplicate. Production winmd integrity
checks cover these twelve exact sites alongside the earlier pilot.

## URLMon binding and activation output pointers

The URLMon layer inventories all 225 remaining ComOutPtr records against the
selected SDK's actual `UrlMon.Idl` and `urlmon.h` owners and retires exactly
three: `IBindHost::MonikerBindToObject::ppvObj`,
`IBindHost::MonikerBindToStorage::ppvObj`, and
`CoGetClassObjectFromURL::ppv`. The remaining corpus is 222 records.
No wildcard selector supplies any of these facts; the two type-override records
containing `void**` are not wildcard selectors. Hlink and Media Foundation
declarations are separate physical owners, not additional URLMon inventory.

One `pre-midl/UrlMon.Idl.patch` owns all edits. The activation function is an
existing `#pragma midl_echo` declaration: only its `_Outptr_` becomes
`_COM_Outptr_`. Both local binding methods already have `[out]`; explicit
native `annotation("_COM_Outptr_")` carries their existing COM-output fact
without changing their local/RPC signatures. Their remote `call_as` methods
already have `[out, iid_is(riid)]` and are unchanged. No retval, optionality,
pointer depth, or independent sidecar is changed.

MIDL propagates each local annotation into the complete C++ definition, C
vtable, and local proxy declaration. There is one complete guarded IBindHost
definition in this owner, not a second alternative hidden behind its forward.
The local proxies have no production import mapping and the existing emitter
omits unmapped imports; the focused fixture uses that same empty default and
the actual activation mapping, asserting the proxies stay absent from final
metadata. No new exclusion, import override, reader, or compatibility fallback
is introduced to conceal those generated annotations.

`UrlMonComOutputHeaderTests` compare the reachable parent's original native
declarations plus all three legacy facts with header-only final winmd bytes on
x86/x64/ARM64, inspect exact attribute blobs, output flags and double-pointer
signatures, and check matching sidecars deduplicate. A byte-level assertion
allows only the seven generated-header SAL substitutions and three IDL edits,
preserving all other bytes and line endings. The existing real-SDK patch/MIDL
fixture now replays URLMon too. The native no-op fixture compiles C and C++
on all three architectures, with URLMon included before or after its COM
dependencies, and compares original/patched/metadata-defined object bytes.
The production integrity test asserts all three retired facts. Existing result
contracts and the IAgileReference, callback, and MsHtml boundaries stay active
and outside this layer.

## Shell item construction and folder/object binding

This layer inventories the 222-record ComOutPtr corpus at the reachable parent
of its fixture-introduction commit against the real selected SDK
owners and retires seventeen exact `=[ComOutPtr]` records, leaving 205.
The eleven initial leads expand to item-array construction, known-folder item
retrieval, and the paired transfer create/open methods:

| Authoritative owning input | Retired selectors |
|---|---|
| `ShObjIdl_core.idl` | `SHCreateItemFromIDList::ppv`, `SHCreateItemFromParsingName::ppv`, `SHCreateItemWithParent::ppvItem`, `SHCreateItemFromRelativeName::ppv`, `SHCreateItemInKnownFolder::ppv`, `SHGetItemFromObject::ppv`, `SHGetItemFromDataObject::ppv` |
| `ShObjIdl_core.idl` | `SHCreateShellItemArrayFromDataObject::ppv`, `SHCreateShellItemArrayFromShellItem::ppv` |
| `ShObjIdl_core.idl` | `ITransferSource::OpenItem::ppv`, `ITransferDestination::CreateItem::ppvItem`, `ITransferDestination::CreateItem::ppvResources` |
| Header-only `ShlObj_core.h` | `SHGetKnownFolderItem::ppv`, `SHBindToParent::ppv`, `SHBindToFolderIDListParent::ppv`, `SHBindToFolderIDListParentEx::ppv`, `SHBindToObject::ppv` |

One pre-MIDL patch changes the nine existing `cpp_quote` function declarations
and three explicit MIDL parameter annotations to native `_COM_Outptr_`.
The transfer methods retain their original `[out, iid_is(...)]` contracts.
Selected MIDL regenerates `ShObjIdl_core.h`: nine function declarations and
six C++/C-vtable parameter sites change. Both transfer interfaces have one
complete guarded definition plus forwards; these local methods have no proxy
declarations. The five header-only declarations use a post-MIDL patch: the SDK
does not ship `ShlObj_core.idl`. `ShObjIdl.idl` imports the core IDL; its inline
calls are references, not duplicate declarations. `ShlObj.h` likewise does not
redeclare these functions. No selected function has a template or inline
overload. All other native bytes and line endings are preserved.

The inventory deliberately retains the independent shell property-store,
association/handler, extract-icon, generic activation, data-object creation,
context-menu/view, docking/menu, and unique-storage-name contracts. Their
presence in the same headers does not make them item construction/binding.
Of eighteen corpus records actually declared in `ShObjIdl_core`, twelve move;
the six retained selectors are `SHCreateAssociationRegistration::ppv`,
`SHCreateDefaultExtractIcon::ppv`,
`SHAssocEnumHandlersForProtocolByApplication::enumHandlers`,
`IShellMenu::GetShellFolder::ppv`, `SHGetPropertyStoreFromIDList::ppv`, and
`SHGetPropertyStoreFromParsingName::ppv`. Of ten actually declared in
`ShlObj_core.h`, five move; `SHCoCreateInstance::ppv`,
`SHCreateDataObject::ppv`, `SHCreateDefaultContextMenu::ppv`,
`SHCreateFileExtractIconW::ppv`, and `StgMakeUniqueName::ppv` stay active.
The two context-menu event records in `ShObjIdl` and the docking toolbar record
in `ShlObj.h` also stay active. Token mentions in comments or inline calls
(including IQueryAssociations) are not counted as owning declarations.
The transfer destination's resource output stays paired with its item output;
source opening belongs to that same create/open protocol. Typed item factories
without a legacy ComOutPtr record, including `SHCreateShellItem`,
`SHCreateShellItemArray`, and `SHCreateShellItemArrayFromIDLists`, are unchanged.
No wildcard or independent Out/result/enum/type record is removed; in particular
the `SHCreateItemInKnownFolder::dwKFFlags` associated enum stays active.
IAgileReference, callback, MsHtml, and result-representation boundaries remain
unchanged.

`ShellItemComOutputHeaderTests` emit the actual selected native declarations
through final winmd on x86/x64/ARM64, comparing every byte against the reachable
parent's native declarations plus all seventeen legacy facts. This parent is
resolved from reachable fixture-introduction ancestry, not a frozen pre-rebase
commit. Replay validates source ownership against the complete selected SDK
`MidlInputRoot`, including the core IDL output and the direct binding header.
They inspect
singleton custom-attribute blobs, exact Out/nonoptional/nonretval flags and
double-pointer signatures, and verify matching sidecars deduplicate. Dependency
types are bounded fixture stubs, not replacement API declarations. Real selected
SDK patch replay and selected MIDL reproduce every owning file byte-for-byte,
then replay those fresh declarations through final emission on all three
architectures. A focused native C/C++ no-op fixture covers every selected
function signature and both transfer vtable calls across x86/x64/ARM64, with
shell/URLMon headers before or after their COM dependencies. It compares exact
parent-native versus patched ordinary/metadata object bytes, excluding only COFF
timestamps. Both use identical include paths and parent SAL helpers: changing
header paths changes MSVC UUID COMDAT ordering independently of SAL. The original
SDK/core/URLMon no-op fixture is unchanged. Production metadata
integrity checks cover all seventeen sites. No reader, transport, compatibility
override, import exclusion, workflow, or extra full-SDK generation is added.

## Property-system COM outputs and property-store acquisition

The property layer audits all 205 remaining ComOutPtr records against their
selected physical owners and retires twenty exact `=[ComOutPtr]` records,
leaving 185. The sixteen property-system leads expand to the four shell
property-store/property-bag acquisition functions retained by the shell layer:

| Authoritative owning input | Exact retired selectors |
|---|---|
| `propsys.idl` | `PSCreateAdapterFromPropertyStore::ppv`, `PSCreateDelayedMultiplexPropertyStore::ppv`, `PSCreateMultiplexPropertyStore::ppv`, `PSCreatePropertyChangeArray::ppv`, `PSCreatePropertyStoreFromObject::ppv`, `PSCreatePropertyStoreFromPropertySetStorage::ppv`, `PSCreateSimplePropertyChange::ppv`, `PSCreateMemoryPropertyStore::ppv` |
| `propsys.idl` | `PSEnumeratePropertyDescriptions::ppv`, `PSGetItemPropertyHandler::ppv`, `PSGetItemPropertyHandlerWithCreateObject::ppv`, `PSGetPropertyDescription::ppv`, `PSGetPropertyDescriptionByName::ppv`, `PSGetPropertyDescriptionListFromString::ppv`, `PSGetPropertySystem::ppv`, `PSPropertyBag_ReadUnknown::ppv` |
| `ShObjIdl_core.idl` | `SHGetPropertyStoreFromIDList::ppv`, `SHGetPropertyStoreFromParsingName::ppv` |
| Header-only `shellapi.h` | `SHGetPropertyStoreForWindow::ppv` |
| Header-only `Shlwapi.h` | `SHGetViewStatePropertyBag::ppv` |

`pre-midl/propsys.idl.patch` changes sixteen existing `cpp_quote` SAL
declarations. The existing `pre-midl/ShObjIdl_core.idl.patch` gains the two
adjacent property-store declarations while preserving the previous shell
edits. Actual selected-SDK MIDL regenerates both headers. The two genuinely
header-only owners use `post-midl/shellapi.h.patch` and
`post-midl/Shlwapi.h.patch`; the existing wildcard selects them. Every edit
replaces only `_Outptr_` with `_COM_Outptr_`. No native prototype, pointer depth,
optional-storage/result contract, retval, guard, import or ABI changes.
The existing property system interface methods already carry native MIDL/SAL
COM facts and have no remaining sidecar to migrate.

Each selected function has exactly one physical declaration in its owner,
with no inline/template overload or generated proxy. Desktop/application
family guards and the shell version/feature guards remain byte-identical.
`ShObjIdl.h`, `ShlObj_core.h`, `ShlObj.h`, `PropIdl.h`, `PropIdlBase.h`,
`propvarutil.h` and `propapi.h` contain no selected duplicate declaration.
Both shell include alternatives continue through the owning core header.
No second parser, name/IID inference or special projection is added.

The complete-owner audit retains four unrelated `ShObjIdl_core` records:
`SHCreateAssociationRegistration::ppv`, `SHCreateDefaultExtractIcon::ppv`,
`SHAssocEnumHandlersForProtocolByApplication::enumHandlers` and
`IShellMenu::GetShellFolder::ppv`. `shellapi.h` retains
`AssocCreateForClasses::ppv` and
`SHGetImageList::ppvObj`; `Shlwapi.h` retains
`AssocCreate::ppv`, `IUnknown_GetSite::ppv`, `IUnknown_QueryService::ppvOut`
and `QISearch::ppv`. Generic activation/association/UI helpers are not
property acquisition. The `SHCreateDefaultContextMenu` mention in
`shellapi.h` and `AssocCreateForClasses` mention in `Shlwapi.h` are comments,
not duplicate owning declarations. The five retained
`ShlObj_core` records, shell menu
events, docking, storage, callbacks, scalar/VARTYPE, enum, availability,
namespace and resource-ownership boundaries remain active. No wildcard
supplies the twenty retired facts. Independent Out and all CanReturn*
sidecars stay unchanged; the seventeen PreserveResult replacements remain
inactive because their metadata shapes differ.

`PropertyComOutputHeaderTests` compares original native declarations plus
legacy sidecars against header-only final winmd bytes on x86/x64/ARM64.
It reuses the shell fixture's exact Out/nonoptional/nonretval, `void**` and
singleton custom-attribute blob assertions and checks equal-sidecar
deduplication. Original native files and sidecars come from the parent of the
property fixture's single reachable introduction commit, following the
core/URLMon/shell baseline mechanism. No pre-rebase parent or SDK-update SHA
is pinned. An explicit ancestry assertion requires the resolved baseline to
be reachable from the tested head, including CI's clean full-history branch
fetch. The complete corpus audit compares the original 205-record corpus
with the remaining 185 and requires every independent sidecar to be unchanged.
Byte audits prove
only the intended SAL substitutions and preserve native encoding/newlines.
The existing selected-SDK replay fixture retains the full `MidlInputRoot`
ownership check for both phases, patches both IDLs, runs actual
MIDL for both owners, checks every owning file byte-for-byte, and runs the
fresh outputs through final emission on all three architectures. The existing
shell native fixture retains its original checks and additionally compiles
all twenty exact function-pointer signatures as C/C++ on x86/x64/ARM64,
in both shell/COM include orders. Original-native, patched and
WIN32METADATA-defined builds use identical header paths and SAL copies and
compare all object bytes except the COFF timestamp. The original SDK/core/
URLMon no-op fixture remains intact. Production integrity checks cover all
twenty retired facts. No workflow or extra full-SDK generation is added;
ordinary CI produces one candidate and compares it to its existing target
CI winmd, with the existing disclosed main/released fallback if unavailable.

## Patch format and validation

Each patch starts with `# SDK-Version: 10.0.28000.2705`, followed by a normal
`git diff` with repository-relative paths under
`generation/WinSDK/RecompiledIdlHeaders/{um,shared,winrt,ucrt}/`.
It must modify exactly one existing file matching its owning filename. Selection
paths use `/`, as in the examples; filesystem command paths use `\`.
Selections are applied in their explicit order, at most once per file per phase.
Missing selections/targets, wrong phases, version drift, traversal, reparse points,
file creation/deletion/rename, stale hunks, and double application throw.
Every selected patch is validated even when it belongs to the other phase.
Each phase is staged before copying results back, so a failed hunk leaves that
phase's authoritative files untouched. Failure stops ingestion; rerun from fresh
SDK copies after fixing the patch.

Header patches in either phase cannot target outputs in the production MIDL
regeneration queue. Patch the authoritative IDL in `pre-midl` instead, including
IDL incorporated through imports or includes. Ingestion captures the queue once
after SDK, Direct3D, and AdditionalHeaders copies and before patches, then uses
the same IDL/output paths for compilation, copy-back, and both phase guards.
The existing exclusions (including `wincrypt.idl`) still apply. A historical
MIDL banner alone does not make a header an output: headers without a queued
shipped source, such as `winsync.h`, remain valid direct-header targets.
AdditionalHeaders and restored compatibility inputs are not reclassified as
shipped SDK sources by this guard.

Standalone application derives the same queue from `HeaderRoot`. If replaying
against a subset of headers, pass `-MidlInputRoot` pointing to the complete
copied ingestion input tree; a header-only subset cannot establish ownership.
Ingestion passes its captured `MidlOutputs` directly so renamed output paths
are checked without guessing the IDL's basename.

Target bytes are not decoded/re-encoded. Patch line endings are matched to each
target's LF or CRLF format; UTF-8 BOMs and legacy byte encodings are preserved.
UTF-16 and mixed line endings are explicitly unsupported. When the SDK changes,
regenerate the patch against fresh copied inputs and update its version line;
do not silently skip it or broaden hunk matching.

Focused tests live in the existing emitter MSTest project, including phase
selection, conversion survival, unchanged proof declarations, hook placement,
encoding/line endings, and negative cases:

```powershell
dotnet test tests\ClangSharpSourceToWinmdTests -c Release --filter SdkPatchTests
dotnet test tests\ClangSharpSourceToWinmdTests -c Release --filter LastErrorHeaderTests
.\scripts\TestTooling.ps1
```

In PR validation, the `sdk-patch-tests` job runs these tests parallel to scraping,
tooling tests, and ordinary candidate builds after the shared change-detection
job. Its path filter includes ingestion scripts, patch assets/fixtures, the two
proof inputs, and relevant build configuration. Unrelated PRs skip this job
without starting a runner. Renamed paths are checked on both sides; incomplete
file lists fail closed by running the tests. Manual and push runs also run them.
The test project declares the existing pinned SDK as a private data-only package.
Normal test restore supplies the real SDK/MIDL fixtures without a separate
BuildTools restore/build or a full SDK regeneration.
`TestTooling.ps1` (also called by `DoTests.ps1`) excludes `SdkPatchTests`, so they
do not add runtime to ordinary tooling or the serial build/test/package job.
The existing required aggregate check (retaining the **Migration qualification**
status name for compatibility) requires this job to
succeed for relevant changes, accepting a skipped job only when change detection
explicitly reports no relevant changes. Failed/cancelled/missing results cannot
pass that gate. No new required status context is needed, so unrelated PRs cannot
get stuck waiting for a workflow that never ran. This is an ordinary CI status
aggregate, not a measurement or receipt verifier. CI compares the candidate
against an already-produced target/upstream/release winmd; there are no additional
reference/paired SDK generations or performance qualification passes.

Original mechanism and proof:
[microsoft/win32metadata#2248](https://github.com/microsoft/win32metadata/pull/2248),
initial commit `a942a455208b374f7c68de8364fc2455e9c64a1a`.
