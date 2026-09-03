# t2embapi.h — accepted-normalized

**Partition:** Gdi
**Size:** ~16055 bytes

## Analysis

`t2embapi.h` (OpenType embedding services, T2EMBED.DLL) declares
`TTLoadEmbeddedFont(_Out_ HANDLE* phFontReference, ...)`. The output
is a **generic, shared `HANDLE`** — not a distinctly-named type — per
the generic/shared-type blocker class (blocker-class 2), annotating it
would incorrectly apply to every `HANDLE` value across the SDK.

## Conclusion

Clean (not fixable — generic `HANDLE` output, no dedicated subtype).
No patch required.
