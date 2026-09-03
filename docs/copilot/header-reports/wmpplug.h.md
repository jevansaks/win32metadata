# Header Report: wmpplug.h

## Partitions
`Wmp`

## Ownership audit (producer-site-only policy)
- Declares only the COM interface `IWMPPluginUI`. No `extern`/`DllImport` functions.

## Conclusion
`accepted-normalized` — no ownership annotation required (COM interface only, no extern functions).
