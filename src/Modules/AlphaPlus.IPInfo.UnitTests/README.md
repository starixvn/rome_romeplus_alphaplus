# AlphaPlus.IPInfo.UnitTests

This project contains unit tests for the AlphaPlus.IPInfo module, which provides IP address information and geolocation services.

## Project Structure

The test project follows the same structure as the main project:

-   **Services/** - Tests for service classes
    -   `IPInfoServiceTests.cs` - Tests for the main IP information service
    -   `IPInfoHelperTests.cs` - Tests for the IP helper utility class
-   **Models/** - Tests for model classes
    -   `IPRangeTests.cs` - Tests for the IP range model and comparison logic
-   **Extensions/** - Tests for extension methods
    -   `ListExtensionsTests.cs` - Tests for binary search and string joining extensions
-   **DataAccess/** - Tests for data access interfaces
    -   `IIPInfoDataAccessTests.cs` - Tests for the data access interface with mocks

## Test Coverage

The tests cover the following functionality:

### IPInfoService

-   IP address validation and processing
-   Country lookup for IPv4 and IPv6 addresses
-   Caching behavior
-   Error handling for invalid inputs

### IPInfoHelper

-   IPv4 to number conversion
-   IP address type detection (IPv4 vs IPv6)
-   IP list splitting and validation

### IPRange Model

-   Constructor validation
-   Comparison logic for ranges and values
-   Edge cases and error conditions

### ListExtensions

-   Binary search functionality for IP ranges
-   String joining operations
-   Null and empty list handling

### DataAccess Interface

-   Mock implementations for testing
-   Interface contract validation

## Running Tests

To run the tests, use one of the following commands:

```bash
# Run all tests
dotnet test

# Run tests with verbose output
dotnet test --verbosity normal

# Run tests with coverage
dotnet test --collect:"XPlat Code Coverage"

# Run specific test class
dotnet test --filter "FullyQualifiedName~IPInfoServiceTests"
```

## Dependencies

The test project uses the following testing frameworks and libraries:

-   **xUnit** - Testing framework
-   **Moq** - Mocking framework
-   **AutoFixture** - Test data generation
-   **coverlet.collector** - Code coverage collection

## Test Patterns

The tests follow these patterns:

1. **Arrange-Act-Assert** - Clear separation of test setup, execution, and verification
2. **Theory Tests** - Parameterized tests for multiple scenarios
3. **Mock Usage** - Proper mocking of dependencies
4. **Edge Cases** - Testing boundary conditions and error scenarios
5. **Async Testing** - Proper async/await patterns for asynchronous operations

## Contributing

When adding new tests:

1. Follow the existing naming conventions
2. Use descriptive test names that explain the scenario being tested
3. Include both positive and negative test cases
4. Test edge cases and error conditions
5. Use appropriate mocking for external dependencies
6. Ensure tests are independent and can run in any order
