#ifndef WIN32METADATA_ANNOTATIONS_H
#define WIN32METADATA_ANNOTATIONS_H

#if defined(WIN32METADATA) && defined(__clang__)
#define _WIN32META_ANNOTATION_(text) __attribute__((annotate("Win32Metadata." text)))
#else
#define _WIN32META_ANNOTATION_(text)
#endif

#define _Win32_SetLastError_ _WIN32META_ANNOTATION_("SetLastError")

#endif
