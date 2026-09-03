# Header Report: EvColl.h

## Partitions
`Wec`

## Ownership audit (producer-site-only policy) — genuine fix applied
- `EC_HANDLE` (`typedef HANDLE EC_HANDLE`, distinctly named) is produced via `EcOpenSubscriptionEnum`/`EcOpenSubscription` **returned directly as the C return value**, closed via the single canonical `EcClose(EC_HANDLE Object)` (confirmed no other close function exists for this type; `EC_HANDLE` used only in this one header). `autoTypes.json` had **no entry at all** for this type. Added a new entry: `Namespace: Windows.Win32.System.EventCollector` (matches the `Wec` partition's namespace), `ValueType: DECLARE_HANDLE`, `CloseApi: EcClose`, `InvalidHandleValues: [0]`. Because this is the type-level `autoTypes.json`/`NativeTypedefStructsCreator` mechanism (not the per-declaration inline C attribute), it correctly covers return-value production too — confirmed by the same reasoning validated for `HINSTANCE`/`HMODULE` (also return-value-produced, also carry `[RAIIFree]` in the baseline winmd). Re-scraped `Wec` partition (0 errors).
- `EC_OBJECT_ARRAY_PROPERTY_HANDLE` (`typedef HANDLE`) appears only as an `_In_` parameter and as a field inside a tagged-union-style `EC_VARIANT`-like struct populated by generic property-get functions serving many different property types — no single dedicated producer function with clear `_Out_` ownership-transfer semantics exists for this field specifically; left un-annotated (out of scope, ambiguous).

## Conclusion
`accepted-normalized` — genuine gap fixed via new `autoTypes.json` entry for `EC_HANDLE` (`CloseApi: EcClose`); `EC_OBJECT_ARRAY_PROPERTY_HANDLE` has no clear annotatable producer site.
