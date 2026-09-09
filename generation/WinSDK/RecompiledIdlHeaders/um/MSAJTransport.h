// AllJoyn APIs to connect, send/receive data, 
// set/enum events for Named Pipe Transport.     
//
// Copyright (c) Microsoft Corporation. All rights reserved.                     


#ifndef _MSAJ_TRANSPORT_API_H
#define _MSAJ_TRANSPORT_API_H
#include <win32metadata_annotations.h>

#ifdef _MSC_VER 
#pragma once 
#endif  // _MSC_VER 

#if (WINVER >= _WIN32_WINNT_WINTHRESHOLD)

#ifdef __cplusplus
extern "C" {
#endif

#pragma region Application Family
#if WINAPI_FAMILY_PARTITION(WINAPI_PARTITION_APP)

#define ALLJOYN_NAMED_PIPE_CONNECT_SPEC     L"npipe:"

_Win32_metadata_supported_os_(windows10.0.10240)
_Win32_metadata_set_last_error_
HANDLE
WINAPI
AllJoynConnectToBus(
    _In_opt_ PCWSTR connectionSpec);

_Win32_metadata_supported_os_(windows10.0.10240)
_Win32_metadata_set_last_error_
BOOL
WINAPI
AllJoynCloseBusHandle(
    _In_ HANDLE busHandle);

_Win32_metadata_supported_os_(windows10.0.10240)
_Win32_metadata_set_last_error_
BOOL
WINAPI 
AllJoynSendToBus(
    _In_ HANDLE connectedBusHandle,
    _In_reads_bytes_opt_(bytesToWrite) const VOID *buffer,
    _In_ DWORD bytesToWrite,
    _Out_opt_ PDWORD bytesTransferred,
    _Inout_ PVOID reserved);

_Win32_metadata_supported_os_(windows10.0.10240)
_Win32_metadata_set_last_error_
BOOL
WINAPI 
AllJoynReceiveFromBus(
    _In_ HANDLE connectedBusHandle,
    _Out_writes_bytes_to_opt_(bytesToRead, *bytesTransferred) PVOID buffer,
    _In_ DWORD bytesToRead,
    _Out_opt_ PDWORD bytesTransferred,
    _Inout_ PVOID reserved);

// AllJoyn bus event types.
#define ALLJOYN_READ_READY      0x1
#define ALLJOYN_WRITE_READY     0x2
#define ALLJOYN_DISCONNECTED    0x4

_Win32_metadata_supported_os_(windows10.0.10240)
_Win32_metadata_set_last_error_
BOOL
WINAPI 
AllJoynEventSelect(
    _In_ HANDLE connectedBusHandle,
    _In_ HANDLE eventHandle,
    _In_ DWORD eventTypes);

_Win32_metadata_supported_os_(windows10.0.10240)
_Win32_metadata_set_last_error_
BOOL
WINAPI 
AllJoynEnumEvents(
    _In_ HANDLE connectedBusHandle,
    _In_opt_ HANDLE eventToReset,
    _Out_ PDWORD eventTypes);

#endif // #if WINAPI_FAMILY_PARTITION(WINAPI_PARTITION_APP)
#pragma endregion


#pragma region Desktop Family
#if WINAPI_FAMILY_PARTITION(WINAPI_PARTITION_DESKTOP)

HANDLE
WINAPI
AllJoynCreateBus(
    _In_ DWORD outBufferSize,
    _In_ DWORD inBufferSize,
    _In_opt_ LPSECURITY_ATTRIBUTES lpSecurityAttributes);

DWORD
WINAPI
AllJoynAcceptBusConnection(
    _In_ HANDLE serverBusHandle,
    _In_ HANDLE abortEvent);

#endif // #if WINAPI_FAMILY_PARTITION(WINAPI_PARTITION_DESKTOP)
#pragma endregion


#ifdef __cplusplus
} // extern "C" 
#endif

#endif // #if (WINVER >= _WIN32_WINNT_WINTHRESHOLD)
#endif // _MSAJ_TRANSPORT_API_H
