# stralign.h — accepted-normalized

**Partitions:** FileHistory, WinProg
**Size:** ~16784 bytes

## Analysis

`stralign.h` (unaligned wide-character string helpers) declares
`ua_*` functions (`ua_wcscpy`, `ua_lstrcmp`, etc.) as **`static`
inline** functions with internal linkage — not exported/scrapable
`extern`/`DllImport` declarations. All operate on transparent string
pointer types (`PUWSTR`), not opaque handles.

## Conclusion

Clean. No scrapable functions, no opaque handle production. No patch
required.
