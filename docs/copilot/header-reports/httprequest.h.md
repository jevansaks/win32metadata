# `httprequest.h`

- **Status:** accepted-normalized
- **Partitions:** WinHttp
- **Batch:** `scraping-investigation-05`
- **Delta:** No retained patch previously existed. Header-scraping investigation (this
  partition is **not** in `ExcludeFromCrossarch`; scraped with x86, one of its
  architecture-specific outputs) succeeded, producing only the pre-existing cross-partition
  auto-remap-consistency warnings already documented for `ual.h`/`winenclave.h`
  (`in_addr`/`sockaddr`/`timeval`/`_CERT_CONTEXT`), unrelated to this header. This header
  declares only the `IWinHttpRequest` COM interface (`__interface`); static review confirms
  no `DECLARE_HANDLE` typedefs.
- **Resolution:** No patch needed — COM interface pointers use standard `_Outptr_`
  semantics, not raw-handle ownership.
- **Artifact:** none (no code change required).
- **Evidence:** Live scrape (`dotnet build generation/WinSDK -c Release -p:ScanArch=x86
  -t:ScrapeHeaders -p:PartitionFilter=WinHttp`) succeeds. Static review confirms no
  `DECLARE_HANDLE` typedefs in this header.
- **Normalization:** No normalization required.
