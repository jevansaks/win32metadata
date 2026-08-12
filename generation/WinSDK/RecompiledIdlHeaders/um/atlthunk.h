/*++

Module Name:

    atlthunk.h

Abstract:

    ATL thunks without writable executable memory.

Author:

    Jay Krell (jaykrell) 26-Apr-2013

--*/

#pragma once

#if defined(WIN32METADATA)
#include <win32metadata_annotations.h>
#endif

#ifdef __cplusplus
extern "C" {
#endif

#ifdef _MANAGED
struct AtlThunkData_t { };
#else
struct AtlThunkData_t; // opaque
typedef struct AtlThunkData_t AtlThunkData_t;
#endif

_Win32_metadata_import_library_("Atlthunk.dll")
AtlThunkData_t*
__stdcall
AtlThunk_AllocateData(
    void
    );

_Win32_metadata_import_library_("Atlthunk.dll")
void
__stdcall
AtlThunk_InitData(
    AtlThunkData_t* Thunk,
    void* /*WNDPROC*/ Proc,
    size_t FirstParameter
    );

_Win32_metadata_import_library_("Atlthunk.dll")
WNDPROC
__stdcall
AtlThunk_DataToCode(
    AtlThunkData_t*
    );

_Win32_metadata_import_library_("Atlthunk.dll")
void
__stdcall
AtlThunk_FreeData(
    AtlThunkData_t* Thunk
    );

#ifdef __cplusplus
} /* extern "C" */
#endif
