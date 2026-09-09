#include <windows.h>
#include <win32metadata_annotations.h>

#ifdef __cplusplus
extern "C" {
#endif

typedef _Return_type_success_(return >= 0) LONG NTSTATUS;

typedef enum _CONSOLECONTROL
{
    Reserved1,
    ConsoleNotifyConsoleApplication,
    Reserved2,
    ConsoleSetCaretInfo,
    Reserved3,
    ConsoleSetForeground,
    ConsoleSetWindowOwner,
    ConsoleEndTask,
} CONSOLECONTROL;

typedef struct _CONSOLEENDTASK
{
    HANDLE ProcessId;
    HWND hwnd;
    ULONG ConsoleEventCode;
    ULONG ConsoleFlags;
} CONSOLEENDTASK, *PCONSOLEENDTASK;

typedef struct _CONSOLEWINDOWOWNER
{
    HWND hwnd;
    ULONG ProcessId;
    ULONG ThreadId;
} CONSOLEWINDOWOWNER, *PCONSOLEWINDOWOWNER;

typedef struct _CONSOLESETFOREGROUND
{
    HANDLE hProcess;
    BOOL bForeground;
} CONSOLESETFOREGROUND, *PCONSOLESETFOREGROUND;

typedef struct _CONSOLE_PROCESS_INFO
{
    IN DWORD dwProcessID;
    IN DWORD dwFlags;
} CONSOLE_PROCESS_INFO, *PCONSOLE_PROCESS_INFO;

typedef struct _CONSOLE_CARET_INFO
{
    IN HWND hwnd;
    IN RECT rc;
} CONSOLE_CARET_INFO, *PCONSOLE_CARET_INFO;

_Win32_metadata_import_library_("USER32.dll")
NTSTATUS ConsoleControl(
  _In_ CONSOLECONTROL Command,
  _In_reads_bytes_(ConsoleInformationLength) PVOID ConsoleInformation,
  _In_ DWORD ConsoleInformationLength
);

#ifdef __cplusplus
}
#endif