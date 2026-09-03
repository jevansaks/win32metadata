# vmsavedstatedumpdefs.h — accepted-normalized

**Partition:** Hypervisor
**Size:** ~11193 bytes

## Analysis

`vmsavedstatedumpdefs.h` declares `VM_SAVED_STATE_DUMP_HANDLE`
(`typedef VOID* VM_SAVED_STATE_DUMP_HANDLE;`) plus transparent
memory-layout structs (`GPA_MEMORY_CHUNK`, paging-mode enum, etc.). It
is a pure "-defs" header: it contains **no function declarations at
all**. The producer/consumer functions for
`VM_SAVED_STATE_DUMP_HANDLE` (`VmSavedStateDumpCreate`,
`VmSavedStateDumpClose`, etc.) live in the separate
`VmSavedStateDumpProvider.h` header. Per the deferred-to-sibling-header
pattern (blocker-class 8), this defs-only header is classified clean
immediately, with ownership audit deferred to the sibling
provider header when it is processed.

## Conclusion

Clean. No functions in this header; ownership deferred to
`VmSavedStateDumpProvider.h`. No patch required here.
