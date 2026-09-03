# Header Report: messagedispatcherapi.h

## Partitions
`ComOle`, `WinRT`

## Scrape validation
- Re-scraped `ComOle` partition (`ScanArch=x86`) after touching `main.cpp`.
- Result: `Build succeeded. 0 Warning(s). 0 Error(s).`

## Ownership audit (producer-site-only policy)
- Two functions: `CoSetMessageDispatcher(_In_opt_ PMessageDispatcher pMessageDispatcher)` (takes an existing `IMessageDispatcher*` as input — does not create/own it) and `CoHandlePriorityEventsFromMessagePump()` (no parameters). No `HANDLE` involved; the COM interface pointer is caller-supplied, not produced here.

## Conclusion
`accepted-normalized` — no ownership annotation required (interface pointer is an input, not a production site).
