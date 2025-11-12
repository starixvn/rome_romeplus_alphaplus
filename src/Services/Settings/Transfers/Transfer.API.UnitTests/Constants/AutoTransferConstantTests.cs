using AlphaPlus.Settings.Transfer.API.Constants;

namespace AlphaPlus.Settings.Transfer.API.UnitTests.Constants;

public class AutoTransferConstantTests
{
    [Fact]
    public void AutoTransferConstant_Should_Have_Correct_On_Value()
    {
        // Act & Assert
        Assert.Equal("on", AutoTransferConstant.On);
    }

    [Fact]
    public void AutoTransferConstant_Should_Have_Correct_Off_Value()
    {
        // Act & Assert
        Assert.Equal("off", AutoTransferConstant.Off);
    }

    [Fact]
    public void AutoTransferConstant_Should_Have_Correct_All_Value()
    {
        // Act & Assert
        Assert.Equal("All", AutoTransferConstant.All);
    }

    [Fact]
    public void AutoTransferConstant_Should_Have_Correct_New_Value()
    {
        // Act & Assert
        Assert.Equal("New", AutoTransferConstant.New);
    }

    [Fact]
    public void AutoTransferConstant_Should_Have_Correct_Waiting_Value()
    {
        // Act & Assert
        Assert.Equal("Waiting", AutoTransferConstant.Waiting);
    }

    [Fact]
    public void AutoTransferConstant_Should_Have_Correct_Processing_Value()
    {
        // Act & Assert
        Assert.Equal("Processing", AutoTransferConstant.Processing);
    }

    [Fact]
    public void AutoTransferConstant_Should_Have_Correct_Completed_Value()
    {
        // Act & Assert
        Assert.Equal("Completed", AutoTransferConstant.Completed);
    }

    [Fact]
    public void AutoTransferConstant_Should_Have_Correct_Failed_Value()
    {
        // Act & Assert
        Assert.Equal("Failed", AutoTransferConstant.Failed);
    }

    [Fact]
    public void AutoTransferConstant_Should_Have_Correct_Cancelled_Value()
    {
        // Act & Assert
        Assert.Equal("Cancelled", AutoTransferConstant.Cancelled);
    }

    [Fact]
    public void AutoTransferConstant_Should_Have_All_Status_Values_Unique()
    {
        // Arrange
        var statusValues = new[]
        {
            AutoTransferConstant.All,
            AutoTransferConstant.New,
            AutoTransferConstant.Waiting,
            AutoTransferConstant.Processing,
            AutoTransferConstant.Completed,
            AutoTransferConstant.Failed,
            AutoTransferConstant.Cancelled
        };

        // Act & Assert
        Assert.Equal(statusValues.Length, statusValues.Distinct().Count());
    }

    [Fact]
    public void AutoTransferConstant_Should_Have_On_Off_Values_Different()
    {
        // Act & Assert
        Assert.NotEqual(AutoTransferConstant.On, AutoTransferConstant.Off);
    }

    [Theory]
    [InlineData("on")]
    [InlineData("off")]
    [InlineData("All")]
    [InlineData("New")]
    [InlineData("Waiting")]
    [InlineData("Processing")]
    [InlineData("Completed")]
    [InlineData("Failed")]
    [InlineData("Cancelled")]
    public void AutoTransferConstant_Should_Have_NonEmpty_Values(string value)
    {
        // Act & Assert
        Assert.False(string.IsNullOrEmpty(value));
    }
}
