# Header Report: diskguid.h

## Partitions
`Fs`

## Ownership audit (producer-site-only policy)
- Pure `DEFINE_GUID` constants for GPT partition-type GUIDs (`PARTITION_BASIC_DATA_GUID`, etc.). No functions.

## Conclusion
`accepted-normalized` — no ownership annotation required (GUID constants only, no functions).
