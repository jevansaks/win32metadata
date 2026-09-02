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
- **Blocker (documentation ambiguity, not fixed here):** Attempted to verify a
  `_Win32_metadata_supported_os_` value for `PTOpenProvider`/`PTCloseProvider` via web
  search; the result claimed "Windows XP", which is implausible for the Print Ticket XML
  API (introduced with the XPS/Vista print pipeline) and is not corroborated by any
  `NTDDI_VERSION`/`_WIN32_WINNT` guard in the header itself (this header has none at all).
  Given the documentation signal is unreliable and no header-level guard corroborates it,
  no `supported_os` annotation was added — this matches the ledger's own stop condition
  ("Ambiguous... requiring spec decision"). Flagged for a future queue entry with a more
  authoritative source (e.g. a live Windows SDK reference build) rather than guessed at.
- **Note:** The custom `_Win32_metadata_raii_free_`/`_Win32_metadata_invalid_handle_`
  annotations are processed at the `EmitWinmd` stage (via `MetadataSyntaxTreeCleaner`), not
  visible as C# attributes at the `ScrapeHeaders` stage (unlike
  `_Win32_metadata_supported_os_`, which renders directly as `[SupportedOSPlatform(...)]`
  during scraping). This live re-scrape therefore validates C syntax/parseability, matching
  the same evidence bar (pattern-match + patch-replay) used throughout the resource-ownership
  audit batches, not full winmd emission.
- **Normalization:** Cleanup ownership and invalid values are producer-use metadata, not
  typedef metadata.
