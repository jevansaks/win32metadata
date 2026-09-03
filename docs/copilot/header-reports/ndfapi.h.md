# Header Report: ndfapi.h

## Partitions
`Ndf`

## Ownership audit (producer-site-only policy) — genuine fix applied
- `NDFHANDLE` (`typedef void* NDFHANDLE`) is produced via `_Outptr_ NDFHANDLE*` out-params by many `NdfCreate*Incident` functions (`NdfCreateIncident`, `NdfCreateWinSockIncident`, `NdfCreateWebIncident`, `NdfCreateWebIncidentEx`, `NdfCreateSharingIncident`, `NdfCreateDNSIncident`, `NdfCreateConnectivityIncident`, `NdfCreateNetConnectionIncident`, `NdfCreatePnrpIncident`, `NdfCreateInboundIncident`, `NdfCreateGroupingIncident`), and consumed by `NdfExecuteDiagnosis`/`NdfDiagnoseIncident`/`NdfRepairIncident`/`NdfCancelIncident`/`NdfGetTraceFile`, all closed via the single canonical `NdfCloseIncident(NDFHANDLE handle)`. `autoTypes.json` had **no entry at all** for this type — a genuine gap (same class as `appnotify.h`/`WinHvEmulation.h`'s original fixes: absent entry, not merely incomplete). Confirmed via grep that `NDFHANDLE` is declared only in this one header (no cross-namespace conflict risk).
- Added a new `autoTypes.json` entry: `Name: NDFHANDLE`, `Namespace: Windows.Win32.NetworkManagement.NetworkDiagnosticsFramework` (matches the `Ndf` partition's namespace), `ValueType: IntPtr`, `CloseApi: NdfCloseIncident`, `InvalidHandleValues: [0]`. Re-scraped `Ndf` partition (0 errors). These APIs are marked deprecated in comments, but are still scraped as ordinary `extern` declarations and therefore still need correct ownership metadata.
- `NdfDiagnoseIncident`'s `_Outptr_result_buffer_(*RootCauseCount) RootCauseInfo** RootCauses` is a transparent struct array, out of scope.

## Conclusion
`accepted-normalized` — genuine gap fixed via new `autoTypes.json` entry for `NDFHANDLE` (`CloseApi: NdfCloseIncident`).
