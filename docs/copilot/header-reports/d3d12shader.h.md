# d3d12shader.h — accepted-normalized

**Partition:** Direct3D12
**Size:** ~28043 bytes

## Analysis

`D3D12Shader.h` declares `D3D12_SHADER_*` enums/structs and COM
interfaces (`ID3D12ShaderReflection`, `ID3D12ShaderReflectionType`,
etc.). Some vtable methods return raw interface pointers (e.g.
`GetMemberTypeByIndex` returns `ID3D12ShaderReflectionType*`), but
these remain **COM vtable methods**, out of scope for producer-site
ownership annotation regardless of what they return, per
blocker-class 5.

## Conclusion

Clean. No patch required.
