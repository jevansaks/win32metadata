# Header Report: windows.graphics.effects.interop.h

## Partitions
`WinRT.Direct2D`

## Scrape validation
- `WinRT.Direct2D` partition previously re-scraped this session (batch `scraping-investigation-28`); result `0 Warning(s), 0 Error(s)`.

## Ownership audit (producer-site-only policy)
- `IGraphicsEffectD2D1Interop` COM interface: `GetEffectId`/`GetNamedPropertyMapping`/`GetPropertyCount`/`GetSourceCount` output plain `GUID`/`UINT`/enum values; `GetProperty`/`GetSource` output COM interface pointers via `_Outptr_` (standard COM factory pattern). No raw `HANDLE` involved.

## Conclusion
`accepted-normalized` — no ownership annotation required (COM interface with plain-value/interface-pointer outputs only).
