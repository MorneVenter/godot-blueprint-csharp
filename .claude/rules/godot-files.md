---
paths:
  - "**/*.{tscn,tres,uid,import}"
  - "project.godot"
  - "export_presets.cfg"
---

# Godot files

- New scenes come from `npm run new:scene`, never hand-written.
- `.tscn`, `.tres` and `project.godot` are rewritten by the editor. Hand-edit only small property changes; keep `uid`s, `ext_resource` ids and section order intact.
- References use `uid://` values from `.uid` files, `.import` files and resource headers. Never invent or change one; `npm run import` generates missing ones. Commit `.import` files: they carry asset uids and import settings.
- Autoloads in `project.godot` are `Name="*uid://..."`; the name is the node path under `/root/`. They load in listed order: `GameLocale` must stay first.
- After editing any of these, run `npm run smoke` to confirm the project still loads.
