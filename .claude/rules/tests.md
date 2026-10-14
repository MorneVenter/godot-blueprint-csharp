---
paths:
  - "tests/**"
---

# Tests

- `tests/Game.Tests.csproj` is a separate NUnit project referencing `Game.csproj`. Godot ignores the folder (`tests/.gdignore`), so there are no `.uid` files here.
- Tests run on Microsoft.Testing.Platform, not VSTest: no `Microsoft.NET.Test.Sdk`. Pass `--solution` or `--project` to `dotnet test`, not a bare path.
- Mirror the source tree: tests for `core/utilities/PrettyNumbers.cs` live in `tests/core/utilities/PrettyNumbersTests.cs`, namespace `Game.Tests.Core.Utilities`.
- `Game`'s global usings don't apply here: add `using` for the namespace under test. `NUnit.Framework` is global.
- Tests run without the engine: no `Node`, `GD.*`, `ResourceLoader` or scene tree. Godot structs (`Vector3`, `Mathf`) are fine. Keep testable logic in pure static helpers.
- `Assert.That(actual, Is.EqualTo(expected))`, not `Assert.That(a == b)`.
- Culture-sensitive code gets a `[SetCulture("en-ZA")]` case: the `GameLocale` autoload doesn't run in tests, so this proves the culture is passed explicitly.
- New test packages: version in `Directory.Packages.props`, versionless `PackageReference` in `Game.Tests.csproj`, never in `Game.csproj`.
