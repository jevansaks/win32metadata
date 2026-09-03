# extsfns.h — accepted-normalized

**Partition:** Debug.Extensions
**Size:** ~101103 bytes

## Analysis

`extsfns.h` ("headers for various known extension functions defined
in different extension dlls... appropriate extension dll must be
loaded in the debugger") declares debugger-extension-DLL callback
contract function signatures, retrieved dynamically via
`IDebugSymbols->GetExtension` — not exported system APIs for
applications to P/Invoke directly. Same pattern as `WDBGEXTS.H`. No
representable producer-site ownership pattern.

## Conclusion

Clean. No patch required — debugger-extension callback contract, no
representable ownership gap.
