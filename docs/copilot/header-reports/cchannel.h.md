# cchannel.h — accepted-normalized

**Partition:** TermServ
**Size:** ~21728 bytes

## Analysis

`cchannel.h` (`pchannel.h`, Windows Terminal Server Virtual Channel
Client API) declares only function-pointer typedefs
(`CHANNEL_INIT_EVENT_FN`, `CHANNEL_OPEN_EVENT_FN`,
`VIRTUALCHANNELINIT`, `VIRTUALCHANNELOPEN`, `VIRTUALCHANNELCLOSE`,
`VIRTUALCHANNELWRITE`, `VIRTUALCHANNELENTRY`) used as dynamically
dispatched struct fields — out of scope per the
function-pointer-typedef convention (blocker-class 4). It contains
**no `extern`/`DllImport` function declarations**.

## Conclusion

Clean. No scrapable functions, no opaque handle production. No patch
required.
