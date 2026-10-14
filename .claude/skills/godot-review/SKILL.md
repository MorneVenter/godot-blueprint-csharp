---
name: godot-review
description: Review Godot C# code for engine-specific bugs and performance traps (signal leaks, freed-node access, per-frame allocations, missing partial, magic node paths). Use when asked to review Godot code, a diff, or before committing gameplay changes.
argument-hint: "[path or git ref, default: uncommitted changes]"
---

# Godot C# review

Scope: $ARGUMENTS. If empty, review `git diff HEAD` plus untracked `.cs`/`.tscn` files. Read whole files, not just hunks: lifecycle bugs span methods.

## Checklist

**Signals**
- `[Signal]` events subscribed with `+=` are never auto-disconnected. Every `+=` to a node that outlives the subscriber (any autoload, especially `SignalBus`) needs `-=` in `_ExitTree`.
- Lambdas that capture variables are never auto-disconnected; store the delegate so it can be removed.
- Plain C# `event`s are never auto-disconnected.
- Signal parameters must be Variant-compatible; custom classes must derive from `GodotObject`.
- Emit with `SignalName.X`, not strings.

**Lifecycle**
- Classes deriving from Godot types are `partial`.
- Node lookups and setup in `_Ready`, not the constructor. `GetNode` results cached, never repeated per frame.
- No use of a node after `QueueFree`/`Free`; guard with `IsInstanceValid` where lifetime is uncertain.
- `await ToSignal(...)` resumes after the awaiting node may be freed: check `IsInstanceValid(this)` after awaits.
- Scene-tree access only from the main thread; use `CallDeferred` from threads.
- Disposable engine handles (`FileAccess`, `DirAccess`) in `using`.

**Hot paths** (`_Process`, `_PhysicsProcess`, `_Input`, per-frame signals)
- No allocations: `new`, string interpolation, LINQ, closures, boxing into `Variant`, Godot collections.
- Movement and physics in `_PhysicsProcess`, scaled by `delta`.
- No work every frame that only needs to happen on change: use a signal or setter.

**References**
- No string node paths where an `[Export]` node or `%UniqueName` works. No `/root/...` outside `AutoloadExtension`.
- Input actions, groups, animation names and other strings are constants, not literals.
- Scenes and resources referenced by `uid://`, not `res://` paths.
- Exported node references are null-checked or validated in `_Ready`.

**Project rules**
- Player-facing formatting with `GameLocale.Culture`, saved/machine data with `InvariantCulture`; never the system culture.
- `.uid` files present and committed for every new script and scene.
- Conventions in `.claude/rules/csharp.md`.

## Report

List findings by severity (bug, performance, convention), each with `file:line`, the problem in one sentence and the fix. Say plainly when nothing is wrong. Don't fix anything unless asked.
