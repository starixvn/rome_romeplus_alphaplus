using AlphaPlus.Reports.Features.WhiteLabel;

namespace AlphaPlus.Reports.UnitTests.Features.WhiteLabel;

[Trait("Category", "WhiteLabel")]
public class DailyWinLossModelTest
{
    [Fact]
    public void Constructor_ShouldInitializeProperties()
    {
        // Arrange
        var date = new DateTime(2023, 1, 1);
        var whiteLabelName = "TestWhiteLabel";
        var agent = "TestAgent";
        var master = "TestMaster";
        var memberWinLoss = 100.50m;
        var promotion = 25.75m;

        // Act
        var model = new DailyWinLossModel
        {
            WinLostDate = date,
            WhiteLabelName = whiteLabelName,
            Agent = agent,
            Master = master,
            MemberWinLoss = memberWinLoss,
            Promotion = promotion
        };

        // Assert
        Assert.Equal(date, model.WinLostDate);
        Assert.Equal(whiteLabelName, model.WhiteLabelName);
        Assert.Equal(agent, model.Agent);
        Assert.Equal(master, model.Master);
        Assert.Equal(memberWinLoss, model.MemberWinLoss);
        Assert.Equal(promotion, model.Promotion);
    }

    [Fact]
    public void OperatorPlus_ShouldAddNumericValues_WhenModelsAreAdded()
    {
        // Arrange
        var model1 = new DailyWinLossModel
        {
            WinLostDate = new DateTime(2023, 1, 1),
            WhiteLabelName = "Label1",
            Agent = "Agent1",
            Master = "Master1",
            MemberWinLoss = 100.50m,
            Promotion = 25.75m
        };

        var model2 = new DailyWinLossModel
        {
            WinLostDate = new DateTime(2023, 1, 2),
            WhiteLabelName = "Label2",
            Agent = "Agent2",
            Master = "Master2",
            MemberWinLoss = 200.25m,
            Promotion = 50.50m
        };

        // Act
        var result = model1 + model2;

        // Assert
        Assert.Equal(300.75m, result.MemberWinLoss);
        Assert.Equal(76.25m, result.Promotion);

        // The non-numeric properties should remain as in model1
        Assert.Equal(model1.WinLostDate, result.WinLostDate);
        Assert.Equal(model1.WhiteLabelName, result.WhiteLabelName);
        Assert.Equal(model1.Agent, result.Agent);
        Assert.Equal(model1.Master, result.Master);
    }

    [Fact]
    public void OperatorPlus_ShouldHandleNegativeValues_WhenModelsAreAdded()
    {
        // Arrange
        var model1 = new DailyWinLossModel
        {
            MemberWinLoss = 100.00m,
            Promotion = 20.00m
        };

        var model2 = new DailyWinLossModel
        {
            MemberWinLoss = -150.00m,
            Promotion = -30.00m
        };

        // Act
        var result = model1 + model2;

        // Assert
        Assert.Equal(-50.00m, result.MemberWinLoss);
        Assert.Equal(-10.00m, result.Promotion);
    }

    [Fact]
    public void OperatorPlus_ShouldReturnFirstModel_AsResultObject()
    {
        // Arrange
        var model1 = new DailyWinLossModel
        {
            WinLostDate = new DateTime(2023, 1, 1),
            WhiteLabelName = "Label1",
            MemberWinLoss = 100.00m,
            Promotion = 20.00m
        };

        var model2 = new DailyWinLossModel
        {
            WinLostDate = new DateTime(2023, 1, 2),
            WhiteLabelName = "Label2",
            MemberWinLoss = 200.00m,
            Promotion = 40.00m
        };

        // Act
        var result = model1 + model2;

        // Assert
        Assert.Same(model1, result);
    }

    [Fact]
    public void OperatorPlus_ShouldHandleMaxDecimalValues_WithoutOverflow()
    {
        // Arrange
        var model1 = new DailyWinLossModel
        {
            MemberWinLoss = decimal.MaxValue / 2,
            Promotion = decimal.MaxValue / 2
        };

        var model2 = new DailyWinLossModel
        {
            MemberWinLoss = decimal.MaxValue / 3,
            Promotion = decimal.MaxValue / 3
        };

        // Act
        var result = model1 + model2;

        // Assert
        Assert.Equal(decimal.MaxValue / 2 + decimal.MaxValue / 3, result.MemberWinLoss);
        Assert.Equal(decimal.MaxValue / 2 + decimal.MaxValue / 3, result.Promotion);
    }
}