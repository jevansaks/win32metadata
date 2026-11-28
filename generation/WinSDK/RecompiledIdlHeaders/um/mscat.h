#include <win32metadata_annotations.h>
//+-------------------------------------------------------------------------
//
//  Microsoft Windows
//
//  Copyright (C) Microsoft Corporation, 1996 - 1999
//
//  File:       mscat.h
//
//  Contents:   Microsoft Internet Security Catalog API
//
//  History:    29-Apr-1997 pberkman    created
//              09-Sep-1997 pberkman    add CATAdmin functions
//
//--------------------------------------------------------------------------


#ifndef MSCAT_H
#define MSCAT_H


#if _MSC_VER > 1000
#pragma once
#endif
#include <winapifamily.h>

#pragma region Desktop Family or Wintrust Package
#if WINAPI_FAMILY_PARTITION(WINAPI_PARTITION_DESKTOP | WINAPI_PARTITION_PKG_WINTRUST)


#include    "mssip.h"

#ifdef __cplusplus
    extern "C"
    {
#endif

/////////////////////////////////////////////////////////////////////////////
//
//  defines:
//
/////////////////////////////////////////////////////////////////////////////

#define     szOID_CATALOG_LIST                  "1.3.6.1.4.1.311.12.1.1"
#define     szOID_CATALOG_LIST_MEMBER           "1.3.6.1.4.1.311.12.1.2"
#define     szOID_CATALOG_LIST_MEMBER2          "1.3.6.1.4.1.311.12.1.3"

#define     CRYPTCAT_FILEEXT                    L"CAT"

#if (NTDDI_VERSION <= NTDDI_WIN10_RS1)
#define     CRYPTCAT_MAX_MEMBERTAG              64
#else
#define     CRYPTCAT_MAX_MEMBERTAG              128
#endif

        //
        // dwPublicVersion
        //
#define     CRYPTCAT_VERSION_1                  0x100
#define     CRYPTCAT_VERSION_2                  0x200

        //
        //  fdwOpenFlags
        //
#define     CRYPTCAT_OPEN_CREATENEW             0x00000001  // creates/overwrites
#define     CRYPTCAT_OPEN_ALWAYS                0x00000002  // opens/creates
#define     CRYPTCAT_OPEN_EXISTING              0x00000004  // opens only

#define     CRYPTCAT_OPEN_EXCLUDE_PAGE_HASHES   0x00010000  // Exclude page hashes in SPC_INDIRECT_DATA

// The above CRYPTCAT_OPEN_EXCLUDE_PAGE_HASHES takes precedence if also set.
#define     CRYPTCAT_OPEN_INCLUDE_PAGE_HASHES   0x00020000  // Include page hashes in SPC_INDIRECT_DATA
#define     CRYPTCAT_OPEN_VERIFYSIGHASH         0x10000000  // verifies the signature (not the certs!)

#define     CRYPTCAT_OPEN_NO_CONTENT_HCRYPTMSG  0x20000000  // does CryptMsgDecode without content
#define     CRYPTCAT_OPEN_SORTED                0x40000000  // does CertCreateContext(CERT_CREATE_CONTEXT_SORTED_FLAG)

#define     CRYPTCAT_OPEN_FLAGS_MASK            0xffff0000  // mask for upper word

        //
        //  fdwMemberFlags  (used internal -- do not fill)
        //

#define     CRYPTCAT_MEMBER_SORTED              0x40000000

        //
        //  dwAttrTypeAndAction
        //
#define     CRYPTCAT_ATTR_AUTHENTICATED         0x10000000
#define     CRYPTCAT_ATTR_UNAUTHENTICATED       0x20000000

#define     CRYPTCAT_ATTR_NAMEASCII             0x00000001  // ascii string
#define     CRYPTCAT_ATTR_NAMEOBJID             0x00000002  // crypt obj id

#define     CRYPTCAT_ATTR_DATAASCII             0x00010000  // do not decode simple ascii chars
#define     CRYPTCAT_ATTR_DATABASE64            0x00020000  // base 64
#define     CRYPTCAT_ATTR_DATAREPLACE           0x00040000  // this data is a replacment for an existing attr

#if (NTDDI_VERSION >= NTDDI_WIN10_RS1)
#define     CRYPTCAT_ATTR_NO_AUTO_COMPAT_ENTRY  0x01000000
#endif //#if (NTDDI_VERSION >= NTDDI_WIN10_RS1)

        //
        //  dwLocalError - CDF Parse
        //
#define     CRYPTCAT_E_AREA_HEADER              0x00000000
#define     CRYPTCAT_E_AREA_MEMBER              0x00010000
#define     CRYPTCAT_E_AREA_ATTRIBUTE           0x00020000

#define     CRYPTCAT_E_CDF_UNSUPPORTED          0x00000001
#define     CRYPTCAT_E_CDF_DUPLICATE            0x00000002
#define     CRYPTCAT_E_CDF_TAGNOTFOUND          0x00000004

#define     CRYPTCAT_E_CDF_MEMBER_FILE_PATH     0x00010001
#define     CRYPTCAT_E_CDF_MEMBER_INDIRECTDATA  0x00010002
#define     CRYPTCAT_E_CDF_MEMBER_FILENOTFOUND  0x00010004

#define     CRYPTCAT_E_CDF_BAD_GUID_CONV        0x00020001
#define     CRYPTCAT_E_CDF_ATTR_TOOFEWVALUES    0x00020002
#define     CRYPTCAT_E_CDF_ATTR_TYPECOMBO       0x00020004

        //
        // dwFlags - CryptCATAdminAddCatalog
        //

#if (NTDDI_VERSION >= NTDDI_WINBLUE)
#define     CRYPTCAT_ADDCATALOG_NONE            0x00000000
#define     CRYPTCAT_ADDCATALOG_HARDLINK        0x00000001
#endif //#if (NTDDI_VERSION >= NTDDI_WINBLUE)



/////////////////////////////////////////////////////////////////////////////
//
//  structures:
//
/////////////////////////////////////////////////////////////////////////////

#include <pshpack8.h>

typedef struct CRYPTCATSTORE_
{
    DWORD                       cbStruct;       // = sizeof(CRYPTCATSTORE)
    DWORD                       dwPublicVersion;
    LPWSTR                      pwszP7File;
    HCRYPTPROV                  hProv;
    DWORD                       dwEncodingType;
    DWORD                       fdwStoreFlags;
    HANDLE                      hReserved;      // pStack(members) (null if init/pbData) INTERNAL!

    HANDLE                      hAttrs;         // pStack(Catalog attrs) INTERNAL!

    // Following is only set if opened with CRYPTCAT_OPEN_NO_CONTENT_HCRYPTMSG
    HCRYPTMSG                   hCryptMsg;
    // Following is only set if opened with CRYPTCAT_OPEN_SORTED
    HANDLE                      hSorted;        // INTERNAL ONLY!

} CRYPTCATSTORE;

typedef struct CRYPTCATMEMBER_
{
    DWORD                       cbStruct;           // = sizeof(CRYPTCATMEMBER)
    LPWSTR                      pwszReferenceTag;
    LPWSTR                      pwszFileName;       // used only by the CDF APIs
    GUID                        gSubjectType;       // may be zeros -- see sEncodedMemberInfo
    DWORD                       fdwMemberFlags;
    struct SIP_INDIRECT_DATA_   *pIndirectData;     // may be null -- see sEncodedIndirectData
    DWORD                       dwCertVersion;      // may be zero -- see sEncodedMemberInfo
    DWORD                       dwReserved;         // used by enum -- DO NOT USE!
    HANDLE                      hReserved;          // pStack(attrs) (null if init) INTERNAL!

    CRYPT_ATTR_BLOB             sEncodedIndirectData;   // lazy decode
    CRYPT_ATTR_BLOB             sEncodedMemberInfo;     // lazy decode

} CRYPTCATMEMBER;

typedef struct CRYPTCATATTRIBUTE_
{
    DWORD                       cbStruct;           // = sizeof(CRYPTCATATTRIBUTE)
    LPWSTR                      pwszReferenceTag;
    DWORD                       dwAttrTypeAndAction;
    DWORD                       cbValue;
    BYTE                        *pbValue;           // encoded CAT_NAMEVALUE struct
    DWORD                       dwReserved;         // used by enum -- DO NOT USE!

} CRYPTCATATTRIBUTE;

typedef struct CRYPTCATCDF_
{
    DWORD                       cbStruct;           // = sizeof(CRYPTCATCDF)
    HANDLE                      hFile;
    DWORD                       dwCurFilePos;
    DWORD                       dwLastMemberOffset;
    BOOL                        fEOF;
    LPWSTR                      pwszResultDir;
    HANDLE                      hCATStore;

} CRYPTCATCDF;

typedef struct CATALOG_INFO_
{
    DWORD                       cbStruct;   // set to sizeof(CATALOG_INFO)

    _Field_z_
    WCHAR                       wszCatalogFile[MAX_PATH];

} CATALOG_INFO;

typedef HANDLE          HCATADMIN;
typedef HANDLE          HCATINFO;

#include <poppack.h>

typedef void (WINAPI *PFN_CDF_PARSE_ERROR_CALLBACK)(IN DWORD dwErrorArea,
                                                    IN DWORD dwLocalError,
                                                    IN WCHAR *pwszLine);
_Win32_SupportedOS_WindowsXP_

/////////////////////////////////////////////////////////////////////////////
//
//  Prototypes:
//
/////////////////////////////////////////////////////////////////////////////


/////////////////////////////////////////////////////////////////////////////
//
//  Open:
// --------------------------------------------------------------------------
//  Usage:
//      open the catalog for Get/Put operations.
//
//  Return:
//      INVALID_HANDLE_VALUE:           an error occured while opening Catalog
//
//  Errors:
//      ERROR_INVALID_PARAMETER:        an input parameter is incorrect
//

extern HANDLE WINAPI CryptCATOpen(_In_        LPWSTR pwszFileName,
                                  _Win32_AssociatedEnum_(CRYPTCAT_OPEN_FLAGS) IN          DWORD fdwOpenFlags,
                                  IN OPTIONAL HCRYPTPROV hProv,
                                  _Win32_AssociatedEnum_(CRYPTCAT_VERSION) IN OPTIONAL DWORD dwPublicVersion,
                                  IN OPTIONAL DWORD dwEncodingType);

// WOB #1496767
/////////////////////////////////////////////////////////////////////////////
//
//  Set or clear page-hashes bit:
// --------------------------------------------------------------------------
//  Usage:
//      set a new value into the bit that suppresses page-hash generation.
//
//  Errors:
//      ERROR_INVALID_PARAMETER:        an input parameter is incorrect
//
extern void WINAPI
CryptCATSetCreatePageHashesFlag( IN HANDLE hCatalog, IN BOOL newCreatePageHashesFlag );
_Win32_SupportedOS_WindowsXP_

/////////////////////////////////////////////////////////////////////////////
//
//  Close:
// --------------------------------------------------------------------------
//  Usage:
//      close the catalog handle.
//
//  Errors:
//      ERROR_INVALID_PARAMETER:        an input parameter is incorrect
//
extern BOOL WINAPI CryptCATClose(IN HANDLE hCatalog);
_Win32_SupportedOS_WindowsXP_

/////////////////////////////////////////////////////////////////////////////
//
//  StoreFromHandle:
// --------------------------------------------------------------------------
//  Usage:
//      retrieve the CRYPTCATSTORE from the store handle.
//
//          *** DO NOT FREE THE POINTER OR ANY OF ITS MEMBERS! ***
//
//  Errors:
//      ERROR_INVALID_PARAMETER:        an input parameter is incorrect
//
extern CRYPTCATSTORE * WINAPI CryptCATStoreFromHandle(IN HANDLE hCatalog);
_Win32_SupportedOS_WindowsXP_

/////////////////////////////////////////////////////////////////////////////
//
// HandleFromStore:
// --------------------------------------------------------------------------
//  Usage:
//      retrieve the handle from a CRYPTCATSTORE pointer.
//
//  Errors:
//      ERROR_INVALID_PARAMETER:        an input parameter is incorrect
//
extern HANDLE WINAPI CryptCATHandleFromStore(IN CRYPTCATSTORE *pCatStore);
_Win32_SetLastError_
_Win32_SupportedOS_WindowsXP_


/////////////////////////////////////////////////////////////////////////////
//
//  PersistStore
// --------------------------------------------------------------------------
//  Usage:
//      Persist the information in the current Catalog Store to an unsigned
//      Catalog File. It is REQUIRED to fill in the pwszP7File member
//      of CRYPTCATSTORE prior to this call!
//
//  Errors:
//      ERROR_INVALID_PARAMETER:        an input parameter is incorrect
//
extern BOOL WINAPI CryptCATPersistStore(IN HANDLE hCatalog);


/////////////////////////////////////////////////////////////////////////////
//
//  CryptCATGetCatAttrInfo
// --------------------------------------------------------------------------
//  Usage:
//
//          *** DO NOT FREE THE POINTER OR ANY OF ITS MEMBERS! ***
//
//  Errors:
//      ERROR_INVALID_PARAMETER:        an input parameter is incorrect
//
extern CRYPTCATATTRIBUTE * WINAPI CryptCATGetCatAttrInfo(IN HANDLE hCatalog,
                                                         _In_ LPWSTR pwszReferenceTag);
_Win32_SetLastError_
_Win32_SupportedOS_WindowsXP_

/////////////////////////////////////////////////////////////////////////////
//
//  CryptCATPutCatAttrInfo
// --------------------------------------------------------------------------
//  Usage:
//      Allocates and adds the attribute to the catalog.  Returns a pointer
//      to the allocated attribute.
//
//          *** DO NOT FREE THE POINTER OR ANY OF ITS MEMBERS! ***
//
//  Errors:
//      ERROR_INVALID_PARAMETER:        an input parameter is incorrect
//
extern CRYPTCATATTRIBUTE * WINAPI CryptCATPutCatAttrInfo(IN HANDLE hCatalog,
                                                         _In_ LPWSTR pwszReferenceTag,
                                                         IN DWORD dwAttrTypeAndAction,
                                                         IN DWORD cbData,
                                                         IN BYTE *pbData);
_Win32_SupportedOS_WindowsXP_

/////////////////////////////////////////////////////////////////////////////
//
//  CryptCATEnumerateCatAttr
// --------------------------------------------------------------------------
//  Usage:
//      Enumerates through the list of attributes associated with the catalog.
//      Returns a pointer to the attribute. This return should be passed in
//      as the 'PrevAttr' to continue the enumeration.  On the first call,
//      the 'PrevAttr' should be set to NULL.
//
//          *** DO NOT FREE THE POINTER OR ANY OF ITS MEMBERS! ***
//
//  Errors:
//      ERROR_INVALID_PARAMETER:        an input parameter is incorrect
//
extern CRYPTCATATTRIBUTE * WINAPI CryptCATEnumerateCatAttr(IN HANDLE hCatalog,
                                                           IN CRYPTCATATTRIBUTE *pPrevAttr);
_Win32_SupportedOS_WindowsXP_

/////////////////////////////////////////////////////////////////////////////
//
//  CryptCATGetMemberInfo
// --------------------------------------------------------------------------
//  Usage:
//      Retrieve the Tag info (member info) structure from the catalog
//      PKCS#7, fill the CRYPTCATMEMBER structure, and return. -- Opens a
//      member context.
//
//          *** DO NOT FREE THE POINTER OR ANY OF ITS MEMBERS! ***
//
//  Errors:
//      ERROR_INVALID_PARAMETER:        an input parameter is incorrect
//
extern CRYPTCATMEMBER * WINAPI CryptCATGetMemberInfo(IN HANDLE hCatalog,
                                                     _In_ LPWSTR pwszReferenceTag);

/////////////////////////////////////////////////////////////////////////////
//
//  CryptCATAllocSortedMemberInfo
// --------------------------------------------------------------------------
//  Usage:
//      If CryptCATOpen is called with CRYPTCAT_OPEN_SORTED, this function
//      must be called instead of the above CryptCATGetMemberInfo().
//
//      The returned member context must be freed by calling
//      CryptCATFreeSortedMemberInfo()
//
//  Errors:
//      ERROR_INVALID_PARAMETER:        an input parameter is incorrect
//
extern CRYPTCATMEMBER * WINAPI CryptCATAllocSortedMemberInfo(IN HANDLE hCatalog, 
                                                     _In_ LPWSTR pwszReferenceTag);

/////////////////////////////////////////////////////////////////////////////
//
//  CryptCATFreeSortedMemberInfo
// --------------------------------------------------------------------------
//  Usage:
//      Free member context returned by the above
//      CryptCATAllocSortedMemberInfo().
//
//  Errors:
//      ERROR_INVALID_PARAMETER:        an input parameter is incorrect
//
extern void WINAPI CryptCATFreeSortedMemberInfo(IN HANDLE hCatalog, 
                                                      IN CRYPTCATMEMBER *pCatMember);
_Win32_SetLastError_
_Win32_SupportedOS_WindowsXP_

/////////////////////////////////////////////////////////////////////////////
//
//  CryptCATGetAttr:
// --------------------------------------------------------------------------
//  Usage:
//      get pwszReferenceTag attribute information for a member.
//
//  Errors:
//      ERROR_INVALID_PARAMETER:        an input parameter is incorrect
//
extern CRYPTCATATTRIBUTE * WINAPI CryptCATGetAttrInfo(IN HANDLE hCatalog,
                                                      IN CRYPTCATMEMBER *pCatMember,
                                                      _In_ LPWSTR pwszReferenceTag);
_Win32_SetLastError_
_Win32_SupportedOS_WindowsXP_

/////////////////////////////////////////////////////////////////////////////
//
//  CryptCATPutMemberInfo
// --------------------------------------------------------------------------
//  Usage:
//      Allocates and adds the member to the catalog.  Returns a pointer
//      to the allocated member.
//
//          *** DO NOT FREE THE POINTER OR ANY OF ITS MEMBERS! ***
//
//  Errors:
//      ERROR_INVALID_PARAMETER:        an input parameter is incorrect
//      CRYPT_E_EXISTS:                 the reference tag already exists
//      CRYPT_E_NOT_FOUND:              the attr was not found
//
extern CRYPTCATMEMBER * WINAPI CryptCATPutMemberInfo(IN HANDLE hCatalog,
                                                     _In_opt_    LPWSTR pwszFileName,
                                                     _In_        LPWSTR pwszReferenceTag,
                                                     IN          GUID *pgSubjectType,
                                                     IN          DWORD dwCertVersion,
                                                     IN          DWORD cbSIPIndirectData,
                                                     IN          BYTE *pbSIPIndirectData);
_Win32_SetLastError_
_Win32_SupportedOS_WindowsXP_

/////////////////////////////////////////////////////////////////////////////
//
//  CryptCATPutAttrInfo
// --------------------------------------------------------------------------
//  Usage:
//      Allocates and adds the attribute to the member.  Returns a pointer
//      to the allocated attribute.
//
//          *** DO NOT FREE THE POINTER OR ANY OF ITS MEMBERS! ***
//
//  Errors:
//      ERROR_INVALID_PARAMETER:        an input parameter is incorrect
//
extern CRYPTCATATTRIBUTE * WINAPI CryptCATPutAttrInfo(IN HANDLE hCatalog,
                                                      IN CRYPTCATMEMBER *pCatMember,
                                                      _In_ LPWSTR pwszReferenceTag,
                                                      IN DWORD dwAttrTypeAndAction,
                                                      IN DWORD cbData,
                                                      IN BYTE *pbData);
_Win32_SupportedOS_WindowsXP_

/////////////////////////////////////////////////////////////////////////////
//
//  CryptCATEnumerateMember
// --------------------------------------------------------------------------
//  Usage:
//      Enumerates through the list of members in the store.  Returns a pointer
//      to the member. This return should be passed in as the 'PrevMember' to
//      continue the enumeration.  On the first call, the 'PrevMember' should
//      be set to NULL.
//
//          *** DO NOT FREE THE POINTER OR ANY OF ITS MEMBERS! ***
//
//  Errors:
//      ERROR_INVALID_PARAMETER:        an input parameter is incorrect
//
extern CRYPTCATMEMBER * WINAPI CryptCATEnumerateMember(IN HANDLE hCatalog,
                                                       IN CRYPTCATMEMBER *pPrevMember);
_Win32_SupportedOS_WindowsXP_

/////////////////////////////////////////////////////////////////////////////
//
//  CryptCATEnumerateAttr
// --------------------------------------------------------------------------
//  Usage:
//      Enumerates through the list of attributes associated with the member.
//      Returns a pointer to the attribute. This return should be passed in
//      as the 'PrevAttr' to continue the enumeration.  On the first call,
//      the 'PrevAttr' should be set to NULL.
//
//          *** DO NOT FREE THE POINTER OR ANY OF ITS MEMBERS! ***
//
//  Errors:
//      ERROR_INVALID_PARAMETER:        an input parameter is incorrect
//
extern CRYPTCATATTRIBUTE * WINAPI CryptCATEnumerateAttr(IN HANDLE hCatalog,
                                                        IN CRYPTCATMEMBER *pCatMember,
                                                        IN CRYPTCATATTRIBUTE *pPrevAttr);
_Win32_SupportedOS_WindowsXP_

/////////////////////////////////////////////////////////////////////////////
//
//  CryptCATCDFOpen
// --------------------------------------------------------------------------
//  Usage:
//      Opens the specified CDF file and initialized the structure
//
//  Errors:
//      ERROR_INVALID_PARAMETER:        an input parameter is incorrect
//      ERROR_FILE_NOT_FOUND:           the CDF file was not found
//
extern CRYPTCATCDF * WINAPI CryptCATCDFOpen(_In_ LPWSTR pwszFilePath,
                                            _In_opt_ PFN_CDF_PARSE_ERROR_CALLBACK pfnParseError);
_Win32_SupportedOS_WindowsXP_


/////////////////////////////////////////////////////////////////////////////
//
//  CryptCATCDFClose
// --------------------------------------------------------------------------
//  Usage:
//      Closes the CDF file and deallocates the structure
//
//  Errors:
//      ERROR_INVALID_PARAMETER:        an input parameter is incorrect
//
extern BOOL WINAPI CryptCATCDFClose(IN CRYPTCATCDF *pCDF);
_Win32_SupportedOS_WindowsXP_

/////////////////////////////////////////////////////////////////////////////
//
//  CryptCATCDFEnumCatAttributes
// --------------------------------------------------------------------------
//  Usage:
//      Enumerates Catalog level attributes within the "[CatalogFiles]"
//      section of the CDF.
//
//  Errors:
//      ERROR_INVALID_PARAMETER:        an input parameter is incorrect
//
extern CRYPTCATATTRIBUTE * WINAPI CryptCATCDFEnumCatAttributes(CRYPTCATCDF *pCDF,
                                                               CRYPTCATATTRIBUTE *pPrevAttr,
                                                                PFN_CDF_PARSE_ERROR_CALLBACK pfnParseError);

/////////////////////////////////////////////////////////////////////////////
//
//  CryptCATCDFEnumMembers
// --------------------------------------------------------------------------
//  Usage:
//      Enumerates files within the "[CatalogFiles]" section of the CDF.
//
//  Errors:
//      ERROR_INVALID_PARAMETER:        an input parameter is incorrect
//
extern CRYPTCATMEMBER * WINAPI CryptCATCDFEnumMembers(IN          CRYPTCATCDF *pCDF,
                                                      IN          CRYPTCATMEMBER *pPrevMember,
                                                      IN OPTIONAL PFN_CDF_PARSE_ERROR_CALLBACK pfnParseError);

/////////////////////////////////////////////////////////////////////////////
//
//  CryptCATCDFEnumAttributes
// --------------------------------------------------------------------------
//  Usage:
//      Enumerates the files attributes within the "[CatalogFiles]" section
//      of the CDF.
//
//  Errors:
//      ERROR_INVALID_PARAMETER:        an input parameter is incorrect
//
extern CRYPTCATATTRIBUTE *WINAPI CryptCATCDFEnumAttributes(IN          CRYPTCATCDF *pCDF,
                                                           IN          CRYPTCATMEMBER *pMember,
                                                           IN          CRYPTCATATTRIBUTE *pPrevAttr,
                                                           IN OPTIONAL PFN_CDF_PARSE_ERROR_CALLBACK pfnParseError);
_Win32_SupportedOS_WindowsXP_

//////////////////////////////////////////////////////////////////////////
//
//  IsCatalogFile
// --------------------------------------------------------------------------
//  Usage:
//      Call this function to determine if the file is a Catalog File.  Both
//      parameters are optional.  HOWEVER, one of them MUST be passed!
//
//  Return:
//      TRUE if it is.
//      FALSE if it isn't or an error occured.
//
//
//  Errors:
//      ERROR_INVALID_PARAMETER:        an input parameter is incorrect
//      ERROR_NOT_ENOUGH_MEMORY:        a memory allocation failed
//      {file errors}                   a file error occured
//
extern BOOL WINAPI      IsCatalogFile(IN OPTIONAL HANDLE hFile,
                                      _In_opt_z_ WCHAR *pwszFileName);
_Win32_SetLastError_
_Win32_SupportedOS_WindowsXP_


//////////////////////////////////////////////////////////////////////////
//
//  CryptCATAdminAcquireContext
// --------------------------------------------------------------------------
//  Usage:
//      Opens a new Admin Context for the pgSubsystem Id using the system
//      default hash algorithm.
//
//      The Guid passed in will be converted to a string and used as the
//      sub-directory under %SystemRoot%\CatRoot to store all Catalog files
//      for this app/sub-system.
//
//      If a NULL is passed in to the pgSubsystem parameter, all finds will be
//      "global" and any Adds will be under the "default" Subsystem.
//
//      All calls to CryptCATAdminEnumCatalogFromHash and
//      CryptCATAdminCalcHashFromFileHandle2 using this context will use the
//      system default hash algorithm.
//
//  Return:
//      TRUE if phCatAdmin points to a valid context.
//      FALSE if an error occurs.
//
//  Errors:
//      ERROR_INVALID_PARAMETER:        an input parameter is incorrect
//      ERROR_NOT_ENOUGH_MEMORY:        a memory allocation failed
//      ERROR_DATABASE_FAILURE:         an error occured while processing
//                                      the database.
//
//  Comments:
//          The dwFlags parameter is reserved for future use.  Must
//          be set to zero.
//
//
_Success_(return != FALSE)
extern BOOL WINAPI CryptCATAdminAcquireContext(
    _Out_       HCATADMIN   *phCatAdmin,
    _In_opt_    const GUID  *pgSubsystem,
    _Reserved_  DWORD       dwFlags
    );

//////////////////////////////////////////////////////////////////////////
//
//  CryptCATAdminAcquireContext2
// --------------------------------------------------------------------------
//  Usage:
//      Opens a new Admin Context for the pgSubsystem Id. The optional
//      hash algorithm and policy can override the system default hash
//      algorithm for this context.
//
//      The Guid passed in will be converted to a string and used as the
//      sub-directory under %SystemRoot%\CatRoot to store all Catalog files
//      for this app/sub-system.
//
//      If a NULL is passed in to the pgSubsystem parameter, all finds will be
//      "global" and any Adds will be under the "default" Subsystem.
//
//      If no hash algorithm is specified, the system default hash algorithm
//      will be used. If the system supports more than one hash algorithm,
//      the optional hash policy can be used to select a stronger algorithm
//      than the default. In all cases, if a policy is specified the hash
//      algorithm is checked againt the policy.
//
//      All calls to CryptCATAdminEnumCatalogFromHash and
//      CryptCATAdminCalcHashFromFileHandle2 using this context will use the
//      hash algorithm determined from the optionally specified algorithm and
//      policy. Calls to CryptCATAdminCalcHashFromFileHandle will continue to
//      use the system default hash algorithm.
//
//  Return:
//      TRUE if phCatAdmin points to a valid context.
//      FALSE if an error occurs.
//
//  Errors:
//      ERROR_INVALID_PARAMETER:        an input parameter is incorrect
//      ERROR_NOT_ENOUGH_MEMORY:        a memory allocation failed
//      ERROR_DATABASE_FAILURE:         an error occured while processing
//                                      the database.
//      NTE_BAD_ALGID:                  the hash algorithm does not meet the
//                                      specified policy.
//
//  Comments:
//          The dwFlags parameter is reserved for future use.  Must
//          be set to zero.
//
//  
#if (NTDDI_VERSION >= NTDDI_WIN8) 
_Win32_SetLastError_
_Win32_SupportedOS_Windows8_
_Success_(return != FALSE)
extern BOOL WINAPI CryptCATAdminAcquireContext2(
    _Out_       HCATADMIN   *phCatAdmin,
    _In_opt_    const GUID  *pgSubsystem,
    _In_opt_    PCWSTR      pwszHashAlgorithm,
    _In_opt_    PCCERT_STRONG_SIGN_PARA
                            pStrongHashPolicy,
    _Reserved_  DWORD       dwFlags
    );
#endif //#if (NTDDI_VERSION >= NTDDI_WIN8) 
_Win32_SupportedOS_WindowsXP_

//////////////////////////////////////////////////////////////////////////
//
//  CryptCATAdminReleaseContext
// --------------------------------------------------------------------------
//  Usage:
//      Releases (frees) all information related to the Admin Context
//
//  Errors:
//      ERROR_INVALID_PARAMETER:        an input parameter is incorrect
//
//  Comments:
//          The dwFlags parameter is reserved for future use.  Must
//          be set to NULL.
//
extern BOOL WINAPI      CryptCATAdminReleaseContext(IN HCATADMIN hCatAdmin,
                                                    IN DWORD dwFlags);
_Win32_SupportedOS_WindowsXP_


//////////////////////////////////////////////////////////////////////////
//
//  CryptCATAdminReleaseCatalogContext
// -----------------------------------------------------------------------
//  Usage:
//      Call this function to release memory associated with the Catalog
//      Info Context.
//
//  Errors:
//      ERROR_INVALID_PARAMETER:        an input parameter is incorrect
//
//  Comments:
//      the dwFlags parameter is reserved for future use and must be assigned
//      to NULL.
//
extern BOOL WINAPI CryptCATAdminReleaseCatalogContext(IN HCATADMIN hCatAdmin,
                                                      IN HCATINFO hCatInfo,
                                                      IN DWORD dwFlags);
_Win32_SetLastError_
_Win32_SupportedOS_WindowsXP_

//////////////////////////////////////////////////////////////////////////
//
//  CryptCATAdminEnumCatalogFromHash
// --------------------------------------------------------------------------
//  Usage:
//      Call this function to retrieve the Catalog Info handle of the Catalog
//      file that currently "points" to the specified Member Hash.
//
//      if hCatInfo is NULL, the first catalog found that contains the
//      hash will be returned.
//
//      if hCatInfo is not NULL, the content must be initialized to NULL prior
//      to going into the enum loop -- this starts the first/next search.
//      This function uses this parameter to determine the last catalog returned.
//
//      if hCatInfo is not NULL, and the loop is terminated prior to this
//      function returning NULL, the application must call
//      CryptCATAdminReleaseCatalogContext to free all memory associated with
//      ppPrevContext.
//
//      cbHash must match the size of the hash for the specified Admin Context.
//
//  Errors:
//      ERROR_INVALID_PARAMETER:        an input parameter is incorrect
//      ERROR_NOT_ENOUGH_MEMORY:        a memory allocation failed
//      ERROR_DATABASE_FAILURE:         an error occurred while processing
//                                      the database.
//
extern HCATINFO WINAPI CryptCATAdminEnumCatalogFromHash(
    _In_                HCATADMIN   hCatAdmin,
    _In_reads_bytes_(cbHash) BYTE        *pbHash,
    _In_                DWORD       cbHash,
    _Reserved_          DWORD       dwFlags,
    _Inout_opt_         HCATINFO    *phPrevCatInfo);
_Win32_SupportedOS_WindowsXP_

//////////////////////////////////////////////////////////////////////////
//
//  CryptCATAdminCalcHashFromFileHandle
// --------------------------------------------------------------------------
//  Usage:
//      Call this function to calculate the has based on an open file handle.
//
//  Return:
//      TRUE if the pbHash was filled with the calculated hash.
//      FALSE if an error occured
//
//      To obtain the size required for pbHash, set pbHash to NULL.  The
//      correct size will be returned in pcbHash, the return value will
//      be TRUE and a call to GetLastError() will equal ERROR_INSUFFICIENT_BUFFER.
//
//      The system default hash algorithm will be used to calculate the hash.
//
//  Errors:
//      ERROR_INVALID_PARAMETER:        an input parameter is incorrect
//      ERROR_INSUFFICIENT_BUFFER:      the *pbHash was not big enough.
//      ERROR_NOT_ENOUGH_MEMORY:        a memory allocation failed
//
//  Comments:
//      the dwFlags parameter is reserved for future use and must be set to
//      zero.
//
_Success_(return != FALSE)
extern BOOL WINAPI CryptCATAdminCalcHashFromFileHandle(
    _In_        HANDLE hFile,
    _Inout_     DWORD *pcbHash,
    _Out_writes_bytes_to_opt_(*pcbHash, *pcbHash) BYTE *pbHash,
    _Reserved_  DWORD dwFlags
    );

//////////////////////////////////////////////////////////////////////////
//
//  CryptCATAdminCalcHashFromFileHandle2
// --------------------------------------------------------------------------
//  Usage:
//      Call this function to calculate the has based on an open file handle.
//
//  Return:
//      TRUE if the pbHash was filled with the calculated hash.
//      FALSE if an error occured
//
//      To obtain the size required for pbHash, set pbHash to NULL.  The
//      correct size will be returned in pcbHash, the return value will
//      be TRUE and a call to GetLastError() will equal ERROR_INSUFFICIENT_BUFFER.
//
//      The hash algorithm of the specified admin context will be used to
//      calculate the hash.
//
//  Errors:
//      ERROR_INVALID_PARAMETER:        an input parameter is incorrect
//      ERROR_INSUFFICIENT_BUFFER:      the *pbHash was not big enough.
//      ERROR_NOT_ENOUGH_MEMORY:        a memory allocation failed
//
//  Comments:
//      the dwFlags parameter is reserved for future use and must be set to
//      zero.
//
#if (NTDDI_VERSION >= NTDDI_WIN8)
_Win32_SetLastError_
_Win32_SupportedOS_Windows8_
_Success_(return != FALSE)
extern BOOL WINAPI CryptCATAdminCalcHashFromFileHandle2(
    _In_        HCATADMIN   hCatAdmin,
    _In_        HANDLE      hFile,
    _Inout_     DWORD       *pcbHash,
    _Out_writes_bytes_to_opt_(*pcbHash, *pcbHash) BYTE *pbHash,
    _Reserved_  DWORD       dwFlags
    );
#endif //#if (NTDDI_VERSION >= NTDDI_WIN8)

//////////////////////////////////////////////////////////////////////////
//
//  CryptCATAdminAddCatalog
// --------------------------------------------------------------------------
//  Usage:
//      Call this function to add a catalog file to the CAT Maintenance
//      subsystem.
//
//      if the pwszSelectedBaseName is NULL, the Catalog Admin system will
//      generate a file base name for you.  Otherwise, this parameter is
//      used as the file name (base & extension only) of the copied Catalog
//      file.
//
//      Call CryptCATAdminReleaseCatalogContext to free the memory associated
//      with the Catalog Context returned if not NULL.
//
//  Return:
//      On success, the HCATINFO of the catalog that was successfully added
//      is returned.  On failure, NULL is returned.
//
//  Errors:
//      ERROR_INVALID_PARAMETER:        an input parameter is incorrect
//      ERROR_NOT_ENOUGH_MEMORY:        a memory allocation failed
//      ERROR_BAD_FORMAT:               the file is not a catalog file.
//      ERROR_DATABASE_FAILURE:         an error occurred while processing
//                                      the database.
//
//  Comments:
//      the dwFlags parameter is reserved for Windows 8 and earlier releases of
//      Windows and must be set to NULL. For later releases of Windows the
//      CRYPTCAT_ADDCATALOG_* flags may be specified as appropriate.
//

#if (NTDDI_VERSION < NTDDI_WINBLUE)
extern HCATINFO WINAPI CryptCATAdminAddCatalog(
    _In_ HCATADMIN hCatAdmin,
    _In_ PWSTR pwszCatalogFile,
    _In_opt_ PWSTR pwszSelectBaseName,
    _Reserved_ DWORD dwFlags
    );
#else
extern HCATINFO WINAPI CryptCATAdminAddCatalog(
    _In_ HCATADMIN hCatAdmin,
    _In_ PWSTR pwszCatalogFile,
    _In_opt_ PWSTR pwszSelectBaseName,
    _In_ DWORD dwFlags
    );
#endif //#if (NTDDI_VERSION < NTDDI_WINBLUE)
_Win32_SetLastError_
_Win32_SupportedOS_WindowsXP_

//////////////////////////////////////////////////////////////////////////
//
//  CryptCATAdminRemoveCatalog
// --------------------------------------------------------------------------
//  Usage:
//      Call this function to remove a catalog file from the CAT Maintenance
//      subsystem.
//
//  Return:
//      On success, TRUE is returned.  FALSE if an error occurs.
//
//  Errors:
//      ERROR_INVALID_PARAMETER:        an input parameter is incorrect
//
//  Comments:
//      pwszCatalogFile must point to a string that contains only the name
//      of the catalog file, ex. "foo.cat", and not a fully qualified path
//      name
//
extern BOOL WINAPI CryptCATAdminRemoveCatalog(IN HCATADMIN hCatAdmin,
                                              IN LPCWSTR pwszCatalogFile,
                                              IN DWORD dwFlags);
_Win32_SetLastError_
_Win32_SupportedOS_WindowsXP_

//////////////////////////////////////////////////////////////////////////
//
//  CryptCATCatalogInfoFromContext
// --------------------------------------------------------------------------
//  Usage:
//      call this function to retrieve information relating to the
//      Catalog info handle passed from the Add Catalog function.
//
//  Return:
//      On success, TRUE is returned and the CATALOG_INFO structure
//      is filled in.
//      FALSE if an error occurs.
//
//  Errors:
//      ERROR_INVALID_PARAMETER:        an input parameter is incorrect
//
//  Comments:
//      the dwFlags parameter is reserved for future use and must be assigned
//      to NULL.
//
_Success_(return != FALSE)
extern BOOL WINAPI CryptCATCatalogInfoFromContext(
    _In_    HCATINFO hCatInfo,
    _Inout_ CATALOG_INFO *psCatInfo,
    _In_    DWORD dwFlags
    );
_Win32_SetLastError_
_Win32_SupportedOS_WindowsXP_

//////////////////////////////////////////////////////////////////////////
//
//  CryptCATAdminResolveCatalogPath
// --------------------------------------------------------------------------
//  Usage:
//      call this function to retrieve the fully qualified path to the
//      catalog specified by pwszCatalogFile
//
//  Return:
//      On success, TRUE is returned and the CATALOG_INFO structure
//      is filled in.
//      FALSE if an error occurs.
//
//  Errors:
//      ERROR_INVALID_PARAMETER:        an input parameter is incorrect
//      ERROR_NOT_ENOUGH_MEMORY:        if the fully qualified path is longer
//                                      than MAX_PATH
//
//  Comments:
//      the dwFlags parameter is reserved for future use and must be assigned
//      to NULL.
//
_Success_(return != FALSE)
extern BOOL WINAPI CryptCATAdminResolveCatalogPath(
    _In_    HCATADMIN hCatAdmin,
    _In_z_  WCHAR *pwszCatalogFile,
    _Inout_ CATALOG_INFO *psCatInfo,
    _In_    DWORD dwFlags
    );

//////////////////////////////////////////////////////////////////////////
//
//  CryptCATAdminPauseServiceForBackup
// --------------------------------------------------------------------------
//  Usage:
//      call this function to pause the catalog sub-system in preparation
//      for backing up the catalog sub-systems files.
//
//  Return:
//      On success, TRUE is returned.  FALSE if an error occurs.
//
//  Errors:
//      ERROR_INVALID_PARAMETER:        an input parameter is incorrect
//      ERROR_TIMEOUT:                  if clients are accessing database files
//                                      and fail to relinquish them in a timely
//                                      manner.
//
//  Comments:
//      the dwFlags parameter is reserved for future use and must be assigned
//      to NULL.  Set fResume to FALSE when pausing the catalog service, and
//      set it to TRUE to resume service.
//
extern BOOL WINAPI CryptCATAdminPauseServiceForBackup(IN DWORD dwFlags,
                                                      IN BOOL  fResume);


#ifdef __cplusplus
}
#endif


