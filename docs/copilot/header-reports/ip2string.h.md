# Header Report: ip2string.h

## Partitions
`IpHlp`, `wnv`

## Ownership audit (producer-site-only policy)
- `RtlIpv4AddressToStringA` and its family write into a caller-supplied buffer and return a `PSTR`/`PWSTR` pointing into that same caller buffer (not a newly-allocated resource) — no ownership transfer, no opaque handle.

## Conclusion
`accepted-normalized` — no ownership annotation required (string-conversion functions write into caller buffers only, no handles).
