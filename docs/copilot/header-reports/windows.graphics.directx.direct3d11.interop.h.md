# Header Report: windows.graphics.directx.direct3d11.interop.h

## Partitions
`WinRT.Direct3D11`

## Scrape validation
- Re-scraped `WinRT.Direct3D11` partition (`ScanArch=x86`) after touching `main.cpp`.
- Result: `Build succeeded. 0 Warning(s). 0 Error(s).`

## Ownership audit (producer-site-only policy)
- `CreateDirect3D11DeviceFromDXGIDevice`/`CreateDirect3D11SurfaceFromDXGISurface` are clean COM factory functions (`_COM_Outptr_ IInspectable**`). `IDirect3DDxgiInterfaceAccess::GetInterface` is a COM interface method (`_COM_Outptr_`). The C++/CX inline helpers (`CreateDirect3DDevice`, `GetDXGIInterface`, etc., guarded by `__cplusplus_winrt`) are template/inline wrappers around the same COM factory pattern. No raw `HANDLE` involved.

## Conclusion
`accepted-normalized` — no ownership annotation required (COM factory pattern throughout).
