# hidpi.h — accepted-normalized

**Partition:** Hid
**Size:** ~94028 bytes

## Analysis

`hidpi.h` (HID Parsing API) declares `HidP_GetCaps` and similar
functions that **consume** an existing `PHIDP_PREPARSED_DATA` (input
only, not produced here — obtained via `HidD_GetPreparsedData` and
freed via `HidD_FreePreparsedData`, both already covered: `CloseApi:
HidD_FreePreparsedData` already exists in `autoTypes.json`) and output
caller-allocated structs (`PHIDP_CAPS`, etc.). No new handle
production or gap.

## Conclusion

Clean. No patch required — existing preparsed-data ownership metadata
already correct; this header only consumes it.
