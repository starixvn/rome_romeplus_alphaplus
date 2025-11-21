using AlphaPlus.Settings.MemberInfo.Domain.Constants;

namespace AlphaPlus.Settings.MemberInfo.Domain.UnitTests.Constants;

public class SportTypeConstantsTests
{
    [Fact]
    public void SportTypeConstants_Should_Have_Correct_Values()
    {
        // Assert
        Assert.Equal(200, SportTypeConstants.Sabaclub);
        Assert.Equal(241, SportTypeConstants.BBin);
        Assert.Equal(171, SportTypeConstants.VGaming);
        Assert.Equal(1, SportTypeConstants.SoccerParlay);
        Assert.Equal(99, SportTypeConstants.MixedSportsParlay);
    }

    [Fact]
    public void SportTypeConstants_Should_Be_Integer_Type()
    {
        // Assert
        Assert.IsType<int>(SportTypeConstants.Sabaclub);
        Assert.IsType<int>(SportTypeConstants.BBin);
        Assert.IsType<int>(SportTypeConstants.VGaming);
        Assert.IsType<int>(SportTypeConstants.SoccerParlay);
        Assert.IsType<int>(SportTypeConstants.MixedSportsParlay);
    }

    [Fact]
    public void SportTypeConstants_Should_Be_Unique()
    {
        // Arrange
        var values = new[]
        {
            SportTypeConstants.Sabaclub,
            SportTypeConstants.BBin,
            SportTypeConstants.VGaming,
            SportTypeConstants.SoccerParlay,
            SportTypeConstants.MixedSportsParlay
        };

        // Act
        var uniqueValues = values.Distinct().ToArray();

        // Assert
        Assert.Equal(values.Length, uniqueValues.Length);
    }

    [Fact]
    public void SportTypeConstants_Should_Be_Positive_Values()
    {
        // Assert
        Assert.True(SportTypeConstants.Sabaclub > 0);
        Assert.True(SportTypeConstants.BBin > 0);
        Assert.True(SportTypeConstants.VGaming > 0);
        Assert.True(SportTypeConstants.SoccerParlay > 0);
        Assert.True(SportTypeConstants.MixedSportsParlay > 0);
    }
}
