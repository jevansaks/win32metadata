// Partially sourced from https://github.com/microsoft/terminal/blob/main/dep/Console/winconp.h

#ifndef _WINCONP_
#define _WINCONP_

#pragma once

#ifdef __cplusplus
extern "C" {
#endif

#if _MSC_VER >= 1200
#pragma warning(push)
#pragma warning(disable:4820) // padding added after data member
#endif

#include <wincontypes.h>
#include <win32metadata_annotations.h>

// ...

//
// Selection flags (internal)
//

#define CONSOLE_SELECTION_INVERTED      0x0010   // selection is inverted (turned off)
#define CONSOLE_SELECTION_VALID         (CONSOLE_SELECTION_IN_PROGRESS | \
                                         CONSOLE_SELECTION_NOT_EMPTY | \
                                         CONSOLE_MOUSE_SELECTION | \
                                         CONSOLE_MOUSE_DOWN)


_Win32_metadata_import_library_("user32.dll")
WINBASEAPI
BOOL
WINAPI
GetConsoleKeyboardLayoutNameA(
    _Out_writes_(KL_NAMELENGTH) LPSTR pszLayout);
_Win32_metadata_import_library_("user32.dll")
WINBASEAPI
BOOL
WINAPI
GetConsoleKeyboardLayoutNameW(
    _Out_writes_(KL_NAMELENGTH) LPWSTR pszLayout);
#ifdef UNICODE
#define GetConsoleKeyboardLayoutName  GetConsoleKeyboardLayoutNameW
#else
#define GetConsoleKeyboardLayoutName  GetConsoleKeyboardLayoutNameA
#endif // !UNICODE

// ...

//
// TrueType font list
//

// doesn't available bold when add BOLD_MARK on first of face name.
#define BOLD_MARK    (L'*')

// ...

#ifndef NOGDI

typedef struct _CONSOLE_GRAPHICS_BUFFER_INFO {
    DWORD dwBitMapInfoLength;
    LPBITMAPINFO lpBitMapInfo;
    DWORD dwUsage;
    HANDLE hMutex;
    PVOID lpBitMap;
} CONSOLE_GRAPHICS_BUFFER_INFO, *PCONSOLE_GRAPHICS_BUFFER_INFO;

#endif

_Win32_metadata_import_library_("KERNEL32.dll")
BOOL
WINAPI
InvalidateConsoleDIBits(
    _In_ HANDLE hConsoleOutput,
    _In_ PSMALL_RECT lpRect);

_Win32_metadata_import_library_("KERNEL32.dll")
VOID
WINAPI
SetLastConsoleEventActive(
    VOID);

#define VDM_HIDE_WINDOW         1
#define VDM_IS_ICONIC           2
#define VDM_CLIENT_RECT         3
#define VDM_CLIENT_TO_SCREEN    4
#define VDM_SCREEN_TO_CLIENT    5
#define VDM_IS_HIDDEN           6
#define VDM_FULLSCREEN_NOPAINT  7
#define VDM_SET_VIDEO_MODE      8

_Win32_metadata_import_library_("KERNEL32.dll")
BOOL
WINAPI
VDMConsoleOperation(
    _In_ DWORD iFunction,
    _Inout_opt_ LPVOID lpData);


_Win32_metadata_import_library_("KERNEL32.dll")
BOOL
WINAPI
SetConsoleIcon(
    _In_ HICON hIcon);

// ...

_Win32_metadata_import_library_("KERNEL32.dll")
BOOL
WINAPI
SetConsoleFont(
    _In_ HANDLE hConsoleOutput,
    _In_ DWORD nFont);

_Win32_metadata_import_library_("KERNEL32.dll")
DWORD
WINAPI
GetConsoleFontInfo(
    _In_ HANDLE hConsoleOutput,
    _In_ BOOL bMaximumWindow,
    _In_ DWORD nLength,
    _Out_ PCONSOLE_FONT_INFO lpConsoleFontInfo);

