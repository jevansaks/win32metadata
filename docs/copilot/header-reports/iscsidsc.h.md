# iscsidsc.h — accepted-normalized

**Partition:** IScsiDisc
**Size:** ~46888 bytes

## Analysis

`iscsidsc.h` (iSCSI Discovery API) declares `LoginIScsiTargetW/A`
(outputs `PISCSI_UNIQUE_SESSION_ID`/`PISCSI_UNIQUE_CONNECTION_ID`) and
`LogoutIScsiTarget(PISCSI_UNIQUE_SESSION_ID)`. `ISCSI_UNIQUE_SESSION_ID`/
`ISCSI_UNIQUE_CONNECTION_ID` are the **same real multi-field
transparent struct** (`{ ULONGLONG AdapterUnique; ULONGLONG
AdapterSpecific; }`), not an opaque pointer-sized handle — matches
the established transparent-struct-with-real-fields blocker-class 6,
out of scope for producer-site handle annotation (passed and
compared by value, not an opaque resource token).

## Conclusion

Clean. No patch required — no representable ownership gap (real
value-type struct, not an opaque handle).
