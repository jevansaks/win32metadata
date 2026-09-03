# Header Report: identitycommon.h

## Partitions
`IdentityProvider`

## Scrape validation
- Re-scraped `IdentityProvider` partition (`ScanArch=x86`) after touching `main.cpp`.
- Result: `Build succeeded. 0 Warning(s). 0 Error(s).`

## Ownership audit (producer-site-only policy)
- MIDL-generated header declaring only the `IDENTITY_TYPE` enum and standard MIDL boilerplate (`RPC_IF_HANDLE` ifspec externs). No functions, no handles.

## Conclusion
`accepted-normalized` — no ownership annotation required (enum + MIDL boilerplate only, no functions).
