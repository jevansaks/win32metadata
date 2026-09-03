# p2p.h — accepted-normalized (producer-site fix applied)

**Partition:** P2p
**Size:** ~72185 bytes

## Analysis

`p2p.h` (Peer-To-Peer infrastructure APIs) declares two genuine
opaque handle typedefs, both `typedef PVOID`/`void*`:

- **`HGRAPH`** — produced by `PeerGraphCreate`/`PeerGraphOpen`
  (`_Out_ HGRAPH*`), destroyed by single-arg
  `PeerGraphClose(HGRAPH)`.
- **`HPEERENUM`** — produced by many different `PeerGraphEnum*`
  functions (`PeerGraphEnumRecords`, `PeerGraphEnumConnections`,
  etc., all `_Out_ HPEERENUM*`), all sharing the single destroyer
  `PeerGraphEndEnumeration(HPEERENUM)`.

Checked `autoTypes.json` first: **no entries existed for either
type.** Confirmed single-file usage, `P2p` partition (not
`ExcludeFromCrossarch`), namespace `Windows.Win32.NetworkManagement.P2P`
(from partition `settings.rsp`). Added both new entries (`ValueType:
DECLARE_HANDLE`, `InvalidHandleValues: [0]`). Validated via `dotnet
build generation/WinSDK -c Release -p:ScanArch=x64 -t:ScrapeHeaders
-p:PartitionFilter=P2p` → 0 errors.

## Conclusion

Producer-site fix applied — new `HGRAPH` and `HPEERENUM`
`autoTypes.json` entries added.
