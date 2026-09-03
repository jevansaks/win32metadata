# sqlucode.h — accepted-normalized

**Partition:** Search
**Size:** ~28812 bytes

## Analysis

`sqlucode.h` (ODBC Unicode/ANSI function prototypes) declares ODBC
functions (`SQLConnectW`, `SQLDescribeColW`, `SQLExecDirectW`, etc.)
that all **consume** existing handles (`SQLHDBC`, `SQLHSTMT`,
`SQLHENV`) passed in by the caller — none allocate/produce a new
handle (confirmed via grep for `_Out_.*SQLH\w+` — no matches). The
actual handle producer (`SQLAllocHandle`) lives in the still-pending
sibling header `sql.h`, which is where any future ODBC handle-ownership
audit belongs (deferred-to-sibling-header pattern, blocker-class 8).

## Conclusion

Clean. No patch required.
