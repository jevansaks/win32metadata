# Header Report: websocket.h

## Partitions
`WebSock`

## Ownership audit (producer-site-only policy)
- `WEB_SOCKET_HANDLE` (`DECLARE_HANDLE`) is produced via `WebSocketCreateClientHandle`/`WebSocketCreateServerHandle`, closed via `WebSocketDeleteHandle`/`WebSocketAbortHandle`. `autoTypes.json` already has a complete entry (`CloseApi: WebSocketDeleteHandle`) — already correctly covered at the type level.
- Remaining functions (`WebSocketSend`/`WebSocketReceive`/`WebSocketGetAction`/etc.) take the handle as `_In_` input or output transparent structs/buffers (`WEB_SOCKET_BUFFER`/`WEB_SOCKET_HTTP_HEADER`), out of scope.

## Conclusion
`accepted-normalized` — no ownership annotation required (`WEB_SOCKET_HANDLE` already correctly covered via existing `autoTypes.json` entry).
