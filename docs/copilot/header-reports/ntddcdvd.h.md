# `ntddcdvd.h`

- **Status:** accepted-normalized
- **Batch:** `scraping-investigation-07`
- **Partitions:** Devices.Dvd
- **Delta:** No retained patch previously existed. Header-scraping investigation (x86
  scrape, the viable architecture for this `ExcludeFromCrossarch` partition) succeeded with
  0 warnings/errors. The generated `Devices.Dvd.cs` contains no `DllImport`/function
  declarations — this header declares only DVD-ROM/CSS IOCTL constants and data structures.
- **Resolution:** No patch needed — there is no function surface to annotate.
- **Artifact:** none (no code change required).
- **Evidence:** Live scrape (`dotnet build generation/WinSDK -c Release -p:ScanArch=x86
  -t:ScrapeHeaders -p:PartitionFilter=Devices.Dvd`) succeeds with 0 warnings/errors;
  generated `obj/generated/common/Devices.Dvd.cs` contains no `DllImport`/`public static
  extern` entries.
- **Normalization:** No normalization required; header is data-structure-only.
