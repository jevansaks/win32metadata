# Header Report: rometadataresolution.h

## Partitions
`WinRT.Metadata`

## Ownership audit (producer-site-only policy)
- `RoGetMetaDataFile`/`RoParseTypeName`/`RoResolveNamespace` output `HSTRING`/`HSTRING**` values — `HSTRING`'s ownership mechanism (`WindowsCreateString`/`WindowsDeleteString`) is tracked separately under `winstring.h` (still pending), same reasoning as `roregistrationapi.h` (batch `scraping-investigation-53`). `IMetaDataImport2**`/`__PropertySet__**`/`__IPropertySetSerializer__**` outputs are standard COM factory patterns (`_Outptr_`/`_COM_Outptr_`). No new `HANDLE`-family gap.

## Conclusion
`accepted-normalized` — COM factory pattern + `HSTRING` outputs whose ownership is tracked separately under `winstring.h`.
