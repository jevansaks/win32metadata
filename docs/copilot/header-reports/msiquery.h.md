# msiquery.h — accepted-normalized

**Partition:** Setup
**Size:** ~51561 bytes

## Analysis

`msiquery.h` (Windows Installer query API) uses `MSIHANDLE`
uniformly for all handle types (per doc comment: "Returned handles of
all types must be closed: MsiCloseHandle(h)"). Checked `autoTypes.json`
first: **`MSIHANDLE` already has a complete entry** (`ValueType:
uint`, `CloseApi: MsiCloseHandle`, `InvalidHandleValues: [-1, 0]`) —
producer-site ownership metadata is already correctly in place. No
gap.

## Conclusion

Clean. No patch required — existing `MSIHANDLE` metadata already
correct and producer-site (not typedef-site).
