# windowsx.h — accepted-normalized

**Partition:** Controls, FileHistory, Gdi, WinProg
**Size:** ~75394 bytes

## Analysis

`windowsx.h` (Macro APIs, window message crackers, and control APIs)
is a **pure macro-only header** (`SNDMSG`, message-cracker macros
like `HANDLE_WM_*`). It contains **no function declarations**.

## Conclusion

Clean. No patch required.
