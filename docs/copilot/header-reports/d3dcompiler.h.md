# `d3dcompiler.h`

- **Status:** accepted-normalized
- **Batch:** `scraping-investigation-08`
- **Partitions:** Direct3DFxc
- **Delta:** No retained patch previously existed. Header-scraping investigation (x86
  scrape, the viable architecture for this `ExcludeFromCrossarch` partition) succeeded with
  0 warnings/errors. This header declares the legacy HLSL Effects Compiler (FXC)/
  `D3DCompiler_47.dll` API surface (`D3DCompile`, `D3DReflect`, etc.; 25 functions); static
  review confirms no `DECLARE_HANDLE` typedefs and no `_Out_`/`_Outptr_ HANDLE`
  output-parameter patterns — all output is via `ID3DBlob`/COM-interface `_Outptr_`
  parameters, not raw handles.
- **Resolution:** No patch needed — there is no ownership-relevant function surface to
  annotate.
- **Artifact:** none (no code change required).
- **Evidence:** Live scrape (`dotnet build generation/WinSDK -c Release -p:ScanArch=x86
  -t:ScrapeHeaders -p:PartitionFilter=Direct3DFxc`) succeeds with 0 warnings/errors. Static
  review confirms no ownership-relevant patterns.
- **Normalization:** No normalization required.
