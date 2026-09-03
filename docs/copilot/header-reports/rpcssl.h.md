# Header Report: rpcssl.h

## Partitions
`Rpc`

## Scrape validation
- `Rpc` partition previously re-scraped this session (batch `scraping-investigation-28`); result `0 Error(s)` (1 pre-existing unrelated cross-partition remap warning).

## Ownership audit (producer-site-only policy)
- `RpcCertGeneratePrincipalName(W/A)(_In_ PCCERT_CONTEXT, DWORD, _Outptr_ RPC_WSTR/RPC_CSTR* pBuffer)` outputs an `RPC_WSTR`/`RPC_CSTR` string buffer (freed via `RpcStringFree`, the standard RPC string-allocation convention) — analogous to `BSTR` allocation, out of scope for the `HANDLE`-specific ownership mechanism. `PCCERT_CONTEXT` is a caller-supplied input, not produced here.

## Conclusion
`accepted-normalized` — no ownership annotation required (RPC string-buffer output, not a HANDLE-family resource).
