# webservices.h

**Classification:** accepted-normalized (producer-site fixes applied)

## Summary
Windows Web Services (WWS) API core header. Confirmed 11 genuine opaque
forward-declared struct-pointer producer/consumer handle pairs
(`typedef struct _WS_X WS_X;`, no body ever defined anywhere in the
header — the same "opaque single-pointer" pattern already established
for `GLUquadric`/`GLUtesselator`/`GLUnurbs`):

| Typedef             | Producer (out-param, `_Outptr_`)                          | Consumer (single-arg free) |
|----------------------|-------------------------------------------------------------|------------------------------|
| `WS_CHANNEL`         | `WsCreateChannel`/`WsCreateChannelForListener`               | `WsFreeChannel`   |
| `WS_ERROR`           | `WsCreateError`                                              | `WsFreeError`      |
| `WS_HEAP`            | `WsCreateHeap`                                               | `WsFreeHeap`       |
| `WS_LISTENER`        | `WsCreateListener`                                           | `WsFreeListener`   |
| `WS_MESSAGE`         | `WsCreateMessage`/`WsCreateMessageForChannel`                | `WsFreeMessage`    |
| `WS_METADATA`        | `WsCreateMetadata`                                           | `WsFreeMetadata`   |
| `WS_SECURITY_TOKEN`  | `WsCreateXmlSecurityToken`                                   | `WsFreeSecurityToken` |
| `WS_SERVICE_HOST`    | `WsCreateServiceHost`                                        | `WsFreeServiceHost` |
| `WS_SERVICE_PROXY`   | `WsCreateServiceProxy`/`WsCreateServiceProxyFromTemplate`     | `WsFreeServiceProxy` |
| `WS_XML_READER`      | `WsCreateReader`                                             | `WsFreeReader`     |
| `WS_XML_WRITER`      | `WsCreateWriter`                                             | `WsFreeWriter`     |

`autoTypes.json` already contained `ValueType`/`NativeTypedef` entries
for all 11 types (declaring the opaque-struct shape, namespace
`Windows.Win32.Networking.WindowsWebServices`) but no ownership fields —
added in this batch. Each type is exclusive to the WWS API surface with
a clear producer/consumer relationship, consistent with the corrected
producer-site policy (not a shared/generic foundational type).

Four related types were investigated but **not** annotated because no
paired `WsFree*` function exists in this header: `WS_XML_BUFFER` (owned
by its originating `WS_HEAP`, freed implicitly when the heap is freed),
`WS_OPERATION_CONTEXT`, `WS_POLICY`, `WS_SECURITY_CONTEXT` (transient
references obtained during a service call, not separately
created/destroyed by the caller).

## Ownership Analysis
Updated 11 existing `autoTypes.json` entries to add `CloseApi`/
`InvalidHandleValues: [0]` (listed above).

## Validation
`ScrapeHeaders x64 Wsw` → Build succeeded, 0 Error(s).

## Action
Producer-site fix applied via `autoTypes.json` (11 entries updated).
