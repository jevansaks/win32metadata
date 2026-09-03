# mapi.h — accepted-normalized

**Partition:** Mapi
**Size:** ~13980 bytes

## Analysis

`mapi.h` (legacy Simple MAPI subset) declares `MAPIFreeBuffer(LPVOID
pv)` — a generic buffer-free function (paired with
`MAPIAllocateBuffer` declared elsewhere), and
`ScMAPIXFromSMAPI(LHANDLE lhSimpleSession, ULONG ulFlags, LPCIID
lpInterface, LPMAPISESSION FAR *lppMAPISession)`.

`LHANDLE` (`typedef ULONG_PTR LHANDLE, FAR * LPLHANDLE;`) is consumed
here only as an *input* — it is not produced/closed by any function in
this file (`MAPILogon`/`MAPILogoff`, the actual producer/consumer
pair, are declared in a separate header). `LHANDLE` is also used
across 3 different files (`MAPI.h`, `MapiUnicodeHelp.h`, `WabDefs.h`)
as MAPI's generic session/object handle type — a shared/generic type
(blocker-class 2), not single-purpose.

`ScMAPIXFromSMAPI`'s output is `LPMAPISESSION *` — a standard COM
interface pointer, out of scope per the COM-interface-pointer
convention.

## Conclusion

Clean. No opaque handle production in this file. No patch required.
