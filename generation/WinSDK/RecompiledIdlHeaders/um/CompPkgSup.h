//*@@@+++@@@@******************************************************************
//
// ComponentPackageSupport
// Copyright (C) Microsoft Corporation. All rights reserved.
//
//*@@@---@@@@******************************************************************
//

//
// CompPkgSup.h is the header containing the realtime work queue APIs
// exported by CompPkgSup.dll
//

#if (_MSC_VER > 1020)
#pragma once
#endif

#if !defined(__COMPPKGSUP_H__)
#define __COMPPKGSUP_H__
#include <win32metadata_annotations.h>

#pragma region Desktop Family
#if WINAPI_FAMILY_PARTITION(WINAPI_PARTITION_DESKTOP)

#include <hstring.h>
#include <inspectable.h>
#include <windows.foundation.h>
#include <mfcontentdecryptionmodule.h>

/////////////////////////////////////////////////////////////////////////////////////////

#if !defined(MIDL_NS_PREFIX) && !defined(____x_ABI_CWindows_CFoundation_CIClosable_FWD_DEFINED__)
#pragma push_macro("ABI")
#undef ABI
#define ABI
#endif

_Win32_metadata_supported_os_(windows8.1)
STDAPI InstantiateComponentFromPackage(_In_ HSTRING classId, _In_ HSTRING packageFullName, _COM_Outptr_ IInspectable** instance);
_Win32_metadata_supported_os_(windows10.0.10240)
STDAPI RegisterServerForPMP(_In_ HSTRING serverClassId, _In_ IClassFactory *classFactory, _Out_ LPVOID *token);
_Win32_metadata_supported_os_(windows10.0.10240)
STDAPI UnregisterServerForPMP(_In_ LPVOID token);
_Win32_metadata_supported_os_(windows10.0.10240)
STDAPI GetServerForPMP(_In_ HSTRING serverClassId, _COM_Outptr_ IUnknown **unknown);

_Win32_metadata_supported_os_(windows10.0.10240)
STDAPI AreDvdCodecsEnabled();
STDAPI RequireNetworkDuringMediaTaskCompletion(_In_ BOOL requireNetwork, _Out_opt_ DWORD* requireCount);
STDAPI_(DWORD) GetNetworkRequestCount();
_Win32_metadata_supported_os_(windows10.0.10240)
STDAPI GetMediaExtensionCommunicationFactory(_Outptr_ LPVOID* factory);
_Win32_metadata_supported_os_(windows10.0.10240)
STDAPI_(BOOL) IsMediaBehaviorEnabled(_In_ GUID mediaBehavior);
_Win32_metadata_supported_os_(windows10.0.10240)
STDAPI  GetMediaComponentPackageInfo( _In_ bool trustedOnly, _In_ HSTRING category,
    _COM_Outptr_ ABI::Windows::Foundation::Collections::IVector<ABI::Windows::Foundation::Collections::IPropertySet*> **codecPropertiesVector);
#if (NTDDI_VERSION >= NTDDI_WIN11_GA)
STDAPI GetSystemNativeProcessorSignature(_Out_ DWORD* processorSignature);
#endif // NTDDI_VERSION >= NTDDI_WIN11_GA

#if (NTDDI_VERSION >= NTDDI_WIN11_GE)
STDAPI GetDefaultContentDecryptionModuleFactory(_In_ PCWSTR keySystem, _Outptr_result_maybenull_ IMFContentDecryptionModuleFactory** contentDecryptionModuleFactory);
STDAPI RegisterMediaExtensionPackage(_In_ PCWSTR packageFamilyName);
#endif // NTDDI_VERSION >= NTDDI_WIN11_GE

//
// MF behavior GUIDs
//

// MEDIA_BEHAVIOR_MEDIAPLAYBACKLIST_AUTOPLAYBACKITEMRESET {45B17027-E16C-4F23-A62B-B37146B2C964}
EXTERN_GUID(MEDIA_BEHAVIOR_MEDIAPLAYBACKLIST_AUTOPLAYBACKITEMRESET,
    0x45b17027, 0xe16c, 0x4f23, 0xa6, 0x2b, 0xb3, 0x71, 0x46, 0xb2, 0xc9, 0x64);

#if !defined(MIDL_NS_PREFIX) && !defined(____x_ABI_CWindows_CFoundation_CIClosable_FWD_DEFINED__)
#pragma pop_macro("ABI")
#endif

#endif /* WINAPI_FAMILY_PARTITION(WINAPI_PARTITION_DESKTOP) */
#pragma endregion

#endif //#if !defined(__COMPPKGSUP_H__)
