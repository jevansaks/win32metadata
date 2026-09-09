// Copyright (c) Microsoft Corporation

#pragma once
#include <win32metadata_annotations.h>

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

// Caller is expected to destroy hSession with DdqCloseSession
_Win32_metadata_supported_os_(windows10.0.19041)
STDAPI DdqCreateSession(
    _In_ DdqAccessLevel accessLevel,
    _Out_ HDIAGNOSTIC_DATA_QUERY_SESSION* hSession);

_Win32_metadata_supported_os_(windows10.0.19041)
STDAPI DdqCloseSession(
    _In_ HDIAGNOSTIC_DATA_QUERY_SESSION hSession);

_Win32_metadata_supported_os_(windows10.0.19041)
STDAPI DdqGetSessionAccessLevel(
    _In_ HDIAGNOSTIC_DATA_QUERY_SESSION hSession,
    _Out_ DdqAccessLevel* accessLevel);

_Win32_metadata_supported_os_(windows10.0.19041)
STDAPI DdqGetDiagnosticDataAccessLevelAllowed(
    _Out_ DdqAccessLevel* accessLevel);

_Win32_metadata_supported_os_(windows10.0.19041)
STDAPI DdqGetDiagnosticRecordStats(
    _In_ HDIAGNOSTIC_DATA_QUERY_SESSION hSession,
    _In_ DIAGNOSTIC_DATA_SEARCH_CRITERIA const* searchCriteria,
    _Out_ UINT32* recordCount,
    _Out_ INT64* minRowId,
    _Out_ INT64* maxRowId);

// Caller is expected to free payload with CoTaskMemFree()
_Win32_metadata_supported_os_(windows10.0.19041)
STDAPI DdqGetDiagnosticRecordPayload(
    _In_ HDIAGNOSTIC_DATA_QUERY_SESSION hSession,
    _In_ INT64 rowId,
    _Out_ PCWSTR* payload);

// Invoke DdqFreeDiagnosticRecordLocaleTags() to free tagDescription
_Win32_metadata_supported_os_(windows10.0.19041)
STDAPI DdqGetDiagnosticRecordLocaleTags(
    _In_ HDIAGNOSTIC_DATA_QUERY_SESSION hSession,
    _In_ PCWSTR locale,
    _Out_ HDIAGNOSTIC_EVENT_TAG_DESCRIPTION* hTagDescription);

_Win32_metadata_supported_os_(windows10.0.19041)
STDAPI DdqFreeDiagnosticRecordLocaleTags(
    _In_ HDIAGNOSTIC_EVENT_TAG_DESCRIPTION hTagDescription);

_Win32_metadata_supported_os_(windows10.0.19041)
STDAPI DdqGetDiagnosticRecordLocaleTagAtIndex(
    _In_ HDIAGNOSTIC_EVENT_TAG_DESCRIPTION hTagDescription,
    _In_ UINT32 index,
    _Out_ DIAGNOSTIC_DATA_EVENT_TAG_DESCRIPTION* tagDescription);

_Win32_metadata_supported_os_(windows10.0.19041)
STDAPI DdqGetDiagnosticRecordLocaleTagCount(
    _In_ HDIAGNOSTIC_EVENT_TAG_DESCRIPTION hTagDescription,
    _Out_ UINT32* tagDescriptionCount);

// Invoke DdqFreeDiagnosticRecordProducers() to free producerDescription
_Win32_metadata_supported_os_(windows10.0.19041)
STDAPI DdqGetDiagnosticRecordProducers(
    _In_ HDIAGNOSTIC_DATA_QUERY_SESSION hSession,
    _Out_ HDIAGNOSTIC_EVENT_PRODUCER_DESCRIPTION* hProducerDescription);

_Win32_metadata_supported_os_(windows10.0.19041)
STDAPI DdqFreeDiagnosticRecordProducers(
    _In_ HDIAGNOSTIC_EVENT_PRODUCER_DESCRIPTION hProducerDescription);

