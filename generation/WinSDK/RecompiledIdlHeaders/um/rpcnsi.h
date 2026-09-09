#include <winapifamily.h>
#include <win32metadata_annotations.h>

/*++

Copyright (c) Microsoft Corporation. All rights reserved.

Module Name:

    rpcnsi.h

Abstract:

    This file contains the types and function definitions to use the
    Name Service Independent APIs.

--*/

#ifndef __RPCNSI_H__
#define __RPCNSI_H__

#if _MSC_VER > 1000
#pragma once
#endif

#pragma region Desktop Family
#if WINAPI_FAMILY_PARTITION(WINAPI_PARTITION_DESKTOP)

typedef void __RPC_FAR * RPC_NS_HANDLE;

#define RPC_C_NS_SYNTAX_DEFAULT 0
#define RPC_C_NS_SYNTAX_DCE 3

#define RPC_C_PROFILE_DEFAULT_ELT 0
#define RPC_C_PROFILE_ALL_ELT 1
#define RPC_C_PROFILE_ALL_ELTS RPC_C_PROFILE_ALL_ELT
#define RPC_C_PROFILE_MATCH_BY_IF 2
#define RPC_C_PROFILE_MATCH_BY_MBR 3
#define RPC_C_PROFILE_MATCH_BY_BOTH 4

#define RPC_C_NS_DEFAULT_EXP_AGE -1

/* Server APIs */

_Win32_metadata_supported_os_(windows5.0)
RPCNSAPI RPC_STATUS RPC_ENTRY
RpcNsBindingExportA(
    _In_ unsigned long EntryNameSyntax,
    _In_opt_ RPC_CSTR EntryName,
    _In_opt_ RPC_IF_HANDLE IfSpec,
    _In_opt_ RPC_BINDING_VECTOR __RPC_FAR *BindingVec,
    _In_opt_ UUID_VECTOR __RPC_FAR *ObjectUuidVec
    );


_Win32_metadata_supported_os_(windows5.0)
RPCNSAPI RPC_STATUS RPC_ENTRY
RpcNsBindingUnexportA(
    _In_ unsigned long EntryNameSyntax,
    _In_opt_ RPC_CSTR EntryName,
    _In_opt_ RPC_IF_HANDLE IfSpec,
    _In_opt_ UUID_VECTOR __RPC_FAR *ObjectUuidVec
    );

#ifdef RPC_UNICODE_SUPPORTED

_Win32_metadata_supported_os_(windows5.0)
RPCNSAPI RPC_STATUS RPC_ENTRY
RpcNsBindingExportW(
    _In_ unsigned long EntryNameSyntax,
    _In_opt_ RPC_WSTR EntryName,
    _In_opt_ RPC_IF_HANDLE IfSpec,
    _In_opt_ RPC_BINDING_VECTOR __RPC_FAR *BindingVec,
    _In_opt_ UUID_VECTOR __RPC_FAR *ObjectUuidVec
    );

_Win32_metadata_supported_os_(windows5.0)
RPCNSAPI RPC_STATUS RPC_ENTRY
RpcNsBindingUnexportW(
    _In_ unsigned long EntryNameSyntax,
    _In_opt_ RPC_WSTR EntryName,
    _In_opt_ RPC_IF_HANDLE IfSpec,
    _In_opt_ UUID_VECTOR __RPC_FAR *ObjectUuidVec
    );

#endif

/* Server PnP APIs */

_Win32_metadata_supported_os_(windows5.0)
RPC_STATUS RPC_ENTRY
RpcNsBindingExportPnPA(
    _In_ unsigned long EntryNameSyntax,
    _In_opt_ RPC_CSTR EntryName,
    _In_opt_ RPC_IF_HANDLE IfSpec,
    _In_opt_ UUID_VECTOR *ObjectVector
    );

