# Smartwyre Developer Test — .NET Refactoring Exercise

A .NET refactoring exercise applying SOLID principles and clean architecture patterns. The original code was refactored to improve maintainability, testability, and adherence to object-oriented design principles.

## SOLID Principles Applied

| Principle | Application |
|-----------|------------|
| **S**ingle Responsibility | Each class has a single, well-defined purpose |
| **O**pen / Closed | New rebate calculators can be added without modifying existing code |
| **L**iskov Substitution | Derived types are interchangeable with base types |
| **I**nterface Segregation | Small, focused interfaces avoid unnecessary dependencies |
| **D**ependency Inversion | High-level modules depend on abstractions, not concrete implementations |

## Architecture Overview

The solution follows a stratified design with clear separation between business logic, data access, and the public API surface. The refactored structure decouples rebate calculations from the main processing pipeline via strategy pattern and dependency injection.

```
Presentation Layer --> Application Layer --> Domain Layer --> Infrastructure Layer
```

- **Domain**: Core entities and calculation interfaces
- **Application**: Orchestration and DTOs
- **Infrastructure**: Data access and external concerns
- **Tests**: Unit tests with mocks

## How to Run Tests

```bash
# Restore dependencies
dotnet restore

# Run all tests
dotnet test

# Run tests with verbose output
dotnet test --verbosity detailed
```

## Tech Stack

- **.NET** — Runtime and base class library
- **C#** — Primary language
- **xUnit / NUnit** — Test framework
- **Moq** — Mocking framework for unit tests
- **Dependency Injection** — Built-in .NET DI container
