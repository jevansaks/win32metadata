# windows.devices.midi.h

**Classification:** accepted-normalized (no producer-site change needed)

## Summary
MIDL-generated WinRT MIDI device COM/ABI interfaces (`IMidiMessage`,
`IMidiChannelPressureMessage`, etc.). All callable declarations found
are `STDMETHODCALLTYPE` COM vtable methods — out of scope per blocker
class 5 (COM vtable methods). No free-function declarations found.

## Ownership Analysis
No free (non-COM) producer/consumer functions exist. No
`autoTypes.json`/`emitter.settings.rsp` changes required.

## Action
None. Classified accepted-normalized with no patch artifact.
