# Header Report: eapauthenticatortypes.h

## Partitions
`EapHost`

## Scrape validation
- `EapHost` partition previously re-scraped this session (batch `scraping-investigation-25`); result `0 Warning(s), 0 Error(s)`.

## Ownership audit (producer-site-only policy)
- MIDL-generated header declaring only the `EAP_AUTHENTICATOR_SEND_TIMEOUT` enum and standard MIDL boilerplate (`RPC_IF_HANDLE` ifspec externs). No functions, no handles.

## Conclusion
`accepted-normalized` — no ownership annotation required (enum + MIDL boilerplate only, no functions).
