# libMappingsManual.rsp Sidecar-Removal Tranche

## Summary

`generation/WinSDK/libMappingsManual.rsp` was a `--with-librarypath` sidecar of
384 manually-curated `NAME=dll.dll` import-library overrides - entries the
`.lib`-file auto-scan (`libMappings.rsp`) got wrong or couldn't find at all
(functions removed from public import libraries, undocumented/internal exports,
etc.). Per `generation/WinSDK/Windows.Win32.proj`'s `ScraperRsp` item order
(`libMappings.rsp` before `libMappingsManual.rsp`), the manual entries always
overrode the auto-generated ones for the same name. The shift-left goal applies
only to the **manual** overrides: `libMappings.rsp` remains a legitimate,
scan-derived global input and was not touched.

## Reconciliation and migration

A declaration-anchored scanner (reusing the validated patterns from the
`WithSetLastError.rsp`/`supportedOS.rsp` tranches, including string-literal
blanking, `virtual`/`STDMETHODCALLTYPE` exclusions for C++ interface methods,
and the off-by-one-safe prefix-expression check) located each entry's
declaration(s) and checked for an existing `_Win32_metadata_import_library_("dll.dll")`
annotation (case-insensitive DLL-name comparison, since Windows module names
are not case sensitive):

- **94 entries** already fully covered by pre-existing annotations (mostly
  `drt.h`, `recapis.h`, `HtmlHelp.h`, `IEObj.h`, `IEPMapi.h`, `wldp.h`,
  `winsvc.h`, `netfw.h`, `prnasntp.h`, `NetCon.h`, `SrRestorePtApi.h` - migrated
  by earlier, pre-ralph-loop tranches) - removed as pure reconciliation.
- One false conflict (`CloneContext`) was resolved by fixing a scanner gap: a
  `virtual HRESULT CloneContext(...)` C++ interface method in `httpserv.h`
  (an unrelated `IHttpContext`-family method, no `STDMETHODCALLTYPE` keyword to
  trigger the existing vtable exclusion) was being matched as if it were the
  real `inkobjcore.dll` `CloneContext` free function in `recapis.h`. Added a
  general "line starts with `virtual`, or a `virtual` keyword appears on a
  preceding wrapped specifier line" exclusion, which fixed this without any
  header change (the real `recapis.h` declaration was already correctly
  annotated).
- **235 entries migrated** to inline `_Win32_metadata_import_library_("dll.dll")`
  annotations across 8 headers: `p2p.h` (P2P.dll/P2PGRAPH.dll Peer* APIs, the
  large majority), `winconp.h` (Wcm*), `mscoree.h` (CLR hosting APIs),
  `cpdk/bcrypt_provider.h` (the 4 `BCrypt*Provider` CNG provider-registration
  functions - see below), `mssign.h` (Signer*/SignError), `metahost.h`
  (`CLRCreateInstance`), `getcurrentpackageinfo3.h`
  (`GetCurrentPackageInfo3`), `consoleapis.h` (`ConsoleControl`). Annotation
  order was normalized to match the established convention
  (`_Win32_metadata_supported_os_` before `_Win32_metadata_import_library_`)
  after an initial pass inserted them in the opposite order (functionally
  harmless but inconsistent with existing examples).
- Added the `win32metadata_annotations.h` guard (unconditional include, per the
  fix established in the `supportedOS.rsp` tranche) to every touched header
  lacking it, including one with no `winapifamily.h`/conventional include-guard
  `#define` at all (`cpdk/bcrypt_provider.h` - placed right after `#pragma once`).

### The `BCrypt*Provider` functions: found in a subdirectory, not absent

`BCryptRegisterProvider`, `BCryptUnregisterProvider`,
`BCryptAddContextFunctionProvider`, `BCryptRemoveContextFunctionProvider` were
flagged in the rsp's own comment as "APIs that have been removed from the SDK
but that we include for completeness" - and indeed absent from `bcrypt.h`.
However, they are declared in `um/cpdk/bcrypt_provider.h` (the Cryptography
Provider Development Kit header, for developers implementing custom CNG
providers), confirmed already present in the generated C# output with full SAL
metadata before this tranche touched anything. They were not "absent from the
SDK" at all - just outside the header this rsp's originating comment assumed.
Migrated normally like any other entry.

## Absent-from-current-SDK / undocumented internal entries (removed, no header change)

**54 entries** remain genuinely unrepresentable: confirmed absent from every
header in `RecompiledIdlHeaders` (including the `cpdk` subdirectory and all
other non-`winrt` locations), absent from every hand-written file under
`generation/WinSDK/manual/`, and absent from the complete generated C# output
(cross-referenced by exact-identifier match) - meaning the sidecar fact was
already functionally inert (no declaration exists anywhere for the scraper to
attach it to). No authoritative signature was reliably available for any of
them, so none were fabricated into "guarded metadata-only" declarations, per
the requirement to only do so where signatures are known:

