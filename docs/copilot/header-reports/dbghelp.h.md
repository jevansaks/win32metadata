# dbghelp.h — accepted-normalized

**Partition:** FileHistory, WinProg
**Size:** ~124135 bytes

## Analysis

`dbghelp.h` (Debug Help Library) declares `SymInitialize(HANDLE
hProcess, ...)`/`SymCleanup(HANDLE hProcess)` — both **consume** an
application-provided process `HANDLE` as a lookup key for internal
symbol-tracking state; `SymInitialize` does not produce a new handle,
and `SymCleanup` just clears the internal tracking associated with
that key. No producer/consumer handle-ownership pattern here.

## Conclusion

Clean. No patch required.
