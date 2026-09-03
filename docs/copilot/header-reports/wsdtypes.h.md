# wsdtypes.h — accepted-normalized

**Partition:** FunctionDiscovery
**Size:** ~19785 bytes

## Analysis

`wsdtypes.h` (auto-generated WSDAPI built-in type definitions)
declares forward-declared struct typedefs (`WSD_DURATION`,
`WSD_DATETIME`, `WSD_HANDLER_CONTEXT`, `WSD_EVENT`) and the
`WSD_STUB_FUNCTION` function-pointer typedef — out of scope per the
function-pointer-typedef convention (blocker-class 4). It contains
**no `extern`/`DllImport` function declarations**.

## Conclusion

Clean. No scrapable functions, no opaque handle production. No patch
required.
