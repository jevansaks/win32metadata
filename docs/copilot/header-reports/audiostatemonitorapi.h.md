# Header Report: audiostatemonitorapi.h

## Partitions
`Audio`

## Scrape validation
- Re-scraped `Audio` partition (`ScanArch=x86`) after touching `main.cpp`.
- Result: `Build succeeded. 0 Warning(s). 0 Error(s).`

## Ownership audit (producer-site-only policy)
- `IAudioStateMonitor::RegisterCallback(..., _Out_ AudioStateMonitorRegistrationHandle* registration)` produces a handle (`typedef __int64 AudioStateMonitorRegistrationHandle`), released via `UnregisterCallback`. This is a **COM interface (vtable) method**, not an `extern "C"`/`DllImport` function — out of scope for the producer-site annotation mechanism (same reasoning as `GameInput.h`'s `IGameInputDispatcher::OpenWaitHandle`, batch `scraping-investigation-13`).
- `CreateRenderAudioStateMonitor*`/`CreateCaptureAudioStateMonitor*` are clean COM factory functions (`_Outptr_ IAudioStateMonitor**`).

## Conclusion
`accepted-normalized` — no ownership annotation required (the one real handle-producing function is a COM vtable method, out of scope for the annotation mechanism; all `extern` functions are COM factories).
