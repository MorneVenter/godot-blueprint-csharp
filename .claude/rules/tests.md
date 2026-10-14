---
paths:
  - "tests/**"
---

# Tests

- NUnit in `tests/<unit_name>/<Unit>Tests.cs`, namespace `Tests.<Unit>Tests`.
- Tests run without the engine: no `Node`, `GD.*`, `ResourceLoader` or scene tree. Godot structs (`Vector3`, `Mathf`) are fine. Keep testable logic in pure static helpers.
- `Assert.That(actual, Is.EqualTo(expected))`, not `Assert.That(a == b)`.
- Culture-sensitive code gets a `[SetCulture("en-ZA")]` case to prove `GameLocale` is used.
