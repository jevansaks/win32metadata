# compstui.h — accepted-normalized

**Partition:** Printing
**Size:** ~172472 bytes

## Analysis

`compstui.h` (Common Property Sheet UI DLL) declares
`CommonPropertySheetUIA/W(HWND, PFNPROPSHEETUI, LPARAM, LPDWORD
pResult)` — outputs a plain `DWORD` result, not a handle. No
representable ownership pattern.

## Conclusion

Clean. No patch required.
