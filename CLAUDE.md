# Yale – Claude Code Context

## Project Overview

Yale is a .NET 10.0 expression parser and evaluator library. It compiles string expressions (e.g., `sqrt(a^2 + b^2)`, `name() = "Maria"`) to Common Intermediate Language (CIL) for fast runtime evaluation. It is a modernization of the [Flee](https://github.com/mparlak/Flee) library. Licensed under LGPL-3.0.

## Repository Structure

```
src/Yale/
  Expression/         # Expression compilation pipeline
    Elements/         # Operator and literal AST nodes
    Elements/Base/    # Base classes for expression elements
    Elements/Literals/
    Elements/LogicalBitwise/
    Elements/MemberElements/
  Core/Interfaces/    # Public-facing interfaces
  Engine/             # Compilation entry points (ComputeInstance, options)
    Interface/
    Internal/
  Parser/             # Tokenizer / grammar (RE/, Internal/)
  Resources/          # Localized error message .resx files
test/Yale.Tests/      # MSTest unit tests
  Core/, Engine/, ExpressionTests/, Parser/, Theory/, Helper/
benchmark/Yale.Benchmarks/  # BenchmarkDotNet suite (Parse/, Engine/)
```

## Build & Test Commands

```bash
# Restore dependencies
dotnet restore

# Build (debug)
dotnet build

# Build (release)
dotnet build --configuration Release

# Run all tests
dotnet test

# Run a specific test project
dotnet test test/Yale.Tests/

# Run benchmarks
dotnet run --project benchmark/Yale.Benchmarks/ --configuration Release
```

Pre-commit hooks (Husky + CSharpier) run automatically on `git commit`. To skip in CI, set `CICD=0`.

## Code Style

- **Formatter:** CSharpier (`dotnet csharpier .`) — run before committing.
- **Nullable:** enabled project-wide; avoid `!` (null-forgiving) unless unavoidable.
- **Naming:** interfaces prefix `I`, all types and members PascalCase.
- **Usings:** outside namespace declarations.
- **Braces:** omit for single-line bodies only when already conventional in surrounding code.
- **`var`:** use freely; type annotation is not required when the type is apparent.
- See `.editorconfig` for the full ruleset.

## Architecture Notes

- `ComputeInstance` (in `Engine/`) is the public entry point for compiling and evaluating expressions.
- `ComputeInstanceOptions` controls variable/function resolution.
- Expression nodes in `Expression/Elements/` emit CIL via `ILGenerator`; each element implements `Emit(ILGenerator)`.
- Error messages live in `Resources/CompileErrors.resx` and `Resources/GeneralErrors.resx` (auto-generated Designer.cs files — do not edit by hand).
- The parser in `Parser/` converts raw strings to an element tree that the engine then emits.

## CI/CD

The GitHub Actions workflow (`.github/workflows/publish_nuget_preview.yml`) is manually triggered (`workflow_dispatch`). It restores, builds in Release, packs with a date-based version suffix, and publishes to NuGet.
