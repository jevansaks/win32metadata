# httpext.h — accepted-normalized

**Partition:** Iis
**Size:** ~20152 bytes

## Analysis

`HttpExt.h` (ISAPI HTTP Server Extension interface) declares
transparent structs (`EXTENSION_CONTROL_BLOCK`, etc.) consumed by
extension-DLL-implemented entry points (`HttpExtensionProc`,
implemented BY the ISAPI DLL, not imported). It contains **no
`extern`/`DllImport` function declarations** (0 `WINAPI`/`STDAPI`
matches).

## Conclusion

Clean. No scrapable functions, no opaque handle production. No patch
required.
