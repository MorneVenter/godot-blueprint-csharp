---
name: upgrade-godot
description: Upgrade the project to a new Godot version (and .NET if the new Godot requires it). Bumps every version reference, migrates breaking changes and verifies the build.
argument-hint: "<version, e.g. 4.8.0>"
arguments: [version]
disable-model-invocation: true
---

# Upgrade Godot to $version

Stop if `git status` isn't clean. Work on a branch: `git switch -c upgrade-godot-$version`.

1. **Read the migration guide.** Fetch `https://docs.godotengine.org/en/latest/tutorials/migrating/upgrading_to_godot_<major.minor>.html` for each minor version crossed. List every breaking change that touches C#, APIs used in this repo (grep for them), or `project.godot` settings. Also check whether the new version needs a newer .NET.
2. **Engine binary.** The user installs the new Godot Mono and updates `GODOT_BIN` in `.env`. Confirm with `npm run godot -- --version`.
3. **Bump versions.** Grep for the old version and update:
   - `Game.csproj`: `Godot.NET.Sdk/<version>`
   - `project.godot`: `config/features` (major.minor)
   - `README.md` requirements table, `CLAUDE.md` intro
4. **.NET (only if required).** `TargetFramework` in `Game.csproj`, `dotnet-version` in `.github/workflows/build.yml`, README, `CLAUDE.md`, and the C# version in `.claude/rules/csharp.md`.
5. **Migrate.** `npm run clean`, `npm run import`, `npm run build`. Fix compile errors and apply the breaking changes from step 1.
6. **Resave project files.** Ask the user to open the project once in the new editor and accept any upgrade prompt, so scenes and resources are saved in the new format.
7. **Dependencies.** `dotnet list package --outdated` and `dotnet tool list`; bump test packages and CSharpier if there are compatible updates.
8. Run `npm run check`. Review the diff for unexpected engine rewrites of `.tscn`/`.tres`/`project.godot`. Summarize what changed and offer to commit.
