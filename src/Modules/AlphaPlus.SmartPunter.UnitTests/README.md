# AlphaPlus.SmartPunter.UnitTests

This project contains unit tests for the AlphaPlus.SmartPunter module.

## Overview

The AlphaPlus.SmartPunter module provides functionality for managing smart punter operations, including:

-   Smart punter service operations
-   Bet list monitoring
-   User permission checking
-   Logging functionality

## Test Coverage

### Services

-   **SmartPunterServiceTests**: Tests for the main smart punter service operations
-   **SmartPunterBetListMonitorTests**: Tests for bet list monitoring functionality

### Criteria

-   **InternalAdminCheckerTests**: Tests for internal admin checking criteria
-   **NAPUserPermissionTests**: Tests for user permission criteria
-   **SmartPunterSMACheckerTests**: Tests for smart punter SMA checking criteria
-   **SmartPunterWriteLogTests**: Tests for write log criteria
-   **SmartPunterWriteLogNoDataTests**: Tests for write log no data criteria
-   **SmartPunterSMACheckerWriteLogTests**: Tests for SMA checker write log criteria

### Models

-   **SMACustomerTests**: Tests for the SMA customer model

## Running Tests

To run the tests, use one of the following commands:

```bash
# Run all tests
dotnet test

# Run tests with verbose output
dotnet test --verbosity normal

# Run tests with coverage
dotnet test --collect:"XPlat Code Coverage"
```

## Dependencies

-   **xUnit**: Testing framework
-   **Moq**: Mocking framework
-   **AutoFixture**: Test data generation
-   **Microsoft.NET.Test.Sdk**: Test SDK
-   **coverlet.collector**: Code coverage collection

## Test Structure

Tests follow the Arrange-Act-Assert pattern and use dependency injection with mocked dependencies to ensure proper isolation and testability.

## Contributing

When adding new functionality to the AlphaPlus.SmartPunter module, please ensure corresponding unit tests are added to maintain good test coverage.
