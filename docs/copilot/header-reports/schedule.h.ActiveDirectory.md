# schedule.h / ActiveDirectory shift-left migration

## Result

`schedule.h` now compares with 0 unresolved deltas for `ActiveDirectory` on x64, x86, and arm64.

## Header patch

- Added `win32metadata_annotations.h`.
- Added `_Win32_FlexibleArray_` to `SCHEDULE.Schedules`.

Durable patch artifact: `generation\WinSDK\patches\post-midl\schedule.h.win32metadata.patch`.

## Evidence

- x64: `C:\Users\jevansa\.copilot\session-state\74f5d9aa-0cce-445c-932c-5c64cd783a41\files\header-loop-artifacts\schedule\ActiveDirectory\x64\comparison\comparison.md`
- x86: `C:\Users\jevansa\.copilot\session-state\74f5d9aa-0cce-445c-932c-5c64cd783a41\files\header-loop-artifacts\schedule\ActiveDirectory\x86\comparison\comparison.md`
- arm64: `C:\Users\jevansa\.copilot\session-state\74f5d9aa-0cce-445c-932c-5c64cd783a41\files\header-loop-artifacts\schedule\ActiveDirectory\arm64\comparison\comparison.md`
