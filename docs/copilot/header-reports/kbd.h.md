# `kbd.h`

- **Status:** accepted-normalized
- **Batch:** `scraping-investigation-08`
- **Partitions:** KeyboardAndMouseInput
- **Delta:** No retained patch previously existed. Header-scraping investigation (x86
  scrape, the viable architecture for this `ExcludeFromCrossarch` partition) succeeded with
  0 warnings/errors. The generated `KeyboardAndMouseInput.cs` contains no `DllImport`/
  function declarations — this header declares only keyboard-layout DLL data-table
  structures (`VK_TO_WCHAR_TABLE`, `VSC_LPWSTR`, etc.), consumed by kbd-layout DLLs, not
  called directly.
- **Resolution:** No patch needed — there is no function surface to annotate.
- **Artifact:** none (no code change required).
- **Evidence:** Live scrape (`dotnet build generation/WinSDK -c Release -p:ScanArch=x86
  -t:ScrapeHeaders -p:PartitionFilter=KeyboardAndMouseInput`) succeeds with 0
  warnings/errors; generated `obj/generated/common/KeyboardAndMouseInput.cs` contains no
  `DllImport`/`public static extern` entries.
- **Normalization:** No normalization required; header is data-structure-only.
