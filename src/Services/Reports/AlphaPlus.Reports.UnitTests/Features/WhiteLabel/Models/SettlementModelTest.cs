using AlphaPlus.Reports.Features.WhiteLabel;

namespace AlphaPlus.Reports.UnitTests.Features.WhiteLabel;

[Trait("Category", "WhiteLabel")]
public class SettlementModelTest
{
    [Fact]
    public void PlusOperator_ShouldAddAllNumericProperties()
    {
        // Arrange
        var settlement1 = new SettlementModel
        {
            Month = "January",
            WhiteLabel = "Label1",
            Agent = "Agent1",
            Master = "Master1",
            MemberWL = 100.50m,
            MemberPromotion = 50.75m,
            LossCarryforward = 25.25m,
            WhiteLabelWinLoss = 200.00m,
            WhiteLabelNetWinLoss = 180.50m,
            WhiteLabelFee = 30.00m,
            WhiteLabelPromotionSettlement = 15.50m,
            WhiteLabelPromotionShare = 5.25m,
            WhiteLabelTotal = 231.25m,
            MasterEarning = 45.50m,
            SuperEarning = 22.75m,
            CompanyIncome = 120.00m,
            CompanyPromotionShare = 10.25m,
            CompanyTotal = 130.25m
        };

        var settlement2 = new SettlementModel
        {
            Month = "February",
            WhiteLabel = "Label2",
            Agent = "Agent2",
            Master = "Master2",
            MemberWL = 200.50m,
            MemberPromotion = 75.25m,
            LossCarryforward = 10.75m,
            WhiteLabelWinLoss = 300.00m,
            WhiteLabelNetWinLoss = 280.00m,
            WhiteLabelFee = 40.50m,
            WhiteLabelPromotionSettlement = 22.50m,
            WhiteLabelPromotionShare = 7.75m,
            WhiteLabelTotal = 350.75m,
            MasterEarning = 65.25m,
            SuperEarning = 32.50m,
            CompanyIncome = 180.00m,
            CompanyPromotionShare = 15.50m,
            CompanyTotal = 195.50m
        };

        // Act
        var result = settlement1 + settlement2;

        // Assert
        Assert.Equal("January", result.Month);
        Assert.Equal("Label1", result.WhiteLabel);
        Assert.Equal("Agent1", result.Agent);
        Assert.Equal("Master1", result.Master);
        Assert.Equal(301.00m, result.MemberWL);
        Assert.Equal(126.00m, result.MemberPromotion);
        Assert.Equal(36.00m, result.LossCarryforward);
        Assert.Equal(500.00m, result.WhiteLabelWinLoss);
        Assert.Equal(460.50m, result.WhiteLabelNetWinLoss);
        Assert.Equal(70.50m, result.WhiteLabelFee);
        Assert.Equal(38.00m, result.WhiteLabelPromotionSettlement);
        Assert.Equal(13.00m, result.WhiteLabelPromotionShare);
        Assert.Equal(582.00m, result.WhiteLabelTotal);
        Assert.Equal(110.75m, result.MasterEarning);
        Assert.Equal(55.25m, result.SuperEarning);
        Assert.Equal(300.00m, result.CompanyIncome);
        Assert.Equal(25.75m, result.CompanyPromotionShare);
        Assert.Equal(325.75m, result.CompanyTotal);
    }

    [Fact]
    public void PlusOperator_WithZeroValues_ShouldNotChangeOriginalValues()
    {
        // Arrange
        var settlement1 = new SettlementModel
        {
            Month = "March",
            WhiteLabel = "Label3",
            Agent = "Agent3",
            Master = "Master3",
            MemberWL = 300.00m,
            MemberPromotion = 45.50m,
            LossCarryforward = 15.75m,
            WhiteLabelWinLoss = 250.00m,
            WhiteLabelNetWinLoss = 234.25m,
            WhiteLabelFee = 35.75m,
            WhiteLabelPromotionSettlement = 12.50m,
            WhiteLabelPromotionShare = 4.25m,
            WhiteLabelTotal = 286.75m,
            MasterEarning = 55.25m,
            SuperEarning = 27.50m,
            CompanyIncome = 150.00m,
            CompanyPromotionShare = 8.25m,
            CompanyTotal = 158.25m
        };

        var emptySettlement = new SettlementModel
        {
            Month = "Empty",
            WhiteLabel = "Empty",
            Agent = "Empty",
            Master = "Empty",
            MemberWL = 0m,
            MemberPromotion = 0m,
            LossCarryforward = 0m,
            WhiteLabelWinLoss = 0m,
            WhiteLabelNetWinLoss = 0m,
            WhiteLabelFee = 0m,
            WhiteLabelPromotionSettlement = 0m,
            WhiteLabelPromotionShare = 0m,
            WhiteLabelTotal = 0m,
            MasterEarning = 0m,
            SuperEarning = 0m,
            CompanyIncome = 0m,
            CompanyPromotionShare = 0m,
            CompanyTotal = 0m
        };

        // Act
        var result = settlement1 + emptySettlement;

        // Assert
        Assert.Equal(300.00m, result.MemberWL);
        Assert.Equal(45.50m, result.MemberPromotion);
        Assert.Equal(15.75m, result.LossCarryforward);
        Assert.Equal(250.00m, result.WhiteLabelWinLoss);
        Assert.Equal(234.25m, result.WhiteLabelNetWinLoss);
        Assert.Equal(35.75m, result.WhiteLabelFee);
        Assert.Equal(12.50m, result.WhiteLabelPromotionSettlement);
        Assert.Equal(4.25m, result.WhiteLabelPromotionShare);
        Assert.Equal(286.75m, result.WhiteLabelTotal);
        Assert.Equal(55.25m, result.MasterEarning);
        Assert.Equal(27.50m, result.SuperEarning);
        Assert.Equal(150.00m, result.CompanyIncome);
        Assert.Equal(8.25m, result.CompanyPromotionShare);
        Assert.Equal(158.25m, result.CompanyTotal);
    }

