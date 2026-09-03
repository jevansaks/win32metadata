# d3d10_1shader.h — accepted-normalized

**Partition:** Direct3D10
**Size:** ~12192 bytes

## Analysis

`d3d10_1shader.h` declares D3D10.1 shader-reflection enums/structs
(`D3D10_SHADER_DEBUG_REGTYPE`, etc.) and COM interfaces
(`ID3D10ShaderReflection1`). It contains **no free functions**
(`STDAPI`/`WINAPI` count: 0) — all methods are COM vtable methods, out
of scope per the COM-vtable-methods convention (blocker-class 5).

## Conclusion

Clean. No patch required.
