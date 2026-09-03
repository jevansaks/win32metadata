# d3dcsx.h — accepted-normalized

**Partition:** Direct3D11
**Size:** ~15964 bytes

## Analysis

`d3dcsx.h` (D3DX11 General Purpose GPU computing) declares
`D3DX11CreateScan`, `D3DX11CreateSegmentedScan`, `D3DX11CreateFFT`
family functions. All outputs are standard COM interface pointers
(`ID3DX11Scan**`, `ID3DX11SegmentedScan**`, `ID3DX11FFT**`), out of
scope per the COM-interface-pointer convention.

## Conclusion

Clean. No opaque handle production. No patch required.
