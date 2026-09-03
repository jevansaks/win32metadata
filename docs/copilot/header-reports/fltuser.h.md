# fltuser.h — accepted-normalized

**Partition:** Ifsk
**Size:** ~11456 bytes

## Analysis

`fltuser.h` declares the Filter Manager user-mode API
(`FilterConnectCommunicationPort`, `FilterFindFirst`,
`FilterVolumeFindFirst`, `FilterInstanceFindFirst`, etc.). All handle
outputs use the **generic, shared `HANDLE` type**
(`_Outptr_ HANDLE *hPort`, `_Out_ LPHANDLE lpFilterFind`,
`_Out_ PHANDLE lpVolumeFind`, `_Out_ LPHANDLE lpFilterInstanceFind`,
`_Out_ LPHANDLE lpVolumeInstanceFind`) — no distinctly-named handle
typedef is declared anywhere in this header (confirmed via grep for
`DECLARE_HANDLE`/`typedef HANDLE`: zero matches). Per the
generic/shared-type blocker class (blocker-class 2), annotating the
shared `HANDLE` type would incorrectly apply to every value of that
type across the entire SDK, so no fix is representable here.

## Conclusion

Clean (not fixable — all outputs are generic `HANDLE`, no dedicated
subtype to annotate). No patch required.
