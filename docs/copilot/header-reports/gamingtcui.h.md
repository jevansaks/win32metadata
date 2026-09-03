# gamingtcui.h — accepted-normalized

**Partition:** Tcui
**Size:** ~10621 bytes

## Analysis

`gamingtcui.h` declares the Xbox/Gaming title-callable UI (TCUI)
surface: `ShowGameInviteUI`, `ShowPlayerPickerUI`, `ShowProfileCardUI`,
`ShowChangeFriendRelationshipUI`, `ShowTitleAchievementsUI`,
`ProcessPendingGameUI`, `TryCancelPendingGameUI`.

All parameters are inputs: `HSTRING` (input, already covered
generically), completion-routine function pointers (input callbacks),
and `void* context` (opaque caller-supplied token passed back to the
callback, not an owned resource). No opaque handle types are produced
or returned by any function in this header.

## Conclusion

Clean. No ownership metadata gap. No patch required.
