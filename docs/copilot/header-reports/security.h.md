# Header Report: security.h

## Partitions
`Identity`

## Scrape validation
- Re-scraped `Identity` partition (`ScanArch=x86`) after touching `main.cpp`.
- Result: `Build succeeded. 0 Error(s).` (3 pre-existing unrelated cross-partition remap warnings for `_CERT_CONTEXT`/`SecPkgContext_IssuerListInfoEx`, not touched by this header).

## Ownership audit (producer-site-only policy)
- `security.h` is a redirect-only header: name-string macros (`NTLMSP_NAME`, `MICROSOFT_KERBEROS_NAME`, `NEGOSSP_NAME`, `CLOUDAP_NAME`) plus `#include <sspi.h>` (already `accepted-normalized`), `#include <secext.h>` (conditional, only when `SECURITY_WIN32`/`SECURITY_KERNEL` defined), and `#include <issper16.h>` (conditional, `ISSP_LEVEL == 16`, a legacy/rare path).
- `issper16.h` is already tracked in the ledger (still `pending`).
- `secext.h` is **not** itself a tracked ledger item, so — per the `wincon.h` lesson — its content was
  read directly: it declares `GetUserNameExA/W`, `GetComputerObjectNameA/W`, `TranslateNameA/W`, all of
  which output plain string buffers (`LPSTR`/`LPWSTR` with a `PULONG` size) — no `HANDLE` involved. All
  already carry `_Win32_metadata_set_last_error_` annotations (consistent with the global
  `WithSetLastError.rsp`/inline-annotation pattern used elsewhere).

## Conclusion
`accepted-normalized` — redirect-only header; `sspi.h` already accepted, `issper16.h` already tracked (pending), and `secext.h` (untracked) was directly verified to contain only string-buffer-output functions, no `HANDLE`-family ownership gap.
