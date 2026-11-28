#include <win32metadata_annotations.h>
// Copyright (c) Microsoft Corporation

#pragma once

#include <diagnosticdataquerytypes.h>

DECLARE_HANDLE(HDIAGNOSTIC_DATA_QUERY_SESSION);
DECLARE_HANDLE(HDIAGNOSTIC_REPORT);
DECLARE_HANDLE(HDIAGNOSTIC_EVENT_TAG_DESCRIPTION);
DECLARE_HANDLE(HDIAGNOSTIC_EVENT_PRODUCER_DESCRIPTION);
DECLARE_HANDLE(HDIAGNOSTIC_EVENT_CATEGORY_DESCRIPTION);
DECLARE_HANDLE(HDIAGNOSTIC_RECORD);

#ifdef __cplusplus
extern "C" {
#endif // __cplusplus
_Win32_SupportedOS_19041_

// Caller is expected to destroy hSession with DdqCloseSession
STDAPI DdqCreateSession(
    _In_ DdqAccessLevel accessLevel,
    _Out_ HDIAGNOSTIC_DATA_QUERY_SESSION* hSession);

STDAPI DdqCloseSession(
    _In_ HDIAGNOSTIC_DATA_QUERY_SESSION hSession);
_Win32_SupportedOS_19041_

STDAPI DdqGetSessionAccessLevel(
    _In_ HDIAGNOSTIC_DATA_QUERY_SESSION hSession,
    _Out_ DdqAccessLevel* accessLevel);
_Win32_SupportedOS_19041_

STDAPI DdqGetDiagnosticDataAccessLevelAllowed(
    _Out_ DdqAccessLevel* accessLevel);
_Win32_SupportedOS_19041_

STDAPI DdqGetDiagnosticRecordStats(
    _In_ HDIAGNOSTIC_DATA_QUERY_SESSION hSession,
    _In_ DIAGNOSTIC_DATA_SEARCH_CRITERIA const* searchCriteria,
    _Out_ UINT32* recordCount,
    _Out_ INT64* minRowId,
    _Out_ INT64* maxRowId);
_Win32_SupportedOS_19041_

// Caller is expected to free payload with CoTaskMemFree()
STDAPI DdqGetDiagnosticRecordPayload(
    _In_ HDIAGNOSTIC_DATA_QUERY_SESSION hSession,
    _In_ INT64 rowId,
    _Out_ PCWSTR* payload);
_Win32_SupportedOS_19041_

// Invoke DdqFreeDiagnosticRecordLocaleTags() to free tagDescription
STDAPI DdqGetDiagnosticRecordLocaleTags(
    _In_ HDIAGNOSTIC_DATA_QUERY_SESSION hSession,
    _In_ PCWSTR locale,
    _Out_ HDIAGNOSTIC_EVENT_TAG_DESCRIPTION* hTagDescription);

STDAPI DdqFreeDiagnosticRecordLocaleTags(
    _In_ HDIAGNOSTIC_EVENT_TAG_DESCRIPTION hTagDescription);
_Win32_SupportedOS_19041_

STDAPI DdqGetDiagnosticRecordLocaleTagAtIndex(
    _In_ HDIAGNOSTIC_EVENT_TAG_DESCRIPTION hTagDescription,
    _In_ UINT32 index,
    _Out_ DIAGNOSTIC_DATA_EVENT_TAG_DESCRIPTION* tagDescription);
_Win32_SupportedOS_19041_

STDAPI DdqGetDiagnosticRecordLocaleTagCount(
    _In_ HDIAGNOSTIC_EVENT_TAG_DESCRIPTION hTagDescription,
    _Out_ UINT32* tagDescriptionCount);
_Win32_SupportedOS_19041_

// Invoke DdqFreeDiagnosticRecordProducers() to free producerDescription
STDAPI DdqGetDiagnosticRecordProducers(
    _In_ HDIAGNOSTIC_DATA_QUERY_SESSION hSession,
    _Out_ HDIAGNOSTIC_EVENT_PRODUCER_DESCRIPTION* hProducerDescription);

STDAPI DdqFreeDiagnosticRecordProducers(
    _In_ HDIAGNOSTIC_EVENT_PRODUCER_DESCRIPTION hProducerDescription);
_Win32_SupportedOS_19041_

STDAPI DdqGetDiagnosticRecordProducerAtIndex(
    _In_ HDIAGNOSTIC_EVENT_PRODUCER_DESCRIPTION hProducerDescription,
    _In_ UINT32 index,
    _Out_ DIAGNOSTIC_DATA_EVENT_PRODUCER_DESCRIPTION* producerDescription);
_Win32_SupportedOS_19041_

STDAPI DdqGetDiagnosticRecordProducerCount(
    _In_ HDIAGNOSTIC_EVENT_PRODUCER_DESCRIPTION hProducerDescription,
    _Out_ UINT32* producerDescriptionCount);
_Win32_SupportedOS_19041_

// Invoke DdqFreeDiagnosticRecordProducerCategories() to free categoryDescription
STDAPI DdqGetDiagnosticRecordProducerCategories(
    _In_ HDIAGNOSTIC_DATA_QUERY_SESSION hSession,
    _In_ PCWSTR producerName,
    _Out_ HDIAGNOSTIC_EVENT_CATEGORY_DESCRIPTION* hCategoryDescription);

STDAPI DdqFreeDiagnosticRecordProducerCategories(
    _In_ HDIAGNOSTIC_EVENT_CATEGORY_DESCRIPTION hCategoryDescription);
_Win32_SupportedOS_19041_

STDAPI DdqGetDiagnosticRecordCategoryAtIndex(
    _In_ HDIAGNOSTIC_EVENT_CATEGORY_DESCRIPTION hCategoryDescription,
    _In_ UINT32 index,
    _Out_ DIAGNOSTIC_DATA_EVENT_CATEGORY_DESCRIPTION* categoryDescription);
_Win32_SupportedOS_19041_

STDAPI DdqGetDiagnosticRecordCategoryCount(
    _In_ HDIAGNOSTIC_EVENT_CATEGORY_DESCRIPTION hCategoryDescription,
    _Out_ UINT32* categoryDescriptionCount);
_Win32_SupportedOS_19041_

STDAPI DdqIsDiagnosticRecordSampledIn(
    _In_ HDIAGNOSTIC_DATA_QUERY_SESSION hSession,
    _In_ const GUID* providerGroup,
    _In_opt_ const GUID* providerId,
    _In_ PCWSTR providerName,
    _In_opt_ const UINT32* eventId,
    _In_ PCWSTR eventName,
    _In_opt_ const UINT32* eventVersion,
    _In_opt_ const UINT64* eventKeywords,
    _Out_ BOOL* isSampledIn);
_Win32_SupportedOS_19041_

// Invoke DdqFreeDiagnosticRecordPage() to free record
STDAPI DdqGetDiagnosticRecordPage(
    _In_ HDIAGNOSTIC_DATA_QUERY_SESSION hSession,
    _In_ DIAGNOSTIC_DATA_SEARCH_CRITERIA* const searchCriteria,
    _In_ UINT32 offset,
    _In_ UINT32 pageRecordCount,
    _In_ INT64 baseRowId,
    _Out_ HDIAGNOSTIC_RECORD* hRecord);

STDAPI DdqFreeDiagnosticRecordPage(
    _In_ HDIAGNOSTIC_RECORD hRecord);
_Win32_SupportedOS_19041_

STDAPI DdqGetDiagnosticRecordAtIndex(
    _In_ HDIAGNOSTIC_RECORD hRecord,
    _In_ UINT32 index,
    _Out_ DIAGNOSTIC_DATA_RECORD* record);
_Win32_SupportedOS_19041_

STDAPI DdqGetDiagnosticRecordCount(
    _In_ HDIAGNOSTIC_RECORD hRecord,
    _Out_ UINT32* recordCount);
_Win32_SupportedOS_19041_

STDAPI DdqGetDiagnosticReportStoreReportCount(
    _In_ HDIAGNOSTIC_DATA_QUERY_SESSION hSession,
    _In_ UINT32 reportStoreType,
    _Out_ UINT32* reportCount);
_Win32_SupportedOS_19041_

STDAPI DdqCancelDiagnosticRecordOperation(
    _In_ HDIAGNOSTIC_DATA_QUERY_SESSION hSession);
_Win32_SupportedOS_19041_

// Invoke DdqFreeDiagnosticReport() to free report
STDAPI DdqGetDiagnosticReport(
    _In_ HDIAGNOSTIC_DATA_QUERY_SESSION hSession,
    _In_ UINT32 reportStoreType,
    _Out_ HDIAGNOSTIC_REPORT* hReport);

STDAPI DdqFreeDiagnosticReport(
    _In_ HDIAGNOSTIC_REPORT hReport);
_Win32_SupportedOS_19041_

STDAPI DdqGetDiagnosticReportAtIndex(
    _In_ HDIAGNOSTIC_REPORT hReport,
    _In_ UINT32 index,
    _Out_ DIAGNOSTIC_REPORT_DATA* report);
_Win32_SupportedOS_19041_

STDAPI DdqGetDiagnosticReportCount(
    _In_ HDIAGNOSTIC_REPORT hReport,
    _Out_ UINT32* reportCount);
_Win32_SupportedOS_19041_

STDAPI DdqExtractDiagnosticReport(
    _In_ HDIAGNOSTIC_DATA_QUERY_SESSION hSession,
    _In_ UINT32 reportStoreType,
    _In_ PCWSTR reportKey,
    _In_ PCWSTR destinationPath);
_Win32_SupportedOS_19041_

// Caller is expected to free tagStats with CoTaskMemFree()
STDAPI DdqGetDiagnosticRecordTagDistribution(
    _In_ HDIAGNOSTIC_DATA_QUERY_SESSION hSession,
    _In_reads_(producerNameCount) PCWSTR* producerNames,
    _In_ UINT32 producerNameCount,
    _Outptr_result_buffer_all_(*statCount) DIAGNOSTIC_DATA_EVENT_TAG_STATS** tagStats,
    _Out_ UINT32* statCount);
_Win32_SupportedOS_19041_

// Caller is expected to free binaryStats with CoTaskMemFree()
STDAPI DdqGetDiagnosticRecordBinaryDistribution(
    _In_ HDIAGNOSTIC_DATA_QUERY_SESSION hSession,
    _In_reads_(producerNameCount) PCWSTR* producerNames,
    _In_ UINT32 producerNameCount,
    _In_ UINT32 topNBinaries,
    _Outptr_result_buffer_all_(*statCount) DIAGNOSTIC_DATA_EVENT_BINARY_STATS** binaryStats,
    _Out_ UINT32* statCount);
_Win32_SupportedOS_19041_

STDAPI DdqGetDiagnosticRecordSummary(
    _In_ HDIAGNOSTIC_DATA_QUERY_SESSION hSession,
    _In_reads_(producerNameCount) const PCWSTR* producerNames,
    _In_ UINT32 producerNameCount,
    _Out_ DIAGNOSTIC_DATA_GENERAL_STATS* generalStats);
_Win32_SupportedOS_19041_

STDAPI DdqSetTranscriptConfiguration(
    _In_ HDIAGNOSTIC_DATA_QUERY_SESSION hSession,
    _In_ const DIAGNOSTIC_DATA_EVENT_TRANSCRIPT_CONFIGURATION* desiredConfig);
_Win32_SupportedOS_19041_

STDAPI DdqGetTranscriptConfiguration(
    _In_ HDIAGNOSTIC_DATA_QUERY_SESSION hSession,
    _Out_ DIAGNOSTIC_DATA_EVENT_TRANSCRIPT_CONFIGURATION* currentConfig);

#ifdef __cplusplus
}
#endif //__cplusplus
