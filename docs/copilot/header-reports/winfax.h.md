# winfax.h — accepted-normalized (producer-site fix applied)

**Partition:** Fax
**Size:** ~65135 bytes

## Analysis

`winfax.h` (Windows Fax API) declares `FaxConnectFaxServerA/W`
(`_Out_ LPHANDLE FaxHandle`) and `FaxOpenPort` (`_Out_ LPHANDLE
FaxPortHandle`) — both output the generic `HANDLE` type directly (not
distinct typedefs) — closed via the single generic `FaxClose(HANDLE)`
(confirmed no dedicated `FaxClosePort`; `FaxClose` documented to
close both server and port handles).

Checked `emitter.settings.rsp` first: **no existing inline
annotations for any of the three functions** — a genuine gap. Added
`FaxConnectFaxServerA/W::FaxHandle=[RAIIFree("FaxClose")]` and
`FaxOpenPort::FaxPortHandle=[RAIIFree("FaxClose")]`, consistent with
the `CfOpenFileWithOplock`/`QOSCreateHandle` precedent for
semantically-specific generic-`HANDLE` outputs.

## Conclusion

Producer-site fix applied — 3 new inline `RAIIFree` parameter
annotations added in `emitter.settings.rsp`.
