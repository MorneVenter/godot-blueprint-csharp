---
name: add-scene
description: Create a new Godot scene with its C# script (player, enemy, level, UI screen, component). Use whenever a new .tscn is needed instead of writing scene files by hand.
argument-hint: "<scenes/feature/Name.tscn> [RootNodeType]"
arguments: [path, type]
---

# Add a scene

Requested: path `$path`, root type `$type`. Fill in whatever is empty from the request:

- path: `scenes/<feature>/<Name>.tscn`; folders snake_case, name PascalCase (`scenes/player/Player.tscn`).
- type: the Godot root node type (`CharacterBody2D`, `Control`, `Node3D`); `Node` if nothing fits.

Never hand-write a new `.tscn`. Generate it:

```bash
npm run new:scene -- <path> --type <type>
```

This creates `<Name>.cs` (namespace from the folders, `partial class <Name> : <type>`), its `.uid`, and `<Name>.tscn` with its own `uid://` and the script attached.

Then:

1. Implement the script. Prefer `[Export]` properties for child nodes and tunables over `GetNode` string paths.
2. Add child nodes in the editor where possible. If hand-editing the `.tscn`, follow `.claude/rules/godot-files.md`.
3. UI scenes: root is a `Control`; show them via `this.GetAutoload<UIHolder>().Add(instance, UIHolder.UILayer.<Layer>)`.
4. Reference the scene by its `uid://` (shown in the output), not its path.
5. Run `npm run check`, then `npm run smoke -- --scene <uid>` to load the new scene directly.
