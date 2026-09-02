# `prntvpt.h`

- **Status:** accepted-normalized
- **Batch:** `scraping-investigation-01`
- **Partitions:** Gdi, PrintTicket
- **Delta:** No retained patch previously existed. Header-scraping investigation (x86 scrape,
  the viable architecture for the `PrintTicket` `ExcludeFromCrossarch` partition) confirmed
  the header scrapes cleanly. Found a genuine resource-ownership gap: `HPTPROVIDER`
  (`DECLARE_HANDLE`) is produced by `PTOpenProvider`/`PTOpenProviderEx` (`_Out_ HPTPROVIDER
  *phProvider`) and consumed/released by `PTCloseProvider` (`_In_ HPTPROVIDER hProvider`) —
  directly verifiable from the header itself, no external documentation needed.
- **Resolution:** Added `_Win32_metadata_invalid_handle_(0)` and
  `_Win32_metadata_raii_free_(PTCloseProvider)` to both producer `_Out_ HPTPROVIDER *`
  parameters, per the corrected shared-handle ownership policy (165b5f09, 7335ddc4) — on the
  producer parameters, not the `HPTPROVIDER` typedef. Added the `win32metadata_annotations.h`
  include guard.
- **Artifact:**
  `generation/WinSDK/patches/post-midl/prntvpt.h.printticket-provider-ownership.patch` (new).
- **Evidence:** `git apply --check --reverse` passes for the new patch. Live re-scrape
  (`dotnet build generation/WinSDK -c Release -p:ScanArch=x86 -t:ScrapeHeaders
  -p:PartitionFilter=PrintTicket`) succeeds with 0 warnings/errors, confirming the annotation
  placement is syntactically valid C parsed cleanly by Clang.
- **Correction:** Initially could not corroborate a `supported_os` value for these functions
  via web search (which implausibly claimed "Windows XP") and left it unannotated pending a
  more authoritative source. Subsequently discovered this repository's own authoritative,
  global `generation/WinSDK/supportedOS.rsp` (17,249 `--with-attribute FunctionName=
  SupportedOSPlatform(...)` entries, loaded for every partition scrape via
  `Windows.Win32.proj`'s `ScraperRsp` item group) already maps `PTOpenProvider`,
  `PTOpenProviderEx`, `PTCloseProvider`, and `PTQuerySchemaVersionSupport` to
  `SupportedOSPlatform("windows5.1.2600")` — confirming the web search result was in fact
  correct, and, more importantly, that **no per-header patch is needed at all**: live
  re-scrape confirms `[SupportedOSPlatform("windows5.1.2600")]` is already emitted on these
  functions purely from the global rsp, with zero inline annotation in the header. The
  earlier "ambiguous, needs external verification" characterization was wrong — the
  repository already has an authoritative answer; the fix was to consult
  `supportedOS.rsp` directly, not the web.
- **Note:** The custom `_Win32_metadata_raii_free_`/`_Win32_metadata_invalid_handle_`
  annotations are processed at the `EmitWinmd` stage (via `MetadataSyntaxTreeCleaner`), not
  visible as C# attributes at the `ScrapeHeaders` stage (unlike
  `_Win32_metadata_supported_os_`, which renders directly as `[SupportedOSPlatform(...)]`
  during scraping). This live re-scrape therefore validates C syntax/parseability, matching
  the same evidence bar (pattern-match + patch-replay) used throughout the resource-ownership
  audit batches, not full winmd emission.
- **Normalization:** Cleanup ownership and invalid values are producer-use metadata, not
  typedef metadata.
