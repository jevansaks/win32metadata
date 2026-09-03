# winsnmp.h — accepted-normalized (patch applied)

**Partition:** Snmp (`Windows.Win32.NetworkManagement.Snmp`)
**Size:** ~23383 bytes

## Analysis

`WinSNMP.h` (third-party-derived WinSNMP API, cleared for limited SNMP
Trap use) declares five distinct `typedef HANDLE X;` handle types,
each with a clean producer/closer pair, confirmed used only in this
one file (repo-wide grep):

| Type | Producer | Closer |
|---|---|---|
| `HSNMP_SESSION` | `SnmpOpen` | `SnmpClose` |
| `HSNMP_ENTITY` | `SnmpStrToEntity` | `SnmpFreeEntity` |
| `HSNMP_CONTEXT` | `SnmpStrToContext` | `SnmpFreeContext` |
| `HSNMP_PDU` | `SnmpCreatePdu` | `SnmpFreePdu` |
| `HSNMP_VBL` | `SnmpCreateVbl` | `SnmpFreeVbl` |

No prior `autoTypes.json` entries existed for any of these types.

## Patch

Edited `generation/WinSDK/autoTypes.json` — added five new entries
under namespace `Windows.Win32.NetworkManagement.Snmp`.

## Validation

Re-scraped the `Snmp` partition (x64, not `ExcludeFromCrossarch`)
after the `main.cpp` touch: build succeeded, 0 errors (4 pre-existing
unrelated cross-partition remap warnings for `in_addr`/`sockaddr`/
`timeval`, unchanged by this patch).

## Conclusion

Genuine producer-site fix applied — five new ownership-metadata
entries added.
