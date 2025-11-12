using AlphaPlus.Settings.Site.Features.MemberInfo.Helpers;

namespace AlphaPlus.Settings.Site.UnitTests.Helpers;

public class JsonHelperTests
{
    [Fact]
    public void ToJsonFunc_Should_Return_Default_ToJson_When_Not_Set()
    {
        // Arrange - Reset the ThreadStatic field
        var jsonHelperType = typeof(JsonHelper);
        var toJsonFuncField = jsonHelperType.GetField("toJsonFunc", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Static);
        toJsonFuncField?.SetValue(null, null);

        // Act
        var func = JsonHelper.ToJsonFunc;

        // Assert
        Assert.NotNull(func);
    }

    [Fact]
    public void ToJsonFunc_Should_Be_Settable()
    {
        // Arrange
        Func<object, string> customFunc = obj => $"Custom: {obj}";

        // Act
        JsonHelper.ToJsonFunc = customFunc;

        // Assert
        Assert.Equal(customFunc, JsonHelper.ToJsonFunc);

        // Cleanup
        JsonHelper.ToJsonFunc = null;
    }

    [Fact]
    public void ToJson_Should_Serialize_Object_To_Json_With_Single_Quotes()
    {
        // Arrange
        var testObject = new { Name = "Test", Value = 123 };

        // Act
        var jsonHelperType = typeof(JsonHelper);
        var toJsonMethod = jsonHelperType.GetMethod("ToJson", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Static);
        var result = (string)toJsonMethod?.Invoke(null, new object[] { testObject });

        // Assert
        Assert.NotNull(result);
        Assert.Contains("'Name'", result);
        Assert.Contains("'Test'", result);
        Assert.Contains("'Value'", result);
        Assert.Contains("123", result);
        Assert.DoesNotContain("\"", result); // Should not contain double quotes
    }

    [Fact]
    public void ToJson_Should_Handle_Null_Object()
    {
        // Act
        var jsonHelperType = typeof(JsonHelper);
        var toJsonMethod = jsonHelperType.GetMethod("ToJson", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Static);
        var result = (string)toJsonMethod?.Invoke(null, new object[] { null });

        // Assert
        Assert.Equal("null", result);
    }

    [Fact]
    public void ToJson_Should_Handle_String_Value()
    {
        // Arrange
        var testString = "Hello World";

        // Act
        var jsonHelperType = typeof(JsonHelper);
        var toJsonMethod = jsonHelperType.GetMethod("ToJson", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Static);
        var result = (string)toJsonMethod?.Invoke(null, new object[] { testString });

        // Assert
        Assert.Equal("'Hello World'", result);
    }

    [Fact]
    public void ToJson_Should_Handle_Integer_Value()
    {
        // Arrange
        var testInt = 42;

        // Act
        var jsonHelperType = typeof(JsonHelper);
        var toJsonMethod = jsonHelperType.GetMethod("ToJson", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Static);
        var result = (string)toJsonMethod?.Invoke(null, new object[] { testInt });

        // Assert
        Assert.Equal("42", result);
    }

    [Fact]
    public void ToJson_Should_Handle_Boolean_Value()
    {
        // Arrange
        var testBool = true;

        // Act
        var jsonHelperType = typeof(JsonHelper);
        var toJsonMethod = jsonHelperType.GetMethod("ToJson", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Static);
        var result = (string)toJsonMethod?.Invoke(null, new object[] { testBool });

        // Assert
        Assert.Equal("true", result);
    }

    [Fact]
    public void ToJson_Should_Handle_Array_Value()
    {
        // Arrange
        var testArray = new[] { 1, 2, 3 };

        // Act
        var jsonHelperType = typeof(JsonHelper);
        var toJsonMethod = jsonHelperType.GetMethod("ToJson", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Static);
        var result = (string)toJsonMethod?.Invoke(null, new object[] { testArray });

        // Assert
        Assert.Contains("[", result);
        Assert.Contains("]", result);
        Assert.Contains("1", result);
        Assert.Contains("2", result);
        Assert.Contains("3", result);
    }

    [Fact]
    public void ToJsonFunc_Should_Use_Custom_Function_When_Set()
    {
        // Arrange
        Func<object, string> originalFunc = JsonHelper.ToJsonFunc;
        Func<object, string> customFunc = obj => $"CUSTOM_JSON: {obj}";
        JsonHelper.ToJsonFunc = customFunc;

        try
        {
            // Act
            var result = JsonHelper.ToJsonFunc("test");

            // Assert
            Assert.Equal("CUSTOM_JSON: test", result);
        }
        finally
        {
            // Cleanup
            JsonHelper.ToJsonFunc = originalFunc;
        }
    }
}
