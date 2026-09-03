# sslprovider.h — accepted-normalized

**Partition:** Security.Cryptography
**Size:** ~59354 bytes

## Analysis

`sslprovider.h` (SSL protocol provider API) declares only structs
(`NCRYPT_SSL_CIPHER_SUITE`, etc.) and function-pointer typedefs
(`typedef SECURITY_STATUS (*...)`) that an SSL provider DLL must
implement — a callback contract table, not directly P/Invokable
functions. No representable ownership pattern.

## Conclusion

Clean. No patch required — no representable ownership gap.
