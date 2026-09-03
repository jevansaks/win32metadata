# isolatedapplauncher.h — accepted-normalized

**Partitions:** FileHistory, Isolation
**Size:** ~19224 bytes

## Analysis

`isolatedapplauncher.h` declares
`IsProcessInWDAGContainer(_In_ PVOID Reserved, _Out_ BOOL
*isProcessInWDAGContainer)` and
`IsProcessInIsolatedContainer(_Out_ BOOL *isProcessInIsolatedContainer)`.
Both output plain `BOOL` values — no opaque handle production.

## Conclusion

Clean. No opaque handle production. No patch required.
