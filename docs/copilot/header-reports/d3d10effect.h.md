# d3d10effect.h — accepted-normalized

**Partition:** Direct3D10
**Size:** ~73607 bytes

## Analysis

`D3D10Effect.h` declares stateblock/effect flat APIs
(`D3D10CreateStateBlock`, `D3D10CreateEffectFromMemory`, etc.) that
output COM interface pointers (`ID3D10StateBlock**`, `ID3D10Effect**`)
or real transparent structs (`D3D10_STATE_BLOCK_MASK`) by value/
pointer — no raw owned-handle production. Out of scope per
blocker-class 5 (COM) / not a handle at all (transparent struct).

## Conclusion

Clean. No patch required.
