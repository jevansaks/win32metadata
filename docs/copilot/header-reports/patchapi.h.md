# patchapi.h — accepted-normalized

**Partition:** Setup
**Size:** ~24284 bytes

## Analysis

`patchapi.h` (file patch creation/application interface) declares
`CreatePatchFile*`, `ApplyPatchToFile*`, `TestApplyPatchToFile*`
families. `*ByHandles` variants take generic, caller-owned `HANDLE`
parameters as *input* (open file handles), not produced by this API.
No opaque handle types are produced.

## Conclusion

Clean. No opaque handle production. No patch required.
