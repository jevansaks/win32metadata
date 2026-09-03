# Header Report: animationcoordinator.h

## Partitions
`Shell`

## Ownership audit (producer-site-only policy)
- Declares only the COM interface `IInputPaneAnimationCoordinator` and related `ShowInputPaneAnimationCoordinator`/`HideInputPaneAnimationCoordinator` coclass forward declarations. COM vtable method — out of scope; no `extern`/`DllImport` functions.

## Conclusion
`accepted-normalized` — no ownership annotation required (COM interface method only, out of scope).
