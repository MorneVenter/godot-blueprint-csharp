---
name: add-autoload
description: Add a new autoload singleton (manager) to this Godot project, registered in project.godot.
---

# Add an autoload

1. Create `core/managers/<snake_name>/<Name>.cs`: `namespace Game.Core.Managers.<Name>;`, `public partial class <Name> : Node`.
2. Run `npm run import` to generate `<Name>.cs.uid`.
3. Register it in `project.godot` under `[autoload]`, named exactly like the class (`GetAutoload<T>` throws otherwise): `<Name>="*uid://<uid from step 2>"`. `GameLocale` stays first; keep the rest alphabetical.
4. Add its purpose to the README "Core Systems" table. Add a line to `CLAUDE.md` only if it brings an architectural rule.
5. Run `npm run check`.
