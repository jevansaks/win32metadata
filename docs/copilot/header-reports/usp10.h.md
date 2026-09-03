# usp10.h — accepted-normalized (1 item identified, unrepresentable)

**Partition:** Intl
**Size:** ~101664 bytes

## Analysis

`usp10.h` (Uniscribe) declares `ScriptStringAnalyse` (producer,
`_Outptr_result_buffer_(1) SCRIPT_STRING_ANALYSIS *pssa`) and
`ScriptStringFree(_Inout_updates_(1) SCRIPT_STRING_ANALYSIS *pssa)`
(destroyer). However, `ScriptStringFree` takes the handle **by
pointer** (in-out, nulling the caller's variable after freeing), the
same structural pattern already identified as unrepresentable for
`RpcBindingFree` in `rpcdce.h` (batch 205) — not matching the
single-value-parameter convention every established `CloseApi`
precedent uses.

## Conclusion

No representable producer-site fix identified for
`SCRIPT_STRING_ANALYSIS` (by-pointer/in-out close signature,
consistent with the `rpcdce.h`/`RPC_BINDING_HANDLE` precedent). No
patch required; evidence recorded for future reconsideration.
