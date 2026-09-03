# Header Report: sqlspi.h

## Partitions
`Search`

## Ownership audit (producer-site-only policy)
- All `SQLSetConnectAttrForDbcInfoW/A`/`SQLSetDriverConnectInfoW/A`/`SQLSetConnectInfoW/A`/`SQLGetPoolID`/`SQLRateConnection`/`SQLPoolConnectW/A`/`SQLCleanupConnectionPoolID` take `SQLHDBC_INFO_TOKEN` (a `SQLHANDLE` alias) as an `_In_` parameter only — the token is produced elsewhere via `SQLAllocHandle(SQL_HANDLE_DBC_INFO_TOKEN, ...)`, ODBC's own generic multi-purpose handle type (managed by a shared `SQLAllocHandle`/`SQLFreeHandle` pair across Connection/Statement/Environment/Descriptor handles) — out of scope, same class as the Win32 generic `HANDLE` precedent.
- `SQLGetPoolID`'s `POOLID*` output is a plain numeric ID (`typedef SQLULEN POOLID`), not a handle.

## Conclusion
`accepted-normalized` — no ownership annotation required (no function in this header produces the generic ODBC handle type; all reference it as pre-owned input).
