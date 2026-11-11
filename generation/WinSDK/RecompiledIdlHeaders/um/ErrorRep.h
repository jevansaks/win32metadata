#include <win32metadata_annotations.h>
/******************************************************************************

Copyright (c) Microsoft Corporation.  All rights reserved.

Module Name:
    errorrep.h

Abstract:
    contains functions used to report errors to Microsoft

******************************************************************************/	

#ifndef __ERRORREP_H__
#define __ERRORREP_H__

#ifdef __cplusplus
extern "C" {
#endif

#include <winapifamily.h>

#pragma region Desktop Family
#if WINAPI_FAMILY_PARTITION(WINAPI_PARTITION_DESKTOP)


typedef enum tagEFaultRepRetVal
{
    frrvOk = 0,
    frrvOkManifest,
    frrvOkQueued,
    frrvErr,
    frrvErrNoDW,
    frrvErrTimeout,
    frrvLaunchDebugger,
    frrvOkHeadless,
    frrvErrAnotherInstance,
    frrvErrNoMemory,
    frrvErrDoubleFault,
} EFaultRepRetVal;
_Win32_SupportedOS_WindowsXP_

EFaultRepRetVal APIENTRY ReportFault(_In_ LPEXCEPTION_POINTERS pep, _In_ DWORD dwOpt);
_Win32_SetLastError_
_Win32_SupportedOS_WindowsXP_

BOOL            APIENTRY AddERExcludedApplicationA(_In_ LPCSTR szApplication);
_Win32_SetLastError_
_Win32_SupportedOS_WindowsXP_
BOOL            APIENTRY AddERExcludedApplicationW(_In_ LPCWSTR wszApplication);


typedef EFaultRepRetVal (APIENTRY *pfn_REPORTFAULT)(_In_ LPEXCEPTION_POINTERS, _In_ DWORD);
typedef EFaultRepRetVal (APIENTRY *pfn_ADDEREXCLUDEDAPPLICATIONA)(_In_ LPCSTR);
typedef EFaultRepRetVal (APIENTRY *pfn_ADDEREXCLUDEDAPPLICATIONW)(_In_ LPCWSTR);

#ifdef UNICODE
#define AddERExcludedApplication AddERExcludedApplicationW
#define pfn_ADDEREXCLUDEDAPPLICATION pfn_ADDEREXCLUDEDAPPLICATIONW
#else
#define AddERExcludedApplication AddERExcludedApplicationA
#define pfn_ADDEREXCLUDEDAPPLICATION pfn_ADDEREXCLUDEDAPPLICATIONA
#endif
_Win32_SupportedOS_WindowsVista_

// 
//+++++++++++++++++++++ Hang reporting api ++++++++++++++++++++++++++++++++
//
HRESULT 
WINAPI
WerReportHang(
    _In_ HWND hwndHungApp,
    _In_opt_ PCWSTR pwzHungApplicationName
    );



#endif /* WINAPI_FAMILY_PARTITION(WINAPI_PARTITION_DESKTOP) */
#pragma endregion

#ifdef __cplusplus
}
#endif

#endif
