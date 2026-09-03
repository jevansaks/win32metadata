# Ralph Loop SDK Header Queue

- Generated: 2026-09-03T08:52:03Z
- Source: `generation/WinSDK/patches/header-progress.json` (authoritative, one row per unique header)
- Total headers: 1403
- Matched: 1011
- In progress: 0
- Blocked: 31
- Remaining: 361

| Header | Partition(s) | Status | Owner | Last Updated | Notes |
|---|---|---|---|---|---|
| `accctrl.h` | Authorization, Authorization.UI, Security, TransactionServer | matched |  |  | Investigated; clean, no ownership metadata gap. See docs/copilot/header-reports/accctrl.h.md |
| `accountssettingspaneinterop.h` | WinRT | matched |  | 09/02/2026 22:02:59 | COM/WinRT interop interface method only, out of scope. |
| `aclapi.h` | Authorization, Authorization.UI | matched |  |  | Investigated; clean, no ownership metadata gap. See docs/copilot/header-reports/aclapi.h.md |
| `aclui.h` | Authorization, Authorization.UI | matched |  |  | Investigated; clean, no ownership metadata gap. See docs/copilot/header-reports/aclui.h.md |
| `activation.h` | WinRT | matched |  | 09/02/2026 20:19:39 | COM/WinRT interface method only, out of scope. |
| `activaut.h` | Debug.ActiveScript | remaining |  |  |  |
| `activdbg.h` | Debug.ActiveScript | remaining |  |  |  |
| `activdbg100.h` | Debug.ActiveScript | remaining |  |  |  |
| `activecf.h` | Media.DShow | matched |  | 09/02/2026 17:09:31 | Clipboard-format data struct only, no functions. |
| `activprof.h` | Debug.ActiveScript | remaining |  |  |  |
| `adhoc.h` | Ndis, NWifi | remaining |  |  |  |
| `adshlp.h` | ActiveDirectory | matched |  | 09/02/2026 19:21:35 | COM refcounting + generic memory/string-allocation conventions only. |
| `adsprop.h` | ActiveDirectory | blocked |  | 09/02/2026 21:49:17 | ADsPropCreateNotifyObj produces generic HWND via direct out-param; extends established blocker class to HWND. |
| `adssts.h` | ActiveDirectory | matched |  | 09/02/2026 19:19:02 | Status-code constants only, no functions. |
| `adtgen.h` | Authorization, Authorization.UI | matched |  |  | Investigated; clean, no ownership metadata gap. See docs/copilot/header-reports/adtgen.h.md |
| `advpub.h` | FileHistory, WinProg | remaining |  |  |  |
| `af_irda.h` | WinSock | matched |  | 09/02/2026 20:41:40 | Constants only, no functions. |
| `afunix.h` | WinSock | matched |  | 09/02/2026 17:09:31 | Data struct + IOCTL constants only, no functions. |
| `alljoyn_c\AboutData.h` | AllJoyn | remaining |  |  |  |
| `alljoyn_c\AboutIconObj.h` | AllJoyn | remaining |  |  |  |
| `alljoyn_c\AboutIconProxy.h` | AllJoyn | remaining |  |  |  |
| `alljoyn_c\AboutObj.h` | AllJoyn | remaining |  |  |  |
| `alljoyn_c\AboutObjectDescription.h` | AllJoyn | remaining |  |  |  |
| `alljoyn_c\AboutProxy.h` | AllJoyn | remaining |  |  |  |
| `alljoyn_c\AjAPI.h` | AllJoyn, WinRT.AllJoyn | remaining |  |  |  |
| `alljoyn_c\ApplicationStateListener.h` | AllJoyn | remaining |  |  |  |
| `alljoyn_c\AuthListener.h` | AllJoyn | remaining |  |  |  |
| `alljoyn_c\AutoPinger.h` | AllJoyn | remaining |  |  |  |
| `alljoyn_c\BusAttachment.h` | AllJoyn | remaining |  |  |  |
| `alljoyn_c\BusListener.h` | AllJoyn | remaining |  |  |  |
| `alljoyn_c\BusObject.h` | AllJoyn | remaining |  |  |  |
| `alljoyn_c\DBusStdDefines.h` | AllJoyn | remaining |  |  |  |
| `alljoyn_c\Init.h` | AllJoyn | remaining |  |  |  |
| `alljoyn_c\InterfaceDescription.h` | AllJoyn | remaining |  |  |  |
| `alljoyn_c\KeyStoreListener.h` | AllJoyn | remaining |  |  |  |
| `alljoyn_c\Message.h` | AllJoyn | remaining |  |  |  |
| `alljoyn_c\MsgArg.h` | AllJoyn | remaining |  |  |  |
| `alljoyn_c\Observer.h` | AllJoyn | remaining |  |  |  |
| `alljoyn_c\PasswordManager.h` | AllJoyn | remaining |  |  |  |
| `alljoyn_c\PermissionConfigurationListener.h` | AllJoyn | remaining |  |  |  |
| `alljoyn_c\PermissionConfigurator.h` | AllJoyn | remaining |  |  |  |
| `alljoyn_c\ProxyBusObject.h` | AllJoyn | remaining |  |  |  |
| `alljoyn_c\SecurityApplicationProxy.h` | AllJoyn | remaining |  |  |  |
| `alljoyn_c\Session.h` | AllJoyn | remaining |  |  |  |
| `alljoyn_c\SessionListener.h` | AllJoyn | remaining |  |  |  |
| `alljoyn_c\version.h` | AllJoyn | remaining |  |  |  |
| `amaudio.h` | Media.DShow | matched |  | 09/02/2026 18:40:30 | COM interface-pointer refcounting pattern, not HANDLE-family. |
| `amparse.h` | Media.DShow | matched |  | 09/02/2026 17:27:04 | COM interface with plain integer output, no handle. |
| `amsi.h` | Antimalware | matched | copilot | 09/03/2026 03:35:00 | New resource-ownership patch created and verified via live re-scrape (build-level validation). |
| `amstream.h` | Media.DShow, Media.DShow.Xml | remaining |  |  |  |
| `amva.h` | Media.DShow | matched |  | 09/02/2026 19:28:18 | Constants + data structs only, no functions. |
| `amvideo.h` | Media.DShow | matched |  |  | Investigated; clean, no ownership metadata gap. See docs/copilot/header-reports/amvideo.h.md |
| `amxmlgraphbuilder.h` | Media.DShow, Media.DShow.Xml | matched |  | 09/02/2026 17:13:02 | COM interface + GUID constants only. |
| `anchorsyncdeviceservice.h` | WpdSdk | matched |  | 09/02/2026 22:06:03 | Constants only, no functions. |
| `animationcoordinator.h` | Shell | matched |  | 09/02/2026 20:49:21 | COM interface method only, out of scope. |
| `apdevpkey.h` | Base, Devices.Properties | matched |  | 09/02/2026 17:05:19 | Device property key constant only, no functions. |
| `apiquery2.h` | FileHistory, WinProg | matched |  | 09/02/2026 17:41:54 | Caller-allocated string buffer output only. |
| `appcompatapi.h` | FileHistory, WinProg | matched |  | 09/02/2026 17:23:42 | No handle involved. |
| `appmgmt.h` | Policy, Shell | matched | copilot | 09/02/2026 19:25:46 | Classified retained artifact in existing-patches-01. |
| `appmodel.h` | AppxPackaging | remaining |  |  |  |
| `appnotify.h` | Shell | matched |  | 09/02/2026 18:20:58 | Genuine ownership gap fixed: RegisterAppStateChangeNotification/RegisterAppConstrainedChangeNotification out-params now carry _Win32_metadata_invalid_handle_/_raii_free_, releasing via UnregisterAppStateChangeNotification/UnregisterAppConstrainedChangeNotification. Verified via live re-scrape (0 errors) and git apply --check --reverse. |
| `appserviceinterop.h` | WinRT | matched |  | 09/02/2026 22:02:59 | COM/WinRT interface methods only, out of scope. |
| `appxpackaging.h` | AppxPackaging | remaining |  |  |  |
| `asferr.h` | Multimedia | matched |  |  | Investigated; clean, no ownership metadata gap. See docs/copilot/header-reports/asferr.h.md |
| `atacct.h` | NetMgmt | matched |  | 09/02/2026 17:09:31 | String-only account-info API, no handle. |
| `atscpsipparser.h` | MsTv | remaining |  |  |  |
| `atsmedia.h` | MsTv | matched |  | 09/02/2026 17:27:04 | GUID constant only, no functions. |
| `audevcod.h` | Media.DShow | matched |  | 09/02/2026 17:48:13 | Error-code enum/constants only, no functions. |
| `AudioAPOTypes.h` | Audio.Apo, TermServ | matched |  | 09/02/2026 19:32:00 | Typedefs/enum/structs only, no functions. |
| `audioclient.h` | Audio | remaining |  |  |  |
| `audioclientactivationparams.h` | Audio | matched |  | 09/02/2026 19:06:16 | Enums/data structs + MIDL boilerplate only, no functions. |
| `audioendpoints.h` | Audio.Endpoints | matched |  | 09/02/2026 19:47:59 | COM interface method only, no extern functions. |
| `audioenginebaseapo.h` | Audio.Apo | remaining |  |  |  |
| `audioengineendpoint.h` | Audio.Endpoints, TermServ | remaining |  |  |  |
| `audioengineextensionapo.h` | Audio.Apo | matched |  |  | Investigated; clean, no ownership metadata gap. See docs/copilot/header-reports/audioengineextensionapo.h.md |
| `audiomediatype.h` | Audio.Apo | matched |  | 09/02/2026 20:53:11 | COM interface + standard COM factory functions only. |
| `audiopolicy.h` | Audio | remaining |  |  |  |
| `audiosessiontypes.h` | Audio | matched |  | 09/02/2026 21:12:30 | Enums only, no functions. |
| `audiostatemonitorapi.h` | Audio | matched |  | 09/02/2026 19:28:18 | Handle-producing method is a COM vtable method, out of scope; extern functions are COM factories. |
| `austream.h` | Media.DShow | matched |  |  | Investigated; clean, no ownership metadata gap. See docs/copilot/header-reports/austream.h.md |
| `authif.h` | Nps | matched |  |  | Investigated; clean, no ownership metadata gap. See docs/copilot/header-reports/authif.h.md |
| `authz.h` | Authorization, Authorization.UI | matched | copilot | 09/02/2026 21:45:00 | Corrected typedef-owned ownership violation found during resource-ownership audit (12 producer sites across 6 handle types). |
| `aux_ulib.h` | FileHistory, WinProg | matched | copilot | 09/02/2026 19:25:46 | Classified retained artifact in existing-patches-01. |
| `avifmt.h` | Media.DShow | matched |  | 09/02/2026 21:55:32 | Constants/structs only, no functions. |
| `aviriff.h` | Media.DShow | matched |  |  | Investigated; clean, no ownership metadata gap. See docs/copilot/header-reports/aviriff.h.md |
| `avrfsdk.h` | ApplicationVerifier, FileHistory | matched |  | 09/02/2026 19:02:46 | HANDLE is a caller-supplied input; diagnostic handle fields are plain ULONG64. |
| `avrt.h` | Threading | blocked |  | 09/02/2026 19:32:00 | Return-value HANDLE (AvSetMmThreadCharacteristics*) + generic PHANDLE direct out-param (AvRtCreateThreadOrderingGroup*). |
| `azroles.h` | Authorization, Authorization.UI | remaining |  |  |  |
| `batclass.h` | Power | matched |  |  | Investigated; clean, no ownership metadata gap. See docs/copilot/header-reports/batclass.h.md |
| `bcp47mrm.h` | Intl | matched |  | 09/02/2026 16:53:36 | No handle-producing functions. |
| `bcrypt_provider.h` | Security.Cryptography | matched |  |  | Investigated; clean, no ownership metadata gap. See docs/copilot/header-reports/bcrypt_provider.h.md |
| `bcrypt.h` | Security.Cryptography, Security.Cryptography.UI | matched | copilot | 09/02/2026 21:45:00 | Corrected typedef-owned ownership violation found during resource-ownership audit; unrelated zz-crypto-security-enums patch verified compatible via full sequential forward replay. |
| `bdaiface.h` | Media.DShow, MsTv | remaining |  |  |  |
| `bdamedia.h` | MsTv | remaining |  |  |  |
| `bdatif.h` | MsTv | remaining |  |  |  |
| `bdatypes.h` | Media.DShow, MsTv | remaining |  |  |  |
| `BiDiSpl.h` | Printing | matched |  |  | Investigated; clean, no ownership metadata gap. See docs/copilot/header-reports/BiDiSpl.h.md |
| `bindlink.h` | Fs | matched |  | 09/02/2026 17:30:54 | Path-string-only API, no handle. |
| `bits.h` | Bits | remaining |  |  |  |
| `bits1_5.h` | Bits | matched |  |  | Investigated; clean, no ownership metadata gap. See docs/copilot/header-reports/bits1_5.h.md |
| `bits10_1.h` | Bits | matched |  |  | Investigated; clean, no ownership metadata gap. See docs/copilot/header-reports/bits10_1.h.md |
| `bits10_2.h` | Bits | matched |  |  | Investigated; clean, no ownership metadata gap. See docs/copilot/header-reports/bits10_2.h.md |
| `bits10_3.h` | Bits | matched |  |  | Investigated; clean, no ownership metadata gap. See docs/copilot/header-reports/bits10_3.h.md |
| `bits2_0.h` | Bits | matched |  |  | Investigated; clean, no ownership metadata gap. See docs/copilot/header-reports/bits2_0.h.md |
| `bits2_5.h` | Bits | matched |  |  | Investigated; clean, no ownership metadata gap. See docs/copilot/header-reports/bits2_5.h.md |
| `bits3_0.h` | Bits | remaining |  |  |  |
| `bits4_0.h` | Bits | matched |  |  | Investigated; clean, no ownership metadata gap. See docs/copilot/header-reports/bits4_0.h.md |
| `bits5_0.h` | Bits | remaining |  |  |  |
| `bitscfg.h` | Bits | matched |  |  | Investigated; clean, no ownership metadata gap. See docs/copilot/header-reports/bitscfg.h.md |
| `bitsmsg.h` | Bits | remaining |  |  |  |
| `bluetoothapis.h` | Bluetooth | matched | copilot | 09/02/2026 23:05:00 | Classified retained artifact in existing-patches-02. |
| `bluetoothleapis.h` | Bluetooth | matched |  |  | Genuine producer-site fix: added BLUETOOTH_GATT_EVENT_HANDLE autoTypes.json entry (CloseApi=BluetoothGATTUnregisterEvent). See docs/copilot/header-reports/bluetoothleapis.h.md |
| `bridgedeviceservice.h` | WpdSdk | remaining |  |  |  |
| `bthdef.h` | Bluetooth | remaining |  |  |  |
| `bthledef.h` | Bluetooth | matched |  |  | Investigated; clean, no ownership metadata gap. See docs/copilot/header-reports/bthledef.h.md |
| `bthsdpdef.h` | Bluetooth | matched |  | 09/02/2026 18:38:01 | Data structs/enums/union only, no functions. |
| `bugcodes.h` | Debug | remaining |  |  |  |
| `calendarapis.h` | Intl | matched |  | 09/02/2026 17:30:54 | Calendar-date-struct-only API, no handle. |
| `calendardeviceservice.h` | WpdSdk | matched |  | 09/02/2026 21:17:15 | Constants only, no functions. |
| `callobj.h` | Com.CallObj, ComOle | matched | copilot | 09/02/2026 19:25:46 | Classified retained artifact in existing-patches-01. |
| `camerauicontrol.h` | FileHistory, Media.DShow, Mf, WinProg | matched |  |  | Investigated; clean, no ownership metadata gap. See docs/copilot/header-reports/camerauicontrol.h.md |
| `capi.h` | FileHistory, WinProg | matched |  | 09/02/2026 17:45:25 | JAVA_TRUST plain data struct; all redirected headers already accepted or separately tracked pending; sipbase.h verified as trivial redirect to accepted mssip.h. |
| `cardmod.h` | Security.Cryptography | remaining |  |  |  |
| `casetup.h` | Security.Cryptography, Security.Cryptography.UI | remaining |  |  |  |
| `CastingInterop.h` | WinRT | matched |  |  | Investigated; clean, no ownership metadata gap. See docs/copilot/header-reports/CastingInterop.h.md |
| `ccgplugins.h` | Identity | matched |  | 09/02/2026 19:37:21 | COM interface method only, out of scope. |
| `cchannel.h` | TermServ | matched |  |  | Investigated; clean, no ownership metadata gap. See docs/copilot/header-reports/cchannel.h.md |
| `cderr.h` | DlgBox | matched |  | 09/02/2026 18:15:12 | Error-code constants only, no functions. |
| `celib.h` | Security | matched |  |  | Investigated; clean, no ownership metadata gap. See docs/copilot/header-reports/celib.h.md |
| `certadm.h` | Certificates | remaining |  |  |  |
| `certbcli.h` | Certificates | matched |  |  | Investigated; clean, no ownership metadata gap. See docs/copilot/header-reports/certbcli.h.md |
| `certcli.h` | Certificates | remaining |  |  |  |
| `certenc.h` | Certificates | matched | copilot | 09/02/2026 19:25:46 | Classified retained artifact in existing-patches-01. |
| `certenroll.h` | Certificates | remaining |  |  |  |
| `certexit.h` | Certificates | matched |  |  | Investigated; clean, no ownership metadata gap. See docs/copilot/header-reports/certexit.h.md |
| `certif.h` | Certificates | matched |  |  | Investigated; clean, no ownership metadata gap. See docs/copilot/header-reports/certif.h.md |
| `certmod.h` | Certificates | matched |  |  | Investigated; clean, no ownership metadata gap. See docs/copilot/header-reports/certmod.h.md |
| `certpol.h` | Certificates | matched |  |  | Investigated; clean, no ownership metadata gap. See docs/copilot/header-reports/certpol.h.md |
| `certpoleng.h` | Certificates | matched |  | 09/02/2026 19:09:17 | Transparent structs with generic free conventions, or generic PVOID; both out of scope. |
| `certreqd.h` | Certificates | matched |  |  | Investigated; clean, no ownership metadata gap. See docs/copilot/header-reports/certreqd.h.md |
| `certsrv.h` | Certificates | remaining |  |  |  |
| `certview.h` | Certificates | remaining |  |  |  |
| `cfapi.h` | Cloudapi | remaining |  |  |  |
| `cfg.h` | DevInst | matched |  |  | Investigated; clean, no ownership metadata gap. See docs/copilot/header-reports/cfg.h.md |
| `cfgmgr32.h` | DevInst | matched | copilot | 09/02/2026 19:25:46 | Classified retained artifact in existing-patches-01. |
| `cierror.h` | Foundation | matched |  |  | Investigated; clean, no ownership metadata gap. See docs/copilot/header-reports/cierror.h.md |
| `clfs.h` | Fs | remaining |  |  |  |
| `clfsmgmt.h` | Fs | matched |  |  | Genuine producer-site fix: added CLFS_MGMT_CLIENT autoTypes.json entry (CloseApi=ClfsMgmtDeregisterManagedClient). See docs/copilot/header-reports/clfsmgmt.h.md |
| `clfsmgmtw32.h` | Fs | matched | copilot | 09/02/2026 23:05:00 | Classified retained artifact in existing-patches-02. |
| `clfsw32.h` | Fs | matched | copilot | 09/02/2026 23:05:00 | Classified retained artifact in existing-patches-02. |
| `cloneviewhelper.h` | Monitor | matched |  |  | Investigated; clean, no ownership metadata gap. See docs/copilot/header-reports/cloneviewhelper.h.md |
| `cluadmex.h` | MsCs | remaining |  |  |  |
| `cmnquery.h` | ActiveDirectory | matched |  | 09/02/2026 21:43:41 | GUID/flag constants only, no extern functions. |
| `codecapi.h` | Media.DShow, Mf | remaining |  |  |  |
| `colordlg.h` | DlgBox | matched |  | 09/02/2026 17:35:31 | Dialog control ID constants only, no functions. |
| `comadmin.h` | Cos | remaining |  |  |  |
| `combaseapi.h` | Base, Com.Events, Com.StructuredStorage, Cos, IO, Multimedia, Shell, TransactionServer, VSS | matched | copilot | 09/02/2026 23:05:00 | Classified retained artifact in existing-patches-02. |
| `comcat.h` | Com, ComOle | matched | copilot | 09/02/2026 23:05:00 | Classified retained artifact in existing-patches-02. |
| `coml2api.h` | Com.StructuredStorage | matched | copilot | 09/02/2026 23:30:00 | Classified retained artifact in existing-patches-03. |
| `commctrl.h` | Controls, Policy, Shell | matched | copilot | 09/02/2026 23:30:00 | Classified retained artifact in existing-patches-03. |
| `commdlg.h` | DlgBox, Media, Media.KernelStreaming, Multimedia | matched | copilot | 09/02/2026 23:30:00 | Classified retained artifact in existing-patches-03. |
| `commoncontrols.h` | Controls | matched | copilot | 09/02/2026 23:30:00 | Classified retained artifact in existing-patches-03. |
| `compressapi.h` | CmpApi | matched | copilot | 09/02/2026 23:30:00 | Classified retained artifact in existing-patches-03. |
| `compstui.h` | Printing | remaining |  |  |  |
| `computecore.h` | HostComputeSystem | matched |  |  | Genuine producer-site fix: added 3 autoTypes.json entries (HCS_SYSTEM/HCS_PROCESS/HCS_OPERATION). HCS_CALLBACK left unannotated (unused). See docs/copilot/header-reports/computecore.h.md |
| `computenetwork.h` | HostComputeNetwork | matched |  |  | Genuine producer-site fix: added 6 autoTypes.json entries (HCN_NETWORK/NAMESPACE/ENDPOINT/LOADBALANCER/GUESTNETWORKSERVICE/CALLBACK). HCN_SERVICE left unannotated (unused). See docs/copilot/header-reports/computenetwork.h.md |
| `computestorage.h` | HostComputeSystem | matched |  | 09/02/2026 19:42:09 | No opaque handles produced; HANDLE params are pre-owned caller inputs. |
| `comsvcs.h` | Com.Events, Cos | matched | copilot | 09/02/2026 23:50:00 | Classified retained artifact in existing-patches-04. |
| `consoleapis.h` | Console | matched |  | 09/02/2026 17:23:42 | HANDLE/HWND fields are caller-supplied input references, not produced/owned here; distinct from the wincon.h blocker (different functions). |
| `contactaggregation.h` | WinContacts | remaining |  |  |  |
| `contactdeviceservice.h` | WpdSdk | matched |  |  | Investigated; clean, no ownership metadata gap. See docs/copilot/header-reports/contactdeviceservice.h.md |
| `contentpartner.h` | Wmp | remaining |  |  |  |
| `control.h` | Media.DShow | matched | copilot | 09/02/2026 23:50:00 | Classified retained artifact in existing-patches-04. |
| `cor.h` | ClrProfiling | remaining |  |  |  |
| `corecrt_io.h` | Multimedia | matched |  |  | Investigated; clean, no ownership metadata gap. See docs/copilot/header-reports/corecrt_io.h.md |
| `CoreWindow.h` | WinRT | matched |  |  | Investigated; clean, no ownership metadata gap. See docs/copilot/header-reports/CoreWindow.h.md |
| `CorHdr.h` | ClrProfiling | remaining |  |  |  |
| `corprof.h` | ClrProfiling | remaining |  |  |  |
| `cpl.h` | Shell | matched |  | 09/02/2026 21:38:18 | Function-pointer typedef + transparent structs only, no extern functions. |
| `cplext.h` | Shell | matched |  | 09/02/2026 18:47:23 | Property-page ID constants only, no functions. |
| `credentialprovider.h` | Shell | matched | copilot | 09/02/2026 23:50:00 | Classified retained artifact in existing-patches-04. |
| `credssp.h` | Credentials | matched |  | 09/02/2026 18:38:01 | Structs/enum/constants only; one function returns a non-owned static function table. |
| `cryptcat.h` | Security.Cryptography.Catalog | matched |  | 09/02/2026 17:16:10 | Consumer-only functions (enumerate existing CRYPTCATCDF); producer pair lives in mscat.h (separately tracked, pending). |
| `cryptdlg.h` | Security.Cryptography, Security.Cryptography.UI | matched |  |  | Investigated; clean, no ownership metadata gap. See docs/copilot/header-reports/cryptdlg.h.md |
| `cryptuiapi.h` | Security.Cryptography, Security.Cryptography.UI | matched | copilot | 09/02/2026 23:50:00 | Classified retained artifact in existing-patches-04. |
| `cryptxml.h` | Security.Cryptography, Security.Cryptography.UI | matched | copilot | 09/02/2026 23:50:00 | Classified retained artifact in existing-patches-04. |
| `cscapi.h` | Of | matched |  | 09/02/2026 18:20:58 | BOOL-output-only API, no handle. |
| `cscobj.h` | Of | remaining |  |  |  |
| `cspdk.h` | Security.Cryptography | matched |  |  | Investigated; clean, no ownership metadata gap. See docs/copilot/header-reports/cspdk.h.md |
| `ctffunc.h` | Tsf | remaining |  |  |  |
| `ctfspui.h` | Tsf | matched |  | 09/02/2026 20:41:40 | COM interface method + GUID constants only, no extern functions. |
| `ctfutb.h` | Tsf | remaining |  |  |  |
| `ctxtcall.h` | Com, ComOle | matched | copilot | 09/03/2026 00:10:00 | Classified retained artifact in existing-patches-05. |
| `custcntl.h` | Controls | matched |  | 09/02/2026 21:49:17 | Constants/transparent structs only, no extern functions. |
| `d2d1_1.h` | Direct2D | matched | copilot | 09/03/2026 00:10:00 | Classified retained artifact in existing-patches-05. |
| `d2d1_2.h` | Direct2D | matched | copilot | 09/03/2026 00:10:00 | Classified retained artifact in existing-patches-05. |
| `d2d1_3.h` | Direct2D | matched | copilot | 09/03/2026 00:10:00 | Classified retained artifact in existing-patches-05. |
| `d2d1.h` | Direct2D | matched | copilot | 09/03/2026 00:10:00 | Classified retained artifact in existing-patches-05. |
| `d2d1effectauthor_1.h` | Direct2D | matched |  | 09/02/2026 18:56:34 | COM factory pattern. |
| `d2d1effectauthor.h` | Direct2D | matched | copilot | 09/03/2026 00:25:00 | Classified retained artifact in existing-patches-06. |
| `d2d1effects_1.h` | Direct2D | matched |  | 09/02/2026 18:40:30 | GUID constant + enums only, no functions. |
| `d2d1effects_2.h` | Direct2D | matched |  |  | Investigated; clean, no ownership metadata gap. See docs/copilot/header-reports/d2d1effects_2.h.md |
| `d2d1effects.h` | Direct2D | remaining |  |  |  |
| `d2d1svg.h` | Direct2D | remaining |  |  |  |
| `d2dbasetypes.h` | Direct2D | matched |  | 09/02/2026 17:05:19 | Type alias only, no functions. |
| `d2derr.h` | Direct2D | matched |  | 09/02/2026 17:48:13 | HRESULT error-code macros only, no functions. |
| `d3d10_1.h` | Direct3D10 | remaining |  |  |  |
| `d3d10_1shader.h` | Direct3D10 | matched |  |  | Investigated; clean, no ownership metadata gap. See docs/copilot/header-reports/d3d10_1shader.h.md |
| `d3d10.h` | Direct3D10 | remaining |  |  |  |
| `d3d10effect.h` | Direct3D10 | remaining |  |  |  |
| `d3d10misc.h` | Direct3D10 | matched |  | 09/02/2026 19:47:59 | All outputs are standard COM interface pointers. |
| `d3d10sdklayers.h` | Direct3D10 | remaining |  |  |  |
| `d3d10shader.h` | Direct3D10 | matched |  |  | Investigated; clean, no ownership metadata gap. See docs/copilot/header-reports/d3d10shader.h.md |
| `d3d11_1.h` | Direct3D11, Mf | matched | copilot | 09/03/2026 00:25:00 | Classified retained artifact in existing-patches-06. |
| `d3d11_2.h` | Direct3D11 | matched | copilot | 09/03/2026 00:25:00 | Classified retained artifact in existing-patches-06. |
| `d3d11_3.h` | Direct3D11 | matched | copilot | 09/03/2026 00:25:00 | Classified retained artifact in existing-patches-06. |
| `d3d11_4.h` | Direct3D11, Mf | matched | copilot | 09/03/2026 00:40:00 | Classified retained artifact in existing-patches-07. |
| `d3d11.h` | Direct3D11, Mf | matched | copilot | 09/03/2026 00:25:00 | Classified retained artifact in existing-patches-06. |
| `d3d11on12.h` | Direct3D11on12 | matched | copilot | 09/03/2026 00:40:00 | Classified retained artifact in existing-patches-07. |
| `d3d11sdklayers.h` | Direct3D11 | matched | copilot | 09/03/2026 00:40:00 | Classified retained artifact in existing-patches-07. |
| `d3d11shader.h` | Direct3D11 | matched | copilot | 09/03/2026 00:40:00 | Classified retained artifact in existing-patches-07. |
| `d3d11shadertracing.h` | Direct3D11 | matched | copilot | 09/03/2026 00:40:00 | Classified retained artifact in existing-patches-07. |
| `d3d12.h` | Direct3D12 | matched | copilot | 09/03/2026 00:55:00 | Classified retained artifact in existing-patches-08. |
| `d3d12compatibility.h` | Direct3D12 | matched |  |  | Investigated; clean, no ownership metadata gap. See docs/copilot/header-reports/d3d12compatibility.h.md |
| `d3d12compiler.h` | Direct3D12 | remaining |  |  |  |
| `d3d12sdklayers.h` | Direct3D12 | remaining |  |  |  |
| `d3d12shader.h` | Direct3D12 | matched |  |  | Investigated; clean, no ownership metadata gap. See docs/copilot/header-reports/d3d12shader.h.md |
| `d3d12video.h` | Mf | matched | copilot | 09/03/2026 00:55:00 | Classified retained artifact in existing-patches-08. |
| `d3d9.h` | Direct3D9, Mf | matched | copilot | 09/03/2026 00:55:00 | Classified retained artifact in existing-patches-08. |
| `d3d9caps.h` | Direct3D9, Mf | matched |  |  | Investigated; clean, no ownership metadata gap. See docs/copilot/header-reports/d3d9caps.h.md |
| `d3d9helper.h` | Media.DShow | remaining |  |  |  |
| `d3d9on12.h` | Direct3D9on12 | matched |  | 09/02/2026 19:21:35 | COM factory/refcounting pattern throughout. |
| `d3d9types.h` | Direct3D9, Mf | remaining |  |  |  |
| `d3dcaps.h` | Direct3D9 | matched |  |  | Investigated; clean, no ownership metadata gap. See docs/copilot/header-reports/d3dcaps.h.md |
| `d3dcommon.h` | Direct2D, Direct3D, Direct3D10 | remaining |  |  |  |
| `d3dcompiler.h` | Direct3DFxc | matched | copilot | 09/03/2026 04:20:00 | No patch needed; COM interface only. |
| `d3dcsx.h` | Direct3D11 | matched |  |  | Investigated; clean, no ownership metadata gap. See docs/copilot/header-reports/d3dcsx.h.md |
| `d3dshadercacheregistration.h` | Direct3D | remaining |  |  |  |
| `d3dtypes.h` | Direct3D9 | remaining |  |  |  |
| `datetimeapi.h` | Intl | matched |  | 09/02/2026 19:19:02 | String/buffer formatting API only, no handle. |
| `davclnt.h` | WebDav | blocked |  | 09/02/2026 19:54:02 | DavAddConnection generic HANDLE out-param; DavRegisterAuthCallback returns generic DWORD-typed OPAQUE_HANDLE via return value. |
| `DbgEng.h` | Debug.Extensions | remaining |  |  |  |
| `dbghelp.h` | FileHistory, WinProg | remaining |  |  |  |
| `DbgModel.h` | Debug.Extensions | remaining |  |  |  |
| `dbgprop.h` | Debug | remaining |  |  |  |
| `dbt.h` | MenuRc | matched |  |  | Investigated; clean, no ownership metadata gap. See docs/copilot/header-reports/dbt.h.md |
| `dciddi.h` | Direct2D | matched |  |  | Investigated; clean, no ownership metadata gap. See docs/copilot/header-reports/dciddi.h.md |
| `dciman.h` | FileHistory, WinProg | blocked |  | 09/02/2026 20:07:23 | DCIOpenProvider/DCICloseProvider generic HDC return-value remains a gap; HWINWATCH gap fixed via autoTypes.json CloseApi addition. |
| `dcommon.h` | Direct2D, Direct2D.Common, DirectWrite | matched |  | 09/02/2026 22:06:03 | Enums/forward declaration only, no functions. |
| `dcomp.h` | DirectComp | matched | copilot | 09/03/2026 00:55:00 | Classified retained artifact in existing-patches-08. |
| `dcompanimation.h` | DirectComp | matched | copilot | 09/03/2026 00:55:00 | Classified retained artifact in existing-patches-08. |
| `dcomptypes.h` | DirectComp | matched |  | 09/02/2026 19:47:59 | Enums/structs/constants only, no functions. |
| `dde.h` | DataXchg | matched |  | 09/02/2026 20:25:02 | No function produces an opaque handle. |
| `ddeml.h` | DataXchg | matched |  |  | Genuine producer-site fix: added CloseApi=DdeFreeStringHandle to existing incomplete HSZ autoTypes.json entry. See docs/copilot/header-reports/ddeml.h.md |
| `ddkernel.h` | DirectDraw | matched |  | 09/02/2026 21:27:04 | COM-style interfaces + GUID constants only, no extern functions. |
| `ddpbackup.h` | Dedup | matched |  |  | Investigated; clean, no ownership metadata gap. See docs/copilot/header-reports/ddpbackup.h.md |
| `ddpchunk.h` | Dedup | matched |  |  | Investigated; clean, no ownership metadata gap. See docs/copilot/header-reports/ddpchunk.h.md |
| `ddpcommon.h` | Dedup | matched |  | 09/02/2026 19:16:41 | Enums/data structs + MIDL boilerplate only, no functions. |
| `ddpdataport.h` | Dedup | matched |  |  | Investigated; clean, no ownership metadata gap. See docs/copilot/header-reports/ddpdataport.h.md |
| `ddraw.h` | DirectDraw, Gdiplus, Media.DShow | matched |  | 09/02/2026 16:34:06 | HMONITOR reference is a pass-through system handle (never created/released by DirectDraw); guarded DECLARE_HANDLE fallback never fires. |
| `ddrawgdi.h` | FileHistory, WinProg | blocked |  | 09/02/2026 19:45:17 | DdGetDxHandle/DdCreateDIBSection return generic HANDLE/HBITMAP via return value. |
| `ddrawi.h` | DirectDraw, FileHistory, WinProg | remaining |  |  |  |
| `ddrawint.h` | DirectDraw | remaining |  |  |  |
| `ddstream.h` | Media.DShow | matched |  |  | Investigated; clean, no ownership metadata gap. See docs/copilot/header-reports/ddstream.h.md |
| `debugapi.h` | Base, Debug | matched |  | 09/02/2026 19:06:16 | HANDLE is a caller-supplied input, not produced here. |
| `defaultbrowsersyncsettings.h` | WinProg | matched |  | 09/02/2026 20:16:15 | COM interface + GUID constants only, no extern functions. |
| `delayloadhandler.h` | WinProg | matched |  | 09/02/2026 17:27:04 | Structs/callback-typedef/extern-variable only, no functions. |
| `deletebrowsinghistory.h` | WinProg | matched |  | 09/02/2026 20:22:05 | COM interface + GUID/flag constants only, no extern functions. |
| `deliveryoptimization.h` | DeliveryOptimization | matched | copilot | 09/03/2026 03:20:00 | No patch needed; verified via live scrape and static review. |
| `devfiltertypes.h` | DeviceQuery | matched |  | 09/02/2026 19:37:21 | Enum/struct only, no functions. |
| `devguid.h` | DevInst | matched |  |  | Investigated; clean, no ownership metadata gap. See docs/copilot/header-reports/devguid.h.md |
| `deviceaccess.h` | DeviceAccess | matched |  |  | Investigated; clean, no ownership metadata gap. See docs/copilot/header-reports/deviceaccess.h.md |
| `deviceservices.h` | WpdSdk | matched |  | 09/02/2026 17:58:11 | GUID/property-key constants only, no functions; BridgeDeviceService.h separately tracked (pending). |
| `devicetopology.h` | Audio | remaining |  |  |  |
| `devioctl.h` | Devices.1394, Devices.Beep, Devices.Cdrom, Devices.Dvd | matched |  | 09/02/2026 21:04:56 | Device-type constants only, no functions. |
| `devpkey.h` | Base, Devices.Properties | remaining |  |  |  |
| `devpropdef.h` | Display, Nfc, Nfp, WpdSdk | matched |  | 09/02/2026 21:21:09 | Constants/typedef only, no functions. |
| `devquery.h` | DeviceQuery, FileHistory | matched |  | 09/02/2026 21:01:40 | Fixed genuine gap: completed existing autoTypes.json entry for HDEVQUERY (CloseApi=DevCloseObjectQuery). |
| `devquerydef.h` | DeviceQuery | matched |  | 09/02/2026 18:59:13 | Enums/data structs only, no functions. |
| `dhcpcsdk.h` | Dhcp | matched |  |  | Investigated; clean, no ownership metadata gap. See docs/copilot/header-reports/dhcpcsdk.h.md |
| `dhcpsapi.h` | Dhcp | remaining |  |  |  |
| `dhcpssdk.h` | Dhcp | matched |  |  | Investigated; clean, no ownership metadata gap. See docs/copilot/header-reports/dhcpssdk.h.md |
| `dhcpv6csdk.h` | Dhcp | matched |  | 09/02/2026 21:12:30 | Constants only, no functions. |
| `diagnosticdataquery.h` | Security.DiagnosticDataQuery | matched |  | 09/02/2026 20:49:21 | All six opaque handle types already correctly covered via existing autoTypes.json entries; resolves deferred DIAGNOSTIC_DATA_QUERY_SESSION item. |
| `diagnosticdataquerytypes.h` | Security.DiagnosticDataQuery | matched |  | 09/02/2026 20:31:37 | Types/RPC boilerplate only, no functions; DIAGNOSTIC_DATA_QUERY_SESSION ownership deferred to DiagnosticDataQuery.h. |
| `digitalv.h` | Multimedia | remaining |  |  |  |
| `dimm.h` | Input.Ime | remaining |  |  |  |
| `dinput.h` | Hid | remaining |  |  |  |
| `dinputd.h` | Hid | matched |  |  | Investigated; clean, no ownership metadata gap. See docs/copilot/header-reports/dinputd.h.md |
| `directmanipulation.h` | DirectManipulation | matched | copilot | 09/03/2026 04:35:00 | No patch needed; COM interface only. |
| `directml.h` | DirectML | matched | copilot | 09/03/2026 04:20:00 | No patch needed; COM interface only. |
| `diskguid.h` | Fs | matched |  | 09/02/2026 20:34:23 | GUID constants only, no functions. |
| `dispatcherqueue.h` | WinRT | matched |  | 09/02/2026 19:09:17 | COM factory pattern. |
| `DispEx.h` | ComOle | matched | copilot | 09/03/2026 01:10:00 | Classified retained artifact in existing-patches-09. |
| `dlnadeviceinterfaceids.h` | Dlna | matched | copilot | 09/03/2026 03:20:00 | No patch needed; constant-only header verified via live scrape. |
| `dlnametadataproviderproperties.h` | Dlna | matched | copilot | 09/03/2026 03:20:00 | No patch needed; constant-only header verified via live scrape. |
| `dls1.h` | Audio.DirectMusic | matched |  | 09/02/2026 22:02:59 | Constants/documentation only, no functions. |
| `dls2.h` | Audio.DirectMusic | matched |  | 09/02/2026 20:13:28 | DLS2 FOURCC/format constants only, no functions. |
| `dmemmgr.h` | DirectDraw | blocked |  | 09/02/2026 20:41:40 | VidMemAlloc/HeapVidMemAllocAligned return generic FLATPTR (ULONG_PTR alias) via return value. |
| `dmerror.h` | TransactionServer | matched |  |  | Investigated; clean, no ownership metadata gap. See docs/copilot/header-reports/dmerror.h.md |
| `dmodshow.h` | Media.DShow | matched |  | 09/02/2026 19:42:09 | COM interface + GUID constants only, no extern functions. |
| `dmoreg.h` | Media.DxMediaObjects | matched |  | 09/02/2026 19:32:00 | Registration/enum functions operate on CLSIDs and standard COM interface pointers only. |
| `dmort.h` | Media.DxMediaObjects | matched |  | 09/02/2026 18:47:23 | DMO_MEDIA_TYPE alloc/free is a transparent-struct memory pattern, not an opaque handle. |
| `dmprocessxmlfiltered.h` | WpdSdk | matched |  | 09/02/2026 17:38:44 | BSTR output, not HANDLE-family. |
| `dmusics.h` | Audio.DirectMusic | matched |  | 09/02/2026 22:09:36 | COM-style interfaces + constants/structs only, no extern functions. |
| `docobj.h` | Com, Com.CallObj, Com.ChannelCreds, Com.Urlmon, ComOle, TransactionServer | matched | copilot | 09/03/2026 01:10:00 | Classified retained artifact in existing-patches-09. |
| `docobjectservice.h` | InternetExplorer | matched |  |  | Investigated; clean, no ownership metadata gap. See docs/copilot/header-reports/docobjectservice.h.md |
| `DocumentSource.h` | WinRT.Printing | matched |  | 09/02/2026 22:12:28 | COM/WinRT interface methods only, out of scope. |
| `documenttarget.h` | XpsPrinting | matched |  |  | Investigated; clean, no ownership metadata gap. See docs/copilot/header-reports/documenttarget.h.md |
| `dot1x.h` | Ndis, NWifi | matched |  | 09/02/2026 20:31:37 | Enums/structs only, no functions. |
| `downloadmgr.h` | InternetExplorer | matched |  | 09/02/2026 20:44:40 | COM interface method + GUID constant only, no extern functions. |
| `dpa_dsa.h` | Controls | matched | copilot | 09/03/2026 01:10:00 | Classified retained artifact in existing-patches-09. |
| `dpapi.h` | Security.Cryptography, Security.Cryptography.UI | matched |  |  | Investigated; clean, no ownership metadata gap. See docs/copilot/header-reports/dpapi.h.md |
| `dragdropinterop.h` | WinRT | matched |  | 09/02/2026 20:31:37 | COM/WinRT interop interface method only, out of scope. |
| `drt.h` | P2p | matched | copilot | 09/03/2026 01:10:00 | Classified retained artifact in existing-patches-09. |
| `dsadmin.h` | ActiveDirectory | matched |  | 09/02/2026 21:27:04 | GUID constants + COM interfaces only, no extern functions. |
| `dsattrib.h` | MsTv | matched |  |  | Investigated; clean, no ownership metadata gap. See docs/copilot/header-reports/dsattrib.h.md |
| `dsclient.h` | ActiveDirectory | matched |  |  | Investigated; clean, no ownership metadata gap. See docs/copilot/header-reports/dsclient.h.md |
| `dsconf.h` | Audio.DirectMusic | matched |  | 09/02/2026 22:06:03 | Constants/enum only, no functions. |
| `DsGetDC.h` | ActiveDirectory | matched |  |  | Investigated; clean, no ownership metadata gap. See docs/copilot/header-reports/DsGetDC.h.md |
| `dshow.h` | Media.DShow | matched |  | 09/02/2026 19:28:18 | Pure umbrella redirect; all sub-headers already individually tracked. |
| `dshowasf.h` | Media.DShow | matched |  |  | Investigated; clean, no ownership metadata gap. See docs/copilot/header-reports/dshowasf.h.md |
| `dskquota.h` | Fs, Shell | matched | copilot | 09/03/2026 01:10:00 | Classified retained artifact in existing-patches-09. |
| `dsound.h` | Audio.DirectSound, FileHistory, WinProg | remaining |  |  |  |
| `dsparse.h` | ActiveDirectory | matched |  |  | Investigated; clean, no ownership metadata gap. See docs/copilot/header-reports/dsparse.h.md |
| `dsquery.h` | ActiveDirectory | matched |  | 09/02/2026 20:13:28 | GUID constants/structs only, no functions. |
| `dsrole.h` | ActiveDirectory | matched |  | 09/02/2026 18:50:10 | Generic memory-buffer allocation, not HANDLE-family. |
| `dssec.h` | Security.DirectoryServices | matched |  | 09/02/2026 21:43:41 | Standard COM interface output + caller-supplied callback typedefs only. |
| `dtchelp.h` | DTC | matched |  | 09/02/2026 21:55:32 | Enum + dynamically-loaded function-pointer typedefs only, no extern functions. |
| `dv.h` | Audio.DirectMusic | matched |  | 09/02/2026 18:20:58 | Constants + data struct only, no functions. |
| `dvbsiparser.h` | MsTv | remaining |  |  |  |
| `dvdevcod.h` | Media.DShow | matched |  |  | Investigated; clean, no ownership metadata gap. See docs/copilot/header-reports/dvdevcod.h.md |
| `dvdmedia.h` | Media.DShow | matched | copilot | 09/03/2026 01:25:00 | Classified retained artifact in existing-patches-10. |
| `dvp.h` | DirectDraw | matched |  |  | Investigated; clean, no ownership metadata gap. See docs/copilot/header-reports/dvp.h.md |
| `dwmapi.h` | Dwm | matched | copilot | 09/03/2026 01:25:00 | Classified retained artifact in existing-patches-10. |
| `dwrite_1.h` | DirectWrite | matched | copilot | 09/03/2026 01:25:00 | Classified retained artifact in existing-patches-10. |
| `dwrite_2.h` | DirectWrite | matched | copilot | 09/03/2026 01:25:00 | Classified retained artifact in existing-patches-10. |
| `dwrite_3.h` | DirectWrite | matched | copilot | 09/03/2026 01:40:00 | Classified retained artifact in existing-patches-11. |
| `dwrite.h` | DirectWrite | matched | copilot | 09/03/2026 01:25:00 | Classified retained artifact in existing-patches-10. |
| `dxcapi.h` | Direct3DDxc | matched | copilot | 09/03/2026 04:20:00 | No patch needed; COM interface only. |
| `dxcore_interface.h` | Debug, DXCore | matched |  |  | Investigated; clean, no ownership metadata gap. See docs/copilot/header-reports/dxcore_interface.h.md |
| `dxcore.h` | Debug, DXCore | matched |  | 09/02/2026 17:23:42 | COM factory pattern (DXCoreCreateAdapterFactory). |
| `dxgi.h` | Dxgi, Dxgi.Common | matched | copilot | 09/03/2026 01:40:00 | Classified retained artifact in existing-patches-11. |
| `dxgi1_2.h` | Dxgi, Dxgi.Common | matched | copilot | 09/03/2026 01:40:00 | Classified retained artifact in existing-patches-11. |
| `dxgi1_3.h` | Dxgi, Dxgi.Common | matched | copilot | 09/03/2026 01:40:00 | Classified retained artifact in existing-patches-11. |
| `dxgi1_4.h` | Dxgi, Dxgi.Common | matched | copilot | 09/03/2026 01:40:00 | Classified retained artifact in existing-patches-11. |
| `dxgi1_5.h` | Dxgi, Dxgi.Common | matched | copilot | 09/03/2026 02:00:00 | Classified retained artifact in existing-patches-12. |
| `dxgi1_6.h` | Dxgi, Dxgi.Common | matched | copilot | 09/03/2026 02:00:00 | Classified retained artifact in existing-patches-12. |
| `dxgicommon.h` | Dxgi, Dxgi.Common | matched |  | 09/02/2026 18:15:12 | Data structs/enum/constants only, no functions. |
| `dxgidebug.h` | Dxgi, Dxgi.Common | matched | copilot | 09/03/2026 02:00:00 | Classified retained artifact in existing-patches-12. |
| `dxgiformat.h` | Dxgi, Dxgi.Common | matched |  | 09/02/2026 21:24:15 | Enum only, no functions. |
| `dxgimessages.h` | Dxgi, Dxgi.Common | matched |  |  | Investigated; clean, no ownership metadata gap. See docs/copilot/header-reports/dxgimessages.h.md |
| `dxgitype.h` | Dxgi, Dxgi.Common | matched |  | 09/02/2026 18:40:30 | Data structs/enums only, no functions. |
| `dxmini.h` | DirectDraw | matched |  |  | Investigated; clean, no ownership metadata gap. See docs/copilot/header-reports/dxmini.h.md |
| `dxprogrammablecapture.h` | Dxgi, Dxgi.Common | matched |  | 09/02/2026 17:45:25 | Parameterless COM interface methods, no handle. |
| `dxva.h` | Media.DShow, Mf | remaining |  |  |  |
| `dxva2api.h` | Media.DShow, Mf | remaining |  |  |  |
| `dxva2swdev.h` | Media.DShow | matched |  | 09/02/2026 20:38:46 | Function-pointer-typedef-in-struct pattern, out of scope; no extern functions. |
| `dxva2trace.h` | Media.DShow | matched |  | 09/02/2026 20:07:23 | ETW tracing constants/structs only, no functions. |
| `dxva9typ.h` | Media.DShow, Mf | matched | copilot | 09/03/2026 02:00:00 | Resolves the replay-mismatch flagged in shared-handle-policy-01; verified via isolated round-trip in this session (no fresh SDK checkout available to rerun the original full-replay evidence). |
| `dxvahd.h` | Mf | remaining |  |  |  |
| `eapauthenticatoractiondefine.h` | EapHost | matched |  | 09/02/2026 18:50:10 | Enums/data structs only, no functions. |
| `eapauthenticatortypes.h` | EapHost | matched |  | 09/02/2026 18:03:29 | Enum + MIDL boilerplate only, no functions. |
| `eaphosterror.h` | EapHost | matched |  |  | Investigated; clean, no ownership metadata gap. See docs/copilot/header-reports/eaphosterror.h.md |
| `eaphostpeerconfigapis.h` | EapHost | matched |  |  | Investigated; clean, no ownership metadata gap. See docs/copilot/header-reports/eaphostpeerconfigapis.h.md |
| `eaphostpeertypes.h` | EapHost | matched |  | 09/02/2026 19:35:00 | Enums/structs only, no functions. |
| `eapmethodauthenticatorapis.h` | EapHost | matched |  |  | Investigated; clean, no ownership metadata gap. See docs/copilot/header-reports/eapmethodauthenticatorapis.h.md |
| `eapmethodpeerapis.h` | EapHost | remaining |  |  |  |
| `eapmethodtypes.h` | EapHost | matched |  | 09/02/2026 17:30:54 | Type declarations only (struct/enum/typedef), no functions; producer functions tracked separately under eapmethodpeerapis.h (pending). |
| `eappapis.h` | EapHost | matched |  |  | Investigated; clean, no ownership metadata gap. See docs/copilot/header-reports/eappapis.h.md |
| `eaptypes.h` | EapHost | matched |  |  | Investigated; clean, no ownership metadata gap. See docs/copilot/header-reports/eaptypes.h.md |
| `edevdefs.h` | DeviceAccess | matched |  |  | Investigated; clean, no ownership metadata gap. See docs/copilot/header-reports/edevdefs.h.md |
| `editionupgradehelper.h` | FileHistory, WinProg | matched |  |  | Investigated; clean, no ownership metadata gap. See docs/copilot/header-reports/editionupgradehelper.h.md |
| `edpwin32.h` | Edp | matched |  | 09/02/2026 17:19:28 | Path-string-only API, no handle. |
| `effects.h` | Wmp | matched |  |  | Investigated; clean, no ownership metadata gap. See docs/copilot/header-reports/effects.h.md |
| `efswrtinterop.h` | Edp | remaining |  |  |  |
| `ehstorapi.h` | Enstor | remaining |  |  |  |
| `ehstorextensions.h` | Enstor | matched |  |  | Investigated; clean, no ownership metadata gap. See docs/copilot/header-reports/ehstorextensions.h.md |
| `ehstormsg.h` | Enstor | matched |  | 09/02/2026 22:12:28 | Constants/macros only, no functions. |
| `elscore.h` | Intl | matched |  | 09/02/2026 20:22:05 | All producer/consumer pairs operate on transparent structs, not opaque handles. |
| `elssrvc.h` | Intl | matched |  | 09/02/2026 18:03:29 | GUID constants only, no functions. |
| `emi.h` | Power | matched |  | 09/02/2026 18:56:34 | GUID/IOCTL constants + data structs only, no functions. |
| `emptyvc.h` | Lwef | matched |  |  | Investigated; clean, no ownership metadata gap. See docs/copilot/header-reports/emptyvc.h.md |
| `encdec.h` | MsTv | remaining |  |  |  |
| `endpointvolume.h` | Audio.Endpoints | remaining |  |  |  |
| `errhandlingapi.h` | Base, Debug | matched | copilot | 09/03/2026 02:15:00 | Classified retained artifact in existing-patches-13. |
| `errorrep.h` | Wer | matched |  | 09/02/2026 18:00:52 | HWND is a caller-supplied input, not produced here. |
| `errors.h` | Media.DShow | matched |  | 09/02/2026 17:38:44 | Caller-allocated string buffer output only, no handle. |
| `esent.h` | Storage.Jet | blocked | copilot | 09/03/2026 04:45:00 | Deferred: 229-function header with complex many-to-many JET handle ownership graph requiring dedicated research pass, not a quick classification. |
| `evalcom2.h` | Setup | matched |  | 09/02/2026 18:06:26 | COM interface with caller-supplied context pointers only, no handle. |
| `evcode.h` | Media.DShow | matched |  |  | Investigated; clean, no ownership metadata gap. See docs/copilot/header-reports/evcode.h.md |
| `evcoll.h` | Wec | matched |  | 09/02/2026 21:38:18 | Fixed genuine gap: added new autoTypes.json entry for EC_HANDLE (CloseApi=EcClose). |
| `eventsys.h` | Com.Events, TransactionServer | matched | copilot | 09/03/2026 02:15:00 | Classified retained artifact in existing-patches-13. |
| `evntcons.h` | Etw | matched |  |  | Investigated; clean, no ownership metadata gap. See docs/copilot/header-reports/evntcons.h.md |
| `evntprov.h` | Etw | remaining |  |  |  |
| `evntrace.h` | Etw, Media.DShow | matched | copilot | 09/03/2026 02:15:00 | Classified retained artifact in existing-patches-13. |
| `evr.h` | Mf | remaining |  |  |  |
| `evr9.h` | Mf | matched |  |  | Investigated; clean, no ownership metadata gap. See docs/copilot/header-reports/evr9.h.md |
| `exdisp.h` | FileHistory, Shell, WinProg | matched | copilot | 09/03/2026 02:15:00 | Classified retained artifact in existing-patches-13. |
| `exdispid.h` | InternetExplorer | matched |  |  | Investigated; clean, no ownership metadata gap. See docs/copilot/header-reports/exdispid.h.md |
| `expandedresources.h` | GameMode | matched |  | 09/02/2026 17:16:10 | No handle-typed values anywhere in this header. |
| `exposeenums2managed.h` | FileHistory, WinProg | matched |  | 09/02/2026 18:26:39 | Macro-definitions only, no declarations. |
| `extensionvalidation.h` | InternetExplorer | matched |  | 09/02/2026 21:17:15 | COM interface + enums only, no extern functions. |
| `extsfns.h` | Debug.Extensions | remaining |  |  |  |
| `faxcomex.h` | Fax | remaining |  |  |  |
| `faxdev.h` | Fax | blocked |  | 09/02/2026 20:25:02 | FaxDevStartJob produces generic HANDLE via direct out-param. |
| `faxext.h` | Fax | blocked |  | 09/02/2026 19:37:21 | FaxExtRegisterForEvents returns generic HANDLE via return value. |
| `faxmmc.h` | Fax | matched |  | 09/02/2026 17:58:11 | GUID/string constants only, no functions. |
| `faxroute.h` | Fax | matched |  | 09/02/2026 19:50:38 | No function produces an opaque handle; HANDLE params are pre-owned caller inputs. |
| `fci.h` | Cabinets | matched |  |  | Genuine producer-site fix: added new HFCI autoTypes.json entry (CloseApi=FCIDestroy). See docs/copilot/header-reports/fci.h.md |
| `fdi_fci_types.h` | Cabinets | matched |  | 09/02/2026 20:13:28 | Constants/structs only, no functions. |
| `fdi.h` | Cabinets | remaining |  |  |  |
| `featurestagingapi.h` | FileHistory, WinProg | matched |  | 09/02/2026 18:40:30 | FEATURE_STATE_CHANGE_SUBSCRIPTION ownership already correctly captured via existing autoTypes.json entry. |
| `fhcfg.h` | FileHistory | matched |  |  | Investigated; clean, no ownership metadata gap. See docs/copilot/header-reports/fhcfg.h.md |
| `fherrors.h` | FileHistory | matched |  | 09/02/2026 19:47:59 | HRESULT constants only, no functions. |
| `fhstatus.h` | FileHistory | matched |  | 09/02/2026 18:00:53 | Status-code constants only, no functions. |
| `fhsvcctl.h` | FileHistory | matched |  | 09/02/2026 18:50:10 | FH_SERVICE_PIPE_HANDLE ownership already correctly captured via existing autoTypes.json entry. |
| `fileapi.h` | Fs | matched | copilot | 09/03/2026 02:15:00 | Classified retained artifact in existing-patches-13. |
| `filehc.h` | FileHistory, Fs | matched | copilot | 09/03/2026 02:30:00 | Classified retained artifact in existing-patches-14. |
| `filter.h` | IndexSrv, Search | matched |  |  | Investigated; clean, no ownership metadata gap. See docs/copilot/header-reports/filter.h.md |
| `filtereg.h` | Search | matched |  |  | Investigated; clean, no ownership metadata gap. See docs/copilot/header-reports/filtereg.h.md |
| `filterpipeline.h` | Printing | remaining |  |  |  |
| `filterr.h` | IndexSrv | matched |  | 09/02/2026 19:14:35 | HRESULT error-code constants only, no functions. |
| `fltdefs.h` | FileHistory, IpHlp | matched |  |  | Genuine producer-site fix: added INTERFACE_HANDLE autoTypes.json entry (CloseApi=PfDeleteInterface). FILTER_HANDLE left unannotated (bulk-array removal, no single-handle CloseApi). See docs/copilot/header-reports/fltdefs.h.md |
| `fltuser.h` | Ifsk | matched |  |  | Investigated; clean, no ownership metadata gap. See docs/copilot/header-reports/fltuser.h.md |
| `fontsub.h` | Gdi | matched |  | 09/02/2026 21:52:12 | Buffer allocation delegated entirely to caller-supplied callbacks, no opaque handle. |
| `fsrm.h` | Fsrm | remaining |  |  |  |
| `fsrmenums.h` | Fsrm | matched |  |  | Investigated; clean, no ownership metadata gap. See docs/copilot/header-reports/fsrmenums.h.md |
| `fsrmerr.h` | Fsrm | matched |  |  | Investigated; clean, no ownership metadata gap. See docs/copilot/header-reports/fsrmerr.h.md |
| `fsrmpipeline.h` | Fsrm | remaining |  |  |  |
| `fsrmquota.h` | Fsrm | remaining |  |  |  |
| `fsrmreports.h` | Fsrm | remaining |  |  |  |
| `fsrmscreen.h` | Fsrm | remaining |  |  |  |
| `fsrmtlb.h` | Fsrm | matched |  |  | Investigated; clean, no ownership metadata gap. See docs/copilot/header-reports/fsrmtlb.h.md |
| `ftpext.h` | Iis | remaining |  |  |  |
| `fttypes.h` | Fs | matched |  | 09/02/2026 19:16:41 | Typedef/enums/data structs only, no functions. |
| `fullenumsyncdeviceservice.h` | WpdSdk | matched |  | 09/02/2026 20:28:23 | Constants only, no functions. |
| `functiondiscovery.h` | FunctionDiscovery | matched |  | 09/02/2026 19:28:18 | CLSID constants + MIDL boilerplate only, no functions. |
| `functiondiscoverycategories.h` | FunctionDiscovery | matched |  | 09/02/2026 19:12:01 | String constants only, no functions. |
| `functiondiscoveryconstraints.h` | FunctionDiscovery | matched |  | 09/02/2026 21:52:12 | Constants only, no functions. |
| `functiondiscoveryerror.h` | FunctionDiscovery | matched |  | 09/02/2026 18:03:29 | HRESULT error-code constants only, no functions. |
| `functiondiscoverykeys_devpkey.h` | Audio | matched |  |  | Investigated; clean, no ownership metadata gap. See docs/copilot/header-reports/functiondiscoverykeys_devpkey.h.md |
| `functiondiscoveryprovider.h` | FunctionDiscovery, WebServicesOnDevices | remaining |  |  |  |
| `fwpmtypes.h` | WindowsFilteringPlatform | remaining |  |  |  |
| `fwpmu.h` | WindowsFilteringPlatform | remaining |  |  |  |
| `fwptypes.h` | WindowsFilteringPlatform | matched |  |  | Investigated; clean, no ownership metadata gap. See docs/copilot/header-reports/fwptypes.h.md |
| `fxsutility.h` | Fax | matched |  | 09/02/2026 17:19:28 | No handle involved. |
| `GameInput.h` | GameInput | matched |  | 09/02/2026 16:34:06 | GameInputCreate is a clean COM-factory pattern; the one raw HANDLE out-param (IGameInputDispatcher::OpenWaitHandle) is a COM vtable method, outside DllImport-based ownership-annotation scope. |
| `gameux.h` | GameMode | matched |  |  | Investigated; clean, no ownership metadata gap. See docs/copilot/header-reports/gameux.h.md |
| `gamingdeviceinformation.h` | GamingDvcInfo | matched |  | 09/02/2026 18:06:26 | Plain struct output, no handle. |
| `gamingtcui.h` | Tcui | matched |  |  | Investigated; clean, no ownership metadata gap. See docs/copilot/header-reports/gamingtcui.h.md |
| `gb18030.h` | Intl | matched |  | 09/02/2026 20:31:37 | Buffer-conversion function only, no handles. |
| `gdiplus.h` | Gdiplus | matched |  | 09/02/2026 16:34:06 | GDI+ create/delete object pattern uses strongly-typed C++ pointers (GpGraphics* etc), not HANDLE-family typedefs; no pre-existing RAII metadata to correct; out of current policy scope. |
| `gdipluseffects.h` | Media.DShow | matched |  |  | Investigated; clean, no ownership metadata gap. See docs/copilot/header-reports/gdipluseffects.h.md |
| `genericusbfnioctl.h` | Buses | matched |  | 09/02/2026 20:49:21 | IOCTL constants only, no functions. |
| `getcurrentpackageinfo3.h` | AppxPackaging | matched |  | 09/02/2026 16:53:36 | Buffer-fill API, no handle production. |
| `getprocesshandlefromhwnd.h` | Threading | blocked | copilot | 09/02/2026 16:49:01 | Deferred: return-value HANDLE ownership has no precedent anywhere in the repo or published baseline winmd (confirmed via WinmdUtils dump); requires dedicated policy decision on annotation placement before this can be fixed. |
| `gl/gl.h` | OpenGL | remaining |  |  |  |
| `gl/glu.h` | OpenGL | remaining |  |  |  |
| `gnssdriver.h` | WinLocation | matched |  |  | Investigated; clean, no ownership metadata gap. See docs/copilot/header-reports/gnssdriver.h.md |
| `gpedit.h` | Policy | matched |  |  | Investigated; clean, no ownership metadata gap. See docs/copilot/header-reports/gpedit.h.md |
| `gpiobuttontypes.h` | Hid | matched |  | 09/02/2026 18:23:41 | Enum/constants/data struct only, no functions. |
| `gpmgmt.h` | Gpmc | matched | copilot | 09/03/2026 05:15:00 | No patch needed; COM interface only. |
| `guiddef.h` | TransactionServer | matched |  | 09/02/2026 20:07:23 | GUID type/macros/inline helpers only, no extern functions. |
| `handleapi.h` | FileHistory, Foundation | matched | copilot | 09/03/2026 02:30:00 | Classified retained artifact in existing-patches-14. |
| `hidclass.h` | Hid | matched |  | 09/02/2026 21:59:01 | GUID/property-key constants only, no functions. |
| `hidpi.h` | Hid | remaining |  |  |  |
| `hidsdi.h` | Hid | matched |  |  | Genuine producer-site fix: added CloseApi=HidD_FreePreparsedData to existing incomplete PHIDP_PREPARSED_DATA autoTypes.json entry. See docs/copilot/header-reports/hidsdi.h.md |
| `hidusage.h` | Hid | remaining |  |  |  |
| `highlevelmonitorconfigurationapi.h` | Monitor | matched |  | 09/02/2026 21:17:15 | No function produces an opaque handle; HANDLE param is a pre-owned caller input. |
| `hintsdeviceservice.h` | WpdSdk | matched |  | 09/02/2026 21:27:04 | Constants only, no functions. |
| `holographicspaceinterop.h` | MixedRealityInterop | matched |  | 09/02/2026 21:04:56 | COM/WinRT interop interface method only, out of scope. |
| `homepagesetting.h` | InternetExplorer | matched |  | 09/02/2026 20:56:17 | COM interface + GUID constants only, no extern functions. |
| `hrtfapoapi.h` | Xaudio2 | matched |  | 09/02/2026 21:04:56 | Standard COM factory function + constants/structs only. |
| `hstring.h` | WinRT | matched |  | 09/02/2026 18:47:23 | Type declarations only (opaque handle typedefs), no functions; producer functions tracked separately under winstring.h (pending). |
| `htiface.h` | InternetExplorer | matched |  |  | Investigated; clean, no ownership metadata gap. See docs/copilot/header-reports/htiface.h.md |
| `htiframe.h` | InternetExplorer | matched |  |  | Investigated; clean, no ownership metadata gap. See docs/copilot/header-reports/htiframe.h.md |
| `htmlhelp.h` | HtmlHelp | matched | copilot | 09/03/2026 02:30:00 | Classified retained artifact in existing-patches-14. |
| `http.h` | HttpServer | remaining |  |  |  |
| `httpext.h` | Iis | matched |  |  | Investigated; clean, no ownership metadata gap. See docs/copilot/header-reports/httpext.h.md |
| `httpfilt.h` | Iis | matched |  |  | Investigated; clean, no ownership metadata gap. See docs/copilot/header-reports/httpfilt.h.md |
| `httprequest.h` | WinHttp | matched | copilot | 09/03/2026 03:35:00 | No patch needed; verified via live scrape and static review. |
| `httprequestid.h` | Iis | matched |  | 09/02/2026 18:09:40 | DISPID constants only, no functions. |
| `httptrace.h` | Iis | matched |  | 09/02/2026 20:44:40 | Enum/structs/GUID constants only, no functions. |
| `hvsocket.h` | Hypervisor | matched |  | 09/02/2026 19:50:38 | Constants/structs only, no functions. |
| `hwebcore.h` | Iis | matched |  | 09/02/2026 19:37:21 | Dynamically-loaded function-pointer typedefs only, no scrapable extern functions. |
| `hypervdevicevirtualization.h` | Hypervisor | matched |  | 09/02/2026 21:43:41 | Fixed genuine gap: added new autoTypes.json entry for HDV_HOST (CloseApi=HdvTeardownDeviceHost). |
| `i_cryptasn1tls.h` | Security.Cryptography, Security.Cryptography.UI | blocked | copilot | 09/02/2026 18:56:34 | Recorded for per-header traceability; same root cause and evidence as getprocesshandlefromhwnd.h. |
| `iaccess.h` | TransactionServer | matched |  |  | Investigated; clean, no ownership metadata gap. See docs/copilot/header-reports/iaccess.h.md |
| `iadmext.h` | Iis | matched |  | 09/02/2026 19:06:16 | COM interface, not HANDLE-family. |
| `iadmw.h` | Iis | remaining |  |  |  |
| `iads.h` | ActiveDirectory, Iis | remaining |  |  |  |
| `icftypes.h` | WindowsFirewall | matched |  | 09/02/2026 20:22:05 | Enums + MIDL boilerplate only, no functions. |
| `ichannelcredentials.h` | Com.ChannelCreds, ComOle | matched |  |  | Investigated; clean, no ownership metadata gap. See docs/copilot/header-reports/ichannelcredentials.h.md |
| `Icm.h` | Wcs | matched | copilot | 09/03/2026 02:30:00 | Classified retained artifact in existing-patches-14. |
| `icmpapi.h` | IpHlp | matched | copilot | 09/03/2026 02:30:00 | Classified retained artifact in existing-patches-14. |
| `icontact.h` | WinContacts | remaining |  |  |  |
| `icontactproperties.h` | WinContacts | matched |  |  | Investigated; clean, no ownership metadata gap. See docs/copilot/header-reports/icontactproperties.h.md |
| `icontentprefetchertasktrigger.h` | Wsw | matched |  | 09/02/2026 21:01:40 | COM/WinRT interface method only, out of scope. |
| `icu.h` | Intl | remaining |  |  |  |
| `icucommon.h` | Intl | matched | copilot | 09/03/2026 02:45:00 | Classified retained artifact in existing-patches-15. |
| `icui18n.h` | Intl | remaining |  |  |  |
| `icwcfg.h` | FileHistory, InternetConnectionWizard | matched | copilot | 09/03/2026 05:00:00 | No patch needed; COM interface only. |
| `identitycommon.h` | IdentityProvider | matched |  | 09/02/2026 17:54:05 | Enum + MIDL boilerplate only, no functions. |
| `identityprovider.h` | IdentityProvider | remaining |  |  |  |
| `identitystore.h` | IdentityProvider | matched |  |  | Investigated; clean, no ownership metadata gap. See docs/copilot/header-reports/identitystore.h.md |
| `idispids.h` | ComOle, InternetExplorer | matched |  | 09/02/2026 17:16:10 | DISPID constants only, no functions. |
| `ie12plugin.h` | InternetExplorer | remaining |  |  |  |
| `ieautomation.h` | InternetExplorer | matched |  |  | Investigated; clean, no ownership metadata gap. See docs/copilot/header-reports/ieautomation.h.md |
| `ieobj.h` | InternetExplorer | matched | copilot | 09/03/2026 02:45:00 | Classified retained artifact in existing-patches-15. |
| `iepmapi.h` | InternetExplorer | matched | copilot | 09/03/2026 02:45:00 | Classified retained artifact in existing-patches-15. |
| `iewebdriver.h` | InternetExplorer | matched |  |  | Investigated; clean, no ownership metadata gap. See docs/copilot/header-reports/iewebdriver.h.md |
| `iextag.h` | InternetExplorer | remaining |  |  |  |
| `il21dec.h` | Media.DShow | matched |  | 09/02/2026 19:21:35 | COM interface with plain-value parameters only, no handle. |
| `ImageHlp.h` | Debug | remaining |  |  |  |
| `imagetranscode.h` | Shell | matched | copilot | 09/03/2026 02:45:00 | Classified retained artifact in existing-patches-15. |
| `imapi.h` | IMapi | remaining |  |  |  |
| `imapi2.h` | IMapi | remaining |  |  |  |
| `imapi2error.h` | IMapi, Wab | matched |  |  | Investigated; clean, no ownership metadata gap. See docs/copilot/header-reports/imapi2error.h.md |
| `imapi2fs.h` | IMapi | remaining |  |  |  |
| `imapi2fserror.h` | Wab | matched |  |  | Investigated; clean, no ownership metadata gap. See docs/copilot/header-reports/imapi2fserror.h.md |
| `imapierror.h` | IMapi | matched |  | 09/02/2026 19:35:00 | HRESULT constants only, no functions. |
| `ime_cmodes.h` | Input.Ime | matched |  | 09/02/2026 17:09:31 | Bit-field constants only, no functions. |
| `ime.h` | FileHistory, WinProg | matched |  | 09/02/2026 20:56:17 | No function produces an opaque handle. |
| `imepad.h` | Input.Ime | matched |  |  | Investigated; clean, no ownership metadata gap. See docs/copilot/header-reports/imepad.h.md |
| `imessage.h` | IMapi | matched |  | 09/02/2026 21:32:46 | Fixed genuine gap: completed autoTypes.json entry for LPMSGSESS (CloseApi=CloseIMsgSession). |
| `imessagedispatcher.h` | TransactionServer | matched |  | 09/02/2026 20:22:05 | COM/WinRT interface method only, out of scope. |
| `imgerror.h` | Printing | matched |  |  | Investigated; clean, no ownership metadata gap. See docs/copilot/header-reports/imgerror.h.md |
| `imgutil.h` | InternetExplorer | matched |  |  | Investigated; clean, no ownership metadata gap. See docs/copilot/header-reports/imgutil.h.md |
| `imm.h` | Input.Ime | matched |  |  | Investigated; clean, no ownership metadata gap. See docs/copilot/header-reports/imm.h.md |
| `immdev.h` | Input.Ime | remaining |  |  |  |
| `in6addr.h` | WinSock, wnv | matched |  | 09/02/2026 17:16:10 | Plain data struct + macros only, no functions. |
| `inaddr.h` | IpHlp, RRas, WinSock | matched |  | 09/02/2026 17:19:28 | Plain data struct + macros only, no functions. |
| `indexsrv.h` | IndexSrv, Search | matched |  |  | Investigated; clean, no ownership metadata gap. See docs/copilot/header-reports/indexsrv.h.md |
| `inetreg.h` | InternetExplorer | matched |  |  | Investigated; clean, no ownership metadata gap. See docs/copilot/header-reports/inetreg.h.md |
| `inetsdk.h` | InternetExplorer | matched |  | 09/02/2026 18:23:41 | Pure MIDL boilerplate, no functions. |
| `infocard.h` | Security.Cryptography, Security.Cryptography.UI | matched |  |  | Investigated; clean, no ownership metadata gap. See docs/copilot/header-reports/infocard.h.md |
| `infotech.h` | HtmlHelp | matched |  |  | Investigated; clean, no ownership metadata gap. See docs/copilot/header-reports/infotech.h.md |
| `infstr.h` | DevInst | matched |  |  | Investigated; clean, no ownership metadata gap. See docs/copilot/header-reports/infstr.h.md |
| `inked.h` | Tablet | remaining |  |  |  |
| `inkpresenterdesktop.h` | Input_Ink | matched |  |  | Investigated; clean, no ownership metadata gap. See docs/copilot/header-reports/inkpresenterdesktop.h.md |
| `inkrenderer.h` | Input_Ink | matched |  | 09/02/2026 21:52:12 | COM interfaces + enum only, no extern functions. |
| `inputpaneinterop.h` | WinRT | matched |  | 09/02/2026 20:56:17 | COM/WinRT interop interface method only, out of scope. |
| `inputpanelconfiguration.h` | Shell | matched | copilot | 09/03/2026 02:45:00 | Classified retained artifact in existing-patches-15. |
| `inputscope.h` | Tsf | matched |  |  | Investigated; clean, no ownership metadata gap. See docs/copilot/header-reports/inputscope.h.md |
| `inspectable.h` | Tcui | matched |  | 09/02/2026 21:12:30 | COM/WinRT interface method only, out of scope. |
| `interactioncontext.h` | Input_IntContext | matched |  |  | Investigated; clean, no ownership metadata gap. See docs/copilot/header-reports/interactioncontext.h.md |
| `intrinfix.h` | ActiveDirectory, AllJoyn, Antimalware, ApplicationVerifier, AppxPackaging, Audio, Audio.Apo, Audio.DirectMusic, Audio.DirectSound, Audio.Endpoints, Authorization, Authorization.UI, Backup, Base, Bits, Bluetooth, Buses, Cabinets, CEIP, Certificates, Cloudapi, ClrHosting, ClrProfiling, CmpApi, Com, Com.CallObj, Com.ChannelCreds, Com.Events, Com.StructuredStorage, Com.Urlmon, Communication, ComOle, CompositionSwapchain, Console, Controls, Controls.RichEdit, Cos, Credentials, DataXchg, Debug, Debug.ActiveScript, Debug.Extensions, Debug.WebApp, Dedup, DeliveryOptimization, DeviceAccess, DeviceQuery, Devices.1394, Devices.Beep, Devices.Cdrom, Devices.Dvd, Devices.Properties, DevInst, DevLic, Dfs, Dhcp, Direct2D, Direct2D.Common, Direct3D, Direct3D10, Direct3D11, Direct3D11on12, Direct3D12, Direct3D9, Direct3D9on12, Direct3DDxc, Direct3DFxc, DirectComp, DirectDraw, DirectManipulation, DirectML, DirectWrite, Display, DlgBox, Dns, DTC, Dwm, DXCore, Dxgi, Dxgi.Common, Eap, EapHost, Edp, Enstor, Environment, Etw, Fax, FileHistory, Foundation, Fs, Fsrm, FunctionDiscovery, GameInput, GameMode, GamingDvcInfo, Gdi, Gdiplus, Gpmc, Hid, HostComputeNetwork, HostComputeSystem, HtmlHelp, HttpServer, Hypervisor, Identity, IdentityProvider, Ifsk, Iis, ImagingDevice, IMapi, IndexSrv, Input_Ink, Input_IntContext, Input.Ime, InputRadial, InternetConnectionWizard, InternetExplorer, Intl, IO, Ioctl, IpHlp, IScsiDisc, Isolation, JobObjects, Js, Kernel, KeyboardAndMouseInput, Ldap, LibraryLoader, Lwef, MachineLearning, MadCap, MagApi, Mapi, Mbn, MdmReg, Media, Media.DShow, Media.DShow.Xml, Media.DxMediaObjects, Media.KernelStreaming, Memory, MenuRc, MessageQueuing, Mf, Mi, MixedReality, MixedRealityInterop, Mmc, Monitor, MsChap, MsCs, MsHtml, MsTv, MsXml, Multimedia, Ndf, Ndis, NetBios, NetMgmt, NetShell, NetworkAccessProtection, Nfc, Nfp, Nla, Nps, NWifi, Of, Opc, OpenGL, P2p, Parcon, Perf, PicAcq, Pipes, Pla, Policy, Power, Printing, PrintTicket, Proc_Snap, ProjFs, Properties, PsApi1, PsApi2, PWM, Qos, Rdc, Rdp, RealTimeCommunications, Registry, RemoteAssist, Rm, Rpc, RRas, RstMgr, Search, SecBitomet, Security, Security.AppLocker, Security.ConfigurationSnapin, Security.Cryptography, Security.Cryptography.Catalog, Security.Cryptography.Sip, Security.Cryptography.UI, Security.DiagnosticDataQuery, Security.DirectoryServices, Security.LicenseProtection, Security.Tpm, Security.WinTrust, Security.WinWlx, SecurityCenter, Sens, SerPorts, Services, Setup, Shell, Shell.Common, Shutdown, SideShow, Smi, Snmp, Speech, Sr, Storage.Jet, Storage.Nvme, SwDevice, SystemInformation, Tablet, Tapi3, TaskSchd, Tbs, Tcui, TermServ, Threading, Time, ToolHelp, TransactionServer, Tsf, Ual, UiAnimation, UpdateAssessment, Upnp, Variant, Vhd, VirtualDiskService, VirtualDOSMachines, VSS, Wab, Wcm, Wcn, Wcs, Wds, WebAuthn, WebDav, WebServicesOnDevices, WebSock, Wec, Wer, Wes, Wia, Wibe, Wic, Win32_Tile_Badge_Notif, WinAuto, WinContacts, WindowsConnectionManager, WindowsFilteringPlatform, WindowsFirewall, WindowsRibbon, WindowsSetupAndMigration, WinHttp, WinInet, WinLocation, WinProg, WinRm, WinRT, WinRT.AllJoyn, WinRT.Composition, WinRT.CoreInputView, WinRT.Direct2D, WinRT.Direct3D11, WinRT.Display, WinRT.Graphics.Capture, WinRT.Graphics.Imaging, WinRT.Holographic, WinRT.Isolation, WinRT.Media, WinRT.Metadata, WinRT.ML, WinRT.Pdf, WinRT.Printing, WinRT.Storage, WinRT.Xaml, WinSat, WinSensors, WinSock, WinSync, WinTouch, Wmdm, WmFormat, Wmi, Wmlss, Wmp, Wnet, wnv, WpdSdk, Wsb, Wsl, Wsw, Wua, Xaml_Diagnostics, Xaudio2, Xblidp, Xinput, XmlLite, Xps, XpsPrinting | remaining |  |  |  |
| `intsafe.h` | Shell | remaining |  |  |  |
| `intshcut.h` | Shell | matched | copilot | 09/03/2026 03:00:00 | Classified retained artifact in existing-patches-16. |
| `ioapiset.h` | Base, IO | matched | copilot | 09/03/2026 03:00:00 | Classified retained artifact in existing-patches-16. |
| `ioevent.h` | MenuRc | matched |  |  | Investigated; clean, no ownership metadata gap. See docs/copilot/header-reports/ioevent.h.md |
| `ioringapi.h` | Fs | matched |  | 09/02/2026 21:59:01 | HIORING already correctly covered via existing autoTypes.json entry. |
| `ip2string.h` | IpHlp, wnv | matched |  | 09/02/2026 20:34:23 | String-conversion functions write into caller buffers only, no handles. |
| `IPExport.h` | IpHlp | matched |  | 09/02/2026 22:09:36 | Type aliases/structs only, no functions. |
| `iphlpapi.h` | IpHlp, Shell | matched | copilot | 09/03/2026 03:00:00 | Classified retained artifact in existing-patches-16. |
| `ipinfoid.h` | IpHlp | matched |  | 09/02/2026 19:47:59 | Constants only, no functions. |
| `ipmib.h` | RRas | matched |  |  | Investigated; clean, no ownership metadata gap. See docs/copilot/header-reports/ipmib.h.md |
| `IPTypes.h` | wnv | matched |  |  | Investigated; clean, no ownership metadata gap. See docs/copilot/header-reports/IPTypes.h.md |
| `iscsidsc.h` | IScsiDisc | remaining |  |  |  |
| `iscsierr.h` | IScsiDisc | matched |  |  | Investigated; clean, no ownership metadata gap. See docs/copilot/header-reports/iscsierr.h.md |
| `isolatedapplauncher.h` | FileHistory, Isolation | matched |  |  | Investigated; clean, no ownership metadata gap. See docs/copilot/header-reports/isolatedapplauncher.h.md |
| `isolatedwindowsenvironmentutils.h` | Isolation | matched |  | 09/02/2026 16:57:06 | Boolean query functions only. |
| `issper16.h` | Identity | matched |  | 09/02/2026 21:08:27 | Constants only, no functions. |
| `iswindowarranged.h` | MenuRc | matched |  | 09/02/2026 16:49:01 | Query-only function taking HWND input; no handle production. |
| `isysmon.h` | Perf | remaining |  |  |  |
| `iwscapi.h` | FileHistory, SecurityCenter | remaining |  |  |  |
| `iwstdec.h` | Media.DShow | matched |  | 09/02/2026 19:14:35 | COM interface with plain-value parameters only, no handle. |
| `joystickapi.h` | Multimedia | matched |  |  | Investigated; clean, no ownership metadata gap. See docs/copilot/header-reports/joystickapi.h.md |
| `jscript9diag.h` | Debug.ActiveScript | remaining |  |  |  |
| `jsrt.h` | Js | matched |  | 09/02/2026 17:35:31 | Redirect-only; reachable branch (jsrt9.h) already tracked pending; chakrart.h unreachable in this partition. |
| `jsrt9.h` | Js | remaining |  |  |  |
| `kbd.h` | KeyboardAndMouseInput | matched | copilot | 09/03/2026 04:20:00 | No patch needed; data-structure-only header. |
| `keycredmgr.h` | Credentials | matched |  | 09/02/2026 18:20:58 | KeyCredentialManagerInfo alloc/free is a transparent-struct memory pattern, not an opaque handle. |
| `knownfolders.h` | Shell | matched |  |  | Investigated; clean, no ownership metadata gap. See docs/copilot/header-reports/knownfolders.h.md |
| `ks.h` | Audio, Audio.DirectMusic, Media.DShow, Media.KernelStreaming, MsTv, Multimedia | remaining |  |  |  |
| `ksmedia.h` | Audio, Audio.DirectMusic, Media.DShow, Media.KernelStreaming, MsTv, Multimedia | remaining |  |  |  |
| `ksproxy.h` | Media.KernelStreaming | matched |  |  | Investigated; clean, no ownership metadata gap. See docs/copilot/header-reports/ksproxy.h.md |
| `ktmtypes.h` | Fs | matched |  | 09/02/2026 21:17:15 | Type aliases/constants only, no functions. |
| `ktmw32.h` | Fs | matched | copilot | 09/03/2026 03:00:00 | Classified retained artifact in existing-patches-16. |
| `libloaderapi.h` | Intl, LibraryLoader | matched | copilot | 09/03/2026 03:00:00 | Classified retained artifact in existing-patches-16. |
| `libloaderapi2.h` | LibraryLoader | blocked | copilot | 09/02/2026 17:58:11 | Recorded for per-header traceability; same root cause and evidence as getprocesshandlefromhwnd.h, no new investigation required. |
| `licenseprotection.h` | Security.LicenseProtection | matched |  | 09/02/2026 17:38:44 | Enum/FILETIME output only, no handle. |
| `lm.h` | NetMgmt | matched |  | 09/02/2026 17:38:44 | Pure umbrella redirect; all 18 sub-headers already individually tracked in ledger (2 accepted, 16 pending). |
| `lmaccess.h` | FileHistory, NetMgmt, WinProg | matched | copilot | 09/03/2026 03:15:00 | Classified retained artifact in existing-patches-17. |
| `lmalert.h` | NetMgmt | matched |  | 09/02/2026 19:19:02 | Caller-supplied input buffers only, no handle. |
| `lmapibuf.h` | NetMgmt | matched |  | 09/02/2026 18:32:46 | Canonical NetApiBufferFree convention, generic memory buffer, not HANDLE-family. |
| `lmat.h` | NetMgmt | matched |  | 09/02/2026 19:16:41 | Net API buffer-allocation convention, not HANDLE-family. |
| `lmaudit.h` | NetMgmt | matched |  |  | Investigated; clean, no ownership metadata gap. See docs/copilot/header-reports/lmaudit.h.md |
| `lmconfig.h` | NetMgmt | matched |  | 09/02/2026 18:03:29 | Net API buffer-allocation convention (NetApiBufferFree), not HANDLE-family. |
| `lmcons.h` | Dfs, NetMgmt | matched |  | 09/02/2026 21:32:46 | Constants only, no functions. |
| `lmdfs.h` | Dfs | remaining |  |  |  |
| `lmerr.h` | NetMgmt | remaining |  |  |  |
| `lmerrlog.h` | NetMgmt | remaining |  |  |  |
| `lmjoin.h` | NetMgmt | matched |  |  | Investigated; clean, no ownership metadata gap. See docs/copilot/header-reports/lmjoin.h.md |
| `lmmsg.h` | NetMgmt | matched |  | 09/02/2026 18:26:39 | Net API buffer-allocation convention (NetApiBufferFree), not HANDLE-family. |
| `lmremutl.h` | NetMgmt | matched |  | 09/02/2026 19:14:35 | Net API buffer-allocation convention, not HANDLE-family. |
| `lmrepl.h` | NetMgmt | matched |  | 09/02/2026 20:53:10 | Net API buffer-allocation convention. |
| `lmserver.h` | Base, NetMgmt | matched | copilot | 09/03/2026 03:15:00 | Classified retained artifact in existing-patches-17. |
| `lmshare.h` | Fs | matched |  |  | Investigated; clean, no ownership metadata gap. See docs/copilot/header-reports/lmshare.h.md |
| `lmsname.h` | NetMgmt | matched |  | 09/02/2026 19:35:00 | Service-name string constants only, no functions. |
| `lmstats.h` | Fs | matched |  | 09/02/2026 20:16:15 | Net API buffer-allocation convention. |
| `lmsvc.h` | NetMgmt | matched |  |  | Investigated; clean, no ownership metadata gap. See docs/copilot/header-reports/lmsvc.h.md |
| `lmuse.h` | NetMgmt | matched |  | 09/02/2026 22:09:36 | Net API buffer-allocation convention. |
| `lmuseflg.h` | NetMgmt | matched |  | 09/02/2026 17:23:42 | Constants/macro only, no functions. |
| `lmwksta.h` | NetMgmt | matched |  |  | Investigated; clean, no ownership metadata gap. See docs/copilot/header-reports/lmwksta.h.md |
| `loadperf.h` | FileHistory, Perf, WinProg | matched |  | 09/02/2026 19:12:01 | String/flag-only API, no handle. |
| `locationapi.h` | WinLocation | remaining |  |  |  |
| `lowlevelmonitorconfigurationapi.h` | Monitor | matched |  | 09/02/2026 17:48:13 | All functions only consume an existing physical-monitor HANDLE; producer pair tracked separately under PhysicalMonitorEnumerationAPI.h (pending). |
| `lzexpand.h` | Fs | matched | copilot | 09/03/2026 03:15:00 | Classified retained artifact in existing-patches-17. |
| `madcapcl.h` | MadCap | matched | copilot | 09/03/2026 05:00:00 | No patch needed; no ownership-relevant handle type. |
| `magnification.h` | MagApi | matched | copilot | 09/03/2026 03:15:00 | Classified retained artifact in existing-patches-17. |
| `mailmsgprops.h` | IMapi | matched |  |  | Investigated; clean, no ownership metadata gap. See docs/copilot/header-reports/mailmsgprops.h.md |
| `manipulations.h` | WinTouch | remaining |  |  |  |
| `manual.h` | Com.StructuredStorage | remaining |  |  |  |
| `mapi.h` | Mapi | matched |  |  | Investigated; clean, no ownership metadata gap. See docs/copilot/header-reports/mapi.h.md |
| `mbnapi.h` | Mbn | matched | copilot | 09/03/2026 05:00:00 | No patch needed; COM interface only. |
| `mciapi.h` | Multimedia | remaining |  |  |  |
| `mciavi.h` | Multimedia | matched |  | 09/02/2026 18:59:13 | Constants only, no functions. |
| `mcx.h` | Communication | matched |  |  | Investigated; clean, no ownership metadata gap. See docs/copilot/header-reports/mcx.h.md |
| `mdhcp.h` | Tapi3 | remaining |  |  |  |
| `mdmlocalmanagement.h` | MdmReg | matched |  | 09/02/2026 17:54:05 | No handle-typed values; string output uses LocalFree, not HANDLE mechanism. |
| `mdmregistration.h` | MdmReg | matched |  |  | Investigated; clean, no ownership metadata gap. See docs/copilot/header-reports/mdmregistration.h.md |
| `mediaerr.h` | Media.DShow, Media.DxMediaObjects | matched |  | 09/02/2026 17:13:02 | HRESULT error constants only, no functions. |
| `mediaobj.h` | Media.DShow, Media.DxMediaObjects | remaining |  |  |  |
| `medparam.h` | Media.DShow | matched |  |  | Investigated; clean, no ownership metadata gap. See docs/copilot/header-reports/medparam.h.md |
| `memorybuffer.h` | WinRT | matched |  | 09/02/2026 17:05:19 | IMemoryBufferByteAccess COM interface; buffer view not a HANDLE-family resource. |
| `menutemplate.h` | MenuRc | matched |  | 09/02/2026 17:01:48 | Plain data structs only, no functions. |
| `mergemod.h` | Setup | remaining |  |  |  |
| `messagedeviceservice.h` | WpdSdk | matched |  |  | Investigated; clean, no ownership metadata gap. See docs/copilot/header-reports/messagedeviceservice.h.md |
| `messagedispatcherapi.h` | ComOle, WinRT | matched |  | 09/02/2026 17:13:02 | IMessageDispatcher pointer is caller-supplied input, not produced here. |
| `metadatadeviceservice.h` | WpdSdk | matched |  | 09/02/2026 18:26:39 | GUID/property-key constants only, no functions. |
| `metahost.h` | ClrHosting | matched | copilot | 09/02/2026 23:40:00 | No patch needed; verified via live scrape plus confirmation that global supportedOS.rsp already covers this header's functions. |
| `mfapi.h` | Mf | remaining |  |  |  |
| `mfcaptureengine.h` | Mf | remaining |  |  |  |
| `mfcontentdecryptionmodule.h` | Mf | matched |  |  | Investigated; clean, no ownership metadata gap. See docs/copilot/header-reports/mfcontentdecryptionmodule.h.md |
| `mfd3d12.h` | Direct3D12, Mf | matched |  |  | Investigated; clean, no ownership metadata gap. See docs/copilot/header-reports/mfd3d12.h.md |
| `mferror.h` | Mf | remaining |  |  |  |
| `mfidl.h` | Mf | remaining |  |  |  |
| `mfmediacapture.h` | Mf | matched |  |  | Investigated; clean, no ownership metadata gap. See docs/copilot/header-reports/mfmediacapture.h.md |
| `mfmediaengine.h` | Mf | remaining |  |  |  |
| `mfmp2dlna.h` | Mf | matched |  | 09/02/2026 20:28:23 | COM interface + GUID constants/struct only, no extern functions. |
| `mfobjects.h` | Mf | matched | copilot | 09/03/2026 03:15:00 | Classified retained artifact in existing-patches-17. |
| `mfplay.h` | Mf | remaining |  |  |  |
| `mfreadwrite.h` | Mf | remaining |  |  |  |
| `mfsharingengine.h` | Mf | remaining |  |  |  |
| `mfspatialaudio.h` | Mf | matched |  |  | Investigated; clean, no ownership metadata gap. See docs/copilot/header-reports/mfspatialaudio.h.md |
| `mftransform.h` | Mf | remaining |  |  |  |
| `mfvirtualcamera.h` | Mf | matched |  |  | Investigated; clean, no ownership metadata gap. See docs/copilot/header-reports/mfvirtualcamera.h.md |
| `mgm.h` | RRas | matched |  |  | Investigated; clean, no ownership metadata gap. See docs/copilot/header-reports/mgm.h.md |
| `mgmtapi.h` | Snmp | matched | copilot | 09/03/2026 03:30:00 | Classified retained artifact in existing-patches-18. |
| `mi.h` | Mi | matched | copilot | 09/03/2026 05:15:00 | No patch needed; no ownership-relevant handle type. |
| `micaut.h` | Tablet | matched |  |  | Investigated; clean, no ownership metadata gap. See docs/copilot/header-reports/micaut.h.md |
| `midles.h` | Rpc | matched |  | 09/02/2026 21:32:46 | Enums/function-pointer typedefs only, no extern functions. |
| `mileffects.h` | Wibe | remaining |  |  |  |
| `minappmodel.h` | AppxPackaging | matched |  | 09/02/2026 19:14:35 | Constants only, no functions. |
| `mindumpdef.h` | Debug | matched |  | 09/02/2026 20:56:17 | Constants/enum only, no functions. |
| `minidumpapiset.h` | Base, Debug | remaining |  |  |  |
| `minwinbase.h` | Base, Fs, Ioctl | matched | copilot | 09/03/2026 03:30:00 | Classified retained artifact in existing-patches-18. |
| `minwindef.h` | Foundation, Multimedia | matched | copilot | 09/02/2026 19:18:45 | Closed in shared-handle-policy-01; current windows-rs full test target is blocked by missing PartitionSpec.include_main_file initializers. |
| `mixerocx.h` | Media.DShow | matched |  |  | Investigated; clean, no ownership metadata gap. See docs/copilot/header-reports/mixerocx.h.md |
| `mlang.h` | Intl | remaining |  |  |  |
| `MLOperatorAuthor.h` | MachineLearning | matched |  |  | Investigated; clean, no ownership metadata gap. See docs/copilot/header-reports/MLOperatorAuthor.h.md |
| `mmc.h` | Mmc | remaining |  |  |  |
| `mmcobj.h` | Lwef, Mmc | remaining |  |  |  |
| `mmddk.h` | Audio.DirectMusic, Multimedia | matched |  |  | Investigated; clean, no ownership metadata gap. See docs/copilot/header-reports/mmddk.h.md |
| `mmdeviceapi.h` | Audio | matched | copilot | 09/03/2026 03:30:00 | Classified retained artifact in existing-patches-18. |
| `mmeapi.h` | Audio, Multimedia | matched | copilot | 09/03/2026 03:30:00 | Classified retained artifact in existing-patches-18. |
| `mmiscapi.h` | Multimedia | matched |  |  | Genuine producer-site fix: added CloseApi=CloseDriver to existing incomplete HDRVR autoTypes.json entry (resolves mmsyscom.h batch-107 follow-up). See docs/copilot/header-reports/mmiscapi.h.md |
| `mmiscapi2.h` | Media | matched |  | 09/02/2026 18:15:12 | Timer ID is a UINT, not a HANDLE. |
| `mmreg.h` | Audio.DirectMusic, Media.DShow, Multimedia | matched | copilot | 09/03/2026 03:30:00 | Classified retained artifact in existing-patches-18. |
| `mmstream.h` | Media.DShow | matched |  |  | Investigated; clean, no ownership metadata gap. See docs/copilot/header-reports/mmstream.h.md |
| `mmsyscom.h` | Media | matched |  |  | Investigated; clean, no ownership metadata gap. See docs/copilot/header-reports/mmsyscom.h.md |
| `mmsystem.h` | Audio, Audio.Apo, Audio.DirectMusic, Audio.DirectSound, Audio.Endpoints, Base, Devices.Properties, FileHistory, GamingDvcInfo, Gdi, Hid, IO, Multimedia, WinProg | matched |  | 09/02/2026 19:42:09 | Pure umbrella redirect; all sub-headers already individually tracked. |
| `mobsync.h` | Shell | matched | copilot | 09/03/2026 03:45:00 | Classified retained artifact in existing-patches-19. |
| `mpconfig.h` | Media.DShow | matched |  | 09/02/2026 20:34:23 | COM-style interface methods only, no extern functions. |
| `mpeg2bits.h` | MsTv | matched |  | 09/02/2026 17:48:13 | Bitfield data structs only, no functions. |
| `mpeg2data.h` | MsTv | matched |  |  | Investigated; clean, no ownership metadata gap. See docs/copilot/header-reports/mpeg2data.h.md |
| `mpeg2error.h` | Media.DShow | matched |  | 09/02/2026 19:35:00 | HRESULT constants only, no functions. |
| `mpeg2psiparser.h` | Media.DShow, MsTv | remaining |  |  |  |
| `mpeg2structs.h` | MsTv | matched |  |  | Investigated; clean, no ownership metadata gap. See docs/copilot/header-reports/mpeg2structs.h.md |
| `mpegtype.h` | Media.DShow | matched |  | 09/02/2026 19:21:35 | COM interface with plain-value parameters only, no handle. |
| `mprapi.h` | RRas | remaining |  |  |  |
| `mprerror.h` | Foundation | matched |  | 09/02/2026 21:59:01 | Constants only, no functions. |
| `mq.h` | MessageQueuing | remaining |  |  |  |
| `mqoai.h` | MessageQueuing | remaining |  |  |  |
| `mrmresourceindexer.h` | MenuRc | matched |  | 09/02/2026 22:02:59 | Transparent wrapper struct, not an opaque handle type representable by the annotation mechanism. |
| `msaatext.h` | Tsf | remaining |  |  |  |
| `msacm.h` | Audio | remaining |  |  |  |
| `msacmdlg.h` | Multimedia | matched |  | 09/02/2026 17:27:04 | Dialog resource ID constants only, no functions. |
| `msacmdrv.h` | Audio | matched |  |  | Investigated; clean, no ownership metadata gap. See docs/copilot/header-reports/msacmdrv.h.md |
| `MSAJTransport.h` | AllJoyn, WinRT | blocked | copilot | 09/02/2026 18:15:12 | Two compounding blockers: parser/toolchain version mismatch prevents live-scrape validation of the AllJoyn partition; and a genuine return-value HANDLE ownership pattern with no annotation precedent. |
| `mscat.h` | Security.Cryptography.Catalog, Security.Cryptography.Sip | remaining |  |  |  |
| `mschapp.h` | MsChap | matched |  | 09/02/2026 18:29:38 | Password-encryption struct/string API only, no handle. |
| `msclmd.h` | Security.Cryptography | matched |  | 09/02/2026 19:45:17 | Constants/structs only, no functions. |
| `msclus.h` | MsCs | remaining |  |  |  |
| `mscoree.h` | ClrHosting | matched | copilot | 09/02/2026 23:40:00 | No patch needed; verified via live scrape plus confirmation that global supportedOS.rsp already covers this header's functions. |
| `msctf.h` | Tsf | remaining |  |  |  |
| `msctfmonitorapi.h` | Tsf | matched |  | 09/02/2026 18:35:09 | HANDLE is a caller-supplied input, not produced here. |
| `msdadc.h` | Search | matched |  |  | Investigated; clean, no ownership metadata gap. See docs/copilot/header-reports/msdadc.h.md |
| `msdaguid.h` | Search | matched |  | 09/02/2026 18:06:26 | GUID constants only, no functions. |
| `msdaora.h` | Search | matched |  | 09/02/2026 18:29:38 | CLSID/GUID constants only, no functions. |
| `msdaosp.h` | Search | matched |  | 09/02/2026 20:53:10 | COM/IDispatch interface method only, out of scope. |
| `msdasc.h` | Search | matched |  |  | Investigated; clean, no ownership metadata gap. See docs/copilot/header-reports/msdasc.h.md |
| `msdasql_interfaces.h` | Search | matched |  |  | Investigated; clean, no ownership metadata gap. See docs/copilot/header-reports/msdasql_interfaces.h.md |
| `msdasql.h` | Search | matched |  | 09/02/2026 19:50:38 | GUID/property constants only, no functions. |
| `msdatsrc.h` | Search | matched |  |  | Investigated; clean, no ownership metadata gap. See docs/copilot/header-reports/msdatsrc.h.md |
| `msdelta.h` | Setup | matched |  |  | Investigated; clean, no ownership metadata gap. See docs/copilot/header-reports/msdelta.h.md |
| `msdrm.h` | Rm | matched |  |  | Producer-site fix: added DRMHANDLE/DRMENVHANDLE/DRMHSESSION/DRMQUERYHANDLE/DRMPUBHANDLE autoTypes.json entries. See docs/copilot/header-reports/msdrm.h.md |
| `msdrmdefs.h` | Rm | matched |  | 09/02/2026 22:09:36 | Types/constants only, no functions; DRM handle ownership deferred to msdrm.h. |
| `msdrmerror.h` | Rm | matched |  |  | Investigated; clean, no ownership metadata gap. See docs/copilot/header-reports/msdrmerror.h.md |
| `msdrmgetinfo.h` | Rm | matched |  | 09/02/2026 21:08:27 | String constants only, no functions. |
| `msdshape.h` | Search | matched |  | 09/02/2026 17:51:18 | Constants/enums only, no functions. |
| `msfeeds.h` | Wmp | remaining |  |  |  |
| `msfeedsid.h` | Wmp | matched |  |  | Investigated; clean, no ownership metadata gap. See docs/copilot/header-reports/msfeedsid.h.md |
| `mshtmcid.h` | MsHtml | matched |  |  | Investigated; clean, no ownership metadata gap. See docs/copilot/header-reports/mshtmcid.h.md |
| `mshtmdid.h` | MsHtml | remaining |  |  |  |
| `mshtmhst.h` | MsHtml | remaining |  |  |  |
| `Mshtml.h` | MsHtml | remaining |  |  |  |
| `mshtmlc.h` | MsHtml | remaining |  |  |  |
| `mshtmldiagnostics.h` | MsHtml | matched |  |  | Investigated; clean, no ownership metadata gap. See docs/copilot/header-reports/mshtmldiagnostics.h.md |
| `msi.h` | Setup | remaining |  |  |  |
| `msidefs.h` | Setup | remaining |  |  |  |
| `msiehost.h` | InternetExplorer | matched |  | 09/02/2026 17:51:18 | GUID/command-ID constants only, no functions. |
| `msiltcfg.h` | Setup | matched |  | 09/02/2026 17:38:44 | No handle involved. |
| `msime.h` | Input.Ime | remaining |  |  |  |
| `msimeapi.h` | Input.Ime | matched |  | 09/02/2026 21:52:12 | COM interface only, no extern functions. |
| `msinkaut.h` | Tablet | remaining |  |  |  |
| `msinkaut15.h` | Tablet | matched |  |  | Investigated; clean, no ownership metadata gap. See docs/copilot/header-reports/msinkaut15.h.md |
| `msiquery.h` | Setup | remaining |  |  |  |
| `msopc.h` | Opc | matched | copilot | 09/03/2026 05:15:00 | No patch needed; COM interface only, no free functions. |
| `msports.h` | SerPorts | matched |  | 09/02/2026 21:01:40 | HCOMDB already correctly covered via existing autoTypes.json entry. |
| `msrdc.h` | Rdc | matched | copilot | 09/03/2026 04:35:00 | No patch needed; COM interface only. |
| `mssign.h` | Security.Cryptography | matched |  |  | Investigated; clean, no ownership metadata gap. See docs/copilot/header-reports/mssign.h.md |
| `mssip.h` | Security.Cryptography.Sip, Security.WinTrust | matched | copilot | 09/03/2026 03:45:00 | Classified retained artifact in existing-patches-19. |
| `mstask.h` | TaskSchd | remaining |  |  |  |
| `mstcpip.h` | WinSock | remaining |  |  |  |
| `msvidctl.h` | MsTv | remaining |  |  |  |
| `mswmdm.h` | Wmdm | remaining |  |  |  |
| `mswsock.h` | WinSock | matched | copilot | 09/03/2026 03:45:00 | Classified retained artifact in existing-patches-19. |
| `mswsockdef.h` | WinSock | matched |  | 09/02/2026 19:28:18 | RIO handle types have no scrapable extern/DllImport producer function (function-pointer table pattern), architecturally out of scope. |
| `msxml.h` | FileHistory, MsXml, WinProg | remaining |  |  |  |
| `msxml6.h` | MsXml, Printing | remaining |  |  |  |
| `mtpext.h` | Wmdm | matched |  | 09/02/2026 18:44:05 | Constants + data structs only, no functions. |
| `mtx.h` | ComOle, TransactionServer | matched |  | 09/02/2026 16:49:01 | Redirect-only header (#include comsvcs.h, already accepted-normalized). |
| `mtxadmin.h` | ComOle, TransactionServer | matched | copilot | 09/03/2026 03:45:00 | Classified retained artifact in existing-patches-19. |
| `mtxattr.h` | ComOle, TransactionServer | matched |  | 09/02/2026 18:29:38 | Typelib attribute macros only, no functions. |
| `mtxdm.h` | Cos | matched |  | 09/02/2026 17:05:19 | GetDispenserManager is a COM-factory pattern; comsvcs.h already accepted. |
| `muiload.h` | Intl | matched |  | 09/02/2026 19:54:02 | HINSTANCE/HMODULE already correctly covered via existing autoTypes.json CloseApi=FreeLibrary entry. |
| `mxdc.h` | Gdi, Printing | matched |  | 09/02/2026 20:56:17 | No function produces an opaque handle; HANDLE param is a pre-owned caller input. |
| `namedpipeapi.h` | Pipes | matched | copilot | 09/03/2026 03:45:00 | Classified retained artifact in existing-patches-19. |
| `napmicrosoftvendorids.h` | NetworkAccessProtection | matched |  | 09/02/2026 17:58:11 | Integer constants only, no functions. |
| `naptypes.h` | NetworkAccessProtection | matched |  | 09/02/2026 21:12:30 | Types only, no functions. |
| `natupnp.h` | WindowsFirewall | remaining |  |  |  |
| `nb30.h` | NetBios | matched |  |  | Investigated; clean, no ownership metadata gap. See docs/copilot/header-reports/nb30.h.md |
| `ncrypt_provider.h` | Security.Cryptography | matched |  |  | Investigated; clean, no ownership metadata gap. See docs/copilot/header-reports/ncrypt_provider.h.md |
| `ncrypt.h` | Security.Cryptography, Security.Cryptography.UI | matched | copilot | 09/02/2026 22:10:00 | Corrected typedef-owned ownership violation found during resource-ownership audit; verified compatible with co-resident zz-crypto-security-enums patch via full replay. |
| `ncryptprotect.h` | Security.Cryptography, Security.Cryptography.UI | matched | copilot | 09/02/2026 21:05:00 | Corrected typedef-owned ownership violation found during resource-ownership audit. |
| `ndattrib.h` | Ndf | matched |  | 09/02/2026 21:49:17 | Data structs only, no functions. |
| `ndfapi.h` | Ndf | matched |  | 09/02/2026 20:38:46 | Fixed genuine gap: added new autoTypes.json entry for NDFHANDLE (CloseApi=NdfCloseIncident). |
| `ndhelper.h` | Ndf | matched |  |  | Investigated; clean, no ownership metadata gap. See docs/copilot/header-reports/ndhelper.h.md |
| `ndisguid.h` | Ndis | matched |  |  | Investigated; clean, no ownership metadata gap. See docs/copilot/header-reports/ndisguid.h.md |
| `ndkinfo.h` | Ndis | matched |  | 09/02/2026 18:00:53 | Data structs/enum/constants only, no functions. |
| `ndr64types.h` | Rpc | matched |  |  | Investigated; clean, no ownership metadata gap. See docs/copilot/header-reports/ndr64types.h.md |
| `netcfgn.h` | NetMgmt | remaining |  |  |  |
| `netcfgx.h` | NetMgmt | remaining |  |  |  |
| `netcon.h` | WindowsFirewall | matched | copilot | 09/03/2026 04:00:00 | Classified retained artifact in existing-patches-20. |
| `netevent.h` | NetMgmt | remaining |  |  |  |
| `netfw.h` | WindowsFirewall | matched | copilot | 09/03/2026 04:00:00 | Classified retained artifact in existing-patches-20. |
| `netiodef.h` | WinSock, wnv | remaining |  |  |  |
| `netlistmgr.h` | Nla, WindowsConnectionManager | remaining |  |  |  |
| `netprov.h` | NetMgmt | matched |  |  | Investigated; clean, no ownership metadata gap. See docs/copilot/header-reports/netprov.h.md |
| `netsh.h` | NetShell | matched | copilot | 09/03/2026 05:15:00 | No patch needed; no ownership-relevant handle type. |
| `nettypes.h` | NetMgmt | matched |  | 09/02/2026 18:40:30 | Typedefs/data structs/constants only, no functions. |
| `newdev.h` | DevInst | matched | copilot | 09/03/2026 02:00:00 | Resolves the replay-mismatch flagged in shared-handle-policy-01; verified via isolated round-trip in this session (no fresh SDK checkout available to rerun the original full-replay evidence). |
| `nfcdtadev.h` | Nfc | matched |  |  | Investigated; clean, no ownership metadata gap. See docs/copilot/header-reports/nfcdtadev.h.md |
| `nfcradiodev.h` | Nfc | matched |  | 09/02/2026 17:54:05 | GUID/IOCTL constants + data structs only, no functions. |
| `nfcsedev.h` | Nfc | matched |  | 09/02/2026 21:32:46 | GUID/IOCTL constants and structs only, no functions. |
| `nfpdev.h` | Nfp | matched | copilot | 09/03/2026 05:00:00 | No patch needed; constant-only header. |
| `notesdeviceservice.h` | WpdSdk | matched |  | 09/02/2026 17:35:31 | GUID/name constants only, no functions. |
| `notificationactivationcallback.h` | Win32_Tile_Badge_Notif | matched |  | 09/02/2026 20:22:05 | COM interface method only, out of scope. |
| `Npapi.h` | Wnet | matched |  |  | Investigated; clean, no ownership metadata gap. See docs/copilot/header-reports/Npapi.h.md |
| `nsemail.h` | WinSock | matched |  | 09/02/2026 20:28:23 | Enums/structs only, no functions. |
| `nserror.h` | Multimedia | remaining |  |  |  |
| `nspapi.h` | WinSock | matched | copilot | 09/03/2026 04:00:00 | Classified retained artifact in existing-patches-20. |
| `ntdd1394.h` | Devices.1394 | matched | copilot | 09/02/2026 23:20:00 | No patch needed; data-structure-only header verified via live scrape. |
| `ntddbeep.h` | Devices.Beep | matched | copilot | 09/03/2026 04:05:00 | No patch needed; data-structure-only header. |
| `ntddcdrm.h` | Devices.Cdrom | matched | copilot | 09/03/2026 03:35:00 | No patch needed; data-structure-only header verified via live scrape. |
| `ntddcdvd.h` | Devices.Dvd | matched | copilot | 09/03/2026 04:05:00 | No patch needed; data-structure-only header. |
| `ntdddisk.h` | Fs | remaining |  |  |  |
| `ntddkbd.h` | Hid | matched |  | 09/02/2026 22:12:28 | Constants only, no functions. |
| `ntddmou.h` | Hid | matched |  | 09/02/2026 20:38:46 | Constants/structs only, no functions. |
| `ntddndis.h` | Ndis, NWifi, Qos | remaining |  |  |  |
| `ntddser.h` | SerPorts | matched |  |  | Investigated; clean, no ownership metadata gap. See docs/copilot/header-reports/ntddser.h.md |
| `ntddstor.h` | Fs, Ioctl | remaining |  |  |  |
| `ntddvdeo.h` | Display | remaining |  |  |  |
| `ntddvol.h` | Fs | matched |  |  | Investigated; clean, no ownership metadata gap. See docs/copilot/header-reports/ntddvol.h.md |
| `ntdef.h` | Kernel | remaining |  |  |  |
| `ntdsapi.h` | ActiveDirectory | remaining |  |  |  |
| `ntdsbmsg.h` | ActiveDirectory | matched |  |  | Investigated; clean, no ownership metadata gap. See docs/copilot/header-reports/ntdsbmsg.h.md |
| `ntiologc.h` | Foundation | remaining |  |  |  |
| `ntioring_x.h` | Fs | matched |  | 09/02/2026 20:38:46 | Enums/constants only, no functions in this types header. |
| `ntldap.h` | Ldap | matched |  |  | Investigated; clean, no ownership metadata gap. See docs/copilot/header-reports/ntldap.h.md |
| `ntmsapi.h` | Fs | matched | copilot | 09/03/2026 04:00:00 | Classified retained artifact in existing-patches-20. |
| `ntmsmli.h` | Fs | matched |  | 09/02/2026 17:45:25 | Data struct + callback-signature typedefs only, no functions. |
| `ntquery.h` | IndexSrv, Shell | matched |  |  | Investigated; clean, no ownership metadata gap. See docs/copilot/header-reports/ntquery.h.md |
| `ntsecapi.h` | ActiveDirectory, Certificates, Identity, Security, Security.AppLocker, Security.ConfigurationSnapin, Security.Cryptography, Security.Cryptography.Catalog, Security.Cryptography.Sip, Security.Cryptography.UI, Security.DiagnosticDataQuery, Security.DirectoryServices, Security.LicenseProtection, Security.Tpm, Security.WinTrust, Security.WinWlx | matched | copilot | 09/02/2026 21:05:00 | Corrected typedef-owned LSA_HANDLE ownership violation found during resource-ownership audit; other annotation groups in file were already compliant. |
| `NTSecPKG.h` | Certificates, Identity, Security, Security.AppLocker, Security.ConfigurationSnapin, Security.Cryptography, Security.Cryptography.Catalog, Security.Cryptography.Sip, Security.Cryptography.UI, Security.DiagnosticDataQuery, Security.DirectoryServices, Security.LicenseProtection, Security.Tpm, Security.WinTrust, Security.WinWlx | matched | copilot | 09/02/2026 20:35:00 | Audited under corrected shared-handle policy (165b5f09, 7335ddc4); already compliant, no code changes required. |
| `ntstatus.h` | Foundation | remaining |  |  |  |
| `nvme.h` | Storage.Nvme | matched | copilot | 09/03/2026 04:35:00 | No patch needed; data-structure-only header. |
| `oaidl.h` | ComOle | matched | copilot | 09/03/2026 04:00:00 | Classified retained artifact in existing-patches-20. |
| `objbase.h` | Base, Com, Com.CallObj, Com.ChannelCreds, Com.StructuredStorage, Com.Urlmon, ComOle, FileHistory, IO, Printing, TransactionServer, VSS, WinProg | matched | copilot | 09/03/2026 04:15:00 | Classified retained artifact in existing-patches-21. |
| `objectarray.h` | Shell, Shell.Common | matched | copilot | 09/03/2026 04:15:00 | Classified retained artifact in existing-patches-21. |
| `ObjIdl.h` | Base, Com, Com.CallObj, Com.ChannelCreds, Com.StructuredStorage, Com.Urlmon, ComOle, IO, Rpc, TransactionServer, VSS | matched | copilot | 09/03/2026 04:15:00 | Classified retained artifact in existing-patches-21. |
| `objidlbase.h` | TransactionServer | remaining |  |  |  |
| `objsafe.h` | Debug | matched |  | 09/02/2026 21:27:04 | COM interface method only, out of scope. |
| `objsel.h` | ActiveDirectory | matched |  |  | Investigated; clean, no ownership metadata gap. See docs/copilot/header-reports/objsel.h.md |
| `ocidl.h` | Com, Com.CallObj, Com.ChannelCreds, Com.Urlmon, ComOle, Media.DShow, Search, TransactionServer | matched | copilot | 09/03/2026 04:15:00 | Classified retained artifact in existing-patches-21. |
| `ocmm.h` | InternetExplorer | remaining |  |  |  |
| `odbcinst.h` | Search | matched |  |  | Investigated; clean, no ownership metadata gap. See docs/copilot/header-reports/odbcinst.h.md |
| `odbcss.h` | Search | matched |  |  | Investigated; clean, no ownership metadata gap. See docs/copilot/header-reports/odbcss.h.md |
| `ole2.h` | Com, Com.CallObj, Com.ChannelCreds, Com.StructuredStorage, Com.Urlmon, ComOle, TransactionServer | matched | copilot | 09/03/2026 04:15:00 | Classified retained artifact in existing-patches-21. |
| `oleacc.h` | WinAuto | matched | copilot | 09/03/2026 04:30:00 | Classified retained artifact in existing-patches-22. |
| `oleauto.h` | ComOle | matched | copilot | 09/03/2026 04:30:00 | Classified retained artifact in existing-patches-22. |
| `olectl.h` | Com, Com.CallObj, Com.ChannelCreds, Com.Urlmon, ComOle, TransactionServer | matched | copilot | 09/03/2026 04:30:00 | Classified retained artifact in existing-patches-22. |
| `oledbdep.h` | Search | remaining |  |  |  |
| `oledberr.h` | Search | remaining |  |  |  |
| `oledlg.h` | Com, Com.CallObj, Com.ChannelCreds, Com.Urlmon, ComOle, TransactionServer | matched | copilot | 09/03/2026 04:30:00 | Classified retained artifact in existing-patches-22. |
| `oleidl.h` | Com, Com.CallObj, Com.ChannelCreds, Com.Urlmon, ComOle, Shell, TransactionServer | matched | copilot | 09/03/2026 04:30:00 | Classified retained artifact in existing-patches-22. |
| `oletx2xa.h` | DTC | matched |  |  | Investigated; clean, no ownership metadata gap. See docs/copilot/header-reports/oletx2xa.h.md |
| `ondemandconnroutehelper.h` | Nla, WindowsConnectionManager | blocked |  | 09/02/2026 19:45:17 | OnDemandRegisterNotification outputs generic HANDLE via direct out-param. |
| `oobenotification.h` | WindowsSetupAndMigration | matched | copilot | 09/02/2026 22:05:00 | Classified retained artifact in existing-patches-23. |
| `openservice.h` | InternetExplorer | remaining |  |  |  |
| `opmapi.h` | Mf | matched |  |  | Investigated; clean, no ownership metadata gap. See docs/copilot/header-reports/opmapi.h.md |
| `opmxbox.h` | Mf | matched |  | 09/02/2026 17:23:42 | Enum-output-only API, no handle. |
| `p2p.h` | P2p | remaining |  |  |  |
| `packagevirtualizationcontext.h` | AppxPackaging | matched |  | 09/02/2026 18:56:34 | Genuine ownership gap fixed: CreatePackageVirtualizationContext/DuplicatePackageVirtualizationContext out-params now carry _Win32_metadata_invalid_handle_/_raii_free_, releasing via ReleasePackageVirtualizationContext. GetCurrentPackageVirtualizationContext is a borrowed/non-owned query (like GetConsoleWindow), left unannotated correctly. GetProcessesInVirtualizationContext's HANDLE** array is the already-documented generic-HANDLE-array limitation (same as physicalmonitorenumerationapi.h), not a new blocker. Verified via live re-scrape (0 errors) and git apply --check --reverse. |
| `pacmanclientapi.h` | Setup | remaining |  |  |  |
| `patchapi.h` | Setup | matched |  |  | Investigated; clean, no ownership metadata gap. See docs/copilot/header-reports/patchapi.h.md |
| `patchwiz.h` | Setup | matched |  |  | Investigated; clean, no ownership metadata gap. See docs/copilot/header-reports/patchwiz.h.md |
| `pathcch.h` | Shell | matched | copilot | 09/02/2026 22:05:00 | Classified retained artifact in existing-patches-23. |
| `pbdaerrors.h` | Media.DShow | matched |  | 09/02/2026 20:13:28 | HRESULT constants/macros only, no functions. |
| `pchannel.h` | TermServ | matched |  | 09/02/2026 21:59:01 | Constants/transparent structs only, no functions. |
| `pdh.h` | Perf | remaining |  |  |  |
| `pdhmsg.h` | Perf | matched |  | 09/02/2026 20:16:15 | Message-compiler constants only, no functions. |
| `peerdist.h` | P2p | matched |  |  | Genuine producer-site fix: added 4 autoTypes.json entries (PEERDIST_INSTANCE_HANDLE/STREAM/CONTENTINFO/CONTENT_HANDLE). See docs/copilot/header-reports/peerdist.h.md |
| `peninputpanel.h` | Tablet | remaining |  |  |  |
| `perceptiondevicecore.h` | MixedReality | matched |  | 09/02/2026 18:23:41 | Data structs + GUID constant only, no functions. |
| `perflib.h` | Perf | remaining |  |  |  |
| `perhist.h` | InternetExplorer | matched |  | 09/02/2026 21:04:56 | COM interface method only, out of scope. |
| `persist.h` | Search | matched |  | 09/02/2026 18:06:26 | Constants only, no functions. |
| `photoacquire.h` | PicAcq, Setup | matched |  | 09/02/2026 16:34:06 | All functions are MIDL RPC marshalling stubs (BSTR/HBITMAP/HICON/HWND/LPSAFEARRAY_User*); pass-through of externally-owned handles only. |
| `physicalmonitorenumerationapi.h` | Monitor | blocked | copilot | 09/02/2026 18:44:05 | Deferred from lowlevelmonitorconfigurationapi.h (batch 30); genuine ownership relationship but generic HANDLE field nested in struct array precludes producer-site annotation, same root class as resourceindexer.h. |
| `pla.h` | Pla | matched |  | 09/02/2026 16:34:06 | All 6 functions operate on strings/flags/paths only; no handle-producing functions. |
| `playlist.h` | Media.DShow | matched |  |  | Investigated; clean, no ownership metadata gap. See docs/copilot/header-reports/playlist.h.md |
| `playsoundapi.h` | Audio | matched | copilot | 09/02/2026 22:05:00 | Classified retained artifact in existing-patches-23. |
| `playto.h` | Mf | matched |  | 09/02/2026 17:51:18 | Pure MIDL boilerplate; real content in separately-tracked MFSharingEngine.h (pending). |
| `PlayToManagerInterop.h` | WinRT | matched |  | 09/02/2026 21:21:09 | COM/WinRT interop interface method only, out of scope. |
| `pluginauthenticator.h` | WebAuthn | matched |  |  | Investigated; clean, no ownership metadata gap. See docs/copilot/header-reports/pluginauthenticator.h.md |
| `pnpxassoc.h` | FunctionDiscovery, WebServicesOnDevices | matched |  |  | Investigated; clean, no ownership metadata gap. See docs/copilot/header-reports/pnpxassoc.h.md |
| `poclass.h` | Power | matched |  |  | Investigated; clean, no ownership metadata gap. See docs/copilot/header-reports/poclass.h.md |
| `portabledevice.h` | WpdSdk | remaining |  |  |  |
| `portabledeviceapi.h` | WpdSdk | remaining |  |  |  |
| `portabledeviceconnectapi.h` | WpdSdk | matched |  |  | Investigated; clean, no ownership metadata gap. See docs/copilot/header-reports/portabledeviceconnectapi.h.md |
| `portabledevicetypes.h` | WpdSdk | remaining |  |  |  |
| `powerbase.h` | Power | matched | copilot | 09/02/2026 22:05:00 | Classified retained artifact in existing-patches-23. |
| `powersetting.h` | Power | blocked |  | 09/02/2026 19:42:09 | PowerRegisterForEffectivePowerModeNotifications outputs generic PVOID via direct out-param; HPOWERNOTIFY portion already covered by autoTypes.json. |
| `powrprof.h` | Power | matched | copilot | 09/02/2026 22:50:00 | Classified retained artifact in existing-patches-26. |
| `prcomoem.h` | Printing | remaining |  |  |  |
| `prdrvcom.h` | Printing | matched |  | 09/02/2026 20:34:23 | COM interface methods only, out of scope. |
| `presentation.h` | CompositionSwapchain | remaining |  |  |  |
| `presentationtypes.h` | CompositionSwapchain | matched |  | 09/02/2026 18:06:26 | Data structs/enum/typedef + MIDL boilerplate only, no functions. |
| `Print3DManagerInterop.h` | WinRT.Printing | matched |  | 09/02/2026 21:43:41 | COM/WinRT interop interface method only, out of scope. |
| `PrinterExtension.h` | Printing | remaining |  |  |  |
| `printerextensiondispid.h` | Printing | matched |  |  | Investigated; clean, no ownership metadata gap. See docs/copilot/header-reports/printerextensiondispid.h.md |
| `PrintManagerInterop.h` | WinRT.Printing | matched |  | 09/02/2026 21:32:46 | COM/WinRT interop interface method only, out of scope. |
| `printoem.h` | Printing | remaining |  |  |  |
| `printpreview.h` | Printing | matched |  | 09/02/2026 20:49:21 | COM interface method only, out of scope. |
| `prnasnot.h` | Gdi, Printing | blocked |  | 09/02/2026 20:44:40 | RegisterForPrintAsyncNotifications produces generic HANDLE via direct out-param. |
| `prnasntp.h` | Printing | matched | copilot | 09/02/2026 22:20:00 | Classified retained artifact in existing-patches-24. |
| `prntfont.h` | Printing | matched |  |  | Investigated; clean, no ownership metadata gap. See docs/copilot/header-reports/prntfont.h.md |
| `prntvpt.h` | Gdi, PrintTicket | matched | copilot | 09/02/2026 23:20:00 | New resource-ownership patch created and verified via live re-scrape (build-level validation); supported-os left unresolved due to ambiguous/uncorroborated documentation. |
| `processenv.h` | Environment, FileHistory, Fs | matched | copilot | 09/02/2026 22:20:00 | Classified retained artifact in existing-patches-24. |
| `processsnapshot.h` | Proc_Snap | matched |  |  | Genuine producer-site fix: added CloseApi=PssFreeSnapshot to existing incomplete HPSS autoTypes.json entry. See docs/copilot/header-reports/processsnapshot.h.md |
| `processthreadsapi.h` | FileHistory, TermServ | matched | copilot | 09/02/2026 22:20:00 | Classified retained artifact in existing-patches-24. |
| `profileapi.h` | Perf | matched | copilot | 09/02/2026 22:20:00 | Classified retained artifact in existing-patches-24. |
| `profinfo.h` | Shell | matched |  | 09/02/2026 18:56:34 | Struct-only header; real gap belongs to UserEnv.h (reclassified blocked). |
| `projectedfslib.h` | ProjFs | matched |  |  | Genuine producer-site fix: added CloseApi=PrjStopVirtualizing to existing incomplete PRJ_NAMESPACE_VIRTUALIZATION_CONTEXT autoTypes.json entry. See docs/copilot/header-reports/projectedfslib.h.md |
| `proofofpossessioncookieinfo.h` | WinInet | matched |  |  | Investigated; clean, no ownership metadata gap. See docs/copilot/header-reports/proofofpossessioncookieinfo.h.md |
| `propapi.h` | Com.StructuredStorage | matched | copilot | 09/02/2026 22:20:00 | Classified retained artifact in existing-patches-24. |
| `propidl.h` | Com.StructuredStorage, Properties, Shell | matched | copilot | 09/02/2026 22:35:00 | Classified retained artifact in existing-patches-25. |
| `propidlbase.h` | Com.StructuredStorage | matched | copilot | 09/02/2026 22:35:00 | Classified retained artifact in existing-patches-25. |
| `propkeydef.h` | Console, Shell, WinContacts, WpdSdk | matched |  | 09/02/2026 17:30:54 | Macros/operators only, no functions. |
| `propsys.h` | Audio.DirectMusic, Properties, Shell | matched | copilot | 09/02/2026 22:35:00 | Classified retained artifact in existing-patches-25. |
| `propvarutil.h` | Com.StructuredStorage, Variant | matched | copilot | 09/02/2026 22:35:00 | Classified retained artifact in existing-patches-25. |
| `prsht.h` | Controls | matched | copilot | 09/02/2026 22:35:00 | Classified retained artifact in existing-patches-25. |
| `psapi.h` | PsApi1, PsApi2 | matched | copilot | 09/02/2026 22:50:00 | Classified retained artifact in existing-patches-26. |
| `pwm.h` | PWM | matched | copilot | 09/02/2026 23:20:00 | No patch needed; data-structure-only header verified via live scrape. |
| `qmgr.h` | Bits | remaining |  |  |  |
| `qnetwork.h` | Media.DShow | matched |  |  | Investigated; clean, no ownership metadata gap. See docs/copilot/header-reports/qnetwork.h.md |
| `qos2.h` | Qos | remaining |  |  |  |
| `qospol.h` | Qos | matched |  | 09/02/2026 17:48:13 | Data struct + constants only, no functions. |
| `qossp.h` | Qos | matched |  |  | Investigated; clean, no ownership metadata gap. See docs/copilot/header-reports/qossp.h.md |
| `query.h` | Search | matched |  |  | Investigated; clean, no ownership metadata gap. See docs/copilot/header-reports/query.h.md |
| `radialcontrollerinterop.h` | InputRadial | matched |  |  | Investigated; clean, no ownership metadata gap. See docs/copilot/header-reports/radialcontrollerinterop.h.md |
| `radiomgr.h` | WpdSdk | matched |  |  | Investigated; clean, no ownership metadata gap. See docs/copilot/header-reports/radiomgr.h.md |
| `ras.h` | RRas | remaining |  |  |  |
| `rasdlg.h` | RRas | matched |  | 09/02/2026 21:12:30 | No function produces an opaque handle. |
| `raseapif.h` | Eap | matched |  |  | Investigated; clean, no ownership metadata gap. See docs/copilot/header-reports/raseapif.h.md |
| `raserror.h` | RRas | remaining |  |  |  |
| `rasshost.h` | RRas | matched | copilot | 09/02/2026 22:50:00 | Classified retained artifact in existing-patches-26. |
| `ratings.h` | InternetExplorer | blocked | copilot | 09/02/2026 19:09:17 | Deferred: direct out-param instance of the generic/shared-type blocker class. |
| `rdpappcontainerclient.h` | TermServ | remaining |  |  |  |
| `rdpencomapi.h` | Rdp, TermServ | matched | copilot | 09/03/2026 05:00:00 | No patch needed; COM interface only. |
| `realtimeapiset.h` | Base | matched |  | 09/02/2026 18:59:13 | HANDLE is a caller-supplied input, not produced here. |
| `reason.h` | FileHistory, Shutdown | matched |  | 09/02/2026 19:50:38 | Constants only, no functions. |
| `recapis.h` | Tablet | matched | copilot | 09/02/2026 22:50:00 | Classified retained artifact in existing-patches-26. |
| `recdefs.h` | Tablet | matched |  |  | Investigated; clean, no ownership metadata gap. See docs/copilot/header-reports/recdefs.h.md |
| `reconcil.h` | Lwef, Shell | matched | copilot | 09/02/2026 22:50:00 | Classified retained artifact in existing-patches-26. |
| `rectypes.h` | Tablet | matched |  |  | Investigated; clean, no ownership metadata gap. See docs/copilot/header-reports/rectypes.h.md |
| `regbag.h` | MsTv | matched |  | 09/02/2026 20:16:15 | COM interface method only, out of scope. |
| `regstr.h` | Registry | remaining |  |  |  |
| `relogger.h` | Etw | matched |  |  | Investigated; clean, no ownership metadata gap. See docs/copilot/header-reports/relogger.h.md |
| `remotesystemadditionalinfo.h` | TermServ | matched |  | 09/02/2026 20:41:40 | COM interface method only, out of scope. |
| `RemoteSystemsInterop.h` | WinRT | matched |  | 09/02/2026 21:08:27 | COM/WinRT interface method only, out of scope. |
| `rend.h` | Tapi3 | remaining |  |  |  |
| `rendezvoussession.h` | RemoteAssist | matched | copilot | 09/03/2026 05:15:00 | No patch needed; no function surface. |
| `resapi.h` | MsCs | remaining |  |  |  |
| `resourceindexer.h` | MenuRc | blocked | copilot | 09/02/2026 17:23:42 | Deferred: genuine ownership pair uses generic PVOID rather than a distinct handle typedef; needs a design decision to introduce a named type before an annotation fix is possible. |
| `restartmanager.h` | RstMgr | matched | copilot | 09/03/2026 04:35:00 | No patch needed; no ownership-relevant handle type. |
| `restrictederrorinfo.h` | WinRT | matched |  |  | Investigated; clean, no ownership metadata gap. See docs/copilot/header-reports/restrictederrorinfo.h.md |
| `richedit.h` | Controls.RichEdit | matched | copilot | 09/03/2026 00:00:00 | Classified retained artifact in existing-patches-27. |
| `richole.h` | Controls.RichEdit | matched | copilot | 09/03/2026 00:00:00 | Classified retained artifact in existing-patches-27. |
| `ringtonedeviceservice.h` | WpdSdk | matched |  | 09/02/2026 17:51:18 | GUID/name constants only, no functions (same pattern as notesdeviceservice.h). |
| `roapi.h` | TransactionServer, WinRT | matched |  | 09/02/2026 21:17:15 | Fixed two genuine gaps: completed autoTypes.json entries for RO_REGISTRATION_COOKIE and APARTMENT_SHUTDOWN_REGISTRATION_COOKIE. |
| `robuffer.h` | WinRT | matched |  | 09/02/2026 17:13:02 | COM-factory (RoGetBufferMarshaler) + buffer-view pattern (IBufferByteAccess), not HANDLE-family. |
| `roerrorapi.h` | WinRT | matched |  |  | Investigated; clean, no ownership metadata gap. See docs/copilot/header-reports/roerrorapi.h.md |
| `rometadata.h` | WinRT.Metadata | matched |  | 09/02/2026 17:01:48 | COM QueryInterface-style factory (MetaDataGetDispenser), no raw HANDLE. |
| `rometadataapi.h` | WinRT.Metadata | remaining |  |  |  |
| `rometadataresolution.h` | WinRT.Metadata | matched |  | 09/02/2026 19:12:01 | COM factory pattern + HSTRING outputs tracked separately under winstring.h. |
| `roparameterizediid.h` | WinRT.Metadata | matched |  | 09/02/2026 19:19:02 | ROPARAMIIDHANDLE ownership already correctly captured via existing autoTypes.json entry. |
| `roregistrationapi.h` | WinRT | matched |  | 09/02/2026 19:06:16 | COM factory; HSTRING array ownership tracked separately under winstring.h. |
| `rpc.h` | Rpc | matched |  | 09/02/2026 20:07:23 | Pure umbrella/typedef header; all sub-headers already tracked; Mac-only function unreachable on Windows. |
| `rpcasync.h` | Rpc | matched |  |  | Investigated; clean, no ownership metadata gap. See docs/copilot/header-reports/rpcasync.h.md |
| `rpcdce.h` | Rpc, TransactionServer | remaining |  |  |  |
| `rpcdcep.h` | Rpc | matched |  |  | Investigated; clean, no ownership metadata gap. See docs/copilot/header-reports/rpcdcep.h.md |
| `rpcndr.h` | FileHistory, Rpc, WinProg | remaining |  |  |  |
| `rpcnsi.h` | Rpc | matched |  |  | Investigated; clean, no ownership metadata gap. See docs/copilot/header-reports/rpcnsi.h.md |
| `rpcnsip.h` | Rpc | matched |  | 09/02/2026 17:41:54 | Internal I_-prefixed RPC stub routines; RPC_BINDING_HANDLE canonical API tracked separately under rpcdce.h (pending). |
| `rpcnterr.h` | Foundation | matched |  |  | Investigated; clean, no ownership metadata gap. See docs/copilot/header-reports/rpcnterr.h.md |
| `rpcssl.h` | Rpc | matched |  | 09/02/2026 17:45:25 | RPC string-buffer output, not HANDLE-family. |
| `rrascfg.h` | Eap | remaining |  |  |  |
| `rtccore.h` | RealTimeCommunications | matched | copilot | 09/03/2026 03:50:00 | No patch needed; COM interface only. |
| `rtcerr.h` | RealTimeCommunications | matched | copilot | 09/03/2026 03:50:00 | No patch needed; constant-only header. |
| `rtinfo.h` | NetMgmt | matched |  | 09/02/2026 21:24:15 | Structs only, no functions. |
| `rtmv2.h` | RRas | matched | copilot | 09/03/2026 00:00:00 | Classified retained artifact in existing-patches-27. |
| `rtscom.h` | Tablet | remaining |  |  |  |
| `rtutils.h` | NetMgmt | matched |  |  | Investigated; clean, no ownership metadata gap. See docs/copilot/header-reports/rtutils.h.md |
| `rtworkq.h` | Threading | blocked |  | 09/02/2026 20:19:39 | RtwqJoinWorkQueue/RtwqSetDeadline produce generic HANDLE via direct out-param. |
| `sapiddk.h` | Speech | remaining |  |  |  |
| `sas.h` | Identity | matched |  | 09/02/2026 16:53:36 | SendSAS(BOOL) - no handle involved. |
| `sbe.h` | MsTv | remaining |  |  |  |
| `sbtsv.h` | TermServ | remaining |  |  |  |
| `scarderr.h` | Credentials | matched |  |  | Investigated; clean, no ownership metadata gap. See docs/copilot/header-reports/scarderr.h.md |
| `scclient.h` | Wmdm | matched |  | 09/02/2026 18:32:46 | Pure C++ class, no extern/DllImport functions; out of scope for annotation mechanism. |
| `scesvc.h` | Security.ConfigurationSnapin | matched |  |  | Investigated; clean, no ownership metadata gap. See docs/copilot/header-reports/scesvc.h.md |
| `schannel.h` | Certificates, Identity, Security, Security.AppLocker, Security.ConfigurationSnapin, Security.Cryptography, Security.Cryptography.Catalog, Security.Cryptography.Sip, Security.Cryptography.UI, Security.DiagnosticDataQuery, Security.DirectoryServices, Security.LicenseProtection, Security.Tpm, Security.WinTrust, Security.WinWlx, Wsw | matched | copilot | 09/03/2026 00:00:00 | Classified retained artifact in existing-patches-27. |
| `schedule.h` | ActiveDirectory | matched |  | 09/02/2026 18:26:39 | Constants + data structs only, no functions. |
| `schnlsp.h` | Identity | matched |  | 09/02/2026 17:01:48 | Redirect-only; schannel.h already accepted-normalized. |
| `scrnsave.h` | Shell | matched | copilot | 09/03/2026 00:00:00 | Classified retained artifact in existing-patches-27. |
| `sddl.h` | Authorization, Authorization.UI | matched | copilot | 09/03/2026 00:15:00 | Classified retained artifact in existing-patches-28. |
| `sdkddkver.h` | Antimalware, ApplicationVerifier, AppxPackaging, Audio, Audio.Apo, Audio.DirectMusic, Audio.DirectSound, Audio.Endpoints, Authorization, Authorization.UI, Backup, Base, Bits, Bluetooth, Buses, Cabinets, CEIP, Certificates, Cloudapi, ClrHosting, ClrProfiling, CmpApi, Com, Com.CallObj, Com.ChannelCreds, Com.Events, Com.StructuredStorage, Com.Urlmon, Communication, ComOle, CompositionSwapchain, Console, Controls, Controls.RichEdit, Cos, Credentials, DataXchg, Debug, Debug.ActiveScript, Debug.Extensions, Debug.WebApp, Dedup, DeliveryOptimization, DeviceAccess, DeviceQuery, Devices.1394, Devices.Beep, Devices.Cdrom, Devices.Dvd, Devices.Properties, DevInst, DevLic, Dfs, Dhcp, Direct2D, Direct2D.Common, Direct3D, Direct3D10, Direct3D11, Direct3D11on12, Direct3D12, Direct3D9, Direct3D9on12, Direct3DDxc, Direct3DFxc, DirectComp, DirectDraw, DirectManipulation, DirectML, DirectWrite, Display, DlgBox, Dlna, Dns, DTC, Dwm, DXCore, Dxgi, Dxgi.Common, Eap, EapHost, Edp, Enstor, Environment, Etw, Fax, FileHistory, Foundation, Fs, Fsrm, FunctionDiscovery, GameInput, GameMode, GamingDvcInfo, Gdi, Gdiplus, Gpmc, Hid, HostComputeNetwork, HostComputeSystem, HtmlHelp, Hypervisor, Identity, IdentityProvider, Ifsk, Iis, ImagingDevice, IMapi, IndexSrv, Input_Ink, Input_IntContext, Input.Ime, InputRadial, InternetConnectionWizard, InternetExplorer, Intl, IO, Ioctl, IScsiDisc, Isolation, JobObjects, Js, KeyboardAndMouseInput, Ldap, LibraryLoader, Lwef, MachineLearning, MagApi, Mapi, Mbn, MdmReg, Media, Media.DShow, Media.DShow.Xml, Media.DxMediaObjects, Media.KernelStreaming, Memory, MenuRc, MessageQueuing, Mf, Mi, MixedReality, MixedRealityInterop, Mmc, Monitor, MsChap, MsCs, MsHtml, MsTv, MsXml, Multimedia, Ndf, Ndis, NetBios, NetMgmt, NetShell, NetworkAccessProtection, Nfc, Nfp, Nla, Nps, NWifi, Of, Opc, OpenGL, Parcon, Perf, PicAcq, Pipes, Pla, Policy, Power, Printing, PrintTicket, Proc_Snap, ProjFs, Properties, PsApi1, PsApi2, PWM, Rdc, Rdp, RemoteAssist, Rm, Rpc, RRas, RstMgr, Search, Security, Security.AppLocker, Security.ConfigurationSnapin, Security.Cryptography, Security.Cryptography.Catalog, Security.Cryptography.Sip, Security.Cryptography.UI, Security.DiagnosticDataQuery, Security.DirectoryServices, Security.LicenseProtection, Security.Tpm, Security.WinTrust, Security.WinWlx, SecurityCenter, Sens, SerPorts, Services, Setup, Shell, Shell.Common, Smi, Snmp, Speech, Sr, Storage.Jet, Storage.Nvme, SwDevice, SystemInformation, Tablet, Tapi3, TaskSchd, Tbs, Tcui, TermServ, Threading, Time, ToolHelp, TransactionServer, Tsf, UiAnimation, UpdateAssessment, Upnp, Variant, Vhd, VirtualDiskService, VirtualDOSMachines, VSS, Wab, Wcm, Wcn, Wcs, Wds, WebAuthn, WebDav, WebServicesOnDevices, WebSock, Wec, Wer, Wes, Wia, Wibe, Wic, Win32_Tile_Badge_Notif, WinAuto, WinContacts, WindowsConnectionManager, WindowsFilteringPlatform, WindowsFirewall, WindowsRibbon, WindowsSetupAndMigration, WinInet, WinLocation, WinProg, WinRm, WinRT, WinRT.AllJoyn, WinRT.Composition, WinRT.CoreInputView, WinRT.Direct2D, WinRT.Direct3D11, WinRT.Display, WinRT.Graphics.Capture, WinRT.Graphics.Imaging, WinRT.Holographic, WinRT.Isolation, WinRT.Media, WinRT.Metadata, WinRT.ML, WinRT.Pdf, WinRT.Printing, WinRT.Storage, WinRT.Xaml, WinSat, WinSensors, WinSync, WinTouch, Wmdm, WmFormat, Wmi, Wmlss, Wmp, Wnet, WpdSdk, Wsb, Wsl, Wsw, Wua, Xaml_Diagnostics, Xaudio2, Xblidp, Xinput, XmlLite, Xps, XpsPrinting | matched |  |  | Investigated; clean, no ownership metadata gap. See docs/copilot/header-reports/sdkddkver.h.md |
| `sdoias.h` | Nps | remaining |  |  |  |
| `search.h` | Search | matched |  | 09/02/2026 16:49:01 | Redirect-only header (#include corecrt_search.h); CRT-only, no Win32 declarations. |
| `searchapi.h` | Lwef, Search | remaining |  |  |  |
| `security.h` | Identity | matched |  | 09/02/2026 18:09:40 | Redirect-only; sspi.h accepted, issper16.h tracked pending, secext.h (untracked) directly verified clean (string-buffer-output only). |
| `securityappcontainer.h` | Isolation | matched | copilot | 09/03/2026 00:15:00 | Classified retained artifact in existing-patches-28. |
| `securitybaseapi.h` | Etw, Identity, Security | matched | copilot | 09/02/2026 20:35:00 | Audited under corrected shared-handle policy (165b5f09, 7335ddc4); already compliant, no code changes required. |
| `segment.h` | Media.DShow, MsTv | remaining |  |  |  |
| `sens.h` | Sens | matched |  | 09/02/2026 18:20:58 | Constants only, no functions. |
| `sensapi.h` | Sens | matched |  | 09/02/2026 17:41:54 | No handle involved. |
| `sensevts.h` | Sens | matched |  |  | Investigated; clean, no ownership metadata gap. See docs/copilot/header-reports/sensevts.h.md |
| `sensors.h` | WinSensors | remaining |  |  |  |
| `sensorsapi.h` | WinLocation, WinSensors | remaining |  |  |  |
| `sensorsdef.h` | WinSensors | remaining |  |  |  |
| `sensorsstructures.h` | WinSensors | matched |  | 09/02/2026 18:00:52 | Data structs/enum only, no functions. |
| `sensorsutils.h` | WinSensors | matched |  | 09/02/2026 21:55:32 | No function produces an opaque handle. |
| `sessdirpublictypes.h` | TermServ | matched |  |  | Investigated; clean, no ownership metadata gap. See docs/copilot/header-reports/sessdirpublictypes.h.md |
| `setupapi.h` | DevInst, Setup | matched | copilot | 09/03/2026 00:15:00 | Classified retained artifact in existing-patches-28. |
| `sfc.h` | Setup | matched | copilot | 09/03/2026 00:15:00 | Classified retained artifact in existing-patches-28. |
| `shappmgr.h` | Shell | matched | copilot | 09/03/2026 00:15:00 | Classified retained artifact in existing-patches-28. |
| `sharewindowcommandsourceinterop.h` | WinRT | matched |  | 09/02/2026 21:55:32 | COM/WinRT interop interface methods only, out of scope. |
| `shcore.h` | WinRT | matched |  | 09/02/2026 17:54:05 | COM factory pattern. |
| `shdeprecated.h` | Shell | matched | copilot | 09/03/2026 00:30:00 | Classified retained artifact in existing-patches-29. |
| `shdispid.h` | Shell | matched |  | 09/02/2026 19:14:35 | DISPID constants only, no functions. |
| `shellapi.h` | Properties, Shell | matched | copilot | 09/03/2026 00:30:00 | Classified retained artifact in existing-patches-29. |
| `shellscalingapi.h` | Shell | matched | copilot | 09/03/2026 00:30:00 | Classified retained artifact in existing-patches-29. |
| `sherrors.h` | Shell | matched |  |  | Investigated; clean, no ownership metadata gap. See docs/copilot/header-reports/sherrors.h.md |
| `shidfact.h` | Shell | matched |  |  | Investigated; clean, no ownership metadata gap. See docs/copilot/header-reports/shidfact.h.md |
| `shimgdata.h` | Shell | matched | copilot | 09/03/2026 00:30:00 | Classified retained artifact in existing-patches-29. |
| `shldisp.h` | Lwef, Shell | matched | copilot | 09/03/2026 00:30:00 | Classified retained artifact in existing-patches-29. |
| `shlguid.h` | Shell | remaining |  |  |  |
| `shlobj_core.h` | ActiveDirectory, Lwef, Properties, Shell | matched | copilot | 09/03/2026 00:45:00 | Classified retained artifact in existing-patches-30. |
| `shlobj.h` | Controls, Lwef, Shell | matched | copilot | 09/03/2026 00:45:00 | Classified retained artifact in existing-patches-30. |
| `shlwapi.h` | Shell | matched | copilot | 09/03/2026 00:45:00 | Classified retained artifact in existing-patches-30. |
| `shobjidl_core.h` | Properties, Search, Shell | matched | copilot | 09/03/2026 00:45:00 | Classified retained artifact in existing-patches-30. |
| `shobjidl.h` | Properties, Shell, TransactionServer | matched | copilot | 09/03/2026 00:45:00 | Classified retained artifact in existing-patches-30. |
| `shtypes.h` | Shell, Shell.Common | matched |  |  | Investigated; clean, no ownership metadata gap. See docs/copilot/header-reports/shtypes.h.md |
| `simpdata.h` | Search | matched |  |  | Investigated; clean, no ownership metadata gap. See docs/copilot/header-reports/simpdata.h.md |
| `slerror.h` | Identity | remaining |  |  |  |
| `sliddefs.h` | Identity | matched |  | 09/02/2026 16:57:06 | GUID constant only, no functions. |
| `slpublic.h` | Identity | remaining |  |  |  |
| `smbclnt.h` | MsCs | matched |  | 09/02/2026 18:50:10 | HANDLE is a caller-supplied input, not produced here. |
| `smtpguid.h` | IMapi | matched |  | 09/02/2026 21:04:56 | GUID/string constants only, no functions. |
| `snmp.h` | Snmp | matched | copilot | 09/03/2026 01:00:00 | Classified retained artifact in existing-patches-31. |
| `socketapi.h` | WinSock | matched |  | 09/02/2026 16:57:06 | SetSocketMediaStreamingMode(BOOL) - no handle. |
| `softpub.h` | Security.WinTrust | matched |  |  | Investigated; clean, no ownership metadata gap. See docs/copilot/header-reports/softpub.h.md |
| `spatialaudioclient.h` | Audio | remaining |  |  |  |
| `spatialaudiohrtf.h` | Audio | matched |  |  | Investigated; clean, no ownership metadata gap. See docs/copilot/header-reports/spatialaudiohrtf.h.md |
| `spatialaudiometadata.h` | Audio | remaining |  |  |  |
| `spatialinteractionmanagerinterop.h` | MixedRealityInterop | matched |  | 09/02/2026 21:21:09 | COM/WinRT interop interface method only, out of scope. |
| `spellcheck.h` | Intl | remaining |  |  |  |
| `spellcheckprovider.h` | Intl | matched |  |  | Investigated; clean, no ownership metadata gap. See docs/copilot/header-reports/spellcheckprovider.h.md |
| `sperror.h` | Speech | matched |  |  | Investigated; clean, no ownership metadata gap. See docs/copilot/header-reports/sperror.h.md |
| `sporder.h` | WinSock | matched |  | 09/02/2026 17:45:25 | DWORD/GUID array API only, no handle. |
| `sql.h` | Search | matched |  |  | Producer-site fix: added SQLHENV/SQLFreeEnv, SQLHDBC/SQLFreeConnect autoTypes.json entries. SQLHSTMT (SQLFreeStmt requires mandatory 2nd param, unrepresentable) and SQLHDESC (only generic SQLAllocHandle/SQLFreeHandle, blocker-class 2) documented as out of scope. See docs/copilot/header-reports/sql.h.md |
| `sqlext.h` | Search | remaining |  |  |  |
| `sqloledb.h` | Search | matched |  |  | Investigated; clean, no ownership metadata gap. See docs/copilot/header-reports/sqloledb.h.md |
| `sqlspi.h` | Search | matched |  | 09/02/2026 21:27:04 | No function produces the generic ODBC handle type; all reference it as pre-owned input. |
| `sqltypes.h` | Search | matched |  | 09/02/2026 21:24:15 | Type definitions only, no functions. |
| `sqlucode.h` | Search | matched |  |  | Investigated; clean, no ownership metadata gap. See docs/copilot/header-reports/sqlucode.h.md |
| `srpapi.h` | Edp | blocked |  | 09/02/2026 19:54:02 | SrpCreateThreadNetworkContext populates HTHREAD_NETWORK_CONTEXT.ThreadContext, a generic HANDLE field. |
| `srrestoreptapi.h` | Sr | matched | copilot | 09/03/2026 01:00:00 | Classified retained artifact in existing-patches-31. |
| `sslprovider.h` | Security.Cryptography | remaining |  |  |  |
| `sspi.h` | Certificates, Identity, Security, Security.AppLocker, Security.ConfigurationSnapin, Security.Cryptography, Security.Cryptography.Catalog, Security.Cryptography.Sip, Security.Cryptography.UI, Security.DiagnosticDataQuery, Security.DirectoryServices, Security.LicenseProtection, Security.Tpm, Security.WinTrust, Security.WinWlx | matched | copilot | 09/02/2026 20:35:00 | Audited under corrected shared-handle policy (165b5f09, 7335ddc4); already compliant, no code changes required. |
| `statehelpers.h` | FileHistory, Registry | matched |  | 09/02/2026 18:09:40 | HKEY parameters are inputs only, not produced here. |
| `statusdeviceservice.h` | WpdSdk | matched |  | 09/02/2026 20:25:02 | Constants only, no functions. |
| `stdint.h` | AllJoyn | remaining |  |  |  |
| `stgprop.h` | Search | matched |  | 09/02/2026 18:26:39 | PROPID/GUID constants only, no functions. |
| `sti.h` | ImagingDevice | matched |  |  | Investigated; clean, no ownership metadata gap. See docs/copilot/header-reports/sti.h.md |
| `stierr.h` | ImagingDevice | matched |  | 09/02/2026 19:32:00 | HRESULT/status-code constants only, no functions. |
| `stireg.h` | ImagingDevice | matched |  | 09/02/2026 19:45:17 | Registry key/property GUID constants only, no functions. |
| `stiusd.h` | ImagingDevice | matched |  | 09/02/2026 21:43:41 | COM interface + GUID constants only, no extern functions. |
| `storageprovider.h` | Shell | matched |  |  | Investigated; clean, no ownership metadata gap. See docs/copilot/header-reports/storageprovider.h.md |
| `storprop.h` | Base | matched |  | 09/02/2026 18:44:05 | HDEVINFO is a consumer-only input from the separate SetupAPI surface. |
| `stralign.h` | FileHistory, WinProg | matched |  |  | Investigated; clean, no ownership metadata gap. See docs/copilot/header-reports/stralign.h.md |
| `stringapiset.h` | Intl | matched |  | 09/02/2026 19:16:41 | String/buffer conversion API only, no handle. |
| `strmif.h` | Media.DShow, Media.KernelStreaming, MsTv, WmFormat | remaining |  |  |  |
| `strsafe.h` | MenuRc | remaining |  |  |  |
| `structuredquery.h` | Search | remaining |  |  |  |
| `structuredquerycondition.h` | Search | matched |  |  | Investigated; clean, no ownership metadata gap. See docs/copilot/header-reports/structuredquerycondition.h.md |
| `subauth.h` | Identity | matched | copilot | 09/03/2026 01:00:00 | Classified retained artifact in existing-patches-31. |
| `subscriptionservices.h` | Wmp | remaining |  |  |  |
| `subsmgr.h` | Search | remaining |  |  |  |
| `svcguid.h` | WinSock | matched |  |  | Investigated; clean, no ownership metadata gap. See docs/copilot/header-reports/svcguid.h.md |
| `swdevice.h` | SwDevice | matched |  | 09/02/2026 18:38:01 | HSWDEVICE ownership already correctly captured via existing autoTypes.json entry (CloseApi: SwDeviceClose); no gap to fix. |
| `swdevicedef.h` | SwDevice | matched |  | 09/02/2026 17:30:54 | Type declarations only (enums/struct), no functions; HSWDEVICE ownership tracked separately under swdevice.h (pending). |
| `syncdeviceservice.h` | WpdSdk | matched |  | 09/02/2026 19:16:41 | GUID/property-key constants only, no functions. |
| `synchronizationerrors.h` | WinSync | matched |  | 09/02/2026 22:12:28 | Constants only, no functions. |
| `syncmgr.h` | Shell | matched | copilot | 09/03/2026 01:00:00 | Classified retained artifact in existing-patches-31. |
| `syncregistration.h` | WinSync | remaining |  |  |  |
| `syncregistrationerrors.h` | WinSync | matched |  | 09/02/2026 18:09:40 | HRESULT error-code constants only, no functions. |
| `sysinfoapi.h` | SystemInformation | matched | copilot | 09/03/2026 01:00:00 | Classified retained artifact in existing-patches-31. |
| `systemmediatransportcontrolsinterop.h` | WinRT | matched |  | 09/02/2026 21:21:09 | COM/WinRT interop interface method only, out of scope. |
| `t2embapi.h` | Gdi | matched |  |  | Investigated; clean, no ownership metadata gap. See docs/copilot/header-reports/t2embapi.h.md |
| `tabflicks.h` | Tablet | matched |  | 09/02/2026 17:58:11 | Enums/bitfield structs only, no functions. |
| `tapi.h` | Tapi3 | remaining |  |  |  |
| `tapi3.h` | Tapi3 | remaining |  |  |  |
| `tapi3ds.h` | Tapi3 | matched |  |  | Investigated; clean, no ownership metadata gap. See docs/copilot/header-reports/tapi3ds.h.md |
| `tapi3err.h` | Tapi3 | matched |  |  | Investigated; clean, no ownership metadata gap. See docs/copilot/header-reports/tapi3err.h.md |
| `tapi3if.h` | Tapi3 | remaining |  |  |  |
| `taskdeviceservice.h` | WpdSdk | matched |  | 09/02/2026 18:59:13 | GUID/property-key constants only, no functions. |
| `taskschd.h` | TaskSchd | remaining |  |  |  |
| `tbs.h` | Tbs | matched | copilot | 09/03/2026 04:05:00 | New resource-ownership patch created and verified via live re-scrape (build-level validation). |
| `tcerror.h` | Qos | matched |  | 09/02/2026 18:35:09 | Error-code constants only, no functions. |
| `tcguid.h` | Qos | matched |  | 09/02/2026 18:59:13 | GUID constants only, no functions. |
| `tcpxcv.h` | Printing | matched |  | 09/02/2026 18:50:10 | Data structs + constants only, no functions. |
| `tdh.h` | Etw | remaining |  |  |  |
| `tdiinfo.h` | FileHistory, WinProg | matched |  | 09/02/2026 20:28:23 | Structs/typedefs only, no functions. |
| `textserv.h` | Controls.RichEdit | matched | copilot | 09/03/2026 01:15:00 | Classified retained artifact in existing-patches-32. |
| `textstor.h` | Tsf | remaining |  |  |  |
| `thumbcache.h` | Shell | matched | copilot | 09/03/2026 01:15:00 | Classified retained artifact in existing-patches-32. |
| `thumbnailstreamcache.h` | Shell | matched | copilot | 09/03/2026 01:15:00 | Classified retained artifact in existing-patches-32. |
| `timeapi.h` | Media, Multimedia | matched |  | 09/02/2026 18:38:01 | No HANDLE-family type involved. |
| `timezoneapi.h` | FileHistory, Time | matched | copilot | 09/03/2026 01:15:00 | Classified retained artifact in existing-patches-32. |
| `tipautocomplete.h` | Tablet | matched |  |  | Investigated; clean, no ownership metadata gap. See docs/copilot/header-reports/tipautocomplete.h.md |
| `tlhelp32.h` | ToolHelp | matched | copilot | 09/03/2026 01:15:00 | Classified retained artifact in existing-patches-32. |
| `tlogstg.h` | Shell | matched |  |  | Investigated; clean, no ownership metadata gap. See docs/copilot/header-reports/tlogstg.h.md |
| `tnef.h` | Tapi3 | matched |  |  | Investigated; clean, no ownership metadata gap. See docs/copilot/header-reports/tnef.h.md |
| `tokenbinding.h` | Identity | matched |  | 09/02/2026 19:35:00 | All outputs are generic heap-allocated buffers/transparent structs (HeapFree convention), not distinctly-named opaque handles. |
| `tom.h` | Controls.RichEdit | matched | copilot | 09/03/2026 01:30:00 | Classified retained artifact in existing-patches-33. |
| `tpcerror.h` | Tablet | matched |  | 09/02/2026 19:54:02 | HRESULT constants/macros only, no functions. |
| `tpcshrd.h` | Tablet | matched |  | 09/02/2026 21:55:32 | Constants/typedefs/enums only, no functions. |
| `TpmVscAttestation.h` | Security.Tpm | matched |  | 09/02/2026 18:03:29 | Enum + MIDL boilerplate only, no functions. |
| `tpmvscmgr.h` | Security.Tpm | remaining |  |  |  |
| `traffic.h` | Qos | matched |  |  | Investigated; clean, no ownership metadata gap. See docs/copilot/header-reports/traffic.h.md |
| `transportsettingcommon.h` | WinSock | matched |  | 09/02/2026 16:57:06 | Plain data struct only, no functions. |
| `transportsettings.h` | RealTimeCommunications | matched | copilot | 09/03/2026 03:50:00 | No patch needed; COM interface only. |
| `tsattrs.h` | Tsf | matched |  |  | Investigated; clean, no ownership metadata gap. See docs/copilot/header-reports/tsattrs.h.md |
| `tsgauthenticationengine.h` | TermServ | matched |  |  | Investigated; clean, no ownership metadata gap. See docs/copilot/header-reports/tsgauthenticationengine.h.md |
| `tsgpolicyengine.h` | TermServ | matched |  |  | Investigated; clean, no ownership metadata gap. See docs/copilot/header-reports/tsgpolicyengine.h.md |
| `TSPI.h` | Tapi3 | remaining |  |  |  |
| `tspubplugin2com.h` | TermServ | matched |  |  | Investigated; clean, no ownership metadata gap. See docs/copilot/header-reports/tspubplugin2com.h.md |
| `tspubplugincom.h` | TermServ | matched |  | 09/02/2026 22:09:36 | COM interface only, no extern functions. |
| `tssbx.h` | TermServ | matched |  |  | Investigated; clean, no ownership metadata gap. See docs/copilot/header-reports/tssbx.h.md |
| `tsuserex.h` | TermServ | matched |  |  | Investigated; clean, no ownership metadata gap. See docs/copilot/header-reports/tsuserex.h.md |
| `tsvirtualchannels.h` | TermServ | remaining |  |  |  |
| `tuner.h` | MsTv | remaining |  |  |  |
| `tvout.h` | Gdi | matched |  | 09/02/2026 19:21:35 | Data struct + constants only, no functions. |
| `tvratings.h` | MsTv | matched |  |  | Investigated; clean, no ownership metadata gap. See docs/copilot/header-reports/tvratings.h.md |
| `txdtc.h` | DTC | remaining |  |  |  |
| `txfw32.h` | Fs | matched | copilot | 09/03/2026 01:30:00 | Classified retained artifact in existing-patches-33. |
| `txlogpub.h` | TransactionServer | matched |  |  | Investigated; clean, no ownership metadata gap. See docs/copilot/header-reports/txlogpub.h.md |
| `ual.h` | Ual | matched | copilot | 09/02/2026 22:59:00 | First header-scraping-investigation batch: no prior patch existed. New supported-os patch created and verified via live re-scrape, not just static review. |
| `uianimation.h` | UiAnimation | remaining |  |  |  |
| `uiautomationclient.h` | WinAuto | matched | copilot | 09/03/2026 01:30:00 | Classified retained artifact in existing-patches-33. |
| `uiautomationcore.h` | WinAuto | matched | copilot | 09/03/2026 01:30:00 | Classified retained artifact in existing-patches-33. |
| `uiautomationcoreapi.h` | WinAuto | matched | copilot | 09/03/2026 01:30:00 | Classified retained artifact in existing-patches-33. |
| `uiribbon.h` | WindowsRibbon | remaining |  |  |  |
| `UIViewSettingsInterop.h` | WinRT | matched |  | 09/02/2026 21:01:40 | COM/WinRT interop interface method only, out of scope. |
| `upnp.h` | Upnp | remaining |  |  |  |
| `upnphost.h` | Upnp | remaining |  |  |  |
| `urlhist.h` | InternetExplorer | matched |  |  | Investigated; clean, no ownership metadata gap. See docs/copilot/header-reports/urlhist.h.md |
| `urlmon.h` | Shell, TransactionServer | matched | copilot | 09/03/2026 01:45:00 | Classified retained artifact in existing-patches-34. |
| `usb.h` | Buses | matched |  |  | Investigated; clean, no ownership metadata gap. See docs/copilot/header-reports/usb.h.md |
| `usb4dbgioctl.h` | Buses | matched |  | 09/02/2026 18:47:24 | Enums/constants/data structs only, no functions. |
| `usbfnbase.h` | Buses | matched |  |  | Investigated; clean, no ownership metadata gap. See docs/copilot/header-reports/usbfnbase.h.md |
| `usbioctl.h` | Buses | remaining |  |  |  |
| `usbiodef.h` | Buses | matched |  | 09/02/2026 21:52:12 | Constants only, no functions. |
| `usbprint.h` | Printing | matched |  | 09/02/2026 20:28:23 | GUID/IOCTL constants only, no functions. |
| `usbscan.h` | Buses | matched |  | 09/02/2026 20:25:02 | IOCTL parameter structs only, no functions. |
| `usbuser.h` | Buses | matched |  |  | Investigated; clean, no ownership metadata gap. See docs/copilot/header-reports/usbuser.h.md |
| `useractivityinterop.h` | WinRT | matched |  |  | Investigated; clean, no ownership metadata gap. See docs/copilot/header-reports/useractivityinterop.h.md |
| `UserConsentVerifierInterop.h` | WinRT | matched |  | 09/02/2026 20:44:40 | COM/WinRT interop interface method only, out of scope. |
| `userenv.h` | Policy, Shell | blocked | copilot | 09/02/2026 18:53:55 | RE-AUDITED: previously classified in existing-patches-34 based only on retained set-last-error/supported-os patches (not an ownership audit). Re-audit found a genuine unaddressed HANDLE ownership gap (LoadUserProfileW/UnloadUserProfile via PROFILEINFOW.hProfile), same class as physicalmonitorenumerationapi.h. Reclassified to blocked; prior retained patches remain valid. |
| `usp10.h` | Intl | remaining |  |  |  |
| `utilapiset.h` | Base, Debug | matched |  | 09/02/2026 18:29:37 | Pointer obfuscation, not resource ownership; HANDLE is an input only. |
| `uuids.h` | Media, Mf | remaining |  |  |  |
| `uxtheme.h` | Controls, WinTouch | matched | copilot | 09/03/2026 01:45:00 | Classified retained artifact in existing-patches-34. |
| `vbinterf.h` | Com, Com.CallObj, Com.ChannelCreds, Com.Urlmon, ComOle, TransactionServer | matched |  | 09/02/2026 19:02:46 | COM interface refcounting pattern, not HANDLE-family. |
| `vdmdbg.h` | FileHistory, VirtualDOSMachines | matched |  |  | Investigated; clean, no ownership metadata gap. See docs/copilot/header-reports/vdmdbg.h.md |
| `vds.h` | VirtualDiskService | remaining |  |  |  |
| `vdserr.h` | VirtualDiskService | remaining |  |  |  |
| `vdslun.h` | VirtualDiskService | matched |  | 09/02/2026 19:50:38 | Enums/structs + MIDL boilerplate only, no functions. |
| `vdssys.h` | VirtualDiskService, VSS | matched |  |  | Investigated; clean, no ownership metadata gap. See docs/copilot/header-reports/vdssys.h.md |
| `VersionHelpers.h` | SystemInformation | matched |  | 09/02/2026 19:54:02 | All functions are inline, no scrapable extern declarations. |
| `vfw.h` | Multimedia | remaining |  |  |  |
| `vfwext.h` | Multimedia | matched |  | 09/02/2026 18:35:09 | Constants + callback-signature typedefs only, no functions. |
| `vfwmsgs.h` | Media.DShow | remaining |  |  |  |
| `vidcap.h` | Media.DShow | remaining |  |  |  |
| `videoacc.h` | Media.DShow | matched |  |  | Investigated; clean, no ownership metadata gap. See docs/copilot/header-reports/videoacc.h.md |
| `virtdisk.h` | Vhd | remaining |  |  |  |
| `vmgenerationcounter.h` | Hypervisor | matched |  | 09/02/2026 17:19:28 | GUID/IOCTL constant + data struct only, no functions. |
| `vmr9.h` | Media.DShow | remaining |  |  |  |
| `vmsavedstatedump.h` | Hypervisor | matched |  |  | Producer-site fix: added VM_SAVED_STATE_DUMP_HANDLE autoTypes.json entry (CloseApi ReleaseSavedStateFiles). See docs/copilot/header-reports/vmsavedstatedump.h.md |
| `vmsavedstatedumpdefs.h` | Hypervisor | matched |  |  | Investigated; clean, no ownership metadata gap. See docs/copilot/header-reports/vmsavedstatedumpdefs.h.md |
| `vpconfig.h` | Media.DShow | matched |  | 09/02/2026 19:45:17 | COM-style interfaces only, no extern functions. |
| `vpnotify.h` | Media.DShow | matched |  | 09/02/2026 18:23:41 | COM interfaces with plain enum/BOOL parameters only, no handle. |
| `vptype.h` | Media.DShow | matched |  | 09/02/2026 18:47:23 | Enums/data structs only, no functions. |
| `vsadmin.h` | VSS | matched |  |  | Investigated; clean, no ownership metadata gap. See docs/copilot/header-reports/vsadmin.h.md |
| `vsbackup.h` | VSS | matched |  |  | Investigated; clean, no ownership metadata gap. See docs/copilot/header-reports/vsbackup.h.md |
| `vsmgmt.h` | VSS | remaining |  |  |  |
| `vsprov.h` | VSS | remaining |  |  |  |
| `vss.h` | VSS | matched |  |  | Investigated; clean, no ownership metadata gap. See docs/copilot/header-reports/vss.h.md |
| `vsserror.h` | VSS | matched |  |  | Investigated; clean, no ownership metadata gap. See docs/copilot/header-reports/vsserror.h.md |
| `vsstyle.h` | Controls | matched |  |  | Investigated; clean, no ownership metadata gap. See docs/copilot/header-reports/vsstyle.h.md |
| `vssym32.h` | Controls | matched |  |  | Investigated; clean, no ownership metadata gap. See docs/copilot/header-reports/vssym32.h.md |
| `vswriter.h` | VSS | remaining |  |  |  |
| `waasapi.h` | UpdateAssessment | matched |  | 09/02/2026 20:19:39 | COM interface + GUID constants only, no extern functions. |
| `waasapitypes.h` | UpdateAssessment | matched |  | 09/02/2026 19:32:00 | MIDL enums/structs/boilerplate only, no functions. |
| `wab.h` | Wab | blocked | copilot | 09/02/2026 16:53:36 | Deferred: 1 of 77 functions (FtgRegisterIdleRoutine) has genuine return-value handle ownership with no annotation precedent; needs the same dedicated policy decision as getprocesshandlefromhwnd.h. |
| `wabdefs.h` | IMapi, Tapi3 | remaining |  |  |  |
| `wbcl.h` | Qos | remaining |  |  |  |
| `wbemads.h` | Wmi | matched |  |  | Investigated; clean, no ownership metadata gap. See docs/copilot/header-reports/wbemads.h.md |
| `wbemcli.h` | TermServ, Wmi | remaining |  |  |  |
| `wbemdisp.h` | Wmi | remaining |  |  |  |
| `wbemidl.h` | Wmi | matched |  | 09/02/2026 16:57:06 | Redirect-only; 40 functions all MIDL RPC marshalling stubs, no DECLARE_HANDLE. |
| `wbemprov.h` | Wmi | remaining |  |  |  |
| `wbemtran.h` | Wmi | remaining |  |  |  |
| `wcmapi.h` | Wcm | matched |  | 09/02/2026 20:16:15 | Generic memory-allocation convention / transparent struct, not distinctly-named opaque handle. |
| `wcmconfig.h` | Smi | remaining |  |  |  |
| `wcmerrors.h` | Smi | matched |  |  | Investigated; clean, no ownership metadata gap. See docs/copilot/header-reports/wcmerrors.h.md |
| `wcnapi.h` | Wcn | matched |  | 09/02/2026 17:01:48 | Redirect-only; 0 functions in entire Wcn partition, no DECLARE_HANDLE. |
| `wcnfunctiondiscoverykeys.h` | Wcn | matched |  | 09/02/2026 17:35:31 | GUID/PROPERTYKEY constants only, no functions. |
| `wcsplugin.h` | Wcs | matched |  |  | Investigated; clean, no ownership metadata gap. See docs/copilot/header-reports/wcsplugin.h.md |
| `wct.h` | Base, Debug | blocked | copilot | 09/02/2026 19:12:01 | Recorded for per-header traceability; same root cause as getprocesshandlefromhwnd.h. |
| `WDBGEXTS.H` | Debug.Extensions | remaining |  |  |  |
| `wdigest.h` | Identity | matched |  | 09/02/2026 17:27:04 | String constants only, no functions. |
| `wdmguid.h` | DevInst | matched |  |  | Investigated; clean, no ownership metadata gap. See docs/copilot/header-reports/wdmguid.h.md |
| `wdsbp.h` | Wds | blocked |  | 09/02/2026 21:24:15 | WdsBpParseInitialize/WdsBpInitialize produce generic HANDLE via direct out-param. |
| `wdsclientapi.h` | Wds | matched | copilot | 09/03/2026 01:45:00 | Classified retained artifact in existing-patches-34. |
| `wdsmcerr.h` | Wds | matched |  |  | Investigated; clean, no ownership metadata gap. See docs/copilot/header-reports/wdsmcerr.h.md |
| `wdspxe.h` | Wds | matched | copilot | 09/03/2026 01:45:00 | Classified retained artifact in existing-patches-34. |
| `wdstci.h` | Wds | matched |  | 09/02/2026 20:53:10 | Constants/enums/structs only, no functions. |
| `wdstpdi.h` | Wds | blocked |  | 09/02/2026 20:25:02 | WdsTransportProviderCreateInstance/OpenContent produce generic HANDLE via direct out-param. |
| `wdstptmgmt.h` | Wds | remaining |  |  |  |
| `wdstptmgmtmsg.h` | Wds | matched |  |  | Investigated; clean, no ownership metadata gap. See docs/copilot/header-reports/wdstptmgmtmsg.h.md |
| `weakreference.h` | WinRT | matched |  | 09/02/2026 21:24:15 | COM/WinRT interface methods only, out of scope. |
| `webapplication.h` | Debug.WebApp | matched |  |  | Investigated; clean, no ownership metadata gap. See docs/copilot/header-reports/webapplication.h.md |
| `WebAuthenticationCoreManagerInterop.h` | WinRT | matched |  | 09/02/2026 21:38:18 | COM/WinRT interop interface method only, out of scope. |
| `webauthn.h` | WebAuthn | remaining |  |  |  |
| `webauthnplugin.h` | WebAuthn | matched |  |  | Investigated; clean, no ownership metadata gap. See docs/copilot/header-reports/webauthnplugin.h.md |
| `webevnts.h` | InternetExplorer | matched |  |  | Investigated; clean, no ownership metadata gap. See docs/copilot/header-reports/webevnts.h.md |
| `webservices.h` | Wsw | remaining |  |  |  |
| `websocket.h` | WebSock | matched |  | 09/02/2026 21:01:40 | WEB_SOCKET_HANDLE already correctly covered via existing autoTypes.json entry. |
| `werapi.h` | Wer | matched |  |  | Investigated; clean, no ownership metadata gap. See docs/copilot/header-reports/werapi.h.md |
| `wheadef.h` | Debug | matched |  |  | Investigated; clean, no ownership metadata gap. See docs/copilot/header-reports/wheadef.h.md |
| `wia.h` | Wia | matched |  | 09/02/2026 16:53:36 | Redirect-only; 185 functions all COM/RPC marshalling stubs, no DECLARE_HANDLE. |
| `wiadef.h` | Wia | remaining |  |  |  |
| `wiadevd.h` | Wia | matched |  | 09/02/2026 19:42:09 | Only extern function takes no opaque-handle output; other handle producers are COM vtable methods. |
| `wiaintfc.h` | Wia | matched |  | 09/02/2026 17:05:19 | Device-interface GUID constant only, no functions. |
| `wiamicro.h` | Wia | matched |  | 09/02/2026 21:38:18 | No concrete annotatable ownership gap identified. |
| `wiamindr_lh.h` | Wia | remaining |  |  |  |
| `wiamindr.h` | Wia | matched |  | 09/02/2026 16:49:01 | Redirect-only header (#include wiamindr_lh.h/wiamindr_xp.h); no direct declarations. |
| `wiatwcmp.h` | Wia | matched |  | 09/02/2026 18:00:53 | Data struct + constants only, no functions. |
| `wiavideo.h` | Wia | matched |  |  | Investigated; clean, no ownership metadata gap. See docs/copilot/header-reports/wiavideo.h.md |
| `wiawsdsc.h` | Wia | matched |  | 09/02/2026 18:35:09 | Property-ID constants only, no functions. |
| `winbase.h` | Base, DataXchg, FileHistory, Fs, Identity, Input.Ime, Intl, IO, MenuRc, Registry, Security, Security.AppLocker, Security.ConfigurationSnapin, Security.Cryptography.Catalog, Security.Cryptography.Sip, Security.DiagnosticDataQuery, Security.DirectoryServices, Security.LicenseProtection, Security.Tpm, Security.WinTrust, Security.WinWlx, Setup, Shutdown, TermServ, WinProg | matched | copilot | 09/03/2026 02:00:00 | Classified retained artifact in existing-patches-35. |
| `winber.h` | Ldap | matched |  |  | Genuine producer-site fix: added BerElement autoTypes.json entry (CloseApi=ber_free), matching sibling PLDAPSearch's typedef-struct ValueType convention. See docs/copilot/header-reports/winber.h.md |
| `winbio_adapter.h` | SecBitomet | remaining |  |  |  |
| `winbio_err.h` | SecBitomet | matched |  |  | Investigated; clean, no ownership metadata gap. See docs/copilot/header-reports/winbio_err.h.md |
| `winbio_ioctl.h` | SecBitomet | matched |  | 09/02/2026 21:49:17 | IOCTL/GUID constants only, no functions. |
| `winbio_types.h` | SecBitomet | remaining |  |  |  |
| `winbio.h` | SecBitomet | matched |  |  | Genuine producer-site fix: added WINBIO_SESSION_HANDLE autoTypes.json entry (ValueType=uint, novel pattern, CloseApi=WinBioCloseSession). See docs/copilot/header-reports/winbio.h.md |
| `wincodec.h` | Wic | matched | copilot | 09/03/2026 02:00:00 | Classified retained artifact in existing-patches-35. |
| `wincodecsdk.h` | Wic | matched | copilot | 09/03/2026 02:00:00 | Classified retained artifact in existing-patches-35. |
| `wincon.h` | Console | blocked | copilot | 09/02/2026 17:19:28 | Deferred: wincon.h redirects to consoleapi.h/consoleapi2.h/consoleapi3.h/wincontypes.h (none separately tracked); live-scrape of the Console partition (~120 functions) found a genuine multi-producer/single-consumer return-value HANDLE ownership relationship with no annotation precedent, same class as getprocesshandlefromhwnd.h/wab.h. |
| `winconp.h` | Console | matched |  |  | Investigated; clean, no ownership metadata gap. See docs/copilot/header-reports/winconp.h.md |
| `wincred.h` | Certificates, Credentials, Identity, Security, Security.AppLocker, Security.ConfigurationSnapin, Security.Cryptography, Security.Cryptography.Catalog, Security.Cryptography.Sip, Security.Cryptography.UI, Security.DiagnosticDataQuery, Security.DirectoryServices, Security.LicenseProtection, Security.Tpm, Security.WinTrust, Security.WinWlx | matched | copilot | 09/03/2026 02:00:00 | Classified retained artifact in existing-patches-35. |
| `wincrypt.h` | NetMgmt, Rpc, Security.Cryptography, Security.Cryptography.UI | matched | copilot | 09/02/2026 22:40:00 | Corrected typedef-owned ownership violation found during resource-ownership audit; final header in the 11-header audit set (all now compliant). Verified compatible with co-resident zz-crypto-security-enums patch via full replay. |
| `winddi.h` | Display, IO, Printing, TransactionServer | remaining |  |  |  |
| `winddiui.h` | Printing | matched |  |  | Investigated; clean, no ownership metadata gap. See docs/copilot/header-reports/winddiui.h.md |
| `windef.h` | Foundation, Gdi, WinAuto | matched | copilot | 09/02/2026 19:18:45 | Closed in shared-handle-policy-01; current windows-rs full test target is blocked by missing PartitionSpec.include_main_file initializers. |
| `windns.h` | Dns, IpHlp | matched | copilot | 09/03/2026 02:00:00 | Classified retained artifact in existing-patches-35. |
| `windnsdef.h` | Dns | remaining |  |  |  |
| `windot11.h` | Ndis, NWifi | remaining |  |  |  |
| `windows.ai.machinelearning.native.h` | WinRT.ML | matched |  |  | Investigated; clean, no ownership metadata gap. See docs/copilot/header-reports/windows.ai.machinelearning.native.h.md |
| `windows.data.pdf.interop.h` | WinRT.Pdf | matched |  | 09/02/2026 21:08:27 | Standard COM factory function + inline helper + COM interface method only. |
| `windows.devices.alljoyn.interop.h` | WinRT.AllJoyn | matched |  |  | Investigated; clean, no ownership metadata gap. See docs/copilot/header-reports/windows.devices.alljoyn.interop.h.md |
| `Windows.Devices.Display.Core.Interop.h` | WinRT.Display | matched |  | 09/02/2026 22:12:28 | COM interface methods + inline helpers only, out of scope. |
| `windows.devices.midi.h` | Media.DShow | remaining |  |  |  |
| `windows.fixed.h` | AllJoyn, Antimalware, ApplicationVerifier, AppxPackaging, Audio, Audio.Apo, Audio.DirectMusic, Audio.DirectSound, Audio.Endpoints, Authorization, Authorization.UI, Backup, Base, Bits, Bluetooth, Buses, Cabinets, CEIP, Certificates, Cloudapi, ClrHosting, ClrProfiling, CmpApi, Com, Com.CallObj, Com.ChannelCreds, Com.Events, Com.StructuredStorage, Com.Urlmon, Communication, ComOle, CompositionSwapchain, Console, Controls, Controls.RichEdit, Cos, Credentials, DataXchg, Debug, Debug.ActiveScript, Debug.Extensions, Debug.WebApp, Dedup, DeliveryOptimization, DeviceAccess, DeviceQuery, Devices.1394, Devices.Beep, Devices.Cdrom, Devices.Dvd, Devices.Properties, DevInst, DevLic, Dfs, Dhcp, Direct2D, Direct2D.Common, Direct3D, Direct3D10, Direct3D11, Direct3D11on12, Direct3D12, Direct3D9, Direct3D9on12, Direct3DDxc, Direct3DFxc, DirectComp, DirectDraw, DirectManipulation, DirectML, DirectWrite, Display, DlgBox, Dlna, Dns, DTC, Dwm, DXCore, Dxgi, Dxgi.Common, Eap, EapHost, Environment, Etw, Fax, FileHistory, Foundation, Fs, Fsrm, FunctionDiscovery, GameInput, GameMode, GamingDvcInfo, Gdi, Gdiplus, Gpmc, Hid, HostComputeNetwork, HostComputeSystem, HtmlHelp, Hypervisor, Identity, IdentityProvider, Ifsk, Iis, ImagingDevice, IMapi, IndexSrv, Input_Ink, Input_IntContext, Input.Ime, InputRadial, InternetConnectionWizard, InternetExplorer, Intl, IO, Ioctl, IScsiDisc, Isolation, JobObjects, Js, KeyboardAndMouseInput, Ldap, LibraryLoader, Lwef, MachineLearning, MadCap, MagApi, Mapi, Mbn, MdmReg, Media, Media.DShow, Media.DShow.Xml, Media.DxMediaObjects, Media.KernelStreaming, Memory, MenuRc, MessageQueuing, Mf, MixedReality, MixedRealityInterop, Mmc, Monitor, MsChap, MsCs, MsHtml, MsTv, MsXml, Multimedia, Ndf, Ndis, NetBios, NetMgmt, NetShell, NetworkAccessProtection, Nfc, Nfp, Nla, Nps, NWifi, Of, Opc, OpenGL, Parcon, Perf, PicAcq, Pipes, Pla, Policy, Power, Printing, PrintTicket, Proc_Snap, ProjFs, Properties, PsApi1, PsApi2, PWM, Rdc, Rdp, RealTimeCommunications, Registry, RemoteAssist, Rm, Rpc, RRas, RstMgr, Search, SecBitomet, Security, Security.AppLocker, Security.ConfigurationSnapin, Security.Cryptography, Security.Cryptography.Catalog, Security.Cryptography.Sip, Security.Cryptography.UI, Security.DiagnosticDataQuery, Security.DirectoryServices, Security.LicenseProtection, Security.Tpm, Security.WinTrust, Security.WinWlx, SecurityCenter, Sens, SerPorts, Services, Setup, Shell, Shell.Common, Shutdown, SideShow, Smi, Snmp, Speech, Sr, Storage.Jet, Storage.Nvme, SwDevice, SystemInformation, Tablet, Tapi3, TaskSchd, Tbs, Tcui, TermServ, Threading, Time, ToolHelp, TransactionServer, Tsf, UiAnimation, UpdateAssessment, Upnp, Variant, Vhd, VirtualDiskService, VirtualDOSMachines, VSS, Wab, Wcm, Wcn, Wcs, Wds, WebAuthn, WebDav, WebServicesOnDevices, WebSock, Wec, Wer, Wes, Wia, Wibe, Wic, Win32_Tile_Badge_Notif, WinAuto, WinContacts, WindowsConnectionManager, WindowsFirewall, WindowsRibbon, WindowsSetupAndMigration, WinInet, WinLocation, WinProg, WinRm, WinRT, WinRT.AllJoyn, WinRT.Composition, WinRT.CoreInputView, WinRT.Direct2D, WinRT.Direct3D11, WinRT.Display, WinRT.Graphics.Capture, WinRT.Graphics.Imaging, WinRT.Holographic, WinRT.Isolation, WinRT.Media, WinRT.Metadata, WinRT.ML, WinRT.Pdf, WinRT.Printing, WinRT.Storage, WinRT.Xaml, WinSat, WinSensors, WinSync, WinTouch, Wmdm, WmFormat, Wmi, Wmlss, Wmp, Wnet, WpdSdk, Wsb, Wsl, Wsw, Wua, Xaml_Diagnostics, Xaudio2, Xblidp, Xinput, XmlLite, Xps, XpsPrinting | remaining |  |  |  |
| `Windows.Graphics.Capture.Interop.h` | WinRT.Graphics.Capture | matched |  | 09/02/2026 17:35:31 | COM factory + query-only HWND/HMONITOR accessors. |
| `windows.graphics.directx.direct3d11.interop.h` | WinRT.Direct3D11 | matched |  | 09/02/2026 19:02:46 | COM factory pattern throughout. |
| `windows.graphics.effects.interop.h` | WinRT.Direct2D | matched |  | 09/02/2026 18:44:05 | COM interface with plain-value/interface-pointer outputs only. |
| `Windows.Graphics.Holographic.Interop.h` | WinRT.Holographic | matched |  |  | Investigated; clean, no ownership metadata gap. See docs/copilot/header-reports/Windows.Graphics.Holographic.Interop.h.md |
| `windows.graphics.imaging.interop.h` | WinRT.Graphics.Imaging | matched |  |  | Investigated; clean, no ownership metadata gap. See docs/copilot/header-reports/windows.graphics.imaging.interop.h.md |
| `windows.graphics.interop.h` | WinRT.Direct2D | matched |  | 09/02/2026 17:41:54 | COM factory pattern. |
| `windows.graphics.printing.workflow.native.h` | WinRT.Printing | matched |  |  | Investigated; clean, no ownership metadata gap. See docs/copilot/header-reports/windows.graphics.printing.workflow.native.h.md |
| `windows.h` | Console, Edp, Enstor, Memory, Mi | matched |  | 09/02/2026 21:08:27 | Pure umbrella redirect; all sub-headers tracked or (dlgs.h) directly confirmed no functions. |
| `windows.media.core.interop.h` | WinRT.Media | matched |  |  | Investigated; clean, no ownership metadata gap. See docs/copilot/header-reports/windows.media.core.interop.h.md |
| `windows.security.isolation.isolatedenvironmentinterop.h` | WinRT.Isolation | matched |  | 09/02/2026 20:41:40 | COM interface method returning non-owned HWND reference, out of scope. |
| `windows.ui.composition.interop.h` | WinRT.Composition | matched |  | 09/02/2026 20:19:39 | COM interop interface methods only, out of scope. |
| `windows.ui.viewmanagement.core.coreframeworkinputviewinterop.h` | WinRT.CoreInputView | matched |  | 09/02/2026 21:21:09 | COM/WinRT interop interface method only, out of scope. |
| `windows.ui.xaml.hosting.desktopwindowxamlsource.h` | WinRT.Xaml | matched |  | 09/02/2026 22:06:03 | COM interface methods only, out of scope. |
| `windows.ui.xaml.hosting.referencetracker.h` | WinRT.Xaml | matched |  |  | Investigated; clean, no ownership metadata gap. See docs/copilot/header-reports/windows.ui.xaml.hosting.referencetracker.h.md |
| `windows.ui.xaml.media.dxinterop.h` | WinRT.Xaml | matched |  |  | Investigated; clean, no ownership metadata gap. See docs/copilot/header-reports/windows.ui.xaml.media.dxinterop.h.md |
| `windowsceip.h` | Base, CEIP, FileHistory | matched |  | 09/02/2026 17:01:48 | CeipIsOptedIn() - no handle involved. |
| `windowsdefender.h` | Lwef | matched |  | 09/02/2026 18:32:46 | BOOL-only API, no handle. |
| `windowssearcherrors.h` | Search | remaining |  |  |  |
| `windowssideshow.h` | SideShow | matched | copilot | 09/03/2026 03:50:00 | No patch needed; COM interface only. |
| `windowssideshowapi.h` | SideShow | matched | copilot | 09/03/2026 03:50:00 | No patch needed; COM interface only. |
| `windowssideshowdriverevents.h` | SideShow | matched | copilot | 09/03/2026 04:05:00 | No patch needed; COM interface only. |
| `windowsstoragecom.h` | WinRT.Storage | matched |  |  | Investigated; clean, no ownership metadata gap. See docs/copilot/header-reports/windowsstoragecom.h.md |
| `windowsx.h` | Controls, FileHistory, Gdi, WinProg | remaining |  |  |  |
| `winefs.h` | Fs | matched | copilot | 09/03/2026 02:15:00 | Classified retained artifact in existing-patches-36. |
| `winenclave.h` | Enclave | matched | copilot | 09/03/2026 03:20:00 | No patch needed; verified via live scrape and static review. |
| `winerror.h` | Direct2D, DXCore, Dxgi, Dxgi.Common, Foundation, Rpc, TaskSchd, TransactionServer, UiAnimation, Wes | remaining |  |  |  |
| `winevt.h` | Wes | matched | copilot | 09/03/2026 02:15:00 | Classified retained artifact in existing-patches-36. |
| `winfax.h` | Fax | remaining |  |  |  |
| `wingdi.h` | DataXchg, Direct3D9, Gdi, Intl, Media.DShow, Tablet, Wcs | matched | copilot | 09/03/2026 02:15:00 | Classified retained artifact in existing-patches-36. |
| `winhttp.h` | HttpServer, WinHttp | matched | copilot | 09/03/2026 02:15:00 | Classified retained artifact in existing-patches-36. |
| `WinHvEmulation.h` | Hypervisor | matched |  | 09/02/2026 20:13:28 | Fixed genuine gap: added new autoTypes.json entry for WHV_EMULATOR_HANDLE (CloseApi=WHvEmulatorDestroyEmulator). |
| `WinHvPlatform.h` | Hypervisor | matched |  |  | Genuine producer-site fix: added WHV_TRIGGER_HANDLE and WHV_NOTIFICATION_PORT_HANDLE autoTypes.json entries. See docs/copilot/header-reports/WinHvPlatform.h.md |
| `wininet.h` | FileHistory, WinInet, WinProg | matched | copilot | 09/03/2026 02:15:00 | Classified retained artifact in existing-patches-36. |
| `winineti.h` | WinInet | matched | copilot | 09/03/2026 02:30:00 | Classified retained artifact in existing-patches-37. |
| `winioctl.h` | Base, Fs, Ioctl, Nfc, Nfp | remaining |  |  |  |
| `winldap.h` | Ldap | matched | copilot | 09/03/2026 02:30:00 | Classified retained artifact in existing-patches-37. |
| `winmeta.h` | TraceLogging | matched | copilot | 09/03/2026 03:20:00 | No patch needed; constant-only header verified via live scrape. |
| `winml.h` | MachineLearning | matched |  |  | Investigated; clean, no ownership metadata gap. See docs/copilot/header-reports/winml.h.md |
| `winnetwk.h` | Wnet | matched | copilot | 09/03/2026 02:30:00 | Classified retained artifact in existing-patches-37. |
| `winnls.h` | Intl | remaining |  |  |  |
| `winnls32.h` | FileHistory, WinProg | matched |  | 09/02/2026 18:23:41 | HWND is an input; IME info struct is caller-allocated, not a handle. |
| `winnt.h` | Backup, Base, Display, DXCore, Dxgi, Dxgi.Common, FileHistory, Fs, Identity, Input.Ime, Intl, IO, JobObjects, MenuRc, Registry, Security, Security.AppLocker, Security.ConfigurationSnapin, Security.Cryptography.Catalog, Security.Cryptography.Sip, Security.DiagnosticDataQuery, Security.DirectoryServices, Security.LicenseProtection, Security.Tpm, Security.WinTrust, Security.WinWlx, Setup, Shell, Shutdown, Threading, WinProg | remaining |  |  |  |
| `winperf.h` | Perf | remaining |  |  |  |
| `winppi.h` | Printing | blocked | copilot | 09/02/2026 17:51:18 | Deferred: genuine spool-file/page HANDLE ownership relationship via return value, no annotation precedent; same class as getprocesshandlefromhwnd.h/wab.h/wincon.h. |
| `winreg.h` | Registry | matched | copilot | 09/03/2026 02:30:00 | Classified retained artifact in existing-patches-37. |
| `winsafer.h` | Security.AppLocker | matched | copilot | 09/02/2026 21:05:00 | Corrected typedef-owned ownership violation found during resource-ownership audit. |
| `winsatcominterfacei.h` | WinSat | remaining |  |  |  |
| `winscard.h` | Credentials | remaining |  |  |  |
| `winsdkver.h` | Foundation | matched |  | 09/02/2026 17:09:31 | Version constants only, no functions. |
| `winsmcrd.h` | Credentials | matched |  |  | Investigated; clean, no ownership metadata gap. See docs/copilot/header-reports/winsmcrd.h.md |
| `winsnmp.h` | Snmp | matched |  |  | Genuine producer-site fix: added 5 autoTypes.json entries (HSNMP_SESSION/ENTITY/CONTEXT/PDU/VBL). See docs/copilot/header-reports/winsnmp.h.md |
| `WinSock.h` | Fsrm, TermServ, WinSock | matched | copilot | 09/03/2026 02:30:00 | Classified retained artifact in existing-patches-37. |
| `winsock2.h` | ActiveDirectory, HttpServer, IpHlp, P2p, Qos, Ual, WindowsFilteringPlatform, WinHttp, WinSock, wnv | matched | copilot | 09/03/2026 02:50:00 | Classified retained artifact in existing-patches-38. |
| `winsplp.h` | Printing | remaining |  |  |  |
| `winspool.h` | Gdi, Printing | matched | copilot | 09/02/2026 22:05:00 | Audited under corrected shared-handle policy (165b5f09, 7335ddc4); already compliant, no code changes required. Not part of the original 11-header audit set (discovered as an additional resource-ownership patch). |
| `winstring.h` | WinRT | matched |  | 09/02/2026 20:07:23 | HSTRING/HSTRING_BUFFER already correctly covered via existing autoTypes.json entries. |
| `winsvc.h` | Services | matched | copilot | 09/02/2026 21:45:00 | Corrected typedef-owned ownership violation found during resource-ownership audit (10 producer sites across 2 handle types). |
| `winsxs.h` | Setup | matched |  |  | Investigated; clean, no ownership metadata gap. See docs/copilot/header-reports/winsxs.h.md |
| `winsync.h` | WinSync | remaining |  |  |  |
| `winternl.h` | ActiveDirectory, Base, Certificates, FileHistory, IpHlp, MadCap, Security, Security.AppLocker, Security.ConfigurationSnapin, Security.Cryptography, Security.Cryptography.Catalog, Security.Cryptography.Sip, Security.Cryptography.UI, Security.DiagnosticDataQuery, Security.DirectoryServices, Security.LicenseProtection, Security.Tpm, Security.WinTrust, Security.WinWlx, WinProg | matched |  |  | Investigated; clean, no ownership metadata gap. See docs/copilot/header-reports/winternl.h.md |
| `wintrust.h` | Security.WinTrust | remaining |  |  |  |
| `winusb.h` | Buses | matched | copilot | 09/03/2026 02:50:00 | Classified retained artifact in existing-patches-38. |
| `winusbio.h` | Buses | matched |  | 09/02/2026 18:38:01 | Constants + data structs only, no functions. |
| `winuser.h` | Base, Controls, DataXchg, DlgBox, Dwm, FileHistory, Gdi, Input.Ime, Intl, IO, MenuRc, Security, Security.AppLocker, Security.ConfigurationSnapin, Security.DiagnosticDataQuery, Security.DirectoryServices, Security.LicenseProtection, Security.Tpm, Security.WinTrust, Security.WinWlx, Shell, TermServ, WinAuto, WinProg, WinTouch | matched | copilot | 09/03/2026 02:50:00 | Classified retained artifact in existing-patches-38. |
| `winver.h` | Fs | matched | copilot | 09/03/2026 02:50:00 | Classified retained artifact in existing-patches-38. |
| `winwlx.h` | Security.WinWlx | remaining |  |  |  |
| `wlanapi.h` | Ndis, NWifi | matched | copilot | 09/03/2026 02:50:00 | Classified retained artifact in existing-patches-38. |
| `wlanihv.h` | NWifi | matched |  |  | Investigated; clean, no ownership metadata gap. See docs/copilot/header-reports/wlanihv.h.md |
| `wlanihvtypes.h` | NWifi | matched |  | 09/02/2026 18:15:12 | Data structs + constants only, no functions. |
| `wlantypes.h` | Ndis, NWifi | matched |  | 09/02/2026 20:19:39 | Enums/structs only, no functions. |
| `wlclient.h` | NWifi | matched |  | 09/02/2026 17:54:05 | Data structs only, no functions. |
| `wldp.h` | FileHistory, WinProg | matched | copilot | 09/03/2026 03:05:00 | Classified retained artifact in existing-patches-39. |
| `wmcodecdsp.h` | Media.DShow, Mf | remaining |  |  |  |
| `wmcontainer.h` | Mf | matched | copilot | 09/03/2026 03:05:00 | Classified retained artifact in existing-patches-39. |
| `wmdmlog.h` | Wmdm | matched |  | 09/02/2026 22:02:59 | COM interface only, no extern functions. |
| `wmdxva.h` | WmFormat | matched |  |  | Investigated; clean, no ownership metadata gap. See docs/copilot/header-reports/wmdxva.h.md |
| `wmistr.h` | Etw | matched |  |  | Investigated; clean, no ownership metadata gap. See docs/copilot/header-reports/wmistr.h.md |
| `wmiutils.h` | Lwef, Wmi | remaining |  |  |  |
| `wmlss.h` | Wmlss | remaining |  |  |  |
| `wmnetsourcecreator.h` | WmFormat | matched |  |  | Investigated; clean, no ownership metadata gap. See docs/copilot/header-reports/wmnetsourcecreator.h.md |
| `wmp.h` | Media.DShow, Wmp | remaining |  |  |  |
| `wmpdevices.h` | Wmp | matched |  | 09/02/2026 20:44:40 | Inline functions generate no bindings; remaining declarations are IOCTL constants/structs. |
| `wmpids.h` | Wmp | remaining |  |  |  |
| `wmpplug.h` | Wmp | matched |  | 09/02/2026 22:06:03 | COM interface only, no extern functions. |
| `wmprealestate.h` | Wmp | matched |  |  | Investigated; clean, no ownership metadata gap. See docs/copilot/header-reports/wmprealestate.h.md |
| `wmpservices.h` | Wmp | matched |  |  | Investigated; clean, no ownership metadata gap. See docs/copilot/header-reports/wmpservices.h.md |
| `wmsbuffer.h` | WmFormat | matched |  |  | Investigated; clean, no ownership metadata gap. See docs/copilot/header-reports/wmsbuffer.h.md |
| `wmsdkidl.h` | WmFormat | remaining |  |  |  |
| `wmsdkvalidate.h` | WmFormat | matched |  | 09/02/2026 18:32:46 | String/buffer/BOOL API only, no handle. |
| `wmsecure.h` | WmFormat | matched |  |  | Investigated; clean, no ownership metadata gap. See docs/copilot/header-reports/wmsecure.h.md |
| `wmsinternaladminnetsource.h` | WmFormat | remaining |  |  |  |
| `wmsysprf.h` | Wmp | matched |  |  | Investigated; clean, no ownership metadata gap. See docs/copilot/header-reports/wmsysprf.h.md |
| `wnnc.h` | Ifsk | matched |  | 09/02/2026 19:19:02 | Constants only, no functions. |
| `wnvapi.h` | wnv | blocked | copilot | 09/02/2026 19:09:17 | Recorded for per-header traceability; same root cause as getprocesshandlefromhwnd.h. |
| `wofapi.h` | Fs | matched |  | 09/02/2026 20:31:37 | No function produces an opaque handle; HANDLE params are pre-owned caller inputs. |
| `workspaceax.h` | TermServ | matched |  |  | Investigated; clean, no ownership metadata gap. See docs/copilot/header-reports/workspaceax.h.md |
| `workspaceruntime.h` | TermServ | remaining |  |  |  |
| `workspaceruntimeclientext.h` | TermServ | matched |  | 09/02/2026 20:49:21 | COM interface method only, out of scope. |
| `wpc.h` | Parcon | matched |  | 09/02/2026 16:34:06 | All functions are MIDL RPC marshalling stubs (BSTR_User*/HWND_User*); no resource-owning functions. |
| `wpdmtpextensions.h` | WpdSdk | matched |  |  | Investigated; clean, no ownership metadata gap. See docs/copilot/header-reports/wpdmtpextensions.h.md |
| `wpdshellextension.h` | WpdSdk | matched |  | 09/02/2026 19:37:21 | GUID/property-key constants only, no functions. |
| `wrdsgraphicschannels.h` | TermServ | matched |  |  | Investigated; clean, no ownership metadata gap. See docs/copilot/header-reports/wrdsgraphicschannels.h.md |
| `ws2atm.h` | WinSock | matched |  |  | Investigated; clean, no ownership metadata gap. See docs/copilot/header-reports/ws2atm.h.md |
| `ws2bth.h` | Bluetooth | matched |  |  | Investigated; clean, no ownership metadata gap. See docs/copilot/header-reports/ws2bth.h.md |
| `ws2def.h` | IpHlp, WinSock | remaining |  |  |  |
| `ws2ipdef.h` | IpHlp, WinSock | matched |  |  | Investigated; clean, no ownership metadata gap. See docs/copilot/header-reports/ws2ipdef.h.md |
| `ws2spi.h` | WinSock | matched | copilot | 09/03/2026 03:05:00 | Classified retained artifact in existing-patches-39. |
| `ws2tcpip.h` | WinSock | matched | copilot | 09/03/2026 03:05:00 | Classified retained artifact in existing-patches-39. |
| `wsbapp.h` | Wsb | matched |  |  | Investigated; clean, no ownership metadata gap. See docs/copilot/header-reports/wsbapp.h.md |
| `wsbapperror.h` | Wsb | matched |  | 09/02/2026 18:35:09 | HRESULT error-code constants/macros only, no functions. |
| `wsbonline.h` | Wsb | matched |  | 09/02/2026 18:32:46 | Struct/GUID input-only API, no handle. |
| `wsbonlineerror.h` | Wsb | matched |  | 09/02/2026 18:44:05 | HRESULT error-code constants/macros only, no functions. |
| `wscapi.h` | FileHistory, SecurityCenter | blocked |  | 09/02/2026 20:53:11 | WscRegisterForChanges produces generic HANDLE via direct out-param. |
| `wsdapi.h` | WebServicesOnDevices | matched |  | 09/02/2026 17:13:02 | Redirect-only; 32 functions all COM-factory/memory-helper pairs (void*, not HANDLE), no DECLARE_HANDLE. |
| `wsdattachment.h` | FunctionDiscovery | matched |  |  | Investigated; clean, no ownership metadata gap. See docs/copilot/header-reports/wsdattachment.h.md |
| `wsdbase.h` | FunctionDiscovery | remaining |  |  |  |
| `wsdclient.h` | FunctionDiscovery | remaining |  |  |  |
| `wsddisco.h` | FunctionDiscovery | remaining |  |  |  |
| `wsdevlicensing.h` | DevLic | matched |  | 09/02/2026 17:16:10 | HWND is input parameter; outputs are plain FILETIME values. |
| `wsdhost.h` | FunctionDiscovery | matched |  |  | Investigated; clean, no ownership metadata gap. See docs/copilot/header-reports/wsdhost.h.md |
| `wsdns.h` | WebServicesOnDevices | matched |  |  | Investigated; clean, no ownership metadata gap. See docs/copilot/header-reports/wsdns.h.md |
| `wsdtypes.h` | FunctionDiscovery | matched |  |  | Investigated; clean, no ownership metadata gap. See docs/copilot/header-reports/wsdtypes.h.md |
| `wsdutil.h` | FunctionDiscovery | matched |  | 09/02/2026 19:02:46 | WSDAPI linked-memory/buffer convention, not HANDLE-family (consistent with wsdapi.h). |
| `wsdxml.h` | FunctionDiscovery | matched |  |  | Investigated; clean, no ownership metadata gap. See docs/copilot/header-reports/wsdxml.h.md |
| `wshisotp.h` | WinSock | matched |  | 09/02/2026 19:09:17 | Data struct + constants/macro only, no functions. |
| `wsipv6ok.h` | WinSock | matched |  | 09/02/2026 19:06:16 | Macro redefinitions only, no declarations. |
| `wsipx.h` | WinSock | matched |  | 09/02/2026 18:09:40 | Data struct + constants only, no functions. |
| `wslapi.h` | Wsl | blocked | copilot | 09/02/2026 19:02:46 | Deferred: direct out-param instance of the generic/shared-type blocker class (HANDLE has no single correct CloseApi). |
| `wsman.h` | WinRm | remaining |  |  |  |
| `wsmandisp.h` | WinRm | remaining |  |  |  |
| `wsmerror.h` | WinRm | remaining |  |  |  |
| `wsnetbs.h` | WinSock | matched |  | 09/02/2026 18:29:38 | Data struct + constants/macro only, no functions. |
| `wsnwlink.h` | WinSock | matched |  | 09/02/2026 21:59:01 | Constants only, no functions. |
| `wsrm.h` | WinSock | matched |  | 09/02/2026 20:34:23 | Socket-option constants only, no functions. |
| `wsvns.h` | WinSock | matched |  | 09/02/2026 17:41:54 | Data struct + constants only, no functions. |
| `wtsapi32.h` | TermServ | remaining |  |  |  |
| `wtsdefs.h` | TermServ | remaining |  |  |  |
| `wtshintapi.h` | TermServ | matched |  | 09/02/2026 19:12:01 | HWND is an input; render hint ID is a plain UINT64, not a HANDLE. |
| `wtsprotocol.h` | TermServ | remaining |  |  |  |
| `wtypes.h` | Com, Com.CallObj, Com.ChannelCreds, Com.StructuredStorage, Com.Urlmon, ComOle, FileHistory, Properties, Registry, Shutdown, TransactionServer, WinProg | matched | copilot | 09/02/2026 19:18:45 | Closed in shared-handle-policy-01; current windows-rs full test target is blocked by missing PartitionSpec.include_main_file initializers. |
| `wtypesbase.h` | TransactionServer, WinSock | matched |  |  | Investigated; clean, no ownership metadata gap. See docs/copilot/header-reports/wtypesbase.h.md |
| `wuapi.h` | Wua | remaining |  |  |  |
| `wuerror.h` | Wua | remaining |  |  |  |
| `xa.h` | DTC | matched |  | 09/02/2026 21:38:18 | No function produces an opaque handle; XA switch entries are function-pointer struct fields. |
| `xamlom.h` | Xaml_Diagnostics | remaining |  |  |  |
| `xapo.h` | Xaudio2 | matched |  |  | Investigated; clean, no ownership metadata gap. See docs/copilot/header-reports/xapo.h.md |
| `xapofx.h` | Xaudio2 | matched |  | 09/02/2026 21:49:17 | Standard COM factory function + GUID/parameter constants only. |
| `xaudio2.h` | Xaudio2 | remaining |  |  |  |
| `xaudio2fx.h` | Xaudio2 | matched |  |  | Investigated; clean, no ownership metadata gap. See docs/copilot/header-reports/xaudio2fx.h.md |
| `xblidpauthmanager.h` | Xblidp | matched |  |  | Investigated; clean, no ownership metadata gap. See docs/copilot/header-reports/xblidpauthmanager.h.md |
| `xenroll.h` | Certificates | remaining |  |  |  |
| `xinput.h` | Xinput | matched |  |  | Investigated; clean, no ownership metadata gap. See docs/copilot/header-reports/xinput.h.md |
| `xmllite.h` | FileHistory, XmlLite | remaining |  |  |  |
| `xolehlp.h` | DTC | matched |  | 09/02/2026 20:38:46 | All outputs are standard COM QueryInterface-style interface pointers. |
| `xprtdefs.h` | Media.DShow | remaining |  |  |  |
| `xpsdigitalsignature.h` | Xps | remaining |  |  |  |
| `xpsobjectmodel_1.h` | Xps | remaining |  |  |  |
| `xpsobjectmodel_2.h` | Xps | matched |  |  | Investigated; clean, no ownership metadata gap. See docs/copilot/header-reports/xpsobjectmodel_2.h.md |
| `xpsobjectmodel.h` | Xps | remaining |  |  |  |
| `xpsprint.h` | Gdi, XpsPrinting | matched |  |  | Investigated; clean, no ownership metadata gap. See docs/copilot/header-reports/xpsprint.h.md |
| `xpsrassvc.h` | Printing | matched |  |  | Investigated; clean, no ownership metadata gap. See docs/copilot/header-reports/xpsrassvc.h.md |
