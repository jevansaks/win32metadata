# d3d10shader.h — accepted-normalized

**Partition:** Direct3D10
**Size:** ~26465 bytes

## Analysis

`D3D10Shader.h` declares `D3D10CompileShader`, `D3D10DisassembleShader`,
`D3D10GetPixelShaderProfile`/`VertexShaderProfile`/
`GeometryShaderProfile` (return `LPCSTR` — static/interned strings,
no ownership transfer), `D3D10ReflectShader`,
`D3D10PreprocessShader`, `D3D10GetInputSignatureBlob`/
`OutputSignatureBlob`/`InputAndOutputSignatureBlob`,
`D3D10GetShaderDebugInfo`. All outputs are standard COM interface
pointers (`ID3D10Blob**`, `ID3D10ShaderReflection**`), out of scope
per the COM-interface-pointer convention.

## Conclusion

Clean. No opaque handle production. No patch required.
