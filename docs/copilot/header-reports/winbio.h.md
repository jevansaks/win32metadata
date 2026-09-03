# winbio.h — accepted-normalized (patch applied)

**Partition:** SecBitomet (`Windows.Win32.Devices.BiometricFramework`)
**Size:** ~23728 bytes

## Analysis

`winbio.h` (Windows Biometric Framework public API) declares
`WinBioOpenSession(..., _Out_ WINBIO_SESSION_HANDLE *SessionHandle)`,
freed via `WinBioCloseSession(WINBIO_SESSION_HANDLE SessionHandle)`.

`WINBIO_SESSION_HANDLE` (defined in the sibling `winbio_types.h`,
same partition) is `typedef ULONG WINBIO_SESSION_HANDLE, *
PWINBIO_SESSION_HANDLE;` — a distinctly-named, **32-bit** handle type
(not pointer-sized). No prior `autoTypes.json` entry existed. Reviewed
`NativeTypedefStructsCreator.cs`: `ValueType` is emitted verbatim as
the wrapper struct's backing-field C# type unless it matches one of a
few special-cased strings (`DECLARE_HANDLE`, `AllJoynHandle`,
`DECLARE_OPAQUE_KEY`, `typedef struct*`) — so a plain C# type keyword
like `uint` (matching `ULONG`'s 32-bit width) is supported by the
mechanism, though not previously used in this codebase's
`autoTypes.json`.

## Patch

Edited `generation/WinSDK/autoTypes.json` — added a new entry for
`WINBIO_SESSION_HANDLE` (`ValueType: uint`, `CloseApi:
WinBioCloseSession`) under namespace
`Windows.Win32.Devices.BiometricFramework`.

## Validation

Re-scraped the `SecBitomet` partition (x64, not `ExcludeFromCrossarch`)
after the `main.cpp` touch: build succeeded, 0 warnings, 0 errors.

Additionally attempted a full `dotnet build generation/WinSDK -c
Release -t:EmitWinmd` to exercise `NativeTypedefStructsCreator`
directly against this new `uint`-typed entry. The build failed, but
**only** on the pre-existing, unrelated `AllJoyn` partition's known
Clang toolchain incompatibility (`use of undeclared identifier
'__builtin_verbose_trap'` in MSVC's `xmemory` header — the
long-documented `__builtin_verbose_trap` blocker from this same
session's earlier batches). This occurs during the `ScrapeHeaders`
scan stage for `AllJoyn`, before `EmitWinmd`/
`NativeTypedefStructsCreator` runs for any partition, so it could not
confirm or refute the `uint` `ValueType` specifically — but it also
confirms this is not a regression introduced by this patch.

## Conclusion

Producer-site fix applied for a novel (32-bit) handle-width case,
validated at the `ScrapeHeaders` level; full `EmitWinmd` validation is
blocked project-wide by the pre-existing `AllJoyn` toolchain issue,
unrelated to this change.
