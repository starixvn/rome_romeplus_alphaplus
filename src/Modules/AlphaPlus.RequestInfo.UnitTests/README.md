# AlphaPlus.RequestInfo.UnitTests

This project contains unit tests for the `AlphaPlus.RequestInfo` module.

## Overview

The unit tests cover the following components:

### DeviceHelperTests

-   Tests for `DeviceHelper` class which handles device information extraction
-   Covers user agent parsing, device info generation, and request info creation
-   Tests both synchronous and asynchronous methods
-   Includes edge cases like null/empty user agents and browser sizes

### GetIPInformationCriteriaTests

-   Tests for `GetIPInformationCriteria` class
-   Validates the `IsValid()` method with various IP input scenarios
-   Tests the `GetSettingKey()` method
-   Verifies property initialization

### ExtensionsTests

-   Tests for the `Extensions` class that handles dependency injection setup
-   Verifies that all required services are properly registered
-   Tests that services are registered as singletons
-   Ensures proper service resolution

### ModelsTests

-   Tests for the model classes (`DeviceInfo` and `RequestInfo`)
-   Verifies property initialization and settability
-   Tests nullable property behavior

## Test Coverage

The tests cover:

-   ✅ All public methods in `DeviceHelper`
-   ✅ All public methods in `GetIPInformationCriteria`
-   ✅ Dependency injection setup in `Extensions`
-   ✅ Model property behavior
-   ✅ Edge cases and error scenarios
-   ✅ Async method testing
-   ✅ Mocking of external dependencies

## Running Tests

```bash
# Run all tests
dotnet test

# Run with verbose output
dotnet test --verbosity normal

# Run specific test class
dotnet test --filter "DeviceHelperTests"
```

## Dependencies

-   **xUnit**: Testing framework
-   **Moq**: Mocking framework
-   **AutoFixture**: Test data generation
-   **Microsoft.NET.Test.Sdk**: Test discovery and execution
-   **coverlet.collector**: Code coverage collection

## Project Structure

```
AlphaPlus.RequestInfo.UnitTests/
├── DeviceHelperTests.cs          # Tests for DeviceHelper class
├── GetIPInformationCriteriaTests.cs  # Tests for criteria class
├── ExtensionsTests.cs            # Tests for DI setup
├── ModelsTests.cs                # Tests for model classes
├── Usings.cs                     # Global using statements
├── AlphaPlus.RequestInfo.UnitTests.csproj  # Project file
└── README.md                     # This file
```

## Notes

-   The tests use proper mocking techniques to isolate the units under test
-   HTTP context and headers are mocked using `HeaderDictionary` for realistic testing
-   Memory cache is added to service collection tests to support `IPService` dependencies
-   All tests follow AAA (Arrange-Act-Assert) pattern
-   Tests include both positive and negative scenarios
