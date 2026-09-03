# Header Report: sqltypes.h

## Partitions
`Search`

## Ownership audit (producer-site-only policy)
- Pure ODBC type typedefs (`SQLCHAR`, `SQLINTEGER`, etc.) and version constants. No functions (the actual ODBC API functions live in separate headers, e.g. `sql.h`/`sqlext.h`).

## Conclusion
`accepted-normalized` — no ownership annotation required (type definitions only, no functions).
