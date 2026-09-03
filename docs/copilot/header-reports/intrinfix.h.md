# intrinfix.h

**Classification:** accepted-normalized (no producer-site change needed)

## Summary
Internal build-fixup header (`generation/WinSDK/inc/intrinfix.h`) that
redefines compiler-intrinsic machine-target macros
(`__MACHINEX86`/`__MACHINEARM64`/etc.) to work around a VS2019 `intrin0.h`
regression. Contains only preprocessor `#define` macros — no functions,
no typedefs, no ownership-relevant content. Included by nearly every
partition as a compatibility shim, not genuine Win32 API surface.

## Ownership Analysis
No functions or typedefs exist. No `autoTypes.json`/
`emitter.settings.rsp` changes required.

## Action
None. Classified accepted-normalized with no patch artifact.