_Win32_metadata_import_library_("KERNEL32.dll")
DWORD
WINAPI
GetNumberOfConsoleFonts(
    VOID);

BOOL
WINAPI
SetConsoleCursor(
    _In_ HANDLE hConsoleOutput,
    _In_ HCURSOR hCursor);

_Win32_metadata_import_library_("KERNEL32.dll")
int
WINAPI
ShowConsoleCursor(
    _In_ HANDLE hConsoleOutput,
    _In_ BOOL bShow);

_Win32_metadata_import_library_("KERNEL32.dll")
HMENU
APIENTRY
ConsoleMenuControl(
    _In_ HANDLE hConsoleOutput,
    _In_ UINT dwCommandIdLow,
    _In_ UINT dwCommandIdHigh);

_Win32_metadata_import_library_("KERNEL32.dll")
BOOL
SetConsolePalette(
    _In_ HANDLE hConsoleOutput,
    _In_ HPALETTE hPalette,
    _In_ UINT dwUsage);

#define CONSOLE_UNREGISTER_VDM 0
#define CONSOLE_REGISTER_VDM   1
#define CONSOLE_REGISTER_WOW   2

_Win32_metadata_import_library_("KERNEL32.dll")
BOOL
APIENTRY
RegisterConsoleVDM(
    _In_ DWORD dwRegisterFlags,
    _In_ HANDLE hStartHardwareEvent,
    _In_ HANDLE hEndHardwareEvent,
    _In_ HANDLE hErrorhardwareEvent,
    _Reserved_ DWORD Reserved,
    _Out_ LPDWORD lpStateLength,
    _Outptr_ PVOID *lpState,
    _In_opt_ COORD VDMBufferSize,
    _Outptr_ PVOID *lpVDMBuffer);

_Win32_metadata_import_library_("KERNEL32.dll")
BOOL
APIENTRY
GetConsoleHardwareState(
    _In_ HANDLE hConsoleOutput,
    _Out_ PCOORD lpResolution,
    _Out_ PCOORD lpFontSize);

_Win32_metadata_import_library_("KERNEL32.dll")
BOOL
APIENTRY
SetConsoleHardwareState(
    _In_ HANDLE hConsoleOutput,
    _In_ COORD dwResolution,
    _In_ COORD dwFontSize);

#define CONSOLE_NOSHORTCUTKEY   0x00000000        /* no shortcut key  */
#define CONSOLE_ALTTAB          0x00000001        /* Alt + Tab        */
#define CONSOLE_ALTESC          0x00000002        /* Alt + Escape     */
#define CONSOLE_ALTSPACE        0x00000004        /* Alt + Space      */
#define CONSOLE_ALTENTER        0x00000008        /* Alt + Enter      */
#define CONSOLE_ALTPRTSC        0x00000010        /* Alt Print screen */
#define CONSOLE_PRTSC           0x00000020        /* Print screen     */
#define CONSOLE_CTRLESC         0x00000040        /* Ctrl + Escape    */

typedef struct _APPKEY {
    WORD Modifier;
    WORD ScanCode;
} APPKEY, *LPAPPKEY;

#define CONSOLE_MODIFIER_SHIFT      0x0003   // Left shift key
#define CONSOLE_MODIFIER_CONTROL    0x0004   // Either Control shift key
#define CONSOLE_MODIFIER_ALT        0x0008   // Either Alt shift key

_Win32_metadata_import_library_("KERNEL32.dll")
BOOL
APIENTRY
SetConsoleKeyShortcuts(
    _In_ BOOL bSet,
    _In_ BYTE bReserveKeys,
    _In_reads_(dwNumAppKeys) LPAPPKEY lpAppKeys,
    _In_ DWORD dwNumAppKeys);

_Win32_metadata_import_library_("KERNEL32.dll")
BOOL
APIENTRY
SetConsoleMenuClose(
    _In_ BOOL bEnable);

