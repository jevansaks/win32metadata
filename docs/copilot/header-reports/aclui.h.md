# aclui.h — accepted-normalized

**Partition:** Authorization, Authorization.UI
**Size:** ~18120 bytes

## Analysis

`aclui.h` (ACLUI.DLL) declares the `ISecurityInformation` COM
interface plus `CreateSecurityPage(_In_ LPSECURITYINFO psi)` (returns
`HPROPSHEETPAGE`), `EditSecurity`, `EditSecurityAdvanced` (take
`HWND`/struct params only). `HPROPSHEETPAGE` already has a
**complete** `autoTypes.json` entry (`CloseApi:
DestroyPropertySheetPage`) — no gap.

## Conclusion

Clean. Ownership metadata already complete for the only handle type
involved. No patch required.
