# Header Report: DiagnosticDataQueryTypes.h

## Partitions
`Security.DiagnosticDataQuery`

## Ownership audit (producer-site-only policy)
- Declares only enums/structs (`DdqAccessLevel`, `DIAGNOSTIC_DATA_RECORD`, `DIAGNOSTIC_DATA_SEARCH_CRITERIA`, `DIAGNOSTIC_REPORT_DATA`, etc.) plus one RPC context-handle type: `typedef /* [context_handle] */ void *DIAGNOSTIC_DATA_QUERY_SESSION`. No functions are declared in this "Types" header — the actual producer/consumer functions for `DIAGNOSTIC_DATA_QUERY_SESSION` live in the separate, still-`pending` `DiagnosticDataQuery.h` (confirmed via grep: only these two files reference the type). Ownership audit for this context-handle type is deferred to that header, per the same deferral pattern established for `HSTRING`/`winstring.h`.

## Conclusion
`accepted-normalized` — no ownership annotation required in this header (types/RPC boilerplate only, no functions); `DIAGNOSTIC_DATA_QUERY_SESSION` ownership will be audited when `DiagnosticDataQuery.h` is classified.