_Win32_metadata_supported_os_(windows5.0)
RPC_STATUS RPC_ENTRY
RpcNsBindingUnexportPnPA(
    _In_ unsigned long EntryNameSyntax,
    _In_opt_ RPC_CSTR EntryName,
    _In_opt_ RPC_IF_HANDLE IfSpec,
    _In_opt_ UUID_VECTOR *ObjectVector
    );

#ifdef RPC_UNICODE_SUPPORTED

_Win32_metadata_supported_os_(windows5.0)
RPC_STATUS RPC_ENTRY
RpcNsBindingExportPnPW(
    _In_ unsigned long EntryNameSyntax,
    _In_opt_ RPC_WSTR EntryName,
    _In_opt_ RPC_IF_HANDLE IfSpec,
    _In_opt_ UUID_VECTOR *ObjectVector
    );

_Win32_metadata_supported_os_(windows5.0)
RPC_STATUS RPC_ENTRY
RpcNsBindingUnexportPnPW(
    _In_ unsigned long EntryNameSyntax,
    _In_opt_ RPC_WSTR EntryName,
    _In_opt_ RPC_IF_HANDLE IfSpec,
    _In_opt_ UUID_VECTOR *ObjectVector
    );

#endif

/* Client APIs */

_Win32_metadata_supported_os_(windows5.0)
RPCNSAPI RPC_STATUS RPC_ENTRY
RpcNsBindingLookupBeginA(
    _In_ unsigned long EntryNameSyntax,
    _In_opt_ RPC_CSTR EntryName,
    _In_opt_ RPC_IF_HANDLE IfSpec,
    _In_opt_ UUID __RPC_FAR *ObjUuid,
    _In_ unsigned long BindingMaxCount,
    _Out_ RPC_NS_HANDLE __RPC_FAR *LookupContext
    );

#ifdef RPC_UNICODE_SUPPORTED

_Win32_metadata_supported_os_(windows5.0)
RPCNSAPI RPC_STATUS RPC_ENTRY
RpcNsBindingLookupBeginW(
    _In_ unsigned long EntryNameSyntax,
    _In_opt_ RPC_WSTR EntryName,
    _In_opt_ RPC_IF_HANDLE IfSpec,
    _In_opt_ UUID __RPC_FAR *ObjUuid,
    _In_ unsigned long BindingMaxCount,
    _Out_ RPC_NS_HANDLE __RPC_FAR *LookupContext
    );
#endif

_Win32_metadata_supported_os_(windows5.0)
RPCNSAPI RPC_STATUS RPC_ENTRY
RpcNsBindingLookupNext(
    IN  RPC_NS_HANDLE LookupContext,
    OUT RPC_BINDING_VECTOR __RPC_FAR * __RPC_FAR * BindingVec
    );

_Win32_metadata_supported_os_(windows5.0)
RPCNSAPI RPC_STATUS RPC_ENTRY
RpcNsBindingLookupDone(
    IN OUT RPC_NS_HANDLE __RPC_FAR * LookupContext
    );

/* Group APIs */

_Win32_metadata_supported_os_(windows5.0)
RPCNSAPI RPC_STATUS RPC_ENTRY
RpcNsGroupDeleteA(
    _In_ unsigned long GroupNameSyntax,
    _In_opt_ RPC_CSTR GroupName
    );

_Win32_metadata_supported_os_(windows5.0)
RPCNSAPI RPC_STATUS RPC_ENTRY
RpcNsGroupMbrAddA(
    _In_ unsigned long GroupNameSyntax,
    _In_ RPC_CSTR GroupName,
    _In_ unsigned long MemberNameSyntax,
    _In_ RPC_CSTR MemberName
    );

_Win32_metadata_supported_os_(windows5.0)
RPCNSAPI RPC_STATUS RPC_ENTRY
RpcNsGroupMbrRemoveA(
    _In_ unsigned long GroupNameSyntax,
    _In_ RPC_CSTR GroupName,
    _In_ unsigned long MemberNameSyntax,
    _In_ RPC_CSTR MemberName
    );

