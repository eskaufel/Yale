# Copilot Instructions for Yale

Yale is a .NET 6 expression parser and evaluator library written in C#. Expressions are compiled to Common Intermediate Language (CIL) for fast evaluation at runtime.

## Project structure

- `src/Yale/` — main library source code
  - `Core/` — core types and interfaces
  - `Engine/` — expression compilation and evaluation engine
  - `Expression/` — expression types and builders
  - `Parser/` — ANTLR-based grammar and parser
  - `Resources/` — localised error message resources (.resx)
- `test/Yale.Tests/` — MSTest unit tests
- `test/Yale.InteractiveConsole/` — interactive REPL for manual testing
- `benchmark/Yale.Benchmarks/` — BenchmarkDotNet performance benchmarks

## How to build and test

```bash
# Restore packages (set CICD=0 to skip Husky git-hook install)
CICD=0 dotnet restore

# Build
dotnet build --configuration Release

# Run unit tests
dotnet test test/Yale.Tests/Yale.Tests.csproj

# Run benchmarks (optional)
dotnet run --project benchmark/Yale.Benchmarks/Yale.Benchmarks.csproj --configuration Release
```

## Code style

- Nullable reference types are enabled (`<Nullable>enable</Nullable>`).
- Code style is enforced in build via `.editorconfig` and `<EnforceCodeStyleInBuild>True</EnforceCodeStyleInBuild>`.
- Pre-commit hooks are managed by [Husky.Net](https://alirezanet.github.io/Husky.Net/). When working outside CI set `CICD=0` to skip hook installation.

## Key conventions

- Target framework is **net6.0** only.
- Tests use **MSTest** (`Microsoft.Testing.Framework` / `MSTest.TestAdapter`).
- Do not add new NuGet dependencies without discussing them first; the library intentionally has zero runtime dependencies.
- Keep public API surface minimal and intuitive — that is the primary design goal of Yale.
