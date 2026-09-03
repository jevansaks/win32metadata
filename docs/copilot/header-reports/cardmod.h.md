# cardmod.h — accepted-normalized

**Partition:** Security.Cryptography
**Size:** ~66008 bytes

## Analysis

`CARDMOD.H` ("commonly used for card modules") declares the smart-
card **minidriver plugin contract** (`CardAcquireContext`,
`CardDeleteContext`, etc.) — functions a card minidriver DLL must
export, called by the base CSP (`basecsp.dll`), not exported by a
system DLL for applications to P/Invoke. `CardAcquireContext`/
`CardDeleteContext` operate on a caller-owned `PCARD_DATA` structure
pointer (in/out), not a distinct opaque handle typedef produced by
the function. `SCARDCONTEXT` (already correctly annotated, fixed in
batch 189) is used here only as a consumed field inside `CARD_DATA`,
not re-produced. No representable ownership gap.

## Conclusion

Clean. No patch required — plugin-contract functions, no
representable ownership pattern.
