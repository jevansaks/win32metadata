# MLOperatorAuthor.h — accepted-normalized

**Partition:** MachineLearning
**Size:** ~33290 bytes

## Analysis

Modern C++-style COM header (DirectML custom-operator-authoring
interfaces: `IMLOperatorAttributes`, `IMLOperatorTensor`, etc., using
`interface DECLSPEC_UUID(...) DECLSPEC_NOVTABLE`). Confirmed via grep
that there are **no free function declarations** — all methods are
COM vtable methods, out of scope per blocker-class 5.

## Conclusion

Clean. No patch required.
