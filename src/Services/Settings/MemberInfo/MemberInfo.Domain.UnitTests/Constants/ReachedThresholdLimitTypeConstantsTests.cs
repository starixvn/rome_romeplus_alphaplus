using AlphaPlus.Settings.MemberInfo.Domain.Constants;

namespace AlphaPlus.Settings.MemberInfo.Domain.UnitTests.Constants;

public class ReachedThresholdLimitTypeConstantsTests
{
    [Fact]
    public void ReachedThresholdLimitTypeConstants_Should_Have_Correct_Values()
    {
        // Assert
        Assert.Equal(0, ReachedThresholdLimitTypeConstants.All);
        Assert.Equal(1, ReachedThresholdLimitTypeConstants.Global);
        Assert.Equal(2, ReachedThresholdLimitTypeConstants.Personal);
    }

    [Fact]
    public void ReachedThresholdLimitTypeConstants_Should_Have_NonNegative_Values()
    {
        // Act & Assert
        Assert.True(ReachedThresholdLimitTypeConstants.All >= 0);
        Assert.True(ReachedThresholdLimitTypeConstants.Global >= 0);
        Assert.True(ReachedThresholdLimitTypeConstants.Personal >= 0);
    }

    [Fact]
    public void ReachedThresholdLimitTypeConstants_Should_Have_Unique_Values()
    {
        // Arrange
        var values = new[]
        {
            ReachedThresholdLimitTypeConstants.All,
            ReachedThresholdLimitTypeConstants.Global,
            ReachedThresholdLimitTypeConstants.Personal
        };

        // Act & Assert
        Assert.Equal(values.Length, values.Distinct().Count());
    }

    [Fact]
    public void ReachedThresholdLimitTypeConstants_Items_Should_Contain_All_Constants()
    {
        // Assert
        Assert.True(ReachedThresholdLimitTypeConstants.Items.ContainsKey(ReachedThresholdLimitTypeConstants.All));
        Assert.True(ReachedThresholdLimitTypeConstants.Items.ContainsKey(ReachedThresholdLimitTypeConstants.Global));
        Assert.True(ReachedThresholdLimitTypeConstants.Items.ContainsKey(ReachedThresholdLimitTypeConstants.Personal));
    }

    [Fact]
    public void ReachedThresholdLimitTypeConstants_Items_Should_Have_Correct_Names()
    {
        // Assert
        Assert.Equal("All", ReachedThresholdLimitTypeConstants.Items[ReachedThresholdLimitTypeConstants.All]);
        Assert.Equal("Global", ReachedThresholdLimitTypeConstants.Items[ReachedThresholdLimitTypeConstants.Global]);
        Assert.Equal("Personal", ReachedThresholdLimitTypeConstants.Items[ReachedThresholdLimitTypeConstants.Personal]);
    }

    [Fact]
    public void GetReachedThresholdLimitType_With_Valid_Key_Should_Return_Correct_Type()
    {
        // Act & Assert
        Assert.Equal("All", ReachedThresholdLimitTypeConstants.GetReachedThresholdLimitType(ReachedThresholdLimitTypeConstants.All));
        Assert.Equal("Global", ReachedThresholdLimitTypeConstants.GetReachedThresholdLimitType(ReachedThresholdLimitTypeConstants.Global));
        Assert.Equal("Personal", ReachedThresholdLimitTypeConstants.GetReachedThresholdLimitType(ReachedThresholdLimitTypeConstants.Personal));
    }

    [Fact]
    public void GetReachedThresholdLimitType_With_Invalid_Key_Should_Return_Empty_String()
    {
        // Act
        var result = ReachedThresholdLimitTypeConstants.GetReachedThresholdLimitType(255);

        // Assert
        Assert.Equal(string.Empty, result);
    }

    [Fact]
    public void ReachedThresholdLimitTypeConstants_Items_Should_Have_Expected_Count()
    {
        // Assert
        Assert.Equal(3, ReachedThresholdLimitTypeConstants.Items.Count);
    }

    [Theory]
    [InlineData(0, "All")]
    [InlineData(1, "Global")]
    [InlineData(2, "Personal")]
    public void GetReachedThresholdLimitType_With_Valid_Keys_Should_Return_Expected_Values(byte key, string expectedValue)
    {
        // Act
        var result = ReachedThresholdLimitTypeConstants.GetReachedThresholdLimitType(key);

        // Assert
        Assert.Equal(expectedValue, result);
    }

    [Theory]
    [InlineData(3)]
    [InlineData(10)]
    [InlineData(255)]
    public void GetReachedThresholdLimitType_With_Invalid_Keys_Should_Return_Empty_String(byte invalidKey)
    {
        // Act
        var result = ReachedThresholdLimitTypeConstants.GetReachedThresholdLimitType(invalidKey);

        // Assert
        Assert.Equal(string.Empty, result);
    }

    [Fact]
    public void ReachedThresholdLimitTypeConstants_Should_Have_Sequential_Values()
    {
        // Arrange
        var expectedValues = new byte[] { 0, 1, 2 };
        var actualValues = new[]
        {
            ReachedThresholdLimitTypeConstants.All,
            ReachedThresholdLimitTypeConstants.Global,
            ReachedThresholdLimitTypeConstants.Personal
        };

        // Act & Assert
        Assert.Equal(expectedValues, actualValues);
    }
}
