# dsparse.h — accepted-normalized

**Partition:** ActiveDirectory
**Size:** ~24864 bytes

## Analysis

`DSPARSE.h` (public NTDS name-parsing APIs) declares
`DsMakeSpn*`/`DsCrackSpn*`/`DsQuoteRdnValue*`/`DsUnquoteRdnValue*`/
`DsIsMangledRdnValue*`/`DsIsMangledDn*`/`DsGetRdnW`. All parameters
are strings, `DWORD` lengths, or transparent output buffers — no
opaque handle production.

## Conclusion

Clean. No opaque handle production. No patch required.
