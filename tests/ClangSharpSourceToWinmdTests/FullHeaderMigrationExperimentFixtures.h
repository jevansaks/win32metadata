// Frozen migration inputs from 88991baf654ff0d89855a7005873addd92a76859.
// Keep diagnostic repros independent of the currently enabled production headers.

// um/winuser.h
typedef _WIN32META_ANNOTATION_("Enum(GR_GLOBAL)", (unsigned int)-2)
    unsigned int GET_GUI_RESOURCES_FLAGS;

// um/winnt.h
_Win32_AssociatedConstant_(SERVICE_NO_CHANGE)
_Win32_Enum_(SERVICE_ERROR, unsigned int,
    SERVICE_ERROR_CRITICAL,
    SERVICE_ERROR_IGNORE,
    SERVICE_ERROR_NORMAL,
    SERVICE_ERROR_SEVERE);

// shared/minwindef.h
_Win32_AlsoUsableFor_(HGDIOBJ) typedef HRGN HRGN;
