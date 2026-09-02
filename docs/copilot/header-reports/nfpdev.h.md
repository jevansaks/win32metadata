# `nfpdev.h`

- **Status:** accepted-normalized
- **Batch:** `scraping-investigation-11`
- **Partitions:** Nfp
- **Delta:** No retained patch previously existed. Header-scraping investigation (x86
  scrape, the viable architecture for this `ExcludeFromCrossarch` partition) succeeded with
  0 warnings/errors. The generated `Nfp.cs` contains no `DllImport`/function declarations —
  this header declares only NFC Proximity device-interface GUIDs and IOCTL constants.
- **Resolution:** No patch needed — there is no function surface to annotate.
- **Artifact:** none (no code change required).
- **Evidence:** Live scrape (`dotnet build generation/WinSDK -c Release -p:ScanArch=x86
  -t:ScrapeHeaders -p:PartitionFilter=Nfp`) succeeds with 0 warnings/errors; generated
  `obj/generated/common/Nfp.cs` contains no `DllImport`/`public static extern` entries.
- **Normalization:** No normalization required; header is constant-only.
