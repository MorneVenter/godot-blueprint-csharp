# CLAUDE.md

Godot 4.7.2 Mono template in C# 14 / .NET 10. It bootstraps future games: keep it small, generic and reusable. Nothing game-specific belongs here.

## Commands

| Command                                                | Purpose                                                            |
| :----------------------------------------------------- | :----------------------------------------------------------------- |
| `npm run check`                                        | Format check, build, test and smoke. Run before calling work done. |
| `npm run build`                                        | `dotnet build Game.slnx`                                           |
| `npm test`                                             | `dotnet test --solution Game.slnx`                                 |
| `npm run format`                                       | CSharpier (`dotnet tool restore` once first)                       |
| `npm run smoke [-- --frames N --scene <path\|uid>]`    | Headless run; fails on any engine or script error                  |
| `npm run import`                                       | Headless import; generates `.uid` files                            |
| `npm run new:scene -- <scenes/x/Name.tscn> --type <T>` | Scene + script with uids                                           |
| `npm run build:godot`                                  | Build through Godot                                                |
| `npm run export -- <preset> <path> [--debug\|--pack]`  | Export a build                                                     |
| `npm run clean`                                        | Delete `.godot/`, build output and test results                    |
| `npm run godot -- <args>`                              | Raw Godot CLI against the project                                  |
| `npm start [-- <scene>]`                               | Opens a window: only when asked, use smoke otherwise               |

Godot comes from `GODOT_BIN` (env or git-ignored `.env`); scripts error out if it's unset. Never hardcode an install path. CI runs restore, format check, build and test on Ubuntu.

## Layout

- `core/configuration/` `GameConfig`; `GameLocale` (first autoload)
- `core/extensions/` `Node` extension methods for config and autoloads
- `core/data/` plain data classes; `save/SaveData.cs` with one folder per section (`save/player/`)
- `core/managers/` autoloads: `FpsCounter`, `SaveManager`, `SignalBus`, `UIHolder`
- `core/utilities/` pure static helpers; `core/resources/` custom resources
- `scenes/` scenes + scripts by feature; `scenes/Main.tscn` is the entry
- `assets/` own assets; `assets/external/` third-party (licenses alongside)
- `tests/` separate NUnit project `Game.Tests.csproj`, invisible to Godot (`.gdignore`); `scripts/` Node CLI shortcuts
- `config.tres` the `GameConfig` instance; `GlobalUsings.cs` global usings

Empty folders hold a `.gitkeep`; delete it once the folder has content.

## Build

- `Game.csproj` and `Game.slnx` stay in the project root next to `project.godot`: Godot builds from there. The assembly is `Game` (`project.godot` `[dotnet]`); rename only together with it.
- `Game.slnx` builds `tests/Game.Tests.csproj` in `Debug` only, so exports never compile tests.
- `Directory.Build.props`: shared properties (nullable, implicit usings, not packable; warnings as errors when `CI=true`). Don't repeat in a `.csproj` what the SDK or this file already sets.
- `Directory.Packages.props`: every NuGet version (central package management); `PackageReference`s never carry a `Version`. Analyzers for all projects (Meziantou.Analyzer) are `GlobalPackageReference`s.
- `TargetFramework` stays in each `.csproj`: Godot reads `Game.csproj` directly and inserts its own if it's missing. Keep both projects on the same version.
- The Godot SDK version lives on `Game.csproj`'s `Sdk` attribute; the editor manages it.
- `global.json` pins the .NET SDK band and runs `dotnet test` on Microsoft.Testing.Platform.

## Architecture

- **Autoloads** are registered by UID in `project.godot`. Reach them only via `AutoloadExtension` (`this.GetSaveManager()`); its `/root/<Name>` paths must match the autoload names. Use the `/add-autoload` skill to add one.
- **Config**: `this.GetConfig()` returns `GameConfig` from `res://config.tres`. New setting: `[Export]` property in an `[ExportGroup]`, value in `config.tres`.
- **SignalBus**: global `[Signal]`s. Emit with `SignalName.X`, subscribe with `+=`, unsubscribe in `_ExitTree`.
- **SaveManager**: `SaveData` (`core/data/save/`) as encrypted JSON per slot at `user://savedata/save_NNN.save`. Edit `Data`, then `Save()`. New state: a section class in its own folder under `core/data/save/`, added to `SaveData` as a property initialised with `new()`. Writes go through a `.tmp`/`.old` swap that the next access recovers after a crash; unreadable slots are renamed `_BROKEN_<time>`, never deleted. Only `storage/file_system/GodotEncryptedFileSystem` calls Godot; test the rest (`SaveStore`, `FileSaveStorage`) with the fakes in `tests/fakes/`.
- **UIHolder**: one full-rect layer per `UILayer` value, higher on top. Values must stay contiguous from 0.
- **GameLocale**: the first autoload. Its constructor sets the process culture to `GameLocale.Culture` (invariant); Godot constructs autoloads in listed order, so this runs before any other game code. Tests don't run it, so always pass the culture explicitly.

## Always

- Comment only where code isn't self-documenting: a non-obvious why, a workaround, an engine quirk. Precise and short. Never restate the code, never filler.
- Never rely on the system culture: `GameLocale.Culture` for anything shown to players, `CultureInfo.InvariantCulture` for anything saved or machine-read (file names, save data).
- Commit `.uid` and `.import` files but never write them by hand: create the file, then `npm run import`. Move them with their file. Of Godot's generated files, only the `.godot/` and `.import/` cache folders are ignored.
- Group files into folders by concern, never one flat folder per system: an entry point on top, a subfolder per part (`save_manager/{store,storage,serialization}/`). Details in `csharp.md`.
- Keep `README.md` in sync when changing public APIs, settings or commands.
- A hook runs CSharpier on every edited `.cs`/`.csproj`/`.props`; don't format by hand.

Topic rules in `.claude/rules/` load when you touch matching files: `csharp.md`, `godot-files.md`, `tests.md`, `scripts.md`.

## Skills

| Skill                      | Invoked by     | Use for                                                              |
| :------------------------- | :------------- | :------------------------------------------------------------------- |
| `/add-scene <path> [Type]` | Claude or user | Any new scene + script. Never hand-write a `.tscn`.                  |
| `/add-autoload`            | Claude or user | A new global singleton, with its `project.godot` entry and accessor. |
| `/godot-review [scope]`    | Claude or user | Reviewing Godot C# changes for engine bugs and perf traps.           |
| `/new-game "<Name>"`       | User only      | Turning a fresh clone of the template into a game. Run once.         |
| `/upgrade-godot <version>` | User only      | Moving to a new Godot (and .NET) version.                            |
