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

### Fork CI guardrails

In `jevansaks/win32metadata`, PR validation runs on every PR base, including
stacked topic branches. `feature/header-annotations` requires the exact checks
`Migration qualification`, `Tooling tests`, `Workflow regression tests`, and
`Build, test, package`. Fork documentation changes also run the full checks;
upstream documentation-only build skipping remains supported. Missing, failed,
cancelled or unexpectedly skipped dependencies never pass the migration gate.
The fork does not require a second reviewer. User sign-off still precedes
coordinator-managed merges; successful automation alone does not authorize merging.

Only the ordinary candidate CI generation runs: three architecture scrapes,
followed by emission, packages, samples and structural tests. No additional
reference/candidate SDK generations, paired measurements, confirmation rounds
or performance receipt framework are run. `Migration qualification` is only a
cheap fail-closed aggregate of these ordinary CI jobs and API comparison.

Run `.\scripts\TestTooling.ps1` for all three tooling suites. Both this entry
point and `.\scripts\DoTests.ps1` reject missing, empty or failing TRX results.
The existing MSTest emitter suite includes ordinary workflow/dependency and
TRX guardrail tests alongside its existing metadata tests.
`TestTooling.ps1` builds the CLI through normal test project references and
executes this coverage without a separate BuildTools prerequisite.
Node remains scoped to the pre-existing API-comment workflow tests:
`node --test .github/tests/pr-api-diff-comment.test.cjs`.

The API comparison reuses an already-produced winmd from successful target-branch
CI at the PR base commit. If unavailable, it tries successful upstream
`microsoft/win32metadata` main CI, then the existing released-package baseline.
The selected repository/branch/commit/run/artifact or released package path is
recorded in `baseline-source.txt`; actual baseline/candidate hashes and candidate
commit are retained in `comparison-inputs.txt`. Missing outputs, dump failures
and comparison command errors fail CI; API differences themselves are retained
for review. This uses the existing WinmdUtils API dump and release helper, not
a binary-normalization or provenance framework.

Each reused-tools package/sample/test Actions step initializes its own VS
developer environment. It avoids redundant tool compilation while retaining
the package script's post-pack rebuild. Actions retains tooling/full-test TRX,
logs, candidate winmd, packages, API dumps/diff and baseline source information.

The WinmdGenerator packages the Scraper and Emitter tooling into an [MSBuild project SDK](https://learn.microsoft.com/visualstudio/msbuild/how-to-use-project-sdk) that acts as the interface for converting C/C++ projects to winmds for use with language projections.

The WinmdGenerator tooling is published to nuget.org as [Microsoft.Windows.WinmdGenerator](https://www.nuget.org/packages/Microsoft.Windows.WinmdGenerator/). As an MSBuild project SDK, it enables a no-code project file based configuration interface for generating winmd files from arbitrary C/C++ projects.

[win32metadata](https://github.com/microsoft/win32metadata/blob/main/generation/WinSDK/Windows.Win32.proj) and [wdkmetadata](https://github.com/microsoft/wdkmetadata/blob/main/generation/WDK/Windows.Wdk.proj) both demonstrate WinmdGenerator project files. The WinmdGenerator version can be controlled as described [here](https://learn.microsoft.com/visualstudio/msbuild/how-to-use-project-sdk#reference-a-project-sdk).