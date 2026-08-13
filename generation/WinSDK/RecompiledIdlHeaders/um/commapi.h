/********************************************************************************
*                                                                               *
* commapi.h - ApiSet Contract for api-ms-win-core-comm-l1                       *
*                                                                               *
* Copyright (c) Microsoft Corporation. All rights reserved.                     *
*                                                                               *
********************************************************************************/

#ifdef _MSC_VER
#pragma once
#endif // _MSC_VER

#ifndef _COMM_H_
#define _COMM_H_

#include <apiset.h>
#include <apisetcconv.h>
#include <windows.h>
#include <winbase.h>
#include <winapifamily.h>
#if defined(WIN32METADATA)
#include <win32metadata_annotations.h>
#endif

#ifdef __cplusplus
extern "C" {
#endif

#pragma region Desktop Family or OneCore or App Family
#if WINAPI_FAMILY_PARTITION(WINAPI_PARTITION_DESKTOP | WINAPI_PARTITION_SYSTEM | WINAPI_PARTITION_APP)

_Win32_metadata_set_last_error_
WINBASEAPI
BOOL
WINAPI
ClearCommBreak(
    _In_ HANDLE hFile
    );

_Win32_metadata_set_last_error_
WINBASEAPI
BOOL
WINAPI
ClearCommError(
    _In_ HANDLE hFile,
    _Out_opt_ LPDWORD lpErrors,
    _Out_opt_ LPCOMSTAT lpStat
    );

_Win32_metadata_set_last_error_
WINBASEAPI
BOOL
WINAPI
SetupComm(
    _In_ HANDLE hFile,
    _In_ DWORD dwInQueue,
    _In_ DWORD dwOutQueue
    );

_Win32_metadata_set_last_error_
WINBASEAPI
BOOL
WINAPI
EscapeCommFunction(
    _In_ HANDLE hFile,
    _In_ DWORD dwFunc
    );

_Win32_metadata_set_last_error_
WINBASEAPI
_Success_(return != FALSE)
BOOL
WINAPI
GetCommConfig(
    _In_ HANDLE hCommDev,
    _Out_writes_bytes_opt_(*lpdwSize) LPCOMMCONFIG lpCC,
    _Inout_ LPDWORD lpdwSize
    );

_Win32_metadata_set_last_error_
WINBASEAPI
BOOL
WINAPI
GetCommMask(
    _In_ HANDLE hFile,
    _Out_ LPDWORD lpEvtMask
    );

_Win32_metadata_set_last_error_
WINBASEAPI
BOOL
WINAPI
GetCommModemStatus(
    _In_ HANDLE hFile,
    _Out_ LPDWORD lpModemStat
    );

_Win32_metadata_set_last_error_
WINBASEAPI
BOOL
WINAPI
GetCommProperties(
    _In_ HANDLE hFile,
    _Inout_ LPCOMMPROP lpCommProp
    );

_Win32_metadata_set_last_error_
WINBASEAPI
BOOL
WINAPI
GetCommState(
    _In_ HANDLE hFile,
    _Out_ LPDCB lpDCB
    );

_Win32_metadata_set_last_error_
WINBASEAPI
BOOL
WINAPI
GetCommTimeouts(
    _In_ HANDLE hFile,
    _Out_ LPCOMMTIMEOUTS lpCommTimeouts
    );

_Win32_metadata_set_last_error_
WINBASEAPI
BOOL
WINAPI
PurgeComm(
    _In_ HANDLE hFile,
    _In_ DWORD dwFlags
    );

_Win32_metadata_set_last_error_
WINBASEAPI
BOOL
WINAPI
SetCommBreak(
    _In_ HANDLE hFile
    );

_Win32_metadata_set_last_error_
WINBASEAPI
BOOL
WINAPI
SetCommConfig(
    _In_ HANDLE hCommDev,
    _In_reads_bytes_(dwSize) LPCOMMCONFIG lpCC,
    _In_ DWORD dwSize
    );

_Win32_metadata_set_last_error_
WINBASEAPI
BOOL
WINAPI
SetCommMask(
    _In_ HANDLE hFile,
    _In_ DWORD dwEvtMask
    );

_Win32_metadata_set_last_error_
WINBASEAPI
BOOL
WINAPI
SetCommState(
    _In_ HANDLE hFile,
    _In_ LPDCB lpDCB
    );

_Win32_metadata_set_last_error_
WINBASEAPI
BOOL
WINAPI
SetCommTimeouts(
    _In_ HANDLE hFile,
    _In_ LPCOMMTIMEOUTS lpCommTimeouts
    );

_Win32_metadata_set_last_error_
WINBASEAPI
BOOL
WINAPI
TransmitCommChar(
    _In_ HANDLE hFile,
    _In_ char cChar
    );

_Win32_metadata_set_last_error_
WINBASEAPI
BOOL
WINAPI
WaitCommEvent(
    _In_ HANDLE hFile,
    _Inout_ LPDWORD lpEvtMask,
    _Inout_opt_ LPOVERLAPPED lpOverlapped
    );

WINBASEAPI
HANDLE
WINAPI
OpenCommPort(
    _In_ ULONG uPortNumber,
    _In_ DWORD dwDesiredAccess,
    _In_ DWORD dwFlagsAndAttributes
    );

WINBASEAPI
ULONG
WINAPI
GetCommPorts(
    _Out_writes_(uPortNumbersCount) PULONG lpPortNumbers,
    _In_ ULONG uPortNumbersCount,
    _Out_ PULONG puPortNumbersFound
    );

#endif // WINAPI_FAMILY_PARTITION(WINAPI_PARTITION_DESKTOP | WINAPI_PARTITION_SYSTEM | WINAPI_PARTITION_APP)
#pragma endregion

#ifdef __cplusplus
}
#endif

#endif // _COMM_H_

