# Header Report: mtxdm.h

## Partitions
`Cos`

## Scrape validation
- Re-scraped `Cos` partition (`ScanArch=x86`) after touching `main.cpp`.
- Result: `Build succeeded. 0 Warning(s). 0 Error(s).`

## Ownership audit (producer-site-only policy)
- Single function: `HRESULT GetDispenserManager(IDispenserManager** ppDispenserManager)` — a COM-factory pattern (output via a double-pointer to a COM interface), consistent with the clean COM-factory pattern established this session. Also `#include "comsvcs.h"` (already `accepted-normalized`). No raw `HANDLE` involved.

## Conclusion
`accepted-normalized` — no ownership annotation required (COM-factory pattern).
