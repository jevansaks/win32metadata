# Header Report: tpcerror.h

## Partitions
`Tablet`

## Ownership audit (producer-site-only policy)
- Pure HRESULT `#define` error codes (`E_INK_*`/`TPC_E_*`/`TPC_S_*`) and unrelated ATL error-reporting macros (`MAKE_OBJ_ERROR_INFO`/`MAKE_INT_ERROR_INFO`, not actual declarations). No functions, no handles.

## Conclusion
`accepted-normalized` — no ownership annotation required (constants/macros only, no functions).
