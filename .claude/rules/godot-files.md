---
paths:
  - "**/*.{tscn,tres,uid,import}"
  - "project.godot"
  - "export_presets.cfg"
---

# Godot files

- New scenes come from `npm run new:scene`, never hand-written.
- `.tscn`, `.tres` and `project.godot` are rewritten by the editor. Hand-edit only small property changes; keep `uid`s, `ext_resource` ids and section order intact.
- References use `uid://` values from `.uid` files, `.import` files and resource headers. Never invent or change one; `npm run import` generates missing ones. Commit `.uid` and `.import` files and move them with their file. Of Godot's generated files, only the `.godot/` cache is git-ignored.
- Autoloads in `project.godot` are `Name="*uid://..."`; the name is the node path under `/root/`.
- After editing any of these, run `npm run smoke` to confirm the project still loads.
