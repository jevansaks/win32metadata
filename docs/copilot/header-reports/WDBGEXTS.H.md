# WDBGEXTS.H — accepted-normalized

**Partition:** Debug.Extensions
**Size:** ~83013 bytes

## Analysis

`wdbgexts.h` ("prototypes and data types for a user to write a
debugger extension DLL") declares the callback contract functions
(`ReadPhysical`, `WritePhysical`, `ReadMsr`, `SetThreadForOperation`,
etc.) provided by the debugger engine (WinDbg/KD) to extension DLLs
via the `ExtensionApis` function-pointer table — these are called BY
the debugger host, not exported system APIs for applications to
P/Invoke directly. No representable producer-site ownership pattern.

## Conclusion

Clean. No patch required — debugger-extension callback contract, no
representable ownership gap.
