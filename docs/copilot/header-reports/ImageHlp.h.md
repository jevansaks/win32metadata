# ImageHlp.h — accepted-normalized

**Partition:** Debug
**Size:** ~132888 bytes

## Analysis

`ImageHlp.h` declares `ImageLoad`/`ImageUnload` operating on
`LOADED_IMAGE` — confirmed a **real multi-field struct**
(`ModuleName`, `hFile`, `MappedAddress`, `FileHeader`,
`LastRvaSection`, `NumberOfSections`, etc.), not an opaque
single-pointer disguise — matches the transparent-struct blocker-
class 6, out of scope. `MapAndLoad` populates a **caller-provided**
`LOADED_IMAGE` (`_Out_ PLOADED_IMAGE LoadedImage`, not `PLOADED_IMAGE*`)
— the "populate caller-provided struct" pattern, also out of scope.

## Conclusion

Clean. No patch required — real transparent struct / caller-provided
struct, not an opaque handle.
