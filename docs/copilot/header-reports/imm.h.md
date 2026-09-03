# imm.h — accepted-normalized

**Partition:** Input.Ime
**Size:** ~28873 bytes

## Analysis

`imm.h` (Input Method Manager) declares `ImmCreateContext`
(producer of `HIMC`) and `ImmDestroyContext` (its destroyer), plus
consumer functions (`ImmGetContext`/`ImmReleaseContext` — borrowed,
not owned; `ImmAssociateContext` — swaps, not owning transfer).

Checked `autoTypes.json` first: **`HIMC` already has a complete
entry** (`Namespace: Windows.Win32.UI.Input.Ime`, `ValueType:
DECLARE_HANDLE`, `CloseApi: ImmDestroyContext`,
`InvalidHandleValues: [-1, 0]`) — producer-site ownership metadata
is already correctly in place at the return-value level. No gap.

`HKL` (returned by `ImmInstallIMEA/W`) is a global/system keyboard
layout handle, not a caller-owned per-call resource — consistent with
existing `HKL` handling elsewhere in the metadata (not misclassified
as owned here).

## Conclusion

Clean. No patch required — existing `HIMC` metadata already correct
and producer-site (not typedef-site).
