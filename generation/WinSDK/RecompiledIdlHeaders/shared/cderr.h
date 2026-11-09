#include <win32metadata_annotations.h>
/*****************************************************************************\
*                                                                             *
* cderr.h -     Common dialog error return codes                              *
*                                                                             *
* Version 1.0								      *
*                                                                             *
* Copyright (c) 1992-1999, Microsoft Corp.	All rights reserved.	      *
*                                                                             *
\*****************************************************************************/

#ifndef _INC_CDERR
#define _INC_CDERR

#if _MSC_VER > 1000
#pragma once
#endif

#define CDERR_DIALOGFAILURE   0xFFFF

#define CDERR_GENERALCODES     0x0000
#define CDERR_STRUCTSIZE       0x0001
#define CDERR_INITIALIZATION   0x0002
#define CDERR_NOTEMPLATE       0x0003
#define CDERR_NOHINSTANCE      0x0004
#define CDERR_LOADSTRFAILURE   0x0005
#define CDERR_FINDRESFAILURE   0x0006
#define CDERR_LOADRESFAILURE   0x0007
#define CDERR_LOCKRESFAILURE   0x0008
#define CDERR_MEMALLOCFAILURE  0x0009
#define CDERR_MEMLOCKFAILURE   0x000A
#define CDERR_NOHOOK           0x000B
#define CDERR_REGISTERMSGFAIL  0x000C

#define PDERR_PRINTERCODES     0x1000
#define PDERR_SETUPFAILURE     0x1001
#define PDERR_PARSEFAILURE     0x1002
#define PDERR_RETDEFFAILURE    0x1003
#define PDERR_LOADDRVFAILURE   0x1004
#define PDERR_GETDEVMODEFAIL   0x1005
#define PDERR_INITFAILURE      0x1006
#define PDERR_NODEVICES        0x1007
#define PDERR_NODEFAULTPRN     0x1008
#define PDERR_DNDMMISMATCH     0x1009
#define PDERR_CREATEICFAILURE  0x100A
#define PDERR_PRINTERNOTFOUND  0x100B
#define PDERR_DEFAULTDIFFERENT 0x100C

#define CFERR_CHOOSEFONTCODES  0x2000
#define CFERR_NOFONTS          0x2001
#define CFERR_MAXLESSTHANMIN   0x2002

#define FNERR_FILENAMECODES    0x3000
#define FNERR_SUBCLASSFAILURE  0x3001
#define FNERR_INVALIDFILENAME  0x3002
#define FNERR_BUFFERTOOSMALL   0x3003

#define FRERR_FINDREPLACECODES 0x4000
#define FRERR_BUFFERLENGTHZERO 0x4001

#define CCERR_CHOOSECOLORCODES 0x5000

#if defined(WIN32METADATA) && defined(__clang__) && defined(__cplusplus) && !defined(MIDL_PASS)
#include <sal.h>
_Win32_Enum_(COMMON_DLG_ERRORS, unsigned long,
    CDERR_DIALOGFAILURE,
    CDERR_GENERALCODES,
    CDERR_STRUCTSIZE,
    CDERR_INITIALIZATION,
    CDERR_NOTEMPLATE,
    CDERR_NOHINSTANCE,
    CDERR_LOADSTRFAILURE,
    CDERR_FINDRESFAILURE,
    CDERR_LOADRESFAILURE,
    CDERR_LOCKRESFAILURE,
    CDERR_MEMALLOCFAILURE,
    CDERR_MEMLOCKFAILURE,
    CDERR_NOHOOK,
    CDERR_REGISTERMSGFAIL,
    PDERR_PRINTERCODES,
    PDERR_SETUPFAILURE,
    PDERR_PARSEFAILURE,
    PDERR_RETDEFFAILURE,
    PDERR_LOADDRVFAILURE,
    PDERR_GETDEVMODEFAIL,
    PDERR_INITFAILURE,
    PDERR_NODEVICES,
    PDERR_NODEFAULTPRN,
    PDERR_DNDMMISMATCH,
    PDERR_CREATEICFAILURE,
    PDERR_PRINTERNOTFOUND,
    PDERR_DEFAULTDIFFERENT,
    CFERR_CHOOSEFONTCODES,
    CFERR_NOFONTS,
    CFERR_MAXLESSTHANMIN,
    FNERR_FILENAMECODES,
    FNERR_SUBCLASSFAILURE,
    FNERR_INVALIDFILENAME,
    FNERR_BUFFERTOOSMALL,
    FRERR_FINDREPLACECODES,
    FRERR_BUFFERLENGTHZERO,
    CCERR_CHOOSECOLORCODES);
#endif

#endif  /* !_INC_CDERR */
