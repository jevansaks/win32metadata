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
The grouped fixture replays all seven owning patches against package-original
SDK files and checks their complete bytes against the reingested owners.
Full validation uses ordinary CI generation and comparison to an existing target
winmd; this migration does not add SDK scrapes or paired benchmark builds.

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
