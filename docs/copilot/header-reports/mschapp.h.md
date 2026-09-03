# Header Report: mschapp.h

## Partitions
`MsChap`

## Scrape validation
- Re-scraped `MsChap` partition (`ScanArch=x86`) after touching `main.cpp`.
- Result: `Build succeeded. 0 Warning(s). 0 Error(s).`

## Ownership audit (producer-site-only policy)
- `MSChapSrvChangePassword`/`MSChapSrvChangePassword2` take only strings and password-encryption data structs (`LM_OWF_PASSWORD`, `NT_OWF_PASSWORD`, `SAMPR_ENCRYPTED_USER_PASSWORD`, etc.) as `_In_` parameters. No `HANDLE` involved.

## Conclusion
`accepted-normalized` — no ownership annotation required (password-encryption struct/string API only, no handle).
