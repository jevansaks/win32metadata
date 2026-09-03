# Header Report: wlantypes.h

## Partitions
`Ndis`, `NWifi`

## Ownership audit (producer-site-only policy)
- Declares only 802.11 (WLAN) enums (`DOT11_BSS_TYPE`, `DOT11_AUTH_ALGORITHM`, `DOT11_CIPHER_ALGORITHM`) and plain data structs (`DOT11_SSID`, `DOT11_AUTH_CIPHER_PAIR`, `DOT11_OI`, `DOT11_ACCESSNETWORKOPTIONS`, `DOT11_VENUEINFO`). No functions.

## Conclusion
`accepted-normalized` — no ownership annotation required (enums/structs only, no functions).
