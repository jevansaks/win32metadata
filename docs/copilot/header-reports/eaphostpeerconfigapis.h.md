# eaphostpeerconfigapis.h — accepted-normalized

**Partition:** EapHost
**Size:** ~17920 bytes

## Analysis

`EapHostPeerConfigApis.h` declares the EAP method configuration API
(`EapHostPeerGetMethods`, `EapHostPeerGetMethodProperties`,
`EapHostPeerInvokeConfigUI`, `EapHostPeerQueryCredentialInputFields`,
etc.). All outputs are `EAP_ERROR**` (transparent struct, already
established out of scope), generic `BYTE**` blob buffers, `LPWSTR*`
strings, or `IXMLDOMDocument2**` (standard COM interface pointer). No
opaque handle types are produced.

## Conclusion

Clean. No opaque handle production. No patch required.
