# Header Report: workspaceruntimeclientext.h

## Partitions
`TermServ`

## Ownership audit (producer-site-only policy)
- Declares only the COM interface `IWorkspaceClientExt` (`GetResourceId`/`GetResourceDisplayName` output `BSTR*`, standard COM string ownership convention; `IssueDisconnect` has no outputs). COM vtable method — out of scope; no `extern`/`DllImport` functions.

## Conclusion
`accepted-normalized` — no ownership annotation required (COM interface method only, out of scope).
