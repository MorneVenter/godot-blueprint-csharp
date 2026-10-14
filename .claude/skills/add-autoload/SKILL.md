---
name: add-autoload
description: Add a new autoload singleton (manager) to this Godot project, wired into project.godot and AutoloadExtension.
---

# Add an autoload

1. Create `core/managers/<snake_name>/<Name>.cs`: `namespace Game.Core.Managers.<Name>;`, `public partial class <Name> : Node`.
2. Run `npm run import` to generate `<Name>.cs.uid`.
3. Register it in `project.godot` under `[autoload]`: `<Name>="*uid://<uid from step 2>"`. `GameLocale` stays first; keep the rest alphabetical.
4. Add `Get<Name>(this Node node)` to `core/extensions/autoload_extension/AutoloadExtension.cs`, matching the existing accessors, with `/root/<Name>`.
5. Add a row to the README "Core Systems" table and, if it's core architecture, a line in `CLAUDE.md`.
6. Run `npm run check`.
