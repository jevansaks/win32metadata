//
//    Copyright (C) Microsoft.  All rights reserved.
//
#pragma once

#if (NTDDI_VERSION >= NTDDI_WIN8)

#include <objidl.h> // for IStream
#include <win32metadata_annotations.h>

#pragma region Application Family or OneCore Family
#if WINAPI_FAMILY_PARTITION(WINAPI_PARTITION_APP | WINAPI_PARTITION_SYSTEM)

_Win32_metadata_supported_os_(windows8.0)
STDAPI CreateRandomAccessStreamOnFile(_In_ PCWSTR filePath, _In_ DWORD accessMode, _In_ REFIID riid, _COM_Outptr_ void **ppv);


typedef enum
{
    BSOS_DEFAULT = 0,             // when creating a byte seeker over a stream, base randomaccessstream behavior on the STGM mode from IStream::Stat.
    BSOS_PREFERDESTINATIONSTREAM  // in addition, utilize IDestinationStreamFactory::GetDestinationStream.
} BSOS_OPTIONS;

_Win32_metadata_supported_os_(windows8.0)
STDAPI CreateRandomAccessStreamOverStream(_In_ IStream *stream, _In_ BSOS_OPTIONS options, _In_ REFIID riid, _COM_Outptr_ void **ppv);
_Win32_metadata_supported_os_(windows8.0)
STDAPI CreateStreamOverRandomAccessStream(_In_ IUnknown *randomAccessStream, _In_ REFIID riid, _COM_Outptr_ void **ppv);

#ifdef __cplusplus_winrt
namespace Windows {
    namespace Storage {
        namespace Streams {
            interface class IRandomAccessStream;
        } /* end namespace */
    } /* end namespace */
} /* end namespace */

// Helper that avoids the need to cast IRandomAccessStream^ to IUnknown*.
_Win32_metadata_supported_os_(windows8.0)
__inline HRESULT CreateStreamOverRandomAccessStream(_In_ ::Windows::Storage::Streams::IRandomAccessStream ^randomAccessStream, _In_ REFIID riid, _COM_Outptr_ void **ppv)
{
    return CreateStreamOverRandomAccessStream(reinterpret_cast<IUnknown *>(randomAccessStream), riid, ppv);
}
#endif // __cplusplus_winrt


#endif /* WINAPI_FAMILY_PARTITION(WINAPI_PARTITION_APP | WINAPI_PARTITION_SYSTEM) */
#pragma endregion

#endif // (NTDDI_VERSION >= NTDDI_WIN8)

#if (NTDDI_VERSION >= NTDDI_WINTHRESHOLD)


#endif // (NTDDI_VERSION >= NTDDI_WINTHRESHOLD)

