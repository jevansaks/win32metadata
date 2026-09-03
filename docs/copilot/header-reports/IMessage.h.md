# Header Report: IMessage.h

## Partitions
`IMapi`

## Ownership audit (producer-site-only policy) — genuine fix applied
- `LPMSGSESS` (`typedef struct _MSGSESS FAR *`) is produced via `OpenIMsgSession(..., LPMSGSESS FAR *lppMsgSess)`, closed via `CloseIMsgSession(LPMSGSESS lpMsgSess)`. `autoTypes.json` had an entry for `LPMSGSESS` but it was **missing `CloseApi`** — the same incomplete-entry pattern previously found for `appnotify.h`/`packagevirtualizationcontext.h`/`dciman.h`/`devquery.h`/`roapi.h`. Added `"CloseApi": "CloseIMsgSession"`. Confirmed the type/close-function pair is declared only in this one header. Re-scraped `IMapi` partition (0 errors).
- `OpenIMsgOnIStg`'s `LPMESSAGE FAR *lppMsg` output is a standard COM interface pointer, out of scope.

## Conclusion
`accepted-normalized` — genuine gap fixed via completing the existing `autoTypes.json` entry for `LPMSGSESS` (`CloseApi: CloseIMsgSession`).
