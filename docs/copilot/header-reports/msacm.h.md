# msacm.h — accepted-normalized

**Partition:** Audio
**Size:** ~58910 bytes

## Analysis

`msacm.h` (Audio Compression Manager) declares `acmDriverOpen`
(producer of `HACMDRIVER`) and `acmDriverClose(HACMDRIVER had, DWORD
fdwClose)` (destroyer, reserved 2nd param). Checked `autoTypes.json`
first: **`HACMDRIVER` already has a complete entry** (`CloseApi:
acmDriverClose`, `InvalidHandleValues: [-1, 0]`) — this is the exact
precedent that proved reserved-flags 2-param close functions are
tolerated by the metadata system (used to correct the `mscat.h`
`HCATADMIN` classification in this same batch). `HACMSTREAM` also
already correctly annotated (`CloseApi: acmStreamClose`).
`HACMDRIVERID`/`HACMOBJ` are identifiers/generic supertypes (not
directly opened/closed instances) — correctly have no `CloseApi`.

## Conclusion

Clean. No patch required — existing metadata already correct across
all ACM handle types.
