//+-------------------------------------------------------------------------
//
//  Microsoft Windows
//  Copyright (c) Microsoft Corporation. All rights reserved.
//
//  Component: WSDAPI - Microsoft Web Services for Devices API
// 
//  File: wsdutil.h
//
//  Abstract: WSDAPI Utilities
//
//--------------------------------------------------------------------------
#pragma once
#include <winapifamily.h>
#include <win32metadata_annotations.h>

#pragma region Desktop Family
#if WINAPI_FAMILY_PARTITION(WINAPI_PARTITION_DESKTOP)


//
// Helpful macros
//
#define WSD_DEFAULT_HOSTING_ADDRESS L"http://*:5357/"
#define WSD_DEFAULT_SECURE_HOSTING_ADDRESS L"https://*:5358/"
#define WSD_DEFAULT_EVENTING_ADDRESS L"http://*:5357/"

#ifdef __cplusplus
extern "C" {
#endif

//
// Configuration
//

// MessageSize: DWORD value between 32768 and 1048576
#define WSDAPI_OPTION_MAX_INBOUND_MESSAGE_SIZE 0x0001

#if (WINVER >= _WIN32_WINNT_WIN7)
// XMLDebug: Send XML strings to debugger session
#define WSDAPI_OPTION_TRACE_XML_TO_DEBUGGER 0x0002

// XMLFile: Send XML strings to file
#define WSDAPI_OPTION_TRACE_XML_TO_FILE 0x0003
#endif
    
_Win32_metadata_supported_os_(windows6.0.6000)
HRESULT WINAPI
WSDSetConfigurationOption(
    DWORD dwOption,
    _In_reads_bytes_(cbInBuffer) LPVOID pVoid,
    DWORD cbInBuffer);

_Win32_metadata_supported_os_(windows6.0.6000)
HRESULT WINAPI
WSDGetConfigurationOption(
    DWORD dwOption,
    _Out_writes_bytes_(cbOutBuffer) LPVOID pVoid,
    DWORD cbOutBuffer);
    
//
// Linked memory
//

_Win32_metadata_supported_os_(windows6.0.6000)
void * WINAPI 
WSDAllocateLinkedMemory(
    void* pParent, 
    size_t cbSize);

_Win32_metadata_supported_os_(windows6.0.6000)
void WINAPI
WSDFreeLinkedMemory(
    void *pVoid);

_Win32_metadata_supported_os_(windows6.0.6000)
void WINAPI
WSDAttachLinkedMemory(
    void* pParent,
    void* pChild);

_Win32_metadata_supported_os_(windows6.0.6000)
void WINAPI
WSDDetachLinkedMemory(
    void* pVoid);

//
// XML helpers 
//

typedef struct _WSDXML_ELEMENT WSDXML_ELEMENT; 

_Win32_metadata_supported_os_(windows6.0.6000)
HRESULT WINAPI
WSDXMLBuildAnyForSingleElement(
    WSDXML_NAME* pElementName, 
    _In_opt_ LPCWSTR pszText, 
    _Outptr_ WSDXML_ELEMENT** ppAny);

_Win32_metadata_supported_os_(windows6.0.6000)
HRESULT WINAPI
WSDXMLGetValueFromAny(
    _In_ LPCWSTR pszNamespace,
    _In_ LPCWSTR pszName, 
    WSDXML_ELEMENT* pAny, 
    _Outptr_ LPCWSTR* ppszValue);

_Win32_metadata_supported_os_(windows6.0.6000)
HRESULT WINAPI
WSDXMLAddSibling(
    WSDXML_ELEMENT* pFirst,
    WSDXML_ELEMENT* pSecond);

_Win32_metadata_supported_os_(windows6.0.6000)
HRESULT WINAPI
WSDXMLAddChild(
    WSDXML_ELEMENT* pParent,
    WSDXML_ELEMENT* pChild);

_Win32_metadata_supported_os_(windows6.0.6000)
HRESULT WINAPI
WSDXMLCleanupElement(
    WSDXML_ELEMENT* pAny);    

_Win32_metadata_supported_os_(windows6.0.6000)
HRESULT WINAPI
WSDGenerateFault(
    _In_ LPCWSTR pszCode,
    _In_opt_ LPCWSTR pszSubCode,
    _In_ LPCWSTR pszReason,
    _In_opt_ LPCWSTR pszDetail,
    _In_ IWSDXMLContext* pContext,
    _Outptr_ WSD_SOAP_FAULT** ppFault);

_Win32_metadata_supported_os_(windows6.0.6000)
HRESULT WINAPI
WSDGenerateFaultEx(
    _In_ WSDXML_NAME* pCode, 
    _In_opt_ WSDXML_NAME* pSubCode, 
    _In_ WSD_LOCALIZED_STRING_LIST* pReasons, 
    _In_opt_ LPCWSTR pszDetail, 
    _Outptr_ WSD_SOAP_FAULT** ppFault);

#if (WINVER >= _WIN32_WINNT_WIN7)
_Win32_metadata_supported_os_(windows6.1)
HRESULT WINAPI
WSDUriEncode(
    _In_reads_(cchSource) LPCWSTR source,
    _In_ DWORD cchSource,
    _Out_writes_(*cchDestOut) LPWSTR *destOut,
    _Out_opt_ DWORD *cchDestOut);

_Win32_metadata_supported_os_(windows6.1)
HRESULT WINAPI
WSDUriDecode(
    _In_reads_(cchSource) LPCWSTR source,
    _In_ DWORD cchSource,
    _Out_writes_(*cchDestOut) LPWSTR *destOut,
    _Out_opt_ DWORD *cchDestOut);
#endif

#ifdef __cplusplus
};
#endif

#endif /* WINAPI_FAMILY_PARTITION(WINAPI_PARTITION_DESKTOP) */
#pragma endregion