_Win32_metadata_import_library_("KERNEL32.dll")
DWORD
GetConsoleInputExeNameA(
    _In_ DWORD nBufferLength,
    _Out_writes_(nBufferLength) LPSTR lpBuffer);
_Win32_metadata_import_library_("KERNEL32.dll")
DWORD
GetConsoleInputExeNameW(
    _In_ DWORD nBufferLength,
    _Out_writes_(nBufferLength) LPWSTR lpBuffer);
#ifdef UNICODE
#define GetConsoleInputExeName  GetConsoleInputExeNameW
#else
#define GetConsoleInputExeName  GetConsoleInputExeNameA
#endif // !UNICODE

_Win32_metadata_import_library_("KERNEL32.dll")
BOOL
SetConsoleInputExeNameA(
    _In_ LPSTR lpExeName);
_Win32_metadata_import_library_("KERNEL32.dll")
BOOL
SetConsoleInputExeNameW(
    _In_ LPWSTR lpExeName);
#ifdef UNICODE
#define SetConsoleInputExeName  SetConsoleInputExeNameW
#else
#define SetConsoleInputExeName  SetConsoleInputExeNameA
#endif // !UNICODE

_Win32_metadata_import_library_("KERNEL32.dll")
BOOL
WINAPI
ReadConsoleInputExA(
    _In_ HANDLE hConsoleInput,
    _Out_writes_(nLength) PINPUT_RECORD lpBuffer,
    _In_ DWORD nLength,
    _Out_ LPDWORD lpNumberOfEventsRead,
    _In_ USHORT wFlags);
_Win32_metadata_import_library_("KERNEL32.dll")
BOOL
WINAPI
ReadConsoleInputExW(
    _In_ HANDLE hConsoleInput,
    _Out_writes_(nLength) PINPUT_RECORD lpBuffer,
    _In_ DWORD nLength,
    _Out_ LPDWORD lpNumberOfEventsRead,
    _In_ USHORT wFlags);
#ifdef UNICODE
#define ReadConsoleInputEx  ReadConsoleInputExW
#else
#define ReadConsoleInputEx  ReadConsoleInputExA
#endif // !UNICODE

_Win32_metadata_import_library_("KERNEL32.dll")
BOOL
WINAPI
WriteConsoleInputVDMA(
    _In_ HANDLE hConsoleInput,
    _In_reads_(nLength) PINPUT_RECORD lpBuffer,
    _In_ DWORD nLength,
    _Out_ LPDWORD lpNumberOfEventsWritten);
_Win32_metadata_import_library_("KERNEL32.dll")
BOOL
WINAPI
WriteConsoleInputVDMW(
    _In_ HANDLE hConsoleInput,
    _In_reads_(nLength) PINPUT_RECORD lpBuffer,
    _In_ DWORD nLength,
    _Out_ LPDWORD lpNumberOfEventsWritten);
#ifdef UNICODE
#define WriteConsoleInputVDM  WriteConsoleInputVDMW
#else
#define WriteConsoleInputVDM  WriteConsoleInputVDMA
#endif // !UNICODE


_Win32_metadata_import_library_("KERNEL32.dll")
BOOL
APIENTRY
GetConsoleNlsMode(
    _In_ HANDLE hConsole,
    _Out_ PDWORD lpdwNlsMode);

_Win32_metadata_import_library_("KERNEL32.dll")
BOOL
APIENTRY
SetConsoleNlsMode(
    _In_ HANDLE hConsole,
    _In_ DWORD fdwNlsMode);

_Win32_metadata_import_library_("KERNEL32.dll")
BOOL
APIENTRY
GetConsoleCharType(
    _In_ HANDLE hConsole,
    _In_ COORD coordCheck,
    _Out_ PDWORD pdwType);

#define CHAR_TYPE_SBCS     0   // Displayed SBCS character
#define CHAR_TYPE_LEADING  2   // Displayed leading byte of DBCS
#define CHAR_TYPE_TRAILING 3   // Displayed trailing byte of DBCS

