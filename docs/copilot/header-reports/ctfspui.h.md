# Header Report: ctfspui.h

## Partitions
`Tsf`

## Ownership audit (producer-site-only policy)
- Declares only the COM interface `ITfSpeechUIServer` (`Initialize`/`ShowUI`/`UpdateBalloon`, all `_In_` inputs, no outputs) plus `CLSID_SpeechUIServer`/`IID_ITfSpeechUIServer` GUID constants. COM vtable method — out of scope; no `extern`/`DllImport` functions.

## Conclusion
`accepted-normalized` — no ownership annotation required (COM interface method + GUID constants only, no extern functions).
