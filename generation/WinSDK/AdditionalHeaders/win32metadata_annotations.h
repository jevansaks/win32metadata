#pragma once

#if defined(WIN32METADATA)
#define _WIN32_METADATA_ANNOTATE_(payload) __attribute__((annotate(payload)))
#define _WIN32_METADATA_VALUE_(name, value) \
    _WIN32_METADATA_ANNOTATE_("win32metadata:" name "=" #value)
#define _WIN32_METADATA_STRING_VALUE_(name, value) \
    _WIN32_METADATA_ANNOTATE_("win32metadata:" name "=" value)

#define _Win32_SetLastError_ \
    _WIN32_METADATA_ANNOTATE_("win32metadata:set_last_error")
#define _Win32_ImportLibrary_(library) \
    _WIN32_METADATA_STRING_VALUE_("import_library", library)
#define _Win32_PreserveResult_ \
    _WIN32_METADATA_ANNOTATE_("win32metadata:preserve_result")
#define _Win32_Agile_ \
    _WIN32_METADATA_ANNOTATE_("win32metadata:agile")
#define _Win32_RAIIFree_(function) \
    _WIN32_METADATA_VALUE_("raii_free", function)
#define _Win32_InvalidHandle_(value) \
    _WIN32_METADATA_VALUE_("invalid_handle", value)
#define _Win32_Retained_ \
    _WIN32_METADATA_ANNOTATE_("win32metadata:retained")
#define _Win32_AlsoUsableFor_(type) \
    _WIN32_METADATA_VALUE_("also_usable_for", type)
#define _Win32_AssociatedEnum_(type) \
    _WIN32_METADATA_VALUE_("associated_enum", type)
#define _Win32_StructSizeField_(field) \
    _WIN32_METADATA_VALUE_("struct_size_field", field)
#define _Win32_FlexibleArray_ \
    _WIN32_METADATA_ANNOTATE_("win32metadata:flexible_array")
#define _Win32_Retval_ \
    _WIN32_METADATA_ANNOTATE_("win32metadata:retval")
#define _Win32_Documentation_(url) \
    _WIN32_METADATA_STRING_VALUE_("documentation", url)

#define _Windows_SupportedOS_Windows2000_ \
    _WIN32_METADATA_VALUE_("supported_os", windows5.0)
#define _Windows_SupportedOS_WindowsXP_ \
    _WIN32_METADATA_VALUE_("supported_os", windows5.1.2600)
#define _Windows_SupportedOS_WindowsVista_ \
    _WIN32_METADATA_VALUE_("supported_os", windows6.0.6000)
#define _Windows_SupportedOS_Windows7_ \
    _WIN32_METADATA_VALUE_("supported_os", windows6.1)
#define _Windows_SupportedOS_Windows8_ \
    _WIN32_METADATA_VALUE_("supported_os", windows8.0)
#define _Windows_SupportedOS_Windows81_ \
    _WIN32_METADATA_VALUE_("supported_os", windows8.1)
#define _Windows_SupportedOS_10240_ \
    _WIN32_METADATA_VALUE_("supported_os", windows10.0.10240)
#define _Windows_SupportedOS_10586_ \
    _WIN32_METADATA_VALUE_("supported_os", windows10.0.10586)
#define _Windows_SupportedOS_14393_ \
    _WIN32_METADATA_VALUE_("supported_os", windows10.0.14393)
#define _Windows_SupportedOS_15063_ \
    _WIN32_METADATA_VALUE_("supported_os", windows10.0.15063)
#define _Windows_SupportedOS_16299_ \
    _WIN32_METADATA_VALUE_("supported_os", windows10.0.16299)
#define _Windows_SupportedOS_17134_ \
    _WIN32_METADATA_VALUE_("supported_os", windows10.0.17134)
#define _Windows_SupportedOS_17763_ \
    _WIN32_METADATA_VALUE_("supported_os", windows10.0.17763)
#define _Windows_SupportedOS_18362_ \
    _WIN32_METADATA_VALUE_("supported_os", windows10.0.18362)
#define _Windows_SupportedOS_19041_ \
    _WIN32_METADATA_VALUE_("supported_os", windows10.0.19041)
#define _Windows_SupportedOS_Server2000_ \
    _WIN32_METADATA_VALUE_("supported_os", windowsserver2000)
#define _Windows_SupportedOS_Server2003_ \
    _WIN32_METADATA_VALUE_("supported_os", windowsserver2003)
#define _Windows_SupportedOS_Server2008_ \
    _WIN32_METADATA_VALUE_("supported_os", windowsserver2008)
#define _Windows_SupportedOS_Server2012_ \
    _WIN32_METADATA_VALUE_("supported_os", windowsserver2012)
#define _Windows_SupportedOS_Server2016_ \
    _WIN32_METADATA_VALUE_("supported_os", windowsserver2016)
#else
#define _Win32_SetLastError_
#define _Win32_ImportLibrary_(library)
#define _Win32_PreserveResult_
#define _Win32_Agile_
#define _Win32_RAIIFree_(function)
#define _Win32_InvalidHandle_(value)
#define _Win32_Retained_
#define _Win32_AlsoUsableFor_(type)
#define _Win32_AssociatedEnum_(type)
#define _Win32_StructSizeField_(field)
#define _Win32_FlexibleArray_
#define _Win32_Retval_
#define _Win32_Documentation_(url)
#define _Windows_SupportedOS_Windows2000_
#define _Windows_SupportedOS_WindowsXP_
#define _Windows_SupportedOS_WindowsVista_
#define _Windows_SupportedOS_Windows7_
#define _Windows_SupportedOS_Windows8_
#define _Windows_SupportedOS_Windows81_
#define _Windows_SupportedOS_10240_
#define _Windows_SupportedOS_10586_
#define _Windows_SupportedOS_14393_
#define _Windows_SupportedOS_15063_
#define _Windows_SupportedOS_16299_
#define _Windows_SupportedOS_17134_
#define _Windows_SupportedOS_17763_
#define _Windows_SupportedOS_18362_
#define _Windows_SupportedOS_19041_
#define _Windows_SupportedOS_Server2000_
#define _Windows_SupportedOS_Server2003_
#define _Windows_SupportedOS_Server2008_
#define _Windows_SupportedOS_Server2012_
#define _Windows_SupportedOS_Server2016_
#endif
