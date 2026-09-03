# Header Report: Windows.h

## Partitions
`Console`, `Edp`, `Enstor`, `Memory`, `Mi` (and many others; master umbrella)

## Redirect-only-header safety test
Pure master umbrella header for Windows applications, including `windef.h`/`winbase.h`/`wingdi.h`/`winuser.h`/`winver.h`/`winreg.h`/`winnetwk.h`/`cderr.h`/`dde.h`/`lzexpand.h`/`mmsystem.h`/`rpc.h`/`shellapi.h`/`winsock.h`/`wincrypt.h`/`winefs.h`/`winspool.h`/`ole2.h`/`commdlg.h`/`winsvc.h` (all `accepted-normalized`), `winnls.h`/`ddeml.h`/`nb30.h`/`winperf.h`/`winscard.h`/`stralign.h`/`mcx.h`/`imm.h` (all `pending`), `wincon.h` (`blocked`, already investigated). One included header — `dlgs.h` — is **not a tracked ledger item**; investigated directly and confirmed it declares only dialog-control-ID constants, no functions (safe).

## Ownership audit (producer-site-only policy)
- No direct declarations in this file itself — pure `#include` redirection with `NO*` feature-exclusion macro documentation.

## Conclusion
`accepted-normalized` — pure umbrella redirect header; all sub-headers are either tracked ledger items or (for the one untracked exception, `dlgs.h`) directly confirmed to contain no functions.