_Win32_metadata_supported_os_(windows10.0.19041)
STDAPI DdqGetDiagnosticRecordProducerAtIndex(
    _In_ HDIAGNOSTIC_EVENT_PRODUCER_DESCRIPTION hProducerDescription,
    _In_ UINT32 index,
    _Out_ DIAGNOSTIC_DATA_EVENT_PRODUCER_DESCRIPTION* producerDescription);

_Win32_metadata_supported_os_(windows10.0.19041)
STDAPI DdqGetDiagnosticRecordProducerCount(
    _In_ HDIAGNOSTIC_EVENT_PRODUCER_DESCRIPTION hProducerDescription,
    _Out_ UINT32* producerDescriptionCount);

// Invoke DdqFreeDiagnosticRecordProducerCategories() to free categoryDescription
_Win32_metadata_supported_os_(windows10.0.19041)
STDAPI DdqGetDiagnosticRecordProducerCategories(
    _In_ HDIAGNOSTIC_DATA_QUERY_SESSION hSession,
    _In_ PCWSTR producerName,
    _Out_ HDIAGNOSTIC_EVENT_CATEGORY_DESCRIPTION* hCategoryDescription);

_Win32_metadata_supported_os_(windows10.0.19041)
STDAPI DdqFreeDiagnosticRecordProducerCategories(
    _In_ HDIAGNOSTIC_EVENT_CATEGORY_DESCRIPTION hCategoryDescription);

_Win32_metadata_supported_os_(windows10.0.19041)
STDAPI DdqGetDiagnosticRecordCategoryAtIndex(
    _In_ HDIAGNOSTIC_EVENT_CATEGORY_DESCRIPTION hCategoryDescription,
    _In_ UINT32 index,
    _Out_ DIAGNOSTIC_DATA_EVENT_CATEGORY_DESCRIPTION* categoryDescription);

_Win32_metadata_supported_os_(windows10.0.19041)
STDAPI DdqGetDiagnosticRecordCategoryCount(
    _In_ HDIAGNOSTIC_EVENT_CATEGORY_DESCRIPTION hCategoryDescription,
    _Out_ UINT32* categoryDescriptionCount);

_Win32_metadata_supported_os_(windows10.0.19041)
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

// Invoke DdqFreeDiagnosticRecordPage() to free record
_Win32_metadata_supported_os_(windows10.0.19041)
STDAPI DdqGetDiagnosticRecordPage(
    _In_ HDIAGNOSTIC_DATA_QUERY_SESSION hSession,
    _In_ DIAGNOSTIC_DATA_SEARCH_CRITERIA* const searchCriteria,
    _In_ UINT32 offset,
    _In_ UINT32 pageRecordCount,
    _In_ INT64 baseRowId,
    _Out_ HDIAGNOSTIC_RECORD* hRecord);

_Win32_metadata_supported_os_(windows10.0.19041)
STDAPI DdqFreeDiagnosticRecordPage(
    _In_ HDIAGNOSTIC_RECORD hRecord);

_Win32_metadata_supported_os_(windows10.0.19041)
STDAPI DdqGetDiagnosticRecordAtIndex(
    _In_ HDIAGNOSTIC_RECORD hRecord,
    _In_ UINT32 index,
    _Out_ DIAGNOSTIC_DATA_RECORD* record);

_Win32_metadata_supported_os_(windows10.0.19041)
STDAPI DdqGetDiagnosticRecordCount(
    _In_ HDIAGNOSTIC_RECORD hRecord,
    _Out_ UINT32* recordCount);

_Win32_metadata_supported_os_(windows10.0.19041)
STDAPI DdqGetDiagnosticReportStoreReportCount(
    _In_ HDIAGNOSTIC_DATA_QUERY_SESSION hSession,
    _In_ UINT32 reportStoreType,
    _Out_ UINT32* reportCount);

