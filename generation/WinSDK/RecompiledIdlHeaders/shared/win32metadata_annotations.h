#ifndef WIN32METADATA_ANNOTATIONS_H
#define WIN32METADATA_ANNOTATIONS_H

#if defined(WIN32METADATA) && defined(__clang__)
#define _WIN32META_ANNOTATION_(text) __attribute__((annotate("Win32Metadata." text)))
#else
#define _WIN32META_ANNOTATION_(text)
#endif

#define _Win32_SetLastError_ _WIN32META_ANNOTATION_("SetLastError")
#define _Win32_Retval_ _WIN32META_ANNOTATION_("Retval")
#define _Win32_Retained_ _WIN32META_ANNOTATION_("Retained")
#define _Win32_ContainsInteriorPointers_ _WIN32META_ANNOTATION_("ContainsInteriorPointers")
#define _Win32_AssociatedEnum_(type) _WIN32META_ANNOTATION_("AssociatedEnum(" #type ")")

#endif
