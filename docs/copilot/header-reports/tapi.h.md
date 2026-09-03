# tapi.h

**Classification:** accepted-normalized (producer-site fixes applied)

## Summary
Telephony API (TAPI) 2.x/3.0 core header. Confirmed 4 genuine
producer/consumer handle typedefs previously missing ownership metadata,
all declared via `DECLARE_OPAQUE32(name)` which expands to
`typedef DWORD name` (fixed 32-bit scalar, not pointer-sized):

| Typedef     | Producer (out-param)                              | Consumer (single-arg close) |
|-------------|----------------------------------------------------|------------------------------|
| `HLINEAPP`  | `lineInitialize`/`lineInitializeExA/W` (`LPHLINEAPP`) | `lineShutdown(HLINEAPP)`   |
| `HLINE`     | `lineOpen` (`LPHLINE lphLine`)                      | `lineClose(HLINE)`          |
| `HPHONEAPP` | `phoneInitialize`/`phoneInitializeExA/W` (`LPHPHONEAPP`) | `phoneShutdown(HPHONEAPP)` |
| `HPHONE`    | `phoneOpen` (`LPHPHONE lphPhone`)                   | `phoneClose(HPHONE)`        |

All four close functions take the handle **by value** (single argument),
matching the standard `CloseApi` mechanism (no by-pointer blocker
sub-class issue here).

## Ownership Analysis
Added 4 new `autoTypes.json` entries (namespace
`Windows.Win32.Devices.Tapi`, partition `Tapi3`):
- `HLINE` → `ValueType: "uint"`, `CloseApi: "lineClose"`, `InvalidHandleValues: [0]`
- `HPHONE` → `ValueType: "uint"`, `CloseApi: "phoneClose"`, `InvalidHandleValues: [0]`
- `HLINEAPP` → `ValueType: "uint"`, `CloseApi: "lineShutdown"`, `InvalidHandleValues: [0]`
- `HPHONEAPP` → `ValueType: "uint"`, `CloseApi: "phoneShutdown"`, `InvalidHandleValues: [0]`

`ValueType: "uint"` follows the `DECLARE_OPAQUE32(name)` macro expansion
(`typedef DWORD name`), consistent with the existing `WINBIO_SESSION_HANDLE`/
`DRMHANDLE`-style uint-based handle precedents. No sentinel invalid-value
macro is defined in the header, so `0` (NULL-equivalent) is used, matching
the same precedent set.

Other TAPI handles in this header (`HCALL`, `HCALLHUB`, `HAGENTSESSION`,
etc.) were not investigated in this pass — out of scope for this ticket;
may be revisited in a future batch if flagged.

## Validation
`ScrapeHeaders x64 Tapi3` → Build succeeded, 0 Error(s).

## Action
Producer-site fix applied via `autoTypes.json` (4 new entries).
