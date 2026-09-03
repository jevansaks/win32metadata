# winsxs.h — accepted-normalized

**Partition:** Setup
**Size:** ~25157 bytes

## Analysis

`winsxs.h` (Side-by-Side assembly cache API) declares
`CreateAssemblyNameObject(LPASSEMBLYNAME *ppAssemblyNameObj, ...)` and
`CreateAssemblyCache(IAssemblyCache **ppAsmCache, ...)`. `LPASSEMBLYNAME`
is `typedef IAssemblyName *LPASSEMBLYNAME;` — both outputs are
standard COM interface pointers, out of scope per the
COM-interface-pointer convention.

## Conclusion

Clean. No opaque handle production. No patch required.
