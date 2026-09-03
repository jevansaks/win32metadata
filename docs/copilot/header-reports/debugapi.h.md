# Header Report: debugapi.h

## Partitions
`Base`, `Debug`

## Ownership audit (producer-site-only policy)
- `IsDebuggerPresent`/`DebugBreak`/`OutputDebugStringA/W`/`ContinueDebugEvent`/`WaitForDebugEvent(Ex)`/`DebugActiveProcess(Stop)` take only plain `DWORD`/string/`LPDEBUG_EVENT` (caller-allocated struct) parameters. `CheckRemoteDebuggerPresent(_In_ HANDLE hProcess, _Out_ PBOOL)` takes `HANDLE` as a caller-supplied input only — not produced here.

## Conclusion
`accepted-normalized` — no ownership annotation required (HANDLE is a caller-supplied input, not produced here).
