#include <windows.h>
#if defined(WIN32METADATA)
#include <win32metadata_annotations.h>
#endif

#ifdef __cplusplus
extern "C" {
#endif

HANDLE WINAPI GetProcessHandleFromHwnd(
  _In_ HWND hwnd
)
    _Win32_metadata_raii_free_(CloseHandle);

#ifdef __cplusplus
}
#endif