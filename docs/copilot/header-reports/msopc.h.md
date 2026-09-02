# `msopc.h`

- **Status:** accepted-normalized
- **Batch:** `scraping-investigation-12`
- **Partitions:** Opc
- **Delta:** No retained patch previously existed. Header-scraping investigation (x86
  scrape, the viable architecture for this `ExcludeFromCrossarch` partition) succeeded with
  0 warnings/errors (only the same pre-existing cross-partition auto-remap-consistency
  warning already documented for `ual.h`, unrelated to this header). The generated `Opc.cs`
  contains no `DllImport`/function declarations — this header declares only the Open
  Packaging Conventions (OPC) COM interface types (`IOpcFactory`, etc.), with no free
  functions.
- **Resolution:** No patch needed — there is no function surface to annotate.
- **Artifact:** none (no code change required).
- **Evidence:** Live scrape (`dotnet build generation/WinSDK -c Release -p:ScanArch=x86
  -t:ScrapeHeaders -p:PartitionFilter=Opc`) succeeds with 0 warnings/errors beyond the
  pre-existing cross-partition remap notice; generated `obj/generated/common/Opc.cs`
  contains no `DllImport`/`public static extern` entries.
- **Normalization:** No normalization required.
