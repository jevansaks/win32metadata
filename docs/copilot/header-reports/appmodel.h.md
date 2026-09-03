# appmodel.h — accepted-normalized (2 producer-site fixes applied)

**Partition:** AppxPackaging
**Size:** ~40476 bytes

## Analysis

`appmodel.h` (App Model runtime APIs) declares two genuine
producer/consumer handle pairs:

1. **`PACKAGE_INFO_REFERENCE`** — `typedef struct _PACKAGE_INFO_REFERENCE
   { void* reserved; } * PACKAGE_INFO_REFERENCE;` (opaque-pointer-to-
   single-field-struct, matching the `"typedef struct*"` `ValueType`
   sentinel — confirmed in `NativeTypedefStructsCreator.cs`:
   `valueType.StartsWith("typedef struct")` → emitted as `IntPtr`).
   Produced by `OpenPackageInfoByFullName`/
   `OpenPackageInfoByFullNameForUser` (`_Out_
   PACKAGE_INFO_REFERENCE*`), destroyed by single-arg
   `ClosePackageInfo(PACKAGE_INFO_REFERENCE)`. **No `autoTypes.json`
   entry existed** — added a new entry.

2. **`PACKAGEDEPENDENCY_CONTEXT`** — produced by
   `AddPackageDependency`/`AddPackageDependency2` (`_Out_
   PACKAGEDEPENDENCY_CONTEXT*`), destroyed by single-arg
   `RemovePackageDependency(PACKAGEDEPENDENCY_CONTEXT)`. **Had an
   existing `autoTypes.json` entry but it was missing both `CloseApi`
   and `InvalidHandleValues`** — a genuine stale-metadata gap. Filled
   in `CloseApi: RemovePackageDependency`, `InvalidHandleValues: [0]`.

Both validated via `dotnet build generation/WinSDK -c Release
-p:ScanArch=x64 -t:ScrapeHeaders -p:PartitionFilter=AppxPackaging` →
0 errors.

**Not owned resources** (confirmed, no gap): `TryCreatePackageDependency`/
`TryCreatePackageDependency2` output `packageDependencyId` as a
`PWSTR*` string "allocated via HeapAlloc; use HeapFree to deallocate"
per doc comment — a raw string buffer, not a distinct handle typedef
(matches the Net-API-buffer-allocation-style blocker-class 7, out of
scope for typedef-level annotation). `DeletePackageDependency` takes
the same string identifier, not a handle.

## Conclusion

Two producer-site fixes applied: new `PACKAGE_INFO_REFERENCE` entry,
and filled-in `CloseApi`/`InvalidHandleValues` on the pre-existing
`PACKAGEDEPENDENCY_CONTEXT` entry.
