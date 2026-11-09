#include <win32metadata_annotations.h>
/*++ BUILD Version: 0001    // Increment this if a change has global effects

Copyright (c) 1991-1999  Microsoft Corporation

Module Name:

    lmuseflg.h

Abstract:

    This file contains deletion force levels for deleting a connection.

Environment:

    User Mode - Win32

Notes:

    This file has no dependencies.  It is included by lmwksta.h and
    lmuse.h.

Revision History:

--*/

#ifndef _LMUSEFLG_
#define _LMUSEFLG_

#if _MSC_VER > 1000
#pragma once
#endif
#include <winapifamily.h>

#pragma region Desktop Family or OneCore Family
#if WINAPI_FAMILY_PARTITION(WINAPI_PARTITION_DESKTOP | WINAPI_PARTITION_SYSTEM)


//
// Definition for NetWkstaTransportDel and NetUseDel deletion force levels and flags. The lower 16 bits
// define the force levels and the upper 16 bits are the use flags defined in lmuse.h
//

#define USE_NOFORCE             0
#define USE_FORCE               1
#define USE_LOTS_OF_FORCE       2

#define FORCE_LEVEL(LEVELFLAGS) ((LEVELFLAGS) & 0xffff)

#endif /* WINAPI_FAMILY_PARTITION(WINAPI_PARTITION_DESKTOP | WINAPI_PARTITION_SYSTEM) */
#pragma endregion

#if defined(WIN32METADATA) && defined(__clang__) && defined(__cplusplus) && !defined(MIDL_PASS)
#include <sal.h>
_Win32_Enum_(FORCE_LEVEL_FLAGS, unsigned long,
    USE_NOFORCE,
    USE_FORCE,
    USE_LOTS_OF_FORCE);
#endif

#endif // _LMUSEFLG_
