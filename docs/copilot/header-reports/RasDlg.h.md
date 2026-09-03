# Header Report: RasDlg.h

## Partitions
`RRas`

## Ownership audit (producer-site-only policy)
- `RasPhonebookDlgA/W`, `RasEntryDlgA/W`, `RasDialDlgA/W` all take transparent, caller-allocated info structs (`LPRASPBDLGW`/`LPRASENTRYDLGW`/etc.) by pointer — no opaque handle produced or returned.
- `RASPBDLGFUNCW`/`RASPBDLGFUNCA` are function-pointer typedefs for caller-supplied callbacks, not `extern`/`DllImport` declarations.

## Conclusion
`accepted-normalized` — no ownership annotation required (no function produces an opaque handle).
