# computenetwork.h — accepted-normalized (patch applied)

**Partition:** HostComputeNetwork (`Windows.Win32.System.HostComputeNetwork`)
**Size:** ~16627 bytes

## Analysis

`computenetwork.h` (Host Compute Network API, ApiSet
`ext-ms-win-hyperv-computenetwork-l1`) declares seven distinct
`typedef void* HCN_X;` handle types. Six have confirmed clean
producer/closer pairs (verified single-file usage via repo-wide
grep); the seventh (`HCN_SERVICE`) is declared but never used as a
parameter by any function in this header — no producer or consumer,
left unannotated.

| Type | Producer | Closer |
|---|---|---|
| `HCN_NETWORK` | `HcnCreateNetwork`/`HcnOpenNetwork` | `HcnCloseNetwork` |
| `HCN_NAMESPACE` | `HcnCreateNamespace`/`HcnOpenNamespace` | `HcnCloseNamespace` |
| `HCN_ENDPOINT` | `HcnCreateEndpoint`/`HcnOpenEndpoint` | `HcnCloseEndpoint` |
| `HCN_LOADBALANCER` | `HcnCreateLoadBalancer`/`HcnOpenLoadBalancer` | `HcnCloseLoadBalancer` |
| `HCN_GUESTNETWORKSERVICE` | `HcnCreateGuestNetworkService`/`HcnOpenGuestNetworkService` | `HcnCloseGuestNetworkService` |
| `HCN_CALLBACK` | `HcnRegisterServiceCallback` (`_Outptr_ HCN_CALLBACK* CallbackHandle`) | `HcnUnregisterServiceCallback` |
| `HCN_SERVICE` | *(none — declared, unused)* | *(none)* |

No prior `autoTypes.json` entries existed for any of these types.

## Patch

Edited `generation/WinSDK/autoTypes.json` — added six new entries
(`HCN_NETWORK`, `HCN_NAMESPACE`, `HCN_ENDPOINT`, `HCN_LOADBALANCER`,
`HCN_GUESTNETWORKSERVICE`, `HCN_CALLBACK`) under namespace
`Windows.Win32.System.HostComputeNetwork`. `HCN_SERVICE` left
unannotated (no representable producer/consumer pair).

## Validation

Re-scraped the `HostComputeNetwork` partition (x64, not
`ExcludeFromCrossarch`) after the `main.cpp` touch: build succeeded, 0
warnings, 0 errors.

## Conclusion

Genuine producer-site fix applied — six new ownership-metadata
entries added.
