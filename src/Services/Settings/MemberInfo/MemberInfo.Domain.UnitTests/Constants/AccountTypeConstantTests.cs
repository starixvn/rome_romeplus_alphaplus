using AlphaPlus.Settings.MemberInfo.Domain.Constants;

namespace AlphaPlus.Settings.MemberInfo.Domain.UnitTests.Constants;

public class AccountTypeConstantTests
{
    [Fact]
    public void AccountTypeConstant_Should_Have_Correct_Credit_Value()
    {
        // Act & Assert
        Assert.Equal(0, AccountTypeConstant.Credit);
    }

    [Fact]
    public void AccountTypeConstant_Should_Have_Correct_Cash_Value()
    {
        // Act & Assert
        Assert.Equal(1, AccountTypeConstant.Cash);
    }

    [Fact]
    public void AccountTypeConstant_Should_Have_Correct_All_Value()
    {
        // Act & Assert
        Assert.Equal(2, AccountTypeConstant.All);
    }

    [Fact]
    public void AccountTypeConstant_Should_Have_Sequential_Values()
    {
        // Arrange
        var expectedValues = new[] { 0, 1, 2 };
        var actualValues = new[]
        {
            AccountTypeConstant.Credit,
            AccountTypeConstant.Cash,
            AccountTypeConstant.All
        };

        // Act & Assert
        Assert.Equal(expectedValues, actualValues);
    }

    [Fact]
    public void AccountTypeConstant_Should_Have_NonNegative_Values()
    {
        // Act & Assert
        Assert.True(AccountTypeConstant.Credit >= 0);
        Assert.True(AccountTypeConstant.Cash >= 0);
        Assert.True(AccountTypeConstant.All >= 0);
    }
}
