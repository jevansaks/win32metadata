# werapi.h — accepted-normalized

**Partition:** Wer
**Size:** ~31714 bytes

## Analysis

`werapi.h` (Windows Error Reporting) declares `WerReportCreate`
(producer, `_Out_ HREPORT *phReportHandle`) and `WerReportCloseHandle`
(destroyer). Checked `autoTypes.json` first: **`HREPORT` already has
a complete entry** (`ValueType: DECLARE_HANDLE`, `CloseApi:
WerReportCloseHandle`, `InvalidHandleValues: [-1, 0]`) — producer-site
ownership metadata is already correctly in place. No gap.

## Conclusion

Clean. No patch required — existing `HREPORT` metadata already
correct and producer-site (not typedef-site).
