#pragma once

#if defined(WIN32METADATA) && defined(__clang__)
#define _WIN32META_ANNOTATION_(text) __attribute__((annotate(text)))
#else
#define _WIN32META_ANNOTATION_(text)
#endif

#define _Win32_metadata_set_last_error_ \
    _WIN32META_ANNOTATION_("win32metadata:set_last_error")
#define _Win32_metadata_import_library_(value) \
    _WIN32META_ANNOTATION_("win32metadata:import_library=" value)
#define _Win32_metadata_supported_os_(value) \
    _WIN32META_ANNOTATION_("win32metadata:supported_os=" #value)
#define _Win32_metadata_errors_as_success_ \
    _WIN32META_ANNOTATION_("win32metadata:can_return_errors_as_success")
#define _Win32_metadata_multiple_success_values_ \
    _WIN32META_ANNOTATION_("win32metadata:can_return_multiple_success_values")
#define _Win32_metadata_static_library_(value) \
    _WIN32META_ANNOTATION_("win32metadata:static_library=" value)
#define _Win32_metadata_agile_ \
    _WIN32META_ANNOTATION_("win32metadata:agile")
#define _Win32_metadata_raii_free_(function) \
    _WIN32META_ANNOTATION_("win32metadata:raii_free=" #function)
#define _Win32_metadata_invalid_handle_(value) \
    _WIN32META_ANNOTATION_("win32metadata:invalid_handle=" #value)
#define _Win32_metadata_free_with_(function) \
    _WIN32META_ANNOTATION_("win32metadata:free_with=" #function)
#define _Win32_metadata_do_not_release_ \
    _WIN32META_ANNOTATION_("win32metadata:do_not_release")
#define _Win32_metadata_not_null_terminated_ \
    _WIN32META_ANNOTATION_("win32metadata:not_null_terminated")
#define _Win32_metadata_null_null_terminated_ \
    _WIN32META_ANNOTATION_("win32metadata:null_null_terminated")
#define _Win32_metadata_retained_ \
    _WIN32META_ANNOTATION_("win32metadata:retained")
#define _Win32_metadata_ignore_if_return_(value) \
    _WIN32META_ANNOTATION_("win32metadata:ignore_if_return=" #value)
#define _Win32_metadata_array_count_param_(index) \
    _WIN32META_ANNOTATION_("win32metadata:array_count_param=" #index)
#define _Win32_metadata_array_count_(count) \
    _WIN32META_ANNOTATION_("win32metadata:array_count_const=" #count)
#define _Win32_metadata_array_count_field_(field) \
    _WIN32META_ANNOTATION_("win32metadata:array_count_field=" #field)
#define _Win32_metadata_memory_size_param_(index) \
    _WIN32META_ANNOTATION_("win32metadata:memory_size_param=" #index)
#define _Win32_metadata_also_usable_for_(type) \
    _WIN32META_ANNOTATION_("win32metadata:also_usable_for=" #type)
#define _Win32_metadata_associated_enum_(type) \
    _WIN32META_ANNOTATION_("win32metadata:associated_enum=" #type)
#define _Win32_metadata_associated_constant_(name) \
    _WIN32META_ANNOTATION_("win32metadata:associated_constant=" #name)
#define _Win32_metadata_native_inheritance_(type) \
    _WIN32META_ANNOTATION_("win32metadata:native_inheritance=" #type)
#define _Win32_metadata_struct_size_field_(field) \
    _WIN32META_ANNOTATION_("win32metadata:struct_size_field=" #field)
#define _Win32_metadata_native_encoding_(encoding) \
    _WIN32META_ANNOTATION_("win32metadata:native_encoding=" #encoding)
#define _Win32_metadata_ansi_ \
    _WIN32META_ANNOTATION_("win32metadata:ansi")
#define _Win32_metadata_unicode_ \
    _WIN32META_ANNOTATION_("win32metadata:unicode")
#define _Win32_metadata_in_ \
    _WIN32META_ANNOTATION_("win32metadata:in")
#define _Win32_metadata_out_ \
    _WIN32META_ANNOTATION_("win32metadata:out")
#define _Win32_metadata_optional_ \
    _WIN32META_ANNOTATION_("win32metadata:optional")
#define _Win32_metadata_reserved_ \
    _WIN32META_ANNOTATION_("win32metadata:reserved")
#define _Win32_metadata_retval_ \
    _WIN32META_ANNOTATION_("win32metadata:retval")
#define _Win32_metadata_com_out_ptr_ \
    _WIN32META_ANNOTATION_("win32metadata:com_out_ptr")
#define _Win32_metadata_const_ \
    _WIN32META_ANNOTATION_("win32metadata:const")
#define _Win32_metadata_canonical_name_(name) \
    _WIN32META_ANNOTATION_("win32metadata:canonical_name=" #name)
#define _Win32_metadata_reduce_pointer_level_ \
    _WIN32META_ANNOTATION_("win32metadata:reduce_pointer_level")
