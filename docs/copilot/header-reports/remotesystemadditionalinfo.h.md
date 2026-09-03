# Header Report: remotesystemadditionalinfo.h

## Partitions
`TermServ`

## Ownership audit (producer-site-only policy)
- Declares only the COM interface `IRemoteSystemAdditionalInfoProvider` (single method `GetAdditionalInfo`, outputting `_Out_ HSTRING *deduplicationId` and a standard COM `QueryInterface`-style `void** mapView`). COM vtable method — out of scope for the annotation mechanism regardless of the `HSTRING` output (already correctly covered via existing `autoTypes.json` entry per the `winstring.h` audit).

## Conclusion
`accepted-normalized` — no ownership annotation required (COM interface method only, out of scope).
