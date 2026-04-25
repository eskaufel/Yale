# GitHub Copilot Instructions – Yale

## Project

Yale is a **.NET 6.0 C# library** that compiles string expressions (e.g., `sqrt(a^2 + b^2)`, `age > 18 AND name() = "Alice"`) to CIL (Common Intermediate Language) for fast runtime evaluation. It is a modernization of the Flee library. LGPL-3.0 licensed.

## Tech Stack

- **Language:** C# on .NET 10.0
- **Testing:** MSTest (`test/Yale.Tests/`)
- **Benchmarking:** BenchmarkDotNet (`benchmark/Yale.Benchmarks/`)
- **Formatter:** CSharpier (enforced via Husky pre-commit hook)
- **NuGet package:** `Yale` by Espen Skaufel

## Key Directories

| Path | Purpose |
|------|---------|
| `src/Yale/Engine/` | Public API — `ComputeInstance` and `ComputeInstanceOptions` |
| `src/Yale/Expression/Elements/` | AST nodes; each emits CIL via `ILGenerator` |
| `src/Yale/Parser/` | Tokenizer and grammar for expression strings |
| `src/Yale/Core/Interfaces/` | Core public interfaces |
| `src/Yale/Resources/` | Localized error messages (`.resx`); Designer.cs files are auto-generated |
| `test/Yale.Tests/` | Unit tests organized by subsystem |

## Coding Conventions

- **Interfaces** must be prefixed with `I` (e.g., `IExpressionElement`).
- **Types and members** use PascalCase.
- **Nullable reference types** are enabled — do not suppress with `!` unless strictly necessary.
- `using` directives go **outside** the namespace.
- Prefer **file-scoped namespaces** (`namespace Foo;`).
- Use `var` when the type is clear from context.
- Prefer expression-bodied members for single-expression properties and methods.
- Use pattern matching and switch expressions over explicit type casts where natural.
- Format code with **CSharpier** before committing (`dotnet csharpier .`).
- Do not edit `*.Designer.cs` files — they are auto-generated from `.resx` resources.

## Common Tasks

```bash
dotnet restore          # restore packages
dotnet build            # build solution
dotnet test             # run all tests
dotnet csharpier .      # format code
```

## Patterns to Follow

- New expression element types go in `src/Yale/Expression/Elements/` and inherit from the appropriate base class in `Elements/Base/`.
- Add corresponding unit tests in `test/Yale.Tests/ExpressionTests/`.
- Error messages belong in `src/Yale/Resources/CompileErrors.resx` or `GeneralErrors.resx`; reference them via the generated Designer class.
- Benchmarks for new features go in `benchmark/Yale.Benchmarks/`.
