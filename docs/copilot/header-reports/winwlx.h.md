# winwlx.h — accepted-normalized

**Partition:** Security.WinWlx
**Size:** ~37282 bytes

## Analysis

`winwlx.h` (WinLogon eXtension / legacy GINA replacement contract)
declares the functions (`WlxNegotiate`, `WlxInitialize`,
`WlxActivateUserShell`, etc.) that a **replacement GINA DLL must
implement** — these are called by Winlogon, not exported by a system
DLL for applications to P/Invoke. `WlxInitialize`'s
`PVOID *pWlxContext` output is an opaque context fully managed
internally by the GINA implementation itself (no corresponding
`WlxClose`-style API call from Winlogon) — no representable
producer/consumer ownership pattern exists here. This is legacy/
deprecated (superseded by Credential Providers since Vista).

## Conclusion

Clean. No patch required — no representable ownership gap.
