# gl/glu.h

**Classification:** accepted-normalized (producer-site fixes applied)

## Summary
OpenGL Utility Library (GLU) header. Includes `gl/gl.h`. Confirmed 3
genuine opaque-struct-pointer producer/consumer handle pairs, all
declared as truly opaque forward-declared structs
(`typedef struct GLUquadric GLUquadric;` etc., no body ever defined —
the OpenGL-defined "opaque single-pointer" pattern that IS annotatable,
distinct from real multi-field transparent structs):

| Typedef         | Producer (return value)         | Consumer (single-arg free) |
|-----------------|----------------------------------|------------------------------|
| `GLUquadric`    | `gluNewQuadric()` returns `GLUquadric*` | `gluDeleteQuadric(GLUquadric*)` |
| `GLUtesselator` | `gluNewTess()` returns `GLUtesselator*` | `gluDeleteTess(GLUtesselator*)` |
| `GLUnurbs`      | `gluNewNurbsRenderer()` returns `GLUnurbs*` | `gluDeleteNurbsRenderer(GLUnurbs*)` |

`autoTypes.json` already contained `ValueType`/`NativeTypedef` entries
for all three types (declaring the opaque-struct shape) but no ownership
fields (`CloseApi`/`InvalidHandleValues`) — these were added in this
batch. Each typedef is exclusive to the OpenGL partition with exactly
one producer and one consumer function, so per-typedef `CloseApi`
annotation is unambiguous and consistent with the corrected
producer-site policy (this is not a shared/generic type like the
foundational windef.h handle families that policy targets).

## Ownership Analysis
Updated 3 existing `autoTypes.json` entries (namespace
`Windows.Win32.Graphics.OpenGL`, partition `OpenGL`) to add:
- `GLUquadric` → `CloseApi: "gluDeleteQuadric"`, `InvalidHandleValues: [0]`
- `GLUtesselator` → `CloseApi: "gluDeleteTess"`, `InvalidHandleValues: [0]`
- `GLUnurbs` → `CloseApi: "gluDeleteNurbsRenderer"`, `InvalidHandleValues: [0]`

## Validation
`ScrapeHeaders x64 OpenGL` → Build succeeded, 0 Error(s).

## Action
Producer-site fix applied via `autoTypes.json` (3 entries updated).
