# msi.h — accepted-normalized

**Partition:** Setup
**Size:** ~125207 bytes

## Analysis

`msi.h` declares `MsiOpenPackageA/W`/`MsiOpenPackageExA/W` (producers
of `_Out_ MSIHANDLE *hProduct`). Checked `autoTypes.json` first:
**`MSIHANDLE` already has a complete entry** (`CloseApi:
MsiCloseHandle`) — producer-site ownership metadata is already
correctly in place. No gap.

## Conclusion

Clean. No patch required — existing `MSIHANDLE` metadata already
correct and producer-site (not typedef-site).
