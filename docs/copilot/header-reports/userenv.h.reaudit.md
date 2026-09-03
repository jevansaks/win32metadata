# Header Report: userenv.h (ownership re-audit)

## Partitions
`Policy`, `Shell`

## Re-audit finding (producer-site-only ownership policy)

This header was previously classified `accepted-normalized` in batch `existing-patches-34`, but that
classification only verified the **retained `set-last-error`/`supported-os` patches** replay cleanly —
it did **not** audit ownership semantics. Re-auditing now (discovered while investigating `profinfo.h`,
which declares the `PROFILEINFOA`/`PROFILEINFOW` structs used by this header):

- `LoadUserProfileW(_In_ HANDLE hToken, _Inout_ LPPROFILEINFOW lpProfileInfo)` populates the caller's
  `PROFILEINFOW.hProfile` field (a plain `HANDLE`, per the struct's own comment: *"Filled in by the
  function. Registry key handle open to the root."*) — a genuine ownership relationship exists.
- `UnloadUserProfile(_In_ HANDLE hToken, _In_ HANDLE hProfile)` is the corresponding release function.

This is the **same generic/shared-type-nested-in-struct blocker class** already documented for
`physicalmonitorenumerationapi.h` (batch `scraping-investigation-47`): the handle field's type is the
plain, generic system `HANDLE` (not a distinctly-named opaque handle type), and it is populated as a
**field inside a caller-allocated struct** (`lpProfileInfo->hProfile`) rather than via a directly
annotatable `_Out_ HANDLE*` out-parameter. Annotating a bare `HANDLE` anywhere would incorrectly apply
ownership metadata to every `HANDLE` value in the published metadata, and there is no single parameter
to attach an annotation to even if the type were distinctly named.

## Conclusion
Reclassifying `userenv.h` from `accepted-normalized` to `blocked` — genuine `HANDLE` ownership
relationship (`LoadUserProfileW`'s `lpProfileInfo->hProfile` → `UnloadUserProfile`), same root blocker
class as `physicalmonitorenumerationapi.h`/`resourceindexer.h` (generic/shared type nested in a struct
field precludes producer-site annotation). The previously-retained `set-last-error`/`supported-os`
patches remain valid and are not affected by this reclassification.
