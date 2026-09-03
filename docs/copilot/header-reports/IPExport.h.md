# Header Report: IPExport.h

## Partitions
`IpHlp`

## Ownership audit (producer-site-only policy)
- Declares only IP type aliases (`IPAddr`/`IPMask`/`IP_STATUS`) and transparent data structs (`IP_OPTION_INFORMATION`, etc.). No functions.

## Conclusion
`accepted-normalized` — no ownership annotation required (type aliases/structs only, no functions).
