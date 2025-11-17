# GitHub Copilot Instructions for RG.RazorMail

## Project Overview
RG.RazorMail is a .NET library for rendering Razor views (.cshtml) and Razor components (.razor) to HTML strings with optional CSS inlining. It wraps RazorLight and PreMailer.Net to provide email-friendly HTML rendering.

## Technology Stack
- .NET 10.0
- ASP.NET Core (Microsoft.NET.Sdk.Web)
- RazorLight for .cshtml rendering
- ASP.NET Core Components for .razor rendering
- PreMailer.Net for CSS inlining
- xUnit for testing
- BenchmarkDotNet for performance benchmarks

## Project Structure
- `RG.RazorMail/` - Main library project
- `Tests/` - xUnit tests
- `Benchmarks/` - BenchmarkDotNet performance tests

## Coding Standards
- Enable nullable reference types
- Enforce code style in build
- XML documentation comments required for public APIs
- Use tabs for indentation (as per .editorconfig)
- Preserve compilation context for runtime compilation

## Key Implementation Details

### Embedded Resources
- Razor views (.cshtml) must be marked as EmbeddedResource in the project file
- Views are loaded from assembly resources at runtime
- Resource names follow the pattern: `{AssemblyName}.{ViewPath.Replace("/", ".")}`

### Caching
- RazorLight caching is enabled via `UseMemoryCachingProvider()`
- Compiled templates are cached to improve performance

### Dependency Injection
- Register services using `services.AddRazorMail()`
- For .cshtml views, configure `ViewsAssembly` in options
- For .razor components, no additional configuration needed

## Testing Guidelines
- Test files should be marked as EmbeddedResource
- Tests verify both .cshtml and .razor rendering
- Test CSS inlining functionality
- Use FluentAssertions for assertions

## Building and Testing
```bash
dotnet restore
dotnet build
dotnet test
```

## Benchmarking
```bash
cd Benchmarks
dotnet run -c Release
```

## Package Publishing
- Package is automatically generated on build
- Version is specified in RG.RazorMail.csproj
- README.md is included in the package
- MIT license with acceptance required

## When Making Changes
- Maintain backward compatibility
- Update XML documentation for API changes
- Add or update tests for new functionality
- Run benchmarks if performance-critical code changes
- Update README.md for user-facing changes
- Keep minimal dependencies
