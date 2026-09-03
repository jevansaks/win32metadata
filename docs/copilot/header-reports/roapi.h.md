# Header Report: roapi.h

## Partitions
`TransactionServer`, `WinRT`

## Ownership audit (producer-site-only policy) — two genuine fixes applied
- `RO_REGISTRATION_COOKIE` (opaque `typedef struct _RO_REGISTRATION_COOKIE {} *`) is produced via `RoRegisterActivationFactories(..., _Out_ RO_REGISTRATION_COOKIE* cookie)`, closed via `RoRevokeActivationFactories(_In_ RO_REGISTRATION_COOKIE cookie)`. `autoTypes.json` had an entry but it was **missing `CloseApi`**. Added `"CloseApi": "RoRevokeActivationFactories"`.
- `APARTMENT_SHUTDOWN_REGISTRATION_COOKIE` (`DECLARE_HANDLE`) is produced via `RoRegisterForApartmentShutdown(..., _Out_ APARTMENT_SHUTDOWN_REGISTRATION_COOKIE* regCookie)`, closed via `RoUnregisterForApartmentShutdown(_In_ APARTMENT_SHUTDOWN_REGISTRATION_COOKIE regCookie)`. `autoTypes.json` had an entry but it was also **missing `CloseApi`**. Added `"CloseApi": "RoUnregisterForApartmentShutdown"`.
- Both types are also referenced (not redefined) in the C++-template-only `wrl/implements.h`/`wrl/module.h` headers; the type definition itself exists only in `roapi.h`, so the fix is namespace-safe. Re-scraped `WinRT` partition (0 errors).
- `RoActivateInstance`/`RoGetActivationFactory` output standard COM interface pointers (`IInspectable**`/`void**` with `REFIID`), out of scope. `RoGetApartmentIdentifier` outputs a plain `UINT64`, no handle.

## Conclusion
`accepted-normalized` — two genuine gaps fixed by completing existing `autoTypes.json` entries for `RO_REGISTRATION_COOKIE` and `APARTMENT_SHUTDOWN_REGISTRATION_COOKIE`.
