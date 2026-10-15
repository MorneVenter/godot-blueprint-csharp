# CLAUDE.md

Godot Mono template in C# / .NET; versions and the folder layout are in `README.md` (Requirements, Project Structure). It bootstraps future games: keep it small, generic and reusable. Nothing game-specific belongs here.

## Commands

Commands are npm scripts in `package.json`, documented in `README.md`. For agents:

- `npm run check` before calling work done.
- `npm run smoke` to run the game headless. `npm start` opens a window: only when asked.

Godot comes from `GODOT_BIN` (env or git-ignored `.env`); scripts error out if it's unset. Never hardcode an install path. CI (`.github/workflows/`) has no Godot, so smoke only runs locally.

## Build

- `Game.csproj` and `Game.slnx` stay in the project root next to `project.godot`: Godot builds from there. The assembly is `Game` (`project.godot` `[dotnet]`); rename only together with it.
- `Game.slnx` builds `tests/Game.Tests.csproj` in `Debug` only, so exports never compile tests.
- `Directory.Build.props`: properties shared by every project. Don't repeat in a `.csproj` what the SDK or this file already sets.
- `Directory.Packages.props`: every NuGet version (central package management); `PackageReference`s never carry a `Version`. Test packages are referenced only from `Game.Tests.csproj`, never `Game.csproj`. Analyzers for every project are `GlobalPackageReference`s there.
- `TargetFramework` stays in each `.csproj`: Godot reads `Game.csproj` directly and inserts its own if it's missing. Keep both projects on the same version.
- The Godot SDK version lives on `Game.csproj`'s `Sdk` attribute; the editor manages it.
- `global.json` pins the .NET SDK and the test platform.

## Architecture

- **Autoloads** are registered in `project.godot` under their type name (`GetAutoload<T>` throws otherwise). Reach them only with `this.GetAutoload<T>()`. Use the `/add-autoload` skill to add one.
- **Config**: `this.GetConfig()` returns the `GameConfig` in `config.tres`. New setting: `[Export]` property in the `[ExportGroup]` named after the system that owns it, value in `config.tres`.
- **SignalBus**: global `[Signal]`s. Emit with `SignalName.X`, subscribe with `+=`, unsubscribe in `_ExitTree`.
- **SaveManager**: persists `SaveData` (`core/data/save/`) per slot. `LoadSave(slot)`, edit `Data`, then `Save()`. New state: a section, per the doc comment on `SaveData`. Writes stay crash-safe, and unreadable saves are set aside, never deleted. Only the Godot adapter under `storage/` touches the engine; test the rest with the fakes in `tests/fakes/`.
- **UIHolder**: one `CanvasLayer` per `UILayer` value; the value is its `Layer`. Moving or unparenting an item re-arms its `_Ready`.
- **GameLocale**: a module initializer sets the process culture to `GameLocale.Culture` (invariant) before any game code runs.

## Engineering

- Every fact has one owner: code, config or a single doc. Everything else derives it or points to it. A copy kept in sync by hand is a defect that rots silently: a value, a list of names, a path, a summary of a file's contents or a "must match" rule.
- Enforce invariants mechanically (types, derivation, a test, a check in `npm run check`), not with prose asking people to keep things in sync. One guard per invariant, where the fact is used. A second check needs a failure the first one misses that actually matters.
- Fix the class, not the instance. Before fixing anything, name the general kind of failure, search for its other symptoms and fix at the level that removes the kind. If that's out of scope, fix locally and say what the general fix would be.
- Write lessons as the rule that caused the failure, with its reason, never as the failure you saw. Test: does it stay true if the specifics change?
- An example in feedback illustrates a class. Generalize from it; don't limit the fix to it.
- Generalize only along axes the evidence shows. A speculative abstraction is slop too.

## Always

- Never rely on the system culture: `GameLocale.Culture` for anything shown to players, `CultureInfo.InvariantCulture` for anything saved or machine-read (file names, save data).
- `.uid` and `.import` files: `.claude/rules/godot-files.md`. Any new scene goes through `/add-scene`; never hand-write a `.tscn`.
- Group files into folders by concern, never one flat folder per system: an entry point on top, a subfolder per part (`save_manager/{store,storage,serialization}/`). Details in `csharp.md`.
- Keep `README.md` in sync when changing public APIs, settings or commands.
- A hook runs CSharpier on every edited C# or MSBuild file; don't format by hand.

Rules in `.claude/rules/` load when you touch files matching their `paths`; those without `paths` always load.
