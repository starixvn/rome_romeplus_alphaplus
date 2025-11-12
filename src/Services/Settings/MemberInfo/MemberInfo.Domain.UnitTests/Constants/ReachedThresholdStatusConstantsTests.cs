using AlphaPlus.Settings.MemberInfo.Domain.Constants;

namespace AlphaPlus.Settings.MemberInfo.Domain.UnitTests.Constants;

public class ReachedThresholdStatusConstantsTests
{
    [Fact]
    public void ReachedThresholdStatusConstants_Should_Have_Correct_Values()
    {
        // Assert
        Assert.Equal(0, ReachedThresholdStatusConstants.All);
        Assert.Equal(1, ReachedThresholdStatusConstants.Open);
        Assert.Equal(2, ReachedThresholdStatusConstants.Suspended);
        Assert.Equal(3, ReachedThresholdStatusConstants.Closed);
        Assert.Equal(4, ReachedThresholdStatusConstants.Disabled);
        Assert.Equal(5, ReachedThresholdStatusConstants.Locked);
    }

    [Fact]
    public void ReachedThresholdStatusConstants_Should_Have_NonNegative_Values()
    {
        // Act & Assert
        Assert.True(ReachedThresholdStatusConstants.All >= 0);
        Assert.True(ReachedThresholdStatusConstants.Open >= 0);
        Assert.True(ReachedThresholdStatusConstants.Suspended >= 0);
        Assert.True(ReachedThresholdStatusConstants.Closed >= 0);
        Assert.True(ReachedThresholdStatusConstants.Disabled >= 0);
        Assert.True(ReachedThresholdStatusConstants.Locked >= 0);
    }

    [Fact]
    public void ReachedThresholdStatusConstants_Should_Have_Unique_Values()
    {
        // Arrange
        var values = new[]
        {
            ReachedThresholdStatusConstants.All,
            ReachedThresholdStatusConstants.Open,
            ReachedThresholdStatusConstants.Suspended,
            ReachedThresholdStatusConstants.Closed,
            ReachedThresholdStatusConstants.Disabled,
            ReachedThresholdStatusConstants.Locked
        };

        // Act & Assert
        Assert.Equal(values.Length, values.Distinct().Count());
    }

    [Fact]
    public void ReachedThresholdStatusConstants_Items_Should_Contain_All_Constants()
    {
        // Assert
        Assert.True(ReachedThresholdStatusConstants.Items.ContainsKey(ReachedThresholdStatusConstants.All));
        Assert.True(ReachedThresholdStatusConstants.Items.ContainsKey(ReachedThresholdStatusConstants.Open));
        Assert.True(ReachedThresholdStatusConstants.Items.ContainsKey(ReachedThresholdStatusConstants.Suspended));
        Assert.True(ReachedThresholdStatusConstants.Items.ContainsKey(ReachedThresholdStatusConstants.Closed));
        Assert.True(ReachedThresholdStatusConstants.Items.ContainsKey(ReachedThresholdStatusConstants.Disabled));
        Assert.True(ReachedThresholdStatusConstants.Items.ContainsKey(ReachedThresholdStatusConstants.Locked));
    }

    [Fact]
    public void ReachedThresholdStatusConstants_Items_Should_Have_Correct_Names()
    {
        // Assert
        Assert.Equal("All", ReachedThresholdStatusConstants.Items[ReachedThresholdStatusConstants.All]);
        Assert.Equal("Open", ReachedThresholdStatusConstants.Items[ReachedThresholdStatusConstants.Open]);
        Assert.Equal("Suspended", ReachedThresholdStatusConstants.Items[ReachedThresholdStatusConstants.Suspended]);
        Assert.Equal("Closed", ReachedThresholdStatusConstants.Items[ReachedThresholdStatusConstants.Closed]);
        Assert.Equal("Disabled", ReachedThresholdStatusConstants.Items[ReachedThresholdStatusConstants.Disabled]);
        Assert.Equal("Locked", ReachedThresholdStatusConstants.Items[ReachedThresholdStatusConstants.Locked]);
    }

    [Fact]
    public void GetReachedThresholdStatus_With_Valid_Key_Should_Return_Correct_Status()
    {
        // Act & Assert
        Assert.Equal("All", ReachedThresholdStatusConstants.GetReachedThresholdStatus(ReachedThresholdStatusConstants.All));
        Assert.Equal("Open", ReachedThresholdStatusConstants.GetReachedThresholdStatus(ReachedThresholdStatusConstants.Open));
        Assert.Equal("Suspended", ReachedThresholdStatusConstants.GetReachedThresholdStatus(ReachedThresholdStatusConstants.Suspended));
        Assert.Equal("Closed", ReachedThresholdStatusConstants.GetReachedThresholdStatus(ReachedThresholdStatusConstants.Closed));
        Assert.Equal("Disabled", ReachedThresholdStatusConstants.GetReachedThresholdStatus(ReachedThresholdStatusConstants.Disabled));
        Assert.Equal("Locked", ReachedThresholdStatusConstants.GetReachedThresholdStatus(ReachedThresholdStatusConstants.Locked));
    }

    [Fact]
    public void GetReachedThresholdStatus_With_Invalid_Key_Should_Return_Empty_String()
    {
        // Act
        var result = ReachedThresholdStatusConstants.GetReachedThresholdStatus(255);

        // Assert
        Assert.Equal(string.Empty, result);
    }

    [Fact]
    public void ReachedThresholdStatusConstants_Items_Should_Have_Expected_Count()
    {
        // Assert
        Assert.Equal(6, ReachedThresholdStatusConstants.Items.Count);
    }

    [Theory]
    [InlineData(0, "All")]
    [InlineData(1, "Open")]
    [InlineData(2, "Suspended")]
    [InlineData(3, "Closed")]
    [InlineData(4, "Disabled")]
    [InlineData(5, "Locked")]
    public void GetReachedThresholdStatus_With_Valid_Keys_Should_Return_Expected_Values(byte key, string expectedValue)
    {
        // Act
        var result = ReachedThresholdStatusConstants.GetReachedThresholdStatus(key);

        // Assert
        Assert.Equal(expectedValue, result);
    }

    [Theory]
    [InlineData(6)]
    [InlineData(10)]
    [InlineData(255)]
    public void GetReachedThresholdStatus_With_Invalid_Keys_Should_Return_Empty_String(byte invalidKey)
    {
        // Act
        var result = ReachedThresholdStatusConstants.GetReachedThresholdStatus(invalidKey);

        // Assert
        Assert.Equal(string.Empty, result);
    }

    [Fact]
    public void ReachedThresholdStatusConstants_Should_Have_Sequential_Values()
    {
        // Arrange
        var expectedValues = new byte[] { 0, 1, 2, 3, 4, 5 };
        var actualValues = new[]
        {
            ReachedThresholdStatusConstants.All,
            ReachedThresholdStatusConstants.Open,
            ReachedThresholdStatusConstants.Suspended,
            ReachedThresholdStatusConstants.Closed,
            ReachedThresholdStatusConstants.Disabled,
            ReachedThresholdStatusConstants.Locked
        };

        // Act & Assert
        Assert.Equal(expectedValues, actualValues);
    }
}
