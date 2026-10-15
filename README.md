# Godot Blueprint C# - 0.0.1

A starter template for Godot 4 games written in C#. It gives you a working project structure, a few core systems (saving, global signals, layered UI, config), tests, CI, cross-platform CLI shortcuts and a Claude Code setup, so a new game starts at gameplay instead of plumbing.

## Software Kit

![Blender](https://img.shields.io/badge/blender-%23F5792A.svg?style=for-the-badge&logo=blender&logoColor=white)
![Godot Engine](https://img.shields.io/badge/GODOT-%23FFFFFF.svg?style=for-the-badge&logo=godot-engine)
![Visual Studio Code](https://img.shields.io/badge/Visual%20Studio%20Code-0078d7.svg?style=for-the-badge&logo=visual-studio-code&logoColor=white)
![Vim](https://img.shields.io/badge/VIM-%2311AB00.svg?style=for-the-badge&logo=vim&logoColor=white)
![Audacity](https://img.shields.io/badge/Audacity-0000CC?style=for-the-badge&logo=audacity&logoColor=white)
![Git](https://img.shields.io/badge/git-%23F05033.svg?style=for-the-badge&logo=git&logoColor=white)
![Gimp Gnu Image Manipulation Program](https://img.shields.io/badge/Gimp-657D8B?style=for-the-badge&logo=gimp&logoColor=FFFFFF)
![Aseprite](https://img.shields.io/badge/Aseprite-FFFFFF?style=for-the-badge&logo=Aseprite&logoColor=#7D929E)

## Requirements

| Name                | Version |                          Link                           |
| :------------------ | :------ | :-----------------------------------------------------: |
| Godot Mono (.NET)   | 4.7.2   |      [Download](https://godotengine.org/download)       |
| .NET SDK            | 10.0    | [Download](https://dotnet.microsoft.com/en-us/download) |
| Node.js             | 20.12+  |       [Download](https://nodejs.org/en/download)        |
| Zed (or any editor) | 1.0+    |          [Download](https://zed.dev/download)           |

Get the **.NET** build of Godot, not the standard one: C# only works there.

Useful reading: [Godot docs](https://docs.godotengine.org/en/stable/) and [C# in Godot](https://docs.godotengine.org/en/stable/tutorials/scripting/c_sharp/c_sharp_basics.html).

---

## Getting Started

1. **Create your project** from this template: use **Use this template** on GitHub, or clone it.

2. **Point the scripts at Godot.** Copy `.env.example` to `.env` and set `GODOT_BIN` to your Godot executable, for example:

   ```bash
   GODOT_BIN=/Applications/Godot Mono.app/Contents/MacOS/Godot
   ```

   `.env` is git-ignored, so every machine keeps its own path. You can also set `GODOT_BIN` as an environment variable.

3. **Install the tools and verify everything works:**

   ```bash
   dotnet tool restore   # installs CSharpier, the C# formatter
   npm run import        # first import of all assets
   npm run check         # format check, build, tests and a headless run
   ```

4. **Make it your game.** With [Claude Code](https://claude.com/claude-code), run `/new-game "My Game"`. Otherwise, by hand:
   - Set the name in `project.godot` (`config/name`) and `package.json`, and add `config/version="0.1.0"`.
   - **Set your own `SaveFileEncryptionKey` in `config.tres`.** The template default is public.
   - Delete `SampleSignal` from `SignalBus`.
   - Add your copyright line to `LICENSE`, and update this README and `CLAUDE.md`.

5. **Open it in Godot:** Project Manager > Import > select `project.godot`. Press F5 to run, or `npm start` from a terminal.

## Everyday Workflow

| Command                                   | What it does                                                                                                                     |
| :---------------------------------------- | :------------------------------------------------------------------------------------------------------------------------------- |
| `npm start`                               | Run the game.                                                                                                                    |
| `npm start -- res://scenes/levels/X.tscn` | Run a single scene.                                                                                                              |
| `npm run new:scene -- <path> --type <T>`  | Create a scene and its C# script, e.g. `scenes/player/Player.tscn --type CharacterBody2D`.                                       |
| `npm run check`                           | Format check, build, tests and smoke run. Run before every commit.                                                               |
| `npm run build`                           | Build the C# code.                                                                                                               |
| `npm test`                                | Run the unit tests.                                                                                                              |
| `npm run format`                          | Format all C# with CSharpier. `format:check` only reports.                                                                       |
| `npm run smoke`                           | Run the game headless for 120 frames; fails on any error. `--frames N`, `--scene <path or uid>`.                                 |
| `npm run import`                          | Import new assets headless and generate missing `.uid` files.                                                                    |
| `npm run build:godot`                     | Build through Godot, exactly like the editor does.                                                                               |
| `npm run export -- <preset> <path>`       | Export a build. Add `--debug` or `--pack`. See [Exporting](#exporting).                                                          |
| `npm run clean`                           | Delete `.godot/`, build output and test results to force a full reimport and rebuild.                                            |
| `npm run godot -- <args>`                 | Run Godot with any [command line arguments](https://docs.godotengine.org/en/stable/tutorials/editor/command_line_tutorial.html). |

The commands are plain Node scripts in `scripts/` and work the same on macOS, Windows and Linux.

GitHub Actions checks formatting, builds and tests every push and pull request to `main`. Warnings fail the CI build.

### Adding Scenes and Scripts

Use `npm run new:scene` instead of creating scenes by hand: it writes the C# script with the right namespace, attaches it to a new scene and gives both a Godot `uid://`.

Every script has a `.uid` file next to it, and every imported asset (images, audio, models) has a `.import` file. **Commit both**, and move them along when you move or rename a file: they hold the `uid://` that scenes and settings reference, and each asset's import settings. When you create files outside the editor, `npm run import` generates the missing ones.

### Exporting

Create your export presets in the editor first (Project > Export, which needs the export templates installed). Then:

```bash
npm run export -- "Windows Desktop" builds/windows/MyGame.exe
npm run export -- "macOS" builds/macos/MyGame.zip --debug
```

`builds/` is git-ignored.

---

## Project Structure

| Path                       | What goes there                                                         |
| :------------------------- | :---------------------------------------------------------------------- |
| `scenes/`                  | Scenes and their scripts, grouped by feature. `Main.tscn` is the entry. |
| `core/managers/`           | Autoloads (global singletons).                                          |
| `core/configuration/`      | `GameConfig` (project settings) and `GameLocale`.                       |
| `core/data/`               | Plain data classes: `save/SaveData.cs` and a folder per save section.   |
| `core/extensions/`         | Extension methods that give any node access to config and autoloads.    |
| `core/resources/`          | Custom `Resource` types.                                                |
| `core/utilities/`          | Static helpers.                                                         |
| `assets/`                  | Your own art, audio, fonts, models and shaders.                         |
| `assets/external/`         | Third-party assets. Keep their licenses next to them.                   |
| `tests/`                   | The unit test project (see [Tests](#tests)).                            |
| `scripts/`                 | The CLI shortcuts.                                                      |
| `config.tres`              | Your project settings (see [Configuration](#configuration)).            |
| `Game.csproj`, `Game.slnx` | The C# project and solution. Godot needs both in the project root.      |
| `Directory.Build.props`    | Build settings shared by all projects (nullable, implicit usings).      |
| `Directory.Packages.props` | Every NuGet package version, in one place.                              |
| `global.json`              | Pins the .NET SDK version (10.0.x, latest patch) and the test runner.   |

Empty folders contain a `.gitkeep` so Git keeps them; delete it once the folder has content.

## Core Systems

All of these are autoloads registered in `project.godot` under their class name. Reach any of them from a node with `this.GetAutoload<T>()`, e.g. `this.GetAutoload<SaveManager>()`. It throws if `T` isn't a registered autoload or the node isn't in the tree.

| Autoload      | Purpose                                                            |
| :------------ | :----------------------------------------------------------------- |
| `SaveManager` | Encrypted save slots.                                              |
| `SignalBus`   | Global signals between unrelated nodes.                            |
| `UIHolder`    | Layered container for all UI.                                      |
| `FpsCounter`  | FPS label. Set `ShowFps` in `config.tres`, or call `Toggle(bool)`. |

Don't need one? Delete its folder and its line in `project.godot` (`[autoload]`).

`GameLocale` is also an autoload, listed first so it runs before everything else; see [Locale](#locale).

### SaveManager

Saves a `SaveData` object (`core/data/save/SaveData.cs`) as encrypted JSON, one file per slot (`user://savedata/save_001.save`). Slot 1 is loaded on startup. Edit `Data` in memory, then call `Save()` to write it. Deleting the loaded slot unloads it; call `LoadSave` before saving again.

```csharp
var saveManager = this.GetAutoload<SaveManager>();

saveManager.Data.Player.TutorialCompleted = true;
saveManager.Save();

var hasSecondSlot = saveManager.SaveExists(2); // without loading it
var result = saveManager.LoadSave(2); // switch slots: Loaded, New or BrokenSetAside
saveManager.DeleteSave(2);
var lastSaved = saveManager.LastWriteTime; // UTC; null if never saved
```

`SaveData` ships with a sample `Player` section (`Name`, `CurrentScene`, `TutorialCompleted`); replace it with your own. Each section is a class in its own folder under `core/data/save/`, added to `SaveData` initialised with `new()`, so older saves without it still load. Use `{ get; set; }`, never `init`: the JSON source generator nulls missing init-only properties, ignoring their initialiser.

```csharp
// core/data/save/inventory/InventoryData.cs
public sealed class InventoryData
{
    public int Coins { get; set; }
}

// core/data/save/SaveData.cs
public sealed class SaveData
{
    public PlayerData Player { get; set; } = new();
    public InventoryData Inventory { get; set; } = new();
}
```

Unknown fields are ignored and missing ones keep their defaults; enums are saved by name. Adding fields is safe. Renaming a property drops its saved value. Renaming an enum member or changing a type makes the save unreadable, so it gets set aside.

`Data` and its sections stay the same objects across `Save()`. `LoadSave` replaces them, so don't keep references across slot switches.

Saves are protected against crashes and bad files:

- **Crash-safe writes.** A save is written to `save_001.save.tmp` first. The old save is then renamed to `.old`, the `.tmp` file is renamed into place, and `.old` is deleted. The next time the slot is accessed, any leftover files are cleaned up, leaving either the old save or the new one.
- **Verified writes.** Godot writes encrypted files on close and doesn't report errors there, for example a full disk. Each `.tmp` file is read back and compared before it replaces the save.
- **Broken saves are set aside, not deleted.** If a slot can't be read (corrupt, wrong encryption key, invalid JSON), it is renamed to `save_001_BROKEN_<UTC time>.save`. The slot then starts empty, `LoadSave` returns `BrokenSetAside` and a warning is logged. To recover a broken save, rename it back to `save_001.save`.

Inside `core/managers/save_manager/`:

- `SaveManager.cs`: the autoload.
- `store/`: `SaveStore`, the slot logic and broken-save policy.
- `storage/`: `FileSaveStorage`, which handles file naming, crash-safe writes and recovery.
- `storage/file_system/`: `GodotEncryptedFileSystem`, the only part that calls Godot.
- `serialization/`: the JSON format.

Everything except `GodotEncryptedFileSystem` runs without the engine and is unit tested.

### SignalBus

For events that many unrelated nodes care about, like "player died" or "score changed". Declare the signal on `SignalBus`:

```csharp
[Signal]
public delegate void ScoreChangedEventHandler(int score);
```

Emit and listen from anywhere:

```csharp
this.GetAutoload<SignalBus>().EmitSignal(SignalBus.SignalName.ScoreChanged, 10);
```

```csharp
public override void _Ready() => this.GetAutoload<SignalBus>().ScoreChanged += OnScoreChanged;

public override void _ExitTree() => this.GetAutoload<SignalBus>().ScoreChanged -= OnScoreChanged;
```

**Always unsubscribe in `_ExitTree`.** Godot doesn't disconnect custom signals for you, so a freed node that's still subscribed causes errors.

### UIHolder

One `CanvasLayer` per `UIHolder.UILayer` value, higher values on top. The `World` layers follow the camera; the rest stay fixed on screen.

```csharp
var uiHolder = this.GetAutoload<UIHolder>();

uiHolder.Add(pauseMenu, UIHolder.UILayer.Menu);         // adds it, or moves it if it has a parent
uiHolder.ToggleVisible(UIHolder.UILayer.Hud, false);    // hide a whole layer
uiHolder.Unparent(pauseMenu);                           // take it out without freeing it
```

### Configuration

`config.tres` holds project-wide settings. Edit it in the Godot inspector and read it in code with `this.GetConfig()`. To add a setting, add an `[Export]` property to `core/configuration/game_config/GameConfig.cs`.

### Locale

`GameLocale` is the first autoload and makes C# format numbers and dates the same on every machine, whatever the player's system language. Without it, `1.5` prints as `1,5` on some systems. It uses the invariant culture by default; change `GameLocale.Culture` to pick another. Saved data and file names always use the invariant culture, so changing it never breaks existing saves.

For translated text, use Godot's [localization](https://docs.godotengine.org/en/stable/tutorials/i18n/internationalizing_games.html) system.

### Utilities

- `PrettyNumbers.Format(1200)` returns `"1.2K"`; also `M` for millions.
- `RotationHelper.LerpRotation(from, to, weight)` interpolates rotations per axis, taking the shortest way around.

## Tests

Unit tests are a separate [NUnit](https://docs.nunit.org/) project, `tests/Game.Tests.csproj`, that references the game project. They run on [Microsoft.Testing.Platform](https://learn.microsoft.com/dotnet/core/testing/microsoft-testing-platform-intro), the modern .NET test runner. Run them with `npm test`, or `dotnet test --solution Game.slnx`.

- **Kept out of your game:** test code and NUnit never end up in `Game.dll`, and exports skip the test project entirely.
- **Invisible to Godot:** `tests/.gdignore` stops the editor from importing the folder, so there are no `.uid` files in it.
- **Same layout as the code:** tests for `core/utilities/PrettyNumbers.cs` live in `tests/core/utilities/PrettyNumbersTests.cs`.

Results are written to `tests/.results/` (git-ignored).

Tests run without the Godot engine, so they can only cover plain C# code: no nodes, scenes or `GD` calls. Godot math types like `Vector3` are fine. Keep game logic you want to test in plain classes.

To add a package, put its version in `Directory.Packages.props` and a `<PackageReference Include="..." />` without a version in the project that uses it. Test packages go in `tests/Game.Tests.csproj`, never in `Game.csproj`.

## Code Style

The project follows the [Godot C# style guide](https://docs.godotengine.org/en/stable/tutorials/scripting/c_sharp/c_sharp_style_guide.html), with C# 14. CSharpier does all formatting: run `npm run format`. [Meziantou.Analyzer](https://github.com/meziantou/Meziantou.Analyzer) checks every build for bugs and bad practices; rules that clash with our conventions are configured in `.editorconfig`. The essentials:

| What                                                       | Style                   | Example                     |
| :--------------------------------------------------------- | :---------------------- | :-------------------------- |
| Classes, methods, properties, constants, enums, namespaces | PascalCase              | `MaxHealth`, `SpawnEnemy()` |
| Local variables and parameters                             | camelCase               | `targetPosition`            |
| Private fields                                             | `_camelCase`            | `_currentHealth`            |
| Signals                                                    | `...EventHandler`       | `HealthChangedEventHandler` |
| Folders                                                    | snake_case              | `save_manager/`             |
| Files                                                      | Named after their class | `SaveManager.cs`            |

- Two-letter acronyms stay uppercase (`UIHolder`); longer ones are words (`FpsCounter`).
- Expose data through properties, not public fields.
- No magic numbers or strings: give them a named constant.
- Comment only what the code can't say itself.
- Files use LF line endings and UTF-8 without BOM.

## Working with Claude Code

The project is set up for [Claude Code](https://claude.com/claude-code):

- `CLAUDE.md` gives Claude the project overview and rules.
- `.claude/rules/` holds detailed conventions. Most load only when Claude works on matching files.
- A hook formats every C# file Claude edits, and safe commands like build and test run without permission prompts.

Skills you can run:

| Skill                      | What it does                                                        |
| :------------------------- | :------------------------------------------------------------------ |
| `/new-game "Name"`         | Turn a fresh copy of the template into your game (run once).        |
| `/add-scene <path> [Type]` | Create a scene with its script and wire it up.                      |
| `/add-autoload`            | Add a new global singleton.                                         |
| `/godot-review`            | Review your changes for common Godot C# bugs and performance traps. |
| `/slop-review`             | Review your changes for padded docs, needless comments and bloat.   |
| `/upgrade-godot <version>` | Move the project to a newer Godot version.                          |
