# mdmregistration.h — accepted-normalized

**Partition:** MdmReg
**Size:** ~14361 bytes

## Analysis

`mdmregistration.h` declares the MDM device-registration API:
`GetDeviceRegistrationInfo`, `IsDeviceRegisteredWithManagement`,
`DiscoverManagementService`/`DiscoverManagementServiceEx`,
`RegisterDeviceWithManagement*`, `UnregisterDeviceWithManagement`,
`GetDeviceManagementConfigInfo`, etc.

- `GetDeviceRegistrationInfo(_In_ REGISTRATION_INFORMATION_CLASS
  DeviceInformationClass, _Out_ PVOID* ppDeviceRegistrationInfo)`
  outputs an **untyped `PVOID`** whose actual pointee type varies by
  the `DeviceInformationClass` enum value — a known unrepresentable
  pattern (untyped `PVOID` ownership varies per call, no single type
  to annotate).
- `DiscoverManagementService`/`Ex` output
  `PMANAGEMENT_SERVICE_INFO*` — `MANAGEMENT_SERVICE_INFO` is a
  transparent struct with no dedicated free function found in this
  header, out of scope per the transparent-struct convention
  (blocker-class 6) regardless.
- `RegisterDeviceWithManagementUsingAADCredentials(HANDLE UserToken)`
  takes a caller-owned generic `HANDLE` as input, not produced here.

## Conclusion

Clean — no representable ownership metadata gap under current policy
(untyped `PVOID` and transparent-struct patterns are both
out-of-scope classes). No patch required.
