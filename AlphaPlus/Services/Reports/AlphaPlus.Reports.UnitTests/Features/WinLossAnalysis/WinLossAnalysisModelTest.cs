using AlphaPlus.Reports.Features.Shared.Models;
using AlphaPlus.Reports.Features.WinLossAnalysis;

namespace AlphaPlus.Reports.UnitTests.Features.WinLossAnalysis;

[Trait("Category", "WinLossAnalysis")]
public class WinLossAnalysisModelTest
{
    [Fact]
    public void Addition_Operator_Should_Combine_Two_Models_Correctly()
    {
        // Arrange
        var model1 = new WinLossAnalysisModel
        {
            RollupKeyId = 1,
            RollupKey = "Key1",
            BetCount = 10,
            Turnover = 1000m,
            MemberWinLoss = -200m,
            MemberComm = 50m,
        };

        var model2 = new WinLossAnalysisModel
        {
            RollupKeyId = 2,
            RollupKey = "Key2",
            BetCount = 20,
            Turnover = 2000m,
            MemberWinLoss = 300m,
            MemberComm = 100m,
        };

        // Act
        var result = model1 + model2;

        // Assert
        Assert.Equal(30, result.BetCount);
        Assert.Equal(3000m, result.Turnover);
        Assert.Equal(100m, result.MemberWinLoss);
        Assert.Equal(150m, result.MemberComm);
        Assert.Equal(250m, result.MemberTotal);
    }

    [Fact]
    public void Model_Properties_Should_Be_Set_Correctly()
    {
        // Arrange
        var model = new WinLossAnalysisModel
        {
            RollupKeyId = 123,
            RollupKey = "TestKey",
            BetCount = 50,
            Turnover = 5000m,
            AgWinLoss = -500m,
            AgComm = 100m
        };

        // Assert
        Assert.Equal(123, model.RollupKeyId);
        Assert.Equal("TestKey", model.RollupKey);
        Assert.Equal(50, model.BetCount);
        Assert.Equal(5000m, model.Turnover);
        Assert.Equal(-500m, model.AgWinLoss);
        Assert.Equal(100m, model.AgComm);
        Assert.Equal(-400m, model.AgTotal);
    }

    [Fact]
    public void Model_Should_Inherit_From_NetWinLoss()
    {
        // Arrange
        var model = new WinLossAnalysisModel();

        // Assert
        Assert.IsAssignableFrom<NetWinLoss>(model);
    }
}