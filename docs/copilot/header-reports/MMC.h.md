# mmc.h

**Classification:** accepted-normalized (no producer-site change needed)

## Summary
Microsoft Management Console (MMC) snap-in COM interfaces plus 5 free
`STDAPI` helper functions: `MMCPropertyChangeNotify`, `MMCPropertyHelp`,
`MMCFreeNotifyHandle`, `MMCPropPageCallback`, `MMCAfxPropPageCallback`.

`MMCPropertyChangeNotify`/`MMCFreeNotifyHandle` both take a
`LONG_PTR lNotifyHandle` parameter — a generic `LONG_PTR` scalar, not a
distinct handle typedef, and no producer function returning this value
exists anywhere in this header (the notify handle originates from a COM
property-notification mechanism external to this header's free-function
surface). This is the established generic/shared-type blocker (class 2):
`LONG_PTR` is reused pervasively and not annotatable without a dedicated
typedef.

## Ownership Analysis
`lNotifyHandle` is a generic scalar (`LONG_PTR`), not a distinct
typedef — not annotatable per the generic/shared-type blocker class. No
`autoTypes.json`/`emitter.settings.rsp` changes required.

## Action
None. Classified accepted-normalized with no patch artifact.
