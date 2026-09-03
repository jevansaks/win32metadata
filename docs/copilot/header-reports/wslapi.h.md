# Header Report: wslapi.h

## Partitions
`Wsl`

## Scrape validation
- Re-scraped `Wsl` partition (`ScanArch=x86`) after touching `main.cpp`.
- Result: `Build succeeded. 0 Warning(s). 0 Error(s).`

## Ownership audit (producer-site-only policy) — BLOCKED (variant of known blocker class)

- `WslLaunch(..., _In_ HANDLE stdIn, _In_ HANDLE stdOut, _In_ HANDLE stdErr, _Out_ HANDLE* process)`
  genuinely **produces** a process `HANDLE` via a direct out-parameter (`stdIn`/`stdOut`/`stdErr` are
  caller-supplied inputs).
- Unlike `appnotify.h`/`packagevirtualizationcontext.h` (distinctly-named opaque handle types), the
  produced type here is the **plain, generic system `HANDLE`** — the same generic/shared-type blocker
  class already documented for `resourceindexer.h` (batch `scraping-investigation-23`),
  `physicalmonitorenumerationapi.h` (`-47`), and `userenv.h` (`-50`). Annotating a bare `HANDLE`
  out-parameter would incorrectly apply ownership metadata to every `HANDLE` value anywhere in the
  published metadata — `HANDLE` is used by thousands of unrelated resource kinds (process, thread,
  event, mutex, file, etc.), each with its own close semantics; there is no single correct
  `CloseApi`/`InvalidHandleValues` pair that could be attached to the type itself. This is a direct
  out-parameter instance of the same class (rather than nested in a struct, as in `userenv.h`/
  `physicalmonitorenumerationapi.h`), but the root blocker (generic type, not distinctly named) is
  identical.
- `WslGetDistributionConfiguration`'s `_Outptr_result_buffer_(...) PSTR** defaultEnvironmentVariables`
  is a generic string-array-buffer allocation, out of scope for the same reason as
  `resourceindexer.h`'s `PWSTR` output.

## Conclusion
`blocked` — genuine process-`HANDLE`-producing out-parameter (`WslLaunch`), but the generic system
`HANDLE` type precludes producer-site annotation, same root blocker class as `resourceindexer.h`/
`physicalmonitorenumerationapi.h`/`userenv.h`.
