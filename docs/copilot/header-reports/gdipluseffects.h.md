# gdipluseffects.h — accepted-normalized

**Partition:** Media.DShow
**Size:** ~12175 bytes

## Analysis

`gdipluseffects.h` declares the GDI+ Effects flat C API
(`GdipCreateEffect(const GUID guid, CGpEffect **effect)`, closed via
`GdipDeleteEffect(CGpEffect *effect)`) plus C++ wrapper classes
(`Effect`, with `friend class Bitmap; friend class Graphics;` —
indicating shared/managed ownership by the Bitmap/Graphics wrapper
objects, not simple caller-owned create/destroy).

`CGpEffect` already has an `autoTypes.json` entry
(`Windows.Win32.Graphics.GdiPlus`, `ValueType: IntPtr`) with no
`CloseApi`. This is consistent with **every other opaque type in the
`Windows.Win32.GdiPlus` namespace** (`Matrix`, `Font`, `FontCollection`,
`InstalledFontCollection`, `PrivateFontCollection`, `PathData`, etc.) —
all of which have obvious dedicated `Gdip*Delete*` functions
(`GdipDeleteMatrix`, `GdipDeleteFont`, `GdipDeleteFontFamily`, etc.)
yet **none** carry a `CloseApi` entry. This is a deliberate,
system-wide policy decision for the entire GDI+ C++-wrapper opaque
type family (consistent with the C++ ownership-management pattern —
e.g. `Effect` objects can be owned/shared by `Bitmap`/`Graphics`), not
a set of individually-missed gaps. Adding `CloseApi` to `CGpEffect`
alone, while the rest of the namespace remains unannotated, would be
inconsistent with this established pattern.

## Conclusion

Clean — not modified. `CGpEffect`'s missing `CloseApi` matches a
consistent, deliberate GdiPlus-namespace-wide convention, not an
isolated gap. No patch applied.
