# computecore.h — accepted-normalized (patch applied)

**Partition:** HostComputeSystem (`Windows.Win32.System.HostComputeSystem`)
**Size:** ~17018 bytes

## Analysis

`ComputeCore.h` (Hyper-V Host Compute System core API, ApiSet
`ext-ms-win-hyperv-compute-l1`) contains the producer/consumer
functions for three `DECLARE_HANDLE`-based types declared in the
included `ComputeDefs.h` (not separately ledger-tracked):

| Type | Producer | Closer |
|---|---|---|
| `HCS_SYSTEM` | `HcsCreateComputeSystem`/`HcsCreateComputeSystemInNamespace`/`HcsOpenComputeSystem`/`HcsOpenComputeSystemInNamespace` (`_Out_ HCS_SYSTEM* computeSystem`) | `HcsCloseComputeSystem` |
| `HCS_PROCESS` | `HcsCreateProcess`/`HcsOpenProcess` (`_Out_ HCS_PROCESS* process`) | `HcsCloseProcess` |
| `HCS_OPERATION` | `HcsCreateOperation`/`HcsCreateOperationWithNotifications` (return value) | `HcsCloseOperation` |
| `HCS_CALLBACK` | *(none found — declared in `ComputeDefs.h`, no producer/consumer function anywhere in the scraped surface)* | *(none)* |

`HCS_SYSTEM`/`HCS_PROCESS`/`HCS_OPERATION` are also referenced (as
input parameters only, not produced) in `hypervdevicevirtualization.h`
(`Hypervisor` partition, already `accepted-normalized`) — confirmed no
namespace conflict via re-scrape of both partitions. No prior
`autoTypes.json` entries existed for any of the four types.

## Patch

Edited `generation/WinSDK/autoTypes.json` — added three new entries
(`HCS_SYSTEM`, `HCS_PROCESS`, `HCS_OPERATION`) under namespace
`Windows.Win32.System.HostComputeSystem`. `HCS_CALLBACK` left
unannotated (no representable producer/consumer pair, same as
`HCN_SERVICE` in `computenetwork.h`).

## Validation

Re-scraped both the `HostComputeSystem` and `Hypervisor` partitions
(x64, neither `ExcludeFromCrossarch`) after touching their
`main.cpp` files: both builds succeeded, 0 warnings, 0 errors.

## Conclusion

Genuine producer-site fix applied — three new ownership-metadata
entries added.