_Win32_metadata_supported_os_(windows5.0)
RPCNSAPI RPC_STATUS RPC_ENTRY
RpcNsGroupMbrInqBeginA(
    _In_ unsigned long GroupNameSyntax,
    _In_ RPC_CSTR GroupName,
    _In_ unsigned long MemberNameSyntax,
    _Out_ RPC_NS_HANDLE __RPC_FAR *InquiryContext
    );

_Win32_metadata_supported_os_(windows5.0)
RPCNSAPI RPC_STATUS RPC_ENTRY
RpcNsGroupMbrInqNextA(
    _Inout_ RPC_NS_HANDLE InquiryContext,
    _Outptr_ RPC_CSTR __RPC_FAR *MemberName
    );

#ifdef RPC_UNICODE_SUPPORTED

_Win32_metadata_supported_os_(windows5.0)
RPCNSAPI RPC_STATUS RPC_ENTRY
RpcNsGroupDeleteW(
    _In_ unsigned long GroupNameSyntax,
    _In_opt_ RPC_WSTR GroupName
    );

_Win32_metadata_supported_os_(windows5.0)
RPCNSAPI RPC_STATUS RPC_ENTRY
RpcNsGroupMbrAddW(
    _In_ unsigned long GroupNameSyntax,
    _In_ RPC_WSTR GroupName,
    _In_ unsigned long MemberNameSyntax,
    _In_ RPC_WSTR MemberName
    );

_Win32_metadata_supported_os_(windows5.0)
RPCNSAPI RPC_STATUS RPC_ENTRY
RpcNsGroupMbrRemoveW(
    _In_ unsigned long GroupNameSyntax,
    _In_ RPC_WSTR GroupName,
    _In_ unsigned long MemberNameSyntax,
    _In_ RPC_WSTR MemberName
    );

_Win32_metadata_supported_os_(windows5.0)
RPCNSAPI RPC_STATUS RPC_ENTRY
RpcNsGroupMbrInqBeginW(
    _In_ unsigned long GroupNameSyntax,
    _In_ RPC_WSTR GroupName,
    _In_ unsigned long MemberNameSyntax,
    _Out_ RPC_NS_HANDLE __RPC_FAR *InquiryContext
    );

_Win32_metadata_supported_os_(windows5.0)
RPCNSAPI RPC_STATUS RPC_ENTRY
RpcNsGroupMbrInqNextW(
    _Inout_ RPC_NS_HANDLE InquiryContext,
    _Outptr_ RPC_WSTR __RPC_FAR *MemberName
    );

#endif

_Win32_metadata_supported_os_(windows5.0)
RPCNSAPI RPC_STATUS RPC_ENTRY
RpcNsGroupMbrInqDone(
    IN OUT RPC_NS_HANDLE __RPC_FAR * InquiryContext
    );

/* Profile APIs */

_Win32_metadata_supported_os_(windows5.0)
RPCNSAPI RPC_STATUS RPC_ENTRY
RpcNsProfileDeleteA(
    _In_ unsigned long ProfileNameSyntax,
    _In_ RPC_CSTR ProfileName
    );

_Win32_metadata_supported_os_(windows5.0)
RPCNSAPI RPC_STATUS RPC_ENTRY
RpcNsProfileEltAddA(
    _In_ unsigned long ProfileNameSyntax,
    _In_ RPC_CSTR ProfileName,
    _In_opt_ RPC_IF_ID __RPC_FAR *IfId,
    _In_ unsigned long MemberNameSyntax,
    _In_ RPC_CSTR MemberName,
    _In_ unsigned long Priority,
    _In_opt_ RPC_CSTR Annotation
    );

_Win32_metadata_supported_os_(windows5.0)
RPCNSAPI RPC_STATUS RPC_ENTRY
RpcNsProfileEltRemoveA(
    _In_ unsigned long ProfileNameSyntax,
    _In_ RPC_CSTR ProfileName,
    _In_opt_ RPC_IF_ID __RPC_FAR *IfId,
    _In_ unsigned long MemberNameSyntax,
    _In_ RPC_CSTR MemberName
    );

