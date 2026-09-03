# TSPI.h — accepted-normalized

**Partition:** Tapi3
**Size:** ~46682 bytes

## Analysis

`tspi.h` (Telephony Service Provider Interface) declares the SPI
contract functions that a **Telephony Service Provider (TSP) DLL must
implement** — called by TAPI, not exported by a system DLL for
applications to P/Invoke (per doc comment: "the API/SPI for making
applications/drivers that interface with the specification"). No
representable producer-site ownership pattern exists here, consistent
with the `raseapif.h`/`winwlx.h` legacy-provider-contract precedent.

## Conclusion

Clean. No patch required — no representable ownership gap.
