# Header Report: ndattrib.h

## Partitions
`Ndf`

## Ownership audit (producer-site-only policy)
- Declares only plain data structs (`OCTET_STRING`, `LIFE_TIME`, `SOCK_ADDR`, `HELPER_ATTRIBUTE`, `RootCauseInfo`, `RepairInfo`/`RepairInfoEx`, etc.) used as parameters by the Network Diagnostics Framework client API (`ndfapi.h`, already `accepted-normalized`). No functions or COM interfaces declared in this header.

## Conclusion
`accepted-normalized` — no ownership annotation required (data structs only, no functions).
