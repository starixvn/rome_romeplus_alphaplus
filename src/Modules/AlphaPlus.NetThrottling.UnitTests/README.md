# AlphaPlus.NetThrottling.UnitTests

This project contains unit tests for the AlphaPlus.NetThrottling module, which provides network throttling functionality for ASP.NET Core applications.

## Test Structure

The test project follows the same structure as the main project:

-   **Filters/** - Tests for filter implementations (IpFilter, SessionFilter, WhitelistSessionFilter, etc.)
-   **Models/** - Tests for data models (CacheIdentity, ThrottleCounter, ThrottlePolicy, ThrottleResult)
-   **Resolvers/** - Tests for resolver implementations (HttpErrorResolver, BaseResolver, etc.)
-   **Utils/** - Tests for utility classes (Clock, Extensions, UserSessionFactory)
-   **NetThrottlerTests.cs** - Tests for the main NetThrottler class
-   **NetThrottlingMiddlewareTests.cs** - Tests for the middleware implementation
-   **ServiceCollectionExtensionsTests.cs** - Tests for dependency injection extensions

## Test Coverage

The tests cover:

-   **NetThrottler**: Core throttling logic, blacklist management, cache operations
-   **Filters**: IP-based, session-based, and whitelist filtering
-   **Resolvers**: Error resolution and response handling
-   **Models**: Data validation and behavior
-   **Utils**: Utility functions and extensions
-   **Middleware**: HTTP pipeline integration
-   **Extensions**: Dependency injection configuration

## Running Tests

```bash
dotnet test
```

## Test Dependencies

-   **xUnit**: Testing framework
-   **Moq**: Mocking framework
-   **AutoFixture**: Test data generation
-   **Microsoft.NET.Test.Sdk**: Test discovery and execution
