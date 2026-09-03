# vdmdbg.h — accepted-normalized

**Partitions:** FileHistory, VirtualDOSMachines
**Size:** ~14640 bytes

## Analysis

`vdmdbg.h` (VDM Debugger interface, legacy 16-bit Windows-on-Windows
debugging) declares many `VDM*` functions
(`VDMTerminateTaskWOW`, `VDMGetThreadSelectorEntry`, `VDMGetPointer`,
`VDMStartTaskInWOW`, `VDMEnumProcessWOW`, `VDMEnumTaskWOW`, etc.). All
handle-like parameters are legacy 16-bit `WORD` task/module/selector
values (`WORD htask`, `WORD hMod16`) or plain `DWORD dwProcessId` —
not opaque pointer-sized handle types. No opaque handle production
found.

## Conclusion

Clean. No opaque handle production. No patch required.
