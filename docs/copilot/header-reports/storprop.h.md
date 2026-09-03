# Header Report: storprop.h

## Partitions
`Base`

## Scrape validation
- Re-scraped `Base` partition (`ScanArch=x86`) after touching `main.cpp`.
- Result: `Build succeeded. 0 Warning(s). 0 Error(s).`

## Ownership audit (producer-site-only policy)
- `CdromCddaInfo`/`CdromKnownGoodDigitalPlayback`/`CdromEnableDigitalPlayback`/`CdromDisableDigitalPlayback`/`CdromIsDigitalPlaybackEnabled` all take `HDEVINFO`/`PSP_DEVINFO_DATA` as `_In_` parameters — these are existing SetupAPI device-info-set handles/data, consumed here, not produced by this header (the `HDEVINFO` producer/consumer pair belongs to `setupapi.h`, a separate, well-established Windows API). `CdromSetDefaultDvdRegion` takes only a `GEOID`. `REDBOOK_DIGITAL_AUDIO_EXTRACTION_INFO` is a plain data struct.

## Conclusion
`accepted-normalized` — no ownership annotation required (HDEVINFO is a consumer-only input from the separate, already-established SetupAPI device-info-set surface).
