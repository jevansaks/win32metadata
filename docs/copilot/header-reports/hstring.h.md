# Header Report: hstring.h

## Partitions
`WinRT`

## Ownership audit (producer-site-only policy)
- Declares the `HSTRING`/`HSTRING_BUFFER` opaque handle *types* only (`typedef struct HSTRING__* HSTRING;`, `DECLARE_HANDLE(HSTRING_BUFFER)`) and MIDL boilerplate — **no functions are declared in this header**. Per the corrected policy, no ownership annotation belongs on a typedef itself.
- The actual producer/consumer functions (`WindowsCreateString`/`WindowsDeleteString`, `WindowsPreallocateStringBuffer`/`WindowsPromoteStringBuffer`/`WindowsDeleteStringBuffer`) live in a separate, already-tracked header (`winstring.h`, still `pending`) — that is where any future producer-site ownership work for `HSTRING`/`HSTRING_BUFFER` belongs.

## Conclusion
`accepted-normalized` — this header declares only opaque handle types (no functions); the producer/consumer functions are tracked separately under `winstring.h` (still pending).