_Win32_metadata_supported_os_(windows10.0.19041)
STDAPI DdqCancelDiagnosticRecordOperation(
    _In_ HDIAGNOSTIC_DATA_QUERY_SESSION hSession);

// Invoke DdqFreeDiagnosticReport() to free report
_Win32_metadata_supported_os_(windows10.0.19041)
STDAPI DdqGetDiagnosticReport(
    _In_ HDIAGNOSTIC_DATA_QUERY_SESSION hSession,
    _In_ UINT32 reportStoreType,
    _Out_ HDIAGNOSTIC_REPORT* hReport);

_Win32_metadata_supported_os_(windows10.0.19041)
STDAPI DdqFreeDiagnosticReport(
    _In_ HDIAGNOSTIC_REPORT hReport);

_Win32_metadata_supported_os_(windows10.0.19041)
STDAPI DdqGetDiagnosticReportAtIndex(
    _In_ HDIAGNOSTIC_REPORT hReport,
    _In_ UINT32 index,
    _Out_ DIAGNOSTIC_REPORT_DATA* report);

_Win32_metadata_supported_os_(windows10.0.19041)
STDAPI DdqGetDiagnosticReportCount(
    _In_ HDIAGNOSTIC_REPORT hReport,
    _Out_ UINT32* reportCount);

_Win32_metadata_supported_os_(windows10.0.19041)
STDAPI DdqExtractDiagnosticReport(
    _In_ HDIAGNOSTIC_DATA_QUERY_SESSION hSession,
    _In_ UINT32 reportStoreType,
    _In_ PCWSTR reportKey,
    _In_ PCWSTR destinationPath);

// Caller is expected to free tagStats with CoTaskMemFree()
_Win32_metadata_supported_os_(windows10.0.19041)
STDAPI DdqGetDiagnosticRecordTagDistribution(
    _In_ HDIAGNOSTIC_DATA_QUERY_SESSION hSession,
    _In_reads_(producerNameCount) PCWSTR* producerNames,
    _In_ UINT32 producerNameCount,
    _Outptr_result_buffer_all_(*statCount) DIAGNOSTIC_DATA_EVENT_TAG_STATS** tagStats,
    _Out_ UINT32* statCount);

// Caller is expected to free binaryStats with CoTaskMemFree()
_Win32_metadata_supported_os_(windows10.0.19041)
STDAPI DdqGetDiagnosticRecordBinaryDistribution(
    _In_ HDIAGNOSTIC_DATA_QUERY_SESSION hSession,
    _In_reads_(producerNameCount) PCWSTR* producerNames,
    _In_ UINT32 producerNameCount,
    _In_ UINT32 topNBinaries,
    _Outptr_result_buffer_all_(*statCount) DIAGNOSTIC_DATA_EVENT_BINARY_STATS** binaryStats,
    _Out_ UINT32* statCount);

_Win32_metadata_supported_os_(windows10.0.19041)
STDAPI DdqGetDiagnosticRecordSummary(
    _In_ HDIAGNOSTIC_DATA_QUERY_SESSION hSession,
    _In_reads_(producerNameCount) const PCWSTR* producerNames,
    _In_ UINT32 producerNameCount,
    _Out_ DIAGNOSTIC_DATA_GENERAL_STATS* generalStats);

_Win32_metadata_supported_os_(windows10.0.19041)
STDAPI DdqSetTranscriptConfiguration(
    _In_ HDIAGNOSTIC_DATA_QUERY_SESSION hSession,
    _In_ const DIAGNOSTIC_DATA_EVENT_TRANSCRIPT_CONFIGURATION* desiredConfig);

_Win32_metadata_supported_os_(windows10.0.19041)
STDAPI DdqGetTranscriptConfiguration(
    _In_ HDIAGNOSTIC_DATA_QUERY_SESSION hSession,
    _Out_ DIAGNOSTIC_DATA_EVENT_TRANSCRIPT_CONFIGURATION* currentConfig);

#ifdef __cplusplus
}
#endif //__cplusplus