_Win32_metadata_supported_os_(windows5.0)
RPCNSAPI RPC_STATUS RPC_ENTRY
RpcNsProfileEltInqBeginA(
    _In_ unsigned long ProfileNameSyntax,
    _In_ RPC_CSTR ProfileName,
    _In_ unsigned long InquiryType,
    _In_opt_ RPC_IF_ID __RPC_FAR *IfId,
    _In_ unsigned long VersOption,
    _In_ unsigned long MemberNameSyntax,
    _In_opt_ RPC_CSTR MemberName,
    _Out_ RPC_NS_HANDLE __RPC_FAR *InquiryContext
    );

_Win32_metadata_supported_os_(windows5.0)
RPCNSAPI RPC_STATUS RPC_ENTRY
RpcNsProfileEltInqNextA(
    _In_ RPC_NS_HANDLE InquiryContext,
    _Out_opt_ RPC_IF_ID __RPC_FAR *IfId,
    _Out_ RPC_CSTR __RPC_FAR *MemberName,
    _Out_ unsigned long __RPC_FAR *Priority,
    _Out_ RPC_CSTR __RPC_FAR *Annotation
    );

#ifdef RPC_UNICODE_SUPPORTED

_Win32_metadata_supported_os_(windows5.0)
RPCNSAPI RPC_STATUS RPC_ENTRY
RpcNsProfileDeleteW(
    _In_ unsigned long ProfileNameSyntax,
    _In_ RPC_WSTR ProfileName
    );

_Win32_metadata_supported_os_(windows5.0)
RPCNSAPI RPC_STATUS RPC_ENTRY
RpcNsProfileEltAddW(
    _In_ unsigned long ProfileNameSyntax,
    _In_ RPC_WSTR ProfileName,
    _In_opt_ RPC_IF_ID __RPC_FAR *IfId,
    _In_ unsigned long MemberNameSyntax,
    _In_ RPC_WSTR MemberName,
    _In_ unsigned long Priority,
    _In_opt_ RPC_WSTR Annotation
    );

_Win32_metadata_supported_os_(windows5.0)
RPCNSAPI RPC_STATUS RPC_ENTRY
RpcNsProfileEltRemoveW(
    _In_ unsigned long ProfileNameSyntax,
    _In_ RPC_WSTR ProfileName,
    _In_opt_ RPC_IF_ID __RPC_FAR *IfId,
    _In_ unsigned long MemberNameSyntax,
    _In_ RPC_WSTR MemberName
    );

_Win32_metadata_supported_os_(windows5.0)
RPCNSAPI RPC_STATUS RPC_ENTRY
RpcNsProfileEltInqBeginW(
    _In_ unsigned long ProfileNameSyntax,
    _In_ RPC_WSTR ProfileName,
    _In_ unsigned long InquiryType,
    _In_opt_ RPC_IF_ID __RPC_FAR *IfId,
    _In_ unsigned long VersOption,
    _In_ unsigned long MemberNameSyntax,
    _In_opt_ RPC_WSTR MemberName,
    _Out_ RPC_NS_HANDLE __RPC_FAR *InquiryContext
    );

_Win32_metadata_supported_os_(windows5.0)
RPCNSAPI RPC_STATUS RPC_ENTRY
RpcNsProfileEltInqNextW(
    _In_ RPC_NS_HANDLE InquiryContext,
    _Out_opt_ RPC_IF_ID __RPC_FAR *IfId,
    _Out_ RPC_WSTR __RPC_FAR *MemberName,
    _Out_ unsigned long __RPC_FAR *Priority,
    _Out_ RPC_WSTR __RPC_FAR *Annotation
    );

#endif

