# Header Report: roregistrationapi.h

## Partitions
`WinRT`

## Ownership audit (producer-site-only policy)
- `RoGetActivatableClassRegistration` is a clean COM factory (`_COM_Outptr_ PActivatableClassRegistration*`).
- `RoGetServerActivatableClasses(..., _Outptr_result_buffer_(*count) HSTRING** activatableClassIds, ...)` outputs an array of `HSTRING` values. `HSTRING` is a distinctly-named opaque string handle whose own ownership mechanism (`WindowsCreateString`/`WindowsDeleteString`) is tracked separately under `winstring.h` (still pending, per `hstring.h`'s report in batch `scraping-investigation-48`) — this function's array-of-`HSTRING` output does not introduce a new type-level gap beyond what's already deferred there.

## Conclusion
`accepted-normalized` — no new ownership annotation required here (COM factory pattern; `HSTRING` array output ownership is tracked separately under `winstring.h`).
