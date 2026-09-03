# Header Report: wct.h

## Partitions
`Base`, `Debug`

## Ownership audit (producer-site-only policy) — BLOCKED (known blocker class)

- `typedef LPVOID HWCT;` — a genuine "wait-chain session" handle (per usage), produced by
  `OpenThreadWaitChainSession(...)` **returned directly as the function return value** (not out-param),
  and released via `CloseThreadWaitChainSession(_In_ HWCT WctHandle)`.
- Checked `autoTypes.json`: no entry exists for `HWCT`.
- This is the same **return-value-handle-ownership blocker class** already documented for
  `getprocesshandlefromhwnd.h`/`wab.h`/`wincon.h`/`winppi.h`/`libloaderapi2.h`/`MSAJTransport.h`/
  `i_cryptasn1tls.h`/`wnvapi.h`: no precedent anywhere in this repo or the published baseline winmd for
  annotating a bare return-value handle, even for a distinctly-named type like `HWCT`.

## Conclusion
`blocked` — genuine `HWCT` ownership relationship via return value; same already-documented
return-value-handle-ownership class, no new investigation required.
