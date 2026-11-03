# Architecture

The [Scraper](#scraper) layer is responsible for traversing header files and generating C# files.

The [Emitter](#emitter) layer is responsible for traversing the generated C# files and generating Windows.Win32.winmd.

The [WinmdGenerator](#winmdgenerator) packages the Scraper and Emitter tooling into an [MSBuild project SDK](https://learn.microsoft.com/visualstudio/msbuild/how-to-use-project-sdk) that acts as the interface for converting C/C++ projects to winmds for use with language projections.

## Scraper

The Scraper layer is responsible for traversing header files and generating C# files.

### ClangSharp

[ClangSharp](https://github.com/dotnet/ClangSharp) traverses header files as defined within [Partitions](../generation/WinSDK/Partitions) and generates C# files within [generated](../generation/WinSDK/obj/generated).

The base settings passed into ClangSharp are defined within [baseSettings.rsp](../sources/GeneratorSdk/tools/assets/scraper/baseSettings.rsp) along with adjacent architecture-specific response files.

Project-specific settings are included within [scraper.settings.rsp](../generation/WinSDK/scraper.settings.rsp) along with adjacent domain-specific response files like [libMappings.rsp](../generation/WinSDK/libMappings.rsp), [supportedOS.rsp](../generation/WinSDK/supportedOS.rsp), and [WithSetLastError.rsp](../generation/WinSDK/WithSetLastError.rsp).

[scraper.header.txt](../generation/WinSDK/scraper.header.txt) includes using statements that are added to the generated C# files to resolve cross-namespace dependencies.

### ConstantsScraper

[ConstantsScraper](../sources/MetadataUtils/ConstantsScraper.cs) walks the header files included within [Partitions](../generation/WinSDK/Partitions) and generates C# constants based on regular expression pattern matching.

The base settings passed into ConstantsScraper are defined within [baseSettings.ConstantsScraper.rsp](../sources/GeneratorSdk/tools/assets/scraper/baseSettings.ConstantsScraper.rsp).

Project-specific settings are included within [ConstantsScraper.settings.rsp](../generation/WinSDK/ConstantsScraper.settings.rsp).

[ConstantsScraper.header.txt](../generation/WinSDK/ConstantsScraper.header.txt) includes using statements that are added to the generated C# files to resolve cross-namespace dependencies.

## Emitter

The Emitter layer is responsible for traversing the generated C# files and generating Windows.Win32.winmd.

[ECMA-335](https://www.ecma-international.org/publications-and-standards/standards/ecma-335/) defines the format of winmd files. ECMA-335 is the binary format used by .NET binaries.

* WinRT winmd files use this format
* Many Win32 concepts already supported from .NET interop
* Reflection-based APIs provide a simple interface for parsing the metadata directly
* Reflection-based APIs provide a simple means to convert winmd to other formats like JSON

The Emitter layer augments ECMA-335 by applying additional patterns and custom attributes that allow language projections to understand Win32-specific semantics and provide an improved developer experience. See [PROJECTIONS.md](projections.md).

### ClangSharpSourceCompilation

This class orchestrates the manipulation and compilation of the generated C# files.

Project-specific settings are included within [emitter.settings.rsp](../generation/WinSDK/emitter.settings.rsp).

ClangSharp was designed to create C#-compilable code from Win32 headers. Because its goal is to create C#-compilable code while also preserving pointers, it can't always express things in the way we would like for metadata, which is meant to be language-agnostic. For example, the CLR will not allow managed types such as "interface" or "delegate" to be on an unsafe struct (a struct that gets pointed to or includes pointers). This means ClangSharp emits COM objects as structs instead of interfaces, so that a COM object can exist on an unsafe struct. A .winmd does not have such restrictions, so ClangSharpSourceCompilation handles manipulating the generated CLR-compliant C# files into the language-agnostic metadata representation.

#### NamesToCorrectNamespacesMover

This class moves APIs to namespaces based on [requiredNamespacesForNames.rsp](../generation/WinSDK/requiredNamespacesForNames.rsp).

#### MetadataSyntaxTreeCleaner

This class visits each node in the C# abstract syntax trees (AST) and applies modifications such as remaps and custom attributes.

#### CrossArchTreeMerger

This class handles merging C# files from multiple architectures to identify architecture-specific APIs.

### ClangSharpSourceWinmdGenerator

This class walks the final C# abstract syntax trees (AST) and writes each node to Windows.Win32.winmd.

## WinmdGenerator

### Fork migration qualification

In `jevansaks/win32metadata`, PR validation runs on every PR base, including
stacked topic branches. `feature/header-annotations` requires the exact checks
`Migration qualification`, `Tooling tests`, `Workflow regression tests`, and
`Build, test, package`. Fork documentation changes also run the full checks;
upstream documentation-only build skipping remains supported. Missing, failed,
cancelled or unexpectedly skipped dependencies never pass the migration gate.
The fork does not require a second reviewer. User sign-off still precedes
coordinator-managed merges; successful automation alone does not authorize merging.

Build, package, samples and structural tests start as soon as all scrape artifacts
are ready, independently of paired measurement. A downstream comparison job waits
for both paths, verifies the frozen reference and emits the existing API diff and
canonical winmd artifacts. The migration aggregate also requires that job to
succeed. This overlaps build work with measurement without changing per-emission
budgets, reference selection or documentation-change qualification.
Qualification setup logs CPU model, logical cores, RAM and effective worker
settings in `runner-hardware.txt`, without hostname or receipt-schema changes.

Run `.\scripts\TestTooling.ps1` for all three tooling suites. Both this entry
point and `.\scripts\DoTests.ps1` reject missing, empty or failing TRX results.
The existing MSTest emitter suite now includes workflow/verdict guardrail tests,
three real emissions of identical fixture inputs, raw/normalized hash evidence,
and checks that every byte outside the two normalized fields remains observed.
`TestTooling.ps1` builds the CLI through normal test project references and
executes this coverage without a separate BuildTools prerequisite. Repeated
fixture emission is not full-SDK determinism or performance evidence.
Node remains scoped to the pre-existing API-comment workflow tests:
`node --test .github/tests/pr-api-diff-comment.test.cjs`.

`.\scripts\TestMigrationQualification.ps1 -ReferenceDirectory <checkout>`
requires a separate checkout at
`86ed37b2846440297afb087c4718acd1a1fb4ad8`. It freshly generates the baseline
and candidate from identical legacy inputs, with SDK `10.0.28000.2705`,
ClangSharp `21.1.8.4`, winmd version `1.0.0.0`, two scraper workers, one MSBuild
node and the existing single-TU-per-partition/architecture behavior. Restoring,
building tools, queue delay, packages and samples are outside the timed region.
Generated markers/sources and the output winmd are removed before each sample.
The measured command runs cross-architecture header and constant scraping
through final emission, not just an isolated partition or tooling tests.

The initial blocking budgets are a confirmed full-generation wall-time increase
greater than 10% and process-tree working-set increase greater than 15%.
One paired sample is normally sufficient; a breach requires confirmation, with
at most three paired samples and alternating execution order. An unresolved
two-sample result fails; three samples resolve the majority. Memory is the
maximum **sampled** sum of live child-process working sets, sampled after each
CIM query plus a 500ms delay; it is not an instantaneous peak and may miss brief
spikes. The proposed five-pair warm WinHttp/Foundation/Com pilot and its median
10%/one-second budget are not implemented in this infrastructure layer.

The drift gate compares the entire final winmd rather than relying on the
decompiler dump or semantic comparison helper. Existing WinmdUtils commands
`compareBinary`, `qualifyMigration`, and `verifyMigrationEvidence` use the BCL
`PEReader`/`MetadataReader` and receipt validation; there is no handwritten
PE/metadata table parser or additional project/package. The comparer normalizes only the Module
MVID (the emitter calls `Guid.NewGuid`) and the COFF PE creation timestamp
(`ManagedPEBuilder` defaults); assembly version is fixed before emission.
There are no approved semantic differences or exclusion masks. Any other
nondeterminism fails qualification and must be investigated, not masked.
This compatibility normalization is temporary until emitter determinism lands
and a deterministic reference is deliberately reviewed; neither side is silently
replaced with candidate output.

Actions retains TRX, source/input SHA receipts, SDK content and toolchain hashes,
output winmds/hashes, raw timing/memory samples, target-phase binlogs and drift
reports, with a linked summary on success and failure. The aggregate downloads
and rechecks evidence and candidate SHA binding. The fork API-comment baseline
is this newly generated frozen reference, never an arbitrary latest successful
branch artifact or last-release fallback. The existing upstream informational
API comparison is separate from blocking migration qualification.

Future layers can extend the explicit reference/candidate pair to A/B/C inputs
after separately approving and documenting a source migration. They must not
replace the frozen reference or budgets with PR output, introduce broad
approved-difference masks, or treat a selected scrape as final-winmd e2e.

The WinmdGenerator packages the Scraper and Emitter tooling into an [MSBuild project SDK](https://learn.microsoft.com/visualstudio/msbuild/how-to-use-project-sdk) that acts as the interface for converting C/C++ projects to winmds for use with language projections.

The WinmdGenerator tooling is published to nuget.org as [Microsoft.Windows.WinmdGenerator](https://www.nuget.org/packages/Microsoft.Windows.WinmdGenerator/). As an MSBuild project SDK, it enables a no-code project file based configuration interface for generating winmd files from arbitrary C/C++ projects.

[win32metadata](https://github.com/microsoft/win32metadata/blob/main/generation/WinSDK/Windows.Win32.proj) and [wdkmetadata](https://github.com/microsoft/wdkmetadata/blob/main/generation/WDK/Windows.Wdk.proj) both demonstrate WinmdGenerator project files. The WinmdGenerator version can be controlled as described [here](https://learn.microsoft.com/visualstudio/msbuild/how-to-use-project-sdk#reference-a-project-sdk).