_Win32_metadata_import_library_("KERNEL32.dll")
BOOL
APIENTRY
SetConsoleLocalEUDC(
    _In_ HANDLE hConsoleHandle,
    _In_ WORD   wCodePoint,
    _In_ COORD  cFontSize,
    _In_ PCHAR  lpSB);

_Win32_metadata_import_library_("KERNEL32.dll")
BOOL
APIENTRY
SetConsoleCursorMode(
    _In_ HANDLE hConsoleHandle,
    _In_ BOOL   Blink,
    _In_ BOOL   DBEnable);

_Win32_metadata_import_library_("KERNEL32.dll")
BOOL
APIENTRY
GetConsoleCursorMode(
    _In_ HANDLE hConsoleHandle,
    _Out_ PBOOL  pbBlink,
    _Out_ PBOOL  pbDBEnable);

_Win32_metadata_import_library_("KERNEL32.dll")
BOOL
APIENTRY
RegisterConsoleOS2(
    _In_ BOOL fOs2Register);

_Win32_metadata_import_library_("KERNEL32.dll")
BOOL
APIENTRY
SetConsoleOS2OemFormat(
    _In_ BOOL fOs2OemFormat);

// ...

#if defined(FE_IME)
_Win32_metadata_import_library_("KERNEL32.dll")
BOOL
APIENTRY
RegisterConsoleIME(
    _In_ HWND  hWndConsoleIME,
    _Out_opt_ DWORD *lpdwConsoleThreadId);

_Win32_metadata_import_library_("KERNEL32.dll")
BOOL
APIENTRY
UnregisterConsoleIME(
    VOID);
#endif // FE_IME

//
// These bits are always on for console handles and are used for routing
// by windows.
//

#define CONSOLE_HANDLE_SIGNATURE 0x00000003
#define CONSOLE_HANDLE_NEVERSET  0x10000000
#define CONSOLE_HANDLE_MASK      (CONSOLE_HANDLE_SIGNATURE | CONSOLE_HANDLE_NEVERSET)

#define CONSOLE_HANDLE(HANDLE) (((ULONG_PTR)(HANDLE) & CONSOLE_HANDLE_MASK) == CONSOLE_HANDLE_SIGNATURE)

//
// These strings are used to open console input or output.
//

#define CONSOLE_INPUT_STRING  L"CONIN$"
#define CONSOLE_OUTPUT_STRING L"CONOUT$"
#define CONSOLE_GENERIC       L"CON"

// ...

//
// stream API definitions.  these API are only supposed to be used by
// subsystems (i.e. OpenFile routes to OpenConsoleW).
//

_Win32_metadata_import_library_("KERNEL32.dll")
HANDLE
APIENTRY
OpenConsoleW(
    _In_ LPWSTR lpConsoleDevice,
    _In_ DWORD dwDesiredAccess,
    _In_ BOOL bInheritHandle,
    _In_ DWORD dwShareMode)
    _Win32_metadata_raii_free_(CloseConsoleHandle);

_Win32_metadata_import_library_("KERNEL32.dll")
HANDLE
APIENTRY
DuplicateConsoleHandle(
    _In_ HANDLE hSourceHandle,
    _In_ DWORD dwDesiredAccess,
    _In_ BOOL bInheritHandle,
    _In_ DWORD dwOptions)
    _Win32_metadata_raii_free_(CloseConsoleHandle);

// ...

_Win32_metadata_import_library_("KERNEL32.dll")
BOOL
APIENTRY
CloseConsoleHandle(
    _In_ HANDLE hConsole);

_Win32_metadata_import_library_("KERNEL32.dll")
BOOL
APIENTRY
VerifyConsoleIoHandle(
    _In_ HANDLE hIoHandle);

_Win32_metadata_import_library_("KERNEL32.dll")
HANDLE
APIENTRY
GetConsoleInputWaitHandle(
    VOID);

