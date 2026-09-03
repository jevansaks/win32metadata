# msdrm.h — accepted-normalized (producer-site fix applied)

**Partition:** Rm
**Size:** ~32099 bytes

## Analysis

`msdrm.h` declares the Windows Rights Management (DRM) SDK API,
including 5 distinct opaque handle typedefs (all `typedef ULONG`,
declared in sibling header `msdrmdefs.h`, whose report explicitly
deferred DRM handle ownership to `msdrm.h`):

| Handle | Producer(s) | Close API |
|---|---|---|
| `DRMHANDLE` | `DRMCreateClientSession` (`phDefaultLibrary`), `DRMLoadLibrary`, `DRMCreateClientSessionEx`, etc. | `DRMCloseHandle` |
| `DRMENVHANDLE` | `DRMCreateClientSession` (`phEnv`), `DRMDuplicateEnvironmentHandle` | `DRMCloseEnvironmentHandle` |
| `DRMHSESSION` | (session-producing calls) | `DRMCloseSession` |
| `DRMQUERYHANDLE` | `DRMGetBoundLicenseObject`/`DRMGetUnboundLicenseObject`-family queries | `DRMCloseQueryHandle` |
| `DRMPUBHANDLE` | `DRMCreateUser`, `DRMCreateRight`, `DRMCreateIssuanceLicense` | `DRMClosePubHandle` |

Checked `autoTypes.json` first: **no entries existed** for any of the
5 types. Confirmed via grep that all 5 typedefs are used exclusively
within `msdrm.h`/`msdrmdefs.h` (single-partition, `Rm`, not
`ExcludeFromCrossarch`). Added 5 new `autoTypes.json` entries
(`ValueType: "uint"` — following the `WINBIO_SESSION_HANDLE`
precedent for `ULONG`-typedef'd handles — with the respective
`CloseApi` and `InvalidHandleValues: [0]`, matching the explicit
`*_INVALID` macros in `msdrmdefs.h`).

Validated via `dotnet build generation/WinSDK -c Release
-p:ScanArch=x64 -t:ScrapeHeaders -p:PartitionFilter=Rm` → 0 errors.

## Conclusion

Producer-site fix applied — 5 new `autoTypes.json` entries added at
the producer/close-function level (not on the `msdrmdefs.h`
typedefs themselves), consistent with the corrected ownership policy.
