# Header Report: prdrvcom.h

## Partitions
`Printing`

## Ownership audit (producer-site-only policy)
- Declares only `DECLARE_INTERFACE_IID_`-style COM interfaces (`IPrintTicketProvider`, `IPrintTicketProvider2`) — pure vtable methods. `hPrinter` is a pre-owned caller input in `GetSupportedVersions`/`BindPrinter`. Other outputs are `BSTR`/`IXMLDOMDocument2**`/transparent `PDEVMODE` — standard COM/generic-buffer conventions, out of scope. No extern functions.

## Conclusion
`accepted-normalized` — no ownership annotation required (COM interface methods only, out of scope).
