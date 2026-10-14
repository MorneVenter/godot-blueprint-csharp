---
paths:
  - "scripts/**"
  - ".claude/hooks/**"
  - "package.json"
---

# Node scripts

- One `.mjs` per command, ES modules, Node built-ins only. No npm dependencies.
- Cross-platform: paths via `node:path`, args via `node:util` `parseArgs`, never shell out to bash.
- Call Godot through `runGodot` / `runGodotAndExit` in `scripts/lib/godot.mjs`; it resolves `GODOT_BIN`, adds `--path` and forwards the exit code.
- Anything that needs the engine API goes in a GDScript helper under `scripts/godot/`, run with `--headless --script`. `scripts/.gdignore` keeps Godot from importing them.
- Fail with `fail(message)` and a usage hint.
- New command: add it to `package.json`, the README commands table and the `CLAUDE.md` commands block.
