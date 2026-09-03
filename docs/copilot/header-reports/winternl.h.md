# winternl.h — accepted-normalized

**Partition:** ActiveDirectory, Base, Certificates, FileHistory, IpHlp, MadCap, Security, Security.AppLocker (multi-partition)
**Size:** ~30360 bytes

## Analysis

`winternl.h` declares NT-internal syscalls (`NtCreateFile`,
`NtOpenFile`, `NtClose`, `NtQuerySystemInformation`, etc.), almost
all documented as "use the Win32 API instead" (i.e. thin
internal mirrors of already-metadata-covered Win32 APIs). Handle
producers (`NtCreateFile`/`NtOpenFile`) output via the fully generic
`OUT PHANDLE FileHandle` parameter type — confirmed via grep that
every `PHANDLE`/`HANDLE *` occurrence in the file uses the generic
`HANDLE` type, not a distinct named handle typedef.

This matches the established Generic/shared-type blocker
(blocker-class 2): `HANDLE` is used far too generically across the
entire SDK to receive a discriminating producer-site ownership
annotation without breaking every other generic `HANDLE` use.

## Conclusion

Clean (no representable ownership-metadata gap — generic `HANDLE`
output, blocker-class 2). No patch required.
