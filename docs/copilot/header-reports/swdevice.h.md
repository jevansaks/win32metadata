# Header Report: swdevice.h

## Partitions
`SwDevice`

## Scrape validation
- `SwDevice` partition previously re-scraped this session (batch `scraping-investigation-25`); result `0 Warning(s), 0 Error(s)`.

## Ownership audit (producer-site-only policy)
- `DECLARE_HANDLE(HSWDEVICE)` is a genuine opaque handle. `SwDeviceCreate(..., _Out_ PHSWDEVICE phSwDevice)` produces it via out-parameter; `SwDeviceClose(_In_ HSWDEVICE hSwDevice)` releases it — this is exactly the shape of a fixable producer-site ownership gap (like `appnotify.h`, batch `scraping-investigation-39`).
- **However, checked `autoTypes.json` first**: `HSWDEVICE` **already has a complete, correct-looking
  ownership entry** — `"ValueType": "DECLARE_HANDLE", "CloseApi": "SwDeviceClose", "InvalidHandleValues": [-1, 0]`
  — matching exactly what is observed in this header (`SwDeviceClose` is indeed the release function).
  This is the same situation as the GDI handles (`HBITMAP`/`HFONT`/etc.) already covered by
  `autoTypes.json` and confirmed in batch `scraping-investigation-14`'s deep-dive: an existing, working,
  correct legacy typedef-based ownership entry needs no new inline producer-site duplicate — the
  corrected policy (commits `165b5f09`/`7335ddc4`) targets *removing* redundant/incorrect typedef
  annotations and *adding* new ones only where no metadata exists yet, not re-deriving metadata that
  already exists and is not shown to be wrong.
- `SwDeviceInterfaceRegister`'s `_Outptr_result_maybenull_ PWSTR* ppszDeviceInterfaceId` (freed via
  `SwMemFree`) is a generic string-buffer allocation, out of scope for the `HANDLE`-specific mechanism
  (same reasoning as `resourceindexer.h`/`dmprocessxmlfiltered.h`).

## Conclusion
`accepted-normalized` — `HSWDEVICE` ownership is already correctly captured via the existing `autoTypes.json` entry (`CloseApi: SwDeviceClose`, matching this header exactly); no gap to fix, no new annotation needed. `SwDeviceInterfaceRegister`'s string output uses `SwMemFree`, out of scope for the HANDLE mechanism.
