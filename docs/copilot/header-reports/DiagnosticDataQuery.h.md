# Header Report: DiagnosticDataQuery.h

## Partitions
`Security.DiagnosticDataQuery`

## Ownership audit (producer-site-only policy) — resolves deferred item from DiagnosticDataQueryTypes.h
- Declares six distinctly-named `DECLARE_HANDLE` opaque types, each with exactly one canonical close function: `HDIAGNOSTIC_DATA_QUERY_SESSION` (`DdqCreateSession`/`DdqCloseSession`), `HDIAGNOSTIC_REPORT` (`DdqGetDiagnosticReport`/`DdqFreeDiagnosticReport`), `HDIAGNOSTIC_EVENT_TAG_DESCRIPTION` (`DdqGetDiagnosticRecordLocaleTags`/`DdqFreeDiagnosticRecordLocaleTags`), `HDIAGNOSTIC_EVENT_PRODUCER_DESCRIPTION` (`DdqGetDiagnosticRecordProducers`/`DdqFreeDiagnosticRecordProducers`), `HDIAGNOSTIC_EVENT_CATEGORY_DESCRIPTION` (`DdqGetDiagnosticRecordProducerCategories`/`DdqFreeDiagnosticRecordProducerCategories`), `HDIAGNOSTIC_RECORD` (`DdqGetDiagnosticRecordPage`/`DdqFreeDiagnosticRecordPage`).
- `autoTypes.json` already has **complete entries for all six types**, each with a `CloseApi` exactly matching the corresponding free function found in this header. No new annotation needed.
- `DdqGetDiagnosticRecordPayload`'s `_Out_ PCWSTR* payload` (freed via `CoTaskMemFree` per comment) and `DdqGetDiagnosticRecordTagDistribution`/`BinaryDistribution`'s transparent struct-array outputs (also `CoTaskMemFree`-freed) are generic-buffer/transparent-struct conventions, out of scope.
- Note: this header's `DECLARE_HANDLE(HDIAGNOSTIC_*)` types are distinct from `DiagnosticDataQueryTypes.h`'s separate RPC context-handle typedef `DIAGNOSTIC_DATA_QUERY_SESSION` (no `H` prefix) — that type has no producer/consumer functions anywhere in the public API (RPC-marshaling infrastructure only), confirming the earlier deferral finding was correct and complete.

## Conclusion
`accepted-normalized` — no ownership annotation required; all six opaque handle types are already correctly covered via existing `autoTypes.json` entries (resolves the item deferred from `DiagnosticDataQueryTypes.h`).
