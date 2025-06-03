using AlphaPlus.Reports.Features.IPMatching;

namespace AlphaPlus.Reports.UnitTests.Features.IPMatching;

[Trait("Category", "IPMatching")]
public class IPMatchingModelTests
{
    [Fact]
    public void Constructor_ShouldInitializeProperties()
    {
        // Act
        var model = new IPMatchingModel();

        // Assert
        Assert.Null(model.IP);
        Assert.Equal(0, model.MemberCount);
        Assert.Equal(0, model.BetCount);
        Assert.Equal(0m, model.Stake);
        Assert.Equal(0m, model.MemberWinLoss);
        Assert.Equal(0m, model.SuperWinLoss);
        Assert.Equal(0m, model.Company);
    }

    [Fact]
    public void Properties_ShouldSetAndGetCorrectly()
    {
        // Arrange
        var model = new IPMatchingModel();

        // Act
        model.IP = "192.168.1.1";
        model.MemberCount = 10;
        model.BetCount = 100;
        model.Stake = 1000m;
        model.MemberWinLoss = 500m;
        model.SuperWinLoss = 200m;
        model.Company = 300m;

        // Assert
        Assert.Equal("192.168.1.1", model.IP);
        Assert.Equal(10, model.MemberCount);
        Assert.Equal(100, model.BetCount);
        Assert.Equal(1000m, model.Stake);
        Assert.Equal(500m, model.MemberWinLoss);
        Assert.Equal(200m, model.SuperWinLoss);
        Assert.Equal(300m, model.Company);
    }

    [Fact]
    public void AdditionOperator_ShouldAddValuesCorrectly()
    {
        // Arrange
        var model1 = new IPMatchingModel
        {
            IP = "192.168.1.1",
            MemberCount = 5,
            BetCount = 50,
            Stake = 500m,
            MemberWinLoss = 200m,
            SuperWinLoss = 100m,
            Company = 150m
        };

        var model2 = new IPMatchingModel
        {
            IP = "192.168.1.2",
            MemberCount = 3,
            BetCount = 30,
            Stake = 300m,
            MemberWinLoss = 150m,
            SuperWinLoss = 75m,
            Company = 100m
        };

        // Act
        var result = model1 + model2;

        // Assert
        Assert.Equal("192.168.1.1", result.IP); // IP should remain the same as model1
        Assert.Equal(8, result.MemberCount);
        Assert.Equal(80, result.BetCount);
        Assert.Equal(800m, result.Stake);
        Assert.Equal(350m, result.MemberWinLoss);
        Assert.Equal(175m, result.SuperWinLoss);
        Assert.Equal(250m, result.Company);
    }

    [Fact]
    public void AdditionOperator_ShouldModifyFirstOperand()
    {
        // Arrange
        var model1 = new IPMatchingModel
        {
            IP = "192.168.1.1",
            MemberCount = 5,
            BetCount = 50
        };

        var model2 = new IPMatchingModel
        {
            IP = "192.168.1.2",
            MemberCount = 3,
            BetCount = 30
        };

        // Act
        var result = model1 + model2;

        // Assert
        Assert.Same(model1, result); // Should return the first model instance
    }
}