# Iads.h / ActiveDirectory shift-left migration

## Result

`Iads.h` now compares with 0 unresolved deltas for `ActiveDirectory` on x64, x86, and arm64.

## Header patch

- Added `win32metadata_annotations.h`.
- Added invalid handle annotations to `ADS_SEARCH_HANDLE` for `-1` and `0`.
- Added `_Win32_PreserveResult_` to ADSI iterator-style HRESULT methods that legacy win32metadata marks as preserving multiple success values:
  - `IADsPropertyList::Next`
  - `IADsPropertyList::Skip`
  - `IDirectorySearch::GetFirstRow`
  - `IDirectorySearch::GetNextRow`
  - `IDirectorySearch::GetPreviousRow`
  - `IDirectorySearch::GetNextColumnName`
- Added explicit `_In_opt_` SAL where MIDL `__RPC__in_opt` inputs need legacy `[Optional][In]` metadata:
  - `IADsPropertyValue::put_SecurityDescriptor`
  - `IADsPropertyValue::put_LargeInteger`
  - `IADsAggregatee::ConnectAsAggregatee`
  - `IADsAggregator::ConnectAsAggregator`
  - `IADsAccessControlList::AddAce`
  - `IADsAccessControlList::RemoveAce`
  - `IADsSecurityDescriptor::put_DiscretionaryAcl`
  - `IADsSecurityDescriptor::put_SystemAcl`
- Added explicit `_In_` SAL to `IDirectorySchemaMgmt` pointer parameters so generated metadata matches the legacy input-only signature instead of treating pointer depth as in/out.

Durable patch artifact: `generation\WinSDK\patches\post-midl\Iads.h.win32metadata.patch`.

## Tooling changes required

- Preserve by-value `BSTR` in COM methods.
- Suppress comment-derived `[retval]`; explicit annotations remain the supported path.
- Emit GUID-bearing empty structs for UUID forward-declared COM coclasses instead of GUID constants.
- Collapse ADSI private aliases such as `ADS_DN_STRING`, `ADS_BOOLEAN`, `ADS_INTEGER`, and local `BYTE` so they do not leak as metadata types.
- Preserve `ADS_SEARCH_HANDLE` as a native typedef over `IntPtr` with invalid-handle metadata.
- Preserve `ADS_SORTKEY.fReverseorder` as `Windows.Win32.Foundation.BOOLEAN`.
- Promote only `ADS_AUTHENTICATION_ENUM` to unsigned representation; other ADSI enums with high-bit values remain signed to match legacy metadata.
- Parse `_Win32_PreserveResult_` as the existing multiple-success HRESULT metadata.

## Accepted non-functional differences

- Documentation attributes are ignored by the comparison because they are not shift-left header metadata.
- `SupportedOSPlatform` is still accepted for macro-authored COM interfaces where the annotation cannot attach to the Clang record cursor but the interface signature is otherwise identical.
- Native SDK constants that are not present in legacy win32metadata are accepted.
- Anonymous nested union compiler names may differ when layout and field signatures are identical.

## Evidence

- x64: `C:\Users\jevansa\.copilot\session-state\74f5d9aa-0cce-445c-932c-5c64cd783a41\files\header-loop-artifacts\Iads\ActiveDirectory\x64\comparison\comparison.md`
- x86: `C:\Users\jevansa\.copilot\session-state\74f5d9aa-0cce-445c-932c-5c64cd783a41\files\header-loop-artifacts\Iads\ActiveDirectory\x86\comparison\comparison.md`
- arm64: `C:\Users\jevansa\.copilot\session-state\74f5d9aa-0cce-445c-932c-5c64cd783a41\files\header-loop-artifacts\Iads\ActiveDirectory\arm64\comparison\comparison.md`
