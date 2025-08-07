# AlphaPlus.PartialUM.UnitTests

This project contains unit tests for the AlphaPlus.PartialUM module, which provides functionality for handling partial user maintenance (UM) scenarios.

## Project Structure

The test project follows the same structure as the main project:

-   **Configurations/**: Tests for configuration classes
-   **DataAccess/**: Tests for data access interfaces
-   **Exceptions/**: Tests for custom exceptions
-   **Extensions/**: Tests for dependency injection extensions
-   **Models/**: Tests for data models
-   **Services/**: Tests for business logic services

## Test Coverage

### Models

-   `UMEntityTests`: Tests for the UMEntity model class
    -   Property initialization
    -   Immutability verification

### Exceptions

-   `PartialUMExceptionTests`: Tests for the PartialUMException class
    -   Constructor behavior
    -   Property initialization
    -   Error code verification
    -   Edge cases with null/empty values

### Configurations

-   `PartialUMOptionsTests`: Tests for the PartialUMOptions configuration class
    -   Property initialization
    -   Valid value acceptance
    -   Immutability verification

### Services

-   `PartialUMServiceTests`: Comprehensive tests for the PartialUMService class
    -   Setting key not found scenarios
    -   Empty UM entities scenarios
    -   Non-matching function scenarios
    -   Matching UM entity scenarios (exception throwing)
    -   Multiple UM entities scenarios
    -   Cache key verification
    -   Cache expiration verification
    -   Edge cases with invalid site names

### Data Access

-   `IPartialUMDataAccessTests`: Tests for the data access interface
    -   Return type verification
    -   Empty/null site name handling
    -   Method call verification

### Extensions

-   `DependencyInjectionTests`: Tests for dependency injection extensions
    -   Service registration
    -   Configuration registration
    -   Data access registration
    -   Null configuration handling
    -   Missing configuration handling

## Running Tests

To run the tests, use one of the following commands:

```bash
# Run all tests
dotnet test

# Run tests with coverage
dotnet test --collect:"XPlat Code Coverage"

# Run specific test class
dotnet test --filter "FullyQualifiedName~PartialUMServiceTests"

# Run tests in watch mode
dotnet watch test
```

## Test Dependencies

-   **xUnit**: Testing framework
-   **Moq**: Mocking framework for creating test doubles
-   **AutoFixture**: For generating test data
-   **Microsoft.NET.Test.Sdk**: Test SDK for .NET
-   **coverlet.collector**: Code coverage collection

## Best Practices

1. **Arrange-Act-Assert**: All tests follow the AAA pattern
2. **Descriptive Names**: Test method names clearly describe the scenario being tested
3. **Mocking**: External dependencies are mocked to ensure unit test isolation
4. **Edge Cases**: Tests include edge cases like null values, empty strings, etc.
5. **Async Testing**: Proper async/await patterns for asynchronous operations
6. **Theory Tests**: Used for parameterized tests with multiple input scenarios

## Contributing

When adding new tests:

1. Follow the existing naming conventions
2. Ensure proper test isolation
3. Add appropriate assertions
4. Include edge cases and error scenarios
5. Update this README if adding new test categories
