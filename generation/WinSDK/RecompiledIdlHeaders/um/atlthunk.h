#include <win32metadata_annotations.h>
/*++

Module Name:

    atlthunk.h

Abstract:

    ATL thunks without writable executable memory.

Author:

    Jay Krell (jaykrell) 26-Apr-2013

--*/

#pragma once
#include <sal.h>

#ifdef __cplusplus
extern "C" {
#endif

#ifdef _MANAGED
struct AtlThunkData_t { };
#else
struct AtlThunkData_t; // opaque
typedef struct AtlThunkData_t AtlThunkData_t;
#endif

AtlThunkData_t*
__stdcall
_Win32_SupportedOS_10240_ AtlThunk_AllocateData(
    void
    );

void
__stdcall
_Win32_SupportedOS_10240_ AtlThunk_InitData(
    AtlThunkData_t* Thunk,
    void* /*WNDPROC*/ Proc,
    size_t FirstParameter
    );

WNDPROC
__stdcall
_Win32_SupportedOS_10240_ AtlThunk_DataToCode(
    AtlThunkData_t*
    );

void
__stdcall
_Win32_SupportedOS_10240_ AtlThunk_FreeData(
    AtlThunkData_t* Thunk
    );

#ifdef __cplusplus
} /* extern "C" */
#endif
