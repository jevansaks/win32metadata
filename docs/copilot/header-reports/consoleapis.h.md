# Header Report: consoleapis.h

## Partitions
`Console`

## Scrape validation
- `Console` partition previously re-scraped this session (batch `scraping-investigation-22`); result `0 Warning(s), 0 Error(s)`.

## Ownership audit (producer-site-only policy)
- Declares `CONSOLECONTROL` enum, several plain data structs (`_CONSOLEENDTASK`, `_CONSOLEWINDOWOWNER`, `_CONSOLESETFOREGROUND`, `_CONSOLE_PROCESS_INFO`, `_CONSOLE_CARET_INFO`), and one function: `ConsoleControl(_In_ CONSOLECONTROL, _In_reads_bytes_(...) PVOID, _In_ DWORD)`.
- The `HANDLE`/`HWND` fields inside the data structs (`CONSOLEENDTASK.ProcessId`, `CONSOLESETFOREGROUND.hProcess`, `CONSOLEWINDOWOWNER.hwnd`, `CONSOLE_CARET_INFO.hwnd`) are all caller-supplied **input** fields describing an *existing* process/window — not produced or released by this header. `ConsoleControl` itself takes only a raw `PVOID` byte buffer, not a `HANDLE`.
- Note: this header is distinct from the genuine `CreateConsoleScreenBuffer`/`OpenConsoleW`/`DuplicateConsoleHandle`/`CloseConsoleHandle` ownership gap already recorded as `blocked` under `wincon.h` (batch `scraping-investigation-22`) — those functions are **not** declared in `consoleapis.h`.

## Conclusion
`accepted-normalized` — no ownership annotation required (data structs carry caller-supplied handle *references*, not produced/owned resources; `ConsoleControl` itself has no handle parameter).
