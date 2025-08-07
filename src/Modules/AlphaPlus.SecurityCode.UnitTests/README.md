# AlphaPlus.SecurityCode.UnitTests

This project contains unit tests for the AlphaPlus.SecurityCode module, which provides security code functionality for the AlphaPlus application.

## Project Structure

The test project follows the same structure as the main project:

-   **Configurations/**: Tests for configuration classes
-   **Services/**: Tests for service classes
-   **Web/**: Tests for web-related classes
-   **Filters/**: Tests for filter attributes
-   **Extensions/**: Tests for extension methods

## Test Coverage

### Configuration Tests

-   `SecurityCodeOptionsTests.cs`: Tests for the SecurityCodeOptions configuration class

### Service Tests

-   `SecurityCodeCheckerTests.cs`: Comprehensive tests for the SecurityCodeChecker service
-   `SecurityCodePathBuilderTests.cs`: Tests for the SecurityCodePathBuilder service

### Web Tests

-   `SecurityCodeSessionTests.cs`: Tests for the SecurityCodeSession class
-   `SecurityCodeSessionManagerTests.cs`: Tests for the SecurityCodeSessionManager static class
-   `SecurityCodeHttpContextTests.cs`: Tests for the SecurityCodeHttpContext class

### Filter Tests

-   `SecurityCodeFilterAttributeTests.cs`: Tests for the SecurityCodeFilterAttribute

### Extension Tests

-   `ServiceCollectionExtensionsTests.cs`: Tests for the ServiceCollectionExtensions

## Testing Framework

This project uses:

-   **xUnit**: Testing framework
-   **Moq**: Mocking framework
-   **AutoFixture**: Test data generation
-   **Microsoft.NET.Test.Sdk**: Test discovery and execution

## Running Tests

To run the tests:

```bash
dotnet test
```

To run tests with coverage:

```bash
dotnet test --collect:"XPlat Code Coverage"
```

## Test Patterns

### Mocking Dependencies

The tests use Moq to mock dependencies, particularly:

-   `IOptions<SecurityCodeOptions>` for configuration
-   `ISecurityCodeSession` for session management
-   `ISecurityCodePathBuilder` for path building

### Test Naming Convention

Tests follow the pattern: `MethodName_Scenario_ExpectedResult`

### Test Structure

Tests follow the Arrange-Act-Assert pattern:

1. **Arrange**: Set up test data and mocks
2. **Act**: Execute the method under test
3. **Assert**: Verify the expected results

## Notes

Some tests are basic structure tests that verify the classes can be instantiated and methods can be called. In a real-world scenario, you would need to mock additional dependencies like:

-   `AlphaContext.Current`
-   `UserSession.Current`
-   HTTP context objects

These dependencies are typically framework-specific and would require more complex setup in integration tests.

## Contributing

When adding new tests:

1. Follow the existing naming conventions
2. Use the Arrange-Act-Assert pattern
3. Mock dependencies appropriately
4. Add tests for both positive and negative scenarios
5. Test edge cases and error conditions
