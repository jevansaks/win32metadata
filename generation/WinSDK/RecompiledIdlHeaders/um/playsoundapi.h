/********************************************************************************
*                                                                               *
* playsoundapi.h -- ApiSet Contract for api-ms-win-mm-playsound-l1-1-0          *
*                                                                               *
* Copyright (c) Microsoft Corporation. All rights reserved.                     *
*                                                                               *
********************************************************************************/

#ifdef _MSC_VER
#pragma once
#endif // _MSC_VER

#ifndef _PLAYSOUNDAPI_H_
#define _PLAYSOUNDAPI_H_

#include <apiset.h>
#include <apisetcconv.h>

#include <mmsyscom.h> // mm common definitions

#if defined(WIN32METADATA)
#include <win32metadata_annotations.h>
#endif

#ifdef __cplusplus
extern "C" {
#endif

#pragma region Desktop Family
#if WINAPI_FAMILY_PARTITION(WINAPI_PARTITION_DESKTOP)

#ifndef MMNOSOUND
/****************************************************************************

                            Sound support

****************************************************************************/

#ifdef _WIN32

WINMMAPI
BOOL
WINAPI
sndPlaySoundA(
    _In_opt_ LPCSTR pszSound,
    _In_ UINT fuSound
    );

WINMMAPI
BOOL
WINAPI
sndPlaySoundW(
    _In_opt_ LPCWSTR pszSound,
    _In_ UINT fuSound
    );

#ifdef UNICODE
#define sndPlaySound  sndPlaySoundW
#else
#define sndPlaySound  sndPlaySoundA
#endif // !UNICODE

#else
DLOAD_RET(FALSE)
BOOL WINAPI sndPlaySound(LPCSTR pszSound, UINT fuSound);
#endif

/*
 *  flag values for fuSound and fdwSound arguments on [snd]PlaySound
 */
#define SND_SYNC            0x0000  /* play synchronously (default) */
#define SND_ASYNC           0x0001  /* play asynchronously */
#define SND_NODEFAULT       0x0002  /* silence (!default) if sound not found */
#define SND_MEMORY          0x0004  /* pszSound points to a memory file */
#define SND_LOOP            0x0008  /* loop the sound until next sndPlaySound */
#define SND_NOSTOP          0x0010  /* don't stop any currently playing sound */

#define SND_NOWAIT      0x00002000L /* don't wait if the driver is busy */
#define SND_ALIAS       0x00010000L /* name is a registry alias */
#define SND_ALIAS_ID    0x00110000L /* alias is a predefined ID */
#define SND_FILENAME    0x00020000L /* name is file name */
#define SND_RESOURCE    0x00040004L /* name is resource name or atom */
#if(WINVER >= 0x0400)
#define SND_PURGE           0x0040  /* purge non-static events for task */
#define SND_APPLICATION     0x0080  /* look for application specific association */
#endif /* WINVER >= 0x0400 */
#define SND_SENTRY      0x00080000L /* Generate a SoundSentry event with this sound */
#define SND_RING        0x00100000L /* Treat this as a "ring" from a communications app - don't duck me */
#define SND_SYSTEM      0x00200000L /* Treat this as a system sound */

#ifdef WIN32METADATA
#pragma push_macro("SND_SYNC")
#pragma push_macro("SND_ASYNC")
#pragma push_macro("SND_NODEFAULT")
#pragma push_macro("SND_MEMORY")
#pragma push_macro("SND_LOOP")
#pragma push_macro("SND_NOSTOP")
#pragma push_macro("SND_NOWAIT")
#pragma push_macro("SND_ALIAS")
#pragma push_macro("SND_FILENAME")
#if(WINVER >= 0x0400)
#pragma push_macro("SND_PURGE")
#pragma push_macro("SND_APPLICATION")
#endif
#pragma push_macro("SND_SENTRY")
#pragma push_macro("SND_SYSTEM")
#undef SND_SYNC
#undef SND_ASYNC
#undef SND_NODEFAULT
#undef SND_MEMORY
#undef SND_LOOP
#undef SND_NOSTOP
#undef SND_NOWAIT
#undef SND_ALIAS
#undef SND_FILENAME
#if(WINVER >= 0x0400)
#undef SND_PURGE
#undef SND_APPLICATION
#endif
#undef SND_SENTRY
#undef SND_SYSTEM
enum class
    _Win32_metadata_associated_constant_(SND_ALIAS_ID)
    _Win32_metadata_associated_constant_(SND_RESOURCE)
    [[clang::flag_enum]]
    SND_FLAGS : int
{
    SND_SYNC = 0x0000,
    SND_ASYNC = 0x0001,
    SND_NODEFAULT = 0x0002,
    SND_MEMORY = 0x0004,
    SND_LOOP = 0x0008,
    SND_NOSTOP = 0x0010,
    SND_NOWAIT = 0x00002000,
    SND_ALIAS = 0x00010000,
    SND_FILENAME = 0x00020000,
#if(WINVER >= 0x0400)
    SND_PURGE = 0x0040,
    SND_APPLICATION = 0x0080,
#endif
    SND_SENTRY = 0x00080000,
    SND_SYSTEM = 0x00200000,
};
#pragma pop_macro("SND_SYSTEM")
#pragma pop_macro("SND_SENTRY")
#if(WINVER >= 0x0400)
#pragma pop_macro("SND_APPLICATION")
#pragma pop_macro("SND_PURGE")
#endif
#pragma pop_macro("SND_FILENAME")
#pragma pop_macro("SND_ALIAS")
#pragma pop_macro("SND_NOWAIT")
#pragma pop_macro("SND_NOSTOP")
#pragma pop_macro("SND_LOOP")
#pragma pop_macro("SND_MEMORY")
#pragma pop_macro("SND_NODEFAULT")
#pragma pop_macro("SND_ASYNC")
#pragma pop_macro("SND_SYNC")
#else
typedef DWORD SND_FLAGS;
#endif

#define SND_ALIAS_START 0           /* alias base */

#ifdef _WIN32
#define sndAlias(ch0, ch1)      (SND_ALIAS_START + (DWORD)(BYTE)(ch0) | ((DWORD)(BYTE)(ch1) << 8))

#define SND_ALIAS_SYSTEMASTERISK        sndAlias('S', '*')
#define SND_ALIAS_SYSTEMQUESTION        sndAlias('S', '?')
#define SND_ALIAS_SYSTEMHAND            sndAlias('S', 'H')
#define SND_ALIAS_SYSTEMEXIT            sndAlias('S', 'E')
#define SND_ALIAS_SYSTEMSTART           sndAlias('S', 'S')
#define SND_ALIAS_SYSTEMWELCOME         sndAlias('S', 'W')
#define SND_ALIAS_SYSTEMEXCLAMATION     sndAlias('S', '!')
#define SND_ALIAS_SYSTEMDEFAULT         sndAlias('S', 'D')

WINMMAPI
BOOL
WINAPI
PlaySoundA(
    _In_opt_ LPCSTR pszSound,
    _In_opt_ HMODULE hmod,
    _In_ SND_FLAGS fdwSound
    );

WINMMAPI
BOOL
WINAPI
PlaySoundW(
    _In_opt_ LPCWSTR pszSound,
    _In_opt_ HMODULE hmod,
    _In_ SND_FLAGS fdwSound
    );

#ifdef UNICODE
#define PlaySound  PlaySoundW
#else
#define PlaySound  PlaySoundA
#endif // !UNICODE

#else
DLOAD_RET(FALSE)
BOOL WINAPI PlaySound(LPCSTR pszSound, HMODULE hmod, DWORD fdwSound);
#endif

#endif  /* ifndef MMNOSOUND */

#endif // WINAPI_FAMILY_PARTITION(WINAPI_PARTITION_DESKTOP)
#pragma endregion

#ifdef __cplusplus
}
#endif

#endif // _PLAYSOUNDAPI_H_
