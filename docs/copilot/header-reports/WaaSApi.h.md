# Header Report: WaaSApi.h

## Partitions
`UpdateAssessment`

## Ownership audit (producer-site-only policy)
- Declares only the COM interface `IWaaSAssessor` (single method `GetOSUpdateAssessment`, outputting a transparent `OSUpdateAssessment` struct via `_Out_retval_`) plus `CLSID_WaaSAssessor`/`LIBID_WaaSAssessorLib` constants. COM vtable method — out of scope; no `extern`/`DllImport` functions.

## Conclusion
`accepted-normalized` — no ownership annotation required (COM interface + GUID constants only, no extern functions).
