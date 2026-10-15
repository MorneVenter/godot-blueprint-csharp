# CLAUDE.md

Godot Mono template in C# / .NET; versions are in `README.md` (Requirements). It bootstraps future games: keep it small, generic and reusable. Nothing game-specific belongs here.

## Commands

Commands are npm scripts in `package.json`, documented in `README.md`. For agents:

- `npm run check` before calling work done.
- `npm run smoke` to run the game headless. `npm start` opens a window: only when asked.

Godot comes from `GODOT_BIN` (env or git-ignored `.env`); scripts error out if it's unset. Never hardcode an install path. CI (`.github/workflows/`) has no Godot, so smoke only runs locally.

## Layout

- `core/configuration/` game-wide configuration
- `core/extensions/` `Node` extension methods
- `core/data/` plain data classes; save data nests one folder per section under `save/`
- `core/managers/` autoloads, one folder each
- `core/utilities/` pure static helpers; `core/resources/` custom resources
- `scenes/` scenes + scripts by feature; the entry scene is `run/main_scene` in `project.godot`
- `assets/` own assets; `assets/external/` third-party (licenses alongside)
- `tests/` separate NUnit project `Game.Tests.csproj`, invisible to Godot (`.gdignore`); `scripts/` Node CLI shortcuts
- `config.tres` the `GameConfig` instance; `GlobalUsings.cs` global usings

Empty folders hold a `.gitkeep`; delete it once the folder has content.

## Build

- `Game.csproj` and `Game.slnx` stay in the project root next to `project.godot`: Godot builds from there. The assembly is `Game` (`project.godot` `[dotnet]`); rename only together with it.
- `Game.slnx` builds `tests/Game.Tests.csproj` in `Debug` only, so exports never compile tests.
- `Directory.Build.props`: properties shared by every project. Don't repeat in a `.csproj` what the SDK or this file already sets.
- `Directory.Packages.props`: every NuGet version (central package management); `PackageReference`s never carry a `Version`. Analyzers for every project are `GlobalPackageReference`s there.
- `TargetFramework` stays in each `.csproj`: Godot reads `Game.csproj` directly and inserts its own if it's missing. Keep both projects on the same version.
- The Godot SDK version lives on `Game.csproj`'s `Sdk` attribute; the editor manages it.
- `global.json` pins the .NET SDK and the test platform.

## Architecture

- **Autoloads** are registered in `project.godot` under their type name (`GetAutoload<T>` throws otherwise). Reach them only with `this.GetAutoload<T>()`. Use the `/add-autoload` skill to add one.
- **Config**: `this.GetConfig()` returns `GameConfig` from `res://config.tres`. New setting: `[Export]` property in an `[ExportGroup]`, value in `config.tres`.
- **SignalBus**: global `[Signal]`s. Emit with `SignalName.X`, subscribe with `+=`, unsubscribe in `_ExitTree`.
- **SaveManager**: persists `SaveData` (`core/data/save/`) per slot. Edit `Data`, then `Save()`. New state: a section class in its own folder under `core/data/save/`, added to `SaveData` as a property initialised with `new()`. Writes stay crash-safe, and unreadable saves are set aside, never deleted. Only the Godot adapter under `storage/` touches the engine; test the rest with the fakes in `tests/fakes/`.
- **UIHolder**: one full-rect layer per `UILayer` value, higher on top. Values must stay contiguous from 0.
- **GameLocale**: the first autoload. Its constructor sets the process culture to `GameLocale.Culture` (invariant); Godot constructs autoloads in listed order, so this runs before any other game code. Tests don't run it, so always pass the culture explicitly.

## Engineering

- Every fact has one owner: code, config or a single doc. Everything else derives it or points to it. A copy kept in sync by hand is a defect that rots silently: a value, a list of names, a path, a summary of a file's contents or a "must match" rule.
- Enforce invariants mechanically (types, derivation, a test, a check in `npm run check`), not with prose asking people to keep things in sync. One guard per invariant, where the fact is used. A second check needs a failure the first one misses that actually matters.
- Fix the class, not the instance. Before fixing anything, name the general kind of failure, search for its other symptoms and fix at the level that removes the kind. If that's out of scope, fix locally and say what the general fix would be.
- Write lessons as the rule that caused the failure, with its reason, never as the failure you saw. Test: does it stay true if the specifics change?
- An example in feedback illustrates a class. Generalize from it; don't limit the fix to it.
- Generalize only along axes the evidence shows. A speculative abstraction is slop too.

## Always

- Comments: the reader is an advanced developer fluent in C# and Godot. Add one only if deleting it would lead that reader into a mistake: an engine quirk, a workaround, a constraint the code can't show. Never explain an idiom (caching, null guards, `static readonly`, LINQ), and never justify the change you just made; that goes in your reply or the commit message. When in doubt, leave it out. Before finishing, reread every comment you added against this.
- Never rely on the system culture: `GameLocale.Culture` for anything shown to players, `CultureInfo.InvariantCulture` for anything saved or machine-read (file names, save data).
- Commit `.uid` and `.import` files but never write them by hand: create the file, then `npm run import`. Move them with their file. Of Godot's generated files, only the `.godot/` and `.import/` cache folders are ignored.
- Group files into folders by concern, never one flat folder per system: an entry point on top, a subfolder per part (`save_manager/{store,storage,serialization}/`). Details in `csharp.md`.
- Keep `README.md` in sync when changing public APIs, settings or commands.
- A hook runs CSharpier on every edited `.cs`/`.csproj`/`.props`; don't format by hand.

Topic rules in `.claude/rules/` load when you touch matching files: `csharp.md`, `godot-files.md`, `tests.md`, `scripts.md`.

## Skills

Skills in `.claude/skills/` own their workflows: use one whenever it covers the task. Any new scene goes through `/add-scene`; never hand-write a `.tscn`.
