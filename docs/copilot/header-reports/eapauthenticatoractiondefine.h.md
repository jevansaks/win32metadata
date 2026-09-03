# Header Report: eapauthenticatoractiondefine.h

## Partitions
`EapHost`

## Ownership audit (producer-site-only policy)
- Declares only enums (`_EAP_METHOD_AUTHENTICATOR_RESPONSE_ACTION`, `tagEapPeerMethodResponseAction`, `tagEapPeerMethodResultReason`) and data structs (`_EAP_METHOD_AUTHENTICATOR_RESULT`, `tagEapPeerMethodOuput`). No functions, no handles.

## Conclusion
`accepted-normalized` — no ownership annotation required (enums/data structs only, no functions).
