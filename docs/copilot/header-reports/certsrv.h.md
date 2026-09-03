# certsrv.h — accepted-normalized

**Partition:** Certificates
**Size:** ~63729 bytes

## Analysis

`CertSrv.h` (Main Certificate Server header) is a pure aggregator
header — it only `#include`s sibling headers (`certadm.h`,
`certbcli.h`, `certcli.h`, `certenc.h`, `certexit.h`, `certif.h`,
`certpol.h`, `certmod.h`, `certview.h`) and defines a debug macro. It
contains **no direct declarations of its own**.

## Conclusion

Clean. No patch required.
