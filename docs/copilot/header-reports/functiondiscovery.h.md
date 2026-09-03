# Header Report: functiondiscovery.h

## Partitions
`FunctionDiscovery`

## Ownership audit (producer-site-only policy)
- Pure MIDL-generated header declaring only forward class declarations (`FunctionDiscovery`, `PropertyStore`, `FunctionInstanceCollection`, `PropertyStoreCollection`), `CLSID_*`/`LIBID_*` constants, and MIDL boilerplate (`RPC_IF_HANDLE` ifspec externs). No functions.

## Conclusion
`accepted-normalized` — no ownership annotation required (CLSID constants + MIDL boilerplate only, no functions).
