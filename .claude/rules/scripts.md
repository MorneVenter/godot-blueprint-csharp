---
paths:
  - "scripts/**"
  - ".claude/hooks/**"
  - "package.json"
---

# Node scripts

- One `.mjs` per command, ES modules, Node built-ins only. No npm dependencies.
- Commands live in a folder per concern: `run/` (start the game), `project/` (create and import files), `build/` (compile, export, clean). Shared code goes in `lib/`.
- Cross-platform: paths via `node:path`, args via `node:util` `parseArgs`, never shell out to bash.
- Call Godot through `runGodot` / `runGodotAndExit` in `lib/godot.mjs`; it resolves `GODOT_BIN`, adds `--path` and forwards the exit code.
- Anything that needs the engine API goes in a GDScript helper next to the command that runs it, resolved from `import.meta.url` and run with `--headless --script`. `scripts/.gdignore` keeps Godot from importing them.
- Fail with `fail(message)` and a usage hint.
- New command: add it to `package.json` and the README commands table.
