# Header Report: mdmlocalmanagement.h

## Partitions
`MdmReg`

## Scrape validation
- Re-scraped `MdmReg` partition (`ScanArch=x86`) after touching `main.cpp`.
- Result: `Build succeeded. 0 Warning(s). 0 Error(s).`

## Ownership audit (producer-site-only policy)
- `RegisterDeviceWithLocalManagement(_Out_opt_ BOOL*)` and `UnregisterDeviceWithLocalManagement()` have no handle-typed parameters.
- `ApplyLocalManagementSyncML(_In_ PCWSTR, _Outptr_opt_result_maybenull_ PWSTR* syncMLResult)` outputs a plain string buffer, explicitly documented as "Caller is responsible releasing memory allocated with `LocalFree`" — a memory-allocation convention, not a `HANDLE`-family resource, out of scope for this mechanism.

## Conclusion
`accepted-normalized` — no ownership annotation required (no handle-typed values; string output uses `LocalFree`, not the HANDLE-specific mechanism).