// ...

#ifdef DEFINE_CONSOLEV2_PROPERTIES
#define PID_CONSOLE_FORCEV2                 1
#define PID_CONSOLE_WRAPTEXT                2
#define PID_CONSOLE_FILTERONPASTE           3
#define PID_CONSOLE_CTRLKEYSDISABLED        4
#define PID_CONSOLE_LINESELECTION           5
#define PID_CONSOLE_WINDOWTRANSPARENCY      6
#define PID_CONSOLE_WINDOWMAXIMIZED         7
#define PID_CONSOLE_CURSOR_TYPE             8
#define PID_CONSOLE_CURSOR_COLOR            9
#define PID_CONSOLE_INTERCEPT_COPY_PASTE    10
#define PID_CONSOLE_DEFAULTFOREGROUND       11
#define PID_CONSOLE_DEFAULTBACKGROUND       12
#define PID_CONSOLE_TERMINALSCROLLING       13

#define CONSOLE_PROPKEY(name, id) \
_Win32_metadata_supported_os_(windows6.0.6000)
DEFINE_PROPERTYKEY(name, 0x0C570607, 0x0396, 0x43DE, 0x9D, 0x61, 0xE3, 0x21, 0xD7, 0xDF, 0x50, 0x26, id);

CONSOLE_PROPKEY(PKEY_Console_ForceV2,                   PID_CONSOLE_FORCEV2);
CONSOLE_PROPKEY(PKEY_Console_WrapText,                  PID_CONSOLE_WRAPTEXT);
CONSOLE_PROPKEY(PKEY_Console_FilterOnPaste,             PID_CONSOLE_FILTERONPASTE);
CONSOLE_PROPKEY(PKEY_Console_CtrlKeyShortcutsDisabled,  PID_CONSOLE_CTRLKEYSDISABLED);
CONSOLE_PROPKEY(PKEY_Console_LineSelection,             PID_CONSOLE_LINESELECTION);
CONSOLE_PROPKEY(PKEY_Console_WindowTransparency,        PID_CONSOLE_WINDOWTRANSPARENCY);
CONSOLE_PROPKEY(PKEY_Console_WindowMaximized,           PID_CONSOLE_WINDOWMAXIMIZED);
CONSOLE_PROPKEY(PKEY_Console_CursorType,                PID_CONSOLE_CURSOR_TYPE);
CONSOLE_PROPKEY(PKEY_Console_CursorColor,               PID_CONSOLE_CURSOR_COLOR);
CONSOLE_PROPKEY(PKEY_Console_InterceptCopyPaste,        PID_CONSOLE_INTERCEPT_COPY_PASTE);
CONSOLE_PROPKEY(PKEY_Console_DefaultForeground,         PID_CONSOLE_DEFAULTFOREGROUND);
CONSOLE_PROPKEY(PKEY_Console_DefaultBackground,         PID_CONSOLE_DEFAULTBACKGROUND);
CONSOLE_PROPKEY(PKEY_Console_TerminalScrolling,         PID_CONSOLE_TERMINALSCROLLING);
#endif


//
// Ensure the alignment is WORD boundary
//

#include <pshpack2.h>

typedef struct {
    WORD wMod;
    WORD wVirKey;
    WCHAR wUnicodeChar;
} ExtKeySubst;

typedef struct {
    ExtKeySubst keys[3];    // 0: Ctrl
                            // 1: Alt
                            // 2: Ctrl+Alt
} ExtKeyDef;

typedef ExtKeyDef ExtKeyDefTable['Z' - 'A' + 1];

typedef struct {
    DWORD dwVersion;
    DWORD dwCheckSum;
    ExtKeyDefTable table;
} ExtKeyDefBuf;

//
// Restore the previous alignment
//

#include <poppack.h>


#if _MSC_VER >= 1200
#pragma warning(pop)
#endif

#ifdef __cplusplus
}
#endif

#endif // _WINCONP_