    [Fact]
    public void PlusOperator_WithNegativeValues_ShouldCalculateCorrectly()
    {
        // Arrange
        var settlement1 = new SettlementModel
        {
            Month = "April",
            MemberWL = 100.00m,
            MemberPromotion = 50.00m,
            LossCarryforward = 25.00m,
            WhiteLabelWinLoss = 200.00m,
            WhiteLabelNetWinLoss = 175.00m,
            WhiteLabelFee = 30.00m,
            WhiteLabelPromotionSettlement = 15.00m,
            WhiteLabelPromotionShare = 5.00m,
            WhiteLabelTotal = 225.00m,
            MasterEarning = 45.00m,
            SuperEarning = 22.50m,
            CompanyIncome = 120.00m,
            CompanyPromotionShare = 10.00m,
            CompanyTotal = 130.00m
        };

        var settlement2 = new SettlementModel
        {
            Month = "May",
            MemberWL = -50.00m,
            MemberPromotion = -25.00m,
            LossCarryforward = -10.00m,
            WhiteLabelWinLoss = -100.00m,
            WhiteLabelNetWinLoss = -90.00m,
            WhiteLabelFee = -15.00m,
            WhiteLabelPromotionSettlement = -7.50m,
            WhiteLabelPromotionShare = -2.50m,
            WhiteLabelTotal = -115.00m,
            MasterEarning = -22.50m,
            SuperEarning = -11.25m,
            CompanyIncome = -60.00m,
            CompanyPromotionShare = -5.00m,
            CompanyTotal = -65.00m
        };

        // Act
        var result = settlement1 + settlement2;

        // Assert
        Assert.Equal(50.00m, result.MemberWL);
        Assert.Equal(25.00m, result.MemberPromotion);
        Assert.Equal(15.00m, result.LossCarryforward);
        Assert.Equal(100.00m, result.WhiteLabelWinLoss);
        Assert.Equal(85.00m, result.WhiteLabelNetWinLoss);
        Assert.Equal(15.00m, result.WhiteLabelFee);
        Assert.Equal(7.50m, result.WhiteLabelPromotionSettlement);
        Assert.Equal(2.50m, result.WhiteLabelPromotionShare);
        Assert.Equal(110.00m, result.WhiteLabelTotal);
        Assert.Equal(22.50m, result.MasterEarning);
        Assert.Equal(11.25m, result.SuperEarning);
        Assert.Equal(60.00m, result.CompanyIncome);
        Assert.Equal(5.00m, result.CompanyPromotionShare);
        Assert.Equal(65.00m, result.CompanyTotal);
    }

    [Fact]
    public void PlusOperator_ChainedAddition_ShouldCalculateCorrectly()
    {
        // Arrange
        var settlement1 = new SettlementModel
        {
            MemberWL = 100.00m,
            MemberPromotion = 50.00m,
            WhiteLabelWinLoss = 200.00m,
            CompanyTotal = 100.00m
        };

        var settlement2 = new SettlementModel
        {
            MemberWL = 200.00m,
            MemberPromotion = 75.00m,
            WhiteLabelWinLoss = 300.00m,
            CompanyTotal = 150.00m
        };

        var settlement3 = new SettlementModel
        {
            MemberWL = 150.00m,
            MemberPromotion = 25.00m,
            WhiteLabelWinLoss = 250.00m,
            CompanyTotal = 125.00m
        };

        // Act
        var result = settlement1 + settlement2 + settlement3;

        // Assert
        Assert.Equal(450.00m, result.MemberWL);
        Assert.Equal(150.00m, result.MemberPromotion);
        Assert.Equal(750.00m, result.WhiteLabelWinLoss);
        Assert.Equal(375.00m, result.CompanyTotal);
    }
}