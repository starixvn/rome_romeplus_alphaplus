using Microsoft.AspNetCore.Http;
using AgencyCash.API.Shared.Models;

namespace AgencyCash.API.Tests.Shared.Models;

[Trait("Category", "Shared")]
public class ModelValidationResultTests
{
    [Fact]
    public void Constructor_WithDictionary_SetsErrorAndStatusCode()
    {
        // Arrange
        var errors = new Dictionary<string, string[]>
        {
            ["Property1"] = new[] { "Error1", "Error2" },
            ["Property2"] = new[] { "Error3" }
        };

        // Act
        var result = new ModelValidationResult(errors);

        // Assert
        Assert.Equal(StatusCodes.Status400BadRequest, result.Code);
        Assert.Equal("Model validation failed.", result.Message);
        Assert.Same(errors, result.Error);
    }

    [Fact]
    public void Constructor_WithKeyValue_SetsErrorAndStatusCode()
    {
        // Arrange
        string key = "PropertyName";
        string value = "Error message";

        // Act
        var result = new ModelValidationResult(key, value);

        // Assert
        Assert.Equal(StatusCodes.Status400BadRequest, result.Code);
        Assert.Equal("Model validation failed.", result.Message);
        Assert.Contains(key, result.Error.Keys);
        Assert.Single(result.Error[key]);
        Assert.Equal(value, result.Error[key][0]);
    }

    [Fact]
    public void Error_Property_IsNotNull_AfterInitialization()
    {
        // Arrange & Act
        var result1 = new ModelValidationResult(new Dictionary<string, string[]>());
        var result2 = new ModelValidationResult("key", "value");

        // Assert
        Assert.NotNull(result1.Error);
        Assert.NotNull(result2.Error);
    }
}
