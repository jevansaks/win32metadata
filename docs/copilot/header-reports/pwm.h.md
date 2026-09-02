# `pwm.h`

- **Status:** accepted-normalized
- **Batch:** `scraping-investigation-01`
- **Partitions:** PWM
- **Delta:** No retained patch previously existed. Header-scraping investigation (x86
  scrape, the viable architecture for this `ExcludeFromCrossarch` partition) succeeded with
  0 warnings/errors. The generated `PWM.cs` contains only structs and an enum (IOCTL
  input/output payloads for the PWM controller driver interface) — no `DllImport`/function
  declarations. Confirmed directly in the header: it declares only 13 `CTL_CODE` IOCTL
  constants and data structures, no `WINAPI`-declared functions.
- **Resolution:** No patch needed — there is no function surface to annotate with
  `set-last-error`/`supported-os`/ownership metadata.
- **Artifact:** none (no code change required).
- **Evidence:** Live scrape (`dotnet build generation/WinSDK -c Release -p:ScanArch=x86
  -t:ScrapeHeaders -p:PartitionFilter=PWM`) succeeds with 0 warnings/errors; generated
  `obj/generated/common/PWM.cs` contains no `DllImport`/`public static extern` entries.
- **Normalization:** No normalization required; header is data-structure-only.
