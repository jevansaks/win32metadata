# sql.h — accepted-normalized (producer-site fix applied; 2 items blocked)

**Partition:** Search
**Size:** ~34995 bytes

## Analysis

`sql.h` declares the core ODBC API. Checked `autoTypes.json` first:
**no entries existed** for any ODBC handle typedef (`SQLHENV`,
`SQLHDBC`, `SQLHSTMT`, `SQLHDESC` — all declared in the already
`accepted-normalized` sibling `sqltypes.h`, whose report explicitly
deferred to `sql.h`/`sqlext.h`). All 4 are `void*` aliases (via
`SQLHANDLE`) on Win32/x64.

**Producer-site fix applied** for the 2 types with a genuine
single-argument dedicated legacy close function:

| Handle | Producer | Close API (single-arg) |
|---|---|---|
| `SQLHENV` | `SQLAllocEnv(_Out_ SQLHENV*)` | `SQLFreeEnv(SQLHENV)` |
| `SQLHDBC` | `SQLAllocConnect(SQLHENV, _Out_ SQLHDBC*)` | `SQLFreeConnect(SQLHDBC)` |

Added 2 new `autoTypes.json` entries (`Namespace:
Windows.Win32.System.Search`, `ValueType: DECLARE_HANDLE` — pointer-
sized opaque handle — `InvalidHandleValues: [0]`, matching
`SQL_NULL_HENV`/`SQL_NULL_HDBC` = 0). Validated via `dotnet build
generation/WinSDK -c Release -p:ScanArch=x64 -t:ScrapeHeaders
-p:PartitionFilter=Search` → 0 errors.

**Blocked (not representable)**:
- `SQLHSTMT`: legacy close is `SQLFreeStmt(SQLHSTMT StatementHandle,
  SQLUSMALLINT Option)` — requires a **mandatory second parameter**
  (no `SQL_DROP`-only convenience overload exists in the header).
  The `RAIIFree` mechanism (confirmed via
  `NativeTypedefStructsCreator.cs`) has no support for supplying
  extra mandatory arguments to the close function — unrepresentable.
- `SQLHDESC`: has **no dedicated single-arg close function at all**
  (only the generic, multi-type-discriminated
  `SQLAllocHandle`/`SQLFreeHandle(SQLSMALLINT HandleType, SQLHANDLE
  Handle)`, matching blocker-class 2, generic/shared-type — same
  reasoning as the `winternl.h` generic `PHANDLE` case).

## Conclusion

Producer-site fix applied for `SQLHENV`/`SQLHDBC`. `SQLHSTMT` and
`SQLHDESC` remain out of scope for typedef-level ownership annotation
(genuinely unrepresentable — multi-arg close function / generic
discriminated handle, respectively), not classification gaps.
