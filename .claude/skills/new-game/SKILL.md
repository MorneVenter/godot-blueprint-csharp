---
name: new-game
description: Turn a fresh clone of this template into a new game project. Renames the project, generates a save encryption key, resets docs and removes template-only content.
argument-hint: "<Game Name>"
arguments: [name]
disable-model-invocation: true
---

# New game from template

Game name: $name. If empty, ask for it. Also ask for a one-line pitch and the copyright holder.

Stop if `git status` isn't clean.

1. `project.godot` `[application]`: set `config/name="$name"`, add `config/version="0.1.0"`, delete the `config/tags` line.
2. `package.json`: set `name` to the kebab-case game name.
3. Save key: generate one with `node -e "console.log(require('node:crypto').randomBytes(32).toString('hex'))"` and set `SaveFileEncryptionKey = "<key>"` in `config.tres` under the `[resource]` section.
4. `core/managers/signal_bus/SignalBus.cs`: delete `SampleSignal`.
5. `LICENSE`: add `Copyright (c) <current year> <holder>` below `MIT License`.
6. `README.md`: title `# $name - 0.1.0`, replace the template intro with the pitch, and remove step 4 ("Make it your game") from Getting Started and the `/new-game` row from the skills table. Keep everything else.
7. `CLAUDE.md`: replace the intro paragraph with `$name`, the pitch, and "Godot 4.x Mono, C# / .NET" keeping the current versions. Remove "Nothing game-specific belongs here".
8. Delete `.claude/skills/new-game/`: it only applies once.
9. Run `npm run check`. Show the diff summary and offer to commit.
