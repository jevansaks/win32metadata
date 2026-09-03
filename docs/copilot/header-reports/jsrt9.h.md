# jsrt9.h — accepted-normalized (producer-site fix applied)

**Partition:** Js
**Size:** ~80566 bytes

## Analysis

`jsrt9.h` (Chakra JsRT API) declares `JsCreateRuntime` (producer,
`_Out_ JsRuntimeHandle *runtime`) and single-arg
`JsDisposeRuntime(JsRuntimeHandle)` (destroyer). `JsRuntimeHandle`
(`typedef void *JsRuntimeHandle;`) is a distinct pointer-sized opaque
typedef.

Checked `autoTypes.json` first: **no entry existed**. Confirmed
single-file usage, `Js` partition (not `ExcludeFromCrossarch`),
namespace `Windows.Win32.System.Js` (from partition `settings.rsp`).
Added new `autoTypes.json` entry (`ValueType: DECLARE_HANDLE`,
`CloseApi: JsDisposeRuntime`, `InvalidHandleValues: [0]`). Validated
via `dotnet build generation/WinSDK -c Release -p:ScanArch=x64
-t:ScrapeHeaders -p:PartitionFilter=Js` → 0 errors.

`JsRef` (also `typedef void*`) is a generic garbage-collected/ref-
counted reference type shared across many different JS object kinds
(values, contexts, etc.), managed via `JsAddRef`/`JsRelease`
(ref-counting, not a simple create/destroy pair) — out of scope,
consistent with COM-style ref-counted object handling.

## Conclusion

Producer-site fix applied — new `JsRuntimeHandle` `autoTypes.json`
entry added.
