# msidefs.h — accepted-normalized

**Partition:** Setup
**Size:** ~39511 bytes

## Analysis

`msidefs.h` (Windows Installer definitions) declares only enums/
constants (`msidbLocatorType`, `msidbIniFileAction`,
`msidbODBCDataSourceRegistration`, etc.) and `NTDDI_*` version
macros. It contains **no function declarations**.

## Conclusion

Clean. No patch required.
