#include <winapifamily.h>
#include <win32metadata_annotations.h>

/****************************************************************************
*                                                                           *
* windef.h -- Basic Windows Type Definitions                                *
*                                                                           *
* Copyright (c) Microsoft Corporation. All rights reserved.                 *
*                                                                           *
****************************************************************************/


#ifndef _WINDEF_
#define _WINDEF_
#pragma once

#ifdef __cplusplus
extern "C" {
#endif

#ifdef _M_CEE_PURE
#define WINAPI_INLINE  __clrcall
#endif

#include <minwindef.h>

#ifndef WINVER
#define WINVER 0x0500
#endif /* WINVER */

#ifndef NT_INCLUDED
#include <winnt.h>
#endif /* NT_INCLUDED */

#ifndef WIN_INTERNAL

#pragma region Application Family or OneCore Family or Games Family
#if WINAPI_FAMILY_PARTITION(WINAPI_PARTITION_APP | WINAPI_PARTITION_SYSTEM | WINAPI_PARTITION_GAMES)

DECLARE_HANDLE            (HWND)
    _Win32_metadata_also_usable_for_(HANDLE);
DECLARE_HANDLE            (HHOOK)
    _Win32_metadata_invalid_handle_(-1)
    _Win32_metadata_invalid_handle_(0)
    _Win32_metadata_raii_free_(UnhookWindowsHookEx);

#endif /* WINAPI_FAMILY_PARTITION(WINAPI_PARTITION_APP | WINAPI_PARTITION_SYSTEM | WINAPI_PARTITION_GAMES) */
#pragma endregion

#ifdef WINABLE

#pragma region Desktop Family
#if WINAPI_FAMILY_PARTITION(WINAPI_PARTITION_DESKTOP)

DECLARE_HANDLE            (HEVENT);

#endif /* WINAPI_FAMILY_PARTITION(WINAPI_PARTITION_DESKTOP) */
#pragma endregion

#endif
#endif

#pragma region Application Family
#if WINAPI_FAMILY_PARTITION(WINAPI_PARTITION_APP | WINAPI_PARTITION_SYSTEM | WINAPI_PARTITION_GAMES)

#if !defined(_MAC) || !defined(GDI_INTERNAL)
#ifdef STRICT
typedef void NEAR* HGDIOBJ
    _Win32_metadata_invalid_handle_(-1)
    _Win32_metadata_invalid_handle_(0)
    _Win32_metadata_raii_free_(DeleteObject);
#else
DECLARE_HANDLE(HGDIOBJ)
    _Win32_metadata_invalid_handle_(-1)
    _Win32_metadata_invalid_handle_(0)
    _Win32_metadata_raii_free_(DeleteObject);
#endif
#endif

#if !defined(_MAC) || !defined(WIN_INTERNAL)
DECLARE_HANDLE(HACCEL)
    _Win32_metadata_invalid_handle_(-1)
    _Win32_metadata_invalid_handle_(0)
    _Win32_metadata_raii_free_(DestroyAcceleratorTable);
#endif
#if !defined(_MAC) || !defined(GDI_INTERNAL)
DECLARE_HANDLE(HBITMAP)
    _Win32_metadata_also_usable_for_(HGDIOBJ)
    _Win32_metadata_invalid_handle_(-1)
    _Win32_metadata_invalid_handle_(0)
    _Win32_metadata_raii_free_(DeleteObject);
DECLARE_HANDLE(HBRUSH)
    _Win32_metadata_also_usable_for_(HGDIOBJ)
    _Win32_metadata_invalid_handle_(-1)
    _Win32_metadata_invalid_handle_(0)
    _Win32_metadata_raii_free_(DeleteObject);
#endif
#if(WINVER >= 0x0400)
DECLARE_HANDLE(HCOLORSPACE)
    _Win32_metadata_invalid_handle_(-1)
    _Win32_metadata_invalid_handle_(0)
    _Win32_metadata_raii_free_(DeleteColorSpace);
#endif /* WINVER >= 0x0400 */
#if !defined(_MAC) || !defined(GDI_INTERNAL)
DECLARE_HANDLE(HDC)
    _Win32_metadata_invalid_handle_(-1)
    _Win32_metadata_invalid_handle_(0);
#endif
DECLARE_HANDLE(HGLRC)
    _Win32_metadata_invalid_handle_(-1)
    _Win32_metadata_invalid_handle_(0)
    _Win32_metadata_raii_free_(wglDeleteContext);          // OpenGL
DECLARE_HANDLE(HDESK)
    _Win32_metadata_invalid_handle_(-1)
    _Win32_metadata_invalid_handle_(0)
    _Win32_metadata_raii_free_(CloseDesktop);
DECLARE_HANDLE(HENHMETAFILE)
    _Win32_metadata_invalid_handle_(-1)
    _Win32_metadata_invalid_handle_(0)
    _Win32_metadata_raii_free_(DeleteEnhMetaFile);
#if !defined(_MAC) || !defined(GDI_INTERNAL)
DECLARE_HANDLE(HFONT)
    _Win32_metadata_also_usable_for_(HGDIOBJ)
    _Win32_metadata_invalid_handle_(-1)
    _Win32_metadata_invalid_handle_(0)
    _Win32_metadata_raii_free_(DeleteObject);
#endif
DECLARE_HANDLE(HICON)
    _Win32_metadata_invalid_handle_(-1)
    _Win32_metadata_invalid_handle_(0)
    _Win32_metadata_raii_free_(DestroyIcon);
#if !defined(_MAC) || !defined(WIN_INTERNAL)
DECLARE_HANDLE(HMENU)
    _Win32_metadata_invalid_handle_(-1)
    _Win32_metadata_invalid_handle_(0)
    _Win32_metadata_raii_free_(DestroyMenu);
#endif
#if !defined(_MAC) || !defined(GDI_INTERNAL)
DECLARE_HANDLE(HPALETTE)
    _Win32_metadata_also_usable_for_(HGDIOBJ)
    _Win32_metadata_invalid_handle_(-1)
    _Win32_metadata_invalid_handle_(0)
    _Win32_metadata_raii_free_(DeleteObject);
DECLARE_HANDLE(HPEN)
    _Win32_metadata_also_usable_for_(HGDIOBJ)
    _Win32_metadata_invalid_handle_(-1)
    _Win32_metadata_invalid_handle_(0)
    _Win32_metadata_raii_free_(DeleteObject);
#endif

#if(WINVER >= 0x0400)
DECLARE_HANDLE(HWINEVENTHOOK)
    _Win32_metadata_invalid_handle_(-1)
    _Win32_metadata_invalid_handle_(0)
    _Win32_metadata_raii_free_(UnhookWinEvent);
#endif /* WINVER >= 0x0400 */

#endif /* WINAPI_FAMILY_PARTITION(WINAPI_PARTITION_APP | WINAPI_PARTITION_SYSTEM | WINAPI_PARTITION_GAMES) */
#pragma endregion

#if(WINVER >= 0x0500)
#ifndef _MAC

#pragma region Application Family
#if WINAPI_FAMILY_PARTITION(WINAPI_PARTITION_APP | WINAPI_PARTITION_SYSTEM)

DECLARE_HANDLE(HMONITOR)
    _Win32_metadata_invalid_handle_(-1)
    _Win32_metadata_invalid_handle_(0);

#endif /* WINAPI_FAMILY_PARTITION(WINAPI_PARTITION_APP | WINAPI_PARTITION_SYSTEM) */
#pragma endregion

#endif

#pragma region Desktop Family
#if WINAPI_FAMILY_PARTITION(WINAPI_PARTITION_DESKTOP)

DECLARE_HANDLE(HUMPD);

#endif /* WINAPI_FAMILY_PARTITION(WINAPI_PARTITION_DESKTOP) */
#pragma endregion

#endif /* WINVER >= 0x0500 */

#pragma region Application Family
#if WINAPI_FAMILY_PARTITION(WINAPI_PARTITION_APP | WINAPI_PARTITION_SYSTEM | WINAPI_PARTITION_GAMES)

#ifndef _MAC
typedef HICON HCURSOR
    _Win32_metadata_also_usable_for_(HICON)
    _Win32_metadata_invalid_handle_(-1)
    _Win32_metadata_invalid_handle_(0)
    _Win32_metadata_raii_free_(DestroyCursor);      /* HICONs & HCURSORs are polymorphic */
#else
DECLARE_HANDLE(HCURSOR)
    _Win32_metadata_also_usable_for_(HICON)
    _Win32_metadata_invalid_handle_(-1)
    _Win32_metadata_invalid_handle_(0)
    _Win32_metadata_raii_free_(DestroyCursor);    /* HICONs & HCURSORs are not polymorphic */
#endif

typedef DWORD   COLORREF;

#endif /* WINAPI_FAMILY_PARTITION(WINAPI_PARTITION_APP | WINAPI_PARTITION_SYSTEM | WINAPI_PARTITION_GAMES) */
#pragma endregion

#pragma region Desktop Family
#if WINAPI_FAMILY_PARTITION(WINAPI_PARTITION_DESKTOP)

typedef DWORD   *LPCOLORREF;

#define HFILE_ERROR ((HFILE)-1)

#endif /* WINAPI_FAMILY_PARTITION(WINAPI_PARTITION_DESKTOP) */
#pragma endregion

#pragma region Application Family or Games Family
#if WINAPI_FAMILY_PARTITION(WINAPI_PARTITION_APP | WINAPI_PARTITION_SYSTEM | WINAPI_PARTITION_GAMES)

typedef struct tagRECT
{
    LONG    left;
    LONG    top;
    LONG    right;
    LONG    bottom;
} RECT, *PRECT, NEAR *NPRECT, FAR *LPRECT;

typedef const RECT FAR* LPCRECT;

typedef struct _RECTL       /* rcl */
{
    LONG    left;
    LONG    top;
    LONG    right;
    LONG    bottom;
} RECTL, *PRECTL, *LPRECTL;

typedef const RECTL FAR* LPCRECTL;

typedef struct tagPOINT
{
    LONG  x;
    LONG  y;
} POINT, *PPOINT, NEAR *NPPOINT, FAR *LPPOINT;

typedef struct _POINTL      /* ptl  */
{
    LONG  x;
    LONG  y;
} POINTL, *PPOINTL;

typedef struct tagSIZE
{
    LONG        cx;
    LONG        cy;
} SIZE, *PSIZE, *LPSIZE;

typedef SIZE               SIZEL;
typedef SIZE               *PSIZEL, *LPSIZEL;

typedef struct tagPOINTS
{
#ifndef _MAC
    SHORT   x;
    SHORT   y;
#else
    SHORT   y;
    SHORT   x;
#endif
} POINTS, *PPOINTS, *LPPOINTS;

#define APP_LOCAL_DEVICE_ID_SIZE 32
typedef struct APP_LOCAL_DEVICE_ID
{
    BYTE value[APP_LOCAL_DEVICE_ID_SIZE];
} APP_LOCAL_DEVICE_ID;

#endif /* WINAPI_FAMILY_PARTITION(WINAPI_PARTITION_APP | WINAPI_PARTITION_SYSTEM | WINAPI_PARTITION_GAMES) */
#pragma endregion

/* mode selections for the device mode function */
#define DM_UPDATE           1
#define DM_COPY             2
#define DM_PROMPT           4
#define DM_MODIFY           8

#define DM_IN_BUFFER        DM_MODIFY
#define DM_IN_PROMPT        DM_PROMPT
#define DM_OUT_BUFFER       DM_COPY
#define DM_OUT_DEFAULT      DM_UPDATE

/* device capabilities indices */
#define DC_FIELDS           1
#define DC_PAPERS           2
#define DC_PAPERSIZE        3
#define DC_MINEXTENT        4
#define DC_MAXEXTENT        5
#define DC_BINS             6
#define DC_DUPLEX           7
#define DC_SIZE             8
#define DC_EXTRA            9
#define DC_VERSION          10
#define DC_DRIVER           11
#define DC_BINNAMES         12
#define DC_ENUMRESOLUTIONS  13
#define DC_FILEDEPENDENCIES 14
#define DC_TRUETYPE         15
#define DC_PAPERNAMES       16
#define DC_ORIENTATION      17
#define DC_COPIES           18

#ifdef __cplusplus
}
#endif

