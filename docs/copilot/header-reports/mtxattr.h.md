# Header Report: mtxattr.h

## Partitions
`ComOle`, `TransactionServer`

## Ownership audit (producer-site-only policy)
- Declares only IDL typelib custom-attribute macros (`TLBATTR_TRANS_*`, `TRANSACTION_*`, `QUEUEABLE`, `COMTI_INTRINSICS_ENABLED`) used exclusively inside `.idl` files, not compiled as C declarations. No functions, no handles.

## Conclusion
`accepted-normalized` — no ownership annotation required (typelib attribute macros only, no functions).
