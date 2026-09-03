# portabledevice.h

**Classification:** accepted-normalized (no producer-site change needed)

## Summary
Windows Portable Devices (WPD) driver-support header. Contains
mostly COM interfaces plus 2 macro-generated inline helper functions
(`IsCommandInWpdCommandAccessMap`/`VerifyWpdCommandAccessFromMap`,
emitted via `DECLARE_IS_COMMAND_IN_WPD_COMMAND_ACCESS_MAP()`/
`DECLARE_VERIFY_WPD_COMMAND_ACCESS`), which are driver-side WPD-Command
access-map validation helpers with no handle-ownership pattern (they
inspect a caller-provided lookup table, no resource is produced or
freed).

## Ownership Analysis
No producer/consumer handle pattern exists in either the COM interfaces
or the two macro-generated helper functions. No `autoTypes.json`/
`emitter.settings.rsp` changes required.

## Action
None. Classified accepted-normalized with no patch artifact.
