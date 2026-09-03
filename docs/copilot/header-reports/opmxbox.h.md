# Header Report: opmxbox.h

## Partitions
`Mf`

## Scrape validation
- Re-scraped `Mf` partition (`ScanArch=x86`) after touching `main.cpp`.
- Result: `Build succeeded. 0 Warning(s). 0 Error(s).`

## Ownership audit (producer-site-only policy)
- Three functions: `OPMXboxEnableHDCP(OPM_HDCP_TYPE)`, `OPMXboxGetHDCPStatus(OPM_HDCP_STATUS*)`, `OPMXboxGetHDCPStatusAndType(OPM_HDCP_STATUS*, OPM_HDCP_TYPE*)`. All outputs are plain enum values. No `HANDLE` involved.

## Conclusion
`accepted-normalized` — no ownership annotation required (enum-output-only API, no handle).
