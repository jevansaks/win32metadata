# manual.h (Com.StructuredStorage)

**Classification:** accepted-normalized (no producer-site change needed)

## Summary
Hand-authored supplemental header
(`generation/WinSDK/Partitions/Com.StructuredStorage/manual.h`) declaring
a single abstract C++ interface `IMemoryAllocator` with two pure-virtual
methods (`Allocate`/`Free`), added to cover content documented but
missing from the public SDK
(https://github.com/microsoft/win32metadata/issues/1566). Both methods
are interface vtable methods — out of scope per blocker class 5 (COM/
interface vtable methods), consistent with treatment of all other
abstract-interface headers regardless of COM registration.

## Ownership Analysis
No free-function producer/consumer pair exists; `Free` is an interface
method invoked polymorphically by the caller who already holds a
reference, not a distinct handle-typedef closer. No `autoTypes.json`/
`emitter.settings.rsp` changes required.

## Action
None. Classified accepted-normalized with no patch artifact.
