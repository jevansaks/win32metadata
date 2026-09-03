# Header Report: Windows.Graphics.Capture.Interop.h

## Partitions
`WinRT.Graphics.Capture`

## Scrape validation
- Re-scraped `WinRT.Graphics.Capture` partition (`ScanArch=x86`) after touching `main.cpp`.
- Result: `Build succeeded. 0 Warning(s). 0 Error(s).`

## Ownership audit (producer-site-only policy)
- `IGraphicsCaptureItemInterop::CreateForWindow`/`CreateForMonitor` are clean COM QueryInterface-style factory methods (`_COM_Outptr_ void** result`); `HWND`/`HMONITOR` are caller-supplied inputs identifying an existing window/monitor, not produced here.
- `IWindowGraphicsCaptureItemInterop::GetWindow(HWND*)`/`IMonitorGraphicsCaptureItemInterop::GetMonitor(HMONITOR*)` are query-only outputs — `HWND`/`HMONITOR` are identifiers for existing system objects, not resources owned/released by this interface (consistent with the `ddraw.h`/`consoleapis.h` precedent that `HWND`/`HMONITOR` references are not ownership-bearing).

## Conclusion
`accepted-normalized` — no ownership annotation required (COM factory + query-only HWND/HMONITOR accessors).
