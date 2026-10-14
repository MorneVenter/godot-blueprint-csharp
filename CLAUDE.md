# CLAUDE.md

Godot 4.7.2 Mono template in C# 14 / .NET 10. It bootstraps future games: keep it small, generic and reusable. Nothing game-specific belongs here.

## Commands

| Command                                                | Purpose                                                            |
| :----------------------------------------------------- | :----------------------------------------------------------------- |
| `npm run check`                                        | Format check, build, test and smoke. Run before calling work done. |
| `npm run build`                                        | `dotnet build`                                                     |
| `npm test`                                             | `dotnet test`                                                      |
| `npm run format`                                       | CSharpier (`dotnet tool restore` once first)                       |
| `npm run smoke [-- --frames N --scene <path\|uid>]`    | Headless run; fails on any engine or script error                  |
| `npm run import`                                       | Headless import; generates `.uid` files                            |
| `npm run new:scene -- <scenes/x/Name.tscn> --type <T>` | Scene + script with uids                                           |
| `npm run build:godot`                                  | Build through Godot                                                |
| `npm run export -- <preset> <path> [--debug\|--pack]`  | Export a build                                                     |
| `npm run clean`                                        | Delete `.godot/`, `bin/`, `obj/`                                   |
| `npm run godot -- <args>`                              | Raw Godot CLI against the project                                  |
| `npm start [-- <scene>]`                               | Opens a window: only when asked, use smoke otherwise               |

Godot comes from `GODOT_BIN` (env or git-ignored `.env`); scripts error out if it's unset. Never hardcode an install path. CI runs restore, format check, build and test on Ubuntu.

## Layout

- `core/configuration/` `GameConfig`, `GameLocale`
- `core/extensions/` `Node` extension methods for config and autoloads
- `core/managers/` autoloads: `FpsCounter`, `SaveManager`, `SignalBus`, `UIHolder`
- `core/utilities/` pure static helpers; `core/resources/` custom resources
- `scenes/` scenes + scripts by feature; `scenes/Main.tscn` is the entry
- `assets/` own assets; `assets/external/` third-party (licenses alongside)
- `tests/` NUnit; `scripts/` Node CLI shortcuts
- `config.tres` the `GameConfig` instance; `GlobalUsings.cs` global usings

The assembly is `Game` (`project.godot` `[dotnet]`): rename `Game.csproj`/`Game.slnx` only together with it. Empty folders hold a `.gitkeep`; delete it once the folder has content.

## Architecture

- **Autoloads** are registered by UID in `project.godot`. Reach them only via `AutoloadExtension` (`this.GetSaveManager()`); its `/root/<Name>` paths must match the autoload names. Use the `/add-autoload` skill to add one.
- **Config**: `this.GetConfig()` returns `GameConfig` from `res://config.tres`. New setting: `[Export]` property in an `[ExportGroup]`, value in `config.tres`.
- **SignalBus**: global `[Signal]`s. Emit with `SignalName.X`, subscribe with `+=`, unsubscribe in `_ExitTree`.
- **SaveManager**: encrypted Godot `Dictionary` per slot at `user://savedata/save_NNN.save`. `SaveValue` writes through immediately.
- **UIHolder**: one full-rect layer per `UILayer` value, higher on top. Values must stay contiguous from 0.
- **GameLocale**: a module initializer sets the process culture to `GameLocale.Culture` (invariant). Still pass it explicitly for stable output: threads that already exist, like the test runner's, keep their own culture.

## Always

- Comment only where code isn't self-documenting: a non-obvious why, a workaround, an engine quirk. Precise and short. Never restate the code, never filler.
- Never rely on the system culture: `GameLocale.Culture` for anything shown to players, `CultureInfo.InvariantCulture` for anything saved or machine-read (file names, save data).
- Commit `.uid` files but never write them by hand: create the file, then `npm run import`. Move a `.uid` with its file.
- Keep `README.md` in sync when changing public APIs, settings or commands.
- A hook runs CSharpier on every edited `.cs`/`.csproj`; don't format by hand.

Topic rules in `.claude/rules/` load when you touch matching files: `csharp.md`, `godot-files.md`, `tests.md`, `scripts.md`.

## Skills

| Skill                      | Invoked by     | Use for                                                              |
| :------------------------- | :------------- | :------------------------------------------------------------------- |
| `/add-scene <path> [Type]` | Claude or user | Any new scene + script. Never hand-write a `.tscn`.                  |
| `/add-autoload`            | Claude or user | A new global singleton, with its `project.godot` entry and accessor. |
| `/godot-review [scope]`    | Claude or user | Reviewing Godot C# changes for engine bugs and perf traps.           |
| `/new-game "<Name>"`       | User only      | Turning a fresh clone of the template into a game. Run once.         |
| `/upgrade-godot <version>` | User only      | Moving to a new Godot (and .NET) version.                            |
