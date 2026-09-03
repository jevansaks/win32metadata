# Header Report: wsipv6ok.h

## Partitions
`WinSock`

## Ownership audit (producer-site-only policy)
- Pure macro-redefinition helper (guarded by `IPV6STRICT`) that renames IPv4-only symbols to trigger compile errors if used — declares no types, functions, or handles itself.

## Conclusion
`accepted-normalized` — no ownership annotation required (macro redefinitions only, no declarations).
