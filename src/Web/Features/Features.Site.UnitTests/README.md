# AlphaPlus.Features.Site.UnitTests

This project contains unit tests for the AlphaPlus.Features.Site project, which is a web application built with ASP.NET Core 8.0.

## Project Structure

```
Features.Site.UnitTests/
├── Controllers/           # Tests for MVC Controllers
│   ├── NewMessageControllerTests.cs
│   ├── SiteBaseControllerTests.cs
│   └── StaticMessageControllerTests.cs
├── Handlers/             # Tests for MediatR Handlers
│   ├── NewMessageHandlerTests.cs
│   └── FeedbackHandlerTests.cs
├── TestHelpers/          # Common testing utilities
│   └── TestHelper.cs
├── ProgramTests.cs       # Tests for Program.cs configuration
├── Features.Site.UnitTests.csproj
└── README.md
```

## Test Framework

-   **xUnit**: Primary testing framework
-   **Moq**: Mocking framework for dependencies
-   **FluentAssertions**: Fluent assertion library for readable test assertions
-   **Microsoft.AspNetCore.Mvc.Testing**: Testing utilities for ASP.NET Core MVC
-   **Microsoft.AspNetCore.TestHost**: Test host for integration testing

## Running Tests

### Using Visual Studio

1. Open the solution in Visual Studio
2. Right-click on the test project and select "Run Tests"
3. View test results in the Test Explorer window

### Using .NET CLI

```bash
# Navigate to the test project directory
cd src/Web/Features/Features.Site.UnitTests

# Run all tests
dotnet test

# Run tests with verbose output
dotnet test --verbosity normal

# Run tests with coverage
dotnet test --collect:"XPlat Code Coverage"

# Run specific test class
dotnet test --filter "FullyQualifiedName~NewMessageControllerTests"

# Run tests in watch mode (automatically re-runs on file changes)
dotnet watch test
```

### Using Rider

1. Open the solution in JetBrains Rider
2. Use the Unit Tests tool window to run tests
3. Right-click on test methods/classes to run specific tests

## Test Categories

### Controllers

-   **NewMessageController**: Tests message handling and validation
-   **SiteBaseController**: Tests base controller functionality and MediatR integration
-   **StaticMessageController**: Tests static file serving and view resolution

### Handlers

-   **NewMessageHandler**: Tests message processing logic
-   **FeedbackHandler**: Tests feedback processing and XML generation

### Configuration

-   **ProgramTests**: Tests application configuration and setup

## Test Patterns

### Mocking Dependencies

```csharp
private readonly Mock<IMediator> _mediatorMock;
private readonly Mock<IParam> _specificationParamMock;

public NewMessageControllerTests()
{
    _mediatorMock = new Mock<IMediator>();
    _specificationParamMock = new Mock<IParam>();
    _controller = new NewMessageController(_mediatorMock.Object, _specificationParamMock.Object);
}
```

### Testing Private Methods

```csharp
// Using reflection to test private static methods
var result = NewMessageController.GetPrivateStaticMethod<bool>("IsDisableMessages", message);
```

### Async Testing

```csharp
[Fact]
public async Task Handle_ShouldCallMessageServiceAndReturnUnreadMessage()
{
    // Arrange
    _messageServiceMock
        .Setup(x => x.CheckNewUnreadMessage(It.IsAny<NumberOfNewMessageParams>()))
        .ReturnsAsync(expectedUnreadMessage);

    // Act
    var result = await _handler.Handle(query, CancellationToken.None);

    // Assert
    result.Should().Be(expectedUnreadMessage);
}
```

### Exception Testing

```csharp
[Fact]
public async Task Handle_WhenServiceThrowsException_ShouldPropagateException()
{
    // Arrange
    var expectedException = new InvalidOperationException("Service error");
    _messageServiceMock
        .Setup(x => x.CheckNewUnreadMessage(It.IsAny<NumberOfNewMessageParams>()))
        .ThrowsAsync(expectedException);

    // Act & Assert
    var exception = await Assert.ThrowsAsync<InvalidOperationException>(
        () => _handler.Handle(query, CancellationToken.None));

    exception.Should().Be(expectedException);
}
```

## Test Coverage

The tests cover:

-   ✅ Controller actions and routing
-   ✅ Handler business logic
-   ✅ Dependency injection and mocking
-   ✅ Exception handling
-   ✅ Configuration validation
-   ✅ Private method behavior
-   ✅ Async/await patterns
-   ✅ View resolution logic
-   ✅ File handling operations

## Best Practices

1. **Arrange-Act-Assert**: Follow the AAA pattern for test structure
2. **Descriptive Names**: Use descriptive test method names that explain the scenario
3. **Single Responsibility**: Each test should verify one specific behavior
4. **Mock External Dependencies**: Mock external services and dependencies
5. **Test Edge Cases**: Include tests for error conditions and edge cases
6. **Clean Setup/Teardown**: Use constructor for setup and implement IDisposable if needed
7. **Meaningful Assertions**: Use specific assertions that verify the expected behavior

## Adding New Tests

When adding new tests:

1. Create test files in the appropriate directory structure
2. Follow the existing naming conventions
3. Use the established mocking patterns
4. Include both positive and negative test cases
5. Test edge cases and error conditions
6. Use descriptive test names and arrange-act-assert structure

## Troubleshooting

### Common Issues

1. **Missing Dependencies**: Ensure all required NuGet packages are installed
2. **Mock Setup**: Verify that mocks are properly configured before use
3. **Async Tests**: Use `async Task` return type for async tests
4. **Private Method Testing**: Use reflection helpers for testing private methods
5. **File System Tests**: Mock file system operations or use test files

### Debugging Tests

1. Set breakpoints in test methods
2. Use `Debugger.Break()` for manual debugging
3. Check test output for detailed error messages
4. Verify mock setups and verifications

## Contributing

1. Follow the existing test patterns and conventions
2. Ensure all tests pass before submitting changes
3. Add tests for new functionality
4. Update this README when adding new test categories or patterns
