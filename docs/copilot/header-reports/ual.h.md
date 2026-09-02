# `ual.h`

- **Status:** accepted-normalized
- **Batch:** `scraping-investigation-01`
- **Partitions:** Ual
- **Delta:** No retained patch previously existed. Header-scraping investigation (x86 scrape,
  the viable architecture for this `ExcludeFromCrossarch` partition — see Blocker note below)
  confirmed the header scrapes cleanly to `UalStart`, `UalStop`, `UalInstrument`, and
  `UalRegisterProduct` (all `HRESULT WINAPI`, guarded by `#if NTDDI_VERSION >= NTDDI_WIN8`).
  Verified via Microsoft Learn documentation
  (`learn.microsoft.com/windows/win32/api/ual/nf-ual-ualstart`) that minimum supported
  client/server is Windows 8 / Windows Server 2012, matching the header's own
  `NTDDI_WIN8` guard and the `windows8.0` convention already used elsewhere in this repo
  (e.g. `combaseapi.h`'s `CoAllowUnmarshalerCLSID`, guarded by
  `_WIN32_WINNT >= _WIN32_WINNT_WIN8`).
- **Resolution:** Added `_Win32_metadata_supported_os_(windows8.0)` to all four functions,
  plus the `win32metadata_annotations.h` include guard. Verified via re-scrape that the
  scraped C# now correctly emits `[SupportedOSPlatform("windows8.0")]` on all four methods.
- **Artifact:** `generation/WinSDK/patches/post-midl/ual.h.zzz-supported-os.patch` (new).
- **Evidence:** `git apply --check --reverse` passes for the new patch. Live re-scrape
  (`dotnet build generation/WinSDK -c Release -p:ScanArch=x86 -t:ScrapeHeaders
  -p:PartitionFilter=Ual`) succeeds and the generated
  `obj/generated/common/Ual.cs` shows `[SupportedOSPlatform("windows8.0")]` on
  `UalStart`/`UalStop`/`UalInstrument`/`UalRegisterProduct`.
- **Blocker (architecture note, non-blocking for this header):** `Ual` is listed in
  `Windows.Win32.proj`'s `ExcludeFromCrossarch` property, so `ScanArch=x64`/`arm64` are
  silently skipped by `MetadataTasks.ScrapeHeaders.ProcessPartition` (architecture-neutral
  partitions are only scraped once, under `ScanArch=x86` → output directory `common`).
  `ScanArch=x64` produces zero output for this partition; `x86` is the correct "smallest
  viable architecture" for `ExcludeFromCrossarch` partitions, not `x64`.
- **Blocker (external, not specific to `ual.h`, recorded for visibility, not fixed here):**
  Every scrape of the `Ual` partition (before and after this patch) emits 4 pre-existing
  "Inconsistent remap" warnings plus a summary warning:
  `in_addr`→`IN_ADDR`, `sockaddr`→`SOCKADDR`, `timeval`→`SOCKADDR`/`TIMEVAL` (declared
  correctly in the `WinSock` partition) and `_CERT_CONTEXT`→`CERT_CONTEXT` (declared in the
  `Wsw` partition), all "discovered in partition 'Ual'". These four type names do **not**
  appear anywhere in the generated `Ual.cs` (verified by direct grep), so they do not affect
  `ual.h`'s own emitted metadata; the warning is a cross-partition auto-remap-consistency
  check unrelated to this header's content, `Ual`'s `main.cpp` already `#include`s
  `winsock2.h`. Root-causing and fixing this pre-existing warning (likely requiring changes
  to `Wsw`'s or `WinSock`'s own remap configuration, or the auto-remap-discovery pass itself)
  is out of scope for this header and is called out here for a future dedicated queue entry
  rather than guessed at.
- **Normalization:** ABI-neutral declaration annotation, verified against official
  documentation rather than assumed.
