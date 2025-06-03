# CashService.Site.Tests

This project contains unit tests for the CashService.Site application.

## Technology Stack

- xUnit - Testing framework
- FluentAssertions - For more readable assertions
- Moq - Mocking framework
- coverlet.collector - Code coverage tool

## Project Structure

The test project follows the same structure as the main project:

```
CashService.Site.Tests/
├── Features/
│   └── Payments/
│       └── Models/
│           └── BankAccountModelTests.cs
└── README.md
```

## Running Tests

### Using Visual Studio

1. Open the solution in Visual Studio
2. Right-click on the test project in Solution Explorer
3. Select "Run Tests"

### Using Command Line

```bash
dotnet test
```

To run tests with code coverage:

```bash
dotnet test --collect:"XPlat Code Coverage"
```

## Writing Tests

### Naming Convention

- Test classes should be named `{ClassUnderTest}Tests`
- Test methods should follow the pattern: `{MethodUnderTest}_{Scenario}_{ExpectedBehavior}`

### Test Structure

Follow the Arrange-Act-Assert pattern:

```csharp
[Fact]
public void MyMethod_WhenSomething_ShouldDoSomething()
{
    // Arrange
    var sut = new SystemUnderTest();

    // Act
    var result = sut.MyMethod();

    // Assert
    result.Should().BeTrue();
}
```

## Contributing

1. Create tests for new features before implementing them (TDD approach)
2. Ensure all tests pass before submitting changes
3. Maintain test coverage for new code
4. Follow the existing test patterns and naming conventions 