# strsafe.h

**Classification:** accepted-normalized (no producer-site change needed)

## Summary
Safer C-library string routine replacements (`StringCchCopy`,
`StringCbPrintf`, etc.), implemented as inline template
functions/macros operating on caller-owned buffers. No handle-producing
or handle-consuming functions exist — every function operates directly
on caller-supplied string buffers with explicit size parameters.

## Ownership Analysis
No producer/consumer handle pattern exists (all functions are inline
buffer-safety helpers). No `autoTypes.json`/`emitter.settings.rsp`
changes required.

## Action
None. Classified accepted-normalized with no patch artifact.
