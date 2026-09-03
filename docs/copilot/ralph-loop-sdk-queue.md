# Ralph Loop SDK Header Queue

- Generated: 2026-09-03T01:09:49Z
- Source: `generation/WinSDK/patches/header-progress.json` (authoritative, one row per unique header)
- Total headers: 1403
- Matched: 370
- In progress: 0
- Blocked: 7
- Remaining: 1026

| Header | Partition(s) | Status | Owner | Last Updated | Notes |
|---|---|---|---|---|---|
| `accctrl.h` | Authorization, Authorization.UI, Security, TransactionServer | remaining |  |  |  |
| `accountssettingspaneinterop.h` | WinRT | remaining |  |  |  |
| `aclapi.h` | Authorization, Authorization.UI | remaining |  |  |  |
| `aclui.h` | Authorization, Authorization.UI | remaining |  |  |  |
| `activation.h` | WinRT | remaining |  |  |  |
| `activaut.h` | Debug.ActiveScript | remaining |  |  |  |
| `activdbg.h` | Debug.ActiveScript | remaining |  |  |  |
| `activdbg100.h` | Debug.ActiveScript | remaining |  |  |  |
| `activecf.h` | Media.DShow | matched |  | 09/02/2026 17:09:31 | Clipboard-format data struct only, no functions. |
| `activprof.h` | Debug.ActiveScript | remaining |  |  |  |
| `adhoc.h` | Ndis, NWifi | remaining |  |  |  |
| `adshlp.h` | ActiveDirectory | remaining |  |  |  |
| `adsprop.h` | ActiveDirectory | remaining |  |  |  |
| `adssts.h` | ActiveDirectory | remaining |  |  |  |
| `adtgen.h` | Authorization, Authorization.UI | remaining |  |  |  |
| `advpub.h` | FileHistory, WinProg | remaining |  |  |  |
| `af_irda.h` | WinSock | remaining |  |  |  |
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
| `amaudio.h` | Media.DShow | remaining |  |  |  |
| `amparse.h` | Media.DShow | matched |  | 09/02/2026 17:27:04 | COM interface with plain integer output, no handle. |
| `amsi.h` | Antimalware | matched | copilot | 09/03/2026 03:35:00 | New resource-ownership patch created and verified via live re-scrape (build-level validation). |
| `amstream.h` | Media.DShow, Media.DShow.Xml | remaining |  |  |  |
| `amva.h` | Media.DShow | remaining |  |  |  |
| `amvideo.h` | Media.DShow | remaining |  |  |  |
| `amxmlgraphbuilder.h` | Media.DShow, Media.DShow.Xml | matched |  | 09/02/2026 17:13:02 | COM interface + GUID constants only. |
| `anchorsyncdeviceservice.h` | WpdSdk | remaining |  |  |  |
| `animationcoordinator.h` | Shell | remaining |  |  |  |
| `apdevpkey.h` | Base, Devices.Properties | matched |  | 09/02/2026 17:05:19 | Device property key constant only, no functions. |
| `apiquery2.h` | FileHistory, WinProg | matched |  | 09/02/2026 17:41:54 | Caller-allocated string buffer output only. |
| `appcompatapi.h` | FileHistory, WinProg | matched |  | 09/02/2026 17:23:42 | No handle involved. |
| `appmgmt.h` | Policy, Shell | matched | copilot | 09/02/2026 19:25:46 | Classified retained artifact in existing-patches-01. |
| `appmodel.h` | AppxPackaging | remaining |  |  |  |
| `appnotify.h` | Shell | remaining |  |  |  |
| `appserviceinterop.h` | WinRT | remaining |  |  |  |
| `appxpackaging.h` | AppxPackaging | remaining |  |  |  |
| `asferr.h` | Multimedia | remaining |  |  |  |
| `atacct.h` | NetMgmt | matched |  | 09/02/2026 17:09:31 | String-only account-info API, no handle. |
| `atscpsipparser.h` | MsTv | remaining |  |  |  |
| `atsmedia.h` | MsTv | matched |  | 09/02/2026 17:27:04 | GUID constant only, no functions. |
| `audevcod.h` | Media.DShow | matched |  | 09/02/2026 17:48:13 | Error-code enum/constants only, no functions. |
| `AudioAPOTypes.h` | Audio.Apo, TermServ | remaining |  |  |  |
| `audioclient.h` | Audio | remaining |  |  |  |
| `audioclientactivationparams.h` | Audio | remaining |  |  |  |
| `audioendpoints.h` | Audio.Endpoints | remaining |  |  |  |
| `audioenginebaseapo.h` | Audio.Apo | remaining |  |  |  |
| `audioengineendpoint.h` | Audio.Endpoints, TermServ | remaining |  |  |  |
| `audioengineextensionapo.h` | Audio.Apo | remaining |  |  |  |
| `audiomediatype.h` | Audio.Apo | remaining |  |  |  |
| `audiopolicy.h` | Audio | remaining |  |  |  |
| `audiosessiontypes.h` | Audio | remaining |  |  |  |
| `audiostatemonitorapi.h` | Audio | remaining |  |  |  |
| `austream.h` | Media.DShow | remaining |  |  |  |
| `authif.h` | Nps | remaining |  |  |  |
| `authz.h` | Authorization, Authorization.UI | matched | copilot | 09/02/2026 21:45:00 | Corrected typedef-owned ownership violation found during resource-ownership audit (12 producer sites across 6 handle types). |
| `aux_ulib.h` | FileHistory, WinProg | matched | copilot | 09/02/2026 19:25:46 | Classified retained artifact in existing-patches-01. |
| `avifmt.h` | Media.DShow | remaining |  |  |  |
| `aviriff.h` | Media.DShow | remaining |  |  |  |
| `avrfsdk.h` | ApplicationVerifier, FileHistory | remaining |  |  |  |
| `avrt.h` | Threading | remaining |  |  |  |
| `azroles.h` | Authorization, Authorization.UI | remaining |  |  |  |
| `batclass.h` | Power | remaining |  |  |  |
| `bcp47mrm.h` | Intl | matched |  | 09/02/2026 16:53:36 | No handle-producing functions. |
| `bcrypt_provider.h` | Security.Cryptography | remaining |  |  |  |
| `bcrypt.h` | Security.Cryptography, Security.Cryptography.UI | matched | copilot | 09/02/2026 21:45:00 | Corrected typedef-owned ownership violation found during resource-ownership audit; unrelated zz-crypto-security-enums patch verified compatible via full sequential forward replay. |
| `bdaiface.h` | Media.DShow, MsTv | remaining |  |  |  |
| `bdamedia.h` | MsTv | remaining |  |  |  |
| `bdatif.h` | MsTv | remaining |  |  |  |
| `bdatypes.h` | Media.DShow, MsTv | remaining |  |  |  |
| `BiDiSpl.h` | Printing | remaining |  |  |  |
| `bindlink.h` | Fs | matched |  | 09/02/2026 17:30:54 | Path-string-only API, no handle. |
| `bits.h` | Bits | remaining |  |  |  |
| `bits1_5.h` | Bits | remaining |  |  |  |
| `bits10_1.h` | Bits | remaining |  |  |  |
| `bits10_2.h` | Bits | remaining |  |  |  |
| `bits10_3.h` | Bits | remaining |  |  |  |
| `bits2_0.h` | Bits | remaining |  |  |  |
| `bits2_5.h` | Bits | remaining |  |  |  |
| `bits3_0.h` | Bits | remaining |  |  |  |
| `bits4_0.h` | Bits | remaining |  |  |  |
| `bits5_0.h` | Bits | remaining |  |  |  |
| `bitscfg.h` | Bits | remaining |  |  |  |
| `bitsmsg.h` | Bits | remaining |  |  |  |
| `bluetoothapis.h` | Bluetooth | matched | copilot | 09/02/2026 23:05:00 | Classified retained artifact in existing-patches-02. |
| `bluetoothleapis.h` | Bluetooth | remaining |  |  |  |
| `bridgedeviceservice.h` | WpdSdk | remaining |  |  |  |
| `bthdef.h` | Bluetooth | remaining |  |  |  |
| `bthledef.h` | Bluetooth | remaining |  |  |  |
| `bthsdpdef.h` | Bluetooth | remaining |  |  |  |
| `bugcodes.h` | Debug | remaining |  |  |  |
| `calendarapis.h` | Intl | matched |  | 09/02/2026 17:30:54 | Calendar-date-struct-only API, no handle. |
| `calendardeviceservice.h` | WpdSdk | remaining |  |  |  |
| `callobj.h` | Com.CallObj, ComOle | matched | copilot | 09/02/2026 19:25:46 | Classified retained artifact in existing-patches-01. |
| `camerauicontrol.h` | FileHistory, Media.DShow, Mf, WinProg | remaining |  |  |  |
| `capi.h` | FileHistory, WinProg | matched |  | 09/02/2026 17:45:25 | JAVA_TRUST plain data struct; all redirected headers already accepted or separately tracked pending; sipbase.h verified as trivial redirect to accepted mssip.h. |
| `cardmod.h` | Security.Cryptography | remaining |  |  |  |
| `casetup.h` | Security.Cryptography, Security.Cryptography.UI | remaining |  |  |  |
| `CastingInterop.h` | WinRT | remaining |  |  |  |
| `ccgplugins.h` | Identity | remaining |  |  |  |
| `cchannel.h` | TermServ | remaining |  |  |  |
| `cderr.h` | DlgBox | remaining |  |  |  |
| `celib.h` | Security | remaining |  |  |  |
| `certadm.h` | Certificates | remaining |  |  |  |
| `certbcli.h` | Certificates | remaining |  |  |  |
| `certcli.h` | Certificates | remaining |  |  |  |
| `certenc.h` | Certificates | matched | copilot | 09/02/2026 19:25:46 | Classified retained artifact in existing-patches-01. |
| `certenroll.h` | Certificates | remaining |  |  |  |
| `certexit.h` | Certificates | remaining |  |  |  |
| `certif.h` | Certificates | remaining |  |  |  |
| `certmod.h` | Certificates | remaining |  |  |  |
| `certpol.h` | Certificates | remaining |  |  |  |
| `certpoleng.h` | Certificates | remaining |  |  |  |
| `certreqd.h` | Certificates | remaining |  |  |  |
| `certsrv.h` | Certificates | remaining |  |  |  |
| `certview.h` | Certificates | remaining |  |  |  |
| `cfapi.h` | Cloudapi | remaining |  |  |  |
| `cfg.h` | DevInst | remaining |  |  |  |
| `cfgmgr32.h` | DevInst | matched | copilot | 09/02/2026 19:25:46 | Classified retained artifact in existing-patches-01. |
| `cierror.h` | Foundation | remaining |  |  |  |
| `clfs.h` | Fs | remaining |  |  |  |
| `clfsmgmt.h` | Fs | remaining |  |  |  |
| `clfsmgmtw32.h` | Fs | matched | copilot | 09/02/2026 23:05:00 | Classified retained artifact in existing-patches-02. |
| `clfsw32.h` | Fs | matched | copilot | 09/02/2026 23:05:00 | Classified retained artifact in existing-patches-02. |
| `cloneviewhelper.h` | Monitor | remaining |  |  |  |
| `cluadmex.h` | MsCs | remaining |  |  |  |
| `cmnquery.h` | ActiveDirectory | remaining |  |  |  |
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
| `computecore.h` | HostComputeSystem | remaining |  |  |  |
| `computenetwork.h` | HostComputeNetwork | remaining |  |  |  |
| `computestorage.h` | HostComputeSystem | remaining |  |  |  |
| `comsvcs.h` | Com.Events, Cos | matched | copilot | 09/02/2026 23:50:00 | Classified retained artifact in existing-patches-04. |
| `consoleapis.h` | Console | matched |  | 09/02/2026 17:23:42 | HANDLE/HWND fields are caller-supplied input references, not produced/owned here; distinct from the wincon.h blocker (different functions). |
| `contactaggregation.h` | WinContacts | remaining |  |  |  |
| `contactdeviceservice.h` | WpdSdk | remaining |  |  |  |
| `contentpartner.h` | Wmp | remaining |  |  |  |
| `control.h` | Media.DShow | matched | copilot | 09/02/2026 23:50:00 | Classified retained artifact in existing-patches-04. |
| `cor.h` | ClrProfiling | remaining |  |  |  |
| `corecrt_io.h` | Multimedia | remaining |  |  |  |
| `CoreWindow.h` | WinRT | remaining |  |  |  |
| `CorHdr.h` | ClrProfiling | remaining |  |  |  |
| `corprof.h` | ClrProfiling | remaining |  |  |  |
| `cpl.h` | Shell | remaining |  |  |  |
| `cplext.h` | Shell | remaining |  |  |  |
| `credentialprovider.h` | Shell | matched | copilot | 09/02/2026 23:50:00 | Classified retained artifact in existing-patches-04. |
| `credssp.h` | Credentials | remaining |  |  |  |
| `cryptcat.h` | Security.Cryptography.Catalog | matched |  | 09/02/2026 17:16:10 | Consumer-only functions (enumerate existing CRYPTCATCDF); producer pair lives in mscat.h (separately tracked, pending). |
| `cryptdlg.h` | Security.Cryptography, Security.Cryptography.UI | remaining |  |  |  |
| `cryptuiapi.h` | Security.Cryptography, Security.Cryptography.UI | matched | copilot | 09/02/2026 23:50:00 | Classified retained artifact in existing-patches-04. |
| `cryptxml.h` | Security.Cryptography, Security.Cryptography.UI | matched | copilot | 09/02/2026 23:50:00 | Classified retained artifact in existing-patches-04. |
| `cscapi.h` | Of | remaining |  |  |  |
| `cscobj.h` | Of | remaining |  |  |  |
| `cspdk.h` | Security.Cryptography | remaining |  |  |  |
| `ctffunc.h` | Tsf | remaining |  |  |  |
| `ctfspui.h` | Tsf | remaining |  |  |  |
| `ctfutb.h` | Tsf | remaining |  |  |  |
| `ctxtcall.h` | Com, ComOle | matched | copilot | 09/03/2026 00:10:00 | Classified retained artifact in existing-patches-05. |
| `custcntl.h` | Controls | remaining |  |  |  |
| `d2d1_1.h` | Direct2D | matched | copilot | 09/03/2026 00:10:00 | Classified retained artifact in existing-patches-05. |
| `d2d1_2.h` | Direct2D | matched | copilot | 09/03/2026 00:10:00 | Classified retained artifact in existing-patches-05. |
| `d2d1_3.h` | Direct2D | matched | copilot | 09/03/2026 00:10:00 | Classified retained artifact in existing-patches-05. |
| `d2d1.h` | Direct2D | matched | copilot | 09/03/2026 00:10:00 | Classified retained artifact in existing-patches-05. |
| `d2d1effectauthor_1.h` | Direct2D | remaining |  |  |  |
| `d2d1effectauthor.h` | Direct2D | matched | copilot | 09/03/2026 00:25:00 | Classified retained artifact in existing-patches-06. |
| `d2d1effects_1.h` | Direct2D | remaining |  |  |  |
| `d2d1effects_2.h` | Direct2D | remaining |  |  |  |
| `d2d1effects.h` | Direct2D | remaining |  |  |  |
| `d2d1svg.h` | Direct2D | remaining |  |  |  |
| `d2dbasetypes.h` | Direct2D | matched |  | 09/02/2026 17:05:19 | Type alias only, no functions. |
| `d2derr.h` | Direct2D | matched |  | 09/02/2026 17:48:13 | HRESULT error-code macros only, no functions. |
| `d3d10_1.h` | Direct3D10 | remaining |  |  |  |
| `d3d10_1shader.h` | Direct3D10 | remaining |  |  |  |
| `d3d10.h` | Direct3D10 | remaining |  |  |  |
| `d3d10effect.h` | Direct3D10 | remaining |  |  |  |
| `d3d10misc.h` | Direct3D10 | remaining |  |  |  |
| `d3d10sdklayers.h` | Direct3D10 | remaining |  |  |  |
| `d3d10shader.h` | Direct3D10 | remaining |  |  |  |
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
| `d3d12compatibility.h` | Direct3D12 | remaining |  |  |  |
| `d3d12compiler.h` | Direct3D12 | remaining |  |  |  |
| `d3d12sdklayers.h` | Direct3D12 | remaining |  |  |  |
| `d3d12shader.h` | Direct3D12 | remaining |  |  |  |
| `d3d12video.h` | Mf | matched | copilot | 09/03/2026 00:55:00 | Classified retained artifact in existing-patches-08. |
| `d3d9.h` | Direct3D9, Mf | matched | copilot | 09/03/2026 00:55:00 | Classified retained artifact in existing-patches-08. |
| `d3d9caps.h` | Direct3D9, Mf | remaining |  |  |  |
| `d3d9helper.h` | Media.DShow | remaining |  |  |  |
| `d3d9on12.h` | Direct3D9on12 | remaining |  |  |  |
| `d3d9types.h` | Direct3D9, Mf | remaining |  |  |  |
| `d3dcaps.h` | Direct3D9 | remaining |  |  |  |
| `d3dcommon.h` | Direct2D, Direct3D, Direct3D10 | remaining |  |  |  |
| `d3dcompiler.h` | Direct3DFxc | matched | copilot | 09/03/2026 04:20:00 | No patch needed; COM interface only. |
| `d3dcsx.h` | Direct3D11 | remaining |  |  |  |
| `d3dshadercacheregistration.h` | Direct3D | remaining |  |  |  |
| `d3dtypes.h` | Direct3D9 | remaining |  |  |  |
| `datetimeapi.h` | Intl | remaining |  |  |  |
| `davclnt.h` | WebDav | remaining |  |  |  |
| `DbgEng.h` | Debug.Extensions | remaining |  |  |  |
| `dbghelp.h` | FileHistory, WinProg | remaining |  |  |  |
| `DbgModel.h` | Debug.Extensions | remaining |  |  |  |
| `dbgprop.h` | Debug | remaining |  |  |  |
| `dbt.h` | MenuRc | remaining |  |  |  |
| `dciddi.h` | Direct2D | remaining |  |  |  |
| `dciman.h` | FileHistory, WinProg | remaining |  |  |  |
| `dcommon.h` | Direct2D, Direct2D.Common, DirectWrite | remaining |  |  |  |
| `dcomp.h` | DirectComp | matched | copilot | 09/03/2026 00:55:00 | Classified retained artifact in existing-patches-08. |
| `dcompanimation.h` | DirectComp | matched | copilot | 09/03/2026 00:55:00 | Classified retained artifact in existing-patches-08. |
| `dcomptypes.h` | DirectComp | remaining |  |  |  |
| `dde.h` | DataXchg | remaining |  |  |  |
| `ddeml.h` | DataXchg | remaining |  |  |  |
| `ddkernel.h` | DirectDraw | remaining |  |  |  |
| `ddpbackup.h` | Dedup | remaining |  |  |  |
| `ddpchunk.h` | Dedup | remaining |  |  |  |
| `ddpcommon.h` | Dedup | remaining |  |  |  |
| `ddpdataport.h` | Dedup | remaining |  |  |  |
| `ddraw.h` | DirectDraw, Gdiplus, Media.DShow | matched |  | 09/02/2026 16:34:06 | HMONITOR reference is a pass-through system handle (never created/released by DirectDraw); guarded DECLARE_HANDLE fallback never fires. |
| `ddrawgdi.h` | FileHistory, WinProg | remaining |  |  |  |
| `ddrawi.h` | DirectDraw, FileHistory, WinProg | remaining |  |  |  |
| `ddrawint.h` | DirectDraw | remaining |  |  |  |
| `ddstream.h` | Media.DShow | remaining |  |  |  |
| `debugapi.h` | Base, Debug | remaining |  |  |  |
| `defaultbrowsersyncsettings.h` | WinProg | remaining |  |  |  |
| `delayloadhandler.h` | WinProg | matched |  | 09/02/2026 17:27:04 | Structs/callback-typedef/extern-variable only, no functions. |
| `deletebrowsinghistory.h` | WinProg | remaining |  |  |  |
| `deliveryoptimization.h` | DeliveryOptimization | matched | copilot | 09/03/2026 03:20:00 | No patch needed; verified via live scrape and static review. |
| `devfiltertypes.h` | DeviceQuery | remaining |  |  |  |
| `devguid.h` | DevInst | remaining |  |  |  |
| `deviceaccess.h` | DeviceAccess | remaining |  |  |  |
| `deviceservices.h` | WpdSdk | matched |  | 09/02/2026 17:58:11 | GUID/property-key constants only, no functions; BridgeDeviceService.h separately tracked (pending). |
| `devicetopology.h` | Audio | remaining |  |  |  |
| `devioctl.h` | Devices.1394, Devices.Beep, Devices.Cdrom, Devices.Dvd | remaining |  |  |  |
| `devpkey.h` | Base, Devices.Properties | remaining |  |  |  |
| `devpropdef.h` | Display, Nfc, Nfp, WpdSdk | remaining |  |  |  |
| `devquery.h` | DeviceQuery, FileHistory | remaining |  |  |  |
| `devquerydef.h` | DeviceQuery | remaining |  |  |  |
| `dhcpcsdk.h` | Dhcp | remaining |  |  |  |
| `dhcpsapi.h` | Dhcp | remaining |  |  |  |
| `dhcpssdk.h` | Dhcp | remaining |  |  |  |
| `dhcpv6csdk.h` | Dhcp | remaining |  |  |  |
| `diagnosticdataquery.h` | Security.DiagnosticDataQuery | remaining |  |  |  |
| `diagnosticdataquerytypes.h` | Security.DiagnosticDataQuery | remaining |  |  |  |
| `digitalv.h` | Multimedia | remaining |  |  |  |
| `dimm.h` | Input.Ime | remaining |  |  |  |
| `dinput.h` | Hid | remaining |  |  |  |
| `dinputd.h` | Hid | remaining |  |  |  |
| `directmanipulation.h` | DirectManipulation | matched | copilot | 09/03/2026 04:35:00 | No patch needed; COM interface only. |
| `directml.h` | DirectML | matched | copilot | 09/03/2026 04:20:00 | No patch needed; COM interface only. |
| `diskguid.h` | Fs | remaining |  |  |  |
| `dispatcherqueue.h` | WinRT | remaining |  |  |  |
| `DispEx.h` | ComOle | matched | copilot | 09/03/2026 01:10:00 | Classified retained artifact in existing-patches-09. |
| `dlnadeviceinterfaceids.h` | Dlna | matched | copilot | 09/03/2026 03:20:00 | No patch needed; constant-only header verified via live scrape. |
| `dlnametadataproviderproperties.h` | Dlna | matched | copilot | 09/03/2026 03:20:00 | No patch needed; constant-only header verified via live scrape. |
| `dls1.h` | Audio.DirectMusic | remaining |  |  |  |
| `dls2.h` | Audio.DirectMusic | remaining |  |  |  |
| `dmemmgr.h` | DirectDraw | remaining |  |  |  |
| `dmerror.h` | TransactionServer | remaining |  |  |  |
| `dmodshow.h` | Media.DShow | remaining |  |  |  |
| `dmoreg.h` | Media.DxMediaObjects | remaining |  |  |  |
| `dmort.h` | Media.DxMediaObjects | remaining |  |  |  |
| `dmprocessxmlfiltered.h` | WpdSdk | matched |  | 09/02/2026 17:38:44 | BSTR output, not HANDLE-family. |
| `dmusics.h` | Audio.DirectMusic | remaining |  |  |  |
| `docobj.h` | Com, Com.CallObj, Com.ChannelCreds, Com.Urlmon, ComOle, TransactionServer | matched | copilot | 09/03/2026 01:10:00 | Classified retained artifact in existing-patches-09. |
| `docobjectservice.h` | InternetExplorer | remaining |  |  |  |
| `DocumentSource.h` | WinRT.Printing | remaining |  |  |  |
| `documenttarget.h` | XpsPrinting | remaining |  |  |  |
| `dot1x.h` | Ndis, NWifi | remaining |  |  |  |
| `downloadmgr.h` | InternetExplorer | remaining |  |  |  |
| `dpa_dsa.h` | Controls | matched | copilot | 09/03/2026 01:10:00 | Classified retained artifact in existing-patches-09. |
| `dpapi.h` | Security.Cryptography, Security.Cryptography.UI | remaining |  |  |  |
| `dragdropinterop.h` | WinRT | remaining |  |  |  |
| `drt.h` | P2p | matched | copilot | 09/03/2026 01:10:00 | Classified retained artifact in existing-patches-09. |
| `dsadmin.h` | ActiveDirectory | remaining |  |  |  |
| `dsattrib.h` | MsTv | remaining |  |  |  |
| `dsclient.h` | ActiveDirectory | remaining |  |  |  |
| `dsconf.h` | Audio.DirectMusic | remaining |  |  |  |
| `DsGetDC.h` | ActiveDirectory | remaining |  |  |  |
| `dshow.h` | Media.DShow | remaining |  |  |  |
| `dshowasf.h` | Media.DShow | remaining |  |  |  |
| `dskquota.h` | Fs, Shell | matched | copilot | 09/03/2026 01:10:00 | Classified retained artifact in existing-patches-09. |
| `dsound.h` | Audio.DirectSound, FileHistory, WinProg | remaining |  |  |  |
| `dsparse.h` | ActiveDirectory | remaining |  |  |  |
| `dsquery.h` | ActiveDirectory | remaining |  |  |  |
| `dsrole.h` | ActiveDirectory | remaining |  |  |  |
| `dssec.h` | Security.DirectoryServices | remaining |  |  |  |
| `dtchelp.h` | DTC | remaining |  |  |  |
| `dv.h` | Audio.DirectMusic | remaining |  |  |  |
| `dvbsiparser.h` | MsTv | remaining |  |  |  |
| `dvdevcod.h` | Media.DShow | remaining |  |  |  |
| `dvdmedia.h` | Media.DShow | matched | copilot | 09/03/2026 01:25:00 | Classified retained artifact in existing-patches-10. |
| `dvp.h` | DirectDraw | remaining |  |  |  |
| `dwmapi.h` | Dwm | matched | copilot | 09/03/2026 01:25:00 | Classified retained artifact in existing-patches-10. |
| `dwrite_1.h` | DirectWrite | matched | copilot | 09/03/2026 01:25:00 | Classified retained artifact in existing-patches-10. |
| `dwrite_2.h` | DirectWrite | matched | copilot | 09/03/2026 01:25:00 | Classified retained artifact in existing-patches-10. |
| `dwrite_3.h` | DirectWrite | matched | copilot | 09/03/2026 01:40:00 | Classified retained artifact in existing-patches-11. |
| `dwrite.h` | DirectWrite | matched | copilot | 09/03/2026 01:25:00 | Classified retained artifact in existing-patches-10. |
| `dxcapi.h` | Direct3DDxc | matched | copilot | 09/03/2026 04:20:00 | No patch needed; COM interface only. |
| `dxcore_interface.h` | Debug, DXCore | remaining |  |  |  |
| `dxcore.h` | Debug, DXCore | matched |  | 09/02/2026 17:23:42 | COM factory pattern (DXCoreCreateAdapterFactory). |
| `dxgi.h` | Dxgi, Dxgi.Common | matched | copilot | 09/03/2026 01:40:00 | Classified retained artifact in existing-patches-11. |
| `dxgi1_2.h` | Dxgi, Dxgi.Common | matched | copilot | 09/03/2026 01:40:00 | Classified retained artifact in existing-patches-11. |
| `dxgi1_3.h` | Dxgi, Dxgi.Common | matched | copilot | 09/03/2026 01:40:00 | Classified retained artifact in existing-patches-11. |
| `dxgi1_4.h` | Dxgi, Dxgi.Common | matched | copilot | 09/03/2026 01:40:00 | Classified retained artifact in existing-patches-11. |
| `dxgi1_5.h` | Dxgi, Dxgi.Common | matched | copilot | 09/03/2026 02:00:00 | Classified retained artifact in existing-patches-12. |
| `dxgi1_6.h` | Dxgi, Dxgi.Common | matched | copilot | 09/03/2026 02:00:00 | Classified retained artifact in existing-patches-12. |
| `dxgicommon.h` | Dxgi, Dxgi.Common | remaining |  |  |  |
| `dxgidebug.h` | Dxgi, Dxgi.Common | matched | copilot | 09/03/2026 02:00:00 | Classified retained artifact in existing-patches-12. |
| `dxgiformat.h` | Dxgi, Dxgi.Common | remaining |  |  |  |
| `dxgimessages.h` | Dxgi, Dxgi.Common | remaining |  |  |  |
| `dxgitype.h` | Dxgi, Dxgi.Common | remaining |  |  |  |
| `dxmini.h` | DirectDraw | remaining |  |  |  |
| `dxprogrammablecapture.h` | Dxgi, Dxgi.Common | matched |  | 09/02/2026 17:45:25 | Parameterless COM interface methods, no handle. |
| `dxva.h` | Media.DShow, Mf | remaining |  |  |  |
| `dxva2api.h` | Media.DShow, Mf | remaining |  |  |  |
| `dxva2swdev.h` | Media.DShow | remaining |  |  |  |
| `dxva2trace.h` | Media.DShow | remaining |  |  |  |
| `dxva9typ.h` | Media.DShow, Mf | matched | copilot | 09/03/2026 02:00:00 | Resolves the replay-mismatch flagged in shared-handle-policy-01; verified via isolated round-trip in this session (no fresh SDK checkout available to rerun the original full-replay evidence). |
| `dxvahd.h` | Mf | remaining |  |  |  |
| `eapauthenticatoractiondefine.h` | EapHost | remaining |  |  |  |
| `eapauthenticatortypes.h` | EapHost | matched |  | 09/02/2026 18:03:29 | Enum + MIDL boilerplate only, no functions. |
| `eaphosterror.h` | EapHost | remaining |  |  |  |
| `eaphostpeerconfigapis.h` | EapHost | remaining |  |  |  |
| `eaphostpeertypes.h` | EapHost | remaining |  |  |  |
| `eapmethodauthenticatorapis.h` | EapHost | remaining |  |  |  |
| `eapmethodpeerapis.h` | EapHost | remaining |  |  |  |
| `eapmethodtypes.h` | EapHost | matched |  | 09/02/2026 17:30:54 | Type declarations only (struct/enum/typedef), no functions; producer functions tracked separately under eapmethodpeerapis.h (pending). |
| `eappapis.h` | EapHost | remaining |  |  |  |
| `eaptypes.h` | EapHost | remaining |  |  |  |
| `edevdefs.h` | DeviceAccess | remaining |  |  |  |
| `editionupgradehelper.h` | FileHistory, WinProg | remaining |  |  |  |
| `edpwin32.h` | Edp | matched |  | 09/02/2026 17:19:28 | Path-string-only API, no handle. |
| `effects.h` | Wmp | remaining |  |  |  |
| `efswrtinterop.h` | Edp | remaining |  |  |  |
| `ehstorapi.h` | Enstor | remaining |  |  |  |
| `ehstorextensions.h` | Enstor | remaining |  |  |  |
| `ehstormsg.h` | Enstor | remaining |  |  |  |
| `elscore.h` | Intl | remaining |  |  |  |
| `elssrvc.h` | Intl | matched |  | 09/02/2026 18:03:29 | GUID constants only, no functions. |
| `emi.h` | Power | remaining |  |  |  |
| `emptyvc.h` | Lwef | remaining |  |  |  |
| `encdec.h` | MsTv | remaining |  |  |  |
| `endpointvolume.h` | Audio.Endpoints | remaining |  |  |  |
| `errhandlingapi.h` | Base, Debug | matched | copilot | 09/03/2026 02:15:00 | Classified retained artifact in existing-patches-13. |
| `errorrep.h` | Wer | matched |  | 09/02/2026 18:00:52 | HWND is a caller-supplied input, not produced here. |
| `errors.h` | Media.DShow | matched |  | 09/02/2026 17:38:44 | Caller-allocated string buffer output only, no handle. |
| `esent.h` | Storage.Jet | blocked | copilot | 09/03/2026 04:45:00 | Deferred: 229-function header with complex many-to-many JET handle ownership graph requiring dedicated research pass, not a quick classification. |
| `evalcom2.h` | Setup | matched |  | 09/02/2026 18:06:26 | COM interface with caller-supplied context pointers only, no handle. |
| `evcode.h` | Media.DShow | remaining |  |  |  |
| `evcoll.h` | Wec | remaining |  |  |  |
| `eventsys.h` | Com.Events, TransactionServer | matched | copilot | 09/03/2026 02:15:00 | Classified retained artifact in existing-patches-13. |
| `evntcons.h` | Etw | remaining |  |  |  |
| `evntprov.h` | Etw | remaining |  |  |  |
| `evntrace.h` | Etw, Media.DShow | matched | copilot | 09/03/2026 02:15:00 | Classified retained artifact in existing-patches-13. |
| `evr.h` | Mf | remaining |  |  |  |
| `evr9.h` | Mf | remaining |  |  |  |
| `exdisp.h` | FileHistory, Shell, WinProg | matched | copilot | 09/03/2026 02:15:00 | Classified retained artifact in existing-patches-13. |
| `exdispid.h` | InternetExplorer | remaining |  |  |  |
| `expandedresources.h` | GameMode | matched |  | 09/02/2026 17:16:10 | No handle-typed values anywhere in this header. |
| `exposeenums2managed.h` | FileHistory, WinProg | remaining |  |  |  |
| `extensionvalidation.h` | InternetExplorer | remaining |  |  |  |
| `extsfns.h` | Debug.Extensions | remaining |  |  |  |
| `faxcomex.h` | Fax | remaining |  |  |  |
| `faxdev.h` | Fax | remaining |  |  |  |
| `faxext.h` | Fax | remaining |  |  |  |
| `faxmmc.h` | Fax | matched |  | 09/02/2026 17:58:11 | GUID/string constants only, no functions. |
| `faxroute.h` | Fax | remaining |  |  |  |
| `fci.h` | Cabinets | remaining |  |  |  |
| `fdi_fci_types.h` | Cabinets | remaining |  |  |  |
| `fdi.h` | Cabinets | remaining |  |  |  |
| `featurestagingapi.h` | FileHistory, WinProg | remaining |  |  |  |
| `fhcfg.h` | FileHistory | remaining |  |  |  |
| `fherrors.h` | FileHistory | remaining |  |  |  |
| `fhstatus.h` | FileHistory | matched |  | 09/02/2026 18:00:53 | Status-code constants only, no functions. |
| `fhsvcctl.h` | FileHistory | remaining |  |  |  |
| `fileapi.h` | Fs | matched | copilot | 09/03/2026 02:15:00 | Classified retained artifact in existing-patches-13. |
| `filehc.h` | FileHistory, Fs | matched | copilot | 09/03/2026 02:30:00 | Classified retained artifact in existing-patches-14. |
| `filter.h` | IndexSrv, Search | remaining |  |  |  |
| `filtereg.h` | Search | remaining |  |  |  |
| `filterpipeline.h` | Printing | remaining |  |  |  |
| `filterr.h` | IndexSrv | remaining |  |  |  |
| `fltdefs.h` | FileHistory, IpHlp | remaining |  |  |  |
| `fltuser.h` | Ifsk | remaining |  |  |  |
| `fontsub.h` | Gdi | remaining |  |  |  |
| `fsrm.h` | Fsrm | remaining |  |  |  |
| `fsrmenums.h` | Fsrm | remaining |  |  |  |
| `fsrmerr.h` | Fsrm | remaining |  |  |  |
| `fsrmpipeline.h` | Fsrm | remaining |  |  |  |
| `fsrmquota.h` | Fsrm | remaining |  |  |  |
| `fsrmreports.h` | Fsrm | remaining |  |  |  |
| `fsrmscreen.h` | Fsrm | remaining |  |  |  |
| `fsrmtlb.h` | Fsrm | remaining |  |  |  |
| `ftpext.h` | Iis | remaining |  |  |  |
| `fttypes.h` | Fs | remaining |  |  |  |
| `fullenumsyncdeviceservice.h` | WpdSdk | remaining |  |  |  |
| `functiondiscovery.h` | FunctionDiscovery | remaining |  |  |  |
| `functiondiscoverycategories.h` | FunctionDiscovery | remaining |  |  |  |
| `functiondiscoveryconstraints.h` | FunctionDiscovery | remaining |  |  |  |
| `functiondiscoveryerror.h` | FunctionDiscovery | matched |  | 09/02/2026 18:03:29 | HRESULT error-code constants only, no functions. |
| `functiondiscoverykeys_devpkey.h` | Audio | remaining |  |  |  |
| `functiondiscoveryprovider.h` | FunctionDiscovery, WebServicesOnDevices | remaining |  |  |  |
| `fwpmtypes.h` | WindowsFilteringPlatform | remaining |  |  |  |
| `fwpmu.h` | WindowsFilteringPlatform | remaining |  |  |  |
| `fwptypes.h` | WindowsFilteringPlatform | remaining |  |  |  |
| `fxsutility.h` | Fax | matched |  | 09/02/2026 17:19:28 | No handle involved. |
| `GameInput.h` | GameInput | matched |  | 09/02/2026 16:34:06 | GameInputCreate is a clean COM-factory pattern; the one raw HANDLE out-param (IGameInputDispatcher::OpenWaitHandle) is a COM vtable method, outside DllImport-based ownership-annotation scope. |
| `gameux.h` | GameMode | remaining |  |  |  |
| `gamingdeviceinformation.h` | GamingDvcInfo | matched |  | 09/02/2026 18:06:26 | Plain struct output, no handle. |
| `gamingtcui.h` | Tcui | remaining |  |  |  |
| `gb18030.h` | Intl | remaining |  |  |  |
| `gdiplus.h` | Gdiplus | matched |  | 09/02/2026 16:34:06 | GDI+ create/delete object pattern uses strongly-typed C++ pointers (GpGraphics* etc), not HANDLE-family typedefs; no pre-existing RAII metadata to correct; out of current policy scope. |
| `gdipluseffects.h` | Media.DShow | remaining |  |  |  |
| `genericusbfnioctl.h` | Buses | remaining |  |  |  |
| `getcurrentpackageinfo3.h` | AppxPackaging | matched |  | 09/02/2026 16:53:36 | Buffer-fill API, no handle production. |
| `getprocesshandlefromhwnd.h` | Threading | blocked | copilot | 09/02/2026 16:49:01 | Deferred: return-value HANDLE ownership has no precedent anywhere in the repo or published baseline winmd (confirmed via WinmdUtils dump); requires dedicated policy decision on annotation placement before this can be fixed. |
| `gl/gl.h` | OpenGL | remaining |  |  |  |
| `gl/glu.h` | OpenGL | remaining |  |  |  |
| `gnssdriver.h` | WinLocation | remaining |  |  |  |
| `gpedit.h` | Policy | remaining |  |  |  |
| `gpiobuttontypes.h` | Hid | remaining |  |  |  |
| `gpmgmt.h` | Gpmc | matched | copilot | 09/03/2026 05:15:00 | No patch needed; COM interface only. |
| `guiddef.h` | TransactionServer | remaining |  |  |  |
| `handleapi.h` | FileHistory, Foundation | matched | copilot | 09/03/2026 02:30:00 | Classified retained artifact in existing-patches-14. |
| `hidclass.h` | Hid | remaining |  |  |  |
| `hidpi.h` | Hid | remaining |  |  |  |
| `hidsdi.h` | Hid | remaining |  |  |  |
| `hidusage.h` | Hid | remaining |  |  |  |
| `highlevelmonitorconfigurationapi.h` | Monitor | remaining |  |  |  |
| `hintsdeviceservice.h` | WpdSdk | remaining |  |  |  |
| `holographicspaceinterop.h` | MixedRealityInterop | remaining |  |  |  |
| `homepagesetting.h` | InternetExplorer | remaining |  |  |  |
| `hrtfapoapi.h` | Xaudio2 | remaining |  |  |  |
| `hstring.h` | WinRT | remaining |  |  |  |
| `htiface.h` | InternetExplorer | remaining |  |  |  |
| `htiframe.h` | InternetExplorer | remaining |  |  |  |
| `htmlhelp.h` | HtmlHelp | matched | copilot | 09/03/2026 02:30:00 | Classified retained artifact in existing-patches-14. |
| `http.h` | HttpServer | remaining |  |  |  |
| `httpext.h` | Iis | remaining |  |  |  |
| `httpfilt.h` | Iis | remaining |  |  |  |
| `httprequest.h` | WinHttp | matched | copilot | 09/03/2026 03:35:00 | No patch needed; verified via live scrape and static review. |
| `httprequestid.h` | Iis | matched |  | 09/02/2026 18:09:40 | DISPID constants only, no functions. |
| `httptrace.h` | Iis | remaining |  |  |  |
| `hvsocket.h` | Hypervisor | remaining |  |  |  |
| `hwebcore.h` | Iis | remaining |  |  |  |
| `hypervdevicevirtualization.h` | Hypervisor | remaining |  |  |  |
| `i_cryptasn1tls.h` | Security.Cryptography, Security.Cryptography.UI | remaining |  |  |  |
| `iaccess.h` | TransactionServer | remaining |  |  |  |
| `iadmext.h` | Iis | remaining |  |  |  |
| `iadmw.h` | Iis | remaining |  |  |  |
| `iads.h` | ActiveDirectory, Iis | remaining |  |  |  |
| `icftypes.h` | WindowsFirewall | remaining |  |  |  |
| `ichannelcredentials.h` | Com.ChannelCreds, ComOle | remaining |  |  |  |
| `Icm.h` | Wcs | matched | copilot | 09/03/2026 02:30:00 | Classified retained artifact in existing-patches-14. |
| `icmpapi.h` | IpHlp | matched | copilot | 09/03/2026 02:30:00 | Classified retained artifact in existing-patches-14. |
| `icontact.h` | WinContacts | remaining |  |  |  |
| `icontactproperties.h` | WinContacts | remaining |  |  |  |
| `icontentprefetchertasktrigger.h` | Wsw | remaining |  |  |  |
| `icu.h` | Intl | remaining |  |  |  |
| `icucommon.h` | Intl | matched | copilot | 09/03/2026 02:45:00 | Classified retained artifact in existing-patches-15. |
| `icui18n.h` | Intl | remaining |  |  |  |
| `icwcfg.h` | FileHistory, InternetConnectionWizard | matched | copilot | 09/03/2026 05:00:00 | No patch needed; COM interface only. |
| `identitycommon.h` | IdentityProvider | matched |  | 09/02/2026 17:54:05 | Enum + MIDL boilerplate only, no functions. |
| `identityprovider.h` | IdentityProvider | remaining |  |  |  |
| `identitystore.h` | IdentityProvider | remaining |  |  |  |
| `idispids.h` | ComOle, InternetExplorer | matched |  | 09/02/2026 17:16:10 | DISPID constants only, no functions. |
| `ie12plugin.h` | InternetExplorer | remaining |  |  |  |
| `ieautomation.h` | InternetExplorer | remaining |  |  |  |
| `ieobj.h` | InternetExplorer | matched | copilot | 09/03/2026 02:45:00 | Classified retained artifact in existing-patches-15. |
| `iepmapi.h` | InternetExplorer | matched | copilot | 09/03/2026 02:45:00 | Classified retained artifact in existing-patches-15. |
| `iewebdriver.h` | InternetExplorer | remaining |  |  |  |
| `iextag.h` | InternetExplorer | remaining |  |  |  |
| `il21dec.h` | Media.DShow | remaining |  |  |  |
| `ImageHlp.h` | Debug | remaining |  |  |  |
| `imagetranscode.h` | Shell | matched | copilot | 09/03/2026 02:45:00 | Classified retained artifact in existing-patches-15. |
| `imapi.h` | IMapi | remaining |  |  |  |
| `imapi2.h` | IMapi | remaining |  |  |  |
| `imapi2error.h` | IMapi, Wab | remaining |  |  |  |
| `imapi2fs.h` | IMapi | remaining |  |  |  |
| `imapi2fserror.h` | Wab | remaining |  |  |  |
| `imapierror.h` | IMapi | remaining |  |  |  |
| `ime_cmodes.h` | Input.Ime | matched |  | 09/02/2026 17:09:31 | Bit-field constants only, no functions. |
| `ime.h` | FileHistory, WinProg | remaining |  |  |  |
| `imepad.h` | Input.Ime | remaining |  |  |  |
| `imessage.h` | IMapi | remaining |  |  |  |
| `imessagedispatcher.h` | TransactionServer | remaining |  |  |  |
| `imgerror.h` | Printing | remaining |  |  |  |
| `imgutil.h` | InternetExplorer | remaining |  |  |  |
| `imm.h` | Input.Ime | remaining |  |  |  |
| `immdev.h` | Input.Ime | remaining |  |  |  |
| `in6addr.h` | WinSock, wnv | matched |  | 09/02/2026 17:16:10 | Plain data struct + macros only, no functions. |
| `inaddr.h` | IpHlp, RRas, WinSock | matched |  | 09/02/2026 17:19:28 | Plain data struct + macros only, no functions. |
| `indexsrv.h` | IndexSrv, Search | remaining |  |  |  |
| `inetreg.h` | InternetExplorer | remaining |  |  |  |
| `inetsdk.h` | InternetExplorer | remaining |  |  |  |
| `infocard.h` | Security.Cryptography, Security.Cryptography.UI | remaining |  |  |  |
| `infotech.h` | HtmlHelp | remaining |  |  |  |
| `infstr.h` | DevInst | remaining |  |  |  |
| `inked.h` | Tablet | remaining |  |  |  |
| `inkpresenterdesktop.h` | Input_Ink | remaining |  |  |  |
| `inkrenderer.h` | Input_Ink | remaining |  |  |  |
| `inputpaneinterop.h` | WinRT | remaining |  |  |  |
| `inputpanelconfiguration.h` | Shell | matched | copilot | 09/03/2026 02:45:00 | Classified retained artifact in existing-patches-15. |
| `inputscope.h` | Tsf | remaining |  |  |  |
| `inspectable.h` | Tcui | remaining |  |  |  |
| `interactioncontext.h` | Input_IntContext | remaining |  |  |  |
| `intrinfix.h` | ActiveDirectory, AllJoyn, Antimalware, ApplicationVerifier, AppxPackaging, Audio, Audio.Apo, Audio.DirectMusic, Audio.DirectSound, Audio.Endpoints, Authorization, Authorization.UI, Backup, Base, Bits, Bluetooth, Buses, Cabinets, CEIP, Certificates, Cloudapi, ClrHosting, ClrProfiling, CmpApi, Com, Com.CallObj, Com.ChannelCreds, Com.Events, Com.StructuredStorage, Com.Urlmon, Communication, ComOle, CompositionSwapchain, Console, Controls, Controls.RichEdit, Cos, Credentials, DataXchg, Debug, Debug.ActiveScript, Debug.Extensions, Debug.WebApp, Dedup, DeliveryOptimization, DeviceAccess, DeviceQuery, Devices.1394, Devices.Beep, Devices.Cdrom, Devices.Dvd, Devices.Properties, DevInst, DevLic, Dfs, Dhcp, Direct2D, Direct2D.Common, Direct3D, Direct3D10, Direct3D11, Direct3D11on12, Direct3D12, Direct3D9, Direct3D9on12, Direct3DDxc, Direct3DFxc, DirectComp, DirectDraw, DirectManipulation, DirectML, DirectWrite, Display, DlgBox, Dns, DTC, Dwm, DXCore, Dxgi, Dxgi.Common, Eap, EapHost, Edp, Enstor, Environment, Etw, Fax, FileHistory, Foundation, Fs, Fsrm, FunctionDiscovery, GameInput, GameMode, GamingDvcInfo, Gdi, Gdiplus, Gpmc, Hid, HostComputeNetwork, HostComputeSystem, HtmlHelp, HttpServer, Hypervisor, Identity, IdentityProvider, Ifsk, Iis, ImagingDevice, IMapi, IndexSrv, Input_Ink, Input_IntContext, Input.Ime, InputRadial, InternetConnectionWizard, InternetExplorer, Intl, IO, Ioctl, IpHlp, IScsiDisc, Isolation, JobObjects, Js, Kernel, KeyboardAndMouseInput, Ldap, LibraryLoader, Lwef, MachineLearning, MadCap, MagApi, Mapi, Mbn, MdmReg, Media, Media.DShow, Media.DShow.Xml, Media.DxMediaObjects, Media.KernelStreaming, Memory, MenuRc, MessageQueuing, Mf, Mi, MixedReality, MixedRealityInterop, Mmc, Monitor, MsChap, MsCs, MsHtml, MsTv, MsXml, Multimedia, Ndf, Ndis, NetBios, NetMgmt, NetShell, NetworkAccessProtection, Nfc, Nfp, Nla, Nps, NWifi, Of, Opc, OpenGL, P2p, Parcon, Perf, PicAcq, Pipes, Pla, Policy, Power, Printing, PrintTicket, Proc_Snap, ProjFs, Properties, PsApi1, PsApi2, PWM, Qos, Rdc, Rdp, RealTimeCommunications, Registry, RemoteAssist, Rm, Rpc, RRas, RstMgr, Search, SecBitomet, Security, Security.AppLocker, Security.ConfigurationSnapin, Security.Cryptography, Security.Cryptography.Catalog, Security.Cryptography.Sip, Security.Cryptography.UI, Security.DiagnosticDataQuery, Security.DirectoryServices, Security.LicenseProtection, Security.Tpm, Security.WinTrust, Security.WinWlx, SecurityCenter, Sens, SerPorts, Services, Setup, Shell, Shell.Common, Shutdown, SideShow, Smi, Snmp, Speech, Sr, Storage.Jet, Storage.Nvme, SwDevice, SystemInformation, Tablet, Tapi3, TaskSchd, Tbs, Tcui, TermServ, Threading, Time, ToolHelp, TransactionServer, Tsf, Ual, UiAnimation, UpdateAssessment, Upnp, Variant, Vhd, VirtualDiskService, VirtualDOSMachines, VSS, Wab, Wcm, Wcn, Wcs, Wds, WebAuthn, WebDav, WebServicesOnDevices, WebSock, Wec, Wer, Wes, Wia, Wibe, Wic, Win32_Tile_Badge_Notif, WinAuto, WinContacts, WindowsConnectionManager, WindowsFilteringPlatform, WindowsFirewall, WindowsRibbon, WindowsSetupAndMigration, WinHttp, WinInet, WinLocation, WinProg, WinRm, WinRT, WinRT.AllJoyn, WinRT.Composition, WinRT.CoreInputView, WinRT.Direct2D, WinRT.Direct3D11, WinRT.Display, WinRT.Graphics.Capture, WinRT.Graphics.Imaging, WinRT.Holographic, WinRT.Isolation, WinRT.Media, WinRT.Metadata, WinRT.ML, WinRT.Pdf, WinRT.Printing, WinRT.Storage, WinRT.Xaml, WinSat, WinSensors, WinSock, WinSync, WinTouch, Wmdm, WmFormat, Wmi, Wmlss, Wmp, Wnet, wnv, WpdSdk, Wsb, Wsl, Wsw, Wua, Xaml_Diagnostics, Xaudio2, Xblidp, Xinput, XmlLite, Xps, XpsPrinting | remaining |  |  |  |
| `intsafe.h` | Shell | remaining |  |  |  |
| `intshcut.h` | Shell | matched | copilot | 09/03/2026 03:00:00 | Classified retained artifact in existing-patches-16. |
| `ioapiset.h` | Base, IO | matched | copilot | 09/03/2026 03:00:00 | Classified retained artifact in existing-patches-16. |
| `ioevent.h` | MenuRc | remaining |  |  |  |
| `ioringapi.h` | Fs | remaining |  |  |  |
| `ip2string.h` | IpHlp, wnv | remaining |  |  |  |
| `IPExport.h` | IpHlp | remaining |  |  |  |
| `iphlpapi.h` | IpHlp, Shell | matched | copilot | 09/03/2026 03:00:00 | Classified retained artifact in existing-patches-16. |
| `ipinfoid.h` | IpHlp | remaining |  |  |  |
| `ipmib.h` | RRas | remaining |  |  |  |
| `IPTypes.h` | wnv | remaining |  |  |  |
| `iscsidsc.h` | IScsiDisc | remaining |  |  |  |
| `iscsierr.h` | IScsiDisc | remaining |  |  |  |
| `isolatedapplauncher.h` | FileHistory, Isolation | remaining |  |  |  |
| `isolatedwindowsenvironmentutils.h` | Isolation | matched |  | 09/02/2026 16:57:06 | Boolean query functions only. |
| `issper16.h` | Identity | remaining |  |  |  |
| `iswindowarranged.h` | MenuRc | matched |  | 09/02/2026 16:49:01 | Query-only function taking HWND input; no handle production. |
| `isysmon.h` | Perf | remaining |  |  |  |
| `iwscapi.h` | FileHistory, SecurityCenter | remaining |  |  |  |
| `iwstdec.h` | Media.DShow | remaining |  |  |  |
| `joystickapi.h` | Multimedia | remaining |  |  |  |
| `jscript9diag.h` | Debug.ActiveScript | remaining |  |  |  |
| `jsrt.h` | Js | matched |  | 09/02/2026 17:35:31 | Redirect-only; reachable branch (jsrt9.h) already tracked pending; chakrart.h unreachable in this partition. |
| `jsrt9.h` | Js | remaining |  |  |  |
| `kbd.h` | KeyboardAndMouseInput | matched | copilot | 09/03/2026 04:20:00 | No patch needed; data-structure-only header. |
| `keycredmgr.h` | Credentials | remaining |  |  |  |
| `knownfolders.h` | Shell | remaining |  |  |  |
| `ks.h` | Audio, Audio.DirectMusic, Media.DShow, Media.KernelStreaming, MsTv, Multimedia | remaining |  |  |  |
| `ksmedia.h` | Audio, Audio.DirectMusic, Media.DShow, Media.KernelStreaming, MsTv, Multimedia | remaining |  |  |  |
| `ksproxy.h` | Media.KernelStreaming | remaining |  |  |  |
| `ktmtypes.h` | Fs | remaining |  |  |  |
| `ktmw32.h` | Fs | matched | copilot | 09/03/2026 03:00:00 | Classified retained artifact in existing-patches-16. |
| `libloaderapi.h` | Intl, LibraryLoader | matched | copilot | 09/03/2026 03:00:00 | Classified retained artifact in existing-patches-16. |
| `libloaderapi2.h` | LibraryLoader | blocked | copilot | 09/02/2026 17:58:11 | Recorded for per-header traceability; same root cause and evidence as getprocesshandlefromhwnd.h, no new investigation required. |
| `licenseprotection.h` | Security.LicenseProtection | matched |  | 09/02/2026 17:38:44 | Enum/FILETIME output only, no handle. |
| `lm.h` | NetMgmt | matched |  | 09/02/2026 17:38:44 | Pure umbrella redirect; all 18 sub-headers already individually tracked in ledger (2 accepted, 16 pending). |
| `lmaccess.h` | FileHistory, NetMgmt, WinProg | matched | copilot | 09/03/2026 03:15:00 | Classified retained artifact in existing-patches-17. |
| `lmalert.h` | NetMgmt | remaining |  |  |  |
| `lmapibuf.h` | NetMgmt | remaining |  |  |  |
| `lmat.h` | NetMgmt | remaining |  |  |  |
| `lmaudit.h` | NetMgmt | remaining |  |  |  |
| `lmconfig.h` | NetMgmt | matched |  | 09/02/2026 18:03:29 | Net API buffer-allocation convention (NetApiBufferFree), not HANDLE-family. |
| `lmcons.h` | Dfs, NetMgmt | remaining |  |  |  |
| `lmdfs.h` | Dfs | remaining |  |  |  |
| `lmerr.h` | NetMgmt | remaining |  |  |  |
| `lmerrlog.h` | NetMgmt | remaining |  |  |  |
| `lmjoin.h` | NetMgmt | remaining |  |  |  |
| `lmmsg.h` | NetMgmt | remaining |  |  |  |
| `lmremutl.h` | NetMgmt | remaining |  |  |  |
| `lmrepl.h` | NetMgmt | remaining |  |  |  |
| `lmserver.h` | Base, NetMgmt | matched | copilot | 09/03/2026 03:15:00 | Classified retained artifact in existing-patches-17. |
| `lmshare.h` | Fs | remaining |  |  |  |
| `lmsname.h` | NetMgmt | remaining |  |  |  |
| `lmstats.h` | Fs | remaining |  |  |  |
| `lmsvc.h` | NetMgmt | remaining |  |  |  |
| `lmuse.h` | NetMgmt | remaining |  |  |  |
| `lmuseflg.h` | NetMgmt | matched |  | 09/02/2026 17:23:42 | Constants/macro only, no functions. |
| `lmwksta.h` | NetMgmt | remaining |  |  |  |
| `loadperf.h` | FileHistory, Perf, WinProg | remaining |  |  |  |
| `locationapi.h` | WinLocation | remaining |  |  |  |
| `lowlevelmonitorconfigurationapi.h` | Monitor | matched |  | 09/02/2026 17:48:13 | All functions only consume an existing physical-monitor HANDLE; producer pair tracked separately under PhysicalMonitorEnumerationAPI.h (pending). |
| `lzexpand.h` | Fs | matched | copilot | 09/03/2026 03:15:00 | Classified retained artifact in existing-patches-17. |
| `madcapcl.h` | MadCap | matched | copilot | 09/03/2026 05:00:00 | No patch needed; no ownership-relevant handle type. |
| `magnification.h` | MagApi | matched | copilot | 09/03/2026 03:15:00 | Classified retained artifact in existing-patches-17. |
| `mailmsgprops.h` | IMapi | remaining |  |  |  |
| `manipulations.h` | WinTouch | remaining |  |  |  |
| `manual.h` | Com.StructuredStorage | remaining |  |  |  |
| `mapi.h` | Mapi | remaining |  |  |  |
| `mbnapi.h` | Mbn | matched | copilot | 09/03/2026 05:00:00 | No patch needed; COM interface only. |
| `mciapi.h` | Multimedia | remaining |  |  |  |
| `mciavi.h` | Multimedia | remaining |  |  |  |
| `mcx.h` | Communication | remaining |  |  |  |
| `mdhcp.h` | Tapi3 | remaining |  |  |  |
| `mdmlocalmanagement.h` | MdmReg | matched |  | 09/02/2026 17:54:05 | No handle-typed values; string output uses LocalFree, not HANDLE mechanism. |
| `mdmregistration.h` | MdmReg | remaining |  |  |  |
| `mediaerr.h` | Media.DShow, Media.DxMediaObjects | matched |  | 09/02/2026 17:13:02 | HRESULT error constants only, no functions. |
| `mediaobj.h` | Media.DShow, Media.DxMediaObjects | remaining |  |  |  |
| `medparam.h` | Media.DShow | remaining |  |  |  |
| `memorybuffer.h` | WinRT | matched |  | 09/02/2026 17:05:19 | IMemoryBufferByteAccess COM interface; buffer view not a HANDLE-family resource. |
| `menutemplate.h` | MenuRc | matched |  | 09/02/2026 17:01:48 | Plain data structs only, no functions. |
| `mergemod.h` | Setup | remaining |  |  |  |
| `messagedeviceservice.h` | WpdSdk | remaining |  |  |  |
| `messagedispatcherapi.h` | ComOle, WinRT | matched |  | 09/02/2026 17:13:02 | IMessageDispatcher pointer is caller-supplied input, not produced here. |
| `metadatadeviceservice.h` | WpdSdk | remaining |  |  |  |
| `metahost.h` | ClrHosting | matched | copilot | 09/02/2026 23:40:00 | No patch needed; verified via live scrape plus confirmation that global supportedOS.rsp already covers this header's functions. |
| `mfapi.h` | Mf | remaining |  |  |  |
| `mfcaptureengine.h` | Mf | remaining |  |  |  |
| `mfcontentdecryptionmodule.h` | Mf | remaining |  |  |  |
| `mfd3d12.h` | Direct3D12, Mf | remaining |  |  |  |
| `mferror.h` | Mf | remaining |  |  |  |
| `mfidl.h` | Mf | remaining |  |  |  |
| `mfmediacapture.h` | Mf | remaining |  |  |  |
| `mfmediaengine.h` | Mf | remaining |  |  |  |
| `mfmp2dlna.h` | Mf | remaining |  |  |  |
| `mfobjects.h` | Mf | matched | copilot | 09/03/2026 03:15:00 | Classified retained artifact in existing-patches-17. |
| `mfplay.h` | Mf | remaining |  |  |  |
| `mfreadwrite.h` | Mf | remaining |  |  |  |
| `mfsharingengine.h` | Mf | remaining |  |  |  |
| `mfspatialaudio.h` | Mf | remaining |  |  |  |
| `mftransform.h` | Mf | remaining |  |  |  |
| `mfvirtualcamera.h` | Mf | remaining |  |  |  |
| `mgm.h` | RRas | remaining |  |  |  |
| `mgmtapi.h` | Snmp | matched | copilot | 09/03/2026 03:30:00 | Classified retained artifact in existing-patches-18. |
| `mi.h` | Mi | matched | copilot | 09/03/2026 05:15:00 | No patch needed; no ownership-relevant handle type. |
| `micaut.h` | Tablet | remaining |  |  |  |
| `midles.h` | Rpc | remaining |  |  |  |
| `mileffects.h` | Wibe | remaining |  |  |  |
| `minappmodel.h` | AppxPackaging | remaining |  |  |  |
| `mindumpdef.h` | Debug | remaining |  |  |  |
| `minidumpapiset.h` | Base, Debug | remaining |  |  |  |
| `minwinbase.h` | Base, Fs, Ioctl | matched | copilot | 09/03/2026 03:30:00 | Classified retained artifact in existing-patches-18. |
| `minwindef.h` | Foundation, Multimedia | matched | copilot | 09/02/2026 19:18:45 | Closed in shared-handle-policy-01; current windows-rs full test target is blocked by missing PartitionSpec.include_main_file initializers. |
| `mixerocx.h` | Media.DShow | remaining |  |  |  |
| `mlang.h` | Intl | remaining |  |  |  |
| `MLOperatorAuthor.h` | MachineLearning | remaining |  |  |  |
| `mmc.h` | Mmc | remaining |  |  |  |
| `mmcobj.h` | Lwef, Mmc | remaining |  |  |  |
| `mmddk.h` | Audio.DirectMusic, Multimedia | remaining |  |  |  |
| `mmdeviceapi.h` | Audio | matched | copilot | 09/03/2026 03:30:00 | Classified retained artifact in existing-patches-18. |
| `mmeapi.h` | Audio, Multimedia | matched | copilot | 09/03/2026 03:30:00 | Classified retained artifact in existing-patches-18. |
| `mmiscapi.h` | Multimedia | remaining |  |  |  |
| `mmiscapi2.h` | Media | remaining |  |  |  |
| `mmreg.h` | Audio.DirectMusic, Media.DShow, Multimedia | matched | copilot | 09/03/2026 03:30:00 | Classified retained artifact in existing-patches-18. |
| `mmstream.h` | Media.DShow | remaining |  |  |  |
| `mmsyscom.h` | Media | remaining |  |  |  |
| `mmsystem.h` | Audio, Audio.Apo, Audio.DirectMusic, Audio.DirectSound, Audio.Endpoints, Base, Devices.Properties, FileHistory, GamingDvcInfo, Gdi, Hid, IO, Multimedia, WinProg | remaining |  |  |  |
| `mobsync.h` | Shell | matched | copilot | 09/03/2026 03:45:00 | Classified retained artifact in existing-patches-19. |
| `mpconfig.h` | Media.DShow | remaining |  |  |  |
| `mpeg2bits.h` | MsTv | matched |  | 09/02/2026 17:48:13 | Bitfield data structs only, no functions. |
| `mpeg2data.h` | MsTv | remaining |  |  |  |
| `mpeg2error.h` | Media.DShow | remaining |  |  |  |
| `mpeg2psiparser.h` | Media.DShow, MsTv | remaining |  |  |  |
| `mpeg2structs.h` | MsTv | remaining |  |  |  |
| `mpegtype.h` | Media.DShow | remaining |  |  |  |
| `mprapi.h` | RRas | remaining |  |  |  |
| `mprerror.h` | Foundation | remaining |  |  |  |
| `mq.h` | MessageQueuing | remaining |  |  |  |
| `mqoai.h` | MessageQueuing | remaining |  |  |  |
| `mrmresourceindexer.h` | MenuRc | remaining |  |  |  |
| `msaatext.h` | Tsf | remaining |  |  |  |
| `msacm.h` | Audio | remaining |  |  |  |
| `msacmdlg.h` | Multimedia | matched |  | 09/02/2026 17:27:04 | Dialog resource ID constants only, no functions. |
| `msacmdrv.h` | Audio | remaining |  |  |  |
| `MSAJTransport.h` | AllJoyn, WinRT | remaining |  |  |  |
| `mscat.h` | Security.Cryptography.Catalog, Security.Cryptography.Sip | remaining |  |  |  |
| `mschapp.h` | MsChap | remaining |  |  |  |
| `msclmd.h` | Security.Cryptography | remaining |  |  |  |
| `msclus.h` | MsCs | remaining |  |  |  |
| `mscoree.h` | ClrHosting | matched | copilot | 09/02/2026 23:40:00 | No patch needed; verified via live scrape plus confirmation that global supportedOS.rsp already covers this header's functions. |
| `msctf.h` | Tsf | remaining |  |  |  |
| `msctfmonitorapi.h` | Tsf | remaining |  |  |  |
| `msdadc.h` | Search | remaining |  |  |  |
| `msdaguid.h` | Search | matched |  | 09/02/2026 18:06:26 | GUID constants only, no functions. |
| `msdaora.h` | Search | remaining |  |  |  |
| `msdaosp.h` | Search | remaining |  |  |  |
| `msdasc.h` | Search | remaining |  |  |  |
| `msdasql_interfaces.h` | Search | remaining |  |  |  |
| `msdasql.h` | Search | remaining |  |  |  |
| `msdatsrc.h` | Search | remaining |  |  |  |
| `msdelta.h` | Setup | remaining |  |  |  |
| `msdrm.h` | Rm | remaining |  |  |  |
| `msdrmdefs.h` | Rm | remaining |  |  |  |
| `msdrmerror.h` | Rm | remaining |  |  |  |
| `msdrmgetinfo.h` | Rm | remaining |  |  |  |
| `msdshape.h` | Search | matched |  | 09/02/2026 17:51:18 | Constants/enums only, no functions. |
| `msfeeds.h` | Wmp | remaining |  |  |  |
| `msfeedsid.h` | Wmp | remaining |  |  |  |
| `mshtmcid.h` | MsHtml | remaining |  |  |  |
| `mshtmdid.h` | MsHtml | remaining |  |  |  |
| `mshtmhst.h` | MsHtml | remaining |  |  |  |
| `Mshtml.h` | MsHtml | remaining |  |  |  |
| `mshtmlc.h` | MsHtml | remaining |  |  |  |
| `mshtmldiagnostics.h` | MsHtml | remaining |  |  |  |
| `msi.h` | Setup | remaining |  |  |  |
| `msidefs.h` | Setup | remaining |  |  |  |
| `msiehost.h` | InternetExplorer | matched |  | 09/02/2026 17:51:18 | GUID/command-ID constants only, no functions. |
| `msiltcfg.h` | Setup | matched |  | 09/02/2026 17:38:44 | No handle involved. |
| `msime.h` | Input.Ime | remaining |  |  |  |
| `msimeapi.h` | Input.Ime | remaining |  |  |  |
| `msinkaut.h` | Tablet | remaining |  |  |  |
| `msinkaut15.h` | Tablet | remaining |  |  |  |
| `msiquery.h` | Setup | remaining |  |  |  |
| `msopc.h` | Opc | matched | copilot | 09/03/2026 05:15:00 | No patch needed; COM interface only, no free functions. |
| `msports.h` | SerPorts | remaining |  |  |  |
| `msrdc.h` | Rdc | matched | copilot | 09/03/2026 04:35:00 | No patch needed; COM interface only. |
| `mssign.h` | Security.Cryptography | remaining |  |  |  |
| `mssip.h` | Security.Cryptography.Sip, Security.WinTrust | matched | copilot | 09/03/2026 03:45:00 | Classified retained artifact in existing-patches-19. |
| `mstask.h` | TaskSchd | remaining |  |  |  |
| `mstcpip.h` | WinSock | remaining |  |  |  |
| `msvidctl.h` | MsTv | remaining |  |  |  |
| `mswmdm.h` | Wmdm | remaining |  |  |  |
| `mswsock.h` | WinSock | matched | copilot | 09/03/2026 03:45:00 | Classified retained artifact in existing-patches-19. |
| `mswsockdef.h` | WinSock | remaining |  |  |  |
| `msxml.h` | FileHistory, MsXml, WinProg | remaining |  |  |  |
| `msxml6.h` | MsXml, Printing | remaining |  |  |  |
| `mtpext.h` | Wmdm | remaining |  |  |  |
| `mtx.h` | ComOle, TransactionServer | matched |  | 09/02/2026 16:49:01 | Redirect-only header (#include comsvcs.h, already accepted-normalized). |
| `mtxadmin.h` | ComOle, TransactionServer | matched | copilot | 09/03/2026 03:45:00 | Classified retained artifact in existing-patches-19. |
| `mtxattr.h` | ComOle, TransactionServer | remaining |  |  |  |
| `mtxdm.h` | Cos | matched |  | 09/02/2026 17:05:19 | GetDispenserManager is a COM-factory pattern; comsvcs.h already accepted. |
| `muiload.h` | Intl | remaining |  |  |  |
| `mxdc.h` | Gdi, Printing | remaining |  |  |  |
| `namedpipeapi.h` | Pipes | matched | copilot | 09/03/2026 03:45:00 | Classified retained artifact in existing-patches-19. |
| `napmicrosoftvendorids.h` | NetworkAccessProtection | matched |  | 09/02/2026 17:58:11 | Integer constants only, no functions. |
| `naptypes.h` | NetworkAccessProtection | remaining |  |  |  |
| `natupnp.h` | WindowsFirewall | remaining |  |  |  |
| `nb30.h` | NetBios | remaining |  |  |  |
| `ncrypt_provider.h` | Security.Cryptography | remaining |  |  |  |
| `ncrypt.h` | Security.Cryptography, Security.Cryptography.UI | matched | copilot | 09/02/2026 22:10:00 | Corrected typedef-owned ownership violation found during resource-ownership audit; verified compatible with co-resident zz-crypto-security-enums patch via full replay. |
| `ncryptprotect.h` | Security.Cryptography, Security.Cryptography.UI | matched | copilot | 09/02/2026 21:05:00 | Corrected typedef-owned ownership violation found during resource-ownership audit. |
| `ndattrib.h` | Ndf | remaining |  |  |  |
| `ndfapi.h` | Ndf | remaining |  |  |  |
| `ndhelper.h` | Ndf | remaining |  |  |  |
| `ndisguid.h` | Ndis | remaining |  |  |  |
| `ndkinfo.h` | Ndis | matched |  | 09/02/2026 18:00:53 | Data structs/enum/constants only, no functions. |
| `ndr64types.h` | Rpc | remaining |  |  |  |
| `netcfgn.h` | NetMgmt | remaining |  |  |  |
| `netcfgx.h` | NetMgmt | remaining |  |  |  |
| `netcon.h` | WindowsFirewall | matched | copilot | 09/03/2026 04:00:00 | Classified retained artifact in existing-patches-20. |
| `netevent.h` | NetMgmt | remaining |  |  |  |
| `netfw.h` | WindowsFirewall | matched | copilot | 09/03/2026 04:00:00 | Classified retained artifact in existing-patches-20. |
| `netiodef.h` | WinSock, wnv | remaining |  |  |  |
| `netlistmgr.h` | Nla, WindowsConnectionManager | remaining |  |  |  |
| `netprov.h` | NetMgmt | remaining |  |  |  |
| `netsh.h` | NetShell | matched | copilot | 09/03/2026 05:15:00 | No patch needed; no ownership-relevant handle type. |
| `nettypes.h` | NetMgmt | remaining |  |  |  |
| `newdev.h` | DevInst | matched | copilot | 09/03/2026 02:00:00 | Resolves the replay-mismatch flagged in shared-handle-policy-01; verified via isolated round-trip in this session (no fresh SDK checkout available to rerun the original full-replay evidence). |
| `nfcdtadev.h` | Nfc | remaining |  |  |  |
| `nfcradiodev.h` | Nfc | matched |  | 09/02/2026 17:54:05 | GUID/IOCTL constants + data structs only, no functions. |
| `nfcsedev.h` | Nfc | remaining |  |  |  |
| `nfpdev.h` | Nfp | matched | copilot | 09/03/2026 05:00:00 | No patch needed; constant-only header. |
| `notesdeviceservice.h` | WpdSdk | matched |  | 09/02/2026 17:35:31 | GUID/name constants only, no functions. |
| `notificationactivationcallback.h` | Win32_Tile_Badge_Notif | remaining |  |  |  |
| `Npapi.h` | Wnet | remaining |  |  |  |
| `nsemail.h` | WinSock | remaining |  |  |  |
| `nserror.h` | Multimedia | remaining |  |  |  |
| `nspapi.h` | WinSock | matched | copilot | 09/03/2026 04:00:00 | Classified retained artifact in existing-patches-20. |
| `ntdd1394.h` | Devices.1394 | matched | copilot | 09/02/2026 23:20:00 | No patch needed; data-structure-only header verified via live scrape. |
| `ntddbeep.h` | Devices.Beep | matched | copilot | 09/03/2026 04:05:00 | No patch needed; data-structure-only header. |
| `ntddcdrm.h` | Devices.Cdrom | matched | copilot | 09/03/2026 03:35:00 | No patch needed; data-structure-only header verified via live scrape. |
| `ntddcdvd.h` | Devices.Dvd | matched | copilot | 09/03/2026 04:05:00 | No patch needed; data-structure-only header. |
| `ntdddisk.h` | Fs | remaining |  |  |  |
| `ntddkbd.h` | Hid | remaining |  |  |  |
| `ntddmou.h` | Hid | remaining |  |  |  |
| `ntddndis.h` | Ndis, NWifi, Qos | remaining |  |  |  |
| `ntddser.h` | SerPorts | remaining |  |  |  |
| `ntddstor.h` | Fs, Ioctl | remaining |  |  |  |
| `ntddvdeo.h` | Display | remaining |  |  |  |
| `ntddvol.h` | Fs | remaining |  |  |  |
| `ntdef.h` | Kernel | remaining |  |  |  |
| `ntdsapi.h` | ActiveDirectory | remaining |  |  |  |
| `ntdsbmsg.h` | ActiveDirectory | remaining |  |  |  |
| `ntiologc.h` | Foundation | remaining |  |  |  |
| `ntioring_x.h` | Fs | remaining |  |  |  |
| `ntldap.h` | Ldap | remaining |  |  |  |
| `ntmsapi.h` | Fs | matched | copilot | 09/03/2026 04:00:00 | Classified retained artifact in existing-patches-20. |
| `ntmsmli.h` | Fs | matched |  | 09/02/2026 17:45:25 | Data struct + callback-signature typedefs only, no functions. |
| `ntquery.h` | IndexSrv, Shell | remaining |  |  |  |
| `ntsecapi.h` | ActiveDirectory, Certificates, Identity, Security, Security.AppLocker, Security.ConfigurationSnapin, Security.Cryptography, Security.Cryptography.Catalog, Security.Cryptography.Sip, Security.Cryptography.UI, Security.DiagnosticDataQuery, Security.DirectoryServices, Security.LicenseProtection, Security.Tpm, Security.WinTrust, Security.WinWlx | matched | copilot | 09/02/2026 21:05:00 | Corrected typedef-owned LSA_HANDLE ownership violation found during resource-ownership audit; other annotation groups in file were already compliant. |
| `NTSecPKG.h` | Certificates, Identity, Security, Security.AppLocker, Security.ConfigurationSnapin, Security.Cryptography, Security.Cryptography.Catalog, Security.Cryptography.Sip, Security.Cryptography.UI, Security.DiagnosticDataQuery, Security.DirectoryServices, Security.LicenseProtection, Security.Tpm, Security.WinTrust, Security.WinWlx | matched | copilot | 09/02/2026 20:35:00 | Audited under corrected shared-handle policy (165b5f09, 7335ddc4); already compliant, no code changes required. |
| `ntstatus.h` | Foundation | remaining |  |  |  |
| `nvme.h` | Storage.Nvme | matched | copilot | 09/03/2026 04:35:00 | No patch needed; data-structure-only header. |
| `oaidl.h` | ComOle | matched | copilot | 09/03/2026 04:00:00 | Classified retained artifact in existing-patches-20. |
| `objbase.h` | Base, Com, Com.CallObj, Com.ChannelCreds, Com.StructuredStorage, Com.Urlmon, ComOle, FileHistory, IO, Printing, TransactionServer, VSS, WinProg | matched | copilot | 09/03/2026 04:15:00 | Classified retained artifact in existing-patches-21. |
| `objectarray.h` | Shell, Shell.Common | matched | copilot | 09/03/2026 04:15:00 | Classified retained artifact in existing-patches-21. |
| `ObjIdl.h` | Base, Com, Com.CallObj, Com.ChannelCreds, Com.StructuredStorage, Com.Urlmon, ComOle, IO, Rpc, TransactionServer, VSS | matched | copilot | 09/03/2026 04:15:00 | Classified retained artifact in existing-patches-21. |
| `objidlbase.h` | TransactionServer | remaining |  |  |  |
| `objsafe.h` | Debug | remaining |  |  |  |
| `objsel.h` | ActiveDirectory | remaining |  |  |  |
| `ocidl.h` | Com, Com.CallObj, Com.ChannelCreds, Com.Urlmon, ComOle, Media.DShow, Search, TransactionServer | matched | copilot | 09/03/2026 04:15:00 | Classified retained artifact in existing-patches-21. |
| `ocmm.h` | InternetExplorer | remaining |  |  |  |
| `odbcinst.h` | Search | remaining |  |  |  |
| `odbcss.h` | Search | remaining |  |  |  |
| `ole2.h` | Com, Com.CallObj, Com.ChannelCreds, Com.StructuredStorage, Com.Urlmon, ComOle, TransactionServer | matched | copilot | 09/03/2026 04:15:00 | Classified retained artifact in existing-patches-21. |
| `oleacc.h` | WinAuto | matched | copilot | 09/03/2026 04:30:00 | Classified retained artifact in existing-patches-22. |
| `oleauto.h` | ComOle | matched | copilot | 09/03/2026 04:30:00 | Classified retained artifact in existing-patches-22. |
| `olectl.h` | Com, Com.CallObj, Com.ChannelCreds, Com.Urlmon, ComOle, TransactionServer | matched | copilot | 09/03/2026 04:30:00 | Classified retained artifact in existing-patches-22. |
| `oledbdep.h` | Search | remaining |  |  |  |
| `oledberr.h` | Search | remaining |  |  |  |
| `oledlg.h` | Com, Com.CallObj, Com.ChannelCreds, Com.Urlmon, ComOle, TransactionServer | matched | copilot | 09/03/2026 04:30:00 | Classified retained artifact in existing-patches-22. |
| `oleidl.h` | Com, Com.CallObj, Com.ChannelCreds, Com.Urlmon, ComOle, Shell, TransactionServer | matched | copilot | 09/03/2026 04:30:00 | Classified retained artifact in existing-patches-22. |
| `oletx2xa.h` | DTC | remaining |  |  |  |
| `ondemandconnroutehelper.h` | Nla, WindowsConnectionManager | remaining |  |  |  |
| `oobenotification.h` | WindowsSetupAndMigration | matched | copilot | 09/02/2026 22:05:00 | Classified retained artifact in existing-patches-23. |
| `openservice.h` | InternetExplorer | remaining |  |  |  |
| `opmapi.h` | Mf | remaining |  |  |  |
| `opmxbox.h` | Mf | matched |  | 09/02/2026 17:23:42 | Enum-output-only API, no handle. |
| `p2p.h` | P2p | remaining |  |  |  |
| `packagevirtualizationcontext.h` | AppxPackaging | remaining |  |  |  |
| `pacmanclientapi.h` | Setup | remaining |  |  |  |
| `patchapi.h` | Setup | remaining |  |  |  |
| `patchwiz.h` | Setup | remaining |  |  |  |
| `pathcch.h` | Shell | matched | copilot | 09/02/2026 22:05:00 | Classified retained artifact in existing-patches-23. |
| `pbdaerrors.h` | Media.DShow | remaining |  |  |  |
| `pchannel.h` | TermServ | remaining |  |  |  |
| `pdh.h` | Perf | remaining |  |  |  |
| `pdhmsg.h` | Perf | remaining |  |  |  |
| `peerdist.h` | P2p | remaining |  |  |  |
| `peninputpanel.h` | Tablet | remaining |  |  |  |
| `perceptiondevicecore.h` | MixedReality | remaining |  |  |  |
| `perflib.h` | Perf | remaining |  |  |  |
| `perhist.h` | InternetExplorer | remaining |  |  |  |
| `persist.h` | Search | matched |  | 09/02/2026 18:06:26 | Constants only, no functions. |
| `photoacquire.h` | PicAcq, Setup | matched |  | 09/02/2026 16:34:06 | All functions are MIDL RPC marshalling stubs (BSTR/HBITMAP/HICON/HWND/LPSAFEARRAY_User*); pass-through of externally-owned handles only. |
| `physicalmonitorenumerationapi.h` | Monitor | remaining |  |  |  |
| `pla.h` | Pla | matched |  | 09/02/2026 16:34:06 | All 6 functions operate on strings/flags/paths only; no handle-producing functions. |
| `playlist.h` | Media.DShow | remaining |  |  |  |
| `playsoundapi.h` | Audio | matched | copilot | 09/02/2026 22:05:00 | Classified retained artifact in existing-patches-23. |
| `playto.h` | Mf | matched |  | 09/02/2026 17:51:18 | Pure MIDL boilerplate; real content in separately-tracked MFSharingEngine.h (pending). |
| `PlayToManagerInterop.h` | WinRT | remaining |  |  |  |
| `pluginauthenticator.h` | WebAuthn | remaining |  |  |  |
| `pnpxassoc.h` | FunctionDiscovery, WebServicesOnDevices | remaining |  |  |  |
| `poclass.h` | Power | remaining |  |  |  |
| `portabledevice.h` | WpdSdk | remaining |  |  |  |
| `portabledeviceapi.h` | WpdSdk | remaining |  |  |  |
| `portabledeviceconnectapi.h` | WpdSdk | remaining |  |  |  |
| `portabledevicetypes.h` | WpdSdk | remaining |  |  |  |
| `powerbase.h` | Power | matched | copilot | 09/02/2026 22:05:00 | Classified retained artifact in existing-patches-23. |
| `powersetting.h` | Power | remaining |  |  |  |
| `powrprof.h` | Power | matched | copilot | 09/02/2026 22:50:00 | Classified retained artifact in existing-patches-26. |
| `prcomoem.h` | Printing | remaining |  |  |  |
| `prdrvcom.h` | Printing | remaining |  |  |  |
| `presentation.h` | CompositionSwapchain | remaining |  |  |  |
| `presentationtypes.h` | CompositionSwapchain | matched |  | 09/02/2026 18:06:26 | Data structs/enum/typedef + MIDL boilerplate only, no functions. |
| `Print3DManagerInterop.h` | WinRT.Printing | remaining |  |  |  |
| `PrinterExtension.h` | Printing | remaining |  |  |  |
| `printerextensiondispid.h` | Printing | remaining |  |  |  |
| `PrintManagerInterop.h` | WinRT.Printing | remaining |  |  |  |
| `printoem.h` | Printing | remaining |  |  |  |
| `printpreview.h` | Printing | remaining |  |  |  |
| `prnasnot.h` | Gdi, Printing | remaining |  |  |  |
| `prnasntp.h` | Printing | matched | copilot | 09/02/2026 22:20:00 | Classified retained artifact in existing-patches-24. |
| `prntfont.h` | Printing | remaining |  |  |  |
| `prntvpt.h` | Gdi, PrintTicket | matched | copilot | 09/02/2026 23:20:00 | New resource-ownership patch created and verified via live re-scrape (build-level validation); supported-os left unresolved due to ambiguous/uncorroborated documentation. |
| `processenv.h` | Environment, FileHistory, Fs | matched | copilot | 09/02/2026 22:20:00 | Classified retained artifact in existing-patches-24. |
| `processsnapshot.h` | Proc_Snap | remaining |  |  |  |
| `processthreadsapi.h` | FileHistory, TermServ | matched | copilot | 09/02/2026 22:20:00 | Classified retained artifact in existing-patches-24. |
| `profileapi.h` | Perf | matched | copilot | 09/02/2026 22:20:00 | Classified retained artifact in existing-patches-24. |
| `profinfo.h` | Shell | remaining |  |  |  |
| `projectedfslib.h` | ProjFs | remaining |  |  |  |
| `proofofpossessioncookieinfo.h` | WinInet | remaining |  |  |  |
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
| `qnetwork.h` | Media.DShow | remaining |  |  |  |
| `qos2.h` | Qos | remaining |  |  |  |
| `qospol.h` | Qos | matched |  | 09/02/2026 17:48:13 | Data struct + constants only, no functions. |
| `qossp.h` | Qos | remaining |  |  |  |
| `query.h` | Search | remaining |  |  |  |
| `radialcontrollerinterop.h` | InputRadial | remaining |  |  |  |
| `radiomgr.h` | WpdSdk | remaining |  |  |  |
| `ras.h` | RRas | remaining |  |  |  |
| `rasdlg.h` | RRas | remaining |  |  |  |
| `raseapif.h` | Eap | remaining |  |  |  |
| `raserror.h` | RRas | remaining |  |  |  |
| `rasshost.h` | RRas | matched | copilot | 09/02/2026 22:50:00 | Classified retained artifact in existing-patches-26. |
| `ratings.h` | InternetExplorer | remaining |  |  |  |
| `rdpappcontainerclient.h` | TermServ | remaining |  |  |  |
| `rdpencomapi.h` | Rdp, TermServ | matched | copilot | 09/03/2026 05:00:00 | No patch needed; COM interface only. |
| `realtimeapiset.h` | Base | remaining |  |  |  |
| `reason.h` | FileHistory, Shutdown | remaining |  |  |  |
| `recapis.h` | Tablet | matched | copilot | 09/02/2026 22:50:00 | Classified retained artifact in existing-patches-26. |
| `recdefs.h` | Tablet | remaining |  |  |  |
| `reconcil.h` | Lwef, Shell | matched | copilot | 09/02/2026 22:50:00 | Classified retained artifact in existing-patches-26. |
| `rectypes.h` | Tablet | remaining |  |  |  |
| `regbag.h` | MsTv | remaining |  |  |  |
| `regstr.h` | Registry | remaining |  |  |  |
| `relogger.h` | Etw | remaining |  |  |  |
| `remotesystemadditionalinfo.h` | TermServ | remaining |  |  |  |
| `RemoteSystemsInterop.h` | WinRT | remaining |  |  |  |
| `rend.h` | Tapi3 | remaining |  |  |  |
| `rendezvoussession.h` | RemoteAssist | matched | copilot | 09/03/2026 05:15:00 | No patch needed; no function surface. |
| `resapi.h` | MsCs | remaining |  |  |  |
| `resourceindexer.h` | MenuRc | blocked | copilot | 09/02/2026 17:23:42 | Deferred: genuine ownership pair uses generic PVOID rather than a distinct handle typedef; needs a design decision to introduce a named type before an annotation fix is possible. |
| `restartmanager.h` | RstMgr | matched | copilot | 09/03/2026 04:35:00 | No patch needed; no ownership-relevant handle type. |
| `restrictederrorinfo.h` | WinRT | remaining |  |  |  |
| `richedit.h` | Controls.RichEdit | matched | copilot | 09/03/2026 00:00:00 | Classified retained artifact in existing-patches-27. |
| `richole.h` | Controls.RichEdit | matched | copilot | 09/03/2026 00:00:00 | Classified retained artifact in existing-patches-27. |
| `ringtonedeviceservice.h` | WpdSdk | matched |  | 09/02/2026 17:51:18 | GUID/name constants only, no functions (same pattern as notesdeviceservice.h). |
| `roapi.h` | TransactionServer, WinRT | remaining |  |  |  |
| `robuffer.h` | WinRT | matched |  | 09/02/2026 17:13:02 | COM-factory (RoGetBufferMarshaler) + buffer-view pattern (IBufferByteAccess), not HANDLE-family. |
| `roerrorapi.h` | WinRT | remaining |  |  |  |
| `rometadata.h` | WinRT.Metadata | matched |  | 09/02/2026 17:01:48 | COM QueryInterface-style factory (MetaDataGetDispenser), no raw HANDLE. |
| `rometadataapi.h` | WinRT.Metadata | remaining |  |  |  |
| `rometadataresolution.h` | WinRT.Metadata | remaining |  |  |  |
| `roparameterizediid.h` | WinRT.Metadata | remaining |  |  |  |
| `roregistrationapi.h` | WinRT | remaining |  |  |  |
| `rpc.h` | Rpc | remaining |  |  |  |
| `rpcasync.h` | Rpc | remaining |  |  |  |
| `rpcdce.h` | Rpc, TransactionServer | remaining |  |  |  |
| `rpcdcep.h` | Rpc | remaining |  |  |  |
| `rpcndr.h` | FileHistory, Rpc, WinProg | remaining |  |  |  |
| `rpcnsi.h` | Rpc | remaining |  |  |  |
| `rpcnsip.h` | Rpc | matched |  | 09/02/2026 17:41:54 | Internal I_-prefixed RPC stub routines; RPC_BINDING_HANDLE canonical API tracked separately under rpcdce.h (pending). |
| `rpcnterr.h` | Foundation | remaining |  |  |  |
| `rpcssl.h` | Rpc | matched |  | 09/02/2026 17:45:25 | RPC string-buffer output, not HANDLE-family. |
| `rrascfg.h` | Eap | remaining |  |  |  |
| `rtccore.h` | RealTimeCommunications | matched | copilot | 09/03/2026 03:50:00 | No patch needed; COM interface only. |
| `rtcerr.h` | RealTimeCommunications | matched | copilot | 09/03/2026 03:50:00 | No patch needed; constant-only header. |
| `rtinfo.h` | NetMgmt | remaining |  |  |  |
| `rtmv2.h` | RRas | matched | copilot | 09/03/2026 00:00:00 | Classified retained artifact in existing-patches-27. |
| `rtscom.h` | Tablet | remaining |  |  |  |
| `rtutils.h` | NetMgmt | remaining |  |  |  |
| `rtworkq.h` | Threading | remaining |  |  |  |
| `sapiddk.h` | Speech | remaining |  |  |  |
| `sas.h` | Identity | matched |  | 09/02/2026 16:53:36 | SendSAS(BOOL) - no handle involved. |
| `sbe.h` | MsTv | remaining |  |  |  |
| `sbtsv.h` | TermServ | remaining |  |  |  |
| `scarderr.h` | Credentials | remaining |  |  |  |
| `scclient.h` | Wmdm | remaining |  |  |  |
| `scesvc.h` | Security.ConfigurationSnapin | remaining |  |  |  |
| `schannel.h` | Certificates, Identity, Security, Security.AppLocker, Security.ConfigurationSnapin, Security.Cryptography, Security.Cryptography.Catalog, Security.Cryptography.Sip, Security.Cryptography.UI, Security.DiagnosticDataQuery, Security.DirectoryServices, Security.LicenseProtection, Security.Tpm, Security.WinTrust, Security.WinWlx, Wsw | matched | copilot | 09/03/2026 00:00:00 | Classified retained artifact in existing-patches-27. |
| `schedule.h` | ActiveDirectory | remaining |  |  |  |
| `schnlsp.h` | Identity | matched |  | 09/02/2026 17:01:48 | Redirect-only; schannel.h already accepted-normalized. |
| `scrnsave.h` | Shell | matched | copilot | 09/03/2026 00:00:00 | Classified retained artifact in existing-patches-27. |
| `sddl.h` | Authorization, Authorization.UI | matched | copilot | 09/03/2026 00:15:00 | Classified retained artifact in existing-patches-28. |
| `sdkddkver.h` | Antimalware, ApplicationVerifier, AppxPackaging, Audio, Audio.Apo, Audio.DirectMusic, Audio.DirectSound, Audio.Endpoints, Authorization, Authorization.UI, Backup, Base, Bits, Bluetooth, Buses, Cabinets, CEIP, Certificates, Cloudapi, ClrHosting, ClrProfiling, CmpApi, Com, Com.CallObj, Com.ChannelCreds, Com.Events, Com.StructuredStorage, Com.Urlmon, Communication, ComOle, CompositionSwapchain, Console, Controls, Controls.RichEdit, Cos, Credentials, DataXchg, Debug, Debug.ActiveScript, Debug.Extensions, Debug.WebApp, Dedup, DeliveryOptimization, DeviceAccess, DeviceQuery, Devices.1394, Devices.Beep, Devices.Cdrom, Devices.Dvd, Devices.Properties, DevInst, DevLic, Dfs, Dhcp, Direct2D, Direct2D.Common, Direct3D, Direct3D10, Direct3D11, Direct3D11on12, Direct3D12, Direct3D9, Direct3D9on12, Direct3DDxc, Direct3DFxc, DirectComp, DirectDraw, DirectManipulation, DirectML, DirectWrite, Display, DlgBox, Dlna, Dns, DTC, Dwm, DXCore, Dxgi, Dxgi.Common, Eap, EapHost, Edp, Enstor, Environment, Etw, Fax, FileHistory, Foundation, Fs, Fsrm, FunctionDiscovery, GameInput, GameMode, GamingDvcInfo, Gdi, Gdiplus, Gpmc, Hid, HostComputeNetwork, HostComputeSystem, HtmlHelp, Hypervisor, Identity, IdentityProvider, Ifsk, Iis, ImagingDevice, IMapi, IndexSrv, Input_Ink, Input_IntContext, Input.Ime, InputRadial, InternetConnectionWizard, InternetExplorer, Intl, IO, Ioctl, IScsiDisc, Isolation, JobObjects, Js, KeyboardAndMouseInput, Ldap, LibraryLoader, Lwef, MachineLearning, MagApi, Mapi, Mbn, MdmReg, Media, Media.DShow, Media.DShow.Xml, Media.DxMediaObjects, Media.KernelStreaming, Memory, MenuRc, MessageQueuing, Mf, Mi, MixedReality, MixedRealityInterop, Mmc, Monitor, MsChap, MsCs, MsHtml, MsTv, MsXml, Multimedia, Ndf, Ndis, NetBios, NetMgmt, NetShell, NetworkAccessProtection, Nfc, Nfp, Nla, Nps, NWifi, Of, Opc, OpenGL, Parcon, Perf, PicAcq, Pipes, Pla, Policy, Power, Printing, PrintTicket, Proc_Snap, ProjFs, Properties, PsApi1, PsApi2, PWM, Rdc, Rdp, RemoteAssist, Rm, Rpc, RRas, RstMgr, Search, Security, Security.AppLocker, Security.ConfigurationSnapin, Security.Cryptography, Security.Cryptography.Catalog, Security.Cryptography.Sip, Security.Cryptography.UI, Security.DiagnosticDataQuery, Security.DirectoryServices, Security.LicenseProtection, Security.Tpm, Security.WinTrust, Security.WinWlx, SecurityCenter, Sens, SerPorts, Services, Setup, Shell, Shell.Common, Smi, Snmp, Speech, Sr, Storage.Jet, Storage.Nvme, SwDevice, SystemInformation, Tablet, Tapi3, TaskSchd, Tbs, Tcui, TermServ, Threading, Time, ToolHelp, TransactionServer, Tsf, UiAnimation, UpdateAssessment, Upnp, Variant, Vhd, VirtualDiskService, VirtualDOSMachines, VSS, Wab, Wcm, Wcn, Wcs, Wds, WebAuthn, WebDav, WebServicesOnDevices, WebSock, Wec, Wer, Wes, Wia, Wibe, Wic, Win32_Tile_Badge_Notif, WinAuto, WinContacts, WindowsConnectionManager, WindowsFilteringPlatform, WindowsFirewall, WindowsRibbon, WindowsSetupAndMigration, WinInet, WinLocation, WinProg, WinRm, WinRT, WinRT.AllJoyn, WinRT.Composition, WinRT.CoreInputView, WinRT.Direct2D, WinRT.Direct3D11, WinRT.Display, WinRT.Graphics.Capture, WinRT.Graphics.Imaging, WinRT.Holographic, WinRT.Isolation, WinRT.Media, WinRT.Metadata, WinRT.ML, WinRT.Pdf, WinRT.Printing, WinRT.Storage, WinRT.Xaml, WinSat, WinSensors, WinSync, WinTouch, Wmdm, WmFormat, Wmi, Wmlss, Wmp, Wnet, WpdSdk, Wsb, Wsl, Wsw, Wua, Xaml_Diagnostics, Xaudio2, Xblidp, Xinput, XmlLite, Xps, XpsPrinting | remaining |  |  |  |
| `sdoias.h` | Nps | remaining |  |  |  |
| `search.h` | Search | matched |  | 09/02/2026 16:49:01 | Redirect-only header (#include corecrt_search.h); CRT-only, no Win32 declarations. |
| `searchapi.h` | Lwef, Search | remaining |  |  |  |
| `security.h` | Identity | matched |  | 09/02/2026 18:09:40 | Redirect-only; sspi.h accepted, issper16.h tracked pending, secext.h (untracked) directly verified clean (string-buffer-output only). |
| `securityappcontainer.h` | Isolation | matched | copilot | 09/03/2026 00:15:00 | Classified retained artifact in existing-patches-28. |
| `securitybaseapi.h` | Etw, Identity, Security | matched | copilot | 09/02/2026 20:35:00 | Audited under corrected shared-handle policy (165b5f09, 7335ddc4); already compliant, no code changes required. |
| `segment.h` | Media.DShow, MsTv | remaining |  |  |  |
| `sens.h` | Sens | remaining |  |  |  |
| `sensapi.h` | Sens | matched |  | 09/02/2026 17:41:54 | No handle involved. |
| `sensevts.h` | Sens | remaining |  |  |  |
| `sensors.h` | WinSensors | remaining |  |  |  |
| `sensorsapi.h` | WinLocation, WinSensors | remaining |  |  |  |
| `sensorsdef.h` | WinSensors | remaining |  |  |  |
| `sensorsstructures.h` | WinSensors | matched |  | 09/02/2026 18:00:52 | Data structs/enum only, no functions. |
| `sensorsutils.h` | WinSensors | remaining |  |  |  |
| `sessdirpublictypes.h` | TermServ | remaining |  |  |  |
| `setupapi.h` | DevInst, Setup | matched | copilot | 09/03/2026 00:15:00 | Classified retained artifact in existing-patches-28. |
| `sfc.h` | Setup | matched | copilot | 09/03/2026 00:15:00 | Classified retained artifact in existing-patches-28. |
| `shappmgr.h` | Shell | matched | copilot | 09/03/2026 00:15:00 | Classified retained artifact in existing-patches-28. |
| `sharewindowcommandsourceinterop.h` | WinRT | remaining |  |  |  |
| `shcore.h` | WinRT | matched |  | 09/02/2026 17:54:05 | COM factory pattern. |
| `shdeprecated.h` | Shell | matched | copilot | 09/03/2026 00:30:00 | Classified retained artifact in existing-patches-29. |
| `shdispid.h` | Shell | remaining |  |  |  |
| `shellapi.h` | Properties, Shell | matched | copilot | 09/03/2026 00:30:00 | Classified retained artifact in existing-patches-29. |
| `shellscalingapi.h` | Shell | matched | copilot | 09/03/2026 00:30:00 | Classified retained artifact in existing-patches-29. |
| `sherrors.h` | Shell | remaining |  |  |  |
| `shidfact.h` | Shell | remaining |  |  |  |
| `shimgdata.h` | Shell | matched | copilot | 09/03/2026 00:30:00 | Classified retained artifact in existing-patches-29. |
| `shldisp.h` | Lwef, Shell | matched | copilot | 09/03/2026 00:30:00 | Classified retained artifact in existing-patches-29. |
| `shlguid.h` | Shell | remaining |  |  |  |
| `shlobj_core.h` | ActiveDirectory, Lwef, Properties, Shell | matched | copilot | 09/03/2026 00:45:00 | Classified retained artifact in existing-patches-30. |
| `shlobj.h` | Controls, Lwef, Shell | matched | copilot | 09/03/2026 00:45:00 | Classified retained artifact in existing-patches-30. |
| `shlwapi.h` | Shell | matched | copilot | 09/03/2026 00:45:00 | Classified retained artifact in existing-patches-30. |
| `shobjidl_core.h` | Properties, Search, Shell | matched | copilot | 09/03/2026 00:45:00 | Classified retained artifact in existing-patches-30. |
| `shobjidl.h` | Properties, Shell, TransactionServer | matched | copilot | 09/03/2026 00:45:00 | Classified retained artifact in existing-patches-30. |
| `shtypes.h` | Shell, Shell.Common | remaining |  |  |  |
| `simpdata.h` | Search | remaining |  |  |  |
| `slerror.h` | Identity | remaining |  |  |  |
| `sliddefs.h` | Identity | matched |  | 09/02/2026 16:57:06 | GUID constant only, no functions. |
| `slpublic.h` | Identity | remaining |  |  |  |
| `smbclnt.h` | MsCs | remaining |  |  |  |
| `smtpguid.h` | IMapi | remaining |  |  |  |
| `snmp.h` | Snmp | matched | copilot | 09/03/2026 01:00:00 | Classified retained artifact in existing-patches-31. |
| `socketapi.h` | WinSock | matched |  | 09/02/2026 16:57:06 | SetSocketMediaStreamingMode(BOOL) - no handle. |
| `softpub.h` | Security.WinTrust | remaining |  |  |  |
| `spatialaudioclient.h` | Audio | remaining |  |  |  |
| `spatialaudiohrtf.h` | Audio | remaining |  |  |  |
| `spatialaudiometadata.h` | Audio | remaining |  |  |  |
| `spatialinteractionmanagerinterop.h` | MixedRealityInterop | remaining |  |  |  |
| `spellcheck.h` | Intl | remaining |  |  |  |
| `spellcheckprovider.h` | Intl | remaining |  |  |  |
| `sperror.h` | Speech | remaining |  |  |  |
| `sporder.h` | WinSock | matched |  | 09/02/2026 17:45:25 | DWORD/GUID array API only, no handle. |
| `sql.h` | Search | remaining |  |  |  |
| `sqlext.h` | Search | remaining |  |  |  |
| `sqloledb.h` | Search | remaining |  |  |  |
| `sqlspi.h` | Search | remaining |  |  |  |
| `sqltypes.h` | Search | remaining |  |  |  |
| `sqlucode.h` | Search | remaining |  |  |  |
| `srpapi.h` | Edp | remaining |  |  |  |
| `srrestoreptapi.h` | Sr | matched | copilot | 09/03/2026 01:00:00 | Classified retained artifact in existing-patches-31. |
| `sslprovider.h` | Security.Cryptography | remaining |  |  |  |
| `sspi.h` | Certificates, Identity, Security, Security.AppLocker, Security.ConfigurationSnapin, Security.Cryptography, Security.Cryptography.Catalog, Security.Cryptography.Sip, Security.Cryptography.UI, Security.DiagnosticDataQuery, Security.DirectoryServices, Security.LicenseProtection, Security.Tpm, Security.WinTrust, Security.WinWlx | matched | copilot | 09/02/2026 20:35:00 | Audited under corrected shared-handle policy (165b5f09, 7335ddc4); already compliant, no code changes required. |
| `statehelpers.h` | FileHistory, Registry | matched |  | 09/02/2026 18:09:40 | HKEY parameters are inputs only, not produced here. |
| `statusdeviceservice.h` | WpdSdk | remaining |  |  |  |
| `stdint.h` | AllJoyn | remaining |  |  |  |
| `stgprop.h` | Search | remaining |  |  |  |
| `sti.h` | ImagingDevice | remaining |  |  |  |
| `stierr.h` | ImagingDevice | remaining |  |  |  |
| `stireg.h` | ImagingDevice | remaining |  |  |  |
| `stiusd.h` | ImagingDevice | remaining |  |  |  |
| `storageprovider.h` | Shell | remaining |  |  |  |
| `storprop.h` | Base | remaining |  |  |  |
| `stralign.h` | FileHistory, WinProg | remaining |  |  |  |
| `stringapiset.h` | Intl | remaining |  |  |  |
| `strmif.h` | Media.DShow, Media.KernelStreaming, MsTv, WmFormat | remaining |  |  |  |
| `strsafe.h` | MenuRc | remaining |  |  |  |
| `structuredquery.h` | Search | remaining |  |  |  |
| `structuredquerycondition.h` | Search | remaining |  |  |  |
| `subauth.h` | Identity | matched | copilot | 09/03/2026 01:00:00 | Classified retained artifact in existing-patches-31. |
| `subscriptionservices.h` | Wmp | remaining |  |  |  |
| `subsmgr.h` | Search | remaining |  |  |  |
| `svcguid.h` | WinSock | remaining |  |  |  |
| `swdevice.h` | SwDevice | remaining |  |  |  |
| `swdevicedef.h` | SwDevice | matched |  | 09/02/2026 17:30:54 | Type declarations only (enums/struct), no functions; HSWDEVICE ownership tracked separately under swdevice.h (pending). |
| `syncdeviceservice.h` | WpdSdk | remaining |  |  |  |
| `synchronizationerrors.h` | WinSync | remaining |  |  |  |
| `syncmgr.h` | Shell | matched | copilot | 09/03/2026 01:00:00 | Classified retained artifact in existing-patches-31. |
| `syncregistration.h` | WinSync | remaining |  |  |  |
| `syncregistrationerrors.h` | WinSync | matched |  | 09/02/2026 18:09:40 | HRESULT error-code constants only, no functions. |
| `sysinfoapi.h` | SystemInformation | matched | copilot | 09/03/2026 01:00:00 | Classified retained artifact in existing-patches-31. |
| `systemmediatransportcontrolsinterop.h` | WinRT | remaining |  |  |  |
| `t2embapi.h` | Gdi | remaining |  |  |  |
| `tabflicks.h` | Tablet | matched |  | 09/02/2026 17:58:11 | Enums/bitfield structs only, no functions. |
| `tapi.h` | Tapi3 | remaining |  |  |  |
| `tapi3.h` | Tapi3 | remaining |  |  |  |
| `tapi3ds.h` | Tapi3 | remaining |  |  |  |
| `tapi3err.h` | Tapi3 | remaining |  |  |  |
| `tapi3if.h` | Tapi3 | remaining |  |  |  |
| `taskdeviceservice.h` | WpdSdk | remaining |  |  |  |
| `taskschd.h` | TaskSchd | remaining |  |  |  |
| `tbs.h` | Tbs | matched | copilot | 09/03/2026 04:05:00 | New resource-ownership patch created and verified via live re-scrape (build-level validation). |
| `tcerror.h` | Qos | remaining |  |  |  |
| `tcguid.h` | Qos | remaining |  |  |  |
| `tcpxcv.h` | Printing | remaining |  |  |  |
| `tdh.h` | Etw | remaining |  |  |  |
| `tdiinfo.h` | FileHistory, WinProg | remaining |  |  |  |
| `textserv.h` | Controls.RichEdit | matched | copilot | 09/03/2026 01:15:00 | Classified retained artifact in existing-patches-32. |
| `textstor.h` | Tsf | remaining |  |  |  |
| `thumbcache.h` | Shell | matched | copilot | 09/03/2026 01:15:00 | Classified retained artifact in existing-patches-32. |
| `thumbnailstreamcache.h` | Shell | matched | copilot | 09/03/2026 01:15:00 | Classified retained artifact in existing-patches-32. |
| `timeapi.h` | Media, Multimedia | remaining |  |  |  |
| `timezoneapi.h` | FileHistory, Time | matched | copilot | 09/03/2026 01:15:00 | Classified retained artifact in existing-patches-32. |
| `tipautocomplete.h` | Tablet | remaining |  |  |  |
| `tlhelp32.h` | ToolHelp | matched | copilot | 09/03/2026 01:15:00 | Classified retained artifact in existing-patches-32. |
| `tlogstg.h` | Shell | remaining |  |  |  |
| `tnef.h` | Tapi3 | remaining |  |  |  |
| `tokenbinding.h` | Identity | remaining |  |  |  |
| `tom.h` | Controls.RichEdit | matched | copilot | 09/03/2026 01:30:00 | Classified retained artifact in existing-patches-33. |
| `tpcerror.h` | Tablet | remaining |  |  |  |
| `tpcshrd.h` | Tablet | remaining |  |  |  |
| `TpmVscAttestation.h` | Security.Tpm | matched |  | 09/02/2026 18:03:29 | Enum + MIDL boilerplate only, no functions. |
| `tpmvscmgr.h` | Security.Tpm | remaining |  |  |  |
| `traffic.h` | Qos | remaining |  |  |  |
| `transportsettingcommon.h` | WinSock | matched |  | 09/02/2026 16:57:06 | Plain data struct only, no functions. |
| `transportsettings.h` | RealTimeCommunications | matched | copilot | 09/03/2026 03:50:00 | No patch needed; COM interface only. |
| `tsattrs.h` | Tsf | remaining |  |  |  |
| `tsgauthenticationengine.h` | TermServ | remaining |  |  |  |
| `tsgpolicyengine.h` | TermServ | remaining |  |  |  |
| `TSPI.h` | Tapi3 | remaining |  |  |  |
| `tspubplugin2com.h` | TermServ | remaining |  |  |  |
| `tspubplugincom.h` | TermServ | remaining |  |  |  |
| `tssbx.h` | TermServ | remaining |  |  |  |
| `tsuserex.h` | TermServ | remaining |  |  |  |
| `tsvirtualchannels.h` | TermServ | remaining |  |  |  |
| `tuner.h` | MsTv | remaining |  |  |  |
| `tvout.h` | Gdi | remaining |  |  |  |
| `tvratings.h` | MsTv | remaining |  |  |  |
| `txdtc.h` | DTC | remaining |  |  |  |
| `txfw32.h` | Fs | matched | copilot | 09/03/2026 01:30:00 | Classified retained artifact in existing-patches-33. |
| `txlogpub.h` | TransactionServer | remaining |  |  |  |
| `ual.h` | Ual | matched | copilot | 09/02/2026 22:59:00 | First header-scraping-investigation batch: no prior patch existed. New supported-os patch created and verified via live re-scrape, not just static review. |
| `uianimation.h` | UiAnimation | remaining |  |  |  |
| `uiautomationclient.h` | WinAuto | matched | copilot | 09/03/2026 01:30:00 | Classified retained artifact in existing-patches-33. |
| `uiautomationcore.h` | WinAuto | matched | copilot | 09/03/2026 01:30:00 | Classified retained artifact in existing-patches-33. |
| `uiautomationcoreapi.h` | WinAuto | matched | copilot | 09/03/2026 01:30:00 | Classified retained artifact in existing-patches-33. |
| `uiribbon.h` | WindowsRibbon | remaining |  |  |  |
| `UIViewSettingsInterop.h` | WinRT | remaining |  |  |  |
| `upnp.h` | Upnp | remaining |  |  |  |
| `upnphost.h` | Upnp | remaining |  |  |  |
| `urlhist.h` | InternetExplorer | remaining |  |  |  |
| `urlmon.h` | Shell, TransactionServer | matched | copilot | 09/03/2026 01:45:00 | Classified retained artifact in existing-patches-34. |
| `usb.h` | Buses | remaining |  |  |  |
| `usb4dbgioctl.h` | Buses | remaining |  |  |  |
| `usbfnbase.h` | Buses | remaining |  |  |  |
| `usbioctl.h` | Buses | remaining |  |  |  |
| `usbiodef.h` | Buses | remaining |  |  |  |
| `usbprint.h` | Printing | remaining |  |  |  |
| `usbscan.h` | Buses | remaining |  |  |  |
| `usbuser.h` | Buses | remaining |  |  |  |
| `useractivityinterop.h` | WinRT | remaining |  |  |  |
| `UserConsentVerifierInterop.h` | WinRT | remaining |  |  |  |
| `userenv.h` | Policy, Shell | matched | copilot | 09/03/2026 01:45:00 | Classified retained artifact in existing-patches-34. |
| `usp10.h` | Intl | remaining |  |  |  |
| `utilapiset.h` | Base, Debug | remaining |  |  |  |
| `uuids.h` | Media, Mf | remaining |  |  |  |
| `uxtheme.h` | Controls, WinTouch | matched | copilot | 09/03/2026 01:45:00 | Classified retained artifact in existing-patches-34. |
| `vbinterf.h` | Com, Com.CallObj, Com.ChannelCreds, Com.Urlmon, ComOle, TransactionServer | remaining |  |  |  |
| `vdmdbg.h` | FileHistory, VirtualDOSMachines | remaining |  |  |  |
| `vds.h` | VirtualDiskService | remaining |  |  |  |
| `vdserr.h` | VirtualDiskService | remaining |  |  |  |
| `vdslun.h` | VirtualDiskService | remaining |  |  |  |
| `vdssys.h` | VirtualDiskService, VSS | remaining |  |  |  |
| `VersionHelpers.h` | SystemInformation | remaining |  |  |  |
| `vfw.h` | Multimedia | remaining |  |  |  |
| `vfwext.h` | Multimedia | remaining |  |  |  |
| `vfwmsgs.h` | Media.DShow | remaining |  |  |  |
| `vidcap.h` | Media.DShow | remaining |  |  |  |
| `videoacc.h` | Media.DShow | remaining |  |  |  |
| `virtdisk.h` | Vhd | remaining |  |  |  |
| `vmgenerationcounter.h` | Hypervisor | matched |  | 09/02/2026 17:19:28 | GUID/IOCTL constant + data struct only, no functions. |
| `vmr9.h` | Media.DShow | remaining |  |  |  |
| `vmsavedstatedump.h` | Hypervisor | remaining |  |  |  |
| `vmsavedstatedumpdefs.h` | Hypervisor | remaining |  |  |  |
| `vpconfig.h` | Media.DShow | remaining |  |  |  |
| `vpnotify.h` | Media.DShow | remaining |  |  |  |
| `vptype.h` | Media.DShow | remaining |  |  |  |
| `vsadmin.h` | VSS | remaining |  |  |  |
| `vsbackup.h` | VSS | remaining |  |  |  |
| `vsmgmt.h` | VSS | remaining |  |  |  |
| `vsprov.h` | VSS | remaining |  |  |  |
| `vss.h` | VSS | remaining |  |  |  |
| `vsserror.h` | VSS | remaining |  |  |  |
| `vsstyle.h` | Controls | remaining |  |  |  |
| `vssym32.h` | Controls | remaining |  |  |  |
| `vswriter.h` | VSS | remaining |  |  |  |
| `waasapi.h` | UpdateAssessment | remaining |  |  |  |
| `waasapitypes.h` | UpdateAssessment | remaining |  |  |  |
| `wab.h` | Wab | blocked | copilot | 09/02/2026 16:53:36 | Deferred: 1 of 77 functions (FtgRegisterIdleRoutine) has genuine return-value handle ownership with no annotation precedent; needs the same dedicated policy decision as getprocesshandlefromhwnd.h. |
| `wabdefs.h` | IMapi, Tapi3 | remaining |  |  |  |
| `wbcl.h` | Qos | remaining |  |  |  |
| `wbemads.h` | Wmi | remaining |  |  |  |
| `wbemcli.h` | TermServ, Wmi | remaining |  |  |  |
| `wbemdisp.h` | Wmi | remaining |  |  |  |
| `wbemidl.h` | Wmi | matched |  | 09/02/2026 16:57:06 | Redirect-only; 40 functions all MIDL RPC marshalling stubs, no DECLARE_HANDLE. |
| `wbemprov.h` | Wmi | remaining |  |  |  |
| `wbemtran.h` | Wmi | remaining |  |  |  |
| `wcmapi.h` | Wcm | remaining |  |  |  |
| `wcmconfig.h` | Smi | remaining |  |  |  |
| `wcmerrors.h` | Smi | remaining |  |  |  |
| `wcnapi.h` | Wcn | matched |  | 09/02/2026 17:01:48 | Redirect-only; 0 functions in entire Wcn partition, no DECLARE_HANDLE. |
| `wcnfunctiondiscoverykeys.h` | Wcn | matched |  | 09/02/2026 17:35:31 | GUID/PROPERTYKEY constants only, no functions. |
| `wcsplugin.h` | Wcs | remaining |  |  |  |
| `wct.h` | Base, Debug | remaining |  |  |  |
| `WDBGEXTS.H` | Debug.Extensions | remaining |  |  |  |
| `wdigest.h` | Identity | matched |  | 09/02/2026 17:27:04 | String constants only, no functions. |
| `wdmguid.h` | DevInst | remaining |  |  |  |
| `wdsbp.h` | Wds | remaining |  |  |  |
| `wdsclientapi.h` | Wds | matched | copilot | 09/03/2026 01:45:00 | Classified retained artifact in existing-patches-34. |
| `wdsmcerr.h` | Wds | remaining |  |  |  |
| `wdspxe.h` | Wds | matched | copilot | 09/03/2026 01:45:00 | Classified retained artifact in existing-patches-34. |
| `wdstci.h` | Wds | remaining |  |  |  |
| `wdstpdi.h` | Wds | remaining |  |  |  |
| `wdstptmgmt.h` | Wds | remaining |  |  |  |
| `wdstptmgmtmsg.h` | Wds | remaining |  |  |  |
| `weakreference.h` | WinRT | remaining |  |  |  |
| `webapplication.h` | Debug.WebApp | remaining |  |  |  |
| `WebAuthenticationCoreManagerInterop.h` | WinRT | remaining |  |  |  |
| `webauthn.h` | WebAuthn | remaining |  |  |  |
| `webauthnplugin.h` | WebAuthn | remaining |  |  |  |
| `webevnts.h` | InternetExplorer | remaining |  |  |  |
| `webservices.h` | Wsw | remaining |  |  |  |
| `websocket.h` | WebSock | remaining |  |  |  |
| `werapi.h` | Wer | remaining |  |  |  |
| `wheadef.h` | Debug | remaining |  |  |  |
| `wia.h` | Wia | matched |  | 09/02/2026 16:53:36 | Redirect-only; 185 functions all COM/RPC marshalling stubs, no DECLARE_HANDLE. |
| `wiadef.h` | Wia | remaining |  |  |  |
| `wiadevd.h` | Wia | remaining |  |  |  |
| `wiaintfc.h` | Wia | matched |  | 09/02/2026 17:05:19 | Device-interface GUID constant only, no functions. |
| `wiamicro.h` | Wia | remaining |  |  |  |
| `wiamindr_lh.h` | Wia | remaining |  |  |  |
| `wiamindr.h` | Wia | matched |  | 09/02/2026 16:49:01 | Redirect-only header (#include wiamindr_lh.h/wiamindr_xp.h); no direct declarations. |
| `wiatwcmp.h` | Wia | matched |  | 09/02/2026 18:00:53 | Data struct + constants only, no functions. |
| `wiavideo.h` | Wia | remaining |  |  |  |
| `wiawsdsc.h` | Wia | remaining |  |  |  |
| `winbase.h` | Base, DataXchg, FileHistory, Fs, Identity, Input.Ime, Intl, IO, MenuRc, Registry, Security, Security.AppLocker, Security.ConfigurationSnapin, Security.Cryptography.Catalog, Security.Cryptography.Sip, Security.DiagnosticDataQuery, Security.DirectoryServices, Security.LicenseProtection, Security.Tpm, Security.WinTrust, Security.WinWlx, Setup, Shutdown, TermServ, WinProg | matched | copilot | 09/03/2026 02:00:00 | Classified retained artifact in existing-patches-35. |
| `winber.h` | Ldap | remaining |  |  |  |
| `winbio_adapter.h` | SecBitomet | remaining |  |  |  |
| `winbio_err.h` | SecBitomet | remaining |  |  |  |
| `winbio_ioctl.h` | SecBitomet | remaining |  |  |  |
| `winbio_types.h` | SecBitomet | remaining |  |  |  |
| `winbio.h` | SecBitomet | remaining |  |  |  |
| `wincodec.h` | Wic | matched | copilot | 09/03/2026 02:00:00 | Classified retained artifact in existing-patches-35. |
| `wincodecsdk.h` | Wic | matched | copilot | 09/03/2026 02:00:00 | Classified retained artifact in existing-patches-35. |
| `wincon.h` | Console | blocked | copilot | 09/02/2026 17:19:28 | Deferred: wincon.h redirects to consoleapi.h/consoleapi2.h/consoleapi3.h/wincontypes.h (none separately tracked); live-scrape of the Console partition (~120 functions) found a genuine multi-producer/single-consumer return-value HANDLE ownership relationship with no annotation precedent, same class as getprocesshandlefromhwnd.h/wab.h. |
| `winconp.h` | Console | remaining |  |  |  |
| `wincred.h` | Certificates, Credentials, Identity, Security, Security.AppLocker, Security.ConfigurationSnapin, Security.Cryptography, Security.Cryptography.Catalog, Security.Cryptography.Sip, Security.Cryptography.UI, Security.DiagnosticDataQuery, Security.DirectoryServices, Security.LicenseProtection, Security.Tpm, Security.WinTrust, Security.WinWlx | matched | copilot | 09/03/2026 02:00:00 | Classified retained artifact in existing-patches-35. |
| `wincrypt.h` | NetMgmt, Rpc, Security.Cryptography, Security.Cryptography.UI | matched | copilot | 09/02/2026 22:40:00 | Corrected typedef-owned ownership violation found during resource-ownership audit; final header in the 11-header audit set (all now compliant). Verified compatible with co-resident zz-crypto-security-enums patch via full replay. |
| `winddi.h` | Display, IO, Printing, TransactionServer | remaining |  |  |  |
| `winddiui.h` | Printing | remaining |  |  |  |
| `windef.h` | Foundation, Gdi, WinAuto | matched | copilot | 09/02/2026 19:18:45 | Closed in shared-handle-policy-01; current windows-rs full test target is blocked by missing PartitionSpec.include_main_file initializers. |
| `windns.h` | Dns, IpHlp | matched | copilot | 09/03/2026 02:00:00 | Classified retained artifact in existing-patches-35. |
| `windnsdef.h` | Dns | remaining |  |  |  |
| `windot11.h` | Ndis, NWifi | remaining |  |  |  |
| `windows.ai.machinelearning.native.h` | WinRT.ML | remaining |  |  |  |
| `windows.data.pdf.interop.h` | WinRT.Pdf | remaining |  |  |  |
| `windows.devices.alljoyn.interop.h` | WinRT.AllJoyn | remaining |  |  |  |
| `Windows.Devices.Display.Core.Interop.h` | WinRT.Display | remaining |  |  |  |
| `windows.devices.midi.h` | Media.DShow | remaining |  |  |  |
| `windows.fixed.h` | AllJoyn, Antimalware, ApplicationVerifier, AppxPackaging, Audio, Audio.Apo, Audio.DirectMusic, Audio.DirectSound, Audio.Endpoints, Authorization, Authorization.UI, Backup, Base, Bits, Bluetooth, Buses, Cabinets, CEIP, Certificates, Cloudapi, ClrHosting, ClrProfiling, CmpApi, Com, Com.CallObj, Com.ChannelCreds, Com.Events, Com.StructuredStorage, Com.Urlmon, Communication, ComOle, CompositionSwapchain, Console, Controls, Controls.RichEdit, Cos, Credentials, DataXchg, Debug, Debug.ActiveScript, Debug.Extensions, Debug.WebApp, Dedup, DeliveryOptimization, DeviceAccess, DeviceQuery, Devices.1394, Devices.Beep, Devices.Cdrom, Devices.Dvd, Devices.Properties, DevInst, DevLic, Dfs, Dhcp, Direct2D, Direct2D.Common, Direct3D, Direct3D10, Direct3D11, Direct3D11on12, Direct3D12, Direct3D9, Direct3D9on12, Direct3DDxc, Direct3DFxc, DirectComp, DirectDraw, DirectManipulation, DirectML, DirectWrite, Display, DlgBox, Dlna, Dns, DTC, Dwm, DXCore, Dxgi, Dxgi.Common, Eap, EapHost, Environment, Etw, Fax, FileHistory, Foundation, Fs, Fsrm, FunctionDiscovery, GameInput, GameMode, GamingDvcInfo, Gdi, Gdiplus, Gpmc, Hid, HostComputeNetwork, HostComputeSystem, HtmlHelp, Hypervisor, Identity, IdentityProvider, Ifsk, Iis, ImagingDevice, IMapi, IndexSrv, Input_Ink, Input_IntContext, Input.Ime, InputRadial, InternetConnectionWizard, InternetExplorer, Intl, IO, Ioctl, IScsiDisc, Isolation, JobObjects, Js, KeyboardAndMouseInput, Ldap, LibraryLoader, Lwef, MachineLearning, MadCap, MagApi, Mapi, Mbn, MdmReg, Media, Media.DShow, Media.DShow.Xml, Media.DxMediaObjects, Media.KernelStreaming, Memory, MenuRc, MessageQueuing, Mf, MixedReality, MixedRealityInterop, Mmc, Monitor, MsChap, MsCs, MsHtml, MsTv, MsXml, Multimedia, Ndf, Ndis, NetBios, NetMgmt, NetShell, NetworkAccessProtection, Nfc, Nfp, Nla, Nps, NWifi, Of, Opc, OpenGL, Parcon, Perf, PicAcq, Pipes, Pla, Policy, Power, Printing, PrintTicket, Proc_Snap, ProjFs, Properties, PsApi1, PsApi2, PWM, Rdc, Rdp, RealTimeCommunications, Registry, RemoteAssist, Rm, Rpc, RRas, RstMgr, Search, SecBitomet, Security, Security.AppLocker, Security.ConfigurationSnapin, Security.Cryptography, Security.Cryptography.Catalog, Security.Cryptography.Sip, Security.Cryptography.UI, Security.DiagnosticDataQuery, Security.DirectoryServices, Security.LicenseProtection, Security.Tpm, Security.WinTrust, Security.WinWlx, SecurityCenter, Sens, SerPorts, Services, Setup, Shell, Shell.Common, Shutdown, SideShow, Smi, Snmp, Speech, Sr, Storage.Jet, Storage.Nvme, SwDevice, SystemInformation, Tablet, Tapi3, TaskSchd, Tbs, Tcui, TermServ, Threading, Time, ToolHelp, TransactionServer, Tsf, UiAnimation, UpdateAssessment, Upnp, Variant, Vhd, VirtualDiskService, VirtualDOSMachines, VSS, Wab, Wcm, Wcn, Wcs, Wds, WebAuthn, WebDav, WebServicesOnDevices, WebSock, Wec, Wer, Wes, Wia, Wibe, Wic, Win32_Tile_Badge_Notif, WinAuto, WinContacts, WindowsConnectionManager, WindowsFirewall, WindowsRibbon, WindowsSetupAndMigration, WinInet, WinLocation, WinProg, WinRm, WinRT, WinRT.AllJoyn, WinRT.Composition, WinRT.CoreInputView, WinRT.Direct2D, WinRT.Direct3D11, WinRT.Display, WinRT.Graphics.Capture, WinRT.Graphics.Imaging, WinRT.Holographic, WinRT.Isolation, WinRT.Media, WinRT.Metadata, WinRT.ML, WinRT.Pdf, WinRT.Printing, WinRT.Storage, WinRT.Xaml, WinSat, WinSensors, WinSync, WinTouch, Wmdm, WmFormat, Wmi, Wmlss, Wmp, Wnet, WpdSdk, Wsb, Wsl, Wsw, Wua, Xaml_Diagnostics, Xaudio2, Xblidp, Xinput, XmlLite, Xps, XpsPrinting | remaining |  |  |  |
| `Windows.Graphics.Capture.Interop.h` | WinRT.Graphics.Capture | matched |  | 09/02/2026 17:35:31 | COM factory + query-only HWND/HMONITOR accessors. |
| `windows.graphics.directx.direct3d11.interop.h` | WinRT.Direct3D11 | remaining |  |  |  |
| `windows.graphics.effects.interop.h` | WinRT.Direct2D | remaining |  |  |  |
| `Windows.Graphics.Holographic.Interop.h` | WinRT.Holographic | remaining |  |  |  |
| `windows.graphics.imaging.interop.h` | WinRT.Graphics.Imaging | remaining |  |  |  |
| `windows.graphics.interop.h` | WinRT.Direct2D | matched |  | 09/02/2026 17:41:54 | COM factory pattern. |
| `windows.graphics.printing.workflow.native.h` | WinRT.Printing | remaining |  |  |  |
| `windows.h` | Console, Edp, Enstor, Memory, Mi | remaining |  |  |  |
| `windows.media.core.interop.h` | WinRT.Media | remaining |  |  |  |
| `windows.security.isolation.isolatedenvironmentinterop.h` | WinRT.Isolation | remaining |  |  |  |
| `windows.ui.composition.interop.h` | WinRT.Composition | remaining |  |  |  |
| `windows.ui.viewmanagement.core.coreframeworkinputviewinterop.h` | WinRT.CoreInputView | remaining |  |  |  |
| `windows.ui.xaml.hosting.desktopwindowxamlsource.h` | WinRT.Xaml | remaining |  |  |  |
| `windows.ui.xaml.hosting.referencetracker.h` | WinRT.Xaml | remaining |  |  |  |
| `windows.ui.xaml.media.dxinterop.h` | WinRT.Xaml | remaining |  |  |  |
| `windowsceip.h` | Base, CEIP, FileHistory | matched |  | 09/02/2026 17:01:48 | CeipIsOptedIn() - no handle involved. |
| `windowsdefender.h` | Lwef | remaining |  |  |  |
| `windowssearcherrors.h` | Search | remaining |  |  |  |
| `windowssideshow.h` | SideShow | matched | copilot | 09/03/2026 03:50:00 | No patch needed; COM interface only. |
| `windowssideshowapi.h` | SideShow | matched | copilot | 09/03/2026 03:50:00 | No patch needed; COM interface only. |
| `windowssideshowdriverevents.h` | SideShow | matched | copilot | 09/03/2026 04:05:00 | No patch needed; COM interface only. |
| `windowsstoragecom.h` | WinRT.Storage | remaining |  |  |  |
| `windowsx.h` | Controls, FileHistory, Gdi, WinProg | remaining |  |  |  |
| `winefs.h` | Fs | matched | copilot | 09/03/2026 02:15:00 | Classified retained artifact in existing-patches-36. |
| `winenclave.h` | Enclave | matched | copilot | 09/03/2026 03:20:00 | No patch needed; verified via live scrape and static review. |
| `winerror.h` | Direct2D, DXCore, Dxgi, Dxgi.Common, Foundation, Rpc, TaskSchd, TransactionServer, UiAnimation, Wes | remaining |  |  |  |
| `winevt.h` | Wes | matched | copilot | 09/03/2026 02:15:00 | Classified retained artifact in existing-patches-36. |
| `winfax.h` | Fax | remaining |  |  |  |
| `wingdi.h` | DataXchg, Direct3D9, Gdi, Intl, Media.DShow, Tablet, Wcs | matched | copilot | 09/03/2026 02:15:00 | Classified retained artifact in existing-patches-36. |
| `winhttp.h` | HttpServer, WinHttp | matched | copilot | 09/03/2026 02:15:00 | Classified retained artifact in existing-patches-36. |
| `WinHvEmulation.h` | Hypervisor | remaining |  |  |  |
| `WinHvPlatform.h` | Hypervisor | remaining |  |  |  |
| `wininet.h` | FileHistory, WinInet, WinProg | matched | copilot | 09/03/2026 02:15:00 | Classified retained artifact in existing-patches-36. |
| `winineti.h` | WinInet | matched | copilot | 09/03/2026 02:30:00 | Classified retained artifact in existing-patches-37. |
| `winioctl.h` | Base, Fs, Ioctl, Nfc, Nfp | remaining |  |  |  |
| `winldap.h` | Ldap | matched | copilot | 09/03/2026 02:30:00 | Classified retained artifact in existing-patches-37. |
| `winmeta.h` | TraceLogging | matched | copilot | 09/03/2026 03:20:00 | No patch needed; constant-only header verified via live scrape. |
| `winml.h` | MachineLearning | remaining |  |  |  |
| `winnetwk.h` | Wnet | matched | copilot | 09/03/2026 02:30:00 | Classified retained artifact in existing-patches-37. |
| `winnls.h` | Intl | remaining |  |  |  |
| `winnls32.h` | FileHistory, WinProg | remaining |  |  |  |
| `winnt.h` | Backup, Base, Display, DXCore, Dxgi, Dxgi.Common, FileHistory, Fs, Identity, Input.Ime, Intl, IO, JobObjects, MenuRc, Registry, Security, Security.AppLocker, Security.ConfigurationSnapin, Security.Cryptography.Catalog, Security.Cryptography.Sip, Security.DiagnosticDataQuery, Security.DirectoryServices, Security.LicenseProtection, Security.Tpm, Security.WinTrust, Security.WinWlx, Setup, Shell, Shutdown, Threading, WinProg | remaining |  |  |  |
| `winperf.h` | Perf | remaining |  |  |  |
| `winppi.h` | Printing | blocked | copilot | 09/02/2026 17:51:18 | Deferred: genuine spool-file/page HANDLE ownership relationship via return value, no annotation precedent; same class as getprocesshandlefromhwnd.h/wab.h/wincon.h. |
| `winreg.h` | Registry | matched | copilot | 09/03/2026 02:30:00 | Classified retained artifact in existing-patches-37. |
| `winsafer.h` | Security.AppLocker | matched | copilot | 09/02/2026 21:05:00 | Corrected typedef-owned ownership violation found during resource-ownership audit. |
| `winsatcominterfacei.h` | WinSat | remaining |  |  |  |
| `winscard.h` | Credentials | remaining |  |  |  |
| `winsdkver.h` | Foundation | matched |  | 09/02/2026 17:09:31 | Version constants only, no functions. |
| `winsmcrd.h` | Credentials | remaining |  |  |  |
| `winsnmp.h` | Snmp | remaining |  |  |  |
| `WinSock.h` | Fsrm, TermServ, WinSock | matched | copilot | 09/03/2026 02:30:00 | Classified retained artifact in existing-patches-37. |
| `winsock2.h` | ActiveDirectory, HttpServer, IpHlp, P2p, Qos, Ual, WindowsFilteringPlatform, WinHttp, WinSock, wnv | matched | copilot | 09/03/2026 02:50:00 | Classified retained artifact in existing-patches-38. |
| `winsplp.h` | Printing | remaining |  |  |  |
| `winspool.h` | Gdi, Printing | matched | copilot | 09/02/2026 22:05:00 | Audited under corrected shared-handle policy (165b5f09, 7335ddc4); already compliant, no code changes required. Not part of the original 11-header audit set (discovered as an additional resource-ownership patch). |
| `winstring.h` | WinRT | remaining |  |  |  |
| `winsvc.h` | Services | matched | copilot | 09/02/2026 21:45:00 | Corrected typedef-owned ownership violation found during resource-ownership audit (10 producer sites across 2 handle types). |
| `winsxs.h` | Setup | remaining |  |  |  |
| `winsync.h` | WinSync | remaining |  |  |  |
| `winternl.h` | ActiveDirectory, Base, Certificates, FileHistory, IpHlp, MadCap, Security, Security.AppLocker, Security.ConfigurationSnapin, Security.Cryptography, Security.Cryptography.Catalog, Security.Cryptography.Sip, Security.Cryptography.UI, Security.DiagnosticDataQuery, Security.DirectoryServices, Security.LicenseProtection, Security.Tpm, Security.WinTrust, Security.WinWlx, WinProg | remaining |  |  |  |
| `wintrust.h` | Security.WinTrust | remaining |  |  |  |
| `winusb.h` | Buses | matched | copilot | 09/03/2026 02:50:00 | Classified retained artifact in existing-patches-38. |
| `winusbio.h` | Buses | remaining |  |  |  |
| `winuser.h` | Base, Controls, DataXchg, DlgBox, Dwm, FileHistory, Gdi, Input.Ime, Intl, IO, MenuRc, Security, Security.AppLocker, Security.ConfigurationSnapin, Security.DiagnosticDataQuery, Security.DirectoryServices, Security.LicenseProtection, Security.Tpm, Security.WinTrust, Security.WinWlx, Shell, TermServ, WinAuto, WinProg, WinTouch | matched | copilot | 09/03/2026 02:50:00 | Classified retained artifact in existing-patches-38. |
| `winver.h` | Fs | matched | copilot | 09/03/2026 02:50:00 | Classified retained artifact in existing-patches-38. |
| `winwlx.h` | Security.WinWlx | remaining |  |  |  |
| `wlanapi.h` | Ndis, NWifi | matched | copilot | 09/03/2026 02:50:00 | Classified retained artifact in existing-patches-38. |
| `wlanihv.h` | NWifi | remaining |  |  |  |
| `wlanihvtypes.h` | NWifi | remaining |  |  |  |
| `wlantypes.h` | Ndis, NWifi | remaining |  |  |  |
| `wlclient.h` | NWifi | matched |  | 09/02/2026 17:54:05 | Data structs only, no functions. |
| `wldp.h` | FileHistory, WinProg | matched | copilot | 09/03/2026 03:05:00 | Classified retained artifact in existing-patches-39. |
| `wmcodecdsp.h` | Media.DShow, Mf | remaining |  |  |  |
| `wmcontainer.h` | Mf | matched | copilot | 09/03/2026 03:05:00 | Classified retained artifact in existing-patches-39. |
| `wmdmlog.h` | Wmdm | remaining |  |  |  |
| `wmdxva.h` | WmFormat | remaining |  |  |  |
| `wmistr.h` | Etw | remaining |  |  |  |
| `wmiutils.h` | Lwef, Wmi | remaining |  |  |  |
| `wmlss.h` | Wmlss | remaining |  |  |  |
| `wmnetsourcecreator.h` | WmFormat | remaining |  |  |  |
| `wmp.h` | Media.DShow, Wmp | remaining |  |  |  |
| `wmpdevices.h` | Wmp | remaining |  |  |  |
| `wmpids.h` | Wmp | remaining |  |  |  |
| `wmpplug.h` | Wmp | remaining |  |  |  |
| `wmprealestate.h` | Wmp | remaining |  |  |  |
| `wmpservices.h` | Wmp | remaining |  |  |  |
| `wmsbuffer.h` | WmFormat | remaining |  |  |  |
| `wmsdkidl.h` | WmFormat | remaining |  |  |  |
| `wmsdkvalidate.h` | WmFormat | remaining |  |  |  |
| `wmsecure.h` | WmFormat | remaining |  |  |  |
| `wmsinternaladminnetsource.h` | WmFormat | remaining |  |  |  |
| `wmsysprf.h` | Wmp | remaining |  |  |  |
| `wnnc.h` | Ifsk | remaining |  |  |  |
| `wnvapi.h` | wnv | remaining |  |  |  |
| `wofapi.h` | Fs | remaining |  |  |  |
| `workspaceax.h` | TermServ | remaining |  |  |  |
| `workspaceruntime.h` | TermServ | remaining |  |  |  |
| `workspaceruntimeclientext.h` | TermServ | remaining |  |  |  |
| `wpc.h` | Parcon | matched |  | 09/02/2026 16:34:06 | All functions are MIDL RPC marshalling stubs (BSTR_User*/HWND_User*); no resource-owning functions. |
| `wpdmtpextensions.h` | WpdSdk | remaining |  |  |  |
| `wpdshellextension.h` | WpdSdk | remaining |  |  |  |
| `wrdsgraphicschannels.h` | TermServ | remaining |  |  |  |
| `ws2atm.h` | WinSock | remaining |  |  |  |
| `ws2bth.h` | Bluetooth | remaining |  |  |  |
| `ws2def.h` | IpHlp, WinSock | remaining |  |  |  |
| `ws2ipdef.h` | IpHlp, WinSock | remaining |  |  |  |
| `ws2spi.h` | WinSock | matched | copilot | 09/03/2026 03:05:00 | Classified retained artifact in existing-patches-39. |
| `ws2tcpip.h` | WinSock | matched | copilot | 09/03/2026 03:05:00 | Classified retained artifact in existing-patches-39. |
| `wsbapp.h` | Wsb | remaining |  |  |  |
| `wsbapperror.h` | Wsb | remaining |  |  |  |
| `wsbonline.h` | Wsb | remaining |  |  |  |
| `wsbonlineerror.h` | Wsb | remaining |  |  |  |
| `wscapi.h` | FileHistory, SecurityCenter | remaining |  |  |  |
| `wsdapi.h` | WebServicesOnDevices | matched |  | 09/02/2026 17:13:02 | Redirect-only; 32 functions all COM-factory/memory-helper pairs (void*, not HANDLE), no DECLARE_HANDLE. |
| `wsdattachment.h` | FunctionDiscovery | remaining |  |  |  |
| `wsdbase.h` | FunctionDiscovery | remaining |  |  |  |
| `wsdclient.h` | FunctionDiscovery | remaining |  |  |  |
| `wsddisco.h` | FunctionDiscovery | remaining |  |  |  |
| `wsdevlicensing.h` | DevLic | matched |  | 09/02/2026 17:16:10 | HWND is input parameter; outputs are plain FILETIME values. |
| `wsdhost.h` | FunctionDiscovery | remaining |  |  |  |
| `wsdns.h` | WebServicesOnDevices | remaining |  |  |  |
| `wsdtypes.h` | FunctionDiscovery | remaining |  |  |  |
| `wsdutil.h` | FunctionDiscovery | remaining |  |  |  |
| `wsdxml.h` | FunctionDiscovery | remaining |  |  |  |
| `wshisotp.h` | WinSock | remaining |  |  |  |
| `wsipv6ok.h` | WinSock | remaining |  |  |  |
| `wsipx.h` | WinSock | matched |  | 09/02/2026 18:09:40 | Data struct + constants only, no functions. |
| `wslapi.h` | Wsl | remaining |  |  |  |
| `wsman.h` | WinRm | remaining |  |  |  |
| `wsmandisp.h` | WinRm | remaining |  |  |  |
| `wsmerror.h` | WinRm | remaining |  |  |  |
| `wsnetbs.h` | WinSock | remaining |  |  |  |
| `wsnwlink.h` | WinSock | remaining |  |  |  |
| `wsrm.h` | WinSock | remaining |  |  |  |
| `wsvns.h` | WinSock | matched |  | 09/02/2026 17:41:54 | Data struct + constants only, no functions. |
| `wtsapi32.h` | TermServ | remaining |  |  |  |
| `wtsdefs.h` | TermServ | remaining |  |  |  |
| `wtshintapi.h` | TermServ | remaining |  |  |  |
| `wtsprotocol.h` | TermServ | remaining |  |  |  |
| `wtypes.h` | Com, Com.CallObj, Com.ChannelCreds, Com.StructuredStorage, Com.Urlmon, ComOle, FileHistory, Properties, Registry, Shutdown, TransactionServer, WinProg | matched | copilot | 09/02/2026 19:18:45 | Closed in shared-handle-policy-01; current windows-rs full test target is blocked by missing PartitionSpec.include_main_file initializers. |
| `wtypesbase.h` | TransactionServer, WinSock | remaining |  |  |  |
| `wuapi.h` | Wua | remaining |  |  |  |
| `wuerror.h` | Wua | remaining |  |  |  |
| `xa.h` | DTC | remaining |  |  |  |
| `xamlom.h` | Xaml_Diagnostics | remaining |  |  |  |
| `xapo.h` | Xaudio2 | remaining |  |  |  |
| `xapofx.h` | Xaudio2 | remaining |  |  |  |
| `xaudio2.h` | Xaudio2 | remaining |  |  |  |
| `xaudio2fx.h` | Xaudio2 | remaining |  |  |  |
| `xblidpauthmanager.h` | Xblidp | remaining |  |  |  |
| `xenroll.h` | Certificates | remaining |  |  |  |
| `xinput.h` | Xinput | remaining |  |  |  |
| `xmllite.h` | FileHistory, XmlLite | remaining |  |  |  |
| `xolehlp.h` | DTC | remaining |  |  |  |
| `xprtdefs.h` | Media.DShow | remaining |  |  |  |
| `xpsdigitalsignature.h` | Xps | remaining |  |  |  |
| `xpsobjectmodel_1.h` | Xps | remaining |  |  |  |
| `xpsobjectmodel_2.h` | Xps | remaining |  |  |  |
| `xpsobjectmodel.h` | Xps | remaining |  |  |  |
| `xpsprint.h` | Gdi, XpsPrinting | remaining |  |  |  |
| `xpsrassvc.h` | Printing | remaining |  |  |  |
