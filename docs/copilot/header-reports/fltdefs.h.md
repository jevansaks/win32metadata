# fltdefs.h — accepted-normalized (patch applied)

**Partitions:** FileHistory, IpHlp (`Windows.Win32.NetworkManagement.IpHelper`)
**Size:** ~14428 bytes

## Analysis

`fltdefs.h` (legacy Win32 IP packet-filter API) declares two
distinctly-named `typedef PVOID` handle types:

- `INTERFACE_HANDLE` — produced by `PfCreateInterface(..., _Out_
  INTERFACE_HANDLE *ppInterface)`, freed via
  `PfDeleteInterface(INTERFACE_HANDLE pInterface)`. Clean 1:1
  alloc/free pair, confirmed used only in this file. No prior
  `autoTypes.json` entry.
- `FILTER_HANDLE` — produced (as an array) by
  `PfAddFiltersToInterface`, consumed only via bulk removal
  (`PfRemoveFilterHandles(INTERFACE_HANDLE pInterface, DWORD
  cFilters, PFILTER_HANDLE pvHandles)`), which takes an *array* of
  filter handles alongside the owning interface handle — not a clean
  1:1 single-handle close API. Left unannotated; no safe single
  `CloseApi` representable.

## Patch

Edited `generation/WinSDK/autoTypes.json` — added a new entry for
`INTERFACE_HANDLE` (`ValueType: IntPtr`, `CloseApi:
PfDeleteInterface`) under namespace
`Windows.Win32.NetworkManagement.IpHelper`.

## Validation

Re-scraped the `IpHlp` partition (x64, not `ExcludeFromCrossarch`)
after the `main.cpp` touch: build succeeded, 0 errors (6 pre-existing
cross-partition remap warnings for unrelated types — `in6_addr`,
`in_addr`, `sockaddr`, `timeval`, `_CERT_CONTEXT` — are unchanged by
this patch).

## Conclusion

Genuine producer-site fix applied for `INTERFACE_HANDLE`.
`FILTER_HANDLE` left unannotated (no single-handle close API).
