# corecrt_io.h — accepted-normalized

**Partition:** Multimedia
**Size:** ~16721 bytes

## Analysis

`corecrt_io.h` (UCRT low-level I/O declarations) declares
`_findfirst32`/`_findnext32`/`_findclose` and related functions.
`_findfirst32` returns `intptr_t` — a generic CRT scalar type used
throughout the CRT for many unrelated purposes (not a distinctly-named
handle type), matching the generic/shared-type blocker class
(blocker-class 2). Not fixable via type-level annotation.

## Conclusion

Clean (not fixable — generic `intptr_t` return, no dedicated
subtype). No patch required.
