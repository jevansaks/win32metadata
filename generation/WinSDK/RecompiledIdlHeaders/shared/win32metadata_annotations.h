#ifndef WIN32METADATA_ANNOTATIONS_H
#define WIN32METADATA_ANNOTATIONS_H

#if defined(WIN32METADATA) && defined(__clang__)
#define _WIN32META_ANNOTATION_(...) __attribute__((annotate("Win32Metadata." __VA_ARGS__)))
#else
#define _WIN32META_ANNOTATION_(...)
#endif

#define _Win32_SetLastError_ _WIN32META_ANNOTATION_("SetLastError")
#define _Win32_Retval_ _WIN32META_ANNOTATION_("Retval")
#define _Win32_Retained_ _WIN32META_ANNOTATION_("Retained")
#define _Win32_ContainsInteriorPointers_ _WIN32META_ANNOTATION_("ContainsInteriorPointers")
#define _Win32_AssociatedEnum_(type) _WIN32META_ANNOTATION_("AssociatedEnum(" #type ")")

#if defined(WIN32METADATA) && defined(__clang__)
#define _Win32_Enum_(name, storage, ...) \
    typedef _WIN32META_ANNOTATION_("Enum(" #__VA_ARGS__ ")", __VA_ARGS__) storage name
#define _Win32_FlagsEnum_(name, storage, ...) \
    typedef _WIN32META_ANNOTATION_("FlagsEnum(" #__VA_ARGS__ ")", __VA_ARGS__) storage name
#else
#define _Win32_Enum_(name, storage, ...)
#define _Win32_FlagsEnum_(name, storage, ...)
#endif

#endif
