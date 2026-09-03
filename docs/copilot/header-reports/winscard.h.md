# winscard.h — accepted-normalized (producer-site fix applied; 1 item blocked)

**Partition:** Credentials
**Size:** ~55649 bytes

## Analysis

`winscard.h` (Smart Card API) declares two distinct handle typedefs:

- **`SCARDCONTEXT`** (`typedef ULONG_PTR SCARDCONTEXT;`) — produced by
  `SCardEstablishContext` (`_Out_ LPSCARDCONTEXT phContext`),
  destroyed by single-arg `SCardReleaseContext(SCARDCONTEXT)`.
  **No `autoTypes.json` entry existed.** Confirmed also used (not yet
  producer-annotated) in the still-pending sibling `cardmod.h`
  (different partition, `Security.Cryptography`) — safe to add now
  since `autoTypes.json` entries are namespace-scoped per type name,
  independent of which header/partition references them. Added new
  entry (`ValueType: DECLARE_HANDLE` — pointer-sized `ULONG_PTR` —
  `CloseApi: SCardReleaseContext`, `InvalidHandleValues: [0]`).
  Validated via `dotnet build generation/WinSDK -c Release
  -p:ScanArch=x64 -t:ScrapeHeaders -p:PartitionFilter=Credentials` →
  0 errors.

- **`SCARDHANDLE`** — produced by `SCardConnectA/W` (`_Out_
  LPSCARDHANDLE`), but its only destroyer, `SCardDisconnect(SCARDHANDLE
  hCard, DWORD dwDisposition)`, requires a **mandatory, semantically
  meaningful second parameter** (`SCARD_LEAVE_CARD`/`RESET_CARD`/
  `UNPOWER_CARD`/`EJECT_CARD` — a real caller choice, not a "reserved,
  must be 0" placeholder). Treated as unrepresentable via single-arg
  `CloseApi`, consistent with the `SQLFreeStmt` precedent.

## Conclusion

Producer-site fix applied for `SCARDCONTEXT`. `SCARDHANDLE` remains
out of scope for typedef-level ownership annotation (genuinely
unrepresentable — mandatory meaningful-choice disposition parameter).
