#if defined(WIN32METADATA)
#include <win32metadata_annotations.h>
#endif

#include <winapifamily.h>

//+-------------------------------------------------------------------------
//
//  Microsoft Windows
//  Copyright (C) Microsoft Corporation, 1992-2006.
//
//  File:       propapi.h
//
//  Contents:   Structured storage properties APIs
//
//--------------------------------------------------------------------------


#ifndef _PROPAPI_H_
#define _PROPAPI_H_

#pragma once

#include <winapifamily.h>

#pragma region Desktop Family
#if WINAPI_FAMILY_PARTITION(WINAPI_PARTITION_DESKTOP)

#ifdef __cplusplus
extern "C" {
#endif

typedef VOID* NTPROP;

#include <propidl.h>

_Win32_metadata_supported_os_(windows5.0)
EXTERN_C
_Success_(TRUE)  /* Raises status on failure */
ULONG __stdcall
StgPropertyLengthAsVariant(
            _In_reads_bytes_(cbProp) const SERIALIZEDPROPERTYVALUE* pProp,
            _In_ ULONG cbProp,
            _In_ USHORT CodePage,
            _Reserved_ BYTE bReserved);

#ifdef __cplusplus
}
#endif

#endif /* WINAPI_FAMILY_PARTITION(WINAPI_PARTITION_DESKTOP) */
#pragma endregion

#endif // ifndef _PROPAPI_H_
