# sqlext.h — accepted-normalized

**Partition:** Search
**Size:** ~93864 bytes

## Analysis

`sqlext.h` (ODBC extended API) declares `SQLAllocHandleStd` which
uses the same generic, multi-type-discriminated `SQLHANDLE` pattern
as `SQLAllocHandle` in the sibling `sql.h` (already documented as
blocker-class 2, generic/shared type — deferred, no new distinct
type introduced here). All other functions
(`SQLDriverConnect`, `SQLBrowseConnect`, etc.) consume existing
`SQLHDBC`/`SQLHSTMT` handles (already covered:
`SQLHDBC`→`SQLFreeConnect` fixed in batch 169; `SQLHSTMT` documented
blocked) — no new production.

## Conclusion

Clean. No patch required — consistent with the existing `sql.h`
ODBC handle-ownership audit; no new representable gap.
