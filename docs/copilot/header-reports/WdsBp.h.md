# Header Report: WdsBp.h

## Partitions
`Wds`

## Ownership audit (producer-site-only policy)
- `WdsBpParseInitialize`/`WdsBpParseInitializev6`/`WdsBpInitialize` all produce a generic `HANDLE` via `_Out_ HANDLE *phHandle`, closed via `WdsBpCloseHandle(_In_ HANDLE hHandle)` — the already-established **generic-type direct-out-param** blocker class (`wslapi.h`/`ratings.h`/`avrt.h`/`powersetting.h`/`ondemandconnroutehelper.h`/`davclnt.h`/`RTWorkQ.h`/`FaxDev.h`/`wdstpdi.h`/`prnasnot.h`/`Wscapi.h`).
- `WdsBpQueryOption`/`WdsBpAddOption`/`WdsBpGetOptionBuffer` take the handle as `_In_` input and output plain buffers, no additional gaps.

## Conclusion
`blocked` — genuine gap in `WdsBpParseInitialize`/`WdsBpParseInitializev6`/`WdsBpInitialize`/`WdsBpCloseHandle` (generic `HANDLE` direct-out-param, reuses established blocker class).
