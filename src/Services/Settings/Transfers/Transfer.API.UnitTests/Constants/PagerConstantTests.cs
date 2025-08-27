using AlphaPlus.Settings.Transfer.API.Constants;

namespace AlphaPlus.Settings.Transfer.API.UnitTests.Constants;

public class PagerConstantTests
{
    [Fact]
    public void PagerConstant_Should_Have_Correct_DefaultPageSize_Value()
    {
        // Act & Assert
        Assert.Equal(500, PagerConstant.DefaultPageSize);
    }

    [Fact]
    public void PagerConstant_Should_Have_Correct_DefaultPageIndex_Value()
    {
        // Act & Assert
        Assert.Equal(1, PagerConstant.DefaultPageIndex);
    }

    [Fact]
    public void PagerConstant_Should_Have_Positive_DefaultPageSize()
    {
        // Act & Assert
        Assert.True(PagerConstant.DefaultPageSize > 0);
    }

    [Fact]
    public void PagerConstant_Should_Have_Positive_DefaultPageIndex()
    {
        // Act & Assert
        Assert.True(PagerConstant.DefaultPageIndex > 0);
    }

    [Fact]
    public void PagerConstant_Should_Have_Reasonable_DefaultPageSize()
    {
        // Act & Assert
        Assert.True(PagerConstant.DefaultPageSize >= 10);
        Assert.True(PagerConstant.DefaultPageSize <= 1000);
    }

    [Fact]
    public void PagerConstant_Should_Have_DefaultPageIndex_As_First_Page()
    {
        // Act & Assert
        Assert.Equal(1, PagerConstant.DefaultPageIndex);
    }
}
