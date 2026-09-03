# winber.h — accepted-normalized (patch applied)

**Partition:** Ldap (`Windows.Win32.Networking.Ldap`)
**Size:** ~13598 bytes

## Analysis

`WinBer.h` (Basic Encoding Rules library) declares the `ber_*` API
family: `ber_alloc_t`/`ber_init` produce a `BerElement *` (return
value), consumed/freed via `ber_free(BerElement *pBerElement, INT
fbuf)`. All other `ber_*` functions (`ber_skip_tag`, `ber_peek_tag`,
`ber_flatten`, `ber_printf`, `ber_scanf`, etc.) take `BerElement*` as
an input parameter only.

`BerElement` is genuinely opaque — it is actually **defined** in the
sibling `Winldap.h` header (already `accepted-normalized`, same `Ldap`
partition/namespace) as:
```c
// A BerElement really maps out to a C++ class object that does BER encoding.
// Don't mess with it as it's opaque.
typedef struct berelement {
    PCHAR opaque;   // used just for compatibility with reference implementation
} BerElement;
```
Confirmed used only in `WinBer.h` and `Winldap.h`, both mapped to the
same `Windows.Win32.Networking.Ldap` namespace (no cross-namespace
conflict). No prior `autoTypes.json` entry existed. The existing
`PLDAPSearch` entry in the same namespace establishes the
`"ValueType": "typedef struct <tag>"` convention for this exact kind
of opaque-struct (non-`DECLARE_HANDLE`, non-pointer-typedef) type.

## Patch

Edited `generation/WinSDK/autoTypes.json` — added a new entry for
`BerElement` (`ValueType: "typedef struct berelement"`, matching the
sibling `PLDAPSearch` convention, `CloseApi: ber_free`) under
namespace `Windows.Win32.Networking.Ldap`.

## Validation

Re-scraped the `Ldap` partition (x64, not `ExcludeFromCrossarch`)
after the `main.cpp` touch: build succeeded, 0 warnings, 0 errors.

## Conclusion

Genuine producer-site fix applied.
