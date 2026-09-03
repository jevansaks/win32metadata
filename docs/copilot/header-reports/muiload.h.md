# Header Report: muiload.h

## Partitions
`Intl`

## Ownership audit (producer-site-only policy)
- `LoadMUILibraryA`/`LoadMUILibraryW` return `HINSTANCE` directly as the C return value, closed via `FreeMUILibrary(_In_ HMODULE hResModule)`. At first glance this resembles the established **return-value handle ownership** blocker class, but `autoTypes.json` already has complete entries for both `HMODULE` (`CloseApi: FreeLibrary`, `AlsoUsableFor: HINSTANCE`) and `HINSTANCE` (`CloseApi: FreeLibrary`, `AlsoUsableFor: HMODULE`) — this attaches ownership metadata to the type itself (single-purpose-type mechanism, not the newer per-producer inline annotation the corrected policy targets). `FreeMUILibrary` is a semantically-compatible alternate close function for the same type (it wraps `FreeLibrary`). No new annotation needed.
- `GetUILanguageFallbackList` writes into a caller-allocated buffer (`_Out_writes_opt_`), no ownership transfer.

## Conclusion
`accepted-normalized` — no ownership annotation required (`HINSTANCE`/`HMODULE` already correctly covered via existing `autoTypes.json` entry).
