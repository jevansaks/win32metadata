# Header Report: dmprocessxmlfiltered.h

## Partitions
`WpdSdk`

## Scrape validation
- `WpdSdk` partition previously re-scraped this session (batch `scraping-investigation-26`); result `0 Warning(s), 0 Error(s)`.

## Ownership audit (producer-site-only policy)
- Single function: `DMProcessConfigXMLFiltered(..., _Outptr_result_z_ BSTR* pbstrXmlOut)`. Output is a `BSTR` (freed via `SysFreeString` per the header's own doc comment) — `BSTR` allocation is outside the scope of the `HANDLE`-specific `_Win32_metadata_raii_free_`/`_invalid_handle_` mechanism (consistent with other `BSTR`-producing functions already classified clean this session, e.g. `wab.h`'s `WABOpen`-family). No `HANDLE` involved.

## Conclusion
`accepted-normalized` — no ownership annotation required (BSTR output, not a HANDLE-family resource).
