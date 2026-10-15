---
paths:
  - "tests/**"
---

# Tests

- Tests run on Microsoft.Testing.Platform, not VSTest: no `Microsoft.NET.Test.Sdk`. Pass `--solution` or `--project` to `dotnet test`, not a bare path.
- Mirror the source tree: tests for `core/utilities/PrettyNumbers.cs` live in `tests/core/utilities/PrettyNumbersTests.cs`, namespace `Game.Tests.Core.Utilities`.
- Test doubles shared across tests (fake storage, fixed clock) live in `tests/fakes/`, namespace `Game.Tests.Fakes`.
- `Game`'s global usings don't apply here: add `using` for the namespace under test. `NUnit.Framework` is global.
- Tests run without the engine: no `Node`, `GD.*`, `ResourceLoader` or scene tree. Godot structs (`Vector3`, `Mathf`) are fine. Keep testable logic in pure static helpers.
- `Assert.That(actual, Is.EqualTo(expected))`, not `Assert.That(a == b)`.
- Culture-sensitive code gets a `[SetCulture("en-ZA")]` case to prove the culture is passed explicitly.