_Win32_metadata_supported_os_(windows5.0)
RPCNSAPI RPC_STATUS RPC_ENTRY
RpcNsProfileEltInqDone(
    IN OUT RPC_NS_HANDLE __RPC_FAR * InquiryContext
    );

/* Entry object APIs */

_Win32_metadata_supported_os_(windows5.0)
RPCNSAPI RPC_STATUS RPC_ENTRY
RpcNsEntryObjectInqBeginA(
    _In_ unsigned long EntryNameSyntax,
    _In_ RPC_CSTR EntryName,
    _Out_ RPC_NS_HANDLE __RPC_FAR *InquiryContext
    );

#ifdef RPC_UNICODE_SUPPORTED

_Win32_metadata_supported_os_(windows5.0)
RPCNSAPI RPC_STATUS RPC_ENTRY
RpcNsEntryObjectInqBeginW(
    _In_ unsigned long EntryNameSyntax,
    _In_ RPC_WSTR EntryName,
    _Out_ RPC_NS_HANDLE __RPC_FAR *InquiryContext
    );

#endif

_Win32_metadata_supported_os_(windows5.0)
RPCNSAPI RPC_STATUS RPC_ENTRY
RpcNsEntryObjectInqNext(
    IN  RPC_NS_HANDLE InquiryContext,
    OUT UUID __RPC_FAR * ObjUuid
    );

_Win32_metadata_supported_os_(windows5.0)
RPCNSAPI RPC_STATUS RPC_ENTRY
RpcNsEntryObjectInqDone(
    IN OUT RPC_NS_HANDLE __RPC_FAR * InquiryContext
    );

/* Management and MISC APIs */

_Win32_metadata_supported_os_(windows5.0)
RPCNSAPI RPC_STATUS RPC_ENTRY
RpcNsEntryExpandNameA(
    _In_ unsigned long EntryNameSyntax,
    _In_ RPC_CSTR EntryName,
    _Out_ RPC_CSTR __RPC_FAR *ExpandedName
    );

_Win32_metadata_supported_os_(windows5.0)
RPCNSAPI RPC_STATUS RPC_ENTRY
RpcNsMgmtBindingUnexportA(
    _In_ unsigned long EntryNameSyntax,
    _In_ RPC_CSTR EntryName,
    _In_opt_ RPC_IF_ID __RPC_FAR *IfId,
    _In_ unsigned long VersOption,
    _In_opt_ UUID_VECTOR __RPC_FAR *ObjectUuidVec
    );

_Win32_metadata_supported_os_(windows5.0)
RPCNSAPI RPC_STATUS RPC_ENTRY
RpcNsMgmtEntryCreateA(
    _In_ unsigned long EntryNameSyntax,
    _In_ RPC_CSTR EntryName
    );

_Win32_metadata_supported_os_(windows5.0)
RPCNSAPI RPC_STATUS RPC_ENTRY
RpcNsMgmtEntryDeleteA(
    _In_ unsigned long EntryNameSyntax,
    _In_ RPC_CSTR EntryName
    );

_Win32_metadata_supported_os_(windows5.0)
RPCNSAPI RPC_STATUS RPC_ENTRY
RpcNsMgmtEntryInqIfIdsA(
    _In_ unsigned long EntryNameSyntax,
    _In_ RPC_CSTR EntryName,
    _Out_ RPC_IF_ID_VECTOR __RPC_FAR * __RPC_FAR *IfIdVec
    );

_Win32_metadata_supported_os_(windows5.0)
RPCNSAPI RPC_STATUS RPC_ENTRY
RpcNsMgmtHandleSetExpAge(
    IN RPC_NS_HANDLE NsHandle,
    IN unsigned long ExpirationAge
    );

_Win32_metadata_supported_os_(windows5.0)
RPCNSAPI RPC_STATUS RPC_ENTRY
RpcNsMgmtInqExpAge(
    OUT unsigned long __RPC_FAR * ExpirationAge
    );

