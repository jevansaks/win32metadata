# Header Report: nsemail.h

## Partitions
`WinSock`

## Ownership audit (producer-site-only policy)
- Declares only enums (`NAPI_PROVIDER_TYPE`, `NAPI_PROVIDER_LEVEL`) and plain data structs (`NAPI_DOMAIN_DESCRIPTION_BLOB`, `NAPI_PROVIDER_INSTALLATION_BLOB`) describing an installation blob layout for email-naming NSPv2 providers (passed to `WSCInstallNameSpaceEx`, tracked elsewhere). No functions.

## Conclusion
`accepted-normalized` — no ownership annotation required (enums/structs only, no functions).
