using AlphaPlus.Settings.MemberInfo.Domain.Constants;

namespace AlphaPlus.Settings.MemberInfo.Domain.UnitTests.Constants;

public class ProblemAccountStatusConstantsTests
{
    [Fact]
    public void ProblemAccountStatusConstants_Should_Have_Correct_Values()
    {
        // Assert
        Assert.Equal(1, ProblemAccountStatusConstants.Open);
        Assert.Equal(2, ProblemAccountStatusConstants.Suspended);
        Assert.Equal(3, ProblemAccountStatusConstants.Closed);
        Assert.Equal(4, ProblemAccountStatusConstants.Disabled);
    }

    [Fact]
    public void ProblemAccountStatusConstants_Should_Have_Positive_Values()
    {
        // Act & Assert
        Assert.True(ProblemAccountStatusConstants.Open > 0);
        Assert.True(ProblemAccountStatusConstants.Suspended > 0);
        Assert.True(ProblemAccountStatusConstants.Closed > 0);
        Assert.True(ProblemAccountStatusConstants.Disabled > 0);
    }

    [Fact]
    public void ProblemAccountStatusConstants_Should_Have_Unique_Values()
    {
        // Arrange
        var values = new[]
        {
            ProblemAccountStatusConstants.Open,
            ProblemAccountStatusConstants.Suspended,
            ProblemAccountStatusConstants.Closed,
            ProblemAccountStatusConstants.Disabled
        };

        // Act & Assert
        Assert.Equal(values.Length, values.Distinct().Count());
    }

    [Fact]
    public void ProblemAccountStatusConstants_Items_Should_Contain_All_Constants()
    {
        // Assert
        Assert.True(ProblemAccountStatusConstants.Items.ContainsKey(ProblemAccountStatusConstants.Open));
        Assert.True(ProblemAccountStatusConstants.Items.ContainsKey(ProblemAccountStatusConstants.Suspended));
        Assert.True(ProblemAccountStatusConstants.Items.ContainsKey(ProblemAccountStatusConstants.Closed));
        Assert.True(ProblemAccountStatusConstants.Items.ContainsKey(ProblemAccountStatusConstants.Disabled));
    }

    [Fact]
    public void ProblemAccountStatusConstants_Items_Should_Have_Correct_Names()
    {
        // Assert
        Assert.Equal("Open", ProblemAccountStatusConstants.Items[ProblemAccountStatusConstants.Open]);
        Assert.Equal("Suspended", ProblemAccountStatusConstants.Items[ProblemAccountStatusConstants.Suspended]);
        Assert.Equal("Closed", ProblemAccountStatusConstants.Items[ProblemAccountStatusConstants.Closed]);
        Assert.Equal("Disabled", ProblemAccountStatusConstants.Items[ProblemAccountStatusConstants.Disabled]);
    }

    [Fact]
    public void GetProblemAccountStatus_With_Valid_Key_Should_Return_Correct_Status()
    {
        // Act & Assert
        Assert.Equal("Open", ProblemAccountStatusConstants.GetProblemAccountStatus(ProblemAccountStatusConstants.Open));
        Assert.Equal("Suspended", ProblemAccountStatusConstants.GetProblemAccountStatus(ProblemAccountStatusConstants.Suspended));
        Assert.Equal("Closed", ProblemAccountStatusConstants.GetProblemAccountStatus(ProblemAccountStatusConstants.Closed));
        Assert.Equal("Disabled", ProblemAccountStatusConstants.GetProblemAccountStatus(ProblemAccountStatusConstants.Disabled));
    }

    [Fact]
    public void GetProblemAccountStatus_With_Invalid_Key_Should_Return_Empty_String()
    {
        // Act
        var result = ProblemAccountStatusConstants.GetProblemAccountStatus(255);

        // Assert
        Assert.Equal(string.Empty, result);
    }

    [Fact]
    public void ProblemAccountStatusConstants_Items_Should_Have_Expected_Count()
    {
        // Assert
        Assert.Equal(4, ProblemAccountStatusConstants.Items.Count);
    }

    [Theory]
    [InlineData(1, "Open")]
    [InlineData(2, "Suspended")]
    [InlineData(3, "Closed")]
    [InlineData(4, "Disabled")]
    public void GetProblemAccountStatus_With_Valid_Keys_Should_Return_Expected_Values(byte key, string expectedValue)
    {
        // Act
        var result = ProblemAccountStatusConstants.GetProblemAccountStatus(key);

        // Assert
        Assert.Equal(expectedValue, result);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(5)]
    [InlineData(255)]
    public void GetProblemAccountStatus_With_Invalid_Keys_Should_Return_Empty_String(byte invalidKey)
    {
        // Act
        var result = ProblemAccountStatusConstants.GetProblemAccountStatus(invalidKey);

        // Assert
        Assert.Equal(string.Empty, result);
    }
}
