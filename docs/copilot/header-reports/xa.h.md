# Header Report: xa.h

## Partitions
`DTC`

## Ownership audit (producer-site-only policy)
- `ax_reg`/`ax_unreg` are `extern` functions operating only on `int`/`XID*`/`long` — no opaque handle produced.
- `xa_switch_t`'s `xa_open_entry`/`xa_close_entry`/`xa_start_entry`/etc. are function-pointer *fields* inside a struct (dynamically dispatched by the XA-compliant resource manager), not `extern`/`DllImport` declarations — out of scope, same class as `mswsockdef.h`/`dxva2swdev.h`.

## Conclusion
`accepted-normalized` — no ownership annotation required (no function produces an opaque handle; XA switch entries are function-pointer struct fields, out of scope).
