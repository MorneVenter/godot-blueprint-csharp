# [Project Template] - 0.0.1

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

### Software

| Name       | Version |                          Link                           |
| :--------- | :------ | :-----------------------------------------------------: |
| Godot Mono | 4.2.1   |      [Download](https://godotengine.org/download)       |
| VSCode     | 1.68+   |   [Download](https://code.visualstudio.com/download)    |
| .NET SDK   | 6+      | [Download](https://dotnet.microsoft.com/en-us/download) |

### Reading

| Source                 |                                              Link                                              |
| :--------------------- | :--------------------------------------------------------------------------------------------: |
| Godot Documentation    |                        [View](https://docs.godotengine.org/en/stable/)                         |
| Godot C# Documentation | [View](https://docs.godotengine.org/en/stable/tutorials/scripting/c_sharp/c_sharp_basics.html) |

---

## 3D and 2D Scenes

This template comes with 3 helper nodes to assist with managing a mix of 3D and 2D scenes (i.e. 3D world and 2D UI). This will allow you to implement a 1920x1080 UI, but render your 3D world at a lower resolution (or use FSR).

- SceneHolder: The main viewport for your scenes. Scene2D/3D should be children of this node.
- Scene3D: Holds Node3D objects. A Scene3D should always be places before a Scene2D to allow overlays.
- Scene2D: Holds Node2D objects. A Scene2D should always be places after a Scene3D to allow overlays.

```
|-- SceneHolder
   |-- Scene3D
   |-- Scene2D
```

## Folder Structure

```
|-- Addons
|-- Assets
   |-- Editor
   |-- Media
   |   |-- Audio
   |   |-- Video
   |-- Sprites
   |-- Models
   |-- Materials
   |-- Shaders
   |   |-- Canvas
   |   |-- Spatial
   |   |-- Particle
   |-- Fonts
   |-- Scenes
   |   |-- Levels
   |   |-- Characters
   |   |-- Other
   |-- Scripts
   |   |-- Editor
   |   |-- Entities
   |   |-- Managers
   |   |-- Utilities
   |   |-- Resources
   |   |-- Other
   |-- Tests
|-- ExternalAssets
   |-- Media
   |   |-- Audio
   |   |-- Video
   |-- Sprites
   |-- Models
   |-- Materials
   |-- Shaders
   |-- Fonts
   |-- Scripts
```

| Folder            | Description                                                                                                            |
| :---------------- | :--------------------------------------------------------------------------------------------------------------------- |
| Addons            | Store all addons/plugins in this folder.                                                                               |
| Media             | Store all audio and video files here.                                                                                  |
| Sprites           | Store all 2D sprites here.                                                                                             |
| Models            | Store all 3D models here.                                                                                              |
| Materials         | Store all materials here.                                                                                              |
| Shaders           | Store all spatial, canvas or particle shaders here.                                                                    |
| Fonts             | Store all font resources here.                                                                                         |
| Scenes            | Store all scenes here.                                                                                                 |
| Scripts           | Store all scripts here.                                                                                                |
| Scripts/Editor    | For Editor specific scripts.                                                                                           |
| Scripts/Managers  | For singleton scripts used in Auto Loading.                                                                            |
| Scripts/Utilities | For utilty scripts used across the project.                                                                            |
| Scripts/Resources | Instances of DataObjects or other Godot resources.                                                                     |
| ExternalAssets    | This is a reflection of the Assets folder, but meant for external assets you want to keep seperate from your project . |

---

## Conventions

- Use line feed (LF) characters to break lines, not CRLF or CR.
- Use one line feed character at the end of each file.
- Use UTF-8 encoding without a byte order mark.
- Use spaces instead of tabs for indentation.

See the [Godot Style Guide](https://docs.godotengine.org/en/stable/tutorials/scripting/c_sharp/c_sharp_style_guide.html) for a full breakdown.

| Name                        | Rule                                                                             | Example                                         |
| :-------------------------- | :------------------------------------------------------------------------------- | :---------------------------------------------- |
| Variables                   | Use camel case. Private members start with an underscore.                        | `private int _privateVar;` or `int myInt = ...` |
| Members                     | Use pascal case.                                                                 | `public int PublicVar;`                         |
| Functions                   | Use pascal case.                                                                 | `private void MyFunction()`                     |
| Namespaces                  | Use pascal case.                                                                 | `namespace MyNameSpace`                         |
| Signals                     | Use pascal case.                                                                 | `public delegate void MySignalEventHandler();`  |
| Constants                   | Use upper snake case.                                                            | `public const int MY_CONST;`                    |
| Enums                       | Use pascal case.                                                                 | `public enum MyEnum`                            |
| Folders                     | Use pascal case.                                                                 | `FolderNameHere`                                |
| CS Files                    | Use pascal case. Class name should match file name.                              | `FileName.cs` and `public FileName`             |
| Classes                     | Use pascal case.                                                                 | `public MyClass`                                |
| Max Parameters              | A method should not have more than 5 parameters.                                 | -                                               |
| No magic numbers or strings | Do not use magic variables, rather define them as constant in a single location. | -                                               |

---

## Commit Structure

Please follow the Conventional Commits standards as well as squashing your commits into a single commit.

Specification can be found at [www.conventionalcommits.org](https://www.conventionalcommits.org/en).

How to squash commits: [https://www.git-tower.com/learn/git/faq/git-squash](https://www.git-tower.com/learn/git/faq/git-squash)

Commits should be in the following form: **type[optional scope]: description**. Examples include:

> feat: allow provided config object to extend other configs

> feat(api)!: send an email to the customer when a product is shipped

> docs: correct spelling of CHANGELOG

|       Type        | Title                    | Emoji | Release | Description                                                                                                 |
| :---------------: | ------------------------ | :---: | :-----: | ----------------------------------------------------------------------------------------------------------- |
|      `feat`       | Features                 |  ✨   | `minor` | A new feature                                                                                               |
|    `refactor`     | Code Refactoring         |  📦   | `patch` | A code change that neither fixes a bug nor adds a feature                                                   |
|      `perf`       | Performance Improvements |  🚀   | `patch` | A code change that improves performance                                                                     |
|       `fix`       | Bug Fixes                |  🐛   | `patch` | A bug Fix                                                                                                   |
|      `chore`      | Chores                   |  ♻   | `patch` | Other changes that don't modify src or test files                                                           |
|     `revert`      | Reverts                  |  🗑   | `patch` | Reverts a previous commit                                                                                   |
|      `docs`       | Documentation            |  📚   | `patch` | Documentation only changes                                                                                  |
|      `style`      | Styles                   |  💎   |    -    | Changes that do not affect the meaning of the code (white-space, formatting, missing semi-colons, etc)      |
|      `test`       | Tests                    |  🚨   |    -    | Adding missing tests or correcting existing tests                                                           |
|      `build`      | Builds                   |  🛠   | `patch` | Changes that affect the build system or external dependencies (example scopes: gulp, broccoli, npm)         |
|       `ci`        | Continuous Integrations  |  ⚙   |    -    | Changes to our CI configuration files and scripts (example scopes: Travis, Circle, BrowserStack, SauceLabs) |
| `BREAKING CHANGE` | Breaking Change          |  💔   | `major` | When a breaking change occurs on the commit. Rather use the "!" operator in conjunction with a type.        |
