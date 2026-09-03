# immdev.h — accepted-normalized (producer-site fix applied)

**Partition:** Input.Ime
**Size:** ~39464 bytes

## Analysis

`immdev.h` (IME developer definitions) re-declares `HIMC` (already
correctly annotated) plus `HIMCC` (`typedef DWORD HIMCC;`, IME memory
container handle), produced by `ImmCreateIMCC(DWORD)` and destroyed by
`ImmDestroyIMCC(HIMCC)` (single-arg).

Checked `autoTypes.json` first: **`HIMCC` had an existing entry but
was MISSING `CloseApi`** (`ValueType: DECLARE_HANDLE`,
`InvalidHandleValues: [-1, 0]`, no `CloseApi`) — a genuine gap in
already-present metadata, not a new type. Added the missing
`CloseApi: ImmDestroyIMCC`. Validated via `dotnet build
generation/WinSDK -c Release -p:ScanArch=x64 -t:ScrapeHeaders
-p:PartitionFilter=Input.Ime` → 0 errors.

## Conclusion

Producer-site fix applied — filled in the missing `CloseApi` on the
pre-existing `HIMCC` `autoTypes.json` entry.
