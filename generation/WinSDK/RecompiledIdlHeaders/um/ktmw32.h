/*++

Copyright (c) 2000  Microsoft Corporation

Module Name:

    ktmw32.h

Abstract:

    Public API include file for the TM subcomponent of the NTOS project

Author:

    Jon Cargille (jcargill) 5-Jun-2001

Revision History:

--*/

#ifndef _KTMUSER_
#define _KTMUSER_

#include <winapifamily.h>
#if defined(WIN32METADATA)
#include <win32metadata_annotations.h>
#endif

#pragma region Desktop Family or BootableSku Package
#if WINAPI_FAMILY_PARTITION(WINAPI_PARTITION_DESKTOP | WINAPI_PARTITION_PKG_BOOTABLESKU)


#ifdef __cplusplus
extern "C" {
#endif


//
// Win32-level transaction manager API calls
//

_Win32_metadata_set_last_error_
HANDLE
APIENTRY
CreateTransaction (
    IN LPSECURITY_ATTRIBUTES lpTransactionAttributes OPTIONAL,
    IN LPGUID UOW OPTIONAL,
    IN DWORD CreateOptions OPTIONAL,
    IN DWORD IsolationLevel OPTIONAL,
    IN DWORD IsolationFlags OPTIONAL,
    IN DWORD Timeout OPTIONAL,
    _In_opt_ LPWSTR Description
    );

_Win32_metadata_set_last_error_
HANDLE
APIENTRY
OpenTransaction (
    IN DWORD dwDesiredAccess,
    IN LPGUID TransactionId
    );

_Win32_metadata_set_last_error_
BOOL
APIENTRY
CommitTransaction (
    IN HANDLE TransactionHandle
    );

_Win32_metadata_set_last_error_
BOOL
APIENTRY
CommitTransactionAsync (
    IN HANDLE TransactionHandle
    );

_Win32_metadata_set_last_error_
BOOL
APIENTRY
RollbackTransaction (
    IN HANDLE TransactionHandle
    );

_Win32_metadata_set_last_error_
BOOL
APIENTRY
RollbackTransactionAsync (
    IN HANDLE TransactionHandle
    );

_Win32_metadata_set_last_error_
BOOL
APIENTRY
GetTransactionId (
    IN HANDLE TransactionHandle,
    OUT LPGUID TransactionId
    );


_Win32_metadata_set_last_error_
BOOL
APIENTRY
GetTransactionInformation (
    IN HANDLE TransactionHandle,
    OUT PDWORD Outcome OPTIONAL,
    OUT PDWORD IsolationLevel OPTIONAL,
    OUT PDWORD IsolationFlags OPTIONAL,
    OUT PDWORD Timeout OPTIONAL,
    _In_ DWORD BufferLength,
    _Out_writes_to_opt_(BufferLength, return + 1) LPWSTR Description
    );

_Win32_metadata_set_last_error_
BOOL
APIENTRY
SetTransactionInformation (
    IN HANDLE TransactionHandle,
    IN DWORD IsolationLevel OPTIONAL,
    IN DWORD IsolationFlags OPTIONAL,
    IN DWORD Timeout OPTIONAL,
    _In_opt_ LPWSTR Description
    );

_Win32_metadata_set_last_error_
HANDLE
APIENTRY
CreateTransactionManager (
    IN LPSECURITY_ATTRIBUTES lpTransactionAttributes OPTIONAL,
    _In_ LPWSTR LogFileName,
    IN ULONG CreateOptions OPTIONAL,
    IN ULONG CommitStrength OPTIONAL
    );

_Win32_metadata_set_last_error_
HANDLE
APIENTRY
OpenTransactionManager (
    _In_ LPWSTR LogFileName,
    IN ACCESS_MASK DesiredAccess,
    IN ULONG OpenOptions OPTIONAL
    );

_Win32_metadata_set_last_error_
HANDLE
APIENTRY
OpenTransactionManagerById (
    _In_ LPGUID TransactionManagerId,
    IN ACCESS_MASK DesiredAccess,
    IN ULONG OpenOptions OPTIONAL
    );

_Win32_metadata_set_last_error_
BOOL
APIENTRY
RenameTransactionManager (
    _In_ LPWSTR LogFileName,
    IN LPGUID ExistingTransactionManagerGuid
    );

_Win32_metadata_set_last_error_
BOOL
APIENTRY
RollforwardTransactionManager (
    IN HANDLE TransactionManagerHandle,
    IN PLARGE_INTEGER TmVirtualClock
    );

_Win32_metadata_set_last_error_
BOOL
APIENTRY
RecoverTransactionManager (
    IN HANDLE TransactionManagerHandle
    );

_Win32_metadata_set_last_error_
BOOL
GetCurrentClockTransactionManager(
    IN HANDLE TransactionManagerHandle,
    OUT PLARGE_INTEGER TmVirtualClock
    );

_Win32_metadata_set_last_error_
BOOL
GetTransactionManagerId(
    IN HANDLE TransactionManagerHandle,
    OUT LPGUID TransactionManagerId
    );

_Win32_metadata_set_last_error_
HANDLE
APIENTRY
CreateResourceManager (
    IN LPSECURITY_ATTRIBUTES lpResourceManagerAttributes OPTIONAL,
    IN LPGUID ResourceManagerId,
    IN DWORD CreateOptions OPTIONAL,
    IN HANDLE TmHandle,
    _In_opt_ LPWSTR Description
    );

_Win32_metadata_set_last_error_
HANDLE
APIENTRY
OpenResourceManager (
    IN DWORD dwDesiredAccess,
    IN HANDLE TmHandle,
    IN LPGUID ResourceManagerId
    );

_Win32_metadata_set_last_error_
BOOL
APIENTRY
RecoverResourceManager (
    IN HANDLE ResourceManagerHandle
    );

_Win32_metadata_set_last_error_
BOOL
APIENTRY
GetNotificationResourceManager (
    IN  HANDLE                          ResourceManagerHandle,
    OUT PTRANSACTION_NOTIFICATION       TransactionNotification,
    IN  ULONG                           NotificationLength,
    IN  DWORD                           dwMilliseconds OPTIONAL,
    OUT PULONG                          ReturnLength OPTIONAL
    );

_Win32_metadata_set_last_error_
BOOL
APIENTRY
GetNotificationResourceManagerAsync (
    IN  HANDLE                        ResourceManagerHandle,
    OUT PTRANSACTION_NOTIFICATION     TransactionNotification,
    IN  ULONG                         TransactionNotificationLength,
    OUT PULONG                        ReturnLength,
    IN  LPOVERLAPPED                  lpOverlapped
    );
    
_Win32_metadata_set_last_error_
BOOL
APIENTRY
SetResourceManagerCompletionPort(
    IN HANDLE    ResourceManagerHandle,
    IN HANDLE    IoCompletionPortHandle,
    IN ULONG_PTR CompletionKey
    );

_Win32_metadata_set_last_error_
HANDLE
APIENTRY
CreateEnlistment (
    IN LPSECURITY_ATTRIBUTES lpEnlistmentAttributes OPTIONAL,
    IN HANDLE            ResourceManagerHandle,
    IN HANDLE            TransactionHandle,
    IN NOTIFICATION_MASK NotificationMask,
    IN DWORD             CreateOptions OPTIONAL,
    IN PVOID             EnlistmentKey OPTIONAL
    );

_Win32_metadata_set_last_error_
HANDLE
APIENTRY
OpenEnlistment (
    IN DWORD              dwDesiredAccess,
    IN HANDLE             ResourceManagerHandle,
    IN LPGUID             EnlistmentId
    );


_Win32_metadata_set_last_error_
BOOL
APIENTRY
RecoverEnlistment (
    IN HANDLE EnlistmentHandle,
    IN PVOID EnlistmentKey OPTIONAL
    );


_Win32_metadata_set_last_error_
BOOL
APIENTRY
GetEnlistmentRecoveryInformation (
    IN  HANDLE EnlistmentHandle,
    IN  ULONG  BufferSize,
    OUT PVOID  Buffer,
    OUT PULONG BufferUsed OPTIONAL
    );

_Win32_metadata_set_last_error_
BOOL
APIENTRY
GetEnlistmentId (
    IN HANDLE EnlistmentHandle,
    OUT LPGUID EnlistmentId
    );

_Win32_metadata_set_last_error_
BOOL
APIENTRY
SetEnlistmentRecoveryInformation (
    IN HANDLE EnlistmentHandle,
    IN ULONG  BufferSize,
    IN PVOID  Buffer
    );

_Win32_metadata_set_last_error_
BOOL
APIENTRY
PrepareEnlistment (
    IN HANDLE EnlistmentHandle,
    IN PLARGE_INTEGER TmVirtualClock
    );

_Win32_metadata_set_last_error_
BOOL
APIENTRY
PrePrepareEnlistment (
    IN HANDLE EnlistmentHandle,
    IN PLARGE_INTEGER TmVirtualClock
    );

_Win32_metadata_set_last_error_
BOOL
APIENTRY
CommitEnlistment (
    IN HANDLE EnlistmentHandle,
    IN PLARGE_INTEGER TmVirtualClock
    );

_Win32_metadata_set_last_error_
BOOL
APIENTRY
RollbackEnlistment (
    IN HANDLE EnlistmentHandle,
    IN PLARGE_INTEGER TmVirtualClock
    );

_Win32_metadata_set_last_error_
BOOL
APIENTRY
PrePrepareComplete (
    IN  HANDLE            EnlistmentHandle,
    IN  PLARGE_INTEGER    TmVirtualClock
    );

_Win32_metadata_set_last_error_
BOOL
APIENTRY
PrepareComplete (
    IN  HANDLE            EnlistmentHandle,
    IN  PLARGE_INTEGER    TmVirtualClock
    );

_Win32_metadata_set_last_error_
BOOL
APIENTRY
ReadOnlyEnlistment (
    IN  HANDLE            EnlistmentHandle,
    IN  PLARGE_INTEGER    TmVirtualClock
    );

_Win32_metadata_set_last_error_
BOOL
APIENTRY
CommitComplete (
    IN  HANDLE            EnlistmentHandle,
    IN  PLARGE_INTEGER    TmVirtualClock
    );

_Win32_metadata_set_last_error_
BOOL
APIENTRY
RollbackComplete (
    IN  HANDLE            EnlistmentHandle,
    IN  PLARGE_INTEGER    TmVirtualClock
    );

_Win32_metadata_set_last_error_
BOOL
APIENTRY
SinglePhaseReject (
    IN  HANDLE            EnlistmentHandle,
    IN  PLARGE_INTEGER    TmVirtualClock
    );

#ifdef __cplusplus
}
#endif



#endif /* WINAPI_FAMILY_PARTITION(WINAPI_PARTITION_DESKTOP | WINAPI_PARTITION_PKG_BOOTABLESKU) */
#pragma endregion

#endif // _KTMUSER_

