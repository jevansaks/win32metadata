# windot11.h

**Classification:** accepted-normalized (no producer-site change needed)

## Summary
Native 802.11 miniport driver specification definitions (structs, enums,
OID constants) consumed by NDIS wireless drivers. Contains no callable
Win32 API functions — no `HRESULT`/`BOOL`/`DWORD`/`STDAPI` top-level
declarations found (0 matches for the standard scrape-target pattern).

## Ownership Analysis
No functions exist in this header, therefore no producer/consumer handle
pattern applies. All types are plain structs/enums consumed directly by
driver code. No `autoTypes.json` or `emitter.settings.rsp` changes
required.

## Action
None. Classified accepted-normalized with no patch artifact.
