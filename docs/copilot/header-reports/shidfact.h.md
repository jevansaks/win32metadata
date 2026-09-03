# shidfact.h — accepted-normalized

**Partition:** Shell
**Size:** ~11785 bytes

## Analysis

`shidfact.h` declares a C++ template class (`CItemIDFactory<T, Magic>`
implementing `IDelegateFolder`) entirely within `#ifdef __cplusplus`.
It contains **no `extern "C"` functions and no `STDAPI` declarations**
— confirmed via grep (0 matches for both). C++ template classes are
not scrapable/emittable declarations.

## Conclusion

Clean. No scrapable functions, no opaque handle production. No patch
required.
