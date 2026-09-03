# Header Report: amxmlgraphbuilder.h

## Partitions
`Media.DShow`, `Media.DShow.Xml`

## Scrape validation
- `Media.DShow` partition previously re-scraped this session (batch `scraping-investigation-19`); result `0 Warning(s), 0 Error(s)`.

## Ownership audit (producer-site-only policy)
- Declares `DEFINE_GUID(IID_IXMLGraphBuilder, ...)`, `DEFINE_GUID(CLSID_XMLGraphBuilder, ...)`, and the `IXMLGraphBuilder` COM interface (methods take caller-supplied `IGraphBuilder*`/`IXMLElement*` as input, or output a `BSTR*` — not a `HANDLE`). No raw `HANDLE` involved.

## Conclusion
`accepted-normalized` — no ownership annotation required (COM interface + GUID constants only).
