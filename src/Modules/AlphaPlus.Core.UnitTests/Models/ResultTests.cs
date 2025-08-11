using AlphaPlus.Core.Models;

namespace AlphaPlus.Core.UnitTests.Models;

public class ResultTests
{
    [Fact]
    public void Default_Constructor_Should_Create_Result_With_Default_Values()
    {
        // Act
        var result = new Result();

        // Assert
        Assert.Equal(0, result.Code);
        Assert.Null(result.Message);
        Assert.True(result.IsSuccessful);
        Assert.False(result.IsFailed);
        Assert.True(result.IsOk);
        Assert.False(result.IsNotOk);
    }

    [Fact]
    public void Constructor_With_Code_Should_Set_Code_And_Default_Message()
    {
        // Arrange
        var code = 42;

        // Act
        var result = new Result(code);

        // Assert
        Assert.Equal(code, result.Code);
        Assert.Null(result.Message);
        Assert.False(result.IsSuccessful);
        Assert.True(result.IsFailed);
        Assert.False(result.IsOk);
        Assert.True(result.IsNotOk);
    }

    [Fact]
    public void Constructor_With_Code_And_Message_Should_Set_Both_Properties()
    {
        // Arrange
        var code = 100;
        var message = "Test message";

        // Act
        var result = new Result(code, message);

        // Assert
        Assert.Equal(code, result.Code);
        Assert.Equal(message, result.Message);
        Assert.False(result.IsSuccessful);
        Assert.True(result.IsFailed);
        Assert.False(result.IsOk);
        Assert.True(result.IsNotOk);
    }

    [Fact]
    public void Constructor_With_Code_Zero_Should_Create_Successful_Result()
    {
        // Arrange
        var code = 0;
        var message = "Success message";

        // Act
        var result = new Result(code, message);

        // Assert
        Assert.Equal(code, result.Code);
        Assert.Equal(message, result.Message);
        Assert.True(result.IsSuccessful);
        Assert.False(result.IsFailed);
        Assert.True(result.IsOk);
        Assert.False(result.IsNotOk);
    }

    [Theory]
    [InlineData(0, true, false, true, false)]
    [InlineData(1, false, true, false, true)]
    [InlineData(-1, false, true, false, true)]
    [InlineData(100, false, true, false, true)]
    public void IsSuccessful_And_IsFailed_Should_Return_Correct_Values_Based_On_Code(int code, bool expectedIsSuccessful, bool expectedIsFailed, bool expectedIsOk, bool expectedIsNotOk)
    {
        // Arrange
        var result = new Result(code);

        // Act & Assert
        Assert.Equal(expectedIsSuccessful, result.IsSuccessful);
        Assert.Equal(expectedIsFailed, result.IsFailed);
        Assert.Equal(expectedIsOk, result.IsOk);
        Assert.Equal(expectedIsNotOk, result.IsNotOk);
    }

    [Fact]
    public void IsOk_And_IsNotOk_Should_Return_Same_Values_As_IsSuccessful_And_IsFailed()
    {
        // Arrange
        var result = new Result(42);

        // Act & Assert
        Assert.Equal(result.IsSuccessful, result.IsOk);
        Assert.Equal(result.IsFailed, result.IsNotOk);
    }

    [Fact]
    public void CreateErrorResult_With_Message_Only_Should_Use_Default_ErrorCode()
    {
        // Arrange
        var message = "Error occurred";

        // Act
        var result = Result.CreateErrorResult(message);

        // Assert
        Assert.Equal(Result.ErrorCode, result.Code);
        Assert.Equal(message, result.Message);
        Assert.False(result.IsSuccessful);
        Assert.True(result.IsFailed);
    }

    [Fact]
    public void CreateErrorResult_With_Message_And_Custom_Code_Should_Use_Custom_Code()
    {
        // Arrange
        var message = "Custom error";
        var customCode = 999;

        // Act
        var result = Result.CreateErrorResult(message, customCode);

        // Assert
        Assert.Equal(customCode, result.Code);
        Assert.Equal(message, result.Message);
        Assert.False(result.IsSuccessful);
        Assert.True(result.IsFailed);
    }

    [Fact]
    public void CreateErrorResult_With_Null_Code_Should_Use_Default_ErrorCode()
    {
        // Arrange
        var message = "Error with null code";

        // Act
        var result = Result.CreateErrorResult(message, null);

        // Assert
        Assert.Equal(Result.ErrorCode, result.Code);
        Assert.Equal(message, result.Message);
        Assert.False(result.IsSuccessful);
        Assert.True(result.IsFailed);
    }

    [Fact]
    public void CreateSuccessfulResult_With_Empty_Message_Should_Create_Successful_Result()
    {
        // Act
        var result = Result.CreateSuccessfulResult();

        // Assert
        Assert.Equal(0, result.Code);
        Assert.Equal(string.Empty, result.Message);
        Assert.True(result.IsSuccessful);
        Assert.False(result.IsFailed);
        Assert.True(result.IsOk);
        Assert.False(result.IsNotOk);
    }

    [Fact]
    public void CreateSuccessfulResult_With_Message_Should_Create_Successful_Result_With_Message()
    {
        // Arrange
        var message = "Operation completed successfully";

        // Act
        var result = Result.CreateSuccessfulResult(message);

        // Assert
        Assert.Equal(0, result.Code);
        Assert.Equal(message, result.Message);
        Assert.True(result.IsSuccessful);
        Assert.False(result.IsFailed);
        Assert.True(result.IsOk);
        Assert.False(result.IsNotOk);
    }

    [Fact]
    public void ErrorCode_Constant_Should_Be_One()
    {
        // Assert
        Assert.Equal(1, Result.ErrorCode);
    }

    [Fact]
    public void Properties_Should_Be_Settable()
    {
        // Arrange
        var result = new Result();

        // Act
        result.Code = 123;
        result.Message = "Updated message";

        // Assert
        Assert.Equal(123, result.Code);
        Assert.Equal("Updated message", result.Message);
        Assert.False(result.IsSuccessful);
        Assert.True(result.IsFailed);
    }

    [Fact]
    public void Properties_Should_Be_Readable()
    {
        // Arrange
        var code = 456;
        var message = "Readable message";
        var result = new Result(code, message);

        // Act & Assert
        Assert.Equal(code, result.Code);
        Assert.Equal(message, result.Message);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(1)]
    [InlineData(-1)]
    [InlineData(int.MaxValue)]
    [InlineData(int.MinValue)]
    public void Code_Should_Accept_Any_Integer_Value(int codeValue)
    {
        // Act
        var result = new Result(codeValue);

        // Assert
        Assert.Equal(codeValue, result.Code);
    }

    [Theory]
    [InlineData("")]
    [InlineData("Hello World")]
    [InlineData("Special characters: !@#$%^&*()")]
    [InlineData("Unicode: 🚀🌟🎉")]
    [InlineData("Very long message")]
    public void Message_Should_Accept_Any_String_Value(string messageValue)
    {
        // Act
        var result = new Result(0, messageValue);

        // Assert
        Assert.Equal(messageValue, result.Message);
    }
}
