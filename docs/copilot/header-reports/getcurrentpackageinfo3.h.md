# Header Report: getcurrentpackageinfo3.h

## Partitions
`AppxPackaging`

## Ownership audit (producer-site-only policy)
- Single function: `HRESULT GetCurrentPackageInfo3(UINT32 flags, PackageInfo3Type packageInfoType, UINT32* bufferLength, void* buffer, UINT32* count)`. All out-parameters are byte-buffer/count values (`UINT32*`, caller-allocated `void*` buffer) — no `HANDLE`/`DECLARE_HANDLE` involved.

## Conclusion
`accepted-normalized` — no ownership annotation required (buffer-fill API, no handle production).
