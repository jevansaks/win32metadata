# objsel.h / ActiveDirectory shift-left migration

## Result

`objsel.h` now compares with 0 unresolved deltas for `ActiveDirectory` on x64, x86, and arm64.

## Header patch

- Added `win32metadata_annotations.h`.
- Added guarded record-level `[[clang::annotate("win32metadata:struct_size_field=cbSize")]]` annotations to:
  - `DSOP_SCOPE_INIT_INFO`
  - `DSOP_INIT_INFO`
- Added `_Win32_FlexibleArray_` to `DS_SELECTION_LIST.aDsSelection`.
- Added `_Windows_SupportedOS_WindowsVista_` to `IDsObjectPicker`.
- Added `_Windows_SupportedOS_Server2008_` to `IDsObjectPickerCredentials`.
- Added `_Out_` to `IDsObjectPicker::InvokeDialog`'s `ppdoSelections` parameter.

Durable patch artifact: `generation\WinSDK\patches\post-midl\objsel.h.win32metadata.patch`.

## Tooling changes required

- Preserve native const metadata for pointer-to-const string aliases such as `LPCWSTR` in macro-authored COM methods.
- Emit old COM macro methods that are virtual but not marked `PURE`; `IDsObjectPickerCredentials::SetCredentials` is present in legacy metadata even though this declaration omits `PURE`.

## Accepted non-functional differences

- Documentation attributes are ignored by comparison.
- `SupportedOSPlatform` is accepted for macro-authored COM interfaces where the annotation cannot attach to the Clang record cursor but the interface signature is otherwise identical.

## Evidence

- x64: `C:\Users\jevansa\.copilot\session-state\74f5d9aa-0cce-445c-932c-5c64cd783a41\files\header-loop-artifacts\objsel\ActiveDirectory\x64\comparison\comparison.md`
- x86: `C:\Users\jevansa\.copilot\session-state\74f5d9aa-0cce-445c-932c-5c64cd783a41\files\header-loop-artifacts\objsel\ActiveDirectory\x86\comparison\comparison.md`
- arm64: `C:\Users\jevansa\.copilot\session-state\74f5d9aa-0cce-445c-932c-5c64cd783a41\files\header-loop-artifacts\objsel\ActiveDirectory\arm64\comparison\comparison.md`