- `ElfReportEventW`, `ElfRegisterEventSourceW`, `ElfFlushEventLog`,
  `ElfDeregisterEventSource` (4) - the rsp's own comment already marks this
  group as removed-from-SDK legacy Event Log APIs.
- `symsrv` (1) - not a real function name (`SymSrv*`-prefixed functions exist
  in `DbgHelp.h`, but bare `symsrv` does not); likely an internal/undocumented
  entry point, no declaration found anywhere.
- 23 old, undocumented internal MAPI32.dll exports (`CbOfEncoded`,
  `CchOfEncoding`, `EncodeID`, `FBadRglpszA`, `FDecodeID`,
  `HrValidateParametersV`, `InstallFilterHook`, `PRProviderInit`,
  `ScGenerateMuid`, `ScMAPIXFromCMC`, `SwapPlong`, `SwapPword`, 8×
  `UNKOBJ_*`, `WrapProgress`, `__CPPValidateParameters`,
  `__ValidateParameters`).
- `RegisterDeviceDualEnrollMmpcUsingAADDeviceCredentials`,
  `RtlWnfCompareChangeStamp`, `CanBrokerForceForeground` (3) - undocumented
  internal MDM/ntdll/user32 exports.
- `PeerIdentityGetCert`, `PeerSSPAddCredentials`, `PeerSSPRemoveCredentials`,
  `PeerGraphSuspendTimers` (4) - unlike the ~150 other Peer* APIs in the same
  rsp (all found and migrated in `p2p.h`), these 4 have no declaration in the
  current `p2p.h`/`p2pgraph.h`.
- `WcmEnterConnectedStandby`, `WcmEnterNetQuiet`, `WcmExitConnectedStandby`,
  `WcmExitNetQuiet` (4).
- 12 `Wldp*` functions - unlike the ~20 other `Wldp*` functions already
  declared and annotated in `wldp.h`, these 12 (mostly Vulnerable Driver
  Blocklist and developer-mode/dynamic-code-trust management APIs) are not yet
  present in this SDK snapshot's `wldp.h` - most likely newer additions from a
  later Windows build than the one this repository currently scrapes, the
  reverse situation from the "removed" group above.
- `ProcessPrngGuid`, `MSCryptConvertRsaPrivateBlobToFullRsaBlob` (2).
- `pSetupModifyGlobalFlags` (1) - the rsp's own comment already documents this
  one as having "no associated import library or header file; you must call it
  using the LoadLibrary and GetProcAddress functions" (this text is copied
  verbatim from Microsoft's own devnotes page for the sibling function
  `pSetupSetGlobalFlags`, confirming the pattern). No public documentation
  exists for `pSetupModifyGlobalFlags` specifically, so its exact signature -
  while very likely similar to `pSetupSetGlobalFlags(DWORD Value)` - is not
  verified, and was not fabricated.

One entry (`ProcessPrng`) turned out to already be fully represented, just not
via a header: it is hand-written C# in
`generation/WinSDK/manual/Security.Cryptography.cs` with
`[DllImport("BCryptPrimitives.dll", ...)]` directly on the method (matching
the rsp's `bcryptprimitives.dll` case-insensitively). Removed from the rsp
with no changes needed - the fact was already fully captured by the existing
manual C#, exactly matching the precedent set for `DWRITE_MAKE_OPENTYPE_TAG`
in the `supportedOS.rsp` tranche.

## Validation

- All 8 touched headers consolidated into `<header>.metadata.patch` (5 new,
  3 updated); 0 superseded per-reason patches existed to remove for these
  files. All replay byte-for-byte from a pristine `d154186c` checkout.
- `ScrapeHeaders -p:ScanArch=crossarch` run across all 5 reachable partitions
  (`Console`, `Security.Cryptography`, `AppxPackaging`, `ClrHosting`, `P2p`) -
  0 errors.

## Residual state

**`libMappingsManual.rsp` reached zero API-specific entries and was deleted**,
along with its `<ScraperRsp Include="libMappingsManual.rsp"/>` reference in
`generation/WinSDK/Windows.Win32.proj`. `libMappings.rsp` (the auto-generated,
scan-derived global mapping) was left completely untouched, as required.
Updated `docs/spec.md` and `CONTRIBUTING.md` to describe the inline-annotation
(or hand-written-C#, for header-less LoadLibrary-only exports) approach going
forward instead of pointing contributors at the removed sidecar.

Final tally: 384 original entries → 94 reconciled as already-covered (no
header change; includes 1 false conflict, `CloneContext`, resolved via a
scanner fix rather than a header change) + 235 migrated to inline
`_Win32_metadata_import_library_("dll.dll")` annotations across 8 headers +
1 already covered via existing hand-written C# (`ProcessPrng`, no action) +
54 documented as absent from the current SDK/manual C#, with no verified
authoritative signature available to safely fabricate a declaration.
Residual API entries in `libMappingsManual.rsp`: **0**. The file itself has
been removed.
