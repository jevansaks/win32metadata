# Header Report: ipinfoid.h

## Partitions
`IpHlp`

## Ownership audit (producer-site-only policy)
- Pure `#define` integer ID constants (`IP_ROUTER_MANAGER_VERSION`, `IP_*_INFO`) for the IP router manager. No functions, no handles.

## Conclusion
`accepted-normalized` — no ownership annotation required (constants only, no functions).
