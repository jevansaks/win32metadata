# dsclient.h — accepted-normalized

**Partition:** ActiveDirectory
**Size:** ~20104 bytes

## Analysis

`dsclient.h` declares GUID constants (`CLSID_MicrosoftDS`,
`CLSID_DsPropertyPages`, etc.) and
`DsGetFriendlyClassName(LPCWSTR pszObjectClass, _Out_writes_(cchBuffer)
LPWSTR pszBuffer, UINT cchBuffer)` — a caller-allocated string buffer
output, not an opaque handle.

## Conclusion

Clean. No opaque handle production. No patch required.
