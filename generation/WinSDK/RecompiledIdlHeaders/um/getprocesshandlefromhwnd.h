#include <windows.h>
#include <win32metadata_annotations.h>

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