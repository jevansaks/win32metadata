# Header Report: vfwext.h

## Partitions
`Multimedia`

## Ownership audit (producer-site-only policy)
- Declares only `VFW_*`/`TARGET_DEVICE_*` constants and function-pointer *typedefs* (`VFWWDMExtensionProc`, `LPFNEXTDEVIO`) used as callback signatures — no actual `DllImport` functions. No `HANDLE` involved.

## Conclusion
`accepted-normalized` — no ownership annotation required (constants + callback-signature typedefs only, no functions).
