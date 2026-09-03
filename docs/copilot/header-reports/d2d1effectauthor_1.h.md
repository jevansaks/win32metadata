# Header Report: d2d1effectauthor_1.h

## Partitions
`Direct2D`

## Ownership audit (producer-site-only policy)
- Declares `IID` constants and the `ID2D1EffectContext1`/`ID2D1EffectContext2` COM interfaces. `CreateLookupTable3D`/`CreateColorContextFromDxgiColorSpace`/`CreateColorContextFromSimpleColorProfile` are all clean COM factory methods (`_COM_Outptr_`). No raw `HANDLE` involved.

## Conclusion
`accepted-normalized` — no ownership annotation required (COM factory pattern).
