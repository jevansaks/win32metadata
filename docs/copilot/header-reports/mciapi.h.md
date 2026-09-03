# mciapi.h — accepted-normalized

**Partition:** Multimedia
**Size:** ~50310 bytes

## Analysis

`mciapi.h` (MCI ApiSet contract) declares `mciSendCommand`-family
functions using the generic `MCIDEVICEID`/`UINT` integer identifiers
(closed via the generic `mciSendCommand(..., MCI_CLOSE, ...)`
command, not a distinct free function), plus internal driver-support
functions `mciLoadCommandResource`/`mciFreeCommandResource` that pass
a plain `UINT wTable` — not a distinct opaque handle typedef. Matches
the established generic/shared-type blocker (blocker-class 2).

## Conclusion

Clean. No patch required — generic integer identifiers, no
representable handle-typedef gap.
