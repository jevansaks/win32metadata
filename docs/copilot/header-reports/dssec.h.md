# Header Report: dssec.h

## Partitions
`Security.DirectoryServices`

## Ownership audit (producer-site-only policy)
- `DSCreateISecurityInfoObject`'s `_Out_ LPSECURITYINFO *ppSI` is a standard COM interface pointer (`ISecurityInformation`), out of scope.
- `PFNREADOBJECTSECURITY`/`PFNWRITEOBJECTSECURITY` are function-pointer typedefs for caller-supplied callbacks, not `extern`/`DllImport` declarations.

## Conclusion
`accepted-normalized` — no ownership annotation required (standard COM interface output + caller-supplied callback typedefs only).
