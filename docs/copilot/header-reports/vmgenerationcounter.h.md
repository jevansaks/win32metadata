# Header Report: vmgenerationcounter.h

## Partitions
`Hypervisor`

## Ownership audit (producer-site-only policy)
- Declares only a `DEFINE_GUID`, an `IOCTL_VMGENCOUNTER_READ` constant, and the `VM_GENCOUNTER` data struct. No functions, no handles.

## Conclusion
`accepted-normalized` — no ownership annotation required (GUID/IOCTL constant + data struct only, no functions).
