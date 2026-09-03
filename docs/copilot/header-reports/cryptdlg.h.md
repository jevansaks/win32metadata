# cryptdlg.h — accepted-normalized

**Partitions:** Security.Cryptography, Security.Cryptography.UI
**Size:** ~17683 bytes

## Analysis

`cryptdlg.h` (Common Cryptographic Dialog API) declares
`CertSelectCertificateA`/`W`, `CertViewPropertiesA`/`W`,
`GetFriendlyNameOfCertA`/`W`, `CertModifyCertificatesToTrust`. All
take transparent structs (`PCERT_SELECT_STRUCT_A`) or existing
`PCCERT_CONTEXT`/`HWND` as input, with no opaque handle production.

## Conclusion

Clean. No opaque handle production. No patch required.
