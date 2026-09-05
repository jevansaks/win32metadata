# UserEnv.h

**Classification:** blocked (corrected evidence)

## Summary
`LoadUserProfileW/A(_In_ HANDLE hToken, _Inout_ LPPROFILEINFOW/A
lpProfileInfo)` populates a plain `HANDLE` field (`lpProfileInfo->hProfile`)
inside a caller-allocated struct, released via `UnloadUserProfile(HANDLE
hToken, HANDLE hProfile)`.

## Correction to prior investigation
Prior blocker text ("generic HANDLE nested in struct field cannot be
annotated, same class as physicalmonitorenumerationapi.h") was an inaccurate
generalization on two counts:
1. A struct-pointer **out-param itself** (not a bare field) can be
   annotated normally when the consumer takes the same pointer/handle alone
   - see srpapi.h's `SrpCreateThreadNetworkContext`, fixed in this batch.
2. The real, narrower blocker here is structurally different from
   physicalmonitorenumerationapi.h: `UnloadUserProfile` needs a **second**
   argument (`hToken`) beyond the value produced (`hProfile`), which the
   established unary-only `RAIIFree` convention (68 existing precedents)
   cannot express - the same root limitation as dmemmgr.h's `VidMemFree`, not
   a struct-nesting problem.

## Ownership Analysis
No `emitter.settings.rsp` change possible with the current unary-only
`RAIIFree` convention.

## Conclusion
`blocked` - remains blocked, but for the corrected reason above (free
function needs a second argument, `hToken`, not "struct nesting").
