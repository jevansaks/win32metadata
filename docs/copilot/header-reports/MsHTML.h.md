# Mshtml.h / mshtmlc.h

**Classification:** accepted-normalized (no producer-site change needed)

## Summary
Trident (MSHTML) DOM COM interfaces (`IHTMLDocument2`, `IHTMLElement`,
etc.). Both `MsHTML.h` (full) and `Mshtmlc.h` (constants/coclass
subset) contain 18234 callable declarations that are `STDMETHOD` COM
vtable methods — out of scope per blocker class 5 (COM vtable methods).
No free-function declarations found in either header.

## Ownership Analysis
No free (non-COM) producer/consumer functions exist. No
`autoTypes.json`/`emitter.settings.rsp` changes required.

## Action
None. Classified accepted-normalized with no patch artifact (covers both
`mshtml.h` and `mshtmlc.h` ledger entries).
