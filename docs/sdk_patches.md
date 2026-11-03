# SDK header patches

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

There are **no production API changes or sidecar retirements** in this layer:
the selected owners have no existing PreserveSig/PreserveResult sidecars.
Prototype [microsoft/win32metadata#2365](https://github.com/microsoft/win32metadata/pull/2365)'s
14 COM methods plus CoInitialize, CoInitializeEx, and
CoGetTreatAsClass are unapproved result-semantics additions, not migrated facts,
and remain inactive. No prototype production patch slices are selected.
Test-only annotations on the shipped CoInitialize declaration and
ISequentialStream::Read prove the capability without changing those SDK owners.
The IDL fixture authors only a temporary copy, runs normal conversion and MIDL,
then verifies the actual generated method through the final emitter.

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
