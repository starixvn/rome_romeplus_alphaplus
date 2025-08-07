# AlphaPlus.Products.UnitTests

This project contains unit tests for the AlphaPlus.Products module.

## Project Structure

The test project follows the same structure as the main project:

-   **Models/** - Tests for domain models and entities
-   **ViewModels/** - Tests for view model classes and factories
-   **Configurations/** - Tests for configuration classes
-   **Shared/** - Tests for shared components

## Test Framework

This project uses:

-   **xUnit** - Testing framework
-   **Moq** - Mocking framework
-   **AutoFixture** - Test data generation
-   **coverlet.collector** - Code coverage collection

## Running Tests

### From Command Line

```bash
dotnet test
```

### From Visual Studio

1. Open the solution in Visual Studio
2. Build the solution
3. Open Test Explorer (Test > Test Explorer)
4. Run all tests or specific test classes/methods

### Code Coverage

To run tests with code coverage:

```bash
dotnet test --collect:"XPlat Code Coverage"
```

## Test Naming Convention

Tests follow the pattern: `[MethodName]_[Scenario]_[ExpectedResult]`

Examples:

-   `CanSeeProduct_WhenProductIsEnabled_ReturnsTrue`
-   `GetViewModels_WhenRequestIsInvalid_ReturnsEmptyCollection`
-   `IsAgent_WhenCustomerLevelIsAgent_ReturnsTrue`

## Mocking Guidelines

-   Use Moq for creating mocks of interfaces and abstract classes
-   Prefer constructor injection for dependencies to enable easy mocking
-   Use AutoFixture for generating test data when possible
-   Create reusable mock setups in test fixture classes

## Best Practices

1. **Arrange-Act-Assert**: Structure tests with clear sections
2. **Single Responsibility**: Each test should verify one specific behavior
3. **Descriptive Names**: Test names should clearly describe what is being tested
4. **Isolation**: Tests should be independent and not rely on other tests
5. **Fast Execution**: Tests should run quickly to enable frequent feedback

## Example Test Structure

```csharp
[Fact]
public void MethodName_Scenario_ExpectedResult()
{
    // Arrange
    var fixture = new AutoFixture.Fixture();
    var mockDependency = new Mock<IDependency>();
    var sut = new ClassUnderTest(mockDependency.Object);

    // Act
    var result = sut.MethodToTest();

    // Assert
    Assert.NotNull(result);
    Assert.Equal(expectedValue, result);
}
```
