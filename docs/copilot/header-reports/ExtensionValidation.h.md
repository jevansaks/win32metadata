# Header Report: ExtensionValidation.h

## Partitions
`InternetExplorer`

## Ownership audit (producer-site-only policy)
- Declares the COM interface `IExtensionValidation : public IUnknown` (out of scope) plus `ExtensionValidationContexts`/`ExtensionValidationResults` enums. No `extern`/`DllImport` functions.

## Conclusion
`accepted-normalized` — no ownership annotation required (COM interface + enums only, no extern functions).
