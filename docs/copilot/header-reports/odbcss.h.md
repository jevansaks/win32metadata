# odbcss.h — accepted-normalized

**Partition:** Search
**Size:** ~28616 bytes

## Analysis

`odbcss.h` (SQL Server ODBC driver-specific defines) declares only
`SQL_*` constants/macros and structs. Confirmed via grep (including
`^SQLRETURN$` for possible multi-line ODBC function declarations)
that there are **no function declarations**.

## Conclusion

Clean. No patch required.
