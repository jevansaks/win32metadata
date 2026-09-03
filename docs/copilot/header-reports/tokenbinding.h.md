# Header Report: tokenbinding.h

## Partitions
`Identity`

## Scrape validation
- Re-scraped `Identity` partition (x64) after touching `main.cpp`.
- Result: `Build succeeded. 0 Error(s).`

## Ownership audit (producer-site-only policy)
- `TokenBindingGenerateBinding`/`TokenBindingGenerateMessage` produce a raw `void **` buffer via `_Outptr_result_buffer_`. Per Microsoft documentation (learn.microsoft.com/windows/win32/api/tokenbinding/nf-tokenbinding-tokenbindinggeneratebinding), the caller must free this buffer with `HeapFree(GetProcessHeap(), 0, ...)` — a generic heap-allocation convention, not a distinctly-named opaque handle with its own `Close`/`Free` API. Same out-of-scope class as other generic-buffer conventions (`CoTaskMemFree`, `NetApiBufferFree`).
- `TokenBindingVerifyMessage`/`TokenBindingGetKeyTypesClient`/`Server`/`TokenBindingGenerateID`/`TokenBindingGenerateIDForUri` output `TOKENBINDING_RESULT_DATA**`/`TOKENBINDING_RESULT_LIST**`/`TOKENBINDING_KEY_TYPES**` — fully-defined (transparent) structs, not opaque handles, freed via the same generic heap convention. Out of scope per the transparent-struct-alloc/free-pairs precedent (`dmort.h`/`keycredmgr.h`/`certpoleng.h`).

## Conclusion
`accepted-normalized` — no ownership annotation required (all outputs are generic heap-allocated buffers/transparent structs, not distinctly-named opaque handles).
