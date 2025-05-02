# CashService.API Tests

This project contains unit tests for the CashService.API project.

## Project Structure

The test project follows the same structure as the main project to make it easy to locate tests:

```
CashService.API.Tests/
├── Constants/           # Tests for constant classes
├── Controllers/         # Tests for API controllers
├── Services/           # Tests for service implementations
├── Models/             # Tests for model validations
└── Helpers/           # Tests for helper classes
```

## Testing Guidelines

1. **Naming Convention**
   - Test classes should be named as `{ClassUnderTest}Tests`
   - Test methods should follow the pattern: `{MethodUnderTest}_{Scenario}_{ExpectedBehavior}`

2. **Test Structure**
   - Use the Arrange-Act-Assert pattern
   - Keep tests focused and test one thing at a time
   - Use meaningful test names that describe the scenario being tested

3. **Test Coverage**
   - Aim for high test coverage but focus on testing business logic
   - Test both happy path and edge cases
   - Include negative test cases where appropriate

4. **Mocking**
   - Use Moq for mocking dependencies
   - Only mock what is necessary
   - Keep mock setup simple and readable

## Running Tests

To run the tests:

```bash
dotnet test
```

To run tests with code coverage:

```bash
dotnet test /p:CollectCoverage=true /p:CoverletOutputFormat=cobertura
```

## Dependencies

- xUnit - Testing framework
- Moq - Mocking framework
- coverlet.collector - Code coverage tool 