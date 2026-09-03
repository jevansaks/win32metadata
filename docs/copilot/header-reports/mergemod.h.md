# mergemod.h — accepted-normalized

**Partition:** Setup
**Size:** ~129071 bytes

## Analysis

`mergemod.h` (MergeMod COM object interface) declares COM interfaces
only (per doc comment, all `[out] BSTR` values are separately
client-released — a `BSTR` ownership concern, already covered
generically by the existing `SysFreeString` `CloseApi` convention for
`BSTR`, not specific to this header). Confirmed via grep that there
are **no free function declarations**.

## Conclusion

Clean. No patch required.