#endif /* WINAPI_FAMILY_PARTITION(WINAPI_PARTITION_DESKTOP | WINAPI_PARTITION_PKG_WINTRUST) */
#pragma endregion

#endif // MSCAT_H

#ifndef _WIN32METADATA_MSCAT_SECURITY_SYSTEM_91_GROUPS_
#define _WIN32METADATA_MSCAT_SECURITY_SYSTEM_91_GROUPS_
_Win32_Enum_(CRYPTCAT_VERSION, unsigned int, CRYPTCAT_VERSION_1, CRYPTCAT_VERSION_2);
_Win32_FlagsEnum_(CRYPTCAT_OPEN_FLAGS, unsigned int, CRYPTCAT_OPEN_ALWAYS, CRYPTCAT_OPEN_CREATENEW, CRYPTCAT_OPEN_EXISTING, CRYPTCAT_OPEN_EXCLUDE_PAGE_HASHES, CRYPTCAT_OPEN_INCLUDE_PAGE_HASHES, CRYPTCAT_OPEN_VERIFYSIGHASH, CRYPTCAT_OPEN_NO_CONTENT_HCRYPTMSG, CRYPTCAT_OPEN_SORTED, CRYPTCAT_OPEN_FLAGS_MASK);
_Win32_AnnotateField_(CRYPTCATSTORE::fdwStoreFlags, _Win32_AssociatedEnum_(CRYPTCAT_OPEN_FLAGS));
_Win32_FlagsEnum_(CRYPTCATATTRIBUTE_FLAGS, unsigned int, CRYPTCAT_ATTR_AUTHENTICATED, CRYPTCAT_ATTR_UNAUTHENTICATED, CRYPTCAT_ATTR_NAMEASCII, CRYPTCAT_ATTR_NAMEOBJID, CRYPTCAT_ATTR_DATAASCII, CRYPTCAT_ATTR_DATABASE64, CRYPTCAT_ATTR_DATAREPLACE, CRYPTCAT_ATTR_NO_AUTO_COMPAT_ENTRY);
_Win32_AnnotateField_(CRYPTCATATTRIBUTE::dwAttrTypeAndAction, _Win32_AssociatedEnum_(CRYPTCATATTRIBUTE_FLAGS));
#endif
