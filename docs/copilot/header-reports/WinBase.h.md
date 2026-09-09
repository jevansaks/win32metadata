# `WinBase.h`

- **Status:** accepted-normalized
- **Batch:** sidecar-removal tranche batch 11 (final)
- **Partitions:** Base, DataXchg, FileHistory, Fs, Identity, Input.Ime, Intl, IO, MenuRc,
  Registry, Security, Security.AppLocker, Security.ConfigurationSnapin,
  Security.Cryptography.Catalog, Security.Cryptography.Sip, Security.DiagnosticDataQuery,
  Security.DirectoryServices, Security.LicenseProtection, Security.Tpm, Security.WinTrust,
  Security.WinWlx, Setup, Shutdown, TermServ, WinProg

## Ownership Analysis (sidecar-removal tranche update)
20 memberRemap `RAIIFree` entries in `emitter.settings.rsp` targeted producer
functions declared in this header. Moved all of them to inline ABI-neutral
annotations directly on the producer declarations in
`RecompiledIdlHeaders/um/WinBase.h`:
```
BeginUpdateResourceA(...) _Win32_metadata_raii_free_(EndUpdateResourceA);
BeginUpdateResourceW(...) _Win32_metadata_raii_free_(EndUpdateResourceW);
CreateActCtxA(...) _Win32_metadata_raii_free_(ReleaseActCtx);
CreateActCtxW(...) _Win32_metadata_raii_free_(ReleaseActCtx);
CreateBoundaryDescriptorA(...) _Win32_metadata_raii_free_(DeleteBoundaryDescriptor);
CreatePrivateNamespaceA(...) _Win32_metadata_raii_free_(ClosePrivateNamespace);
FindFirstFileNameTransactedW(...) _Win32_metadata_raii_free_(FindClose);
FindFirstFileTransactedA(...) _Win32_metadata_raii_free_(FindClose);
FindFirstFileTransactedW(...) _Win32_metadata_raii_free_(FindClose);
FindFirstStreamTransactedW(...) _Win32_metadata_raii_free_(FindClose);
FindFirstVolumeA(...) _Win32_metadata_raii_free_(FindVolumeClose);
FindFirstVolumeMountPointA(...) _Win32_metadata_raii_free_(FindVolumeMountPointClose);
FindFirstVolumeMountPointW(...) _Win32_metadata_raii_free_(FindVolumeMountPointClose);
OpenBackupEventLogA(...) _Win32_metadata_raii_free_(CloseEventLog);
OpenBackupEventLogW(...) _Win32_metadata_raii_free_(CloseEventLog);
OpenEventLogA(...) _Win32_metadata_raii_free_(CloseEventLog);
OpenEventLogW(...) _Win32_metadata_raii_free_(CloseEventLog);
OpenPrivateNamespaceA(...) _Win32_metadata_raii_free_(ClosePrivateNamespace);
RegisterEventSourceA(...) _Win32_metadata_raii_free_(DeregisterEventSource);
RegisterEventSourceW(...) _Win32_metadata_raii_free_(DeregisterEventSource);
```
This header already carried the `win32metadata_annotations.h` guard. All 20
former sidecar entries were removed from `emitter.settings.rsp` - this was
the last remaining header in the RAIIFree sidecar-removal tranche, bringing
the sidecar `RAIIFree` count in `emitter.settings.rsp` to zero. Consolidated
all three pre-existing patches (`callback-canonical-name`,
`service-security-set-last-error`, `zzz-set-last-error`) together with these
20 new annotations into a single cumulative
`generation/WinSDK/patches/post-midl/WinBase.h.metadata.patch` against the
pristine `d154186c` SDK baseline; removed all three now-superseded
per-reason patch files.

## Validation
- Patch replay: `git apply` of `WinBase.h.metadata.patch` against the
  `d154186c` baseline reproduces the current committed header byte-for-byte.
- ScrapeHeaders (`-p:ScanArch=crossarch`) for all 24 partitions that include
  this header (Base, DataXchg, FileHistory, Fs, Identity, Input.Ime, Intl,
  IO, MenuRc, Registry, Security, Security.AppLocker,
  Security.ConfigurationSnapin, Security.Cryptography.Catalog,
  Security.Cryptography.Sip, Security.DiagnosticDataQuery,
  Security.DirectoryServices, Security.LicenseProtection, Security.Tpm,
  Security.WinTrust, Security.WinWlx, Setup, Shutdown, TermServ, WinProg):
  Build succeeded, 0 Error(s) for every partition (MenuRc and WinProg
  scraped per-arch as x64/x86/arm64 automatically under crossarch).
