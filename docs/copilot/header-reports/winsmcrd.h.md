# winsmcrd.h — accepted-normalized

**Partition:** Credentials
**Size:** ~14216 bytes

## Analysis

`winsmcrd.h` (`shared\winsmcrd.h`) declares Smart Card class/port
IOCTL codes and transparent data structures shared between user mode
and kernel mode. It contains **no function declarations**
(`WINAPI`/`STDAPI` count: 0).

## Conclusion

Clean. No functions, no opaque handle production. No patch required.
