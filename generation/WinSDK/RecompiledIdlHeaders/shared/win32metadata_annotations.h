#ifndef WIN32METADATA_ANNOTATIONS_H
#define WIN32METADATA_ANNOTATIONS_H

#if defined(WIN32METADATA) && defined(__clang__)
#define _WIN32META_ANNOTATION_(...) __attribute__((annotate("Win32Metadata." __VA_ARGS__)))
#else
#define _WIN32META_ANNOTATION_(...)
#endif

#define _Win32_SetLastError_ _WIN32META_ANNOTATION_("SetLastError")
#define _Win32_PreserveResult_ _WIN32META_ANNOTATION_("PreserveResult")
#define _Win32_ImportLibrary_(module) _WIN32META_ANNOTATION_("ImportLibrary(" #module ")")
#define _Win32_SupportedOS_Windows2000_ _WIN32META_ANNOTATION_("SupportedOSPlatform(\"windows5.0\")")
#define _Win32_SupportedOS_WindowsXP_ _WIN32META_ANNOTATION_("SupportedOSPlatform(\"windows5.1.2600\")")
#define _Win32_SupportedOS_WindowsVista_ _WIN32META_ANNOTATION_("SupportedOSPlatform(\"windows6.0.6000\")")
#define _Win32_SupportedOS_Windows7_ _WIN32META_ANNOTATION_("SupportedOSPlatform(\"windows6.1\")")
#define _Win32_SupportedOS_Windows8_ _WIN32META_ANNOTATION_("SupportedOSPlatform(\"windows8.0\")")
#define _Win32_SupportedOS_Windows81_ _WIN32META_ANNOTATION_("SupportedOSPlatform(\"windows8.1\")")
#define _Win32_SupportedOS_10240_ _WIN32META_ANNOTATION_("SupportedOSPlatform(\"windows10.0.10240\")")
#define _Win32_SupportedOS_10586_ _WIN32META_ANNOTATION_("SupportedOSPlatform(\"windows10.0.10586\")")
#define _Win32_SupportedOS_14393_ _WIN32META_ANNOTATION_("SupportedOSPlatform(\"windows10.0.14393\")")
#define _Win32_SupportedOS_15063_ _WIN32META_ANNOTATION_("SupportedOSPlatform(\"windows10.0.15063\")")
#define _Win32_SupportedOS_16299_ _WIN32META_ANNOTATION_("SupportedOSPlatform(\"windows10.0.16299\")")
#define _Win32_SupportedOS_17134_ _WIN32META_ANNOTATION_("SupportedOSPlatform(\"windows10.0.17134\")")
#define _Win32_SupportedOS_17763_ _WIN32META_ANNOTATION_("SupportedOSPlatform(\"windows10.0.17763\")")
#define _Win32_SupportedOS_18362_ _WIN32META_ANNOTATION_("SupportedOSPlatform(\"windows10.0.18362\")")
#define _Win32_SupportedOS_19041_ _WIN32META_ANNOTATION_("SupportedOSPlatform(\"windows10.0.19041\")")
#define _Win32_SupportedOS_Server2000_ _WIN32META_ANNOTATION_("SupportedOSPlatform(\"windowsserver2000\")")
#define _Win32_SupportedOS_Server2003_ _WIN32META_ANNOTATION_("SupportedOSPlatform(\"windowsserver2003\")")
#define _Win32_SupportedOS_Server2008_ _WIN32META_ANNOTATION_("SupportedOSPlatform(\"windowsserver2008\")")
#define _Win32_SupportedOS_Server2012_ _WIN32META_ANNOTATION_("SupportedOSPlatform(\"windowsserver2012\")")
#define _Win32_SupportedOS_Server2016_ _WIN32META_ANNOTATION_("SupportedOSPlatform(\"windowsserver2016\")")
#define _Win32_Retval_ _WIN32META_ANNOTATION_("RetVal")
#define _Win32_Retained_ _WIN32META_ANNOTATION_("Retained")
#define _Win32_ContainsInteriorPointers_ _WIN32META_ANNOTATION_("ContainsInteriorPointers")
#define _Win32_AssociatedEnum_(type) _WIN32META_ANNOTATION_("AssociatedEnum(" #type ")")
#define _Win32_AlsoUsableFor_(type) _WIN32META_ANNOTATION_("AlsoUsableFor(" #type ")", sizeof(type))
// Pointer sentinels are not Clang annotate constant arguments; sizeof retains their typed AST unevaluated.
#define _Win32_RAIIFree_(cleanup, ...) \
    _WIN32META_ANNOTATION_("RAIIFree", sizeof(&(cleanup)), sizeof((0, ##__VA_ARGS__)))
#define _Win32_RAIIFree_Handle_(cleanup) _Win32_RAIIFree_(cleanup, -1, 0)

#if defined(WIN32METADATA) && defined(__clang__) && defined(__cplusplus)
#define _WIN32META_JOIN_RAW_(left, right) left##right
#define _WIN32META_JOIN_(left, right) _WIN32META_JOIN_RAW_(left, right)
#define _WIN32META_ANNOTATE_FIELD_(field, annotation) \
    _WIN32META_ANNOTATION_("Field", sizeof(&field)) annotation \
    static const unsigned char _WIN32META_JOIN_(_WIN32META_FIELD_, __COUNTER__) = 0
#if !defined(CINTERFACE)
#define _WIN32META_ANNOTATE_METHOD_(method, annotation) \
    _WIN32META_ANNOTATION_("Method", sizeof(&method)) annotation \
    static const unsigned char _WIN32META_JOIN_(_WIN32META_METHOD_, __COUNTER__) = 0
#define _WIN32META_ANNOTATE_PARAMETER_(method, parameter, annotation) \
    _WIN32META_ANNOTATION_("Parameter(" #parameter ")", sizeof(&method)) annotation \
    static const unsigned char _WIN32META_JOIN_(_WIN32META_PARAMETER_, __COUNTER__) = 0
#endif
#else
#define _WIN32META_ANNOTATE_FIELD_(field, annotation)
#endif
#ifndef _WIN32META_ANNOTATE_METHOD_
#define _WIN32META_ANNOTATE_METHOD_(method, annotation)
#define _WIN32META_ANNOTATE_PARAMETER_(method, parameter, annotation)
#endif

#if defined(MIDL_PASS)
#define _Win32_AnnotateMethod_(method, annotation) \
    cpp_quote("_WIN32META_ANNOTATE_METHOD_(" #method ", " #annotation ");")
#define _Win32_AnnotateParameter_(method, parameter, annotation) \
    cpp_quote("_WIN32META_ANNOTATE_PARAMETER_(" #method ", " #parameter ", " #annotation ");")
#define _Win32_AnnotateField_(field, annotation) \
    cpp_quote("_WIN32META_ANNOTATE_FIELD_(" #field ", " #annotation ");")
#else
#define _Win32_AnnotateMethod_(method, annotation) _WIN32META_ANNOTATE_METHOD_(method, annotation)
#define _Win32_AnnotateParameter_(method, parameter, annotation) _WIN32META_ANNOTATE_PARAMETER_(method, parameter, annotation)
#define _Win32_AnnotateField_(field, annotation) _WIN32META_ANNOTATE_FIELD_(field, annotation)
#endif

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
