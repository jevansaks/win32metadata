# `mmeapi.h`

- **Status:** accepted-normalized
- **Batch:** `existing-patches-18`
- **Partitions:** Audio, Multimedia
- **Delta:** Retained artifact applies ABI-compatible semantic enum typing to the legacy
  waveform/MIDI/mixer API constants declared in this header.
- **Resolution:** No further changes needed; retained as-is.
- **Artifact:** `generation/WinSDK/patches/post-midl/mmeapi.h.zzz-multimedia-enums.patch`
- **Evidence:** `git apply --check --reverse` passes for the retained patch. Prior sequential
  x64, arm64, and x86 generation validated the annotation form (per
  `shift-left-metadata-worklog.md`).
- **Normalization:** Equivalent typedef/enum spelling preserving size, signedness, and ABI.
