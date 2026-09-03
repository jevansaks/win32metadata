# Header Report: pbdaerrors.h

## Partitions
`Media.DShow`

## Ownership audit (producer-site-only policy)
- Pure HRESULT `const` status codes (`MSDRI_S_*`/`BDA_E_*`) and bit-test macros (`IS_BDAERROR`/`IS_BDASUCCESS`). No functions, no handles.

## Conclusion
`accepted-normalized` — no ownership annotation required (constants/macros only, no functions).
