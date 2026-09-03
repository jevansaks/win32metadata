# Header Report: mmsystem.h

## Partitions
`Audio`, `Audio.Apo`, `Audio.DirectMusic`, `Audio.DirectSound`, `Audio.Endpoints`, `Base`, `Devices.Properties`, and others (umbrella header referenced across many partitions)

## Redirect-only-header safety test
Pure umbrella header including: `mmsyscom.h` (pending), `mciapi.h` (pending), `mmiscapi.h` (pending), `mmiscapi2.h` (accepted-normalized), `playsoundapi.h` (accepted-normalized), `mmeapi.h` (accepted-normalized), `timeapi.h` (accepted-normalized), `joystickapi.h` (pending). Every included sub-header is already a tracked ledger item (any status) — per the established redirect-only-header safety test, no live-scrape is required beyond confirming the umbrella itself has no direct declarations.

## Ownership audit (producer-site-only policy)
- Only direct declarations in the file itself are `#define` display/escape constants (`NEWTRANSPARENT`, `QUERYROPSUPPORT`, `SELECTDIB`, `DIBINDEX`, `SC_SCREENSAVE`) — no functions, no handles.

## Conclusion
`accepted-normalized` — pure umbrella redirect; all sub-headers already individually tracked ledger items; no direct ownership-relevant declarations.