#pragma region Desktop Family

#if WINAPI_FAMILY_PARTITION(WINAPI_PARTITION_DESKTOP)

#define _DPI_AWARENESS_CONTEXTS_

DECLARE_HANDLE(DPI_AWARENESS_CONTEXT);

typedef enum DPI_AWARENESS {
    DPI_AWARENESS_INVALID           = -1,
    DPI_AWARENESS_UNAWARE           = 0,
    DPI_AWARENESS_SYSTEM_AWARE      = 1,
    DPI_AWARENESS_PER_MONITOR_AWARE = 2
} DPI_AWARENESS;

#define DPI_AWARENESS_CONTEXT_UNAWARE               ((DPI_AWARENESS_CONTEXT)-1)
#define DPI_AWARENESS_CONTEXT_SYSTEM_AWARE          ((DPI_AWARENESS_CONTEXT)-2)
#define DPI_AWARENESS_CONTEXT_PER_MONITOR_AWARE     ((DPI_AWARENESS_CONTEXT)-3)
#define DPI_AWARENESS_CONTEXT_PER_MONITOR_AWARE_V2  ((DPI_AWARENESS_CONTEXT)-4)
#define DPI_AWARENESS_CONTEXT_UNAWARE_GDISCALED     ((DPI_AWARENESS_CONTEXT)-5)

typedef enum DPI_HOSTING_BEHAVIOR {
    DPI_HOSTING_BEHAVIOR_INVALID     = -1,
    DPI_HOSTING_BEHAVIOR_DEFAULT     = 0,
    DPI_HOSTING_BEHAVIOR_MIXED       = 1
} DPI_HOSTING_BEHAVIOR;

#endif

#endif /* _WINDEF_ */
