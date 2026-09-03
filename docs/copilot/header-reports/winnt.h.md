# winnt.h

**Classification:** accepted-normalized (no producer-site change needed)

## Summary
Foundational NT/Win32 type and macro definitions (`CONTEXT`,
`EXCEPTION_RECORD`, security-descriptor structs, `STDMETHODCALLTYPE`-
family macros, etc.). Contains no callable function declarations (all
`STDMETHOD`-related matches are macro `#define`s, not real
declarations) and no `DECLARE_HANDLE`/`_Win32_metadata_raii_free_`
handle-type declarations (those live in windef.h, already covered by
the corrected shared-handle policy from commit 165b5f09).

## Ownership Analysis
No functions or handle typedefs exist in this header. No
`autoTypes.json`/`emitter.settings.rsp` changes required.

## Action
None. Classified accepted-normalized with no patch artifact.
