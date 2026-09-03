# opmapi.h — accepted-normalized

**Partition:** Mf
**Size:** ~26019 bytes

## Analysis

`opmapi.h` (Output Protection Manager API) declares
`OPMGetVideoOutputsFromHMONITOR`, `OPMGetVideoOutputForTarget`,
`OPMGetVideoOutputsFromIDirect3DDevice9Object`. All output standard
COM interface pointers (`IOPMVideoOutput***`), out of scope per the
COM-interface-pointer convention. `HMONITOR` is a generic, caller-owned
input.

## Conclusion

Clean. No opaque handle production. No patch required.