_Win32_metadata_supported_os_(windows5.0)
RPCNSAPI RPC_STATUS RPC_ENTRY
RpcNsMgmtSetExpAge(
    IN unsigned long ExpirationAge
    );

#ifdef RPC_UNICODE_SUPPORTED

_Win32_metadata_supported_os_(windows5.0)
RPCNSAPI RPC_STATUS RPC_ENTRY
RpcNsEntryExpandNameW(
    _In_ unsigned long EntryNameSyntax,
    _In_ RPC_WSTR EntryName,
    _Out_ RPC_WSTR __RPC_FAR *ExpandedName
    );

_Win32_metadata_supported_os_(windows5.0)
RPCNSAPI RPC_STATUS RPC_ENTRY
RpcNsMgmtBindingUnexportW(
    _In_ unsigned long EntryNameSyntax,
    _In_ RPC_WSTR EntryName,
    _In_opt_ RPC_IF_ID __RPC_FAR *IfId,
    _In_ unsigned long VersOption,
    _In_opt_ UUID_VECTOR __RPC_FAR *ObjectUuidVec
    );

_Win32_metadata_supported_os_(windows5.0)
RPCNSAPI RPC_STATUS RPC_ENTRY
RpcNsMgmtEntryCreateW(
    _In_ unsigned long EntryNameSyntax,
    _In_ RPC_WSTR EntryName
    );

_Win32_metadata_supported_os_(windows5.0)
RPCNSAPI RPC_STATUS RPC_ENTRY
RpcNsMgmtEntryDeleteW(
    _In_ unsigned long EntryNameSyntax,
    _In_ RPC_WSTR EntryName
    );

_Win32_metadata_supported_os_(windows5.0)
RPCNSAPI RPC_STATUS RPC_ENTRY
RpcNsMgmtEntryInqIfIdsW(
    _In_ unsigned long EntryNameSyntax,
    _In_ RPC_WSTR EntryName,
    _Out_ RPC_IF_ID_VECTOR __RPC_FAR * __RPC_FAR *IfIdVec
    );

#endif

/* Client API's implemented in wrappers. */

_Win32_metadata_supported_os_(windows5.0)
RPCNSAPI RPC_STATUS RPC_ENTRY
RpcNsBindingImportBeginA(
    _In_ unsigned long EntryNameSyntax,
    _In_opt_ RPC_CSTR EntryName,
    _In_opt_ RPC_IF_HANDLE IfSpec,
    _In_opt_ UUID __RPC_FAR *ObjUuid,
    _Out_ RPC_NS_HANDLE __RPC_FAR *ImportContext
    );

#ifdef RPC_UNICODE_SUPPORTED

_Win32_metadata_supported_os_(windows5.0)
RPCNSAPI RPC_STATUS RPC_ENTRY
RpcNsBindingImportBeginW(
    _In_ unsigned long EntryNameSyntax,
    _In_opt_ RPC_WSTR EntryName,
    _In_opt_ RPC_IF_HANDLE IfSpec,
    _In_opt_ UUID __RPC_FAR *ObjUuid,
    _Out_ RPC_NS_HANDLE __RPC_FAR *ImportContext
    );

#endif

_Win32_metadata_supported_os_(windows5.0)
RPCNSAPI RPC_STATUS RPC_ENTRY
RpcNsBindingImportNext(
    IN RPC_NS_HANDLE ImportContext,
    OUT RPC_BINDING_HANDLE  __RPC_FAR * Binding
    );

_Win32_metadata_supported_os_(windows5.0)
RPCNSAPI RPC_STATUS RPC_ENTRY
RpcNsBindingImportDone(
    IN OUT RPC_NS_HANDLE __RPC_FAR * ImportContext
    );

_Win32_metadata_supported_os_(windows5.0)
RPCNSAPI RPC_STATUS RPC_ENTRY
RpcNsBindingSelect(
    IN OUT RPC_BINDING_VECTOR __RPC_FAR * BindingVec,
    OUT RPC_BINDING_HANDLE  __RPC_FAR * Binding
    );

