# Header Report: msclmd.h

## Partitions
`Security.Cryptography`

## Ownership audit (producer-site-only policy)
- Pure PIV smart-card constants (`ROLE_*`/`CP_PIV_*`/`CLMD_*`) and plain data structs (`CLMD_PIV_CERT_DATA`, `CLMD_PIV_GENERATE_ASYMMETRIC_KEY`, `CLMD_PIV_PUBLIC_KEY_DATA`, used for `CardSetProperty`/`CardGetProperty` interop). No functions declared in this header.

## Conclusion
`accepted-normalized` — no ownership annotation required (constants/structs only, no functions).
