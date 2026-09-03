# fwpmu.h — accepted-normalized

**Partition:** WindowsFilteringPlatform
**Size:** ~125989 bytes

## Analysis

`fwpmu.h` (FWP management API) declares `FwpmEngineOpen0` (producer,
`_Out_ HANDLE* engineHandle`) and `FwpmEngineClose0(HANDLE)`
(destroyer). Checked `autoTypes.json` first: **`FWPM_ENGINE_HANDLE`
already has a complete entry** (a synthetic type via member-remap,
`CloseApi: FwpmEngineClose0`) — producer-site ownership metadata is
already correctly in place. No gap.

## Conclusion

Clean. No patch required — existing `FWPM_ENGINE_HANDLE` metadata
already correct and producer-site (not typedef-site).
