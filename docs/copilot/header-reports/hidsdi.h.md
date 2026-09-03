# hidsdi.h — accepted-normalized (patch applied)

**Partition:** Hid (`Windows.Win32.Devices.HumanInterfaceDevice`)
**Size:** ~16103 bytes

## Analysis

`hidsdi.h` declares the HID public API (`HidD_GetAttributes`,
`HidD_GetPreparsedData`, `HidD_FreePreparsedData`, `HidD_FlushQueue`,
`HidD_GetConfiguration`/`SetConfiguration`, feature/input/output
report accessors, string accessors).

`HidD_GetPreparsedData(_In_ HANDLE HidDeviceObject, _Out_
PHIDP_PREPARSED_DATA *PreparsedData)` produces a `PHIDP_PREPARSED_DATA`
(defined in the sibling `hidpi.h` as `typedef struct
_HIDP_PREPARSED_DATA * PHIDP_PREPARSED_DATA;` — genuinely opaque, no
struct body defined anywhere), freed via
`HidD_FreePreparsedData(PHIDP_PREPARSED_DATA PreparsedData)`.

An `autoTypes.json` entry for `PHIDP_PREPARSED_DATA` already existed
(used across `hidclass.h` (accepted-normalized, constants-only),
`hidpi.h` (still pending, declares the type and `HidP_*` parsing
functions that consume it as input), and this file) but was **missing
`CloseApi`**.

## Patch

Edited `generation/WinSDK/autoTypes.json` — added
`"CloseApi": "HidD_FreePreparsedData"` to the existing
`PHIDP_PREPARSED_DATA` entry.

## Validation

Re-scraped the `Hid` partition (x64, not `ExcludeFromCrossarch`) after
the `main.cpp` touch: build succeeded, 0 warnings, 0 errors.

## Conclusion

Genuine producer-site fix applied — completed a pre-existing
incomplete `autoTypes.json` entry, correctly attributing the close API
to its producer/consumer site in this header.
