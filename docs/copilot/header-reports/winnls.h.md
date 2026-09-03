# winnls.h — accepted-normalized

**Partition:** Intl
**Size:** ~129219 bytes

## Analysis

`winnls.h` (National Language Support procedures/constants) declares
locale/string-conversion query functions (`GetLocaleInfoW`,
`LCMapStringW`, etc.). Confirmed via grep that there are **no
handle-producing functions** — all outputs are plain buffers/values.

## Conclusion

Clean. No patch required.
