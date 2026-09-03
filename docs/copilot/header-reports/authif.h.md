# authif.h — accepted-normalized

**Partition:** Nps
**Size:** ~16898 bytes

## Analysis

`authif.h` (Internet Authentication Service extension interface)
declares only function-pointer typedefs
(`PRADIUS_EXTENSION_INIT`, `PRADIUS_EXTENSION_TERM`,
`PRADIUS_EXTENSION_PROCESS`, `PRADIUS_EXTENSION_PROCESS_EX`,
`PRADIUS_EXTENSION_FREE_ATTRIBUTES`, `PRADIUS_EXTENSION_PROCESS_2`)
and the transparent `RADIUS_EXTENSION_CONTROL_BLOCK` struct with
embedded function-pointer fields — out of scope per the
function-pointer-typedef convention (blocker-class 4). It contains
**no `extern`/`DllImport` function declarations**.

## Conclusion

Clean. No scrapable functions, no opaque handle production. No patch
required.
