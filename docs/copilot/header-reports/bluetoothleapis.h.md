# bluetoothleapis.h — accepted-normalized (patch applied)

**Partition:** Bluetooth (`Windows.Win32.Devices.Bluetooth`)
**Size:** ~26278 bytes

## Analysis

`BluetoothLEApis.h` (Bluetooth LE application development interfaces)
declares `BluetoothGATTGetServices`/`GetCharacteristics`/etc. (all
take generic, caller-owned `HANDLE hDevice` as input, output
transparent `BTH_LE_GATT_*` struct arrays — no opaque handle
production), plus
`BluetoothGATTRegisterEvent(..., _Out_ BLUETOOTH_GATT_EVENT_HANDLE *
pEventHandle, ...)`, freed via
`BluetoothGATTUnregisterEvent(_In_ BLUETOOTH_GATT_EVENT_HANDLE
EventHandle, ...)`.

`BLUETOOTH_GATT_EVENT_HANDLE` (`typedef HANDLE
BLUETOOTH_GATT_EVENT_HANDLE;`, declared in the sibling `bthledef.h`,
already `accepted-normalized` — that header itself declares no
functions, so no stale-closure issue) is confirmed used only in these
two files (repo-wide grep), both in the `Bluetooth` partition. No
prior `autoTypes.json` entry existed.

## Patch

Edited `generation/WinSDK/autoTypes.json` — added a new entry for
`BLUETOOTH_GATT_EVENT_HANDLE` (`CloseApi:
BluetoothGATTUnregisterEvent`) under namespace
`Windows.Win32.Devices.Bluetooth`.

## Validation

Re-scraped the `Bluetooth` partition (x64, not `ExcludeFromCrossarch`)
after the `main.cpp` touch: build succeeded, 0 warnings, 0 errors.

## Conclusion

Genuine producer-site fix applied.
