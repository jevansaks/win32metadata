# peerdist.h — accepted-normalized (patch applied)

**Partition:** P2p (`Windows.Win32.NetworkManagement.P2P`)
**Size:** ~10995 bytes

## Analysis

`peerdist.h` declares the BranchCache/Peer Distribution API. It
defines four distinct, single-purpose `typedef HANDLE X;`
handle-family types, each with a clear producer function (an
`_Out_ PPEERDIST_*_HANDLE` parameter) and a clear dedicated
close/free function — confirmed used **only in this one file**
(repo-wide grep):

| Type | Producer | Closer |
|---|---|---|
| `PEERDIST_INSTANCE_HANDLE` | `PeerDistStartup` (`_Out_ PPEERDIST_INSTANCE_HANDLE phPeerDist`) | `PeerDistShutdown` |
| `PEERDIST_STREAM_HANDLE` | `PeerDistServerPublishStream` (`_Out_ PPEERDIST_STREAM_HANDLE phStream`) | `PeerDistServerCloseStreamHandle` |
| `PEERDIST_CONTENTINFO_HANDLE` | `PeerDistServerOpenContentInformation` (`_Out_ PPEERDIST_CONTENTINFO_HANDLE phContentInfo`) | `PeerDistServerCloseContentInformation` |
| `PEERDIST_CONTENT_HANDLE` | `PeerDistClientOpenContent` (`_Out_ PPEERDIST_CONTENT_HANDLE phContentHandle`) | `PeerDistClientCloseContent` |

No `autoTypes.json` entries existed for any of these four types prior
to this patch. Per the producer-site-only ownership policy (genuine,
distinctly-named, single-purpose `HANDLE` subtype with a dedicated
close function, verified single-file usage), added four new
`autoTypes.json` entries (`ValueType: DECLARE_HANDLE`,
`InvalidHandleValues: [0]`, `NativeTypedef: true`), matching the
established `EC_HANDLE`/`HDEVQUERY` pattern for `typedef HANDLE X;`
declarations.

## Patch

Edited `generation/WinSDK/autoTypes.json` — added entries for
`PEERDIST_INSTANCE_HANDLE`, `PEERDIST_STREAM_HANDLE`,
`PEERDIST_CONTENTINFO_HANDLE`, `PEERDIST_CONTENT_HANDLE` under
namespace `Windows.Win32.NetworkManagement.P2P`.

## Validation

Re-scraped the `P2p` partition (x64, not `ExcludeFromCrossarch`) after
the `main.cpp` touch: build succeeded, 0 warnings, 0 errors.

## Conclusion

Genuine producer-site fix applied. Four new ownership-metadata entries
added.
