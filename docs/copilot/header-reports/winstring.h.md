# Header Report: winstring.h

## Partitions
`WinRT`

## Ownership audit (producer-site-only policy)
This header resolves the long-deferred "HSTRING ownership tracked separately under winstring.h" item (many prior headers — `roregistrationapi.h`, `rometadataresolution.h`, etc. — output `HSTRING`/`HSTRING**` and deferred to this header rather than needing their own fix).

- `WindowsCreateString`/`WindowsCreateStringReference`/`WindowsDuplicateString`/`WindowsSubstring`/`WindowsSubstringWithSpecifiedLength`/`WindowsConcatString`/`WindowsReplaceString`/`WindowsTrimStringStart`/`WindowsTrimStringEnd` all produce `HSTRING` via `_Outptr_` out-params, closed via `WindowsDeleteString(_In_opt_ HSTRING string)`. `autoTypes.json` already has a complete entry (`Name: HSTRING`, `ValueType: DECLARE_HANDLE`, `CloseApi: WindowsDeleteString`) — this is a single-purpose opaque handle type correctly covered at the type level (same class as `HPOWERNOTIFY`/`FH_SERVICE_PIPE_HANDLE`), which is why every HSTRING-producing header across the whole SDK is already correctly closed without a header-specific annotation.
- `WindowsPreallocateStringBuffer`/`WindowsPromoteStringBuffer`/`WindowsDeleteStringBuffer` use the separate `HSTRING_BUFFER` two-phase-construction handle — `autoTypes.json` also has a complete entry for this (`CloseApi: WindowsDeleteStringBuffer`).
- `HSTRING_UserSize`/`UserMarshal`/`UserUnmarshal`/`UserFree` are standard RPC marshaling plumbing functions (fixed signature required by the RPC runtime), not ownership-relevant producer/consumer pairs.
- `WindowsInspectString`/`WindowsInspectString2` take a raw `UINT_PTR`/`UINT64` address (not an `HSTRING` handle) for debug inspection — no ownership transfer.

## Conclusion
`accepted-normalized` — no ownership annotation required (`HSTRING`/`HSTRING_BUFFER` already correctly covered via existing `autoTypes.json` entries; this closes out the long-deferred HSTRING investigation for the whole SDK).
