# Header Report: DtcHelp.h

## Partitions
`DTC`

## Ownership audit (producer-site-only policy)
- Declares only enums (`DTC_STATUS`) and function-pointer typedefs (`DTC_GET_TRANSACTION_MANAGER`/`DTC_GET_TRANSACTION_MANAGER_EX_A`/etc.) intended for `GetProcAddress`-based dynamic loading of the DTC helper DLL — no `extern`/`DllImport` function declarations.

## Conclusion
`accepted-normalized` — no ownership annotation required (enum + dynamically-loaded function-pointer typedefs only, no extern functions).
