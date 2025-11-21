using AlphaPlus.Settings.MemberInfo.Domain.Constants;

namespace AlphaPlus.Settings.MemberInfo.Domain.UnitTests.Constants;

public class LockedAccessConstantTests
{
    [Fact]
    public void LockedAccessConstant_Should_Have_Correct_FalseOTPSC_Value()
    {
        // Act & Assert
        Assert.Equal(1, LockedAccessConstant.FalseOTPSC);
    }

    [Fact]
    public void LockedAccessConstant_Should_Have_Correct_FalsePassword_Value()
    {
        // Act & Assert
        Assert.Equal(2, LockedAccessConstant.FalsePassword);
    }

    [Fact]
    public void LockedAccessConstant_Should_Have_Positive_Values()
    {
        // Act & Assert
        Assert.True(LockedAccessConstant.FalseOTPSC > 0);
        Assert.True(LockedAccessConstant.FalsePassword > 0);
    }

    [Fact]
    public void LockedAccessConstant_Should_Have_Unique_Values()
    {
        // Arrange
        var values = new[]
        {
            LockedAccessConstant.FalseOTPSC,
            LockedAccessConstant.FalsePassword
        };

        // Act & Assert
        Assert.Equal(values.Length, values.Distinct().Count());
    }

    [Fact]
    public void LockedAccessConstant_Should_Have_Sequential_Values()
    {
        // Arrange
        var expectedValues = new byte[] { 1, 2 };
        var actualValues = new[]
        {
            LockedAccessConstant.FalseOTPSC,
            LockedAccessConstant.FalsePassword
        };

        // Act & Assert
        Assert.Equal(expectedValues, actualValues);
    }
}
