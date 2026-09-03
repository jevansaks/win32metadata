# ddrawi.h — accepted-normalized

**Partition:** DirectDraw, FileHistory, WinProg
**Size:** ~143341 bytes

## Analysis

`ddrawi.h` ("DirectDraw internal header file, used by DirectDraw and
by display drivers") declares kernel/display-driver-interface
structs/tables (per doc comment, handles are deliberately stored as
`DWORD`s for 16-bit thunk compatibility — not a distinct opaque
handle typedef). Internal driver-interface contract, not an
application-callable API surface with representable ownership
metadata.

## Conclusion

Clean. No patch required.
