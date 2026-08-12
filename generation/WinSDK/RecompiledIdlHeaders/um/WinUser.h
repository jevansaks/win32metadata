#include <winapifamily.h>
#if defined(WIN32METADATA)
#include <win32metadata_annotations.h>
#endif

/****************************************************************************
*                                                                           *
* winuser.h -- USER procedure declarations, constant definitions and macros *
*                                                                           *
* Copyright (c) Microsoft Corporation. All rights reserved.                 *
*                                                                           *
****************************************************************************/



#ifndef _WINUSER_
#define _WINUSER_



#pragma once

//
// Define API decoration for direct importing of DLL references.
//
#if !defined(WINUSERAPI)
#if !defined(_USER32_)
#define WINUSERAPI DECLSPEC_IMPORT
#else
#define WINUSERAPI extern "C"
#endif
#endif

#if !defined(WINABLEAPI)
#if !defined(_USER32_)
#define WINABLEAPI DECLSPEC_IMPORT
#else
#define WINABLEAPI
#endif
#endif

#ifdef _MAC
#include <macwin32.h>
#endif

#ifdef __cplusplus
extern "C" {
#endif /* __cplusplus */
#if _MSC_VER >= 1200
#if defined(WIN32METADATA)
enum class ALERT_SYSTEM_SEV : int;
enum class APPCOMMAND_ID : int;
enum class ATF_FLAGS : int;
enum class WNDCLASS_STYLES : int;
enum class CWP_FLAGS : int;
enum class DFC_TYPE : int;
enum class DFCS_STATE : int;
enum class CDS_TYPE : int;
enum class DISP_CHANGE : int;
enum class DRAWSTATE_FLAGS : int;
enum class GESTURECONFIG_ID : int;
enum class GESTURECONFIG_FLAGS : int;
enum class POINTER_FLAGS : int;
enum class REDRAW_WINDOW_FLAGS : int;
enum class SHOW_WINDOW_STATUS : int;
enum class STICKYKEYS_FLAGS : int;
enum class SYSTEM_PARAMETERS_INFO_ACTION : int;
enum class SOUNDSENTRY_FLAGS : int;
enum class TOUCH_FEEDBACK_MODE : int;
enum class TOUCHEVENTF_FLAGS : int;
enum class TOUCHINPUTMASKF_MASK : int;
enum class TRACK_POPUP_MENU_FLAGS : int;
enum class WINDOW_EX_STYLE : int;
enum class WINDOW_STYLE : int;
enum class OBJECT_IDENTIFIER : int;
enum class ENUM_DISPLAY_SETTINGS_MODE : int;
enum class MENU_ITEM_TYPE : int;
enum class RAW_INPUT_DATA_COMMAND_FLAGS : int;
enum class BROADCAST_SYSTEM_MESSAGE_FLAGS : int;
enum class GET_DCX_FLAGS : int;
enum class RAW_INPUT_DEVICE_INFO_COMMAND : int;
enum class UPDATE_LAYERED_WINDOW_FLAGS : int;
enum class ANIMATE_WINDOW_FLAGS : int;
enum class CHANGE_WINDOW_MESSAGE_FILTER_FLAGS : int;
enum class WINDOWS_HOOK_ID : int;
enum class REGISTER_TOUCH_WINDOW_FLAGS : int;
enum class SYSTEM_PARAMETERS_INFO_UPDATE_FLAGS : int;
enum class PRINT_WINDOW_FLAGS : int;
enum class SET_WINDOW_POS_FLAGS : int;
enum class DLG_DIR_LIST_FILE_TYPE : int;
enum class MSG_WAIT_FOR_MULTIPLE_OBJECTS_EX_FLAGS : int;
enum class QUEUE_STATUS_FLAGS : int;
enum class USER_OBJECT_INFORMATION_INDEX : int;
enum class SYSTEM_CURSOR_ID : int;
enum class ENABLE_SCROLL_BAR_ARROWS : unsigned int;
enum class LAYERED_WINDOW_ATTRIBUTES_FLAGS : int;
enum class SEND_MESSAGE_TIMEOUT_FLAGS : int;
enum class PEEK_MESSAGE_REMOVE_TYPE : int;
enum class GET_WINDOW_CMD : int;
enum class SYSTEM_METRICS_INDEX : int;
enum class GET_MOUSE_MOVE_POINTS_EX_RESOLUTION : int;
enum class DLG_BUTTON_CHECK_STATE : int;
enum class KEYBD_EVENT_FLAGS : int;
enum class GET_ANCESTOR_FLAGS : int;
enum class DRAW_CAPTION_FLAGS : int;
enum class TILE_WINDOWS_HOW : int;
enum class WINDOW_DISPLAY_AFFINITY : int;
enum class EXIT_WINDOWS_FLAGS : int;
enum class FOREGROUND_WINDOW_LOCK_CODE : int;
enum class MOUSE_EVENT_FLAGS : int;
enum class CASCADE_WINDOWS_HOW : int;
enum class WINDOW_MESSAGE_FILTER_ACTION : int;
enum class GET_MENU_DEFAULT_ITEM_FLAGS : int;
enum class GET_GUI_RESOURCES_FLAGS : int;
enum class SOUND_SENTRY_GRAPHICS_EFFECT : int;
enum class RID_DEVICE_INFO_TYPE : int;
enum class MSGFLTINFO_STATUS : int;
enum class SERIALKEYS_FLAGS : int;
enum class HIGHCONTRASTW_FLAGS : int;
enum class SOUNDSENTRY_TEXT_EFFECT : int;
enum class MENU_ITEM_MASK : int;
enum class FLASHWINFO_FLAGS : int;
enum class CURSORINFO_FLAGS : int;
enum class RAWINPUTDEVICE_FLAGS : int;
enum class INPUT_TYPE : int;
enum class MENUINFO_STYLE : int;
enum class WINDOWPLACEMENT_FLAGS : int;
enum class MENUINFO_MASK : int;
enum class DRAWITEMSTRUCT_CTL_TYPE : int;
enum class SOUNDSENTRY_WINDOWS_EFFECT : int;
enum class MINIMIZEDMETRICS_ARRANGE : int;
enum class SCROLLINFO_MASK : int;
enum class MENUGETOBJECTINFO_FLAGS : int;
enum class GUITHREADINFO_FLAGS : int;
enum class VIRTUAL_KEY : unsigned short;
enum class KBDLLHOOKSTRUCT_FLAGS : int;
enum class STATIC_STYLES : unsigned int;
enum class ODA_FLAGS : unsigned int;
enum class ODS_FLAGS : unsigned int;
enum class HELP_INFO_TYPE : int;
enum class DESKTOP_CONTROL_FLAGS : unsigned int;
enum class DESKTOP_ACCESS_FLAGS : unsigned int;
enum class SCROLLBAR_COMMAND : int;
enum class MAP_VIRTUAL_KEY_TYPE : int;
enum class ENUM_DISPLAY_SETTINGS_FLAGS : int;
#else
typedef int ALERT_SYSTEM_SEV;
typedef int APPCOMMAND_ID;
typedef int ATF_FLAGS;
typedef UINT WNDCLASS_STYLES;
typedef UINT CWP_FLAGS;
typedef UINT DFC_TYPE;
typedef UINT DFCS_STATE;
typedef DWORD CDS_TYPE;
typedef LONG DISP_CHANGE;
typedef UINT DRAWSTATE_FLAGS;
typedef DWORD GESTURECONFIG_ID;
typedef int GESTURECONFIG_FLAGS;
typedef UINT32 POINTER_FLAGS;
typedef UINT REDRAW_WINDOW_FLAGS;
typedef int SHOW_WINDOW_STATUS;
typedef DWORD STICKYKEYS_FLAGS;
typedef UINT SYSTEM_PARAMETERS_INFO_ACTION;
typedef DWORD SOUNDSENTRY_FLAGS;
typedef DWORD TOUCH_FEEDBACK_MODE;
typedef DWORD TOUCHEVENTF_FLAGS;
typedef DWORD TOUCHINPUTMASKF_MASK;
typedef UINT TRACK_POPUP_MENU_FLAGS;
typedef DWORD WINDOW_EX_STYLE;
typedef DWORD WINDOW_STYLE;
typedef LONG OBJECT_IDENTIFIER;
typedef DWORD ENUM_DISPLAY_SETTINGS_MODE;
typedef UINT MENU_ITEM_TYPE;
typedef UINT RAW_INPUT_DATA_COMMAND_FLAGS;
typedef DWORD BROADCAST_SYSTEM_MESSAGE_FLAGS;
typedef DWORD GET_DCX_FLAGS;
typedef UINT RAW_INPUT_DEVICE_INFO_COMMAND;
typedef DWORD UPDATE_LAYERED_WINDOW_FLAGS;
typedef DWORD ANIMATE_WINDOW_FLAGS;
typedef DWORD CHANGE_WINDOW_MESSAGE_FILTER_FLAGS;
typedef int WINDOWS_HOOK_ID;
typedef ULONG REGISTER_TOUCH_WINDOW_FLAGS;
typedef UINT SYSTEM_PARAMETERS_INFO_UPDATE_FLAGS;
typedef UINT PRINT_WINDOW_FLAGS;
typedef UINT SET_WINDOW_POS_FLAGS;
typedef UINT DLG_DIR_LIST_FILE_TYPE;
typedef DWORD MSG_WAIT_FOR_MULTIPLE_OBJECTS_EX_FLAGS;
typedef UINT QUEUE_STATUS_FLAGS;
typedef int USER_OBJECT_INFORMATION_INDEX;
typedef DWORD SYSTEM_CURSOR_ID;
typedef UINT ENABLE_SCROLL_BAR_ARROWS;
typedef DWORD LAYERED_WINDOW_ATTRIBUTES_FLAGS;
typedef UINT SEND_MESSAGE_TIMEOUT_FLAGS;
typedef UINT PEEK_MESSAGE_REMOVE_TYPE;
typedef UINT GET_WINDOW_CMD;
typedef int SYSTEM_METRICS_INDEX;
typedef DWORD GET_MOUSE_MOVE_POINTS_EX_RESOLUTION;
typedef UINT DLG_BUTTON_CHECK_STATE;
typedef DWORD KEYBD_EVENT_FLAGS;
typedef UINT GET_ANCESTOR_FLAGS;
typedef UINT DRAW_CAPTION_FLAGS;
typedef UINT TILE_WINDOWS_HOW;
typedef DWORD WINDOW_DISPLAY_AFFINITY;
typedef UINT EXIT_WINDOWS_FLAGS;
typedef UINT FOREGROUND_WINDOW_LOCK_CODE;
typedef DWORD MOUSE_EVENT_FLAGS;
typedef UINT CASCADE_WINDOWS_HOW;
typedef DWORD WINDOW_MESSAGE_FILTER_ACTION;
typedef UINT GET_MENU_DEFAULT_ITEM_FLAGS;
typedef DWORD GET_GUI_RESOURCES_FLAGS;
typedef DWORD SOUND_SENTRY_GRAPHICS_EFFECT;
typedef DWORD RID_DEVICE_INFO_TYPE;
typedef DWORD MSGFLTINFO_STATUS;
typedef DWORD SERIALKEYS_FLAGS;
typedef DWORD HIGHCONTRASTW_FLAGS;
typedef DWORD SOUNDSENTRY_TEXT_EFFECT;
typedef UINT MENU_ITEM_MASK;
typedef DWORD FLASHWINFO_FLAGS;
typedef DWORD CURSORINFO_FLAGS;
typedef DWORD RAWINPUTDEVICE_FLAGS;
typedef DWORD INPUT_TYPE;
typedef DWORD MENUINFO_STYLE;
typedef UINT WINDOWPLACEMENT_FLAGS;
typedef DWORD MENUINFO_MASK;
typedef UINT DRAWITEMSTRUCT_CTL_TYPE;
typedef DWORD SOUNDSENTRY_WINDOWS_EFFECT;
typedef int MINIMIZEDMETRICS_ARRANGE;
typedef UINT SCROLLINFO_MASK;
typedef DWORD MENUGETOBJECTINFO_FLAGS;
typedef DWORD GUITHREADINFO_FLAGS;
typedef WORD VIRTUAL_KEY;
typedef DWORD KBDLLHOOKSTRUCT_FLAGS;
typedef unsigned int STATIC_STYLES;
typedef UINT ODA_FLAGS;
typedef UINT ODS_FLAGS;
typedef int HELP_INFO_TYPE;
typedef DWORD DESKTOP_CONTROL_FLAGS;
typedef ACCESS_MASK DESKTOP_ACCESS_FLAGS;
typedef int SCROLLBAR_COMMAND;
typedef UINT MAP_VIRTUAL_KEY_TYPE;
typedef DWORD ENUM_DISPLAY_SETTINGS_FLAGS;
#endif

#pragma warning(push)
#ifndef _MSC_EXTENSIONS
#pragma warning(disable:4309) // truncation of constant value
#endif
#pragma warning(disable:4820) // padding added after data member
#endif

#ifndef WINVER
#define WINVER  0x0500      /* version 5.0 */
#endif /* !WINVER */

#include <stdarg.h>

#ifndef NOAPISET
#include <libloaderapi.h> // LoadString%
#endif

#ifndef NOUSER

#pragma region Desktop Family
#if WINAPI_FAMILY_PARTITION(WINAPI_PARTITION_DESKTOP)

typedef HANDLE HDWP;
typedef VOID MENUTEMPLATEA;
typedef VOID MENUTEMPLATEW;
#ifdef UNICODE
typedef MENUTEMPLATEW MENUTEMPLATE;
#else
typedef MENUTEMPLATEA MENUTEMPLATE;
#endif // UNICODE
typedef PVOID LPMENUTEMPLATEA;
typedef PVOID LPMENUTEMPLATEW;
#ifdef UNICODE
typedef LPMENUTEMPLATEW LPMENUTEMPLATE;
#else
typedef LPMENUTEMPLATEA LPMENUTEMPLATE;
#endif // UNICODE

#endif /* WINAPI_FAMILY_PARTITION(WINAPI_PARTITION_DESKTOP) */
#pragma endregion

#pragma region Desktop Family
#if WINAPI_FAMILY_PARTITION(WINAPI_PARTITION_DESKTOP | WINAPI_PARTITION_GAMES)

typedef LRESULT (CALLBACK* WNDPROC)(HWND, UINT, WPARAM, LPARAM);

#endif /* WINAPI_FAMILY_PARTITION(WINAPI_PARTITION_DESKTOP | WINAPI_PARTITION_GAMES) */
#pragma endregion

#ifdef STRICT

#pragma region Application Family or OneCore Family
#if WINAPI_FAMILY_PARTITION(WINAPI_PARTITION_APP | WINAPI_PARTITION_SYSTEM | WINAPI_PARTITION_GAMES)

typedef INT_PTR (CALLBACK* DLGPROC)(HWND, UINT, WPARAM, LPARAM);

#endif /* WINAPI_FAMILY_PARTITION(WINAPI_PARTITION_APP | WINAPI_PARTITION_SYSTEM | WINAPI_PARTITION_GAMES) */
#pragma endregion

#pragma region Desktop Family or Games Family
#if WINAPI_FAMILY_PARTITION(WINAPI_PARTITION_DESKTOP | WINAPI_PARTITION_GAMES)

typedef VOID (CALLBACK* TIMERPROC)(HWND, UINT, UINT_PTR, DWORD);

#endif /* WINAPI_FAMILY_PARTITION(WINAPI_PARTITION_DESKTOP | WINAPI_PARTITION_GAMES) */
#pragma endregion

#pragma region Desktop Family
#if WINAPI_FAMILY_PARTITION(WINAPI_PARTITION_DESKTOP)

typedef BOOL (CALLBACK* GRAYSTRINGPROC)(HDC, LPARAM, int);
typedef BOOL (CALLBACK* WNDENUMPROC)(HWND, LPARAM);
typedef LRESULT (CALLBACK* HOOKPROC)(int code, WPARAM wParam, LPARAM lParam);
typedef VOID (CALLBACK* SENDASYNCPROC)(HWND, UINT, ULONG_PTR, LRESULT);

typedef BOOL (CALLBACK* PROPENUMPROCA)(HWND, LPCSTR, HANDLE);
typedef BOOL (CALLBACK* PROPENUMPROCW)(HWND, LPCWSTR, HANDLE);

typedef BOOL (CALLBACK* PROPENUMPROCEXA)(HWND, LPSTR, HANDLE, ULONG_PTR);
typedef BOOL (CALLBACK* PROPENUMPROCEXW)(HWND, LPWSTR, HANDLE, ULONG_PTR);

typedef int (CALLBACK* EDITWORDBREAKPROCA)(LPSTR lpch, int ichCurrent, int cch, int code);
typedef int (CALLBACK* EDITWORDBREAKPROCW)(LPWSTR lpch, int ichCurrent, int cch, int code);

#if(WINVER >= 0x0400)
typedef BOOL (CALLBACK* DRAWSTATEPROC)(HDC hdc, LPARAM lData, WPARAM wData, int cx, int cy);
#endif /* WINVER >= 0x0400 */


#endif /* WINAPI_FAMILY_PARTITION(WINAPI_PARTITION_DESKTOP) */
#pragma endregion

#else /* !STRICT */

#pragma region Application Family or OneCore Family
#if WINAPI_FAMILY_PARTITION(WINAPI_PARTITION_APP | WINAPI_PARTITION_SYSTEM)

typedef FARPROC DLGPROC;

#endif /* WINAPI_FAMILY_PARTITION(WINAPI_PARTITION_APP | WINAPI_PARTITION_SYSTEM) */
#pragma endregion

#pragma region Desktop Family or Games Family
#if WINAPI_FAMILY_PARTITION(WINAPI_PARTITION_DESKTOP | WINAPI_PARTITION_GAMES)

typedef FARPROC TIMERPROC;

#endif /* WINAPI_FAMILY_PARTITION(WINAPI_PARTITION_DESKTOP | WINAPI_PARTITION_GAMES) */
#pragma endregion

#pragma region Desktop Family
#if WINAPI_FAMILY_PARTITION(WINAPI_PARTITION_DESKTOP)

typedef FARPROC GRAYSTRINGPROC;
typedef FARPROC WNDENUMPROC;
typedef FARPROC HOOKPROC;
typedef FARPROC SENDASYNCPROC;

typedef FARPROC EDITWORDBREAKPROCA;
typedef FARPROC EDITWORDBREAKPROCW;

typedef FARPROC PROPENUMPROCA;
typedef FARPROC PROPENUMPROCW;

typedef FARPROC PROPENUMPROCEXA;
typedef FARPROC PROPENUMPROCEXW;

#if(WINVER >= 0x0400)
typedef FARPROC DRAWSTATEPROC;
#endif /* WINVER >= 0x0400 */


#endif /* WINAPI_FAMILY_PARTITION(WINAPI_PARTITION_DESKTOP) */
#pragma endregion

#endif /* !STRICT */

#pragma region Desktop Family
#if WINAPI_FAMILY_PARTITION(WINAPI_PARTITION_DESKTOP)

#ifdef UNICODE
typedef PROPENUMPROCW        PROPENUMPROC;
typedef PROPENUMPROCEXW      PROPENUMPROCEX;
typedef EDITWORDBREAKPROCW   EDITWORDBREAKPROC;
#else  /* !UNICODE */
typedef PROPENUMPROCA        PROPENUMPROC;
typedef PROPENUMPROCEXA      PROPENUMPROCEX;
typedef EDITWORDBREAKPROCA   EDITWORDBREAKPROC;
#endif /* UNICODE */

#ifdef STRICT

typedef BOOL (CALLBACK* NAMEENUMPROCA)(LPSTR, LPARAM);
typedef BOOL (CALLBACK* NAMEENUMPROCW)(LPWSTR, LPARAM);

typedef NAMEENUMPROCA   WINSTAENUMPROCA;
typedef NAMEENUMPROCA   DESKTOPENUMPROCA;
typedef NAMEENUMPROCW   WINSTAENUMPROCW;
typedef NAMEENUMPROCW   DESKTOPENUMPROCW;


#else /* !STRICT */

typedef FARPROC NAMEENUMPROCA;
typedef FARPROC NAMEENUMPROCW;
typedef FARPROC WINSTAENUMPROCA;
typedef FARPROC DESKTOPENUMPROCA;
typedef FARPROC WINSTAENUMPROCW;
typedef FARPROC DESKTOPENUMPROCW;


#endif /* !STRICT */

#ifdef UNICODE
typedef WINSTAENUMPROCW     WINSTAENUMPROC;
typedef DESKTOPENUMPROCW    DESKTOPENUMPROC;


#else  /* !UNICODE */
typedef WINSTAENUMPROCA     WINSTAENUMPROC;
typedef DESKTOPENUMPROCA    DESKTOPENUMPROC;

#endif /* UNICODE */

#endif /* WINAPI_FAMILY_PARTITION(WINAPI_PARTITION_DESKTOP) */
#pragma endregion

#define IS_INTRESOURCE(_r) ((((ULONG_PTR)(_r)) >> 16) == 0)
#define MAKEINTRESOURCEA(i) ((LPSTR)((ULONG_PTR)((WORD)(i))))
#define MAKEINTRESOURCEW(i) ((LPWSTR)((ULONG_PTR)((WORD)(i))))
#ifdef UNICODE
#define MAKEINTRESOURCE  MAKEINTRESOURCEW
#else
#define MAKEINTRESOURCE  MAKEINTRESOURCEA
#endif // !UNICODE

#ifndef NORESOURCE

/*
 * Predefined Resource Types
 */
#define RT_CURSOR           MAKEINTRESOURCE(1)
#define RT_BITMAP           MAKEINTRESOURCE(2)
#define RT_ICON             MAKEINTRESOURCE(3)
#define RT_MENU             MAKEINTRESOURCE(4)
#define RT_DIALOG           MAKEINTRESOURCE(5)
#define RT_STRING           MAKEINTRESOURCE(6)
#define RT_FONTDIR          MAKEINTRESOURCE(7)
#define RT_FONT             MAKEINTRESOURCE(8)
#define RT_ACCELERATOR      MAKEINTRESOURCE(9)
#define RT_RCDATA           MAKEINTRESOURCE(10)
#define RT_MESSAGETABLE     MAKEINTRESOURCE(11)

#define DIFFERENCE     11
#define RT_GROUP_CURSOR MAKEINTRESOURCE((ULONG_PTR)(RT_CURSOR) + DIFFERENCE)
#define RT_GROUP_ICON   MAKEINTRESOURCE((ULONG_PTR)(RT_ICON) + DIFFERENCE)
#define RT_VERSION      MAKEINTRESOURCE(16)
#define RT_DLGINCLUDE   MAKEINTRESOURCE(17)
#if(WINVER >= 0x0400)
#define RT_PLUGPLAY     MAKEINTRESOURCE(19)
#define RT_VXD          MAKEINTRESOURCE(20)
#define RT_ANICURSOR    MAKEINTRESOURCE(21)
#define RT_ANIICON      MAKEINTRESOURCE(22)
#endif /* WINVER >= 0x0400 */
#define RT_HTML         MAKEINTRESOURCE(23)
#ifdef RC_INVOKED
#define RT_MANIFEST                        24
#define CREATEPROCESS_MANIFEST_RESOURCE_ID  1
#define ISOLATIONAWARE_MANIFEST_RESOURCE_ID 2
#define ISOLATIONAWARE_NOSTATICIMPORT_MANIFEST_RESOURCE_ID 3
#define ISOLATIONPOLICY_MANIFEST_RESOURCE_ID 4
#define ISOLATIONPOLICY_BROWSER_MANIFEST_RESOURCE_ID 5
#define MINIMUM_RESERVED_MANIFEST_RESOURCE_ID 1   /* inclusive */
#define MAXIMUM_RESERVED_MANIFEST_RESOURCE_ID 16  /* inclusive */
#else  /* RC_INVOKED */
#define RT_MANIFEST                        MAKEINTRESOURCE(24)
#define CREATEPROCESS_MANIFEST_RESOURCE_ID MAKEINTRESOURCE( 1)
#define ISOLATIONAWARE_MANIFEST_RESOURCE_ID MAKEINTRESOURCE(2)
#define ISOLATIONAWARE_NOSTATICIMPORT_MANIFEST_RESOURCE_ID MAKEINTRESOURCE(3)
#define ISOLATIONPOLICY_MANIFEST_RESOURCE_ID MAKEINTRESOURCE(4)
#define ISOLATIONPOLICY_BROWSER_MANIFEST_RESOURCE_ID MAKEINTRESOURCE(5)
#define MINIMUM_RESERVED_MANIFEST_RESOURCE_ID MAKEINTRESOURCE( 1 /*inclusive*/)
#define MAXIMUM_RESERVED_MANIFEST_RESOURCE_ID MAKEINTRESOURCE(16 /*inclusive*/)
#endif /* RC_INVOKED */


#endif /* !NORESOURCE */

#pragma region Desktop Family
#if WINAPI_FAMILY_PARTITION(WINAPI_PARTITION_DESKTOP)

#if defined(DEPRECATE_SUPPORTED)
#pragma warning(push)
#pragma warning(disable:4995)
#endif

_Win32_metadata_set_last_error_
WINUSERAPI
int
WINAPI
wvsprintfA(
    _Out_ LPSTR,
    _In_ _Printf_format_string_ LPCSTR,
    _In_ va_list arglist);
_Win32_metadata_set_last_error_
WINUSERAPI
int
WINAPI
wvsprintfW(
    _Out_ LPWSTR,
    _In_ _Printf_format_string_ LPCWSTR,
    _In_ va_list arglist);
#ifdef UNICODE
#define wvsprintf  wvsprintfW
#else
#define wvsprintf  wvsprintfA
#endif // !UNICODE

_Win32_metadata_set_last_error_
WINUSERAPI
int
WINAPIV
wsprintfA(
    _Out_ LPSTR,
    _In_ _Printf_format_string_ LPCSTR,
    ...);
_Win32_metadata_set_last_error_
WINUSERAPI
int
WINAPIV
wsprintfW(
    _Out_ LPWSTR,
    _In_ _Printf_format_string_ LPCWSTR,
    ...);
#ifdef UNICODE
#define wsprintf  wsprintfW
#else
#define wsprintf  wsprintfA
#endif // !UNICODE

#if defined(DEPRECATE_SUPPORTED)
#pragma warning(pop)
#endif

#endif /* WINAPI_FAMILY_PARTITION(WINAPI_PARTITION_DESKTOP) */
#pragma endregion


/*
 * SPI_SETDESKWALLPAPER defined constants
 */
#define SETWALLPAPER_DEFAULT    ((LPWSTR)-1)

#ifndef NOSCROLL

/*
 * Scroll Bar Constants
 */
#define SB_MIN              0
#define SB_HORZ             0
#define SB_VERT             1
#define SB_CTL              2
#define SB_BOTH             3

/*
 * Scroll Bar Commands
 */
#define SB_LINEUP           0
#define SB_LINELEFT         0
#define SB_LINEDOWN         1
#define SB_LINERIGHT        1
#define SB_PAGEUP           2
#define SB_PAGELEFT         2
#define SB_PAGEDOWN         3
#define SB_PAGERIGHT        3
#define SB_THUMBPOSITION    4
#define SB_THUMBTRACK       5
#define SB_TOP              6
#define SB_LEFT             6
#define SB_BOTTOM           7
#define SB_RIGHT            7
#define SB_ENDSCROLL        8

#endif /* !NOSCROLL */

#ifndef NOSHOWWINDOW


/*
 * ShowWindow() Commands
 */
#define SW_HIDE             0
#define SW_SHOWNORMAL       1
#define SW_NORMAL           1
#define SW_SHOWMINIMIZED    2
#define SW_SHOWMAXIMIZED    3
#define SW_MAXIMIZE         3
#define SW_SHOWNOACTIVATE   4
#define SW_SHOW             5
#define SW_MINIMIZE         6
#define SW_SHOWMINNOACTIVE  7
#define SW_SHOWNA           8
#define SW_RESTORE          9
#define SW_SHOWDEFAULT      10
#define SW_FORCEMINIMIZE    11
#define SW_MAX              11


/*
 * Old ShowWindow() Commands
 */
#define HIDE_WINDOW         0
#define SHOW_OPENWINDOW     1
#define SHOW_ICONWINDOW     2
#define SHOW_FULLSCREEN     3
#define SHOW_OPENNOACTIVATE 4

/*
 * Identifiers for the WM_SHOWWINDOW message
 */
#define SW_PARENTCLOSING    1
#define SW_OTHERZOOM        2
#define SW_PARENTOPENING    3
#define SW_OTHERUNZOOM      4


#endif /* !NOSHOWWINDOW */

#if(WINVER >= 0x0500)
/*
 * AnimateWindow() Commands
 */
#define AW_HOR_POSITIVE             0x00000001
#define AW_HOR_NEGATIVE             0x00000002
#define AW_VER_POSITIVE             0x00000004
#define AW_VER_NEGATIVE             0x00000008
#define AW_CENTER                   0x00000010
#define AW_HIDE                     0x00010000
#define AW_ACTIVATE                 0x00020000
#define AW_SLIDE                    0x00040000
#define AW_BLEND                    0x00080000

#endif /* WINVER >= 0x0500 */


/*
 * WM_KEYUP/DOWN/CHAR HIWORD(lParam) flags
 */
#define KF_EXTENDED       0x0100
#define KF_DLGMODE        0x0800
#define KF_MENUMODE       0x1000
#define KF_ALTDOWN        0x2000
#define KF_REPEAT         0x4000
#define KF_UP             0x8000

#ifndef NOVIRTUALKEYCODES


/*
 * Virtual Keys, Standard Set
 */
#define VK_LBUTTON        0x01
#define VK_RBUTTON        0x02
#define VK_CANCEL         0x03
#define VK_MBUTTON        0x04    /* NOT contiguous with L & RBUTTON */

#if(_WIN32_WINNT >= 0x0500)
#define VK_XBUTTON1       0x05    /* NOT contiguous with L & RBUTTON */
#define VK_XBUTTON2       0x06    /* NOT contiguous with L & RBUTTON */
#endif /* _WIN32_WINNT >= 0x0500 */

/*
 * 0x07 : reserved
 */


#define VK_BACK           0x08
#define VK_TAB            0x09

/*
 * 0x0A - 0x0B : reserved
 */

#define VK_CLEAR          0x0C
#define VK_RETURN         0x0D

/*
 * 0x0E - 0x0F : unassigned
 */

#define VK_SHIFT          0x10
#define VK_CONTROL        0x11
#define VK_MENU           0x12
#define VK_PAUSE          0x13
#define VK_CAPITAL        0x14

#define VK_KANA           0x15
#define VK_HANGEUL        0x15  /* old name - should be here for compatibility */
#define VK_HANGUL         0x15
#define VK_IME_ON         0x16
#define VK_JUNJA          0x17
#define VK_FINAL          0x18
#define VK_HANJA          0x19
#define VK_KANJI          0x19
#define VK_IME_OFF        0x1A

#define VK_ESCAPE         0x1B

#define VK_CONVERT        0x1C
#define VK_NONCONVERT     0x1D
#define VK_ACCEPT         0x1E
#define VK_MODECHANGE     0x1F

#define VK_SPACE          0x20
#define VK_PRIOR          0x21
#define VK_NEXT           0x22
#define VK_END            0x23
#define VK_HOME           0x24
#define VK_LEFT           0x25
#define VK_UP             0x26
#define VK_RIGHT          0x27
#define VK_DOWN           0x28
#define VK_SELECT         0x29
#define VK_PRINT          0x2A
#define VK_EXECUTE        0x2B
#define VK_SNAPSHOT       0x2C
#define VK_INSERT         0x2D
#define VK_DELETE         0x2E
#define VK_HELP           0x2F

/*
 * VK_0 - VK_9 are the same as ASCII '0' - '9' (0x30 - 0x39)
 * 0x3A - 0x40 : unassigned
 * VK_A - VK_Z are the same as ASCII 'A' - 'Z' (0x41 - 0x5A)
 */

#define VK_LWIN           0x5B
#define VK_RWIN           0x5C
#define VK_APPS           0x5D

/*
 * 0x5E : reserved
 */

#define VK_SLEEP          0x5F

#define VK_NUMPAD0        0x60
#define VK_NUMPAD1        0x61
#define VK_NUMPAD2        0x62
#define VK_NUMPAD3        0x63
#define VK_NUMPAD4        0x64
#define VK_NUMPAD5        0x65
#define VK_NUMPAD6        0x66
#define VK_NUMPAD7        0x67
#define VK_NUMPAD8        0x68
#define VK_NUMPAD9        0x69
#define VK_MULTIPLY       0x6A
#define VK_ADD            0x6B
#define VK_SEPARATOR      0x6C
#define VK_SUBTRACT       0x6D
#define VK_DECIMAL        0x6E
#define VK_DIVIDE         0x6F
#define VK_F1             0x70
#define VK_F2             0x71
#define VK_F3             0x72
#define VK_F4             0x73
#define VK_F5             0x74
#define VK_F6             0x75
#define VK_F7             0x76
#define VK_F8             0x77
#define VK_F9             0x78
#define VK_F10            0x79
#define VK_F11            0x7A
#define VK_F12            0x7B
#define VK_F13            0x7C
#define VK_F14            0x7D
#define VK_F15            0x7E
#define VK_F16            0x7F
#define VK_F17            0x80
#define VK_F18            0x81
#define VK_F19            0x82
#define VK_F20            0x83
#define VK_F21            0x84
#define VK_F22            0x85
#define VK_F23            0x86
#define VK_F24            0x87

#if(_WIN32_WINNT >= 0x0604)

/*
 * 0x88 - 0x8F : UI navigation
 */

#define VK_NAVIGATION_VIEW     0x88 // reserved
#define VK_NAVIGATION_MENU     0x89 // reserved
#define VK_NAVIGATION_UP       0x8A // reserved
#define VK_NAVIGATION_DOWN     0x8B // reserved
#define VK_NAVIGATION_LEFT     0x8C // reserved
#define VK_NAVIGATION_RIGHT    0x8D // reserved
#define VK_NAVIGATION_ACCEPT   0x8E // reserved
#define VK_NAVIGATION_CANCEL   0x8F // reserved

#endif /* _WIN32_WINNT >= 0x0604 */

#define VK_NUMLOCK        0x90
#define VK_SCROLL         0x91

/*
 * NEC PC-9800 kbd definitions
 */
#define VK_OEM_NEC_EQUAL  0x92   // '=' key on numpad

/*
 * Fujitsu/OASYS kbd definitions
 */
#define VK_OEM_FJ_JISHO   0x92   // 'Dictionary' key
#define VK_OEM_FJ_MASSHOU 0x93   // 'Unregister word' key
#define VK_OEM_FJ_TOUROKU 0x94   // 'Register word' key
#define VK_OEM_FJ_LOYA    0x95   // 'Left OYAYUBI' key
#define VK_OEM_FJ_ROYA    0x96   // 'Right OYAYUBI' key

/*
 * 0x97 - 0x9F : unassigned
 */

/*
 * VK_L* & VK_R* - left and right Alt, Ctrl and Shift virtual keys.
 * Used only as parameters to GetAsyncKeyState() and GetKeyState().
 * No other API or message will distinguish left and right keys in this way.
 */
#define VK_LSHIFT         0xA0
#define VK_RSHIFT         0xA1
#define VK_LCONTROL       0xA2
#define VK_RCONTROL       0xA3
#define VK_LMENU          0xA4
#define VK_RMENU          0xA5

#if(_WIN32_WINNT >= 0x0500)
#define VK_BROWSER_BACK        0xA6
#define VK_BROWSER_FORWARD     0xA7
#define VK_BROWSER_REFRESH     0xA8
#define VK_BROWSER_STOP        0xA9
#define VK_BROWSER_SEARCH      0xAA
#define VK_BROWSER_FAVORITES   0xAB
#define VK_BROWSER_HOME        0xAC

#define VK_VOLUME_MUTE         0xAD
#define VK_VOLUME_DOWN         0xAE
#define VK_VOLUME_UP           0xAF
#define VK_MEDIA_NEXT_TRACK    0xB0
#define VK_MEDIA_PREV_TRACK    0xB1
#define VK_MEDIA_STOP          0xB2
#define VK_MEDIA_PLAY_PAUSE    0xB3
#define VK_LAUNCH_MAIL         0xB4
#define VK_LAUNCH_MEDIA_SELECT 0xB5
#define VK_LAUNCH_APP1         0xB6
#define VK_LAUNCH_APP2         0xB7

#endif /* _WIN32_WINNT >= 0x0500 */

/*
 * 0xB8 - 0xB9 : reserved
 */

#define VK_OEM_1          0xBA   // ';:' for US
#define VK_OEM_PLUS       0xBB   // '+' any country
#define VK_OEM_COMMA      0xBC   // ',' any country
#define VK_OEM_MINUS      0xBD   // '-' any country
#define VK_OEM_PERIOD     0xBE   // '.' any country
#define VK_OEM_2          0xBF   // '/?' for US
#define VK_OEM_3          0xC0   // '`~' for US

/*
 * 0xC1 - 0xC2 : reserved
 */

#if(_WIN32_WINNT >= 0x0604)

/*
 * 0xC3 - 0xDA : Gamepad input
 */

#define VK_GAMEPAD_A                         0xC3 // reserved
#define VK_GAMEPAD_B                         0xC4 // reserved
#define VK_GAMEPAD_X                         0xC5 // reserved
#define VK_GAMEPAD_Y                         0xC6 // reserved
#define VK_GAMEPAD_RIGHT_SHOULDER            0xC7 // reserved
#define VK_GAMEPAD_LEFT_SHOULDER             0xC8 // reserved
#define VK_GAMEPAD_LEFT_TRIGGER              0xC9 // reserved
#define VK_GAMEPAD_RIGHT_TRIGGER             0xCA // reserved
#define VK_GAMEPAD_DPAD_UP                   0xCB // reserved
#define VK_GAMEPAD_DPAD_DOWN                 0xCC // reserved
#define VK_GAMEPAD_DPAD_LEFT                 0xCD // reserved
#define VK_GAMEPAD_DPAD_RIGHT                0xCE // reserved
#define VK_GAMEPAD_MENU                      0xCF // reserved
#define VK_GAMEPAD_VIEW                      0xD0 // reserved
#define VK_GAMEPAD_LEFT_THUMBSTICK_BUTTON    0xD1 // reserved
#define VK_GAMEPAD_RIGHT_THUMBSTICK_BUTTON   0xD2 // reserved
#define VK_GAMEPAD_LEFT_THUMBSTICK_UP        0xD3 // reserved
#define VK_GAMEPAD_LEFT_THUMBSTICK_DOWN      0xD4 // reserved
#define VK_GAMEPAD_LEFT_THUMBSTICK_RIGHT     0xD5 // reserved
#define VK_GAMEPAD_LEFT_THUMBSTICK_LEFT      0xD6 // reserved
#define VK_GAMEPAD_RIGHT_THUMBSTICK_UP       0xD7 // reserved
#define VK_GAMEPAD_RIGHT_THUMBSTICK_DOWN     0xD8 // reserved
#define VK_GAMEPAD_RIGHT_THUMBSTICK_RIGHT    0xD9 // reserved
#define VK_GAMEPAD_RIGHT_THUMBSTICK_LEFT     0xDA // reserved

#endif /* _WIN32_WINNT >= 0x0604 */


#define VK_OEM_4          0xDB  //  '[{' for US
#define VK_OEM_5          0xDC  //  '\|' for US
#define VK_OEM_6          0xDD  //  ']}' for US
#define VK_OEM_7          0xDE  //  ''"' for US
#define VK_OEM_8          0xDF

/*
 * 0xE0 : reserved
 */

/*
 * Various extended or enhanced keyboards
 */
#define VK_OEM_AX         0xE1  //  'AX' key on Japanese AX kbd
#define VK_OEM_102        0xE2  //  "<>" or "\|" on RT 102-key kbd.
#define VK_ICO_HELP       0xE3  //  Help key on ICO
#define VK_ICO_00         0xE4  //  00 key on ICO

#if(WINVER >= 0x0400)
#define VK_PROCESSKEY     0xE5
#endif /* WINVER >= 0x0400 */

#define VK_ICO_CLEAR      0xE6


#if(_WIN32_WINNT >= 0x0500)
#define VK_PACKET         0xE7
#endif /* _WIN32_WINNT >= 0x0500 */

/*
 * 0xE8 : unassigned
 */

/*
 * Nokia/Ericsson definitions
 */
#define VK_OEM_RESET      0xE9
#define VK_OEM_JUMP       0xEA
#define VK_OEM_PA1        0xEB
#define VK_OEM_PA2        0xEC
#define VK_OEM_PA3        0xED
#define VK_OEM_WSCTRL     0xEE
#define VK_OEM_CUSEL      0xEF
#define VK_OEM_ATTN       0xF0
#define VK_OEM_FINISH     0xF1
#define VK_OEM_COPY       0xF2
#define VK_OEM_AUTO       0xF3
#define VK_OEM_ENLW       0xF4
#define VK_OEM_BACKTAB    0xF5

#define VK_ATTN           0xF6
#define VK_CRSEL          0xF7
#define VK_EXSEL          0xF8
#define VK_EREOF          0xF9
#define VK_PLAY           0xFA
#define VK_ZOOM           0xFB
#define VK_NONAME         0xFC
#define VK_PA1            0xFD
#define VK_OEM_CLEAR      0xFE

/*
 * 0xFF : reserved
 */


#endif /* !NOVIRTUALKEYCODES */

#ifndef NOWH

/*
 * SetWindowsHook() codes
 */
#define WH_MIN              (-1)
#define WH_MSGFILTER        (-1)
#define WH_JOURNALRECORD    0   // OBSOLETE: discontinued
#define WH_JOURNALPLAYBACK  1   // OBSOLETE: discontinued
#define WH_KEYBOARD         2
#define WH_GETMESSAGE       3
#define WH_CALLWNDPROC      4
#define WH_CBT              5
#define WH_SYSMSGFILTER     6
#define WH_MOUSE            7
#if defined(_WIN32_WINDOWS)
#define WH_HARDWARE         8
#endif
#define WH_DEBUG            9
#define WH_SHELL           10
#define WH_FOREGROUNDIDLE  11
#if(WINVER >= 0x0400)
#define WH_CALLWNDPROCRET  12
#endif /* WINVER >= 0x0400 */

#if (_WIN32_WINNT >= 0x0400)
#define WH_KEYBOARD_LL     13
#define WH_MOUSE_LL        14
#endif // (_WIN32_WINNT >= 0x0400)

#if(WINVER >= 0x0400)
#if (_WIN32_WINNT >= 0x0400)
#define WH_MAX             14
#else
#define WH_MAX             12
#endif // (_WIN32_WINNT >= 0x0400)
#else
#define WH_MAX             11
#endif

#define WH_MINHOOK         WH_MIN
#define WH_MAXHOOK         WH_MAX

/*
 * Hook Codes
 */
#define HC_ACTION           0
#define HC_GETNEXT          1
#define HC_SKIP             2
#define HC_NOREMOVE         3
#define HC_NOREM            HC_NOREMOVE
#define HC_SYSMODALON       4
#define HC_SYSMODALOFF      5

/*
 * CBT Hook Codes
 */
#define HCBT_MOVESIZE       0
#define HCBT_MINMAX         1
#define HCBT_QS             2
#define HCBT_CREATEWND      3
#define HCBT_DESTROYWND     4
#define HCBT_ACTIVATE       5
#define HCBT_CLICKSKIPPED   6
#define HCBT_KEYSKIPPED     7
#define HCBT_SYSCOMMAND     8
#define HCBT_SETFOCUS       9

#pragma region Desktop Family
#if WINAPI_FAMILY_PARTITION(WINAPI_PARTITION_DESKTOP)

/*
 * HCBT_CREATEWND parameters pointed to by lParam
 */
typedef struct tagCBT_CREATEWNDA
{
    struct tagCREATESTRUCTA *lpcs;
    HWND           hwndInsertAfter;
} CBT_CREATEWNDA, *LPCBT_CREATEWNDA;
/*
 * HCBT_CREATEWND parameters pointed to by lParam
 */
typedef struct tagCBT_CREATEWNDW
{
    struct tagCREATESTRUCTW *lpcs;
    HWND           hwndInsertAfter;
} CBT_CREATEWNDW, *LPCBT_CREATEWNDW;
#ifdef UNICODE
typedef CBT_CREATEWNDW CBT_CREATEWND;
typedef LPCBT_CREATEWNDW LPCBT_CREATEWND;
#else
typedef CBT_CREATEWNDA CBT_CREATEWND;
typedef LPCBT_CREATEWNDA LPCBT_CREATEWND;
#endif // UNICODE

/*
 * HCBT_ACTIVATE structure pointed to by lParam
 */
typedef struct tagCBTACTIVATESTRUCT
{
    BOOL    fMouse;
    HWND    hWndActive;
} CBTACTIVATESTRUCT, *LPCBTACTIVATESTRUCT;

#endif /* WINAPI_FAMILY_PARTITION(WINAPI_PARTITION_DESKTOP) */
#pragma endregion

#if(_WIN32_WINNT >= 0x0501)

#pragma region Desktop Family
#if WINAPI_FAMILY_PARTITION(WINAPI_PARTITION_DESKTOP)

/*
 * WTSSESSION_NOTIFICATION struct pointed by lParam, for WM_WTSSESSION_CHANGE
 */
typedef struct tagWTSSESSION_NOTIFICATION
{
    DWORD cbSize;
    DWORD dwSessionId;

} WTSSESSION_NOTIFICATION, *PWTSSESSION_NOTIFICATION;

#endif /* WINAPI_FAMILY_PARTITION(WINAPI_PARTITION_DESKTOP) */
#pragma endregion

/*
 * codes passed in WPARAM for WM_WTSSESSION_CHANGE
 */

#define WTS_CONSOLE_CONNECT                0x1
#define WTS_CONSOLE_DISCONNECT             0x2
#define WTS_REMOTE_CONNECT                 0x3
#define WTS_REMOTE_DISCONNECT              0x4
#define WTS_SESSION_LOGON                  0x5
#define WTS_SESSION_LOGOFF                 0x6
#define WTS_SESSION_LOCK                   0x7
#define WTS_SESSION_UNLOCK                 0x8
#define WTS_SESSION_REMOTE_CONTROL         0x9
#define WTS_SESSION_CREATE                 0xa
#define WTS_SESSION_TERMINATE              0xb
#define WTS_SESSION_DESKTOP_READY          0xf

#endif /* _WIN32_WINNT >= 0x0501 */

/*
 * WH_MSGFILTER Filter Proc Codes
 */
#define MSGF_DIALOGBOX      0
#define MSGF_MESSAGEBOX     1
#define MSGF_MENU           2
#define MSGF_SCROLLBAR      5
#define MSGF_NEXTWINDOW     6
#define MSGF_MAX            8                       // unused
#define MSGF_USER           4096

/*
 * Shell support
 */
#define HSHELL_WINDOWCREATED        1
#define HSHELL_WINDOWDESTROYED      2
#define HSHELL_ACTIVATESHELLWINDOW  3

#if(WINVER >= 0x0400)
#define HSHELL_WINDOWACTIVATED      4
#define HSHELL_GETMINRECT           5
#define HSHELL_REDRAW               6
#define HSHELL_TASKMAN              7
#define HSHELL_LANGUAGE             8
#define HSHELL_SYSMENU              9
#define HSHELL_ENDTASK              10
#endif /* WINVER >= 0x0400 */
#if(_WIN32_WINNT >= 0x0500)
#define HSHELL_ACCESSIBILITYSTATE   11
#define HSHELL_APPCOMMAND           12
#endif /* _WIN32_WINNT >= 0x0500 */

#if(_WIN32_WINNT >= 0x0501)
#define HSHELL_WINDOWREPLACED       13
#define HSHELL_WINDOWREPLACING      14
#endif /* _WIN32_WINNT >= 0x0501 */


#if(_WIN32_WINNT >= 0x0602)
#define HSHELL_MONITORCHANGED            16

// Deprecated by Feature_NoMoreSwpIamNotifyPosChanged
#if (NTDDI_VERSION >= NTDDI_WIN10_RS3)
#endif // NTDDI_VERSION >= NTDDI_WIN10_RS3

#endif /* _WIN32_WINNT >= 0x0602 */


#define HSHELL_HIGHBIT            0x8000
#define HSHELL_FLASH              (HSHELL_REDRAW|HSHELL_HIGHBIT)
#define HSHELL_RUDEAPPACTIVATED   (HSHELL_WINDOWACTIVATED|HSHELL_HIGHBIT)

#if(_WIN32_WINNT >= 0x0500)
/* cmd for HSHELL_APPCOMMAND and WM_APPCOMMAND */
#define APPCOMMAND_BROWSER_BACKWARD       1
#define APPCOMMAND_BROWSER_FORWARD        2
#define APPCOMMAND_BROWSER_REFRESH        3
#define APPCOMMAND_BROWSER_STOP           4
#define APPCOMMAND_BROWSER_SEARCH         5
#define APPCOMMAND_BROWSER_FAVORITES      6
#define APPCOMMAND_BROWSER_HOME           7
#define APPCOMMAND_VOLUME_MUTE            8
#define APPCOMMAND_VOLUME_DOWN            9
#define APPCOMMAND_VOLUME_UP              10
#define APPCOMMAND_MEDIA_NEXTTRACK        11
#define APPCOMMAND_MEDIA_PREVIOUSTRACK    12
#define APPCOMMAND_MEDIA_STOP             13
#define APPCOMMAND_MEDIA_PLAY_PAUSE       14
#define APPCOMMAND_LAUNCH_MAIL            15
#define APPCOMMAND_LAUNCH_MEDIA_SELECT    16
#define APPCOMMAND_LAUNCH_APP1            17
#define APPCOMMAND_LAUNCH_APP2            18
#define APPCOMMAND_BASS_DOWN              19
#define APPCOMMAND_BASS_BOOST             20
#define APPCOMMAND_BASS_UP                21
#define APPCOMMAND_TREBLE_DOWN            22
#define APPCOMMAND_TREBLE_UP              23
#if(_WIN32_WINNT >= 0x0501)
#define APPCOMMAND_MICROPHONE_VOLUME_MUTE 24
#define APPCOMMAND_MICROPHONE_VOLUME_DOWN 25
#define APPCOMMAND_MICROPHONE_VOLUME_UP   26
#define APPCOMMAND_HELP                   27
#define APPCOMMAND_FIND                   28
#define APPCOMMAND_NEW                    29
#define APPCOMMAND_OPEN                   30
#define APPCOMMAND_CLOSE                  31
#define APPCOMMAND_SAVE                   32
#define APPCOMMAND_PRINT                  33
#define APPCOMMAND_UNDO                   34
#define APPCOMMAND_REDO                   35
#define APPCOMMAND_COPY                   36
#define APPCOMMAND_CUT                    37
#define APPCOMMAND_PASTE                  38
#define APPCOMMAND_REPLY_TO_MAIL          39
#define APPCOMMAND_FORWARD_MAIL           40
#define APPCOMMAND_SEND_MAIL              41
#define APPCOMMAND_SPELL_CHECK            42
#define APPCOMMAND_DICTATE_OR_COMMAND_CONTROL_TOGGLE    43
#define APPCOMMAND_MIC_ON_OFF_TOGGLE      44
#define APPCOMMAND_CORRECTION_LIST        45
#define APPCOMMAND_MEDIA_PLAY             46
#define APPCOMMAND_MEDIA_PAUSE            47
#define APPCOMMAND_MEDIA_RECORD           48
#define APPCOMMAND_MEDIA_FAST_FORWARD     49
#define APPCOMMAND_MEDIA_REWIND           50
#define APPCOMMAND_MEDIA_CHANNEL_UP       51
#define APPCOMMAND_MEDIA_CHANNEL_DOWN     52
#endif /* _WIN32_WINNT >= 0x0501 */
#if(_WIN32_WINNT >= 0x0600)
#define APPCOMMAND_DELETE                 53
#define APPCOMMAND_DWM_FLIP3D             54
#endif /* _WIN32_WINNT >= 0x0600 */

#define FAPPCOMMAND_MOUSE 0x8000
#define FAPPCOMMAND_KEY   0
#define FAPPCOMMAND_OEM   0x1000
#define FAPPCOMMAND_MASK  0xF000

#define GET_APPCOMMAND_LPARAM(lParam) ((short)(HIWORD(lParam) & ~FAPPCOMMAND_MASK))
#define GET_DEVICE_LPARAM(lParam)     ((WORD)(HIWORD(lParam) & FAPPCOMMAND_MASK))
#define GET_MOUSEORKEY_LPARAM         GET_DEVICE_LPARAM
#define GET_FLAGS_LPARAM(lParam)      (LOWORD(lParam))
#define GET_KEYSTATE_LPARAM(lParam)   GET_FLAGS_LPARAM(lParam)
#endif /* _WIN32_WINNT >= 0x0500 */

#pragma region Desktop Family
#if WINAPI_FAMILY_PARTITION(WINAPI_PARTITION_DESKTOP)

typedef struct
{
    HWND    hwnd;
    RECT    rc;
} SHELLHOOKINFO, *LPSHELLHOOKINFO;

/*
 * Message Structure used in Journaling
 */
typedef struct tagEVENTMSG {
    UINT    message;
    UINT    paramL;
    UINT    paramH;
    DWORD    time;
    HWND     hwnd;
} EVENTMSG, *PEVENTMSGMSG, NEAR *NPEVENTMSGMSG, FAR *LPEVENTMSGMSG;

typedef struct tagEVENTMSG *PEVENTMSG, NEAR *NPEVENTMSG, FAR *LPEVENTMSG;

/*
 * Message structure used by WH_CALLWNDPROC
 */
typedef struct tagCWPSTRUCT {
    LPARAM  lParam;
    WPARAM  wParam;
    UINT    message;
    HWND    hwnd;
} CWPSTRUCT, *PCWPSTRUCT, NEAR *NPCWPSTRUCT, FAR *LPCWPSTRUCT;

#if(WINVER >= 0x0400)
/*
 * Message structure used by WH_CALLWNDPROCRET
 */
typedef struct tagCWPRETSTRUCT {
    LRESULT lResult;
    LPARAM  lParam;
    WPARAM  wParam;
    UINT    message;
    HWND    hwnd;
} CWPRETSTRUCT, *PCWPRETSTRUCT, NEAR *NPCWPRETSTRUCT, FAR *LPCWPRETSTRUCT;

#endif /* WINVER >= 0x0400 */

#endif /* WINAPI_FAMILY_PARTITION(WINAPI_PARTITION_DESKTOP) */
#pragma endregion

#if (_WIN32_WINNT >= 0x0400)

/*
 * Low level hook flags
 */

#define LLKHF_EXTENDED       (KF_EXTENDED >> 8) /* 0x00000001 */
#define LLKHF_INJECTED       0x00000010
#define LLKHF_ALTDOWN        (KF_ALTDOWN >> 8) /* 0x00000020 */
#define LLKHF_UP             (KF_UP >> 8)      /* 0x00000080 */
#define LLKHF_LOWER_IL_INJECTED        0x00000002

#define LLMHF_INJECTED       0x00000001
#define LLMHF_LOWER_IL_INJECTED        0x00000002

#pragma region Desktop Family
#if WINAPI_FAMILY_PARTITION(WINAPI_PARTITION_DESKTOP)

/*
 * Structure used by WH_KEYBOARD_LL
 */
typedef struct tagKBDLLHOOKSTRUCT {
    DWORD   vkCode;
    DWORD   scanCode;
    KBDLLHOOKSTRUCT_FLAGS   flags;
    DWORD   time;
    ULONG_PTR dwExtraInfo;
} KBDLLHOOKSTRUCT, FAR *LPKBDLLHOOKSTRUCT, *PKBDLLHOOKSTRUCT;

/*
 * Structure used by WH_MOUSE_LL
 */
typedef struct tagMSLLHOOKSTRUCT {
    POINT   pt;
    DWORD   mouseData;
    DWORD   flags;
    DWORD   time;
    ULONG_PTR dwExtraInfo;
} MSLLHOOKSTRUCT, FAR *LPMSLLHOOKSTRUCT, *PMSLLHOOKSTRUCT;

#endif /* WINAPI_FAMILY_PARTITION(WINAPI_PARTITION_DESKTOP) */
#pragma endregion

#endif // (_WIN32_WINNT >= 0x0400)

#pragma region Desktop Family
#if WINAPI_FAMILY_PARTITION(WINAPI_PARTITION_DESKTOP)

/*
 * Structure used by WH_DEBUG
 */
typedef struct tagDEBUGHOOKINFO
{
    DWORD   idThread;
    DWORD   idThreadInstaller;
    LPARAM  lParam;
    WPARAM  wParam;
    int     code;
} DEBUGHOOKINFO, *PDEBUGHOOKINFO, NEAR *NPDEBUGHOOKINFO, FAR* LPDEBUGHOOKINFO;

/*
 * Structure used by WH_MOUSE
 */
typedef struct tagMOUSEHOOKSTRUCT {
    POINT   pt;
    HWND    hwnd;
    UINT    wHitTestCode;
    ULONG_PTR dwExtraInfo;
} MOUSEHOOKSTRUCT, FAR *LPMOUSEHOOKSTRUCT, *PMOUSEHOOKSTRUCT;

#if(_WIN32_WINNT >= 0x0500)
#ifdef __cplusplus
typedef struct tagMOUSEHOOKSTRUCTEX : public tagMOUSEHOOKSTRUCT
{
    DWORD   mouseData;
} MOUSEHOOKSTRUCTEX, *LPMOUSEHOOKSTRUCTEX, *PMOUSEHOOKSTRUCTEX;
#else // ndef __cplusplus
typedef struct tagMOUSEHOOKSTRUCTEX
{
    MOUSEHOOKSTRUCT DUMMYSTRUCTNAME;
    DWORD   mouseData;
} MOUSEHOOKSTRUCTEX, *LPMOUSEHOOKSTRUCTEX, *PMOUSEHOOKSTRUCTEX;
#endif
#endif /* _WIN32_WINNT >= 0x0500 */

#if(WINVER >= 0x0400)
/*
 * Structure used by WH_HARDWARE
 */
typedef struct tagHARDWAREHOOKSTRUCT {
    HWND    hwnd;
    UINT    message;
    WPARAM  wParam;
    LPARAM  lParam;
} HARDWAREHOOKSTRUCT, FAR *LPHARDWAREHOOKSTRUCT, *PHARDWAREHOOKSTRUCT;
#endif /* WINVER >= 0x0400 */

#endif /* WINAPI_FAMILY_PARTITION(WINAPI_PARTITION_DESKTOP) */
#pragma endregion

#endif /* !NOWH */

/*
 * Keyboard Layout API
 */
#define HKL_PREV            0
#define HKL_NEXT            1


#define KLF_ACTIVATE        0x00000001
#define KLF_SUBSTITUTE_OK   0x00000002
#define KLF_REORDER         0x00000008
#if(WINVER >= 0x0400)
#define KLF_REPLACELANG     0x00000010
#define KLF_NOTELLSHELL     0x00000080
#endif /* WINVER >= 0x0400 */
#define KLF_SETFORPROCESS   0x00000100
#if(_WIN32_WINNT >= 0x0500)
#define KLF_SHIFTLOCK       0x00010000
#define KLF_RESET           0x40000000
#endif /* _WIN32_WINNT >= 0x0500 */


#if(WINVER >= 0x0500)
/*
 * Bits in wParam of WM_INPUTLANGCHANGEREQUEST message
 */
#define INPUTLANGCHANGE_SYSCHARSET 0x0001
#define INPUTLANGCHANGE_FORWARD    0x0002
#define INPUTLANGCHANGE_BACKWARD   0x0004
#endif /* WINVER >= 0x0500 */

/*
 * Size of KeyboardLayoutName (number of characters), including nul terminator
 */
#define KL_NAMELENGTH 9

#pragma region Desktop Family
#if WINAPI_FAMILY_PARTITION(WINAPI_PARTITION_DESKTOP)

_Win32_metadata_set_last_error_
WINUSERAPI
HKL
WINAPI
LoadKeyboardLayoutA(
    _In_ LPCSTR pwszKLID,
    _In_ UINT Flags);
_Win32_metadata_set_last_error_
WINUSERAPI
HKL
WINAPI
LoadKeyboardLayoutW(
    _In_ LPCWSTR pwszKLID,
    _In_ UINT Flags);
#ifdef UNICODE
#define LoadKeyboardLayout  LoadKeyboardLayoutW
#else
#define LoadKeyboardLayout  LoadKeyboardLayoutA
#endif // !UNICODE


#if(WINVER >= 0x0400)
_Win32_metadata_set_last_error_
WINUSERAPI
HKL
WINAPI
ActivateKeyboardLayout(
    _In_ HKL hkl,
    _In_ UINT Flags);
#else
_Win32_metadata_set_last_error_
WINUSERAPI
BOOL
WINAPI
ActivateKeyboardLayout(
    _In_ HKL hkl,
    _In_ UINT Flags);
#endif /* WINVER >= 0x0400 */

#if(WINVER >= 0x0400)
WINUSERAPI
int
WINAPI
ToUnicodeEx(
    _In_ UINT wVirtKey,
    _In_ UINT wScanCode,
    _In_reads_bytes_(256) CONST BYTE *lpKeyState,
    _Out_writes_(cchBuff) LPWSTR pwszBuff,
    _In_ int cchBuff,
    _In_ UINT wFlags,
    _In_opt_ HKL dwhkl);
#endif /* WINVER >= 0x0400 */

_Win32_metadata_set_last_error_
WINUSERAPI
BOOL
WINAPI
UnloadKeyboardLayout(
    _In_ HKL hkl);

_Win32_metadata_set_last_error_
WINUSERAPI
BOOL
WINAPI
GetKeyboardLayoutNameA(
    _Out_writes_(KL_NAMELENGTH) LPSTR pwszKLID);
_Win32_metadata_set_last_error_
WINUSERAPI
BOOL
WINAPI
GetKeyboardLayoutNameW(
    _Out_writes_(KL_NAMELENGTH) LPWSTR pwszKLID);
#ifdef UNICODE
#define GetKeyboardLayoutName  GetKeyboardLayoutNameW
#else
#define GetKeyboardLayoutName  GetKeyboardLayoutNameA
#endif // !UNICODE

#if(WINVER >= 0x0400)
_Win32_metadata_set_last_error_
WINUSERAPI
int
WINAPI
GetKeyboardLayoutList(
    _In_ int nBuff,
    _Out_writes_to_opt_(nBuff, return) HKL FAR *lpList);

WINUSERAPI
HKL
WINAPI
GetKeyboardLayout(
    _In_ DWORD idThread);

#endif /* WINVER >= 0x0400 */


#endif /* WINAPI_FAMILY_PARTITION(WINAPI_PARTITION_DESKTOP) */
#pragma endregion

#if(WINVER >= 0x0500)

#pragma region Desktop Family
#if WINAPI_FAMILY_PARTITION(WINAPI_PARTITION_DESKTOP)

typedef struct tagMOUSEMOVEPOINT {
    int   x;
    int   y;
    DWORD time;
    ULONG_PTR dwExtraInfo;
} MOUSEMOVEPOINT, *PMOUSEMOVEPOINT, FAR* LPMOUSEMOVEPOINT;

#endif /* WINAPI_FAMILY_PARTITION(WINAPI_PARTITION_DESKTOP) */
#pragma endregion

/*
 * Values for resolution parameter of GetMouseMovePointsEx
 */
#define GMMP_USE_DISPLAY_POINTS          1
#define GMMP_USE_HIGH_RESOLUTION_POINTS  2

#pragma region Desktop Family
#if WINAPI_FAMILY_PARTITION(WINAPI_PARTITION_DESKTOP)

_Win32_metadata_set_last_error_
WINUSERAPI
int
WINAPI
GetMouseMovePointsEx(
    _In_ UINT cbSize,
    _In_ LPMOUSEMOVEPOINT lppt,
    _Out_writes_(nBufPoints) LPMOUSEMOVEPOINT lpptBuf,
    _In_ int nBufPoints,
    _In_ GET_MOUSE_MOVE_POINTS_EX_RESOLUTION resolution);

#endif /* WINAPI_FAMILY_PARTITION(WINAPI_PARTITION_DESKTOP) */
#pragma endregion

#endif /* WINVER >= 0x0500 */

#ifndef NODESKTOP
/*
 * Desktop-specific access flags
 */
#define DESKTOP_READOBJECTS         0x0001L
#define DESKTOP_CREATEWINDOW        0x0002L
#define DESKTOP_CREATEMENU          0x0004L
#define DESKTOP_HOOKCONTROL         0x0008L
#define DESKTOP_JOURNALRECORD       0x0010L
#define DESKTOP_JOURNALPLAYBACK     0x0020L
#define DESKTOP_ENUMERATE           0x0040L
#define DESKTOP_WRITEOBJECTS        0x0080L
#define DESKTOP_SWITCHDESKTOP       0x0100L

/*
 * Desktop-specific control flags
 */
#define DF_ALLOWOTHERACCOUNTHOOK    0x0001L

#ifdef _WINGDI_
#ifndef NOGDI

#pragma region Desktop Family
#if WINAPI_FAMILY_PARTITION(WINAPI_PARTITION_DESKTOP)

_Win32_metadata_set_last_error_
WINUSERAPI
HDESK
WINAPI
CreateDesktopA(
    _In_ LPCSTR lpszDesktop,
    _Reserved_ LPCSTR lpszDevice,
    _Reserved_ DEVMODEA* pDevmode,
    _In_ DESKTOP_CONTROL_FLAGS dwFlags,
    _In_ ACCESS_MASK dwDesiredAccess,
    _In_opt_ LPSECURITY_ATTRIBUTES lpsa);
_Win32_metadata_set_last_error_
WINUSERAPI
HDESK
WINAPI
CreateDesktopW(
    _In_ LPCWSTR lpszDesktop,
    _Reserved_ LPCWSTR lpszDevice,
    _Reserved_ DEVMODEW* pDevmode,
    _In_ DESKTOP_CONTROL_FLAGS dwFlags,
    _In_ ACCESS_MASK dwDesiredAccess,
    _In_opt_ LPSECURITY_ATTRIBUTES lpsa);
#ifdef UNICODE
#define CreateDesktop  CreateDesktopW
#else
#define CreateDesktop  CreateDesktopA
#endif // !UNICODE

_Win32_metadata_set_last_error_
WINUSERAPI
HDESK
WINAPI
CreateDesktopExA(
    _In_ LPCSTR lpszDesktop,
    _Reserved_ LPCSTR lpszDevice,
    _Reserved_ DEVMODEA* pDevmode,
    _In_ DESKTOP_CONTROL_FLAGS dwFlags,
    _In_ ACCESS_MASK dwDesiredAccess,
    _In_opt_ LPSECURITY_ATTRIBUTES lpsa,
    _In_ ULONG ulHeapSize,
    _Reserved_ PVOID pvoid);
_Win32_metadata_set_last_error_
WINUSERAPI
HDESK
WINAPI
CreateDesktopExW(
    _In_ LPCWSTR lpszDesktop,
    _Reserved_ LPCWSTR lpszDevice,
    _Reserved_ DEVMODEW* pDevmode,
    _In_ DESKTOP_CONTROL_FLAGS dwFlags,
    _In_ ACCESS_MASK dwDesiredAccess,
    _In_opt_ LPSECURITY_ATTRIBUTES lpsa,
    _In_ ULONG ulHeapSize,
    _Reserved_ PVOID pvoid);
#ifdef UNICODE
#define CreateDesktopEx  CreateDesktopExW
#else
#define CreateDesktopEx  CreateDesktopExA
#endif // !UNICODE

#endif /* WINAPI_FAMILY_PARTITION(WINAPI_PARTITION_DESKTOP) */
#pragma endregion

#endif /* NOGDI */
#endif /* _WINGDI_ */

#pragma region Desktop Family
#if WINAPI_FAMILY_PARTITION(WINAPI_PARTITION_DESKTOP)

_Win32_metadata_set_last_error_
WINUSERAPI
HDESK
WINAPI
OpenDesktopA(
    _In_ LPCSTR lpszDesktop,
    _In_ DESKTOP_CONTROL_FLAGS dwFlags,
    _In_ BOOL fInherit,
    _In_ ACCESS_MASK dwDesiredAccess);
_Win32_metadata_set_last_error_
WINUSERAPI
HDESK
WINAPI
OpenDesktopW(
    _In_ LPCWSTR lpszDesktop,
    _In_ DESKTOP_CONTROL_FLAGS dwFlags,
    _In_ BOOL fInherit,
    _In_ ACCESS_MASK dwDesiredAccess);
#ifdef UNICODE
#define OpenDesktop  OpenDesktopW
#else
#define OpenDesktop  OpenDesktopA
#endif // !UNICODE

_Win32_metadata_set_last_error_
WINUSERAPI
HDESK
WINAPI
OpenInputDesktop(
    _In_ DESKTOP_CONTROL_FLAGS dwFlags,
    _In_ BOOL fInherit,
    _In_ DESKTOP_ACCESS_FLAGS dwDesiredAccess);


_Win32_metadata_set_last_error_
WINUSERAPI
BOOL
WINAPI
EnumDesktopsA(
    _In_opt_ HWINSTA hwinsta,
    _In_ DESKTOPENUMPROCA lpEnumFunc,
    _In_ LPARAM lParam);
_Win32_metadata_set_last_error_
WINUSERAPI
BOOL
WINAPI
EnumDesktopsW(
    _In_opt_ HWINSTA hwinsta,
    _In_ DESKTOPENUMPROCW lpEnumFunc,
    _In_ LPARAM lParam);
#ifdef UNICODE
#define EnumDesktops  EnumDesktopsW
#else
#define EnumDesktops  EnumDesktopsA
#endif // !UNICODE

_Win32_metadata_set_last_error_
WINUSERAPI
BOOL
WINAPI
EnumDesktopWindows(
    _In_opt_ HDESK hDesktop,
    _In_ WNDENUMPROC lpfn,
    _In_ LPARAM lParam);


_Win32_metadata_set_last_error_
WINUSERAPI
BOOL
WINAPI
SwitchDesktop(
    _In_ HDESK hDesktop);


_Win32_metadata_set_last_error_
WINUSERAPI
BOOL
WINAPI
SetThreadDesktop(
     _In_ HDESK hDesktop);

_Win32_metadata_set_last_error_
WINUSERAPI
BOOL
WINAPI
CloseDesktop(
    _In_ HDESK hDesktop);

_Win32_metadata_set_last_error_
WINUSERAPI
HDESK
WINAPI
GetThreadDesktop(
    _In_ DWORD dwThreadId);

#endif /* WINAPI_FAMILY_PARTITION(WINAPI_PARTITION_DESKTOP) */
#pragma endregion

#endif  /* !NODESKTOP */

#ifndef NOWINDOWSTATION
/*
 * Windowstation-specific access flags
 */
#define WINSTA_ENUMDESKTOPS         0x0001L
#define WINSTA_READATTRIBUTES       0x0002L
#define WINSTA_ACCESSCLIPBOARD      0x0004L
#define WINSTA_CREATEDESKTOP        0x0008L
#define WINSTA_WRITEATTRIBUTES      0x0010L
#define WINSTA_ACCESSGLOBALATOMS    0x0020L
#define WINSTA_EXITWINDOWS          0x0040L
#define WINSTA_ENUMERATE            0x0100L
#define WINSTA_READSCREEN           0x0200L

#define WINSTA_ALL_ACCESS           (WINSTA_ENUMDESKTOPS  | WINSTA_READATTRIBUTES  | WINSTA_ACCESSCLIPBOARD | \
                                     WINSTA_CREATEDESKTOP | WINSTA_WRITEATTRIBUTES | WINSTA_ACCESSGLOBALATOMS | \
                                     WINSTA_EXITWINDOWS   | WINSTA_ENUMERATE       | WINSTA_READSCREEN)

/*
 * Windowstation creation flags.
 */
#define CWF_CREATE_ONLY           0x00000001

// Remove when Feature_MarkWinstaIOForAgentic is removed.

/*
 * Windowstation-specific attribute flags
 */
#define WSF_VISIBLE                 0x0001L

#pragma region Desktop Family
#if WINAPI_FAMILY_PARTITION(WINAPI_PARTITION_DESKTOP)

_Win32_metadata_set_last_error_
WINUSERAPI
HWINSTA
WINAPI
CreateWindowStationA(
    _In_opt_ LPCSTR lpwinsta,
    _In_ DWORD dwFlags,
    _In_ ACCESS_MASK dwDesiredAccess,
    _In_opt_ LPSECURITY_ATTRIBUTES lpsa);
_Win32_metadata_set_last_error_
WINUSERAPI
HWINSTA
WINAPI
CreateWindowStationW(
    _In_opt_ LPCWSTR lpwinsta,
    _In_ DWORD dwFlags,
    _In_ ACCESS_MASK dwDesiredAccess,
    _In_opt_ LPSECURITY_ATTRIBUTES lpsa);
#ifdef UNICODE
#define CreateWindowStation  CreateWindowStationW
#else
#define CreateWindowStation  CreateWindowStationA
#endif // !UNICODE

_Win32_metadata_set_last_error_
WINUSERAPI
HWINSTA
WINAPI
OpenWindowStationA(
    _In_ LPCSTR lpszWinSta,
    _In_ BOOL fInherit,
    _In_ ACCESS_MASK dwDesiredAccess);
_Win32_metadata_set_last_error_
WINUSERAPI
HWINSTA
WINAPI
OpenWindowStationW(
    _In_ LPCWSTR lpszWinSta,
    _In_ BOOL fInherit,
    _In_ ACCESS_MASK dwDesiredAccess);
#ifdef UNICODE
#define OpenWindowStation  OpenWindowStationW
#else
#define OpenWindowStation  OpenWindowStationA
#endif // !UNICODE

_Win32_metadata_set_last_error_
WINUSERAPI
BOOL
WINAPI
EnumWindowStationsA(
    _In_ WINSTAENUMPROCA lpEnumFunc,
    _In_ LPARAM lParam);
_Win32_metadata_set_last_error_
WINUSERAPI
BOOL
WINAPI
EnumWindowStationsW(
    _In_ WINSTAENUMPROCW lpEnumFunc,
    _In_ LPARAM lParam);
#ifdef UNICODE
#define EnumWindowStations  EnumWindowStationsW
#else
#define EnumWindowStations  EnumWindowStationsA
#endif // !UNICODE

_Win32_metadata_set_last_error_
WINUSERAPI
BOOL
WINAPI
CloseWindowStation(
    _In_ HWINSTA hWinSta);

_Win32_metadata_set_last_error_
WINUSERAPI
BOOL
WINAPI
SetProcessWindowStation(
    _In_ HWINSTA hWinSta);

_Win32_metadata_set_last_error_
WINUSERAPI
HWINSTA
WINAPI
GetProcessWindowStation(
    VOID);

#endif /* WINAPI_FAMILY_PARTITION(WINAPI_PARTITION_DESKTOP) */
#pragma endregion

#endif  /* !NOWINDOWSTATION */

#ifndef NOSECURITY

#pragma region Desktop Family
#if WINAPI_FAMILY_PARTITION(WINAPI_PARTITION_DESKTOP)

_Win32_metadata_set_last_error_
WINUSERAPI
BOOL
WINAPI
SetUserObjectSecurity(
    _In_ HANDLE hObj,
    _In_ PSECURITY_INFORMATION pSIRequested,
    _In_ PSECURITY_DESCRIPTOR pSID);

_Win32_metadata_set_last_error_
WINUSERAPI
BOOL
WINAPI
GetUserObjectSecurity(
    _In_ HANDLE hObj,
    _In_ PSECURITY_INFORMATION pSIRequested,
    _Out_writes_bytes_opt_(nLength) PSECURITY_DESCRIPTOR pSID,
    _In_ DWORD nLength,
    _Out_ LPDWORD lpnLengthNeeded);

#endif /* WINAPI_FAMILY_PARTITION(WINAPI_PARTITION_DESKTOP) */
#pragma endregion

#define UOI_FLAGS       1
#define UOI_NAME        2
#define UOI_TYPE        3
#define UOI_USER_SID    4
#if(WINVER >= 0x0600)
#define UOI_HEAPSIZE    5
#define UOI_IO          6
#endif /* WINVER >= 0x0600 */
#define UOI_TIMERPROC_EXCEPTION_SUPPRESSION       7

#pragma region Desktop Family
#if WINAPI_FAMILY_PARTITION(WINAPI_PARTITION_DESKTOP)

typedef struct tagUSEROBJECTFLAGS {
    BOOL fInherit;
    BOOL fReserved;
    DWORD dwFlags;
} USEROBJECTFLAGS, *PUSEROBJECTFLAGS;

_Win32_metadata_set_last_error_
WINUSERAPI
BOOL
WINAPI
GetUserObjectInformationA(
    _In_ HANDLE hObj,
    _In_ USER_OBJECT_INFORMATION_INDEX nIndex,
    _Out_writes_bytes_opt_(nLength) PVOID pvInfo,
    _In_ DWORD nLength,
    _Out_opt_ LPDWORD lpnLengthNeeded);
_Win32_metadata_set_last_error_
WINUSERAPI
BOOL
WINAPI
GetUserObjectInformationW(
    _In_ HANDLE hObj,
    _In_ USER_OBJECT_INFORMATION_INDEX nIndex,
    _Out_writes_bytes_opt_(nLength) PVOID pvInfo,
    _In_ DWORD nLength,
    _Out_opt_ LPDWORD lpnLengthNeeded);
#ifdef UNICODE
#define GetUserObjectInformation  GetUserObjectInformationW
#else
#define GetUserObjectInformation  GetUserObjectInformationA
#endif // !UNICODE

_Win32_metadata_set_last_error_
WINUSERAPI
BOOL
WINAPI
SetUserObjectInformationA(
    _In_ HANDLE hObj,
    _In_ int nIndex,
    _In_reads_bytes_(nLength) PVOID pvInfo,
    _In_ DWORD nLength);
_Win32_metadata_set_last_error_
WINUSERAPI
BOOL
WINAPI
SetUserObjectInformationW(
    _In_ HANDLE hObj,
    _In_ int nIndex,
    _In_reads_bytes_(nLength) PVOID pvInfo,
    _In_ DWORD nLength);
#ifdef UNICODE
#define SetUserObjectInformation  SetUserObjectInformationW
#else
#define SetUserObjectInformation  SetUserObjectInformationA
#endif // !UNICODE

#endif /* WINAPI_FAMILY_PARTITION(WINAPI_PARTITION_DESKTOP) */
#pragma endregion

#endif  /* !NOSECURITY */

#pragma region Desktop or Games Family
#if WINAPI_FAMILY_PARTITION(WINAPI_PARTITION_DESKTOP | WINAPI_PARTITION_GAMES)

#if(WINVER >= 0x0400)
typedef struct tagWNDCLASSEXA {
    UINT        cbSize;
    /* Win 3.x */
    WNDCLASS_STYLES        style;
    WNDPROC     lpfnWndProc;
    int         cbClsExtra;
    int         cbWndExtra;
    HINSTANCE   hInstance;
    HICON       hIcon;
    HCURSOR     hCursor;
    HBRUSH      hbrBackground;
    LPCSTR      lpszMenuName;
    LPCSTR      lpszClassName;
    /* Win 4.0 */
    HICON       hIconSm;
} WNDCLASSEXA, *PWNDCLASSEXA, NEAR *NPWNDCLASSEXA, FAR *LPWNDCLASSEXA;
typedef struct tagWNDCLASSEXW {
    UINT        cbSize;
    /* Win 3.x */
    WNDCLASS_STYLES        style;
    WNDPROC     lpfnWndProc;
    int         cbClsExtra;
    int         cbWndExtra;
    HINSTANCE   hInstance;
    HICON       hIcon;
    HCURSOR     hCursor;
    HBRUSH      hbrBackground;
    LPCWSTR     lpszMenuName;
    LPCWSTR     lpszClassName;
    /* Win 4.0 */
    HICON       hIconSm;
} WNDCLASSEXW, *PWNDCLASSEXW, NEAR *NPWNDCLASSEXW, FAR *LPWNDCLASSEXW;
#ifdef UNICODE
typedef WNDCLASSEXW WNDCLASSEX;
typedef PWNDCLASSEXW PWNDCLASSEX;
typedef NPWNDCLASSEXW NPWNDCLASSEX;
typedef LPWNDCLASSEXW LPWNDCLASSEX;
#else
typedef WNDCLASSEXA WNDCLASSEX;
typedef PWNDCLASSEXA PWNDCLASSEX;
typedef NPWNDCLASSEXA NPWNDCLASSEX;
typedef LPWNDCLASSEXA LPWNDCLASSEX;
#endif // UNICODE
#endif /* WINVER >= 0x0400 */

typedef struct tagWNDCLASSA {
    WNDCLASS_STYLES        style;
    WNDPROC     lpfnWndProc;
    int         cbClsExtra;
    int         cbWndExtra;
    HINSTANCE   hInstance;
    HICON       hIcon;
    HCURSOR     hCursor;
    HBRUSH      hbrBackground;
    LPCSTR      lpszMenuName;
    LPCSTR      lpszClassName;
} WNDCLASSA, *PWNDCLASSA, NEAR *NPWNDCLASSA, FAR *LPWNDCLASSA;
typedef struct tagWNDCLASSW {
    WNDCLASS_STYLES        style;
    WNDPROC     lpfnWndProc;
    int         cbClsExtra;
    int         cbWndExtra;
    HINSTANCE   hInstance;
    HICON       hIcon;
    HCURSOR     hCursor;
    HBRUSH      hbrBackground;
    LPCWSTR     lpszMenuName;
    LPCWSTR     lpszClassName;
} WNDCLASSW, *PWNDCLASSW, NEAR *NPWNDCLASSW, FAR *LPWNDCLASSW;
#ifdef UNICODE
typedef WNDCLASSW WNDCLASS;
typedef PWNDCLASSW PWNDCLASS;
typedef NPWNDCLASSW NPWNDCLASS;
typedef LPWNDCLASSW LPWNDCLASS;
#else
typedef WNDCLASSA WNDCLASS;
typedef PWNDCLASSA PWNDCLASS;
typedef NPWNDCLASSA NPWNDCLASS;
typedef LPWNDCLASSA LPWNDCLASS;
#endif // UNICODE

#endif /* WINAPI_FAMILY_PARTITION(WINAPI_PARTITION_DESKTOP | WINAPI_PARTITION_GAMES) */
#pragma endregion

#pragma region Desktop Family
#if WINAPI_FAMILY_PARTITION(WINAPI_PARTITION_DESKTOP)

WINUSERAPI
BOOL
WINAPI
IsHungAppWindow(
    _In_ HWND hwnd);


#if(WINVER >= 0x0501)
WINUSERAPI
VOID
WINAPI
DisableProcessWindowsGhosting(
    VOID);
#endif /* WINVER >= 0x0501 */

#endif /* WINAPI_FAMILY_PARTITION(WINAPI_PARTITION_DESKTOP) */
#pragma endregion


#ifndef NOMSG

#pragma region Application Family or OneCore Family or Games Family
#if WINAPI_FAMILY_PARTITION(WINAPI_PARTITION_APP | WINAPI_PARTITION_SYSTEM | WINAPI_PARTITION_GAMES)

/*
 * Message structure
 */
typedef struct tagMSG {
    HWND        hwnd;
    UINT        message;
    WPARAM      wParam;
    LPARAM      lParam;
    DWORD       time;
    POINT       pt;
#ifdef _MAC
    DWORD       lPrivate;
#endif
} MSG, *PMSG, NEAR *NPMSG, FAR *LPMSG;

#endif /* WINAPI_FAMILY_PARTITION(WINAPI_PARTITION_APP | WINAPI_PARTITION_SYSTEM | WINAPI_PARTITION_GAMES) */
#pragma endregion

#define POINTSTOPOINT(pt, pts)                          \
        { (pt).x = (LONG)(SHORT)LOWORD(*(LONG*)&pts);   \
          (pt).y = (LONG)(SHORT)HIWORD(*(LONG*)&pts); }

#define POINTTOPOINTS(pt)      (MAKELONG((short)((pt).x), (short)((pt).y)))
#define MAKEWPARAM(l, h)      ((WPARAM)(DWORD)MAKELONG(l, h))
#define MAKELPARAM(l, h)      ((LPARAM)(DWORD)MAKELONG(l, h))
#define MAKELRESULT(l, h)     ((LRESULT)(DWORD)MAKELONG(l, h))


#endif /* !NOMSG */

#ifndef NOWINOFFSETS

/*
 * Window field offsets for GetWindowLong()
 */
#define GWL_WNDPROC         (-4)
#define GWL_HINSTANCE       (-6)
#define GWL_HWNDPARENT      (-8)
#define GWL_STYLE           (-16)
#define GWL_EXSTYLE         (-20)
#define GWL_USERDATA        (-21)
#define GWL_ID              (-12)

#ifdef _WIN64

#undef GWL_WNDPROC
#undef GWL_HINSTANCE
#undef GWL_HWNDPARENT
#undef GWL_USERDATA

#endif /* _WIN64 */

#define GWLP_WNDPROC        (-4)
#define GWLP_HINSTANCE      (-6)
#define GWLP_HWNDPARENT     (-8)
#define GWLP_USERDATA       (-21)
#define GWLP_ID             (-12)

/*
 * Used in _GetWindowLongPtr to return internal state of user32 edit control
 */

/*
 * Class field offsets for GetClassLong()
 */
#define GCL_MENUNAME        (-8)
#define GCL_HBRBACKGROUND   (-10)
#define GCL_HCURSOR         (-12)
#define GCL_HICON           (-14)
#define GCL_HMODULE         (-16)
#define GCL_CBWNDEXTRA      (-18)
#define GCL_CBCLSEXTRA      (-20)
#define GCL_WNDPROC         (-24)
#define GCL_STYLE           (-26)
#define GCW_ATOM            (-32)

#if(WINVER >= 0x0400)
#define GCL_HICONSM         (-34)
#endif /* WINVER >= 0x0400 */

#ifdef _WIN64

#undef GCL_MENUNAME
#undef GCL_HBRBACKGROUND
#undef GCL_HCURSOR
#undef GCL_HICON
#undef GCL_HMODULE
#undef GCL_WNDPROC
#undef GCL_HICONSM

#endif /* _WIN64 */

#define GCLP_MENUNAME       (-8)
#define GCLP_HBRBACKGROUND  (-10)
#define GCLP_HCURSOR        (-12)
#define GCLP_HICON          (-14)
#define GCLP_HMODULE        (-16)
#define GCLP_WNDPROC        (-24)
#define GCLP_HICONSM        (-34)

#endif /* !NOWINOFFSETS */

#ifndef NOWINMESSAGES


/*
 * Window Messages
 */

#define WM_NULL                         0x0000
#define WM_CREATE                       0x0001
#define WM_DESTROY                      0x0002
#define WM_MOVE                         0x0003
#define WM_SIZE                         0x0005

#define WM_ACTIVATE                     0x0006
/*
 * WM_ACTIVATE state values
 */
#define     WA_INACTIVE     0
#define     WA_ACTIVE       1
#define     WA_CLICKACTIVE  2

#define WM_SETFOCUS                     0x0007
#define WM_KILLFOCUS                    0x0008
#define WM_ENABLE                       0x000A
#define WM_SETREDRAW                    0x000B
#define WM_SETTEXT                      0x000C
#define WM_GETTEXT                      0x000D
#define WM_GETTEXTLENGTH                0x000E
#define WM_PAINT                        0x000F
#define WM_CLOSE                        0x0010
#ifndef _WIN32_WCE
#define WM_QUERYENDSESSION              0x0011
#define WM_QUERYOPEN                    0x0013
#define WM_ENDSESSION                   0x0016
#endif
#define WM_QUIT                         0x0012
#define WM_ERASEBKGND                   0x0014
#define WM_SYSCOLORCHANGE               0x0015
#define WM_SHOWWINDOW                   0x0018
#define WM_WININICHANGE                 0x001A
#if(WINVER >= 0x0400)
#define WM_SETTINGCHANGE                WM_WININICHANGE
#endif /* WINVER >= 0x0400 */

#if (NTDDI_VERSION >= NTDDI_WIN10_19H1)
#endif // NTDDI_VERSION >= NTDDI_WIN10_19H1


#define WM_DEVMODECHANGE                0x001B
#define WM_ACTIVATEAPP                  0x001C
#define WM_FONTCHANGE                   0x001D
#define WM_TIMECHANGE                   0x001E
#define WM_CANCELMODE                   0x001F
#define WM_SETCURSOR                    0x0020
#define WM_MOUSEACTIVATE                0x0021
#define WM_CHILDACTIVATE                0x0022
#define WM_QUEUESYNC                    0x0023

#define WM_GETMINMAXINFO                0x0024

#pragma region Desktop Family
#if WINAPI_FAMILY_PARTITION(WINAPI_PARTITION_DESKTOP)

/*
 * Struct pointed to by WM_GETMINMAXINFO lParam
 */
typedef struct tagMINMAXINFO {
    POINT ptReserved;
    POINT ptMaxSize;
    POINT ptMaxPosition;
    POINT ptMinTrackSize;
    POINT ptMaxTrackSize;
} MINMAXINFO, *PMINMAXINFO, *LPMINMAXINFO;

#endif /* WINAPI_FAMILY_PARTITION(WINAPI_PARTITION_DESKTOP) */
#pragma endregion

#define WM_PAINTICON                    0x0026
#define WM_ICONERASEBKGND               0x0027
#define WM_NEXTDLGCTL                   0x0028
#define WM_SPOOLERSTATUS                0x002A
#define WM_DRAWITEM                     0x002B
#define WM_MEASUREITEM                  0x002C
#define WM_DELETEITEM                   0x002D
#define WM_VKEYTOITEM                   0x002E
#define WM_CHARTOITEM                   0x002F
#define WM_SETFONT                      0x0030
#define WM_GETFONT                      0x0031
#define WM_SETHOTKEY                    0x0032
#define WM_GETHOTKEY                    0x0033
#define WM_QUERYDRAGICON                0x0037
#define WM_COMPAREITEM                  0x0039
#if(WINVER >= 0x0500)
#ifndef _WIN32_WCE
#define WM_GETOBJECT                    0x003D
#endif
#endif /* WINVER >= 0x0500 */
#define WM_COMPACTING                   0x0041
#define WM_COMMNOTIFY                   0x0044  /* no longer suported */
#define WM_WINDOWPOSCHANGING            0x0046
#define WM_WINDOWPOSCHANGED             0x0047

#define WM_POWER                        0x0048
/*
 * wParam for WM_POWER window message and DRV_POWER driver notification
 */
#define PWR_OK              1
#define PWR_FAIL            (-1)
#define PWR_SUSPENDREQUEST  1
#define PWR_SUSPENDRESUME   2
#define PWR_CRITICALRESUME  3

#define WM_COPYDATA                     0x004A
#define WM_CANCELJOURNAL                0x004B


#pragma region Desktop Family
#if WINAPI_FAMILY_PARTITION(WINAPI_PARTITION_DESKTOP)

/*
 * lParam of WM_COPYDATA message points to...
 */
typedef struct tagCOPYDATASTRUCT {
    ULONG_PTR dwData;
    DWORD cbData;
    _Field_size_bytes_(cbData) PVOID lpData;
} COPYDATASTRUCT, *PCOPYDATASTRUCT;

#if(WINVER >= 0x0400)
typedef struct tagMDINEXTMENU
{
    HMENU   hmenuIn;
    HMENU   hmenuNext;
    HWND    hwndNext;
} MDINEXTMENU, * PMDINEXTMENU, FAR * LPMDINEXTMENU;
#endif /* WINVER >= 0x0400 */

#endif /* WINAPI_FAMILY_PARTITION(WINAPI_PARTITION_DESKTOP) */
#pragma endregion


#if(WINVER >= 0x0400)
#define WM_NOTIFY                       0x004E
#define WM_INPUTLANGCHANGEREQUEST       0x0050
#define WM_INPUTLANGCHANGE              0x0051
#define WM_TCARD                        0x0052
#define WM_HELP                         0x0053
#define WM_USERCHANGED                  0x0054
#define WM_NOTIFYFORMAT                 0x0055

#define NFR_ANSI                             1
#define NFR_UNICODE                          2
#define NF_QUERY                             3
#define NF_REQUERY                           4

#define WM_CONTEXTMENU                  0x007B
#define WM_STYLECHANGING                0x007C
#define WM_STYLECHANGED                 0x007D
#define WM_DISPLAYCHANGE                0x007E
#define WM_GETICON                      0x007F
#define WM_SETICON                      0x0080
#endif /* WINVER >= 0x0400 */

#define WM_NCCREATE                     0x0081
#define WM_NCDESTROY                    0x0082
#define WM_NCCALCSIZE                   0x0083
#define WM_NCHITTEST                    0x0084
#define WM_NCPAINT                      0x0085
#define WM_NCACTIVATE                   0x0086
#define WM_GETDLGCODE                   0x0087
#ifndef _WIN32_WCE
#define WM_SYNCPAINT                    0x0088
#endif
#define WM_NCMOUSEMOVE                  0x00A0
#define WM_NCLBUTTONDOWN                0x00A1
#define WM_NCLBUTTONUP                  0x00A2
#define WM_NCLBUTTONDBLCLK              0x00A3
#define WM_NCRBUTTONDOWN                0x00A4
#define WM_NCRBUTTONUP                  0x00A5
#define WM_NCRBUTTONDBLCLK              0x00A6
#define WM_NCMBUTTONDOWN                0x00A7
#define WM_NCMBUTTONUP                  0x00A8
#define WM_NCMBUTTONDBLCLK              0x00A9



#if(_WIN32_WINNT >= 0x0500)
#define WM_NCXBUTTONDOWN                0x00AB
#define WM_NCXBUTTONUP                  0x00AC
#define WM_NCXBUTTONDBLCLK              0x00AD
#endif /* _WIN32_WINNT >= 0x0500 */


#if(_WIN32_WINNT >= 0x0501)
#define WM_INPUT_DEVICE_CHANGE          0x00FE
#endif /* _WIN32_WINNT >= 0x0501 */

#if(_WIN32_WINNT >= 0x0501)
#define WM_INPUT                        0x00FF
#endif /* _WIN32_WINNT >= 0x0501 */

#define WM_KEYFIRST                     0x0100
#define WM_KEYDOWN                      0x0100
#define WM_KEYUP                        0x0101
#define WM_CHAR                         0x0102
#define WM_DEADCHAR                     0x0103
#define WM_SYSKEYDOWN                   0x0104
#define WM_SYSKEYUP                     0x0105
#define WM_SYSCHAR                      0x0106
#define WM_SYSDEADCHAR                  0x0107
#if(_WIN32_WINNT >= 0x0501)
#define WM_UNICHAR                      0x0109
#define WM_KEYLAST                      0x0109
#define UNICODE_NOCHAR                  0xFFFF
#else
#define WM_KEYLAST                      0x0108
#endif /* _WIN32_WINNT >= 0x0501 */

#if(WINVER >= 0x0400)
#define WM_IME_STARTCOMPOSITION         0x010D
#define WM_IME_ENDCOMPOSITION           0x010E
#define WM_IME_COMPOSITION              0x010F
#define WM_IME_KEYLAST                  0x010F
#endif /* WINVER >= 0x0400 */

#define WM_INITDIALOG                   0x0110
#define WM_COMMAND                      0x0111
#define WM_SYSCOMMAND                   0x0112
#define WM_TIMER                        0x0113
#define WM_HSCROLL                      0x0114
#define WM_VSCROLL                      0x0115
#define WM_INITMENU                     0x0116
#define WM_INITMENUPOPUP                0x0117
#if(WINVER >= 0x0601)
#define WM_GESTURE                      0x0119
#define WM_GESTURENOTIFY                0x011A
#endif /* WINVER >= 0x0601 */
#define WM_MENUSELECT                   0x011F
#define WM_MENUCHAR                     0x0120
#define WM_ENTERIDLE                    0x0121
#if(WINVER >= 0x0500)
#ifndef _WIN32_WCE
#define WM_MENURBUTTONUP                0x0122
#define WM_MENUDRAG                     0x0123
#define WM_MENUGETOBJECT                0x0124
#define WM_UNINITMENUPOPUP              0x0125
#define WM_MENUCOMMAND                  0x0126

#ifndef _WIN32_WCE
#if(_WIN32_WINNT >= 0x0500)
#define WM_CHANGEUISTATE                0x0127
#define WM_UPDATEUISTATE                0x0128
#define WM_QUERYUISTATE                 0x0129

/*
 * LOWORD(wParam) values in WM_*UISTATE*
 */
#define UIS_SET                         1
#define UIS_CLEAR                       2
#define UIS_INITIALIZE                  3

/*
 * HIWORD(wParam) values in WM_*UISTATE*
 */
#define UISF_HIDEFOCUS                  0x1
#define UISF_HIDEACCEL                  0x2
#if(_WIN32_WINNT >= 0x0501)
#define UISF_ACTIVE                     0x4
#endif /* _WIN32_WINNT >= 0x0501 */
#endif /* _WIN32_WINNT >= 0x0500 */
#endif

#endif
#endif /* WINVER >= 0x0500 */

#define WM_CTLCOLORMSGBOX               0x0132
#define WM_CTLCOLOREDIT                 0x0133
#define WM_CTLCOLORLISTBOX              0x0134
#define WM_CTLCOLORBTN                  0x0135
#define WM_CTLCOLORDLG                  0x0136
#define WM_CTLCOLORSCROLLBAR            0x0137
#define WM_CTLCOLORSTATIC               0x0138
#define MN_GETHMENU                     0x01E1

#define WM_MOUSEFIRST                   0x0200
#define WM_MOUSEMOVE                    0x0200
#define WM_LBUTTONDOWN                  0x0201
#define WM_LBUTTONUP                    0x0202
#define WM_LBUTTONDBLCLK                0x0203
#define WM_RBUTTONDOWN                  0x0204
#define WM_RBUTTONUP                    0x0205
#define WM_RBUTTONDBLCLK                0x0206
#define WM_MBUTTONDOWN                  0x0207
#define WM_MBUTTONUP                    0x0208
#define WM_MBUTTONDBLCLK                0x0209
#if (_WIN32_WINNT >= 0x0400) || (_WIN32_WINDOWS > 0x0400)
#define WM_MOUSEWHEEL                   0x020A
#endif
#if (_WIN32_WINNT >= 0x0500)
#define WM_XBUTTONDOWN                  0x020B
#define WM_XBUTTONUP                    0x020C
#define WM_XBUTTONDBLCLK                0x020D
#endif
#if (_WIN32_WINNT >= 0x0600)
#define WM_MOUSEHWHEEL                  0x020E
#endif

#if (_WIN32_WINNT >= 0x0600)
#define WM_MOUSELAST                    0x020E
#elif (_WIN32_WINNT >= 0x0500)
#define WM_MOUSELAST                    0x020D
#elif (_WIN32_WINNT >= 0x0400) || (_WIN32_WINDOWS > 0x0400)
#define WM_MOUSELAST                    0x020A
#else
#define WM_MOUSELAST                    0x0209
#endif /* (_WIN32_WINNT >= 0x0600) */


#if(_WIN32_WINNT >= 0x0400)
/* Value for rolling one detent */
#define WHEEL_DELTA                     120
#define GET_WHEEL_DELTA_WPARAM(wParam)  ((short)HIWORD(wParam))

/* Setting to scroll one page for SPI_GET/SETWHEELSCROLLLINES */
#define WHEEL_PAGESCROLL                (UINT_MAX)
#endif /* _WIN32_WINNT >= 0x0400 */

#if(_WIN32_WINNT >= 0x0500)
#define GET_KEYSTATE_WPARAM(wParam)     (LOWORD(wParam))
#define GET_NCHITTEST_WPARAM(wParam)    ((short)LOWORD(wParam))
#define GET_XBUTTON_WPARAM(wParam)      (HIWORD(wParam))

/* XButton values are WORD flags */
#define XBUTTON1      0x0001
#define XBUTTON2      0x0002
/* Were there to be an XBUTTON3, its value would be 0x0004 */
#endif /* _WIN32_WINNT >= 0x0500 */

#define WM_PARENTNOTIFY                 0x0210
#define WM_ENTERMENULOOP                0x0211
#define WM_EXITMENULOOP                 0x0212

#if(WINVER >= 0x0400)
#define WM_NEXTMENU                     0x0213
#define WM_SIZING                       0x0214
#define WM_CAPTURECHANGED               0x0215
#define WM_MOVING                       0x0216
#endif /* WINVER >= 0x0400 */

#if(WINVER >= 0x0400)


#define WM_POWERBROADCAST               0x0218

#ifndef _WIN32_WCE
#define PBT_APMQUERYSUSPEND             0x0000
#define PBT_APMQUERYSTANDBY             0x0001

#define PBT_APMQUERYSUSPENDFAILED       0x0002
#define PBT_APMQUERYSTANDBYFAILED       0x0003

#define PBT_APMSUSPEND                  0x0004
#define PBT_APMSTANDBY                  0x0005

#define PBT_APMRESUMECRITICAL           0x0006
#define PBT_APMRESUMESUSPEND            0x0007
#define PBT_APMRESUMESTANDBY            0x0008

#define PBTF_APMRESUMEFROMFAILURE       0x00000001

#define PBT_APMBATTERYLOW               0x0009
#define PBT_APMPOWERSTATUSCHANGE        0x000A

#define PBT_APMOEMEVENT                 0x000B


#define PBT_APMRESUMEAUTOMATIC          0x0012
#if (_WIN32_WINNT >= 0x0502)
#ifndef PBT_POWERSETTINGCHANGE
#define PBT_POWERSETTINGCHANGE          0x8013

#pragma region Desktop Family
#if WINAPI_FAMILY_PARTITION(WINAPI_PARTITION_DESKTOP)

typedef struct {
    GUID PowerSetting;
    DWORD DataLength;
    UCHAR Data[1];
} POWERBROADCAST_SETTING, *PPOWERBROADCAST_SETTING;


#endif /* WINAPI_FAMILY_PARTITION(WINAPI_PARTITION_DESKTOP) */
#pragma endregion

#endif // PBT_POWERSETTINGCHANGE

#endif // (_WIN32_WINNT >= 0x0502)
#endif

#endif /* WINVER >= 0x0400 */

#if(WINVER >= 0x0400)
#define WM_DEVICECHANGE                 0x0219
#endif /* WINVER >= 0x0400 */

#define WM_MDICREATE                    0x0220
#define WM_MDIDESTROY                   0x0221
#define WM_MDIACTIVATE                  0x0222
#define WM_MDIRESTORE                   0x0223
#define WM_MDINEXT                      0x0224
#define WM_MDIMAXIMIZE                  0x0225
#define WM_MDITILE                      0x0226
#define WM_MDICASCADE                   0x0227
#define WM_MDIICONARRANGE               0x0228
#define WM_MDIGETACTIVE                 0x0229


#define WM_MDISETMENU                   0x0230
#define WM_ENTERSIZEMOVE                0x0231
#define WM_EXITSIZEMOVE                 0x0232
#define WM_DROPFILES                    0x0233
#define WM_MDIREFRESHMENU               0x0234

#if(WINVER >= 0x0602)
#define WM_POINTERDEVICECHANGE          0x238
#define WM_POINTERDEVICEINRANGE         0x239
#define WM_POINTERDEVICEOUTOFRANGE      0x23A
#endif /* WINVER >= 0x0602 */

// TODO(47499024): Make public when Feature_TouchpadPublicApis3 is enabled

#if(WINVER >= 0x0601)
#define WM_TOUCH                        0x0240
#endif /* WINVER >= 0x0601 */

#if(WINVER >= 0x0602)
#define WM_NCPOINTERUPDATE              0x0241
#define WM_NCPOINTERDOWN                0x0242
#define WM_NCPOINTERUP                  0x0243
#define WM_POINTERUPDATE                0x0245
#define WM_POINTERDOWN                  0x0246
#define WM_POINTERUP                    0x0247
#define WM_POINTERENTER                 0x0249
#define WM_POINTERLEAVE                 0x024A
#define WM_POINTERACTIVATE              0x024B
#define WM_POINTERCAPTURECHANGED        0x024C
#define WM_TOUCHHITTESTING              0x024D
#define WM_POINTERWHEEL                 0x024E
#define WM_POINTERHWHEEL                0x024F
#define DM_POINTERHITTEST               0x0250
#define WM_POINTERROUTEDTO              0x0251
#define WM_POINTERROUTEDAWAY            0x0252
#define WM_POINTERROUTEDRELEASED        0x0253
#endif /* WINVER >= 0x0602 */


#if(WINVER >= 0x0400)
#define WM_IME_SETCONTEXT               0x0281
#define WM_IME_NOTIFY                   0x0282
#define WM_IME_CONTROL                  0x0283
#define WM_IME_COMPOSITIONFULL          0x0284
#define WM_IME_SELECT                   0x0285
#define WM_IME_CHAR                     0x0286
#endif /* WINVER >= 0x0400 */
#if(WINVER >= 0x0500)
#define WM_IME_REQUEST                  0x0288
#endif /* WINVER >= 0x0500 */
#if(WINVER >= 0x0400)
#define WM_IME_KEYDOWN                  0x0290
#define WM_IME_KEYUP                    0x0291
#endif /* WINVER >= 0x0400 */

#if((_WIN32_WINNT >= 0x0400) || (WINVER >= 0x0500))
#define WM_MOUSEHOVER                   0x02A1
#define WM_MOUSELEAVE                   0x02A3
#endif
#if(WINVER >= 0x0500)
#define WM_NCMOUSEHOVER                 0x02A0
#define WM_NCMOUSELEAVE                 0x02A2
#endif /* WINVER >= 0x0500 */

#if(_WIN32_WINNT >= 0x0501)
#define WM_WTSSESSION_CHANGE            0x02B1

#define WM_TABLET_FIRST                 0x02c0
#define WM_TABLET_LAST                  0x02df
#endif /* _WIN32_WINNT >= 0x0501 */

#if(WINVER >= 0x0601)
#define WM_DPICHANGED                   0x02E0
#endif /* WINVER >= 0x0601 */
#if(WINVER >= 0x0605)
#define WM_DPICHANGED_BEFOREPARENT      0x02E2
#define WM_DPICHANGED_AFTERPARENT       0x02E3
#define WM_GETDPISCALEDSIZE             0x02E4
#endif /* WINVER >= 0x0605 */

#define WM_CUT                          0x0300
#define WM_COPY                         0x0301
#define WM_PASTE                        0x0302
#define WM_CLEAR                        0x0303
#define WM_UNDO                         0x0304
#define WM_RENDERFORMAT                 0x0305
#define WM_RENDERALLFORMATS             0x0306
#define WM_DESTROYCLIPBOARD             0x0307
#define WM_DRAWCLIPBOARD                0x0308
#define WM_PAINTCLIPBOARD               0x0309
#define WM_VSCROLLCLIPBOARD             0x030A
#define WM_SIZECLIPBOARD                0x030B
#define WM_ASKCBFORMATNAME              0x030C
#define WM_CHANGECBCHAIN                0x030D
#define WM_HSCROLLCLIPBOARD             0x030E
#define WM_QUERYNEWPALETTE              0x030F
#define WM_PALETTEISCHANGING            0x0310
#define WM_PALETTECHANGED               0x0311
#define WM_HOTKEY                       0x0312

#if(WINVER >= 0x0400)
#define WM_PRINT                        0x0317
#define WM_PRINTCLIENT                  0x0318
#endif /* WINVER >= 0x0400 */

#if(_WIN32_WINNT >= 0x0500)
#define WM_APPCOMMAND                   0x0319
#endif /* _WIN32_WINNT >= 0x0500 */

#if(_WIN32_WINNT >= 0x0501)
#define WM_THEMECHANGED                 0x031A
#endif /* _WIN32_WINNT >= 0x0501 */


#if(_WIN32_WINNT >= 0x0501)
#define WM_CLIPBOARDUPDATE              0x031D
#endif /* _WIN32_WINNT >= 0x0501 */

#if(_WIN32_WINNT >= 0x0600)
#define WM_DWMCOMPOSITIONCHANGED        0x031E
#define WM_DWMNCRENDERINGCHANGED        0x031F
#define WM_DWMCOLORIZATIONCOLORCHANGED  0x0320
#define WM_DWMWINDOWMAXIMIZEDCHANGE     0x0321
#endif /* _WIN32_WINNT >= 0x0600 */

#if(_WIN32_WINNT >= 0x0601)
#define WM_DWMSENDICONICTHUMBNAIL           0x0323
#define WM_DWMSENDICONICLIVEPREVIEWBITMAP   0x0326
#endif /* _WIN32_WINNT >= 0x0601 */


#if(WINVER >= 0x0600)
#define WM_GETTITLEBARINFOEX            0x033F
#endif /* WINVER >= 0x0600 */

#if(WINVER >= 0x0400)
#endif /* WINVER >= 0x0400 */


#if(WINVER >= 0x0400)
#define WM_HANDHELDFIRST                0x0358
#define WM_HANDHELDLAST                 0x035F

#define WM_AFXFIRST                     0x0360
#define WM_AFXLAST                      0x037F
#endif /* WINVER >= 0x0400 */

#define WM_PENWINFIRST                  0x0380
#define WM_PENWINLAST                   0x038F


#if(WINVER >= 0x0400)
#define WM_APP                          0x8000
#endif /* WINVER >= 0x0400 */


/*
 * NOTE: All Message Numbers below 0x0400 are RESERVED.
 *
 * Private Window Messages Start Here:
 */
#define WM_USER                         0x0400

#if(WINVER >= 0x0400)

/*  wParam for WM_SIZING message  */
#define WMSZ_LEFT           1
#define WMSZ_RIGHT          2
#define WMSZ_TOP            3
#define WMSZ_TOPLEFT        4
#define WMSZ_TOPRIGHT       5
#define WMSZ_BOTTOM         6
#define WMSZ_BOTTOMLEFT     7
#define WMSZ_BOTTOMRIGHT    8
#endif /* WINVER >= 0x0400 */

#ifndef NONCMESSAGES

/*
 * WM_NCHITTEST and MOUSEHOOKSTRUCT Mouse Position Codes
 */
#define HTERROR             (-2)
#define HTTRANSPARENT       (-1)
#define HTNOWHERE           0
#define HTCLIENT            1
#define HTCAPTION           2
#define HTSYSMENU           3
#define HTGROWBOX           4
#define HTSIZE              HTGROWBOX
#define HTMENU              5
#define HTHSCROLL           6
#define HTVSCROLL           7
#define HTMINBUTTON         8
#define HTMAXBUTTON         9
#define HTLEFT              10
#define HTRIGHT             11
#define HTTOP               12
#define HTTOPLEFT           13
#define HTTOPRIGHT          14
#define HTBOTTOM            15
#define HTBOTTOMLEFT        16
#define HTBOTTOMRIGHT       17
#define HTBORDER            18
#define HTREDUCE            HTMINBUTTON
#define HTZOOM              HTMAXBUTTON
#define HTSIZEFIRST         HTLEFT
#define HTSIZELAST          HTBOTTOMRIGHT
#if(WINVER >= 0x0400)
#define HTOBJECT            19
#define HTCLOSE             20
#define HTHELP              21
#endif /* WINVER >= 0x0400 */


/*
 * SendMessageTimeout values
 */
#define SMTO_NORMAL         0x0000
#define SMTO_BLOCK          0x0001
#define SMTO_ABORTIFHUNG    0x0002
#if(WINVER >= 0x0500)
#define SMTO_NOTIMEOUTIFNOTHUNG 0x0008
#endif /* WINVER >= 0x0500 */
#if(WINVER >= 0x0600)
#define SMTO_ERRORONEXIT    0x0020
#endif /* WINVER >= 0x0600 */
#if(WINVER >= 0x0602)
#endif /* WINVER >= 0x0602 */

#endif /* !NONCMESSAGES */

/*
 * WM_MOUSEACTIVATE Return Codes
 */
#define MA_ACTIVATE         1
#define MA_ACTIVATEANDEAT   2
#define MA_NOACTIVATE       3
#define MA_NOACTIVATEANDEAT 4

/*
 * WM_SETICON / WM_GETICON Type Codes
 */
#define ICON_SMALL          0
#define ICON_BIG            1
#if(_WIN32_WINNT >= 0x0501)
#define ICON_SMALL2         2
#endif /* _WIN32_WINNT >= 0x0501 */


#pragma region Desktop Family
#if WINAPI_FAMILY_PARTITION(WINAPI_PARTITION_DESKTOP)

_Win32_metadata_set_last_error_
WINUSERAPI
UINT
WINAPI
RegisterWindowMessageA(
    _In_ LPCSTR lpString);
_Win32_metadata_set_last_error_
WINUSERAPI
UINT
WINAPI
RegisterWindowMessageW(
    _In_ LPCWSTR lpString);
#ifdef UNICODE
#define RegisterWindowMessage  RegisterWindowMessageW
#else
#define RegisterWindowMessage  RegisterWindowMessageA
#endif // !UNICODE

#endif /* WINAPI_FAMILY_PARTITION(WINAPI_PARTITION_DESKTOP) */
#pragma endregion


/*
 * WM_SIZE message wParam values
 */
#define SIZE_RESTORED       0
#define SIZE_MINIMIZED      1
#define SIZE_MAXIMIZED      2
#define SIZE_MAXSHOW        3
#define SIZE_MAXHIDE        4

/*
 * Obsolete constant names
 */
#define SIZENORMAL          SIZE_RESTORED
#define SIZEICONIC          SIZE_MINIMIZED
#define SIZEFULLSCREEN      SIZE_MAXIMIZED
#define SIZEZOOMSHOW        SIZE_MAXSHOW
#define SIZEZOOMHIDE        SIZE_MAXHIDE


#pragma region Desktop Family
#if WINAPI_FAMILY_PARTITION(WINAPI_PARTITION_DESKTOP)

/*
 * WM_WINDOWPOSCHANGING/CHANGED struct pointed to by lParam
 */
typedef struct tagWINDOWPOS {
    HWND    hwnd;
    HWND    hwndInsertAfter;
    int     x;
    int     y;
    int     cx;
    int     cy;
    SET_WINDOW_POS_FLAGS    flags;
} WINDOWPOS, *LPWINDOWPOS, *PWINDOWPOS;

/*
 * WM_NCCALCSIZE parameter structure
 */
typedef struct tagNCCALCSIZE_PARAMS {
    RECT       rgrc[3];
    PWINDOWPOS lppos;
} NCCALCSIZE_PARAMS, *LPNCCALCSIZE_PARAMS;

#endif /* WINAPI_FAMILY_PARTITION(WINAPI_PARTITION_DESKTOP) */
#pragma endregion

/*
 * WM_NCCALCSIZE "window valid rect" return values
 */
#define WVR_ALIGNTOP        0x0010
#define WVR_ALIGNLEFT       0x0020
#define WVR_ALIGNBOTTOM     0x0040
#define WVR_ALIGNRIGHT      0x0080
#define WVR_HREDRAW         0x0100
#define WVR_VREDRAW         0x0200
#define WVR_REDRAW         (WVR_HREDRAW | \
                            WVR_VREDRAW)
#define WVR_VALIDRECTS      0x0400


#ifndef NOKEYSTATES

/*
 * Key State Masks for Mouse Messages
 */
#define MK_LBUTTON          0x0001
#define MK_RBUTTON          0x0002
#define MK_SHIFT            0x0004
#define MK_CONTROL          0x0008
#define MK_MBUTTON          0x0010
#if(_WIN32_WINNT >= 0x0500)
#define MK_XBUTTON1         0x0020
#define MK_XBUTTON2         0x0040
#endif /* _WIN32_WINNT >= 0x0500 */

#endif /* !NOKEYSTATES */


#if(_WIN32_WINNT >= 0x0400)
#ifndef NOTRACKMOUSEEVENT

#define TME_HOVER       0x00000001
#define TME_LEAVE       0x00000002
#if(WINVER >= 0x0500)
#define TME_NONCLIENT   0x00000010
#endif /* WINVER >= 0x0500 */
#define TME_QUERY       0x40000000
#define TME_CANCEL      0x80000000


#define HOVER_DEFAULT   0xFFFFFFFF
#endif /* _WIN32_WINNT >= 0x0400 */

#if(_WIN32_WINNT >= 0x0400)

#pragma region Desktop Family
#if WINAPI_FAMILY_PARTITION(WINAPI_PARTITION_DESKTOP)

typedef struct tagTRACKMOUSEEVENT {
    DWORD cbSize;
    DWORD dwFlags;
    HWND  hwndTrack;
    DWORD dwHoverTime;
} TRACKMOUSEEVENT, *LPTRACKMOUSEEVENT;

_Win32_metadata_set_last_error_
WINUSERAPI
BOOL
WINAPI
TrackMouseEvent(
    _Inout_ LPTRACKMOUSEEVENT lpEventTrack);

#endif /* WINAPI_FAMILY_PARTITION(WINAPI_PARTITION_DESKTOP) */
#pragma endregion

#endif /* _WIN32_WINNT >= 0x0400 */

#if(_WIN32_WINNT >= 0x0400)

#endif /* !NOTRACKMOUSEEVENT */
#endif /* _WIN32_WINNT >= 0x0400 */


#endif /* !NOWINMESSAGES */

#ifndef NOWINSTYLES


/*
 * Window Styles
 */
#define WS_OVERLAPPED       0x00000000L
#define WS_POPUP            0x80000000L
#define WS_CHILD            0x40000000L
#define WS_MINIMIZE         0x20000000L
#define WS_VISIBLE          0x10000000L
#define WS_DISABLED         0x08000000L
#define WS_CLIPSIBLINGS     0x04000000L
#define WS_CLIPCHILDREN     0x02000000L
#define WS_MAXIMIZE         0x01000000L
#define WS_CAPTION          0x00C00000L     /* WS_BORDER | WS_DLGFRAME  */
#define WS_BORDER           0x00800000L
#define WS_DLGFRAME         0x00400000L
#define WS_VSCROLL          0x00200000L
#define WS_HSCROLL          0x00100000L
#define WS_SYSMENU          0x00080000L
#define WS_THICKFRAME       0x00040000L
#define WS_GROUP            0x00020000L
#define WS_TABSTOP          0x00010000L

#define WS_MINIMIZEBOX      0x00020000L
#define WS_MAXIMIZEBOX      0x00010000L


#define WS_TILED            WS_OVERLAPPED
#define WS_ICONIC           WS_MINIMIZE
#define WS_SIZEBOX          WS_THICKFRAME
#define WS_TILEDWINDOW      WS_OVERLAPPEDWINDOW

/*
 * Common Window Styles
 */
#define WS_OVERLAPPEDWINDOW (WS_OVERLAPPED     | \
                             WS_CAPTION        | \
                             WS_SYSMENU        | \
                             WS_THICKFRAME     | \
                             WS_MINIMIZEBOX    | \
                             WS_MAXIMIZEBOX)

#define WS_POPUPWINDOW      (WS_POPUP          | \
                             WS_BORDER         | \
                             WS_SYSMENU)

#define WS_CHILDWINDOW      (WS_CHILD)

/*
 * Extended Window Styles
 */
#define WS_EX_DLGMODALFRAME     0x00000001L
#define WS_EX_NOPARENTNOTIFY    0x00000004L
#define WS_EX_TOPMOST           0x00000008L
#define WS_EX_ACCEPTFILES       0x00000010L
#define WS_EX_TRANSPARENT       0x00000020L
#if(WINVER >= 0x0400)
#define WS_EX_MDICHILD          0x00000040L
#define WS_EX_TOOLWINDOW        0x00000080L
#define WS_EX_WINDOWEDGE        0x00000100L
#define WS_EX_CLIENTEDGE        0x00000200L
#define WS_EX_CONTEXTHELP       0x00000400L

#endif /* WINVER >= 0x0400 */
#if(WINVER >= 0x0400)

#define WS_EX_RIGHT             0x00001000L
#define WS_EX_LEFT              0x00000000L
#define WS_EX_RTLREADING        0x00002000L
#define WS_EX_LTRREADING        0x00000000L
#define WS_EX_LEFTSCROLLBAR     0x00004000L
#define WS_EX_RIGHTSCROLLBAR    0x00000000L

#define WS_EX_CONTROLPARENT     0x00010000L
#define WS_EX_STATICEDGE        0x00020000L
#define WS_EX_APPWINDOW         0x00040000L


#define WS_EX_OVERLAPPEDWINDOW  (WS_EX_WINDOWEDGE | WS_EX_CLIENTEDGE)
#define WS_EX_PALETTEWINDOW     (WS_EX_WINDOWEDGE | WS_EX_TOOLWINDOW | WS_EX_TOPMOST)

#endif /* WINVER >= 0x0400 */

#if(_WIN32_WINNT >= 0x0500)
#define WS_EX_LAYERED           0x00080000

#endif /* _WIN32_WINNT >= 0x0500 */


#if(WINVER >= 0x0500)
#define WS_EX_NOINHERITLAYOUT   0x00100000L // Disable inheritence of mirroring by children
#endif /* WINVER >= 0x0500 */

#if(WINVER >= 0x0602)
#define WS_EX_NOREDIRECTIONBITMAP 0x00200000L
#endif /* WINVER >= 0x0602 */

#if(WINVER >= 0x0500)
#define WS_EX_LAYOUTRTL         0x00400000L // Right to left mirroring
#endif /* WINVER >= 0x0500 */

#if(_WIN32_WINNT >= 0x0501)
#define WS_EX_COMPOSITED        0x02000000L
#endif /* _WIN32_WINNT >= 0x0501 */
#if(_WIN32_WINNT >= 0x0500)
#define WS_EX_NOACTIVATE        0x08000000L
#endif /* _WIN32_WINNT >= 0x0500 */


/*
 * Class styles
 */
#define CS_VREDRAW          0x0001
#define CS_HREDRAW          0x0002
#define CS_DBLCLKS          0x0008
#define CS_OWNDC            0x0020
#define CS_CLASSDC          0x0040
#define CS_PARENTDC         0x0080
#define CS_NOCLOSE          0x0200
#define CS_SAVEBITS         0x0800
#define CS_BYTEALIGNCLIENT  0x1000
#define CS_BYTEALIGNWINDOW  0x2000
#define CS_GLOBALCLASS      0x4000

#define CS_IME              0x00010000
#if(_WIN32_WINNT >= 0x0501)
#define CS_DROPSHADOW       0x00020000
#endif /* _WIN32_WINNT >= 0x0501 */



#endif /* !NOWINSTYLES */
#if(WINVER >= 0x0400)
/* WM_PRINT flags */
#define PRF_CHECKVISIBLE    0x00000001L
#define PRF_NONCLIENT       0x00000002L
#define PRF_CLIENT          0x00000004L
#define PRF_ERASEBKGND      0x00000008L
#define PRF_CHILDREN        0x00000010L
#define PRF_OWNED           0x00000020L

/* 3D border styles */
#define BDR_RAISEDOUTER 0x0001
#define BDR_SUNKENOUTER 0x0002
#define BDR_RAISEDINNER 0x0004
#define BDR_SUNKENINNER 0x0008

#define BDR_OUTER       (BDR_RAISEDOUTER | BDR_SUNKENOUTER)
#define BDR_INNER       (BDR_RAISEDINNER | BDR_SUNKENINNER)
#define BDR_RAISED      (BDR_RAISEDOUTER | BDR_RAISEDINNER)
#define BDR_SUNKEN      (BDR_SUNKENOUTER | BDR_SUNKENINNER)


#define EDGE_RAISED     (BDR_RAISEDOUTER | BDR_RAISEDINNER)
#define EDGE_SUNKEN     (BDR_SUNKENOUTER | BDR_SUNKENINNER)
#define EDGE_ETCHED     (BDR_SUNKENOUTER | BDR_RAISEDINNER)
#define EDGE_BUMP       (BDR_RAISEDOUTER | BDR_SUNKENINNER)

/* Border flags */
#define BF_LEFT         0x0001
#define BF_TOP          0x0002
#define BF_RIGHT        0x0004
#define BF_BOTTOM       0x0008

#define BF_TOPLEFT      (BF_TOP | BF_LEFT)
#define BF_TOPRIGHT     (BF_TOP | BF_RIGHT)
#define BF_BOTTOMLEFT   (BF_BOTTOM | BF_LEFT)
#define BF_BOTTOMRIGHT  (BF_BOTTOM | BF_RIGHT)
#define BF_RECT         (BF_LEFT | BF_TOP | BF_RIGHT | BF_BOTTOM)

#define BF_DIAGONAL     0x0010

// For diagonal lines, the BF_RECT flags specify the end point of the
// vector bounded by the rectangle parameter.
#define BF_DIAGONAL_ENDTOPRIGHT     (BF_DIAGONAL | BF_TOP | BF_RIGHT)
#define BF_DIAGONAL_ENDTOPLEFT      (BF_DIAGONAL | BF_TOP | BF_LEFT)
#define BF_DIAGONAL_ENDBOTTOMLEFT   (BF_DIAGONAL | BF_BOTTOM | BF_LEFT)
#define BF_DIAGONAL_ENDBOTTOMRIGHT  (BF_DIAGONAL | BF_BOTTOM | BF_RIGHT)


#define BF_MIDDLE       0x0800  /* Fill in the middle */
#define BF_SOFT         0x1000  /* For softer buttons */
#define BF_ADJUST       0x2000  /* Calculate the space left over */
#define BF_FLAT         0x4000  /* For flat rather than 3D borders */
#define BF_MONO         0x8000  /* For monochrome borders */


#pragma region Desktop Family
#if WINAPI_FAMILY_PARTITION(WINAPI_PARTITION_DESKTOP)

WINUSERAPI
BOOL
WINAPI
DrawEdge(
    _In_ HDC hdc,
    _Inout_ LPRECT qrc,
    _In_ UINT edge,
    _In_ UINT grfFlags);

#endif /* WINAPI_FAMILY_PARTITION(WINAPI_PARTITION_DESKTOP) */
#pragma endregion

/* flags for DrawFrameControl */

#define DFC_CAPTION             1
#define DFC_MENU                2
#define DFC_SCROLL              3
#define DFC_BUTTON              4
#if(WINVER >= 0x0500)
#define DFC_POPUPMENU           5
#endif /* WINVER >= 0x0500 */

#define DFCS_CAPTIONCLOSE       0x0000
#define DFCS_CAPTIONMIN         0x0001
#define DFCS_CAPTIONMAX         0x0002
#define DFCS_CAPTIONRESTORE     0x0003
#define DFCS_CAPTIONHELP        0x0004

#define DFCS_MENUARROW          0x0000
#define DFCS_MENUCHECK          0x0001
#define DFCS_MENUBULLET         0x0002
#define DFCS_MENUARROWRIGHT     0x0004
#define DFCS_SCROLLUP           0x0000
#define DFCS_SCROLLDOWN         0x0001
#define DFCS_SCROLLLEFT         0x0002
#define DFCS_SCROLLRIGHT        0x0003
#define DFCS_SCROLLCOMBOBOX     0x0005
#define DFCS_SCROLLSIZEGRIP     0x0008
#define DFCS_SCROLLSIZEGRIPRIGHT 0x0010

#define DFCS_BUTTONCHECK        0x0000
#define DFCS_BUTTONRADIOIMAGE   0x0001
#define DFCS_BUTTONRADIOMASK    0x0002
#define DFCS_BUTTONRADIO        0x0004
#define DFCS_BUTTON3STATE       0x0008
#define DFCS_BUTTONPUSH         0x0010

#define DFCS_INACTIVE           0x0100
#define DFCS_PUSHED             0x0200
#define DFCS_CHECKED            0x0400

#if(WINVER >= 0x0500)
#define DFCS_TRANSPARENT        0x0800
#define DFCS_HOT                0x1000
#endif /* WINVER >= 0x0500 */

#define DFCS_ADJUSTRECT         0x2000
#define DFCS_FLAT               0x4000
#define DFCS_MONO               0x8000

#pragma region Desktop Family
#if WINAPI_FAMILY_PARTITION(WINAPI_PARTITION_DESKTOP)

WINUSERAPI
BOOL
WINAPI
DrawFrameControl(
    HDC hdc,
    _Inout_ LPRECT lprc,
    DFC_TYPE uType,
    DFCS_STATE uState);


#endif /* WINAPI_FAMILY_PARTITION(WINAPI_PARTITION_DESKTOP) */
#pragma endregion


/* flags for DrawCaption */
#define DC_ACTIVE           0x0001
#define DC_SMALLCAP         0x0002
#define DC_ICON             0x0004
#define DC_TEXT             0x0008
#define DC_INBUTTON         0x0010
#if(WINVER >= 0x0500)
#define DC_GRADIENT         0x0020
#endif /* WINVER >= 0x0500 */
#if(_WIN32_WINNT >= 0x0501)
#define DC_BUTTONS          0x1000
#endif /* _WIN32_WINNT >= 0x0501 */

#pragma region Desktop Family
#if WINAPI_FAMILY_PARTITION(WINAPI_PARTITION_DESKTOP)

WINUSERAPI
BOOL
WINAPI
DrawCaption(
    _In_ HWND hwnd,
    _In_ HDC hdc,
    _In_ CONST RECT * lprect,
    _In_ DRAW_CAPTION_FLAGS flags);


#endif /* WINAPI_FAMILY_PARTITION(WINAPI_PARTITION_DESKTOP) */
#pragma endregion

#define IDANI_OPEN          1
#define IDANI_CAPTION       3

#pragma region Desktop Family
#if WINAPI_FAMILY_PARTITION(WINAPI_PARTITION_DESKTOP)

WINUSERAPI
BOOL
WINAPI
DrawAnimatedRects(
    _In_opt_ HWND hwnd,
    _In_ int idAni,
    _In_ CONST RECT *lprcFrom,
    _In_ CONST RECT *lprcTo);

#endif /* WINAPI_FAMILY_PARTITION(WINAPI_PARTITION_DESKTOP) */
#pragma endregion

#endif /* WINVER >= 0x0400 */

#ifndef NOCLIPBOARD


/*
 * Predefined Clipboard Formats
 */
#define CF_TEXT             1
#define CF_BITMAP           2
#define CF_METAFILEPICT     3
#define CF_SYLK             4
#define CF_DIF              5
#define CF_TIFF             6
#define CF_OEMTEXT          7
#define CF_DIB              8
#define CF_PALETTE          9
#define CF_PENDATA          10
#define CF_RIFF             11
#define CF_WAVE             12
#define CF_UNICODETEXT      13
#define CF_ENHMETAFILE      14
#if(WINVER >= 0x0400)
#define CF_HDROP            15
#define CF_LOCALE           16
#endif /* WINVER >= 0x0400 */
#if(WINVER >= 0x0500)
#define CF_DIBV5            17
#endif /* WINVER >= 0x0500 */

#if(WINVER >= 0x0500)
#define CF_MAX              18
#elif(WINVER >= 0x0400)
#define CF_MAX              17
#else
#define CF_MAX              15
#endif

#define CF_OWNERDISPLAY     0x0080
#define CF_DSPTEXT          0x0081
#define CF_DSPBITMAP        0x0082
#define CF_DSPMETAFILEPICT  0x0083
#define CF_DSPENHMETAFILE   0x008E

/*
 * "Private" formats don't get GlobalFree()'d
 */
#define CF_PRIVATEFIRST     0x0200
#define CF_PRIVATELAST      0x02FF

/*
 * "GDIOBJ" formats do get DeleteObject()'d
 */
#define CF_GDIOBJFIRST      0x0300
#define CF_GDIOBJLAST       0x03FF


#endif /* !NOCLIPBOARD */

/*
 * Defines for the fVirt field of the Accelerator table structure.
 */
#define FVIRTKEY  TRUE          /* Assumed to be == TRUE */
#define FNOINVERT 0x02
#define FSHIFT    0x04
#define FCONTROL  0x08
#define FALT      0x10

#pragma region Desktop Family
#if WINAPI_FAMILY_PARTITION(WINAPI_PARTITION_DESKTOP)

typedef struct tagACCEL {
#ifndef _MAC
    BYTE   fVirt;               /* Also called the flags field */
    WORD   key;
    WORD   cmd;
#else
    WORD   fVirt;               /* Also called the flags field */
    WORD   key;
    DWORD  cmd;
#endif
} ACCEL, *LPACCEL;

typedef struct tagPAINTSTRUCT {
    HDC         hdc;
    BOOL        fErase;
    RECT        rcPaint;
    BOOL        fRestore;
    BOOL        fIncUpdate;
    BYTE        rgbReserved[32];
} PAINTSTRUCT, *PPAINTSTRUCT, *NPPAINTSTRUCT, *LPPAINTSTRUCT;

#endif /* WINAPI_FAMILY_PARTITION(WINAPI_PARTITION_DESKTOP) */
#pragma endregion

#pragma region Desktop Family or Games Family
#if WINAPI_FAMILY_PARTITION(WINAPI_PARTITION_DESKTOP | WINAPI_PARTITION_GAMES)

typedef struct tagCREATESTRUCTA {
    LPVOID      lpCreateParams;
    HINSTANCE   hInstance;
    HMENU       hMenu;
    HWND        hwndParent;
    int         cy;
    int         cx;
    int         y;
    int         x;
    LONG        style;
    LPCSTR      lpszName;
    LPCSTR      lpszClass;
    WINDOW_EX_STYLE       dwExStyle;
} CREATESTRUCTA, *LPCREATESTRUCTA;
typedef struct tagCREATESTRUCTW {
    LPVOID      lpCreateParams;
    HINSTANCE   hInstance;
    HMENU       hMenu;
    HWND        hwndParent;
    int         cy;
    int         cx;
    int         y;
    int         x;
    LONG        style;
    LPCWSTR     lpszName;
    LPCWSTR     lpszClass;
    WINDOW_EX_STYLE       dwExStyle;
} CREATESTRUCTW, *LPCREATESTRUCTW;
#ifdef UNICODE
typedef CREATESTRUCTW CREATESTRUCT;
typedef LPCREATESTRUCTW LPCREATESTRUCT;
#else
typedef CREATESTRUCTA CREATESTRUCT;
typedef LPCREATESTRUCTA LPCREATESTRUCT;
#endif // UNICODE

#endif /* WINAPI_FAMILY_PARTITION(WINAPI_PARTITION_DESKTOP | WINAPI_PARTITION_GAMES) */
#pragma endregion

#pragma region Desktop Family
#if WINAPI_FAMILY_PARTITION(WINAPI_PARTITION_DESKTOP)

typedef struct tagWINDOWPLACEMENT {
    UINT  length;
    WINDOWPLACEMENT_FLAGS  flags;
    UINT  showCmd;
    POINT ptMinPosition;
    POINT ptMaxPosition;
    RECT  rcNormalPosition;
#ifdef _MAC
    RECT  rcDevice;
#endif
} WINDOWPLACEMENT;
typedef WINDOWPLACEMENT *PWINDOWPLACEMENT, *LPWINDOWPLACEMENT;

#define WPF_SETMINPOSITION          0x0001
#define WPF_RESTORETOMAXIMIZED      0x0002
#if(_WIN32_WINNT >= 0x0500)
#define WPF_ASYNCWINDOWPLACEMENT    0x0004
#endif /* _WIN32_WINNT >= 0x0500 */

#endif /* WINAPI_FAMILY_PARTITION(WINAPI_PARTITION_DESKTOP) */
#pragma endregion

#if(WINVER >= 0x0400)

#pragma region Application Family or OneCore Family
#if WINAPI_FAMILY_PARTITION(WINAPI_PARTITION_APP | WINAPI_PARTITION_SYSTEM)

typedef struct tagNMHDR
{
    HWND      hwndFrom;
    UINT_PTR  idFrom;
    UINT      code;         // NM_ code
}   NMHDR;

#endif /* WINAPI_FAMILY_PARTITION(WINAPI_PARTITION_APP | WINAPI_PARTITION_SYSTEM) */
#pragma endregion

#pragma region Desktop Family
#if WINAPI_FAMILY_PARTITION(WINAPI_PARTITION_DESKTOP)

typedef NMHDR FAR * LPNMHDR;

typedef struct tagSTYLESTRUCT
{
    DWORD   styleOld;
    DWORD   styleNew;
} STYLESTRUCT, * LPSTYLESTRUCT;

#endif /* WINAPI_FAMILY_PARTITION(WINAPI_PARTITION_DESKTOP) */
#pragma endregion
#endif /* WINVER >= 0x0400 */


/*
 * Owner draw control types
 */
#define ODT_MENU        1
#define ODT_LISTBOX     2
#define ODT_COMBOBOX    3
#define ODT_BUTTON      4
#if(WINVER >= 0x0400)
#define ODT_STATIC      5
#endif /* WINVER >= 0x0400 */

/*
 * Owner draw actions
 */
#define ODA_DRAWENTIRE  0x0001
#define ODA_SELECT      0x0002
#define ODA_FOCUS       0x0004

/*
 * Owner draw state
 */
#define ODS_SELECTED    0x0001
#define ODS_GRAYED      0x0002
#define ODS_DISABLED    0x0004
#define ODS_CHECKED     0x0008
#define ODS_FOCUS       0x0010
#if(WINVER >= 0x0400)
#define ODS_DEFAULT         0x0020
#define ODS_COMBOBOXEDIT    0x1000
#endif /* WINVER >= 0x0400 */
#if(WINVER >= 0x0500)
#define ODS_HOTLIGHT        0x0040
#define ODS_INACTIVE        0x0080
#if(_WIN32_WINNT >= 0x0500)
#define ODS_NOACCEL         0x0100
#define ODS_NOFOCUSRECT     0x0200
#endif /* _WIN32_WINNT >= 0x0500 */
#endif /* WINVER >= 0x0500 */

#pragma region Desktop Family
#if WINAPI_FAMILY_PARTITION(WINAPI_PARTITION_DESKTOP)

/*
 * MEASUREITEMSTRUCT for ownerdraw
 */
typedef struct tagMEASUREITEMSTRUCT {
    DRAWITEMSTRUCT_CTL_TYPE       CtlType;
    UINT       CtlID;
    UINT       itemID;
    UINT       itemWidth;
    UINT       itemHeight;
    ULONG_PTR  itemData;
} MEASUREITEMSTRUCT, NEAR *PMEASUREITEMSTRUCT, FAR *LPMEASUREITEMSTRUCT;

/*
 * DRAWITEMSTRUCT for ownerdraw
 */
typedef struct tagDRAWITEMSTRUCT {
    DRAWITEMSTRUCT_CTL_TYPE        CtlType;
    UINT        CtlID;
    UINT        itemID;
    ODA_FLAGS        itemAction;
    ODS_FLAGS        itemState;
    HWND        hwndItem;
    HDC         hDC;
    RECT        rcItem;
    ULONG_PTR   itemData;
} DRAWITEMSTRUCT, NEAR *PDRAWITEMSTRUCT, FAR *LPDRAWITEMSTRUCT;

/*
 * DELETEITEMSTRUCT for ownerdraw
 */
typedef struct tagDELETEITEMSTRUCT {
    DRAWITEMSTRUCT_CTL_TYPE       CtlType;
    UINT       CtlID;
    UINT       itemID;
    HWND       hwndItem;
    ULONG_PTR  itemData;
} DELETEITEMSTRUCT, NEAR *PDELETEITEMSTRUCT, FAR *LPDELETEITEMSTRUCT;

/*
 * COMPAREITEMSTUCT for ownerdraw sorting
 */
typedef struct tagCOMPAREITEMSTRUCT {
    DRAWITEMSTRUCT_CTL_TYPE        CtlType;
    UINT        CtlID;
    HWND        hwndItem;
    UINT        itemID1;
    ULONG_PTR   itemData1;
    UINT        itemID2;
    ULONG_PTR   itemData2;
    DWORD       dwLocaleId;
} COMPAREITEMSTRUCT, NEAR *PCOMPAREITEMSTRUCT, FAR *LPCOMPAREITEMSTRUCT;

#endif /* WINAPI_FAMILY_PARTITION(WINAPI_PARTITION_DESKTOP) */
#pragma endregion

#ifndef NOMSG

#pragma region Desktop Family or Games Family
#if WINAPI_FAMILY_PARTITION(WINAPI_PARTITION_DESKTOP | WINAPI_PARTITION_GAMES)

/*
 * Message Function Templates
 */

_Win32_metadata_set_last_error_
WINUSERAPI
BOOL
WINAPI
GetMessageA(
    _Out_ LPMSG lpMsg,
    _In_opt_ HWND hWnd,
    _In_ UINT wMsgFilterMin,
    _In_ UINT wMsgFilterMax);
_Win32_metadata_set_last_error_
WINUSERAPI
BOOL
WINAPI
GetMessageW(
    _Out_ LPMSG lpMsg,
    _In_opt_ HWND hWnd,
    _In_ UINT wMsgFilterMin,
    _In_ UINT wMsgFilterMax);
#ifdef UNICODE
#define GetMessage  GetMessageW
#else
#define GetMessage  GetMessageA
#endif // !UNICODE

#if defined(_M_CEE)
#undef GetMessage
__inline
BOOL
GetMessage(
    LPMSG lpMsg,
    HWND hWnd,
    UINT wMsgFilterMin,
    UINT wMsgFilterMax
    )
{
#ifdef UNICODE
    return GetMessageW(
#else
    return GetMessageA(
#endif
        lpMsg,
        hWnd,
        wMsgFilterMin,
        wMsgFilterMax
        );
}
#endif  /* _M_CEE */


WINUSERAPI
BOOL
WINAPI
TranslateMessage(
    _In_ CONST MSG *lpMsg);

WINUSERAPI
LRESULT
WINAPI
DispatchMessageA(
    _In_ CONST MSG *lpMsg);
WINUSERAPI
LRESULT
WINAPI
DispatchMessageW(
    _In_ CONST MSG *lpMsg);
#ifdef UNICODE
#define DispatchMessage  DispatchMessageW
#else
#define DispatchMessage  DispatchMessageA
#endif // !UNICODE

#if defined(_M_CEE)
#undef DispatchMessage
__inline
LRESULT
DispatchMessage(
    CONST MSG *lpMsg
    )
{
#ifdef UNICODE
    return DispatchMessageW(
#else
    return DispatchMessageA(
#endif
        lpMsg
        );
}
#endif  /* _M_CEE */

#endif /* WINAPI_FAMILY_PARTITION(WINAPI_PARTITION_DESKTOP | WINAPI_PARTITION_GAMES) */
#pragma endregion

#pragma region Desktop Family
#if WINAPI_FAMILY_PARTITION(WINAPI_PARTITION_DESKTOP)

WINUSERAPI
BOOL
WINAPI
SetMessageQueue(
    _In_ int cMessagesMax);

#endif /* WINAPI_FAMILY_PARTITION(WINAPI_PARTITION_DESKTOP) */
#pragma endregion

#pragma region Desktop Family or Games Family
#if WINAPI_FAMILY_PARTITION(WINAPI_PARTITION_DESKTOP | WINAPI_PARTITION_GAMES)

WINUSERAPI
BOOL
WINAPI
PeekMessageA(
    _Out_ LPMSG lpMsg,
    _In_opt_ HWND hWnd,
    _In_ UINT wMsgFilterMin,
    _In_ UINT wMsgFilterMax,
    _In_ PEEK_MESSAGE_REMOVE_TYPE wRemoveMsg);
WINUSERAPI
BOOL
WINAPI
PeekMessageW(
    _Out_ LPMSG lpMsg,
    _In_opt_ HWND hWnd,
    _In_ UINT wMsgFilterMin,
    _In_ UINT wMsgFilterMax,
    _In_ PEEK_MESSAGE_REMOVE_TYPE wRemoveMsg);
#ifdef UNICODE
#define PeekMessage  PeekMessageW
#else
#define PeekMessage  PeekMessageA
#endif // !UNICODE

#endif /* WINAPI_FAMILY_PARTITION(WINAPI_PARTITION_DESKTOP | WINAPI_PARTITION_GAMES) */
#pragma endregion

/*
 * PeekMessage() Options
 */
#define PM_NOREMOVE         0x0000
#define PM_REMOVE           0x0001
#define PM_NOYIELD          0x0002


#if(WINVER >= 0x0500)
#define PM_QS_INPUT         (QS_INPUT << 16)
#define PM_QS_POSTMESSAGE   ((QS_POSTMESSAGE | QS_HOTKEY | QS_TIMER) << 16)
#define PM_QS_PAINT         (QS_PAINT << 16)
#define PM_QS_SENDMESSAGE   (QS_SENDMESSAGE << 16)
#endif /* WINVER >= 0x0500 */


#endif /* !NOMSG */

#pragma region Desktop Family
#if WINAPI_FAMILY_PARTITION(WINAPI_PARTITION_DESKTOP)

_Win32_metadata_set_last_error_
WINUSERAPI
BOOL
WINAPI
RegisterHotKey(
    _In_opt_ HWND hWnd,
    _In_ int id,
    _In_ UINT fsModifiers,
    _In_ UINT vk);

_Win32_metadata_set_last_error_
WINUSERAPI
BOOL
WINAPI
UnregisterHotKey(
    _In_opt_ HWND hWnd,
    _In_ int id);

#endif /* WINAPI_FAMILY_PARTITION(WINAPI_PARTITION_DESKTOP) */
#pragma endregion

#define MOD_ALT             0x0001
#define MOD_CONTROL         0x0002
#define MOD_SHIFT           0x0004
#define MOD_WIN             0x0008
#if(WINVER >= 0x0601)
#define MOD_NOREPEAT        0x4000
#endif /* WINVER >= 0x0601 */


#define IDHOT_SNAPWINDOW        (-1)    /* SHIFT-PRINTSCRN  */
#define IDHOT_SNAPDESKTOP       (-2)    /* PRINTSCRN        */

#ifdef WIN_INTERNAL
    #ifndef LSTRING
    #define NOLSTRING
    #endif /* LSTRING */
    #ifndef LFILEIO
    #define NOLFILEIO
    #endif /* LFILEIO */
#endif /* WIN_INTERNAL */

#if(WINVER >= 0x0400)
#endif /* WINVER >= 0x0400 */

#if(_WIN32_WINNT >= 0x0400)
#define ENDSESSION_CLOSEAPP         0x00000001
#endif /* _WIN32_WINNT >= 0x0400 */
#if(_WIN32_WINNT >= 0x0400)
#define ENDSESSION_CRITICAL         0x40000000
#endif /* _WIN32_WINNT >= 0x0400 */
#if(_WIN32_WINNT >= 0x0400)
#define ENDSESSION_LOGOFF           0x80000000
#endif /* _WIN32_WINNT >= 0x0400 */

#define EWX_LOGOFF                  0x00000000
#define EWX_SHUTDOWN                0x00000001
#define EWX_REBOOT                  0x00000002
#define EWX_FORCE                   0x00000004
#define EWX_POWEROFF                0x00000008
#if(_WIN32_WINNT >= 0x0500)
#define EWX_FORCEIFHUNG             0x00000010
#endif /* _WIN32_WINNT >= 0x0500 */
#define EWX_QUICKRESOLVE            0x00000020
#if(_WIN32_WINNT >= 0x0600)
#define EWX_RESTARTAPPS             0x00000040
#endif /* _WIN32_WINNT >= 0x0600 */
#define EWX_HYBRID_SHUTDOWN         0x00400000
#define EWX_BOOTOPTIONS             0x01000000
#define EWX_ARSO                    0x04000000
#define EWX_CHECK_SAFE_FOR_SERVER   0x08000000
#define EWX_SYSTEM_INITIATED        0x10000000

#pragma region Desktop Family
#if WINAPI_FAMILY_PARTITION(WINAPI_PARTITION_DESKTOP)

#define ExitWindows(dwReserved, Code) ExitWindowsEx(EWX_LOGOFF, 0xFFFFFFFF)

_When_((uFlags&(EWX_POWEROFF|EWX_SHUTDOWN|EWX_FORCE))!=0,
    __drv_preferredFunction("InitiateSystemShutdownEx",
        "Legacy API. Rearchitect to avoid Reboot"))
_Win32_metadata_set_last_error_
WINUSERAPI
BOOL
WINAPI
ExitWindowsEx(
    _In_ EXIT_WINDOWS_FLAGS uFlags,
    _In_ DWORD dwReason);

WINUSERAPI
BOOL
WINAPI
SwapMouseButton(
    _In_ BOOL fSwap);

WINUSERAPI
DWORD
WINAPI
GetMessagePos(
    VOID);

WINUSERAPI
LONG
WINAPI
GetMessageTime(
    VOID);

WINUSERAPI
LPARAM
WINAPI
GetMessageExtraInfo(
    VOID);

#if(_WIN32_WINNT >= 0x0602)
WINUSERAPI
DWORD
WINAPI
GetUnpredictedMessagePos(
    VOID);
#endif /* _WIN32_WINNT >= 0x0602 */

#if(_WIN32_WINNT >= 0x0501)
WINUSERAPI
BOOL
WINAPI
IsWow64Message(
    VOID);
#endif /* _WIN32_WINNT >= 0x0501 */

#if(WINVER >= 0x0400)
WINUSERAPI
LPARAM
WINAPI
SetMessageExtraInfo(
    _In_ LPARAM lParam);
#endif /* WINVER >= 0x0400 */

#endif /* WINAPI_FAMILY_PARTITION(WINAPI_PARTITION_DESKTOP) */
#pragma endregion

#pragma region Desktop Family or Games Family
#if WINAPI_FAMILY_PARTITION(WINAPI_PARTITION_DESKTOP | WINAPI_PARTITION_GAMES)

_Win32_metadata_set_last_error_
WINUSERAPI
LRESULT
WINAPI
SendMessageA(
    _In_ HWND hWnd,
    _In_ UINT Msg,
    _Pre_maybenull_ _Post_valid_ WPARAM wParam,
    _Pre_maybenull_ _Post_valid_ LPARAM lParam);
_Win32_metadata_set_last_error_
WINUSERAPI
LRESULT
WINAPI
SendMessageW(
    _In_ HWND hWnd,
    _In_ UINT Msg,
    _Pre_maybenull_ _Post_valid_ WPARAM wParam,
    _Pre_maybenull_ _Post_valid_ LPARAM lParam);
#ifdef UNICODE
#define SendMessage  SendMessageW
#else
#define SendMessage  SendMessageA
#endif // !UNICODE

#if defined(_M_CEE)
#undef SendMessage
__inline
LRESULT
SendMessage(
    HWND hWnd,
    UINT Msg,
    WPARAM wParam,
    LPARAM lParam
    )
{
#ifdef UNICODE
    return SendMessageW(
#else
    return SendMessageA(
#endif
        hWnd,
        Msg,
        wParam,
        lParam
        );
}
#endif  /* _M_CEE */

#endif /* WINAPI_FAMILY_PARTITION(WINAPI_PARTITION_DESKTOP | WINAPI_PARTITION_GAMES) */
#pragma endregion

#pragma region Desktop Family
#if WINAPI_FAMILY_PARTITION(WINAPI_PARTITION_DESKTOP)

_Win32_metadata_set_last_error_
WINUSERAPI
LRESULT
WINAPI
SendMessageTimeoutA(
    _In_ HWND hWnd,
    _In_ UINT Msg,
    _In_ WPARAM wParam,
    _In_ LPARAM lParam,
    _In_ SEND_MESSAGE_TIMEOUT_FLAGS fuFlags,
    _In_ UINT uTimeout,
    _Out_opt_ PDWORD_PTR lpdwResult);
_Win32_metadata_set_last_error_
WINUSERAPI
LRESULT
WINAPI
SendMessageTimeoutW(
    _In_ HWND hWnd,
    _In_ UINT Msg,
    _In_ WPARAM wParam,
    _In_ LPARAM lParam,
    _In_ SEND_MESSAGE_TIMEOUT_FLAGS fuFlags,
    _In_ UINT uTimeout,
    _Out_opt_ PDWORD_PTR lpdwResult);
#ifdef UNICODE
#define SendMessageTimeout  SendMessageTimeoutW
#else
#define SendMessageTimeout  SendMessageTimeoutA
#endif // !UNICODE

_Win32_metadata_set_last_error_
WINUSERAPI
BOOL
WINAPI
SendNotifyMessageA(
    _In_ HWND hWnd,
    _In_ UINT Msg,
    _In_ WPARAM wParam,
    _In_ LPARAM lParam);
_Win32_metadata_set_last_error_
WINUSERAPI
BOOL
WINAPI
SendNotifyMessageW(
    _In_ HWND hWnd,
    _In_ UINT Msg,
    _In_ WPARAM wParam,
    _In_ LPARAM lParam);
#ifdef UNICODE
#define SendNotifyMessage  SendNotifyMessageW
#else
#define SendNotifyMessage  SendNotifyMessageA
#endif // !UNICODE

_Win32_metadata_set_last_error_
WINUSERAPI
BOOL
WINAPI
SendMessageCallbackA(
    _In_ HWND hWnd,
    _In_ UINT Msg,
    _In_ WPARAM wParam,
    _In_ LPARAM lParam,
    _In_ SENDASYNCPROC lpResultCallBack,
    _In_ ULONG_PTR dwData);
_Win32_metadata_set_last_error_
WINUSERAPI
BOOL
WINAPI
SendMessageCallbackW(
    _In_ HWND hWnd,
    _In_ UINT Msg,
    _In_ WPARAM wParam,
    _In_ LPARAM lParam,
    _In_ SENDASYNCPROC lpResultCallBack,
    _In_ ULONG_PTR dwData);
#ifdef UNICODE
#define SendMessageCallback  SendMessageCallbackW
#else
#define SendMessageCallback  SendMessageCallbackA
#endif // !UNICODE

#if(_WIN32_WINNT >= 0x0501)
typedef struct {
    UINT  cbSize;
    HDESK hdesk;
    HWND  hwnd;
    LUID  luid;
} BSMINFO, *PBSMINFO;

_Win32_metadata_set_last_error_
WINUSERAPI
long
WINAPI
BroadcastSystemMessageExA(
    _In_ BROADCAST_SYSTEM_MESSAGE_FLAGS flags,
    _Inout_opt_ LPDWORD lpInfo,
    _In_ UINT Msg,
    _In_ WPARAM wParam,
    _In_ LPARAM lParam,
    _Out_opt_ PBSMINFO pbsmInfo);
_Win32_metadata_set_last_error_
WINUSERAPI
long
WINAPI
BroadcastSystemMessageExW(
    _In_ BROADCAST_SYSTEM_MESSAGE_FLAGS flags,
    _Inout_opt_ LPDWORD lpInfo,
    _In_ UINT Msg,
    _In_ WPARAM wParam,
    _In_ LPARAM lParam,
    _Out_opt_ PBSMINFO pbsmInfo);
#ifdef UNICODE
#define BroadcastSystemMessageEx  BroadcastSystemMessageExW
#else
#define BroadcastSystemMessageEx  BroadcastSystemMessageExA
#endif // !UNICODE
#endif /* _WIN32_WINNT >= 0x0501 */

#endif /* WINAPI_FAMILY_PARTITION(WINAPI_PARTITION_DESKTOP) */
#pragma endregion

#if(WINVER >= 0x0400)

#pragma region Desktop Family
#if WINAPI_FAMILY_PARTITION(WINAPI_PARTITION_DESKTOP)

#if defined(_WIN32_WINNT)
WINUSERAPI
long
WINAPI
BroadcastSystemMessageA(
    _In_ DWORD flags,
    _Inout_opt_ LPDWORD lpInfo,
    _In_ UINT Msg,
    _In_ WPARAM wParam,
    _In_ LPARAM lParam);
_Win32_metadata_set_last_error_
WINUSERAPI
long
WINAPI
BroadcastSystemMessageW(
    _In_ BROADCAST_SYSTEM_MESSAGE_FLAGS flags,
    _Inout_opt_ LPDWORD lpInfo,
    _In_ UINT Msg,
    _In_ WPARAM wParam,
    _In_ LPARAM lParam);
#ifdef UNICODE
#define BroadcastSystemMessage  BroadcastSystemMessageW
#else
#define BroadcastSystemMessage  BroadcastSystemMessageA
#endif // !UNICODE
#elif defined(_WIN32_WINDOWS)
// The Win95 version isn't A/W decorated
_Win32_metadata_set_last_error_
WINUSERAPI
long
WINAPI
BroadcastSystemMessage(
    _In_ DWORD flags,
    _Inout_opt_ LPDWORD lpInfo,
    _In_ UINT Msg,
    _In_ WPARAM wParam,
    _In_ LPARAM lParam);

#endif

#endif /* WINAPI_FAMILY_PARTITION(WINAPI_PARTITION_DESKTOP) */
#pragma endregion

//Broadcast Special Message Recipient list
#define BSM_ALLCOMPONENTS       0x00000000
#define BSM_VXDS                0x00000001
#define BSM_NETDRIVER           0x00000002
#define BSM_INSTALLABLEDRIVERS  0x00000004
#define BSM_APPLICATIONS        0x00000008
#define BSM_ALLDESKTOPS         0x00000010

//Broadcast Special Message Flags
#define BSF_QUERY               0x00000001
#define BSF_IGNORECURRENTTASK   0x00000002
#define BSF_FLUSHDISK           0x00000004
#define BSF_NOHANG              0x00000008
#define BSF_POSTMESSAGE         0x00000010
#define BSF_FORCEIFHUNG         0x00000020
#define BSF_NOTIMEOUTIFNOTHUNG  0x00000040
#if(_WIN32_WINNT >= 0x0500)
#define BSF_ALLOWSFW            0x00000080
#define BSF_SENDNOTIFYMESSAGE   0x00000100
#endif /* _WIN32_WINNT >= 0x0500 */
#if(_WIN32_WINNT >= 0x0501)
#define BSF_RETURNHDESK         0x00000200
#define BSF_LUID                0x00000400
#endif /* _WIN32_WINNT >= 0x0501 */

#define BROADCAST_QUERY_DENY         0x424D5144  // Return this value to deny a query.
#endif /* WINVER >= 0x0400 */

// RegisterDeviceNotification

#pragma region Desktop Family
#if WINAPI_FAMILY_PARTITION(WINAPI_PARTITION_DESKTOP)

#if(WINVER >= 0x0500)
typedef  PVOID           HDEVNOTIFY;
typedef  HDEVNOTIFY     *PHDEVNOTIFY;

#define DEVICE_NOTIFY_WINDOW_HANDLE          0x00000000
#define DEVICE_NOTIFY_SERVICE_HANDLE         0x00000001
#if(_WIN32_WINNT >= 0x0501)
#define DEVICE_NOTIFY_ALL_INTERFACE_CLASSES  0x00000004
#endif /* _WIN32_WINNT >= 0x0501 */

_Win32_metadata_set_last_error_
WINUSERAPI
HDEVNOTIFY
WINAPI
RegisterDeviceNotificationA(
    _In_ HANDLE hRecipient,
    _In_ LPVOID NotificationFilter,
    _In_ DWORD Flags);
_Win32_metadata_set_last_error_
WINUSERAPI
HDEVNOTIFY
WINAPI
RegisterDeviceNotificationW(
    _In_ HANDLE hRecipient,
    _In_ LPVOID NotificationFilter,
    _In_ DWORD Flags);
#ifdef UNICODE
#define RegisterDeviceNotification  RegisterDeviceNotificationW
#else
#define RegisterDeviceNotification  RegisterDeviceNotificationA
#endif // !UNICODE

_Win32_metadata_set_last_error_
WINUSERAPI
BOOL
WINAPI
UnregisterDeviceNotification(
    _In_ HDEVNOTIFY Handle
    );

#if (_WIN32_WINNT >= 0x0502)

#if !defined(_HPOWERNOTIFY_DEF_)

#define _HPOWERNOTIFY_DEF_

typedef  PVOID           HPOWERNOTIFY;
typedef  HPOWERNOTIFY   *PHPOWERNOTIFY;

#endif

_Win32_metadata_set_last_error_
WINUSERAPI
HPOWERNOTIFY
WINAPI
RegisterPowerSettingNotification(
    IN HANDLE hRecipient,
    IN LPCGUID PowerSettingGuid,
    IN DWORD Flags
    );

_Win32_metadata_set_last_error_
WINUSERAPI
BOOL
WINAPI
UnregisterPowerSettingNotification(
    IN HPOWERNOTIFY Handle
    );

_Win32_metadata_set_last_error_
WINUSERAPI
HPOWERNOTIFY
WINAPI
RegisterSuspendResumeNotification (
    IN HANDLE hRecipient,
    IN DWORD Flags
    );

_Win32_metadata_set_last_error_
WINUSERAPI
BOOL
WINAPI
UnregisterSuspendResumeNotification (
    IN HPOWERNOTIFY Handle
    );


#endif // (_WIN32_WINNT >= 0x0502)
#endif /* WINVER >= 0x0500 */

#endif /* WINAPI_FAMILY_PARTITION(WINAPI_PARTITION_DESKTOP) */
#pragma endregion

#pragma region Desktop Family
#if WINAPI_FAMILY_PARTITION(WINAPI_PARTITION_DESKTOP | WINAPI_PARTITION_GAMES)

_Win32_metadata_set_last_error_
WINUSERAPI
BOOL
WINAPI
PostMessageA(
    _In_opt_ HWND hWnd,
    _In_ UINT Msg,
    _In_ WPARAM wParam,
    _In_ LPARAM lParam);
_Win32_metadata_set_last_error_
WINUSERAPI
BOOL
WINAPI
PostMessageW(
    _In_opt_ HWND hWnd,
    _In_ UINT Msg,
    _In_ WPARAM wParam,
    _In_ LPARAM lParam);
#ifdef UNICODE
#define PostMessage  PostMessageW
#else
#define PostMessage  PostMessageA
#endif // !UNICODE

_Win32_metadata_set_last_error_
WINUSERAPI
BOOL
WINAPI
PostThreadMessageA(
    _In_ DWORD idThread,
    _In_ UINT Msg,
    _In_ WPARAM wParam,
    _In_ LPARAM lParam);
_Win32_metadata_set_last_error_
WINUSERAPI
BOOL
WINAPI
PostThreadMessageW(
    _In_ DWORD idThread,
    _In_ UINT Msg,
    _In_ WPARAM wParam,
    _In_ LPARAM lParam);
#ifdef UNICODE
#define PostThreadMessage  PostThreadMessageW
#else
#define PostThreadMessage  PostThreadMessageA
#endif // !UNICODE

#endif /* WINAPI_FAMILY_PARTITION(WINAPI_PARTITION_DESKTOP | WINAPI_PARTITION_GAMES) */
#pragma endregion

#define PostAppMessageA(idThread, wMsg, wParam, lParam)\
        PostThreadMessageA((DWORD)idThread, wMsg, wParam, lParam)
#define PostAppMessageW(idThread, wMsg, wParam, lParam)\
        PostThreadMessageW((DWORD)idThread, wMsg, wParam, lParam)
#ifdef UNICODE
#define PostAppMessage  PostAppMessageW
#else
#define PostAppMessage  PostAppMessageA
#endif // !UNICODE

/*
 * Special HWND value for use with PostMessage() and SendMessage()
 */
#define HWND_BROADCAST  ((HWND)0xffff)

#if(WINVER >= 0x0500)
#define HWND_MESSAGE     ((HWND)-3)
#endif /* WINVER >= 0x0500 */

#pragma region Desktop Family
#if WINAPI_FAMILY_PARTITION(WINAPI_PARTITION_DESKTOP)

WINUSERAPI
BOOL
WINAPI
AttachThreadInput(
    _In_ DWORD idAttach,
    _In_ DWORD idAttachTo,
    _In_ BOOL fAttach);


WINUSERAPI
BOOL
WINAPI
ReplyMessage(
    _In_ LRESULT lResult);

_Win32_metadata_set_last_error_
WINUSERAPI
BOOL
WINAPI
WaitMessage(
    VOID);

#if (_WIN32_WINNT >= 0x602)
#endif


WINUSERAPI
DWORD
WINAPI
WaitForInputIdle(
    _In_ HANDLE hProcess,
    _In_ DWORD dwMilliseconds);

#endif /* WINAPI_FAMILY_PARTITION(WINAPI_PARTITION_DESKTOP) */
#pragma endregion

#pragma region Desktop Family or Games Family
#if WINAPI_FAMILY_PARTITION(WINAPI_PARTITION_DESKTOP | WINAPI_PARTITION_GAMES)

WINUSERAPI
#ifndef _MAC
LRESULT
WINAPI
#else
LRESULT
CALLBACK
#endif
DefWindowProcA(
    _In_ HWND hWnd,
    _In_ UINT Msg,
    _In_ WPARAM wParam,
    _In_ LPARAM lParam);
WINUSERAPI
#ifndef _MAC
LRESULT
WINAPI
#else
LRESULT
CALLBACK
#endif
DefWindowProcW(
    _In_ HWND hWnd,
    _In_ UINT Msg,
    _In_ WPARAM wParam,
    _In_ LPARAM lParam);
#ifdef UNICODE
#define DefWindowProc  DefWindowProcW
#else
#define DefWindowProc  DefWindowProcA
#endif // !UNICODE

WINUSERAPI
VOID
WINAPI
PostQuitMessage(
    _In_ int nExitCode);

#ifdef STRICT

WINUSERAPI
LRESULT
WINAPI
CallWindowProcA(
    _In_ WNDPROC lpPrevWndFunc,
    _In_ HWND hWnd,
    _In_ UINT Msg,
    _In_ WPARAM wParam,
    _In_ LPARAM lParam);
WINUSERAPI
LRESULT
WINAPI
CallWindowProcW(
    _In_ WNDPROC lpPrevWndFunc,
    _In_ HWND hWnd,
    _In_ UINT Msg,
    _In_ WPARAM wParam,
    _In_ LPARAM lParam);
#ifdef UNICODE
#define CallWindowProc  CallWindowProcW
#else
#define CallWindowProc  CallWindowProcA
#endif // !UNICODE

#else /* !STRICT */

WINUSERAPI
LRESULT
WINAPI
CallWindowProcA(
    _In_ FARPROC lpPrevWndFunc,
    _In_ HWND hWnd,
    _In_ UINT Msg,
    _In_ WPARAM wParam,
    _In_ LPARAM lParam);
WINUSERAPI
LRESULT
WINAPI
CallWindowProcW(
    _In_ FARPROC lpPrevWndFunc,
    _In_ HWND hWnd,
    _In_ UINT Msg,
    _In_ WPARAM wParam,
    _In_ LPARAM lParam);
#ifdef UNICODE
#define CallWindowProc  CallWindowProcW
#else
#define CallWindowProc  CallWindowProcA
#endif // !UNICODE

#endif /* !STRICT */

#endif /* WINAPI_FAMILY_PARTITION(WINAPI_PARTITION_DESKTOP | WINAPI_PARTITION_GAMES) */
#pragma endregion

#pragma region Desktop Family
#if WINAPI_FAMILY_PARTITION(WINAPI_PARTITION_DESKTOP)

WINUSERAPI
BOOL
WINAPI
InSendMessage(
    VOID);

#endif /* WINAPI_FAMILY_PARTITION(WINAPI_PARTITION_DESKTOP) */
#pragma endregion

#if(WINVER >= 0x0500)

#pragma region Desktop Family
#if WINAPI_FAMILY_PARTITION(WINAPI_PARTITION_DESKTOP)

WINUSERAPI
DWORD
WINAPI
InSendMessageEx(
    _Reserved_ LPVOID lpReserved);

#endif /* WINAPI_FAMILY_PARTITION(WINAPI_PARTITION_DESKTOP) */
#pragma endregion

/*
 * InSendMessageEx return value
 */
#define ISMEX_NOSEND      0x00000000
#define ISMEX_SEND        0x00000001
#define ISMEX_NOTIFY      0x00000002
#define ISMEX_CALLBACK    0x00000004
#define ISMEX_REPLIED     0x00000008
#endif /* WINVER >= 0x0500 */


#pragma region Desktop Family
#if WINAPI_FAMILY_PARTITION(WINAPI_PARTITION_DESKTOP)

WINUSERAPI
UINT
WINAPI
GetDoubleClickTime(
    VOID);

_Win32_metadata_set_last_error_
WINUSERAPI
BOOL
WINAPI
SetDoubleClickTime(
    _In_ UINT);

#endif /* WINAPI_FAMILY_PARTITION(WINAPI_PARTITION_DESKTOP) */
#pragma endregion

#pragma region Desktop Family or Games Family
#if WINAPI_FAMILY_PARTITION(WINAPI_PARTITION_DESKTOP | WINAPI_PARTITION_GAMES)

_Win32_metadata_set_last_error_
WINUSERAPI
ATOM
WINAPI
RegisterClassA(
    _In_ CONST WNDCLASSA *lpWndClass);
_Win32_metadata_set_last_error_
WINUSERAPI
ATOM
WINAPI
RegisterClassW(
    _In_ CONST WNDCLASSW *lpWndClass);
#ifdef UNICODE
#define RegisterClass  RegisterClassW
#else
#define RegisterClass  RegisterClassA
#endif // !UNICODE

_Win32_metadata_set_last_error_
WINUSERAPI
BOOL
WINAPI
UnregisterClassA(
    _In_ LPCSTR lpClassName,
    _In_opt_ HINSTANCE hInstance);
_Win32_metadata_set_last_error_
WINUSERAPI
BOOL
WINAPI
UnregisterClassW(
    _In_ LPCWSTR lpClassName,
    _In_opt_ HINSTANCE hInstance);
#ifdef UNICODE
#define UnregisterClass  UnregisterClassW
#else
#define UnregisterClass  UnregisterClassA
#endif // !UNICODE

#endif /* WINAPI_FAMILY_PARTITION(WINAPI_PARTITION_DESKTOP | WINAPI_PARTITION_GAMES) */
#pragma endregion

#pragma region Desktop Family
#if WINAPI_FAMILY_PARTITION(WINAPI_PARTITION_DESKTOP)

_Success_(return)
_Win32_metadata_set_last_error_
WINUSERAPI
BOOL
WINAPI
GetClassInfoA(
    _In_opt_ HINSTANCE hInstance,
    _In_ LPCSTR lpClassName,
    _Out_ LPWNDCLASSA lpWndClass);
_Success_(return)
_Win32_metadata_set_last_error_
WINUSERAPI
BOOL
WINAPI
GetClassInfoW(
    _In_opt_ HINSTANCE hInstance,
    _In_ LPCWSTR lpClassName,
    _Out_ LPWNDCLASSW lpWndClass);
#ifdef UNICODE
#define GetClassInfo  GetClassInfoW
#else
#define GetClassInfo  GetClassInfoA
#endif // !UNICODE

#endif /* WINAPI_FAMILY_PARTITION(WINAPI_PARTITION_DESKTOP) */
#pragma endregion

#pragma region Desktop Family or Games Family
#if WINAPI_FAMILY_PARTITION(WINAPI_PARTITION_DESKTOP | WINAPI_PARTITION_GAMES)

#if(WINVER >= 0x0400)
_Win32_metadata_set_last_error_
WINUSERAPI
ATOM
WINAPI
RegisterClassExA(
    _In_ CONST WNDCLASSEXA *);
_Win32_metadata_set_last_error_
WINUSERAPI
ATOM
WINAPI
RegisterClassExW(
    _In_ CONST WNDCLASSEXW *);
#ifdef UNICODE
#define RegisterClassEx  RegisterClassExW
#else
#define RegisterClassEx  RegisterClassExA
#endif // !UNICODE

#endif /* WINAPI_FAMILY_PARTITION(WINAPI_PARTITION_DESKTOP | WINAPI_PARTITION_GAMES) */
#pragma endregion

#pragma region Desktop Family
#if WINAPI_FAMILY_PARTITION(WINAPI_PARTITION_DESKTOP)

_Success_(return)
_Win32_metadata_set_last_error_
WINUSERAPI
BOOL
WINAPI
GetClassInfoExA(
    _In_opt_ HINSTANCE hInstance,
    _In_ LPCSTR lpszClass,
    _Out_ LPWNDCLASSEXA lpwcx);
_Success_(return)
_Win32_metadata_set_last_error_
WINUSERAPI
BOOL
WINAPI
GetClassInfoExW(
    _In_opt_ HINSTANCE hInstance,
    _In_ LPCWSTR lpszClass,
    _Out_ LPWNDCLASSEXW lpwcx);
#ifdef UNICODE
#define GetClassInfoEx  GetClassInfoExW
#else
#define GetClassInfoEx  GetClassInfoExA
#endif // !UNICODE

#endif /* WINVER >= 0x0400 */

#endif /* WINAPI_FAMILY_PARTITION(WINAPI_PARTITION_DESKTOP) */
#pragma endregion

#define CW_USEDEFAULT       ((int)0x80000000)

/*
 * Special value for CreateWindow, et al.
 */
#define HWND_DESKTOP        ((HWND)0)

#pragma region Desktop Family or Games Family
#if WINAPI_FAMILY_PARTITION(WINAPI_PARTITION_DESKTOP | WINAPI_PARTITION_GAMES)

#if(_WIN32_WINNT >= 0x0501)
typedef BOOLEAN (WINAPI * PREGISTERCLASSNAMEW)(LPCWSTR);
#endif /* _WIN32_WINNT >= 0x0501 */

_Win32_metadata_set_last_error_
WINUSERAPI
HWND
WINAPI
CreateWindowExA(
    _In_ WINDOW_EX_STYLE dwExStyle,
    _In_opt_ LPCSTR lpClassName,
    _In_opt_ LPCSTR lpWindowName,
    _In_ WINDOW_STYLE dwStyle,
    _In_ int X,
    _In_ int Y,
    _In_ int nWidth,
    _In_ int nHeight,
    _In_opt_ HWND hWndParent,
    _In_opt_ HMENU hMenu,
    _In_opt_ HINSTANCE hInstance,
    _In_opt_ LPVOID lpParam);
_Win32_metadata_set_last_error_
WINUSERAPI
HWND
WINAPI
CreateWindowExW(
    _In_ WINDOW_EX_STYLE dwExStyle,
    _In_opt_ LPCWSTR lpClassName,
    _In_opt_ LPCWSTR lpWindowName,
    _In_ WINDOW_STYLE dwStyle,
    _In_ int X,
    _In_ int Y,
    _In_ int nWidth,
    _In_ int nHeight,
    _In_opt_ HWND hWndParent,
    _In_opt_ HMENU hMenu,
    _In_opt_ HINSTANCE hInstance,
    _In_opt_ LPVOID lpParam);
#ifdef UNICODE
#define CreateWindowEx  CreateWindowExW
#else
#define CreateWindowEx  CreateWindowExA
#endif // !UNICODE

#endif /* WINAPI_FAMILY_PARTITION(WINAPI_PARTITION_DESKTOP | WINAPI_PARTITION_GAMES) */
#pragma endregion

#define CreateWindowA(lpClassName, lpWindowName, dwStyle, x, y,\
nWidth, nHeight, hWndParent, hMenu, hInstance, lpParam)\
CreateWindowExA(0L, lpClassName, lpWindowName, dwStyle, x, y,\
nWidth, nHeight, hWndParent, hMenu, hInstance, lpParam)
#define CreateWindowW(lpClassName, lpWindowName, dwStyle, x, y,\
nWidth, nHeight, hWndParent, hMenu, hInstance, lpParam)\
CreateWindowExW(0L, lpClassName, lpWindowName, dwStyle, x, y,\
nWidth, nHeight, hWndParent, hMenu, hInstance, lpParam)
#ifdef UNICODE
#define CreateWindow  CreateWindowW
#else
#define CreateWindow  CreateWindowA
#endif // !UNICODE

#pragma region Desktop Family
#if WINAPI_FAMILY_PARTITION(WINAPI_PARTITION_DESKTOP)


WINUSERAPI
BOOL
WINAPI
IsWindow(
    _In_opt_ HWND hWnd);


WINUSERAPI
BOOL
WINAPI
IsMenu(
    _In_ HMENU hMenu);

WINUSERAPI
BOOL
WINAPI
IsChild(
    _In_ HWND hWndParent,
    _In_ HWND hWnd);

#endif /* WINAPI_FAMILY_PARTITION(WINAPI_PARTITION_DESKTOP) */
#pragma endregion

#pragma region Desktop or Games Family
#if WINAPI_FAMILY_PARTITION(WINAPI_PARTITION_DESKTOP | WINAPI_PARTITION_GAMES)

_Win32_metadata_set_last_error_
WINUSERAPI
BOOL
WINAPI
DestroyWindow(
    _In_ HWND hWnd);

WINUSERAPI
BOOL
WINAPI
ShowWindow(
    _In_ HWND hWnd,
    _In_ int nCmdShow);

#endif /* WINAPI_FAMILY_PARTITION(WINAPI_PARTITION_DESKTOP | WINAPI_PARTITION_GAMES) */
#pragma endregion

#pragma region Desktop Family
#if WINAPI_FAMILY_PARTITION(WINAPI_PARTITION_DESKTOP)

#if(WINVER >= 0x0500)
_Win32_metadata_set_last_error_
WINUSERAPI
BOOL
WINAPI
AnimateWindow(
    _In_ HWND hWnd,
    _In_ DWORD dwTime,
    _In_ ANIMATE_WINDOW_FLAGS dwFlags);
#endif /* WINVER >= 0x0500 */

#endif /* WINAPI_FAMILY_PARTITION(WINAPI_PARTITION_DESKTOP) */
#pragma endregion

#if(_WIN32_WINNT >= 0x0500)
#if defined(_WINGDI_) && !defined(NOGDI)

#pragma region Desktop Family
#if WINAPI_FAMILY_PARTITION(WINAPI_PARTITION_DESKTOP)

_Win32_metadata_set_last_error_
WINUSERAPI
BOOL
WINAPI
UpdateLayeredWindow(
    _In_ HWND hWnd,
    _In_opt_ HDC hdcDst,
    _In_opt_ POINT* pptDst,
    _In_opt_ SIZE* psize,
    _In_opt_ HDC hdcSrc,
    _In_opt_ POINT* pptSrc,
    _In_ COLORREF crKey,
    _In_opt_ BLENDFUNCTION* pblend,
    _In_ UPDATE_LAYERED_WINDOW_FLAGS dwFlags);

/*
 * Layered Window Update information
 */
typedef struct tagUPDATELAYEREDWINDOWINFO
{
    DWORD cbSize;
    HDC hdcDst;
    const POINT* pptDst;
    const SIZE* psize;
    HDC hdcSrc;
    const POINT* pptSrc;
    COLORREF crKey;
    const BLENDFUNCTION* pblend;
    UPDATE_LAYERED_WINDOW_FLAGS dwFlags;
    const RECT* prcDirty;
} UPDATELAYEREDWINDOWINFO, *PUPDATELAYEREDWINDOWINFO;


#if (_WIN32_WINNT < 0x0502)
typedef
#endif /* _WIN32_WINNT < 0x0502 */
WINUSERAPI
BOOL
WINAPI
UpdateLayeredWindowIndirect(
    _In_ HWND hWnd,
    _In_ const UPDATELAYEREDWINDOWINFO* pULWInfo);

#endif /* WINAPI_FAMILY_PARTITION(WINAPI_PARTITION_DESKTOP) */
#pragma endregion

#endif

#if(_WIN32_WINNT >= 0x0501)

#pragma region Desktop Family
#if WINAPI_FAMILY_PARTITION(WINAPI_PARTITION_DESKTOP)

_Win32_metadata_set_last_error_
WINUSERAPI
BOOL
WINAPI
GetLayeredWindowAttributes(
    _In_ HWND hwnd,
    _Out_opt_ COLORREF* pcrKey,
    _Out_opt_ BYTE* pbAlpha,
    _Out_opt_ LAYERED_WINDOW_ATTRIBUTES_FLAGS* pdwFlags);

#define PW_CLIENTONLY           0x00000001

#if(_WIN32_WINNT >= 0x0603)
#define PW_RENDERFULLCONTENT    0x00000002
#endif /* _WIN32_WINNT >= 0x0603 */


WINUSERAPI
BOOL
WINAPI
PrintWindow(
    _In_ HWND hwnd,
    _In_ HDC hdcBlt,
    _In_ PRINT_WINDOW_FLAGS nFlags);

#endif /* WINAPI_FAMILY_PARTITION(WINAPI_PARTITION_DESKTOP) */
#pragma endregion

#endif /* _WIN32_WINNT >= 0x0501 */

#pragma region Desktop Family
#if WINAPI_FAMILY_PARTITION(WINAPI_PARTITION_DESKTOP)

_Win32_metadata_set_last_error_
WINUSERAPI
BOOL
WINAPI
SetLayeredWindowAttributes(
    _In_ HWND hwnd,
    _In_ COLORREF crKey,
    _In_ BYTE bAlpha,
    _In_ LAYERED_WINDOW_ATTRIBUTES_FLAGS dwFlags);

#endif /* WINAPI_FAMILY_PARTITION(WINAPI_PARTITION_DESKTOP) */
#pragma endregion

#define LWA_COLORKEY          0x00000001
#define LWA_ALPHA             0x00000002


#define ULW_COLORKEY           0x00000001
#define ULW_ALPHA              0x00000002
#define ULW_OPAQUE             0x00000004

#define ULW_EX_NORESIZE        0x00000008

#endif /* _WIN32_WINNT >= 0x0500 */
/*
 * FRAME_MARGIN
 * A distance from each edge of a rect. Positive values are inside, negative
 * are outside.
 */
typedef struct
{
    short left;
    short right;
    short top;
    short bottom;
} FRAME_MARGIN;


#pragma region Desktop Family
#if WINAPI_FAMILY_PARTITION(WINAPI_PARTITION_DESKTOP)


#if(WINVER >= 0x0400)
WINUSERAPI
BOOL
WINAPI
ShowWindowAsync(
    _In_  HWND hWnd,
    _In_  int nCmdShow);
#endif /* WINVER >= 0x0400 */

WINUSERAPI
BOOL
WINAPI
FlashWindow(
    _In_  HWND hWnd,
    _In_  BOOL bInvert);

#if(WINVER >= 0x0500)
typedef struct {
    UINT  cbSize;
    HWND  hwnd;
    FLASHWINFO_FLAGS dwFlags;
    UINT  uCount;
    DWORD dwTimeout;
} FLASHWINFO, *PFLASHWINFO;

WINUSERAPI
BOOL
WINAPI
FlashWindowEx(
    _In_ PFLASHWINFO pfwi);

#define FLASHW_STOP         0
#define FLASHW_CAPTION      0x00000001
#define FLASHW_TRAY         0x00000002
#define FLASHW_ALL          (FLASHW_CAPTION | FLASHW_TRAY)
#define FLASHW_TIMER        0x00000004
#define FLASHW_TIMERNOFG    0x0000000C

#endif /* WINVER >= 0x0500 */

_Win32_metadata_set_last_error_
WINUSERAPI
BOOL
WINAPI
ShowOwnedPopups(
    _In_  HWND hWnd,
    _In_  BOOL fShow);

_Win32_metadata_set_last_error_
WINUSERAPI
BOOL
WINAPI
OpenIcon(
    _In_  HWND hWnd);

_Win32_metadata_set_last_error_
WINUSERAPI
BOOL
WINAPI
CloseWindow(
    _In_  HWND hWnd);

#endif /* WINAPI_FAMILY_PARTITION(WINAPI_PARTITION_DESKTOP) */
#pragma endregion

#pragma region Desktop Family
#if WINAPI_FAMILY_PARTITION(WINAPI_PARTITION_DESKTOP | WINAPI_PARTITION_GAMES)

_Win32_metadata_set_last_error_
WINUSERAPI
BOOL
WINAPI
MoveWindow(
    _In_ HWND hWnd,
    _In_ int X,
    _In_ int Y,
    _In_ int nWidth,
    _In_ int nHeight,
    _In_ BOOL bRepaint);

_Win32_metadata_set_last_error_
WINUSERAPI
BOOL
WINAPI
SetWindowPos(
    _In_ HWND hWnd,
    _In_opt_ HWND hWndInsertAfter,
    _In_ int X,
    _In_ int Y,
    _In_ int cx,
    _In_ int cy,
    _In_ SET_WINDOW_POS_FLAGS uFlags);

#endif /* WINAPI_FAMILY_PARTITION(WINAPI_PARTITION_DESKTOP | WINAPI_PARTITION_GAMES) */
#pragma endregion

#pragma region Desktop Family
#if WINAPI_FAMILY_PARTITION(WINAPI_PARTITION_DESKTOP)

_Win32_metadata_set_last_error_
WINUSERAPI
BOOL
WINAPI
GetWindowPlacement(
    _In_ HWND hWnd,
    _Inout_ WINDOWPLACEMENT *lpwndpl);

_Win32_metadata_set_last_error_
WINUSERAPI
BOOL
WINAPI
SetWindowPlacement(
    _In_ HWND hWnd,
    _In_ CONST WINDOWPLACEMENT *lpwndpl);

#if(_WIN32_WINNT >= 0x0601)
#define WDA_NONE        0x00000000
#define WDA_MONITOR     0x00000001
#define WDA_EXCLUDEFROMCAPTURE 0x00000011


_Win32_metadata_set_last_error_
WINUSERAPI
BOOL
WINAPI
GetWindowDisplayAffinity(
    _In_ HWND hWnd,
    _Out_ DWORD* pdwAffinity);

_Win32_metadata_set_last_error_
WINUSERAPI
BOOL
WINAPI
SetWindowDisplayAffinity(
    _In_ HWND hWnd,
    _In_ WINDOW_DISPLAY_AFFINITY dwAffinity);

#endif /* _WIN32_WINNT >= 0x0601 */

#endif /* WINAPI_FAMILY_PARTITION(WINAPI_PARTITION_DESKTOP) */
#pragma endregion

#ifndef NODEFERWINDOWPOS

#pragma region Desktop Family
#if WINAPI_FAMILY_PARTITION(WINAPI_PARTITION_DESKTOP)

_Win32_metadata_set_last_error_
WINUSERAPI
HDWP
WINAPI
BeginDeferWindowPos(
    _In_ int nNumWindows);

_Win32_metadata_set_last_error_
WINUSERAPI
HDWP
WINAPI
DeferWindowPos(
    _In_ HDWP hWinPosInfo,
    _In_ HWND hWnd,
    _In_opt_ HWND hWndInsertAfter,
    _In_ int x,
    _In_ int y,
    _In_ int cx,
    _In_ int cy,
    _In_ SET_WINDOW_POS_FLAGS uFlags);


_Win32_metadata_set_last_error_
WINUSERAPI
BOOL
WINAPI
EndDeferWindowPos(
    _In_ HDWP hWinPosInfo);

#endif /* WINAPI_FAMILY_PARTITION(WINAPI_PARTITION_DESKTOP) */
#pragma endregion

#endif /* !NODEFERWINDOWPOS */

#pragma region Desktop Family
#if WINAPI_FAMILY_PARTITION(WINAPI_PARTITION_DESKTOP | WINAPI_PARTITION_GAMES)

WINUSERAPI
BOOL
WINAPI
IsWindowVisible(
    _In_ HWND hWnd);

#endif /* WINAPI_FAMILY_PARTITION(WINAPI_PARTITION_DESKTOP | WINAPI_PARTITION_GAMES) */
#pragma endregion

#pragma region Desktop Family
#if WINAPI_FAMILY_PARTITION(WINAPI_PARTITION_DESKTOP)

WINUSERAPI
BOOL
WINAPI
IsIconic(
    _In_ HWND hWnd);

WINUSERAPI
BOOL
WINAPI
AnyPopup(
    VOID);

_Win32_metadata_set_last_error_
WINUSERAPI
BOOL
WINAPI
BringWindowToTop(
    _In_ HWND hWnd);

WINUSERAPI
BOOL
WINAPI
IsZoomed(
    _In_ HWND hWnd);

#endif /* WINAPI_FAMILY_PARTITION(WINAPI_PARTITION_DESKTOP) */
#pragma endregion

/*
 * SetWindowPos Flags
 */
#define SWP_NONE            0x0000
#define SWP_NOSIZE          0x0001
#define SWP_NOMOVE          0x0002
#define SWP_NOZORDER        0x0004
#define SWP_NOREDRAW        0x0008
#define SWP_NOACTIVATE      0x0010
#define SWP_FRAMECHANGED    0x0020  /* The frame changed: send WM_NCCALCSIZE */
#define SWP_SHOWWINDOW      0x0040
#define SWP_HIDEWINDOW      0x0080
#define SWP_NOCOPYBITS      0x0100
#define SWP_NOOWNERZORDER   0x0200  /* Don't do owner Z ordering */
#define SWP_NOSENDCHANGING  0x0400  /* Don't send WM_WINDOWPOSCHANGING */

#define SWP_DRAWFRAME       SWP_FRAMECHANGED
#define SWP_NOREPOSITION    SWP_NOOWNERZORDER

#if(WINVER >= 0x0400)
#define SWP_DEFERERASE      0x2000 // same as SWP_DEFERDRAWING
#define SWP_ASYNCWINDOWPOS  0x4000 // same as SWP_CREATESPB
#endif /* WINVER >= 0x0400 */


#define HWND_TOP        ((HWND)0)
#define HWND_BOTTOM     ((HWND)1)
#define HWND_TOPMOST    ((HWND)-1)
#define HWND_NOTOPMOST  ((HWND)-2)

#ifndef NOCTLMGR

/*
 * WARNING:
 * The following structures must NOT be DWORD padded because they are
 * followed by strings, etc that do not have to be DWORD aligned.
 */
#include <pshpack2.h>

#pragma region Application Family or OneCore Family
#if WINAPI_FAMILY_PARTITION(WINAPI_PARTITION_APP | WINAPI_PARTITION_SYSTEM)

/*
 * original NT 32 bit dialog template:
 */
typedef struct {
    DWORD style;
    DWORD dwExtendedStyle;
    WORD cdit;
    short x;
    short y;
    short cx;
    short cy;
} DLGTEMPLATE;

#endif /* WINAPI_FAMILY_PARTITION(WINAPI_PARTITION_APP | WINAPI_PARTITION_SYSTEM) */
#pragma endregion

#pragma region Desktop Family
#if WINAPI_FAMILY_PARTITION(WINAPI_PARTITION_DESKTOP)

typedef DLGTEMPLATE *LPDLGTEMPLATEA;
typedef DLGTEMPLATE *LPDLGTEMPLATEW;
#ifdef UNICODE
typedef LPDLGTEMPLATEW LPDLGTEMPLATE;
#else
typedef LPDLGTEMPLATEA LPDLGTEMPLATE;
#endif // UNICODE

#endif /* WINAPI_FAMILY_PARTITION(WINAPI_PARTITION_DESKTOP) */
#pragma endregion

#pragma region Application Family or OneCore Family
#if WINAPI_FAMILY_PARTITION(WINAPI_PARTITION_APP | WINAPI_PARTITION_SYSTEM)

typedef CONST DLGTEMPLATE *LPCDLGTEMPLATEA;
typedef CONST DLGTEMPLATE *LPCDLGTEMPLATEW;
#ifdef UNICODE
typedef LPCDLGTEMPLATEW LPCDLGTEMPLATE;
#else
typedef LPCDLGTEMPLATEA LPCDLGTEMPLATE;
#endif // UNICODE

#endif /* WINAPI_FAMILY_PARTITION(WINAPI_PARTITION_APP | WINAPI_PARTITION_SYSTEM) */
#pragma endregion


#pragma region Desktop Family
#if WINAPI_FAMILY_PARTITION(WINAPI_PARTITION_DESKTOP)

/*
 * 32 bit Dialog item template.
 */
typedef struct {
    DWORD style;
    DWORD dwExtendedStyle;
    short x;
    short y;
    short cx;
    short cy;
    WORD id;
} DLGITEMTEMPLATE;
typedef DLGITEMTEMPLATE *PDLGITEMTEMPLATEA;
typedef DLGITEMTEMPLATE *PDLGITEMTEMPLATEW;
#ifdef UNICODE
typedef PDLGITEMTEMPLATEW PDLGITEMTEMPLATE;
#else
typedef PDLGITEMTEMPLATEA PDLGITEMTEMPLATE;
#endif // UNICODE
typedef DLGITEMTEMPLATE *LPDLGITEMTEMPLATEA;
typedef DLGITEMTEMPLATE *LPDLGITEMTEMPLATEW;
#ifdef UNICODE
typedef LPDLGITEMTEMPLATEW LPDLGITEMTEMPLATE;
#else
typedef LPDLGITEMTEMPLATEA LPDLGITEMTEMPLATE;
#endif // UNICODE

#endif /* WINAPI_FAMILY_PARTITION(WINAPI_PARTITION_DESKTOP) */
#pragma endregion


#include <poppack.h> /* Resume normal packing */

#pragma region Desktop Family
#if WINAPI_FAMILY_PARTITION(WINAPI_PARTITION_DESKTOP)

_Win32_metadata_set_last_error_
WINUSERAPI
HWND
WINAPI
CreateDialogParamA(
    _In_opt_ HINSTANCE hInstance,
    _In_ LPCSTR lpTemplateName,
    _In_opt_ HWND hWndParent,
    _In_opt_ DLGPROC lpDialogFunc,
    _In_ LPARAM dwInitParam);
_Win32_metadata_set_last_error_
WINUSERAPI
HWND
WINAPI
CreateDialogParamW(
    _In_opt_ HINSTANCE hInstance,
    _In_ LPCWSTR lpTemplateName,
    _In_opt_ HWND hWndParent,
    _In_opt_ DLGPROC lpDialogFunc,
    _In_ LPARAM dwInitParam);
#ifdef UNICODE
#define CreateDialogParam  CreateDialogParamW
#else
#define CreateDialogParam  CreateDialogParamA
#endif // !UNICODE

_Win32_metadata_set_last_error_
WINUSERAPI
HWND
WINAPI
CreateDialogIndirectParamA(
    _In_opt_ HINSTANCE hInstance,
    _In_ LPCDLGTEMPLATEA lpTemplate,
    _In_opt_ HWND hWndParent,
    _In_opt_ DLGPROC lpDialogFunc,
    _In_ LPARAM dwInitParam);
_Win32_metadata_set_last_error_
WINUSERAPI
HWND
WINAPI
CreateDialogIndirectParamW(
    _In_opt_ HINSTANCE hInstance,
    _In_ LPCDLGTEMPLATEW lpTemplate,
    _In_opt_ HWND hWndParent,
    _In_opt_ DLGPROC lpDialogFunc,
    _In_ LPARAM dwInitParam);
#ifdef UNICODE
#define CreateDialogIndirectParam  CreateDialogIndirectParamW
#else
#define CreateDialogIndirectParam  CreateDialogIndirectParamA
#endif // !UNICODE

#define CreateDialogA(hInstance, lpName, hWndParent, lpDialogFunc) \
CreateDialogParamA(hInstance, lpName, hWndParent, lpDialogFunc, 0L)
#define CreateDialogW(hInstance, lpName, hWndParent, lpDialogFunc) \
CreateDialogParamW(hInstance, lpName, hWndParent, lpDialogFunc, 0L)
#ifdef UNICODE
#define CreateDialog  CreateDialogW
#else
#define CreateDialog  CreateDialogA
#endif // !UNICODE

#define CreateDialogIndirectA(hInstance, lpTemplate, hWndParent, lpDialogFunc) \
CreateDialogIndirectParamA(hInstance, lpTemplate, hWndParent, lpDialogFunc, 0L)
#define CreateDialogIndirectW(hInstance, lpTemplate, hWndParent, lpDialogFunc) \
CreateDialogIndirectParamW(hInstance, lpTemplate, hWndParent, lpDialogFunc, 0L)
#ifdef UNICODE
#define CreateDialogIndirect  CreateDialogIndirectW
#else
#define CreateDialogIndirect  CreateDialogIndirectA
#endif // !UNICODE

_Win32_metadata_set_last_error_
WINUSERAPI
INT_PTR
WINAPI
DialogBoxParamA(
    _In_opt_ HINSTANCE hInstance,
    _In_ LPCSTR lpTemplateName,
    _In_opt_ HWND hWndParent,
    _In_opt_ DLGPROC lpDialogFunc,
    _In_ LPARAM dwInitParam);
_Win32_metadata_set_last_error_
WINUSERAPI
INT_PTR
WINAPI
DialogBoxParamW(
    _In_opt_ HINSTANCE hInstance,
    _In_ LPCWSTR lpTemplateName,
    _In_opt_ HWND hWndParent,
    _In_opt_ DLGPROC lpDialogFunc,
    _In_ LPARAM dwInitParam);
#ifdef UNICODE
#define DialogBoxParam  DialogBoxParamW
#else
#define DialogBoxParam  DialogBoxParamA
#endif // !UNICODE

_Win32_metadata_set_last_error_
WINUSERAPI
INT_PTR
WINAPI
DialogBoxIndirectParamA(
    _In_opt_ HINSTANCE hInstance,
    _In_ LPCDLGTEMPLATEA hDialogTemplate,
    _In_opt_ HWND hWndParent,
    _In_opt_ DLGPROC lpDialogFunc,
    _In_ LPARAM dwInitParam);
_Win32_metadata_set_last_error_
WINUSERAPI
INT_PTR
WINAPI
DialogBoxIndirectParamW(
    _In_opt_ HINSTANCE hInstance,
    _In_ LPCDLGTEMPLATEW hDialogTemplate,
    _In_opt_ HWND hWndParent,
    _In_opt_ DLGPROC lpDialogFunc,
    _In_ LPARAM dwInitParam);
#ifdef UNICODE
#define DialogBoxIndirectParam  DialogBoxIndirectParamW
#else
#define DialogBoxIndirectParam  DialogBoxIndirectParamA
#endif // !UNICODE

#define DialogBoxA(hInstance, lpTemplate, hWndParent, lpDialogFunc) \
DialogBoxParamA(hInstance, lpTemplate, hWndParent, lpDialogFunc, 0L)
#define DialogBoxW(hInstance, lpTemplate, hWndParent, lpDialogFunc) \
DialogBoxParamW(hInstance, lpTemplate, hWndParent, lpDialogFunc, 0L)
#ifdef UNICODE
#define DialogBox  DialogBoxW
#else
#define DialogBox  DialogBoxA
#endif // !UNICODE

#define DialogBoxIndirectA(hInstance, lpTemplate, hWndParent, lpDialogFunc) \
DialogBoxIndirectParamA(hInstance, lpTemplate, hWndParent, lpDialogFunc, 0L)
#define DialogBoxIndirectW(hInstance, lpTemplate, hWndParent, lpDialogFunc) \
DialogBoxIndirectParamW(hInstance, lpTemplate, hWndParent, lpDialogFunc, 0L)
#ifdef UNICODE
#define DialogBoxIndirect  DialogBoxIndirectW
#else
#define DialogBoxIndirect  DialogBoxIndirectA
#endif // !UNICODE

_Win32_metadata_set_last_error_
WINUSERAPI
BOOL
WINAPI
EndDialog(
    _In_ HWND hDlg,
    _In_ INT_PTR nResult);

_Win32_metadata_set_last_error_
WINUSERAPI
HWND
WINAPI
GetDlgItem(
    _In_opt_ HWND hDlg,
    _In_ int nIDDlgItem);

_Win32_metadata_set_last_error_
WINUSERAPI
BOOL
WINAPI
SetDlgItemInt(
    _In_ HWND hDlg,
    _In_ int nIDDlgItem,
    _In_ UINT uValue,
    _In_ BOOL bSigned);

_Win32_metadata_set_last_error_
WINUSERAPI
UINT
WINAPI
GetDlgItemInt(
    _In_ HWND hDlg,
    _In_ int nIDDlgItem,
    _Out_opt_ BOOL *lpTranslated,
    _In_ BOOL bSigned);

_Win32_metadata_set_last_error_
WINUSERAPI
BOOL
WINAPI
SetDlgItemTextA(
    _In_ HWND hDlg,
    _In_ int nIDDlgItem,
    _In_ LPCSTR lpString);
_Win32_metadata_set_last_error_
WINUSERAPI
BOOL
WINAPI
SetDlgItemTextW(
    _In_ HWND hDlg,
    _In_ int nIDDlgItem,
    _In_ LPCWSTR lpString);
#ifdef UNICODE
#define SetDlgItemText  SetDlgItemTextW
#else
#define SetDlgItemText  SetDlgItemTextA
#endif // !UNICODE

_Ret_range_(0, cchMax)
_Win32_metadata_set_last_error_
WINUSERAPI
UINT
WINAPI
GetDlgItemTextA(
    _In_ HWND hDlg,
    _In_ int nIDDlgItem,
    _Out_writes_(cchMax) LPSTR lpString,
    _In_ int cchMax);
_Ret_range_(0, cchMax)
_Win32_metadata_set_last_error_
WINUSERAPI
UINT
WINAPI
GetDlgItemTextW(
    _In_ HWND hDlg,
    _In_ int nIDDlgItem,
    _Out_writes_(cchMax) LPWSTR lpString,
    _In_ int cchMax);
#ifdef UNICODE
#define GetDlgItemText  GetDlgItemTextW
#else
#define GetDlgItemText  GetDlgItemTextA
#endif // !UNICODE

_Win32_metadata_set_last_error_
WINUSERAPI
BOOL
WINAPI
CheckDlgButton(
    _In_ HWND hDlg,
    _In_ int nIDButton,
    _In_ DLG_BUTTON_CHECK_STATE uCheck);

_Win32_metadata_set_last_error_
WINUSERAPI
BOOL
WINAPI
CheckRadioButton(
    _In_ HWND hDlg,
    _In_ int nIDFirstButton,
    _In_ int nIDLastButton,
    _In_ int nIDCheckButton);

WINUSERAPI
UINT
WINAPI
IsDlgButtonChecked(
    _In_ HWND hDlg,
    _In_ int nIDButton);

WINUSERAPI
LRESULT
WINAPI
SendDlgItemMessageA(
    _In_ HWND hDlg,
    _In_ int nIDDlgItem,
    _In_ UINT Msg,
    _In_ WPARAM wParam,
    _In_ LPARAM lParam);
WINUSERAPI
LRESULT
WINAPI
SendDlgItemMessageW(
    _In_ HWND hDlg,
    _In_ int nIDDlgItem,
    _In_ UINT Msg,
    _In_ WPARAM wParam,
    _In_ LPARAM lParam);
#ifdef UNICODE
#define SendDlgItemMessage  SendDlgItemMessageW
#else
#define SendDlgItemMessage  SendDlgItemMessageA
#endif // !UNICODE

_Win32_metadata_set_last_error_
WINUSERAPI
HWND
WINAPI
GetNextDlgGroupItem(
    _In_ HWND hDlg,
    _In_opt_ HWND hCtl,
    _In_ BOOL bPrevious);

_Win32_metadata_set_last_error_
WINUSERAPI
HWND
WINAPI
GetNextDlgTabItem(
    _In_ HWND hDlg,
    _In_opt_ HWND hCtl,
    _In_ BOOL bPrevious);

_Win32_metadata_set_last_error_
WINUSERAPI
int
WINAPI
GetDlgCtrlID(
    _In_ HWND hWnd);

WINUSERAPI
long
WINAPI
GetDialogBaseUnits(VOID);


WINUSERAPI
#ifndef _MAC
LRESULT
WINAPI
#else
LRESULT
CALLBACK
#endif
DefDlgProcA(
    _In_ HWND hDlg,
    _In_ UINT Msg,
    _In_ WPARAM wParam,
    _In_ LPARAM lParam);
WINUSERAPI
#ifndef _MAC
LRESULT
WINAPI
#else
LRESULT
CALLBACK
#endif
DefDlgProcW(
    _In_ HWND hDlg,
    _In_ UINT Msg,
    _In_ WPARAM wParam,
    _In_ LPARAM lParam);
#ifdef UNICODE
#define DefDlgProc  DefDlgProcW
#else
#define DefDlgProc  DefDlgProcA
#endif // !UNICODE

typedef enum DIALOG_CONTROL_DPI_CHANGE_BEHAVIORS {
     DCDC_DEFAULT                  = 0x0000,
     DCDC_DISABLE_FONT_UPDATE      = 0x0001,
     DCDC_DISABLE_RELAYOUT         = 0x0002,
} DIALOG_CONTROL_DPI_CHANGE_BEHAVIORS;

#ifndef MIDL_PASS
DEFINE_ENUM_FLAG_OPERATORS(DIALOG_CONTROL_DPI_CHANGE_BEHAVIORS);
#endif

_Win32_metadata_set_last_error_
BOOL
WINAPI
SetDialogControlDpiChangeBehavior(
    _In_ HWND hWnd,
    _In_ DIALOG_CONTROL_DPI_CHANGE_BEHAVIORS mask,
    _In_ DIALOG_CONTROL_DPI_CHANGE_BEHAVIORS values);

_Win32_metadata_set_last_error_
DIALOG_CONTROL_DPI_CHANGE_BEHAVIORS
WINAPI
GetDialogControlDpiChangeBehavior(
    _In_ HWND hWnd);

typedef enum DIALOG_DPI_CHANGE_BEHAVIORS {
    DDC_DEFAULT                     = 0x0000,
    DDC_DISABLE_ALL                 = 0x0001,
    DDC_DISABLE_RESIZE              = 0x0002,
    DDC_DISABLE_CONTROL_RELAYOUT    = 0x0004,
} DIALOG_DPI_CHANGE_BEHAVIORS;

#ifndef MIDL_PASS
DEFINE_ENUM_FLAG_OPERATORS(DIALOG_DPI_CHANGE_BEHAVIORS);
#endif

_Win32_metadata_set_last_error_
BOOL
WINAPI
SetDialogDpiChangeBehavior(
    _In_ HWND hDlg,
    _In_ DIALOG_DPI_CHANGE_BEHAVIORS mask,
    _In_ DIALOG_DPI_CHANGE_BEHAVIORS values);

_Win32_metadata_set_last_error_
DIALOG_DPI_CHANGE_BEHAVIORS
WINAPI
GetDialogDpiChangeBehavior(
    _In_ HWND hDlg);

#endif /* WINAPI_FAMILY_PARTITION(WINAPI_PARTITION_DESKTOP) */
#pragma endregion

/*
 * Window extra byted needed for private dialog classes.
 */
#ifndef _MAC
#define DLGWINDOWEXTRA 30
#else
#define DLGWINDOWEXTRA 48
#endif

#endif /* !NOCTLMGR */

#pragma region Desktop Family
#if WINAPI_FAMILY_PARTITION(WINAPI_PARTITION_DESKTOP)

#ifndef NOMSG

WINUSERAPI
BOOL
WINAPI
CallMsgFilterA(
    _In_ LPMSG lpMsg,
    _In_ int nCode);
WINUSERAPI
BOOL
WINAPI
CallMsgFilterW(
    _In_ LPMSG lpMsg,
    _In_ int nCode);
#ifdef UNICODE
#define CallMsgFilter  CallMsgFilterW
#else
#define CallMsgFilter  CallMsgFilterA
#endif // !UNICODE

#endif /* !NOMSG */

#ifndef NOCLIPBOARD

/*
 * Clipboard Manager Functions
 */

_Win32_metadata_set_last_error_
WINUSERAPI
BOOL
WINAPI
OpenClipboard(
    _In_opt_ HWND hWndNewOwner);

_Win32_metadata_set_last_error_
WINUSERAPI
BOOL
WINAPI
CloseClipboard(
    VOID);


#if(WINVER >= 0x0500)

WINUSERAPI
DWORD
WINAPI
GetClipboardSequenceNumber(
    VOID);

#endif /* WINVER >= 0x0500 */

_Win32_metadata_set_last_error_
WINUSERAPI
HWND
WINAPI
GetClipboardOwner(
    VOID);

_Win32_metadata_set_last_error_
WINUSERAPI
HWND
WINAPI
SetClipboardViewer(
    _In_ HWND hWndNewViewer);

_Win32_metadata_set_last_error_
WINUSERAPI
HWND
WINAPI
GetClipboardViewer(
    VOID);

WINUSERAPI
BOOL
WINAPI
ChangeClipboardChain(
    _In_ HWND hWndRemove,
    _In_ HWND hWndNewNext);

_Win32_metadata_set_last_error_
WINUSERAPI
HANDLE
WINAPI
SetClipboardData(
    _In_ UINT uFormat,
    _In_opt_ HANDLE hMem);

_Win32_metadata_set_last_error_
WINUSERAPI
HANDLE
WINAPI
GetClipboardData(
    _In_ UINT uFormat);

typedef struct tagGETCLIPBMETADATA {

    UINT Version; // Currently version 1; increment this value to add more fields.
    BOOL IsDelayRendered; // Indicates if the data is delay-rendered.
    BOOL IsSynthetic; // Indicates if the data is produced on-demand from another format (some bitmap/text/metafile).

} GETCLIPBMETADATA, *PGETCLIPBMETADATA;

WINUSERAPI
BOOL
WINAPI
GetClipboardMetadata(
    UINT format,
    _Inout_ PGETCLIPBMETADATA metadata);

_Win32_metadata_set_last_error_
WINUSERAPI
UINT
WINAPI
RegisterClipboardFormatA(
    _In_ LPCSTR lpszFormat);
_Win32_metadata_set_last_error_
WINUSERAPI
UINT
WINAPI
RegisterClipboardFormatW(
    _In_ LPCWSTR lpszFormat);
#ifdef UNICODE
#define RegisterClipboardFormat  RegisterClipboardFormatW
#else
#define RegisterClipboardFormat  RegisterClipboardFormatA
#endif // !UNICODE

_Win32_metadata_set_last_error_
WINUSERAPI
int
WINAPI
CountClipboardFormats(
    VOID);

_Win32_metadata_set_last_error_
WINUSERAPI
UINT
WINAPI
EnumClipboardFormats(
    _In_ UINT format);

_Win32_metadata_set_last_error_
WINUSERAPI
int
WINAPI
GetClipboardFormatNameA(
    _In_ UINT format,
    _Out_writes_(cchMaxCount) LPSTR lpszFormatName,
    _In_ int cchMaxCount);
_Win32_metadata_set_last_error_
WINUSERAPI
int
WINAPI
GetClipboardFormatNameW(
    _In_ UINT format,
    _Out_writes_(cchMaxCount) LPWSTR lpszFormatName,
    _In_ int cchMaxCount);
#ifdef UNICODE
#define GetClipboardFormatName  GetClipboardFormatNameW
#else
#define GetClipboardFormatName  GetClipboardFormatNameA
#endif // !UNICODE

_Win32_metadata_set_last_error_
WINUSERAPI
BOOL
WINAPI
EmptyClipboard(
    VOID);

_Win32_metadata_set_last_error_
WINUSERAPI
BOOL
WINAPI
IsClipboardFormatAvailable(
    _In_ UINT format);

_Win32_metadata_set_last_error_
WINUSERAPI
int
WINAPI
GetPriorityClipboardFormat(
    _In_reads_(cFormats) UINT *paFormatPriorityList,
    _In_ int cFormats);

_Win32_metadata_set_last_error_
WINUSERAPI
HWND
WINAPI
GetOpenClipboardWindow(
    VOID);

#if(WINVER >= 0x0600)
_Win32_metadata_set_last_error_
WINUSERAPI
BOOL
WINAPI
AddClipboardFormatListener(
    _In_ HWND hwnd);

_Win32_metadata_set_last_error_
WINUSERAPI
BOOL
WINAPI
RemoveClipboardFormatListener(
    _In_ HWND hwnd);

_Win32_metadata_set_last_error_
WINUSERAPI
BOOL
WINAPI
GetUpdatedClipboardFormats(
    _Out_writes_(cFormats) PUINT lpuiFormats,
    _In_ UINT cFormats,
    _Out_ PUINT pcFormatsOut);
#endif /* WINVER >= 0x0600 */

#endif /* !NOCLIPBOARD */

/*
 * Character Translation Routines
 */

_Win32_metadata_set_last_error_
WINUSERAPI
BOOL
WINAPI
CharToOemA(
    _In_ LPCSTR pSrc,
    _Out_writes_(_Inexpressible_(strlen(pSrc) + 1)) LPSTR pDst);
_Win32_metadata_set_last_error_
WINUSERAPI
BOOL
WINAPI
CharToOemW(
    _In_ LPCWSTR pSrc,
    _Out_writes_(_Inexpressible_(strlen(pSrc) + 1)) LPSTR pDst);
#ifdef UNICODE
#define CharToOem  CharToOemW
#else
#define CharToOem  CharToOemA
#endif // !UNICODE

__drv_preferredFunction("OemToCharBuff","Does not validate buffer size")
_Win32_metadata_set_last_error_
WINUSERAPI
BOOL
WINAPI
OemToCharA(
    _In_ LPCSTR pSrc,
    _Out_writes_(_Inexpressible_(strlen(pSrc) + 1)) LPSTR pDst);
__drv_preferredFunction("OemToCharBuff","Does not validate buffer size")
_Win32_metadata_set_last_error_
WINUSERAPI
BOOL
WINAPI
OemToCharW(
    _In_ LPCSTR pSrc,
    _Out_writes_(_Inexpressible_(strlen(pSrc) + 1)) LPWSTR pDst);
#ifdef UNICODE
#define OemToChar  OemToCharW
#else
#define OemToChar  OemToCharA
#endif // !UNICODE

_Win32_metadata_set_last_error_
WINUSERAPI
BOOL
WINAPI
CharToOemBuffA(
    _In_ LPCSTR lpszSrc,
    _Out_writes_(cchDstLength) LPSTR lpszDst,
    _In_ DWORD cchDstLength);
_Win32_metadata_set_last_error_
WINUSERAPI
BOOL
WINAPI
CharToOemBuffW(
    _In_ LPCWSTR lpszSrc,
    _Out_writes_(cchDstLength) LPSTR lpszDst,
    _In_ DWORD cchDstLength);
#ifdef UNICODE
#define CharToOemBuff  CharToOemBuffW
#else
#define CharToOemBuff  CharToOemBuffA
#endif // !UNICODE

_Win32_metadata_set_last_error_
WINUSERAPI
BOOL
WINAPI
OemToCharBuffA(
    _In_ LPCSTR lpszSrc,
    _Out_writes_(cchDstLength) LPSTR lpszDst,
    _In_ DWORD cchDstLength);
_Win32_metadata_set_last_error_
WINUSERAPI
BOOL
WINAPI
OemToCharBuffW(
    _In_ LPCSTR lpszSrc,
    _Out_writes_(cchDstLength) LPWSTR lpszDst,
    _In_ DWORD cchDstLength);
#ifdef UNICODE
#define OemToCharBuff  OemToCharBuffW
#else
#define OemToCharBuff  OemToCharBuffA
#endif // !UNICODE

#endif /* WINAPI_FAMILY_PARTITION(WINAPI_PARTITION_DESKTOP) */
#pragma endregion

#pragma region Desktop Family or OneCore Family
#if WINAPI_FAMILY_PARTITION(WINAPI_PARTITION_DESKTOP | WINAPI_PARTITION_SYSTEM)

_Win32_metadata_set_last_error_
WINUSERAPI
LPSTR
WINAPI
CharUpperA(
    _Inout_ LPSTR lpsz);
_Win32_metadata_set_last_error_
WINUSERAPI
LPWSTR
WINAPI
CharUpperW(
    _Inout_ LPWSTR lpsz);
#ifdef UNICODE
#define CharUpper  CharUpperW
#else
#define CharUpper  CharUpperA
#endif // !UNICODE

WINUSERAPI
DWORD
WINAPI
CharUpperBuffA(
    _Inout_updates_(cchLength) LPSTR lpsz,
    _In_ DWORD cchLength);
WINUSERAPI
DWORD
WINAPI
CharUpperBuffW(
    _Inout_updates_(cchLength) LPWSTR lpsz,
    _In_ DWORD cchLength);
#ifdef UNICODE
#define CharUpperBuff  CharUpperBuffW
#else
#define CharUpperBuff  CharUpperBuffA
#endif // !UNICODE

_Win32_metadata_set_last_error_
WINUSERAPI
LPSTR
WINAPI
CharLowerA(
    _Inout_ LPSTR lpsz);
_Win32_metadata_set_last_error_
WINUSERAPI
LPWSTR
WINAPI
CharLowerW(
    _Inout_ LPWSTR lpsz);
#ifdef UNICODE
#define CharLower  CharLowerW
#else
#define CharLower  CharLowerA
#endif // !UNICODE

WINUSERAPI
DWORD
WINAPI
CharLowerBuffA(
    _Inout_updates_(cchLength) LPSTR lpsz,
    _In_ DWORD cchLength);
WINUSERAPI
DWORD
WINAPI
CharLowerBuffW(
    _Inout_updates_(cchLength) LPWSTR lpsz,
    _In_ DWORD cchLength);
#ifdef UNICODE
#define CharLowerBuff  CharLowerBuffW
#else
#define CharLowerBuff  CharLowerBuffA
#endif // !UNICODE

WINUSERAPI
LPSTR
WINAPI
CharNextA(
    _In_ LPCSTR lpsz);
WINUSERAPI
LPWSTR
WINAPI
CharNextW(
    _In_ LPCWSTR lpsz);
#ifdef UNICODE
#define CharNext  CharNextW
#else
#define CharNext  CharNextA
#endif // !UNICODE

WINUSERAPI
LPSTR
WINAPI
CharPrevA(
    _In_ LPCSTR lpszStart,
    _In_ LPCSTR lpszCurrent);
WINUSERAPI
LPWSTR
WINAPI
CharPrevW(
    _In_ LPCWSTR lpszStart,
    _In_ LPCWSTR lpszCurrent);
#ifdef UNICODE
#define CharPrev  CharPrevW
#else
#define CharPrev  CharPrevA
#endif // !UNICODE

#if(WINVER >= 0x0400)
WINUSERAPI
LPSTR
WINAPI
CharNextExA(
     _In_ WORD CodePage,
     _In_ LPCSTR lpCurrentChar,
     _In_ DWORD dwFlags);

WINUSERAPI
LPSTR
WINAPI
CharPrevExA(
     _In_ WORD CodePage,
     _In_ LPCSTR lpStart,
     _In_ LPCSTR lpCurrentChar,
     _In_ DWORD dwFlags);
#endif /* WINVER >= 0x0400 */

#endif /* WINAPI_FAMILY_PARTITION(WINAPI_PARTITION_DESKTOP | WINAPI_PARTITION_SYSTEM) */
#pragma endregion

/*
 * Compatibility defines for character translation routines
 */
#define AnsiToOem CharToOemA
#define OemToAnsi OemToCharA
#define AnsiToOemBuff CharToOemBuffA
#define OemToAnsiBuff OemToCharBuffA
#define AnsiUpper CharUpperA
#define AnsiUpperBuff CharUpperBuffA
#define AnsiLower CharLowerA
#define AnsiLowerBuff CharLowerBuffA
#define AnsiNext CharNextA
#define AnsiPrev CharPrevA

#pragma region Desktop or OneCore Family
#if WINAPI_FAMILY_PARTITION(WINAPI_PARTITION_DESKTOP | WINAPI_PARTITION_SYSTEM)

#ifndef  NOLANGUAGE
/*
 * Language dependent Routines
 */

_Win32_metadata_set_last_error_
WINUSERAPI
BOOL
WINAPI
IsCharAlphaA(
    _In_ CHAR ch);
_Win32_metadata_set_last_error_
WINUSERAPI
BOOL
WINAPI
IsCharAlphaW(
    _In_ WCHAR ch);
#ifdef UNICODE
#define IsCharAlpha  IsCharAlphaW
#else
#define IsCharAlpha  IsCharAlphaA
#endif // !UNICODE

_Win32_metadata_set_last_error_
WINUSERAPI
BOOL
WINAPI
IsCharAlphaNumericA(
    _In_ CHAR ch);
_Win32_metadata_set_last_error_
WINUSERAPI
BOOL
WINAPI
IsCharAlphaNumericW(
    _In_ WCHAR ch);
#ifdef UNICODE
#define IsCharAlphaNumeric  IsCharAlphaNumericW
#else
#define IsCharAlphaNumeric  IsCharAlphaNumericA
#endif // !UNICODE

_Win32_metadata_set_last_error_
WINUSERAPI
BOOL
WINAPI
IsCharUpperA(
    _In_ CHAR ch);
_Win32_metadata_set_last_error_
WINUSERAPI
BOOL
WINAPI
IsCharUpperW(
    _In_ WCHAR ch);
#ifdef UNICODE
#define IsCharUpper  IsCharUpperW
#else
#define IsCharUpper  IsCharUpperA
#endif // !UNICODE

_Win32_metadata_set_last_error_
WINUSERAPI
BOOL
WINAPI
IsCharLowerA(
    _In_ CHAR ch);
_Win32_metadata_set_last_error_
WINUSERAPI
BOOL
WINAPI
IsCharLowerW(
    _In_ WCHAR ch);
#ifdef UNICODE
#define IsCharLower  IsCharLowerW
#else
#define IsCharLower  IsCharLowerA
#endif // !UNICODE

#endif  /* !NOLANGUAGE */

#endif /* WINAPI_FAMILY_PARTITION(WINAPI_PARTITION_DESKTOP | WINAPI_PARTITION_SYSTEM) */
#pragma endregion

#pragma region Desktop Family
#if WINAPI_FAMILY_PARTITION(WINAPI_PARTITION_DESKTOP)

_Win32_metadata_set_last_error_
WINUSERAPI
HWND
WINAPI
SetFocus(
    _In_opt_ HWND hWnd);

WINUSERAPI
HWND
WINAPI
GetActiveWindow(
    VOID);

WINUSERAPI
HWND
WINAPI
GetFocus(
    VOID);

WINUSERAPI
UINT
WINAPI
GetKBCodePage(
    VOID);

WINUSERAPI
SHORT
WINAPI
GetKeyState(
    _In_ int nVirtKey);

WINUSERAPI
SHORT
WINAPI
GetAsyncKeyState(
    _In_ int vKey);

_Win32_metadata_set_last_error_
WINUSERAPI
_Check_return_
BOOL
WINAPI
GetKeyboardState(
    _Out_writes_(256) PBYTE lpKeyState);

_Win32_metadata_set_last_error_
WINUSERAPI
BOOL
WINAPI
SetKeyboardState(
    _In_reads_(256) LPBYTE lpKeyState);


#endif /* WINAPI_FAMILY_PARTITION(WINAPI_PARTITION_DESKTOP) */
#pragma endregion

#pragma region  Desktop or PC Family
#if WINAPI_FAMILY_PARTITION(WINAPI_PARTITION_DESKTOP | WINAPI_PARTITION_PC_APP)

_Win32_metadata_set_last_error_
WINUSERAPI
int
WINAPI
GetKeyNameTextA(
    _In_ LONG lParam,
    _Out_writes_(cchSize) LPSTR lpString,
    _In_ int cchSize);
_Win32_metadata_set_last_error_
WINUSERAPI
int
WINAPI
GetKeyNameTextW(
    _In_ LONG lParam,
    _Out_writes_(cchSize) LPWSTR lpString,
    _In_ int cchSize);
#ifdef UNICODE
#define GetKeyNameText  GetKeyNameTextW
#else
#define GetKeyNameText  GetKeyNameTextA
#endif // !UNICODE

#endif /* WINAPI_FAMILY_PARTITION(WINAPI_PARTITION_DESKTOP | WINAPI_PARTITION_PC_APP) */
#pragma endregion

#pragma region Desktop Family
#if WINAPI_FAMILY_PARTITION(WINAPI_PARTITION_DESKTOP)


_Win32_metadata_set_last_error_
WINUSERAPI
int
WINAPI
GetKeyboardType(
    _In_ int nTypeFlag);

WINUSERAPI
int
WINAPI
ToAscii(
    _In_ UINT uVirtKey,
    _In_ UINT uScanCode,
    _In_reads_opt_(256) CONST BYTE *lpKeyState,
    _Out_ LPWORD lpChar,
    _In_ UINT uFlags);

#if(WINVER >= 0x0400)
WINUSERAPI
int
WINAPI
ToAsciiEx(
    _In_ UINT uVirtKey,
    _In_ UINT uScanCode,
    _In_reads_opt_(256) CONST BYTE *lpKeyState,
    _Out_ LPWORD lpChar,
    _In_ UINT uFlags,
    _In_opt_ HKL dwhkl);
#endif /* WINVER >= 0x0400 */

WINUSERAPI
int
WINAPI
ToUnicode(
    _In_ UINT wVirtKey,
    _In_ UINT wScanCode,
    _In_reads_bytes_opt_(256) CONST BYTE *lpKeyState,
    _Out_writes_(cchBuff) LPWSTR pwszBuff,
    _In_ int cchBuff,
    _In_ UINT wFlags);

WINUSERAPI
DWORD
WINAPI
OemKeyScan(
    _In_ WORD wOemChar);

WINUSERAPI
SHORT
WINAPI
VkKeyScanA(
    _In_ CHAR ch);
WINUSERAPI
SHORT
WINAPI
VkKeyScanW(
    _In_ WCHAR ch);
#ifdef UNICODE
#define VkKeyScan  VkKeyScanW
#else
#define VkKeyScan  VkKeyScanA
#endif // !UNICODE

#if(WINVER >= 0x0400)
WINUSERAPI
SHORT
WINAPI
VkKeyScanExA(
    _In_ CHAR ch,
    _In_ HKL dwhkl);
WINUSERAPI
SHORT
WINAPI
VkKeyScanExW(
    _In_ WCHAR ch,
    _In_ HKL dwhkl);
#ifdef UNICODE
#define VkKeyScanEx  VkKeyScanExW
#else
#define VkKeyScanEx  VkKeyScanExA
#endif // !UNICODE
#endif /* WINVER >= 0x0400 */
#define KEYEVENTF_EXTENDEDKEY 0x0001
#define KEYEVENTF_KEYUP       0x0002
#if(_WIN32_WINNT >= 0x0500)
#define KEYEVENTF_UNICODE     0x0004
#define KEYEVENTF_SCANCODE    0x0008
#endif /* _WIN32_WINNT >= 0x0500 */

WINUSERAPI
VOID
WINAPI
keybd_event(
    _In_ BYTE bVk,
    _In_ BYTE bScan,
    _In_ KEYBD_EVENT_FLAGS dwFlags,
    _In_ ULONG_PTR dwExtraInfo);

#endif /* WINAPI_FAMILY_PARTITION(WINAPI_PARTITION_DESKTOP) */
#pragma endregion

#define MOUSEEVENTF_MOVE        0x0001 /* mouse move */
#define MOUSEEVENTF_LEFTDOWN    0x0002 /* left button down */
#define MOUSEEVENTF_LEFTUP      0x0004 /* left button up */
#define MOUSEEVENTF_RIGHTDOWN   0x0008 /* right button down */
#define MOUSEEVENTF_RIGHTUP     0x0010 /* right button up */
#define MOUSEEVENTF_MIDDLEDOWN  0x0020 /* middle button down */
#define MOUSEEVENTF_MIDDLEUP    0x0040 /* middle button up */
#define MOUSEEVENTF_XDOWN       0x0080 /* x button down */
#define MOUSEEVENTF_XUP         0x0100 /* x button down */
#define MOUSEEVENTF_WHEEL                0x0800 /* wheel button rolled */
#if (_WIN32_WINNT >= 0x0600)
#define MOUSEEVENTF_HWHEEL              0x01000 /* hwheel button rolled */
#endif
#if(WINVER >= 0x0600)
#define MOUSEEVENTF_MOVE_NOCOALESCE      0x2000 /* do not coalesce mouse moves */
#endif /* WINVER >= 0x0600 */
#define MOUSEEVENTF_VIRTUALDESK          0x4000 /* map to entire virtual desktop */
#define MOUSEEVENTF_ABSOLUTE             0x8000 /* absolute move */

#pragma region Desktop Family
#if WINAPI_FAMILY_PARTITION(WINAPI_PARTITION_DESKTOP)

WINUSERAPI
VOID
WINAPI
mouse_event(
    _In_ MOUSE_EVENT_FLAGS dwFlags,
    _In_ DWORD dx,
    _In_ DWORD dy,
    _In_ DWORD dwData,
    _In_ ULONG_PTR dwExtraInfo);

#endif /* WINAPI_FAMILY_PARTITION(WINAPI_PARTITION_DESKTOP) */
#pragma endregion

#if (_WIN32_WINNT > 0x0400)

typedef struct tagMOUSEINPUT {
    LONG    dx;
    LONG    dy;
    DWORD   mouseData;
    MOUSE_EVENT_FLAGS   dwFlags;
    DWORD   time;
    ULONG_PTR dwExtraInfo;
} MOUSEINPUT, *PMOUSEINPUT, FAR* LPMOUSEINPUT;

typedef struct tagKEYBDINPUT {
    VIRTUAL_KEY    wVk;
    WORD    wScan;
    KEYBD_EVENT_FLAGS dwFlags;
    DWORD   time;

    /*
     * When dwFlags has KEYEVENTF_SYSTEMINJECTION specified this field may carry
     * KEY_UNICODE_SEQUENCE_ITEM or KEY_UNICODE_SEQUENCE_END which are used by InputService
     * to distinguish injected unicode sequences. Those flags are stored in low word.
     * When dwFlags has KEYEVENTF_ATTRIBUTED_INPUT specified this field carries in its high word
     * ID of attributes associated with injected input. This ID is assigned by InputService and
     * recognized only by it.
     * For all other usage scenarios please refer to official documentation.
     */
    ULONG_PTR dwExtraInfo;
} KEYBDINPUT, *PKEYBDINPUT, FAR* LPKEYBDINPUT;

#pragma region Desktop Family
#if WINAPI_FAMILY_PARTITION(WINAPI_PARTITION_DESKTOP)

typedef struct tagHARDWAREINPUT {
    DWORD   uMsg;
    WORD    wParamL;
    WORD    wParamH;
} HARDWAREINPUT, *PHARDWAREINPUT, FAR* LPHARDWAREINPUT;

#define INPUT_MOUSE     0
#define INPUT_KEYBOARD  1
#define INPUT_HARDWARE  2

typedef struct tagINPUT {
    INPUT_TYPE   type;

    union
    {
        MOUSEINPUT      mi;
        KEYBDINPUT      ki;
        HARDWAREINPUT   hi;
    } DUMMYUNIONNAME;
} INPUT, *PINPUT, FAR* LPINPUT;

_Win32_metadata_set_last_error_
WINUSERAPI
UINT
WINAPI
SendInput(
    _In_ UINT cInputs,                     // number of input in the array
    _In_reads_(cInputs) LPINPUT pInputs,  // array of inputs
    _In_ int cbSize);                      // sizeof(INPUT)

#endif /* WINAPI_FAMILY_PARTITION(WINAPI_PARTITION_DESKTOP) */
#pragma endregion

#endif // (_WIN32_WINNT > 0x0400)

#if(WINVER >= 0x0601)

#pragma region Desktop Family
#if WINAPI_FAMILY_PARTITION(WINAPI_PARTITION_DESKTOP)

/*
 * Touch Input defines and functions
 */

/*
 * Touch input handle
 */
DECLARE_HANDLE(HTOUCHINPUT);

typedef struct tagTOUCHINPUT {
    LONG x;
    LONG y;
    HANDLE hSource;
    DWORD dwID;
    TOUCHEVENTF_FLAGS dwFlags;
    TOUCHINPUTMASKF_MASK dwMask;
    DWORD dwTime;
    ULONG_PTR dwExtraInfo;
    DWORD cxContact;
    DWORD cyContact;
} TOUCHINPUT, *PTOUCHINPUT;
typedef TOUCHINPUT const * PCTOUCHINPUT;


#endif /* WINAPI_FAMILY_PARTITION(WINAPI_PARTITION_DESKTOP) */
#pragma endregion

/*
 * Conversion of touch input coordinates to pixels
 */
#define TOUCH_COORD_TO_PIXEL(l)         ((l) / 100)

/*
 * Touch input flag values (TOUCHINPUT.dwFlags)
 */
#define TOUCHEVENTF_MOVE            0x0001
#define TOUCHEVENTF_DOWN            0x0002
#define TOUCHEVENTF_UP              0x0004
#define TOUCHEVENTF_INRANGE         0x0008
#define TOUCHEVENTF_PRIMARY         0x0010
#define TOUCHEVENTF_NOCOALESCE      0x0020
#define TOUCHEVENTF_PEN             0x0040
#define TOUCHEVENTF_PALM            0x0080

/*
 * Touch input mask values (TOUCHINPUT.dwMask)
 */
#define TOUCHINPUTMASKF_TIMEFROMSYSTEM  0x0001  // the dwTime field contains a system generated value
#define TOUCHINPUTMASKF_EXTRAINFO       0x0002  // the dwExtraInfo field is valid
#define TOUCHINPUTMASKF_CONTACTAREA     0x0004  // the cxContact and cyContact fields are valid

#pragma region Desktop Family
#if WINAPI_FAMILY_PARTITION(WINAPI_PARTITION_DESKTOP)

_Win32_metadata_set_last_error_
WINUSERAPI
BOOL
WINAPI
GetTouchInputInfo(
    _In_ HTOUCHINPUT hTouchInput,               // input event handle; from touch message lParam
    _In_ UINT cInputs,                          // number of elements in the array
    _Out_writes_(cInputs) PTOUCHINPUT pInputs,  // array of touch inputs
    _In_ int cbSize);                           // sizeof(TOUCHINPUT)

_Win32_metadata_set_last_error_
WINUSERAPI
BOOL
WINAPI
CloseTouchInputHandle(
    _In_ HTOUCHINPUT hTouchInput);                   // input event handle; from touch message lParam


#endif /* WINAPI_FAMILY_PARTITION(WINAPI_PARTITION_DESKTOP) */
#pragma endregion


/*
 * RegisterTouchWindow flag values
 */
#define TWF_FINETOUCH       (0x00000001)
#define TWF_WANTPALM        (0x00000002)

#pragma region Desktop Family
#if WINAPI_FAMILY_PARTITION(WINAPI_PARTITION_DESKTOP)

_Win32_metadata_set_last_error_
WINUSERAPI
BOOL
WINAPI
RegisterTouchWindow(
    _In_ HWND hwnd,
    _In_ REGISTER_TOUCH_WINDOW_FLAGS ulFlags);

_Win32_metadata_set_last_error_
WINUSERAPI
BOOL
WINAPI
UnregisterTouchWindow(
    _In_ HWND hwnd);

WINUSERAPI
BOOL
WINAPI
IsTouchWindow(
    _In_ HWND hwnd,
    _Out_opt_ PULONG pulFlags);

#endif /* WINAPI_FAMILY_PARTITION(WINAPI_PARTITION_DESKTOP) */
#pragma endregion

#endif /* WINVER >= 0x0601 */

#if(WINVER >= 0x0602)

#pragma region Desktop Family
#if WINAPI_FAMILY_PARTITION(WINAPI_PARTITION_DESKTOP)

#define POINTER_STRUCTURES

enum tagPOINTER_INPUT_TYPE {
    PT_POINTER  = 1,   // Generic pointer
    PT_TOUCH    = 2,   // Touch
    PT_PEN      = 3,   // Pen
    PT_MOUSE    = 4,   // Mouse
#if(WINVER >= 0x0603)
    PT_TOUCHPAD = 5,   // Touchpad
#endif /* WINVER >= 0x0603 */
};


typedef DWORD POINTER_INPUT_TYPE;

#endif /* WINAPI_FAMILY_PARTITION(WINAPI_PARTITION_DESKTOP) */
#pragma endregion

#define POINTER_FLAG_NONE               0x00000000 // Default
#define POINTER_FLAG_NEW                0x00000001 // New pointer
#define POINTER_FLAG_INRANGE            0x00000002 // Pointer has not departed
#define POINTER_FLAG_INCONTACT          0x00000004 // Pointer is in contact
#define POINTER_FLAG_FIRSTBUTTON        0x00000010 // Primary action
#define POINTER_FLAG_SECONDBUTTON       0x00000020 // Secondary action
#define POINTER_FLAG_THIRDBUTTON        0x00000040 // Third button
#define POINTER_FLAG_FOURTHBUTTON       0x00000080 // Fourth button
#define POINTER_FLAG_FIFTHBUTTON        0x00000100 // Fifth button
#define POINTER_FLAG_PRIMARY            0x00002000 // Pointer is primary for system
#define POINTER_FLAG_CONFIDENCE         0x00004000 // Pointer is considered unlikely to be accidental
#define POINTER_FLAG_CANCELED           0x00008000 // Pointer is departing in an abnormal manner
#define POINTER_FLAG_DOWN               0x00010000 // Pointer transitioned to down state (made contact)
#define POINTER_FLAG_UPDATE             0x00020000 // Pointer update
#define POINTER_FLAG_UP                 0x00040000 // Pointer transitioned from down state (broke contact)
#define POINTER_FLAG_WHEEL              0x00080000 // Vertical wheel
#define POINTER_FLAG_HWHEEL             0x00100000 // Horizontal wheel
#define POINTER_FLAG_CAPTURECHANGED     0x00200000 // Lost capture
#define POINTER_FLAG_HASTRANSFORM       0x00400000 // Input has a transform associated with it


/*
 * Pointer info key states defintions.
 */
#define POINTER_MOD_SHIFT   (0x0004)    // Shift key is held down.
#define POINTER_MOD_CTRL    (0x0008)    // Ctrl key is held down.

#pragma region Desktop Family
#if WINAPI_FAMILY_PARTITION(WINAPI_PARTITION_DESKTOP)

typedef enum tagPOINTER_BUTTON_CHANGE_TYPE {
    POINTER_CHANGE_NONE,
    POINTER_CHANGE_FIRSTBUTTON_DOWN,
    POINTER_CHANGE_FIRSTBUTTON_UP,
    POINTER_CHANGE_SECONDBUTTON_DOWN,
    POINTER_CHANGE_SECONDBUTTON_UP,
    POINTER_CHANGE_THIRDBUTTON_DOWN,
    POINTER_CHANGE_THIRDBUTTON_UP,
    POINTER_CHANGE_FOURTHBUTTON_DOWN,
    POINTER_CHANGE_FOURTHBUTTON_UP,
    POINTER_CHANGE_FIFTHBUTTON_DOWN,
    POINTER_CHANGE_FIFTHBUTTON_UP,
} POINTER_BUTTON_CHANGE_TYPE;

typedef struct tagPOINTER_INFO {
    POINTER_INPUT_TYPE    pointerType;
    UINT32          pointerId;
    UINT32          frameId;
    POINTER_FLAGS   pointerFlags;
    HANDLE          sourceDevice;
    HWND            hwndTarget;
    POINT           ptPixelLocation;
    POINT           ptHimetricLocation;
    POINT           ptPixelLocationRaw;
    POINT           ptHimetricLocationRaw;
    DWORD           dwTime;
    UINT32          historyCount;
    INT32           InputData;
    DWORD           dwKeyStates;
    UINT64          PerformanceCount;
    POINTER_BUTTON_CHANGE_TYPE ButtonChangeType;
} POINTER_INFO;


typedef UINT32 TOUCH_FLAGS;
#define TOUCH_FLAG_NONE                 0x00000000 // Default

typedef UINT32 TOUCH_MASK;
#define TOUCH_MASK_NONE                 0x00000000 // Default - none of the optional fields are valid
#define TOUCH_MASK_CONTACTAREA          0x00000001 // The rcContact field is valid
#define TOUCH_MASK_ORIENTATION          0x00000002 // The orientation field is valid
#define TOUCH_MASK_PRESSURE             0x00000004 // The pressure field is valid

typedef struct tagPOINTER_TOUCH_INFO {
    POINTER_INFO    pointerInfo;
    TOUCH_FLAGS     touchFlags;
    TOUCH_MASK      touchMask;
    RECT            rcContact;
    RECT            rcContactRaw;
    UINT32          orientation;
    UINT32          pressure;
} POINTER_TOUCH_INFO;

typedef UINT32 PEN_FLAGS;
#define PEN_FLAG_NONE                   0x00000000 // Default
#define PEN_FLAG_BARREL                 0x00000001 // The barrel button is pressed
#define PEN_FLAG_INVERTED               0x00000002 // The pen is inverted
#define PEN_FLAG_ERASER                 0x00000004 // The eraser button is pressed

typedef UINT32 PEN_MASK;
#define PEN_MASK_NONE                   0x00000000 // Default - none of the optional fields are valid
#define PEN_MASK_PRESSURE               0x00000001 // The pressure field is valid
#define PEN_MASK_ROTATION               0x00000002 // The rotation field is valid
#define PEN_MASK_TILT_X                 0x00000004 // The tiltX field is valid
#define PEN_MASK_TILT_Y                 0x00000008 // The tiltY field is valid

typedef struct tagPOINTER_PEN_INFO {
    POINTER_INFO    pointerInfo;
    PEN_FLAGS       penFlags;
    PEN_MASK        penMask;
    UINT32          pressure;
    UINT32          rotation;
    INT32           tiltX;
    INT32           tiltY;
} POINTER_PEN_INFO;

#endif /* WINAPI_FAMILY_PARTITION(WINAPI_PARTITION_DESKTOP) */
#pragma endregion

/*
 * Flags that appear in pointer input message parameters
 */
#define POINTER_MESSAGE_FLAG_NEW                0x00000001 // New pointer
#define POINTER_MESSAGE_FLAG_INRANGE            0x00000002 // Pointer has not departed
#define POINTER_MESSAGE_FLAG_INCONTACT          0x00000004 // Pointer is in contact
#define POINTER_MESSAGE_FLAG_FIRSTBUTTON        0x00000010 // Primary action
#define POINTER_MESSAGE_FLAG_SECONDBUTTON       0x00000020 // Secondary action
#define POINTER_MESSAGE_FLAG_THIRDBUTTON        0x00000040 // Third button
#define POINTER_MESSAGE_FLAG_FOURTHBUTTON       0x00000080 // Fourth button
#define POINTER_MESSAGE_FLAG_FIFTHBUTTON        0x00000100 // Fifth button
#define POINTER_MESSAGE_FLAG_PRIMARY            0x00002000 // Pointer is primary
#define POINTER_MESSAGE_FLAG_CONFIDENCE         0x00004000 // Pointer is considered unlikely to be accidental
#define POINTER_MESSAGE_FLAG_CANCELED           0x00008000 // Pointer is departing in an abnormal manner

/*
 * Macros to retrieve information from pointer input message parameters
 */
#define GET_POINTERID_WPARAM(wParam)                (LOWORD(wParam))
#define IS_POINTER_FLAG_SET_WPARAM(wParam, flag)    (((DWORD)HIWORD(wParam) & (flag)) == (flag))
#define IS_POINTER_NEW_WPARAM(wParam)               IS_POINTER_FLAG_SET_WPARAM(wParam, POINTER_MESSAGE_FLAG_NEW)
#define IS_POINTER_INRANGE_WPARAM(wParam)           IS_POINTER_FLAG_SET_WPARAM(wParam, POINTER_MESSAGE_FLAG_INRANGE)
#define IS_POINTER_INCONTACT_WPARAM(wParam)         IS_POINTER_FLAG_SET_WPARAM(wParam, POINTER_MESSAGE_FLAG_INCONTACT)
#define IS_POINTER_FIRSTBUTTON_WPARAM(wParam)       IS_POINTER_FLAG_SET_WPARAM(wParam, POINTER_MESSAGE_FLAG_FIRSTBUTTON)
#define IS_POINTER_SECONDBUTTON_WPARAM(wParam)      IS_POINTER_FLAG_SET_WPARAM(wParam, POINTER_MESSAGE_FLAG_SECONDBUTTON)
#define IS_POINTER_THIRDBUTTON_WPARAM(wParam)       IS_POINTER_FLAG_SET_WPARAM(wParam, POINTER_MESSAGE_FLAG_THIRDBUTTON)
#define IS_POINTER_FOURTHBUTTON_WPARAM(wParam)      IS_POINTER_FLAG_SET_WPARAM(wParam, POINTER_MESSAGE_FLAG_FOURTHBUTTON)
#define IS_POINTER_FIFTHBUTTON_WPARAM(wParam)       IS_POINTER_FLAG_SET_WPARAM(wParam, POINTER_MESSAGE_FLAG_FIFTHBUTTON)
#define IS_POINTER_PRIMARY_WPARAM(wParam)           IS_POINTER_FLAG_SET_WPARAM(wParam, POINTER_MESSAGE_FLAG_PRIMARY)
#define HAS_POINTER_CONFIDENCE_WPARAM(wParam)       IS_POINTER_FLAG_SET_WPARAM(wParam, POINTER_MESSAGE_FLAG_CONFIDENCE)
#define IS_POINTER_CANCELED_WPARAM(wParam)          IS_POINTER_FLAG_SET_WPARAM(wParam, POINTER_MESSAGE_FLAG_CANCELED)

/*
 * WM_POINTERACTIVATE return codes
 */
#define PA_ACTIVATE                     MA_ACTIVATE
#define PA_NOACTIVATE                   MA_NOACTIVATE


#define MAX_TOUCH_COUNT 256

#define TOUCH_FEEDBACK_DEFAULT 0x1
#define TOUCH_FEEDBACK_INDIRECT 0x2
#define TOUCH_FEEDBACK_NONE 0x3

typedef enum {
    POINTER_FEEDBACK_DEFAULT = 1,   // The injected pointer input feedback may get suppressed by the end-user settings in the Pen and Touch control panel.
    POINTER_FEEDBACK_INDIRECT = 2,  // The injected pointer input feedback overrides the end-user settings in the Pen and Touch control panel.
    POINTER_FEEDBACK_NONE = 3,      // No touch visualizations.
} POINTER_FEEDBACK_MODE;

#pragma region Desktop Family
#if WINAPI_FAMILY_PARTITION(WINAPI_PARTITION_DESKTOP)

_Win32_metadata_set_last_error_
WINUSERAPI
BOOL
WINAPI
InitializeTouchInjection(
    _In_ UINT32 maxCount,
    _In_ TOUCH_FEEDBACK_MODE dwMode);

_Win32_metadata_set_last_error_
WINUSERAPI
BOOL
WINAPI
InjectTouchInput(
    _In_ UINT32 count,
    _In_reads_(count) CONST POINTER_TOUCH_INFO *contacts);

typedef struct tagUSAGE_PROPERTIES {
    USHORT level;
    USHORT page;
    USHORT usage;
    INT32 logicalMinimum;
    INT32 logicalMaximum;
    USHORT unit;
    USHORT exponent;
    BYTE   count;
    INT32 physicalMinimum;
    INT32 physicalMaximum;
}USAGE_PROPERTIES, *PUSAGE_PROPERTIES;

typedef struct tagPOINTER_TYPE_INFO {
    POINTER_INPUT_TYPE type;
    union{
        POINTER_INFO       pointerInfo;
        POINTER_TOUCH_INFO touchInfo;
        POINTER_PEN_INFO   penInfo;
    } DUMMYUNIONNAME;
} POINTER_TYPE_INFO, *PPOINTER_TYPE_INFO;

typedef struct tagINPUT_INJECTION_VALUE {
    USHORT page;
    USHORT usage;
    INT32  value;
    USHORT index;
}INPUT_INJECTION_VALUE, *PINPUT_INJECTION_VALUE;

_Win32_metadata_set_last_error_
WINUSERAPI
BOOL
WINAPI
GetPointerType(
    _In_ UINT32 pointerId,
    _Out_ POINTER_INPUT_TYPE *pointerType);

_Win32_metadata_set_last_error_
WINUSERAPI
BOOL
WINAPI
GetPointerCursorId(
    _In_ UINT32 pointerId,
    _Out_ UINT32 *cursorId);

_Win32_metadata_set_last_error_
WINUSERAPI
BOOL
WINAPI
GetPointerInfo(
    _In_ UINT32 pointerId,
    _Out_writes_(1) POINTER_INFO *pointerInfo);

_Win32_metadata_set_last_error_
WINUSERAPI
BOOL
WINAPI
GetPointerInfoHistory(
    _In_ UINT32 pointerId,
    _Inout_ UINT32 *entriesCount,
    _Out_writes_opt_(*entriesCount) POINTER_INFO *pointerInfo);

_Win32_metadata_set_last_error_
WINUSERAPI
BOOL
WINAPI
GetPointerFrameInfo(
    _In_ UINT32 pointerId,
    _Inout_ UINT32 *pointerCount,
    _Out_writes_opt_(*pointerCount) POINTER_INFO *pointerInfo);

_Win32_metadata_set_last_error_
WINUSERAPI
BOOL
WINAPI
GetPointerFrameInfoHistory(
    _In_ UINT32 pointerId,
    _Inout_ UINT32 *entriesCount,
    _Inout_ UINT32 *pointerCount,
    _Out_writes_opt_(*entriesCount * *pointerCount) POINTER_INFO *pointerInfo);

_Win32_metadata_set_last_error_
WINUSERAPI
BOOL
WINAPI
GetPointerTouchInfo(
    _In_ UINT32 pointerId,
    _Out_writes_(1) POINTER_TOUCH_INFO *touchInfo);

_Win32_metadata_set_last_error_
WINUSERAPI
BOOL
WINAPI
GetPointerTouchInfoHistory(
    _In_ UINT32 pointerId,
    _Inout_ UINT32 *entriesCount,
    _Out_writes_opt_(*entriesCount) POINTER_TOUCH_INFO *touchInfo);

_Win32_metadata_set_last_error_
WINUSERAPI
BOOL
WINAPI
GetPointerFrameTouchInfo(
    _In_ UINT32 pointerId,
    _Inout_ UINT32 *pointerCount,
    _Out_writes_opt_(*pointerCount) POINTER_TOUCH_INFO *touchInfo);

_Win32_metadata_set_last_error_
WINUSERAPI
BOOL
WINAPI
GetPointerFrameTouchInfoHistory(
    _In_ UINT32 pointerId,
    _Inout_ UINT32 *entriesCount,
    _Inout_ UINT32 *pointerCount,
    _Out_writes_opt_(*entriesCount * *pointerCount) POINTER_TOUCH_INFO *touchInfo);

_Win32_metadata_set_last_error_
WINUSERAPI
BOOL
WINAPI
GetPointerPenInfo(
    _In_ UINT32 pointerId,
    _Out_writes_(1) POINTER_PEN_INFO *penInfo);

_Win32_metadata_set_last_error_
WINUSERAPI
BOOL
WINAPI
GetPointerPenInfoHistory(
    _In_ UINT32 pointerId,
    _Inout_ UINT32 *entriesCount,
    _Out_writes_opt_(*entriesCount) POINTER_PEN_INFO *penInfo);

_Win32_metadata_set_last_error_
WINUSERAPI
BOOL
WINAPI
GetPointerFramePenInfo(
    _In_ UINT32 pointerId,
    _Inout_ UINT32 *pointerCount,
    _Out_writes_opt_(*pointerCount) POINTER_PEN_INFO *penInfo);

_Win32_metadata_set_last_error_
WINUSERAPI
BOOL
WINAPI
GetPointerFramePenInfoHistory(
    _In_ UINT32 pointerId,
    _Inout_ UINT32 *entriesCount,
    _Inout_ UINT32 *pointerCount,
    _Out_writes_opt_(*entriesCount * *pointerCount) POINTER_PEN_INFO *penInfo);

// TODO(47499024): Make public when Feature_TouchpadPublicApis3 is enabled

_Win32_metadata_set_last_error_
WINUSERAPI
BOOL
WINAPI
SkipPointerFrameMessages(
    _In_ UINT32 pointerId);

_Win32_metadata_set_last_error_
WINUSERAPI
BOOL
WINAPI
RegisterPointerInputTarget(
    _In_ HWND hwnd,
    _In_ POINTER_INPUT_TYPE pointerType);

_Win32_metadata_set_last_error_
WINUSERAPI
BOOL
WINAPI
UnregisterPointerInputTarget(
    _In_ HWND hwnd,
    _In_ POINTER_INPUT_TYPE pointerType);

WINUSERAPI
BOOL
WINAPI
RegisterPointerInputTargetEx(
    _In_ HWND hwnd,
    _In_ POINTER_INPUT_TYPE pointerType,
    _In_ BOOL fObserve);

WINUSERAPI
BOOL
WINAPI
UnregisterPointerInputTargetEx(
    _In_ HWND hwnd,
    _In_ POINTER_INPUT_TYPE pointerType);

#if (NTDDI_VERSION >= NTDDI_WIN10_RS5)
DECLARE_HANDLE(HSYNTHETICPOINTERDEVICE);
_Win32_metadata_set_last_error_
WINUSERAPI
HSYNTHETICPOINTERDEVICE
WINAPI
CreateSyntheticPointerDevice(
    _In_ POINTER_INPUT_TYPE pointerType,
    _In_ ULONG maxCount,
    _In_ POINTER_FEEDBACK_MODE mode);

_Win32_metadata_set_last_error_
WINUSERAPI
BOOL
WINAPI
InjectSyntheticPointerInput(
    _In_ HSYNTHETICPOINTERDEVICE device,
    _In_reads_(count) CONST POINTER_TYPE_INFO* pointerInfo,
    _In_ UINT32 count);

WINUSERAPI
VOID
WINAPI
DestroySyntheticPointerDevice(
    _In_ HSYNTHETICPOINTERDEVICE device);
#endif // NTDDI_VERSION >= NTDDI_WIN10_RS5

// TODO(47499024): Make public when Feature_TouchpadPublicApis3 is enabled

_Win32_metadata_set_last_error_
WINUSERAPI
BOOL
WINAPI
EnableMouseInPointer(
    _In_ BOOL fEnable);

WINUSERAPI
BOOL
WINAPI
IsMouseInPointerEnabled(
    VOID);

#if WDK_NTDDI_VERSION >= NTDDI_WIN10_RS3
WINUSERAPI
BOOL
WINAPI
EnableMouseInPointerForThread(VOID);
#endif


#define TOUCH_HIT_TESTING_DEFAULT 0x0
#define TOUCH_HIT_TESTING_CLIENT  0x1
#define TOUCH_HIT_TESTING_NONE    0x2

_Win32_metadata_set_last_error_
WINUSERAPI
BOOL
WINAPI
RegisterTouchHitTestingWindow(
    _In_ HWND hwnd,
    _In_ ULONG value);

typedef struct tagTOUCH_HIT_TESTING_PROXIMITY_EVALUATION
{
    UINT16 score;
    POINT adjustedPoint;
} TOUCH_HIT_TESTING_PROXIMITY_EVALUATION, *PTOUCH_HIT_TESTING_PROXIMITY_EVALUATION;

/*
 * WM_TOUCHHITTESTING structure
*/

typedef struct tagTOUCH_HIT_TESTING_INPUT
{
    UINT32 pointerId;
    POINT point;
    RECT boundingBox;
    RECT nonOccludedBoundingBox;
    UINT32 orientation;
} TOUCH_HIT_TESTING_INPUT, *PTOUCH_HIT_TESTING_INPUT;


#define TOUCH_HIT_TESTING_PROXIMITY_CLOSEST  0x0
#define TOUCH_HIT_TESTING_PROXIMITY_FARTHEST  0xFFF

_Win32_metadata_set_last_error_
WINUSERAPI
BOOL
WINAPI
EvaluateProximityToRect(
    _In_ const RECT *controlBoundingBox,
    _In_ const TOUCH_HIT_TESTING_INPUT *pHitTestingInput,
    _Out_ TOUCH_HIT_TESTING_PROXIMITY_EVALUATION *pProximityEval);

_Win32_metadata_set_last_error_
WINUSERAPI
BOOL
WINAPI
EvaluateProximityToPolygon(
    UINT32 numVertices,
    _In_reads_(numVertices) const POINT *controlPolygon,
    _In_ const TOUCH_HIT_TESTING_INPUT *pHitTestingInput,
    _Out_ TOUCH_HIT_TESTING_PROXIMITY_EVALUATION *pProximityEval);

_Win32_metadata_set_last_error_
WINUSERAPI
LRESULT
WINAPI
PackTouchHitTestingProximityEvaluation(
    _In_ const TOUCH_HIT_TESTING_INPUT *pHitTestingInput,
    _In_ const TOUCH_HIT_TESTING_PROXIMITY_EVALUATION *pProximityEval);


typedef enum tagFEEDBACK_TYPE {
    FEEDBACK_TOUCH_CONTACTVISUALIZATION = 1,
    FEEDBACK_PEN_BARRELVISUALIZATION    = 2,
    FEEDBACK_PEN_TAP                    = 3,
    FEEDBACK_PEN_DOUBLETAP              = 4,
    FEEDBACK_PEN_PRESSANDHOLD           = 5,
    FEEDBACK_PEN_RIGHTTAP               = 6,
    FEEDBACK_TOUCH_TAP                  = 7,
    FEEDBACK_TOUCH_DOUBLETAP            = 8,
    FEEDBACK_TOUCH_PRESSANDHOLD         = 9,
    FEEDBACK_TOUCH_RIGHTTAP             = 10,
    FEEDBACK_GESTURE_PRESSANDTAP        = 11,
    FEEDBACK_MAX                        = 0xFFFFFFFF
} FEEDBACK_TYPE;


#define GWFS_INCLUDE_ANCESTORS           0x00000001


WINUSERAPI
BOOL
WINAPI
GetWindowFeedbackSetting(
    _In_ HWND hwnd,
    _In_ FEEDBACK_TYPE feedback,
    _In_ DWORD dwFlags,
    _Inout_ UINT32* pSize,
    _Out_writes_bytes_opt_(*pSize) VOID* config);

WINUSERAPI
BOOL
WINAPI
SetWindowFeedbackSetting(
    _In_ HWND hwnd,
    _In_ FEEDBACK_TYPE feedback,
    _In_ DWORD dwFlags,
    _In_ UINT32 size,
    _In_reads_bytes_opt_(size) CONST VOID* configuration);


#endif /* WINAPI_FAMILY_PARTITION(WINAPI_PARTITION_DESKTOP) */
#pragma endregion

#endif /* WINVER >= 0x0602 */

#if(WINVER >= 0x0603)

#pragma region Desktop Family
#if WINAPI_FAMILY_PARTITION(WINAPI_PARTITION_DESKTOP)

//Disable warning C4201:nameless struct/union
#if _MSC_VER >= 1200
#pragma warning(push)
#endif
#pragma warning(disable : 4201)

typedef struct tagINPUT_TRANSFORM {
    union {
        struct {
            float        _11, _12, _13, _14;
            float        _21, _22, _23, _24;
            float        _31, _32, _33, _34;
            float        _41, _42, _43, _44;
        } DUMMYSTRUCTNAME;
        float m[4][4];
    } DUMMYUNIONNAME;
} INPUT_TRANSFORM;

#if _MSC_VER >= 1200
#pragma warning(pop)
#endif


_Win32_metadata_set_last_error_
WINUSERAPI
BOOL
WINAPI
GetPointerInputTransform(
    _In_ UINT32 pointerId,
    _In_ UINT32 historyCount,
    _Out_writes_(historyCount) INPUT_TRANSFORM *inputTransform);

#endif /* WINAPI_FAMILY_PARTITION(WINAPI_PARTITION_DESKTOP) */
#pragma endregion

#endif /* WINVER >= 0x0603 */

#pragma region Desktop Family
#if WINAPI_FAMILY_PARTITION(WINAPI_PARTITION_DESKTOP)

// TODO(47499024): Make public when Feature_TouchpadPublicApis3 is enabled

#endif /* WINAPI_FAMILY_PARTITION(WINAPI_PARTITION_DESKTOP) */
#pragma endregion

#pragma region Desktop Family
#if WINAPI_FAMILY_PARTITION(WINAPI_PARTITION_DESKTOP | WINAPI_PARTITION_PC_APP)

#if(_WIN32_WINNT >= 0x0500)
typedef struct tagLASTINPUTINFO {
    UINT cbSize;
    DWORD dwTime;
} LASTINPUTINFO, * PLASTINPUTINFO;

WINUSERAPI
BOOL
WINAPI
GetLastInputInfo(
    _Out_ PLASTINPUTINFO plii);
#endif /* _WIN32_WINNT >= 0x0500 */

#endif /* WINAPI_FAMILY_PARTITION(WINAPI_PARTITION_DESKTOP | WINAPI_PARTITION_PC_APP) */
#pragma endregion

#pragma region Desktop or PC Family
#if WINAPI_FAMILY_PARTITION(WINAPI_PARTITION_DESKTOP | WINAPI_PARTITION_PC_APP)

WINUSERAPI
UINT
WINAPI
MapVirtualKeyA(
    _In_ UINT uCode,
    _In_ MAP_VIRTUAL_KEY_TYPE uMapType);
WINUSERAPI
UINT
WINAPI
MapVirtualKeyW(
    _In_ UINT uCode,
    _In_ MAP_VIRTUAL_KEY_TYPE uMapType);
#ifdef UNICODE
#define MapVirtualKey  MapVirtualKeyW
#else
#define MapVirtualKey  MapVirtualKeyA
#endif // !UNICODE

#if(WINVER >= 0x0400)
WINUSERAPI
UINT
WINAPI
MapVirtualKeyExA(
    _In_ UINT uCode,
    _In_ MAP_VIRTUAL_KEY_TYPE uMapType,
    _In_opt_ HKL dwhkl);
WINUSERAPI
UINT
WINAPI
MapVirtualKeyExW(
    _In_ UINT uCode,
    _In_ MAP_VIRTUAL_KEY_TYPE uMapType,
    _In_opt_ HKL dwhkl);
#ifdef UNICODE
#define MapVirtualKeyEx  MapVirtualKeyExW
#else
#define MapVirtualKeyEx  MapVirtualKeyExA
#endif // !UNICODE

#endif /* WINAPI_FAMILY_PARTITION(WINAPI_PARTITION_DESKTOP | WINAPI_PARTITION_PC_APP) */
#pragma endregion

#pragma region Desktop Family
#if WINAPI_FAMILY_PARTITION(WINAPI_PARTITION_DESKTOP)

#define MAPVK_VK_TO_VSC     (0)
#define MAPVK_VSC_TO_VK     (1)
#define MAPVK_VK_TO_CHAR    (2)
#define MAPVK_VSC_TO_VK_EX  (3)
#endif /* WINVER >= 0x0400 */
#if(WINVER >= 0x0600)
#define MAPVK_VK_TO_VSC_EX  (4)
#endif /* WINVER >= 0x0600 */

WINUSERAPI
BOOL
WINAPI
GetInputState(
    VOID);

WINUSERAPI
DWORD
WINAPI
GetQueueStatus(
    _In_ QUEUE_STATUS_FLAGS flags);


WINUSERAPI
HWND
WINAPI
GetCapture(
    VOID);

WINUSERAPI
HWND
WINAPI
SetCapture(
    _In_ HWND hWnd);

_Win32_metadata_set_last_error_
WINUSERAPI
BOOL
WINAPI
ReleaseCapture(
    VOID);

_Win32_metadata_set_last_error_
WINUSERAPI
DWORD
WINAPI
MsgWaitForMultipleObjects(
    _In_ DWORD nCount,
    _In_reads_opt_(nCount) CONST HANDLE *pHandles,
    _In_ BOOL fWaitAll,
    _In_ DWORD dwMilliseconds,
    _In_ QUEUE_STATUS_FLAGS dwWakeMask);

_Win32_metadata_set_last_error_
WINUSERAPI
DWORD
WINAPI
MsgWaitForMultipleObjectsEx(
    _In_ DWORD nCount,
    _In_reads_opt_(nCount) CONST HANDLE *pHandles,
    _In_ DWORD dwMilliseconds,
    _In_ QUEUE_STATUS_FLAGS dwWakeMask,
    _In_ MSG_WAIT_FOR_MULTIPLE_OBJECTS_EX_FLAGS dwFlags);


#endif /* WINAPI_FAMILY_PARTITION(WINAPI_PARTITION_DESKTOP) */
#pragma endregion

#define MWMO_WAITALL        0x0001
#define MWMO_ALERTABLE      0x0002
#define MWMO_INPUTAVAILABLE 0x0004

/*
 * Queue status flags for GetQueueStatus() and MsgWaitForMultipleObjects()
 */
#define QS_KEY              0x0001
#define QS_MOUSEMOVE        0x0002
#define QS_MOUSEBUTTON      0x0004
#define QS_POSTMESSAGE      0x0008
#define QS_TIMER            0x0010
#define QS_PAINT            0x0020
#define QS_SENDMESSAGE      0x0040
#define QS_HOTKEY           0x0080
#define QS_ALLPOSTMESSAGE   0x0100

#if(_WIN32_WINNT >= 0x0501)
#define QS_RAWINPUT         0x0400
#endif /* _WIN32_WINNT >= 0x0501 */

#if(_WIN32_WINNT >= 0x0602)
#define QS_TOUCH            0x0800
#define QS_POINTER          0x1000

#endif /* _WIN32_WINNT >= 0x0602 */


#define QS_MOUSE           (QS_MOUSEMOVE     | \
                            QS_MOUSEBUTTON)

#if (_WIN32_WINNT >= 0x602)
#define QS_INPUT           (QS_MOUSE         | \
                            QS_KEY           | \
                            QS_RAWINPUT      | \
                            QS_TOUCH         | \
                            QS_POINTER)

#else
#if (_WIN32_WINNT >= 0x0501)
#define QS_INPUT           (QS_MOUSE         | \
                            QS_KEY           | \
                            QS_RAWINPUT)
#else
#define QS_INPUT           (QS_MOUSE         | \
                            QS_KEY)
#endif // (_WIN32_WINNT >= 0x0501)
#endif

#define QS_ALLEVENTS       (QS_INPUT         | \
                            QS_POSTMESSAGE   | \
                            QS_TIMER         | \
                            QS_PAINT         | \
                            QS_HOTKEY)

#define QS_ALLINPUT        (QS_INPUT         | \
                            QS_POSTMESSAGE   | \
                            QS_TIMER         | \
                            QS_PAINT         | \
                            QS_HOTKEY        | \
                            QS_SENDMESSAGE)


#pragma region Desktop Family or Games Family
#if WINAPI_FAMILY_PARTITION(WINAPI_PARTITION_DESKTOP | WINAPI_PARTITION_GAMES)

#define USER_TIMER_MAXIMUM  0x7FFFFFFF
#define USER_TIMER_MINIMUM  0x0000000A

/*
 * Windows Functions
 */

_Win32_metadata_set_last_error_
WINUSERAPI
UINT_PTR
WINAPI
SetTimer(
    _In_opt_ HWND hWnd,
    _In_ UINT_PTR nIDEvent,
    _In_ UINT uElapse,
    _In_opt_ TIMERPROC lpTimerFunc);

#if(WINVER >= 0x0601)

#define TIMERV_DEFAULT_COALESCING   (0)
#define TIMERV_NO_COALESCING        (0xFFFFFFFF)

#define TIMERV_COALESCING_MIN       (1)
#define TIMERV_COALESCING_MAX       (0x7FFFFFF5)

#endif /* WINVER >= 0x0601 */

#endif /* WINAPI_FAMILY_PARTITION(WINAPI_PARTITION_DESKTOP | WINAPI_PARTITION_GAMES) */
#pragma endregion

#pragma region Desktop Family
#if WINAPI_FAMILY_PARTITION(WINAPI_PARTITION_DESKTOP)

#if(WINVER >= 0x0601)

_Win32_metadata_set_last_error_
WINUSERAPI
UINT_PTR
WINAPI
SetCoalescableTimer(
    _In_opt_ HWND hWnd,
    _In_ UINT_PTR nIDEvent,
    _In_ UINT uElapse,
    _In_opt_ TIMERPROC lpTimerFunc,
    _In_ ULONG uToleranceDelay);

#endif /* WINVER >= 0x0601 */

#endif /* WINAPI_FAMILY_PARTITION(WINAPI_PARTITION_DESKTOP) */
#pragma endregion

#pragma region Desktop Family or Games Family
#if WINAPI_FAMILY_PARTITION(WINAPI_PARTITION_DESKTOP | WINAPI_PARTITION_GAMES)

_Win32_metadata_set_last_error_
WINUSERAPI
BOOL
WINAPI
KillTimer(
    _In_opt_ HWND hWnd,
    _In_ UINT_PTR uIDEvent);

#endif /* WINAPI_FAMILY_PARTITION(WINAPI_PARTITION_DESKTOP | WINAPI_PARTITION_GAMES) */
#pragma endregion

#pragma region Desktop Family
#if WINAPI_FAMILY_PARTITION(WINAPI_PARTITION_DESKTOP)

WINUSERAPI
BOOL
WINAPI
IsWindowUnicode(
    _In_ HWND hWnd);

WINUSERAPI
BOOL
WINAPI
EnableWindow(
    _In_ HWND hWnd,
    _In_ BOOL bEnable);

WINUSERAPI
BOOL
WINAPI
IsWindowEnabled(
    _In_ HWND hWnd);

_Win32_metadata_set_last_error_
WINUSERAPI
HACCEL
WINAPI
LoadAcceleratorsA(
    _In_opt_ HINSTANCE hInstance,
    _In_ LPCSTR lpTableName);
_Win32_metadata_set_last_error_
WINUSERAPI
HACCEL
WINAPI
LoadAcceleratorsW(
    _In_opt_ HINSTANCE hInstance,
    _In_ LPCWSTR lpTableName);
#ifdef UNICODE
#define LoadAccelerators  LoadAcceleratorsW
#else
#define LoadAccelerators  LoadAcceleratorsA
#endif // !UNICODE

_Win32_metadata_set_last_error_
WINUSERAPI
HACCEL
WINAPI
CreateAcceleratorTableA(
    _In_reads_(cAccel) LPACCEL paccel,
    _In_ int cAccel);
_Win32_metadata_set_last_error_
WINUSERAPI
HACCEL
WINAPI
CreateAcceleratorTableW(
    _In_reads_(cAccel) LPACCEL paccel,
    _In_ int cAccel);
#ifdef UNICODE
#define CreateAcceleratorTable  CreateAcceleratorTableW
#else
#define CreateAcceleratorTable  CreateAcceleratorTableA
#endif // !UNICODE

WINUSERAPI
BOOL
WINAPI
DestroyAcceleratorTable(
    _In_ HACCEL hAccel);

WINUSERAPI
int
WINAPI
CopyAcceleratorTableA(
    _In_ HACCEL hAccelSrc,
    _Out_writes_to_opt_(cAccelEntries, return) LPACCEL lpAccelDst,
    _In_ int cAccelEntries);
WINUSERAPI
int
WINAPI
CopyAcceleratorTableW(
    _In_ HACCEL hAccelSrc,
    _Out_writes_to_opt_(cAccelEntries, return) LPACCEL lpAccelDst,
    _In_ int cAccelEntries);
#ifdef UNICODE
#define CopyAcceleratorTable  CopyAcceleratorTableW
#else
#define CopyAcceleratorTable  CopyAcceleratorTableA
#endif // !UNICODE

#ifndef NOMSG

_Win32_metadata_set_last_error_
WINUSERAPI
int
WINAPI
TranslateAcceleratorA(
    _In_ HWND hWnd,
    _In_ HACCEL hAccTable,
    _In_ LPMSG lpMsg);
_Win32_metadata_set_last_error_
WINUSERAPI
int
WINAPI
TranslateAcceleratorW(
    _In_ HWND hWnd,
    _In_ HACCEL hAccTable,
    _In_ LPMSG lpMsg);
#ifdef UNICODE
#define TranslateAccelerator  TranslateAcceleratorW
#else
#define TranslateAccelerator  TranslateAcceleratorA
#endif // !UNICODE

#endif /* !NOMSG */

#endif /* WINAPI_FAMILY_PARTITION(WINAPI_PARTITION_DESKTOP) */
#pragma endregion

#ifndef NOSYSMETRICS

/*
 * GetSystemMetrics() codes
 */

#define SM_CXSCREEN             0
#define SM_CYSCREEN             1
#define SM_CXVSCROLL            2
#define SM_CYHSCROLL            3
#define SM_CYCAPTION            4
#define SM_CXBORDER             5
#define SM_CYBORDER             6
#define SM_CXDLGFRAME           7
#define SM_CYDLGFRAME           8
#define SM_CYVTHUMB             9
#define SM_CXHTHUMB             10
#define SM_CXICON               11
#define SM_CYICON               12
#define SM_CXCURSOR             13
#define SM_CYCURSOR             14
#define SM_CYMENU               15
#define SM_CXFULLSCREEN         16
#define SM_CYFULLSCREEN         17
#define SM_CYKANJIWINDOW        18
#define SM_MOUSEPRESENT         19
#define SM_CYVSCROLL            20
#define SM_CXHSCROLL            21
#define SM_DEBUG                22
#define SM_SWAPBUTTON           23
#define SM_RESERVED1            24
#define SM_RESERVED2            25
#define SM_RESERVED3            26
#define SM_RESERVED4            27
#define SM_CXMIN                28
#define SM_CYMIN                29
#define SM_CXSIZE               30
#define SM_CYSIZE               31
#define SM_CXFRAME              32
#define SM_CYFRAME              33
#define SM_CXMINTRACK           34
#define SM_CYMINTRACK           35
#define SM_CXDOUBLECLK          36
#define SM_CYDOUBLECLK          37
#define SM_CXICONSPACING        38
#define SM_CYICONSPACING        39
#define SM_MENUDROPALIGNMENT    40
#define SM_PENWINDOWS           41
#define SM_DBCSENABLED          42
#define SM_CMOUSEBUTTONS        43

#if(WINVER >= 0x0400)
#define SM_CXFIXEDFRAME           SM_CXDLGFRAME  /* ;win40 name change */
#define SM_CYFIXEDFRAME           SM_CYDLGFRAME  /* ;win40 name change */
#define SM_CXSIZEFRAME            SM_CXFRAME     /* ;win40 name change */
#define SM_CYSIZEFRAME            SM_CYFRAME     /* ;win40 name change */

#define SM_SECURE               44
#define SM_CXEDGE               45
#define SM_CYEDGE               46
#define SM_CXMINSPACING         47
#define SM_CYMINSPACING         48
#define SM_CXSMICON             49
#define SM_CYSMICON             50
#define SM_CYSMCAPTION          51
#define SM_CXSMSIZE             52
#define SM_CYSMSIZE             53
#define SM_CXMENUSIZE           54
#define SM_CYMENUSIZE           55
#define SM_ARRANGE              56
#define SM_CXMINIMIZED          57
#define SM_CYMINIMIZED          58
#define SM_CXMAXTRACK           59
#define SM_CYMAXTRACK           60
#define SM_CXMAXIMIZED          61
#define SM_CYMAXIMIZED          62
#define SM_NETWORK              63
#define SM_CLEANBOOT            67
#define SM_CXDRAG               68
#define SM_CYDRAG               69
#endif /* WINVER >= 0x0400 */
#define SM_SHOWSOUNDS           70
#if(WINVER >= 0x0400)
#define SM_CXMENUCHECK          71   /* Use instead of GetMenuCheckMarkDimensions()! */
#define SM_CYMENUCHECK          72
#define SM_SLOWMACHINE          73
#define SM_MIDEASTENABLED       74
#endif /* WINVER >= 0x0400 */

#if (WINVER >= 0x0500) || (_WIN32_WINNT >= 0x0400)
#define SM_MOUSEWHEELPRESENT    75
#endif
#if(WINVER >= 0x0500)
#define SM_XVIRTUALSCREEN       76
#define SM_YVIRTUALSCREEN       77
#define SM_CXVIRTUALSCREEN      78
#define SM_CYVIRTUALSCREEN      79
#define SM_CMONITORS            80
#define SM_SAMEDISPLAYFORMAT    81
#endif /* WINVER >= 0x0500 */
#if(_WIN32_WINNT >= 0x0500)
#define SM_IMMENABLED           82
#endif /* _WIN32_WINNT >= 0x0500 */
#if(_WIN32_WINNT >= 0x0501)
#define SM_CXFOCUSBORDER        83
#define SM_CYFOCUSBORDER        84
#endif /* _WIN32_WINNT >= 0x0501 */

#if(_WIN32_WINNT >= 0x0501)
#define SM_TABLETPC             86
#define SM_MEDIACENTER          87
#define SM_STARTER              88
#define SM_SERVERR2             89
#endif /* _WIN32_WINNT >= 0x0501 */

#if(_WIN32_WINNT >= 0x0600)
#define SM_MOUSEHORIZONTALWHEELPRESENT    91
#define SM_CXPADDEDBORDER       92
#endif /* _WIN32_WINNT >= 0x0600 */

#if(WINVER >= 0x0601)

#define SM_DIGITIZER            94
#define SM_MAXIMUMTOUCHES       95
#endif /* WINVER >= 0x0601 */

#if (WINVER < 0x0500) && (!defined(_WIN32_WINNT) || (_WIN32_WINNT < 0x0400))
#define SM_CMETRICS             76
#elif WINVER == 0x500
#define SM_CMETRICS             83
#elif WINVER == 0x501
#define SM_CMETRICS             91
#elif WINVER == 0x600
#define SM_CMETRICS             93
#else
#define SM_CMETRICS             97
#endif

#if(WINVER >= 0x0500)
#define SM_REMOTESESSION        0x1000


#if(_WIN32_WINNT >= 0x0501)
#define SM_SHUTTINGDOWN           0x2000
#endif /* _WIN32_WINNT >= 0x0501 */

#if(WINVER >= 0x0501)
#define SM_REMOTECONTROL          0x2001
#endif /* WINVER >= 0x0501 */

#if(WINVER >= 0x0501)
#define SM_CARETBLINKINGENABLED   0x2002
#endif /* WINVER >= 0x0501 */

#if(WINVER >= 0x0602)
#define SM_CONVERTIBLESLATEMODE   0x2003
#define SM_SYSTEMDOCKED           0x2004
#endif /* WINVER >= 0x0602 */

#endif /* WINVER >= 0x0500 */


#pragma region Desktop Family
#if WINAPI_FAMILY_PARTITION(WINAPI_PARTITION_DESKTOP)

WINUSERAPI
int
WINAPI
GetSystemMetrics(
    _In_ SYSTEM_METRICS_INDEX nIndex);


#if(WINVER >= 0x0605)
_Win32_metadata_set_last_error_
WINUSERAPI
int
WINAPI
GetSystemMetricsForDpi(
    _In_ SYSTEM_METRICS_INDEX nIndex,
    _In_ UINT dpi);

#endif /* WINVER >= 0x0605 */

#endif /* WINAPI_FAMILY_PARTITION(WINAPI_PARTITION_DESKTOP) */
#pragma endregion

#endif /* !NOSYSMETRICS */

#pragma region Desktop Family
#if WINAPI_FAMILY_PARTITION(WINAPI_PARTITION_DESKTOP)

#ifndef NOMENUS
/*
 * Menu-specific access flags
 */
#define MENU_GET_ITEM_INFO      (0x0001) // GetMenuInfo, GetMenuItemInfo
#define MENU_GET_ITEM_DATA      (0x0002) // Get dwMenuData
#define MENU_GET_SUBMENU        (0x0004) // Get Sub Menu
#define MENU_INSERT_MENU        (0x0008) // InsertMenu
#define MENU_INSERT_ITEM        (0x0010) // InsertMenuItem
#define MENU_DELETE_MENU        (0x0020) // DeleteMenu, RemoveMenu
#define MENU_SET_ITEM_INFO      (0x0040) // SetMenuItemInfo, ModifyMenu
#define MENU_ENABLE_ITEM        (0x0080) // EnableMenuItem
#define MENU_CHECK_ITEM         (0x0100) // CheckMenuItem
#define MENU_SET_DEFAULT_ITEM   (0x0200) // SetMenuDefaultItem
#define MENU_SET_ITEM_DATA      (0x0400) // Set dwMenuData
#define MENU_SET_SUBMENU        (0x0800) // Set Sub Menu

#define MENU_READ_ACCESS       (STANDARD_RIGHTS_READ   |\
                                MENU_GET_ITEM_INFO     |\
                                MENU_GET_ITEM_DATA     |\
                                MENU_GET_SUBMENU)

#define MENU_WRITE_ACCESS      (STANDARD_RIGHTS_WRITE  |\
                                MENU_INSERT_MENU            |\
                                MENU_INSERT_ITEM       |\
                                MENU_DELETE_MENU            |\
                                MENU_SET_ITEM_INFO     |\
                                MENU_ENABLE_ITEM |\
                                MENU_CHECK_ITEM |\
                                MENU_SET_DEFAULT_ITEM  |\
                                MENU_SET_ITEM_DATA     |\
                                MENU_SET_SUBMENU)

#define MENU_EXECUTE_ACCESS   (STANDARD_RIGHTS_EXECUTE)

#define MENU_ALL_ACCESS (STANDARD_RIGHTS_ALL | MENU_READ_ACCESS | MENU_WRITE_ACCESS | MENU_EXECUTE_ACCESS)

_Win32_metadata_set_last_error_
WINUSERAPI
HMENU
WINAPI
LoadMenuA(
    _In_opt_ HINSTANCE hInstance,
    _In_ LPCSTR lpMenuName);
_Win32_metadata_set_last_error_
WINUSERAPI
HMENU
WINAPI
LoadMenuW(
    _In_opt_ HINSTANCE hInstance,
    _In_ LPCWSTR lpMenuName);
#ifdef UNICODE
#define LoadMenu  LoadMenuW
#else
#define LoadMenu  LoadMenuA
#endif // !UNICODE

_Win32_metadata_set_last_error_
WINUSERAPI
HMENU
WINAPI
LoadMenuIndirectA(
    _In_ CONST MENUTEMPLATEA *lpMenuTemplate);
_Win32_metadata_set_last_error_
WINUSERAPI
HMENU
WINAPI
LoadMenuIndirectW(
    _In_ CONST MENUTEMPLATEW *lpMenuTemplate);
#ifdef UNICODE
#define LoadMenuIndirect  LoadMenuIndirectW
#else
#define LoadMenuIndirect  LoadMenuIndirectA
#endif // !UNICODE

WINUSERAPI
HMENU
WINAPI
GetMenu(
    _In_ HWND hWnd);

_Win32_metadata_set_last_error_
WINUSERAPI
BOOL
WINAPI
SetMenu(
    _In_ HWND hWnd,
    _In_opt_ HMENU hMenu);

WINUSERAPI
BOOL
WINAPI
ChangeMenuA(
    _In_ HMENU hMenu,
    _In_ UINT cmd,
    _In_opt_ LPCSTR lpszNewItem,
    _In_ UINT cmdInsert,
    _In_ UINT flags);
WINUSERAPI
BOOL
WINAPI
ChangeMenuW(
    _In_ HMENU hMenu,
    _In_ UINT cmd,
    _In_opt_ LPCWSTR lpszNewItem,
    _In_ UINT cmdInsert,
    _In_ UINT flags);
#ifdef UNICODE
#define ChangeMenu  ChangeMenuW
#else
#define ChangeMenu  ChangeMenuA
#endif // !UNICODE

WINUSERAPI
BOOL
WINAPI
HiliteMenuItem(
    _In_ HWND hWnd,
    _In_ HMENU hMenu,
    _In_ UINT uIDHiliteItem,
    _In_ UINT uHilite);

WINUSERAPI
int
WINAPI
GetMenuStringA(
    _In_ HMENU hMenu,
    _In_ UINT uIDItem,
    _Out_writes_opt_(cchMax) LPSTR lpString,
    _In_ int cchMax,
    _In_ UINT flags);
WINUSERAPI
int
WINAPI
GetMenuStringW(
    _In_ HMENU hMenu,
    _In_ UINT uIDItem,
    _Out_writes_opt_(cchMax) LPWSTR lpString,
    _In_ int cchMax,
    _In_ UINT flags);
#ifdef UNICODE
#define GetMenuString  GetMenuStringW
#else
#define GetMenuString  GetMenuStringA
#endif // !UNICODE

WINUSERAPI
UINT
WINAPI
GetMenuState(
    _In_ HMENU hMenu,
    _In_ UINT uId,
    _In_ UINT uFlags);

_Win32_metadata_set_last_error_
WINUSERAPI
BOOL
WINAPI
DrawMenuBar(
    _In_ HWND hWnd);

#if(_WIN32_WINNT >= 0x0501)
#define PMB_ACTIVE      0x00000001

#endif /* _WIN32_WINNT >= 0x0501 */


WINUSERAPI
HMENU
WINAPI
GetSystemMenu(
    _In_ HWND hWnd,
    _In_ BOOL bRevert);


_Win32_metadata_set_last_error_
WINUSERAPI
HMENU
WINAPI
CreateMenu(
    VOID);

_Win32_metadata_set_last_error_
WINUSERAPI
HMENU
WINAPI
CreatePopupMenu(
    VOID);

_Win32_metadata_set_last_error_
WINUSERAPI
BOOL
WINAPI
DestroyMenu(
    _In_ HMENU hMenu);

WINUSERAPI
DWORD
WINAPI
CheckMenuItem(
    _In_ HMENU hMenu,
    _In_ UINT uIDCheckItem,
    _In_ UINT uCheck);

WINUSERAPI
BOOL
WINAPI
EnableMenuItem(
    _In_ HMENU hMenu,
    _In_ UINT uIDEnableItem,
    _In_ UINT uEnable);

WINUSERAPI
HMENU
WINAPI
GetSubMenu(
    _In_ HMENU hMenu,
    _In_ int nPos);

WINUSERAPI
UINT
WINAPI
GetMenuItemID(
    _In_ HMENU hMenu,
    _In_ int nPos);

_Win32_metadata_set_last_error_
WINUSERAPI
int
WINAPI
GetMenuItemCount(
    _In_opt_ HMENU hMenu);

_Win32_metadata_set_last_error_
WINUSERAPI
BOOL
WINAPI
InsertMenuA(
    _In_ HMENU hMenu,
    _In_ UINT uPosition,
    _In_ UINT uFlags,
    _In_ UINT_PTR uIDNewItem,
    _In_opt_ LPCSTR lpNewItem);
_Win32_metadata_set_last_error_
WINUSERAPI
BOOL
WINAPI
InsertMenuW(
    _In_ HMENU hMenu,
    _In_ UINT uPosition,
    _In_ UINT uFlags,
    _In_ UINT_PTR uIDNewItem,
    _In_opt_ LPCWSTR lpNewItem);
#ifdef UNICODE
#define InsertMenu  InsertMenuW
#else
#define InsertMenu  InsertMenuA
#endif // !UNICODE

_Win32_metadata_set_last_error_
WINUSERAPI
BOOL
WINAPI
AppendMenuA(
    _In_ HMENU hMenu,
    _In_ UINT uFlags,
    _In_ UINT_PTR uIDNewItem,
    _In_opt_ LPCSTR lpNewItem);
_Win32_metadata_set_last_error_
WINUSERAPI
BOOL
WINAPI
AppendMenuW(
    _In_ HMENU hMenu,
    _In_ UINT uFlags,
    _In_ UINT_PTR uIDNewItem,
    _In_opt_ LPCWSTR lpNewItem);
#ifdef UNICODE
#define AppendMenu  AppendMenuW
#else
#define AppendMenu  AppendMenuA
#endif // !UNICODE

_Win32_metadata_set_last_error_
WINUSERAPI
BOOL
WINAPI
ModifyMenuA(
    _In_ HMENU hMnu,
    _In_ UINT uPosition,
    _In_ UINT uFlags,
    _In_ UINT_PTR uIDNewItem,
    _In_opt_ LPCSTR lpNewItem);
_Win32_metadata_set_last_error_
WINUSERAPI
BOOL
WINAPI
ModifyMenuW(
    _In_ HMENU hMnu,
    _In_ UINT uPosition,
    _In_ UINT uFlags,
    _In_ UINT_PTR uIDNewItem,
    _In_opt_ LPCWSTR lpNewItem);
#ifdef UNICODE
#define ModifyMenu  ModifyMenuW
#else
#define ModifyMenu  ModifyMenuA
#endif // !UNICODE

_Win32_metadata_set_last_error_
WINUSERAPI
BOOL
WINAPI RemoveMenu(
    _In_ HMENU hMenu,
    _In_ UINT uPosition,
    _In_ UINT uFlags);

_Win32_metadata_set_last_error_
WINUSERAPI
BOOL
WINAPI
DeleteMenu(
    _In_ HMENU hMenu,
    _In_ UINT uPosition,
    _In_ UINT uFlags);

_Win32_metadata_set_last_error_
WINUSERAPI
BOOL
WINAPI
SetMenuItemBitmaps(
    _In_ HMENU hMenu,
    _In_ UINT uPosition,
    _In_ UINT uFlags,
    _In_opt_ HBITMAP hBitmapUnchecked,
    _In_opt_ HBITMAP hBitmapChecked);

WINUSERAPI
LONG
WINAPI
GetMenuCheckMarkDimensions(
    VOID);

_Win32_metadata_set_last_error_
WINUSERAPI
BOOL
WINAPI
TrackPopupMenu(
    _In_ HMENU hMenu,
    _In_ TRACK_POPUP_MENU_FLAGS uFlags,
    _In_ int x,
    _In_ int y,
    _Reserved_ int nReserved,
    _In_ HWND hWnd,
    _Reserved_ CONST RECT *prcRect);

#if(WINVER >= 0x0400)
/* return codes for WM_MENUCHAR */
#define MNC_IGNORE  0
#define MNC_CLOSE   1
#define MNC_EXECUTE 2
#define MNC_SELECT  3

typedef struct tagTPMPARAMS
{
    UINT    cbSize;     /* Size of structure */
    RECT    rcExclude;  /* Screen coordinates of rectangle to exclude when positioning */
}   TPMPARAMS;
typedef TPMPARAMS FAR *LPTPMPARAMS;

_Win32_metadata_set_last_error_
WINUSERAPI
BOOL
WINAPI
TrackPopupMenuEx(
    _In_ HMENU hMenu,
    _In_ UINT uFlags,
    _In_ int x,
    _In_ int y,
    _In_ HWND hwnd,
    _In_opt_ LPTPMPARAMS lptpm);
#endif /* WINVER >= 0x0400 */

#if(_WIN32_WINNT >= 0x0601)
_Win32_metadata_set_last_error_
WINUSERAPI
BOOL
WINAPI
CalculatePopupWindowPosition(
    _In_ const POINT *anchorPoint,
    _In_ const SIZE *windowSize,
    _In_ UINT /* TPM_XXX values */ flags,
    _In_opt_ RECT *excludeRect,
    _Out_ RECT *popupWindowPosition);

#endif /* _WIN32_WINNT >= 0x0601 */

#if(WINVER >= 0x0500)

#define MNS_NOCHECK         0x80000000
#define MNS_MODELESS        0x40000000
#define MNS_DRAGDROP        0x20000000
#define MNS_AUTODISMISS     0x10000000
#define MNS_NOTIFYBYPOS     0x08000000
#define MNS_CHECKORBMP      0x04000000

#define MIM_MAXHEIGHT               0x00000001
#define MIM_BACKGROUND              0x00000002
#define MIM_HELPID                  0x00000004
#define MIM_MENUDATA                0x00000008
#define MIM_STYLE                   0x00000010
#define MIM_APPLYTOSUBMENUS         0x80000000

typedef struct tagMENUINFO
{
    DWORD   cbSize;
    MENUINFO_MASK   fMask;
    MENUINFO_STYLE   dwStyle;
    UINT    cyMax;
    HBRUSH  hbrBack;
    DWORD   dwContextHelpID;
    ULONG_PTR dwMenuData;
}   MENUINFO, FAR *LPMENUINFO;
typedef MENUINFO CONST FAR *LPCMENUINFO;

_Win32_metadata_set_last_error_
WINUSERAPI
BOOL
WINAPI
GetMenuInfo(
    _In_ HMENU,
    _Inout_ LPMENUINFO);

_Win32_metadata_set_last_error_
WINUSERAPI
BOOL
WINAPI
SetMenuInfo(
    _In_ HMENU,
    _In_ LPCMENUINFO);

_Win32_metadata_set_last_error_
WINUSERAPI
BOOL
WINAPI
EndMenu(
        VOID);

/*
 * WM_MENUDRAG return values.
 */
#define MND_CONTINUE       0
#define MND_ENDMENU        1

typedef struct tagMENUGETOBJECTINFO
{
    MENUGETOBJECTINFO_FLAGS dwFlags;
    UINT uPos;
    HMENU hmenu;
    PVOID riid;
    PVOID pvObj;
} MENUGETOBJECTINFO, * PMENUGETOBJECTINFO;

/*
 * MENUGETOBJECTINFO dwFlags values
 */
#define MNGOF_TOPGAP         0x00000001
#define MNGOF_BOTTOMGAP      0x00000002

/*
 * WM_MENUGETOBJECT return values
 */
#define MNGO_NOINTERFACE     0x00000000
#define MNGO_NOERROR         0x00000001
#endif /* WINVER >= 0x0500 */

#if(WINVER >= 0x0400)
#define MIIM_STATE       0x00000001
#define MIIM_ID          0x00000002
#define MIIM_SUBMENU     0x00000004
#define MIIM_CHECKMARKS  0x00000008
#define MIIM_TYPE        0x00000010
#define MIIM_DATA        0x00000020
#endif /* WINVER >= 0x0400 */

#if(WINVER >= 0x0500)
#define MIIM_STRING      0x00000040
#define MIIM_BITMAP      0x00000080
#define MIIM_FTYPE       0x00000100

#define HBMMENU_CALLBACK            ((HBITMAP) -1)
#define HBMMENU_SYSTEM              ((HBITMAP)  1)
#define HBMMENU_MBAR_RESTORE        ((HBITMAP)  2)
#define HBMMENU_MBAR_MINIMIZE       ((HBITMAP)  3)
#define HBMMENU_MBAR_CLOSE          ((HBITMAP)  5)
#define HBMMENU_MBAR_CLOSE_D        ((HBITMAP)  6)
#define HBMMENU_MBAR_MINIMIZE_D     ((HBITMAP)  7)
#define HBMMENU_POPUP_CLOSE         ((HBITMAP)  8)
#define HBMMENU_POPUP_RESTORE       ((HBITMAP)  9)
#define HBMMENU_POPUP_MAXIMIZE      ((HBITMAP) 10)
#define HBMMENU_POPUP_MINIMIZE      ((HBITMAP) 11)
#endif /* WINVER >= 0x0500 */

#if(WINVER >= 0x0400)
typedef struct tagMENUITEMINFOA
{
    UINT     cbSize;
    MENU_ITEM_MASK     fMask;
    MENU_ITEM_TYPE     fType;         // used if MIIM_TYPE (4.0) or MIIM_FTYPE (>4.0)
    UINT     fState;        // used if MIIM_STATE
    UINT     wID;           // used if MIIM_ID
    HMENU    hSubMenu;      // used if MIIM_SUBMENU
    HBITMAP  hbmpChecked;   // used if MIIM_CHECKMARKS
    HBITMAP  hbmpUnchecked; // used if MIIM_CHECKMARKS
    ULONG_PTR dwItemData;   // used if MIIM_DATA
    LPSTR    dwTypeData;    // used if MIIM_TYPE (4.0) or MIIM_STRING (>4.0)
    UINT     cch;           // used if MIIM_TYPE (4.0) or MIIM_STRING (>4.0)
#if(WINVER >= 0x0500)
    HBITMAP  hbmpItem;      // used if MIIM_BITMAP
#endif /* WINVER >= 0x0500 */
}   MENUITEMINFOA, FAR *LPMENUITEMINFOA;
typedef struct tagMENUITEMINFOW
{
    UINT     cbSize;
    MENU_ITEM_MASK     fMask;
    MENU_ITEM_TYPE     fType;         // used if MIIM_TYPE (4.0) or MIIM_FTYPE (>4.0)
    UINT     fState;        // used if MIIM_STATE
    UINT     wID;           // used if MIIM_ID
    HMENU    hSubMenu;      // used if MIIM_SUBMENU
    HBITMAP  hbmpChecked;   // used if MIIM_CHECKMARKS
    HBITMAP  hbmpUnchecked; // used if MIIM_CHECKMARKS
    ULONG_PTR dwItemData;   // used if MIIM_DATA
    LPWSTR   dwTypeData;    // used if MIIM_TYPE (4.0) or MIIM_STRING (>4.0)
    UINT     cch;           // used if MIIM_TYPE (4.0) or MIIM_STRING (>4.0)
#if(WINVER >= 0x0500)
    HBITMAP  hbmpItem;      // used if MIIM_BITMAP
#endif /* WINVER >= 0x0500 */
}   MENUITEMINFOW, FAR *LPMENUITEMINFOW;
#ifdef UNICODE
typedef MENUITEMINFOW MENUITEMINFO;
typedef LPMENUITEMINFOW LPMENUITEMINFO;
#else
typedef MENUITEMINFOA MENUITEMINFO;
typedef LPMENUITEMINFOA LPMENUITEMINFO;
#endif // UNICODE
typedef MENUITEMINFOA CONST FAR *LPCMENUITEMINFOA;
typedef MENUITEMINFOW CONST FAR *LPCMENUITEMINFOW;
#ifdef UNICODE
typedef LPCMENUITEMINFOW LPCMENUITEMINFO;
#else
typedef LPCMENUITEMINFOA LPCMENUITEMINFO;
#endif // UNICODE


_Win32_metadata_set_last_error_
WINUSERAPI
BOOL
WINAPI
InsertMenuItemA(
    _In_ HMENU hmenu,
    _In_ UINT item,
    _In_ BOOL fByPosition,
    _In_ LPCMENUITEMINFOA lpmi);
_Win32_metadata_set_last_error_
WINUSERAPI
BOOL
WINAPI
InsertMenuItemW(
    _In_ HMENU hmenu,
    _In_ UINT item,
    _In_ BOOL fByPosition,
    _In_ LPCMENUITEMINFOW lpmi);
#ifdef UNICODE
#define InsertMenuItem  InsertMenuItemW
#else
#define InsertMenuItem  InsertMenuItemA
#endif // !UNICODE

_Win32_metadata_set_last_error_
WINUSERAPI
BOOL
WINAPI
GetMenuItemInfoA(
    _In_ HMENU hmenu,
    _In_ UINT item,
    _In_ BOOL fByPosition,
    _Inout_ LPMENUITEMINFOA lpmii);
_Win32_metadata_set_last_error_
WINUSERAPI
BOOL
WINAPI
GetMenuItemInfoW(
    _In_ HMENU hmenu,
    _In_ UINT item,
    _In_ BOOL fByPosition,
    _Inout_ LPMENUITEMINFOW lpmii);
#ifdef UNICODE
#define GetMenuItemInfo  GetMenuItemInfoW
#else
#define GetMenuItemInfo  GetMenuItemInfoA
#endif // !UNICODE

_Win32_metadata_set_last_error_
WINUSERAPI
BOOL
WINAPI
SetMenuItemInfoA(
    _In_ HMENU hmenu,
    _In_ UINT item,
    _In_ BOOL fByPositon,
    _In_ LPCMENUITEMINFOA lpmii);
_Win32_metadata_set_last_error_
WINUSERAPI
BOOL
WINAPI
SetMenuItemInfoW(
    _In_ HMENU hmenu,
    _In_ UINT item,
    _In_ BOOL fByPositon,
    _In_ LPCMENUITEMINFOW lpmii);
#ifdef UNICODE
#define SetMenuItemInfo  SetMenuItemInfoW
#else
#define SetMenuItemInfo  SetMenuItemInfoA
#endif // !UNICODE


#define GMDI_USEDISABLED    0x0001L
#define GMDI_GOINTOPOPUPS   0x0002L

_Win32_metadata_set_last_error_
WINUSERAPI
UINT
WINAPI
GetMenuDefaultItem(
    _In_ HMENU hMenu,
    _In_ UINT fByPos,
    _In_ GET_MENU_DEFAULT_ITEM_FLAGS gmdiFlags);

_Win32_metadata_set_last_error_
WINUSERAPI
BOOL
WINAPI
SetMenuDefaultItem(
    _In_ HMENU hMenu,
    _In_ UINT uItem,
    _In_ UINT fByPos);

_Win32_metadata_set_last_error_
WINUSERAPI
BOOL
WINAPI
GetMenuItemRect(
    _In_opt_ HWND hWnd,
    _In_ HMENU hMenu,
    _In_ UINT uItem,
    _Out_ LPRECT lprcItem);

WINUSERAPI
int
WINAPI
MenuItemFromPoint(
    _In_opt_ HWND hWnd,
    _In_ HMENU hMenu,
    _In_ POINT ptScreen);
#endif /* WINVER >= 0x0400 */

/*
 * Flags for TrackPopupMenu
 */
#define TPM_LEFTBUTTON  0x0000L
#define TPM_RIGHTBUTTON 0x0002L
#define TPM_LEFTALIGN   0x0000L
#define TPM_CENTERALIGN 0x0004L
#define TPM_RIGHTALIGN  0x0008L
#if(WINVER >= 0x0400)
#define TPM_TOPALIGN        0x0000L
#define TPM_VCENTERALIGN    0x0010L
#define TPM_BOTTOMALIGN     0x0020L

#define TPM_HORIZONTAL      0x0000L     /* Horz alignment matters more */
#define TPM_VERTICAL        0x0040L     /* Vert alignment matters more */
#define TPM_NONOTIFY        0x0080L     /* Don't send any notification msgs */
#define TPM_RETURNCMD       0x0100L
#endif /* WINVER >= 0x0400 */
#if(WINVER >= 0x0500)
#define TPM_RECURSE         0x0001L
#define TPM_HORPOSANIMATION 0x0400L
#define TPM_HORNEGANIMATION 0x0800L
#define TPM_VERPOSANIMATION 0x1000L
#define TPM_VERNEGANIMATION 0x2000L
#if(_WIN32_WINNT >= 0x0500)
#define TPM_NOANIMATION     0x4000L
#endif /* _WIN32_WINNT >= 0x0500 */
#if(_WIN32_WINNT >= 0x0501)
#define TPM_LAYOUTRTL       0x8000L
#endif /* _WIN32_WINNT >= 0x0501 */
#endif /* WINVER >= 0x0500 */
#if(_WIN32_WINNT >= 0x0601)
#define TPM_WORKAREA        0x10000L
#endif /* _WIN32_WINNT >= 0x0601 */


#endif /* !NOMENUS */

#endif /* WINAPI_FAMILY_PARTITION(WINAPI_PARTITION_DESKTOP) */
#pragma endregion


#if(WINVER >= 0x0400)

#pragma region Desktop Family
#if WINAPI_FAMILY_PARTITION(WINAPI_PARTITION_DESKTOP)

//
// Drag-and-drop support
// Obsolete - use OLE instead
//
typedef struct tagDROPSTRUCT
{
    HWND    hwndSource;
    HWND    hwndSink;
    DWORD   wFmt;
    ULONG_PTR dwData;
    POINT   ptDrop;
    DWORD   dwControlData;
} DROPSTRUCT, *PDROPSTRUCT, *LPDROPSTRUCT;

#endif /* WINAPI_FAMILY_PARTITION(WINAPI_PARTITION_DESKTOP) */
#pragma endregion

#define DOF_EXECUTABLE      0x8001      // wFmt flags
#define DOF_DOCUMENT        0x8002
#define DOF_DIRECTORY       0x8003
#define DOF_MULTIPLE        0x8004
#define DOF_PROGMAN         0x0001
#define DOF_SHELLDATA       0x0002

#define DO_DROPFILE         0x454C4946L
#define DO_PRINTFILE        0x544E5250L

#pragma region Desktop Family
#if WINAPI_FAMILY_PARTITION(WINAPI_PARTITION_DESKTOP)

WINUSERAPI
DWORD
WINAPI
DragObject(
    _In_ HWND hwndParent,
    _In_ HWND hwndFrom,
    _In_ UINT fmt,
    _In_ ULONG_PTR data,
    _In_opt_ HCURSOR hcur);

WINUSERAPI
BOOL
WINAPI
DragDetect(
    _In_ HWND hwnd,
    _In_ POINT pt);

#endif /* WINAPI_FAMILY_PARTITION(WINAPI_PARTITION_DESKTOP) */
#pragma endregion

#endif /* WINVER >= 0x0400 */

#pragma region Desktop Family
#if WINAPI_FAMILY_PARTITION(WINAPI_PARTITION_DESKTOP)

_Win32_metadata_set_last_error_
WINUSERAPI
BOOL
WINAPI
DrawIcon(
    _In_ HDC hDC,
    _In_ int X,
    _In_ int Y,
    _In_ HICON hIcon);

#endif /* WINAPI_FAMILY_PARTITION(WINAPI_PARTITION_DESKTOP) */
#pragma endregion

#ifndef NODRAWTEXT

/*
 * DrawText() Format Flags
 */
#define DT_TOP                      0x00000000
#define DT_LEFT                     0x00000000
#define DT_CENTER                   0x00000001
#define DT_RIGHT                    0x00000002
#define DT_VCENTER                  0x00000004
#define DT_BOTTOM                   0x00000008
#define DT_WORDBREAK                0x00000010
#define DT_SINGLELINE               0x00000020
#define DT_EXPANDTABS               0x00000040
#define DT_TABSTOP                  0x00000080
#define DT_NOCLIP                   0x00000100
#define DT_EXTERNALLEADING          0x00000200
#define DT_CALCRECT                 0x00000400
#define DT_NOPREFIX                 0x00000800
#define DT_INTERNAL                 0x00001000

#if(WINVER >= 0x0400)
#define DT_EDITCONTROL              0x00002000
#define DT_PATH_ELLIPSIS            0x00004000
#define DT_END_ELLIPSIS             0x00008000
#define DT_MODIFYSTRING             0x00010000
#define DT_RTLREADING               0x00020000
#define DT_WORD_ELLIPSIS            0x00040000
#if(WINVER >= 0x0500)
#define DT_NOFULLWIDTHCHARBREAK     0x00080000
#if(_WIN32_WINNT >= 0x0500)
#define DT_HIDEPREFIX               0x00100000
#define DT_PREFIXONLY               0x00200000
#endif /* _WIN32_WINNT >= 0x0500 */
#endif /* WINVER >= 0x0500 */

#pragma region Desktop Family
#if WINAPI_FAMILY_PARTITION(WINAPI_PARTITION_DESKTOP)

typedef struct tagDRAWTEXTPARAMS
{
    UINT    cbSize;
    int     iTabLength;
    int     iLeftMargin;
    int     iRightMargin;
    UINT    uiLengthDrawn;
} DRAWTEXTPARAMS, FAR *LPDRAWTEXTPARAMS;

#endif /* WINAPI_FAMILY_PARTITION(WINAPI_PARTITION_DESKTOP) */
#pragma endregion

#endif /* WINVER >= 0x0400 */


#pragma region Desktop Family
#if WINAPI_FAMILY_PARTITION(WINAPI_PARTITION_DESKTOP)

#define _In_bypassable_reads_or_z_(size) \
    _When_(((size) == -1) || (_String_length_(_Curr_) <  (size)), _In_z_) \
    _When_(((size) != -1) && (_String_length_(_Curr_) >= (size)), _In_reads_(size))

#define _Inout_grows_updates_bypassable_or_z_(size, grows) \
    _When_(((size) == -1) || (_String_length_(_Curr_) <  (size)), _Pre_z_ _Pre_valid_ _Out_writes_z_(_String_length_(_Curr_) + (grows))) \
    _When_(((size) != -1) && (_String_length_(_Curr_) >= (size)), _Pre_count_(size) _Pre_valid_ _Out_writes_z_((size) + (grows)))

WINUSERAPI
_Success_(return)
int
WINAPI
DrawTextA(
    _In_ HDC hdc,
    _When_((format & DT_MODIFYSTRING), _At_((LPSTR)lpchText, _Inout_grows_updates_bypassable_or_z_(cchText, 4)))
    _When_((!(format & DT_MODIFYSTRING)), _In_bypassable_reads_or_z_(cchText))
    LPCSTR lpchText,
    _In_ int cchText,
    _Inout_ LPRECT lprc,
    _In_ UINT format);
WINUSERAPI
_Success_(return)
int
WINAPI
DrawTextW(
    _In_ HDC hdc,
    _When_((format & DT_MODIFYSTRING), _At_((LPWSTR)lpchText, _Inout_grows_updates_bypassable_or_z_(cchText, 4)))
    _When_((!(format & DT_MODIFYSTRING)), _In_bypassable_reads_or_z_(cchText))
    LPCWSTR lpchText,
    _In_ int cchText,
    _Inout_ LPRECT lprc,
    _In_ UINT format);
#ifdef UNICODE
#define DrawText  DrawTextW
#else
#define DrawText  DrawTextA
#endif // !UNICODE

#if defined(_M_CEE)
#undef DrawText
__inline
int
DrawText(
    HDC hdc,
    LPCTSTR lpchText,
    int cchText,
    LPRECT lprc,
    UINT format
    )
{
#ifdef UNICODE
    return DrawTextW(
#else
    return DrawTextA(
#endif
        hdc,
    lpchText,
    cchText,
    lprc,
    format
        );
}
#endif  /* _M_CEE */


#if(WINVER >= 0x0400)
WINUSERAPI
_Success_(return)
int
WINAPI
DrawTextExA(
    _In_ HDC hdc,
    _When_((cchText) < -1, _Unreferenced_parameter_)
    _When_((format & DT_MODIFYSTRING), _Inout_grows_updates_bypassable_or_z_(cchText, 4))
    _When_((!(format & DT_MODIFYSTRING)), _At_((LPCSTR)lpchText, _In_bypassable_reads_or_z_(cchText)))
    LPSTR lpchText,
    _In_ int cchText,
    _Inout_ LPRECT lprc,
    _In_ UINT format,
    _In_opt_ LPDRAWTEXTPARAMS lpdtp);
WINUSERAPI
_Success_(return)
int
WINAPI
DrawTextExW(
    _In_ HDC hdc,
    _When_((cchText) < -1, _Unreferenced_parameter_)
    _When_((format & DT_MODIFYSTRING), _Inout_grows_updates_bypassable_or_z_(cchText, 4))
    _When_((!(format & DT_MODIFYSTRING)), _At_((LPCWSTR)lpchText, _In_bypassable_reads_or_z_(cchText)))
    LPWSTR lpchText,
    _In_ int cchText,
    _Inout_ LPRECT lprc,
    _In_ UINT format,
    _In_opt_ LPDRAWTEXTPARAMS lpdtp);
#ifdef UNICODE
#define DrawTextEx  DrawTextExW
#else
#define DrawTextEx  DrawTextExA
#endif // !UNICODE
#endif /* WINVER >= 0x0400 */

#endif /* WINAPI_FAMILY_PARTITION(WINAPI_PARTITION_DESKTOP) */
#pragma endregion

#endif /* !NODRAWTEXT */

#pragma region Desktop Family
#if WINAPI_FAMILY_PARTITION(WINAPI_PARTITION_DESKTOP)

WINUSERAPI
BOOL
WINAPI
GrayStringA(
    _In_ HDC hDC,
    _In_opt_ HBRUSH hBrush,
    _In_opt_ GRAYSTRINGPROC lpOutputFunc,
    _In_ LPARAM lpData,
    _In_ int nCount,
    _In_ int X,
    _In_ int Y,
    _In_ int nWidth,
    _In_ int nHeight);
WINUSERAPI
BOOL
WINAPI
GrayStringW(
    _In_ HDC hDC,
    _In_opt_ HBRUSH hBrush,
    _In_opt_ GRAYSTRINGPROC lpOutputFunc,
    _In_ LPARAM lpData,
    _In_ int nCount,
    _In_ int X,
    _In_ int Y,
    _In_ int nWidth,
    _In_ int nHeight);
#ifdef UNICODE
#define GrayString  GrayStringW
#else
#define GrayString  GrayStringA
#endif // !UNICODE

#endif /* WINAPI_FAMILY_PARTITION(WINAPI_PARTITION_DESKTOP) */
#pragma endregion

#if(WINVER >= 0x0400)
/* Monolithic state-drawing routine */
/* Image type */
#define DST_COMPLEX     0x0000
#define DST_TEXT        0x0001
#define DST_PREFIXTEXT  0x0002
#define DST_ICON        0x0003
#define DST_BITMAP      0x0004

/* State type */
#define DSS_NORMAL      0x0000
#define DSS_UNION       0x0010  /* Gray string appearance */
#define DSS_DISABLED    0x0020
#define DSS_MONO        0x0080
#if(_WIN32_WINNT >= 0x0500)
#define DSS_HIDEPREFIX  0x0200
#define DSS_PREFIXONLY  0x0400
#endif /* _WIN32_WINNT >= 0x0500 */
#define DSS_RIGHT       0x8000

#pragma region Desktop Family
#if WINAPI_FAMILY_PARTITION(WINAPI_PARTITION_DESKTOP)

WINUSERAPI
BOOL
WINAPI
DrawStateA(
    _In_ HDC hdc,
    _In_opt_ HBRUSH hbrFore,
    _In_opt_ DRAWSTATEPROC qfnCallBack,
    _In_ LPARAM lData,
    _In_ WPARAM wData,
    _In_ int x,
    _In_ int y,
    _In_ int cx,
    _In_ int cy,
    _In_ DRAWSTATE_FLAGS uFlags);
WINUSERAPI
BOOL
WINAPI
DrawStateW(
    _In_ HDC hdc,
    _In_opt_ HBRUSH hbrFore,
    _In_opt_ DRAWSTATEPROC qfnCallBack,
    _In_ LPARAM lData,
    _In_ WPARAM wData,
    _In_ int x,
    _In_ int y,
    _In_ int cx,
    _In_ int cy,
    _In_ DRAWSTATE_FLAGS uFlags);
#ifdef UNICODE
#define DrawState  DrawStateW
#else
#define DrawState  DrawStateA
#endif // !UNICODE

#endif /* WINAPI_FAMILY_PARTITION(WINAPI_PARTITION_DESKTOP) */
#pragma endregion

#endif /* WINVER >= 0x0400 */

#pragma region Desktop Family
#if WINAPI_FAMILY_PARTITION(WINAPI_PARTITION_DESKTOP)

WINUSERAPI
LONG
WINAPI
TabbedTextOutA(
    _In_ HDC hdc,
    _In_ int x,
    _In_ int y,
    _In_reads_(chCount) LPCSTR lpString,
    _In_ int chCount,
    _In_ int nTabPositions,
    _In_reads_opt_(nTabPositions) CONST INT *lpnTabStopPositions,
    _In_ int nTabOrigin);
WINUSERAPI
LONG
WINAPI
TabbedTextOutW(
    _In_ HDC hdc,
    _In_ int x,
    _In_ int y,
    _In_reads_(chCount) LPCWSTR lpString,
    _In_ int chCount,
    _In_ int nTabPositions,
    _In_reads_opt_(nTabPositions) CONST INT *lpnTabStopPositions,
    _In_ int nTabOrigin);
#ifdef UNICODE
#define TabbedTextOut  TabbedTextOutW
#else
#define TabbedTextOut  TabbedTextOutA
#endif // !UNICODE

WINUSERAPI
DWORD
WINAPI
GetTabbedTextExtentA(
    _In_ HDC hdc,
    _In_reads_(chCount) LPCSTR lpString,
    _In_ int chCount,
    _In_ int nTabPositions,
    _In_reads_opt_(nTabPositions) CONST INT *lpnTabStopPositions);
WINUSERAPI
DWORD
WINAPI
GetTabbedTextExtentW(
    _In_ HDC hdc,
    _In_reads_(chCount) LPCWSTR lpString,
    _In_ int chCount,
    _In_ int nTabPositions,
    _In_reads_opt_(nTabPositions) CONST INT *lpnTabStopPositions);
#ifdef UNICODE
#define GetTabbedTextExtent  GetTabbedTextExtentW
#else
#define GetTabbedTextExtent  GetTabbedTextExtentA
#endif // !UNICODE

WINUSERAPI
BOOL
WINAPI
UpdateWindow(
    _In_ HWND hWnd);

_Win32_metadata_set_last_error_
WINUSERAPI
HWND
WINAPI
SetActiveWindow(
    _In_ HWND hWnd);


WINUSERAPI
HWND
WINAPI
GetForegroundWindow(
    VOID);

#if(WINVER >= 0x0400)
WINUSERAPI
BOOL
WINAPI
PaintDesktop(
    _In_ HDC hdc);

WINUSERAPI
VOID
WINAPI
SwitchToThisWindow(
    _In_ HWND hwnd,
    _In_ BOOL fUnknown);
#endif /* WINVER >= 0x0400 */


WINUSERAPI
BOOL
WINAPI
SetForegroundWindow(
    _In_ HWND hWnd);

#if(_WIN32_WINNT >= 0x0500)
_Win32_metadata_set_last_error_
WINUSERAPI
BOOL
WINAPI
AllowSetForegroundWindow(
    _In_ DWORD dwProcessId);

#define ASFW_ANY    ((DWORD)-1)

_Win32_metadata_set_last_error_
WINUSERAPI
BOOL
WINAPI
LockSetForegroundWindow(
    _In_ FOREGROUND_WINDOW_LOCK_CODE uLockCode);

#define LSFW_LOCK       1
#define LSFW_UNLOCK     2

#endif /* _WIN32_WINNT >= 0x0500 */

WINUSERAPI
HWND
WINAPI
WindowFromDC(
    _In_ HDC hDC);

WINUSERAPI
HDC
WINAPI
GetDC(
    _In_opt_ HWND hWnd);

WINUSERAPI
HDC
WINAPI
GetDCEx(
    _In_opt_ HWND hWnd,
    _In_opt_ HRGN hrgnClip,
    _In_ GET_DCX_FLAGS flags);

#endif /* WINAPI_FAMILY_PARTITION(WINAPI_PARTITION_DESKTOP) */
#pragma endregion

/*
 * GetDCEx() flags
 */
#define DCX_WINDOW           0x00000001L
#define DCX_CACHE            0x00000002L
#define DCX_NORESETATTRS     0x00000004L
#define DCX_CLIPCHILDREN     0x00000008L
#define DCX_CLIPSIBLINGS     0x00000010L
#define DCX_PARENTCLIP       0x00000020L
#define DCX_EXCLUDERGN       0x00000040L
#define DCX_INTERSECTRGN     0x00000080L
#define DCX_EXCLUDEUPDATE    0x00000100L
#define DCX_INTERSECTUPDATE  0x00000200L
#define DCX_LOCKWINDOWUPDATE 0x00000400L

#define DCX_VALIDATE         0x00200000L

#pragma region Desktop Family
#if WINAPI_FAMILY_PARTITION(WINAPI_PARTITION_DESKTOP)

WINUSERAPI
HDC
WINAPI
GetWindowDC(
    _In_opt_ HWND hWnd);

WINUSERAPI
int
WINAPI
ReleaseDC(
    _In_opt_ HWND hWnd,
    _In_ HDC hDC);

WINUSERAPI
HDC
WINAPI
BeginPaint(
    _In_ HWND hWnd,
    _Out_ LPPAINTSTRUCT lpPaint);

WINUSERAPI
BOOL
WINAPI
EndPaint(
    _In_ HWND hWnd,
    _In_ CONST PAINTSTRUCT *lpPaint);

WINUSERAPI
BOOL
WINAPI
GetUpdateRect(
    _In_ HWND hWnd,
    _Out_opt_ LPRECT lpRect,
    _In_ BOOL bErase);

WINUSERAPI
int
WINAPI
GetUpdateRgn(
    _In_ HWND hWnd,
    _In_ HRGN hRgn,
    _In_ BOOL bErase);

WINUSERAPI
int
WINAPI
SetWindowRgn(
    _In_ HWND hWnd,
    _In_opt_ HRGN hRgn,
    _In_ BOOL bRedraw);

#endif /* WINAPI_FAMILY_PARTITION(WINAPI_PARTITION_DESKTOP) */
#pragma endregion


#pragma region Desktop Family
#if WINAPI_FAMILY_PARTITION(WINAPI_PARTITION_DESKTOP)

WINUSERAPI
int
WINAPI
GetWindowRgn(
    _In_ HWND hWnd,
    _In_ HRGN hRgn);

#if(_WIN32_WINNT >= 0x0501)

WINUSERAPI
int
WINAPI
GetWindowRgnBox(
    _In_ HWND hWnd,
    _Out_ LPRECT lprc);

#endif /* _WIN32_WINNT >= 0x0501 */

WINUSERAPI
int
WINAPI
ExcludeUpdateRgn(
    _In_ HDC hDC,
    _In_ HWND hWnd);

WINUSERAPI
BOOL
WINAPI
InvalidateRect(
    _In_opt_ HWND hWnd,
    _In_opt_ CONST RECT *lpRect,
    _In_ BOOL bErase);

WINUSERAPI
BOOL
WINAPI
ValidateRect(
    _In_opt_ HWND hWnd,
    _In_opt_ CONST RECT *lpRect);

WINUSERAPI
BOOL
WINAPI
InvalidateRgn(
    _In_ HWND hWnd,
    _In_opt_ HRGN hRgn,
    _In_ BOOL bErase);

WINUSERAPI
BOOL
WINAPI
ValidateRgn(
    _In_ HWND hWnd,
    _In_opt_ HRGN hRgn);


WINUSERAPI
BOOL
WINAPI
RedrawWindow(
    _In_opt_ HWND hWnd,
    _In_opt_ CONST RECT *lprcUpdate,
    _In_opt_ HRGN hrgnUpdate,
    _In_ REDRAW_WINDOW_FLAGS flags);

#endif /* WINAPI_FAMILY_PARTITION(WINAPI_PARTITION_DESKTOP) */
#pragma endregion

/*
 * RedrawWindow() flags
 */
#define RDW_INVALIDATE          0x0001
#define RDW_INTERNALPAINT       0x0002
#define RDW_ERASE               0x0004

#define RDW_VALIDATE            0x0008
#define RDW_NOINTERNALPAINT     0x0010
#define RDW_NOERASE             0x0020

#define RDW_NOCHILDREN          0x0040
#define RDW_ALLCHILDREN         0x0080

#define RDW_UPDATENOW           0x0100
#define RDW_ERASENOW            0x0200

#define RDW_FRAME               0x0400
#define RDW_NOFRAME             0x0800


#pragma region Desktop Family
#if WINAPI_FAMILY_PARTITION(WINAPI_PARTITION_DESKTOP)

/*
 * LockWindowUpdate API
 */

WINUSERAPI
BOOL
WINAPI
LockWindowUpdate(
    _In_opt_ HWND hWndLock);

_Win32_metadata_set_last_error_
WINUSERAPI
BOOL
WINAPI
ScrollWindow(
    _In_ HWND hWnd,
    _In_ int XAmount,
    _In_ int YAmount,
    _In_opt_ CONST RECT *lpRect,
    _In_opt_ CONST RECT *lpClipRect);

_Win32_metadata_set_last_error_
WINUSERAPI
BOOL
WINAPI
ScrollDC(
    _In_ HDC hDC,
    _In_ int dx,
    _In_ int dy,
    _In_opt_ CONST RECT *lprcScroll,
    _In_opt_ CONST RECT *lprcClip,
    _In_opt_ HRGN hrgnUpdate,
    _Out_opt_ LPRECT lprcUpdate);

_Win32_metadata_set_last_error_
WINUSERAPI
int
WINAPI
ScrollWindowEx(
    _In_ HWND hWnd,
    _In_ int dx,
    _In_ int dy,
    _In_opt_ CONST RECT *prcScroll,
    _In_opt_ CONST RECT *prcClip,
    _In_opt_ HRGN hrgnUpdate,
    _Out_opt_ LPRECT prcUpdate,
    _In_ UINT flags);

#endif /* WINAPI_FAMILY_PARTITION(WINAPI_PARTITION_DESKTOP) */
#pragma endregion

#define SW_SCROLLCHILDREN   0x0001  /* Scroll children within *lprcScroll. */
#define SW_INVALIDATE       0x0002  /* Invalidate after scrolling */
#define SW_ERASE            0x0004  /* If SW_INVALIDATE, don't send WM_ERASEBACKGROUND */
#if(WINVER >= 0x0500)
#define SW_SMOOTHSCROLL     0x0010  /* Use smooth scrolling */
#endif /* WINVER >= 0x0500 */

#pragma region Desktop Family
#if WINAPI_FAMILY_PARTITION(WINAPI_PARTITION_DESKTOP)

#ifndef NOSCROLL

_Win32_metadata_set_last_error_
WINUSERAPI
int
WINAPI
SetScrollPos(
    _In_ HWND hWnd,
    _In_ int nBar,
    _In_ int nPos,
    _In_ BOOL bRedraw);

_Win32_metadata_set_last_error_
WINUSERAPI
int
WINAPI
GetScrollPos(
    _In_ HWND hWnd,
    _In_ int nBar);

_Win32_metadata_set_last_error_
WINUSERAPI
BOOL
WINAPI
SetScrollRange(
    _In_ HWND hWnd,
    _In_ int nBar,
    _In_ int nMinPos,
    _In_ int nMaxPos,
    _In_ BOOL bRedraw);

_Win32_metadata_set_last_error_
WINUSERAPI
BOOL
WINAPI
GetScrollRange(
    _In_ HWND hWnd,
    _In_ int nBar,
    _Out_ LPINT lpMinPos,
    _Out_ LPINT lpMaxPos);

_Win32_metadata_set_last_error_
WINUSERAPI
BOOL
WINAPI
ShowScrollBar(
    _In_ HWND hWnd,
    _In_ int wBar,
    _In_ BOOL bShow);

_Win32_metadata_set_last_error_
WINUSERAPI
BOOL
WINAPI
EnableScrollBar(
    _In_ HWND hWnd,
    _In_ UINT wSBflags,
    _In_ ENABLE_SCROLL_BAR_ARROWS wArrows);


/*
 * EnableScrollBar() flags
 */
#define ESB_ENABLE_BOTH     0x0000
#define ESB_DISABLE_BOTH    0x0003

#define ESB_DISABLE_LEFT    0x0001
#define ESB_DISABLE_RIGHT   0x0002

#define ESB_DISABLE_UP      0x0001
#define ESB_DISABLE_DOWN    0x0002

#define ESB_DISABLE_LTUP    ESB_DISABLE_LEFT
#define ESB_DISABLE_RTDN    ESB_DISABLE_RIGHT


#endif  /* !NOSCROLL */


_Win32_metadata_set_last_error_
WINUSERAPI
BOOL
WINAPI
SetPropA(
    _In_ HWND hWnd,
    _In_ LPCSTR lpString,
    _In_opt_ HANDLE hData);
_Win32_metadata_set_last_error_
WINUSERAPI
BOOL
WINAPI
SetPropW(
    _In_ HWND hWnd,
    _In_ LPCWSTR lpString,
    _In_opt_ HANDLE hData);
#ifdef UNICODE
#define SetProp  SetPropW
#else
#define SetProp  SetPropA
#endif // !UNICODE

WINUSERAPI
HANDLE
WINAPI
GetPropA(
    _In_ HWND hWnd,
    _In_ LPCSTR lpString);
WINUSERAPI
HANDLE
WINAPI
GetPropW(
    _In_ HWND hWnd,
    _In_ LPCWSTR lpString);
#ifdef UNICODE
#define GetProp  GetPropW
#else
#define GetProp  GetPropA
#endif // !UNICODE

_Win32_metadata_set_last_error_
WINUSERAPI
HANDLE
WINAPI
RemovePropA(
    _In_ HWND hWnd,
    _In_ LPCSTR lpString);
_Win32_metadata_set_last_error_
WINUSERAPI
HANDLE
WINAPI
RemovePropW(
    _In_ HWND hWnd,
    _In_ LPCWSTR lpString);
#ifdef UNICODE
#define RemoveProp  RemovePropW
#else
#define RemoveProp  RemovePropA
#endif // !UNICODE

WINUSERAPI
int
WINAPI
EnumPropsExA(
    _In_ HWND hWnd,
    _In_ PROPENUMPROCEXA lpEnumFunc,
    _In_ LPARAM lParam);
WINUSERAPI
int
WINAPI
EnumPropsExW(
    _In_ HWND hWnd,
    _In_ PROPENUMPROCEXW lpEnumFunc,
    _In_ LPARAM lParam);
#ifdef UNICODE
#define EnumPropsEx  EnumPropsExW
#else
#define EnumPropsEx  EnumPropsExA
#endif // !UNICODE

WINUSERAPI
int
WINAPI
EnumPropsA(
    _In_ HWND hWnd,
    _In_ PROPENUMPROCA lpEnumFunc);
WINUSERAPI
int
WINAPI
EnumPropsW(
    _In_ HWND hWnd,
    _In_ PROPENUMPROCW lpEnumFunc);
#ifdef UNICODE
#define EnumProps  EnumPropsW
#else
#define EnumProps  EnumPropsA
#endif // !UNICODE

_Win32_metadata_set_last_error_
WINUSERAPI
BOOL
WINAPI
SetWindowTextA(
    _In_ HWND hWnd,
    _In_opt_ LPCSTR lpString);
_Win32_metadata_set_last_error_
WINUSERAPI
BOOL
WINAPI
SetWindowTextW(
    _In_ HWND hWnd,
    _In_opt_ LPCWSTR lpString);
#ifdef UNICODE
#define SetWindowText  SetWindowTextW
#else
#define SetWindowText  SetWindowTextA
#endif // !UNICODE

_Ret_range_(0, nMaxCount)
_Win32_metadata_set_last_error_
WINUSERAPI
int
WINAPI
GetWindowTextA(
    _In_ HWND hWnd,
    _Out_writes_(nMaxCount) LPSTR lpString,
    _In_ int nMaxCount);
_Ret_range_(0, nMaxCount)
_Win32_metadata_set_last_error_
WINUSERAPI
int
WINAPI
GetWindowTextW(
    _In_ HWND hWnd,
    _Out_writes_(nMaxCount) LPWSTR lpString,
    _In_ int nMaxCount);
#ifdef UNICODE
#define GetWindowText  GetWindowTextW
#else
#define GetWindowText  GetWindowTextA
#endif // !UNICODE

_Win32_metadata_set_last_error_
WINUSERAPI
int
WINAPI
GetWindowTextLengthA(
    _In_ HWND hWnd);
_Win32_metadata_set_last_error_
WINUSERAPI
int
WINAPI
GetWindowTextLengthW(
    _In_ HWND hWnd);
#ifdef UNICODE
#define GetWindowTextLength  GetWindowTextLengthW
#else
#define GetWindowTextLength  GetWindowTextLengthA
#endif // !UNICODE

#endif /* WINAPI_FAMILY_PARTITION(WINAPI_PARTITION_DESKTOP) */
#pragma endregion

#pragma region Desktop Family or Games Family
#if WINAPI_FAMILY_PARTITION(WINAPI_PARTITION_DESKTOP | WINAPI_PARTITION_GAMES)

_Win32_metadata_set_last_error_
WINUSERAPI
BOOL
WINAPI
GetClientRect(
    _In_ HWND hWnd,
    _Out_ LPRECT lpRect);

#endif /* WINAPI_FAMILY_PARTITION(WINAPI_PARTITION_DESKTOP | WINAPI_PARTITION_GAMES) */
#pragma endregion

#pragma region Desktop Family
#if WINAPI_FAMILY_PARTITION(WINAPI_PARTITION_DESKTOP)

_Win32_metadata_set_last_error_
WINUSERAPI
BOOL
WINAPI
GetWindowRect(
    _In_ HWND hWnd,
    _Out_ LPRECT lpRect);

#endif /* WINAPI_FAMILY_PARTITION(WINAPI_PARTITION_DESKTOP) */
#pragma endregion

#pragma region Desktop Family or Games Family
#if WINAPI_FAMILY_PARTITION(WINAPI_PARTITION_DESKTOP | WINAPI_PARTITION_GAMES)

_Win32_metadata_set_last_error_
WINUSERAPI
BOOL
WINAPI
AdjustWindowRect(
    _Inout_ LPRECT lpRect,
    _In_ WINDOW_STYLE dwStyle,
    _In_ BOOL bMenu);

_Win32_metadata_set_last_error_
WINUSERAPI
BOOL
WINAPI
AdjustWindowRectEx(
    _Inout_ LPRECT lpRect,
    _In_ WINDOW_STYLE dwStyle,
    _In_ BOOL bMenu,
    _In_ WINDOW_EX_STYLE dwExStyle);

#endif /* WINAPI_FAMILY_PARTITION(WINAPI_PARTITION_DESKTOP | WINAPI_PARTITION_GAMES) */
#pragma endregion

#pragma region Desktop Family
#if WINAPI_FAMILY_PARTITION(WINAPI_PARTITION_DESKTOP)

#if(WINVER >= 0x0605)
_Win32_metadata_set_last_error_
WINUSERAPI
BOOL
WINAPI
AdjustWindowRectExForDpi(
    _Inout_ LPRECT lpRect,
    _In_ WINDOW_STYLE dwStyle,
    _In_ BOOL bMenu,
    _In_ WINDOW_EX_STYLE dwExStyle,
    _In_ UINT dpi);
#endif /* WINVER >= 0x0605 */


#endif /* WINAPI_FAMILY_PARTITION(WINAPI_PARTITION_DESKTOP) */
#pragma endregion

#if(WINVER >= 0x0400)
#define HELPINFO_WINDOW    0x0001
#define HELPINFO_MENUITEM  0x0002

#pragma region Desktop Family
#if WINAPI_FAMILY_PARTITION(WINAPI_PARTITION_DESKTOP)

typedef struct tagHELPINFO      /* Structure pointed to by lParam of WM_HELP */
{
    UINT    cbSize;             /* Size in bytes of this struct  */
    HELP_INFO_TYPE     iContextType;       /* Either HELPINFO_WINDOW or HELPINFO_MENUITEM */
    int     iCtrlId;            /* Control Id or a Menu item Id. */
    HANDLE  hItemHandle;        /* hWnd of control or hMenu.     */
    DWORD_PTR dwContextId;      /* Context Id associated with this item */
    POINT   MousePos;           /* Mouse Position in screen co-ordinates */
}  HELPINFO, FAR *LPHELPINFO;

_Win32_metadata_set_last_error_
WINUSERAPI
BOOL
WINAPI
SetWindowContextHelpId(
    _In_ HWND,
    _In_ DWORD);

WINUSERAPI
DWORD
WINAPI
GetWindowContextHelpId(
    _In_ HWND);

_Win32_metadata_set_last_error_
WINUSERAPI
BOOL
WINAPI
SetMenuContextHelpId(
    _In_ HMENU,
    _In_ DWORD);

WINUSERAPI
DWORD
WINAPI
GetMenuContextHelpId(
    _In_ HMENU);

#endif /* WINAPI_FAMILY_PARTITION(WINAPI_PARTITION_DESKTOP) */
#pragma endregion

#endif /* WINVER >= 0x0400 */


#ifndef NOMB

/*
 * MessageBox() Flags
 */
#define MB_OK                       0x00000000L
#define MB_OKCANCEL                 0x00000001L
#define MB_ABORTRETRYIGNORE         0x00000002L
#define MB_YESNOCANCEL              0x00000003L
#define MB_YESNO                    0x00000004L
#define MB_RETRYCANCEL              0x00000005L
#if(WINVER >= 0x0500)
#define MB_CANCELTRYCONTINUE        0x00000006L
#endif /* WINVER >= 0x0500 */


#define MB_ICONHAND                 0x00000010L
#define MB_ICONQUESTION             0x00000020L
#define MB_ICONEXCLAMATION          0x00000030L
#define MB_ICONASTERISK             0x00000040L

#if(WINVER >= 0x0400)
#define MB_USERICON                 0x00000080L
#define MB_ICONWARNING              MB_ICONEXCLAMATION
#define MB_ICONERROR                MB_ICONHAND
#endif /* WINVER >= 0x0400 */

#define MB_ICONINFORMATION          MB_ICONASTERISK
#define MB_ICONSTOP                 MB_ICONHAND

#define MB_DEFBUTTON1               0x00000000L
#define MB_DEFBUTTON2               0x00000100L
#define MB_DEFBUTTON3               0x00000200L
#if(WINVER >= 0x0400)
#define MB_DEFBUTTON4               0x00000300L
#endif /* WINVER >= 0x0400 */

#define MB_APPLMODAL                0x00000000L
#define MB_SYSTEMMODAL              0x00001000L
#define MB_TASKMODAL                0x00002000L
#if(WINVER >= 0x0400)
#define MB_HELP                     0x00004000L // Help Button
#endif /* WINVER >= 0x0400 */

#define MB_NOFOCUS                  0x00008000L
#define MB_SETFOREGROUND            0x00010000L
#define MB_DEFAULT_DESKTOP_ONLY     0x00020000L

#if(WINVER >= 0x0400)
#define MB_TOPMOST                  0x00040000L
#define MB_RIGHT                    0x00080000L
#define MB_RTLREADING               0x00100000L

#endif /* WINVER >= 0x0400 */

#ifdef _WIN32_WINNT
#if (_WIN32_WINNT >= 0x0400)
#define MB_SERVICE_NOTIFICATION          0x00200000L
#else
#define MB_SERVICE_NOTIFICATION          0x00040000L
#endif
#define MB_SERVICE_NOTIFICATION_NT3X     0x00040000L
#endif

#define MB_TYPEMASK                 0x0000000FL
#define MB_ICONMASK                 0x000000F0L
#define MB_DEFMASK                  0x00000F00L
#define MB_MODEMASK                 0x00003000L
#define MB_MISCMASK                 0x0000C000L

#pragma region Desktop Family
#if WINAPI_FAMILY_PARTITION(WINAPI_PARTITION_DESKTOP)

_Win32_metadata_set_last_error_
WINUSERAPI
int
WINAPI
MessageBoxA(
    _In_opt_ HWND hWnd,
    _In_opt_ LPCSTR lpText,
    _In_opt_ LPCSTR lpCaption,
    _In_ UINT uType);
_Win32_metadata_set_last_error_
WINUSERAPI
int
WINAPI
MessageBoxW(
    _In_opt_ HWND hWnd,
    _In_opt_ LPCWSTR lpText,
    _In_opt_ LPCWSTR lpCaption,
    _In_ UINT uType);
#ifdef UNICODE
#define MessageBox  MessageBoxW
#else
#define MessageBox  MessageBoxA
#endif // !UNICODE

#if defined(_M_CEE)
#undef MessageBox
__inline
int
MessageBox(
    HWND hWnd,
    LPCTSTR lpText,
    LPCTSTR lpCaption,
    UINT uType
    )
{
#ifdef UNICODE
    return MessageBoxW(
#else
    return MessageBoxA(
#endif
        hWnd,
    lpText,
    lpCaption,
    uType
        );
}
#endif  /* _M_CEE */

_Win32_metadata_set_last_error_
WINUSERAPI
int
WINAPI
MessageBoxExA(
    _In_opt_ HWND hWnd,
    _In_opt_ LPCSTR lpText,
    _In_opt_ LPCSTR lpCaption,
    _In_ UINT uType,
    _In_ WORD wLanguageId);
_Win32_metadata_set_last_error_
WINUSERAPI
int
WINAPI
MessageBoxExW(
    _In_opt_ HWND hWnd,
    _In_opt_ LPCWSTR lpText,
    _In_opt_ LPCWSTR lpCaption,
    _In_ UINT uType,
    _In_ WORD wLanguageId);
#ifdef UNICODE
#define MessageBoxEx  MessageBoxExW
#else
#define MessageBoxEx  MessageBoxExA
#endif // !UNICODE

#if(WINVER >= 0x0400)

typedef VOID (CALLBACK *MSGBOXCALLBACK)(LPHELPINFO lpHelpInfo);

typedef struct tagMSGBOXPARAMSA
{
    UINT        cbSize;
    HWND        hwndOwner;
    HINSTANCE   hInstance;
    LPCSTR      lpszText;
    LPCSTR      lpszCaption;
    DWORD       dwStyle;
    LPCSTR      lpszIcon;
    DWORD_PTR   dwContextHelpId;
    MSGBOXCALLBACK      lpfnMsgBoxCallback;
    DWORD       dwLanguageId;
} MSGBOXPARAMSA, *PMSGBOXPARAMSA, *LPMSGBOXPARAMSA;
typedef struct tagMSGBOXPARAMSW
{
    UINT        cbSize;
    HWND        hwndOwner;
    HINSTANCE   hInstance;
    LPCWSTR     lpszText;
    LPCWSTR     lpszCaption;
    DWORD       dwStyle;
    LPCWSTR     lpszIcon;
    DWORD_PTR   dwContextHelpId;
    MSGBOXCALLBACK      lpfnMsgBoxCallback;
    DWORD       dwLanguageId;
} MSGBOXPARAMSW, *PMSGBOXPARAMSW, *LPMSGBOXPARAMSW;
#ifdef UNICODE
typedef MSGBOXPARAMSW MSGBOXPARAMS;
typedef PMSGBOXPARAMSW PMSGBOXPARAMS;
typedef LPMSGBOXPARAMSW LPMSGBOXPARAMS;
#else
typedef MSGBOXPARAMSA MSGBOXPARAMS;
typedef PMSGBOXPARAMSA PMSGBOXPARAMS;
typedef LPMSGBOXPARAMSA LPMSGBOXPARAMS;
#endif // UNICODE

WINUSERAPI
int
WINAPI
MessageBoxIndirectA(
    _In_ CONST MSGBOXPARAMSA * lpmbp);
WINUSERAPI
int
WINAPI
MessageBoxIndirectW(
    _In_ CONST MSGBOXPARAMSW * lpmbp);
#ifdef UNICODE
#define MessageBoxIndirect  MessageBoxIndirectW
#else
#define MessageBoxIndirect  MessageBoxIndirectA
#endif // !UNICODE
#endif /* WINVER >= 0x0400 */


#endif /* WINAPI_FAMILY_PARTITION(WINAPI_PARTITION_DESKTOP) */
#pragma endregion


#pragma region Desktop Family
#if WINAPI_FAMILY_PARTITION(WINAPI_PARTITION_DESKTOP)

_Win32_metadata_set_last_error_
WINUSERAPI
BOOL
WINAPI
MessageBeep(
    _In_ UINT uType);

#endif /* WINAPI_FAMILY_PARTITION(WINAPI_PARTITION_DESKTOP) */
#pragma endregion

#endif /* !NOMB */

#pragma region Desktop Family
#if WINAPI_FAMILY_PARTITION(WINAPI_PARTITION_DESKTOP)


#endif /* WINAPI_FAMILY_PARTITION(WINAPI_PARTITION_DESKTOP) */
#pragma endregion

#pragma region Desktop Family or Games Family
#if WINAPI_FAMILY_PARTITION(WINAPI_PARTITION_DESKTOP | WINAPI_PARTITION_GAMES)

WINUSERAPI
int
WINAPI
ShowCursor(
    _In_ BOOL bShow);

#endif /* WINAPI_FAMILY_PARTITION(WINAPI_PARTITION_DESKTOP | WINAPI_PARTITION_GAMES) */
#pragma endregion

#pragma region Desktop Family
#if WINAPI_FAMILY_PARTITION(WINAPI_PARTITION_DESKTOP)

_Win32_metadata_set_last_error_
WINUSERAPI
BOOL
WINAPI
SetCursorPos(
    _In_ int X,
    _In_ int Y);

#if(WINVER >= 0x0600)
_Win32_metadata_set_last_error_
WINUSERAPI
BOOL
WINAPI
SetPhysicalCursorPos(
    _In_ int X,
    _In_ int Y);
#endif /* WINVER >= 0x0600 */

#endif /* WINAPI_FAMILY_PARTITION(WINAPI_PARTITION_DESKTOP) */
#pragma endregion

#pragma region Desktop Family or Games Family
#if WINAPI_FAMILY_PARTITION(WINAPI_PARTITION_DESKTOP | WINAPI_PARTITION_GAMES)

WINUSERAPI
HCURSOR
WINAPI
SetCursor(
    _In_opt_ HCURSOR hCursor);

_Win32_metadata_set_last_error_
WINUSERAPI
BOOL
WINAPI
GetCursorPos(
    _Out_ LPPOINT lpPoint);

#endif /* WINAPI_FAMILY_PARTITION(WINAPI_PARTITION_DESKTOP | WINAPI_PARTITION_GAMES) */
#pragma endregion

#pragma region Desktop Family
#if WINAPI_FAMILY_PARTITION(WINAPI_PARTITION_DESKTOP)

#if(WINVER >= 0x0600)
_Win32_metadata_set_last_error_
WINUSERAPI
BOOL
WINAPI
GetPhysicalCursorPos(
    _Out_ LPPOINT lpPoint);
#endif /* WINVER >= 0x0600 */


_Win32_metadata_set_last_error_
WINUSERAPI
BOOL
WINAPI
GetClipCursor(
    _Out_ LPRECT lpRect);

WINUSERAPI
HCURSOR
WINAPI
GetCursor(
    VOID);

_Win32_metadata_set_last_error_
WINUSERAPI
BOOL
WINAPI
CreateCaret(
    _In_ HWND hWnd,
    _In_opt_ HBITMAP hBitmap,
    _In_ int nWidth,
    _In_ int nHeight);

_Win32_metadata_set_last_error_
WINUSERAPI
UINT
WINAPI
GetCaretBlinkTime(
    VOID);

_Win32_metadata_set_last_error_
WINUSERAPI
BOOL
WINAPI
SetCaretBlinkTime(
    _In_ UINT uMSeconds);

_Win32_metadata_set_last_error_
WINUSERAPI
BOOL
WINAPI
DestroyCaret(
    VOID);

_Win32_metadata_set_last_error_
WINUSERAPI
BOOL
WINAPI
HideCaret(
    _In_opt_ HWND hWnd);

_Win32_metadata_set_last_error_
WINUSERAPI
BOOL
WINAPI
ShowCaret(
    _In_opt_ HWND hWnd);

_Win32_metadata_set_last_error_
WINUSERAPI
BOOL
WINAPI
SetCaretPos(
    _In_ int X,
    _In_ int Y);

_Win32_metadata_set_last_error_
WINUSERAPI
BOOL
WINAPI
GetCaretPos(
    _Out_ LPPOINT lpPoint);

WINUSERAPI
BOOL
WINAPI
ClientToScreen(
    _In_ HWND hWnd,
    _Inout_ LPPOINT lpPoint);

WINUSERAPI
BOOL
WINAPI
ScreenToClient(
    _In_ HWND hWnd,
    _Inout_ LPPOINT lpPoint);

#if(WINVER >= 0x0600)
WINUSERAPI
BOOL
WINAPI
LogicalToPhysicalPoint(
    _In_ HWND hWnd,
    _Inout_ LPPOINT lpPoint);

WINUSERAPI
BOOL
WINAPI
PhysicalToLogicalPoint(
    _In_ HWND hWnd,
    _Inout_ LPPOINT lpPoint);

#endif /* WINVER >= 0x0600 */

#if(WINVER >= 0x0603)
WINUSERAPI
BOOL
WINAPI
LogicalToPhysicalPointForPerMonitorDPI(
    _In_opt_ HWND hWnd,
    _Inout_ LPPOINT lpPoint);

WINUSERAPI
BOOL
WINAPI
PhysicalToLogicalPointForPerMonitorDPI(
    _In_opt_ HWND hWnd,
    _Inout_ LPPOINT lpPoint);

#endif /* WINVER >= 0x0603 */

WINUSERAPI
int
WINAPI
MapWindowPoints(
    _In_opt_ HWND hWndFrom,
    _In_opt_ HWND hWndTo,
    _Inout_updates_(cPoints) LPPOINT lpPoints,
    _In_ UINT cPoints);

WINUSERAPI
HWND
WINAPI
WindowFromPoint(
    _In_ POINT Point);

#if(WINVER >= 0x0600)
WINUSERAPI
HWND
WINAPI
WindowFromPhysicalPoint(
    _In_ POINT Point);
#endif /* WINVER >= 0x0600 */

WINUSERAPI
HWND
WINAPI
ChildWindowFromPoint(
    _In_ HWND hWndParent,
    _In_ POINT Point);

#endif /* WINAPI_FAMILY_PARTITION(WINAPI_PARTITION_DESKTOP) */
#pragma endregion

#pragma region Desktop or PC Family
#if WINAPI_FAMILY_PARTITION(WINAPI_PARTITION_DESKTOP | WINAPI_PARTITION_PC_APP)
_Win32_metadata_set_last_error_
WINUSERAPI
BOOL
WINAPI
ClipCursor(
    _In_opt_ CONST RECT *lpRect);
#endif /* WINAPI_FAMILY_PARTITION(WINAPI_PARTITION_DESKTOP | WINAPI_PARTITION_PC_APP) */
#pragma endregion

#if(WINVER >= 0x0400)
#define CWP_ALL             0x0000
#define CWP_SKIPINVISIBLE   0x0001
#define CWP_SKIPDISABLED    0x0002
#define CWP_SKIPTRANSPARENT 0x0004

#pragma region Desktop Family
#if WINAPI_FAMILY_PARTITION(WINAPI_PARTITION_DESKTOP)

WINUSERAPI
HWND
WINAPI
ChildWindowFromPointEx(
    _In_ HWND hwnd,
    _In_ POINT pt,
    _In_ CWP_FLAGS flags);

#endif /* WINAPI_FAMILY_PARTITION(WINAPI_PARTITION_DESKTOP) */
#pragma endregion

#endif /* WINVER >= 0x0400 */

#ifndef NOCOLOR

/*
 * Color Types
 */
#define CTLCOLOR_MSGBOX         0
#define CTLCOLOR_EDIT           1
#define CTLCOLOR_LISTBOX        2
#define CTLCOLOR_BTN            3
#define CTLCOLOR_DLG            4
#define CTLCOLOR_SCROLLBAR      5
#define CTLCOLOR_STATIC         6
#define CTLCOLOR_MAX            7

#define COLOR_SCROLLBAR         0
#define COLOR_BACKGROUND        1
#define COLOR_ACTIVECAPTION     2
#define COLOR_INACTIVECAPTION   3
#define COLOR_MENU              4
#define COLOR_WINDOW            5
#define COLOR_WINDOWFRAME       6
#define COLOR_MENUTEXT          7
#define COLOR_WINDOWTEXT        8
#define COLOR_CAPTIONTEXT       9
#define COLOR_ACTIVEBORDER      10
#define COLOR_INACTIVEBORDER    11
#define COLOR_APPWORKSPACE      12
#define COLOR_HIGHLIGHT         13
#define COLOR_HIGHLIGHTTEXT     14
#define COLOR_BTNFACE           15
#define COLOR_BTNSHADOW         16
#define COLOR_GRAYTEXT          17
#define COLOR_BTNTEXT           18
#define COLOR_INACTIVECAPTIONTEXT 19
#define COLOR_BTNHIGHLIGHT      20

#if(WINVER >= 0x0400)
#define COLOR_3DDKSHADOW        21
#define COLOR_3DLIGHT           22
#define COLOR_INFOTEXT          23
#define COLOR_INFOBK            24
#endif /* WINVER >= 0x0400 */

#if(WINVER >= 0x0500)
#define COLOR_HOTLIGHT          26
#define COLOR_GRADIENTACTIVECAPTION 27
#define COLOR_GRADIENTINACTIVECAPTION 28
#if(WINVER >= 0x0501)
#define COLOR_MENUHILIGHT       29
#define COLOR_MENUBAR           30
#endif /* WINVER >= 0x0501 */
#endif /* WINVER >= 0x0500 */

#if(WINVER >= 0x0400)
#define COLOR_DESKTOP           COLOR_BACKGROUND
#define COLOR_3DFACE            COLOR_BTNFACE
#define COLOR_3DSHADOW          COLOR_BTNSHADOW
#define COLOR_3DHIGHLIGHT       COLOR_BTNHIGHLIGHT
#define COLOR_3DHILIGHT         COLOR_BTNHIGHLIGHT
#define COLOR_BTNHILIGHT        COLOR_BTNHIGHLIGHT
#endif /* WINVER >= 0x0400 */


#pragma region Desktop Family
#if WINAPI_FAMILY_PARTITION(WINAPI_PARTITION_DESKTOP)

WINUSERAPI
DWORD
WINAPI
GetSysColor(
    _In_ int nIndex);

#if(WINVER >= 0x0400)
WINUSERAPI
HBRUSH
WINAPI
GetSysColorBrush(
    _In_ int nIndex);


#endif /* WINVER >= 0x0400 */

_Win32_metadata_set_last_error_
WINUSERAPI
BOOL
WINAPI
SetSysColors(
    _In_ int cElements,
    _In_reads_(cElements) CONST INT * lpaElements,
    _In_reads_(cElements) CONST COLORREF * lpaRgbValues);

#endif /* WINAPI_FAMILY_PARTITION(WINAPI_PARTITION_DESKTOP) */
#pragma endregion

#endif /* !NOCOLOR */

#pragma region Desktop Family
#if WINAPI_FAMILY_PARTITION(WINAPI_PARTITION_DESKTOP)

WINUSERAPI
BOOL
WINAPI
DrawFocusRect(
    _In_ HDC hDC,
    _In_ CONST RECT * lprc);

WINUSERAPI
int
WINAPI
FillRect(
    _In_ HDC hDC,
    _In_ CONST RECT *lprc,
    _In_ HBRUSH hbr);

WINUSERAPI
int
WINAPI
FrameRect(
    _In_ HDC hDC,
    _In_ CONST RECT *lprc,
    _In_ HBRUSH hbr);

WINUSERAPI
BOOL
WINAPI
InvertRect(
    _In_ HDC hDC,
    _In_ CONST RECT *lprc);

WINUSERAPI
BOOL
WINAPI
SetRect(
    _Out_ LPRECT lprc,
    _In_ int xLeft,
    _In_ int yTop,
    _In_ int xRight,
    _In_ int yBottom);

WINUSERAPI
BOOL
WINAPI
SetRectEmpty(
    _Out_ LPRECT lprc);

WINUSERAPI
BOOL
WINAPI
CopyRect(
    _Out_ LPRECT lprcDst,
    _In_ CONST RECT *lprcSrc);

WINUSERAPI
BOOL
WINAPI
InflateRect(
    _Inout_ LPRECT lprc,
    _In_ int dx,
    _In_ int dy);

WINUSERAPI
BOOL
WINAPI
IntersectRect(
    _Out_ LPRECT lprcDst,
    _In_ CONST RECT *lprcSrc1,
    _In_ CONST RECT *lprcSrc2);

WINUSERAPI
BOOL
WINAPI
UnionRect(
    _Out_ LPRECT lprcDst,
    _In_ CONST RECT *lprcSrc1,
    _In_ CONST RECT *lprcSrc2);

WINUSERAPI
BOOL
WINAPI
SubtractRect(
    _Out_ LPRECT lprcDst,
    _In_ CONST RECT *lprcSrc1,
    _In_ CONST RECT *lprcSrc2);

WINUSERAPI
BOOL
WINAPI
OffsetRect(
    _Inout_ LPRECT lprc,
    _In_ int dx,
    _In_ int dy);

WINUSERAPI
BOOL
WINAPI
IsRectEmpty(
    _In_ CONST RECT *lprc);

WINUSERAPI
BOOL
WINAPI
EqualRect(
    _In_ CONST RECT *lprc1,
    _In_ CONST RECT *lprc2);

WINUSERAPI
BOOL
WINAPI
PtInRect(
    _In_ CONST RECT *lprc,
    _In_ POINT pt);

#ifndef NOWINOFFSETS

WINUSERAPI
WORD
WINAPI
GetWindowWord(
    _In_ HWND hWnd,
    _In_ int nIndex);

WINUSERAPI
WORD
WINAPI
SetWindowWord(
    _In_ HWND hWnd,
    _In_ int nIndex,
    _In_ WORD wNewWord);

#endif /* !NOWINOFFSETS */

#endif /* WINAPI_FAMILY_PARTITION(WINAPI_PARTITION_DESKTOP) */
#pragma endregion

#pragma region Desktop Family or Games Family
#if WINAPI_FAMILY_PARTITION(WINAPI_PARTITION_DESKTOP | WINAPI_PARTITION_GAMES)

#ifndef NOWINOFFSETS

_Win32_metadata_set_last_error_
WINUSERAPI
LONG
WINAPI
GetWindowLongA(
    _In_ HWND hWnd,
    _In_ int nIndex);
_Win32_metadata_set_last_error_
WINUSERAPI
LONG
WINAPI
GetWindowLongW(
    _In_ HWND hWnd,
    _In_ int nIndex);
#ifdef UNICODE
#define GetWindowLong  GetWindowLongW
#else
#define GetWindowLong  GetWindowLongA
#endif // !UNICODE

_Win32_metadata_set_last_error_
WINUSERAPI
LONG
WINAPI
SetWindowLongA(
    _In_ HWND hWnd,
    _In_ int nIndex,
    _In_ LONG dwNewLong);
_Win32_metadata_set_last_error_
WINUSERAPI
LONG
WINAPI
SetWindowLongW(
    _In_ HWND hWnd,
    _In_ int nIndex,
    _In_ LONG dwNewLong);
#ifdef UNICODE
#define SetWindowLong  SetWindowLongW
#else
#define SetWindowLong  SetWindowLongA
#endif // !UNICODE

#ifdef _WIN64

_Win32_metadata_set_last_error_
WINUSERAPI
LONG_PTR
WINAPI
GetWindowLongPtrA(
    _In_ HWND hWnd,
    _In_ int nIndex);
_Win32_metadata_set_last_error_
WINUSERAPI
LONG_PTR
WINAPI
GetWindowLongPtrW(
    _In_ HWND hWnd,
    _In_ int nIndex);
#ifdef UNICODE
#define GetWindowLongPtr  GetWindowLongPtrW
#else
#define GetWindowLongPtr  GetWindowLongPtrA
#endif // !UNICODE

_Win32_metadata_set_last_error_
WINUSERAPI
LONG_PTR
WINAPI
SetWindowLongPtrA(
    _In_ HWND hWnd,
    _In_ int nIndex,
    _In_ LONG_PTR dwNewLong);
_Win32_metadata_set_last_error_
WINUSERAPI
LONG_PTR
WINAPI
SetWindowLongPtrW(
    _In_ HWND hWnd,
    _In_ int nIndex,
    _In_ LONG_PTR dwNewLong);
#ifdef UNICODE
#define SetWindowLongPtr  SetWindowLongPtrW
#else
#define SetWindowLongPtr  SetWindowLongPtrA
#endif // !UNICODE

#else  /* _WIN64 */

#define GetWindowLongPtrA   GetWindowLongA
#define GetWindowLongPtrW   GetWindowLongW
#ifdef UNICODE
#define GetWindowLongPtr  GetWindowLongPtrW
#else
#define GetWindowLongPtr  GetWindowLongPtrA
#endif // !UNICODE

#define SetWindowLongPtrA   SetWindowLongA
#define SetWindowLongPtrW   SetWindowLongW
#ifdef UNICODE
#define SetWindowLongPtr  SetWindowLongPtrW
#else
#define SetWindowLongPtr  SetWindowLongPtrA
#endif // !UNICODE

#endif /* _WIN64 */

#endif /* !NOWINOFFSETS */

#endif /* WINAPI_FAMILY_PARTITION(WINAPI_PARTITION_DESKTOP | WINAPI_PARTITION_GAMES) */
#pragma endregion

#pragma region Desktop Family
#if WINAPI_FAMILY_PARTITION(WINAPI_PARTITION_DESKTOP)

#ifndef NOWINOFFSETS

_Win32_metadata_set_last_error_
WINUSERAPI
WORD
WINAPI
GetClassWord(
    _In_ HWND hWnd,
    _In_ int nIndex);

_Win32_metadata_set_last_error_
WINUSERAPI
WORD
WINAPI
SetClassWord(
    _In_ HWND hWnd,
    _In_ int nIndex,
    _In_ WORD wNewWord);

_Win32_metadata_set_last_error_
WINUSERAPI
DWORD
WINAPI
GetClassLongA(
    _In_ HWND hWnd,
    _In_ int nIndex);
_Win32_metadata_set_last_error_
WINUSERAPI
DWORD
WINAPI
GetClassLongW(
    _In_ HWND hWnd,
    _In_ int nIndex);
#ifdef UNICODE
#define GetClassLong  GetClassLongW
#else
#define GetClassLong  GetClassLongA
#endif // !UNICODE

_Win32_metadata_set_last_error_
WINUSERAPI
DWORD
WINAPI
SetClassLongA(
    _In_ HWND hWnd,
    _In_ int nIndex,
    _In_ LONG dwNewLong);
_Win32_metadata_set_last_error_
WINUSERAPI
DWORD
WINAPI
SetClassLongW(
    _In_ HWND hWnd,
    _In_ int nIndex,
    _In_ LONG dwNewLong);
#ifdef UNICODE
#define SetClassLong  SetClassLongW
#else
#define SetClassLong  SetClassLongA
#endif // !UNICODE

#ifdef _WIN64

_Win32_metadata_set_last_error_
WINUSERAPI
ULONG_PTR
WINAPI
GetClassLongPtrA(
    _In_ HWND hWnd,
    _In_ int nIndex);
_Win32_metadata_set_last_error_
WINUSERAPI
ULONG_PTR
WINAPI
GetClassLongPtrW(
    _In_ HWND hWnd,
    _In_ int nIndex);
#ifdef UNICODE
#define GetClassLongPtr  GetClassLongPtrW
#else
#define GetClassLongPtr  GetClassLongPtrA
#endif // !UNICODE

_Win32_metadata_set_last_error_
WINUSERAPI
ULONG_PTR
WINAPI
SetClassLongPtrA(
    _In_ HWND hWnd,
    _In_ int nIndex,
    _In_ LONG_PTR dwNewLong);
_Win32_metadata_set_last_error_
WINUSERAPI
ULONG_PTR
WINAPI
SetClassLongPtrW(
    _In_ HWND hWnd,
    _In_ int nIndex,
    _In_ LONG_PTR dwNewLong);
#ifdef UNICODE
#define SetClassLongPtr  SetClassLongPtrW
#else
#define SetClassLongPtr  SetClassLongPtrA
#endif // !UNICODE

#else  /* _WIN64 */

#define GetClassLongPtrA    GetClassLongA
#define GetClassLongPtrW    GetClassLongW
#ifdef UNICODE
#define GetClassLongPtr  GetClassLongPtrW
#else
#define GetClassLongPtr  GetClassLongPtrA
#endif // !UNICODE

#define SetClassLongPtrA    SetClassLongA
#define SetClassLongPtrW    SetClassLongW
#ifdef UNICODE
#define SetClassLongPtr  SetClassLongPtrW
#else
#define SetClassLongPtr  SetClassLongPtrA
#endif // !UNICODE

#endif /* _WIN64 */

#endif /* !NOWINOFFSETS */

#if(WINVER >= 0x0500)
_Win32_metadata_set_last_error_
WINUSERAPI
BOOL
WINAPI
GetProcessDefaultLayout(
    _Out_ DWORD *pdwDefaultLayout);

_Win32_metadata_set_last_error_
WINUSERAPI
BOOL
WINAPI
SetProcessDefaultLayout(
    _In_ DWORD dwDefaultLayout);
#endif /* WINVER >= 0x0500 */

WINUSERAPI
HWND
WINAPI
GetDesktopWindow(
    VOID);


_Win32_metadata_set_last_error_
WINUSERAPI
HWND
WINAPI
GetParent(
    _In_ HWND hWnd);

_Win32_metadata_set_last_error_
WINUSERAPI
HWND
WINAPI
SetParent(
    _In_ HWND hWndChild,
    _In_opt_ HWND hWndNewParent);

WINUSERAPI
BOOL
WINAPI
EnumChildWindows(
    _In_opt_ HWND hWndParent,
    _In_ WNDENUMPROC lpEnumFunc,
    _In_ LPARAM lParam);


_Win32_metadata_set_last_error_
WINUSERAPI
HWND
WINAPI
FindWindowA(
    _In_opt_ LPCSTR lpClassName,
    _In_opt_ LPCSTR lpWindowName);
_Win32_metadata_set_last_error_
WINUSERAPI
HWND
WINAPI
FindWindowW(
    _In_opt_ LPCWSTR lpClassName,
    _In_opt_ LPCWSTR lpWindowName);
#ifdef UNICODE
#define FindWindow  FindWindowW
#else
#define FindWindow  FindWindowA
#endif // !UNICODE

#if(WINVER >= 0x0400)
_Win32_metadata_set_last_error_
WINUSERAPI
HWND
WINAPI
FindWindowExA(
    _In_opt_ HWND hWndParent,
    _In_opt_ HWND hWndChildAfter,
    _In_opt_ LPCSTR lpszClass,
    _In_opt_ LPCSTR lpszWindow);
_Win32_metadata_set_last_error_
WINUSERAPI
HWND
WINAPI
FindWindowExW(
    _In_opt_ HWND hWndParent,
    _In_opt_ HWND hWndChildAfter,
    _In_opt_ LPCWSTR lpszClass,
    _In_opt_ LPCWSTR lpszWindow);
#ifdef UNICODE
#define FindWindowEx  FindWindowExW
#else
#define FindWindowEx  FindWindowExA
#endif // !UNICODE

WINUSERAPI
HWND
WINAPI
GetShellWindow(
    VOID);

#endif /* WINVER >= 0x0400 */


WINUSERAPI
BOOL
WINAPI
RegisterShellHookWindow(
    _In_ HWND hwnd);

WINUSERAPI
BOOL
WINAPI
DeregisterShellHookWindow(
    _In_ HWND hwnd);

_Win32_metadata_set_last_error_
WINUSERAPI
BOOL
WINAPI
EnumWindows(
    _In_ WNDENUMPROC lpEnumFunc,
    _In_ LPARAM lParam);

WINUSERAPI
BOOL
WINAPI
EnumThreadWindows(
    _In_ DWORD dwThreadId,
    _In_ WNDENUMPROC lpfn,
    _In_ LPARAM lParam);


#define EnumTaskWindows(hTask, lpfn, lParam) EnumThreadWindows(HandleToUlong(hTask), lpfn, lParam)

#endif /* WINAPI_FAMILY_PARTITION(WINAPI_PARTITION_DESKTOP) */
#pragma endregion

#pragma region Desktop Family or Games Family
#if WINAPI_FAMILY_PARTITION(WINAPI_PARTITION_DESKTOP | WINAPI_PARTITION_GAMES)

_Win32_metadata_set_last_error_
WINUSERAPI
int
WINAPI
GetClassNameA(
    _In_ HWND hWnd,
    _Out_writes_to_(nMaxCount, return) LPSTR lpClassName,
    _In_ int nMaxCount
    );
_Win32_metadata_set_last_error_
WINUSERAPI
int
WINAPI
GetClassNameW(
    _In_ HWND hWnd,
    _Out_writes_to_(nMaxCount, return) LPWSTR lpClassName,
    _In_ int nMaxCount
    );
#ifdef UNICODE
#define GetClassName  GetClassNameW
#else
#define GetClassName  GetClassNameA
#endif // !UNICODE

#if defined(_M_CEE)
#undef GetClassName
__inline
int
GetClassName(
    HWND hWnd,
    LPTSTR lpClassName,
    int nMaxCount
    )
{
#ifdef UNICODE
    return GetClassNameW(
#else
    return GetClassNameA(
#endif
        hWnd,
    lpClassName,
    nMaxCount
        );
}
#endif  /* _M_CEE */

#endif /* WINAPI_FAMILY_PARTITION(WINAPI_PARTITION_DESKTOP | WINAPI_PARTITION_GAMES) */
#pragma endregion

#pragma region Desktop Family
#if WINAPI_FAMILY_PARTITION(WINAPI_PARTITION_DESKTOP)

_Win32_metadata_set_last_error_
WINUSERAPI
HWND
WINAPI
GetTopWindow(
    _In_opt_ HWND hWnd);

#define GetNextWindow(hWnd, wCmd) GetWindow(hWnd, wCmd)
#define GetSysModalWindow() (NULL)
#define SetSysModalWindow(hWnd) (NULL)

WINUSERAPI
DWORD
WINAPI
GetWindowThreadProcessId(
    _In_ HWND hWnd,
    _Out_opt_ LPDWORD lpdwProcessId);


#if(_WIN32_WINNT >= 0x0501)
WINUSERAPI
BOOL
WINAPI
IsGUIThread(
    _In_ BOOL bConvert);

#endif /* _WIN32_WINNT >= 0x0501 */


#define GetWindowTask(hWnd) \
        ((HANDLE)(DWORD_PTR)GetWindowThreadProcessId(hWnd, NULL))

WINUSERAPI
HWND
WINAPI
GetLastActivePopup(
    _In_ HWND hWnd);

/*
 * GetWindow() Constants
 */
#define GW_HWNDFIRST        0
#define GW_HWNDLAST         1
#define GW_HWNDNEXT         2
#define GW_HWNDPREV         3
#define GW_OWNER            4
#define GW_CHILD            5
#if(WINVER <= 0x0400)
#define GW_MAX              5
#else
#define GW_ENABLEDPOPUP     6
#define GW_MAX              6
#endif

_Win32_metadata_set_last_error_
WINUSERAPI
HWND
WINAPI
GetWindow(
    _In_ HWND hWnd,
    _In_ GET_WINDOW_CMD uCmd);


#ifndef NOWH

#ifdef STRICT

WINUSERAPI
HHOOK
WINAPI
SetWindowsHookA(
    _In_ int nFilterType,
    _In_ HOOKPROC pfnFilterProc);
WINUSERAPI
HHOOK
WINAPI
SetWindowsHookW(
    _In_ int nFilterType,
    _In_ HOOKPROC pfnFilterProc);
#ifdef UNICODE
#define SetWindowsHook  SetWindowsHookW
#else
#define SetWindowsHook  SetWindowsHookA
#endif // !UNICODE

#else /* !STRICT */

WINUSERAPI
HOOKPROC
WINAPI
SetWindowsHookA(
    _In_ int nFilterType,
    _In_ HOOKPROC pfnFilterProc);
WINUSERAPI
HOOKPROC
WINAPI
SetWindowsHookW(
    _In_ int nFilterType,
    _In_ HOOKPROC pfnFilterProc);
#ifdef UNICODE
#define SetWindowsHook  SetWindowsHookW
#else
#define SetWindowsHook  SetWindowsHookA
#endif // !UNICODE

#endif /* !STRICT */

WINUSERAPI
BOOL
WINAPI
UnhookWindowsHook(
    _In_ int nCode,
    _In_ HOOKPROC pfnFilterProc);

_Win32_metadata_set_last_error_
WINUSERAPI
HHOOK
WINAPI
SetWindowsHookExA(
    _In_ WINDOWS_HOOK_ID idHook,
    _In_ HOOKPROC lpfn,
    _In_opt_ HINSTANCE hmod,
    _In_ DWORD dwThreadId);
_Win32_metadata_set_last_error_
WINUSERAPI
HHOOK
WINAPI
SetWindowsHookExW(
    _In_ WINDOWS_HOOK_ID idHook,
    _In_ HOOKPROC lpfn,
    _In_opt_ HINSTANCE hmod,
    _In_ DWORD dwThreadId);
#ifdef UNICODE
#define SetWindowsHookEx  SetWindowsHookExW
#else
#define SetWindowsHookEx  SetWindowsHookExA
#endif // !UNICODE


_Win32_metadata_set_last_error_
WINUSERAPI
BOOL
WINAPI
UnhookWindowsHookEx(
    _In_ HHOOK hhk);

WINUSERAPI
LRESULT
WINAPI
CallNextHookEx(
    _In_opt_ HHOOK hhk,
    _In_ int nCode,
    _In_ WPARAM wParam,
    _In_ LPARAM lParam);

/*
 * Macros for source-level compatibility with old functions.
 */
#ifdef STRICT
#define DefHookProc(nCode, wParam, lParam, phhk)\
        CallNextHookEx(*phhk, nCode, wParam, lParam)
#else
#define DefHookProc(nCode, wParam, lParam, phhk)\
        CallNextHookEx((HHOOK)*phhk, nCode, wParam, lParam)
#endif /* STRICT */
#endif /* !NOWH */

#endif /* WINAPI_FAMILY_PARTITION(WINAPI_PARTITION_DESKTOP) */
#pragma endregion

#ifndef NOMENUS


/* ;win40  -- A lot of MF_* flags have been renamed as MFT_* and MFS_* flags */
/*
 * Menu flags for Add/Check/EnableMenuItem()
 */
#define MF_INSERT           0x00000000L
#define MF_CHANGE           0x00000080L
#define MF_APPEND           0x00000100L
#define MF_DELETE           0x00000200L
#define MF_REMOVE           0x00001000L

#define MF_BYCOMMAND        0x00000000L
#define MF_BYPOSITION       0x00000400L

#define MF_SEPARATOR        0x00000800L

#define MF_ENABLED          0x00000000L
#define MF_GRAYED           0x00000001L
#define MF_DISABLED         0x00000002L

#define MF_UNCHECKED        0x00000000L
#define MF_CHECKED          0x00000008L
#define MF_USECHECKBITMAPS  0x00000200L

#define MF_STRING           0x00000000L
#define MF_BITMAP           0x00000004L
#define MF_OWNERDRAW        0x00000100L

#define MF_POPUP            0x00000010L
#define MF_MENUBARBREAK     0x00000020L
#define MF_MENUBREAK        0x00000040L

#define MF_UNHILITE         0x00000000L
#define MF_HILITE           0x00000080L

#if(WINVER >= 0x0400)
#define MF_DEFAULT          0x00001000L
#endif /* WINVER >= 0x0400 */
#define MF_SYSMENU          0x00002000L
#define MF_HELP             0x00004000L
#if(WINVER >= 0x0400)
#define MF_RIGHTJUSTIFY     0x00004000L
#endif /* WINVER >= 0x0400 */

#define MF_MOUSESELECT      0x00008000L
#if(WINVER >= 0x0400)
#define MF_END              0x00000080L  /* Obsolete -- only used by old RES files */
#endif /* WINVER >= 0x0400 */


#if(WINVER >= 0x0400)
#define MFT_STRING          MF_STRING
#define MFT_BITMAP          MF_BITMAP
#define MFT_MENUBARBREAK    MF_MENUBARBREAK
#define MFT_MENUBREAK       MF_MENUBREAK
#define MFT_OWNERDRAW       MF_OWNERDRAW
#define MFT_RADIOCHECK      0x00000200L
#define MFT_SEPARATOR       MF_SEPARATOR
#define MFT_RIGHTORDER      0x00002000L
#define MFT_RIGHTJUSTIFY    MF_RIGHTJUSTIFY

/* Menu flags for Add/Check/EnableMenuItem() */
#define MFS_GRAYED          0x00000003L
#define MFS_DISABLED        MFS_GRAYED
#define MFS_CHECKED         MF_CHECKED
#define MFS_HILITE          MF_HILITE
#define MFS_ENABLED         MF_ENABLED
#define MFS_UNCHECKED       MF_UNCHECKED
#define MFS_UNHILITE        MF_UNHILITE
#define MFS_DEFAULT         MF_DEFAULT
#endif /* WINVER >= 0x0400 */


#pragma region Desktop Family
#if WINAPI_FAMILY_PARTITION(WINAPI_PARTITION_DESKTOP)

#if(WINVER >= 0x0400)

_Win32_metadata_set_last_error_
WINUSERAPI
BOOL
WINAPI
CheckMenuRadioItem(
    _In_ HMENU hmenu,
    _In_ UINT first,
    _In_ UINT last,
    _In_ UINT check,
    _In_ UINT flags);
#endif /* WINVER >= 0x0400 */

/*
 * Menu item resource format
 */
typedef struct {
    WORD versionNumber;
    WORD offset;
} MENUITEMTEMPLATEHEADER, *PMENUITEMTEMPLATEHEADER;

typedef struct {        // version 0
    WORD mtOption;
    WORD mtID;
    WCHAR mtString[1];
} MENUITEMTEMPLATE, *PMENUITEMTEMPLATE;
#define MF_END             0x00000080L

#endif /* WINAPI_FAMILY_PARTITION(WINAPI_PARTITION_DESKTOP) */
#pragma endregion

#endif /* !NOMENUS */

#ifndef NOSYSCOMMANDS

/*
 * System Menu Command Values
 */
#define SC_SIZE         0xF000
#define SC_MOVE         0xF010
#define SC_MINIMIZE     0xF020
#define SC_MAXIMIZE     0xF030
#define SC_NEXTWINDOW   0xF040
#define SC_PREVWINDOW   0xF050
#define SC_CLOSE        0xF060
#define SC_VSCROLL      0xF070
#define SC_HSCROLL      0xF080
#define SC_MOUSEMENU    0xF090
#define SC_KEYMENU      0xF100
#define SC_ARRANGE      0xF110
#define SC_RESTORE      0xF120
#define SC_TASKLIST     0xF130
#define SC_SCREENSAVE   0xF140
#define SC_HOTKEY       0xF150
#if(WINVER >= 0x0400)
#define SC_DEFAULT      0xF160
#define SC_MONITORPOWER 0xF170
#define SC_CONTEXTHELP  0xF180
#define SC_SEPARATOR    0xF00F
#endif /* WINVER >= 0x0400 */

#if(WINVER >= 0x0600)
#define SCF_ISSECURE    0x00000001
#endif /* WINVER >= 0x0600 */

#define GET_SC_WPARAM(wParam) ((int)wParam & 0xFFF0)

/*
 * Obsolete names
 */
#define SC_ICON         SC_MINIMIZE
#define SC_ZOOM         SC_MAXIMIZE

#endif /* !NOSYSCOMMANDS */

/*
 * Resource Loading Routines
 */

#pragma region Desktop Family
#if WINAPI_FAMILY_PARTITION(WINAPI_PARTITION_DESKTOP)

WINUSERAPI
HBITMAP
WINAPI
LoadBitmapA(
    _In_opt_ HINSTANCE hInstance,
    _In_ LPCSTR lpBitmapName);
WINUSERAPI
HBITMAP
WINAPI
LoadBitmapW(
    _In_opt_ HINSTANCE hInstance,
    _In_ LPCWSTR lpBitmapName);
#ifdef UNICODE
#define LoadBitmap  LoadBitmapW
#else
#define LoadBitmap  LoadBitmapA
#endif // !UNICODE

#endif /* WINAPI_FAMILY_PARTITION(WINAPI_PARTITION_DESKTOP) */
#pragma endregion

#pragma region Desktop or Games Family
#if WINAPI_FAMILY_PARTITION(WINAPI_PARTITION_DESKTOP | WINAPI_PARTITION_GAMES)

_Win32_metadata_set_last_error_
WINUSERAPI
HCURSOR
WINAPI
LoadCursorA(
    _In_opt_ HINSTANCE hInstance,
    _In_ LPCSTR lpCursorName);
_Win32_metadata_set_last_error_
WINUSERAPI
HCURSOR
WINAPI
LoadCursorW(
    _In_opt_ HINSTANCE hInstance,
    _In_ LPCWSTR lpCursorName);
#ifdef UNICODE
#define LoadCursor  LoadCursorW
#else
#define LoadCursor  LoadCursorA
#endif // !UNICODE

#endif /* WINAPI_FAMILY_PARTITION(WINAPI_PARTITION_DESKTOP | WINAPI_PARTITION_GAMES) */
#pragma endregion

#pragma region Desktop Family
#if WINAPI_FAMILY_PARTITION(WINAPI_PARTITION_DESKTOP)

_Win32_metadata_set_last_error_
WINUSERAPI
HCURSOR
WINAPI
LoadCursorFromFileA(
    _In_ LPCSTR lpFileName);
_Win32_metadata_set_last_error_
WINUSERAPI
HCURSOR
WINAPI
LoadCursorFromFileW(
    _In_ LPCWSTR lpFileName);
#ifdef UNICODE
#define LoadCursorFromFile  LoadCursorFromFileW
#else
#define LoadCursorFromFile  LoadCursorFromFileA
#endif // !UNICODE

#endif /* WINAPI_FAMILY_PARTITION(WINAPI_PARTITION_DESKTOP) */
#pragma endregion

#pragma region Desktop or Games Family
#if WINAPI_FAMILY_PARTITION(WINAPI_PARTITION_DESKTOP | WINAPI_PARTITION_GAMES)

_Win32_metadata_set_last_error_
WINUSERAPI
HCURSOR
WINAPI
CreateCursor(
    _In_opt_ HINSTANCE hInst,
    _In_ int xHotSpot,
    _In_ int yHotSpot,
    _In_ int nWidth,
    _In_ int nHeight,
    _In_ CONST VOID *pvANDPlane,
    _In_ CONST VOID *pvXORPlane);

_Win32_metadata_set_last_error_
WINUSERAPI
BOOL
WINAPI
DestroyCursor(
    _In_ HCURSOR hCursor);

#endif /* WINAPI_FAMILY_PARTITION(WINAPI_PARTITION_DESKTOP | WINAPI_PARTITION_GAMES) */
#pragma endregion

#pragma region Desktop Family
#if WINAPI_FAMILY_PARTITION(WINAPI_PARTITION_DESKTOP)

#ifndef _MAC
#define CopyCursor(pcur) ((HCURSOR)CopyIcon((HICON)(pcur)))
#else
WINUSERAPI
HCURSOR
WINAPI
CopyCursor(
    _In_ HCURSOR hCursor);
#endif

#endif /* WINAPI_FAMILY_PARTITION(WINAPI_PARTITION_DESKTOP) */
#pragma endregion

/*
 * Standard Cursor IDs
 */
#define IDC_ARROW           MAKEINTRESOURCE(32512)
#define IDC_IBEAM           MAKEINTRESOURCE(32513)
#define IDC_WAIT            MAKEINTRESOURCE(32514)
#define IDC_CROSS           MAKEINTRESOURCE(32515)
#define IDC_UPARROW         MAKEINTRESOURCE(32516)
#define IDC_SIZE            MAKEINTRESOURCE(32640)  /* OBSOLETE: use IDC_SIZEALL */
#define IDC_ICON            MAKEINTRESOURCE(32641)  /* OBSOLETE: use IDC_ARROW */
#define IDC_SIZENWSE        MAKEINTRESOURCE(32642)
#define IDC_SIZENESW        MAKEINTRESOURCE(32643)
#define IDC_SIZEWE          MAKEINTRESOURCE(32644)
#define IDC_SIZENS          MAKEINTRESOURCE(32645)
#define IDC_SIZEALL         MAKEINTRESOURCE(32646)
#define IDC_NO              MAKEINTRESOURCE(32648) /*not in win3.1 */
#if(WINVER >= 0x0500)
#define IDC_HAND            MAKEINTRESOURCE(32649)
#endif /* WINVER >= 0x0500 */
#define IDC_APPSTARTING     MAKEINTRESOURCE(32650) /*not in win3.1 */
#if(WINVER >= 0x0400)
#define IDC_HELP            MAKEINTRESOURCE(32651)
#endif /* WINVER >= 0x0400 */

#if(WINVER >= 0x0606)
#define IDC_PIN            MAKEINTRESOURCE(32671)
#define IDC_PERSON         MAKEINTRESOURCE(32672)
#endif /* WINVER >= 0x0606 */

#pragma region Desktop Family
#if WINAPI_FAMILY_PARTITION(WINAPI_PARTITION_DESKTOP)

_Win32_metadata_set_last_error_
WINUSERAPI
BOOL
WINAPI
SetSystemCursor(
    _In_ HCURSOR hcur,
    _In_ SYSTEM_CURSOR_ID id);

typedef struct _ICONINFO {
    BOOL    fIcon;
    DWORD   xHotspot;
    DWORD   yHotspot;
    HBITMAP hbmMask;
    HBITMAP hbmColor;
} ICONINFO;
typedef ICONINFO *PICONINFO;

_Win32_metadata_set_last_error_
WINUSERAPI
HICON
WINAPI
LoadIconA(
    _In_opt_ HINSTANCE hInstance,
    _In_ LPCSTR lpIconName);
_Win32_metadata_set_last_error_
WINUSERAPI
HICON
WINAPI
LoadIconW(
    _In_opt_ HINSTANCE hInstance,
    _In_ LPCWSTR lpIconName);
#ifdef UNICODE
#define LoadIcon  LoadIconW
#else
#define LoadIcon  LoadIconA
#endif // !UNICODE


WINUSERAPI
UINT
WINAPI
PrivateExtractIconsA(
    _In_reads_(MAX_PATH) LPCSTR szFileName,
    _In_ int nIconIndex,
    _In_ int cxIcon,
    _In_ int cyIcon,
    _Out_writes_opt_(nIcons) HICON *phicon,
    _Out_writes_opt_(nIcons) UINT *piconid,
    _In_ UINT nIcons,
    _In_ UINT flags);
WINUSERAPI
UINT
WINAPI
PrivateExtractIconsW(
    _In_reads_(MAX_PATH) LPCWSTR szFileName,
    _In_ int nIconIndex,
    _In_ int cxIcon,
    _In_ int cyIcon,
    _Out_writes_opt_(nIcons) HICON *phicon,
    _Out_writes_opt_(nIcons) UINT *piconid,
    _In_ UINT nIcons,
    _In_ UINT flags);
#ifdef UNICODE
#define PrivateExtractIcons  PrivateExtractIconsW
#else
#define PrivateExtractIcons  PrivateExtractIconsA
#endif // !UNICODE

_Win32_metadata_set_last_error_
WINUSERAPI
HICON
WINAPI
CreateIcon(
    _In_opt_ HINSTANCE hInstance,
    _In_ int nWidth,
    _In_ int nHeight,
    _In_ BYTE cPlanes,
    _In_ BYTE cBitsPixel,
    _In_ CONST BYTE *lpbANDbits,
    _In_ CONST BYTE *lpbXORbits);

_Win32_metadata_set_last_error_
WINUSERAPI
BOOL
WINAPI
DestroyIcon(
    _In_ HICON hIcon);

_Win32_metadata_set_last_error_
WINUSERAPI
int
WINAPI
LookupIconIdFromDirectory(
    _In_reads_bytes_(sizeof(WORD) * 3) PBYTE presbits,
    _In_ BOOL fIcon);

#if(WINVER >= 0x0400)
_Win32_metadata_set_last_error_
WINUSERAPI
int
WINAPI
LookupIconIdFromDirectoryEx(
    _In_reads_bytes_(sizeof(WORD) * 3) PBYTE presbits,
    _In_ BOOL fIcon,
    _In_ int cxDesired,
    _In_ int cyDesired,
    _In_ UINT Flags);
#endif /* WINVER >= 0x0400 */

_Win32_metadata_set_last_error_
WINUSERAPI
HICON
WINAPI
CreateIconFromResource(
    _In_reads_bytes_(dwResSize) PBYTE presbits,
    _In_ DWORD dwResSize,
    _In_ BOOL fIcon,
    _In_ DWORD dwVer);

#if(WINVER >= 0x0400)
_Win32_metadata_set_last_error_
WINUSERAPI
HICON
WINAPI
CreateIconFromResourceEx(
    _In_reads_bytes_(dwResSize) PBYTE presbits,
    _In_ DWORD dwResSize,
    _In_ BOOL fIcon,
    _In_ DWORD dwVer,
    _In_ int cxDesired,
    _In_ int cyDesired,
    _In_ UINT Flags);

/* Icon/Cursor header */
typedef struct tagCURSORSHAPE
{
    int     xHotSpot;
    int     yHotSpot;
    int     cx;
    int     cy;
    int     cbWidth;
    BYTE    Planes;
    BYTE    BitsPixel;
} CURSORSHAPE, FAR *LPCURSORSHAPE;
#endif /* WINVER >= 0x0400 */

#endif /* WINAPI_FAMILY_PARTITION(WINAPI_PARTITION_DESKTOP) */
#pragma endregion

#pragma region Desktop or Games Family
#if WINAPI_FAMILY_PARTITION(WINAPI_PARTITION_DESKTOP | WINAPI_PARTITION_GAMES)
#if (NTDDI_VERSION >= NTDDI_WIN10_CO)
#define CURSOR_CREATION_SCALING_NONE    1
#define CURSOR_CREATION_SCALING_DEFAULT 2

WINUSERAPI
UINT
WINAPI
SetThreadCursorCreationScaling(
    UINT cursorDpi);
#endif /* NTDDI_VERSION >= NTDDI_WIN10_CO */
#endif /* WINAPI_FAMILY_PARTITION(WINAPI_PARTITION_DESKTOP | WINAPI_PARTITION_GAMES) */
#pragma endregion

#define IMAGE_BITMAP        0
#define IMAGE_ICON          1
#define IMAGE_CURSOR        2
#if(WINVER >= 0x0400)
#define IMAGE_ENHMETAFILE   3

#define LR_DEFAULTCOLOR     0x00000000
#define LR_MONOCHROME       0x00000001
#define LR_COLOR            0x00000002
#define LR_COPYRETURNORG    0x00000004
#define LR_COPYDELETEORG    0x00000008
#define LR_LOADFROMFILE     0x00000010
#define LR_LOADTRANSPARENT  0x00000020
#define LR_DEFAULTSIZE      0x00000040
#define LR_VGACOLOR         0x00000080
#define LR_LOADMAP3DCOLORS  0x00001000
#define LR_CREATEDIBSECTION 0x00002000
#define LR_COPYFROMRESOURCE 0x00004000
#define LR_SHARED           0x00008000

#pragma region Desktop Family
#if WINAPI_FAMILY_PARTITION(WINAPI_PARTITION_DESKTOP)

_Win32_metadata_set_last_error_
WINUSERAPI
HANDLE
WINAPI
LoadImageA(
    _In_opt_ HINSTANCE hInst,
    _In_ LPCSTR name,
    _In_ UINT type,
    _In_ int cx,
    _In_ int cy,
    _In_ UINT fuLoad);
_Win32_metadata_set_last_error_
WINUSERAPI
HANDLE
WINAPI
LoadImageW(
    _In_opt_ HINSTANCE hInst,
    _In_ LPCWSTR name,
    _In_ UINT type,
    _In_ int cx,
    _In_ int cy,
    _In_ UINT fuLoad);
#ifdef UNICODE
#define LoadImage  LoadImageW
#else
#define LoadImage  LoadImageA
#endif // !UNICODE

_Win32_metadata_set_last_error_
WINUSERAPI
HANDLE
WINAPI
CopyImage(
    _In_ HANDLE h,
    _In_ UINT type,
    _In_ int cx,
    _In_ int cy,
    _In_ UINT flags);

#define DI_MASK         0x0001
#define DI_IMAGE        0x0002
#define DI_NORMAL       0x0003
#define DI_COMPAT       0x0004
#define DI_DEFAULTSIZE  0x0008
#if(_WIN32_WINNT >= 0x0501)
#define DI_NOMIRROR     0x0010
#endif /* _WIN32_WINNT >= 0x0501 */

_Win32_metadata_set_last_error_
WINUSERAPI BOOL WINAPI DrawIconEx(
    _In_ HDC hdc,
    _In_ int xLeft,
    _In_ int yTop,
    _In_ HICON hIcon,
    _In_ int cxWidth,
    _In_ int cyWidth,
    _In_ UINT istepIfAniCur,
    _In_opt_ HBRUSH hbrFlickerFreeDraw,
    _In_ UINT diFlags);

#endif /* WINAPI_FAMILY_PARTITION(WINAPI_PARTITION_DESKTOP) */
#pragma endregion

#endif /* WINVER >= 0x0400 */

#pragma region Desktop Family
#if WINAPI_FAMILY_PARTITION(WINAPI_PARTITION_DESKTOP)

_Win32_metadata_set_last_error_
WINUSERAPI
HICON
WINAPI
CreateIconIndirect(
    _In_ PICONINFO piconinfo);

_Win32_metadata_set_last_error_
WINUSERAPI
HICON
WINAPI
CopyIcon(
    _In_ HICON hIcon);

_Win32_metadata_set_last_error_
WINUSERAPI
BOOL
WINAPI
GetIconInfo(
    _In_ HICON hIcon,
    _Out_ PICONINFO piconinfo);

#if(_WIN32_WINNT >= 0x0600)
typedef struct _ICONINFOEXA {
    DWORD   cbSize;
    BOOL    fIcon;
    DWORD   xHotspot;
    DWORD   yHotspot;
    HBITMAP hbmMask;
    HBITMAP hbmColor;
    WORD    wResID;
    CHAR    szModName[MAX_PATH];
    CHAR    szResName[MAX_PATH];
} ICONINFOEXA, *PICONINFOEXA;
typedef struct _ICONINFOEXW {
    DWORD   cbSize;
    BOOL    fIcon;
    DWORD   xHotspot;
    DWORD   yHotspot;
    HBITMAP hbmMask;
    HBITMAP hbmColor;
    WORD    wResID;
    WCHAR   szModName[MAX_PATH];
    WCHAR   szResName[MAX_PATH];
} ICONINFOEXW, *PICONINFOEXW;
#ifdef UNICODE
typedef ICONINFOEXW ICONINFOEX;
typedef PICONINFOEXW PICONINFOEX;
#else
typedef ICONINFOEXA ICONINFOEX;
typedef PICONINFOEXA PICONINFOEX;
#endif // UNICODE

WINUSERAPI
BOOL
WINAPI
GetIconInfoExA(
    _In_ HICON hicon,
    _Inout_ PICONINFOEXA piconinfo);
WINUSERAPI
BOOL
WINAPI
GetIconInfoExW(
    _In_ HICON hicon,
    _Inout_ PICONINFOEXW piconinfo);
#ifdef UNICODE
#define GetIconInfoEx  GetIconInfoExW
#else
#define GetIconInfoEx  GetIconInfoExA
#endif // !UNICODE
#endif /* _WIN32_WINNT >= 0x0600 */

#if(WINVER >= 0x0400)
#define RES_ICON    1
#define RES_CURSOR  2
#endif /* WINVER >= 0x0400 */

#endif /* WINAPI_FAMILY_PARTITION(WINAPI_PARTITION_DESKTOP) */
#pragma endregion

#ifdef OEMRESOURCE


/*
 * OEM Resource Ordinal Numbers
 */
#define OBM_CLOSE           32754
#define OBM_UPARROW         32753
#define OBM_DNARROW         32752
#define OBM_RGARROW         32751
#define OBM_LFARROW         32750
#define OBM_REDUCE          32749
#define OBM_ZOOM            32748
#define OBM_RESTORE         32747
#define OBM_REDUCED         32746
#define OBM_ZOOMD           32745
#define OBM_RESTORED        32744
#define OBM_UPARROWD        32743
#define OBM_DNARROWD        32742
#define OBM_RGARROWD        32741
#define OBM_LFARROWD        32740
#define OBM_MNARROW         32739
#define OBM_COMBO           32738
#define OBM_UPARROWI        32737
#define OBM_DNARROWI        32736
#define OBM_RGARROWI        32735
#define OBM_LFARROWI        32734

#define OBM_OLD_CLOSE       32767
#define OBM_SIZE            32766
#define OBM_OLD_UPARROW     32765
#define OBM_OLD_DNARROW     32764
#define OBM_OLD_RGARROW     32763
#define OBM_OLD_LFARROW     32762
#define OBM_BTSIZE          32761
#define OBM_CHECK           32760
#define OBM_CHECKBOXES      32759
#define OBM_BTNCORNERS      32758
#define OBM_OLD_REDUCE      32757
#define OBM_OLD_ZOOM        32756
#define OBM_OLD_RESTORE     32755


#define OCR_NORMAL          32512
#define OCR_IBEAM           32513
#define OCR_WAIT            32514
#define OCR_CROSS           32515
#define OCR_UP              32516
#define OCR_SIZE            32640   /* OBSOLETE: use OCR_SIZEALL */
#define OCR_ICON            32641   /* OBSOLETE: use OCR_NORMAL */
#define OCR_SIZENWSE        32642
#define OCR_SIZENESW        32643
#define OCR_SIZEWE          32644
#define OCR_SIZENS          32645
#define OCR_SIZEALL         32646
#define OCR_ICOCUR          32647   /* OBSOLETE: use OIC_WINLOGO */
#define OCR_NO              32648
#if(WINVER >= 0x0500)
#define OCR_HAND            32649
#endif /* WINVER >= 0x0500 */
#if(WINVER >= 0x0400)
#define OCR_APPSTARTING     32650
#endif /* WINVER >= 0x0400 */


#define OIC_SAMPLE          32512
#define OIC_HAND            32513
#define OIC_QUES            32514
#define OIC_BANG            32515
#define OIC_NOTE            32516
#if(WINVER >= 0x0400)
#define OIC_WINLOGO         32517
#define OIC_WARNING         OIC_BANG
#define OIC_ERROR           OIC_HAND
#define OIC_INFORMATION     OIC_NOTE
#endif /* WINVER >= 0x0400 */
#if(WINVER >= 0x0600)
#define OIC_SHIELD          32518
#endif /* WINVER >= 0x0600 */



#endif /* OEMRESOURCE */


// The ordinal number for the entry point of language drivers.
#define ORD_LANGDRIVER    1

#ifndef NOICONS

/*
 * Standard Icon IDs
 */
#ifdef RC_INVOKED
#define IDI_APPLICATION     32512
#define IDI_HAND            32513
#define IDI_QUESTION        32514
#define IDI_EXCLAMATION     32515
#define IDI_ASTERISK        32516
#if(WINVER >= 0x0400)
#define IDI_WINLOGO         32517
#endif /* WINVER >= 0x0400 */
#if(WINVER >= 0x0600)
#define IDI_SHIELD          32518
#endif /* WINVER >= 0x0600 */
#else
#define IDI_APPLICATION     MAKEINTRESOURCE(32512)
#define IDI_HAND            MAKEINTRESOURCE(32513)
#define IDI_QUESTION        MAKEINTRESOURCE(32514)
#define IDI_EXCLAMATION     MAKEINTRESOURCE(32515)
#define IDI_ASTERISK        MAKEINTRESOURCE(32516)
#if(WINVER >= 0x0400)
#define IDI_WINLOGO         MAKEINTRESOURCE(32517)
#endif /* WINVER >= 0x0400 */
#if(WINVER >= 0x0600)
#define IDI_SHIELD          MAKEINTRESOURCE(32518)
#endif /* WINVER >= 0x0600 */
#endif /* RC_INVOKED */

#if(WINVER >= 0x0400)
#define IDI_WARNING     IDI_EXCLAMATION
#define IDI_ERROR       IDI_HAND
#define IDI_INFORMATION IDI_ASTERISK
#endif /* WINVER >= 0x0400 */


#endif /* !NOICONS */


#ifdef NOAPISET

#pragma region Desktop Family
#if WINAPI_FAMILY_PARTITION(WINAPI_PARTITION_DESKTOP)

_Win32_metadata_set_last_error_
WINUSERAPI
int
WINAPI
LoadStringA(
    _In_opt_ HINSTANCE hInstance,
    _In_ UINT uID,
    _Out_writes_to_(cchBufferMax, return + 1) LPSTR lpBuffer,
    _In_ int cchBufferMax);
_Win32_metadata_set_last_error_
WINUSERAPI
int
WINAPI
LoadStringW(
    _In_opt_ HINSTANCE hInstance,
    _In_ UINT uID,
    _Out_writes_to_(cchBufferMax, return + 1) LPWSTR lpBuffer,
    _In_ int cchBufferMax);
#ifdef UNICODE
#define LoadString  LoadStringW
#else
#define LoadString  LoadStringA
#endif // !UNICODE

#endif /* WINAPI_FAMILY_PARTITION(WINAPI_PARTITION_DESKTOP) */
#pragma endregion

#endif


/*
 * Dialog Box Command IDs
 */
#define IDOK                1
#define IDCANCEL            2
#define IDABORT             3
#define IDRETRY             4
#define IDIGNORE            5
#define IDYES               6
#define IDNO                7
#if(WINVER >= 0x0400)
#define IDCLOSE         8
#define IDHELP          9
#endif /* WINVER >= 0x0400 */

#if(WINVER >= 0x0500)
#define IDTRYAGAIN      10
#define IDCONTINUE      11
#endif /* WINVER >= 0x0500 */

#if(WINVER >= 0x0501)
#ifndef IDTIMEOUT
#define IDTIMEOUT 32000
#endif
#endif /* WINVER >= 0x0501 */


#ifndef NOCTLMGR

/*
 * Control Manager Structures and Definitions
 */

#ifndef NOWINSTYLES


/*
 * Edit Control Styles
 */
#define ES_LEFT             0x0000L
#define ES_CENTER           0x0001L
#define ES_RIGHT            0x0002L
#define ES_MULTILINE        0x0004L
#define ES_UPPERCASE        0x0008L
#define ES_LOWERCASE        0x0010L
#define ES_PASSWORD         0x0020L
#define ES_AUTOVSCROLL      0x0040L
#define ES_AUTOHSCROLL      0x0080L
#define ES_NOHIDESEL        0x0100L
#define ES_OEMCONVERT       0x0400L
#define ES_READONLY         0x0800L
#define ES_WANTRETURN       0x1000L
#if(WINVER >= 0x0400)
#define ES_NUMBER           0x2000L
#endif /* WINVER >= 0x0400 */


#endif /* !NOWINSTYLES */

/*
 * Edit Control Notification Codes
 */
#define EN_SETFOCUS         0x0100
#define EN_KILLFOCUS        0x0200
#define EN_CHANGE           0x0300
#define EN_UPDATE           0x0400
#define EN_ERRSPACE         0x0500
#define EN_MAXTEXT          0x0501
#define EN_HSCROLL          0x0601
#define EN_VSCROLL          0x0602

#if(_WIN32_WINNT >= 0x0500)
#define EN_ALIGN_LTR_EC     0x0700
#define EN_ALIGN_RTL_EC     0x0701
#endif /* _WIN32_WINNT >= 0x0500 */

#if(WINVER >= 0x0604)
#define EN_BEFORE_PASTE     0x0800
#define EN_AFTER_PASTE      0x0801
#endif /* WINVER >= 0x0604 */

#if(WINVER >= 0x0400)
/* Edit control EM_SETMARGIN parameters */
#define EC_LEFTMARGIN       0x0001
#define EC_RIGHTMARGIN      0x0002
#define EC_USEFONTINFO      0xffff
#endif /* WINVER >= 0x0400 */

#if(WINVER >= 0x0500)
/* wParam of EM_GET/SETIMESTATUS  */
#define EMSIS_COMPOSITIONSTRING        0x0001

/* lParam for EMSIS_COMPOSITIONSTRING  */
#define EIMES_GETCOMPSTRATONCE         0x0001
#define EIMES_CANCELCOMPSTRINFOCUS     0x0002
#define EIMES_COMPLETECOMPSTRKILLFOCUS 0x0004
#endif /* WINVER >= 0x0500 */

#ifndef NOWINMESSAGES


/*
 * Edit Control Messages
 */
#define EM_GETSEL               0x00B0
#define EM_SETSEL               0x00B1
#define EM_GETRECT              0x00B2
#define EM_SETRECT              0x00B3
#define EM_SETRECTNP            0x00B4
#define EM_SCROLL               0x00B5
#define EM_LINESCROLL           0x00B6
#define EM_SCROLLCARET          0x00B7
#define EM_GETMODIFY            0x00B8
#define EM_SETMODIFY            0x00B9
#define EM_GETLINECOUNT         0x00BA
#define EM_LINEINDEX            0x00BB
#define EM_SETHANDLE            0x00BC
#define EM_GETHANDLE            0x00BD
#define EM_GETTHUMB             0x00BE
#define EM_LINELENGTH           0x00C1
#define EM_REPLACESEL           0x00C2
#define EM_GETLINE              0x00C4
#define EM_LIMITTEXT            0x00C5
#define EM_CANUNDO              0x00C6
#define EM_UNDO                 0x00C7
#define EM_FMTLINES             0x00C8
#define EM_LINEFROMCHAR         0x00C9
#define EM_SETTABSTOPS          0x00CB
#define EM_SETPASSWORDCHAR      0x00CC
#define EM_EMPTYUNDOBUFFER      0x00CD
#define EM_GETFIRSTVISIBLELINE  0x00CE
#define EM_SETREADONLY          0x00CF
#define EM_SETWORDBREAKPROC     0x00D0
#define EM_GETWORDBREAKPROC     0x00D1
#define EM_GETPASSWORDCHAR      0x00D2
#if(WINVER >= 0x0400)
#define EM_SETMARGINS           0x00D3
#define EM_GETMARGINS           0x00D4
#define EM_SETLIMITTEXT         EM_LIMITTEXT   /* ;win40 Name change */
#define EM_GETLIMITTEXT         0x00D5
#define EM_POSFROMCHAR          0x00D6
#define EM_CHARFROMPOS          0x00D7
#endif /* WINVER >= 0x0400 */

#if(WINVER >= 0x0500)
#define EM_SETIMESTATUS         0x00D8
#define EM_GETIMESTATUS         0x00D9
#endif /* WINVER >= 0x0500 */

#if(WINVER >= 0x0604)
#define EM_ENABLEFEATURE        0x00DA
#endif /* WINVER >= 0x0604 */


#endif /* !NOWINMESSAGES */

#if(WINVER >= 0x0604)
/*
 * EM_ENABLEFEATURE options
 */
typedef enum {
    EDIT_CONTROL_FEATURE_ENTERPRISE_DATA_PROTECTION_PASTE_SUPPORT  = 0,
    EDIT_CONTROL_FEATURE_PASTE_NOTIFICATIONS                       = 1,
} EDIT_CONTROL_FEATURE;
#endif /* WINVER >= 0x0604 */

/*
 * EDITWORDBREAKPROC code values
 */
#define WB_LEFT            0
#define WB_RIGHT           1
#define WB_ISDELIMITER     2


/*
 * Button Control Styles
 */
#define BS_PUSHBUTTON       0x00000000L
#define BS_DEFPUSHBUTTON    0x00000001L
#define BS_CHECKBOX         0x00000002L
#define BS_AUTOCHECKBOX     0x00000003L
#define BS_RADIOBUTTON      0x00000004L
#define BS_3STATE           0x00000005L
#define BS_AUTO3STATE       0x00000006L
#define BS_GROUPBOX         0x00000007L
#define BS_USERBUTTON       0x00000008L
#define BS_AUTORADIOBUTTON  0x00000009L
#define BS_PUSHBOX          0x0000000AL
#define BS_OWNERDRAW        0x0000000BL
#define BS_TYPEMASK         0x0000000FL
#define BS_LEFTTEXT         0x00000020L
#if(WINVER >= 0x0400)
#define BS_TEXT             0x00000000L
#define BS_ICON             0x00000040L
#define BS_BITMAP           0x00000080L
#define BS_LEFT             0x00000100L
#define BS_RIGHT            0x00000200L
#define BS_CENTER           0x00000300L
#define BS_TOP              0x00000400L
#define BS_BOTTOM           0x00000800L
#define BS_VCENTER          0x00000C00L
#define BS_PUSHLIKE         0x00001000L
#define BS_MULTILINE        0x00002000L
#define BS_NOTIFY           0x00004000L
#define BS_FLAT             0x00008000L
#define BS_RIGHTBUTTON      BS_LEFTTEXT
#endif /* WINVER >= 0x0400 */

/*
 * User Button Notification Codes
 */
#define BN_CLICKED          0
#define BN_PAINT            1
#define BN_HILITE           2
#define BN_UNHILITE         3
#define BN_DISABLE          4
#define BN_DOUBLECLICKED    5
#if(WINVER >= 0x0400)
#define BN_PUSHED           BN_HILITE
#define BN_UNPUSHED         BN_UNHILITE
#define BN_DBLCLK           BN_DOUBLECLICKED
#define BN_SETFOCUS         6
#define BN_KILLFOCUS        7
#endif /* WINVER >= 0x0400 */

/*
 * Button Control Messages
 */
#define BM_GETCHECK        0x00F0
#define BM_SETCHECK        0x00F1
#define BM_GETSTATE        0x00F2
#define BM_SETSTATE        0x00F3
#define BM_SETSTYLE        0x00F4
#if(WINVER >= 0x0400)
#define BM_CLICK           0x00F5
#define BM_GETIMAGE        0x00F6
#define BM_SETIMAGE        0x00F7
#endif /* WINVER >= 0x0400 */
#if(WINVER >= 0x0600)
#define BM_SETDONTCLICK    0x00F8
#endif /* WINVER >= 0x0600 */

#if(WINVER >= 0x0400)
#define BST_UNCHECKED      0x0000
#define BST_CHECKED        0x0001
#define BST_INDETERMINATE  0x0002
#define BST_PUSHED         0x0004
#define BST_FOCUS          0x0008
#endif /* WINVER >= 0x0400 */

/*
 * Static Control Constants
 */
#define SS_LEFT             0x00000000L
#define SS_CENTER           0x00000001L
#define SS_RIGHT            0x00000002L
#define SS_ICON             0x00000003L
#define SS_BLACKRECT        0x00000004L
#define SS_GRAYRECT         0x00000005L
#define SS_WHITERECT        0x00000006L
#define SS_BLACKFRAME       0x00000007L
#define SS_GRAYFRAME        0x00000008L
#define SS_WHITEFRAME       0x00000009L
#define SS_USERITEM         0x0000000AL
#define SS_SIMPLE           0x0000000BL
#define SS_LEFTNOWORDWRAP   0x0000000CL
#if(WINVER >= 0x0400)
#define SS_OWNERDRAW        0x0000000DL
#define SS_BITMAP           0x0000000EL
#define SS_ENHMETAFILE      0x0000000FL
#define SS_ETCHEDHORZ       0x00000010L
#define SS_ETCHEDVERT       0x00000011L
#define SS_ETCHEDFRAME      0x00000012L
#define SS_TYPEMASK         0x0000001FL
#endif /* WINVER >= 0x0400 */
#if(WINVER >= 0x0501)
#define SS_REALSIZECONTROL  0x00000040L
#endif /* WINVER >= 0x0501 */
#define SS_NOPREFIX         0x00000080L /* Don't do "&" character translation */
#if(WINVER >= 0x0400)
#define SS_NOTIFY           0x00000100L
#define SS_CENTERIMAGE      0x00000200L
#define SS_RIGHTJUST        0x00000400L
#define SS_REALSIZEIMAGE    0x00000800L
#define SS_SUNKEN           0x00001000L
#define SS_EDITCONTROL      0x00002000L
#define SS_ENDELLIPSIS      0x00004000L
#define SS_PATHELLIPSIS     0x00008000L
#define SS_WORDELLIPSIS     0x0000C000L
#define SS_ELLIPSISMASK     0x0000C000L
#endif /* WINVER >= 0x0400 */



#ifndef NOWINMESSAGES
/*
 * Static Control Mesages
 */
#define STM_SETICON         0x0170
#define STM_GETICON         0x0171
#if(WINVER >= 0x0400)
#define STM_SETIMAGE        0x0172
#define STM_GETIMAGE        0x0173
#define STN_CLICKED         0
#define STN_DBLCLK          1
#define STN_ENABLE          2
#define STN_DISABLE         3
#endif /* WINVER >= 0x0400 */
#define STM_MSGMAX          0x0174
#endif /* !NOWINMESSAGES */

/*
 * Dialog window class
 */
#define WC_DIALOG       (MAKEINTATOM(0x8002))

/*
 * Get/SetWindowWord/Long offsets for use with WC_DIALOG windows
 */
#define DWL_MSGRESULT   0
#define DWL_DLGPROC     4
#define DWL_USER        8

#ifdef _WIN64

#undef DWL_MSGRESULT
#undef DWL_DLGPROC
#undef DWL_USER

#endif /* _WIN64 */

#define DWLP_MSGRESULT  0
#define DWLP_DLGPROC    DWLP_MSGRESULT + sizeof(LRESULT)
#define DWLP_USER       DWLP_DLGPROC + sizeof(DLGPROC)

#pragma region Desktop Family
#if WINAPI_FAMILY_PARTITION(WINAPI_PARTITION_DESKTOP)

/*
 * Dialog Manager Routines
 */

#ifndef NOMSG

WINUSERAPI
BOOL
WINAPI
IsDialogMessageA(
    _In_ HWND hDlg,
    _In_ LPMSG lpMsg);
WINUSERAPI
BOOL
WINAPI
IsDialogMessageW(
    _In_ HWND hDlg,
    _In_ LPMSG lpMsg);
#ifdef UNICODE
#define IsDialogMessage  IsDialogMessageW
#else
#define IsDialogMessage  IsDialogMessageA
#endif // !UNICODE

#endif /* !NOMSG */

_Win32_metadata_set_last_error_
WINUSERAPI
BOOL
WINAPI
MapDialogRect(
    _In_ HWND hDlg,
    _Inout_ LPRECT lpRect);

WINUSERAPI
int
WINAPI
DlgDirListA(
    _In_ HWND hDlg,
    _Inout_ LPSTR lpPathSpec,
    _In_ int nIDListBox,
    _In_ int nIDStaticPath,
    _In_ DLG_DIR_LIST_FILE_TYPE uFileType);
WINUSERAPI
int
WINAPI
DlgDirListW(
    _In_ HWND hDlg,
    _Inout_ LPWSTR lpPathSpec,
    _In_ int nIDListBox,
    _In_ int nIDStaticPath,
    _In_ DLG_DIR_LIST_FILE_TYPE uFileType);
#ifdef UNICODE
#define DlgDirList  DlgDirListW
#else
#define DlgDirList  DlgDirListA
#endif // !UNICODE

#endif /* WINAPI_FAMILY_PARTITION(WINAPI_PARTITION_DESKTOP) */
#pragma endregion

/*
 * DlgDirList, DlgDirListComboBox flags values
 */
#define DDL_READWRITE       0x0000
#define DDL_READONLY        0x0001
#define DDL_HIDDEN          0x0002
#define DDL_SYSTEM          0x0004
#define DDL_DIRECTORY       0x0010
#define DDL_ARCHIVE         0x0020

#define DDL_POSTMSGS        0x2000
#define DDL_DRIVES          0x4000
#define DDL_EXCLUSIVE       0x8000

#pragma region Desktop Family
#if WINAPI_FAMILY_PARTITION(WINAPI_PARTITION_DESKTOP)

_Win32_metadata_set_last_error_
WINUSERAPI
BOOL
WINAPI
DlgDirSelectExA(
    _In_ HWND hwndDlg,
    _Out_writes_(chCount) LPSTR lpString,
    _In_ int chCount,
    _In_ int idListBox);
_Win32_metadata_set_last_error_
WINUSERAPI
BOOL
WINAPI
DlgDirSelectExW(
    _In_ HWND hwndDlg,
    _Out_writes_(chCount) LPWSTR lpString,
    _In_ int chCount,
    _In_ int idListBox);
#ifdef UNICODE
#define DlgDirSelectEx  DlgDirSelectExW
#else
#define DlgDirSelectEx  DlgDirSelectExA
#endif // !UNICODE

_Win32_metadata_set_last_error_
WINUSERAPI
int
WINAPI
DlgDirListComboBoxA(
    _In_ HWND hDlg,
    _Inout_ LPSTR lpPathSpec,
    _In_ int nIDComboBox,
    _In_ int nIDStaticPath,
    _In_ DLG_DIR_LIST_FILE_TYPE uFiletype);
_Win32_metadata_set_last_error_
WINUSERAPI
int
WINAPI
DlgDirListComboBoxW(
    _In_ HWND hDlg,
    _Inout_ LPWSTR lpPathSpec,
    _In_ int nIDComboBox,
    _In_ int nIDStaticPath,
    _In_ DLG_DIR_LIST_FILE_TYPE uFiletype);
#ifdef UNICODE
#define DlgDirListComboBox  DlgDirListComboBoxW
#else
#define DlgDirListComboBox  DlgDirListComboBoxA
#endif // !UNICODE

_Win32_metadata_set_last_error_
WINUSERAPI
BOOL
WINAPI
DlgDirSelectComboBoxExA(
    _In_ HWND hwndDlg,
    _Out_writes_(cchOut) LPSTR lpString,
    _In_ int cchOut,
    _In_ int idComboBox);
_Win32_metadata_set_last_error_
WINUSERAPI
BOOL
WINAPI
DlgDirSelectComboBoxExW(
    _In_ HWND hwndDlg,
    _Out_writes_(cchOut) LPWSTR lpString,
    _In_ int cchOut,
    _In_ int idComboBox);
#ifdef UNICODE
#define DlgDirSelectComboBoxEx  DlgDirSelectComboBoxExW
#else
#define DlgDirSelectComboBoxEx  DlgDirSelectComboBoxExA
#endif // !UNICODE

#endif /* WINAPI_FAMILY_PARTITION(WINAPI_PARTITION_DESKTOP) */
#pragma endregion



/*
 * Dialog Styles
 */
#define DS_ABSALIGN         0x01L
#define DS_SYSMODAL         0x02L
#define DS_LOCALEDIT        0x20L   /* 16-bit: Edit items get Local storage. 32-bit and up: meaningless. */
#define DS_SETFONT          0x40L   /* User specified font for Dlg controls */
#define DS_MODALFRAME       0x80L   /* Can be combined with WS_CAPTION  */
#define DS_NOIDLEMSG        0x100L  /* WM_ENTERIDLE message will not be sent */
#define DS_SETFOREGROUND    0x200L  /* not in win3.1 */


#if(WINVER >= 0x0400)
#define DS_3DLOOK           0x0004L
#define DS_FIXEDSYS         0x0008L
#define DS_NOFAILCREATE     0x0010L
#define DS_CONTROL          0x0400L
#define DS_CENTER           0x0800L
#define DS_CENTERMOUSE      0x1000L
#define DS_CONTEXTHELP      0x2000L

#define DS_SHELLFONT        (DS_SETFONT | DS_FIXEDSYS)
#endif /* WINVER >= 0x0400 */

#if defined(_WIN32_WCE) && (_WIN32_WCE >= 0x0500)
#define DS_USEPIXELS        0x8000L
#endif


#define DM_GETDEFID         (WM_USER+0)
#define DM_SETDEFID         (WM_USER+1)

#if(WINVER >= 0x0400)
#define DM_REPOSITION       (WM_USER+2)
#endif /* WINVER >= 0x0400 */
/*
 * Returned in HIWORD() of DM_GETDEFID result if msg is supported
 */
#define DC_HASDEFID         0x534B

/*
 * Dialog Codes
 */
#define DLGC_WANTARROWS     0x0001      /* Control wants arrow keys         */
#define DLGC_WANTTAB        0x0002      /* Control wants tab keys           */
#define DLGC_WANTALLKEYS    0x0004      /* Control wants all keys           */
#define DLGC_WANTMESSAGE    0x0004      /* Pass message to control          */
#define DLGC_HASSETSEL      0x0008      /* Understands EM_SETSEL message    */
#define DLGC_DEFPUSHBUTTON  0x0010      /* Default pushbutton               */
#define DLGC_UNDEFPUSHBUTTON 0x0020     /* Non-default pushbutton           */
#define DLGC_RADIOBUTTON    0x0040      /* Radio button                     */
#define DLGC_WANTCHARS      0x0080      /* Want WM_CHAR messages            */
#define DLGC_STATIC         0x0100      /* Static item: don't include       */
#define DLGC_BUTTON         0x2000      /* Button item: can be checked      */

#define LB_CTLCODE          0L

/*
 * Listbox Return Values
 */
#define LB_OKAY             0
#define LB_ERR              (-1)
#define LB_ERRSPACE         (-2)

/*
**  The idStaticPath parameter to DlgDirList can have the following values
**  ORed if the list box should show other details of the files along with
**  the name of the files;
*/
                                  /* all other details also will be returned */


/*
 * Listbox Notification Codes
 */
#define LBN_ERRSPACE        (-2)
#define LBN_SELCHANGE       1
#define LBN_DBLCLK          2
#define LBN_SELCANCEL       3
#define LBN_SETFOCUS        4
#define LBN_KILLFOCUS       5



#ifndef NOWINMESSAGES

/*
 * Listbox messages
 */
#define LB_ADDSTRING            0x0180
#define LB_INSERTSTRING         0x0181
#define LB_DELETESTRING         0x0182
#define LB_SELITEMRANGEEX       0x0183
#define LB_RESETCONTENT         0x0184
#define LB_SETSEL               0x0185
#define LB_SETCURSEL            0x0186
#define LB_GETSEL               0x0187
#define LB_GETCURSEL            0x0188
#define LB_GETTEXT              0x0189
#define LB_GETTEXTLEN           0x018A
#define LB_GETCOUNT             0x018B
#define LB_SELECTSTRING         0x018C
#define LB_DIR                  0x018D
#define LB_GETTOPINDEX          0x018E
#define LB_FINDSTRING           0x018F
#define LB_GETSELCOUNT          0x0190
#define LB_GETSELITEMS          0x0191
#define LB_SETTABSTOPS          0x0192
#define LB_GETHORIZONTALEXTENT  0x0193
#define LB_SETHORIZONTALEXTENT  0x0194
#define LB_SETCOLUMNWIDTH       0x0195
#define LB_ADDFILE              0x0196
#define LB_SETTOPINDEX          0x0197
#define LB_GETITEMRECT          0x0198
#define LB_GETITEMDATA          0x0199
#define LB_SETITEMDATA          0x019A
#define LB_SELITEMRANGE         0x019B
#define LB_SETANCHORINDEX       0x019C
#define LB_GETANCHORINDEX       0x019D
#define LB_SETCARETINDEX        0x019E
#define LB_GETCARETINDEX        0x019F
#define LB_SETITEMHEIGHT        0x01A0
#define LB_GETITEMHEIGHT        0x01A1
#define LB_FINDSTRINGEXACT      0x01A2
#define LB_SETLOCALE            0x01A5
#define LB_GETLOCALE            0x01A6
#define LB_SETCOUNT             0x01A7
#if(WINVER >= 0x0400)
#define LB_INITSTORAGE          0x01A8
#define LB_ITEMFROMPOINT        0x01A9
#endif /* WINVER >= 0x0400 */
#if defined(_WIN32_WCE) && (_WIN32_WCE >= 0x0400)
#define LB_MULTIPLEADDSTRING    0x01B1
#endif


#if(_WIN32_WINNT >= 0x0501)
#define LB_GETLISTBOXINFO       0x01B2
#endif /* _WIN32_WINNT >= 0x0501 */

#if(_WIN32_WINNT >= 0x0501)
#define LB_MSGMAX               0x01B3
#elif defined(_WIN32_WCE) && (_WIN32_WCE >= 0x0400)
#define LB_MSGMAX               0x01B1
#elif(WINVER >= 0x0400)
#define LB_MSGMAX               0x01B0
#else
#define LB_MSGMAX               0x01A8
#endif

#endif /* !NOWINMESSAGES */

#ifndef NOWINSTYLES


/*
 * Listbox Styles
 */
#define LBS_NOTIFY            0x0001L
#define LBS_SORT              0x0002L
#define LBS_NOREDRAW          0x0004L
#define LBS_MULTIPLESEL       0x0008L
#define LBS_OWNERDRAWFIXED    0x0010L
#define LBS_OWNERDRAWVARIABLE 0x0020L
#define LBS_HASSTRINGS        0x0040L
#define LBS_USETABSTOPS       0x0080L
#define LBS_NOINTEGRALHEIGHT  0x0100L
#define LBS_MULTICOLUMN       0x0200L
#define LBS_WANTKEYBOARDINPUT 0x0400L
#define LBS_EXTENDEDSEL       0x0800L
#define LBS_DISABLENOSCROLL   0x1000L
#define LBS_NODATA            0x2000L
#if(WINVER >= 0x0400)
#define LBS_NOSEL             0x4000L
#endif /* WINVER >= 0x0400 */
#define LBS_COMBOBOX          0x8000L

#define LBS_STANDARD          (LBS_NOTIFY | LBS_SORT | WS_VSCROLL | WS_BORDER)


#endif /* !NOWINSTYLES */


/*
 * Combo Box return Values
 */
#define CB_OKAY             0
#define CB_ERR              (-1)
#define CB_ERRSPACE         (-2)


/*
 * Combo Box Notification Codes
 */
#define CBN_ERRSPACE        (-1)
#define CBN_SELCHANGE       1
#define CBN_DBLCLK          2
#define CBN_SETFOCUS        3
#define CBN_KILLFOCUS       4
#define CBN_EDITCHANGE      5
#define CBN_EDITUPDATE      6
#define CBN_DROPDOWN        7
#define CBN_CLOSEUP         8
#define CBN_SELENDOK        9
#define CBN_SELENDCANCEL    10

#ifndef NOWINSTYLES

/*
 * Combo Box styles
 */
#define CBS_SIMPLE            0x0001L
#define CBS_DROPDOWN          0x0002L
#define CBS_DROPDOWNLIST      0x0003L
#define CBS_OWNERDRAWFIXED    0x0010L
#define CBS_OWNERDRAWVARIABLE 0x0020L
#define CBS_AUTOHSCROLL       0x0040L
#define CBS_OEMCONVERT        0x0080L
#define CBS_SORT              0x0100L
#define CBS_HASSTRINGS        0x0200L
#define CBS_NOINTEGRALHEIGHT  0x0400L
#define CBS_DISABLENOSCROLL   0x0800L
#if(WINVER >= 0x0400)
#define CBS_UPPERCASE         0x2000L
#define CBS_LOWERCASE         0x4000L
#endif /* WINVER >= 0x0400 */

#endif  /* !NOWINSTYLES */


/*
 * Combo Box messages
 */
#ifndef NOWINMESSAGES
#define CB_GETEDITSEL               0x0140
#define CB_LIMITTEXT                0x0141
#define CB_SETEDITSEL               0x0142
#define CB_ADDSTRING                0x0143
#define CB_DELETESTRING             0x0144
#define CB_DIR                      0x0145
#define CB_GETCOUNT                 0x0146
#define CB_GETCURSEL                0x0147
#define CB_GETLBTEXT                0x0148
#define CB_GETLBTEXTLEN             0x0149
#define CB_INSERTSTRING             0x014A
#define CB_RESETCONTENT             0x014B
#define CB_FINDSTRING               0x014C
#define CB_SELECTSTRING             0x014D
#define CB_SETCURSEL                0x014E
#define CB_SHOWDROPDOWN             0x014F
#define CB_GETITEMDATA              0x0150
#define CB_SETITEMDATA              0x0151
#define CB_GETDROPPEDCONTROLRECT    0x0152
#define CB_SETITEMHEIGHT            0x0153
#define CB_GETITEMHEIGHT            0x0154
#define CB_SETEXTENDEDUI            0x0155
#define CB_GETEXTENDEDUI            0x0156
#define CB_GETDROPPEDSTATE          0x0157
#define CB_FINDSTRINGEXACT          0x0158
#define CB_SETLOCALE                0x0159
#define CB_GETLOCALE                0x015A
#if(WINVER >= 0x0400)
#define CB_GETTOPINDEX              0x015b
#define CB_SETTOPINDEX              0x015c
#define CB_GETHORIZONTALEXTENT      0x015d
#define CB_SETHORIZONTALEXTENT      0x015e
#define CB_GETDROPPEDWIDTH          0x015f
#define CB_SETDROPPEDWIDTH          0x0160
#define CB_INITSTORAGE              0x0161
#if defined(_WIN32_WCE) &&(_WIN32_WCE >= 0x0400)
#define CB_MULTIPLEADDSTRING        0x0163
#endif
#endif /* WINVER >= 0x0400 */

#if(_WIN32_WINNT >= 0x0501)
#define CB_GETCOMBOBOXINFO          0x0164
#endif /* _WIN32_WINNT >= 0x0501 */

#if(_WIN32_WINNT >= 0x0501)
#define CB_MSGMAX                   0x0165
#elif defined(_WIN32_WCE) && (_WIN32_WCE >= 0x0400)
#define CB_MSGMAX                   0x0163
#elif(WINVER >= 0x0400)
#define CB_MSGMAX                   0x0162
#else
#define CB_MSGMAX                   0x015B
#endif
#endif  /* !NOWINMESSAGES */



#ifndef NOWINSTYLES


/*
 * Scroll Bar Styles
 */
#define SBS_HORZ                    0x0000L
#define SBS_VERT                    0x0001L
#define SBS_TOPALIGN                0x0002L
#define SBS_LEFTALIGN               0x0002L
#define SBS_BOTTOMALIGN             0x0004L
#define SBS_RIGHTALIGN              0x0004L
#define SBS_SIZEBOXTOPLEFTALIGN     0x0002L
#define SBS_SIZEBOXBOTTOMRIGHTALIGN 0x0004L
#define SBS_SIZEBOX                 0x0008L
#if(WINVER >= 0x0400)
#define SBS_SIZEGRIP                0x0010L
#endif /* WINVER >= 0x0400 */


#endif /* !NOWINSTYLES */

/*
 * Scroll bar messages
 */
#ifndef NOWINMESSAGES
#define SBM_SETPOS                  0x00E0 /*not in win3.1 */
#define SBM_GETPOS                  0x00E1 /*not in win3.1 */
#define SBM_SETRANGE                0x00E2 /*not in win3.1 */
#define SBM_SETRANGEREDRAW          0x00E6 /*not in win3.1 */
#define SBM_GETRANGE                0x00E3 /*not in win3.1 */
#define SBM_ENABLE_ARROWS           0x00E4 /*not in win3.1 */
#if(WINVER >= 0x0400)
#define SBM_SETSCROLLINFO           0x00E9
#define SBM_GETSCROLLINFO           0x00EA
#endif /* WINVER >= 0x0400 */

#if(_WIN32_WINNT >= 0x0501)
#define SBM_GETSCROLLBARINFO        0x00EB
#endif /* _WIN32_WINNT >= 0x0501 */

#if(WINVER >= 0x0400)
#define SIF_RANGE           0x0001
#define SIF_PAGE            0x0002
#define SIF_POS             0x0004
#define SIF_DISABLENOSCROLL 0x0008
#define SIF_TRACKPOS        0x0010
#define SIF_ALL             (SIF_RANGE | SIF_PAGE | SIF_POS | SIF_TRACKPOS)

#pragma region Desktop Family
#if WINAPI_FAMILY_PARTITION(WINAPI_PARTITION_DESKTOP)

typedef struct tagSCROLLINFO
{
    UINT    cbSize;
    SCROLLINFO_MASK    fMask;
    int     nMin;
    int     nMax;
    UINT    nPage;
    int     nPos;
    int     nTrackPos;
}   SCROLLINFO, FAR *LPSCROLLINFO;
typedef SCROLLINFO CONST FAR *LPCSCROLLINFO;

WINUSERAPI
int
WINAPI
SetScrollInfo(
    _In_ HWND hwnd,
    _In_ int nBar,
    _In_ LPCSCROLLINFO lpsi,
    _In_ BOOL redraw);

_Win32_metadata_set_last_error_
WINUSERAPI
BOOL
WINAPI
GetScrollInfo(
    _In_ HWND hwnd,
    _In_ int nBar,
    _Inout_ LPSCROLLINFO lpsi);


#endif /* WINAPI_FAMILY_PARTITION(WINAPI_PARTITION_DESKTOP) */
#pragma endregion
#endif /* WINVER >= 0x0400 */

#endif /* !NOWINMESSAGES */
#endif /* !NOCTLMGR */

#ifndef NOMDI

/*
 * MDI client style bits
 */
#define MDIS_ALLCHILDSTYLES    0x0001

/*
 * wParam Flags for WM_MDITILE and WM_MDICASCADE messages.
 */
#define MDITILE_VERTICAL       0x0000 /*not in win3.1 */
#define MDITILE_HORIZONTAL     0x0001 /*not in win3.1 */
#define MDITILE_SKIPDISABLED   0x0002 /*not in win3.1 */
#if(_WIN32_WINNT >= 0x0500)
#define MDITILE_ZORDER         0x0004
#endif /* _WIN32_WINNT >= 0x0500 */

#pragma region Desktop Family
#if WINAPI_FAMILY_PARTITION(WINAPI_PARTITION_DESKTOP)

typedef struct tagMDICREATESTRUCTA {
    LPCSTR   szClass;
    LPCSTR   szTitle;
    HANDLE hOwner;
    int x;
    int y;
    int cx;
    int cy;
    WINDOW_STYLE style;
    LPARAM lParam;        /* app-defined stuff */
} MDICREATESTRUCTA, *LPMDICREATESTRUCTA;
typedef struct tagMDICREATESTRUCTW {
    LPCWSTR  szClass;
    LPCWSTR  szTitle;
    HANDLE hOwner;
    int x;
    int y;
    int cx;
    int cy;
    WINDOW_STYLE style;
    LPARAM lParam;        /* app-defined stuff */
} MDICREATESTRUCTW, *LPMDICREATESTRUCTW;
#ifdef UNICODE
typedef MDICREATESTRUCTW MDICREATESTRUCT;
typedef LPMDICREATESTRUCTW LPMDICREATESTRUCT;
#else
typedef MDICREATESTRUCTA MDICREATESTRUCT;
typedef LPMDICREATESTRUCTA LPMDICREATESTRUCT;
#endif // UNICODE

typedef struct tagCLIENTCREATESTRUCT {
    HANDLE hWindowMenu;
    UINT idFirstChild;
} CLIENTCREATESTRUCT, *LPCLIENTCREATESTRUCT;

WINUSERAPI
LRESULT
WINAPI
DefFrameProcA(
    _In_ HWND hWnd,
    _In_opt_ HWND hWndMDIClient,
    _In_ UINT uMsg,
    _In_ WPARAM wParam,
    _In_ LPARAM lParam);
WINUSERAPI
LRESULT
WINAPI
DefFrameProcW(
    _In_ HWND hWnd,
    _In_opt_ HWND hWndMDIClient,
    _In_ UINT uMsg,
    _In_ WPARAM wParam,
    _In_ LPARAM lParam);
#ifdef UNICODE
#define DefFrameProc  DefFrameProcW
#else
#define DefFrameProc  DefFrameProcA
#endif // !UNICODE

WINUSERAPI
#ifndef _MAC
LRESULT
WINAPI
#else
LRESULT
CALLBACK
#endif
DefMDIChildProcA(
    _In_ HWND hWnd,
    _In_ UINT uMsg,
    _In_ WPARAM wParam,
    _In_ LPARAM lParam);
WINUSERAPI
#ifndef _MAC
LRESULT
WINAPI
#else
LRESULT
CALLBACK
#endif
DefMDIChildProcW(
    _In_ HWND hWnd,
    _In_ UINT uMsg,
    _In_ WPARAM wParam,
    _In_ LPARAM lParam);
#ifdef UNICODE
#define DefMDIChildProc  DefMDIChildProcW
#else
#define DefMDIChildProc  DefMDIChildProcA
#endif // !UNICODE

#ifndef NOMSG

WINUSERAPI
BOOL
WINAPI
TranslateMDISysAccel(
    _In_ HWND hWndClient,
    _In_ LPMSG lpMsg);

#endif /* !NOMSG */

_Win32_metadata_set_last_error_
WINUSERAPI
UINT
WINAPI
ArrangeIconicWindows(
    _In_ HWND hWnd);

_Win32_metadata_set_last_error_
WINUSERAPI
HWND
WINAPI
CreateMDIWindowA(
    _In_ LPCSTR lpClassName,
    _In_ LPCSTR lpWindowName,
    _In_ WINDOW_STYLE dwStyle,
    _In_ int X,
    _In_ int Y,
    _In_ int nWidth,
    _In_ int nHeight,
    _In_opt_ HWND hWndParent,
    _In_opt_ HINSTANCE hInstance,
    _In_ LPARAM lParam);
_Win32_metadata_set_last_error_
WINUSERAPI
HWND
WINAPI
CreateMDIWindowW(
    _In_ LPCWSTR lpClassName,
    _In_ LPCWSTR lpWindowName,
    _In_ WINDOW_STYLE dwStyle,
    _In_ int X,
    _In_ int Y,
    _In_ int nWidth,
    _In_ int nHeight,
    _In_opt_ HWND hWndParent,
    _In_opt_ HINSTANCE hInstance,
    _In_ LPARAM lParam);
#ifdef UNICODE
#define CreateMDIWindow  CreateMDIWindowW
#else
#define CreateMDIWindow  CreateMDIWindowA
#endif // !UNICODE

#if(WINVER >= 0x0400)
_Win32_metadata_set_last_error_
WINUSERAPI
WORD
WINAPI
TileWindows(
    _In_opt_ HWND hwndParent,
    _In_ TILE_WINDOWS_HOW wHow,
    _In_opt_ CONST RECT * lpRect,
    _In_ UINT cKids,
    _In_reads_opt_(cKids) const HWND FAR * lpKids);

_Win32_metadata_set_last_error_
WINUSERAPI
WORD
WINAPI CascadeWindows(
    _In_opt_ HWND hwndParent,
    _In_ CASCADE_WINDOWS_HOW wHow,
    _In_opt_ CONST RECT * lpRect,
    _In_ UINT cKids,
    _In_reads_opt_(cKids) const HWND FAR * lpKids);

#endif /* WINVER >= 0x0400 */

#endif /* WINAPI_FAMILY_PARTITION(WINAPI_PARTITION_DESKTOP) */
#pragma endregion

#endif /* !NOMDI */

#endif /* !NOUSER */

/****** Help support ********************************************************/

#pragma region Desktop Family
#if WINAPI_FAMILY_PARTITION(WINAPI_PARTITION_DESKTOP)

#ifndef NOHELP

typedef DWORD HELPPOLY;
typedef struct tagMULTIKEYHELPA {
#ifndef _MAC
    DWORD  mkSize;
#else
    WORD   mkSize;
#endif
    CHAR   mkKeylist;
    CHAR   szKeyphrase[1];
} MULTIKEYHELPA, *PMULTIKEYHELPA, *LPMULTIKEYHELPA;
typedef struct tagMULTIKEYHELPW {
#ifndef _MAC
    DWORD  mkSize;
#else
    WORD   mkSize;
#endif
    WCHAR  mkKeylist;
    WCHAR  szKeyphrase[1];
} MULTIKEYHELPW, *PMULTIKEYHELPW, *LPMULTIKEYHELPW;
#ifdef UNICODE
typedef MULTIKEYHELPW MULTIKEYHELP;
typedef PMULTIKEYHELPW PMULTIKEYHELP;
typedef LPMULTIKEYHELPW LPMULTIKEYHELP;
#else
typedef MULTIKEYHELPA MULTIKEYHELP;
typedef PMULTIKEYHELPA PMULTIKEYHELP;
typedef LPMULTIKEYHELPA LPMULTIKEYHELP;
#endif // UNICODE

typedef struct tagHELPWININFOA {
    int  wStructSize;
    int  x;
    int  y;
    int  dx;
    int  dy;
    int  wMax;
    CHAR   rgchMember[2];
} HELPWININFOA, *PHELPWININFOA, *LPHELPWININFOA;
typedef struct tagHELPWININFOW {
    int  wStructSize;
    int  x;
    int  y;
    int  dx;
    int  dy;
    int  wMax;
    WCHAR  rgchMember[2];
} HELPWININFOW, *PHELPWININFOW, *LPHELPWININFOW;
#ifdef UNICODE
typedef HELPWININFOW HELPWININFO;
typedef PHELPWININFOW PHELPWININFO;
typedef LPHELPWININFOW LPHELPWININFO;
#else
typedef HELPWININFOA HELPWININFO;
typedef PHELPWININFOA PHELPWININFO;
typedef LPHELPWININFOA LPHELPWININFO;
#endif // UNICODE


/*
 * Commands to pass to WinHelp()
 */
#define HELP_CONTEXT      0x0001L  /* Display topic in ulTopic */
#define HELP_QUIT         0x0002L  /* Terminate help */
#define HELP_INDEX        0x0003L  /* Display index */
#define HELP_CONTENTS     0x0003L
#define HELP_HELPONHELP   0x0004L  /* Display help on using help */
#define HELP_SETINDEX     0x0005L  /* Set current Index for multi index help */
#define HELP_SETCONTENTS  0x0005L
#define HELP_CONTEXTPOPUP 0x0008L
#define HELP_FORCEFILE    0x0009L
#define HELP_KEY          0x0101L  /* Display topic for keyword in offabData */
#define HELP_COMMAND      0x0102L
#define HELP_PARTIALKEY   0x0105L
#define HELP_MULTIKEY     0x0201L
#define HELP_SETWINPOS    0x0203L
#if(WINVER >= 0x0400)
#define HELP_CONTEXTMENU  0x000a
#define HELP_FINDER       0x000b
#define HELP_WM_HELP      0x000c
#define HELP_SETPOPUP_POS 0x000d

#define HELP_TCARD              0x8000
#define HELP_TCARD_DATA         0x0010
#define HELP_TCARD_OTHER_CALLER 0x0011

// These are in winhelp.h in Win95.
#define IDH_NO_HELP                     28440
#define IDH_MISSING_CONTEXT             28441 // Control doesn't have matching help context
#define IDH_GENERIC_HELP_BUTTON         28442 // Property sheet help button
#define IDH_OK                          28443
#define IDH_CANCEL                      28444
#define IDH_HELP                        28445

#endif /* WINVER >= 0x0400 */



_Win32_metadata_set_last_error_
WINUSERAPI
BOOL
WINAPI
WinHelpA(
    _In_opt_ HWND hWndMain,
    _In_opt_ LPCSTR lpszHelp,
    _In_ UINT uCommand,
    _In_ ULONG_PTR dwData);
_Win32_metadata_set_last_error_
WINUSERAPI
BOOL
WINAPI
WinHelpW(
    _In_opt_ HWND hWndMain,
    _In_opt_ LPCWSTR lpszHelp,
    _In_ UINT uCommand,
    _In_ ULONG_PTR dwData);
#ifdef UNICODE
#define WinHelp  WinHelpW
#else
#define WinHelp  WinHelpA
#endif // !UNICODE

#endif /* !NOHELP */

#endif /* WINAPI_FAMILY_PARTITION(WINAPI_PARTITION_DESKTOP) */
#pragma endregion

#if(WINVER >= 0x0500)

#define GR_GDIOBJECTS       0       /* Count of GDI objects */
#define GR_USEROBJECTS      1       /* Count of USER objects */
#endif /* WINVER >= 0x0500 */
#if(WINVER >= 0x0601)
#define GR_GDIOBJECTS_PEAK  2       /* Peak count of GDI objects */
#define GR_USEROBJECTS_PEAK 4       /* Peak count of USER objects */
#endif /* WINVER >= 0x0601 */

#if(WINVER >= 0x0601)
#define GR_GLOBAL           ((HANDLE)-2)
#endif /* WINVER >= 0x0601 */

#pragma region Desktop Family
#if WINAPI_FAMILY_PARTITION(WINAPI_PARTITION_DESKTOP)

#if(WINVER >= 0x0500)
_Win32_metadata_set_last_error_
WINUSERAPI
DWORD
WINAPI
GetGuiResources(
    _In_ HANDLE hProcess,
    _In_ GET_GUI_RESOURCES_FLAGS uiFlags);
#endif /* WINVER >= 0x0500 */

#endif /* WINAPI_FAMILY_PARTITION(WINAPI_PARTITION_DESKTOP) */
#pragma endregion


#pragma region Desktop Family
#if WINAPI_FAMILY_PARTITION(WINAPI_PARTITION_DESKTOP)


#endif /* WINAPI_FAMILY_PARTITION(WINAPI_PARTITION_DESKTOP) */
#pragma endregion

#ifndef NOSYSPARAMSINFO

/*
 * Parameter for SystemParametersInfo.
 */

#define SPI_GETBEEP                 0x0001
#define SPI_SETBEEP                 0x0002
#define SPI_GETMOUSE                0x0003
#define SPI_SETMOUSE                0x0004
#define SPI_GETBORDER               0x0005
#define SPI_SETBORDER               0x0006
#define SPI_GETKEYBOARDSPEED        0x000A
#define SPI_SETKEYBOARDSPEED        0x000B
#define SPI_LANGDRIVER              0x000C
#define SPI_ICONHORIZONTALSPACING   0x000D
#define SPI_GETSCREENSAVETIMEOUT    0x000E
#define SPI_SETSCREENSAVETIMEOUT    0x000F
#define SPI_GETSCREENSAVEACTIVE     0x0010
#define SPI_SETSCREENSAVEACTIVE     0x0011
#define SPI_GETGRIDGRANULARITY      0x0012
#define SPI_SETGRIDGRANULARITY      0x0013
#define SPI_SETDESKWALLPAPER        0x0014
#define SPI_SETDESKPATTERN          0x0015
#define SPI_GETKEYBOARDDELAY        0x0016
#define SPI_SETKEYBOARDDELAY        0x0017
#define SPI_ICONVERTICALSPACING     0x0018
#define SPI_GETICONTITLEWRAP        0x0019
#define SPI_SETICONTITLEWRAP        0x001A
#define SPI_GETMENUDROPALIGNMENT    0x001B
#define SPI_SETMENUDROPALIGNMENT    0x001C
#define SPI_SETDOUBLECLKWIDTH       0x001D
#define SPI_SETDOUBLECLKHEIGHT      0x001E
#define SPI_GETICONTITLELOGFONT     0x001F
#define SPI_SETDOUBLECLICKTIME      0x0020
#define SPI_SETMOUSEBUTTONSWAP      0x0021
#define SPI_SETICONTITLELOGFONT     0x0022
#define SPI_GETFASTTASKSWITCH       0x0023
#define SPI_SETFASTTASKSWITCH       0x0024
#if(WINVER >= 0x0400)
#define SPI_SETDRAGFULLWINDOWS      0x0025
#define SPI_GETDRAGFULLWINDOWS      0x0026
#define SPI_GETNONCLIENTMETRICS     0x0029
#define SPI_SETNONCLIENTMETRICS     0x002A
#define SPI_GETMINIMIZEDMETRICS     0x002B
#define SPI_SETMINIMIZEDMETRICS     0x002C
#define SPI_GETICONMETRICS          0x002D
#define SPI_SETICONMETRICS          0x002E
#define SPI_SETWORKAREA             0x002F
#define SPI_GETWORKAREA             0x0030
#define SPI_SETPENWINDOWS           0x0031

#define SPI_GETHIGHCONTRAST         0x0042
#define SPI_SETHIGHCONTRAST         0x0043
#define SPI_GETKEYBOARDPREF         0x0044
#define SPI_SETKEYBOARDPREF         0x0045
#define SPI_GETSCREENREADER         0x0046
#define SPI_SETSCREENREADER         0x0047
#define SPI_GETANIMATION            0x0048
#define SPI_SETANIMATION            0x0049
#define SPI_GETFONTSMOOTHING        0x004A
#define SPI_SETFONTSMOOTHING        0x004B
#define SPI_SETDRAGWIDTH            0x004C
#define SPI_SETDRAGHEIGHT           0x004D
#define SPI_SETHANDHELD             0x004E
#define SPI_GETLOWPOWERTIMEOUT      0x004F
#define SPI_GETPOWEROFFTIMEOUT      0x0050
#define SPI_SETLOWPOWERTIMEOUT      0x0051
#define SPI_SETPOWEROFFTIMEOUT      0x0052
#define SPI_GETLOWPOWERACTIVE       0x0053
#define SPI_GETPOWEROFFACTIVE       0x0054
#define SPI_SETLOWPOWERACTIVE       0x0055
#define SPI_SETPOWEROFFACTIVE       0x0056
#define SPI_SETCURSORS              0x0057
#define SPI_SETICONS                0x0058
#define SPI_GETDEFAULTINPUTLANG     0x0059
#define SPI_SETDEFAULTINPUTLANG     0x005A
#define SPI_SETLANGTOGGLE           0x005B
#define SPI_GETWINDOWSEXTENSION     0x005C
#define SPI_SETMOUSETRAILS          0x005D
#define SPI_GETMOUSETRAILS          0x005E
#define SPI_SETSCREENSAVERRUNNING   0x0061
#define SPI_SCREENSAVERRUNNING     SPI_SETSCREENSAVERRUNNING
#endif /* WINVER >= 0x0400 */
#define SPI_GETFILTERKEYS          0x0032
#define SPI_SETFILTERKEYS          0x0033
#define SPI_GETTOGGLEKEYS          0x0034
#define SPI_SETTOGGLEKEYS          0x0035
#define SPI_GETMOUSEKEYS           0x0036
#define SPI_SETMOUSEKEYS           0x0037
#define SPI_GETSHOWSOUNDS          0x0038
#define SPI_SETSHOWSOUNDS          0x0039
#define SPI_GETSTICKYKEYS          0x003A
#define SPI_SETSTICKYKEYS          0x003B
#define SPI_GETACCESSTIMEOUT       0x003C
#define SPI_SETACCESSTIMEOUT       0x003D
#if(WINVER >= 0x0400)
#define SPI_GETSERIALKEYS          0x003E
#define SPI_SETSERIALKEYS          0x003F
#endif /* WINVER >= 0x0400 */
#define SPI_GETSOUNDSENTRY         0x0040
#define SPI_SETSOUNDSENTRY         0x0041
#if(_WIN32_WINNT >= 0x0400)
#define SPI_GETSNAPTODEFBUTTON     0x005F
#define SPI_SETSNAPTODEFBUTTON     0x0060
#endif /* _WIN32_WINNT >= 0x0400 */
#if (_WIN32_WINNT >= 0x0400) || (_WIN32_WINDOWS > 0x0400)
#define SPI_GETMOUSEHOVERWIDTH     0x0062
#define SPI_SETMOUSEHOVERWIDTH     0x0063
#define SPI_GETMOUSEHOVERHEIGHT    0x0064
#define SPI_SETMOUSEHOVERHEIGHT    0x0065
#define SPI_GETMOUSEHOVERTIME      0x0066
#define SPI_SETMOUSEHOVERTIME      0x0067
#define SPI_GETWHEELSCROLLLINES    0x0068
#define SPI_SETWHEELSCROLLLINES    0x0069
#define SPI_GETMENUSHOWDELAY       0x006A
#define SPI_SETMENUSHOWDELAY       0x006B

#if (_WIN32_WINNT >= 0x0600)
#define SPI_GETWHEELSCROLLCHARS   0x006C
#define SPI_SETWHEELSCROLLCHARS   0x006D
#endif

#define SPI_GETSHOWIMEUI          0x006E
#define SPI_SETSHOWIMEUI          0x006F
#endif


#if(WINVER >= 0x0500)
#define SPI_GETMOUSESPEED         0x0070
#define SPI_SETMOUSESPEED         0x0071
#define SPI_GETSCREENSAVERRUNNING 0x0072
#define SPI_GETDESKWALLPAPER      0x0073
#endif /* WINVER >= 0x0500 */

#if(WINVER >= 0x0600)
#define SPI_GETAUDIODESCRIPTION   0x0074
#define SPI_SETAUDIODESCRIPTION   0x0075

#define SPI_GETSCREENSAVESECURE   0x0076
#define SPI_SETSCREENSAVESECURE   0x0077
#endif /* WINVER >= 0x0600 */

#if(_WIN32_WINNT >= 0x0601)
#define SPI_GETHUNGAPPTIMEOUT           0x0078
#define SPI_SETHUNGAPPTIMEOUT           0x0079
#define SPI_GETWAITTOKILLTIMEOUT        0x007A
#define SPI_SETWAITTOKILLTIMEOUT        0x007B
#define SPI_GETWAITTOKILLSERVICETIMEOUT 0x007C
#define SPI_SETWAITTOKILLSERVICETIMEOUT 0x007D
#define SPI_GETMOUSEDOCKTHRESHOLD       0x007E
#define SPI_SETMOUSEDOCKTHRESHOLD       0x007F
#define SPI_GETPENDOCKTHRESHOLD         0x0080
#define SPI_SETPENDOCKTHRESHOLD         0x0081
#define SPI_GETWINARRANGING             0x0082
#define SPI_SETWINARRANGING             0x0083
#define SPI_GETMOUSEDRAGOUTTHRESHOLD    0x0084
#define SPI_SETMOUSEDRAGOUTTHRESHOLD    0x0085
#define SPI_GETPENDRAGOUTTHRESHOLD      0x0086
#define SPI_SETPENDRAGOUTTHRESHOLD      0x0087
#define SPI_GETMOUSESIDEMOVETHRESHOLD   0x0088
#define SPI_SETMOUSESIDEMOVETHRESHOLD   0x0089
#define SPI_GETPENSIDEMOVETHRESHOLD     0x008A
#define SPI_SETPENSIDEMOVETHRESHOLD     0x008B
#define SPI_GETDRAGFROMMAXIMIZE         0x008C
#define SPI_SETDRAGFROMMAXIMIZE         0x008D
#define SPI_GETSNAPSIZING               0x008E
#define SPI_SETSNAPSIZING               0x008F
#define SPI_GETDOCKMOVING               0x0090
#define SPI_SETDOCKMOVING               0x0091
#endif /* _WIN32_WINNT >= 0x0601 */

#if(WINVER >= 0x0602)
#define MAX_TOUCH_PREDICTION_FILTER_TAPS 3

#pragma region Desktop Family
#if WINAPI_FAMILY_PARTITION(WINAPI_PARTITION_DESKTOP)

typedef struct tagTouchPredictionParameters
{
    UINT cbSize;
    UINT dwLatency;       // Latency in millisecs
    UINT dwSampleTime;    // Sample time in millisecs (used to deduce velocity)
    UINT bUseHWTimeStamp; // Use H/W TimeStamps
} TOUCHPREDICTIONPARAMETERS, *PTOUCHPREDICTIONPARAMETERS;

#define TOUCHPREDICTIONPARAMETERS_DEFAULT_LATENCY 8
#define TOUCHPREDICTIONPARAMETERS_DEFAULT_SAMPLETIME 8
#define TOUCHPREDICTIONPARAMETERS_DEFAULT_USE_HW_TIMESTAMP 1
#define TOUCHPREDICTIONPARAMETERS_DEFAULT_RLS_DELTA 0.001f
#define TOUCHPREDICTIONPARAMETERS_DEFAULT_RLS_LAMBDA_MIN 0.9f
#define TOUCHPREDICTIONPARAMETERS_DEFAULT_RLS_LAMBDA_MAX 0.999f
#define TOUCHPREDICTIONPARAMETERS_DEFAULT_RLS_LAMBDA_LEARNING_RATE 0.001f
#define TOUCHPREDICTIONPARAMETERS_DEFAULT_RLS_EXPO_SMOOTH_ALPHA 0.99f

#endif /* WINAPI_FAMILY_PARTITION(WINAPI_PARTITION_DESKTOP) */
#pragma endregion

#define SPI_GETTOUCHPREDICTIONPARAMETERS 0x009C
#define SPI_SETTOUCHPREDICTIONPARAMETERS 0x009D

#define MAX_LOGICALDPIOVERRIDE  2
#define MIN_LOGICALDPIOVERRIDE  -2

#define SPI_GETLOGICALDPIOVERRIDE       0x009E
#define SPI_SETLOGICALDPIOVERRIDE       0x009F


#define SPI_GETMENURECT   0x00A2
#define SPI_SETMENURECT   0x00A3
#endif /* WINVER >= 0x0602 */


#if (NTDDI_VERSION >= NTDDI_WIN11_GE)
#define SPI_GETTOUCHPADPARAMETERS    0x00AE
#define SPI_SETTOUCHPADPARAMETERS    0x00AF
#endif // NTDDI_VERSION >= NTDDI_WIN11_GE

#if (NTDDI_VERSION >= NTDDI_WIN10_CO)
/* constants for SPI_{GET|SET}WAKEONINPUTDEVICETYPES */
#endif // NTDDI_VERSION >= NTDDI_WIN10_CO

#if (NTDDI_VERSION >= NTDDI_WIN10_CU)
#endif // NTDDI_VERSION >= NTDDI_WIN10_CU

#if (NTDDI_VERSION >= NTDDI_WIN11_GE)
#endif // NTDDI_VERSION >= NTDDI_WIN11_GE


#if(WINVER >= 0x0500)
#define SPI_GETACTIVEWINDOWTRACKING         0x1000
#define SPI_SETACTIVEWINDOWTRACKING         0x1001
#define SPI_GETMENUANIMATION                0x1002
#define SPI_SETMENUANIMATION                0x1003
#define SPI_GETCOMBOBOXANIMATION            0x1004
#define SPI_SETCOMBOBOXANIMATION            0x1005
#define SPI_GETLISTBOXSMOOTHSCROLLING       0x1006
#define SPI_SETLISTBOXSMOOTHSCROLLING       0x1007
#define SPI_GETGRADIENTCAPTIONS             0x1008
#define SPI_SETGRADIENTCAPTIONS             0x1009
#define SPI_GETKEYBOARDCUES                 0x100A
#define SPI_SETKEYBOARDCUES                 0x100B
#define SPI_GETMENUUNDERLINES               SPI_GETKEYBOARDCUES
#define SPI_SETMENUUNDERLINES               SPI_SETKEYBOARDCUES
#define SPI_GETACTIVEWNDTRKZORDER           0x100C
#define SPI_SETACTIVEWNDTRKZORDER           0x100D
#define SPI_GETHOTTRACKING                  0x100E
#define SPI_SETHOTTRACKING                  0x100F
#define SPI_GETMENUFADE                     0x1012
#define SPI_SETMENUFADE                     0x1013
#define SPI_GETSELECTIONFADE                0x1014
#define SPI_SETSELECTIONFADE                0x1015
#define SPI_GETTOOLTIPANIMATION             0x1016
#define SPI_SETTOOLTIPANIMATION             0x1017
#define SPI_GETTOOLTIPFADE                  0x1018
#define SPI_SETTOOLTIPFADE                  0x1019
#define SPI_GETCURSORSHADOW                 0x101A
#define SPI_SETCURSORSHADOW                 0x101B
#if(_WIN32_WINNT >= 0x0501)
#define SPI_GETMOUSESONAR                   0x101C
#define SPI_SETMOUSESONAR                   0x101D
#define SPI_GETMOUSECLICKLOCK               0x101E
#define SPI_SETMOUSECLICKLOCK               0x101F
#define SPI_GETMOUSEVANISH                  0x1020
#define SPI_SETMOUSEVANISH                  0x1021
#define SPI_GETFLATMENU                     0x1022
#define SPI_SETFLATMENU                     0x1023
#define SPI_GETDROPSHADOW                   0x1024
#define SPI_SETDROPSHADOW                   0x1025
#define SPI_GETBLOCKSENDINPUTRESETS         0x1026
#define SPI_SETBLOCKSENDINPUTRESETS         0x1027
#endif /* _WIN32_WINNT >= 0x0501 */

#define SPI_GETUIEFFECTS                    0x103E
#define SPI_SETUIEFFECTS                    0x103F

#if(_WIN32_WINNT >= 0x0600)
#define SPI_GETDISABLEOVERLAPPEDCONTENT     0x1040
#define SPI_SETDISABLEOVERLAPPEDCONTENT     0x1041
#define SPI_GETCLIENTAREAANIMATION          0x1042
#define SPI_SETCLIENTAREAANIMATION          0x1043
#define SPI_GETCLEARTYPE                    0x1048
#define SPI_SETCLEARTYPE                    0x1049
#define SPI_GETSPEECHRECOGNITION            0x104A
#define SPI_SETSPEECHRECOGNITION            0x104B
#endif /* _WIN32_WINNT >= 0x0600 */

#if(WINVER >= 0x0601)
#define SPI_GETCARETBROWSING                0x104C
#define SPI_SETCARETBROWSING                0x104D
#define SPI_GETTHREADLOCALINPUTSETTINGS     0x104E
#define SPI_SETTHREADLOCALINPUTSETTINGS     0x104F
#define SPI_GETSYSTEMLANGUAGEBAR            0x1050
#define SPI_SETSYSTEMLANGUAGEBAR            0x1051
#endif /* WINVER >= 0x0601 */

#if (NTDDI_VERSION >= NTDDI_WIN10_RS3)
#endif // NTDDI_VERSION >= NTDDI_WIN10_RS3

#define SPI_GETFOREGROUNDLOCKTIMEOUT        0x2000
#define SPI_SETFOREGROUNDLOCKTIMEOUT        0x2001
#define SPI_GETACTIVEWNDTRKTIMEOUT          0x2002
#define SPI_SETACTIVEWNDTRKTIMEOUT          0x2003
#define SPI_GETFOREGROUNDFLASHCOUNT         0x2004
#define SPI_SETFOREGROUNDFLASHCOUNT         0x2005
#define SPI_GETCARETWIDTH                   0x2006
#define SPI_SETCARETWIDTH                   0x2007

#if(_WIN32_WINNT >= 0x0501)
#define SPI_GETMOUSECLICKLOCKTIME           0x2008
#define SPI_SETMOUSECLICKLOCKTIME           0x2009
#define SPI_GETFONTSMOOTHINGTYPE            0x200A
#define SPI_SETFONTSMOOTHINGTYPE            0x200B

/* constants for SPI_GETFONTSMOOTHINGTYPE and SPI_SETFONTSMOOTHINGTYPE: */
#define FE_FONTSMOOTHINGSTANDARD            0x0001
#define FE_FONTSMOOTHINGCLEARTYPE           0x0002

#define SPI_GETFONTSMOOTHINGCONTRAST           0x200C
#define SPI_SETFONTSMOOTHINGCONTRAST           0x200D

#define SPI_GETFOCUSBORDERWIDTH             0x200E
#define SPI_SETFOCUSBORDERWIDTH             0x200F
#define SPI_GETFOCUSBORDERHEIGHT            0x2010
#define SPI_SETFOCUSBORDERHEIGHT            0x2011

#define SPI_GETFONTSMOOTHINGORIENTATION           0x2012
#define SPI_SETFONTSMOOTHINGORIENTATION           0x2013

/* constants for SPI_GETFONTSMOOTHINGORIENTATION and SPI_SETFONTSMOOTHINGORIENTATION: */
#define FE_FONTSMOOTHINGORIENTATIONBGR   0x0000
#define FE_FONTSMOOTHINGORIENTATIONRGB   0x0001
#endif /* _WIN32_WINNT >= 0x0501 */

#if(_WIN32_WINNT >= 0x0600)
#define SPI_GETMINIMUMHITRADIUS             0x2014
#define SPI_SETMINIMUMHITRADIUS             0x2015
#define SPI_GETMESSAGEDURATION              0x2016
#define SPI_SETMESSAGEDURATION              0x2017
#endif /* _WIN32_WINNT >= 0x0600 */

#if(WINVER >= 0x0602)
#define SPI_GETCONTACTVISUALIZATION         0x2018
#define SPI_SETCONTACTVISUALIZATION         0x2019
/* constants for SPI_GETCONTACTVISUALIZATION and SPI_SETCONTACTVISUALIZATION */
#define CONTACTVISUALIZATION_OFF                 0x0000
#define CONTACTVISUALIZATION_ON                  0x0001
#define CONTACTVISUALIZATION_PRESENTATIONMODE    0x0002

#define SPI_GETGESTUREVISUALIZATION         0x201A
#define SPI_SETGESTUREVISUALIZATION         0x201B
/* constants for SPI_GETGESTUREVISUALIZATION and SPI_SETGESTUREVISUALIZATION */
#define GESTUREVISUALIZATION_OFF                 0x0000
#define GESTUREVISUALIZATION_ON                  0x001F
#define GESTUREVISUALIZATION_TAP                 0x0001
#define GESTUREVISUALIZATION_DOUBLETAP           0x0002
#define GESTUREVISUALIZATION_PRESSANDTAP         0x0004
#define GESTUREVISUALIZATION_PRESSANDHOLD        0x0008
#define GESTUREVISUALIZATION_RIGHTTAP            0x0010
#endif /* WINVER >= 0x0602 */

#if(WINVER >= 0x0602)
#define SPI_GETMOUSEWHEELROUTING            0x201C
#define SPI_SETMOUSEWHEELROUTING            0x201D

#define MOUSEWHEEL_ROUTING_FOCUS                  0
#define MOUSEWHEEL_ROUTING_HYBRID                 1
#if(WINVER >= 0x0603)
#define MOUSEWHEEL_ROUTING_MOUSE_POS              2
#endif /* WINVER >= 0x0603 */
#endif /* WINVER >= 0x0602 */

#if(WINVER >= 0x0604)
#define SPI_GETPENVISUALIZATION                  0x201E
#define SPI_SETPENVISUALIZATION                  0x201F
/* constants for SPI_{GET|SET}PENVISUALIZATION */
#define PENVISUALIZATION_ON                      0x0023
#define PENVISUALIZATION_OFF                     0x0000
#define PENVISUALIZATION_TAP                     0x0001
#define PENVISUALIZATION_DOUBLETAP               0x0002
#define PENVISUALIZATION_CURSOR                  0x0020

#define SPI_GETPENARBITRATIONTYPE                0x2020
#define SPI_SETPENARBITRATIONTYPE                0x2021
/* constants for SPI_{GET|SET}PENARBITRATIONTYPE */
#define PENARBITRATIONTYPE_NONE                  0x0000
#define PENARBITRATIONTYPE_WIN8                  0x0001
#define PENARBITRATIONTYPE_FIS                   0x0002
#define PENARBITRATIONTYPE_SPT                   0x0003
#define PENARBITRATIONTYPE_MAX                   0x0004
#endif /* WINVER >= 0x0604 */

#if (NTDDI_VERSION >= NTDDI_WIN10_RS3)
#define SPI_GETCARETTIMEOUT                      0x2022
#define SPI_SETCARETTIMEOUT                      0x2023
#endif // NTDDI_VERSION >= NTDDI_WIN10_RS3

#if (NTDDI_VERSION >= NTDDI_WIN10_RS4)
#define SPI_GETHANDEDNESS                        0x2024
#define SPI_SETHANDEDNESS                        0x2025
typedef enum tagHANDEDNESS {
    HANDEDNESS_LEFT = 0,
    HANDEDNESS_RIGHT
} HANDEDNESS, *PHANDEDNESS;
#endif // NTDDI_VERSION >= NTDDI_WIN10_RS4

#endif /* WINVER >= 0x0500 */

/*
 * Flags
 */
#define SPIF_UPDATEINIFILE    0x0001
#define SPIF_SENDWININICHANGE 0x0002
#define SPIF_SENDCHANGE       SPIF_SENDWININICHANGE


#define METRICS_USEDEFAULT -1
#ifdef _WINGDI_
#ifndef NOGDI

#pragma region Desktop Family
#if WINAPI_FAMILY_PARTITION(WINAPI_PARTITION_DESKTOP)

typedef struct tagNONCLIENTMETRICSA
{
    UINT    cbSize;
    int     iBorderWidth;
    int     iScrollWidth;
    int     iScrollHeight;
    int     iCaptionWidth;
    int     iCaptionHeight;
    LOGFONTA lfCaptionFont;
    int     iSmCaptionWidth;
    int     iSmCaptionHeight;
    LOGFONTA lfSmCaptionFont;
    int     iMenuWidth;
    int     iMenuHeight;
    LOGFONTA lfMenuFont;
    LOGFONTA lfStatusFont;
    LOGFONTA lfMessageFont;
#if(WINVER >= 0x0600)
    int     iPaddedBorderWidth;
#endif /* WINVER >= 0x0600 */
}   NONCLIENTMETRICSA, *PNONCLIENTMETRICSA, FAR* LPNONCLIENTMETRICSA;
typedef struct tagNONCLIENTMETRICSW
{
    UINT    cbSize;
    int     iBorderWidth;
    int     iScrollWidth;
    int     iScrollHeight;
    int     iCaptionWidth;
    int     iCaptionHeight;
    LOGFONTW lfCaptionFont;
    int     iSmCaptionWidth;
    int     iSmCaptionHeight;
    LOGFONTW lfSmCaptionFont;
    int     iMenuWidth;
    int     iMenuHeight;
    LOGFONTW lfMenuFont;
    LOGFONTW lfStatusFont;
    LOGFONTW lfMessageFont;
#if(WINVER >= 0x0600)
    int     iPaddedBorderWidth;
#endif /* WINVER >= 0x0600 */
}   NONCLIENTMETRICSW, *PNONCLIENTMETRICSW, FAR* LPNONCLIENTMETRICSW;
#ifdef UNICODE
typedef NONCLIENTMETRICSW NONCLIENTMETRICS;
typedef PNONCLIENTMETRICSW PNONCLIENTMETRICS;
typedef LPNONCLIENTMETRICSW LPNONCLIENTMETRICS;
#else
typedef NONCLIENTMETRICSA NONCLIENTMETRICS;
typedef PNONCLIENTMETRICSA PNONCLIENTMETRICS;
typedef LPNONCLIENTMETRICSA LPNONCLIENTMETRICS;
#endif // UNICODE

#endif /* WINAPI_FAMILY_PARTITION(WINAPI_PARTITION_DESKTOP) */
#pragma endregion

#endif /* NOGDI */
#endif /* _WINGDI_ */

#define ARW_BOTTOMLEFT              0x0000L
#define ARW_BOTTOMRIGHT             0x0001L
#define ARW_TOPLEFT                 0x0002L
#define ARW_TOPRIGHT                0x0003L
#define ARW_STARTMASK               0x0003L
#define ARW_STARTRIGHT              0x0001L
#define ARW_STARTTOP                0x0002L

#define ARW_LEFT                    0x0000L
#define ARW_RIGHT                   0x0000L
#define ARW_UP                      0x0004L
#define ARW_DOWN                    0x0004L
#define ARW_HIDE                    0x0008L

#pragma region Desktop Family
#if WINAPI_FAMILY_PARTITION(WINAPI_PARTITION_DESKTOP)

typedef struct tagMINIMIZEDMETRICS
{
    UINT    cbSize;
    int     iWidth;
    int     iHorzGap;
    int     iVertGap;
    MINIMIZEDMETRICS_ARRANGE     iArrange;
}   MINIMIZEDMETRICS, *PMINIMIZEDMETRICS, *LPMINIMIZEDMETRICS;

#ifdef _WINGDI_
#ifndef NOGDI
typedef struct tagICONMETRICSA
{
    UINT    cbSize;
    int     iHorzSpacing;
    int     iVertSpacing;
    int     iTitleWrap;
    LOGFONTA lfFont;
}   ICONMETRICSA, *PICONMETRICSA, *LPICONMETRICSA;
typedef struct tagICONMETRICSW
{
    UINT    cbSize;
    int     iHorzSpacing;
    int     iVertSpacing;
    int     iTitleWrap;
    LOGFONTW lfFont;
}   ICONMETRICSW, *PICONMETRICSW, *LPICONMETRICSW;
#ifdef UNICODE
typedef ICONMETRICSW ICONMETRICS;
typedef PICONMETRICSW PICONMETRICS;
typedef LPICONMETRICSW LPICONMETRICS;
#else
typedef ICONMETRICSA ICONMETRICS;
typedef PICONMETRICSA PICONMETRICS;
typedef LPICONMETRICSA LPICONMETRICS;
#endif // UNICODE
#endif /* NOGDI */
#endif /* _WINGDI_ */

typedef struct tagANIMATIONINFO
{
    UINT    cbSize;
    int     iMinAnimate;
}   ANIMATIONINFO, *LPANIMATIONINFO;

typedef struct tagSERIALKEYSA
{
    UINT    cbSize;
    SERIALKEYS_FLAGS   dwFlags;
    LPSTR     lpszActivePort;
    LPSTR     lpszPort;
    UINT    iBaudRate;
    UINT    iPortState;
    UINT    iActive;
}   SERIALKEYSA, *LPSERIALKEYSA;
typedef struct tagSERIALKEYSW
{
    UINT    cbSize;
    SERIALKEYS_FLAGS   dwFlags;
    LPWSTR    lpszActivePort;
    LPWSTR    lpszPort;
    UINT    iBaudRate;
    UINT    iPortState;
    UINT    iActive;
}   SERIALKEYSW, *LPSERIALKEYSW;
#ifdef UNICODE
typedef SERIALKEYSW SERIALKEYS;
typedef LPSERIALKEYSW LPSERIALKEYS;
#else
typedef SERIALKEYSA SERIALKEYS;
typedef LPSERIALKEYSA LPSERIALKEYS;
#endif // UNICODE

/* flags for SERIALKEYS dwFlags field */
#define SERKF_SERIALKEYSON  0x00000001
#define SERKF_AVAILABLE     0x00000002
#define SERKF_INDICATOR     0x00000004


typedef struct tagHIGHCONTRASTA
{
    UINT    cbSize;
    HIGHCONTRASTW_FLAGS   dwFlags;
    LPSTR   lpszDefaultScheme;
}   HIGHCONTRASTA, *LPHIGHCONTRASTA;
typedef struct tagHIGHCONTRASTW
{
    UINT    cbSize;
    HIGHCONTRASTW_FLAGS   dwFlags;
    LPWSTR  lpszDefaultScheme;
}   HIGHCONTRASTW, *LPHIGHCONTRASTW;
#ifdef UNICODE
typedef HIGHCONTRASTW HIGHCONTRAST;
typedef LPHIGHCONTRASTW LPHIGHCONTRAST;
#else
typedef HIGHCONTRASTA HIGHCONTRAST;
typedef LPHIGHCONTRASTA LPHIGHCONTRAST;
#endif // UNICODE

#endif /* WINAPI_FAMILY_PARTITION(WINAPI_PARTITION_DESKTOP) */
#pragma endregion

/* flags for HIGHCONTRAST dwFlags field */
#define HCF_HIGHCONTRASTON          0x00000001
#define HCF_AVAILABLE               0x00000002
#define HCF_HOTKEYACTIVE            0x00000004
#define HCF_CONFIRMHOTKEY           0x00000008
#define HCF_HOTKEYSOUND             0x00000010
#define HCF_INDICATOR               0x00000020
#define HCF_HOTKEYAVAILABLE         0x00000040
#define HCF_LOGONDESKTOP            0x00000100
#define HCF_DEFAULTDESKTOP          0x00000200
#define HCF_OPTION_NOTHEMECHANGE    0x00001000

/* Flags for ChangeDisplaySettings */
#define CDS_UPDATEREGISTRY           0x00000001
#define CDS_TEST                     0x00000002
#define CDS_FULLSCREEN               0x00000004
#define CDS_GLOBAL                   0x00000008
#define CDS_SET_PRIMARY              0x00000010
#define CDS_VIDEOPARAMETERS          0x00000020
#if(WINVER >= 0x0600)
#define CDS_ENABLE_UNSAFE_MODES      0x00000100
#define CDS_DISABLE_UNSAFE_MODES     0x00000200
#endif /* WINVER >= 0x0600 */
#define CDS_RESET                    0x40000000
#define CDS_RESET_EX                 0x20000000
#define CDS_NORESET                  0x10000000

#include <tvout.h>

/* Return values for ChangeDisplaySettings */
#define DISP_CHANGE_SUCCESSFUL       0
#define DISP_CHANGE_RESTART          1
#define DISP_CHANGE_FAILED          -1
#define DISP_CHANGE_BADMODE         -2
#define DISP_CHANGE_NOTUPDATED      -3
#define DISP_CHANGE_BADFLAGS        -4
#define DISP_CHANGE_BADPARAM        -5
#if(_WIN32_WINNT >= 0x0501)
#define DISP_CHANGE_BADDUALVIEW     -6
#endif /* _WIN32_WINNT >= 0x0501 */

#pragma region Desktop Family
#if WINAPI_FAMILY_PARTITION(WINAPI_PARTITION_DESKTOP)

#ifdef _WINGDI_
#ifndef NOGDI

WINUSERAPI
DISP_CHANGE
WINAPI
ChangeDisplaySettingsA(
    _In_opt_ DEVMODEA* lpDevMode,
    _In_ CDS_TYPE dwFlags);
WINUSERAPI
DISP_CHANGE
WINAPI
ChangeDisplaySettingsW(
    _In_opt_ DEVMODEW* lpDevMode,
    _In_ CDS_TYPE dwFlags);
#ifdef UNICODE
#define ChangeDisplaySettings  ChangeDisplaySettingsW
#else
#define ChangeDisplaySettings  ChangeDisplaySettingsA
#endif // !UNICODE

WINUSERAPI
DISP_CHANGE
WINAPI
ChangeDisplaySettingsExA(
    _In_opt_ LPCSTR lpszDeviceName,
    _In_opt_ DEVMODEA* lpDevMode,
    _Reserved_ HWND hwnd,
    _In_ CDS_TYPE dwflags,
    _In_opt_ LPVOID lParam);
WINUSERAPI
DISP_CHANGE
WINAPI
ChangeDisplaySettingsExW(
    _In_opt_ LPCWSTR lpszDeviceName,
    _In_opt_ DEVMODEW* lpDevMode,
    _Reserved_ HWND hwnd,
    _In_ CDS_TYPE dwflags,
    _In_opt_ LPVOID lParam);
#ifdef UNICODE
#define ChangeDisplaySettingsEx  ChangeDisplaySettingsExW
#else
#define ChangeDisplaySettingsEx  ChangeDisplaySettingsExA
#endif // !UNICODE


#define ENUM_CURRENT_SETTINGS       ((DWORD)-1)
#define ENUM_REGISTRY_SETTINGS      ((DWORD)-2)

WINUSERAPI
BOOL
WINAPI
EnumDisplaySettingsA(
    _In_opt_ LPCSTR lpszDeviceName,
    _In_ ENUM_DISPLAY_SETTINGS_MODE iModeNum,
    _Inout_ DEVMODEA* lpDevMode);
WINUSERAPI
BOOL
WINAPI
EnumDisplaySettingsW(
    _In_opt_ LPCWSTR lpszDeviceName,
    _In_ ENUM_DISPLAY_SETTINGS_MODE iModeNum,
    _Inout_ DEVMODEW* lpDevMode);
#ifdef UNICODE
#define EnumDisplaySettings  EnumDisplaySettingsW
#else
#define EnumDisplaySettings  EnumDisplaySettingsA
#endif // !UNICODE

#if(WINVER >= 0x0500)

WINUSERAPI
BOOL
WINAPI
EnumDisplaySettingsExA(
    _In_opt_ LPCSTR lpszDeviceName,
    _In_ ENUM_DISPLAY_SETTINGS_MODE iModeNum,
    _Inout_ DEVMODEA* lpDevMode,
    _In_ ENUM_DISPLAY_SETTINGS_FLAGS dwFlags);
WINUSERAPI
BOOL
WINAPI
EnumDisplaySettingsExW(
    _In_opt_ LPCWSTR lpszDeviceName,
    _In_ ENUM_DISPLAY_SETTINGS_MODE iModeNum,
    _Inout_ DEVMODEW* lpDevMode,
    _In_ ENUM_DISPLAY_SETTINGS_FLAGS dwFlags);
#ifdef UNICODE
#define EnumDisplaySettingsEx  EnumDisplaySettingsExW
#else
#define EnumDisplaySettingsEx  EnumDisplaySettingsExA
#endif // !UNICODE

/* Flags for EnumDisplaySettingsEx */
#define EDS_RAWMODE                   0x00000002
#define EDS_ROTATEDMODE               0x00000004

WINUSERAPI
BOOL
WINAPI
EnumDisplayDevicesA(
    _In_opt_ LPCSTR lpDevice,
    _In_ DWORD iDevNum,
    _Inout_ PDISPLAY_DEVICEA lpDisplayDevice,
    _In_ DWORD dwFlags);
WINUSERAPI
BOOL
WINAPI
EnumDisplayDevicesW(
    _In_opt_ LPCWSTR lpDevice,
    _In_ DWORD iDevNum,
    _Inout_ PDISPLAY_DEVICEW lpDisplayDevice,
    _In_ DWORD dwFlags);
#ifdef UNICODE
#define EnumDisplayDevices  EnumDisplayDevicesW
#else
#define EnumDisplayDevices  EnumDisplayDevicesA
#endif // !UNICODE

/* Flags for EnumDisplayDevices */
#define EDD_GET_DEVICE_INTERFACE_NAME 0x00000001

#endif /* WINVER >= 0x0500 */

#if(WINVER >= 0x0601)

WINUSERAPI
LONG
WINAPI
GetDisplayConfigBufferSizes(
    _In_ UINT32 flags,
    _Out_ UINT32* numPathArrayElements,
    _Out_ UINT32* numModeInfoArrayElements);

WINUSERAPI
LONG
WINAPI
SetDisplayConfig(
    _In_ UINT32 numPathArrayElements,
    _In_reads_opt_(numPathArrayElements) DISPLAYCONFIG_PATH_INFO* pathArray,
    _In_ UINT32 numModeInfoArrayElements,
    _In_reads_opt_(numModeInfoArrayElements) DISPLAYCONFIG_MODE_INFO* modeInfoArray,
    _In_ UINT32 flags);

WINUSERAPI
_Success_(return == ERROR_SUCCESS) LONG
WINAPI
QueryDisplayConfig(
    _In_ UINT32 flags,
    _Inout_ UINT32* numPathArrayElements,
    _Out_writes_to_(*numPathArrayElements, *numPathArrayElements) DISPLAYCONFIG_PATH_INFO* pathArray,
    _Inout_ UINT32* numModeInfoArrayElements,
    _Out_writes_to_(*numModeInfoArrayElements, *numModeInfoArrayElements) DISPLAYCONFIG_MODE_INFO* modeInfoArray,
    _When_(!(flags & QDC_DATABASE_CURRENT), _Pre_null_)
    _When_(flags & QDC_DATABASE_CURRENT, _Out_)
        DISPLAYCONFIG_TOPOLOGY_ID* currentTopologyId);

WINUSERAPI
LONG
WINAPI
DisplayConfigGetDeviceInfo(
    _Inout_ DISPLAYCONFIG_DEVICE_INFO_HEADER* requestPacket);

WINUSERAPI
LONG
WINAPI
DisplayConfigSetDeviceInfo(
    _In_ DISPLAYCONFIG_DEVICE_INFO_HEADER* setPacket);

#endif /* WINVER >= 0x0601 */


#endif /* NOGDI */
#endif /* _WINGDI_ */


_Win32_metadata_set_last_error_
WINUSERAPI
_Success_(return != FALSE)
BOOL
WINAPI
SystemParametersInfoA(
    _In_ SYSTEM_PARAMETERS_INFO_ACTION uiAction,
    _In_ UINT uiParam,
    _Pre_maybenull_ _Post_valid_ PVOID pvParam,
    _In_ SYSTEM_PARAMETERS_INFO_UPDATE_FLAGS fWinIni);
_Win32_metadata_set_last_error_
WINUSERAPI
_Success_(return != FALSE)
BOOL
WINAPI
SystemParametersInfoW(
    _In_ SYSTEM_PARAMETERS_INFO_ACTION uiAction,
    _In_ UINT uiParam,
    _Pre_maybenull_ _Post_valid_ PVOID pvParam,
    _In_ SYSTEM_PARAMETERS_INFO_UPDATE_FLAGS fWinIni);
#ifdef UNICODE
#define SystemParametersInfo  SystemParametersInfoW
#else
#define SystemParametersInfo  SystemParametersInfoA
#endif // !UNICODE


#if(WINVER >= 0x0605)
_Win32_metadata_set_last_error_
WINUSERAPI
_Success_(return != FALSE)
BOOL
WINAPI
SystemParametersInfoForDpi(
    _In_ UINT uiAction,
    _In_ UINT uiParam,
    _Pre_maybenull_ _Post_valid_ PVOID pvParam,
    _In_ UINT fWinIni,
    _In_ UINT dpi);

#endif /* WINVER >= 0x0605 */

#endif /* WINAPI_FAMILY_PARTITION(WINAPI_PARTITION_DESKTOP) */
#pragma endregion

#endif  /* !NOSYSPARAMSINFO  */

#pragma region Desktop Family
#if WINAPI_FAMILY_PARTITION(WINAPI_PARTITION_DESKTOP)

/*
 * Accessibility support
 */
typedef struct tagFILTERKEYS
{
    UINT  cbSize;
    DWORD dwFlags;
    DWORD iWaitMSec;            // Acceptance Delay
    DWORD iDelayMSec;           // Delay Until Repeat
    DWORD iRepeatMSec;          // Repeat Rate
    DWORD iBounceMSec;          // Debounce Time
} FILTERKEYS, *LPFILTERKEYS;

#endif /* WINAPI_FAMILY_PARTITION(WINAPI_PARTITION_DESKTOP) */
#pragma endregion

/*
 * FILTERKEYS dwFlags field
 */
#define FKF_FILTERKEYSON    0x00000001
#define FKF_AVAILABLE       0x00000002
#define FKF_HOTKEYACTIVE    0x00000004
#define FKF_CONFIRMHOTKEY   0x00000008
#define FKF_HOTKEYSOUND     0x00000010
#define FKF_INDICATOR       0x00000020
#define FKF_CLICKON         0x00000040

#pragma region Desktop Family
#if WINAPI_FAMILY_PARTITION(WINAPI_PARTITION_DESKTOP)

typedef struct tagSTICKYKEYS
{
    UINT  cbSize;
    STICKYKEYS_FLAGS dwFlags;
} STICKYKEYS, *LPSTICKYKEYS;

#endif /* WINAPI_FAMILY_PARTITION(WINAPI_PARTITION_DESKTOP) */
#pragma endregion

/*
 * STICKYKEYS dwFlags field
 */
#define SKF_STICKYKEYSON    0x00000001
#define SKF_AVAILABLE       0x00000002
#define SKF_HOTKEYACTIVE    0x00000004
#define SKF_CONFIRMHOTKEY   0x00000008
#define SKF_HOTKEYSOUND     0x00000010
#define SKF_INDICATOR       0x00000020
#define SKF_AUDIBLEFEEDBACK 0x00000040
#define SKF_TRISTATE        0x00000080
#define SKF_TWOKEYSOFF      0x00000100
#if(_WIN32_WINNT >= 0x0500)
#define SKF_LALTLATCHED       0x10000000
#define SKF_LCTLLATCHED       0x04000000
#define SKF_LSHIFTLATCHED     0x01000000
#define SKF_RALTLATCHED       0x20000000
#define SKF_RCTLLATCHED       0x08000000
#define SKF_RSHIFTLATCHED     0x02000000
#define SKF_LWINLATCHED       0x40000000
#define SKF_RWINLATCHED       0x80000000
#define SKF_LALTLOCKED        0x00100000
#define SKF_LCTLLOCKED        0x00040000
#define SKF_LSHIFTLOCKED      0x00010000
#define SKF_RALTLOCKED        0x00200000
#define SKF_RCTLLOCKED        0x00080000
#define SKF_RSHIFTLOCKED      0x00020000
#define SKF_LWINLOCKED        0x00400000
#define SKF_RWINLOCKED        0x00800000
#endif /* _WIN32_WINNT >= 0x0500 */

#pragma region Desktop Family
#if WINAPI_FAMILY_PARTITION(WINAPI_PARTITION_DESKTOP)

typedef struct tagMOUSEKEYS
{
    UINT cbSize;
    DWORD dwFlags;
    DWORD iMaxSpeed;
    DWORD iTimeToMaxSpeed;
    DWORD iCtrlSpeed;
    DWORD dwReserved1;
    DWORD dwReserved2;
} MOUSEKEYS, *LPMOUSEKEYS;

#endif /* WINAPI_FAMILY_PARTITION(WINAPI_PARTITION_DESKTOP) */
#pragma endregion

/*
 * MOUSEKEYS dwFlags field
 */
#define MKF_MOUSEKEYSON     0x00000001
#define MKF_AVAILABLE       0x00000002
#define MKF_HOTKEYACTIVE    0x00000004
#define MKF_CONFIRMHOTKEY   0x00000008
#define MKF_HOTKEYSOUND     0x00000010
#define MKF_INDICATOR       0x00000020
#define MKF_MODIFIERS       0x00000040
#define MKF_REPLACENUMBERS  0x00000080
#if(_WIN32_WINNT >= 0x0500)
#define MKF_LEFTBUTTONSEL   0x10000000
#define MKF_RIGHTBUTTONSEL  0x20000000
#define MKF_LEFTBUTTONDOWN  0x01000000
#define MKF_RIGHTBUTTONDOWN 0x02000000
#define MKF_MOUSEMODE       0x80000000
#endif /* _WIN32_WINNT >= 0x0500 */

#pragma region Desktop Family
#if WINAPI_FAMILY_PARTITION(WINAPI_PARTITION_DESKTOP)

typedef struct tagACCESSTIMEOUT
{
    UINT  cbSize;
    DWORD dwFlags;
    DWORD iTimeOutMSec;
} ACCESSTIMEOUT, *LPACCESSTIMEOUT;

#endif /* WINAPI_FAMILY_PARTITION(WINAPI_PARTITION_DESKTOP) */
#pragma endregion

/*
 * ACCESSTIMEOUT dwFlags field
 */
#define ATF_TIMEOUTON       0x00000001
#define ATF_ONOFFFEEDBACK   0x00000002

/* values for SOUNDSENTRY iFSGrafEffect field */
#define SSGF_NONE       0
#define SSGF_DISPLAY    3

/* values for SOUNDSENTRY iFSTextEffect field */
#define SSTF_NONE       0
#define SSTF_CHARS      1
#define SSTF_BORDER     2
#define SSTF_DISPLAY    3

/* values for SOUNDSENTRY iWindowsEffect field */
#define SSWF_NONE     0
#define SSWF_TITLE    1
#define SSWF_WINDOW   2
#define SSWF_DISPLAY  3
#define SSWF_CUSTOM   4

#pragma region Desktop Family
#if WINAPI_FAMILY_PARTITION(WINAPI_PARTITION_DESKTOP)

typedef struct tagSOUNDSENTRYA
{
    UINT cbSize;
    SOUNDSENTRY_FLAGS dwFlags;
    SOUNDSENTRY_TEXT_EFFECT iFSTextEffect;
    DWORD iFSTextEffectMSec;
    DWORD iFSTextEffectColorBits;
    SOUND_SENTRY_GRAPHICS_EFFECT iFSGrafEffect;
    DWORD iFSGrafEffectMSec;
    DWORD iFSGrafEffectColor;
    SOUNDSENTRY_WINDOWS_EFFECT iWindowsEffect;
    DWORD iWindowsEffectMSec;
    LPSTR   lpszWindowsEffectDLL;
    DWORD iWindowsEffectOrdinal;
} SOUNDSENTRYA, *LPSOUNDSENTRYA;
typedef struct tagSOUNDSENTRYW
{
    UINT cbSize;
    SOUNDSENTRY_FLAGS dwFlags;
    SOUNDSENTRY_TEXT_EFFECT iFSTextEffect;
    DWORD iFSTextEffectMSec;
    DWORD iFSTextEffectColorBits;
    SOUND_SENTRY_GRAPHICS_EFFECT iFSGrafEffect;
    DWORD iFSGrafEffectMSec;
    DWORD iFSGrafEffectColor;
    SOUNDSENTRY_WINDOWS_EFFECT iWindowsEffect;
    DWORD iWindowsEffectMSec;
    LPWSTR  lpszWindowsEffectDLL;
    DWORD iWindowsEffectOrdinal;
} SOUNDSENTRYW, *LPSOUNDSENTRYW;
#ifdef UNICODE
typedef SOUNDSENTRYW SOUNDSENTRY;
typedef LPSOUNDSENTRYW LPSOUNDSENTRY;
#else
typedef SOUNDSENTRYA SOUNDSENTRY;
typedef LPSOUNDSENTRYA LPSOUNDSENTRY;
#endif // UNICODE

#endif /* WINAPI_FAMILY_PARTITION(WINAPI_PARTITION_DESKTOP) */
#pragma endregion

/*
 * SOUNDSENTRY dwFlags field
 */
#define SSF_SOUNDSENTRYON   0x00000001
#define SSF_AVAILABLE       0x00000002
#define SSF_INDICATOR       0x00000004

#pragma region Desktop or PC Family
#if WINAPI_FAMILY_PARTITION(WINAPI_PARTITION_DESKTOP | WINAPI_PARTITION_PC_APP)
#if(_WIN32_WINNT >= 0x0600)
WINUSERAPI
BOOL
WINAPI
SoundSentry(VOID);
#endif /* _WIN32_WINNT >= 0x0600 */
#endif /* WINAPI_FAMILY_PARTITION(WINAPI_PARTITION_DESKTOP | WINAPI_PARTITION_PC_APP) */
#pragma endregion

#pragma region Desktop Family
#if WINAPI_FAMILY_PARTITION(WINAPI_PARTITION_DESKTOP)
typedef struct tagTOGGLEKEYS
{
    UINT cbSize;
    DWORD dwFlags;
} TOGGLEKEYS, *LPTOGGLEKEYS;

#endif /* WINAPI_FAMILY_PARTITION(WINAPI_PARTITION_DESKTOP) */
#pragma endregion

/*
 * TOGGLEKEYS dwFlags field
 */
#define TKF_TOGGLEKEYSON    0x00000001
#define TKF_AVAILABLE       0x00000002
#define TKF_HOTKEYACTIVE    0x00000004
#define TKF_CONFIRMHOTKEY   0x00000008
#define TKF_HOTKEYSOUND     0x00000010
#define TKF_INDICATOR       0x00000020

#pragma region Desktop Family
#if WINAPI_FAMILY_PARTITION(WINAPI_PARTITION_DESKTOP)

#if(_WIN32_WINNT >= 0x0600)
typedef struct tagAUDIODESCRIPTION {
    UINT cbSize;   // sizeof(AudioDescriptionType)
    BOOL Enabled;  // On/Off
    LCID Locale;   // locale ID for language
} AUDIODESCRIPTION, *LPAUDIODESCRIPTION;
#endif /* _WIN32_WINNT >= 0x0600 */


/*
 * Set debug level
 */

WINUSERAPI
VOID
WINAPI
SetDebugErrorLevel(
    _In_ DWORD dwLevel);

#endif /* WINAPI_FAMILY_PARTITION(WINAPI_PARTITION_DESKTOP) */
#pragma endregion

/*
 * SetLastErrorEx() types.
 */

#define SLE_ERROR       0x00000001
#define SLE_MINORERROR  0x00000002
#define SLE_WARNING     0x00000003

#pragma region Desktop Family
#if WINAPI_FAMILY_PARTITION(WINAPI_PARTITION_DESKTOP)

_Win32_metadata_set_last_error_
WINUSERAPI
VOID
WINAPI
SetLastErrorEx(
    _In_ DWORD dwErrCode,
    _In_ DWORD dwType);

_Win32_metadata_set_last_error_
WINUSERAPI
int
WINAPI
InternalGetWindowText(
    _In_ HWND hWnd,
    _Out_writes_to_(cchMaxCount, return + 1) LPWSTR pString,
    _In_ int cchMaxCount);


#if defined(WINNT)
_Win32_metadata_set_last_error_
WINUSERAPI
BOOL
WINAPI
EndTask(
    _In_ HWND hWnd,
    _In_ BOOL fShutDown,
    _In_ BOOL fForce);
#endif

WINUSERAPI
BOOL
WINAPI
CancelShutdown(
    VOID);

#endif /* WINAPI_FAMILY_PARTITION(WINAPI_PARTITION_DESKTOP) */
#pragma endregion


#if(WINVER >= 0x0500)

/*
 * Multimonitor API.
 */

#define MONITOR_DEFAULTTONULL       0x00000000
#define MONITOR_DEFAULTTOPRIMARY    0x00000001
#define MONITOR_DEFAULTTONEAREST    0x00000002

#pragma region Desktop Family
#if WINAPI_FAMILY_PARTITION(WINAPI_PARTITION_DESKTOP)

WINUSERAPI
HMONITOR
WINAPI
MonitorFromPoint(
    _In_ POINT pt,
    _In_ DWORD dwFlags);

WINUSERAPI
HMONITOR
WINAPI
MonitorFromRect(
    _In_ LPCRECT lprc,
    _In_ DWORD dwFlags);

WINUSERAPI
HMONITOR
WINAPI
MonitorFromWindow(
    _In_ HWND hwnd,
    _In_ DWORD dwFlags);

#endif /* WINAPI_FAMILY_PARTITION(WINAPI_PARTITION_DESKTOP) */
#pragma endregion

#define MONITORINFOF_PRIMARY        0x00000001

#ifndef CCHDEVICENAME
#define CCHDEVICENAME 32
#endif

#pragma region Desktop Family
#if WINAPI_FAMILY_PARTITION(WINAPI_PARTITION_DESKTOP)

typedef struct tagMONITORINFO
{
    DWORD   cbSize;
    RECT    rcMonitor;
    RECT    rcWork;
    DWORD   dwFlags;
} MONITORINFO, *LPMONITORINFO;

#ifdef __cplusplus
typedef struct tagMONITORINFOEXA : public tagMONITORINFO
{
    CHAR        szDevice[CCHDEVICENAME];
} MONITORINFOEXA, *LPMONITORINFOEXA;
typedef struct tagMONITORINFOEXW : public tagMONITORINFO
{
    WCHAR       szDevice[CCHDEVICENAME];
} MONITORINFOEXW, *LPMONITORINFOEXW;
#ifdef UNICODE
typedef MONITORINFOEXW MONITORINFOEX;
typedef LPMONITORINFOEXW LPMONITORINFOEX;
#else
typedef MONITORINFOEXA MONITORINFOEX;
typedef LPMONITORINFOEXA LPMONITORINFOEX;
#endif // UNICODE
#else // ndef __cplusplus
typedef struct tagMONITORINFOEXA
{
    MONITORINFO DUMMYSTRUCTNAME;
    CHAR        szDevice[CCHDEVICENAME];
} MONITORINFOEXA, *LPMONITORINFOEXA;
typedef struct tagMONITORINFOEXW
{
    MONITORINFO DUMMYSTRUCTNAME;
    WCHAR       szDevice[CCHDEVICENAME];
} MONITORINFOEXW, *LPMONITORINFOEXW;
#ifdef UNICODE
typedef MONITORINFOEXW MONITORINFOEX;
typedef LPMONITORINFOEXW LPMONITORINFOEX;
#else
typedef MONITORINFOEXA MONITORINFOEX;
typedef LPMONITORINFOEXA LPMONITORINFOEX;
#endif // UNICODE
#endif

WINUSERAPI
BOOL
WINAPI
GetMonitorInfoA(
    _In_ HMONITOR hMonitor,
    _Inout_ LPMONITORINFO lpmi);
WINUSERAPI
BOOL
WINAPI
GetMonitorInfoW(
    _In_ HMONITOR hMonitor,
    _Inout_ LPMONITORINFO lpmi);
#ifdef UNICODE
#define GetMonitorInfo  GetMonitorInfoW
#else
#define GetMonitorInfo  GetMonitorInfoA
#endif // !UNICODE

typedef BOOL (CALLBACK* MONITORENUMPROC)(HMONITOR, HDC, LPRECT, LPARAM);

WINUSERAPI
BOOL
WINAPI
EnumDisplayMonitors(
    _In_opt_ HDC hdc,
    _In_opt_ LPCRECT lprcClip,
    _In_ MONITORENUMPROC lpfnEnum,
    _In_ LPARAM dwData);

#endif /* WINAPI_FAMILY_PARTITION(WINAPI_PARTITION_DESKTOP) */
#pragma endregion

#ifndef NOWINABLE

#pragma region Desktop Family
#if WINAPI_FAMILY_PARTITION(WINAPI_PARTITION_DESKTOP)

/*
 * WinEvents - Active Accessibility hooks
 */

WINUSERAPI
VOID
WINAPI
NotifyWinEvent(
    _In_ DWORD event,
    _In_ HWND  hwnd,
    _In_ LONG  idObject,
    _In_ LONG  idChild);

typedef VOID (CALLBACK* WINEVENTPROC)(
    HWINEVENTHOOK hWinEventHook,
    DWORD         event,
    HWND          hwnd,
    LONG          idObject,
    LONG          idChild,
    DWORD         idEventThread,
    DWORD         dwmsEventTime);

WINUSERAPI
HWINEVENTHOOK
WINAPI
SetWinEventHook(
    _In_ DWORD eventMin,
    _In_ DWORD eventMax,
    _In_opt_ HMODULE hmodWinEventProc,
    _In_ WINEVENTPROC pfnWinEventProc,
    _In_ DWORD idProcess,
    _In_ DWORD idThread,
    _In_ DWORD dwFlags);

#if(_WIN32_WINNT >= 0x0501)
WINUSERAPI
BOOL
WINAPI
IsWinEventHookInstalled(
    _In_ DWORD event);
#endif /* _WIN32_WINNT >= 0x0501 */

#endif /* WINAPI_FAMILY_PARTITION(WINAPI_PARTITION_DESKTOP) */
#pragma endregion

/*
 * dwFlags for SetWinEventHook
 */
#define WINEVENT_OUTOFCONTEXT   0x0000  // Events are ASYNC
#define WINEVENT_SKIPOWNTHREAD  0x0001  // Don't call back for events on installer's thread
#define WINEVENT_SKIPOWNPROCESS 0x0002  // Don't call back for events on installer's process
#define WINEVENT_INCONTEXT      0x0004  // Events are SYNC, this causes your dll to be injected into every process

#pragma region Desktop Family
#if WINAPI_FAMILY_PARTITION(WINAPI_PARTITION_DESKTOP)

WINUSERAPI
BOOL
WINAPI
UnhookWinEvent(
    _In_ HWINEVENTHOOK hWinEventHook);

#endif /* WINAPI_FAMILY_PARTITION(WINAPI_PARTITION_DESKTOP) */
#pragma endregion

/*
 * idObject values for WinEventProc and NotifyWinEvent
 */

/*
 * hwnd + idObject can be used with OLEACC.DLL's OleGetObjectFromWindow()
 * to get an interface pointer to the container.  indexChild is the item
 * within the container in question.  Setup a VARIANT with vt VT_I4 and
 * lVal the indexChild and pass that in to all methods.  Then you
 * are raring to go.
 */


/*
 * Common object IDs (cookies, only for sending WM_GETOBJECT to get at the
 * thing in question).  Positive IDs are reserved for apps (app specific),
 * negative IDs are system things and are global, 0 means "just little old
 * me".
 */
#define     CHILDID_SELF        0
#define     INDEXID_OBJECT      0
#define     INDEXID_CONTAINER   0

/*
 * Reserved IDs for system objects
 */
#define     OBJID_WINDOW        ((LONG)0x00000000)
#define     OBJID_SYSMENU       ((LONG)0xFFFFFFFF)
#define     OBJID_TITLEBAR      ((LONG)0xFFFFFFFE)
#define     OBJID_MENU          ((LONG)0xFFFFFFFD)
#define     OBJID_CLIENT        ((LONG)0xFFFFFFFC)
#define     OBJID_VSCROLL       ((LONG)0xFFFFFFFB)
#define     OBJID_HSCROLL       ((LONG)0xFFFFFFFA)
#define     OBJID_SIZEGRIP      ((LONG)0xFFFFFFF9)
#define     OBJID_CARET         ((LONG)0xFFFFFFF8)
#define     OBJID_CURSOR        ((LONG)0xFFFFFFF7)
#define     OBJID_ALERT         ((LONG)0xFFFFFFF6)
#define     OBJID_SOUND         ((LONG)0xFFFFFFF5)
#define     OBJID_QUERYCLASSNAMEIDX ((LONG)0xFFFFFFF4)
#define     OBJID_NATIVEOM      ((LONG)0xFFFFFFF0)

/*
 * EVENT DEFINITION
 */
#define EVENT_MIN           0x00000001
#define EVENT_MAX           0x7FFFFFFF

/*
 *  EVENT_SYSTEM_SOUND
 *  Sent when a sound is played.  Currently nothing is generating this, we
 *  this event when a system sound (for menus, etc) is played.  Apps
 *  generate this, if accessible, when a private sound is played.  For
 *  example, if Mail plays a "New Mail" sound.
 *
 *  System Sounds:
 *  (Generated by PlaySoundEvent in USER itself)
 *      hwnd            is NULL
 *      idObject        is OBJID_SOUND
 *      idChild         is sound child ID if one
 *  App Sounds:
 *  (PlaySoundEvent won't generate notification; up to app)
 *      hwnd + idObject gets interface pointer to Sound object
 *      idChild identifies the sound in question
 *  are going to be cleaning up the SOUNDSENTRY feature in the control panel
 *  and will use this at that time.  Applications implementing WinEvents
 *  are perfectly welcome to use it.  Clients of IAccessible* will simply
 *  turn around and get back a non-visual object that describes the sound.
 */
#define EVENT_SYSTEM_SOUND              0x0001

/*
 * EVENT_SYSTEM_ALERT
 * System Alerts:
 * (Generated by MessageBox() calls for example)
 *      hwnd            is hwndMessageBox
 *      idObject        is OBJID_ALERT
 * App Alerts:
 * (Generated whenever)
 *      hwnd+idObject gets interface pointer to Alert
 */
#define EVENT_SYSTEM_ALERT              0x0002

/*
 * EVENT_SYSTEM_FOREGROUND
 * Sent when the foreground (active) window changes, even if it is changing
 * to another window in the same thread as the previous one.
 *      hwnd            is hwndNewForeground
 *      idObject        is OBJID_WINDOW
 *      idChild    is INDEXID_OBJECT
 */
#define EVENT_SYSTEM_FOREGROUND         0x0003

/*
 * Menu
 *      hwnd            is window (top level window or popup menu window)
 *      idObject        is ID of control (OBJID_MENU, OBJID_SYSMENU, OBJID_SELF for popup)
 *      idChild         is CHILDID_SELF
 *
 * EVENT_SYSTEM_MENUSTART
 * EVENT_SYSTEM_MENUEND
 * For MENUSTART, hwnd+idObject+idChild refers to the control with the menu bar,
 *  or the control bringing up the context menu.
 *
 * Sent when entering into and leaving from menu mode (system, app bar, and
 * track popups).
 */
#define EVENT_SYSTEM_MENUSTART          0x0004
#define EVENT_SYSTEM_MENUEND            0x0005

/*
 * EVENT_SYSTEM_MENUPOPUPSTART
 * EVENT_SYSTEM_MENUPOPUPEND
 * Sent when a menu popup comes up and just before it is taken down.  Note
 * that for a call to TrackPopupMenu(), a client will see EVENT_SYSTEM_MENUSTART
 * followed almost immediately by EVENT_SYSTEM_MENUPOPUPSTART for the popup
 * being shown.
 *
 * For MENUPOPUP, hwnd+idObject+idChild refers to the NEW popup coming up, not the
 * parent item which is hierarchical.  You can get the parent menu/popup by
 * asking for the accParent object.
 */
#define EVENT_SYSTEM_MENUPOPUPSTART     0x0006
#define EVENT_SYSTEM_MENUPOPUPEND       0x0007


/*
 * EVENT_SYSTEM_CAPTURESTART
 * EVENT_SYSTEM_CAPTUREEND
 * Sent when a window takes the capture and releases the capture.
 */
#define EVENT_SYSTEM_CAPTURESTART       0x0008
#define EVENT_SYSTEM_CAPTUREEND         0x0009

/*
 * Move Size
 * EVENT_SYSTEM_MOVESIZESTART
 * EVENT_SYSTEM_MOVESIZEEND
 * Sent when a window enters and leaves move-size dragging mode.
 */
#define EVENT_SYSTEM_MOVESIZESTART      0x000A
#define EVENT_SYSTEM_MOVESIZEEND        0x000B

/*
 * Context Help
 * EVENT_SYSTEM_CONTEXTHELPSTART
 * EVENT_SYSTEM_CONTEXTHELPEND
 * Sent when a window enters and leaves context sensitive help mode.
 */
#define EVENT_SYSTEM_CONTEXTHELPSTART   0x000C
#define EVENT_SYSTEM_CONTEXTHELPEND     0x000D

/*
 * Drag & Drop
 * EVENT_SYSTEM_DRAGDROPSTART
 * EVENT_SYSTEM_DRAGDROPEND
 * Send the START notification just before going into drag&drop loop.  Send
 * the END notification just after canceling out.
 * Note that it is up to apps and OLE to generate this, since the system
 * doesn't know.  Like EVENT_SYSTEM_SOUND, it will be a while before this
 * is prevalent.
 */
#define EVENT_SYSTEM_DRAGDROPSTART      0x000E
#define EVENT_SYSTEM_DRAGDROPEND        0x000F

/*
 * Dialog
 * Send the START notification right after the dialog is completely
 *  initialized and visible.  Send the END right before the dialog
 *  is hidden and goes away.
 * EVENT_SYSTEM_DIALOGSTART
 * EVENT_SYSTEM_DIALOGEND
 */
#define EVENT_SYSTEM_DIALOGSTART        0x0010
#define EVENT_SYSTEM_DIALOGEND          0x0011

/*
 * EVENT_SYSTEM_SCROLLING
 * EVENT_SYSTEM_SCROLLINGSTART
 * EVENT_SYSTEM_SCROLLINGEND
 * Sent when beginning and ending the tracking of a scrollbar in a window,
 * and also for scrollbar controls.
 */
#define EVENT_SYSTEM_SCROLLINGSTART     0x0012
#define EVENT_SYSTEM_SCROLLINGEND       0x0013

/*
 * Alt-Tab Window
 * Send the START notification right after the switch window is initialized
 * and visible.  Send the END right before it is hidden and goes away.
 * EVENT_SYSTEM_SWITCHSTART
 * EVENT_SYSTEM_SWITCHEND
 */
#define EVENT_SYSTEM_SWITCHSTART        0x0014
#define EVENT_SYSTEM_SWITCHEND          0x0015

/*
 * EVENT_SYSTEM_MINIMIZESTART
 * EVENT_SYSTEM_MINIMIZEEND
 * Sent when a window minimizes and just before it restores.
 */
#define EVENT_SYSTEM_MINIMIZESTART      0x0016
#define EVENT_SYSTEM_MINIMIZEEND        0x0017


#if(_WIN32_WINNT >= 0x0600)
#define EVENT_SYSTEM_DESKTOPSWITCH      0x0020
#endif /* _WIN32_WINNT >= 0x0600 */


#if(_WIN32_WINNT >= 0x0602)
// AppGrabbed: HWND = hwnd of app thumbnail, objectID = 0, childID = 0
#define EVENT_SYSTEM_SWITCHER_APPGRABBED    0x0024
// OverTarget: HWND = hwnd of app thumbnail, objectID =
//            1 for center
//            2 for near snapped
//            3 for far snapped
//            4 for prune
//            childID = 0
#define EVENT_SYSTEM_SWITCHER_APPOVERTARGET 0x0025
// Dropped: HWND = hwnd of app thumbnail, objectID = <same as above>, childID = 0
#define EVENT_SYSTEM_SWITCHER_APPDROPPED    0x0026
// Cancelled: HWND = hwnd of app thumbnail, objectID = 0, childID = 0
#define EVENT_SYSTEM_SWITCHER_CANCELLED     0x0027
#endif /* _WIN32_WINNT >= 0x0602 */


#if(_WIN32_WINNT >= 0x0602)

/*
 * Sent when an IME's soft key is pressed and should be echoed,
 * but is not passed through the keyboard hook.
 * Must not be sent when a key is sent through the keyboard hook.
 *     HWND             is the hwnd of the UI containing the soft key
 *     idChild          is the Unicode value of the character entered
 *     idObject         is a bitfield
 *         0x00000001: set if a 32-bit Unicode surrogate pair is used
 */
#define EVENT_SYSTEM_IME_KEY_NOTIFICATION  0x0029

#endif /* _WIN32_WINNT >= 0x0602 */


#if(_WIN32_WINNT >= 0x0601)
#define EVENT_SYSTEM_END        0x00FF

#define EVENT_OEM_DEFINED_START     0x0101
#define EVENT_OEM_DEFINED_END       0x01FF

#define EVENT_UIA_EVENTID_START         0x4E00
#define EVENT_UIA_EVENTID_END           0x4EFF

#define EVENT_UIA_PROPID_START          0x7500
#define EVENT_UIA_PROPID_END            0x75FF
#endif /* _WIN32_WINNT >= 0x0601 */

#if(_WIN32_WINNT >= 0x0501)
#define EVENT_CONSOLE_CARET             0x4001
#define EVENT_CONSOLE_UPDATE_REGION     0x4002
#define EVENT_CONSOLE_UPDATE_SIMPLE     0x4003
#define EVENT_CONSOLE_UPDATE_SCROLL     0x4004
#define EVENT_CONSOLE_LAYOUT            0x4005
#define EVENT_CONSOLE_START_APPLICATION 0x4006
#define EVENT_CONSOLE_END_APPLICATION   0x4007

/*
 * Flags for EVENT_CONSOLE_START/END_APPLICATION.
 */
#if defined(_WIN64)
#define CONSOLE_APPLICATION_16BIT       0x0000
#else
#define CONSOLE_APPLICATION_16BIT       0x0001
#endif

/*
 * Flags for EVENT_CONSOLE_CARET
 */
#define CONSOLE_CARET_SELECTION         0x0001
#define CONSOLE_CARET_VISIBLE           0x0002
#endif /* _WIN32_WINNT >= 0x0501 */

#if(_WIN32_WINNT >= 0x0601)
#define EVENT_CONSOLE_END       0x40FF
#endif /* _WIN32_WINNT >= 0x0601 */

/*
 * Object events
 *
 * The system AND apps generate these.  The system generates these for
 * real windows.  Apps generate these for objects within their window which
 * act like a separate control, e.g. an item in a list view.
 *
 * When the system generate them, dwParam2 is always WMOBJID_SELF.  When
 * apps generate them, apps put the has-meaning-to-the-app-only ID value
 * in dwParam2.
 * For all events, if you want detailed accessibility information, callers
 * should
 *      * Call AccessibleObjectFromWindow() with the hwnd, idObject parameters
 *          of the event, and IID_IAccessible as the REFIID, to get back an
 *          IAccessible* to talk to
 *      * Initialize and fill in a VARIANT as VT_I4 with lVal the idChild
 *          parameter of the event.
 *      * If idChild isn't zero, call get_accChild() in the container to see
 *          if the child is an object in its own right.  If so, you will get
 *          back an IDispatch* object for the child.  You should release the
 *          parent, and call QueryInterface() on the child object to get its
 *          IAccessible*.  Then you talk directly to the child.  Otherwise,
 *          if get_accChild() returns you nothing, you should continue to
 *          use the child VARIANT.  You will ask the container for the properties
 *          of the child identified by the VARIANT.  In other words, the
 *          child in this case is accessible but not a full-blown object.
 *          Like a button on a titlebar which is 'small' and has no children.
 */

/*
 * For all EVENT_OBJECT events,
 *      hwnd is the dude to Send the WM_GETOBJECT message to (unless NULL,
 *          see above for system things)
 *      idObject is the ID of the object that can resolve any queries a
 *          client might have.  It's a way to deal with windowless controls,
 *          controls that are just drawn on the screen in some larger parent
 *          window (like SDM), or standard frame elements of a window.
 *      idChild is the piece inside of the object that is affected.  This
 *          allows clients to access things that are too small to have full
 *          blown objects in their own right.  Like the thumb of a scrollbar.
 *          The hwnd/idObject pair gets you to the container, the dude you
 *          probably want to talk to most of the time anyway.  The idChild
 *          can then be passed into the acc properties to get the name/value
 *          of it as needed.
 *
 * Example #1:
 *      System propagating a listbox selection change
 *      EVENT_OBJECT_SELECTION
 *          hwnd == listbox hwnd
 *          idObject == OBJID_WINDOW
 *          idChild == new selected item, or CHILDID_SELF if
 *              nothing now selected within container.
 *      Word '97 propagating a listbox selection change
 *          hwnd == SDM window
 *          idObject == SDM ID to get at listbox 'control'
 *          idChild == new selected item, or CHILDID_SELF if
 *              nothing
 *
 * Example #2:
 *      System propagating a menu item selection on the menu bar
 *      EVENT_OBJECT_SELECTION
 *          hwnd == top level window
 *          idObject == OBJID_MENU
 *          idChild == ID of child menu bar item selected
 *
 * Example #3:
 *      System propagating a dropdown coming off of said menu bar item
 *      EVENT_OBJECT_CREATE
 *          hwnd == popup item
 *          idObject == OBJID_WINDOW
 *          idChild == CHILDID_SELF
 *
 * Example #4:
 *
 * For EVENT_OBJECT_REORDER, the object referred to by hwnd/idObject is the
 * PARENT container in which the zorder is occurring.  This is because if
 * one child is zordering, all of them are changing their relative zorder.
 */
#define EVENT_OBJECT_CREATE                 0x8000  // hwnd + ID + idChild is created item
#define EVENT_OBJECT_DESTROY                0x8001  // hwnd + ID + idChild is destroyed item
#define EVENT_OBJECT_SHOW                   0x8002  // hwnd + ID + idChild is shown item
#define EVENT_OBJECT_HIDE                   0x8003  // hwnd + ID + idChild is hidden item
#define EVENT_OBJECT_REORDER                0x8004  // hwnd + ID + idChild is parent of zordering children
/*
 * NOTE:
 * Minimize the number of notifications!
 *
 * When you are hiding a parent object, obviously all child objects are no
 * longer visible on screen.  They still have the same "visible" status,
 * but are not truly visible.  Hence do not send HIDE notifications for the
 * children also.  One implies all.  The same goes for SHOW.
 */


#define EVENT_OBJECT_FOCUS                  0x8005  // hwnd + ID + idChild is focused item
#define EVENT_OBJECT_SELECTION              0x8006  // hwnd + ID + idChild is selected item (if only one), or idChild is OBJID_WINDOW if complex
#define EVENT_OBJECT_SELECTIONADD           0x8007  // hwnd + ID + idChild is item added
#define EVENT_OBJECT_SELECTIONREMOVE        0x8008  // hwnd + ID + idChild is item removed
#define EVENT_OBJECT_SELECTIONWITHIN        0x8009  // hwnd + ID + idChild is parent of changed selected items

/*
 * NOTES:
 * There is only one "focused" child item in a parent.  This is the place
 * keystrokes are going at a given moment.  Hence only send a notification
 * about where the NEW focus is going.  A NEW item getting the focus already
 * implies that the OLD item is losing it.
 *
 * SELECTION however can be multiple.  Hence the different SELECTION
 * notifications.  Here's when to use each:
 *
 * (1) Send a SELECTION notification in the simple single selection
 *     case (like the focus) when the item with the selection is
 *     merely moving to a different item within a container.  hwnd + ID
 *     is the container control, idChildItem is the new child with the
 *     selection.
 *
 * (2) Send a SELECTIONADD notification when a new item has simply been added
 *     to the selection within a container.  This is appropriate when the
 *     number of newly selected items is very small.  hwnd + ID is the
 *     container control, idChildItem is the new child added to the selection.
 *
 * (3) Send a SELECTIONREMOVE notification when a new item has simply been
 *     removed from the selection within a container.  This is appropriate
 *     when the number of newly selected items is very small, just like
 *     SELECTIONADD.  hwnd + ID is the container control, idChildItem is the
 *     new child removed from the selection.
 *
 * (4) Send a SELECTIONWITHIN notification when the selected items within a
 *     control have changed substantially.  Rather than propagate a large
 *     number of changes to reflect removal for some items, addition of
 *     others, just tell somebody who cares that a lot happened.  It will
 *     be faster an easier for somebody watching to just turn around and
 *     query the container control what the new bunch of selected items
 *     are.
 */

#define EVENT_OBJECT_STATECHANGE            0x800A  // hwnd + ID + idChild is item w/ state change
/*
 * Examples of when to send an EVENT_OBJECT_STATECHANGE include
 *      * It is being enabled/disabled (USER does for windows)
 *      * It is being pressed/released (USER does for buttons)
 *      * It is being checked/unchecked (USER does for radio/check buttons)
 */
#define EVENT_OBJECT_LOCATIONCHANGE         0x800B  // hwnd + ID + idChild is moved/sized item

/*
 * Note:
 * A LOCATIONCHANGE is not sent for every child object when the parent
 * changes shape/moves.  Send one notification for the topmost object
 * that is changing.  For example, if the user resizes a top level window,
 * USER will generate a LOCATIONCHANGE for it, but not for the menu bar,
 * title bar, scrollbars, etc.  that are also changing shape/moving.
 *
 * In other words, it only generates LOCATIONCHANGE notifications for
 * real windows that are moving/sizing.  It will not generate a LOCATIONCHANGE
 * for every non-floating child window when the parent moves (the children are
 * logically moving also on screen, but not relative to the parent).
 *
 * Now, if the app itself resizes child windows as a result of being
 * sized, USER will generate LOCATIONCHANGEs for those dudes also because
 * it doesn't know better.
 *
 * Note also that USER will generate LOCATIONCHANGE notifications for two
 * non-window sys objects:
 *      (1) System caret
 *      (2) Cursor
 */

#define EVENT_OBJECT_NAMECHANGE             0x800C  // hwnd + ID + idChild is item w/ name change
#define EVENT_OBJECT_DESCRIPTIONCHANGE      0x800D  // hwnd + ID + idChild is item w/ desc change
#define EVENT_OBJECT_VALUECHANGE            0x800E  // hwnd + ID + idChild is item w/ value change
#define EVENT_OBJECT_PARENTCHANGE           0x800F  // hwnd + ID + idChild is item w/ new parent
#define EVENT_OBJECT_HELPCHANGE             0x8010  // hwnd + ID + idChild is item w/ help change
#define EVENT_OBJECT_DEFACTIONCHANGE        0x8011  // hwnd + ID + idChild is item w/ def action change
#define EVENT_OBJECT_ACCELERATORCHANGE      0x8012  // hwnd + ID + idChild is item w/ keybd accel change

#if(_WIN32_WINNT >= 0x0600)
#define EVENT_OBJECT_INVOKED                0x8013  // hwnd + ID + idChild is item invoked
#define EVENT_OBJECT_TEXTSELECTIONCHANGED   0x8014  // hwnd + ID + idChild is item w? test selection change

/*
 * EVENT_OBJECT_CONTENTSCROLLED
 * Sent when ending the scrolling of a window object.
 *
 * Unlike the similar event (EVENT_SYSTEM_SCROLLEND), this event will be
 * associated with the scrolling window itself. There is no difference
 * between horizontal or vertical scrolling.
 *
 * This event should be posted whenever scroll action is completed, including
 * when it is scrolled by scroll bars, mouse wheel, or keyboard navigations.
 *
 *   example:
 *          hwnd == window that is scrolling
 *          idObject == OBJID_CLIENT
 *          idChild == CHILDID_SELF
 */
#define EVENT_OBJECT_CONTENTSCROLLED        0x8015
#endif /* _WIN32_WINNT >= 0x0600 */

#if(_WIN32_WINNT >= 0x0601)
#define EVENT_SYSTEM_ARRANGMENTPREVIEW      0x8016
#endif /* _WIN32_WINNT >= 0x0601 */

#if(_WIN32_WINNT >= 0x0602)

/*
 * EVENT_OBJECT_CLOAKED / UNCLOAKED
 * Sent when a window is cloaked or uncloaked.
 * A cloaked window still exists, but is invisible to
 * the user.
 */
#define EVENT_OBJECT_CLOAKED                0x8017
#define EVENT_OBJECT_UNCLOAKED              0x8018

/*
 * EVENT_OBJECT_LIVEREGIONCHANGED
 * Sent when an object that is part of a live region
 * changes.  A live region is an area of an application
 * that changes frequently and/or asynchronously, so
 * that an assistive technology tool might want to pay
 * special attention to it.
 */
#define EVENT_OBJECT_LIVEREGIONCHANGED      0x8019

/*
 * EVENT_OBJECT_HOSTEDOBJECTSINVALIDATED
 * Sent when a window that is hosting other Accessible
 * objects changes the hosted objects.  A client may
 * wish to requery to see what the new hosted objects are,
 * especially if it has been monitoring events from this
 * window.  A hosted object is one with a different Accessibility
 * framework (MSAA or UI Automation) from its host.
 *
 * Changes in hosted objects with the *same* framework
 * as the parent should be handed with the usual structural
 * change events, such as EVENT_OBJECT_CREATED for MSAA.
 * see above.
 */
#define EVENT_OBJECT_HOSTEDOBJECTSINVALIDATED 0x8020

/*
 * Drag / Drop Events
 * These events are used in conjunction with the
 * UI Automation Drag/Drop patterns.
 *
 * For DRAGSTART, DRAGCANCEL, and DRAGCOMPLETE,
 * HWND+objectID+childID refers to the object being dragged.
 *
 * For DRAGENTER, DRAGLEAVE, and DRAGDROPPED,
 * HWND+objectID+childID refers to the target of the drop
 * that is being hovered over.
 */

#define EVENT_OBJECT_DRAGSTART              0x8021
#define EVENT_OBJECT_DRAGCANCEL             0x8022
#define EVENT_OBJECT_DRAGCOMPLETE           0x8023

#define EVENT_OBJECT_DRAGENTER              0x8024
#define EVENT_OBJECT_DRAGLEAVE              0x8025
#define EVENT_OBJECT_DRAGDROPPED            0x8026

/*
 * EVENT_OBJECT_IME_SHOW/HIDE
 * Sent by an IME window when it has become visible or invisible.
 */
#define EVENT_OBJECT_IME_SHOW               0x8027
#define EVENT_OBJECT_IME_HIDE               0x8028

/*
 * EVENT_OBJECT_IME_CHANGE
 * Sent by an IME window whenever it changes size or position.
 */
#define EVENT_OBJECT_IME_CHANGE             0x8029

#define EVENT_OBJECT_TEXTEDIT_CONVERSIONTARGETCHANGED 0x8030

#endif /* _WIN32_WINNT >= 0x0602 */

#if(_WIN32_WINNT >= 0x0601)
#define EVENT_OBJECT_END                    0x80FF

#define EVENT_AIA_START                     0xA000
#define EVENT_AIA_END                       0xAFFF
#endif /* _WIN32_WINNT >= 0x0601 */


/*
 * Child IDs
 */


/*
 * System Sounds (idChild of system SOUND notification)
 */
#define SOUND_SYSTEM_STARTUP            1
#define SOUND_SYSTEM_SHUTDOWN           2
#define SOUND_SYSTEM_BEEP               3
#define SOUND_SYSTEM_ERROR              4
#define SOUND_SYSTEM_QUESTION           5
#define SOUND_SYSTEM_WARNING            6
#define SOUND_SYSTEM_INFORMATION        7
#define SOUND_SYSTEM_MAXIMIZE           8
#define SOUND_SYSTEM_MINIMIZE           9
#define SOUND_SYSTEM_RESTOREUP          10
#define SOUND_SYSTEM_RESTOREDOWN        11
#define SOUND_SYSTEM_APPSTART           12
#define SOUND_SYSTEM_FAULT              13
#define SOUND_SYSTEM_APPEND             14
#define SOUND_SYSTEM_MENUCOMMAND        15
#define SOUND_SYSTEM_MENUPOPUP          16
#define CSOUND_SYSTEM                   16

/*
 * System Alerts (indexChild of system ALERT notification)
 */
#define ALERT_SYSTEM_INFORMATIONAL      1       // MB_INFORMATION
#define ALERT_SYSTEM_WARNING            2       // MB_WARNING
#define ALERT_SYSTEM_ERROR              3       // MB_ERROR
#define ALERT_SYSTEM_QUERY              4       // MB_QUESTION
#define ALERT_SYSTEM_CRITICAL           5       // HardSysErrBox
#define CALERT_SYSTEM                   6

#pragma region Desktop Family
#if WINAPI_FAMILY_PARTITION(WINAPI_PARTITION_DESKTOP)

typedef struct tagGUITHREADINFO
{
    DWORD   cbSize;
    GUITHREADINFO_FLAGS   flags;
    HWND    hwndActive;
    HWND    hwndFocus;
    HWND    hwndCapture;
    HWND    hwndMenuOwner;
    HWND    hwndMoveSize;
    HWND    hwndCaret;
    RECT    rcCaret;
} GUITHREADINFO, *PGUITHREADINFO, FAR * LPGUITHREADINFO;

#endif /* WINAPI_FAMILY_PARTITION(WINAPI_PARTITION_DESKTOP) */
#pragma endregion

#define GUI_CARETBLINKING   0x00000001
#define GUI_INMOVESIZE      0x00000002
#define GUI_INMENUMODE      0x00000004
#define GUI_SYSTEMMENUMODE  0x00000008
#define GUI_POPUPMENUMODE   0x00000010
#if(_WIN32_WINNT >= 0x0501)
#if defined(_WIN64)
#define GUI_16BITTASK       0x00000000
#else
#define GUI_16BITTASK       0x00000020
#endif
#endif /* _WIN32_WINNT >= 0x0501 */

#pragma region Desktop Family
#if WINAPI_FAMILY_PARTITION(WINAPI_PARTITION_DESKTOP)

_Win32_metadata_set_last_error_
WINUSERAPI
BOOL
WINAPI
GetGUIThreadInfo(
    _In_ DWORD idThread,
    _Inout_ PGUITHREADINFO pgui);

_Win32_metadata_set_last_error_
WINUSERAPI
BOOL
WINAPI
BlockInput(
    BOOL fBlockIt);

#if(_WIN32_WINNT >= 0x0600)

#define USER_DEFAULT_SCREEN_DPI 96

WINUSERAPI
BOOL
WINAPI
SetProcessDPIAware(
    VOID);

WINUSERAPI
BOOL
WINAPI
IsProcessDPIAware(
    VOID);

#endif /* _WIN32_WINNT >= 0x0600 */

#if(WINVER >= 0x0605)
WINUSERAPI
DPI_AWARENESS_CONTEXT
WINAPI
SetThreadDpiAwarenessContext(
    _In_ DPI_AWARENESS_CONTEXT dpiContext);

WINUSERAPI
DPI_AWARENESS_CONTEXT
WINAPI
GetThreadDpiAwarenessContext(
    VOID);

WINUSERAPI
DPI_AWARENESS_CONTEXT
WINAPI
GetWindowDpiAwarenessContext(
    _In_ HWND hwnd);

WINUSERAPI
DPI_AWARENESS
WINAPI
GetAwarenessFromDpiAwarenessContext(
    _In_ DPI_AWARENESS_CONTEXT value);

WINUSERAPI
UINT
WINAPI
GetDpiFromDpiAwarenessContext(
    _In_ DPI_AWARENESS_CONTEXT value);

WINUSERAPI
BOOL
WINAPI
AreDpiAwarenessContextsEqual(
    _In_ DPI_AWARENESS_CONTEXT dpiContextA,
    _In_ DPI_AWARENESS_CONTEXT dpiContextB);

WINUSERAPI
BOOL
WINAPI
IsValidDpiAwarenessContext(
    _In_ DPI_AWARENESS_CONTEXT value);

WINUSERAPI
UINT
WINAPI
GetDpiForWindow(
    _In_ HWND hwnd);

WINUSERAPI
UINT
WINAPI
GetDpiForSystem(
    VOID);

WINUSERAPI
UINT
WINAPI
GetSystemDpiForProcess(
    _In_ HANDLE hProcess);

_Win32_metadata_set_last_error_
WINUSERAPI
BOOL
WINAPI
EnableNonClientDpiScaling(
    _In_ HWND hwnd);

WINUSERAPI
BOOL
WINAPI
InheritWindowMonitor(
    _In_ HWND hwnd,
    _In_opt_ HWND hwndInherit);

#endif /* WINVER >= 0x0605 */

#if(WINVER >= 0x0605)
_Win32_metadata_set_last_error_
WINUSERAPI
BOOL
WINAPI
SetProcessDpiAwarenessContext(
    _In_ DPI_AWARENESS_CONTEXT value);

#endif /* WINVER >= 0x0605 */


#if (NTDDI_VERSION >= NTDDI_WIN10_19H1)

WINUSERAPI
DPI_AWARENESS_CONTEXT
WINAPI
GetDpiAwarenessContextForProcess(
    _In_ HANDLE hProcess);

#endif // NTDDI_VERSION >= NTDDI_WIN10_19H1

#if(WINVER >= 0x0606)

WINUSERAPI
DPI_HOSTING_BEHAVIOR
WINAPI
SetThreadDpiHostingBehavior(
    _In_ DPI_HOSTING_BEHAVIOR value);

WINUSERAPI
DPI_HOSTING_BEHAVIOR
WINAPI
GetThreadDpiHostingBehavior(VOID);

WINUSERAPI
DPI_HOSTING_BEHAVIOR
WINAPI
GetWindowDpiHostingBehavior(
    _In_ HWND hwnd);

#endif /* WINVER >= 0x0606 */


WINUSERAPI
UINT
WINAPI
GetWindowModuleFileNameA(
    _In_ HWND hwnd,
    _Out_writes_to_(cchFileNameMax, return) LPSTR pszFileName,
    _In_ UINT cchFileNameMax);
WINUSERAPI
UINT
WINAPI
GetWindowModuleFileNameW(
    _In_ HWND hwnd,
    _Out_writes_to_(cchFileNameMax, return) LPWSTR pszFileName,
    _In_ UINT cchFileNameMax);
#ifdef UNICODE
#define GetWindowModuleFileName  GetWindowModuleFileNameW
#else
#define GetWindowModuleFileName  GetWindowModuleFileNameA
#endif // !UNICODE

#endif /* WINAPI_FAMILY_PARTITION(WINAPI_PARTITION_DESKTOP) */
#pragma endregion

#ifndef NO_STATE_FLAGS
#define STATE_SYSTEM_UNAVAILABLE        0x00000001  // Disabled
#define STATE_SYSTEM_SELECTED           0x00000002
#define STATE_SYSTEM_FOCUSED            0x00000004
#define STATE_SYSTEM_PRESSED            0x00000008
#define STATE_SYSTEM_CHECKED            0x00000010
#define STATE_SYSTEM_MIXED              0x00000020  // 3-state checkbox or toolbar button
#define STATE_SYSTEM_INDETERMINATE      STATE_SYSTEM_MIXED
#define STATE_SYSTEM_READONLY           0x00000040
#define STATE_SYSTEM_HOTTRACKED         0x00000080
#define STATE_SYSTEM_DEFAULT            0x00000100
#define STATE_SYSTEM_EXPANDED           0x00000200
#define STATE_SYSTEM_COLLAPSED          0x00000400
#define STATE_SYSTEM_BUSY               0x00000800
#define STATE_SYSTEM_FLOATING           0x00001000  // Children "owned" not "contained" by parent
#define STATE_SYSTEM_MARQUEED           0x00002000
#define STATE_SYSTEM_ANIMATED           0x00004000
#define STATE_SYSTEM_INVISIBLE          0x00008000
#define STATE_SYSTEM_OFFSCREEN          0x00010000
#define STATE_SYSTEM_SIZEABLE           0x00020000
#define STATE_SYSTEM_MOVEABLE           0x00040000
#define STATE_SYSTEM_SELFVOICING        0x00080000
#define STATE_SYSTEM_FOCUSABLE          0x00100000
#define STATE_SYSTEM_SELECTABLE         0x00200000
#define STATE_SYSTEM_LINKED             0x00400000
#define STATE_SYSTEM_TRAVERSED          0x00800000
#define STATE_SYSTEM_MULTISELECTABLE    0x01000000  // Supports multiple selection
#define STATE_SYSTEM_EXTSELECTABLE      0x02000000  // Supports extended selection
#define STATE_SYSTEM_ALERT_LOW          0x04000000  // This information is of low priority
#define STATE_SYSTEM_ALERT_MEDIUM       0x08000000  // This information is of medium priority
#define STATE_SYSTEM_ALERT_HIGH         0x10000000  // This information is of high priority
#define STATE_SYSTEM_PROTECTED          0x20000000  // access to this is restricted
#define STATE_SYSTEM_VALID              0x3FFFFFFF
#endif

#define CCHILDREN_TITLEBAR              5
#define CCHILDREN_SCROLLBAR             5

#pragma region Desktop Family or Games Family
#if WINAPI_FAMILY_PARTITION(WINAPI_PARTITION_DESKTOP | WINAPI_PARTITION_GAMES)

/*
 * Information about the global cursor.
 */
typedef struct tagCURSORINFO
{
    DWORD   cbSize;
    CURSORINFO_FLAGS   flags;
    HCURSOR hCursor;
    POINT   ptScreenPos;
} CURSORINFO, *PCURSORINFO, *LPCURSORINFO;

#define CURSOR_INVISIBLE   0x00000000
#define CURSOR_SHOWING     0x00000001
#if(WINVER >= 0x0602)
#define CURSOR_SUPPRESSED  0x00000002
#endif /* WINVER >= 0x0602 */

_Win32_metadata_set_last_error_
WINUSERAPI
BOOL
WINAPI
GetCursorInfo(
    _Inout_ PCURSORINFO pci);

#endif /* WINAPI_FAMILY_PARTITION(WINAPI_PARTITION_DESKTOP | WINAPI_PARTITION_GAMES) */
#pragma endregion

#pragma region Desktop Family
#if WINAPI_FAMILY_PARTITION(WINAPI_PARTITION_DESKTOP)

/*
 * Window information snapshot
 */
typedef struct tagWINDOWINFO
{
    DWORD cbSize;
    RECT rcWindow;
    RECT rcClient;
    WINDOW_STYLE dwStyle;
    WINDOW_EX_STYLE dwExStyle;
    DWORD dwWindowStatus;
    UINT cxWindowBorders;
    UINT cyWindowBorders;
    ATOM atomWindowType;
    WORD wCreatorVersion;
} WINDOWINFO, *PWINDOWINFO, *LPWINDOWINFO;

#define WS_ACTIVECAPTION    0x0001

_Win32_metadata_set_last_error_
WINUSERAPI
BOOL
WINAPI
GetWindowInfo(
    _In_ HWND hwnd,
    _Inout_ PWINDOWINFO pwi);

/*
 * Titlebar information.
 */
typedef struct tagTITLEBARINFO
{
    DWORD cbSize;
    RECT rcTitleBar;
    DWORD rgstate[CCHILDREN_TITLEBAR + 1];
} TITLEBARINFO, *PTITLEBARINFO, *LPTITLEBARINFO;

_Win32_metadata_set_last_error_
WINUSERAPI
BOOL
WINAPI
GetTitleBarInfo(
    _In_ HWND hwnd,
    _Inout_ PTITLEBARINFO pti);

#if(WINVER >= 0x0600)
typedef struct tagTITLEBARINFOEX
{
    DWORD cbSize;
    RECT rcTitleBar;
    DWORD rgstate[CCHILDREN_TITLEBAR + 1];
    RECT rgrect[CCHILDREN_TITLEBAR + 1];
} TITLEBARINFOEX, *PTITLEBARINFOEX, *LPTITLEBARINFOEX;
#endif /* WINVER >= 0x0600 */

/*
 * Menubar information
 */
typedef struct tagMENUBARINFO
{
    DWORD cbSize;
    RECT rcBar;          // rect of bar, popup, item
    HMENU hMenu;         // real menu handle of bar, popup
    HWND hwndMenu;       // hwnd of item submenu if one
    BOOL fBarFocused:1;  // bar, popup has the focus
    BOOL fFocused:1;     // item has the focus
    BOOL fUnused:30;     // reserved
} MENUBARINFO, *PMENUBARINFO, *LPMENUBARINFO;

_Win32_metadata_set_last_error_
WINUSERAPI
BOOL
WINAPI
GetMenuBarInfo(
    _In_ HWND hwnd,
    _In_ OBJECT_IDENTIFIER idObject,
    _In_ LONG idItem,
    _Inout_ PMENUBARINFO pmbi);

/*
 * Scrollbar information
 */
typedef struct tagSCROLLBARINFO
{
    DWORD cbSize;
    RECT rcScrollBar;
    int dxyLineButton;
    int xyThumbTop;
    int xyThumbBottom;
    int reserved;
    DWORD rgstate[CCHILDREN_SCROLLBAR + 1];
} SCROLLBARINFO, *PSCROLLBARINFO, *LPSCROLLBARINFO;

_Win32_metadata_set_last_error_
WINUSERAPI
BOOL
WINAPI
GetScrollBarInfo(
    _In_ HWND hwnd,
    _In_ OBJECT_IDENTIFIER idObject,
    _Inout_ PSCROLLBARINFO psbi);

/*
 * Combobox information
 */
typedef struct tagCOMBOBOXINFO
{
    DWORD cbSize;
    RECT rcItem;
    RECT rcButton;
    DWORD stateButton;
    HWND hwndCombo;
    HWND hwndItem;
    HWND hwndList;
} COMBOBOXINFO, *PCOMBOBOXINFO, *LPCOMBOBOXINFO;

_Win32_metadata_set_last_error_
WINUSERAPI
BOOL
WINAPI
GetComboBoxInfo(
    _In_ HWND hwndCombo,
    _Inout_ PCOMBOBOXINFO pcbi);

#endif /* WINAPI_FAMILY_PARTITION(WINAPI_PARTITION_DESKTOP) */
#pragma endregion

/*
 * The "real" ancestor window
 */
#define     GA_PARENT       1
#define     GA_ROOT         2
#define     GA_ROOTOWNER    3

#pragma region Desktop Family
#if WINAPI_FAMILY_PARTITION(WINAPI_PARTITION_DESKTOP)

WINUSERAPI
HWND
WINAPI
GetAncestor(
    _In_ HWND hwnd,
    _In_ GET_ANCESTOR_FLAGS gaFlags);


/*
 * This gets the REAL child window at the point.  If it is in the dead
 * space of a group box, it will try a sibling behind it.  But static
 * fields will get returned.  In other words, it is kind of a cross between
 * ChildWindowFromPointEx and WindowFromPoint.
 */
WINUSERAPI
HWND
WINAPI
RealChildWindowFromPoint(
    _In_ HWND hwndParent,
    _In_ POINT ptParentClientCoords);


/*
 * This gets the name of the window TYPE, not class.  This allows us to
 * recognize ThunderButton32 et al.
 */
WINUSERAPI
UINT
WINAPI
RealGetWindowClassA(
    _In_ HWND hwnd,
    _Out_writes_to_(cchClassNameMax, return) LPSTR ptszClassName,
    _In_ UINT cchClassNameMax);
/*
 * This gets the name of the window TYPE, not class.  This allows us to
 * recognize ThunderButton32 et al.
 */
_Win32_metadata_set_last_error_
WINUSERAPI
UINT
WINAPI
RealGetWindowClassW(
    _In_ HWND hwnd,
    _Out_writes_to_(cchClassNameMax, return) LPWSTR ptszClassName,
    _In_ UINT cchClassNameMax);
#ifdef UNICODE
#define RealGetWindowClass  RealGetWindowClassW
#else
#define RealGetWindowClass  RealGetWindowClassA
#endif // !UNICODE

/*
 * Alt-Tab Switch window information.
 */
typedef struct tagALTTABINFO
{
    DWORD cbSize;
    int cItems;
    int cColumns;
    int cRows;
    int iColFocus;
    int iRowFocus;
    int cxItem;
    int cyItem;
    POINT ptStart;
} ALTTABINFO, *PALTTABINFO, *LPALTTABINFO;

_Win32_metadata_set_last_error_
WINUSERAPI
BOOL
WINAPI
GetAltTabInfoA(
    _In_opt_ HWND hwnd,
    _In_ int iItem,
    _Inout_ PALTTABINFO pati,
    _Out_writes_opt_(cchItemText) LPSTR pszItemText,
    _In_ UINT cchItemText);
_Win32_metadata_set_last_error_
WINUSERAPI
BOOL
WINAPI
GetAltTabInfoW(
    _In_opt_ HWND hwnd,
    _In_ int iItem,
    _Inout_ PALTTABINFO pati,
    _Out_writes_opt_(cchItemText) LPWSTR pszItemText,
    _In_ UINT cchItemText);
#ifdef UNICODE
#define GetAltTabInfo  GetAltTabInfoW
#else
#define GetAltTabInfo  GetAltTabInfoA
#endif // !UNICODE

/*
 * Listbox information.
 * Returns the number of items per row.
 */
WINUSERAPI
DWORD
WINAPI
GetListBoxInfo(
    _In_ HWND hwnd);

#endif /* WINAPI_FAMILY_PARTITION(WINAPI_PARTITION_DESKTOP) */
#pragma endregion

#endif /* NOWINABLE */
#endif /* WINVER >= 0x0500 */

#pragma region Desktop Family
#if WINAPI_FAMILY_PARTITION(WINAPI_PARTITION_DESKTOP)


#if(_WIN32_WINNT >= 0x0500)
_Win32_metadata_set_last_error_
WINUSERAPI
BOOL
WINAPI
LockWorkStation(
    VOID);
#endif /* _WIN32_WINNT >= 0x0500 */

#if(_WIN32_WINNT >= 0x0500)

_Win32_metadata_set_last_error_
WINUSERAPI
BOOL
WINAPI
UserHandleGrantAccess(
    _In_ HANDLE hUserHandle,
    _In_ HANDLE hJob,
    _In_ BOOL   bGrant);

#endif /* _WIN32_WINNT >= 0x0500 */

#endif /* WINAPI_FAMILY_PARTITION(WINAPI_PARTITION_DESKTOP) */
#pragma endregion

#if(_WIN32_WINNT >= 0x0501)

/*
 * Raw Input Messages.
 */

#pragma region Desktop Family
#if WINAPI_FAMILY_PARTITION(WINAPI_PARTITION_DESKTOP)

DECLARE_HANDLE(HRAWINPUT);

#endif /* WINAPI_FAMILY_PARTITION(WINAPI_PARTITION_DESKTOP) */
#pragma endregion

/*
 * WM_INPUT wParam
 */

/*
 * Use this macro to get the input code from wParam.
 */
#define GET_RAWINPUT_CODE_WPARAM(wParam)    ((wParam) & 0xff)

/*
 * The input is in the regular message flow,
 * the app is required to call DefWindowProc
 * so that the system can perform clean ups.
 */
#define RIM_INPUT       0

/*
 * The input is sink only. The app is expected
 * to behave nicely.
 */
#define RIM_INPUTSINK   1

#pragma region Desktop Family
#if WINAPI_FAMILY_PARTITION(WINAPI_PARTITION_DESKTOP)

/*
 * Raw Input data header
 */
typedef struct tagRAWINPUTHEADER {
    DWORD dwType;
    DWORD dwSize;
    HANDLE hDevice;
    WPARAM wParam;
} RAWINPUTHEADER, *PRAWINPUTHEADER, *LPRAWINPUTHEADER;

#endif /* WINAPI_FAMILY_PARTITION(WINAPI_PARTITION_DESKTOP) */
#pragma endregion

/*
 * Type of the raw input
 */
#define RIM_TYPEMOUSE       0
#define RIM_TYPEKEYBOARD    1
#define RIM_TYPEHID         2
#define RIM_TYPEMAX         2

#pragma region Desktop Family
#if WINAPI_FAMILY_PARTITION(WINAPI_PARTITION_DESKTOP)

//Disable warning C4201:nameless struct/union
#if _MSC_VER >= 1200
#pragma warning(push)
#endif
#pragma warning(disable : 4201)

/*
 * Raw format of the mouse input
 */
typedef struct tagRAWMOUSE {
    /*
     * Indicator flags.
     */
    USHORT usFlags;

    /*
     * The transition state of the mouse buttons.
     */
    union {
        ULONG ulButtons;
        struct  {
            USHORT  usButtonFlags;
            USHORT  usButtonData;
        } DUMMYSTRUCTNAME;
    } DUMMYUNIONNAME;


    /*
     * The raw state of the mouse buttons.
     */
    ULONG ulRawButtons;

    /*
     * The signed relative or absolute motion in the X direction.
     */
    LONG lLastX;

    /*
     * The signed relative or absolute motion in the Y direction.
     */
    LONG lLastY;

    /*
     * Device-specific additional information for the event.
     */
    ULONG ulExtraInformation;

} RAWMOUSE, *PRAWMOUSE, *LPRAWMOUSE;

#if _MSC_VER >= 1200
#pragma warning(pop)
#endif

#endif /* WINAPI_FAMILY_PARTITION(WINAPI_PARTITION_DESKTOP) */
#pragma endregion

/*
 * Define the mouse button state indicators.
 */

#define RI_MOUSE_LEFT_BUTTON_DOWN   0x0001  // Left Button changed to down.
#define RI_MOUSE_LEFT_BUTTON_UP     0x0002  // Left Button changed to up.
#define RI_MOUSE_RIGHT_BUTTON_DOWN  0x0004  // Right Button changed to down.
#define RI_MOUSE_RIGHT_BUTTON_UP    0x0008  // Right Button changed to up.
#define RI_MOUSE_MIDDLE_BUTTON_DOWN 0x0010  // Middle Button changed to down.
#define RI_MOUSE_MIDDLE_BUTTON_UP   0x0020  // Middle Button changed to up.

#define RI_MOUSE_BUTTON_1_DOWN      RI_MOUSE_LEFT_BUTTON_DOWN
#define RI_MOUSE_BUTTON_1_UP        RI_MOUSE_LEFT_BUTTON_UP
#define RI_MOUSE_BUTTON_2_DOWN      RI_MOUSE_RIGHT_BUTTON_DOWN
#define RI_MOUSE_BUTTON_2_UP        RI_MOUSE_RIGHT_BUTTON_UP
#define RI_MOUSE_BUTTON_3_DOWN      RI_MOUSE_MIDDLE_BUTTON_DOWN
#define RI_MOUSE_BUTTON_3_UP        RI_MOUSE_MIDDLE_BUTTON_UP

#define RI_MOUSE_BUTTON_4_DOWN      0x0040
#define RI_MOUSE_BUTTON_4_UP        0x0080
#define RI_MOUSE_BUTTON_5_DOWN      0x0100
#define RI_MOUSE_BUTTON_5_UP        0x0200

/*
 * If usButtonFlags has RI_MOUSE_WHEEL, the wheel delta is stored in usButtonData.
 * Take it as a signed value.
 */
#define RI_MOUSE_WHEEL              0x0400
#if(WINVER >= 0x0600)
#define RI_MOUSE_HWHEEL             0x0800
#endif /* WINVER >= 0x0600 */

/*
 * Define the mouse indicator flags.
 */
#define MOUSE_MOVE_RELATIVE         0
#define MOUSE_MOVE_ABSOLUTE         1
#define MOUSE_VIRTUAL_DESKTOP    0x02  // the coordinates are mapped to the virtual desktop
#define MOUSE_ATTRIBUTES_CHANGED 0x04  // requery for mouse attributes
#if(WINVER >= 0x0600)
#define MOUSE_MOVE_NOCOALESCE    0x08  // do not coalesce mouse moves
#endif /* WINVER >= 0x0600 */
// NOTE: 0x100 is already claimed by MOUSE_TERMSRV_SRC_SHADOW in ntddmou.w

#pragma region Desktop Family
#if WINAPI_FAMILY_PARTITION(WINAPI_PARTITION_DESKTOP)

/*
 * Raw format of the keyboard input
 */
typedef struct tagRAWKEYBOARD {
    /*
     * The "make" scan code (key depression).
     */
    USHORT MakeCode;

    /*
     * The flags field indicates a "break" (key release) and other
     * miscellaneous scan code information defined in ntddkbd.h.
     */
    USHORT Flags;

    USHORT Reserved;

    /*
     * Windows message compatible information
     */
    USHORT VKey;
    UINT   Message;

    /*
     * Device-specific additional information for the event.
     */
    ULONG ExtraInformation;


} RAWKEYBOARD, *PRAWKEYBOARD, *LPRAWKEYBOARD;

#endif /* WINAPI_FAMILY_PARTITION(WINAPI_PARTITION_DESKTOP) */
#pragma endregion

/*
 * Define the keyboard overrun MakeCode.
 */

#define KEYBOARD_OVERRUN_MAKE_CODE    0xFF

/*
 * Define the keyboard input data Flags.
 */
#define RI_KEY_MAKE             0
#define RI_KEY_BREAK            1
#define RI_KEY_E0               2
#define RI_KEY_E1               4
#define RI_KEY_TERMSRV_SET_LED  8
#define RI_KEY_TERMSRV_SHADOW   0x10

#pragma region Desktop Family
#if WINAPI_FAMILY_PARTITION(WINAPI_PARTITION_DESKTOP)

/*
 * Raw format of the input from Human Input Devices
 */
typedef struct tagRAWHID {
    DWORD dwSizeHid;    // byte size of each report
    DWORD dwCount;      // number of input packed
    BYTE bRawData[1];
} RAWHID, *PRAWHID, *LPRAWHID;

#endif /* WINAPI_FAMILY_PARTITION(WINAPI_PARTITION_DESKTOP) */
#pragma endregion

/*
 * RAWINPUT data structure.
 */

#pragma region Desktop Family
#if WINAPI_FAMILY_PARTITION(WINAPI_PARTITION_DESKTOP)

typedef struct tagRAWINPUT {
    RAWINPUTHEADER header;
    union {
        RAWMOUSE    mouse;
        RAWKEYBOARD keyboard;
        RAWHID      hid;
    } data;
} RAWINPUT, *PRAWINPUT, *LPRAWINPUT;

#endif /* WINAPI_FAMILY_PARTITION(WINAPI_PARTITION_DESKTOP) */
#pragma endregion

#ifdef _WIN64
#define RAWINPUT_ALIGN(x)   (((x) + sizeof(QWORD) - 1) & ~(sizeof(QWORD) - 1))
#else   // _WIN64
#define RAWINPUT_ALIGN(x)   (((x) + sizeof(DWORD) - 1) & ~(sizeof(DWORD) - 1))
#endif  // _WIN64

#define NEXTRAWINPUTBLOCK(ptr) ((PRAWINPUT)RAWINPUT_ALIGN((ULONG_PTR)((PBYTE)(ptr) + (ptr)->header.dwSize)))

/*
 * Flags for GetRawInputData
 */

#define RID_INPUT               0x10000003
#define RID_HEADER              0x10000005

#pragma region Desktop Family
#if WINAPI_FAMILY_PARTITION(WINAPI_PARTITION_DESKTOP)

WINUSERAPI
UINT
WINAPI
GetRawInputData(
    _In_ HRAWINPUT hRawInput,
    _In_ RAW_INPUT_DATA_COMMAND_FLAGS uiCommand,
    _Out_writes_bytes_to_opt_(*pcbSize, return) LPVOID pData,
    _Inout_ PUINT pcbSize,
    _In_ UINT cbSizeHeader);

#endif /* WINAPI_FAMILY_PARTITION(WINAPI_PARTITION_DESKTOP) */
#pragma endregion

/*
 * Raw Input Device Information
 */
#define RIDI_PREPARSEDDATA      0x20000005
#define RIDI_DEVICENAME         0x20000007  // the return valus is the character length, not the byte size
#define RIDI_DEVICEINFO         0x2000000b

#pragma region Desktop Family
#if WINAPI_FAMILY_PARTITION(WINAPI_PARTITION_DESKTOP)

typedef struct tagRID_DEVICE_INFO_MOUSE {
    DWORD dwId;
    DWORD dwNumberOfButtons;
    DWORD dwSampleRate;
    BOOL  fHasHorizontalWheel;
} RID_DEVICE_INFO_MOUSE, *PRID_DEVICE_INFO_MOUSE;

typedef struct tagRID_DEVICE_INFO_KEYBOARD {
    DWORD dwType;
    DWORD dwSubType;
    DWORD dwKeyboardMode;
    DWORD dwNumberOfFunctionKeys;
    DWORD dwNumberOfIndicators;
    DWORD dwNumberOfKeysTotal;
} RID_DEVICE_INFO_KEYBOARD, *PRID_DEVICE_INFO_KEYBOARD;

typedef struct tagRID_DEVICE_INFO_HID {
    DWORD dwVendorId;
    DWORD dwProductId;
    DWORD dwVersionNumber;

    /*
     * Top level collection UsagePage and Usage
     */
    USHORT usUsagePage;
    USHORT usUsage;
} RID_DEVICE_INFO_HID, *PRID_DEVICE_INFO_HID;

typedef struct tagRID_DEVICE_INFO {
    DWORD cbSize;
    RID_DEVICE_INFO_TYPE dwType;
    union {
        RID_DEVICE_INFO_MOUSE mouse;
        RID_DEVICE_INFO_KEYBOARD keyboard;
        RID_DEVICE_INFO_HID hid;
    } DUMMYUNIONNAME;
} RID_DEVICE_INFO, *PRID_DEVICE_INFO, *LPRID_DEVICE_INFO;

_Win32_metadata_set_last_error_
WINUSERAPI
UINT
WINAPI
GetRawInputDeviceInfoA(
    _In_opt_ HANDLE hDevice,
    _In_ RAW_INPUT_DEVICE_INFO_COMMAND uiCommand,
    _Inout_updates_bytes_to_opt_(*pcbSize, *pcbSize) LPVOID pData,
    _Inout_ PUINT pcbSize);
_Win32_metadata_set_last_error_
WINUSERAPI
UINT
WINAPI
GetRawInputDeviceInfoW(
    _In_opt_ HANDLE hDevice,
    _In_ RAW_INPUT_DEVICE_INFO_COMMAND uiCommand,
    _Inout_updates_bytes_to_opt_(*pcbSize, *pcbSize) LPVOID pData,
    _Inout_ PUINT pcbSize);
#ifdef UNICODE
#define GetRawInputDeviceInfo  GetRawInputDeviceInfoW
#else
#define GetRawInputDeviceInfo  GetRawInputDeviceInfoA
#endif // !UNICODE


/*
 * Raw Input Bulk Read: GetRawInputBuffer
 */
_Win32_metadata_set_last_error_
WINUSERAPI
UINT
WINAPI
GetRawInputBuffer(
    _Out_writes_bytes_opt_(*pcbSize) PRAWINPUT pData,
    _Inout_ PUINT pcbSize,
    _In_ UINT cbSizeHeader);

/*
 * Raw Input request APIs
 */
typedef struct tagRAWINPUTDEVICE {
    USHORT usUsagePage; // Toplevel collection UsagePage
    USHORT usUsage;     // Toplevel collection Usage
    RAWINPUTDEVICE_FLAGS dwFlags;
    HWND hwndTarget;    // Target hwnd. NULL = follows keyboard focus
} RAWINPUTDEVICE, *PRAWINPUTDEVICE, *LPRAWINPUTDEVICE;

typedef CONST RAWINPUTDEVICE* PCRAWINPUTDEVICE;

#endif /* WINAPI_FAMILY_PARTITION(WINAPI_PARTITION_DESKTOP) */
#pragma endregion

#define RIDEV_REMOVE            0x00000001
#define RIDEV_EXCLUDE           0x00000010
#define RIDEV_PAGEONLY          0x00000020
#define RIDEV_NOLEGACY          0x00000030
#define RIDEV_INPUTSINK         0x00000100
#define RIDEV_CAPTUREMOUSE      0x00000200  // effective when mouse nolegacy is specified, otherwise it would be an error
#define RIDEV_NOHOTKEYS         0x00000200  // effective for keyboard.
#define RIDEV_APPKEYS           0x00000400  // effective for keyboard.
#if(_WIN32_WINNT >= 0x0501)
#define RIDEV_EXINPUTSINK       0x00001000
#define RIDEV_DEVNOTIFY         0x00002000
#endif /* _WIN32_WINNT >= 0x0501 */
#define RIDEV_EXMODEMASK        0x000000F0

#define RIDEV_EXMODE(mode)  ((mode) & RIDEV_EXMODEMASK)

#if(_WIN32_WINNT >= 0x0501)
/*
 * Flags for the WM_INPUT_DEVICE_CHANGE message.
 */
#define GIDC_ARRIVAL             1
#define GIDC_REMOVAL             2
#endif /* _WIN32_WINNT >= 0x0501 */

#if (_WIN32_WINNT >= 0x0601)
#define GET_DEVICE_CHANGE_WPARAM(wParam)  (LOWORD(wParam))
#elif (_WIN32_WINNT >= 0x0501)
#define GET_DEVICE_CHANGE_LPARAM(lParam)  (LOWORD(lParam))
#endif /* (_WIN32_WINNT >= 0x0601) */

#pragma region Desktop Family
#if WINAPI_FAMILY_PARTITION(WINAPI_PARTITION_DESKTOP)

_Win32_metadata_set_last_error_
WINUSERAPI
BOOL
WINAPI
RegisterRawInputDevices(
    _In_reads_(uiNumDevices) PCRAWINPUTDEVICE pRawInputDevices,
    _In_ UINT uiNumDevices,
    _In_ UINT cbSize);

_Win32_metadata_set_last_error_
WINUSERAPI
UINT
WINAPI
GetRegisteredRawInputDevices(
    _Out_writes_opt_( *puiNumDevices) PRAWINPUTDEVICE pRawInputDevices,
    _Inout_ PUINT puiNumDevices,
    _In_ UINT cbSize);


typedef struct tagRAWINPUTDEVICELIST {
    HANDLE hDevice;
    RID_DEVICE_INFO_TYPE dwType;
} RAWINPUTDEVICELIST, *PRAWINPUTDEVICELIST;

_Win32_metadata_set_last_error_
WINUSERAPI
UINT
WINAPI
GetRawInputDeviceList(
    _Out_writes_opt_(*puiNumDevices) PRAWINPUTDEVICELIST pRawInputDeviceList,
    _Inout_ PUINT puiNumDevices,
    _In_ UINT cbSize);

WINUSERAPI
LRESULT
WINAPI
DefRawInputProc(
    _In_reads_(nInput) PRAWINPUT* paRawInput,
    _In_ INT nInput,
    _In_ UINT cbSizeHeader);

#endif /* WINAPI_FAMILY_PARTITION(WINAPI_PARTITION_DESKTOP) */
#pragma endregion

#endif /* _WIN32_WINNT >= 0x0501 */


#if(WINVER >= 0x0602)

#define POINTER_DEVICE_PRODUCT_STRING_MAX 520
/*
 * wParam values for WM_POINTERDEVICECHANGE
 */
#define PDC_ARRIVAL                   0x001
#define PDC_REMOVAL                   0x002
#define PDC_ORIENTATION_0             0x004
#define PDC_ORIENTATION_90            0x008
#define PDC_ORIENTATION_180           0x010
#define PDC_ORIENTATION_270           0x020
#define PDC_MODE_DEFAULT              0x040
#define PDC_MODE_CENTERED             0x080
#define PDC_MAPPING_CHANGE            0x100
#define PDC_RESOLUTION                0x200
#define PDC_ORIGIN                    0x400
#define PDC_MODE_ASPECTRATIOPRESERVED 0x800

#pragma region Desktop Family
#if WINAPI_FAMILY_PARTITION(WINAPI_PARTITION_DESKTOP)

typedef enum tagPOINTER_DEVICE_TYPE {
    POINTER_DEVICE_TYPE_INTEGRATED_PEN = 0x00000001,
    POINTER_DEVICE_TYPE_EXTERNAL_PEN   = 0x00000002,
    POINTER_DEVICE_TYPE_TOUCH          = 0x00000003,
#if(WINVER >= 0x0603)
    POINTER_DEVICE_TYPE_TOUCH_PAD      = 0x00000004,
#endif /* WINVER >= 0x0603 */
    POINTER_DEVICE_TYPE_MAX            = 0xFFFFFFFF
} POINTER_DEVICE_TYPE;

typedef struct tagPOINTER_DEVICE_INFO {
    DWORD displayOrientation;
    HANDLE device;
    POINTER_DEVICE_TYPE pointerDeviceType;
    HMONITOR monitor;
    ULONG startingCursorId;
    USHORT maxActiveContacts;
    WCHAR productString[POINTER_DEVICE_PRODUCT_STRING_MAX];
} POINTER_DEVICE_INFO;

typedef struct tagPOINTER_DEVICE_PROPERTY {
    INT32 logicalMin;
    INT32 logicalMax;
    INT32 physicalMin;
    INT32 physicalMax;
    UINT32 unit;
    UINT32 unitExponent;
    USHORT usagePageId;
    USHORT usageId;
} POINTER_DEVICE_PROPERTY;

typedef enum tagPOINTER_DEVICE_CURSOR_TYPE {
    POINTER_DEVICE_CURSOR_TYPE_UNKNOWN   = 0x00000000,
    POINTER_DEVICE_CURSOR_TYPE_TIP       = 0x00000001,
    POINTER_DEVICE_CURSOR_TYPE_ERASER    = 0x00000002,
    POINTER_DEVICE_CURSOR_TYPE_MAX       = 0xFFFFFFFF
} POINTER_DEVICE_CURSOR_TYPE;

typedef struct tagPOINTER_DEVICE_CURSOR_INFO {
    UINT32 cursorId;
    POINTER_DEVICE_CURSOR_TYPE cursor;
} POINTER_DEVICE_CURSOR_INFO;

_Win32_metadata_set_last_error_
WINUSERAPI
BOOL
WINAPI
GetPointerDevices(
    _Inout_ UINT32* deviceCount,
    _Out_writes_opt_(*deviceCount) POINTER_DEVICE_INFO *pointerDevices);

_Win32_metadata_set_last_error_
WINUSERAPI
BOOL
WINAPI
GetPointerDevice(
    _In_ HANDLE device,
    _Out_writes_(1) POINTER_DEVICE_INFO *pointerDevice);

_Win32_metadata_set_last_error_
WINUSERAPI
BOOL
WINAPI
GetPointerDeviceProperties(
    _In_ HANDLE device,
    _Inout_ UINT32* propertyCount,
    _Out_writes_opt_(*propertyCount) POINTER_DEVICE_PROPERTY *pointerProperties);

_Win32_metadata_set_last_error_
WINUSERAPI
BOOL
WINAPI
RegisterPointerDeviceNotifications(
    _In_ HWND window,
    _In_ BOOL notifyRange);

_Win32_metadata_set_last_error_
WINUSERAPI
BOOL
WINAPI
GetPointerDeviceRects(
    _In_ HANDLE device,
    _Out_writes_(1) RECT* pointerDeviceRect,
    _Out_writes_(1) RECT* displayRect);

_Win32_metadata_set_last_error_
WINUSERAPI
BOOL
WINAPI
GetPointerDeviceCursors(
    _In_ HANDLE device,
    _Inout_ UINT32* cursorCount,
    _Out_writes_opt_(*cursorCount) POINTER_DEVICE_CURSOR_INFO *deviceCursors);

_Win32_metadata_set_last_error_
WINUSERAPI
BOOL
WINAPI
GetRawPointerDeviceData(
    _In_ UINT32 pointerId,
    _In_ UINT32 historyCount,
    _In_ UINT32 propertiesCount,
    _In_reads_(propertiesCount) POINTER_DEVICE_PROPERTY* pProperties,
    _Out_writes_(historyCount * propertiesCount) LONG* pValues);


// Support for SPI_GETTOUCHPADPARAMETERS/SPI_SETTOUCHPADPARAMETERS
#if WINAPI_FAMILY_PARTITION(WINAPI_PARTITION_DESKTOP)
typedef enum LEGACY_TOUCHPAD_FEATURES {
    LEGACY_TOUCHPAD_FEATURE_NONE                     = 0x00000000,
    LEGACY_TOUCHPAD_FEATURE_ENABLE_DISABLE           = 0x00000001,
    LEGACY_TOUCHPAD_FEATURE_REVERSE_SCROLL_DIRECTION = 0x00000004,
} LEGACY_TOUCHPAD_FEATURES;

#ifndef MIDL_PASS
// Don't define this for MIDL compiler passes over winuser.h. Some of them
// don't include winnt.h (where DEFINE_ENUM_FLAG_OPERATORS is defined) and
// get compile errors.
DEFINE_ENUM_FLAG_OPERATORS(LEGACY_TOUCHPAD_FEATURES)
#endif

typedef enum TOUCHPAD_SENSITIVITY_LEVEL {
    TOUCHPAD_SENSITIVITY_LEVEL_MOST_SENSITIVE     = 0x00000000,
    TOUCHPAD_SENSITIVITY_LEVEL_HIGH_SENSITIVITY   = 0x00000001,
    TOUCHPAD_SENSITIVITY_LEVEL_MEDIUM_SENSITIVITY = 0x00000002,
    TOUCHPAD_SENSITIVITY_LEVEL_LOW_SENSITIVITY    = 0x00000003,
    TOUCHPAD_SENSITIVITY_LEVEL_LEAST_SENSITIVE    = 0x00000004,
} TOUCHPAD_SENSITIVITY_LEVEL;

// For a stable struct, use a numbered variant such as TOUCHPAD_PARAMETERS_VERSION_1 + TOUCHPAD_PARAMETERS_V1.
#define TOUCHPAD_PARAMETERS_VERSION_1 1

typedef struct TOUCHPAD_PARAMETERS_V1 {
    UINT versionNumber;

    // These are status fields calculated dynamically, rather than user settings.
    // Their values are ignored in SPI_SETTOUCHPADPARAMETERS.
    UINT maxSupportedContacts;
    LEGACY_TOUCHPAD_FEATURES legacyTouchpadFeatures;
    BOOL touchpadPresent             : 1;
    BOOL legacyTouchpadPresent       : 1;
    BOOL externalMousePresent        : 1;
    BOOL touchpadEnabled             : 1;
    BOOL touchpadActive              : 1;
    BOOL feedbackSupported           : 1;
    BOOL clickForceSupported         : 1;
    BOOL Reserved1                   : 25;

    // These correspond to user settings and can be changed via SPI_SETTOUCHPADPARAMETERS.
    BOOL allowActiveWhenMousePresent : 1;
    BOOL feedbackEnabled             : 1;
    BOOL tapEnabled                  : 1;
    BOOL tapAndDragEnabled           : 1;
    BOOL twoFingerTapEnabled         : 1;
    BOOL rightClickZoneEnabled       : 1;
    BOOL mouseAccelSettingHonored    : 1;
    BOOL panEnabled                  : 1;
    BOOL zoomEnabled                 : 1;
    BOOL scrollDirectionReversed     : 1;
    BOOL Reserved2                   : 22;
    TOUCHPAD_SENSITIVITY_LEVEL sensitivityLevel;
    UINT cursorSpeed;
    UINT feedbackIntensity;
    UINT clickForceSensitivity;
    UINT rightClickZoneWidth;
    UINT rightClickZoneHeight;
} TOUCHPAD_PARAMETERS_V1, *PTOUCHPAD_PARAMETERS_V1;

#if _MSC_VER >= 1200
#pragma warning(push)
#endif
#pragma warning( disable : 4201 )   // nonstandard extension used : nameless struct/union

#define TOUCHPAD_PARAMETERS_VERSION_2 0x2

#if defined(__cplusplus)
typedef struct tagTOUCHPAD_PARAMETERS_V2 : public TOUCHPAD_PARAMETERS_V1 {
    BOOL button1Supported            : 1;
    BOOL button2Supported            : 1;
    BOOL button3Supported            : 1;
    BOOL Reserved3                   : 29;
} TOUCHPAD_PARAMETERS_V2, *PTOUCHPAD_PARAMETERS_V2;
#else
typedef struct tagTOUCHPAD_PARAMETERS_V2 {
    TOUCHPAD_PARAMETERS_V1 DUMMYSTRUCTNAME;
    BOOL button1Supported            : 1;
    BOOL button2Supported            : 1;
    BOOL button3Supported            : 1;
    BOOL Reserved3                   : 29;
} TOUCHPAD_PARAMETERS_V2, *PTOUCHPAD_PARAMETERS_V2;
#endif

#if _MSC_VER >= 1200
#pragma warning(pop)
#endif

#endif // WINAPI_FAMILY_PARTITION(WINAPI_PARTITION_DESKTOP)


// TODO(47499024): Make public when Feature_TouchpadPublicApis3 is enabled

#endif /* WINAPI_FAMILY_PARTITION(WINAPI_PARTITION_DESKTOP) */
#pragma endregion

#endif /* WINVER >= 0x0602 */


#if(WINVER >= 0x0600)

/*
 * Message Filter
 */

#define MSGFLT_ADD 1
#define MSGFLT_REMOVE 2

#pragma region Desktop Family
#if WINAPI_FAMILY_PARTITION(WINAPI_PARTITION_DESKTOP)

_Win32_metadata_set_last_error_
WINUSERAPI
BOOL
WINAPI
ChangeWindowMessageFilter(
    _In_ UINT message,
    _In_ CHANGE_WINDOW_MESSAGE_FILTER_FLAGS dwFlag);

#endif /* WINAPI_FAMILY_PARTITION(WINAPI_PARTITION_DESKTOP) */
#pragma endregion

#endif /* WINVER >= 0x0600 */

#if(WINVER >= 0x0601)

/*
 * Message filter info values (CHANGEFILTERSTRUCT.ExtStatus)
 */
#define MSGFLTINFO_NONE                         (0)
#define MSGFLTINFO_ALREADYALLOWED_FORWND        (1)
#define MSGFLTINFO_ALREADYDISALLOWED_FORWND     (2)
#define MSGFLTINFO_ALLOWED_HIGHER               (3)

#pragma region Desktop Family
#if WINAPI_FAMILY_PARTITION(WINAPI_PARTITION_DESKTOP)

typedef struct tagCHANGEFILTERSTRUCT {
    DWORD cbSize;
    MSGFLTINFO_STATUS ExtStatus;
} CHANGEFILTERSTRUCT, *PCHANGEFILTERSTRUCT;

#endif /* WINAPI_FAMILY_PARTITION(WINAPI_PARTITION_DESKTOP) */
#pragma endregion

/*
 * Message filter action values (action parameter to ChangeWindowMessageFilterEx)
 */
#define MSGFLT_RESET                            (0)
#define MSGFLT_ALLOW                            (1)
#define MSGFLT_DISALLOW                         (2)

#pragma region Desktop Family
#if WINAPI_FAMILY_PARTITION(WINAPI_PARTITION_DESKTOP)

_Win32_metadata_set_last_error_
WINUSERAPI
BOOL
WINAPI
ChangeWindowMessageFilterEx(
    _In_ HWND hwnd,                                         // Window
    _In_ UINT message,                                      // WM_ message
    _In_ WINDOW_MESSAGE_FILTER_ACTION action,                                      // Message filter action value
    _Inout_opt_ PCHANGEFILTERSTRUCT pChangeFilterStruct);   // Optional

#endif /* WINAPI_FAMILY_PARTITION(WINAPI_PARTITION_DESKTOP) */
#pragma endregion

#endif /* WINVER >= 0x0601 */

#if(WINVER >= 0x0601)
#endif /* WINVER >= 0x0601 */

#if(WINVER >= 0x0601)

/*
 * Gesture defines and functions
 */

#pragma region Desktop Family
#if WINAPI_FAMILY_PARTITION(WINAPI_PARTITION_DESKTOP)

/*
 * Gesture information handle
 */
DECLARE_HANDLE(HGESTUREINFO);

#endif /* WINAPI_FAMILY_PARTITION(WINAPI_PARTITION_DESKTOP) */
#pragma endregion

/*
 * Gesture flags - GESTUREINFO.dwFlags
 */
#define GF_BEGIN                        0x00000001
#define GF_INERTIA                      0x00000002
#define GF_END                          0x00000004

/*
 * Gesture IDs
 */
#define GID_BEGIN                       1
#define GID_END                         2
#define GID_ZOOM                        3
#define GID_PAN                         4
#define GID_ROTATE                      5
#define GID_TWOFINGERTAP                6
#define GID_PRESSANDTAP                 7
#define GID_ROLLOVER                    GID_PRESSANDTAP

#pragma region Desktop Family
#if WINAPI_FAMILY_PARTITION(WINAPI_PARTITION_DESKTOP)

/*
 * Gesture information structure
 *   - Pass the HGESTUREINFO received in the WM_GESTURE message lParam into the
 *     GetGestureInfo function to retrieve this information.
 *   - If cbExtraArgs is non-zero, pass the HGESTUREINFO received in the WM_GESTURE
 *     message lParam into the GetGestureExtraArgs function to retrieve extended
 *     argument information.
 */
typedef struct tagGESTUREINFO {
    UINT cbSize;                    // size, in bytes, of this structure (including variable length Args field)
    DWORD dwFlags;                  // see GF_* flags
    DWORD dwID;                     // gesture ID, see GID_* defines
    HWND hwndTarget;                // handle to window targeted by this gesture
    POINTS ptsLocation;             // current location of this gesture
    DWORD dwInstanceID;             // internally used
    DWORD dwSequenceID;             // internally used
    ULONGLONG ullArguments;         // arguments for gestures whose arguments fit in 8 BYTES
    UINT cbExtraArgs;               // size, in bytes, of extra arguments, if any, that accompany this gesture
} GESTUREINFO, *PGESTUREINFO;
typedef GESTUREINFO const * PCGESTUREINFO;


/*
 * Gesture notification structure
 *   - The WM_GESTURENOTIFY message lParam contains a pointer to this structure.
 *   - The WM_GESTURENOTIFY message notifies a window that gesture recognition is
 *     in progress and a gesture will be generated if one is recognized under the
 *     current gesture settings.
 */
typedef struct tagGESTURENOTIFYSTRUCT {
    UINT cbSize;                    // size, in bytes, of this structure
    DWORD dwFlags;                  // unused
    HWND hwndTarget;                // handle to window targeted by the gesture
    POINTS ptsLocation;             // starting location
    DWORD dwInstanceID;             // internally used
} GESTURENOTIFYSTRUCT, *PGESTURENOTIFYSTRUCT;

/*
 * Gesture argument helpers
 *   - Angle should be a double in the range of -2pi to +2pi
 *   - Argument should be an unsigned 16-bit value
 */
#define GID_ROTATE_ANGLE_TO_ARGUMENT(_arg_)     ((USHORT)((((_arg_) + 2.0 * 3.14159265) / (4.0 * 3.14159265)) * 65535.0))
#define GID_ROTATE_ANGLE_FROM_ARGUMENT(_arg_)   ((((double)(_arg_) / 65535.0) * 4.0 * 3.14159265) - 2.0 * 3.14159265)

/*
 * Gesture information retrieval
 *   - HGESTUREINFO is received by a window in the lParam of a WM_GESTURE message.
 */
_Win32_metadata_set_last_error_
WINUSERAPI
BOOL
WINAPI
GetGestureInfo(
    _In_ HGESTUREINFO hGestureInfo,
    _Out_ PGESTUREINFO pGestureInfo);

/*
 * Gesture extra arguments retrieval
 *   - HGESTUREINFO is received by a window in the lParam of a WM_GESTURE message.
 *   - Size, in bytes, of the extra argument data is available in the cbExtraArgs
 *     field of the GESTUREINFO structure retrieved using the GetGestureInfo function.
 */
_Win32_metadata_set_last_error_
WINUSERAPI
BOOL
WINAPI
GetGestureExtraArgs(
    _In_ HGESTUREINFO hGestureInfo,
    _In_ UINT cbExtraArgs,
    _Out_writes_bytes_(cbExtraArgs) PBYTE pExtraArgs);

/*
 * Gesture information handle management
 *   - If an application processes the WM_GESTURE message, then once it is done
 *     with the associated HGESTUREINFO, the application is responsible for
 *     closing the handle using this function. Failure to do so may result in
 *     process memory leaks.
 *   - If the message is instead passed to DefWindowProc, or is forwarded using
 *     one of the PostMessage or SendMessage class of API functions, the handle
 *     is transfered with the message and need not be closed by the application.
 */
_Win32_metadata_set_last_error_
WINUSERAPI
BOOL
WINAPI
CloseGestureInfoHandle(
    _In_ HGESTUREINFO hGestureInfo);


/*
 * Gesture configuration structure
 *   - Used in SetGestureConfig and GetGestureConfig
 *   - Note that any setting not included in either GESTURECONFIG.dwWant or
 *     GESTURECONFIG.dwBlock will use the parent window's preferences or
 *     system defaults.
 */
typedef struct tagGESTURECONFIG {
    GESTURECONFIG_ID dwID;                     // gesture ID
    DWORD dwWant;                   // settings related to gesture ID that are to be turned on
    DWORD dwBlock;                  // settings related to gesture ID that are to be turned off
} GESTURECONFIG, *PGESTURECONFIG;

#endif /* WINAPI_FAMILY_PARTITION(WINAPI_PARTITION_DESKTOP) */
#pragma endregion

/*
 * Gesture configuration flags - GESTURECONFIG.dwWant or GESTURECONFIG.dwBlock
 */

/*
 * Common gesture configuration flags - set GESTURECONFIG.dwID to zero
 */
#define GC_ALLGESTURES                              0x00000001

/*
 * Zoom gesture configuration flags - set GESTURECONFIG.dwID to GID_ZOOM
 */
#define GC_ZOOM                                     0x00000001

/*
 * Pan gesture configuration flags - set GESTURECONFIG.dwID to GID_PAN
 */
#define GC_PAN                                      0x00000001
#define GC_PAN_WITH_SINGLE_FINGER_VERTICALLY        0x00000002
#define GC_PAN_WITH_SINGLE_FINGER_HORIZONTALLY      0x00000004
#define GC_PAN_WITH_GUTTER                          0x00000008
#define GC_PAN_WITH_INERTIA                         0x00000010

/*
 * Rotate gesture configuration flags - set GESTURECONFIG.dwID to GID_ROTATE
 */
#define GC_ROTATE                                   0x00000001

/*
 * Two finger tap gesture configuration flags - set GESTURECONFIG.dwID to GID_TWOFINGERTAP
 */
#define GC_TWOFINGERTAP                             0x00000001

/*
 * PressAndTap gesture configuration flags - set GESTURECONFIG.dwID to GID_PRESSANDTAP
 */
#define GC_PRESSANDTAP                              0x00000001
#define GC_ROLLOVER                                 GC_PRESSANDTAP

#define GESTURECONFIGMAXCOUNT           256             // Maximum number of gestures that can be included
                                                        // in a single call to SetGestureConfig / GetGestureConfig

#pragma region Desktop Family
#if WINAPI_FAMILY_PARTITION(WINAPI_PARTITION_DESKTOP)

_Win32_metadata_set_last_error_
WINUSERAPI
BOOL
WINAPI
SetGestureConfig(
    _In_ HWND hwnd,                                     // window for which configuration is specified
    _In_ DWORD dwReserved,                              // reserved, must be 0
    _In_ UINT cIDs,                                     // count of GESTURECONFIG structures
    _In_reads_(cIDs) PGESTURECONFIG pGestureConfig,    // array of GESTURECONFIG structures, dwIDs will be processed in the
                                                        // order specified and repeated occurances will overwrite previous ones
    _In_ UINT cbSize);                                  // sizeof(GESTURECONFIG)


#define GCF_INCLUDE_ANCESTORS           0x00000001      // If specified, GetGestureConfig returns consolidated configuration
                                                        // for the specified window and it's parent window chain

_Win32_metadata_set_last_error_
WINUSERAPI
BOOL
WINAPI
GetGestureConfig(
    _In_ HWND hwnd,                                     // window for which configuration is required
    _In_ DWORD dwReserved,                              // reserved, must be 0
    _In_ DWORD dwFlags,                                 // see GCF_* flags
    _In_ PUINT pcIDs,                                   // *pcIDs contains the size, in number of GESTURECONFIG structures,
                                                        // of the buffer pointed to by pGestureConfig
    _Inout_updates_(*pcIDs) PGESTURECONFIG pGestureConfig,
                                                        // pointer to buffer to receive the returned array of GESTURECONFIG structures
    _In_ UINT cbSize);                                  // sizeof(GESTURECONFIG)

#endif /* WINAPI_FAMILY_PARTITION(WINAPI_PARTITION_DESKTOP) */
#pragma endregion


#endif /* WINVER >= 0x0601 */


#if(WINVER >= 0x0601)

/*
 * GetSystemMetrics(SM_DIGITIZER) flag values
 */
#define NID_INTEGRATED_TOUCH  0x00000001
#define NID_EXTERNAL_TOUCH    0x00000002
#define NID_INTEGRATED_PEN    0x00000004
#define NID_EXTERNAL_PEN      0x00000008
#define NID_MULTI_INPUT       0x00000040
#define NID_READY             0x00000080

#endif /* WINVER >= 0x0601 */


#define MAX_STR_BLOCKREASON 256

#pragma region Desktop Family
#if WINAPI_FAMILY_PARTITION(WINAPI_PARTITION_DESKTOP)

_Win32_metadata_set_last_error_
WINUSERAPI
BOOL
WINAPI
ShutdownBlockReasonCreate(
    _In_ HWND hWnd,
    _In_ LPCWSTR pwszReason);

_Win32_metadata_set_last_error_
WINUSERAPI
BOOL
WINAPI
ShutdownBlockReasonQuery(
    _In_ HWND hWnd,
    _Out_writes_opt_(*pcchBuff) LPWSTR pwszBuff,
    _Inout_ DWORD *pcchBuff);

_Win32_metadata_set_last_error_
WINUSERAPI
BOOL
WINAPI
ShutdownBlockReasonDestroy(
    _In_ HWND hWnd);

#endif /* WINAPI_FAMILY_PARTITION(WINAPI_PARTITION_DESKTOP) */
#pragma endregion


#if(WINVER >= 0x0601)

#pragma region Desktop Family
#if WINAPI_FAMILY_PARTITION(WINAPI_PARTITION_DESKTOP)

/*
 * Identifiers for message input source device type.
 */
typedef enum tagINPUT_MESSAGE_DEVICE_TYPE  {
    IMDT_UNAVAILABLE        = 0x00000000,       // not specified
    IMDT_KEYBOARD           = 0x00000001,       // from keyboard
    IMDT_MOUSE              = 0x00000002,       // from mouse
    IMDT_TOUCH              = 0x00000004,       // from touch
    IMDT_PEN                = 0x00000008,       // from pen
#if(WINVER >= 0x0603)
    IMDT_TOUCHPAD           = 0x00000010,       // from touchpad
#endif /* WINVER >= 0x0603 */
 } INPUT_MESSAGE_DEVICE_TYPE;

typedef enum tagINPUT_MESSAGE_ORIGIN_ID {
     IMO_UNAVAILABLE = 0x00000000,  // not specified
     IMO_HARDWARE    = 0x00000001,  // from a hardware device or injected by a UIAccess app
     IMO_INJECTED    = 0x00000002,  // injected via SendInput() by a non-UIAccess app
     IMO_SYSTEM      = 0x00000004,  // injected by the system
} INPUT_MESSAGE_ORIGIN_ID;

/*
 * Input source structure.
 */
 typedef struct tagINPUT_MESSAGE_SOURCE {
     INPUT_MESSAGE_DEVICE_TYPE deviceType;
     INPUT_MESSAGE_ORIGIN_ID   originId;
 } INPUT_MESSAGE_SOURCE;


/*
 * API to determine the input source of the current messsage.
 */
_Win32_metadata_set_last_error_
WINUSERAPI
BOOL
WINAPI
GetCurrentInputMessageSource(
    _Out_ INPUT_MESSAGE_SOURCE *inputMessageSource);

WINUSERAPI
BOOL
WINAPI
GetCIMSSM(
    _Out_ INPUT_MESSAGE_SOURCE *inputMessageSource);

#endif /* WINAPI_FAMILY_PARTITION(WINAPI_PARTITION_DESKTOP) */
#pragma endregion

#endif /* WINVER >= 0x0601 */

#if(WINVER >= 0x0602)

#pragma region Application Family or OneCore Family or Games Family
#if WINAPI_FAMILY_PARTITION(WINAPI_PARTITION_APP | WINAPI_PARTITION_SYSTEM | WINAPI_PARTITION_GAMES)

/*
 * AutoRotation state structure
 */
typedef enum tagAR_STATE {
    AR_ENABLED        = 0x0,
    AR_DISABLED       = 0x1,
    AR_SUPPRESSED     = 0x2,
    AR_REMOTESESSION  = 0x4,
    AR_MULTIMON       = 0x8,
    AR_NOSENSOR       = 0x10,
    AR_NOT_SUPPORTED  = 0x20,
    AR_DOCKED         = 0x40,
    AR_LAPTOP         = 0x80
} AR_STATE, *PAR_STATE;

#endif /* WINAPI_FAMILY_PARTITION(WINAPI_PARTITION_APP | WINAPI_PARTITION_SYSTEM | WINAPI_PARTITION_GAMES) */
#pragma endregion

#ifndef MIDL_PASS
// Don't define this for MIDL compiler passes over winuser.h. Some of them
// don't include winnt.h (where DEFINE_ENUM_FLAG_OPERATORS is defined and
// get compile errors.
DEFINE_ENUM_FLAG_OPERATORS(AR_STATE)
#endif

#pragma region Desktop Family
#if WINAPI_FAMILY_PARTITION(WINAPI_PARTITION_DESKTOP)

/*
 * Orientation preference structure. This is used by applications to specify
 * their orientation preferences to windows.
 */
typedef enum ORIENTATION_PREFERENCE {
    ORIENTATION_PREFERENCE_NONE              = 0x0,
    ORIENTATION_PREFERENCE_LANDSCAPE         = 0x1,
    ORIENTATION_PREFERENCE_PORTRAIT          = 0x2,
    ORIENTATION_PREFERENCE_LANDSCAPE_FLIPPED = 0x4,
    ORIENTATION_PREFERENCE_PORTRAIT_FLIPPED  = 0x8
} ORIENTATION_PREFERENCE;


#ifndef MIDL_PASS
// Don't define this for MIDL compiler passes over winuser.h. Some of them
// don't include winnt.h (where DEFINE_ENUM_FLAG_OPERATORS is defined and
// get compile errors.
DEFINE_ENUM_FLAG_OPERATORS(ORIENTATION_PREFERENCE)
#endif

WINUSERAPI
BOOL
WINAPI
GetAutoRotationState(
    _Out_ PAR_STATE pState);

WINUSERAPI
BOOL
WINAPI
GetDisplayAutoRotationPreferences(
    _Out_ ORIENTATION_PREFERENCE *pOrientation);

WINUSERAPI
BOOL
WINAPI
GetDisplayAutoRotationPreferencesByProcessId(
    _In_ DWORD dwProcessId,
    _Out_ ORIENTATION_PREFERENCE *pOrientation,
    _Out_ BOOL *fRotateScreen);

WINUSERAPI
BOOL
WINAPI
SetDisplayAutoRotationPreferences(
    _In_ ORIENTATION_PREFERENCE orientation);

#endif /* WINAPI_FAMILY_PARTITION(WINAPI_PARTITION_DESKTOP) */
#pragma endregion

#endif /* WINVER >= 0x0602 */


#if(WINVER >= 0x0601)
#pragma region Desktop Family
#if WINAPI_FAMILY_PARTITION(WINAPI_PARTITION_DESKTOP)


_Win32_metadata_set_last_error_
WINUSERAPI
BOOL
WINAPI
IsImmersiveProcess(
    _In_ HANDLE hProcess);

_Win32_metadata_set_last_error_
WINUSERAPI
BOOL
WINAPI
SetProcessRestrictionExemption(
    _In_ BOOL fEnableExemption);

#endif /* WINAPI_FAMILY_PARTITION(WINAPI_PARTITION_DESKTOP) */
#pragma endregion

#endif /* WINVER >= 0x0601 */


#if (NTDDI_VERSION >= NTDDI_WIN11_GE)

WINUSERAPI
BOOL
WINAPI
ConvertToInterceptWindow(
    HWND topLevelWindow);

WINUSERAPI
BOOL
WINAPI
IsInterceptWindow(
    HWND topLevelWindow,
    _Out_ BOOL* isIntercept);

#define WM_INTERCEPTED_WINDOW_ACTION     0x0346

/*
 * A WINDOW_ACTION defines one or more changes to make to a top-level window.
 * This is used by WM_INTERCEPTED_WINDOW_ACTION and ApplyWindowAction.
 */

typedef enum _WINDOW_PLACEMENT_STATE {
    WPS_NORMAL                  = 0,
    WPS_MAXIMIZED               = 1,
    WPS_MINIMIZED               = 2,
    WPS_ARRANGED                = 3,
} WINDOW_PLACEMENT_STATE;

typedef enum _WINDOW_ACTION_KINDS {
    WAK_NONE                    = 0x0000,

    // The action shows or hides the window, depending on the visible field.
    WAK_VISIBILITY              = 0x0001,

    // The action moves the window (sets top/left corner position in screen
    // coordinates) to the value in the position field.
    WAK_POSITION                = 0x0002,

    // The action sets the size of the window to the value in the size field.
    WAK_SIZE                    = 0x0004,

    // The action moves the window in z-order, making the window after (below)
    // the window specified by the insertAfter field.
    WAK_INSERT_AFTER            = 0x0008,

    // The action activates the window.
    WAK_ACTIVATE                = 0x0010,

    // The action sets the placement state of the window, Maximize, Minimize,
    // Arranged or Restored, depending on the placementState field.
    WAK_PLACEMENT_STATE         = 0x0020,

    // The action sets the normal position. This is the restore position for
    // Maximized/Minimized/Arranged windows. For normal (restored) windows, the
    // normal rect is the same as the position and size.
    WAK_NORMAL_RECT             = 0x0040,

    // The action moves the window to a monitor, using the nearest monitor to
    // the point in the pointOnMonitor field.
    WAK_MOVE_TO_MONITOR         = 0x0080,

    // The action adjusts the final window position (normal position) to keep
    // it entirely within the bounds of the work area of the monitor.
    WAK_FIT_TO_MONITOR          = 0x0100,

    // Used only by Intercept windows. The intercepted window action was
    // generated by a display change (the window is being moved because the
    // window's monitor changed in some way). The monitorTopologyId field is
    // the ID of the monitor topology (GetCurrentMonitorTopologyId) at the
    // time the action was generated.
    //
    // When the intercept window applies the action, it should set the display
    // change flag and monitor topology ID to these same values. This is used
    // to know if the window needs additional actions (if the monitors change
    // while the window processes the first display change action the window
    // may need to move again).
    WAK_DISPLAY_CHANGE          = 0x0200,

    // Used only by Intercept windows. The intercepted window action has some
    // internal work that needs to happen after the action is completed. The
    // intercept window is expected to set this flag when it applies the action,
    // in order for the window to behave properly in all cases.
    WAK_SYSTEM_OPERATION        = 0x0400,

    // Actions with only the coalescable flags can be combined (coalesced).
    // If a window has multiple actions to process that are coalescable, the
    // actions can be combined into a single action, using the latest value
    // of each of each field.
    WAK_COALESCEABLE            = WAK_POSITION |
                                    WAK_SIZE |
                                    WAK_ACTIVATE |
                                    WAK_VISIBILITY |
                                    WAK_INSERT_AFTER,

} WINDOW_ACTION_KINDS;
DEFINE_ENUM_FLAG_OPERATORS(WINDOW_ACTION_KINDS)

typedef enum _WINDOW_ACTION_MODIFIERS {
    WAM_NONE                    = 0x0000,

    // The provided position and size are the visible bounds of the window.
    // The final rect is expanded by the size of the window's invisible resize
    // borders.
    WAM_FRAME_BOUNDS            = 0x0001,

    // Used by Intercept windows only. ApplyWindowAction ignores this modifier.
    // The intercepted action is activating a window on the foreground thread,
    // which will set the foreground window.
    WAM_ACTIVATE_FOREGROUND     = 0x0002,

    // Used by Intercept windows only. ApplyWindowAction ignores this modifier.
    // The intercepted action was generated by input (like a mouse click).
    WAM_ACTIVATE_INPUT          = 0x0004,

    // The action is activating the window, but should not change the window's
    // z-order. By default, activating raises the window to top of z-order.
    WAM_ACTIVATE_NO_ZORDER      = 0x0008,

    // The action is moving the window in z-order, but should not move windows
    // owned to this window. By default, moving a window in z-order also moves
    // owned windows.
    WAM_INSERT_AFTER_NO_OWNER   = 0x0010,

    // The action is minimizing the window, and overriding the restore state.
    // By default, minimized windows restore to their previous state.
    // For WAM_RESTORE_TO_ARRANGED, the position and size must also be set,
    // which are interpretted as the restore to arranged position.
    WAM_RESTORE_TO_NORMAL       = 0x0020,
    WAM_RESTORE_TO_MAXIMIZED    = 0x0040,
    WAM_RESTORE_TO_ARRANGED     = 0x0080,

    // The workArea field is set to the previous monitor work area. By default,
    // the provided position and size are assumed fit to the current monitor
    // topology. Providing a previous work area causes the position to be
    // adjusted as needed to the work area of the window's monitor.
    WAM_WORK_AREA               = 0x0100,

    // The dpi field has the DPI (base 96 scale factor) for the provided size.
    // By default, the provided size is scaled to the window's DPI
    // (GetDpiForWindow), and is scaled as needed to the DPI of the monitor the
    // window is moving to.
    WAM_DPI                     = 0x0200,

    // The pointOnMonitor field specifies the monitor that the provided
    // position and size are scaled to. This overrides the monitor that the
    // window will be associated with (scaling to) after applying the action.
    // By default, the window is scaling to the monitor the position/size is
    // mostly on.
    WAM_SCALED_TO_MONITOR       = 0x0400,
} WINDOW_ACTION_MODIFIERS;
DEFINE_ENUM_FLAG_OPERATORS(WINDOW_ACTION_MODIFIERS)

typedef struct _WINDOW_ACTION
{
    WINDOW_ACTION_KINDS kinds;
    WINDOW_ACTION_MODIFIERS modifiers;

    // Valid if WAK_VISIBILITY.
    // Showing if true, hiding if false.
    BOOL visible;

    // Valid if WAK_POSITION.
    // This sets the window position, top/left coordinates.
    POINT position;

    // Valid if WAK_SIZE.
    // This sets the window size, width/height.
    SIZE size;

    // Valid if WAK_INSERT_AFTER,
    // Insert after window, the window this window should be after/below in
    // zorder. This can be a sentinal value like HWND_TOP.
    HWND insertAfter;

    // Valid if WAK_PLACEMENT_STATE
    // This is the new placement state of the window (maximized, minimized,
    // arranged, normal).
    WINDOW_PLACEMENT_STATE placementState;

    // Valid if WAK_NORMAL_RECT.
    // This sets a the normal rect. This is the restore position for a window
    // in a non-normal state (maximized, minimized, arranged).
    RECT normalRect;

    // Valid if WAM_WORK_AREA.
    // This is the previous work area for the provided position and size.
    // Without specifying the work area, the current or specified position
    // and size are assumed fit to the current monitors.
    RECT workArea;

    // Valid if WAM_DPI.
    // This is the DPI scale of the provided size.
    // Without specifying the DPI, the provided size is assumed assumed to be
    // scaled to the window's current DPI.
    UINT dpi;

    // Valid if WAK_MOVE_TO_MONITOR or WAM_SCALED_TO_MONITOR.
    // This point is used to pick a monitor (default to nearest).
    //
    // WAK_MOVE_TO_MONITOR
    // The window is moved to this monitor. The current or provided window
    // position, size, and state, and the previous and selected monitor work
    // area, are used to pick a 'good' position for the window on this monitor.
    //
    // WAM_SCALED_TO_MONITOR
    // The provided position and size are already scaled to the specified
    // monitor. The size should not be scaled to the monitor DPI and the
    // window should end up scaling to this monitor. (Note: This allows a
    // window to be scaling to, GetDpiForMonitor, a monitor it is not mostly
    // on, MonitorFromRect.)
    POINT pointOnMonitor;

    // Valid if WAK_DISPLAY_CHANGE.
    // The ID of the current monitor topology, see GetCurrentMonitorTopologyId.
    // The WAK_DISPLAY_CHANGE kind is used by intercepting windows when they are
    // moved in response to the monitors changing. The window must provide the
    // same ID when applying these changes, allowing the system to handle cases
    // where the monitors change multiple times very quickly.
    UINT monitorTopologyId;
} WINDOW_ACTION, *PWINDOW_ACTION;

typedef WINDOW_ACTION const * PCWINDOW_ACTION;

WINUSERAPI
BOOL
WINAPI
ApplyWindowAction(
    HWND hwnd,
    WINDOW_ACTION *pAction);

#endif // NTDDI_VERSION >= NTDDI_WIN11_GE


#pragma region Desktop Family
#if WINAPI_FAMILY_PARTITION(WINAPI_PARTITION_DESKTOP)


#endif /* WINAPI_FAMILY_PARTITION(WINAPI_PARTITION_DESKTOP) */


/*
 * Ink Feedback APIs
 */

#if(WINVER >= 0x0607)

WINUSERAPI
BOOL
WINAPI
SetAdditionalForegroundBoostProcesses(HWND topLevelWindow,
                                      DWORD processHandleCount,
                                      _In_reads_(processHandleCount) HANDLE *processHandleArray);

/* TOOLTIP_DISMISS_FLAGS:
 *
 * These are flags used with the RegisterForTooltipDismissNotification API.
 *
 * TDF_REGISTER -   Used to register tooltip to receive notification of trigger key combination
 *                  via WM_TOOLTIPDISMISS.
 *
 * TDF_UNREGISTER - Used to unregister tooltip from receiving notification of trigger key
 *                  combination via WM_TOOLTIPDISMISS.
 */

typedef enum {
    TDF_REGISTER                = 0x0001,
    TDF_UNREGISTER              = 0x0002,
} TOOLTIP_DISMISS_FLAGS;

WINUSERAPI
BOOL
WINAPI
RegisterForTooltipDismissNotification(HWND hWnd,
                                      TOOLTIP_DISMISS_FLAGS tdFlags);

/*
 * Support for Accessibility Tooltip Dismissal API:
 *
 * This message notifies apps/frameworks that a trigger event has occurred and they should dismiss the
 * respective tooltip window.
 * This is used with RegisterForTooltipDismissNotification.
 *
 * Note that only kernel mode can originate this message.
 */
#define WM_TOOLTIPDISMISS               0x0345

#endif /* WINVER >= 0x0607 */


#if (NTDDI_VERSION >= NTDDI_WIN11_GE)
WINUSERAPI
BOOL
WINAPI
ConvertPrimaryPointerToMouseDrag(VOID);
#endif // NTDDI_VERSION >= NTDDI_WIN11_GE

#if(WINVER >= 0x0604)
WINUSERAPI
BOOL
WINAPI
IsWindowArranged(
    _In_ HWND hwnd);

#endif /* WINVER >= 0x0604 */

#if (NTDDI_VERSION >= NTDDI_WIN11_GE)

#define INVALID_MONITOR_TOPOLOGY_ID 0

WINUSERAPI
UINT
WINAPI
GetCurrentMonitorTopologyId(VOID);

// Send to the window registered with NtUserRegisterCloakedNotification
// when cloak state of the window has changed
// wParam - if window cloak state changed contains cloaking value
//          which can be one/all of the below
//          DWM_CLOAKED_APP(0x0000001).The window was cloaked by its owner application.
//          DWM_CLOAKED_SHELL(0x0000002).The window was cloaked by the Shell.
//          0 - window is not cloaked
//
// lParam - 0 (unused)
//
#define WM_CLOAKED_STATE_CHANGED 0x0347

WINUSERAPI
BOOL
WINAPI
RegisterCloakedNotification(HWND hwnd, BOOL fRegister);

typedef enum _MOVESIZE_OPERATION
{
    MSO_SIZE_LEFT =             1,
    MSO_SIZE_RIGHT =            2,
    MSO_SIZE_TOP =              3,
    MSO_SIZE_TOPLEFT =          4,
    MSO_SIZE_TOPRIGHT =         5,
    MSO_SIZE_BOTTOM =           6,
    MSO_SIZE_BOTTOMLEFT =       7,
    MSO_SIZE_BOTTOMRIGHT =      8,
    MSO_MOVE =                  9,
} MOVESIZE_OPERATION;

WINUSERAPI
BOOL
WINAPI
EnterMoveSizeLoop(
    HWND hwnd,
    POINT ptCursor,
    MOVESIZE_OPERATION moveSizeCode);

#endif // NTDDI_VERSION >= NTDDI_WIN11_GE

#if _MSC_VER >= 1200
#pragma warning(pop)
#endif


#if !defined(RC_INVOKED) /* RC complains about long symbols in #ifs */
#if defined(ISOLATION_AWARE_ENABLED) && (ISOLATION_AWARE_ENABLED != 0)
#include "winuser.inl"
#endif /* ISOLATION_AWARE_ENABLED */
#endif /* RC */

#ifdef WIN32METADATA
#pragma push_macro("ALERT_SYSTEM_INFORMATIONAL")
#pragma push_macro("ALERT_SYSTEM_WARNING")
#pragma push_macro("ALERT_SYSTEM_ERROR")
#pragma push_macro("ALERT_SYSTEM_QUERY")
#pragma push_macro("ALERT_SYSTEM_CRITICAL")
#undef ALERT_SYSTEM_INFORMATIONAL
#undef ALERT_SYSTEM_WARNING
#undef ALERT_SYSTEM_ERROR
#undef ALERT_SYSTEM_QUERY
#undef ALERT_SYSTEM_CRITICAL
enum class
    ALERT_SYSTEM_SEV : int
{
    ALERT_SYSTEM_INFORMATIONAL = 1,
    ALERT_SYSTEM_WARNING = 2,
    ALERT_SYSTEM_ERROR = 3,
    ALERT_SYSTEM_QUERY = 4,
    ALERT_SYSTEM_CRITICAL = 5,
};
#pragma pop_macro("ALERT_SYSTEM_CRITICAL")
#pragma pop_macro("ALERT_SYSTEM_QUERY")
#pragma pop_macro("ALERT_SYSTEM_ERROR")
#pragma pop_macro("ALERT_SYSTEM_WARNING")
#pragma pop_macro("ALERT_SYSTEM_INFORMATIONAL")

#pragma push_macro("APPCOMMAND_BROWSER_BACKWARD")
#pragma push_macro("APPCOMMAND_BROWSER_FORWARD")
#pragma push_macro("APPCOMMAND_BROWSER_REFRESH")
#pragma push_macro("APPCOMMAND_BROWSER_STOP")
#pragma push_macro("APPCOMMAND_BROWSER_SEARCH")
#pragma push_macro("APPCOMMAND_BROWSER_FAVORITES")
#pragma push_macro("APPCOMMAND_BROWSER_HOME")
#pragma push_macro("APPCOMMAND_VOLUME_MUTE")
#pragma push_macro("APPCOMMAND_VOLUME_DOWN")
#pragma push_macro("APPCOMMAND_VOLUME_UP")
#pragma push_macro("APPCOMMAND_MEDIA_NEXTTRACK")
#pragma push_macro("APPCOMMAND_MEDIA_PREVIOUSTRACK")
#pragma push_macro("APPCOMMAND_MEDIA_STOP")
#pragma push_macro("APPCOMMAND_MEDIA_PLAY_PAUSE")
#pragma push_macro("APPCOMMAND_LAUNCH_MAIL")
#pragma push_macro("APPCOMMAND_LAUNCH_MEDIA_SELECT")
#pragma push_macro("APPCOMMAND_LAUNCH_APP1")
#pragma push_macro("APPCOMMAND_LAUNCH_APP2")
#pragma push_macro("APPCOMMAND_BASS_DOWN")
#pragma push_macro("APPCOMMAND_BASS_BOOST")
#pragma push_macro("APPCOMMAND_BASS_UP")
#pragma push_macro("APPCOMMAND_TREBLE_DOWN")
#pragma push_macro("APPCOMMAND_TREBLE_UP")
#pragma push_macro("APPCOMMAND_MICROPHONE_VOLUME_MUTE")
#pragma push_macro("APPCOMMAND_MICROPHONE_VOLUME_DOWN")
#pragma push_macro("APPCOMMAND_MICROPHONE_VOLUME_UP")
#pragma push_macro("APPCOMMAND_HELP")
#pragma push_macro("APPCOMMAND_FIND")
#pragma push_macro("APPCOMMAND_NEW")
#pragma push_macro("APPCOMMAND_OPEN")
#pragma push_macro("APPCOMMAND_CLOSE")
#pragma push_macro("APPCOMMAND_SAVE")
#pragma push_macro("APPCOMMAND_PRINT")
#pragma push_macro("APPCOMMAND_UNDO")
#pragma push_macro("APPCOMMAND_REDO")
#pragma push_macro("APPCOMMAND_COPY")
#pragma push_macro("APPCOMMAND_CUT")
#pragma push_macro("APPCOMMAND_PASTE")
#pragma push_macro("APPCOMMAND_REPLY_TO_MAIL")
#pragma push_macro("APPCOMMAND_FORWARD_MAIL")
#pragma push_macro("APPCOMMAND_SEND_MAIL")
#pragma push_macro("APPCOMMAND_SPELL_CHECK")
#pragma push_macro("APPCOMMAND_DICTATE_OR_COMMAND_CONTROL_TOGGLE")
#pragma push_macro("APPCOMMAND_MIC_ON_OFF_TOGGLE")
#pragma push_macro("APPCOMMAND_CORRECTION_LIST")
#pragma push_macro("APPCOMMAND_MEDIA_PLAY")
#pragma push_macro("APPCOMMAND_MEDIA_PAUSE")
#pragma push_macro("APPCOMMAND_MEDIA_RECORD")
#pragma push_macro("APPCOMMAND_MEDIA_FAST_FORWARD")
#pragma push_macro("APPCOMMAND_MEDIA_REWIND")
#pragma push_macro("APPCOMMAND_MEDIA_CHANNEL_UP")
#pragma push_macro("APPCOMMAND_MEDIA_CHANNEL_DOWN")
#pragma push_macro("APPCOMMAND_DELETE")
#pragma push_macro("APPCOMMAND_DWM_FLIP3D")
#undef APPCOMMAND_BROWSER_BACKWARD
#undef APPCOMMAND_BROWSER_FORWARD
#undef APPCOMMAND_BROWSER_REFRESH
#undef APPCOMMAND_BROWSER_STOP
#undef APPCOMMAND_BROWSER_SEARCH
#undef APPCOMMAND_BROWSER_FAVORITES
#undef APPCOMMAND_BROWSER_HOME
#undef APPCOMMAND_VOLUME_MUTE
#undef APPCOMMAND_VOLUME_DOWN
#undef APPCOMMAND_VOLUME_UP
#undef APPCOMMAND_MEDIA_NEXTTRACK
#undef APPCOMMAND_MEDIA_PREVIOUSTRACK
#undef APPCOMMAND_MEDIA_STOP
#undef APPCOMMAND_MEDIA_PLAY_PAUSE
#undef APPCOMMAND_LAUNCH_MAIL
#undef APPCOMMAND_LAUNCH_MEDIA_SELECT
#undef APPCOMMAND_LAUNCH_APP1
#undef APPCOMMAND_LAUNCH_APP2
#undef APPCOMMAND_BASS_DOWN
#undef APPCOMMAND_BASS_BOOST
#undef APPCOMMAND_BASS_UP
#undef APPCOMMAND_TREBLE_DOWN
#undef APPCOMMAND_TREBLE_UP
#undef APPCOMMAND_MICROPHONE_VOLUME_MUTE
#undef APPCOMMAND_MICROPHONE_VOLUME_DOWN
#undef APPCOMMAND_MICROPHONE_VOLUME_UP
#undef APPCOMMAND_HELP
#undef APPCOMMAND_FIND
#undef APPCOMMAND_NEW
#undef APPCOMMAND_OPEN
#undef APPCOMMAND_CLOSE
#undef APPCOMMAND_SAVE
#undef APPCOMMAND_PRINT
#undef APPCOMMAND_UNDO
#undef APPCOMMAND_REDO
#undef APPCOMMAND_COPY
#undef APPCOMMAND_CUT
#undef APPCOMMAND_PASTE
#undef APPCOMMAND_REPLY_TO_MAIL
#undef APPCOMMAND_FORWARD_MAIL
#undef APPCOMMAND_SEND_MAIL
#undef APPCOMMAND_SPELL_CHECK
#undef APPCOMMAND_DICTATE_OR_COMMAND_CONTROL_TOGGLE
#undef APPCOMMAND_MIC_ON_OFF_TOGGLE
#undef APPCOMMAND_CORRECTION_LIST
#undef APPCOMMAND_MEDIA_PLAY
#undef APPCOMMAND_MEDIA_PAUSE
#undef APPCOMMAND_MEDIA_RECORD
#undef APPCOMMAND_MEDIA_FAST_FORWARD
#undef APPCOMMAND_MEDIA_REWIND
#undef APPCOMMAND_MEDIA_CHANNEL_UP
#undef APPCOMMAND_MEDIA_CHANNEL_DOWN
#undef APPCOMMAND_DELETE
#undef APPCOMMAND_DWM_FLIP3D
enum class
    APPCOMMAND_ID : int
{
    APPCOMMAND_BROWSER_BACKWARD = 1,
    APPCOMMAND_BROWSER_FORWARD = 2,
    APPCOMMAND_BROWSER_REFRESH = 3,
    APPCOMMAND_BROWSER_STOP = 4,
    APPCOMMAND_BROWSER_SEARCH = 5,
    APPCOMMAND_BROWSER_FAVORITES = 6,
    APPCOMMAND_BROWSER_HOME = 7,
    APPCOMMAND_VOLUME_MUTE = 8,
    APPCOMMAND_VOLUME_DOWN = 9,
    APPCOMMAND_VOLUME_UP = 10,
    APPCOMMAND_MEDIA_NEXTTRACK = 11,
    APPCOMMAND_MEDIA_PREVIOUSTRACK = 12,
    APPCOMMAND_MEDIA_STOP = 13,
    APPCOMMAND_MEDIA_PLAY_PAUSE = 14,
    APPCOMMAND_LAUNCH_MAIL = 15,
    APPCOMMAND_LAUNCH_MEDIA_SELECT = 16,
    APPCOMMAND_LAUNCH_APP1 = 17,
    APPCOMMAND_LAUNCH_APP2 = 18,
    APPCOMMAND_BASS_DOWN = 19,
    APPCOMMAND_BASS_BOOST = 20,
    APPCOMMAND_BASS_UP = 21,
    APPCOMMAND_TREBLE_DOWN = 22,
    APPCOMMAND_TREBLE_UP = 23,
    APPCOMMAND_MICROPHONE_VOLUME_MUTE = 24,
    APPCOMMAND_MICROPHONE_VOLUME_DOWN = 25,
    APPCOMMAND_MICROPHONE_VOLUME_UP = 26,
    APPCOMMAND_HELP = 27,
    APPCOMMAND_FIND = 28,
    APPCOMMAND_NEW = 29,
    APPCOMMAND_OPEN = 30,
    APPCOMMAND_CLOSE = 31,
    APPCOMMAND_SAVE = 32,
    APPCOMMAND_PRINT = 33,
    APPCOMMAND_UNDO = 34,
    APPCOMMAND_REDO = 35,
    APPCOMMAND_COPY = 36,
    APPCOMMAND_CUT = 37,
    APPCOMMAND_PASTE = 38,
    APPCOMMAND_REPLY_TO_MAIL = 39,
    APPCOMMAND_FORWARD_MAIL = 40,
    APPCOMMAND_SEND_MAIL = 41,
    APPCOMMAND_SPELL_CHECK = 42,
    APPCOMMAND_DICTATE_OR_COMMAND_CONTROL_TOGGLE = 43,
    APPCOMMAND_MIC_ON_OFF_TOGGLE = 44,
    APPCOMMAND_CORRECTION_LIST = 45,
    APPCOMMAND_MEDIA_PLAY = 46,
    APPCOMMAND_MEDIA_PAUSE = 47,
    APPCOMMAND_MEDIA_RECORD = 48,
    APPCOMMAND_MEDIA_FAST_FORWARD = 49,
    APPCOMMAND_MEDIA_REWIND = 50,
    APPCOMMAND_MEDIA_CHANNEL_UP = 51,
    APPCOMMAND_MEDIA_CHANNEL_DOWN = 52,
    APPCOMMAND_DELETE = 53,
    APPCOMMAND_DWM_FLIP3D = 54,
};
#pragma pop_macro("APPCOMMAND_DWM_FLIP3D")
#pragma pop_macro("APPCOMMAND_DELETE")
#pragma pop_macro("APPCOMMAND_MEDIA_CHANNEL_DOWN")
#pragma pop_macro("APPCOMMAND_MEDIA_CHANNEL_UP")
#pragma pop_macro("APPCOMMAND_MEDIA_REWIND")
#pragma pop_macro("APPCOMMAND_MEDIA_FAST_FORWARD")
#pragma pop_macro("APPCOMMAND_MEDIA_RECORD")
#pragma pop_macro("APPCOMMAND_MEDIA_PAUSE")
#pragma pop_macro("APPCOMMAND_MEDIA_PLAY")
#pragma pop_macro("APPCOMMAND_CORRECTION_LIST")
#pragma pop_macro("APPCOMMAND_MIC_ON_OFF_TOGGLE")
#pragma pop_macro("APPCOMMAND_DICTATE_OR_COMMAND_CONTROL_TOGGLE")
#pragma pop_macro("APPCOMMAND_SPELL_CHECK")
#pragma pop_macro("APPCOMMAND_SEND_MAIL")
#pragma pop_macro("APPCOMMAND_FORWARD_MAIL")
#pragma pop_macro("APPCOMMAND_REPLY_TO_MAIL")
#pragma pop_macro("APPCOMMAND_PASTE")
#pragma pop_macro("APPCOMMAND_CUT")
#pragma pop_macro("APPCOMMAND_COPY")
#pragma pop_macro("APPCOMMAND_REDO")
#pragma pop_macro("APPCOMMAND_UNDO")
#pragma pop_macro("APPCOMMAND_PRINT")
#pragma pop_macro("APPCOMMAND_SAVE")
#pragma pop_macro("APPCOMMAND_CLOSE")
#pragma pop_macro("APPCOMMAND_OPEN")
#pragma pop_macro("APPCOMMAND_NEW")
#pragma pop_macro("APPCOMMAND_FIND")
#pragma pop_macro("APPCOMMAND_HELP")
#pragma pop_macro("APPCOMMAND_MICROPHONE_VOLUME_UP")
#pragma pop_macro("APPCOMMAND_MICROPHONE_VOLUME_DOWN")
#pragma pop_macro("APPCOMMAND_MICROPHONE_VOLUME_MUTE")
#pragma pop_macro("APPCOMMAND_TREBLE_UP")
#pragma pop_macro("APPCOMMAND_TREBLE_DOWN")
#pragma pop_macro("APPCOMMAND_BASS_UP")
#pragma pop_macro("APPCOMMAND_BASS_BOOST")
#pragma pop_macro("APPCOMMAND_BASS_DOWN")
#pragma pop_macro("APPCOMMAND_LAUNCH_APP2")
#pragma pop_macro("APPCOMMAND_LAUNCH_APP1")
#pragma pop_macro("APPCOMMAND_LAUNCH_MEDIA_SELECT")
#pragma pop_macro("APPCOMMAND_LAUNCH_MAIL")
#pragma pop_macro("APPCOMMAND_MEDIA_PLAY_PAUSE")
#pragma pop_macro("APPCOMMAND_MEDIA_STOP")
#pragma pop_macro("APPCOMMAND_MEDIA_PREVIOUSTRACK")
#pragma pop_macro("APPCOMMAND_MEDIA_NEXTTRACK")
#pragma pop_macro("APPCOMMAND_VOLUME_UP")
#pragma pop_macro("APPCOMMAND_VOLUME_DOWN")
#pragma pop_macro("APPCOMMAND_VOLUME_MUTE")
#pragma pop_macro("APPCOMMAND_BROWSER_HOME")
#pragma pop_macro("APPCOMMAND_BROWSER_FAVORITES")
#pragma pop_macro("APPCOMMAND_BROWSER_SEARCH")
#pragma pop_macro("APPCOMMAND_BROWSER_STOP")
#pragma pop_macro("APPCOMMAND_BROWSER_REFRESH")
#pragma pop_macro("APPCOMMAND_BROWSER_FORWARD")
#pragma pop_macro("APPCOMMAND_BROWSER_BACKWARD")

#pragma push_macro("ATF_TIMEOUTON")
#pragma push_macro("ATF_ONOFFFEEDBACK")
#undef ATF_TIMEOUTON
#undef ATF_ONOFFFEEDBACK
enum class
    [[clang::flag_enum]]
    ATF_FLAGS : int
{
    ATF_TIMEOUTON = 1,
    ATF_ONOFFFEEDBACK = 2,
};
#pragma pop_macro("ATF_ONOFFFEEDBACK")
#pragma pop_macro("ATF_TIMEOUTON")

#pragma push_macro("CS_VREDRAW")
#pragma push_macro("CS_HREDRAW")
#pragma push_macro("CS_DBLCLKS")
#pragma push_macro("CS_OWNDC")
#pragma push_macro("CS_CLASSDC")
#pragma push_macro("CS_PARENTDC")
#pragma push_macro("CS_NOCLOSE")
#pragma push_macro("CS_SAVEBITS")
#pragma push_macro("CS_BYTEALIGNCLIENT")
#pragma push_macro("CS_BYTEALIGNWINDOW")
#pragma push_macro("CS_GLOBALCLASS")
#pragma push_macro("CS_IME")
#pragma push_macro("CS_DROPSHADOW")
#undef CS_VREDRAW
#undef CS_HREDRAW
#undef CS_DBLCLKS
#undef CS_OWNDC
#undef CS_CLASSDC
#undef CS_PARENTDC
#undef CS_NOCLOSE
#undef CS_SAVEBITS
#undef CS_BYTEALIGNCLIENT
#undef CS_BYTEALIGNWINDOW
#undef CS_GLOBALCLASS
#undef CS_IME
#undef CS_DROPSHADOW
enum class
    [[clang::flag_enum]]
    WNDCLASS_STYLES : int
{
    CS_VREDRAW = 1,
    CS_HREDRAW = 2,
    CS_DBLCLKS = 8,
    CS_OWNDC = 32,
    CS_CLASSDC = 64,
    CS_PARENTDC = 128,
    CS_NOCLOSE = 512,
    CS_SAVEBITS = 2048,
    CS_BYTEALIGNCLIENT = 4096,
    CS_BYTEALIGNWINDOW = 8192,
    CS_GLOBALCLASS = 16384,
    CS_IME = 65536,
    CS_DROPSHADOW = 131072,
};
#pragma pop_macro("CS_DROPSHADOW")
#pragma pop_macro("CS_IME")
#pragma pop_macro("CS_GLOBALCLASS")
#pragma pop_macro("CS_BYTEALIGNWINDOW")
#pragma pop_macro("CS_BYTEALIGNCLIENT")
#pragma pop_macro("CS_SAVEBITS")
#pragma pop_macro("CS_NOCLOSE")
#pragma pop_macro("CS_PARENTDC")
#pragma pop_macro("CS_CLASSDC")
#pragma pop_macro("CS_OWNDC")
#pragma pop_macro("CS_DBLCLKS")
#pragma pop_macro("CS_HREDRAW")
#pragma pop_macro("CS_VREDRAW")

#pragma push_macro("CWP_ALL")
#pragma push_macro("CWP_SKIPINVISIBLE")
#pragma push_macro("CWP_SKIPDISABLED")
#pragma push_macro("CWP_SKIPTRANSPARENT")
#undef CWP_ALL
#undef CWP_SKIPINVISIBLE
#undef CWP_SKIPDISABLED
#undef CWP_SKIPTRANSPARENT
enum class
    [[clang::flag_enum]]
    CWP_FLAGS : int
{
    CWP_ALL = 0,
    CWP_SKIPINVISIBLE = 1,
    CWP_SKIPDISABLED = 2,
    CWP_SKIPTRANSPARENT = 4,
};
#pragma pop_macro("CWP_SKIPTRANSPARENT")
#pragma pop_macro("CWP_SKIPDISABLED")
#pragma pop_macro("CWP_SKIPINVISIBLE")
#pragma pop_macro("CWP_ALL")

#pragma push_macro("DFC_CAPTION")
#pragma push_macro("DFC_MENU")
#pragma push_macro("DFC_SCROLL")
#pragma push_macro("DFC_BUTTON")
#pragma push_macro("DFC_POPUPMENU")
#undef DFC_CAPTION
#undef DFC_MENU
#undef DFC_SCROLL
#undef DFC_BUTTON
#undef DFC_POPUPMENU
enum class
    DFC_TYPE : int
{
    DFC_CAPTION = 1,
    DFC_MENU = 2,
    DFC_SCROLL = 3,
    DFC_BUTTON = 4,
    DFC_POPUPMENU = 5,
};
#pragma pop_macro("DFC_POPUPMENU")
#pragma pop_macro("DFC_BUTTON")
#pragma pop_macro("DFC_SCROLL")
#pragma pop_macro("DFC_MENU")
#pragma pop_macro("DFC_CAPTION")

#pragma push_macro("DFCS_CAPTIONCLOSE")
#pragma push_macro("DFCS_CAPTIONMIN")
#pragma push_macro("DFCS_CAPTIONMAX")
#pragma push_macro("DFCS_CAPTIONRESTORE")
#pragma push_macro("DFCS_CAPTIONHELP")
#pragma push_macro("DFCS_MENUARROW")
#pragma push_macro("DFCS_MENUCHECK")
#pragma push_macro("DFCS_MENUBULLET")
#pragma push_macro("DFCS_MENUARROWRIGHT")
#pragma push_macro("DFCS_SCROLLUP")
#pragma push_macro("DFCS_SCROLLDOWN")
#pragma push_macro("DFCS_SCROLLLEFT")
#pragma push_macro("DFCS_SCROLLRIGHT")
#pragma push_macro("DFCS_SCROLLCOMBOBOX")
#pragma push_macro("DFCS_SCROLLSIZEGRIP")
#pragma push_macro("DFCS_SCROLLSIZEGRIPRIGHT")
#pragma push_macro("DFCS_BUTTONCHECK")
#pragma push_macro("DFCS_BUTTONRADIOIMAGE")
#pragma push_macro("DFCS_BUTTONRADIOMASK")
#pragma push_macro("DFCS_BUTTONRADIO")
#pragma push_macro("DFCS_BUTTON3STATE")
#pragma push_macro("DFCS_BUTTONPUSH")
#pragma push_macro("DFCS_INACTIVE")
#pragma push_macro("DFCS_PUSHED")
#pragma push_macro("DFCS_CHECKED")
#pragma push_macro("DFCS_TRANSPARENT")
#pragma push_macro("DFCS_HOT")
#pragma push_macro("DFCS_ADJUSTRECT")
#pragma push_macro("DFCS_FLAT")
#pragma push_macro("DFCS_MONO")
#undef DFCS_CAPTIONCLOSE
#undef DFCS_CAPTIONMIN
#undef DFCS_CAPTIONMAX
#undef DFCS_CAPTIONRESTORE
#undef DFCS_CAPTIONHELP
#undef DFCS_MENUARROW
#undef DFCS_MENUCHECK
#undef DFCS_MENUBULLET
#undef DFCS_MENUARROWRIGHT
#undef DFCS_SCROLLUP
#undef DFCS_SCROLLDOWN
#undef DFCS_SCROLLLEFT
#undef DFCS_SCROLLRIGHT
#undef DFCS_SCROLLCOMBOBOX
#undef DFCS_SCROLLSIZEGRIP
#undef DFCS_SCROLLSIZEGRIPRIGHT
#undef DFCS_BUTTONCHECK
#undef DFCS_BUTTONRADIOIMAGE
#undef DFCS_BUTTONRADIOMASK
#undef DFCS_BUTTONRADIO
#undef DFCS_BUTTON3STATE
#undef DFCS_BUTTONPUSH
#undef DFCS_INACTIVE
#undef DFCS_PUSHED
#undef DFCS_CHECKED
#undef DFCS_TRANSPARENT
#undef DFCS_HOT
#undef DFCS_ADJUSTRECT
#undef DFCS_FLAT
#undef DFCS_MONO
enum class
    [[clang::flag_enum]]
    DFCS_STATE : int
{
    DFCS_CAPTIONCLOSE = 0,
    DFCS_CAPTIONMIN = 1,
    DFCS_CAPTIONMAX = 2,
    DFCS_CAPTIONRESTORE = 3,
    DFCS_CAPTIONHELP = 4,
    DFCS_MENUARROW = 0,
    DFCS_MENUCHECK = 1,
    DFCS_MENUBULLET = 2,
    DFCS_MENUARROWRIGHT = 4,
    DFCS_SCROLLUP = 0,
    DFCS_SCROLLDOWN = 1,
    DFCS_SCROLLLEFT = 2,
    DFCS_SCROLLRIGHT = 3,
    DFCS_SCROLLCOMBOBOX = 5,
    DFCS_SCROLLSIZEGRIP = 8,
    DFCS_SCROLLSIZEGRIPRIGHT = 16,
    DFCS_BUTTONCHECK = 0,
    DFCS_BUTTONRADIOIMAGE = 1,
    DFCS_BUTTONRADIOMASK = 2,
    DFCS_BUTTONRADIO = 4,
    DFCS_BUTTON3STATE = 8,
    DFCS_BUTTONPUSH = 16,
    DFCS_INACTIVE = 256,
    DFCS_PUSHED = 512,
    DFCS_CHECKED = 1024,
    DFCS_TRANSPARENT = 2048,
    DFCS_HOT = 4096,
    DFCS_ADJUSTRECT = 8192,
    DFCS_FLAT = 16384,
    DFCS_MONO = 32768,
};
#pragma pop_macro("DFCS_MONO")
#pragma pop_macro("DFCS_FLAT")
#pragma pop_macro("DFCS_ADJUSTRECT")
#pragma pop_macro("DFCS_HOT")
#pragma pop_macro("DFCS_TRANSPARENT")
#pragma pop_macro("DFCS_CHECKED")
#pragma pop_macro("DFCS_PUSHED")
#pragma pop_macro("DFCS_INACTIVE")
#pragma pop_macro("DFCS_BUTTONPUSH")
#pragma pop_macro("DFCS_BUTTON3STATE")
#pragma pop_macro("DFCS_BUTTONRADIO")
#pragma pop_macro("DFCS_BUTTONRADIOMASK")
#pragma pop_macro("DFCS_BUTTONRADIOIMAGE")
#pragma pop_macro("DFCS_BUTTONCHECK")
#pragma pop_macro("DFCS_SCROLLSIZEGRIPRIGHT")
#pragma pop_macro("DFCS_SCROLLSIZEGRIP")
#pragma pop_macro("DFCS_SCROLLCOMBOBOX")
#pragma pop_macro("DFCS_SCROLLRIGHT")
#pragma pop_macro("DFCS_SCROLLLEFT")
#pragma pop_macro("DFCS_SCROLLDOWN")
#pragma pop_macro("DFCS_SCROLLUP")
#pragma pop_macro("DFCS_MENUARROWRIGHT")
#pragma pop_macro("DFCS_MENUBULLET")
#pragma pop_macro("DFCS_MENUCHECK")
#pragma pop_macro("DFCS_MENUARROW")
#pragma pop_macro("DFCS_CAPTIONHELP")
#pragma pop_macro("DFCS_CAPTIONRESTORE")
#pragma pop_macro("DFCS_CAPTIONMAX")
#pragma pop_macro("DFCS_CAPTIONMIN")
#pragma pop_macro("DFCS_CAPTIONCLOSE")

#pragma push_macro("CDS_UPDATEREGISTRY")
#pragma push_macro("CDS_TEST")
#pragma push_macro("CDS_FULLSCREEN")
#pragma push_macro("CDS_GLOBAL")
#pragma push_macro("CDS_SET_PRIMARY")
#pragma push_macro("CDS_VIDEOPARAMETERS")
#pragma push_macro("CDS_ENABLE_UNSAFE_MODES")
#pragma push_macro("CDS_DISABLE_UNSAFE_MODES")
#pragma push_macro("CDS_RESET")
#pragma push_macro("CDS_RESET_EX")
#pragma push_macro("CDS_NORESET")
#undef CDS_UPDATEREGISTRY
#undef CDS_TEST
#undef CDS_FULLSCREEN
#undef CDS_GLOBAL
#undef CDS_SET_PRIMARY
#undef CDS_VIDEOPARAMETERS
#undef CDS_ENABLE_UNSAFE_MODES
#undef CDS_DISABLE_UNSAFE_MODES
#undef CDS_RESET
#undef CDS_RESET_EX
#undef CDS_NORESET
enum class
    [[clang::flag_enum]]
    CDS_TYPE : int
{
    CDS_UPDATEREGISTRY = 1,
    CDS_TEST = 2,
    CDS_FULLSCREEN = 4,
    CDS_GLOBAL = 8,
    CDS_SET_PRIMARY = 16,
    CDS_VIDEOPARAMETERS = 32,
    CDS_ENABLE_UNSAFE_MODES = 256,
    CDS_DISABLE_UNSAFE_MODES = 512,
    CDS_RESET = 1073741824,
    CDS_RESET_EX = 536870912,
    CDS_NORESET = 268435456,
};
#pragma pop_macro("CDS_NORESET")
#pragma pop_macro("CDS_RESET_EX")
#pragma pop_macro("CDS_RESET")
#pragma pop_macro("CDS_DISABLE_UNSAFE_MODES")
#pragma pop_macro("CDS_ENABLE_UNSAFE_MODES")
#pragma pop_macro("CDS_VIDEOPARAMETERS")
#pragma pop_macro("CDS_SET_PRIMARY")
#pragma pop_macro("CDS_GLOBAL")
#pragma pop_macro("CDS_FULLSCREEN")
#pragma pop_macro("CDS_TEST")
#pragma pop_macro("CDS_UPDATEREGISTRY")

#pragma push_macro("DISP_CHANGE_SUCCESSFUL")
#pragma push_macro("DISP_CHANGE_RESTART")
#pragma push_macro("DISP_CHANGE_FAILED")
#pragma push_macro("DISP_CHANGE_BADMODE")
#pragma push_macro("DISP_CHANGE_NOTUPDATED")
#pragma push_macro("DISP_CHANGE_BADFLAGS")
#pragma push_macro("DISP_CHANGE_BADPARAM")
#pragma push_macro("DISP_CHANGE_BADDUALVIEW")
#undef DISP_CHANGE_SUCCESSFUL
#undef DISP_CHANGE_RESTART
#undef DISP_CHANGE_FAILED
#undef DISP_CHANGE_BADMODE
#undef DISP_CHANGE_NOTUPDATED
#undef DISP_CHANGE_BADFLAGS
#undef DISP_CHANGE_BADPARAM
#undef DISP_CHANGE_BADDUALVIEW
enum class
    DISP_CHANGE : int
{
    DISP_CHANGE_SUCCESSFUL = 0,
    DISP_CHANGE_RESTART = 1,
    DISP_CHANGE_FAILED = -1,
    DISP_CHANGE_BADMODE = -2,
    DISP_CHANGE_NOTUPDATED = -3,
    DISP_CHANGE_BADFLAGS = -4,
    DISP_CHANGE_BADPARAM = -5,
    DISP_CHANGE_BADDUALVIEW = -6,
};
#pragma pop_macro("DISP_CHANGE_BADDUALVIEW")
#pragma pop_macro("DISP_CHANGE_BADPARAM")
#pragma pop_macro("DISP_CHANGE_BADFLAGS")
#pragma pop_macro("DISP_CHANGE_NOTUPDATED")
#pragma pop_macro("DISP_CHANGE_BADMODE")
#pragma pop_macro("DISP_CHANGE_FAILED")
#pragma pop_macro("DISP_CHANGE_RESTART")
#pragma pop_macro("DISP_CHANGE_SUCCESSFUL")

#pragma push_macro("DST_COMPLEX")
#pragma push_macro("DST_TEXT")
#pragma push_macro("DST_PREFIXTEXT")
#pragma push_macro("DST_ICON")
#pragma push_macro("DST_BITMAP")
#pragma push_macro("DSS_NORMAL")
#pragma push_macro("DSS_UNION")
#pragma push_macro("DSS_DISABLED")
#pragma push_macro("DSS_MONO")
#pragma push_macro("DSS_HIDEPREFIX")
#pragma push_macro("DSS_PREFIXONLY")
#pragma push_macro("DSS_RIGHT")
#undef DST_COMPLEX
#undef DST_TEXT
#undef DST_PREFIXTEXT
#undef DST_ICON
#undef DST_BITMAP
#undef DSS_NORMAL
#undef DSS_UNION
#undef DSS_DISABLED
#undef DSS_MONO
#undef DSS_HIDEPREFIX
#undef DSS_PREFIXONLY
#undef DSS_RIGHT
enum class
    [[clang::flag_enum]]
    DRAWSTATE_FLAGS : int
{
    DST_COMPLEX = 0,
    DST_TEXT = 1,
    DST_PREFIXTEXT = 2,
    DST_ICON = 3,
    DST_BITMAP = 4,
    DSS_NORMAL = 0,
    DSS_UNION = 16,
    DSS_DISABLED = 32,
    DSS_MONO = 128,
    DSS_HIDEPREFIX = 512,
    DSS_PREFIXONLY = 1024,
    DSS_RIGHT = 32768,
};
#pragma pop_macro("DSS_RIGHT")
#pragma pop_macro("DSS_PREFIXONLY")
#pragma pop_macro("DSS_HIDEPREFIX")
#pragma pop_macro("DSS_MONO")
#pragma pop_macro("DSS_DISABLED")
#pragma pop_macro("DSS_UNION")
#pragma pop_macro("DSS_NORMAL")
#pragma pop_macro("DST_BITMAP")
#pragma pop_macro("DST_ICON")
#pragma pop_macro("DST_PREFIXTEXT")
#pragma pop_macro("DST_TEXT")
#pragma pop_macro("DST_COMPLEX")

#pragma push_macro("GID_BEGIN")
#pragma push_macro("GID_END")
#pragma push_macro("GID_ZOOM")
#pragma push_macro("GID_PAN")
#pragma push_macro("GID_ROTATE")
#pragma push_macro("GID_TWOFINGERTAP")
#pragma push_macro("GID_PRESSANDTAP")
#pragma push_macro("GID_ROLLOVER")
#undef GID_BEGIN
#undef GID_END
#undef GID_ZOOM
#undef GID_PAN
#undef GID_ROTATE
#undef GID_TWOFINGERTAP
#undef GID_PRESSANDTAP
#undef GID_ROLLOVER
enum class
    [[clang::flag_enum]]
    GESTURECONFIG_ID : int
{
    GID_BEGIN = 1,
    GID_END = 2,
    GID_ZOOM = 3,
    GID_PAN = 4,
    GID_ROTATE = 5,
    GID_TWOFINGERTAP = 6,
    GID_PRESSANDTAP = 7,
    GID_ROLLOVER = 7,
};
#pragma pop_macro("GID_ROLLOVER")
#pragma pop_macro("GID_PRESSANDTAP")
#pragma pop_macro("GID_TWOFINGERTAP")
#pragma pop_macro("GID_ROTATE")
#pragma pop_macro("GID_PAN")
#pragma pop_macro("GID_ZOOM")
#pragma pop_macro("GID_END")
#pragma pop_macro("GID_BEGIN")

#pragma push_macro("GC_ALLGESTURES")
#pragma push_macro("GC_ZOOM")
#pragma push_macro("GC_PAN")
#pragma push_macro("GC_PAN_WITH_SINGLE_FINGER_VERTICALLY")
#pragma push_macro("GC_PAN_WITH_SINGLE_FINGER_HORIZONTALLY")
#pragma push_macro("GC_PAN_WITH_GUTTER")
#pragma push_macro("GC_PAN_WITH_INERTIA")
#pragma push_macro("GC_ROTATE")
#pragma push_macro("GC_TWOFINGERTAP")
#pragma push_macro("GC_PRESSANDTAP")
#pragma push_macro("GC_ROLLOVER")
#undef GC_ALLGESTURES
#undef GC_ZOOM
#undef GC_PAN
#undef GC_PAN_WITH_SINGLE_FINGER_VERTICALLY
#undef GC_PAN_WITH_SINGLE_FINGER_HORIZONTALLY
#undef GC_PAN_WITH_GUTTER
#undef GC_PAN_WITH_INERTIA
#undef GC_ROTATE
#undef GC_TWOFINGERTAP
#undef GC_PRESSANDTAP
#undef GC_ROLLOVER
enum class
    [[clang::flag_enum]]
    GESTURECONFIG_FLAGS : int
{
    GC_ALLGESTURES = 1,
    GC_ZOOM = 1,
    GC_PAN = 1,
    GC_PAN_WITH_SINGLE_FINGER_VERTICALLY = 2,
    GC_PAN_WITH_SINGLE_FINGER_HORIZONTALLY = 4,
    GC_PAN_WITH_GUTTER = 8,
    GC_PAN_WITH_INERTIA = 16,
    GC_ROTATE = 1,
    GC_TWOFINGERTAP = 1,
    GC_PRESSANDTAP = 1,
    GC_ROLLOVER = 1,
};
#pragma pop_macro("GC_ROLLOVER")
#pragma pop_macro("GC_PRESSANDTAP")
#pragma pop_macro("GC_TWOFINGERTAP")
#pragma pop_macro("GC_ROTATE")
#pragma pop_macro("GC_PAN_WITH_INERTIA")
#pragma pop_macro("GC_PAN_WITH_GUTTER")
#pragma pop_macro("GC_PAN_WITH_SINGLE_FINGER_HORIZONTALLY")
#pragma pop_macro("GC_PAN_WITH_SINGLE_FINGER_VERTICALLY")
#pragma pop_macro("GC_PAN")
#pragma pop_macro("GC_ZOOM")
#pragma pop_macro("GC_ALLGESTURES")

#pragma push_macro("POINTER_FLAG_NONE")
#pragma push_macro("POINTER_FLAG_NEW")
#pragma push_macro("POINTER_FLAG_INRANGE")
#pragma push_macro("POINTER_FLAG_INCONTACT")
#pragma push_macro("POINTER_FLAG_FIRSTBUTTON")
#pragma push_macro("POINTER_FLAG_SECONDBUTTON")
#pragma push_macro("POINTER_FLAG_THIRDBUTTON")
#pragma push_macro("POINTER_FLAG_FOURTHBUTTON")
#pragma push_macro("POINTER_FLAG_FIFTHBUTTON")
#pragma push_macro("POINTER_FLAG_PRIMARY")
#pragma push_macro("POINTER_FLAG_CONFIDENCE")
#pragma push_macro("POINTER_FLAG_CANCELED")
#pragma push_macro("POINTER_FLAG_DOWN")
#pragma push_macro("POINTER_FLAG_UPDATE")
#pragma push_macro("POINTER_FLAG_UP")
#pragma push_macro("POINTER_FLAG_WHEEL")
#pragma push_macro("POINTER_FLAG_HWHEEL")
#pragma push_macro("POINTER_FLAG_CAPTURECHANGED")
#pragma push_macro("POINTER_FLAG_HASTRANSFORM")
#undef POINTER_FLAG_NONE
#undef POINTER_FLAG_NEW
#undef POINTER_FLAG_INRANGE
#undef POINTER_FLAG_INCONTACT
#undef POINTER_FLAG_FIRSTBUTTON
#undef POINTER_FLAG_SECONDBUTTON
#undef POINTER_FLAG_THIRDBUTTON
#undef POINTER_FLAG_FOURTHBUTTON
#undef POINTER_FLAG_FIFTHBUTTON
#undef POINTER_FLAG_PRIMARY
#undef POINTER_FLAG_CONFIDENCE
#undef POINTER_FLAG_CANCELED
#undef POINTER_FLAG_DOWN
#undef POINTER_FLAG_UPDATE
#undef POINTER_FLAG_UP
#undef POINTER_FLAG_WHEEL
#undef POINTER_FLAG_HWHEEL
#undef POINTER_FLAG_CAPTURECHANGED
#undef POINTER_FLAG_HASTRANSFORM
enum class
    [[clang::flag_enum]]
    POINTER_FLAGS : int
{
    POINTER_FLAG_NONE = 0,
    POINTER_FLAG_NEW = 1,
    POINTER_FLAG_INRANGE = 2,
    POINTER_FLAG_INCONTACT = 4,
    POINTER_FLAG_FIRSTBUTTON = 16,
    POINTER_FLAG_SECONDBUTTON = 32,
    POINTER_FLAG_THIRDBUTTON = 64,
    POINTER_FLAG_FOURTHBUTTON = 128,
    POINTER_FLAG_FIFTHBUTTON = 256,
    POINTER_FLAG_PRIMARY = 8192,
    POINTER_FLAG_CONFIDENCE = 16384,
    POINTER_FLAG_CANCELED = 32768,
    POINTER_FLAG_DOWN = 65536,
    POINTER_FLAG_UPDATE = 131072,
    POINTER_FLAG_UP = 262144,
    POINTER_FLAG_WHEEL = 524288,
    POINTER_FLAG_HWHEEL = 1048576,
    POINTER_FLAG_CAPTURECHANGED = 2097152,
    POINTER_FLAG_HASTRANSFORM = 4194304,
};
#pragma pop_macro("POINTER_FLAG_HASTRANSFORM")
#pragma pop_macro("POINTER_FLAG_CAPTURECHANGED")
#pragma pop_macro("POINTER_FLAG_HWHEEL")
#pragma pop_macro("POINTER_FLAG_WHEEL")
#pragma pop_macro("POINTER_FLAG_UP")
#pragma pop_macro("POINTER_FLAG_UPDATE")
#pragma pop_macro("POINTER_FLAG_DOWN")
#pragma pop_macro("POINTER_FLAG_CANCELED")
#pragma pop_macro("POINTER_FLAG_CONFIDENCE")
#pragma pop_macro("POINTER_FLAG_PRIMARY")
#pragma pop_macro("POINTER_FLAG_FIFTHBUTTON")
#pragma pop_macro("POINTER_FLAG_FOURTHBUTTON")
#pragma pop_macro("POINTER_FLAG_THIRDBUTTON")
#pragma pop_macro("POINTER_FLAG_SECONDBUTTON")
#pragma pop_macro("POINTER_FLAG_FIRSTBUTTON")
#pragma pop_macro("POINTER_FLAG_INCONTACT")
#pragma pop_macro("POINTER_FLAG_INRANGE")
#pragma pop_macro("POINTER_FLAG_NEW")
#pragma pop_macro("POINTER_FLAG_NONE")

#pragma push_macro("RDW_INVALIDATE")
#pragma push_macro("RDW_INTERNALPAINT")
#pragma push_macro("RDW_ERASE")
#pragma push_macro("RDW_VALIDATE")
#pragma push_macro("RDW_NOINTERNALPAINT")
#pragma push_macro("RDW_NOERASE")
#pragma push_macro("RDW_NOCHILDREN")
#pragma push_macro("RDW_ALLCHILDREN")
#pragma push_macro("RDW_UPDATENOW")
#pragma push_macro("RDW_ERASENOW")
#pragma push_macro("RDW_FRAME")
#pragma push_macro("RDW_NOFRAME")
#undef RDW_INVALIDATE
#undef RDW_INTERNALPAINT
#undef RDW_ERASE
#undef RDW_VALIDATE
#undef RDW_NOINTERNALPAINT
#undef RDW_NOERASE
#undef RDW_NOCHILDREN
#undef RDW_ALLCHILDREN
#undef RDW_UPDATENOW
#undef RDW_ERASENOW
#undef RDW_FRAME
#undef RDW_NOFRAME
enum class
    [[clang::flag_enum]]
    REDRAW_WINDOW_FLAGS : int
{
    RDW_INVALIDATE = 1,
    RDW_INTERNALPAINT = 2,
    RDW_ERASE = 4,
    RDW_VALIDATE = 8,
    RDW_NOINTERNALPAINT = 16,
    RDW_NOERASE = 32,
    RDW_NOCHILDREN = 64,
    RDW_ALLCHILDREN = 128,
    RDW_UPDATENOW = 256,
    RDW_ERASENOW = 512,
    RDW_FRAME = 1024,
    RDW_NOFRAME = 2048,
};
#pragma pop_macro("RDW_NOFRAME")
#pragma pop_macro("RDW_FRAME")
#pragma pop_macro("RDW_ERASENOW")
#pragma pop_macro("RDW_UPDATENOW")
#pragma pop_macro("RDW_ALLCHILDREN")
#pragma pop_macro("RDW_NOCHILDREN")
#pragma pop_macro("RDW_NOERASE")
#pragma pop_macro("RDW_NOINTERNALPAINT")
#pragma pop_macro("RDW_VALIDATE")
#pragma pop_macro("RDW_ERASE")
#pragma pop_macro("RDW_INTERNALPAINT")
#pragma pop_macro("RDW_INVALIDATE")

#pragma push_macro("SW_PARENTCLOSING")
#pragma push_macro("SW_OTHERZOOM")
#pragma push_macro("SW_PARENTOPENING")
#pragma push_macro("SW_OTHERUNZOOM")
#undef SW_PARENTCLOSING
#undef SW_OTHERZOOM
#undef SW_PARENTOPENING
#undef SW_OTHERUNZOOM
enum class
    SHOW_WINDOW_STATUS : int
{
    SW_PARENTCLOSING = 1,
    SW_OTHERZOOM = 2,
    SW_PARENTOPENING = 3,
    SW_OTHERUNZOOM = 4,
};
#pragma pop_macro("SW_OTHERUNZOOM")
#pragma pop_macro("SW_PARENTOPENING")
#pragma pop_macro("SW_OTHERZOOM")
#pragma pop_macro("SW_PARENTCLOSING")

#pragma push_macro("SKF_STICKYKEYSON")
#pragma push_macro("SKF_AVAILABLE")
#pragma push_macro("SKF_HOTKEYACTIVE")
#pragma push_macro("SKF_CONFIRMHOTKEY")
#pragma push_macro("SKF_HOTKEYSOUND")
#pragma push_macro("SKF_INDICATOR")
#pragma push_macro("SKF_AUDIBLEFEEDBACK")
#pragma push_macro("SKF_TRISTATE")
#pragma push_macro("SKF_TWOKEYSOFF")
#pragma push_macro("SKF_LALTLATCHED")
#pragma push_macro("SKF_LCTLLATCHED")
#pragma push_macro("SKF_LSHIFTLATCHED")
#pragma push_macro("SKF_RALTLATCHED")
#pragma push_macro("SKF_RCTLLATCHED")
#pragma push_macro("SKF_RSHIFTLATCHED")
#pragma push_macro("SKF_LWINLATCHED")
#pragma push_macro("SKF_RWINLATCHED")
#pragma push_macro("SKF_LALTLOCKED")
#pragma push_macro("SKF_LCTLLOCKED")
#pragma push_macro("SKF_LSHIFTLOCKED")
#pragma push_macro("SKF_RALTLOCKED")
#pragma push_macro("SKF_RCTLLOCKED")
#pragma push_macro("SKF_RSHIFTLOCKED")
#pragma push_macro("SKF_LWINLOCKED")
#pragma push_macro("SKF_RWINLOCKED")
#undef SKF_STICKYKEYSON
#undef SKF_AVAILABLE
#undef SKF_HOTKEYACTIVE
#undef SKF_CONFIRMHOTKEY
#undef SKF_HOTKEYSOUND
#undef SKF_INDICATOR
#undef SKF_AUDIBLEFEEDBACK
#undef SKF_TRISTATE
#undef SKF_TWOKEYSOFF
#undef SKF_LALTLATCHED
#undef SKF_LCTLLATCHED
#undef SKF_LSHIFTLATCHED
#undef SKF_RALTLATCHED
#undef SKF_RCTLLATCHED
#undef SKF_RSHIFTLATCHED
#undef SKF_LWINLATCHED
#undef SKF_RWINLATCHED
#undef SKF_LALTLOCKED
#undef SKF_LCTLLOCKED
#undef SKF_LSHIFTLOCKED
#undef SKF_RALTLOCKED
#undef SKF_RCTLLOCKED
#undef SKF_RSHIFTLOCKED
#undef SKF_LWINLOCKED
#undef SKF_RWINLOCKED
enum class
    [[clang::flag_enum]]
    STICKYKEYS_FLAGS : int
{
    SKF_STICKYKEYSON = 1,
    SKF_AVAILABLE = 2,
    SKF_HOTKEYACTIVE = 4,
    SKF_CONFIRMHOTKEY = 8,
    SKF_HOTKEYSOUND = 16,
    SKF_INDICATOR = 32,
    SKF_AUDIBLEFEEDBACK = 64,
    SKF_TRISTATE = 128,
    SKF_TWOKEYSOFF = 256,
    SKF_LALTLATCHED = 268435456,
    SKF_LCTLLATCHED = 67108864,
    SKF_LSHIFTLATCHED = 16777216,
    SKF_RALTLATCHED = 536870912,
    SKF_RCTLLATCHED = 134217728,
    SKF_RSHIFTLATCHED = 33554432,
    SKF_LWINLATCHED = 1073741824,
    SKF_RWINLATCHED = -2147483648,
    SKF_LALTLOCKED = 1048576,
    SKF_LCTLLOCKED = 262144,
    SKF_LSHIFTLOCKED = 65536,
    SKF_RALTLOCKED = 2097152,
    SKF_RCTLLOCKED = 524288,
    SKF_RSHIFTLOCKED = 131072,
    SKF_LWINLOCKED = 4194304,
    SKF_RWINLOCKED = 8388608,
};
#pragma pop_macro("SKF_RWINLOCKED")
#pragma pop_macro("SKF_LWINLOCKED")
#pragma pop_macro("SKF_RSHIFTLOCKED")
#pragma pop_macro("SKF_RCTLLOCKED")
#pragma pop_macro("SKF_RALTLOCKED")
#pragma pop_macro("SKF_LSHIFTLOCKED")
#pragma pop_macro("SKF_LCTLLOCKED")
#pragma pop_macro("SKF_LALTLOCKED")
#pragma pop_macro("SKF_RWINLATCHED")
#pragma pop_macro("SKF_LWINLATCHED")
#pragma pop_macro("SKF_RSHIFTLATCHED")
#pragma pop_macro("SKF_RCTLLATCHED")
#pragma pop_macro("SKF_RALTLATCHED")
#pragma pop_macro("SKF_LSHIFTLATCHED")
#pragma pop_macro("SKF_LCTLLATCHED")
#pragma pop_macro("SKF_LALTLATCHED")
#pragma pop_macro("SKF_TWOKEYSOFF")
#pragma pop_macro("SKF_TRISTATE")
#pragma pop_macro("SKF_AUDIBLEFEEDBACK")
#pragma pop_macro("SKF_INDICATOR")
#pragma pop_macro("SKF_HOTKEYSOUND")
#pragma pop_macro("SKF_CONFIRMHOTKEY")
#pragma pop_macro("SKF_HOTKEYACTIVE")
#pragma pop_macro("SKF_AVAILABLE")
#pragma pop_macro("SKF_STICKYKEYSON")

#pragma push_macro("SPI_GETBEEP")
#pragma push_macro("SPI_SETBEEP")
#pragma push_macro("SPI_GETMOUSE")
#pragma push_macro("SPI_SETMOUSE")
#pragma push_macro("SPI_GETBORDER")
#pragma push_macro("SPI_SETBORDER")
#pragma push_macro("SPI_GETKEYBOARDSPEED")
#pragma push_macro("SPI_SETKEYBOARDSPEED")
#pragma push_macro("SPI_LANGDRIVER")
#pragma push_macro("SPI_ICONHORIZONTALSPACING")
#pragma push_macro("SPI_GETSCREENSAVETIMEOUT")
#pragma push_macro("SPI_SETSCREENSAVETIMEOUT")
#pragma push_macro("SPI_GETSCREENSAVEACTIVE")
#pragma push_macro("SPI_SETSCREENSAVEACTIVE")
#pragma push_macro("SPI_GETGRIDGRANULARITY")
#pragma push_macro("SPI_SETGRIDGRANULARITY")
#pragma push_macro("SPI_SETDESKWALLPAPER")
#pragma push_macro("SPI_SETDESKPATTERN")
#pragma push_macro("SPI_GETKEYBOARDDELAY")
#pragma push_macro("SPI_SETKEYBOARDDELAY")
#pragma push_macro("SPI_ICONVERTICALSPACING")
#pragma push_macro("SPI_GETICONTITLEWRAP")
#pragma push_macro("SPI_SETICONTITLEWRAP")
#pragma push_macro("SPI_GETMENUDROPALIGNMENT")
#pragma push_macro("SPI_SETMENUDROPALIGNMENT")
#pragma push_macro("SPI_SETDOUBLECLKWIDTH")
#pragma push_macro("SPI_SETDOUBLECLKHEIGHT")
#pragma push_macro("SPI_GETICONTITLELOGFONT")
#pragma push_macro("SPI_SETDOUBLECLICKTIME")
#pragma push_macro("SPI_SETMOUSEBUTTONSWAP")
#pragma push_macro("SPI_SETICONTITLELOGFONT")
#pragma push_macro("SPI_GETFASTTASKSWITCH")
#pragma push_macro("SPI_SETFASTTASKSWITCH")
#pragma push_macro("SPI_SETDRAGFULLWINDOWS")
#pragma push_macro("SPI_GETDRAGFULLWINDOWS")
#pragma push_macro("SPI_GETNONCLIENTMETRICS")
#pragma push_macro("SPI_SETNONCLIENTMETRICS")
#pragma push_macro("SPI_GETMINIMIZEDMETRICS")
#pragma push_macro("SPI_SETMINIMIZEDMETRICS")
#pragma push_macro("SPI_GETICONMETRICS")
#pragma push_macro("SPI_SETICONMETRICS")
#pragma push_macro("SPI_SETWORKAREA")
#pragma push_macro("SPI_GETWORKAREA")
#pragma push_macro("SPI_SETPENWINDOWS")
#pragma push_macro("SPI_GETHIGHCONTRAST")
#pragma push_macro("SPI_SETHIGHCONTRAST")
#pragma push_macro("SPI_GETKEYBOARDPREF")
#pragma push_macro("SPI_SETKEYBOARDPREF")
#pragma push_macro("SPI_GETSCREENREADER")
#pragma push_macro("SPI_SETSCREENREADER")
#pragma push_macro("SPI_GETANIMATION")
#pragma push_macro("SPI_SETANIMATION")
#pragma push_macro("SPI_GETFONTSMOOTHING")
#pragma push_macro("SPI_SETFONTSMOOTHING")
#pragma push_macro("SPI_SETDRAGWIDTH")
#pragma push_macro("SPI_SETDRAGHEIGHT")
#pragma push_macro("SPI_SETHANDHELD")
#pragma push_macro("SPI_GETLOWPOWERTIMEOUT")
#pragma push_macro("SPI_GETPOWEROFFTIMEOUT")
#pragma push_macro("SPI_SETLOWPOWERTIMEOUT")
#pragma push_macro("SPI_SETPOWEROFFTIMEOUT")
#pragma push_macro("SPI_GETLOWPOWERACTIVE")
#pragma push_macro("SPI_GETPOWEROFFACTIVE")
#pragma push_macro("SPI_SETLOWPOWERACTIVE")
#pragma push_macro("SPI_SETPOWEROFFACTIVE")
#pragma push_macro("SPI_SETCURSORS")
#pragma push_macro("SPI_SETICONS")
#pragma push_macro("SPI_GETDEFAULTINPUTLANG")
#pragma push_macro("SPI_SETDEFAULTINPUTLANG")
#pragma push_macro("SPI_SETLANGTOGGLE")
#pragma push_macro("SPI_GETWINDOWSEXTENSION")
#pragma push_macro("SPI_SETMOUSETRAILS")
#pragma push_macro("SPI_GETMOUSETRAILS")
#pragma push_macro("SPI_SETSCREENSAVERRUNNING")
#pragma push_macro("SPI_SCREENSAVERRUNNING")
#pragma push_macro("SPI_GETFILTERKEYS")
#pragma push_macro("SPI_SETFILTERKEYS")
#pragma push_macro("SPI_GETTOGGLEKEYS")
#pragma push_macro("SPI_SETTOGGLEKEYS")
#pragma push_macro("SPI_GETMOUSEKEYS")
#pragma push_macro("SPI_SETMOUSEKEYS")
#pragma push_macro("SPI_GETSHOWSOUNDS")
#pragma push_macro("SPI_SETSHOWSOUNDS")
#pragma push_macro("SPI_GETSTICKYKEYS")
#pragma push_macro("SPI_SETSTICKYKEYS")
#pragma push_macro("SPI_GETACCESSTIMEOUT")
#pragma push_macro("SPI_SETACCESSTIMEOUT")
#pragma push_macro("SPI_GETSERIALKEYS")
#pragma push_macro("SPI_SETSERIALKEYS")
#pragma push_macro("SPI_GETSOUNDSENTRY")
#pragma push_macro("SPI_SETSOUNDSENTRY")
#pragma push_macro("SPI_GETSNAPTODEFBUTTON")
#pragma push_macro("SPI_SETSNAPTODEFBUTTON")
#pragma push_macro("SPI_GETMOUSEHOVERWIDTH")
#pragma push_macro("SPI_SETMOUSEHOVERWIDTH")
#pragma push_macro("SPI_GETMOUSEHOVERHEIGHT")
#pragma push_macro("SPI_SETMOUSEHOVERHEIGHT")
#pragma push_macro("SPI_GETMOUSEHOVERTIME")
#pragma push_macro("SPI_SETMOUSEHOVERTIME")
#pragma push_macro("SPI_GETWHEELSCROLLLINES")
#pragma push_macro("SPI_SETWHEELSCROLLLINES")
#pragma push_macro("SPI_GETMENUSHOWDELAY")
#pragma push_macro("SPI_SETMENUSHOWDELAY")
#pragma push_macro("SPI_GETWHEELSCROLLCHARS")
#pragma push_macro("SPI_SETWHEELSCROLLCHARS")
#pragma push_macro("SPI_GETSHOWIMEUI")
#pragma push_macro("SPI_SETSHOWIMEUI")
#pragma push_macro("SPI_GETMOUSESPEED")
#pragma push_macro("SPI_SETMOUSESPEED")
#pragma push_macro("SPI_GETSCREENSAVERRUNNING")
#pragma push_macro("SPI_GETDESKWALLPAPER")
#pragma push_macro("SPI_GETAUDIODESCRIPTION")
#pragma push_macro("SPI_SETAUDIODESCRIPTION")
#pragma push_macro("SPI_GETSCREENSAVESECURE")
#pragma push_macro("SPI_SETSCREENSAVESECURE")
#pragma push_macro("SPI_GETHUNGAPPTIMEOUT")
#pragma push_macro("SPI_SETHUNGAPPTIMEOUT")
#pragma push_macro("SPI_GETWAITTOKILLTIMEOUT")
#pragma push_macro("SPI_SETWAITTOKILLTIMEOUT")
#pragma push_macro("SPI_GETWAITTOKILLSERVICETIMEOUT")
#pragma push_macro("SPI_SETWAITTOKILLSERVICETIMEOUT")
#pragma push_macro("SPI_GETMOUSEDOCKTHRESHOLD")
#pragma push_macro("SPI_SETMOUSEDOCKTHRESHOLD")
#pragma push_macro("SPI_GETPENDOCKTHRESHOLD")
#pragma push_macro("SPI_SETPENDOCKTHRESHOLD")
#pragma push_macro("SPI_GETWINARRANGING")
#pragma push_macro("SPI_SETWINARRANGING")
#pragma push_macro("SPI_GETMOUSEDRAGOUTTHRESHOLD")
#pragma push_macro("SPI_SETMOUSEDRAGOUTTHRESHOLD")
#pragma push_macro("SPI_GETPENDRAGOUTTHRESHOLD")
#pragma push_macro("SPI_SETPENDRAGOUTTHRESHOLD")
#pragma push_macro("SPI_GETMOUSESIDEMOVETHRESHOLD")
#pragma push_macro("SPI_SETMOUSESIDEMOVETHRESHOLD")
#pragma push_macro("SPI_GETPENSIDEMOVETHRESHOLD")
#pragma push_macro("SPI_SETPENSIDEMOVETHRESHOLD")
#pragma push_macro("SPI_GETDRAGFROMMAXIMIZE")
#pragma push_macro("SPI_SETDRAGFROMMAXIMIZE")
#pragma push_macro("SPI_GETSNAPSIZING")
#pragma push_macro("SPI_SETSNAPSIZING")
#pragma push_macro("SPI_GETDOCKMOVING")
#pragma push_macro("SPI_SETDOCKMOVING")
#pragma push_macro("SPI_GETTOUCHPREDICTIONPARAMETERS")
#pragma push_macro("SPI_SETTOUCHPREDICTIONPARAMETERS")
#pragma push_macro("SPI_GETLOGICALDPIOVERRIDE")
#pragma push_macro("SPI_SETLOGICALDPIOVERRIDE")
#pragma push_macro("SPI_GETMENURECT")
#pragma push_macro("SPI_SETMENURECT")
#pragma push_macro("SPI_GETTOUCHPADPARAMETERS")
#pragma push_macro("SPI_SETTOUCHPADPARAMETERS")
#pragma push_macro("SPI_GETACTIVEWINDOWTRACKING")
#pragma push_macro("SPI_SETACTIVEWINDOWTRACKING")
#pragma push_macro("SPI_GETMENUANIMATION")
#pragma push_macro("SPI_SETMENUANIMATION")
#pragma push_macro("SPI_GETCOMBOBOXANIMATION")
#pragma push_macro("SPI_SETCOMBOBOXANIMATION")
#pragma push_macro("SPI_GETLISTBOXSMOOTHSCROLLING")
#pragma push_macro("SPI_SETLISTBOXSMOOTHSCROLLING")
#pragma push_macro("SPI_GETGRADIENTCAPTIONS")
#pragma push_macro("SPI_SETGRADIENTCAPTIONS")
#pragma push_macro("SPI_GETKEYBOARDCUES")
#pragma push_macro("SPI_SETKEYBOARDCUES")
#pragma push_macro("SPI_GETMENUUNDERLINES")
#pragma push_macro("SPI_SETMENUUNDERLINES")
#pragma push_macro("SPI_GETACTIVEWNDTRKZORDER")
#pragma push_macro("SPI_SETACTIVEWNDTRKZORDER")
#pragma push_macro("SPI_GETHOTTRACKING")
#pragma push_macro("SPI_SETHOTTRACKING")
#pragma push_macro("SPI_GETMENUFADE")
#pragma push_macro("SPI_SETMENUFADE")
#pragma push_macro("SPI_GETSELECTIONFADE")
#pragma push_macro("SPI_SETSELECTIONFADE")
#pragma push_macro("SPI_GETTOOLTIPANIMATION")
#pragma push_macro("SPI_SETTOOLTIPANIMATION")
#pragma push_macro("SPI_GETTOOLTIPFADE")
#pragma push_macro("SPI_SETTOOLTIPFADE")
#pragma push_macro("SPI_GETCURSORSHADOW")
#pragma push_macro("SPI_SETCURSORSHADOW")
#pragma push_macro("SPI_GETMOUSESONAR")
#pragma push_macro("SPI_SETMOUSESONAR")
#pragma push_macro("SPI_GETMOUSECLICKLOCK")
#pragma push_macro("SPI_SETMOUSECLICKLOCK")
#pragma push_macro("SPI_GETMOUSEVANISH")
#pragma push_macro("SPI_SETMOUSEVANISH")
#pragma push_macro("SPI_GETFLATMENU")
#pragma push_macro("SPI_SETFLATMENU")
#pragma push_macro("SPI_GETDROPSHADOW")
#pragma push_macro("SPI_SETDROPSHADOW")
#pragma push_macro("SPI_GETBLOCKSENDINPUTRESETS")
#pragma push_macro("SPI_SETBLOCKSENDINPUTRESETS")
#pragma push_macro("SPI_GETUIEFFECTS")
#pragma push_macro("SPI_SETUIEFFECTS")
#pragma push_macro("SPI_GETDISABLEOVERLAPPEDCONTENT")
#pragma push_macro("SPI_SETDISABLEOVERLAPPEDCONTENT")
#pragma push_macro("SPI_GETCLIENTAREAANIMATION")
#pragma push_macro("SPI_SETCLIENTAREAANIMATION")
#pragma push_macro("SPI_GETCLEARTYPE")
#pragma push_macro("SPI_SETCLEARTYPE")
#pragma push_macro("SPI_GETSPEECHRECOGNITION")
#pragma push_macro("SPI_SETSPEECHRECOGNITION")
#pragma push_macro("SPI_GETCARETBROWSING")
#pragma push_macro("SPI_SETCARETBROWSING")
#pragma push_macro("SPI_GETTHREADLOCALINPUTSETTINGS")
#pragma push_macro("SPI_SETTHREADLOCALINPUTSETTINGS")
#pragma push_macro("SPI_GETSYSTEMLANGUAGEBAR")
#pragma push_macro("SPI_SETSYSTEMLANGUAGEBAR")
#pragma push_macro("SPI_GETFOREGROUNDLOCKTIMEOUT")
#pragma push_macro("SPI_SETFOREGROUNDLOCKTIMEOUT")
#pragma push_macro("SPI_GETACTIVEWNDTRKTIMEOUT")
#pragma push_macro("SPI_SETACTIVEWNDTRKTIMEOUT")
#pragma push_macro("SPI_GETFOREGROUNDFLASHCOUNT")
#pragma push_macro("SPI_SETFOREGROUNDFLASHCOUNT")
#pragma push_macro("SPI_GETCARETWIDTH")
#pragma push_macro("SPI_SETCARETWIDTH")
#pragma push_macro("SPI_GETMOUSECLICKLOCKTIME")
#pragma push_macro("SPI_SETMOUSECLICKLOCKTIME")
#pragma push_macro("SPI_GETFONTSMOOTHINGTYPE")
#pragma push_macro("SPI_SETFONTSMOOTHINGTYPE")
#pragma push_macro("SPI_GETFONTSMOOTHINGCONTRAST")
#pragma push_macro("SPI_SETFONTSMOOTHINGCONTRAST")
#pragma push_macro("SPI_GETFOCUSBORDERWIDTH")
#pragma push_macro("SPI_SETFOCUSBORDERWIDTH")
#pragma push_macro("SPI_GETFOCUSBORDERHEIGHT")
#pragma push_macro("SPI_SETFOCUSBORDERHEIGHT")
#pragma push_macro("SPI_GETFONTSMOOTHINGORIENTATION")
#pragma push_macro("SPI_SETFONTSMOOTHINGORIENTATION")
#pragma push_macro("SPI_GETMINIMUMHITRADIUS")
#pragma push_macro("SPI_SETMINIMUMHITRADIUS")
#pragma push_macro("SPI_GETMESSAGEDURATION")
#pragma push_macro("SPI_SETMESSAGEDURATION")
#pragma push_macro("SPI_GETCONTACTVISUALIZATION")
#pragma push_macro("SPI_SETCONTACTVISUALIZATION")
#pragma push_macro("SPI_GETGESTUREVISUALIZATION")
#pragma push_macro("SPI_SETGESTUREVISUALIZATION")
#pragma push_macro("SPI_GETMOUSEWHEELROUTING")
#pragma push_macro("SPI_SETMOUSEWHEELROUTING")
#pragma push_macro("SPI_GETPENVISUALIZATION")
#pragma push_macro("SPI_SETPENVISUALIZATION")
#pragma push_macro("SPI_GETPENARBITRATIONTYPE")
#pragma push_macro("SPI_SETPENARBITRATIONTYPE")
#pragma push_macro("SPI_GETCARETTIMEOUT")
#pragma push_macro("SPI_SETCARETTIMEOUT")
#pragma push_macro("SPI_GETHANDEDNESS")
#pragma push_macro("SPI_SETHANDEDNESS")
#undef SPI_GETBEEP
#undef SPI_SETBEEP
#undef SPI_GETMOUSE
#undef SPI_SETMOUSE
#undef SPI_GETBORDER
#undef SPI_SETBORDER
#undef SPI_GETKEYBOARDSPEED
#undef SPI_SETKEYBOARDSPEED
#undef SPI_LANGDRIVER
#undef SPI_ICONHORIZONTALSPACING
#undef SPI_GETSCREENSAVETIMEOUT
#undef SPI_SETSCREENSAVETIMEOUT
#undef SPI_GETSCREENSAVEACTIVE
#undef SPI_SETSCREENSAVEACTIVE
#undef SPI_GETGRIDGRANULARITY
#undef SPI_SETGRIDGRANULARITY
#undef SPI_SETDESKWALLPAPER
#undef SPI_SETDESKPATTERN
#undef SPI_GETKEYBOARDDELAY
#undef SPI_SETKEYBOARDDELAY
#undef SPI_ICONVERTICALSPACING
#undef SPI_GETICONTITLEWRAP
#undef SPI_SETICONTITLEWRAP
#undef SPI_GETMENUDROPALIGNMENT
#undef SPI_SETMENUDROPALIGNMENT
#undef SPI_SETDOUBLECLKWIDTH
#undef SPI_SETDOUBLECLKHEIGHT
#undef SPI_GETICONTITLELOGFONT
#undef SPI_SETDOUBLECLICKTIME
#undef SPI_SETMOUSEBUTTONSWAP
#undef SPI_SETICONTITLELOGFONT
#undef SPI_GETFASTTASKSWITCH
#undef SPI_SETFASTTASKSWITCH
#undef SPI_SETDRAGFULLWINDOWS
#undef SPI_GETDRAGFULLWINDOWS
#undef SPI_GETNONCLIENTMETRICS
#undef SPI_SETNONCLIENTMETRICS
#undef SPI_GETMINIMIZEDMETRICS
#undef SPI_SETMINIMIZEDMETRICS
#undef SPI_GETICONMETRICS
#undef SPI_SETICONMETRICS
#undef SPI_SETWORKAREA
#undef SPI_GETWORKAREA
#undef SPI_SETPENWINDOWS
#undef SPI_GETHIGHCONTRAST
#undef SPI_SETHIGHCONTRAST
#undef SPI_GETKEYBOARDPREF
#undef SPI_SETKEYBOARDPREF
#undef SPI_GETSCREENREADER
#undef SPI_SETSCREENREADER
#undef SPI_GETANIMATION
#undef SPI_SETANIMATION
#undef SPI_GETFONTSMOOTHING
#undef SPI_SETFONTSMOOTHING
#undef SPI_SETDRAGWIDTH
#undef SPI_SETDRAGHEIGHT
#undef SPI_SETHANDHELD
#undef SPI_GETLOWPOWERTIMEOUT
#undef SPI_GETPOWEROFFTIMEOUT
#undef SPI_SETLOWPOWERTIMEOUT
#undef SPI_SETPOWEROFFTIMEOUT
#undef SPI_GETLOWPOWERACTIVE
#undef SPI_GETPOWEROFFACTIVE
#undef SPI_SETLOWPOWERACTIVE
#undef SPI_SETPOWEROFFACTIVE
#undef SPI_SETCURSORS
#undef SPI_SETICONS
#undef SPI_GETDEFAULTINPUTLANG
#undef SPI_SETDEFAULTINPUTLANG
#undef SPI_SETLANGTOGGLE
#undef SPI_GETWINDOWSEXTENSION
#undef SPI_SETMOUSETRAILS
#undef SPI_GETMOUSETRAILS
#undef SPI_SETSCREENSAVERRUNNING
#undef SPI_SCREENSAVERRUNNING
#undef SPI_GETFILTERKEYS
#undef SPI_SETFILTERKEYS
#undef SPI_GETTOGGLEKEYS
#undef SPI_SETTOGGLEKEYS
#undef SPI_GETMOUSEKEYS
#undef SPI_SETMOUSEKEYS
#undef SPI_GETSHOWSOUNDS
#undef SPI_SETSHOWSOUNDS
#undef SPI_GETSTICKYKEYS
#undef SPI_SETSTICKYKEYS
#undef SPI_GETACCESSTIMEOUT
#undef SPI_SETACCESSTIMEOUT
#undef SPI_GETSERIALKEYS
#undef SPI_SETSERIALKEYS
#undef SPI_GETSOUNDSENTRY
#undef SPI_SETSOUNDSENTRY
#undef SPI_GETSNAPTODEFBUTTON
#undef SPI_SETSNAPTODEFBUTTON
#undef SPI_GETMOUSEHOVERWIDTH
#undef SPI_SETMOUSEHOVERWIDTH
#undef SPI_GETMOUSEHOVERHEIGHT
#undef SPI_SETMOUSEHOVERHEIGHT
#undef SPI_GETMOUSEHOVERTIME
#undef SPI_SETMOUSEHOVERTIME
#undef SPI_GETWHEELSCROLLLINES
#undef SPI_SETWHEELSCROLLLINES
#undef SPI_GETMENUSHOWDELAY
#undef SPI_SETMENUSHOWDELAY
#undef SPI_GETWHEELSCROLLCHARS
#undef SPI_SETWHEELSCROLLCHARS
#undef SPI_GETSHOWIMEUI
#undef SPI_SETSHOWIMEUI
#undef SPI_GETMOUSESPEED
#undef SPI_SETMOUSESPEED
#undef SPI_GETSCREENSAVERRUNNING
#undef SPI_GETDESKWALLPAPER
#undef SPI_GETAUDIODESCRIPTION
#undef SPI_SETAUDIODESCRIPTION
#undef SPI_GETSCREENSAVESECURE
#undef SPI_SETSCREENSAVESECURE
#undef SPI_GETHUNGAPPTIMEOUT
#undef SPI_SETHUNGAPPTIMEOUT
#undef SPI_GETWAITTOKILLTIMEOUT
#undef SPI_SETWAITTOKILLTIMEOUT
#undef SPI_GETWAITTOKILLSERVICETIMEOUT
#undef SPI_SETWAITTOKILLSERVICETIMEOUT
#undef SPI_GETMOUSEDOCKTHRESHOLD
#undef SPI_SETMOUSEDOCKTHRESHOLD
#undef SPI_GETPENDOCKTHRESHOLD
#undef SPI_SETPENDOCKTHRESHOLD
#undef SPI_GETWINARRANGING
#undef SPI_SETWINARRANGING
#undef SPI_GETMOUSEDRAGOUTTHRESHOLD
#undef SPI_SETMOUSEDRAGOUTTHRESHOLD
#undef SPI_GETPENDRAGOUTTHRESHOLD
#undef SPI_SETPENDRAGOUTTHRESHOLD
#undef SPI_GETMOUSESIDEMOVETHRESHOLD
#undef SPI_SETMOUSESIDEMOVETHRESHOLD
#undef SPI_GETPENSIDEMOVETHRESHOLD
#undef SPI_SETPENSIDEMOVETHRESHOLD
#undef SPI_GETDRAGFROMMAXIMIZE
#undef SPI_SETDRAGFROMMAXIMIZE
#undef SPI_GETSNAPSIZING
#undef SPI_SETSNAPSIZING
#undef SPI_GETDOCKMOVING
#undef SPI_SETDOCKMOVING
#undef SPI_GETTOUCHPREDICTIONPARAMETERS
#undef SPI_SETTOUCHPREDICTIONPARAMETERS
#undef SPI_GETLOGICALDPIOVERRIDE
#undef SPI_SETLOGICALDPIOVERRIDE
#undef SPI_GETMENURECT
#undef SPI_SETMENURECT
#undef SPI_GETTOUCHPADPARAMETERS
#undef SPI_SETTOUCHPADPARAMETERS
#undef SPI_GETACTIVEWINDOWTRACKING
#undef SPI_SETACTIVEWINDOWTRACKING
#undef SPI_GETMENUANIMATION
#undef SPI_SETMENUANIMATION
#undef SPI_GETCOMBOBOXANIMATION
#undef SPI_SETCOMBOBOXANIMATION
#undef SPI_GETLISTBOXSMOOTHSCROLLING
#undef SPI_SETLISTBOXSMOOTHSCROLLING
#undef SPI_GETGRADIENTCAPTIONS
#undef SPI_SETGRADIENTCAPTIONS
#undef SPI_GETKEYBOARDCUES
#undef SPI_SETKEYBOARDCUES
#undef SPI_GETMENUUNDERLINES
#undef SPI_SETMENUUNDERLINES
#undef SPI_GETACTIVEWNDTRKZORDER
#undef SPI_SETACTIVEWNDTRKZORDER
#undef SPI_GETHOTTRACKING
#undef SPI_SETHOTTRACKING
#undef SPI_GETMENUFADE
#undef SPI_SETMENUFADE
#undef SPI_GETSELECTIONFADE
#undef SPI_SETSELECTIONFADE
#undef SPI_GETTOOLTIPANIMATION
#undef SPI_SETTOOLTIPANIMATION
#undef SPI_GETTOOLTIPFADE
#undef SPI_SETTOOLTIPFADE
#undef SPI_GETCURSORSHADOW
#undef SPI_SETCURSORSHADOW
#undef SPI_GETMOUSESONAR
#undef SPI_SETMOUSESONAR
#undef SPI_GETMOUSECLICKLOCK
#undef SPI_SETMOUSECLICKLOCK
#undef SPI_GETMOUSEVANISH
#undef SPI_SETMOUSEVANISH
#undef SPI_GETFLATMENU
#undef SPI_SETFLATMENU
#undef SPI_GETDROPSHADOW
#undef SPI_SETDROPSHADOW
#undef SPI_GETBLOCKSENDINPUTRESETS
#undef SPI_SETBLOCKSENDINPUTRESETS
#undef SPI_GETUIEFFECTS
#undef SPI_SETUIEFFECTS
#undef SPI_GETDISABLEOVERLAPPEDCONTENT
#undef SPI_SETDISABLEOVERLAPPEDCONTENT
#undef SPI_GETCLIENTAREAANIMATION
#undef SPI_SETCLIENTAREAANIMATION
#undef SPI_GETCLEARTYPE
#undef SPI_SETCLEARTYPE
#undef SPI_GETSPEECHRECOGNITION
#undef SPI_SETSPEECHRECOGNITION
#undef SPI_GETCARETBROWSING
#undef SPI_SETCARETBROWSING
#undef SPI_GETTHREADLOCALINPUTSETTINGS
#undef SPI_SETTHREADLOCALINPUTSETTINGS
#undef SPI_GETSYSTEMLANGUAGEBAR
#undef SPI_SETSYSTEMLANGUAGEBAR
#undef SPI_GETFOREGROUNDLOCKTIMEOUT
#undef SPI_SETFOREGROUNDLOCKTIMEOUT
#undef SPI_GETACTIVEWNDTRKTIMEOUT
#undef SPI_SETACTIVEWNDTRKTIMEOUT
#undef SPI_GETFOREGROUNDFLASHCOUNT
#undef SPI_SETFOREGROUNDFLASHCOUNT
#undef SPI_GETCARETWIDTH
#undef SPI_SETCARETWIDTH
#undef SPI_GETMOUSECLICKLOCKTIME
#undef SPI_SETMOUSECLICKLOCKTIME
#undef SPI_GETFONTSMOOTHINGTYPE
#undef SPI_SETFONTSMOOTHINGTYPE
#undef SPI_GETFONTSMOOTHINGCONTRAST
#undef SPI_SETFONTSMOOTHINGCONTRAST
#undef SPI_GETFOCUSBORDERWIDTH
#undef SPI_SETFOCUSBORDERWIDTH
#undef SPI_GETFOCUSBORDERHEIGHT
#undef SPI_SETFOCUSBORDERHEIGHT
#undef SPI_GETFONTSMOOTHINGORIENTATION
#undef SPI_SETFONTSMOOTHINGORIENTATION
#undef SPI_GETMINIMUMHITRADIUS
#undef SPI_SETMINIMUMHITRADIUS
#undef SPI_GETMESSAGEDURATION
#undef SPI_SETMESSAGEDURATION
#undef SPI_GETCONTACTVISUALIZATION
#undef SPI_SETCONTACTVISUALIZATION
#undef SPI_GETGESTUREVISUALIZATION
#undef SPI_SETGESTUREVISUALIZATION
#undef SPI_GETMOUSEWHEELROUTING
#undef SPI_SETMOUSEWHEELROUTING
#undef SPI_GETPENVISUALIZATION
#undef SPI_SETPENVISUALIZATION
#undef SPI_GETPENARBITRATIONTYPE
#undef SPI_SETPENARBITRATIONTYPE
#undef SPI_GETCARETTIMEOUT
#undef SPI_SETCARETTIMEOUT
#undef SPI_GETHANDEDNESS
#undef SPI_SETHANDEDNESS
enum class
    [[clang::flag_enum]]
    SYSTEM_PARAMETERS_INFO_ACTION : int
{
    SPI_GETBEEP = 1,
    SPI_SETBEEP = 2,
    SPI_GETMOUSE = 3,
    SPI_SETMOUSE = 4,
    SPI_GETBORDER = 5,
    SPI_SETBORDER = 6,
    SPI_GETKEYBOARDSPEED = 10,
    SPI_SETKEYBOARDSPEED = 11,
    SPI_LANGDRIVER = 12,
    SPI_ICONHORIZONTALSPACING = 13,
    SPI_GETSCREENSAVETIMEOUT = 14,
    SPI_SETSCREENSAVETIMEOUT = 15,
    SPI_GETSCREENSAVEACTIVE = 16,
    SPI_SETSCREENSAVEACTIVE = 17,
    SPI_GETGRIDGRANULARITY = 18,
    SPI_SETGRIDGRANULARITY = 19,
    SPI_SETDESKWALLPAPER = 20,
    SPI_SETDESKPATTERN = 21,
    SPI_GETKEYBOARDDELAY = 22,
    SPI_SETKEYBOARDDELAY = 23,
    SPI_ICONVERTICALSPACING = 24,
    SPI_GETICONTITLEWRAP = 25,
    SPI_SETICONTITLEWRAP = 26,
    SPI_GETMENUDROPALIGNMENT = 27,
    SPI_SETMENUDROPALIGNMENT = 28,
    SPI_SETDOUBLECLKWIDTH = 29,
    SPI_SETDOUBLECLKHEIGHT = 30,
    SPI_GETICONTITLELOGFONT = 31,
    SPI_SETDOUBLECLICKTIME = 32,
    SPI_SETMOUSEBUTTONSWAP = 33,
    SPI_SETICONTITLELOGFONT = 34,
    SPI_GETFASTTASKSWITCH = 35,
    SPI_SETFASTTASKSWITCH = 36,
    SPI_SETDRAGFULLWINDOWS = 37,
    SPI_GETDRAGFULLWINDOWS = 38,
    SPI_GETNONCLIENTMETRICS = 41,
    SPI_SETNONCLIENTMETRICS = 42,
    SPI_GETMINIMIZEDMETRICS = 43,
    SPI_SETMINIMIZEDMETRICS = 44,
    SPI_GETICONMETRICS = 45,
    SPI_SETICONMETRICS = 46,
    SPI_SETWORKAREA = 47,
    SPI_GETWORKAREA = 48,
    SPI_SETPENWINDOWS = 49,
    SPI_GETHIGHCONTRAST = 66,
    SPI_SETHIGHCONTRAST = 67,
    SPI_GETKEYBOARDPREF = 68,
    SPI_SETKEYBOARDPREF = 69,
    SPI_GETSCREENREADER = 70,
    SPI_SETSCREENREADER = 71,
    SPI_GETANIMATION = 72,
    SPI_SETANIMATION = 73,
    SPI_GETFONTSMOOTHING = 74,
    SPI_SETFONTSMOOTHING = 75,
    SPI_SETDRAGWIDTH = 76,
    SPI_SETDRAGHEIGHT = 77,
    SPI_SETHANDHELD = 78,
    SPI_GETLOWPOWERTIMEOUT = 79,
    SPI_GETPOWEROFFTIMEOUT = 80,
    SPI_SETLOWPOWERTIMEOUT = 81,
    SPI_SETPOWEROFFTIMEOUT = 82,
    SPI_GETLOWPOWERACTIVE = 83,
    SPI_GETPOWEROFFACTIVE = 84,
    SPI_SETLOWPOWERACTIVE = 85,
    SPI_SETPOWEROFFACTIVE = 86,
    SPI_SETCURSORS = 87,
    SPI_SETICONS = 88,
    SPI_GETDEFAULTINPUTLANG = 89,
    SPI_SETDEFAULTINPUTLANG = 90,
    SPI_SETLANGTOGGLE = 91,
    SPI_GETWINDOWSEXTENSION = 92,
    SPI_SETMOUSETRAILS = 93,
    SPI_GETMOUSETRAILS = 94,
    SPI_SETSCREENSAVERRUNNING = 97,
    SPI_SCREENSAVERRUNNING = 97,
    SPI_GETFILTERKEYS = 50,
    SPI_SETFILTERKEYS = 51,
    SPI_GETTOGGLEKEYS = 52,
    SPI_SETTOGGLEKEYS = 53,
    SPI_GETMOUSEKEYS = 54,
    SPI_SETMOUSEKEYS = 55,
    SPI_GETSHOWSOUNDS = 56,
    SPI_SETSHOWSOUNDS = 57,
    SPI_GETSTICKYKEYS = 58,
    SPI_SETSTICKYKEYS = 59,
    SPI_GETACCESSTIMEOUT = 60,
    SPI_SETACCESSTIMEOUT = 61,
    SPI_GETSERIALKEYS = 62,
    SPI_SETSERIALKEYS = 63,
    SPI_GETSOUNDSENTRY = 64,
    SPI_SETSOUNDSENTRY = 65,
    SPI_GETSNAPTODEFBUTTON = 95,
    SPI_SETSNAPTODEFBUTTON = 96,
    SPI_GETMOUSEHOVERWIDTH = 98,
    SPI_SETMOUSEHOVERWIDTH = 99,
    SPI_GETMOUSEHOVERHEIGHT = 100,
    SPI_SETMOUSEHOVERHEIGHT = 101,
    SPI_GETMOUSEHOVERTIME = 102,
    SPI_SETMOUSEHOVERTIME = 103,
    SPI_GETWHEELSCROLLLINES = 104,
    SPI_SETWHEELSCROLLLINES = 105,
    SPI_GETMENUSHOWDELAY = 106,
    SPI_SETMENUSHOWDELAY = 107,
    SPI_GETWHEELSCROLLCHARS = 108,
    SPI_SETWHEELSCROLLCHARS = 109,
    SPI_GETSHOWIMEUI = 110,
    SPI_SETSHOWIMEUI = 111,
    SPI_GETMOUSESPEED = 112,
    SPI_SETMOUSESPEED = 113,
    SPI_GETSCREENSAVERRUNNING = 114,
    SPI_GETDESKWALLPAPER = 115,
    SPI_GETAUDIODESCRIPTION = 116,
    SPI_SETAUDIODESCRIPTION = 117,
    SPI_GETSCREENSAVESECURE = 118,
    SPI_SETSCREENSAVESECURE = 119,
    SPI_GETHUNGAPPTIMEOUT = 120,
    SPI_SETHUNGAPPTIMEOUT = 121,
    SPI_GETWAITTOKILLTIMEOUT = 122,
    SPI_SETWAITTOKILLTIMEOUT = 123,
    SPI_GETWAITTOKILLSERVICETIMEOUT = 124,
    SPI_SETWAITTOKILLSERVICETIMEOUT = 125,
    SPI_GETMOUSEDOCKTHRESHOLD = 126,
    SPI_SETMOUSEDOCKTHRESHOLD = 127,
    SPI_GETPENDOCKTHRESHOLD = 128,
    SPI_SETPENDOCKTHRESHOLD = 129,
    SPI_GETWINARRANGING = 130,
    SPI_SETWINARRANGING = 131,
    SPI_GETMOUSEDRAGOUTTHRESHOLD = 132,
    SPI_SETMOUSEDRAGOUTTHRESHOLD = 133,
    SPI_GETPENDRAGOUTTHRESHOLD = 134,
    SPI_SETPENDRAGOUTTHRESHOLD = 135,
    SPI_GETMOUSESIDEMOVETHRESHOLD = 136,
    SPI_SETMOUSESIDEMOVETHRESHOLD = 137,
    SPI_GETPENSIDEMOVETHRESHOLD = 138,
    SPI_SETPENSIDEMOVETHRESHOLD = 139,
    SPI_GETDRAGFROMMAXIMIZE = 140,
    SPI_SETDRAGFROMMAXIMIZE = 141,
    SPI_GETSNAPSIZING = 142,
    SPI_SETSNAPSIZING = 143,
    SPI_GETDOCKMOVING = 144,
    SPI_SETDOCKMOVING = 145,
    SPI_GETTOUCHPREDICTIONPARAMETERS = 156,
    SPI_SETTOUCHPREDICTIONPARAMETERS = 157,
    SPI_GETLOGICALDPIOVERRIDE = 158,
    SPI_SETLOGICALDPIOVERRIDE = 159,
    SPI_GETMENURECT = 162,
    SPI_SETMENURECT = 163,
    SPI_GETTOUCHPADPARAMETERS = 174,
    SPI_SETTOUCHPADPARAMETERS = 175,
    SPI_GETACTIVEWINDOWTRACKING = 4096,
    SPI_SETACTIVEWINDOWTRACKING = 4097,
    SPI_GETMENUANIMATION = 4098,
    SPI_SETMENUANIMATION = 4099,
    SPI_GETCOMBOBOXANIMATION = 4100,
    SPI_SETCOMBOBOXANIMATION = 4101,
    SPI_GETLISTBOXSMOOTHSCROLLING = 4102,
    SPI_SETLISTBOXSMOOTHSCROLLING = 4103,
    SPI_GETGRADIENTCAPTIONS = 4104,
    SPI_SETGRADIENTCAPTIONS = 4105,
    SPI_GETKEYBOARDCUES = 4106,
    SPI_SETKEYBOARDCUES = 4107,
    SPI_GETMENUUNDERLINES = 4106,
    SPI_SETMENUUNDERLINES = 4107,
    SPI_GETACTIVEWNDTRKZORDER = 4108,
    SPI_SETACTIVEWNDTRKZORDER = 4109,
    SPI_GETHOTTRACKING = 4110,
    SPI_SETHOTTRACKING = 4111,
    SPI_GETMENUFADE = 4114,
    SPI_SETMENUFADE = 4115,
    SPI_GETSELECTIONFADE = 4116,
    SPI_SETSELECTIONFADE = 4117,
    SPI_GETTOOLTIPANIMATION = 4118,
    SPI_SETTOOLTIPANIMATION = 4119,
    SPI_GETTOOLTIPFADE = 4120,
    SPI_SETTOOLTIPFADE = 4121,
    SPI_GETCURSORSHADOW = 4122,
    SPI_SETCURSORSHADOW = 4123,
    SPI_GETMOUSESONAR = 4124,
    SPI_SETMOUSESONAR = 4125,
    SPI_GETMOUSECLICKLOCK = 4126,
    SPI_SETMOUSECLICKLOCK = 4127,
    SPI_GETMOUSEVANISH = 4128,
    SPI_SETMOUSEVANISH = 4129,
    SPI_GETFLATMENU = 4130,
    SPI_SETFLATMENU = 4131,
    SPI_GETDROPSHADOW = 4132,
    SPI_SETDROPSHADOW = 4133,
    SPI_GETBLOCKSENDINPUTRESETS = 4134,
    SPI_SETBLOCKSENDINPUTRESETS = 4135,
    SPI_GETUIEFFECTS = 4158,
    SPI_SETUIEFFECTS = 4159,
    SPI_GETDISABLEOVERLAPPEDCONTENT = 4160,
    SPI_SETDISABLEOVERLAPPEDCONTENT = 4161,
    SPI_GETCLIENTAREAANIMATION = 4162,
    SPI_SETCLIENTAREAANIMATION = 4163,
    SPI_GETCLEARTYPE = 4168,
    SPI_SETCLEARTYPE = 4169,
    SPI_GETSPEECHRECOGNITION = 4170,
    SPI_SETSPEECHRECOGNITION = 4171,
    SPI_GETCARETBROWSING = 4172,
    SPI_SETCARETBROWSING = 4173,
    SPI_GETTHREADLOCALINPUTSETTINGS = 4174,
    SPI_SETTHREADLOCALINPUTSETTINGS = 4175,
    SPI_GETSYSTEMLANGUAGEBAR = 4176,
    SPI_SETSYSTEMLANGUAGEBAR = 4177,
    SPI_GETFOREGROUNDLOCKTIMEOUT = 8192,
    SPI_SETFOREGROUNDLOCKTIMEOUT = 8193,
    SPI_GETACTIVEWNDTRKTIMEOUT = 8194,
    SPI_SETACTIVEWNDTRKTIMEOUT = 8195,
    SPI_GETFOREGROUNDFLASHCOUNT = 8196,
    SPI_SETFOREGROUNDFLASHCOUNT = 8197,
    SPI_GETCARETWIDTH = 8198,
    SPI_SETCARETWIDTH = 8199,
    SPI_GETMOUSECLICKLOCKTIME = 8200,
    SPI_SETMOUSECLICKLOCKTIME = 8201,
    SPI_GETFONTSMOOTHINGTYPE = 8202,
    SPI_SETFONTSMOOTHINGTYPE = 8203,
    SPI_GETFONTSMOOTHINGCONTRAST = 8204,
    SPI_SETFONTSMOOTHINGCONTRAST = 8205,
    SPI_GETFOCUSBORDERWIDTH = 8206,
    SPI_SETFOCUSBORDERWIDTH = 8207,
    SPI_GETFOCUSBORDERHEIGHT = 8208,
    SPI_SETFOCUSBORDERHEIGHT = 8209,
    SPI_GETFONTSMOOTHINGORIENTATION = 8210,
    SPI_SETFONTSMOOTHINGORIENTATION = 8211,
    SPI_GETMINIMUMHITRADIUS = 8212,
    SPI_SETMINIMUMHITRADIUS = 8213,
    SPI_GETMESSAGEDURATION = 8214,
    SPI_SETMESSAGEDURATION = 8215,
    SPI_GETCONTACTVISUALIZATION = 8216,
    SPI_SETCONTACTVISUALIZATION = 8217,
    SPI_GETGESTUREVISUALIZATION = 8218,
    SPI_SETGESTUREVISUALIZATION = 8219,
    SPI_GETMOUSEWHEELROUTING = 8220,
    SPI_SETMOUSEWHEELROUTING = 8221,
    SPI_GETPENVISUALIZATION = 8222,
    SPI_SETPENVISUALIZATION = 8223,
    SPI_GETPENARBITRATIONTYPE = 8224,
    SPI_SETPENARBITRATIONTYPE = 8225,
    SPI_GETCARETTIMEOUT = 8226,
    SPI_SETCARETTIMEOUT = 8227,
    SPI_GETHANDEDNESS = 8228,
    SPI_SETHANDEDNESS = 8229,
};
#pragma pop_macro("SPI_SETHANDEDNESS")
#pragma pop_macro("SPI_GETHANDEDNESS")
#pragma pop_macro("SPI_SETCARETTIMEOUT")
#pragma pop_macro("SPI_GETCARETTIMEOUT")
#pragma pop_macro("SPI_SETPENARBITRATIONTYPE")
#pragma pop_macro("SPI_GETPENARBITRATIONTYPE")
#pragma pop_macro("SPI_SETPENVISUALIZATION")
#pragma pop_macro("SPI_GETPENVISUALIZATION")
#pragma pop_macro("SPI_SETMOUSEWHEELROUTING")
#pragma pop_macro("SPI_GETMOUSEWHEELROUTING")
#pragma pop_macro("SPI_SETGESTUREVISUALIZATION")
#pragma pop_macro("SPI_GETGESTUREVISUALIZATION")
#pragma pop_macro("SPI_SETCONTACTVISUALIZATION")
#pragma pop_macro("SPI_GETCONTACTVISUALIZATION")
#pragma pop_macro("SPI_SETMESSAGEDURATION")
#pragma pop_macro("SPI_GETMESSAGEDURATION")
#pragma pop_macro("SPI_SETMINIMUMHITRADIUS")
#pragma pop_macro("SPI_GETMINIMUMHITRADIUS")
#pragma pop_macro("SPI_SETFONTSMOOTHINGORIENTATION")
#pragma pop_macro("SPI_GETFONTSMOOTHINGORIENTATION")
#pragma pop_macro("SPI_SETFOCUSBORDERHEIGHT")
#pragma pop_macro("SPI_GETFOCUSBORDERHEIGHT")
#pragma pop_macro("SPI_SETFOCUSBORDERWIDTH")
#pragma pop_macro("SPI_GETFOCUSBORDERWIDTH")
#pragma pop_macro("SPI_SETFONTSMOOTHINGCONTRAST")
#pragma pop_macro("SPI_GETFONTSMOOTHINGCONTRAST")
#pragma pop_macro("SPI_SETFONTSMOOTHINGTYPE")
#pragma pop_macro("SPI_GETFONTSMOOTHINGTYPE")
#pragma pop_macro("SPI_SETMOUSECLICKLOCKTIME")
#pragma pop_macro("SPI_GETMOUSECLICKLOCKTIME")
#pragma pop_macro("SPI_SETCARETWIDTH")
#pragma pop_macro("SPI_GETCARETWIDTH")
#pragma pop_macro("SPI_SETFOREGROUNDFLASHCOUNT")
#pragma pop_macro("SPI_GETFOREGROUNDFLASHCOUNT")
#pragma pop_macro("SPI_SETACTIVEWNDTRKTIMEOUT")
#pragma pop_macro("SPI_GETACTIVEWNDTRKTIMEOUT")
#pragma pop_macro("SPI_SETFOREGROUNDLOCKTIMEOUT")
#pragma pop_macro("SPI_GETFOREGROUNDLOCKTIMEOUT")
#pragma pop_macro("SPI_SETSYSTEMLANGUAGEBAR")
#pragma pop_macro("SPI_GETSYSTEMLANGUAGEBAR")
#pragma pop_macro("SPI_SETTHREADLOCALINPUTSETTINGS")
#pragma pop_macro("SPI_GETTHREADLOCALINPUTSETTINGS")
#pragma pop_macro("SPI_SETCARETBROWSING")
#pragma pop_macro("SPI_GETCARETBROWSING")
#pragma pop_macro("SPI_SETSPEECHRECOGNITION")
#pragma pop_macro("SPI_GETSPEECHRECOGNITION")
#pragma pop_macro("SPI_SETCLEARTYPE")
#pragma pop_macro("SPI_GETCLEARTYPE")
#pragma pop_macro("SPI_SETCLIENTAREAANIMATION")
#pragma pop_macro("SPI_GETCLIENTAREAANIMATION")
#pragma pop_macro("SPI_SETDISABLEOVERLAPPEDCONTENT")
#pragma pop_macro("SPI_GETDISABLEOVERLAPPEDCONTENT")
#pragma pop_macro("SPI_SETUIEFFECTS")
#pragma pop_macro("SPI_GETUIEFFECTS")
#pragma pop_macro("SPI_SETBLOCKSENDINPUTRESETS")
#pragma pop_macro("SPI_GETBLOCKSENDINPUTRESETS")
#pragma pop_macro("SPI_SETDROPSHADOW")
#pragma pop_macro("SPI_GETDROPSHADOW")
#pragma pop_macro("SPI_SETFLATMENU")
#pragma pop_macro("SPI_GETFLATMENU")
#pragma pop_macro("SPI_SETMOUSEVANISH")
#pragma pop_macro("SPI_GETMOUSEVANISH")
#pragma pop_macro("SPI_SETMOUSECLICKLOCK")
#pragma pop_macro("SPI_GETMOUSECLICKLOCK")
#pragma pop_macro("SPI_SETMOUSESONAR")
#pragma pop_macro("SPI_GETMOUSESONAR")
#pragma pop_macro("SPI_SETCURSORSHADOW")
#pragma pop_macro("SPI_GETCURSORSHADOW")
#pragma pop_macro("SPI_SETTOOLTIPFADE")
#pragma pop_macro("SPI_GETTOOLTIPFADE")
#pragma pop_macro("SPI_SETTOOLTIPANIMATION")
#pragma pop_macro("SPI_GETTOOLTIPANIMATION")
#pragma pop_macro("SPI_SETSELECTIONFADE")
#pragma pop_macro("SPI_GETSELECTIONFADE")
#pragma pop_macro("SPI_SETMENUFADE")
#pragma pop_macro("SPI_GETMENUFADE")
#pragma pop_macro("SPI_SETHOTTRACKING")
#pragma pop_macro("SPI_GETHOTTRACKING")
#pragma pop_macro("SPI_SETACTIVEWNDTRKZORDER")
#pragma pop_macro("SPI_GETACTIVEWNDTRKZORDER")
#pragma pop_macro("SPI_SETMENUUNDERLINES")
#pragma pop_macro("SPI_GETMENUUNDERLINES")
#pragma pop_macro("SPI_SETKEYBOARDCUES")
#pragma pop_macro("SPI_GETKEYBOARDCUES")
#pragma pop_macro("SPI_SETGRADIENTCAPTIONS")
#pragma pop_macro("SPI_GETGRADIENTCAPTIONS")
#pragma pop_macro("SPI_SETLISTBOXSMOOTHSCROLLING")
#pragma pop_macro("SPI_GETLISTBOXSMOOTHSCROLLING")
#pragma pop_macro("SPI_SETCOMBOBOXANIMATION")
#pragma pop_macro("SPI_GETCOMBOBOXANIMATION")
#pragma pop_macro("SPI_SETMENUANIMATION")
#pragma pop_macro("SPI_GETMENUANIMATION")
#pragma pop_macro("SPI_SETACTIVEWINDOWTRACKING")
#pragma pop_macro("SPI_GETACTIVEWINDOWTRACKING")
#pragma pop_macro("SPI_SETTOUCHPADPARAMETERS")
#pragma pop_macro("SPI_GETTOUCHPADPARAMETERS")
#pragma pop_macro("SPI_SETMENURECT")
#pragma pop_macro("SPI_GETMENURECT")
#pragma pop_macro("SPI_SETLOGICALDPIOVERRIDE")
#pragma pop_macro("SPI_GETLOGICALDPIOVERRIDE")
#pragma pop_macro("SPI_SETTOUCHPREDICTIONPARAMETERS")
#pragma pop_macro("SPI_GETTOUCHPREDICTIONPARAMETERS")
#pragma pop_macro("SPI_SETDOCKMOVING")
#pragma pop_macro("SPI_GETDOCKMOVING")
#pragma pop_macro("SPI_SETSNAPSIZING")
#pragma pop_macro("SPI_GETSNAPSIZING")
#pragma pop_macro("SPI_SETDRAGFROMMAXIMIZE")
#pragma pop_macro("SPI_GETDRAGFROMMAXIMIZE")
#pragma pop_macro("SPI_SETPENSIDEMOVETHRESHOLD")
#pragma pop_macro("SPI_GETPENSIDEMOVETHRESHOLD")
#pragma pop_macro("SPI_SETMOUSESIDEMOVETHRESHOLD")
#pragma pop_macro("SPI_GETMOUSESIDEMOVETHRESHOLD")
#pragma pop_macro("SPI_SETPENDRAGOUTTHRESHOLD")
#pragma pop_macro("SPI_GETPENDRAGOUTTHRESHOLD")
#pragma pop_macro("SPI_SETMOUSEDRAGOUTTHRESHOLD")
#pragma pop_macro("SPI_GETMOUSEDRAGOUTTHRESHOLD")
#pragma pop_macro("SPI_SETWINARRANGING")
#pragma pop_macro("SPI_GETWINARRANGING")
#pragma pop_macro("SPI_SETPENDOCKTHRESHOLD")
#pragma pop_macro("SPI_GETPENDOCKTHRESHOLD")
#pragma pop_macro("SPI_SETMOUSEDOCKTHRESHOLD")
#pragma pop_macro("SPI_GETMOUSEDOCKTHRESHOLD")
#pragma pop_macro("SPI_SETWAITTOKILLSERVICETIMEOUT")
#pragma pop_macro("SPI_GETWAITTOKILLSERVICETIMEOUT")
#pragma pop_macro("SPI_SETWAITTOKILLTIMEOUT")
#pragma pop_macro("SPI_GETWAITTOKILLTIMEOUT")
#pragma pop_macro("SPI_SETHUNGAPPTIMEOUT")
#pragma pop_macro("SPI_GETHUNGAPPTIMEOUT")
#pragma pop_macro("SPI_SETSCREENSAVESECURE")
#pragma pop_macro("SPI_GETSCREENSAVESECURE")
#pragma pop_macro("SPI_SETAUDIODESCRIPTION")
#pragma pop_macro("SPI_GETAUDIODESCRIPTION")
#pragma pop_macro("SPI_GETDESKWALLPAPER")
#pragma pop_macro("SPI_GETSCREENSAVERRUNNING")
#pragma pop_macro("SPI_SETMOUSESPEED")
#pragma pop_macro("SPI_GETMOUSESPEED")
#pragma pop_macro("SPI_SETSHOWIMEUI")
#pragma pop_macro("SPI_GETSHOWIMEUI")
#pragma pop_macro("SPI_SETWHEELSCROLLCHARS")
#pragma pop_macro("SPI_GETWHEELSCROLLCHARS")
#pragma pop_macro("SPI_SETMENUSHOWDELAY")
#pragma pop_macro("SPI_GETMENUSHOWDELAY")
#pragma pop_macro("SPI_SETWHEELSCROLLLINES")
#pragma pop_macro("SPI_GETWHEELSCROLLLINES")
#pragma pop_macro("SPI_SETMOUSEHOVERTIME")
#pragma pop_macro("SPI_GETMOUSEHOVERTIME")
#pragma pop_macro("SPI_SETMOUSEHOVERHEIGHT")
#pragma pop_macro("SPI_GETMOUSEHOVERHEIGHT")
#pragma pop_macro("SPI_SETMOUSEHOVERWIDTH")
#pragma pop_macro("SPI_GETMOUSEHOVERWIDTH")
#pragma pop_macro("SPI_SETSNAPTODEFBUTTON")
#pragma pop_macro("SPI_GETSNAPTODEFBUTTON")
#pragma pop_macro("SPI_SETSOUNDSENTRY")
#pragma pop_macro("SPI_GETSOUNDSENTRY")
#pragma pop_macro("SPI_SETSERIALKEYS")
#pragma pop_macro("SPI_GETSERIALKEYS")
#pragma pop_macro("SPI_SETACCESSTIMEOUT")
#pragma pop_macro("SPI_GETACCESSTIMEOUT")
#pragma pop_macro("SPI_SETSTICKYKEYS")
#pragma pop_macro("SPI_GETSTICKYKEYS")
#pragma pop_macro("SPI_SETSHOWSOUNDS")
#pragma pop_macro("SPI_GETSHOWSOUNDS")
#pragma pop_macro("SPI_SETMOUSEKEYS")
#pragma pop_macro("SPI_GETMOUSEKEYS")
#pragma pop_macro("SPI_SETTOGGLEKEYS")
#pragma pop_macro("SPI_GETTOGGLEKEYS")
#pragma pop_macro("SPI_SETFILTERKEYS")
#pragma pop_macro("SPI_GETFILTERKEYS")
#pragma pop_macro("SPI_SCREENSAVERRUNNING")
#pragma pop_macro("SPI_SETSCREENSAVERRUNNING")
#pragma pop_macro("SPI_GETMOUSETRAILS")
#pragma pop_macro("SPI_SETMOUSETRAILS")
#pragma pop_macro("SPI_GETWINDOWSEXTENSION")
#pragma pop_macro("SPI_SETLANGTOGGLE")
#pragma pop_macro("SPI_SETDEFAULTINPUTLANG")
#pragma pop_macro("SPI_GETDEFAULTINPUTLANG")
#pragma pop_macro("SPI_SETICONS")
#pragma pop_macro("SPI_SETCURSORS")
#pragma pop_macro("SPI_SETPOWEROFFACTIVE")
#pragma pop_macro("SPI_SETLOWPOWERACTIVE")
#pragma pop_macro("SPI_GETPOWEROFFACTIVE")
#pragma pop_macro("SPI_GETLOWPOWERACTIVE")
#pragma pop_macro("SPI_SETPOWEROFFTIMEOUT")
#pragma pop_macro("SPI_SETLOWPOWERTIMEOUT")
#pragma pop_macro("SPI_GETPOWEROFFTIMEOUT")
#pragma pop_macro("SPI_GETLOWPOWERTIMEOUT")
#pragma pop_macro("SPI_SETHANDHELD")
#pragma pop_macro("SPI_SETDRAGHEIGHT")
#pragma pop_macro("SPI_SETDRAGWIDTH")
#pragma pop_macro("SPI_SETFONTSMOOTHING")
#pragma pop_macro("SPI_GETFONTSMOOTHING")
#pragma pop_macro("SPI_SETANIMATION")
#pragma pop_macro("SPI_GETANIMATION")
#pragma pop_macro("SPI_SETSCREENREADER")
#pragma pop_macro("SPI_GETSCREENREADER")
#pragma pop_macro("SPI_SETKEYBOARDPREF")
#pragma pop_macro("SPI_GETKEYBOARDPREF")
#pragma pop_macro("SPI_SETHIGHCONTRAST")
#pragma pop_macro("SPI_GETHIGHCONTRAST")
#pragma pop_macro("SPI_SETPENWINDOWS")
#pragma pop_macro("SPI_GETWORKAREA")
#pragma pop_macro("SPI_SETWORKAREA")
#pragma pop_macro("SPI_SETICONMETRICS")
#pragma pop_macro("SPI_GETICONMETRICS")
#pragma pop_macro("SPI_SETMINIMIZEDMETRICS")
#pragma pop_macro("SPI_GETMINIMIZEDMETRICS")
#pragma pop_macro("SPI_SETNONCLIENTMETRICS")
#pragma pop_macro("SPI_GETNONCLIENTMETRICS")
#pragma pop_macro("SPI_GETDRAGFULLWINDOWS")
#pragma pop_macro("SPI_SETDRAGFULLWINDOWS")
#pragma pop_macro("SPI_SETFASTTASKSWITCH")
#pragma pop_macro("SPI_GETFASTTASKSWITCH")
#pragma pop_macro("SPI_SETICONTITLELOGFONT")
#pragma pop_macro("SPI_SETMOUSEBUTTONSWAP")
#pragma pop_macro("SPI_SETDOUBLECLICKTIME")
#pragma pop_macro("SPI_GETICONTITLELOGFONT")
#pragma pop_macro("SPI_SETDOUBLECLKHEIGHT")
#pragma pop_macro("SPI_SETDOUBLECLKWIDTH")
#pragma pop_macro("SPI_SETMENUDROPALIGNMENT")
#pragma pop_macro("SPI_GETMENUDROPALIGNMENT")
#pragma pop_macro("SPI_SETICONTITLEWRAP")
#pragma pop_macro("SPI_GETICONTITLEWRAP")
#pragma pop_macro("SPI_ICONVERTICALSPACING")
#pragma pop_macro("SPI_SETKEYBOARDDELAY")
#pragma pop_macro("SPI_GETKEYBOARDDELAY")
#pragma pop_macro("SPI_SETDESKPATTERN")
#pragma pop_macro("SPI_SETDESKWALLPAPER")
#pragma pop_macro("SPI_SETGRIDGRANULARITY")
#pragma pop_macro("SPI_GETGRIDGRANULARITY")
#pragma pop_macro("SPI_SETSCREENSAVEACTIVE")
#pragma pop_macro("SPI_GETSCREENSAVEACTIVE")
#pragma pop_macro("SPI_SETSCREENSAVETIMEOUT")
#pragma pop_macro("SPI_GETSCREENSAVETIMEOUT")
#pragma pop_macro("SPI_ICONHORIZONTALSPACING")
#pragma pop_macro("SPI_LANGDRIVER")
#pragma pop_macro("SPI_SETKEYBOARDSPEED")
#pragma pop_macro("SPI_GETKEYBOARDSPEED")
#pragma pop_macro("SPI_SETBORDER")
#pragma pop_macro("SPI_GETBORDER")
#pragma pop_macro("SPI_SETMOUSE")
#pragma pop_macro("SPI_GETMOUSE")
#pragma pop_macro("SPI_SETBEEP")
#pragma pop_macro("SPI_GETBEEP")

#pragma push_macro("SSF_SOUNDSENTRYON")
#pragma push_macro("SSF_AVAILABLE")
#pragma push_macro("SSF_INDICATOR")
#undef SSF_SOUNDSENTRYON
#undef SSF_AVAILABLE
#undef SSF_INDICATOR
enum class
    [[clang::flag_enum]]
    SOUNDSENTRY_FLAGS : int
{
    SSF_SOUNDSENTRYON = 1,
    SSF_AVAILABLE = 2,
    SSF_INDICATOR = 4,
};
#pragma pop_macro("SSF_INDICATOR")
#pragma pop_macro("SSF_AVAILABLE")
#pragma pop_macro("SSF_SOUNDSENTRYON")

#pragma push_macro("TOUCH_FEEDBACK_DEFAULT")
#pragma push_macro("TOUCH_FEEDBACK_INDIRECT")
#pragma push_macro("TOUCH_FEEDBACK_NONE")
#undef TOUCH_FEEDBACK_DEFAULT
#undef TOUCH_FEEDBACK_INDIRECT
#undef TOUCH_FEEDBACK_NONE
enum class
    TOUCH_FEEDBACK_MODE : int
{
    TOUCH_FEEDBACK_DEFAULT = 1,
    TOUCH_FEEDBACK_INDIRECT = 2,
    TOUCH_FEEDBACK_NONE = 3,
};
#pragma pop_macro("TOUCH_FEEDBACK_NONE")
#pragma pop_macro("TOUCH_FEEDBACK_INDIRECT")
#pragma pop_macro("TOUCH_FEEDBACK_DEFAULT")

#pragma push_macro("TOUCHEVENTF_MOVE")
#pragma push_macro("TOUCHEVENTF_DOWN")
#pragma push_macro("TOUCHEVENTF_UP")
#pragma push_macro("TOUCHEVENTF_INRANGE")
#pragma push_macro("TOUCHEVENTF_PRIMARY")
#pragma push_macro("TOUCHEVENTF_NOCOALESCE")
#pragma push_macro("TOUCHEVENTF_PEN")
#pragma push_macro("TOUCHEVENTF_PALM")
#undef TOUCHEVENTF_MOVE
#undef TOUCHEVENTF_DOWN
#undef TOUCHEVENTF_UP
#undef TOUCHEVENTF_INRANGE
#undef TOUCHEVENTF_PRIMARY
#undef TOUCHEVENTF_NOCOALESCE
#undef TOUCHEVENTF_PEN
#undef TOUCHEVENTF_PALM
enum class
    [[clang::flag_enum]]
    TOUCHEVENTF_FLAGS : int
{
    TOUCHEVENTF_MOVE = 1,
    TOUCHEVENTF_DOWN = 2,
    TOUCHEVENTF_UP = 4,
    TOUCHEVENTF_INRANGE = 8,
    TOUCHEVENTF_PRIMARY = 16,
    TOUCHEVENTF_NOCOALESCE = 32,
    TOUCHEVENTF_PEN = 64,
    TOUCHEVENTF_PALM = 128,
};
#pragma pop_macro("TOUCHEVENTF_PALM")
#pragma pop_macro("TOUCHEVENTF_PEN")
#pragma pop_macro("TOUCHEVENTF_NOCOALESCE")
#pragma pop_macro("TOUCHEVENTF_PRIMARY")
#pragma pop_macro("TOUCHEVENTF_INRANGE")
#pragma pop_macro("TOUCHEVENTF_UP")
#pragma pop_macro("TOUCHEVENTF_DOWN")
#pragma pop_macro("TOUCHEVENTF_MOVE")

#pragma push_macro("TOUCHINPUTMASKF_TIMEFROMSYSTEM")
#pragma push_macro("TOUCHINPUTMASKF_EXTRAINFO")
#pragma push_macro("TOUCHINPUTMASKF_CONTACTAREA")
#undef TOUCHINPUTMASKF_TIMEFROMSYSTEM
#undef TOUCHINPUTMASKF_EXTRAINFO
#undef TOUCHINPUTMASKF_CONTACTAREA
enum class
    [[clang::flag_enum]]
    TOUCHINPUTMASKF_MASK : int
{
    TOUCHINPUTMASKF_TIMEFROMSYSTEM = 1,
    TOUCHINPUTMASKF_EXTRAINFO = 2,
    TOUCHINPUTMASKF_CONTACTAREA = 4,
};
#pragma pop_macro("TOUCHINPUTMASKF_CONTACTAREA")
#pragma pop_macro("TOUCHINPUTMASKF_EXTRAINFO")
#pragma pop_macro("TOUCHINPUTMASKF_TIMEFROMSYSTEM")

#pragma push_macro("TPM_LEFTBUTTON")
#pragma push_macro("TPM_RIGHTBUTTON")
#pragma push_macro("TPM_LEFTALIGN")
#pragma push_macro("TPM_CENTERALIGN")
#pragma push_macro("TPM_RIGHTALIGN")
#pragma push_macro("TPM_TOPALIGN")
#pragma push_macro("TPM_VCENTERALIGN")
#pragma push_macro("TPM_BOTTOMALIGN")
#pragma push_macro("TPM_HORIZONTAL")
#pragma push_macro("TPM_VERTICAL")
#pragma push_macro("TPM_NONOTIFY")
#pragma push_macro("TPM_RETURNCMD")
#pragma push_macro("TPM_RECURSE")
#pragma push_macro("TPM_HORPOSANIMATION")
#pragma push_macro("TPM_HORNEGANIMATION")
#pragma push_macro("TPM_VERPOSANIMATION")
#pragma push_macro("TPM_VERNEGANIMATION")
#pragma push_macro("TPM_NOANIMATION")
#pragma push_macro("TPM_LAYOUTRTL")
#pragma push_macro("TPM_WORKAREA")
#undef TPM_LEFTBUTTON
#undef TPM_RIGHTBUTTON
#undef TPM_LEFTALIGN
#undef TPM_CENTERALIGN
#undef TPM_RIGHTALIGN
#undef TPM_TOPALIGN
#undef TPM_VCENTERALIGN
#undef TPM_BOTTOMALIGN
#undef TPM_HORIZONTAL
#undef TPM_VERTICAL
#undef TPM_NONOTIFY
#undef TPM_RETURNCMD
#undef TPM_RECURSE
#undef TPM_HORPOSANIMATION
#undef TPM_HORNEGANIMATION
#undef TPM_VERPOSANIMATION
#undef TPM_VERNEGANIMATION
#undef TPM_NOANIMATION
#undef TPM_LAYOUTRTL
#undef TPM_WORKAREA
enum class
    [[clang::flag_enum]]
    TRACK_POPUP_MENU_FLAGS : int
{
    TPM_LEFTBUTTON = 0,
    TPM_RIGHTBUTTON = 2,
    TPM_LEFTALIGN = 0,
    TPM_CENTERALIGN = 4,
    TPM_RIGHTALIGN = 8,
    TPM_TOPALIGN = 0,
    TPM_VCENTERALIGN = 16,
    TPM_BOTTOMALIGN = 32,
    TPM_HORIZONTAL = 0,
    TPM_VERTICAL = 64,
    TPM_NONOTIFY = 128,
    TPM_RETURNCMD = 256,
    TPM_RECURSE = 1,
    TPM_HORPOSANIMATION = 1024,
    TPM_HORNEGANIMATION = 2048,
    TPM_VERPOSANIMATION = 4096,
    TPM_VERNEGANIMATION = 8192,
    TPM_NOANIMATION = 16384,
    TPM_LAYOUTRTL = 32768,
    TPM_WORKAREA = 65536,
};
#pragma pop_macro("TPM_WORKAREA")
#pragma pop_macro("TPM_LAYOUTRTL")
#pragma pop_macro("TPM_NOANIMATION")
#pragma pop_macro("TPM_VERNEGANIMATION")
#pragma pop_macro("TPM_VERPOSANIMATION")
#pragma pop_macro("TPM_HORNEGANIMATION")
#pragma pop_macro("TPM_HORPOSANIMATION")
#pragma pop_macro("TPM_RECURSE")
#pragma pop_macro("TPM_RETURNCMD")
#pragma pop_macro("TPM_NONOTIFY")
#pragma pop_macro("TPM_VERTICAL")
#pragma pop_macro("TPM_HORIZONTAL")
#pragma pop_macro("TPM_BOTTOMALIGN")
#pragma pop_macro("TPM_VCENTERALIGN")
#pragma pop_macro("TPM_TOPALIGN")
#pragma pop_macro("TPM_RIGHTALIGN")
#pragma pop_macro("TPM_CENTERALIGN")
#pragma pop_macro("TPM_LEFTALIGN")
#pragma pop_macro("TPM_RIGHTBUTTON")
#pragma pop_macro("TPM_LEFTBUTTON")

#pragma push_macro("WS_EX_DLGMODALFRAME")
#pragma push_macro("WS_EX_NOPARENTNOTIFY")
#pragma push_macro("WS_EX_TOPMOST")
#pragma push_macro("WS_EX_ACCEPTFILES")
#pragma push_macro("WS_EX_TRANSPARENT")
#pragma push_macro("WS_EX_MDICHILD")
#pragma push_macro("WS_EX_TOOLWINDOW")
#pragma push_macro("WS_EX_WINDOWEDGE")
#pragma push_macro("WS_EX_CLIENTEDGE")
#pragma push_macro("WS_EX_CONTEXTHELP")
#pragma push_macro("WS_EX_RIGHT")
#pragma push_macro("WS_EX_LEFT")
#pragma push_macro("WS_EX_RTLREADING")
#pragma push_macro("WS_EX_LTRREADING")
#pragma push_macro("WS_EX_LEFTSCROLLBAR")
#pragma push_macro("WS_EX_RIGHTSCROLLBAR")
#pragma push_macro("WS_EX_CONTROLPARENT")
#pragma push_macro("WS_EX_STATICEDGE")
#pragma push_macro("WS_EX_APPWINDOW")
#pragma push_macro("WS_EX_OVERLAPPEDWINDOW")
#pragma push_macro("WS_EX_PALETTEWINDOW")
#pragma push_macro("WS_EX_LAYERED")
#pragma push_macro("WS_EX_NOINHERITLAYOUT")
#pragma push_macro("WS_EX_NOREDIRECTIONBITMAP")
#pragma push_macro("WS_EX_LAYOUTRTL")
#pragma push_macro("WS_EX_COMPOSITED")
#pragma push_macro("WS_EX_NOACTIVATE")
#undef WS_EX_DLGMODALFRAME
#undef WS_EX_NOPARENTNOTIFY
#undef WS_EX_TOPMOST
#undef WS_EX_ACCEPTFILES
#undef WS_EX_TRANSPARENT
#undef WS_EX_MDICHILD
#undef WS_EX_TOOLWINDOW
#undef WS_EX_WINDOWEDGE
#undef WS_EX_CLIENTEDGE
#undef WS_EX_CONTEXTHELP
#undef WS_EX_RIGHT
#undef WS_EX_LEFT
#undef WS_EX_RTLREADING
#undef WS_EX_LTRREADING
#undef WS_EX_LEFTSCROLLBAR
#undef WS_EX_RIGHTSCROLLBAR
#undef WS_EX_CONTROLPARENT
#undef WS_EX_STATICEDGE
#undef WS_EX_APPWINDOW
#undef WS_EX_OVERLAPPEDWINDOW
#undef WS_EX_PALETTEWINDOW
#undef WS_EX_LAYERED
#undef WS_EX_NOINHERITLAYOUT
#undef WS_EX_NOREDIRECTIONBITMAP
#undef WS_EX_LAYOUTRTL
#undef WS_EX_COMPOSITED
#undef WS_EX_NOACTIVATE
enum class
    [[clang::flag_enum]]
    WINDOW_EX_STYLE : int
{
    WS_EX_DLGMODALFRAME = 1,
    WS_EX_NOPARENTNOTIFY = 4,
    WS_EX_TOPMOST = 8,
    WS_EX_ACCEPTFILES = 16,
    WS_EX_TRANSPARENT = 32,
    WS_EX_MDICHILD = 64,
    WS_EX_TOOLWINDOW = 128,
    WS_EX_WINDOWEDGE = 256,
    WS_EX_CLIENTEDGE = 512,
    WS_EX_CONTEXTHELP = 1024,
    WS_EX_RIGHT = 4096,
    WS_EX_LEFT = 0,
    WS_EX_RTLREADING = 8192,
    WS_EX_LTRREADING = 0,
    WS_EX_LEFTSCROLLBAR = 16384,
    WS_EX_RIGHTSCROLLBAR = 0,
    WS_EX_CONTROLPARENT = 65536,
    WS_EX_STATICEDGE = 131072,
    WS_EX_APPWINDOW = 262144,
    WS_EX_OVERLAPPEDWINDOW = 768,
    WS_EX_PALETTEWINDOW = 392,
    WS_EX_LAYERED = 524288,
    WS_EX_NOINHERITLAYOUT = 1048576,
    WS_EX_NOREDIRECTIONBITMAP = 2097152,
    WS_EX_LAYOUTRTL = 4194304,
    WS_EX_COMPOSITED = 33554432,
    WS_EX_NOACTIVATE = 134217728,
};
#pragma pop_macro("WS_EX_NOACTIVATE")
#pragma pop_macro("WS_EX_COMPOSITED")
#pragma pop_macro("WS_EX_LAYOUTRTL")
#pragma pop_macro("WS_EX_NOREDIRECTIONBITMAP")
#pragma pop_macro("WS_EX_NOINHERITLAYOUT")
#pragma pop_macro("WS_EX_LAYERED")
#pragma pop_macro("WS_EX_PALETTEWINDOW")
#pragma pop_macro("WS_EX_OVERLAPPEDWINDOW")
#pragma pop_macro("WS_EX_APPWINDOW")
#pragma pop_macro("WS_EX_STATICEDGE")
#pragma pop_macro("WS_EX_CONTROLPARENT")
#pragma pop_macro("WS_EX_RIGHTSCROLLBAR")
#pragma pop_macro("WS_EX_LEFTSCROLLBAR")
#pragma pop_macro("WS_EX_LTRREADING")
#pragma pop_macro("WS_EX_RTLREADING")
#pragma pop_macro("WS_EX_LEFT")
#pragma pop_macro("WS_EX_RIGHT")
#pragma pop_macro("WS_EX_CONTEXTHELP")
#pragma pop_macro("WS_EX_CLIENTEDGE")
#pragma pop_macro("WS_EX_WINDOWEDGE")
#pragma pop_macro("WS_EX_TOOLWINDOW")
#pragma pop_macro("WS_EX_MDICHILD")
#pragma pop_macro("WS_EX_TRANSPARENT")
#pragma pop_macro("WS_EX_ACCEPTFILES")
#pragma pop_macro("WS_EX_TOPMOST")
#pragma pop_macro("WS_EX_NOPARENTNOTIFY")
#pragma pop_macro("WS_EX_DLGMODALFRAME")

#pragma push_macro("WS_OVERLAPPED")
#pragma push_macro("WS_POPUP")
#pragma push_macro("WS_CHILD")
#pragma push_macro("WS_MINIMIZE")
#pragma push_macro("WS_VISIBLE")
#pragma push_macro("WS_DISABLED")
#pragma push_macro("WS_CLIPSIBLINGS")
#pragma push_macro("WS_CLIPCHILDREN")
#pragma push_macro("WS_MAXIMIZE")
#pragma push_macro("WS_CAPTION")
#pragma push_macro("WS_BORDER")
#pragma push_macro("WS_DLGFRAME")
#pragma push_macro("WS_VSCROLL")
#pragma push_macro("WS_HSCROLL")
#pragma push_macro("WS_SYSMENU")
#pragma push_macro("WS_THICKFRAME")
#pragma push_macro("WS_GROUP")
#pragma push_macro("WS_TABSTOP")
#pragma push_macro("WS_MINIMIZEBOX")
#pragma push_macro("WS_MAXIMIZEBOX")
#pragma push_macro("WS_TILED")
#pragma push_macro("WS_ICONIC")
#pragma push_macro("WS_SIZEBOX")
#pragma push_macro("WS_TILEDWINDOW")
#pragma push_macro("WS_OVERLAPPEDWINDOW")
#pragma push_macro("WS_POPUPWINDOW")
#pragma push_macro("WS_CHILDWINDOW")
#pragma push_macro("WS_EX_DLGMODALFRAME")
#pragma push_macro("WS_EX_NOPARENTNOTIFY")
#pragma push_macro("WS_EX_TOPMOST")
#pragma push_macro("WS_EX_ACCEPTFILES")
#pragma push_macro("WS_EX_TRANSPARENT")
#pragma push_macro("WS_EX_MDICHILD")
#pragma push_macro("WS_EX_TOOLWINDOW")
#pragma push_macro("WS_EX_WINDOWEDGE")
#pragma push_macro("WS_EX_CLIENTEDGE")
#pragma push_macro("WS_EX_CONTEXTHELP")
#pragma push_macro("WS_EX_RIGHT")
#pragma push_macro("WS_EX_LEFT")
#pragma push_macro("WS_EX_RTLREADING")
#pragma push_macro("WS_EX_LTRREADING")
#pragma push_macro("WS_EX_LEFTSCROLLBAR")
#pragma push_macro("WS_EX_RIGHTSCROLLBAR")
#pragma push_macro("WS_EX_CONTROLPARENT")
#pragma push_macro("WS_EX_STATICEDGE")
#pragma push_macro("WS_EX_APPWINDOW")
#pragma push_macro("WS_EX_OVERLAPPEDWINDOW")
#pragma push_macro("WS_EX_PALETTEWINDOW")
#pragma push_macro("WS_EX_LAYERED")
#pragma push_macro("WS_EX_NOINHERITLAYOUT")
#pragma push_macro("WS_EX_NOREDIRECTIONBITMAP")
#pragma push_macro("WS_EX_LAYOUTRTL")
#pragma push_macro("WS_EX_COMPOSITED")
#pragma push_macro("WS_EX_NOACTIVATE")
#pragma push_macro("WS_ACTIVECAPTION")
#undef WS_OVERLAPPED
#undef WS_POPUP
#undef WS_CHILD
#undef WS_MINIMIZE
#undef WS_VISIBLE
#undef WS_DISABLED
#undef WS_CLIPSIBLINGS
#undef WS_CLIPCHILDREN
#undef WS_MAXIMIZE
#undef WS_CAPTION
#undef WS_BORDER
#undef WS_DLGFRAME
#undef WS_VSCROLL
#undef WS_HSCROLL
#undef WS_SYSMENU
#undef WS_THICKFRAME
#undef WS_GROUP
#undef WS_TABSTOP
#undef WS_MINIMIZEBOX
#undef WS_MAXIMIZEBOX
#undef WS_TILED
#undef WS_ICONIC
#undef WS_SIZEBOX
#undef WS_TILEDWINDOW
#undef WS_OVERLAPPEDWINDOW
#undef WS_POPUPWINDOW
#undef WS_CHILDWINDOW
#undef WS_EX_DLGMODALFRAME
#undef WS_EX_NOPARENTNOTIFY
#undef WS_EX_TOPMOST
#undef WS_EX_ACCEPTFILES
#undef WS_EX_TRANSPARENT
#undef WS_EX_MDICHILD
#undef WS_EX_TOOLWINDOW
#undef WS_EX_WINDOWEDGE
#undef WS_EX_CLIENTEDGE
#undef WS_EX_CONTEXTHELP
#undef WS_EX_RIGHT
#undef WS_EX_LEFT
#undef WS_EX_RTLREADING
#undef WS_EX_LTRREADING
#undef WS_EX_LEFTSCROLLBAR
#undef WS_EX_RIGHTSCROLLBAR
#undef WS_EX_CONTROLPARENT
#undef WS_EX_STATICEDGE
#undef WS_EX_APPWINDOW
#undef WS_EX_OVERLAPPEDWINDOW
#undef WS_EX_PALETTEWINDOW
#undef WS_EX_LAYERED
#undef WS_EX_NOINHERITLAYOUT
#undef WS_EX_NOREDIRECTIONBITMAP
#undef WS_EX_LAYOUTRTL
#undef WS_EX_COMPOSITED
#undef WS_EX_NOACTIVATE
#undef WS_ACTIVECAPTION
enum class
    [[clang::flag_enum]]
    WINDOW_STYLE : int
{
    WS_OVERLAPPED = 0,
    WS_POPUP = -2147483648,
    WS_CHILD = 1073741824,
    WS_MINIMIZE = 536870912,
    WS_VISIBLE = 268435456,
    WS_DISABLED = 134217728,
    WS_CLIPSIBLINGS = 67108864,
    WS_CLIPCHILDREN = 33554432,
    WS_MAXIMIZE = 16777216,
    WS_CAPTION = 12582912,
    WS_BORDER = 8388608,
    WS_DLGFRAME = 4194304,
    WS_VSCROLL = 2097152,
    WS_HSCROLL = 1048576,
    WS_SYSMENU = 524288,
    WS_THICKFRAME = 262144,
    WS_GROUP = 131072,
    WS_TABSTOP = 65536,
    WS_MINIMIZEBOX = 131072,
    WS_MAXIMIZEBOX = 65536,
    WS_TILED = 0,
    WS_ICONIC = 536870912,
    WS_SIZEBOX = 262144,
    WS_TILEDWINDOW = 13565952,
    WS_OVERLAPPEDWINDOW = 13565952,
    WS_POPUPWINDOW = -2138570752,
    WS_CHILDWINDOW = 1073741824,
    WS_EX_DLGMODALFRAME = 1,
    WS_EX_NOPARENTNOTIFY = 4,
    WS_EX_TOPMOST = 8,
    WS_EX_ACCEPTFILES = 16,
    WS_EX_TRANSPARENT = 32,
    WS_EX_MDICHILD = 64,
    WS_EX_TOOLWINDOW = 128,
    WS_EX_WINDOWEDGE = 256,
    WS_EX_CLIENTEDGE = 512,
    WS_EX_CONTEXTHELP = 1024,
    WS_EX_RIGHT = 4096,
    WS_EX_LEFT = 0,
    WS_EX_RTLREADING = 8192,
    WS_EX_LTRREADING = 0,
    WS_EX_LEFTSCROLLBAR = 16384,
    WS_EX_RIGHTSCROLLBAR = 0,
    WS_EX_CONTROLPARENT = 65536,
    WS_EX_STATICEDGE = 131072,
    WS_EX_APPWINDOW = 262144,
    WS_EX_OVERLAPPEDWINDOW = 768,
    WS_EX_PALETTEWINDOW = 392,
    WS_EX_LAYERED = 524288,
    WS_EX_NOINHERITLAYOUT = 1048576,
    WS_EX_NOREDIRECTIONBITMAP = 2097152,
    WS_EX_LAYOUTRTL = 4194304,
    WS_EX_COMPOSITED = 33554432,
    WS_EX_NOACTIVATE = 134217728,
    WS_ACTIVECAPTION = 1,
};
#pragma pop_macro("WS_ACTIVECAPTION")
#pragma pop_macro("WS_EX_NOACTIVATE")
#pragma pop_macro("WS_EX_COMPOSITED")
#pragma pop_macro("WS_EX_LAYOUTRTL")
#pragma pop_macro("WS_EX_NOREDIRECTIONBITMAP")
#pragma pop_macro("WS_EX_NOINHERITLAYOUT")
#pragma pop_macro("WS_EX_LAYERED")
#pragma pop_macro("WS_EX_PALETTEWINDOW")
#pragma pop_macro("WS_EX_OVERLAPPEDWINDOW")
#pragma pop_macro("WS_EX_APPWINDOW")
#pragma pop_macro("WS_EX_STATICEDGE")
#pragma pop_macro("WS_EX_CONTROLPARENT")
#pragma pop_macro("WS_EX_RIGHTSCROLLBAR")
#pragma pop_macro("WS_EX_LEFTSCROLLBAR")
#pragma pop_macro("WS_EX_LTRREADING")
#pragma pop_macro("WS_EX_RTLREADING")
#pragma pop_macro("WS_EX_LEFT")
#pragma pop_macro("WS_EX_RIGHT")
#pragma pop_macro("WS_EX_CONTEXTHELP")
#pragma pop_macro("WS_EX_CLIENTEDGE")
#pragma pop_macro("WS_EX_WINDOWEDGE")
#pragma pop_macro("WS_EX_TOOLWINDOW")
#pragma pop_macro("WS_EX_MDICHILD")
#pragma pop_macro("WS_EX_TRANSPARENT")
#pragma pop_macro("WS_EX_ACCEPTFILES")
#pragma pop_macro("WS_EX_TOPMOST")
#pragma pop_macro("WS_EX_NOPARENTNOTIFY")
#pragma pop_macro("WS_EX_DLGMODALFRAME")
#pragma pop_macro("WS_CHILDWINDOW")
#pragma pop_macro("WS_POPUPWINDOW")
#pragma pop_macro("WS_OVERLAPPEDWINDOW")
#pragma pop_macro("WS_TILEDWINDOW")
#pragma pop_macro("WS_SIZEBOX")
#pragma pop_macro("WS_ICONIC")
#pragma pop_macro("WS_TILED")
#pragma pop_macro("WS_MAXIMIZEBOX")
#pragma pop_macro("WS_MINIMIZEBOX")
#pragma pop_macro("WS_TABSTOP")
#pragma pop_macro("WS_GROUP")
#pragma pop_macro("WS_THICKFRAME")
#pragma pop_macro("WS_SYSMENU")
#pragma pop_macro("WS_HSCROLL")
#pragma pop_macro("WS_VSCROLL")
#pragma pop_macro("WS_DLGFRAME")
#pragma pop_macro("WS_BORDER")
#pragma pop_macro("WS_CAPTION")
#pragma pop_macro("WS_MAXIMIZE")
#pragma pop_macro("WS_CLIPCHILDREN")
#pragma pop_macro("WS_CLIPSIBLINGS")
#pragma pop_macro("WS_DISABLED")
#pragma pop_macro("WS_VISIBLE")
#pragma pop_macro("WS_MINIMIZE")
#pragma pop_macro("WS_CHILD")
#pragma pop_macro("WS_POPUP")
#pragma pop_macro("WS_OVERLAPPED")

#pragma push_macro("OBJID_WINDOW")
#pragma push_macro("OBJID_SYSMENU")
#pragma push_macro("OBJID_TITLEBAR")
#pragma push_macro("OBJID_MENU")
#pragma push_macro("OBJID_CLIENT")
#pragma push_macro("OBJID_VSCROLL")
#pragma push_macro("OBJID_HSCROLL")
#pragma push_macro("OBJID_SIZEGRIP")
#pragma push_macro("OBJID_CARET")
#pragma push_macro("OBJID_CURSOR")
#pragma push_macro("OBJID_ALERT")
#pragma push_macro("OBJID_SOUND")
#pragma push_macro("OBJID_QUERYCLASSNAMEIDX")
#pragma push_macro("OBJID_NATIVEOM")
#undef OBJID_WINDOW
#undef OBJID_SYSMENU
#undef OBJID_TITLEBAR
#undef OBJID_MENU
#undef OBJID_CLIENT
#undef OBJID_VSCROLL
#undef OBJID_HSCROLL
#undef OBJID_SIZEGRIP
#undef OBJID_CARET
#undef OBJID_CURSOR
#undef OBJID_ALERT
#undef OBJID_SOUND
#undef OBJID_QUERYCLASSNAMEIDX
#undef OBJID_NATIVEOM
enum class
    OBJECT_IDENTIFIER : int
{
    OBJID_WINDOW = 0,
    OBJID_SYSMENU = -1,
    OBJID_TITLEBAR = -2,
    OBJID_MENU = -3,
    OBJID_CLIENT = -4,
    OBJID_VSCROLL = -5,
    OBJID_HSCROLL = -6,
    OBJID_SIZEGRIP = -7,
    OBJID_CARET = -8,
    OBJID_CURSOR = -9,
    OBJID_ALERT = -10,
    OBJID_SOUND = -11,
    OBJID_QUERYCLASSNAMEIDX = -12,
    OBJID_NATIVEOM = -16,
};
#pragma pop_macro("OBJID_NATIVEOM")
#pragma pop_macro("OBJID_QUERYCLASSNAMEIDX")
#pragma pop_macro("OBJID_SOUND")
#pragma pop_macro("OBJID_ALERT")
#pragma pop_macro("OBJID_CURSOR")
#pragma pop_macro("OBJID_CARET")
#pragma pop_macro("OBJID_SIZEGRIP")
#pragma pop_macro("OBJID_HSCROLL")
#pragma pop_macro("OBJID_VSCROLL")
#pragma pop_macro("OBJID_CLIENT")
#pragma pop_macro("OBJID_MENU")
#pragma pop_macro("OBJID_TITLEBAR")
#pragma pop_macro("OBJID_SYSMENU")
#pragma pop_macro("OBJID_WINDOW")

#pragma push_macro("ENUM_CURRENT_SETTINGS")
#pragma push_macro("ENUM_REGISTRY_SETTINGS")
#undef ENUM_CURRENT_SETTINGS
#undef ENUM_REGISTRY_SETTINGS
enum class
    ENUM_DISPLAY_SETTINGS_MODE : int
{
    ENUM_CURRENT_SETTINGS = -1,
    ENUM_REGISTRY_SETTINGS = -2,
};
#pragma pop_macro("ENUM_REGISTRY_SETTINGS")
#pragma pop_macro("ENUM_CURRENT_SETTINGS")

#pragma push_macro("MFT_BITMAP")
#pragma push_macro("MFT_MENUBARBREAK")
#pragma push_macro("MFT_MENUBREAK")
#pragma push_macro("MFT_OWNERDRAW")
#pragma push_macro("MFT_RADIOCHECK")
#pragma push_macro("MFT_RIGHTJUSTIFY")
#pragma push_macro("MFT_RIGHTORDER")
#pragma push_macro("MFT_SEPARATOR")
#pragma push_macro("MFT_STRING")
#undef MFT_BITMAP
#undef MFT_MENUBARBREAK
#undef MFT_MENUBREAK
#undef MFT_OWNERDRAW
#undef MFT_RADIOCHECK
#undef MFT_RIGHTJUSTIFY
#undef MFT_RIGHTORDER
#undef MFT_SEPARATOR
#undef MFT_STRING
enum class
    [[clang::flag_enum]]
    MENU_ITEM_TYPE : int
{
    MFT_BITMAP = 4,
    MFT_MENUBARBREAK = 32,
    MFT_MENUBREAK = 64,
    MFT_OWNERDRAW = 256,
    MFT_RADIOCHECK = 512,
    MFT_RIGHTJUSTIFY = 16384,
    MFT_RIGHTORDER = 8192,
    MFT_SEPARATOR = 2048,
    MFT_STRING = 0,
};
#pragma pop_macro("MFT_STRING")
#pragma pop_macro("MFT_SEPARATOR")
#pragma pop_macro("MFT_RIGHTORDER")
#pragma pop_macro("MFT_RIGHTJUSTIFY")
#pragma pop_macro("MFT_RADIOCHECK")
#pragma pop_macro("MFT_OWNERDRAW")
#pragma pop_macro("MFT_MENUBREAK")
#pragma pop_macro("MFT_MENUBARBREAK")
#pragma pop_macro("MFT_BITMAP")

#pragma push_macro("RID_HEADER")
#pragma push_macro("RID_INPUT")
#undef RID_HEADER
#undef RID_INPUT
enum class
    RAW_INPUT_DATA_COMMAND_FLAGS : int
{
    RID_HEADER = 268435461,
    RID_INPUT = 268435459,
};
#pragma pop_macro("RID_INPUT")
#pragma pop_macro("RID_HEADER")

#pragma push_macro("BSF_ALLOWSFW")
#pragma push_macro("BSF_FLUSHDISK")
#pragma push_macro("BSF_FORCEIFHUNG")
#pragma push_macro("BSF_IGNORECURRENTTASK")
#pragma push_macro("BSF_NOHANG")
#pragma push_macro("BSF_NOTIMEOUTIFNOTHUNG")
#pragma push_macro("BSF_POSTMESSAGE")
#pragma push_macro("BSF_QUERY")
#pragma push_macro("BSF_SENDNOTIFYMESSAGE")
#pragma push_macro("BSF_LUID")
#pragma push_macro("BSF_RETURNHDESK")
#undef BSF_ALLOWSFW
#undef BSF_FLUSHDISK
#undef BSF_FORCEIFHUNG
#undef BSF_IGNORECURRENTTASK
#undef BSF_NOHANG
#undef BSF_NOTIMEOUTIFNOTHUNG
#undef BSF_POSTMESSAGE
#undef BSF_QUERY
#undef BSF_SENDNOTIFYMESSAGE
#undef BSF_LUID
#undef BSF_RETURNHDESK
enum class
    [[clang::flag_enum]]
    BROADCAST_SYSTEM_MESSAGE_FLAGS : int
{
    BSF_ALLOWSFW = 128,
    BSF_FLUSHDISK = 4,
    BSF_FORCEIFHUNG = 32,
    BSF_IGNORECURRENTTASK = 2,
    BSF_NOHANG = 8,
    BSF_NOTIMEOUTIFNOTHUNG = 64,
    BSF_POSTMESSAGE = 16,
    BSF_QUERY = 1,
    BSF_SENDNOTIFYMESSAGE = 256,
    BSF_LUID = 1024,
    BSF_RETURNHDESK = 512,
};
#pragma pop_macro("BSF_RETURNHDESK")
#pragma pop_macro("BSF_LUID")
#pragma pop_macro("BSF_SENDNOTIFYMESSAGE")
#pragma pop_macro("BSF_QUERY")
#pragma pop_macro("BSF_POSTMESSAGE")
#pragma pop_macro("BSF_NOTIMEOUTIFNOTHUNG")
#pragma pop_macro("BSF_NOHANG")
#pragma pop_macro("BSF_IGNORECURRENTTASK")
#pragma pop_macro("BSF_FORCEIFHUNG")
#pragma pop_macro("BSF_FLUSHDISK")
#pragma pop_macro("BSF_ALLOWSFW")

#pragma push_macro("DCX_WINDOW")
#pragma push_macro("DCX_CACHE")
#pragma push_macro("DCX_PARENTCLIP")
#pragma push_macro("DCX_CLIPSIBLINGS")
#pragma push_macro("DCX_CLIPCHILDREN")
#pragma push_macro("DCX_NORESETATTRS")
#pragma push_macro("DCX_LOCKWINDOWUPDATE")
#pragma push_macro("DCX_EXCLUDERGN")
#pragma push_macro("DCX_INTERSECTRGN")
#pragma push_macro("DCX_INTERSECTUPDATE")
#pragma push_macro("DCX_VALIDATE")
#undef DCX_WINDOW
#undef DCX_CACHE
#undef DCX_PARENTCLIP
#undef DCX_CLIPSIBLINGS
#undef DCX_CLIPCHILDREN
#undef DCX_NORESETATTRS
#undef DCX_LOCKWINDOWUPDATE
#undef DCX_EXCLUDERGN
#undef DCX_INTERSECTRGN
#undef DCX_INTERSECTUPDATE
#undef DCX_VALIDATE
enum class
    [[clang::flag_enum]]
    GET_DCX_FLAGS : int
{
    DCX_WINDOW = 1,
    DCX_CACHE = 2,
    DCX_PARENTCLIP = 32,
    DCX_CLIPSIBLINGS = 16,
    DCX_CLIPCHILDREN = 8,
    DCX_NORESETATTRS = 4,
    DCX_LOCKWINDOWUPDATE = 1024,
    DCX_EXCLUDERGN = 64,
    DCX_INTERSECTRGN = 128,
    DCX_INTERSECTUPDATE = 512,
    DCX_VALIDATE = 2097152,
};
#pragma pop_macro("DCX_VALIDATE")
#pragma pop_macro("DCX_INTERSECTUPDATE")
#pragma pop_macro("DCX_INTERSECTRGN")
#pragma pop_macro("DCX_EXCLUDERGN")
#pragma pop_macro("DCX_LOCKWINDOWUPDATE")
#pragma pop_macro("DCX_NORESETATTRS")
#pragma pop_macro("DCX_CLIPCHILDREN")
#pragma pop_macro("DCX_CLIPSIBLINGS")
#pragma pop_macro("DCX_PARENTCLIP")
#pragma pop_macro("DCX_CACHE")
#pragma pop_macro("DCX_WINDOW")

#pragma push_macro("RIDI_PREPARSEDDATA")
#pragma push_macro("RIDI_DEVICENAME")
#pragma push_macro("RIDI_DEVICEINFO")
#undef RIDI_PREPARSEDDATA
#undef RIDI_DEVICENAME
#undef RIDI_DEVICEINFO
enum class
    RAW_INPUT_DEVICE_INFO_COMMAND : int
{
    RIDI_PREPARSEDDATA = 536870917,
    RIDI_DEVICENAME = 536870919,
    RIDI_DEVICEINFO = 536870923,
};
#pragma pop_macro("RIDI_DEVICEINFO")
#pragma pop_macro("RIDI_DEVICENAME")
#pragma pop_macro("RIDI_PREPARSEDDATA")

#pragma push_macro("ULW_ALPHA")
#pragma push_macro("ULW_COLORKEY")
#pragma push_macro("ULW_OPAQUE")
#pragma push_macro("ULW_EX_NORESIZE")
#undef ULW_ALPHA
#undef ULW_COLORKEY
#undef ULW_OPAQUE
#undef ULW_EX_NORESIZE
enum class
    UPDATE_LAYERED_WINDOW_FLAGS : int
{
    ULW_ALPHA = 2,
    ULW_COLORKEY = 1,
    ULW_OPAQUE = 4,
    ULW_EX_NORESIZE = 8,
};
#pragma pop_macro("ULW_EX_NORESIZE")
#pragma pop_macro("ULW_OPAQUE")
#pragma pop_macro("ULW_COLORKEY")
#pragma pop_macro("ULW_ALPHA")

#pragma push_macro("AW_ACTIVATE")
#pragma push_macro("AW_BLEND")
#pragma push_macro("AW_CENTER")
#pragma push_macro("AW_HIDE")
#pragma push_macro("AW_HOR_POSITIVE")
#pragma push_macro("AW_HOR_NEGATIVE")
#pragma push_macro("AW_SLIDE")
#pragma push_macro("AW_VER_POSITIVE")
#pragma push_macro("AW_VER_NEGATIVE")
#undef AW_ACTIVATE
#undef AW_BLEND
#undef AW_CENTER
#undef AW_HIDE
#undef AW_HOR_POSITIVE
#undef AW_HOR_NEGATIVE
#undef AW_SLIDE
#undef AW_VER_POSITIVE
#undef AW_VER_NEGATIVE
enum class
    [[clang::flag_enum]]
    ANIMATE_WINDOW_FLAGS : int
{
    AW_ACTIVATE = 131072,
    AW_BLEND = 524288,
    AW_CENTER = 16,
    AW_HIDE = 65536,
    AW_HOR_POSITIVE = 1,
    AW_HOR_NEGATIVE = 2,
    AW_SLIDE = 262144,
    AW_VER_POSITIVE = 4,
    AW_VER_NEGATIVE = 8,
};
#pragma pop_macro("AW_VER_NEGATIVE")
#pragma pop_macro("AW_VER_POSITIVE")
#pragma pop_macro("AW_SLIDE")
#pragma pop_macro("AW_HOR_NEGATIVE")
#pragma pop_macro("AW_HOR_POSITIVE")
#pragma pop_macro("AW_HIDE")
#pragma pop_macro("AW_CENTER")
#pragma pop_macro("AW_BLEND")
#pragma pop_macro("AW_ACTIVATE")

#pragma push_macro("MSGFLT_ADD")
#pragma push_macro("MSGFLT_REMOVE")
#undef MSGFLT_ADD
#undef MSGFLT_REMOVE
enum class
    CHANGE_WINDOW_MESSAGE_FILTER_FLAGS : int
{
    MSGFLT_ADD = 1,
    MSGFLT_REMOVE = 2,
};
#pragma pop_macro("MSGFLT_REMOVE")
#pragma pop_macro("MSGFLT_ADD")

#pragma push_macro("WH_CALLWNDPROC")
#pragma push_macro("WH_CALLWNDPROCRET")
#pragma push_macro("WH_CBT")
#pragma push_macro("WH_DEBUG")
#pragma push_macro("WH_FOREGROUNDIDLE")
#pragma push_macro("WH_GETMESSAGE")
#pragma push_macro("WH_JOURNALPLAYBACK")
#pragma push_macro("WH_JOURNALRECORD")
#pragma push_macro("WH_KEYBOARD")
#pragma push_macro("WH_KEYBOARD_LL")
#pragma push_macro("WH_MOUSE")
#pragma push_macro("WH_MOUSE_LL")
#pragma push_macro("WH_MSGFILTER")
#pragma push_macro("WH_SHELL")
#pragma push_macro("WH_SYSMSGFILTER")
#undef WH_CALLWNDPROC
#undef WH_CALLWNDPROCRET
#undef WH_CBT
#undef WH_DEBUG
#undef WH_FOREGROUNDIDLE
#undef WH_GETMESSAGE
#undef WH_JOURNALPLAYBACK
#undef WH_JOURNALRECORD
#undef WH_KEYBOARD
#undef WH_KEYBOARD_LL
#undef WH_MOUSE
#undef WH_MOUSE_LL
#undef WH_MSGFILTER
#undef WH_SHELL
#undef WH_SYSMSGFILTER
enum class
    WINDOWS_HOOK_ID : int
{
    WH_CALLWNDPROC = 4,
    WH_CALLWNDPROCRET = 12,
    WH_CBT = 5,
    WH_DEBUG = 9,
    WH_FOREGROUNDIDLE = 11,
    WH_GETMESSAGE = 3,
    WH_JOURNALPLAYBACK = 1,
    WH_JOURNALRECORD = 0,
    WH_KEYBOARD = 2,
    WH_KEYBOARD_LL = 13,
    WH_MOUSE = 7,
    WH_MOUSE_LL = 14,
    WH_MSGFILTER = -1,
    WH_SHELL = 10,
    WH_SYSMSGFILTER = 6,
};
#pragma pop_macro("WH_SYSMSGFILTER")
#pragma pop_macro("WH_SHELL")
#pragma pop_macro("WH_MSGFILTER")
#pragma pop_macro("WH_MOUSE_LL")
#pragma pop_macro("WH_MOUSE")
#pragma pop_macro("WH_KEYBOARD_LL")
#pragma pop_macro("WH_KEYBOARD")
#pragma pop_macro("WH_JOURNALRECORD")
#pragma pop_macro("WH_JOURNALPLAYBACK")
#pragma pop_macro("WH_GETMESSAGE")
#pragma pop_macro("WH_FOREGROUNDIDLE")
#pragma pop_macro("WH_DEBUG")
#pragma pop_macro("WH_CBT")
#pragma pop_macro("WH_CALLWNDPROCRET")
#pragma pop_macro("WH_CALLWNDPROC")

#pragma push_macro("TWF_FINETOUCH")
#pragma push_macro("TWF_WANTPALM")
#undef TWF_FINETOUCH
#undef TWF_WANTPALM
enum class
    REGISTER_TOUCH_WINDOW_FLAGS : int
{
    TWF_FINETOUCH = 1,
    TWF_WANTPALM = 2,
};
#pragma pop_macro("TWF_WANTPALM")
#pragma pop_macro("TWF_FINETOUCH")

#pragma push_macro("SPIF_UPDATEINIFILE")
#pragma push_macro("SPIF_SENDCHANGE")
#pragma push_macro("SPIF_SENDWININICHANGE")
#undef SPIF_UPDATEINIFILE
#undef SPIF_SENDCHANGE
#undef SPIF_SENDWININICHANGE
enum class
    [[clang::flag_enum]]
    SYSTEM_PARAMETERS_INFO_UPDATE_FLAGS : int
{
    SPIF_UPDATEINIFILE = 1,
    SPIF_SENDCHANGE = 2,
    SPIF_SENDWININICHANGE = 2,
};
#pragma pop_macro("SPIF_SENDWININICHANGE")
#pragma pop_macro("SPIF_SENDCHANGE")
#pragma pop_macro("SPIF_UPDATEINIFILE")

#pragma push_macro("PW_CLIENTONLY")
#undef PW_CLIENTONLY
enum class
    PRINT_WINDOW_FLAGS : int
{
    PW_CLIENTONLY = 1,
};
#pragma pop_macro("PW_CLIENTONLY")

#pragma push_macro("SWP_ASYNCWINDOWPOS")
#pragma push_macro("SWP_DEFERERASE")
#pragma push_macro("SWP_DRAWFRAME")
#pragma push_macro("SWP_FRAMECHANGED")
#pragma push_macro("SWP_HIDEWINDOW")
#pragma push_macro("SWP_NOACTIVATE")
#pragma push_macro("SWP_NOCOPYBITS")
#pragma push_macro("SWP_NOMOVE")
#pragma push_macro("SWP_NOOWNERZORDER")
#pragma push_macro("SWP_NOREDRAW")
#pragma push_macro("SWP_NOREPOSITION")
#pragma push_macro("SWP_NOSENDCHANGING")
#pragma push_macro("SWP_NOSIZE")
#pragma push_macro("SWP_NOZORDER")
#pragma push_macro("SWP_SHOWWINDOW")
#undef SWP_ASYNCWINDOWPOS
#undef SWP_DEFERERASE
#undef SWP_DRAWFRAME
#undef SWP_FRAMECHANGED
#undef SWP_HIDEWINDOW
#undef SWP_NOACTIVATE
#undef SWP_NOCOPYBITS
#undef SWP_NOMOVE
#undef SWP_NOOWNERZORDER
#undef SWP_NOREDRAW
#undef SWP_NOREPOSITION
#undef SWP_NOSENDCHANGING
#undef SWP_NOSIZE
#undef SWP_NOZORDER
#undef SWP_SHOWWINDOW
enum class
    [[clang::flag_enum]]
    SET_WINDOW_POS_FLAGS : int
{
    SWP_ASYNCWINDOWPOS = 16384,
    SWP_DEFERERASE = 8192,
    SWP_DRAWFRAME = 32,
    SWP_FRAMECHANGED = 32,
    SWP_HIDEWINDOW = 128,
    SWP_NOACTIVATE = 16,
    SWP_NOCOPYBITS = 256,
    SWP_NOMOVE = 2,
    SWP_NOOWNERZORDER = 512,
    SWP_NOREDRAW = 8,
    SWP_NOREPOSITION = 512,
    SWP_NOSENDCHANGING = 1024,
    SWP_NOSIZE = 1,
    SWP_NOZORDER = 4,
    SWP_SHOWWINDOW = 64,
};
#pragma pop_macro("SWP_SHOWWINDOW")
#pragma pop_macro("SWP_NOZORDER")
#pragma pop_macro("SWP_NOSIZE")
#pragma pop_macro("SWP_NOSENDCHANGING")
#pragma pop_macro("SWP_NOREPOSITION")
#pragma pop_macro("SWP_NOREDRAW")
#pragma pop_macro("SWP_NOOWNERZORDER")
#pragma pop_macro("SWP_NOMOVE")
#pragma pop_macro("SWP_NOCOPYBITS")
#pragma pop_macro("SWP_NOACTIVATE")
#pragma pop_macro("SWP_HIDEWINDOW")
#pragma pop_macro("SWP_FRAMECHANGED")
#pragma pop_macro("SWP_DRAWFRAME")
#pragma pop_macro("SWP_DEFERERASE")
#pragma pop_macro("SWP_ASYNCWINDOWPOS")

#pragma push_macro("DDL_ARCHIVE")
#pragma push_macro("DDL_DIRECTORY")
#pragma push_macro("DDL_DRIVES")
#pragma push_macro("DDL_EXCLUSIVE")
#pragma push_macro("DDL_HIDDEN")
#pragma push_macro("DDL_READONLY")
#pragma push_macro("DDL_READWRITE")
#pragma push_macro("DDL_SYSTEM")
#pragma push_macro("DDL_POSTMSGS")
#undef DDL_ARCHIVE
#undef DDL_DIRECTORY
#undef DDL_DRIVES
#undef DDL_EXCLUSIVE
#undef DDL_HIDDEN
#undef DDL_READONLY
#undef DDL_READWRITE
#undef DDL_SYSTEM
#undef DDL_POSTMSGS
enum class
    [[clang::flag_enum]]
    DLG_DIR_LIST_FILE_TYPE : int
{
    DDL_ARCHIVE = 32,
    DDL_DIRECTORY = 16,
    DDL_DRIVES = 16384,
    DDL_EXCLUSIVE = 32768,
    DDL_HIDDEN = 2,
    DDL_READONLY = 1,
    DDL_READWRITE = 0,
    DDL_SYSTEM = 4,
    DDL_POSTMSGS = 8192,
};
#pragma pop_macro("DDL_POSTMSGS")
#pragma pop_macro("DDL_SYSTEM")
#pragma pop_macro("DDL_READWRITE")
#pragma pop_macro("DDL_READONLY")
#pragma pop_macro("DDL_HIDDEN")
#pragma pop_macro("DDL_EXCLUSIVE")
#pragma pop_macro("DDL_DRIVES")
#pragma pop_macro("DDL_DIRECTORY")
#pragma pop_macro("DDL_ARCHIVE")

#pragma push_macro("MWMO_NONE")
#pragma push_macro("MWMO_ALERTABLE")
#pragma push_macro("MWMO_INPUTAVAILABLE")
#pragma push_macro("MWMO_WAITALL")
#undef MWMO_NONE
#undef MWMO_ALERTABLE
#undef MWMO_INPUTAVAILABLE
#undef MWMO_WAITALL
enum class
    [[clang::flag_enum]]
    MSG_WAIT_FOR_MULTIPLE_OBJECTS_EX_FLAGS : int
{
    MWMO_NONE = 0,
    MWMO_ALERTABLE = 2,
    MWMO_INPUTAVAILABLE = 4,
    MWMO_WAITALL = 1,
};
#pragma pop_macro("MWMO_WAITALL")
#pragma pop_macro("MWMO_INPUTAVAILABLE")
#pragma pop_macro("MWMO_ALERTABLE")
#pragma pop_macro("MWMO_NONE")

#pragma push_macro("QS_ALLEVENTS")
#pragma push_macro("QS_ALLINPUT")
#pragma push_macro("QS_ALLPOSTMESSAGE")
#pragma push_macro("QS_HOTKEY")
#pragma push_macro("QS_INPUT")
#pragma push_macro("QS_KEY")
#pragma push_macro("QS_MOUSE")
#pragma push_macro("QS_MOUSEBUTTON")
#pragma push_macro("QS_MOUSEMOVE")
#pragma push_macro("QS_PAINT")
#pragma push_macro("QS_POSTMESSAGE")
#pragma push_macro("QS_RAWINPUT")
#pragma push_macro("QS_SENDMESSAGE")
#pragma push_macro("QS_TIMER")
#undef QS_ALLEVENTS
#undef QS_ALLINPUT
#undef QS_ALLPOSTMESSAGE
#undef QS_HOTKEY
#undef QS_INPUT
#undef QS_KEY
#undef QS_MOUSE
#undef QS_MOUSEBUTTON
#undef QS_MOUSEMOVE
#undef QS_PAINT
#undef QS_POSTMESSAGE
#undef QS_RAWINPUT
#undef QS_SENDMESSAGE
#undef QS_TIMER
enum class
    [[clang::flag_enum]]
    QUEUE_STATUS_FLAGS : int
{
    QS_ALLEVENTS = 7359,
    QS_ALLINPUT = 7423,
    QS_ALLPOSTMESSAGE = 256,
    QS_HOTKEY = 128,
    QS_INPUT = 7175,
    QS_KEY = 1,
    QS_MOUSE = 6,
    QS_MOUSEBUTTON = 4,
    QS_MOUSEMOVE = 2,
    QS_PAINT = 32,
    QS_POSTMESSAGE = 8,
    QS_RAWINPUT = 1024,
    QS_SENDMESSAGE = 64,
    QS_TIMER = 16,
};
#pragma pop_macro("QS_TIMER")
#pragma pop_macro("QS_SENDMESSAGE")
#pragma pop_macro("QS_RAWINPUT")
#pragma pop_macro("QS_POSTMESSAGE")
#pragma pop_macro("QS_PAINT")
#pragma pop_macro("QS_MOUSEMOVE")
#pragma pop_macro("QS_MOUSEBUTTON")
#pragma pop_macro("QS_MOUSE")
#pragma pop_macro("QS_KEY")
#pragma pop_macro("QS_INPUT")
#pragma pop_macro("QS_HOTKEY")
#pragma pop_macro("QS_ALLPOSTMESSAGE")
#pragma pop_macro("QS_ALLINPUT")
#pragma pop_macro("QS_ALLEVENTS")

#pragma push_macro("UOI_FLAGS")
#pragma push_macro("UOI_HEAPSIZE")
#pragma push_macro("UOI_IO")
#pragma push_macro("UOI_NAME")
#pragma push_macro("UOI_TYPE")
#pragma push_macro("UOI_USER_SID")
#undef UOI_FLAGS
#undef UOI_HEAPSIZE
#undef UOI_IO
#undef UOI_NAME
#undef UOI_TYPE
#undef UOI_USER_SID
enum class
    USER_OBJECT_INFORMATION_INDEX : int
{
    UOI_FLAGS = 1,
    UOI_HEAPSIZE = 5,
    UOI_IO = 6,
    UOI_NAME = 2,
    UOI_TYPE = 3,
    UOI_USER_SID = 4,
};
#pragma pop_macro("UOI_USER_SID")
#pragma pop_macro("UOI_TYPE")
#pragma pop_macro("UOI_NAME")
#pragma pop_macro("UOI_IO")
#pragma pop_macro("UOI_HEAPSIZE")
#pragma pop_macro("UOI_FLAGS")

#pragma push_macro("OCR_APPSTARTING")
#pragma push_macro("OCR_NORMAL")
#pragma push_macro("OCR_CROSS")
#pragma push_macro("OCR_HAND")
#pragma push_macro("OCR_HELP")
#pragma push_macro("OCR_IBEAM")
#pragma push_macro("OCR_NO")
#pragma push_macro("OCR_SIZEALL")
#pragma push_macro("OCR_SIZENESW")
#pragma push_macro("OCR_SIZENS")
#pragma push_macro("OCR_SIZENWSE")
#pragma push_macro("OCR_SIZEWE")
#pragma push_macro("OCR_UP")
#pragma push_macro("OCR_WAIT")
#undef OCR_APPSTARTING
#undef OCR_NORMAL
#undef OCR_CROSS
#undef OCR_HAND
#undef OCR_HELP
#undef OCR_IBEAM
#undef OCR_NO
#undef OCR_SIZEALL
#undef OCR_SIZENESW
#undef OCR_SIZENS
#undef OCR_SIZENWSE
#undef OCR_SIZEWE
#undef OCR_UP
#undef OCR_WAIT
enum class
    SYSTEM_CURSOR_ID : int
{
    OCR_APPSTARTING = 32650,
    OCR_NORMAL = 32512,
    OCR_CROSS = 32515,
    OCR_HAND = 32649,
    OCR_HELP = 32651,
    OCR_IBEAM = 32513,
    OCR_NO = 32648,
    OCR_SIZEALL = 32646,
    OCR_SIZENESW = 32643,
    OCR_SIZENS = 32645,
    OCR_SIZENWSE = 32642,
    OCR_SIZEWE = 32644,
    OCR_UP = 32516,
    OCR_WAIT = 32514,
};
#pragma pop_macro("OCR_WAIT")
#pragma pop_macro("OCR_UP")
#pragma pop_macro("OCR_SIZEWE")
#pragma pop_macro("OCR_SIZENWSE")
#pragma pop_macro("OCR_SIZENS")
#pragma pop_macro("OCR_SIZENESW")
#pragma pop_macro("OCR_SIZEALL")
#pragma pop_macro("OCR_NO")
#pragma pop_macro("OCR_IBEAM")
#pragma pop_macro("OCR_HELP")
#pragma pop_macro("OCR_HAND")
#pragma pop_macro("OCR_CROSS")
#pragma pop_macro("OCR_NORMAL")
#pragma pop_macro("OCR_APPSTARTING")

#pragma push_macro("ESB_DISABLE_BOTH")
#pragma push_macro("ESB_DISABLE_DOWN")
#pragma push_macro("ESB_DISABLE_LEFT")
#pragma push_macro("ESB_DISABLE_LTUP")
#pragma push_macro("ESB_DISABLE_RIGHT")
#pragma push_macro("ESB_DISABLE_RTDN")
#pragma push_macro("ESB_DISABLE_UP")
#pragma push_macro("ESB_ENABLE_BOTH")
#undef ESB_DISABLE_BOTH
#undef ESB_DISABLE_DOWN
#undef ESB_DISABLE_LEFT
#undef ESB_DISABLE_LTUP
#undef ESB_DISABLE_RIGHT
#undef ESB_DISABLE_RTDN
#undef ESB_DISABLE_UP
#undef ESB_ENABLE_BOTH
enum class
    ENABLE_SCROLL_BAR_ARROWS : unsigned int
{
    ESB_DISABLE_BOTH = 0x00000003u,
    ESB_DISABLE_DOWN = 0x00000002u,
    ESB_DISABLE_LEFT = 0x00000001u,
    ESB_DISABLE_LTUP = 0x00000001u,
    ESB_DISABLE_RIGHT = 0x00000002u,
    ESB_DISABLE_RTDN = 0x00000002u,
    ESB_DISABLE_UP = 0x00000001u,
    ESB_ENABLE_BOTH = 0x00000000u,
};
#pragma pop_macro("ESB_ENABLE_BOTH")
#pragma pop_macro("ESB_DISABLE_UP")
#pragma pop_macro("ESB_DISABLE_RTDN")
#pragma pop_macro("ESB_DISABLE_RIGHT")
#pragma pop_macro("ESB_DISABLE_LTUP")
#pragma pop_macro("ESB_DISABLE_LEFT")
#pragma pop_macro("ESB_DISABLE_DOWN")
#pragma pop_macro("ESB_DISABLE_BOTH")

#pragma push_macro("LWA_ALPHA")
#pragma push_macro("LWA_COLORKEY")
#undef LWA_ALPHA
#undef LWA_COLORKEY
enum class
    [[clang::flag_enum]]
    LAYERED_WINDOW_ATTRIBUTES_FLAGS : int
{
    LWA_ALPHA = 2,
    LWA_COLORKEY = 1,
};
#pragma pop_macro("LWA_COLORKEY")
#pragma pop_macro("LWA_ALPHA")

#pragma push_macro("SMTO_ABORTIFHUNG")
#pragma push_macro("SMTO_BLOCK")
#pragma push_macro("SMTO_NORMAL")
#pragma push_macro("SMTO_NOTIMEOUTIFNOTHUNG")
#pragma push_macro("SMTO_ERRORONEXIT")
#undef SMTO_ABORTIFHUNG
#undef SMTO_BLOCK
#undef SMTO_NORMAL
#undef SMTO_NOTIMEOUTIFNOTHUNG
#undef SMTO_ERRORONEXIT
enum class
    [[clang::flag_enum]]
    SEND_MESSAGE_TIMEOUT_FLAGS : int
{
    SMTO_ABORTIFHUNG = 2,
    SMTO_BLOCK = 1,
    SMTO_NORMAL = 0,
    SMTO_NOTIMEOUTIFNOTHUNG = 8,
    SMTO_ERRORONEXIT = 32,
};
#pragma pop_macro("SMTO_ERRORONEXIT")
#pragma pop_macro("SMTO_NOTIMEOUTIFNOTHUNG")
#pragma pop_macro("SMTO_NORMAL")
#pragma pop_macro("SMTO_BLOCK")
#pragma pop_macro("SMTO_ABORTIFHUNG")

#pragma push_macro("PM_NOREMOVE")
#pragma push_macro("PM_REMOVE")
#pragma push_macro("PM_NOYIELD")
#pragma push_macro("PM_QS_INPUT")
#pragma push_macro("PM_QS_POSTMESSAGE")
#pragma push_macro("PM_QS_PAINT")
#pragma push_macro("PM_QS_SENDMESSAGE")
#undef PM_NOREMOVE
#undef PM_REMOVE
#undef PM_NOYIELD
#undef PM_QS_INPUT
#undef PM_QS_POSTMESSAGE
#undef PM_QS_PAINT
#undef PM_QS_SENDMESSAGE
enum class
    [[clang::flag_enum]]
    PEEK_MESSAGE_REMOVE_TYPE : int
{
    PM_NOREMOVE = 0,
    PM_REMOVE = 1,
    PM_NOYIELD = 2,
    PM_QS_INPUT = 470220800,
    PM_QS_POSTMESSAGE = 9961472,
    PM_QS_PAINT = 2097152,
    PM_QS_SENDMESSAGE = 4194304,
};
#pragma pop_macro("PM_QS_SENDMESSAGE")
#pragma pop_macro("PM_QS_PAINT")
#pragma pop_macro("PM_QS_POSTMESSAGE")
#pragma pop_macro("PM_QS_INPUT")
#pragma pop_macro("PM_NOYIELD")
#pragma pop_macro("PM_REMOVE")
#pragma pop_macro("PM_NOREMOVE")

#pragma push_macro("GW_CHILD")
#pragma push_macro("GW_ENABLEDPOPUP")
#pragma push_macro("GW_HWNDFIRST")
#pragma push_macro("GW_HWNDLAST")
#pragma push_macro("GW_HWNDNEXT")
#pragma push_macro("GW_HWNDPREV")
#pragma push_macro("GW_OWNER")
#undef GW_CHILD
#undef GW_ENABLEDPOPUP
#undef GW_HWNDFIRST
#undef GW_HWNDLAST
#undef GW_HWNDNEXT
#undef GW_HWNDPREV
#undef GW_OWNER
enum class
    GET_WINDOW_CMD : int
{
    GW_CHILD = 5,
    GW_ENABLEDPOPUP = 6,
    GW_HWNDFIRST = 0,
    GW_HWNDLAST = 1,
    GW_HWNDNEXT = 2,
    GW_HWNDPREV = 3,
    GW_OWNER = 4,
};
#pragma pop_macro("GW_OWNER")
#pragma pop_macro("GW_HWNDPREV")
#pragma pop_macro("GW_HWNDNEXT")
#pragma pop_macro("GW_HWNDLAST")
#pragma pop_macro("GW_HWNDFIRST")
#pragma pop_macro("GW_ENABLEDPOPUP")
#pragma pop_macro("GW_CHILD")

#pragma push_macro("SM_ARRANGE")
#pragma push_macro("SM_CLEANBOOT")
#pragma push_macro("SM_CMONITORS")
#pragma push_macro("SM_CMOUSEBUTTONS")
#pragma push_macro("SM_CONVERTIBLESLATEMODE")
#pragma push_macro("SM_CXBORDER")
#pragma push_macro("SM_CXCURSOR")
#pragma push_macro("SM_CXDLGFRAME")
#pragma push_macro("SM_CXDOUBLECLK")
#pragma push_macro("SM_CXDRAG")
#pragma push_macro("SM_CXEDGE")
#pragma push_macro("SM_CXFIXEDFRAME")
#pragma push_macro("SM_CXFOCUSBORDER")
#pragma push_macro("SM_CXFRAME")
#pragma push_macro("SM_CXFULLSCREEN")
#pragma push_macro("SM_CXHSCROLL")
#pragma push_macro("SM_CXHTHUMB")
#pragma push_macro("SM_CXICON")
#pragma push_macro("SM_CXICONSPACING")
#pragma push_macro("SM_CXMAXIMIZED")
#pragma push_macro("SM_CXMAXTRACK")
#pragma push_macro("SM_CXMENUCHECK")
#pragma push_macro("SM_CXMENUSIZE")
#pragma push_macro("SM_CXMIN")
#pragma push_macro("SM_CXMINIMIZED")
#pragma push_macro("SM_CXMINSPACING")
#pragma push_macro("SM_CXMINTRACK")
#pragma push_macro("SM_CXPADDEDBORDER")
#pragma push_macro("SM_CXSCREEN")
#pragma push_macro("SM_CXSIZE")
#pragma push_macro("SM_CXSIZEFRAME")
#pragma push_macro("SM_CXSMICON")
#pragma push_macro("SM_CXSMSIZE")
#pragma push_macro("SM_CXVIRTUALSCREEN")
#pragma push_macro("SM_CXVSCROLL")
#pragma push_macro("SM_CYBORDER")
#pragma push_macro("SM_CYCAPTION")
#pragma push_macro("SM_CYCURSOR")
#pragma push_macro("SM_CYDLGFRAME")
#pragma push_macro("SM_CYDOUBLECLK")
#pragma push_macro("SM_CYDRAG")
#pragma push_macro("SM_CYEDGE")
#pragma push_macro("SM_CYFIXEDFRAME")
#pragma push_macro("SM_CYFOCUSBORDER")
#pragma push_macro("SM_CYFRAME")
#pragma push_macro("SM_CYFULLSCREEN")
#pragma push_macro("SM_CYHSCROLL")
#pragma push_macro("SM_CYICON")
#pragma push_macro("SM_CYICONSPACING")
#pragma push_macro("SM_CYKANJIWINDOW")
#pragma push_macro("SM_CYMAXIMIZED")
#pragma push_macro("SM_CYMAXTRACK")
#pragma push_macro("SM_CYMENU")
#pragma push_macro("SM_CYMENUCHECK")
#pragma push_macro("SM_CYMENUSIZE")
#pragma push_macro("SM_CYMIN")
#pragma push_macro("SM_CYMINIMIZED")
#pragma push_macro("SM_CYMINSPACING")
#pragma push_macro("SM_CYMINTRACK")
#pragma push_macro("SM_CYSCREEN")
#pragma push_macro("SM_CYSIZE")
#pragma push_macro("SM_CYSIZEFRAME")
#pragma push_macro("SM_CYSMCAPTION")
#pragma push_macro("SM_CYSMICON")
#pragma push_macro("SM_CYSMSIZE")
#pragma push_macro("SM_CYVIRTUALSCREEN")
#pragma push_macro("SM_CYVSCROLL")
#pragma push_macro("SM_CYVTHUMB")
#pragma push_macro("SM_DBCSENABLED")
#pragma push_macro("SM_DEBUG")
#pragma push_macro("SM_DIGITIZER")
#pragma push_macro("SM_IMMENABLED")
#pragma push_macro("SM_MAXIMUMTOUCHES")
#pragma push_macro("SM_MEDIACENTER")
#pragma push_macro("SM_MENUDROPALIGNMENT")
#pragma push_macro("SM_MIDEASTENABLED")
#pragma push_macro("SM_MOUSEPRESENT")
#pragma push_macro("SM_MOUSEHORIZONTALWHEELPRESENT")
#pragma push_macro("SM_MOUSEWHEELPRESENT")
#pragma push_macro("SM_NETWORK")
#pragma push_macro("SM_PENWINDOWS")
#pragma push_macro("SM_REMOTECONTROL")
#pragma push_macro("SM_REMOTESESSION")
#pragma push_macro("SM_SAMEDISPLAYFORMAT")
#pragma push_macro("SM_SECURE")
#pragma push_macro("SM_SERVERR2")
#pragma push_macro("SM_SHOWSOUNDS")
#pragma push_macro("SM_SHUTTINGDOWN")
#pragma push_macro("SM_SLOWMACHINE")
#pragma push_macro("SM_STARTER")
#pragma push_macro("SM_SWAPBUTTON")
#pragma push_macro("SM_SYSTEMDOCKED")
#pragma push_macro("SM_TABLETPC")
#pragma push_macro("SM_XVIRTUALSCREEN")
#pragma push_macro("SM_YVIRTUALSCREEN")
#undef SM_ARRANGE
#undef SM_CLEANBOOT
#undef SM_CMONITORS
#undef SM_CMOUSEBUTTONS
#undef SM_CONVERTIBLESLATEMODE
#undef SM_CXBORDER
#undef SM_CXCURSOR
#undef SM_CXDLGFRAME
#undef SM_CXDOUBLECLK
#undef SM_CXDRAG
#undef SM_CXEDGE
#undef SM_CXFIXEDFRAME
#undef SM_CXFOCUSBORDER
#undef SM_CXFRAME
#undef SM_CXFULLSCREEN
#undef SM_CXHSCROLL
#undef SM_CXHTHUMB
#undef SM_CXICON
#undef SM_CXICONSPACING
#undef SM_CXMAXIMIZED
#undef SM_CXMAXTRACK
#undef SM_CXMENUCHECK
#undef SM_CXMENUSIZE
#undef SM_CXMIN
#undef SM_CXMINIMIZED
#undef SM_CXMINSPACING
#undef SM_CXMINTRACK
#undef SM_CXPADDEDBORDER
#undef SM_CXSCREEN
#undef SM_CXSIZE
#undef SM_CXSIZEFRAME
#undef SM_CXSMICON
#undef SM_CXSMSIZE
#undef SM_CXVIRTUALSCREEN
#undef SM_CXVSCROLL
#undef SM_CYBORDER
#undef SM_CYCAPTION
#undef SM_CYCURSOR
#undef SM_CYDLGFRAME
#undef SM_CYDOUBLECLK
#undef SM_CYDRAG
#undef SM_CYEDGE
#undef SM_CYFIXEDFRAME
#undef SM_CYFOCUSBORDER
#undef SM_CYFRAME
#undef SM_CYFULLSCREEN
#undef SM_CYHSCROLL
#undef SM_CYICON
#undef SM_CYICONSPACING
#undef SM_CYKANJIWINDOW
#undef SM_CYMAXIMIZED
#undef SM_CYMAXTRACK
#undef SM_CYMENU
#undef SM_CYMENUCHECK
#undef SM_CYMENUSIZE
#undef SM_CYMIN
#undef SM_CYMINIMIZED
#undef SM_CYMINSPACING
#undef SM_CYMINTRACK
#undef SM_CYSCREEN
#undef SM_CYSIZE
#undef SM_CYSIZEFRAME
#undef SM_CYSMCAPTION
#undef SM_CYSMICON
#undef SM_CYSMSIZE
#undef SM_CYVIRTUALSCREEN
#undef SM_CYVSCROLL
#undef SM_CYVTHUMB
#undef SM_DBCSENABLED
#undef SM_DEBUG
#undef SM_DIGITIZER
#undef SM_IMMENABLED
#undef SM_MAXIMUMTOUCHES
#undef SM_MEDIACENTER
#undef SM_MENUDROPALIGNMENT
#undef SM_MIDEASTENABLED
#undef SM_MOUSEPRESENT
#undef SM_MOUSEHORIZONTALWHEELPRESENT
#undef SM_MOUSEWHEELPRESENT
#undef SM_NETWORK
#undef SM_PENWINDOWS
#undef SM_REMOTECONTROL
#undef SM_REMOTESESSION
#undef SM_SAMEDISPLAYFORMAT
#undef SM_SECURE
#undef SM_SERVERR2
#undef SM_SHOWSOUNDS
#undef SM_SHUTTINGDOWN
#undef SM_SLOWMACHINE
#undef SM_STARTER
#undef SM_SWAPBUTTON
#undef SM_SYSTEMDOCKED
#undef SM_TABLETPC
#undef SM_XVIRTUALSCREEN
#undef SM_YVIRTUALSCREEN
enum class
    SYSTEM_METRICS_INDEX : int
{
    SM_ARRANGE = 56,
    SM_CLEANBOOT = 67,
    SM_CMONITORS = 80,
    SM_CMOUSEBUTTONS = 43,
    SM_CONVERTIBLESLATEMODE = 8195,
    SM_CXBORDER = 5,
    SM_CXCURSOR = 13,
    SM_CXDLGFRAME = 7,
    SM_CXDOUBLECLK = 36,
    SM_CXDRAG = 68,
    SM_CXEDGE = 45,
    SM_CXFIXEDFRAME = 7,
    SM_CXFOCUSBORDER = 83,
    SM_CXFRAME = 32,
    SM_CXFULLSCREEN = 16,
    SM_CXHSCROLL = 21,
    SM_CXHTHUMB = 10,
    SM_CXICON = 11,
    SM_CXICONSPACING = 38,
    SM_CXMAXIMIZED = 61,
    SM_CXMAXTRACK = 59,
    SM_CXMENUCHECK = 71,
    SM_CXMENUSIZE = 54,
    SM_CXMIN = 28,
    SM_CXMINIMIZED = 57,
    SM_CXMINSPACING = 47,
    SM_CXMINTRACK = 34,
    SM_CXPADDEDBORDER = 92,
    SM_CXSCREEN = 0,
    SM_CXSIZE = 30,
    SM_CXSIZEFRAME = 32,
    SM_CXSMICON = 49,
    SM_CXSMSIZE = 52,
    SM_CXVIRTUALSCREEN = 78,
    SM_CXVSCROLL = 2,
    SM_CYBORDER = 6,
    SM_CYCAPTION = 4,
    SM_CYCURSOR = 14,
    SM_CYDLGFRAME = 8,
    SM_CYDOUBLECLK = 37,
    SM_CYDRAG = 69,
    SM_CYEDGE = 46,
    SM_CYFIXEDFRAME = 8,
    SM_CYFOCUSBORDER = 84,
    SM_CYFRAME = 33,
    SM_CYFULLSCREEN = 17,
    SM_CYHSCROLL = 3,
    SM_CYICON = 12,
    SM_CYICONSPACING = 39,
    SM_CYKANJIWINDOW = 18,
    SM_CYMAXIMIZED = 62,
    SM_CYMAXTRACK = 60,
    SM_CYMENU = 15,
    SM_CYMENUCHECK = 72,
    SM_CYMENUSIZE = 55,
    SM_CYMIN = 29,
    SM_CYMINIMIZED = 58,
    SM_CYMINSPACING = 48,
    SM_CYMINTRACK = 35,
    SM_CYSCREEN = 1,
    SM_CYSIZE = 31,
    SM_CYSIZEFRAME = 33,
    SM_CYSMCAPTION = 51,
    SM_CYSMICON = 50,
    SM_CYSMSIZE = 53,
    SM_CYVIRTUALSCREEN = 79,
    SM_CYVSCROLL = 20,
    SM_CYVTHUMB = 9,
    SM_DBCSENABLED = 42,
    SM_DEBUG = 22,
    SM_DIGITIZER = 94,
    SM_IMMENABLED = 82,
    SM_MAXIMUMTOUCHES = 95,
    SM_MEDIACENTER = 87,
    SM_MENUDROPALIGNMENT = 40,
    SM_MIDEASTENABLED = 74,
    SM_MOUSEPRESENT = 19,
    SM_MOUSEHORIZONTALWHEELPRESENT = 91,
    SM_MOUSEWHEELPRESENT = 75,
    SM_NETWORK = 63,
    SM_PENWINDOWS = 41,
    SM_REMOTECONTROL = 8193,
    SM_REMOTESESSION = 4096,
    SM_SAMEDISPLAYFORMAT = 81,
    SM_SECURE = 44,
    SM_SERVERR2 = 89,
    SM_SHOWSOUNDS = 70,
    SM_SHUTTINGDOWN = 8192,
    SM_SLOWMACHINE = 73,
    SM_STARTER = 88,
    SM_SWAPBUTTON = 23,
    SM_SYSTEMDOCKED = 8196,
    SM_TABLETPC = 86,
    SM_XVIRTUALSCREEN = 76,
    SM_YVIRTUALSCREEN = 77,
};
#pragma pop_macro("SM_YVIRTUALSCREEN")
#pragma pop_macro("SM_XVIRTUALSCREEN")
#pragma pop_macro("SM_TABLETPC")
#pragma pop_macro("SM_SYSTEMDOCKED")
#pragma pop_macro("SM_SWAPBUTTON")
#pragma pop_macro("SM_STARTER")
#pragma pop_macro("SM_SLOWMACHINE")
#pragma pop_macro("SM_SHUTTINGDOWN")
#pragma pop_macro("SM_SHOWSOUNDS")
#pragma pop_macro("SM_SERVERR2")
#pragma pop_macro("SM_SECURE")
#pragma pop_macro("SM_SAMEDISPLAYFORMAT")
#pragma pop_macro("SM_REMOTESESSION")
#pragma pop_macro("SM_REMOTECONTROL")
#pragma pop_macro("SM_PENWINDOWS")
#pragma pop_macro("SM_NETWORK")
#pragma pop_macro("SM_MOUSEWHEELPRESENT")
#pragma pop_macro("SM_MOUSEHORIZONTALWHEELPRESENT")
#pragma pop_macro("SM_MOUSEPRESENT")
#pragma pop_macro("SM_MIDEASTENABLED")
#pragma pop_macro("SM_MENUDROPALIGNMENT")
#pragma pop_macro("SM_MEDIACENTER")
#pragma pop_macro("SM_MAXIMUMTOUCHES")
#pragma pop_macro("SM_IMMENABLED")
#pragma pop_macro("SM_DIGITIZER")
#pragma pop_macro("SM_DEBUG")
#pragma pop_macro("SM_DBCSENABLED")
#pragma pop_macro("SM_CYVTHUMB")
#pragma pop_macro("SM_CYVSCROLL")
#pragma pop_macro("SM_CYVIRTUALSCREEN")
#pragma pop_macro("SM_CYSMSIZE")
#pragma pop_macro("SM_CYSMICON")
#pragma pop_macro("SM_CYSMCAPTION")
#pragma pop_macro("SM_CYSIZEFRAME")
#pragma pop_macro("SM_CYSIZE")
#pragma pop_macro("SM_CYSCREEN")
#pragma pop_macro("SM_CYMINTRACK")
#pragma pop_macro("SM_CYMINSPACING")
#pragma pop_macro("SM_CYMINIMIZED")
#pragma pop_macro("SM_CYMIN")
#pragma pop_macro("SM_CYMENUSIZE")
#pragma pop_macro("SM_CYMENUCHECK")
#pragma pop_macro("SM_CYMENU")
#pragma pop_macro("SM_CYMAXTRACK")
#pragma pop_macro("SM_CYMAXIMIZED")
#pragma pop_macro("SM_CYKANJIWINDOW")
#pragma pop_macro("SM_CYICONSPACING")
#pragma pop_macro("SM_CYICON")
#pragma pop_macro("SM_CYHSCROLL")
#pragma pop_macro("SM_CYFULLSCREEN")
#pragma pop_macro("SM_CYFRAME")
#pragma pop_macro("SM_CYFOCUSBORDER")
#pragma pop_macro("SM_CYFIXEDFRAME")
#pragma pop_macro("SM_CYEDGE")
#pragma pop_macro("SM_CYDRAG")
#pragma pop_macro("SM_CYDOUBLECLK")
#pragma pop_macro("SM_CYDLGFRAME")
#pragma pop_macro("SM_CYCURSOR")
#pragma pop_macro("SM_CYCAPTION")
#pragma pop_macro("SM_CYBORDER")
#pragma pop_macro("SM_CXVSCROLL")
#pragma pop_macro("SM_CXVIRTUALSCREEN")
#pragma pop_macro("SM_CXSMSIZE")
#pragma pop_macro("SM_CXSMICON")
#pragma pop_macro("SM_CXSIZEFRAME")
#pragma pop_macro("SM_CXSIZE")
#pragma pop_macro("SM_CXSCREEN")
#pragma pop_macro("SM_CXPADDEDBORDER")
#pragma pop_macro("SM_CXMINTRACK")
#pragma pop_macro("SM_CXMINSPACING")
#pragma pop_macro("SM_CXMINIMIZED")
#pragma pop_macro("SM_CXMIN")
#pragma pop_macro("SM_CXMENUSIZE")
#pragma pop_macro("SM_CXMENUCHECK")
#pragma pop_macro("SM_CXMAXTRACK")
#pragma pop_macro("SM_CXMAXIMIZED")
#pragma pop_macro("SM_CXICONSPACING")
#pragma pop_macro("SM_CXICON")
#pragma pop_macro("SM_CXHTHUMB")
#pragma pop_macro("SM_CXHSCROLL")
#pragma pop_macro("SM_CXFULLSCREEN")
#pragma pop_macro("SM_CXFRAME")
#pragma pop_macro("SM_CXFOCUSBORDER")
#pragma pop_macro("SM_CXFIXEDFRAME")
#pragma pop_macro("SM_CXEDGE")
#pragma pop_macro("SM_CXDRAG")
#pragma pop_macro("SM_CXDOUBLECLK")
#pragma pop_macro("SM_CXDLGFRAME")
#pragma pop_macro("SM_CXCURSOR")
#pragma pop_macro("SM_CXBORDER")
#pragma pop_macro("SM_CONVERTIBLESLATEMODE")
#pragma pop_macro("SM_CMOUSEBUTTONS")
#pragma pop_macro("SM_CMONITORS")
#pragma pop_macro("SM_CLEANBOOT")
#pragma pop_macro("SM_ARRANGE")

#pragma push_macro("GMMP_USE_DISPLAY_POINTS")
#pragma push_macro("GMMP_USE_HIGH_RESOLUTION_POINTS")
#undef GMMP_USE_DISPLAY_POINTS
#undef GMMP_USE_HIGH_RESOLUTION_POINTS
enum class
    GET_MOUSE_MOVE_POINTS_EX_RESOLUTION : int
{
    GMMP_USE_DISPLAY_POINTS = 1,
    GMMP_USE_HIGH_RESOLUTION_POINTS = 2,
};
#pragma pop_macro("GMMP_USE_HIGH_RESOLUTION_POINTS")
#pragma pop_macro("GMMP_USE_DISPLAY_POINTS")

#pragma push_macro("BST_CHECKED")
#pragma push_macro("BST_INDETERMINATE")
#pragma push_macro("BST_UNCHECKED")
#undef BST_CHECKED
#undef BST_INDETERMINATE
#undef BST_UNCHECKED
enum class
    DLG_BUTTON_CHECK_STATE : int
{
    BST_CHECKED = 1,
    BST_INDETERMINATE = 2,
    BST_UNCHECKED = 0,
};
#pragma pop_macro("BST_UNCHECKED")
#pragma pop_macro("BST_INDETERMINATE")
#pragma pop_macro("BST_CHECKED")

#pragma push_macro("KEYEVENTF_EXTENDEDKEY")
#pragma push_macro("KEYEVENTF_KEYUP")
#pragma push_macro("KEYEVENTF_SCANCODE")
#pragma push_macro("KEYEVENTF_UNICODE")
#undef KEYEVENTF_EXTENDEDKEY
#undef KEYEVENTF_KEYUP
#undef KEYEVENTF_SCANCODE
#undef KEYEVENTF_UNICODE
enum class
    [[clang::flag_enum]]
    KEYBD_EVENT_FLAGS : int
{
    KEYEVENTF_EXTENDEDKEY = 1,
    KEYEVENTF_KEYUP = 2,
    KEYEVENTF_SCANCODE = 8,
    KEYEVENTF_UNICODE = 4,
};
#pragma pop_macro("KEYEVENTF_UNICODE")
#pragma pop_macro("KEYEVENTF_SCANCODE")
#pragma pop_macro("KEYEVENTF_KEYUP")
#pragma pop_macro("KEYEVENTF_EXTENDEDKEY")

#pragma push_macro("GA_PARENT")
#pragma push_macro("GA_ROOT")
#pragma push_macro("GA_ROOTOWNER")
#undef GA_PARENT
#undef GA_ROOT
#undef GA_ROOTOWNER
enum class
    GET_ANCESTOR_FLAGS : int
{
    GA_PARENT = 1,
    GA_ROOT = 2,
    GA_ROOTOWNER = 3,
};
#pragma pop_macro("GA_ROOTOWNER")
#pragma pop_macro("GA_ROOT")
#pragma pop_macro("GA_PARENT")

#pragma push_macro("DC_ACTIVE")
#pragma push_macro("DC_BUTTONS")
#pragma push_macro("DC_GRADIENT")
#pragma push_macro("DC_ICON")
#pragma push_macro("DC_INBUTTON")
#pragma push_macro("DC_SMALLCAP")
#pragma push_macro("DC_TEXT")
#undef DC_ACTIVE
#undef DC_BUTTONS
#undef DC_GRADIENT
#undef DC_ICON
#undef DC_INBUTTON
#undef DC_SMALLCAP
#undef DC_TEXT
enum class
    [[clang::flag_enum]]
    DRAW_CAPTION_FLAGS : int
{
    DC_ACTIVE = 1,
    DC_BUTTONS = 4096,
    DC_GRADIENT = 32,
    DC_ICON = 4,
    DC_INBUTTON = 16,
    DC_SMALLCAP = 2,
    DC_TEXT = 8,
};
#pragma pop_macro("DC_TEXT")
#pragma pop_macro("DC_SMALLCAP")
#pragma pop_macro("DC_INBUTTON")
#pragma pop_macro("DC_ICON")
#pragma pop_macro("DC_GRADIENT")
#pragma pop_macro("DC_BUTTONS")
#pragma pop_macro("DC_ACTIVE")

#pragma push_macro("MDITILE_HORIZONTAL")
#pragma push_macro("MDITILE_VERTICAL")
#undef MDITILE_HORIZONTAL
#undef MDITILE_VERTICAL
enum class
    TILE_WINDOWS_HOW : int
{
    MDITILE_HORIZONTAL = 1,
    MDITILE_VERTICAL = 0,
};
#pragma pop_macro("MDITILE_VERTICAL")
#pragma pop_macro("MDITILE_HORIZONTAL")

#pragma push_macro("WDA_NONE")
#pragma push_macro("WDA_MONITOR")
#pragma push_macro("WDA_EXCLUDEFROMCAPTURE")
#undef WDA_NONE
#undef WDA_MONITOR
#undef WDA_EXCLUDEFROMCAPTURE
enum class
    WINDOW_DISPLAY_AFFINITY : int
{
    WDA_NONE = 0,
    WDA_MONITOR = 1,
    WDA_EXCLUDEFROMCAPTURE = 17,
};
#pragma pop_macro("WDA_EXCLUDEFROMCAPTURE")
#pragma pop_macro("WDA_MONITOR")
#pragma pop_macro("WDA_NONE")

#pragma push_macro("EWX_LOGOFF")
#pragma push_macro("EWX_SHUTDOWN")
#pragma push_macro("EWX_REBOOT")
#pragma push_macro("EWX_FORCE")
#pragma push_macro("EWX_POWEROFF")
#pragma push_macro("EWX_FORCEIFHUNG")
#pragma push_macro("EWX_QUICKRESOLVE")
#pragma push_macro("EWX_RESTARTAPPS")
#pragma push_macro("EWX_HYBRID_SHUTDOWN")
#pragma push_macro("EWX_BOOTOPTIONS")
#pragma push_macro("EWX_ARSO")
#pragma push_macro("EWX_CHECK_SAFE_FOR_SERVER")
#pragma push_macro("EWX_SYSTEM_INITIATED")
#undef EWX_LOGOFF
#undef EWX_SHUTDOWN
#undef EWX_REBOOT
#undef EWX_FORCE
#undef EWX_POWEROFF
#undef EWX_FORCEIFHUNG
#undef EWX_QUICKRESOLVE
#undef EWX_RESTARTAPPS
#undef EWX_HYBRID_SHUTDOWN
#undef EWX_BOOTOPTIONS
#undef EWX_ARSO
#undef EWX_CHECK_SAFE_FOR_SERVER
#undef EWX_SYSTEM_INITIATED
enum class
    [[clang::flag_enum]]
    EXIT_WINDOWS_FLAGS : int
{
    EWX_LOGOFF = 0,
    EWX_SHUTDOWN = 1,
    EWX_REBOOT = 2,
    EWX_FORCE = 4,
    EWX_POWEROFF = 8,
    EWX_FORCEIFHUNG = 16,
    EWX_QUICKRESOLVE = 32,
    EWX_RESTARTAPPS = 64,
    EWX_HYBRID_SHUTDOWN = 4194304,
    EWX_BOOTOPTIONS = 16777216,
    EWX_ARSO = 67108864,
    EWX_CHECK_SAFE_FOR_SERVER = 134217728,
    EWX_SYSTEM_INITIATED = 268435456,
};
#pragma pop_macro("EWX_SYSTEM_INITIATED")
#pragma pop_macro("EWX_CHECK_SAFE_FOR_SERVER")
#pragma pop_macro("EWX_ARSO")
#pragma pop_macro("EWX_BOOTOPTIONS")
#pragma pop_macro("EWX_HYBRID_SHUTDOWN")
#pragma pop_macro("EWX_RESTARTAPPS")
#pragma pop_macro("EWX_QUICKRESOLVE")
#pragma pop_macro("EWX_FORCEIFHUNG")
#pragma pop_macro("EWX_POWEROFF")
#pragma pop_macro("EWX_FORCE")
#pragma pop_macro("EWX_REBOOT")
#pragma pop_macro("EWX_SHUTDOWN")
#pragma pop_macro("EWX_LOGOFF")

#pragma push_macro("LSFW_LOCK")
#pragma push_macro("LSFW_UNLOCK")
#undef LSFW_LOCK
#undef LSFW_UNLOCK
enum class
    FOREGROUND_WINDOW_LOCK_CODE : int
{
    LSFW_LOCK = 1,
    LSFW_UNLOCK = 2,
};
#pragma pop_macro("LSFW_UNLOCK")
#pragma pop_macro("LSFW_LOCK")

#pragma push_macro("MOUSEEVENTF_ABSOLUTE")
#pragma push_macro("MOUSEEVENTF_LEFTDOWN")
#pragma push_macro("MOUSEEVENTF_LEFTUP")
#pragma push_macro("MOUSEEVENTF_MIDDLEDOWN")
#pragma push_macro("MOUSEEVENTF_MIDDLEUP")
#pragma push_macro("MOUSEEVENTF_MOVE")
#pragma push_macro("MOUSEEVENTF_RIGHTDOWN")
#pragma push_macro("MOUSEEVENTF_RIGHTUP")
#pragma push_macro("MOUSEEVENTF_WHEEL")
#pragma push_macro("MOUSEEVENTF_XDOWN")
#pragma push_macro("MOUSEEVENTF_XUP")
#pragma push_macro("MOUSEEVENTF_HWHEEL")
#pragma push_macro("MOUSEEVENTF_MOVE_NOCOALESCE")
#pragma push_macro("MOUSEEVENTF_VIRTUALDESK")
#undef MOUSEEVENTF_ABSOLUTE
#undef MOUSEEVENTF_LEFTDOWN
#undef MOUSEEVENTF_LEFTUP
#undef MOUSEEVENTF_MIDDLEDOWN
#undef MOUSEEVENTF_MIDDLEUP
#undef MOUSEEVENTF_MOVE
#undef MOUSEEVENTF_RIGHTDOWN
#undef MOUSEEVENTF_RIGHTUP
#undef MOUSEEVENTF_WHEEL
#undef MOUSEEVENTF_XDOWN
#undef MOUSEEVENTF_XUP
#undef MOUSEEVENTF_HWHEEL
#undef MOUSEEVENTF_MOVE_NOCOALESCE
#undef MOUSEEVENTF_VIRTUALDESK
enum class
    MOUSE_EVENT_FLAGS : int
{
    MOUSEEVENTF_ABSOLUTE = 32768,
    MOUSEEVENTF_LEFTDOWN = 2,
    MOUSEEVENTF_LEFTUP = 4,
    MOUSEEVENTF_MIDDLEDOWN = 32,
    MOUSEEVENTF_MIDDLEUP = 64,
    MOUSEEVENTF_MOVE = 1,
    MOUSEEVENTF_RIGHTDOWN = 8,
    MOUSEEVENTF_RIGHTUP = 16,
    MOUSEEVENTF_WHEEL = 2048,
    MOUSEEVENTF_XDOWN = 128,
    MOUSEEVENTF_XUP = 256,
    MOUSEEVENTF_HWHEEL = 4096,
    MOUSEEVENTF_MOVE_NOCOALESCE = 8192,
    MOUSEEVENTF_VIRTUALDESK = 16384,
};
#pragma pop_macro("MOUSEEVENTF_VIRTUALDESK")
#pragma pop_macro("MOUSEEVENTF_MOVE_NOCOALESCE")
#pragma pop_macro("MOUSEEVENTF_HWHEEL")
#pragma pop_macro("MOUSEEVENTF_XUP")
#pragma pop_macro("MOUSEEVENTF_XDOWN")
#pragma pop_macro("MOUSEEVENTF_WHEEL")
#pragma pop_macro("MOUSEEVENTF_RIGHTUP")
#pragma pop_macro("MOUSEEVENTF_RIGHTDOWN")
#pragma pop_macro("MOUSEEVENTF_MOVE")
#pragma pop_macro("MOUSEEVENTF_MIDDLEUP")
#pragma pop_macro("MOUSEEVENTF_MIDDLEDOWN")
#pragma pop_macro("MOUSEEVENTF_LEFTUP")
#pragma pop_macro("MOUSEEVENTF_LEFTDOWN")
#pragma pop_macro("MOUSEEVENTF_ABSOLUTE")

#pragma push_macro("MDITILE_SKIPDISABLED")
#pragma push_macro("MDITILE_ZORDER")
#undef MDITILE_SKIPDISABLED
#undef MDITILE_ZORDER
enum class
    [[clang::flag_enum]]
    CASCADE_WINDOWS_HOW : int
{
    MDITILE_SKIPDISABLED = 2,
    MDITILE_ZORDER = 4,
};
#pragma pop_macro("MDITILE_ZORDER")
#pragma pop_macro("MDITILE_SKIPDISABLED")

#pragma push_macro("MSGFLT_ALLOW")
#pragma push_macro("MSGFLT_DISALLOW")
#pragma push_macro("MSGFLT_RESET")
#undef MSGFLT_ALLOW
#undef MSGFLT_DISALLOW
#undef MSGFLT_RESET
enum class
    WINDOW_MESSAGE_FILTER_ACTION : int
{
    MSGFLT_ALLOW = 1,
    MSGFLT_DISALLOW = 2,
    MSGFLT_RESET = 0,
};
#pragma pop_macro("MSGFLT_RESET")
#pragma pop_macro("MSGFLT_DISALLOW")
#pragma pop_macro("MSGFLT_ALLOW")

#pragma push_macro("GMDI_GOINTOPOPUPS")
#pragma push_macro("GMDI_USEDISABLED")
#undef GMDI_GOINTOPOPUPS
#undef GMDI_USEDISABLED
enum class
    [[clang::flag_enum]]
    GET_MENU_DEFAULT_ITEM_FLAGS : int
{
    GMDI_GOINTOPOPUPS = 2,
    GMDI_USEDISABLED = 1,
};
#pragma pop_macro("GMDI_USEDISABLED")
#pragma pop_macro("GMDI_GOINTOPOPUPS")

#pragma push_macro("GR_GLOBAL")
#pragma push_macro("GR_GDIOBJECTS")
#pragma push_macro("GR_GDIOBJECTS_PEAK")
#pragma push_macro("GR_USEROBJECTS")
#pragma push_macro("GR_USEROBJECTS_PEAK")
#undef GR_GLOBAL
#undef GR_GDIOBJECTS
#undef GR_GDIOBJECTS_PEAK
#undef GR_USEROBJECTS
#undef GR_USEROBJECTS_PEAK
enum class
    GET_GUI_RESOURCES_FLAGS : int
{
    GR_GLOBAL = -2,
    GR_GDIOBJECTS = 0,
    GR_GDIOBJECTS_PEAK = 2,
    GR_USEROBJECTS = 1,
    GR_USEROBJECTS_PEAK = 4,
};
#pragma pop_macro("GR_USEROBJECTS_PEAK")
#pragma pop_macro("GR_USEROBJECTS")
#pragma pop_macro("GR_GDIOBJECTS_PEAK")
#pragma pop_macro("GR_GDIOBJECTS")
#pragma pop_macro("GR_GLOBAL")

#pragma push_macro("SSGF_DISPLAY")
#pragma push_macro("SSGF_NONE")
#undef SSGF_DISPLAY
#undef SSGF_NONE
enum class
    SOUND_SENTRY_GRAPHICS_EFFECT : int
{
    SSGF_DISPLAY = 3,
    SSGF_NONE = 0,
};
#pragma pop_macro("SSGF_NONE")
#pragma pop_macro("SSGF_DISPLAY")

#pragma push_macro("RIM_TYPEMOUSE")
#pragma push_macro("RIM_TYPEKEYBOARD")
#pragma push_macro("RIM_TYPEHID")
#undef RIM_TYPEMOUSE
#undef RIM_TYPEKEYBOARD
#undef RIM_TYPEHID
enum class
    RID_DEVICE_INFO_TYPE : int
{
    RIM_TYPEMOUSE = 0,
    RIM_TYPEKEYBOARD = 1,
    RIM_TYPEHID = 2,
};
#pragma pop_macro("RIM_TYPEHID")
#pragma pop_macro("RIM_TYPEKEYBOARD")
#pragma pop_macro("RIM_TYPEMOUSE")

#pragma push_macro("MSGFLTINFO_NONE")
#pragma push_macro("MSGFLTINFO_ALLOWED_HIGHER")
#pragma push_macro("MSGFLTINFO_ALREADYALLOWED_FORWND")
#pragma push_macro("MSGFLTINFO_ALREADYDISALLOWED_FORWND")
#undef MSGFLTINFO_NONE
#undef MSGFLTINFO_ALLOWED_HIGHER
#undef MSGFLTINFO_ALREADYALLOWED_FORWND
#undef MSGFLTINFO_ALREADYDISALLOWED_FORWND
enum class
    MSGFLTINFO_STATUS : int
{
    MSGFLTINFO_NONE = 0,
    MSGFLTINFO_ALLOWED_HIGHER = 3,
    MSGFLTINFO_ALREADYALLOWED_FORWND = 1,
    MSGFLTINFO_ALREADYDISALLOWED_FORWND = 2,
};
#pragma pop_macro("MSGFLTINFO_ALREADYDISALLOWED_FORWND")
#pragma pop_macro("MSGFLTINFO_ALREADYALLOWED_FORWND")
#pragma pop_macro("MSGFLTINFO_ALLOWED_HIGHER")
#pragma pop_macro("MSGFLTINFO_NONE")

#pragma push_macro("SERKF_AVAILABLE")
#pragma push_macro("SERKF_INDICATOR")
#pragma push_macro("SERKF_SERIALKEYSON")
#undef SERKF_AVAILABLE
#undef SERKF_INDICATOR
#undef SERKF_SERIALKEYSON
enum class
    [[clang::flag_enum]]
    SERIALKEYS_FLAGS : int
{
    SERKF_AVAILABLE = 2,
    SERKF_INDICATOR = 4,
    SERKF_SERIALKEYSON = 1,
};
#pragma pop_macro("SERKF_SERIALKEYSON")
#pragma pop_macro("SERKF_INDICATOR")
#pragma pop_macro("SERKF_AVAILABLE")

#pragma push_macro("HCF_HIGHCONTRASTON")
#pragma push_macro("HCF_AVAILABLE")
#pragma push_macro("HCF_HOTKEYACTIVE")
#pragma push_macro("HCF_CONFIRMHOTKEY")
#pragma push_macro("HCF_HOTKEYSOUND")
#pragma push_macro("HCF_INDICATOR")
#pragma push_macro("HCF_HOTKEYAVAILABLE")
#pragma push_macro("HCF_OPTION_NOTHEMECHANGE")
#undef HCF_HIGHCONTRASTON
#undef HCF_AVAILABLE
#undef HCF_HOTKEYACTIVE
#undef HCF_CONFIRMHOTKEY
#undef HCF_HOTKEYSOUND
#undef HCF_INDICATOR
#undef HCF_HOTKEYAVAILABLE
#undef HCF_OPTION_NOTHEMECHANGE
enum class
    [[clang::flag_enum]]
    HIGHCONTRASTW_FLAGS : int
{
    HCF_HIGHCONTRASTON = 1,
    HCF_AVAILABLE = 2,
    HCF_HOTKEYACTIVE = 4,
    HCF_CONFIRMHOTKEY = 8,
    HCF_HOTKEYSOUND = 16,
    HCF_INDICATOR = 32,
    HCF_HOTKEYAVAILABLE = 64,
    HCF_OPTION_NOTHEMECHANGE = 4096,
};
#pragma pop_macro("HCF_OPTION_NOTHEMECHANGE")
#pragma pop_macro("HCF_HOTKEYAVAILABLE")
#pragma pop_macro("HCF_INDICATOR")
#pragma pop_macro("HCF_HOTKEYSOUND")
#pragma pop_macro("HCF_CONFIRMHOTKEY")
#pragma pop_macro("HCF_HOTKEYACTIVE")
#pragma pop_macro("HCF_AVAILABLE")
#pragma pop_macro("HCF_HIGHCONTRASTON")

#pragma push_macro("SSTF_BORDER")
#pragma push_macro("SSTF_CHARS")
#pragma push_macro("SSTF_DISPLAY")
#pragma push_macro("SSTF_NONE")
#undef SSTF_BORDER
#undef SSTF_CHARS
#undef SSTF_DISPLAY
#undef SSTF_NONE
enum class
    SOUNDSENTRY_TEXT_EFFECT : int
{
    SSTF_BORDER = 2,
    SSTF_CHARS = 1,
    SSTF_DISPLAY = 3,
    SSTF_NONE = 0,
};
#pragma pop_macro("SSTF_NONE")
#pragma pop_macro("SSTF_DISPLAY")
#pragma pop_macro("SSTF_CHARS")
#pragma pop_macro("SSTF_BORDER")

#pragma push_macro("MIIM_BITMAP")
#pragma push_macro("MIIM_CHECKMARKS")
#pragma push_macro("MIIM_DATA")
#pragma push_macro("MIIM_FTYPE")
#pragma push_macro("MIIM_ID")
#pragma push_macro("MIIM_STATE")
#pragma push_macro("MIIM_STRING")
#pragma push_macro("MIIM_SUBMENU")
#pragma push_macro("MIIM_TYPE")
#undef MIIM_BITMAP
#undef MIIM_CHECKMARKS
#undef MIIM_DATA
#undef MIIM_FTYPE
#undef MIIM_ID
#undef MIIM_STATE
#undef MIIM_STRING
#undef MIIM_SUBMENU
#undef MIIM_TYPE
enum class
    [[clang::flag_enum]]
    MENU_ITEM_MASK : int
{
    MIIM_BITMAP = 128,
    MIIM_CHECKMARKS = 8,
    MIIM_DATA = 32,
    MIIM_FTYPE = 256,
    MIIM_ID = 2,
    MIIM_STATE = 1,
    MIIM_STRING = 64,
    MIIM_SUBMENU = 4,
    MIIM_TYPE = 16,
};
#pragma pop_macro("MIIM_TYPE")
#pragma pop_macro("MIIM_SUBMENU")
#pragma pop_macro("MIIM_STRING")
#pragma pop_macro("MIIM_STATE")
#pragma pop_macro("MIIM_ID")
#pragma pop_macro("MIIM_FTYPE")
#pragma pop_macro("MIIM_DATA")
#pragma pop_macro("MIIM_CHECKMARKS")
#pragma pop_macro("MIIM_BITMAP")

#pragma push_macro("FLASHW_ALL")
#pragma push_macro("FLASHW_CAPTION")
#pragma push_macro("FLASHW_STOP")
#pragma push_macro("FLASHW_TIMER")
#pragma push_macro("FLASHW_TIMERNOFG")
#pragma push_macro("FLASHW_TRAY")
#undef FLASHW_ALL
#undef FLASHW_CAPTION
#undef FLASHW_STOP
#undef FLASHW_TIMER
#undef FLASHW_TIMERNOFG
#undef FLASHW_TRAY
enum class
    [[clang::flag_enum]]
    FLASHWINFO_FLAGS : int
{
    FLASHW_ALL = 3,
    FLASHW_CAPTION = 1,
    FLASHW_STOP = 0,
    FLASHW_TIMER = 4,
    FLASHW_TIMERNOFG = 12,
    FLASHW_TRAY = 2,
};
#pragma pop_macro("FLASHW_TRAY")
#pragma pop_macro("FLASHW_TIMERNOFG")
#pragma pop_macro("FLASHW_TIMER")
#pragma pop_macro("FLASHW_STOP")
#pragma pop_macro("FLASHW_CAPTION")
#pragma pop_macro("FLASHW_ALL")

#pragma push_macro("CURSOR_SHOWING")
#pragma push_macro("CURSOR_SUPPRESSED")
#undef CURSOR_SHOWING
#undef CURSOR_SUPPRESSED
enum class
    CURSORINFO_FLAGS : int
{
    CURSOR_SHOWING = 1,
    CURSOR_SUPPRESSED = 2,
};
#pragma pop_macro("CURSOR_SUPPRESSED")
#pragma pop_macro("CURSOR_SHOWING")

#pragma push_macro("RIDEV_REMOVE")
#pragma push_macro("RIDEV_EXCLUDE")
#pragma push_macro("RIDEV_PAGEONLY")
#pragma push_macro("RIDEV_NOLEGACY")
#pragma push_macro("RIDEV_INPUTSINK")
#pragma push_macro("RIDEV_CAPTUREMOUSE")
#pragma push_macro("RIDEV_NOHOTKEYS")
#pragma push_macro("RIDEV_APPKEYS")
#pragma push_macro("RIDEV_EXINPUTSINK")
#pragma push_macro("RIDEV_DEVNOTIFY")
#undef RIDEV_REMOVE
#undef RIDEV_EXCLUDE
#undef RIDEV_PAGEONLY
#undef RIDEV_NOLEGACY
#undef RIDEV_INPUTSINK
#undef RIDEV_CAPTUREMOUSE
#undef RIDEV_NOHOTKEYS
#undef RIDEV_APPKEYS
#undef RIDEV_EXINPUTSINK
#undef RIDEV_DEVNOTIFY
enum class
    [[clang::flag_enum]]
    RAWINPUTDEVICE_FLAGS : int
{
    RIDEV_REMOVE = 1,
    RIDEV_EXCLUDE = 16,
    RIDEV_PAGEONLY = 32,
    RIDEV_NOLEGACY = 48,
    RIDEV_INPUTSINK = 256,
    RIDEV_CAPTUREMOUSE = 512,
    RIDEV_NOHOTKEYS = 512,
    RIDEV_APPKEYS = 1024,
    RIDEV_EXINPUTSINK = 4096,
    RIDEV_DEVNOTIFY = 8192,
};
#pragma pop_macro("RIDEV_DEVNOTIFY")
#pragma pop_macro("RIDEV_EXINPUTSINK")
#pragma pop_macro("RIDEV_APPKEYS")
#pragma pop_macro("RIDEV_NOHOTKEYS")
#pragma pop_macro("RIDEV_CAPTUREMOUSE")
#pragma pop_macro("RIDEV_INPUTSINK")
#pragma pop_macro("RIDEV_NOLEGACY")
#pragma pop_macro("RIDEV_PAGEONLY")
#pragma pop_macro("RIDEV_EXCLUDE")
#pragma pop_macro("RIDEV_REMOVE")

#pragma push_macro("INPUT_MOUSE")
#pragma push_macro("INPUT_KEYBOARD")
#pragma push_macro("INPUT_HARDWARE")
#undef INPUT_MOUSE
#undef INPUT_KEYBOARD
#undef INPUT_HARDWARE
enum class
    INPUT_TYPE : int
{
    INPUT_MOUSE = 0,
    INPUT_KEYBOARD = 1,
    INPUT_HARDWARE = 2,
};
#pragma pop_macro("INPUT_HARDWARE")
#pragma pop_macro("INPUT_KEYBOARD")
#pragma pop_macro("INPUT_MOUSE")

#pragma push_macro("MNS_AUTODISMISS")
#pragma push_macro("MNS_CHECKORBMP")
#pragma push_macro("MNS_DRAGDROP")
#pragma push_macro("MNS_MODELESS")
#pragma push_macro("MNS_NOCHECK")
#pragma push_macro("MNS_NOTIFYBYPOS")
#undef MNS_AUTODISMISS
#undef MNS_CHECKORBMP
#undef MNS_DRAGDROP
#undef MNS_MODELESS
#undef MNS_NOCHECK
#undef MNS_NOTIFYBYPOS
enum class
    [[clang::flag_enum]]
    MENUINFO_STYLE : int
{
    MNS_AUTODISMISS = 268435456,
    MNS_CHECKORBMP = 67108864,
    MNS_DRAGDROP = 536870912,
    MNS_MODELESS = 1073741824,
    MNS_NOCHECK = -2147483648,
    MNS_NOTIFYBYPOS = 134217728,
};
#pragma pop_macro("MNS_NOTIFYBYPOS")
#pragma pop_macro("MNS_NOCHECK")
#pragma pop_macro("MNS_MODELESS")
#pragma pop_macro("MNS_DRAGDROP")
#pragma pop_macro("MNS_CHECKORBMP")
#pragma pop_macro("MNS_AUTODISMISS")

#pragma push_macro("WPF_ASYNCWINDOWPLACEMENT")
#pragma push_macro("WPF_RESTORETOMAXIMIZED")
#pragma push_macro("WPF_SETMINPOSITION")
#undef WPF_ASYNCWINDOWPLACEMENT
#undef WPF_RESTORETOMAXIMIZED
#undef WPF_SETMINPOSITION
enum class
    [[clang::flag_enum]]
    WINDOWPLACEMENT_FLAGS : int
{
    WPF_ASYNCWINDOWPLACEMENT = 4,
    WPF_RESTORETOMAXIMIZED = 2,
    WPF_SETMINPOSITION = 1,
};
#pragma pop_macro("WPF_SETMINPOSITION")
#pragma pop_macro("WPF_RESTORETOMAXIMIZED")
#pragma pop_macro("WPF_ASYNCWINDOWPLACEMENT")

#pragma push_macro("MIM_APPLYTOSUBMENUS")
#pragma push_macro("MIM_BACKGROUND")
#pragma push_macro("MIM_HELPID")
#pragma push_macro("MIM_MAXHEIGHT")
#pragma push_macro("MIM_MENUDATA")
#pragma push_macro("MIM_STYLE")
#undef MIM_APPLYTOSUBMENUS
#undef MIM_BACKGROUND
#undef MIM_HELPID
#undef MIM_MAXHEIGHT
#undef MIM_MENUDATA
#undef MIM_STYLE
enum class
    [[clang::flag_enum]]
    MENUINFO_MASK : int
{
    MIM_APPLYTOSUBMENUS = -2147483648,
    MIM_BACKGROUND = 2,
    MIM_HELPID = 4,
    MIM_MAXHEIGHT = 1,
    MIM_MENUDATA = 8,
    MIM_STYLE = 16,
};
#pragma pop_macro("MIM_STYLE")
#pragma pop_macro("MIM_MENUDATA")
#pragma pop_macro("MIM_MAXHEIGHT")
#pragma pop_macro("MIM_HELPID")
#pragma pop_macro("MIM_BACKGROUND")
#pragma pop_macro("MIM_APPLYTOSUBMENUS")

#pragma push_macro("ODT_BUTTON")
#pragma push_macro("ODT_COMBOBOX")
#pragma push_macro("ODT_LISTBOX")
#pragma push_macro("ODT_LISTVIEW")
#pragma push_macro("ODT_MENU")
#pragma push_macro("ODT_STATIC")
#pragma push_macro("ODT_TAB")
#undef ODT_BUTTON
#undef ODT_COMBOBOX
#undef ODT_LISTBOX
#undef ODT_LISTVIEW
#undef ODT_MENU
#undef ODT_STATIC
#undef ODT_TAB
enum class
    DRAWITEMSTRUCT_CTL_TYPE : int
{
    ODT_BUTTON = 4,
    ODT_COMBOBOX = 3,
    ODT_LISTBOX = 2,
    ODT_LISTVIEW = 102,
    ODT_MENU = 1,
    ODT_STATIC = 5,
    ODT_TAB = 101,
};
#pragma pop_macro("ODT_TAB")
#pragma pop_macro("ODT_STATIC")
#pragma pop_macro("ODT_MENU")
#pragma pop_macro("ODT_LISTVIEW")
#pragma pop_macro("ODT_LISTBOX")
#pragma pop_macro("ODT_COMBOBOX")
#pragma pop_macro("ODT_BUTTON")

#pragma push_macro("SSWF_CUSTOM")
#pragma push_macro("SSWF_DISPLAY")
#pragma push_macro("SSWF_NONE")
#pragma push_macro("SSWF_TITLE")
#pragma push_macro("SSWF_WINDOW")
#undef SSWF_CUSTOM
#undef SSWF_DISPLAY
#undef SSWF_NONE
#undef SSWF_TITLE
#undef SSWF_WINDOW
enum class
    SOUNDSENTRY_WINDOWS_EFFECT : int
{
    SSWF_CUSTOM = 4,
    SSWF_DISPLAY = 3,
    SSWF_NONE = 0,
    SSWF_TITLE = 1,
    SSWF_WINDOW = 2,
};
#pragma pop_macro("SSWF_WINDOW")
#pragma pop_macro("SSWF_TITLE")
#pragma pop_macro("SSWF_NONE")
#pragma pop_macro("SSWF_DISPLAY")
#pragma pop_macro("SSWF_CUSTOM")

#pragma push_macro("ARW_BOTTOMLEFT")
#pragma push_macro("ARW_BOTTOMRIGHT")
#pragma push_macro("ARW_TOPLEFT")
#pragma push_macro("ARW_TOPRIGHT")
#undef ARW_BOTTOMLEFT
#undef ARW_BOTTOMRIGHT
#undef ARW_TOPLEFT
#undef ARW_TOPRIGHT
enum class
    MINIMIZEDMETRICS_ARRANGE : int
{
    ARW_BOTTOMLEFT = 0,
    ARW_BOTTOMRIGHT = 1,
    ARW_TOPLEFT = 2,
    ARW_TOPRIGHT = 3,
};
#pragma pop_macro("ARW_TOPRIGHT")
#pragma pop_macro("ARW_TOPLEFT")
#pragma pop_macro("ARW_BOTTOMRIGHT")
#pragma pop_macro("ARW_BOTTOMLEFT")

#pragma push_macro("SIF_ALL")
#pragma push_macro("SIF_DISABLENOSCROLL")
#pragma push_macro("SIF_PAGE")
#pragma push_macro("SIF_POS")
#pragma push_macro("SIF_RANGE")
#pragma push_macro("SIF_TRACKPOS")
#undef SIF_ALL
#undef SIF_DISABLENOSCROLL
#undef SIF_PAGE
#undef SIF_POS
#undef SIF_RANGE
#undef SIF_TRACKPOS
enum class
    [[clang::flag_enum]]
    SCROLLINFO_MASK : int
{
    SIF_ALL = 23,
    SIF_DISABLENOSCROLL = 8,
    SIF_PAGE = 2,
    SIF_POS = 4,
    SIF_RANGE = 1,
    SIF_TRACKPOS = 16,
};
#pragma pop_macro("SIF_TRACKPOS")
#pragma pop_macro("SIF_RANGE")
#pragma pop_macro("SIF_POS")
#pragma pop_macro("SIF_PAGE")
#pragma pop_macro("SIF_DISABLENOSCROLL")
#pragma pop_macro("SIF_ALL")

#pragma push_macro("MNGOF_BOTTOMGAP")
#pragma push_macro("MNGOF_TOPGAP")
#undef MNGOF_BOTTOMGAP
#undef MNGOF_TOPGAP
enum class
    MENUGETOBJECTINFO_FLAGS : int
{
    MNGOF_BOTTOMGAP = 2,
    MNGOF_TOPGAP = 1,
};
#pragma pop_macro("MNGOF_TOPGAP")
#pragma pop_macro("MNGOF_BOTTOMGAP")

#pragma push_macro("GUI_CARETBLINKING")
#pragma push_macro("GUI_INMENUMODE")
#pragma push_macro("GUI_INMOVESIZE")
#pragma push_macro("GUI_POPUPMENUMODE")
#pragma push_macro("GUI_SYSTEMMENUMODE")
#undef GUI_CARETBLINKING
#undef GUI_INMENUMODE
#undef GUI_INMOVESIZE
#undef GUI_POPUPMENUMODE
#undef GUI_SYSTEMMENUMODE
enum class
    [[clang::flag_enum]]
    GUITHREADINFO_FLAGS : int
{
    GUI_CARETBLINKING = 1,
    GUI_INMENUMODE = 4,
    GUI_INMOVESIZE = 2,
    GUI_POPUPMENUMODE = 16,
    GUI_SYSTEMMENUMODE = 8,
};
#pragma pop_macro("GUI_SYSTEMMENUMODE")
#pragma pop_macro("GUI_POPUPMENUMODE")
#pragma pop_macro("GUI_INMOVESIZE")
#pragma pop_macro("GUI_INMENUMODE")
#pragma pop_macro("GUI_CARETBLINKING")

#pragma push_macro("VK_LBUTTON")
#pragma push_macro("VK_RBUTTON")
#pragma push_macro("VK_CANCEL")
#pragma push_macro("VK_MBUTTON")
#pragma push_macro("VK_XBUTTON1")
#pragma push_macro("VK_XBUTTON2")
#pragma push_macro("VK_BACK")
#pragma push_macro("VK_TAB")
#pragma push_macro("VK_CLEAR")
#pragma push_macro("VK_RETURN")
#pragma push_macro("VK_SHIFT")
#pragma push_macro("VK_CONTROL")
#pragma push_macro("VK_MENU")
#pragma push_macro("VK_PAUSE")
#pragma push_macro("VK_CAPITAL")
#pragma push_macro("VK_KANA")
#pragma push_macro("VK_HANGEUL")
#pragma push_macro("VK_HANGUL")
#pragma push_macro("VK_IME_ON")
#pragma push_macro("VK_JUNJA")
#pragma push_macro("VK_FINAL")
#pragma push_macro("VK_HANJA")
#pragma push_macro("VK_KANJI")
#pragma push_macro("VK_IME_OFF")
#pragma push_macro("VK_ESCAPE")
#pragma push_macro("VK_CONVERT")
#pragma push_macro("VK_NONCONVERT")
#pragma push_macro("VK_ACCEPT")
#pragma push_macro("VK_MODECHANGE")
#pragma push_macro("VK_SPACE")
#pragma push_macro("VK_PRIOR")
#pragma push_macro("VK_NEXT")
#pragma push_macro("VK_END")
#pragma push_macro("VK_HOME")
#pragma push_macro("VK_LEFT")
#pragma push_macro("VK_UP")
#pragma push_macro("VK_RIGHT")
#pragma push_macro("VK_DOWN")
#pragma push_macro("VK_SELECT")
#pragma push_macro("VK_PRINT")
#pragma push_macro("VK_EXECUTE")
#pragma push_macro("VK_SNAPSHOT")
#pragma push_macro("VK_INSERT")
#pragma push_macro("VK_DELETE")
#pragma push_macro("VK_HELP")
#pragma push_macro("VK_LWIN")
#pragma push_macro("VK_RWIN")
#pragma push_macro("VK_APPS")
#pragma push_macro("VK_SLEEP")
#pragma push_macro("VK_NUMPAD0")
#pragma push_macro("VK_NUMPAD1")
#pragma push_macro("VK_NUMPAD2")
#pragma push_macro("VK_NUMPAD3")
#pragma push_macro("VK_NUMPAD4")
#pragma push_macro("VK_NUMPAD5")
#pragma push_macro("VK_NUMPAD6")
#pragma push_macro("VK_NUMPAD7")
#pragma push_macro("VK_NUMPAD8")
#pragma push_macro("VK_NUMPAD9")
#pragma push_macro("VK_MULTIPLY")
#pragma push_macro("VK_ADD")
#pragma push_macro("VK_SEPARATOR")
#pragma push_macro("VK_SUBTRACT")
#pragma push_macro("VK_DECIMAL")
#pragma push_macro("VK_DIVIDE")
#pragma push_macro("VK_F1")
#pragma push_macro("VK_F2")
#pragma push_macro("VK_F3")
#pragma push_macro("VK_F4")
#pragma push_macro("VK_F5")
#pragma push_macro("VK_F6")
#pragma push_macro("VK_F7")
#pragma push_macro("VK_F8")
#pragma push_macro("VK_F9")
#pragma push_macro("VK_F10")
#pragma push_macro("VK_F11")
#pragma push_macro("VK_F12")
#pragma push_macro("VK_F13")
#pragma push_macro("VK_F14")
#pragma push_macro("VK_F15")
#pragma push_macro("VK_F16")
#pragma push_macro("VK_F17")
#pragma push_macro("VK_F18")
#pragma push_macro("VK_F19")
#pragma push_macro("VK_F20")
#pragma push_macro("VK_F21")
#pragma push_macro("VK_F22")
#pragma push_macro("VK_F23")
#pragma push_macro("VK_F24")
#pragma push_macro("VK_NAVIGATION_VIEW")
#pragma push_macro("VK_NAVIGATION_MENU")
#pragma push_macro("VK_NAVIGATION_UP")
#pragma push_macro("VK_NAVIGATION_DOWN")
#pragma push_macro("VK_NAVIGATION_LEFT")
#pragma push_macro("VK_NAVIGATION_RIGHT")
#pragma push_macro("VK_NAVIGATION_ACCEPT")
#pragma push_macro("VK_NAVIGATION_CANCEL")
#pragma push_macro("VK_NUMLOCK")
#pragma push_macro("VK_SCROLL")
#pragma push_macro("VK_OEM_NEC_EQUAL")
#pragma push_macro("VK_OEM_FJ_JISHO")
#pragma push_macro("VK_OEM_FJ_MASSHOU")
#pragma push_macro("VK_OEM_FJ_TOUROKU")
#pragma push_macro("VK_OEM_FJ_LOYA")
#pragma push_macro("VK_OEM_FJ_ROYA")
#pragma push_macro("VK_LSHIFT")
#pragma push_macro("VK_RSHIFT")
#pragma push_macro("VK_LCONTROL")
#pragma push_macro("VK_RCONTROL")
#pragma push_macro("VK_LMENU")
#pragma push_macro("VK_RMENU")
#pragma push_macro("VK_BROWSER_BACK")
#pragma push_macro("VK_BROWSER_FORWARD")
#pragma push_macro("VK_BROWSER_REFRESH")
#pragma push_macro("VK_BROWSER_STOP")
#pragma push_macro("VK_BROWSER_SEARCH")
#pragma push_macro("VK_BROWSER_FAVORITES")
#pragma push_macro("VK_BROWSER_HOME")
#pragma push_macro("VK_VOLUME_MUTE")
#pragma push_macro("VK_VOLUME_DOWN")
#pragma push_macro("VK_VOLUME_UP")
#pragma push_macro("VK_MEDIA_NEXT_TRACK")
#pragma push_macro("VK_MEDIA_PREV_TRACK")
#pragma push_macro("VK_MEDIA_STOP")
#pragma push_macro("VK_MEDIA_PLAY_PAUSE")
#pragma push_macro("VK_LAUNCH_MAIL")
#pragma push_macro("VK_LAUNCH_MEDIA_SELECT")
#pragma push_macro("VK_LAUNCH_APP1")
#pragma push_macro("VK_LAUNCH_APP2")
#pragma push_macro("VK_OEM_1")
#pragma push_macro("VK_OEM_PLUS")
#pragma push_macro("VK_OEM_COMMA")
#pragma push_macro("VK_OEM_MINUS")
#pragma push_macro("VK_OEM_PERIOD")
#pragma push_macro("VK_OEM_2")
#pragma push_macro("VK_OEM_3")
#pragma push_macro("VK_GAMEPAD_A")
#pragma push_macro("VK_GAMEPAD_B")
#pragma push_macro("VK_GAMEPAD_X")
#pragma push_macro("VK_GAMEPAD_Y")
#pragma push_macro("VK_GAMEPAD_RIGHT_SHOULDER")
#pragma push_macro("VK_GAMEPAD_LEFT_SHOULDER")
#pragma push_macro("VK_GAMEPAD_LEFT_TRIGGER")
#pragma push_macro("VK_GAMEPAD_RIGHT_TRIGGER")
#pragma push_macro("VK_GAMEPAD_DPAD_UP")
#pragma push_macro("VK_GAMEPAD_DPAD_DOWN")
#pragma push_macro("VK_GAMEPAD_DPAD_LEFT")
#pragma push_macro("VK_GAMEPAD_DPAD_RIGHT")
#pragma push_macro("VK_GAMEPAD_MENU")
#pragma push_macro("VK_GAMEPAD_VIEW")
#pragma push_macro("VK_GAMEPAD_LEFT_THUMBSTICK_BUTTON")
#pragma push_macro("VK_GAMEPAD_RIGHT_THUMBSTICK_BUTTON")
#pragma push_macro("VK_GAMEPAD_LEFT_THUMBSTICK_UP")
#pragma push_macro("VK_GAMEPAD_LEFT_THUMBSTICK_DOWN")
#pragma push_macro("VK_GAMEPAD_LEFT_THUMBSTICK_RIGHT")
#pragma push_macro("VK_GAMEPAD_LEFT_THUMBSTICK_LEFT")
#pragma push_macro("VK_GAMEPAD_RIGHT_THUMBSTICK_UP")
#pragma push_macro("VK_GAMEPAD_RIGHT_THUMBSTICK_DOWN")
#pragma push_macro("VK_GAMEPAD_RIGHT_THUMBSTICK_RIGHT")
#pragma push_macro("VK_GAMEPAD_RIGHT_THUMBSTICK_LEFT")
#pragma push_macro("VK_OEM_4")
#pragma push_macro("VK_OEM_5")
#pragma push_macro("VK_OEM_6")
#pragma push_macro("VK_OEM_7")
#pragma push_macro("VK_OEM_8")
#pragma push_macro("VK_OEM_AX")
#pragma push_macro("VK_OEM_102")
#pragma push_macro("VK_ICO_HELP")
#pragma push_macro("VK_ICO_00")
#pragma push_macro("VK_PROCESSKEY")
#pragma push_macro("VK_ICO_CLEAR")
#pragma push_macro("VK_PACKET")
#pragma push_macro("VK_OEM_RESET")
#pragma push_macro("VK_OEM_JUMP")
#pragma push_macro("VK_OEM_PA1")
#pragma push_macro("VK_OEM_PA2")
#pragma push_macro("VK_OEM_PA3")
#pragma push_macro("VK_OEM_WSCTRL")
#pragma push_macro("VK_OEM_CUSEL")
#pragma push_macro("VK_OEM_ATTN")
#pragma push_macro("VK_OEM_FINISH")
#pragma push_macro("VK_OEM_COPY")
#pragma push_macro("VK_OEM_AUTO")
#pragma push_macro("VK_OEM_ENLW")
#pragma push_macro("VK_OEM_BACKTAB")
#pragma push_macro("VK_ATTN")
#pragma push_macro("VK_CRSEL")
#pragma push_macro("VK_EXSEL")
#pragma push_macro("VK_EREOF")
#pragma push_macro("VK_PLAY")
#pragma push_macro("VK_ZOOM")
#pragma push_macro("VK_NONAME")
#pragma push_macro("VK_PA1")
#pragma push_macro("VK_OEM_CLEAR")
#pragma push_macro("VK_0")
#pragma push_macro("VK_1")
#pragma push_macro("VK_2")
#pragma push_macro("VK_3")
#pragma push_macro("VK_4")
#pragma push_macro("VK_5")
#pragma push_macro("VK_6")
#pragma push_macro("VK_7")
#pragma push_macro("VK_8")
#pragma push_macro("VK_9")
#pragma push_macro("VK_A")
#pragma push_macro("VK_B")
#pragma push_macro("VK_C")
#pragma push_macro("VK_D")
#pragma push_macro("VK_E")
#pragma push_macro("VK_F")
#pragma push_macro("VK_G")
#pragma push_macro("VK_H")
#pragma push_macro("VK_I")
#pragma push_macro("VK_J")
#pragma push_macro("VK_K")
#pragma push_macro("VK_L")
#pragma push_macro("VK_M")
#pragma push_macro("VK_N")
#pragma push_macro("VK_O")
#pragma push_macro("VK_P")
#pragma push_macro("VK_Q")
#pragma push_macro("VK_R")
#pragma push_macro("VK_S")
#pragma push_macro("VK_T")
#pragma push_macro("VK_U")
#pragma push_macro("VK_V")
#pragma push_macro("VK_W")
#pragma push_macro("VK_X")
#pragma push_macro("VK_Y")
#pragma push_macro("VK_Z")
#pragma push_macro("VK_ABNT_C1")
#pragma push_macro("VK_ABNT_C2")
#pragma push_macro("VK_DBE_ALPHANUMERIC")
#pragma push_macro("VK_DBE_CODEINPUT")
#pragma push_macro("VK_DBE_DBCSCHAR")
#pragma push_macro("VK_DBE_DETERMINESTRING")
#pragma push_macro("VK_DBE_ENTERDLGCONVERSIONMODE")
#pragma push_macro("VK_DBE_ENTERIMECONFIGMODE")
#pragma push_macro("VK_DBE_ENTERWORDREGISTERMODE")
#pragma push_macro("VK_DBE_FLUSHSTRING")
#pragma push_macro("VK_DBE_HIRAGANA")
#pragma push_macro("VK_DBE_KATAKANA")
#pragma push_macro("VK_DBE_NOCODEINPUT")
#pragma push_macro("VK_DBE_NOROMAN")
#pragma push_macro("VK_DBE_ROMAN")
#pragma push_macro("VK_DBE_SBCSCHAR")
#pragma push_macro("VK__none_")
#undef VK_LBUTTON
#undef VK_RBUTTON
#undef VK_CANCEL
#undef VK_MBUTTON
#undef VK_XBUTTON1
#undef VK_XBUTTON2
#undef VK_BACK
#undef VK_TAB
#undef VK_CLEAR
#undef VK_RETURN
#undef VK_SHIFT
#undef VK_CONTROL
#undef VK_MENU
#undef VK_PAUSE
#undef VK_CAPITAL
#undef VK_KANA
#undef VK_HANGEUL
#undef VK_HANGUL
#undef VK_IME_ON
#undef VK_JUNJA
#undef VK_FINAL
#undef VK_HANJA
#undef VK_KANJI
#undef VK_IME_OFF
#undef VK_ESCAPE
#undef VK_CONVERT
#undef VK_NONCONVERT
#undef VK_ACCEPT
#undef VK_MODECHANGE
#undef VK_SPACE
#undef VK_PRIOR
#undef VK_NEXT
#undef VK_END
#undef VK_HOME
#undef VK_LEFT
#undef VK_UP
#undef VK_RIGHT
#undef VK_DOWN
#undef VK_SELECT
#undef VK_PRINT
#undef VK_EXECUTE
#undef VK_SNAPSHOT
#undef VK_INSERT
#undef VK_DELETE
#undef VK_HELP
#undef VK_LWIN
#undef VK_RWIN
#undef VK_APPS
#undef VK_SLEEP
#undef VK_NUMPAD0
#undef VK_NUMPAD1
#undef VK_NUMPAD2
#undef VK_NUMPAD3
#undef VK_NUMPAD4
#undef VK_NUMPAD5
#undef VK_NUMPAD6
#undef VK_NUMPAD7
#undef VK_NUMPAD8
#undef VK_NUMPAD9
#undef VK_MULTIPLY
#undef VK_ADD
#undef VK_SEPARATOR
#undef VK_SUBTRACT
#undef VK_DECIMAL
#undef VK_DIVIDE
#undef VK_F1
#undef VK_F2
#undef VK_F3
#undef VK_F4
#undef VK_F5
#undef VK_F6
#undef VK_F7
#undef VK_F8
#undef VK_F9
#undef VK_F10
#undef VK_F11
#undef VK_F12
#undef VK_F13
#undef VK_F14
#undef VK_F15
#undef VK_F16
#undef VK_F17
#undef VK_F18
#undef VK_F19
#undef VK_F20
#undef VK_F21
#undef VK_F22
#undef VK_F23
#undef VK_F24
#undef VK_NAVIGATION_VIEW
#undef VK_NAVIGATION_MENU
#undef VK_NAVIGATION_UP
#undef VK_NAVIGATION_DOWN
#undef VK_NAVIGATION_LEFT
#undef VK_NAVIGATION_RIGHT
#undef VK_NAVIGATION_ACCEPT
#undef VK_NAVIGATION_CANCEL
#undef VK_NUMLOCK
#undef VK_SCROLL
#undef VK_OEM_NEC_EQUAL
#undef VK_OEM_FJ_JISHO
#undef VK_OEM_FJ_MASSHOU
#undef VK_OEM_FJ_TOUROKU
#undef VK_OEM_FJ_LOYA
#undef VK_OEM_FJ_ROYA
#undef VK_LSHIFT
#undef VK_RSHIFT
#undef VK_LCONTROL
#undef VK_RCONTROL
#undef VK_LMENU
#undef VK_RMENU
#undef VK_BROWSER_BACK
#undef VK_BROWSER_FORWARD
#undef VK_BROWSER_REFRESH
#undef VK_BROWSER_STOP
#undef VK_BROWSER_SEARCH
#undef VK_BROWSER_FAVORITES
#undef VK_BROWSER_HOME
#undef VK_VOLUME_MUTE
#undef VK_VOLUME_DOWN
#undef VK_VOLUME_UP
#undef VK_MEDIA_NEXT_TRACK
#undef VK_MEDIA_PREV_TRACK
#undef VK_MEDIA_STOP
#undef VK_MEDIA_PLAY_PAUSE
#undef VK_LAUNCH_MAIL
#undef VK_LAUNCH_MEDIA_SELECT
#undef VK_LAUNCH_APP1
#undef VK_LAUNCH_APP2
#undef VK_OEM_1
#undef VK_OEM_PLUS
#undef VK_OEM_COMMA
#undef VK_OEM_MINUS
#undef VK_OEM_PERIOD
#undef VK_OEM_2
#undef VK_OEM_3
#undef VK_GAMEPAD_A
#undef VK_GAMEPAD_B
#undef VK_GAMEPAD_X
#undef VK_GAMEPAD_Y
#undef VK_GAMEPAD_RIGHT_SHOULDER
#undef VK_GAMEPAD_LEFT_SHOULDER
#undef VK_GAMEPAD_LEFT_TRIGGER
#undef VK_GAMEPAD_RIGHT_TRIGGER
#undef VK_GAMEPAD_DPAD_UP
#undef VK_GAMEPAD_DPAD_DOWN
#undef VK_GAMEPAD_DPAD_LEFT
#undef VK_GAMEPAD_DPAD_RIGHT
#undef VK_GAMEPAD_MENU
#undef VK_GAMEPAD_VIEW
#undef VK_GAMEPAD_LEFT_THUMBSTICK_BUTTON
#undef VK_GAMEPAD_RIGHT_THUMBSTICK_BUTTON
#undef VK_GAMEPAD_LEFT_THUMBSTICK_UP
#undef VK_GAMEPAD_LEFT_THUMBSTICK_DOWN
#undef VK_GAMEPAD_LEFT_THUMBSTICK_RIGHT
#undef VK_GAMEPAD_LEFT_THUMBSTICK_LEFT
#undef VK_GAMEPAD_RIGHT_THUMBSTICK_UP
#undef VK_GAMEPAD_RIGHT_THUMBSTICK_DOWN
#undef VK_GAMEPAD_RIGHT_THUMBSTICK_RIGHT
#undef VK_GAMEPAD_RIGHT_THUMBSTICK_LEFT
#undef VK_OEM_4
#undef VK_OEM_5
#undef VK_OEM_6
#undef VK_OEM_7
#undef VK_OEM_8
#undef VK_OEM_AX
#undef VK_OEM_102
#undef VK_ICO_HELP
#undef VK_ICO_00
#undef VK_PROCESSKEY
#undef VK_ICO_CLEAR
#undef VK_PACKET
#undef VK_OEM_RESET
#undef VK_OEM_JUMP
#undef VK_OEM_PA1
#undef VK_OEM_PA2
#undef VK_OEM_PA3
#undef VK_OEM_WSCTRL
#undef VK_OEM_CUSEL
#undef VK_OEM_ATTN
#undef VK_OEM_FINISH
#undef VK_OEM_COPY
#undef VK_OEM_AUTO
#undef VK_OEM_ENLW
#undef VK_OEM_BACKTAB
#undef VK_ATTN
#undef VK_CRSEL
#undef VK_EXSEL
#undef VK_EREOF
#undef VK_PLAY
#undef VK_ZOOM
#undef VK_NONAME
#undef VK_PA1
#undef VK_OEM_CLEAR
#undef VK_0
#undef VK_1
#undef VK_2
#undef VK_3
#undef VK_4
#undef VK_5
#undef VK_6
#undef VK_7
#undef VK_8
#undef VK_9
#undef VK_A
#undef VK_B
#undef VK_C
#undef VK_D
#undef VK_E
#undef VK_F
#undef VK_G
#undef VK_H
#undef VK_I
#undef VK_J
#undef VK_K
#undef VK_L
#undef VK_M
#undef VK_N
#undef VK_O
#undef VK_P
#undef VK_Q
#undef VK_R
#undef VK_S
#undef VK_T
#undef VK_U
#undef VK_V
#undef VK_W
#undef VK_X
#undef VK_Y
#undef VK_Z
#undef VK_ABNT_C1
#undef VK_ABNT_C2
#undef VK_DBE_ALPHANUMERIC
#undef VK_DBE_CODEINPUT
#undef VK_DBE_DBCSCHAR
#undef VK_DBE_DETERMINESTRING
#undef VK_DBE_ENTERDLGCONVERSIONMODE
#undef VK_DBE_ENTERIMECONFIGMODE
#undef VK_DBE_ENTERWORDREGISTERMODE
#undef VK_DBE_FLUSHSTRING
#undef VK_DBE_HIRAGANA
#undef VK_DBE_KATAKANA
#undef VK_DBE_NOCODEINPUT
#undef VK_DBE_NOROMAN
#undef VK_DBE_ROMAN
#undef VK_DBE_SBCSCHAR
#undef VK__none_
enum class
    VIRTUAL_KEY : unsigned short
{
    VK_LBUTTON = 0x0001,
    VK_RBUTTON = 0x0002,
    VK_CANCEL = 0x0003,
    VK_MBUTTON = 0x0004,
    VK_XBUTTON1 = 0x0005,
    VK_XBUTTON2 = 0x0006,
    VK_BACK = 0x0008,
    VK_TAB = 0x0009,
    VK_CLEAR = 0x000c,
    VK_RETURN = 0x000d,
    VK_SHIFT = 0x0010,
    VK_CONTROL = 0x0011,
    VK_MENU = 0x0012,
    VK_PAUSE = 0x0013,
    VK_CAPITAL = 0x0014,
    VK_KANA = 0x0015,
    VK_HANGEUL = 0x0015,
    VK_HANGUL = 0x0015,
    VK_IME_ON = 0x0016,
    VK_JUNJA = 0x0017,
    VK_FINAL = 0x0018,
    VK_HANJA = 0x0019,
    VK_KANJI = 0x0019,
    VK_IME_OFF = 0x001a,
    VK_ESCAPE = 0x001b,
    VK_CONVERT = 0x001c,
    VK_NONCONVERT = 0x001d,
    VK_ACCEPT = 0x001e,
    VK_MODECHANGE = 0x001f,
    VK_SPACE = 0x0020,
    VK_PRIOR = 0x0021,
    VK_NEXT = 0x0022,
    VK_END = 0x0023,
    VK_HOME = 0x0024,
    VK_LEFT = 0x0025,
    VK_UP = 0x0026,
    VK_RIGHT = 0x0027,
    VK_DOWN = 0x0028,
    VK_SELECT = 0x0029,
    VK_PRINT = 0x002a,
    VK_EXECUTE = 0x002b,
    VK_SNAPSHOT = 0x002c,
    VK_INSERT = 0x002d,
    VK_DELETE = 0x002e,
    VK_HELP = 0x002f,
    VK_LWIN = 0x005b,
    VK_RWIN = 0x005c,
    VK_APPS = 0x005d,
    VK_SLEEP = 0x005f,
    VK_NUMPAD0 = 0x0060,
    VK_NUMPAD1 = 0x0061,
    VK_NUMPAD2 = 0x0062,
    VK_NUMPAD3 = 0x0063,
    VK_NUMPAD4 = 0x0064,
    VK_NUMPAD5 = 0x0065,
    VK_NUMPAD6 = 0x0066,
    VK_NUMPAD7 = 0x0067,
    VK_NUMPAD8 = 0x0068,
    VK_NUMPAD9 = 0x0069,
    VK_MULTIPLY = 0x006a,
    VK_ADD = 0x006b,
    VK_SEPARATOR = 0x006c,
    VK_SUBTRACT = 0x006d,
    VK_DECIMAL = 0x006e,
    VK_DIVIDE = 0x006f,
    VK_F1 = 0x0070,
    VK_F2 = 0x0071,
    VK_F3 = 0x0072,
    VK_F4 = 0x0073,
    VK_F5 = 0x0074,
    VK_F6 = 0x0075,
    VK_F7 = 0x0076,
    VK_F8 = 0x0077,
    VK_F9 = 0x0078,
    VK_F10 = 0x0079,
    VK_F11 = 0x007a,
    VK_F12 = 0x007b,
    VK_F13 = 0x007c,
    VK_F14 = 0x007d,
    VK_F15 = 0x007e,
    VK_F16 = 0x007f,
    VK_F17 = 0x0080,
    VK_F18 = 0x0081,
    VK_F19 = 0x0082,
    VK_F20 = 0x0083,
    VK_F21 = 0x0084,
    VK_F22 = 0x0085,
    VK_F23 = 0x0086,
    VK_F24 = 0x0087,
    VK_NAVIGATION_VIEW = 0x0088,
    VK_NAVIGATION_MENU = 0x0089,
    VK_NAVIGATION_UP = 0x008a,
    VK_NAVIGATION_DOWN = 0x008b,
    VK_NAVIGATION_LEFT = 0x008c,
    VK_NAVIGATION_RIGHT = 0x008d,
    VK_NAVIGATION_ACCEPT = 0x008e,
    VK_NAVIGATION_CANCEL = 0x008f,
    VK_NUMLOCK = 0x0090,
    VK_SCROLL = 0x0091,
    VK_OEM_NEC_EQUAL = 0x0092,
    VK_OEM_FJ_JISHO = 0x0092,
    VK_OEM_FJ_MASSHOU = 0x0093,
    VK_OEM_FJ_TOUROKU = 0x0094,
    VK_OEM_FJ_LOYA = 0x0095,
    VK_OEM_FJ_ROYA = 0x0096,
    VK_LSHIFT = 0x00a0,
    VK_RSHIFT = 0x00a1,
    VK_LCONTROL = 0x00a2,
    VK_RCONTROL = 0x00a3,
    VK_LMENU = 0x00a4,
    VK_RMENU = 0x00a5,
    VK_BROWSER_BACK = 0x00a6,
    VK_BROWSER_FORWARD = 0x00a7,
    VK_BROWSER_REFRESH = 0x00a8,
    VK_BROWSER_STOP = 0x00a9,
    VK_BROWSER_SEARCH = 0x00aa,
    VK_BROWSER_FAVORITES = 0x00ab,
    VK_BROWSER_HOME = 0x00ac,
    VK_VOLUME_MUTE = 0x00ad,
    VK_VOLUME_DOWN = 0x00ae,
    VK_VOLUME_UP = 0x00af,
    VK_MEDIA_NEXT_TRACK = 0x00b0,
    VK_MEDIA_PREV_TRACK = 0x00b1,
    VK_MEDIA_STOP = 0x00b2,
    VK_MEDIA_PLAY_PAUSE = 0x00b3,
    VK_LAUNCH_MAIL = 0x00b4,
    VK_LAUNCH_MEDIA_SELECT = 0x00b5,
    VK_LAUNCH_APP1 = 0x00b6,
    VK_LAUNCH_APP2 = 0x00b7,
    VK_OEM_1 = 0x00ba,
    VK_OEM_PLUS = 0x00bb,
    VK_OEM_COMMA = 0x00bc,
    VK_OEM_MINUS = 0x00bd,
    VK_OEM_PERIOD = 0x00be,
    VK_OEM_2 = 0x00bf,
    VK_OEM_3 = 0x00c0,
    VK_GAMEPAD_A = 0x00c3,
    VK_GAMEPAD_B = 0x00c4,
    VK_GAMEPAD_X = 0x00c5,
    VK_GAMEPAD_Y = 0x00c6,
    VK_GAMEPAD_RIGHT_SHOULDER = 0x00c7,
    VK_GAMEPAD_LEFT_SHOULDER = 0x00c8,
    VK_GAMEPAD_LEFT_TRIGGER = 0x00c9,
    VK_GAMEPAD_RIGHT_TRIGGER = 0x00ca,
    VK_GAMEPAD_DPAD_UP = 0x00cb,
    VK_GAMEPAD_DPAD_DOWN = 0x00cc,
    VK_GAMEPAD_DPAD_LEFT = 0x00cd,
    VK_GAMEPAD_DPAD_RIGHT = 0x00ce,
    VK_GAMEPAD_MENU = 0x00cf,
    VK_GAMEPAD_VIEW = 0x00d0,
    VK_GAMEPAD_LEFT_THUMBSTICK_BUTTON = 0x00d1,
    VK_GAMEPAD_RIGHT_THUMBSTICK_BUTTON = 0x00d2,
    VK_GAMEPAD_LEFT_THUMBSTICK_UP = 0x00d3,
    VK_GAMEPAD_LEFT_THUMBSTICK_DOWN = 0x00d4,
    VK_GAMEPAD_LEFT_THUMBSTICK_RIGHT = 0x00d5,
    VK_GAMEPAD_LEFT_THUMBSTICK_LEFT = 0x00d6,
    VK_GAMEPAD_RIGHT_THUMBSTICK_UP = 0x00d7,
    VK_GAMEPAD_RIGHT_THUMBSTICK_DOWN = 0x00d8,
    VK_GAMEPAD_RIGHT_THUMBSTICK_RIGHT = 0x00d9,
    VK_GAMEPAD_RIGHT_THUMBSTICK_LEFT = 0x00da,
    VK_OEM_4 = 0x00db,
    VK_OEM_5 = 0x00dc,
    VK_OEM_6 = 0x00dd,
    VK_OEM_7 = 0x00de,
    VK_OEM_8 = 0x00df,
    VK_OEM_AX = 0x00e1,
    VK_OEM_102 = 0x00e2,
    VK_ICO_HELP = 0x00e3,
    VK_ICO_00 = 0x00e4,
    VK_PROCESSKEY = 0x00e5,
    VK_ICO_CLEAR = 0x00e6,
    VK_PACKET = 0x00e7,
    VK_OEM_RESET = 0x00e9,
    VK_OEM_JUMP = 0x00ea,
    VK_OEM_PA1 = 0x00eb,
    VK_OEM_PA2 = 0x00ec,
    VK_OEM_PA3 = 0x00ed,
    VK_OEM_WSCTRL = 0x00ee,
    VK_OEM_CUSEL = 0x00ef,
    VK_OEM_ATTN = 0x00f0,
    VK_OEM_FINISH = 0x00f1,
    VK_OEM_COPY = 0x00f2,
    VK_OEM_AUTO = 0x00f3,
    VK_OEM_ENLW = 0x00f4,
    VK_OEM_BACKTAB = 0x00f5,
    VK_ATTN = 0x00f6,
    VK_CRSEL = 0x00f7,
    VK_EXSEL = 0x00f8,
    VK_EREOF = 0x00f9,
    VK_PLAY = 0x00fa,
    VK_ZOOM = 0x00fb,
    VK_NONAME = 0x00fc,
    VK_PA1 = 0x00fd,
    VK_OEM_CLEAR = 0x00fe,
    VK_0 = 0x0030,
    VK_1 = 0x0031,
    VK_2 = 0x0032,
    VK_3 = 0x0033,
    VK_4 = 0x0034,
    VK_5 = 0x0035,
    VK_6 = 0x0036,
    VK_7 = 0x0037,
    VK_8 = 0x0038,
    VK_9 = 0x0039,
    VK_A = 0x0041,
    VK_B = 0x0042,
    VK_C = 0x0043,
    VK_D = 0x0044,
    VK_E = 0x0045,
    VK_F = 0x0046,
    VK_G = 0x0047,
    VK_H = 0x0048,
    VK_I = 0x0049,
    VK_J = 0x004a,
    VK_K = 0x004b,
    VK_L = 0x004c,
    VK_M = 0x004d,
    VK_N = 0x004e,
    VK_O = 0x004f,
    VK_P = 0x0050,
    VK_Q = 0x0051,
    VK_R = 0x0052,
    VK_S = 0x0053,
    VK_T = 0x0054,
    VK_U = 0x0055,
    VK_V = 0x0056,
    VK_W = 0x0057,
    VK_X = 0x0058,
    VK_Y = 0x0059,
    VK_Z = 0x005a,
    VK_ABNT_C1 = 0x00c1,
    VK_ABNT_C2 = 0x00c2,
    VK_DBE_ALPHANUMERIC = 0x00f0,
    VK_DBE_CODEINPUT = 0x00fa,
    VK_DBE_DBCSCHAR = 0x00f4,
    VK_DBE_DETERMINESTRING = 0x00fc,
    VK_DBE_ENTERDLGCONVERSIONMODE = 0x00fd,
    VK_DBE_ENTERIMECONFIGMODE = 0x00f8,
    VK_DBE_ENTERWORDREGISTERMODE = 0x00f7,
    VK_DBE_FLUSHSTRING = 0x00f9,
    VK_DBE_HIRAGANA = 0x00f2,
    VK_DBE_KATAKANA = 0x00f1,
    VK_DBE_NOCODEINPUT = 0x00fb,
    VK_DBE_NOROMAN = 0x00f6,
    VK_DBE_ROMAN = 0x00f5,
    VK_DBE_SBCSCHAR = 0x00f3,
    VK__none_ = 0x00ff,
};
#pragma pop_macro("VK__none_")
#pragma pop_macro("VK_DBE_SBCSCHAR")
#pragma pop_macro("VK_DBE_ROMAN")
#pragma pop_macro("VK_DBE_NOROMAN")
#pragma pop_macro("VK_DBE_NOCODEINPUT")
#pragma pop_macro("VK_DBE_KATAKANA")
#pragma pop_macro("VK_DBE_HIRAGANA")
#pragma pop_macro("VK_DBE_FLUSHSTRING")
#pragma pop_macro("VK_DBE_ENTERWORDREGISTERMODE")
#pragma pop_macro("VK_DBE_ENTERIMECONFIGMODE")
#pragma pop_macro("VK_DBE_ENTERDLGCONVERSIONMODE")
#pragma pop_macro("VK_DBE_DETERMINESTRING")
#pragma pop_macro("VK_DBE_DBCSCHAR")
#pragma pop_macro("VK_DBE_CODEINPUT")
#pragma pop_macro("VK_DBE_ALPHANUMERIC")
#pragma pop_macro("VK_ABNT_C2")
#pragma pop_macro("VK_ABNT_C1")
#pragma pop_macro("VK_Z")
#pragma pop_macro("VK_Y")
#pragma pop_macro("VK_X")
#pragma pop_macro("VK_W")
#pragma pop_macro("VK_V")
#pragma pop_macro("VK_U")
#pragma pop_macro("VK_T")
#pragma pop_macro("VK_S")
#pragma pop_macro("VK_R")
#pragma pop_macro("VK_Q")
#pragma pop_macro("VK_P")
#pragma pop_macro("VK_O")
#pragma pop_macro("VK_N")
#pragma pop_macro("VK_M")
#pragma pop_macro("VK_L")
#pragma pop_macro("VK_K")
#pragma pop_macro("VK_J")
#pragma pop_macro("VK_I")
#pragma pop_macro("VK_H")
#pragma pop_macro("VK_G")
#pragma pop_macro("VK_F")
#pragma pop_macro("VK_E")
#pragma pop_macro("VK_D")
#pragma pop_macro("VK_C")
#pragma pop_macro("VK_B")
#pragma pop_macro("VK_A")
#pragma pop_macro("VK_9")
#pragma pop_macro("VK_8")
#pragma pop_macro("VK_7")
#pragma pop_macro("VK_6")
#pragma pop_macro("VK_5")
#pragma pop_macro("VK_4")
#pragma pop_macro("VK_3")
#pragma pop_macro("VK_2")
#pragma pop_macro("VK_1")
#pragma pop_macro("VK_0")
#pragma pop_macro("VK_OEM_CLEAR")
#pragma pop_macro("VK_PA1")
#pragma pop_macro("VK_NONAME")
#pragma pop_macro("VK_ZOOM")
#pragma pop_macro("VK_PLAY")
#pragma pop_macro("VK_EREOF")
#pragma pop_macro("VK_EXSEL")
#pragma pop_macro("VK_CRSEL")
#pragma pop_macro("VK_ATTN")
#pragma pop_macro("VK_OEM_BACKTAB")
#pragma pop_macro("VK_OEM_ENLW")
#pragma pop_macro("VK_OEM_AUTO")
#pragma pop_macro("VK_OEM_COPY")
#pragma pop_macro("VK_OEM_FINISH")
#pragma pop_macro("VK_OEM_ATTN")
#pragma pop_macro("VK_OEM_CUSEL")
#pragma pop_macro("VK_OEM_WSCTRL")
#pragma pop_macro("VK_OEM_PA3")
#pragma pop_macro("VK_OEM_PA2")
#pragma pop_macro("VK_OEM_PA1")
#pragma pop_macro("VK_OEM_JUMP")
#pragma pop_macro("VK_OEM_RESET")
#pragma pop_macro("VK_PACKET")
#pragma pop_macro("VK_ICO_CLEAR")
#pragma pop_macro("VK_PROCESSKEY")
#pragma pop_macro("VK_ICO_00")
#pragma pop_macro("VK_ICO_HELP")
#pragma pop_macro("VK_OEM_102")
#pragma pop_macro("VK_OEM_AX")
#pragma pop_macro("VK_OEM_8")
#pragma pop_macro("VK_OEM_7")
#pragma pop_macro("VK_OEM_6")
#pragma pop_macro("VK_OEM_5")
#pragma pop_macro("VK_OEM_4")
#pragma pop_macro("VK_GAMEPAD_RIGHT_THUMBSTICK_LEFT")
#pragma pop_macro("VK_GAMEPAD_RIGHT_THUMBSTICK_RIGHT")
#pragma pop_macro("VK_GAMEPAD_RIGHT_THUMBSTICK_DOWN")
#pragma pop_macro("VK_GAMEPAD_RIGHT_THUMBSTICK_UP")
#pragma pop_macro("VK_GAMEPAD_LEFT_THUMBSTICK_LEFT")
#pragma pop_macro("VK_GAMEPAD_LEFT_THUMBSTICK_RIGHT")
#pragma pop_macro("VK_GAMEPAD_LEFT_THUMBSTICK_DOWN")
#pragma pop_macro("VK_GAMEPAD_LEFT_THUMBSTICK_UP")
#pragma pop_macro("VK_GAMEPAD_RIGHT_THUMBSTICK_BUTTON")
#pragma pop_macro("VK_GAMEPAD_LEFT_THUMBSTICK_BUTTON")
#pragma pop_macro("VK_GAMEPAD_VIEW")
#pragma pop_macro("VK_GAMEPAD_MENU")
#pragma pop_macro("VK_GAMEPAD_DPAD_RIGHT")
#pragma pop_macro("VK_GAMEPAD_DPAD_LEFT")
#pragma pop_macro("VK_GAMEPAD_DPAD_DOWN")
#pragma pop_macro("VK_GAMEPAD_DPAD_UP")
#pragma pop_macro("VK_GAMEPAD_RIGHT_TRIGGER")
#pragma pop_macro("VK_GAMEPAD_LEFT_TRIGGER")
#pragma pop_macro("VK_GAMEPAD_LEFT_SHOULDER")
#pragma pop_macro("VK_GAMEPAD_RIGHT_SHOULDER")
#pragma pop_macro("VK_GAMEPAD_Y")
#pragma pop_macro("VK_GAMEPAD_X")
#pragma pop_macro("VK_GAMEPAD_B")
#pragma pop_macro("VK_GAMEPAD_A")
#pragma pop_macro("VK_OEM_3")
#pragma pop_macro("VK_OEM_2")
#pragma pop_macro("VK_OEM_PERIOD")
#pragma pop_macro("VK_OEM_MINUS")
#pragma pop_macro("VK_OEM_COMMA")
#pragma pop_macro("VK_OEM_PLUS")
#pragma pop_macro("VK_OEM_1")
#pragma pop_macro("VK_LAUNCH_APP2")
#pragma pop_macro("VK_LAUNCH_APP1")
#pragma pop_macro("VK_LAUNCH_MEDIA_SELECT")
#pragma pop_macro("VK_LAUNCH_MAIL")
#pragma pop_macro("VK_MEDIA_PLAY_PAUSE")
#pragma pop_macro("VK_MEDIA_STOP")
#pragma pop_macro("VK_MEDIA_PREV_TRACK")
#pragma pop_macro("VK_MEDIA_NEXT_TRACK")
#pragma pop_macro("VK_VOLUME_UP")
#pragma pop_macro("VK_VOLUME_DOWN")
#pragma pop_macro("VK_VOLUME_MUTE")
#pragma pop_macro("VK_BROWSER_HOME")
#pragma pop_macro("VK_BROWSER_FAVORITES")
#pragma pop_macro("VK_BROWSER_SEARCH")
#pragma pop_macro("VK_BROWSER_STOP")
#pragma pop_macro("VK_BROWSER_REFRESH")
#pragma pop_macro("VK_BROWSER_FORWARD")
#pragma pop_macro("VK_BROWSER_BACK")
#pragma pop_macro("VK_RMENU")
#pragma pop_macro("VK_LMENU")
#pragma pop_macro("VK_RCONTROL")
#pragma pop_macro("VK_LCONTROL")
#pragma pop_macro("VK_RSHIFT")
#pragma pop_macro("VK_LSHIFT")
#pragma pop_macro("VK_OEM_FJ_ROYA")
#pragma pop_macro("VK_OEM_FJ_LOYA")
#pragma pop_macro("VK_OEM_FJ_TOUROKU")
#pragma pop_macro("VK_OEM_FJ_MASSHOU")
#pragma pop_macro("VK_OEM_FJ_JISHO")
#pragma pop_macro("VK_OEM_NEC_EQUAL")
#pragma pop_macro("VK_SCROLL")
#pragma pop_macro("VK_NUMLOCK")
#pragma pop_macro("VK_NAVIGATION_CANCEL")
#pragma pop_macro("VK_NAVIGATION_ACCEPT")
#pragma pop_macro("VK_NAVIGATION_RIGHT")
#pragma pop_macro("VK_NAVIGATION_LEFT")
#pragma pop_macro("VK_NAVIGATION_DOWN")
#pragma pop_macro("VK_NAVIGATION_UP")
#pragma pop_macro("VK_NAVIGATION_MENU")
#pragma pop_macro("VK_NAVIGATION_VIEW")
#pragma pop_macro("VK_F24")
#pragma pop_macro("VK_F23")
#pragma pop_macro("VK_F22")
#pragma pop_macro("VK_F21")
#pragma pop_macro("VK_F20")
#pragma pop_macro("VK_F19")
#pragma pop_macro("VK_F18")
#pragma pop_macro("VK_F17")
#pragma pop_macro("VK_F16")
#pragma pop_macro("VK_F15")
#pragma pop_macro("VK_F14")
#pragma pop_macro("VK_F13")
#pragma pop_macro("VK_F12")
#pragma pop_macro("VK_F11")
#pragma pop_macro("VK_F10")
#pragma pop_macro("VK_F9")
#pragma pop_macro("VK_F8")
#pragma pop_macro("VK_F7")
#pragma pop_macro("VK_F6")
#pragma pop_macro("VK_F5")
#pragma pop_macro("VK_F4")
#pragma pop_macro("VK_F3")
#pragma pop_macro("VK_F2")
#pragma pop_macro("VK_F1")
#pragma pop_macro("VK_DIVIDE")
#pragma pop_macro("VK_DECIMAL")
#pragma pop_macro("VK_SUBTRACT")
#pragma pop_macro("VK_SEPARATOR")
#pragma pop_macro("VK_ADD")
#pragma pop_macro("VK_MULTIPLY")
#pragma pop_macro("VK_NUMPAD9")
#pragma pop_macro("VK_NUMPAD8")
#pragma pop_macro("VK_NUMPAD7")
#pragma pop_macro("VK_NUMPAD6")
#pragma pop_macro("VK_NUMPAD5")
#pragma pop_macro("VK_NUMPAD4")
#pragma pop_macro("VK_NUMPAD3")
#pragma pop_macro("VK_NUMPAD2")
#pragma pop_macro("VK_NUMPAD1")
#pragma pop_macro("VK_NUMPAD0")
#pragma pop_macro("VK_SLEEP")
#pragma pop_macro("VK_APPS")
#pragma pop_macro("VK_RWIN")
#pragma pop_macro("VK_LWIN")
#pragma pop_macro("VK_HELP")
#pragma pop_macro("VK_DELETE")
#pragma pop_macro("VK_INSERT")
#pragma pop_macro("VK_SNAPSHOT")
#pragma pop_macro("VK_EXECUTE")
#pragma pop_macro("VK_PRINT")
#pragma pop_macro("VK_SELECT")
#pragma pop_macro("VK_DOWN")
#pragma pop_macro("VK_RIGHT")
#pragma pop_macro("VK_UP")
#pragma pop_macro("VK_LEFT")
#pragma pop_macro("VK_HOME")
#pragma pop_macro("VK_END")
#pragma pop_macro("VK_NEXT")
#pragma pop_macro("VK_PRIOR")
#pragma pop_macro("VK_SPACE")
#pragma pop_macro("VK_MODECHANGE")
#pragma pop_macro("VK_ACCEPT")
#pragma pop_macro("VK_NONCONVERT")
#pragma pop_macro("VK_CONVERT")
#pragma pop_macro("VK_ESCAPE")
#pragma pop_macro("VK_IME_OFF")
#pragma pop_macro("VK_KANJI")
#pragma pop_macro("VK_HANJA")
#pragma pop_macro("VK_FINAL")
#pragma pop_macro("VK_JUNJA")
#pragma pop_macro("VK_IME_ON")
#pragma pop_macro("VK_HANGUL")
#pragma pop_macro("VK_HANGEUL")
#pragma pop_macro("VK_KANA")
#pragma pop_macro("VK_CAPITAL")
#pragma pop_macro("VK_PAUSE")
#pragma pop_macro("VK_MENU")
#pragma pop_macro("VK_CONTROL")
#pragma pop_macro("VK_SHIFT")
#pragma pop_macro("VK_RETURN")
#pragma pop_macro("VK_CLEAR")
#pragma pop_macro("VK_TAB")
#pragma pop_macro("VK_BACK")
#pragma pop_macro("VK_XBUTTON2")
#pragma pop_macro("VK_XBUTTON1")
#pragma pop_macro("VK_MBUTTON")
#pragma pop_macro("VK_CANCEL")
#pragma pop_macro("VK_RBUTTON")
#pragma pop_macro("VK_LBUTTON")

#pragma push_macro("LLKHF_EXTENDED")
#pragma push_macro("LLKHF_INJECTED")
#pragma push_macro("LLKHF_ALTDOWN")
#pragma push_macro("LLKHF_UP")
#pragma push_macro("LLKHF_LOWER_IL_INJECTED")
#undef LLKHF_EXTENDED
#undef LLKHF_INJECTED
#undef LLKHF_ALTDOWN
#undef LLKHF_UP
#undef LLKHF_LOWER_IL_INJECTED
enum class
    [[clang::flag_enum]]
    KBDLLHOOKSTRUCT_FLAGS : int
{
    LLKHF_EXTENDED = 1,
    LLKHF_INJECTED = 16,
    LLKHF_ALTDOWN = 32,
    LLKHF_UP = 128,
    LLKHF_LOWER_IL_INJECTED = 2,
};
#pragma pop_macro("LLKHF_LOWER_IL_INJECTED")
#pragma pop_macro("LLKHF_UP")
#pragma pop_macro("LLKHF_ALTDOWN")
#pragma pop_macro("LLKHF_INJECTED")
#pragma pop_macro("LLKHF_EXTENDED")

#pragma push_macro("SS_LEFT")
#pragma push_macro("SS_CENTER")
#pragma push_macro("SS_RIGHT")
#pragma push_macro("SS_ICON")
#pragma push_macro("SS_BLACKRECT")
#pragma push_macro("SS_GRAYRECT")
#pragma push_macro("SS_WHITERECT")
#pragma push_macro("SS_BLACKFRAME")
#pragma push_macro("SS_GRAYFRAME")
#pragma push_macro("SS_WHITEFRAME")
#pragma push_macro("SS_USERITEM")
#pragma push_macro("SS_SIMPLE")
#pragma push_macro("SS_LEFTNOWORDWRAP")
#pragma push_macro("SS_OWNERDRAW")
#pragma push_macro("SS_BITMAP")
#pragma push_macro("SS_ENHMETAFILE")
#pragma push_macro("SS_ETCHEDHORZ")
#pragma push_macro("SS_ETCHEDVERT")
#pragma push_macro("SS_ETCHEDFRAME")
#pragma push_macro("SS_TYPEMASK")
#pragma push_macro("SS_REALSIZECONTROL")
#pragma push_macro("SS_NOPREFIX")
#pragma push_macro("SS_NOTIFY")
#pragma push_macro("SS_CENTERIMAGE")
#pragma push_macro("SS_RIGHTJUST")
#pragma push_macro("SS_REALSIZEIMAGE")
#pragma push_macro("SS_SUNKEN")
#pragma push_macro("SS_EDITCONTROL")
#pragma push_macro("SS_ENDELLIPSIS")
#pragma push_macro("SS_PATHELLIPSIS")
#pragma push_macro("SS_WORDELLIPSIS")
#pragma push_macro("SS_ELLIPSISMASK")
#undef SS_LEFT
#undef SS_CENTER
#undef SS_RIGHT
#undef SS_ICON
#undef SS_BLACKRECT
#undef SS_GRAYRECT
#undef SS_WHITERECT
#undef SS_BLACKFRAME
#undef SS_GRAYFRAME
#undef SS_WHITEFRAME
#undef SS_USERITEM
#undef SS_SIMPLE
#undef SS_LEFTNOWORDWRAP
#undef SS_OWNERDRAW
#undef SS_BITMAP
#undef SS_ENHMETAFILE
#undef SS_ETCHEDHORZ
#undef SS_ETCHEDVERT
#undef SS_ETCHEDFRAME
#undef SS_TYPEMASK
#undef SS_REALSIZECONTROL
#undef SS_NOPREFIX
#undef SS_NOTIFY
#undef SS_CENTERIMAGE
#undef SS_RIGHTJUST
#undef SS_REALSIZEIMAGE
#undef SS_SUNKEN
#undef SS_EDITCONTROL
#undef SS_ENDELLIPSIS
#undef SS_PATHELLIPSIS
#undef SS_WORDELLIPSIS
#undef SS_ELLIPSISMASK
enum class
    STATIC_STYLES : unsigned int
{
    SS_LEFT = 0x00000000u,
    SS_CENTER = 0x00000001u,
    SS_RIGHT = 0x00000002u,
    SS_ICON = 0x00000003u,
    SS_BLACKRECT = 0x00000004u,
    SS_GRAYRECT = 0x00000005u,
    SS_WHITERECT = 0x00000006u,
    SS_BLACKFRAME = 0x00000007u,
    SS_GRAYFRAME = 0x00000008u,
    SS_WHITEFRAME = 0x00000009u,
    SS_USERITEM = 0x0000000au,
    SS_SIMPLE = 0x0000000bu,
    SS_LEFTNOWORDWRAP = 0x0000000cu,
    SS_OWNERDRAW = 0x0000000du,
    SS_BITMAP = 0x0000000eu,
    SS_ENHMETAFILE = 0x0000000fu,
    SS_ETCHEDHORZ = 0x00000010u,
    SS_ETCHEDVERT = 0x00000011u,
    SS_ETCHEDFRAME = 0x00000012u,
    SS_TYPEMASK = 0x0000001fu,
    SS_REALSIZECONTROL = 0x00000040u,
    SS_NOPREFIX = 0x00000080u,
    SS_NOTIFY = 0x00000100u,
    SS_CENTERIMAGE = 0x00000200u,
    SS_RIGHTJUST = 0x00000400u,
    SS_REALSIZEIMAGE = 0x00000800u,
    SS_SUNKEN = 0x00001000u,
    SS_EDITCONTROL = 0x00002000u,
    SS_ENDELLIPSIS = 0x00004000u,
    SS_PATHELLIPSIS = 0x00008000u,
    SS_WORDELLIPSIS = 0x0000c000u,
    SS_ELLIPSISMASK = 0x0000c000u,
};
#pragma pop_macro("SS_ELLIPSISMASK")
#pragma pop_macro("SS_WORDELLIPSIS")
#pragma pop_macro("SS_PATHELLIPSIS")
#pragma pop_macro("SS_ENDELLIPSIS")
#pragma pop_macro("SS_EDITCONTROL")
#pragma pop_macro("SS_SUNKEN")
#pragma pop_macro("SS_REALSIZEIMAGE")
#pragma pop_macro("SS_RIGHTJUST")
#pragma pop_macro("SS_CENTERIMAGE")
#pragma pop_macro("SS_NOTIFY")
#pragma pop_macro("SS_NOPREFIX")
#pragma pop_macro("SS_REALSIZECONTROL")
#pragma pop_macro("SS_TYPEMASK")
#pragma pop_macro("SS_ETCHEDFRAME")
#pragma pop_macro("SS_ETCHEDVERT")
#pragma pop_macro("SS_ETCHEDHORZ")
#pragma pop_macro("SS_ENHMETAFILE")
#pragma pop_macro("SS_BITMAP")
#pragma pop_macro("SS_OWNERDRAW")
#pragma pop_macro("SS_LEFTNOWORDWRAP")
#pragma pop_macro("SS_SIMPLE")
#pragma pop_macro("SS_USERITEM")
#pragma pop_macro("SS_WHITEFRAME")
#pragma pop_macro("SS_GRAYFRAME")
#pragma pop_macro("SS_BLACKFRAME")
#pragma pop_macro("SS_WHITERECT")
#pragma pop_macro("SS_GRAYRECT")
#pragma pop_macro("SS_BLACKRECT")
#pragma pop_macro("SS_ICON")
#pragma pop_macro("SS_RIGHT")
#pragma pop_macro("SS_CENTER")
#pragma pop_macro("SS_LEFT")

#pragma push_macro("ODA_DRAWENTIRE")
#pragma push_macro("ODA_SELECT")
#pragma push_macro("ODA_FOCUS")
#undef ODA_DRAWENTIRE
#undef ODA_SELECT
#undef ODA_FOCUS
enum class
    ODA_FLAGS : unsigned int
{
    ODA_DRAWENTIRE = 0x00000001u,
    ODA_SELECT = 0x00000002u,
    ODA_FOCUS = 0x00000004u,
};
#pragma pop_macro("ODA_FOCUS")
#pragma pop_macro("ODA_SELECT")
#pragma pop_macro("ODA_DRAWENTIRE")

#pragma push_macro("ODS_SELECTED")
#pragma push_macro("ODS_GRAYED")
#pragma push_macro("ODS_DISABLED")
#pragma push_macro("ODS_CHECKED")
#pragma push_macro("ODS_FOCUS")
#pragma push_macro("ODS_DEFAULT")
#pragma push_macro("ODS_COMBOBOXEDIT")
#pragma push_macro("ODS_HOTLIGHT")
#pragma push_macro("ODS_INACTIVE")
#pragma push_macro("ODS_NOACCEL")
#pragma push_macro("ODS_NOFOCUSRECT")
#undef ODS_SELECTED
#undef ODS_GRAYED
#undef ODS_DISABLED
#undef ODS_CHECKED
#undef ODS_FOCUS
#undef ODS_DEFAULT
#undef ODS_COMBOBOXEDIT
#undef ODS_HOTLIGHT
#undef ODS_INACTIVE
#undef ODS_NOACCEL
#undef ODS_NOFOCUSRECT
enum class
    ODS_FLAGS : unsigned int
{
    ODS_SELECTED = 0x00000001u,
    ODS_GRAYED = 0x00000002u,
    ODS_DISABLED = 0x00000004u,
    ODS_CHECKED = 0x00000008u,
    ODS_FOCUS = 0x00000010u,
    ODS_DEFAULT = 0x00000020u,
    ODS_COMBOBOXEDIT = 0x00001000u,
    ODS_HOTLIGHT = 0x00000040u,
    ODS_INACTIVE = 0x00000080u,
    ODS_NOACCEL = 0x00000100u,
    ODS_NOFOCUSRECT = 0x00000200u,
};
#pragma pop_macro("ODS_NOFOCUSRECT")
#pragma pop_macro("ODS_NOACCEL")
#pragma pop_macro("ODS_INACTIVE")
#pragma pop_macro("ODS_HOTLIGHT")
#pragma pop_macro("ODS_COMBOBOXEDIT")
#pragma pop_macro("ODS_DEFAULT")
#pragma pop_macro("ODS_FOCUS")
#pragma pop_macro("ODS_CHECKED")
#pragma pop_macro("ODS_DISABLED")
#pragma pop_macro("ODS_GRAYED")
#pragma pop_macro("ODS_SELECTED")

#pragma push_macro("HELPINFO_WINDOW")
#pragma push_macro("HELPINFO_MENUITEM")
#undef HELPINFO_WINDOW
#undef HELPINFO_MENUITEM
enum class
    HELP_INFO_TYPE : int
{
    HELPINFO_WINDOW = 1,
    HELPINFO_MENUITEM = 2,
};
#pragma pop_macro("HELPINFO_MENUITEM")
#pragma pop_macro("HELPINFO_WINDOW")

#pragma push_macro("DF_ALLOWOTHERACCOUNTHOOK")
#undef DF_ALLOWOTHERACCOUNTHOOK
enum class
    DESKTOP_CONTROL_FLAGS : unsigned int
{
    DF_ALLOWOTHERACCOUNTHOOK = 0x00000001u,
};
#pragma pop_macro("DF_ALLOWOTHERACCOUNTHOOK")

#pragma push_macro("DESKTOP_READOBJECTS")
#pragma push_macro("DESKTOP_CREATEWINDOW")
#pragma push_macro("DESKTOP_CREATEMENU")
#pragma push_macro("DESKTOP_HOOKCONTROL")
#pragma push_macro("DESKTOP_JOURNALRECORD")
#pragma push_macro("DESKTOP_JOURNALPLAYBACK")
#pragma push_macro("DESKTOP_ENUMERATE")
#pragma push_macro("DESKTOP_WRITEOBJECTS")
#pragma push_macro("DESKTOP_SWITCHDESKTOP")
#pragma push_macro("DESKTOP_DELETE")
#pragma push_macro("DESKTOP_READ_CONTROL")
#pragma push_macro("DESKTOP_WRITE_DAC")
#pragma push_macro("DESKTOP_WRITE_OWNER")
#pragma push_macro("DESKTOP_SYNCHRONIZE")
#undef DESKTOP_READOBJECTS
#undef DESKTOP_CREATEWINDOW
#undef DESKTOP_CREATEMENU
#undef DESKTOP_HOOKCONTROL
#undef DESKTOP_JOURNALRECORD
#undef DESKTOP_JOURNALPLAYBACK
#undef DESKTOP_ENUMERATE
#undef DESKTOP_WRITEOBJECTS
#undef DESKTOP_SWITCHDESKTOP
#undef DESKTOP_DELETE
#undef DESKTOP_READ_CONTROL
#undef DESKTOP_WRITE_DAC
#undef DESKTOP_WRITE_OWNER
#undef DESKTOP_SYNCHRONIZE
enum class
    DESKTOP_ACCESS_FLAGS : unsigned int
{
    DESKTOP_READOBJECTS = 0x00000001u,
    DESKTOP_CREATEWINDOW = 0x00000002u,
    DESKTOP_CREATEMENU = 0x00000004u,
    DESKTOP_HOOKCONTROL = 0x00000008u,
    DESKTOP_JOURNALRECORD = 0x00000010u,
    DESKTOP_JOURNALPLAYBACK = 0x00000020u,
    DESKTOP_ENUMERATE = 0x00000040u,
    DESKTOP_WRITEOBJECTS = 0x00000080u,
    DESKTOP_SWITCHDESKTOP = 0x00000100u,
    DESKTOP_DELETE = 0x00010000u,
    DESKTOP_READ_CONTROL = 0x00020000u,
    DESKTOP_WRITE_DAC = 0x00040000u,
    DESKTOP_WRITE_OWNER = 0x00080000u,
    DESKTOP_SYNCHRONIZE = 0x00100000u,
};
#pragma pop_macro("DESKTOP_SYNCHRONIZE")
#pragma pop_macro("DESKTOP_WRITE_OWNER")
#pragma pop_macro("DESKTOP_WRITE_DAC")
#pragma pop_macro("DESKTOP_READ_CONTROL")
#pragma pop_macro("DESKTOP_DELETE")
#pragma pop_macro("DESKTOP_SWITCHDESKTOP")
#pragma pop_macro("DESKTOP_WRITEOBJECTS")
#pragma pop_macro("DESKTOP_ENUMERATE")
#pragma pop_macro("DESKTOP_JOURNALPLAYBACK")
#pragma pop_macro("DESKTOP_JOURNALRECORD")
#pragma pop_macro("DESKTOP_HOOKCONTROL")
#pragma pop_macro("DESKTOP_CREATEMENU")
#pragma pop_macro("DESKTOP_CREATEWINDOW")
#pragma pop_macro("DESKTOP_READOBJECTS")

#pragma push_macro("SB_LINEUP")
#pragma push_macro("SB_LINELEFT")
#pragma push_macro("SB_LINEDOWN")
#pragma push_macro("SB_LINERIGHT")
#pragma push_macro("SB_PAGEUP")
#pragma push_macro("SB_PAGELEFT")
#pragma push_macro("SB_PAGEDOWN")
#pragma push_macro("SB_PAGERIGHT")
#pragma push_macro("SB_THUMBPOSITION")
#pragma push_macro("SB_THUMBTRACK")
#pragma push_macro("SB_TOP")
#pragma push_macro("SB_LEFT")
#pragma push_macro("SB_RIGHT")
#pragma push_macro("SB_BOTTOM")
#pragma push_macro("SB_ENDSCROLL")
#undef SB_LINEUP
#undef SB_LINELEFT
#undef SB_LINEDOWN
#undef SB_LINERIGHT
#undef SB_PAGEUP
#undef SB_PAGELEFT
#undef SB_PAGEDOWN
#undef SB_PAGERIGHT
#undef SB_THUMBPOSITION
#undef SB_THUMBTRACK
#undef SB_TOP
#undef SB_LEFT
#undef SB_RIGHT
#undef SB_BOTTOM
#undef SB_ENDSCROLL
enum class
    SCROLLBAR_COMMAND : int
{
    SB_LINEUP = 0,
    SB_LINELEFT = 0,
    SB_LINEDOWN = 1,
    SB_LINERIGHT = 1,
    SB_PAGEUP = 2,
    SB_PAGELEFT = 2,
    SB_PAGEDOWN = 3,
    SB_PAGERIGHT = 3,
    SB_THUMBPOSITION = 4,
    SB_THUMBTRACK = 5,
    SB_TOP = 6,
    SB_LEFT = 6,
    SB_RIGHT = 7,
    SB_BOTTOM = 7,
    SB_ENDSCROLL = 8,
};
#pragma pop_macro("SB_ENDSCROLL")
#pragma pop_macro("SB_BOTTOM")
#pragma pop_macro("SB_RIGHT")
#pragma pop_macro("SB_LEFT")
#pragma pop_macro("SB_TOP")
#pragma pop_macro("SB_THUMBTRACK")
#pragma pop_macro("SB_THUMBPOSITION")
#pragma pop_macro("SB_PAGERIGHT")
#pragma pop_macro("SB_PAGEDOWN")
#pragma pop_macro("SB_PAGELEFT")
#pragma pop_macro("SB_PAGEUP")
#pragma pop_macro("SB_LINERIGHT")
#pragma pop_macro("SB_LINEDOWN")
#pragma pop_macro("SB_LINELEFT")
#pragma pop_macro("SB_LINEUP")

#pragma push_macro("MAPVK_VK_TO_VSC")
#pragma push_macro("MAPVK_VSC_TO_VK")
#pragma push_macro("MAPVK_VK_TO_CHAR")
#pragma push_macro("MAPVK_VSC_TO_VK_EX")
#pragma push_macro("MAPVK_VK_TO_VSC_EX")
#undef MAPVK_VK_TO_VSC
#undef MAPVK_VSC_TO_VK
#undef MAPVK_VK_TO_CHAR
#undef MAPVK_VSC_TO_VK_EX
#undef MAPVK_VK_TO_VSC_EX
enum class
    MAP_VIRTUAL_KEY_TYPE : int
{
    MAPVK_VK_TO_VSC = 0,
    MAPVK_VSC_TO_VK = 1,
    MAPVK_VK_TO_CHAR = 2,
    MAPVK_VSC_TO_VK_EX = 3,
    MAPVK_VK_TO_VSC_EX = 4,
};
#pragma pop_macro("MAPVK_VK_TO_VSC_EX")
#pragma pop_macro("MAPVK_VSC_TO_VK_EX")
#pragma pop_macro("MAPVK_VK_TO_CHAR")
#pragma pop_macro("MAPVK_VSC_TO_VK")
#pragma pop_macro("MAPVK_VK_TO_VSC")

#pragma push_macro("EDS_RAWMODE")
#pragma push_macro("EDS_ROTATEDMODE")
#undef EDS_RAWMODE
#undef EDS_ROTATEDMODE
enum class
    [[clang::flag_enum]]
    ENUM_DISPLAY_SETTINGS_FLAGS : int
{
    EDS_RAWMODE = 2,
    EDS_ROTATEDMODE = 4,
};
#pragma pop_macro("EDS_ROTATEDMODE")
#pragma pop_macro("EDS_RAWMODE")

#endif


#ifdef __cplusplus
}
#endif  /* __cplusplus */

#endif /* !_WINUSER_ */



