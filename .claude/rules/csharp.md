---
paths:
  - "**/*.cs"
---

# C# conventions

Godot C# style guide, with project overrides marked.

## Language

- C# 14 (project override: the guide targets C# 12). Nullable reference types on. CI treats warnings as errors, so keep local builds at 0 warnings.
- Meziantou.Analyzer runs on every project (NUnit.Analyzers on tests). Fix what it reports. If a rule conflicts with a project convention, set its severity in `.editorconfig` with a one-line reason; never `#pragma` or `[SuppressMessage]`.
- Implicit usings plus `GlobalUsings.cs`. `Godot.Collections` is not global: add `using Godot.Collections;` only where engine collections are needed.

## Formatting

CSharpier owns layout: Allman braces, 4-space indent, ~100 columns, operator and comma spacing. Beyond that:

- LF, UTF-8 without BOM, one trailing newline.
- Blank line after `using`s, between members, and between groups of related fields.
- No blank line after `{`, before `}`, after a comment, or next to another blank line.

## Naming

| Kind | Style | Example |
| :-- | :-- | :-- |
| Namespaces, types, methods, properties, events, constants, enum values | PascalCase | `MaxHealth`, `State.Idle` |
| Locals, parameters | camelCase | `targetPosition` |
| Private fields | `_camelCase` | `_currentHealth` |
| Interfaces | `I` prefix | `IDamageable` |
| Signals | `<Name>EventHandler` delegate | `HealthChangedEventHandler` |

- Two-letter acronyms are uppercase in PascalCase (`UIHolder`), lowercase in camelCase (`uiLayer`). Longer ones are words (`FpsCounter`). `Id` isn't an acronym.
- Descriptive names, no abbreviations, no type prefixes (`strName`, `fSpeed`).
- Namespace mirrors the folder in PascalCase: `core/managers/save_manager/SaveManager.cs` → `Game.Core.Managers.SaveManager`, class `SaveManager`. The repetition in the full name is fine because types are never fully qualified.
- Folders `snake_case`; one class per file, file named after it. Godot scripts under `core/` get their own folder: `foo_bar/FooBar.cs`.

## Folders

Group files into folders by concern. Never pile a system's files into one flat folder.

- A system with more than one kind of part gets a subfolder per concern; its entry point stays at the top. Example: `save_manager/SaveManager.cs` (the autoload), `save_manager/store/`, `save_manager/storage/` (interface + implementations), `save_manager/serialization/`.
- Data classes nest the way they compose: `core/data/save/SaveData.cs`, its sections in `core/data/save/player/PlayerData.cs`, a section's own sub-objects beside it.
- Split a folder once it holds files serving different concerns, or roughly five or more files. One-file folders are fine when the concern is distinct.

## Code

- `var` everywhere (project override), except numeric literals where `float`/`double` matters: `float speed = 5f`.
- Explicit access modifiers. Properties, not non-private fields.
- Modifier order: access, `virtual`/`override`/`abstract`/`new`, `static`, `readonly`.
- Declare locals close to first use. Don't make a field of something only one method uses.
- Never fully qualify a type: add a `using`, or a `using` alias on a clash (`using FileAccess = Godot.FileAccess;`). No `this.` unless required (extension calls like `this.GetConfig()`).
- Remove unused `using`s and redundant parentheses. Omit default initial values.
- `as` when the type may differ, direct cast when it can't. Use `?.` and initializers to stay compact.
- No magic numbers or strings: named constants in one place. At most 5 parameters.

## Godot

- Classes deriving from Godot types are `partial`. `[GlobalClass]` to expose in the editor.
- Use generated `SignalName.X`, `MethodName.X`, `PropertyName.X`, never string names.
- Godot collections only at engine boundaries (exports, signals, `Variant`); `System.Collections.Generic` elsewhere.
- `GetNode<T>`; `IsInstanceValid` before touching nodes that may be freed; `?? throw` with a clear message when a required node or resource is missing.
- New script: create the `.cs`, then `npm run import` to generate its `.uid`.
