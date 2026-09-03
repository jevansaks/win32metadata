# Header Report: lowlevelmonitorconfigurationapi.h

## Partitions
`Monitor`

## Scrape validation
- Re-scraped `Monitor` partition (`ScanArch=x86`) after touching `main.cpp`.
- Result: `Build succeeded. 0 Warning(s). 0 Error(s).`

## Ownership audit (producer-site-only policy)
- All six functions (`GetVCPFeatureAndVCPFeatureReply`, `SetVCPFeature`, `SaveCurrentSettings`, `GetCapabilitiesStringLength`, `CapabilitiesRequestAndCapabilitiesReply`, `GetTimingReport`) take `HANDLE hMonitor` as an **`_In_` (consumer) parameter only** — none of them produce or release a physical-monitor handle.
- The actual physical-monitor handle producer/consumer pair (`GetPhysicalMonitorsFromHMONITOR`/`DestroyPhysicalMonitors`) lives in `PhysicalMonitorEnumerationAPI.h` (`#include`d by this header), which is already separately tracked in the ledger (still `pending`) — that is where any future producer-site ownership work belongs, not here.

## Conclusion
`accepted-normalized` — this header's functions only *consume* an already-open physical-monitor `HANDLE` (never produce/release one). The producer/consumer pair itself is tracked separately under `PhysicalMonitorEnumerationAPI.h` (still pending).
