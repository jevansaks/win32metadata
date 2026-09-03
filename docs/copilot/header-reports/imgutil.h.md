# imgutil.h — accepted-normalized

**Partition:** InternetExplorer
**Size:** ~10603 bytes

## Analysis

`imgutil.h` is MIDL-generated, declaring `ISniffStream`/
`IMapMIMEToCLSID` COM interfaces plus free functions:
`CreateMIMEMap`, `DecodeImage`, `DecodeImageEx`, `SniffStream`,
`GetMaxMIMEIDBytes`, `IdentifyMIMEType`, `ComputeInvCMAP`, `DitherTo8`,
`CreateDDrawSurfaceOnDIB`.

All outputs are either standard COM interface pointers
(`IMapMIMEToCLSID**`, `IDirectDrawSurface**`) — out of scope per the
COM-interface-pointer convention — or plain scalars/caller-supplied
buffers (`UINT*`, `ULONG*`, `BYTE*`). `CreateDDrawSurfaceOnDIB` takes
`HBITMAP hbmDib` as an *input* (caller-owned, generic GDI handle), not
a production site.

## Conclusion

Clean. No opaque handle production requiring new metadata.
