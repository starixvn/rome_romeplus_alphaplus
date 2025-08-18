# AccountInfo.Site.UnitTests

This project contains comprehensive unit tests for the AccountInfo.Site project, following .NET testing best practices and patterns.

## Project Structure

```
AccountInfo.Site.UnitTests/
├── Features/                          # Tests for feature-specific components
│   ├── SearchAccount/                 # SearchAccount feature tests
│   │   ├── SearchAccountControllerTests.cs
│   │   ├── IndexViewModelTests.cs
│   │   └── IndexQueryTests.cs
│   └── Shared/                        # Shared feature tests
│       ├── SiteBaseControllerTests.cs
│       ├── KickOutCustomerServiceTests.cs
│       ├── StatusNamesTests.cs
│       └── DirectMemberAgentIdsTests.cs
├── Extensions/                        # Extension method tests
│   └── ExtensionsTests.cs
├── TestHelpers/                       # Common test utilities
│   └── TestBase.cs
├── ProgramTests.cs                    # Program.cs configuration tests
├── Usings.cs                          # Global using statements
└── README.md                          # This file
```

## Test Coverage

### Controllers

-   **SiteBaseController**: Tests for base controller functionality including View and Json methods
-   **SearchAccountController**: Tests for all controller actions and ViewBag assignments

### Services

-   **KickOutCustomerService**: Tests for customer kickout functionality, error handling, and configuration options

### ViewModels

-   **IndexViewModel**: Tests for property initialization and computed properties

### Queries

-   **IndexQuery**: Tests for validation logic and inheritance

### Extensions

-   **Extensions**: Tests for service registration, authentication configuration, and MVC setup

### Configuration

-   **Program**: Tests for application startup configuration and service registration

## Testing Patterns

### Test Structure

Each test follows the **Arrange-Act-Assert** pattern:

```csharp
[Fact]
public void TestName_Should_ExpectedBehavior()
{
    // Arrange - Set up test data and mocks

    // Act - Execute the method under test

    // Assert - Verify the expected outcome
}
```

### Mocking

Tests use **Moq** for mocking dependencies:

-   `Mock<IMediator>` for MediatR operations
-   `Mock<HttpContext>` for HTTP context simulation
-   `Mock<IConfiguration>` for configuration testing

### Test Data

Tests use **AutoFixture** for generating test data:

```csharp
private readonly Fixture _fixture = new Fixture();
var testData = _fixture.Create<TestType>();
```

## Running Tests

### Command Line

```bash
# Run all tests
dotnet test

# Run tests with coverage
dotnet test --collect:"XPlat Code Coverage"

# Run specific test class
dotnet test --filter "FullyQualifiedName~SearchAccountControllerTests"
```

### Visual Studio

1. Open the solution in Visual Studio
2. Build the solution
3. Open Test Explorer (Test > Test Explorer)
4. Run all tests or specific test categories

### Continuous Integration

Tests are configured to run automatically in CI/CD pipelines with:

-   xUnit test framework
-   Coverlet for code coverage
-   .NET 9.0 target framework

## Test Categories

### Unit Tests

-   **Fast**: Execute in milliseconds
-   **Isolated**: No external dependencies
-   **Deterministic**: Same result every time
-   **Focused**: Test one specific behavior

### Integration Tests

-   **Service Registration**: Verify DI container configuration
-   **Configuration**: Test application startup settings
-   **Authentication**: Test auth pipeline setup

## Best Practices

### Naming Conventions

-   Test class names: `{ClassName}Tests`
-   Test method names: `{MethodName}_{Scenario}_{ExpectedResult}`
-   Mock variables: `_{type}Mock`

### Test Organization

-   Group related tests in the same test class
-   Use `[Theory]` for parameterized tests
-   Use `[InlineData]` for simple test data
-   Use `[Fact]` for single scenario tests

### Assertions

-   Use specific assertion methods (e.g., `Assert.Equal`, `Assert.NotNull`)
-   Provide meaningful error messages
-   Test both positive and negative scenarios
-   Verify mock interactions when appropriate

## Dependencies

-   **xUnit**: Testing framework
-   **Moq**: Mocking library
-   **AutoFixture**: Test data generation
-   **Microsoft.NET.Test.Sdk**: Test execution
-   **coverlet.collector**: Code coverage

## Contributing

When adding new tests:

1. Follow the existing naming conventions
2. Use the established test patterns
3. Ensure proper test coverage for new functionality
4. Update this README if adding new test categories
5. Run all tests to ensure no regressions

## Troubleshooting

### Common Issues

-   **Missing Dependencies**: Ensure all project references are properly configured
-   **Mock Setup Errors**: Verify mock configurations match actual method signatures
-   **Test Isolation**: Ensure tests don't depend on each other or external state

### Debugging Tests

-   Use `Debugger.Break()` in test methods
-   Set breakpoints in test code
-   Use `Console.WriteLine()` for debugging output
-   Check test output in Test Explorer
