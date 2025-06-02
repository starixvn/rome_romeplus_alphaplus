using AlphaPlus.Reports.Site.Features.Shared.Helpers;

namespace AlphaPlus.Reports.Site.UnitTests.Features.Shared.Helpers;

[Trait("Category", "Shared")]
public class JsonHelperTest
{
    [Fact]
    public void TryParse_With_Valid_Json_Should_Return_True_And_Deserialize_Object()
    {
        // Arrange
        string json = "{\"name\":\"Test\",\"value\":123}";

        // Act
        bool result = JsonHelper.TryParse<TestClass>(json, out var value);

        // Assert
        Assert.True(result);
        Assert.NotNull(value);
        Assert.Equal("Test", value.Name);
        Assert.Equal(123, value.Value);
    }

    [Fact]
    public void TryParse_With_Invalid_Json_Should_Return_False()
    {
        // Arrange
        string json = "invalid json format";

        // Act
        bool result = JsonHelper.TryParse<TestClass>(json, out var value);

        // Assert
        Assert.False(result);
        Assert.Null(value);
    }

    [Fact]
    public void TryParse_With_Null_Json_Should_Return_False()
    {
        // Arrange
        string json = null;

        // Act
        bool result = JsonHelper.TryParse<TestClass>(json, out var value);

        // Assert
        Assert.False(result);
        Assert.Null(value);
    }

    [Fact]
    public void TryParse_With_Empty_Json_Should_Return_False()
    {
        // Arrange
        string json = "";

        // Act
        bool result = JsonHelper.TryParse<TestClass>(json, out var value);

        // Assert
        Assert.False(result);
        Assert.Null(value);
    }

    [Fact]
    public void TryParse_With_Whitespace_Json_Should_Return_False()
    {
        // Arrange
        string json = "   ";

        // Act
        bool result = JsonHelper.TryParse<TestClass>(json, out var value);

        // Assert
        Assert.False(result);
        Assert.Null(value);
    }

    [Fact]
    public void ToJson_Should_Serialize_Object()
    {
        // Arrange
        var testObj = new TestClass { Name = "Test", Value = 123 };

        // Act
        string result = JsonHelper.ToJson(testObj);

        // Assert
        Assert.Contains("\"Name\":\"Test\"", result);
        Assert.Contains("\"Value\":123", result);
    }

    [Fact]
    public void ToJson_Should_Ignore_Default_Values()
    {
        // Arrange
        var testObj = new TestClass { Name = "Test" }; // Value will be default (0)

        // Act
        string result = JsonHelper.ToJson(testObj);

        // Assert
        Assert.Contains("\"Name\":\"Test\"", result);
        Assert.DoesNotContain("\"Value\":", result);
    }

    private class TestClass
    {
        public string Name { get; set; }
        public int Value { get; set; }
    }
}