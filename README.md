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
| Godot Mono | 4.7.2   |      [Download](https://godotengine.org/download)       |
| Zed        | 1.0+    |          [Download](https://zed.dev/download)           |
| .NET SDK   | 10.0    | [Download](https://dotnet.microsoft.com/en-us/download) |

### Reading

| Source                 |                                              Link                                              |
| :--------------------- | :--------------------------------------------------------------------------------------------: |
| Godot Documentation    |                        [View](https://docs.godotengine.org/en/stable/)                         |
| Godot C# Documentation | [View](https://docs.godotengine.org/en/stable/tutorials/scripting/c_sharp/c_sharp_basics.html) |

---

## Utility Scripts

This project includes a few utility scripts. Feel free to delete them if they are not needed. These scripts are:

- PrettyNumbers - A static class used to format numbers.
- SaveManager - A singleton save system.
- SignalBus - A shared signal bus.

Signals from the signal bus can be emitted like so: `SignalBus.EmitSignal(nameof(SignalBus.MySignal));`. The name of the event is passed instead of magic strings. To listen to a signal, simply do: `SignalBus.MySignal += func`.

## UI Holder Pattern

This project includes a top-level UI manager component: UIHolder. Simply use the `UIHolder.Add` and `UIHolder.Remove` to add and remove Control nodes.
Note: removing a Control node frees it from the scene tree.

When adding a Control node via the UIHolder, a UI layer is required. This can be found and changed in `UIHolder`. Control nodes will be ordered according to this enum, with higher values in the front.

Example: `UIHolder.Add(ui, UIHolder.UILayers.WORLD)`.

## Custom Settings

This projects includes a few custom settings added in the Project Settings under the "Blueprint" section:

- Save File Encryption Key: encryption key used to store save data.
- Show FPS Counter: Toggle on to show an FPS counter. Can also be turned on/off in code via `FPSCounter.Toggle(bool)`. This is on by default.

## Tests

Write and put your tests in `./tests`. This template uses NUnit to run tests. Run `dotnet test` to run your tests.

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
| Folders                     | Use snake case.                                                                  | `folder_name_here`                              |
| CS Files                    | Use pascal case. Class name should match file name.                              | `FileName.cs` and `public FileName`             |
| Classes                     | Use pascal case.                                                                 | `public MyClass`                                |
| Max Parameters              | A method should not have more than 5 parameters.                                 | -                                               |
| No magic numbers or strings | Do not use magic variables, rather define them as constant in a single location. | -                                               |

---
