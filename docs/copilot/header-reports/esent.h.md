# `esent.h`

- **Status:** blocked
- **Partitions:** Storage.Jet
- **Delta:** No retained patch previously existed. Header-scraping investigation (x86
  scrape; this partition is **not** in `ExcludeFromCrossarch`, output is
  architecture-specific under `obj/generated/x86/`) succeeded with 0 warnings/errors — the
  header parses and scrapes cleanly as-is. The header declares 229 Extensible Storage
  Engine (ESE/JET Blue) functions across four pointer/integer-sized identifier types:
  `JET_INSTANCE` (`typedef JET_API_PTR JET_INSTANCE;`), `JET_SESID`, `JET_TABLEID` (both
  also `JET_API_PTR`), and `JET_DBID` (`typedef JET_UINT32 JET_DBID;`). None use
  `DECLARE_HANDLE`, and there is no single 1:1 open/close function pair per type — JET has
  many producer functions per identifier kind (e.g. `JetCreateInstance`/
  `JetCreateInstance2`/`JetCreateInstance3` all produce `JET_INSTANCE`;
  `JetBeginSession`/`JetDupSession` produce `JET_SESID`; `JetOpenTable`/`JetOpenTempTable`/
  `JetOpenTempTable2`/`JetOpenTempTable3`/`JetCreateTableColumnIndex*` variants all produce
  `JET_TABLEID`) and multiple consumer/close functions per type
  (`JetEndSession`/`JetTerm`/`JetTerm2` for instances/sessions;
  `JetCloseTable`/`JetCloseDatabase` for tables/databases), with cross-referencing
  semantics (e.g. a `JET_TABLEID` is scoped to a `JET_SESID`) that require careful,
  authoritative per-function research to annotate correctly without introducing incorrect
  ownership metadata.
- **Blocker:** Correctly mapping every JET producer/consumer function to the right
  `_Win32_metadata_raii_free_` target (and confirming which functions are genuine
  "producers" vs. merely receiving an existing identifier) requires a dedicated,
  authoritative pass through the ESE documentation (`learn.microsoft.com/windows/win32/extensible-storage-engine/`)
  function-by-function — this is a large, standalone task (229 functions, 4 handle kinds,
  many-to-many open/close relationships) that risks introducing incorrect ownership
  metadata if rushed. Deferred to its own dedicated queue entry rather than guessed at or
  silently skipped, per the ledger's own stop condition ("Ambiguous ownership semantics
  requiring spec decision").
- **Resolution:** Not yet resolved; requires a dedicated future session focused solely on
  this header's JET handle-ownership graph.
- **Artifact:** none yet.
- **Evidence:** Live scrape (`dotnet build generation/WinSDK -c Release -p:ScanArch=x86
  -t:ScrapeHeaders -p:PartitionFilter=Storage.Jet`) succeeds with 0 warnings/errors,
  confirming the header itself has no parser/build issues — the blocker is scope/research
  complexity for ownership annotation, not a technical defect.
