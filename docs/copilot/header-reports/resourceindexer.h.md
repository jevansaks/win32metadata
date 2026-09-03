# Header Report: resourceindexer.h

## Partitions
`MenuRc`

## Ownership audit (producer-site-only policy) — BLOCKED

- `CreateResourceIndexer(..., _Outptr_ PVOID* ppResourceIndexer)` produces an opaque indexer handle;
  `DestroyResourceIndexer(_In_opt_ PVOID resourceIndexer)` releases it. This is a genuine, well-defined
  producer/consumer ownership relationship, expressed via an **out-parameter** (unlike the
  `getprocesshandlefromhwnd.h`/`wab.h`/`wincon.h` return-value class) — so the *placement* is not the
  problem here.
- The blocker is different: the parameter type is **generic, untyped `PVOID`** (`void*`), not a
  distinctly-named handle typedef (unlike `TBS_HCONTEXT`, `BCRYPT_ALG_HANDLE`, etc. used in every other
  fixed case this session). Since `_Win32_metadata_raii_free_`/`_Win32_metadata_invalid_handle_`
  ultimately attach `RAIIFree`/`InvalidHandleValue` metadata to the parameter's **type declaration**
  (confirmed via the `WinmdUtils.exe dump` investigation in `getprocesshandlefromhwnd.h.md` — every
  occurrence in the published winmd attaches to a `struct` type, never a parameter), annotating a bare
  `PVOID`/`void*` parameter here would incorrectly apply `RAIIFree(DestroyResourceIndexer)` to **every**
  `void*` value anywhere in the entire published metadata — a clear correctness bug, not a fix.
- Similarly, `IndexFilePath`'s `_Outptr_ PWSTR* ppResourceUri` (freed via `DestroyIndexedResults`) is a
  string-buffer allocation, not a `HANDLE`-family resource — outside the scope of this mechanism
  entirely (consistent with `BSTR`/memory-buffer patterns already classified clean elsewhere in this
  session without needing this annotation).
- Fixing `CreateResourceIndexer`/`DestroyResourceIndexer` correctly would require first introducing a
  new, distinctly-named opaque handle typedef (e.g. `HRESOURCEINDEXER`) in place of the current generic
  `PVOID` — a header content/design change beyond what an annotation-only patch can safely do, and a
  decision this audit is not positioned to make unilaterally.

## Conclusion
`blocked` — genuine ownership relationship (`CreateResourceIndexer`/`DestroyResourceIndexer`), correctly
expressed via out-param, but the parameter's generic untyped `PVOID` type means the current
type-level annotation mechanism cannot be applied without first introducing a new named handle type — a
design decision, not a mechanical annotation fix.
