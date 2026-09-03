# patchwiz.h — accepted-normalized

**Partition:** Setup
**Size:** ~23497 bytes

## Analysis

`patchwiz.h` (Windows Installer PatchWiz) declares
`UiCreatePatchPackageA`/`W` and `UiCreatePatchPackageExA`/`W`. All
parameters are strings, `HWND`, `BOOL`, `DWORD` — no opaque handle
production.

## Conclusion

Clean. No opaque handle production. No patch required.
