# Intentional API deltas

This journal records source-backed semantic corrections separately from
annotation transfers. Entries do not claim hosted qualification until the
published head's ordinary CI and actual output have been accepted.

## 2026-11-27: WSAGetLastError must retrieve the existing error

**Delta:** remove `SetLastError=true` from `WS2_32.dll!WSAGetLastError`.
The getter's incorrect RSP entry is retired without adding a replacement
marker in either `WinSock2.h` or `winsock.h`. This is one intentional semantic
correction, not one of this batch's 33 annotation transfers at 46 native sites.

**Reason:** [WSAGetLastError](https://learn.microsoft.com/windows/win32/api/winsock/nf-winsock-wsagetlasterror)
retrieves the calling thread's existing error. Modern .NET
[clears the native error before calls marked SetLastError=true](https://learn.microsoft.com/dotnet/api/system.runtime.interopservices.dllimportattribute.setlasterror),
erasing the value the getter must retrieve. On .NET 10.0.15/X64, seeded
errors 10022, 10035 and 10061 are returned and preserved by the false getter;
the true getter returns zero and clears the native and cached errors.
This corrects pre-existing metadata wrongness, not a new runtime regression.

**Conservation:** return type, parameters, calling convention, PreserveSig,
module, entrypoint and every other API contract are unchanged. The complete
unmasked API changes only by removing this getter's flag: SHA256
`F781562FA67BCBEDA63572343132C368260EF2A7D6A4255090055B7FEBC83821`
becomes `93793909FF04C8B475A979E66BB29CD7093DC53156D16C4FDD08754C2D544D29`.
The owner fixture preserves explicit original-legacy true, corrected-legacy
false and native false paths, compares complete corrected images, and
reconstructs every original/corrected byte with verified deterministic identity.

**Evidence:** `NetworkLastErrorHeaderTests` covers both complete native owners
on x86, x64 and ARM64. Private immutable runtime proof
`3F8B780EA0C09F32BC20A5AD4B12A56B66C696FFD201C5D1DFA64373B60976A7`
and independent technical approval
`1522FD9915119159300305FA0C5B62E9878209E8466E368C7EBF6B3F69FF2E8A`
bind the native/runtime cause, whole-image reconstruction, SDK replay and
protected contracts. [Observed problems](PROBLEMS.md) and
[checkpoint history](RECOVERY.md) retain superseded proposals and failures;
their saved-only status describes the frozen proposal, not a later publication.
