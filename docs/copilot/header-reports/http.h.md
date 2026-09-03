# http.h — accepted-normalized (2 producer-site fixes applied)

**Partition:** HttpServer
**Size:** ~99935 bytes

## Analysis

`http.h` (HTTP Server API) declares several `HTTP_OPAQUE_ID`-derived
(`typedef ULONGLONG`) identifier typedefs. Checked `autoTypes.json`
first: **`HTTP_REQUEST_QUEUE_HANDLE` already has a correct entry**
(`CloseApi: HttpCloseRequestQueue`, via an existing member-remap
synthetic type) — no gap.

**Producer-site fixes applied** for two genuine, previously
unannotated producer/consumer pairs:
- **`HTTP_SERVER_SESSION_ID`** → produced by `HttpCreateServerSession`
  (`_Out_ PHTTP_SERVER_SESSION_ID`), destroyed by single-arg
  `HttpCloseServerSession(HTTP_SERVER_SESSION_ID)`.
- **`HTTP_URL_GROUP_ID`** → produced by `HttpCreateUrlGroup`
  (`_Out_ PHTTP_URL_GROUP_ID`), destroyed by single-arg
  `HttpCloseUrlGroup(HTTP_URL_GROUP_ID)`.

Both added with `ValueType: "ulong"` (matching the `ULONGLONG`
underlying type, consistent with sibling ETW `*_HANDLE` entries using
the same convention) and `InvalidHandleValues: [0]` (matching
`HTTP_NULL_ID = 0ui64`). Validated via `dotnet build
generation/WinSDK -c Release -p:ScanArch=x64 -t:ScrapeHeaders
-p:PartitionFilter=HttpServer` → 0 errors.

`HttpCreateHttpHandle` outputs a generic `PHANDLE`, explicitly
documented "Use `CloseHandle()` to release" — generic `HANDLE`,
blocker-class 2, out of scope. Other `HTTP_OPAQUE_ID`-derived types
(`HTTP_REQUEST_ID`, `HTTP_CONNECTION_ID`, `HTTP_CLIENT_*_ID`) are
received identifiers for active requests/connections, not separately
allocated/freed resources — no `Close*` function exists for them.

## Conclusion

Producer-site fixes applied — new `HTTP_SERVER_SESSION_ID` and
`HTTP_URL_GROUP_ID` `autoTypes.json` entries added.
