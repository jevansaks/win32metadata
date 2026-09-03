# webauthn.h — accepted-normalized

**Partition:** WebAuthn
**Size:** ~76998 bytes

## Analysis

`webauthn.h` declares `WebAuthNAuthenticatorMakeCredential`/
`WebAuthNAuthenticatorGetAssertion` (producers of
`PWEBAUTHN_CREDENTIAL_ATTESTATION`/`PWEBAUTHN_ASSERTION`), freed via
`WebAuthNFreeCredentialAttestation`/`WebAuthNFreeAssertion`. Both
output types are **real multi-field structs** (confirmed:
`WEBAUTHN_CREDENTIAL_ATTESTATION` has `dwVersion`, `pwszFormatType`,
`cbAuthenticatorData`, etc. — not an opaque single-pointer disguise),
matching the established transparent-struct-alloc/free-pair
blocker-class 6, out of scope for handle-typedef annotation.

## Conclusion

Clean. No patch required — allocated struct with real fields, not an
opaque handle.
