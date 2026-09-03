# gl/gl.h

**Classification:** accepted-normalized (no producer-site change needed)

## Summary
Core OpenGL 1.1 API (`GL.h`). Contains 336 `WINGDIAPI`-declared functions.
All handle-like values (e.g. texture/list/buffer IDs produced by
`glGenTextures`/`glGenLists` and consumed by `glDeleteTextures`/
`glDeleteLists`) are plain `GLuint` scalars — a generic/shared numeric ID
type reused across the entire OpenGL surface, not a distinct Windows
handle typedef. This matches the established generic/shared-type blocker
class (class 2): IDs are plain integers with caller-managed arrays, not
annotatable via `autoTypes.json`/`emitter.settings.rsp`.

## Ownership Analysis
No distinct producer/consumer typedef exists. No `autoTypes.json`/
`emitter.settings.rsp` changes required.

## Action
None. Classified accepted-normalized with no patch artifact.