#ifdef UNICODE

#define RpcNsBindingLookupBegin RpcNsBindingLookupBeginW
#define RpcNsBindingImportBegin RpcNsBindingImportBeginW
#define RpcNsBindingExport RpcNsBindingExportW
#define RpcNsBindingUnexport RpcNsBindingUnexportW
#define RpcNsGroupDelete RpcNsGroupDeleteW
#define RpcNsGroupMbrAdd RpcNsGroupMbrAddW
#define RpcNsGroupMbrRemove RpcNsGroupMbrRemoveW
#define RpcNsGroupMbrInqBegin RpcNsGroupMbrInqBeginW
#define RpcNsGroupMbrInqNext RpcNsGroupMbrInqNextW
#define RpcNsEntryExpandName RpcNsEntryExpandNameW
#define RpcNsEntryObjectInqBegin RpcNsEntryObjectInqBeginW
#define RpcNsMgmtBindingUnexport RpcNsMgmtBindingUnexportW
#define RpcNsMgmtEntryCreate RpcNsMgmtEntryCreateW
#define RpcNsMgmtEntryDelete RpcNsMgmtEntryDeleteW
#define RpcNsMgmtEntryInqIfIds RpcNsMgmtEntryInqIfIdsW
#define RpcNsProfileDelete RpcNsProfileDeleteW
#define RpcNsProfileEltAdd RpcNsProfileEltAddW
#define RpcNsProfileEltRemove RpcNsProfileEltRemoveW
#define RpcNsProfileEltInqBegin RpcNsProfileEltInqBeginW
#define RpcNsProfileEltInqNext RpcNsProfileEltInqNextW
#define RpcNsBindingExportPnP RpcNsBindingExportPnPW
#define RpcNsBindingUnexportPnP RpcNsBindingUnexportPnPW

#else

#define RpcNsBindingLookupBegin RpcNsBindingLookupBeginA
#define RpcNsBindingImportBegin RpcNsBindingImportBeginA
#define RpcNsBindingExport RpcNsBindingExportA
#define RpcNsBindingUnexport RpcNsBindingUnexportA
#define RpcNsGroupDelete RpcNsGroupDeleteA
#define RpcNsGroupMbrAdd RpcNsGroupMbrAddA
#define RpcNsGroupMbrRemove RpcNsGroupMbrRemoveA
#define RpcNsGroupMbrInqBegin RpcNsGroupMbrInqBeginA
#define RpcNsGroupMbrInqNext RpcNsGroupMbrInqNextA
#define RpcNsEntryExpandName RpcNsEntryExpandNameA
#define RpcNsEntryObjectInqBegin RpcNsEntryObjectInqBeginA
#define RpcNsMgmtBindingUnexport RpcNsMgmtBindingUnexportA
#define RpcNsMgmtEntryCreate RpcNsMgmtEntryCreateA
#define RpcNsMgmtEntryDelete RpcNsMgmtEntryDeleteA
#define RpcNsMgmtEntryInqIfIds RpcNsMgmtEntryInqIfIdsA
#define RpcNsProfileDelete RpcNsProfileDeleteA
#define RpcNsProfileEltAdd RpcNsProfileEltAddA
#define RpcNsProfileEltRemove RpcNsProfileEltRemoveA
#define RpcNsProfileEltInqBegin RpcNsProfileEltInqBeginA
#define RpcNsProfileEltInqNext RpcNsProfileEltInqNextA
#define RpcNsBindingExportPnP RpcNsBindingExportPnPA
#define RpcNsBindingUnexportPnP RpcNsBindingUnexportPnPA
#endif /* UNICODE */

#endif /* WINAPI_FAMILY_PARTITION(WINAPI_PARTITION_DESKTOP) */
#pragma endregion

#endif /* __RPCNSI_H__ */
