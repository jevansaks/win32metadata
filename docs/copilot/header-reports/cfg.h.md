# cfg.h — accepted-normalized

**Partition:** DevInst
**Size:** ~15380 bytes

## Analysis

`cfg.h` (`shared\cfg.h`) declares Configuration Manager `CM_PROB_*`
problem-code constants and the transparent `PNP_VETO_TYPE` enum
consumed by `CM_Disable_DevNode`/`CM_Uninstall_DevNode`/
`CM_Query_And_Remove_SubTree` (declared in the separate `cfgmgr32.h`
header). It contains **no function declarations of its own**.

## Conclusion

Clean. No functions in this header. No patch required.
