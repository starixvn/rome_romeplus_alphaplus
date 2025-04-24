using AlphaPlus.Reports.Features.WinLossByProduct;

namespace AlphaPlus.Reports.UnitTests.Features.WinLossByProduct;

[Trait("Category", "WinLossByProduct")]
public class WinLossByBetTypeModelTest
{
    [Fact]
    public void MemberTotal_Should_Return_Sum_Of_MemberWinLoss_And_MemberComm()
    {
        // Arrange
        var model = new WinLossByBetTypeModel
        {
            MemberWinLoss = 100,
            MemberComm = 50
        };

        // Act
        var result = model.MemberTotal;

        // Assert
        Assert.Equal(150, result);
    }

    [Fact]
    public void Total_Should_Return_Sum_Of_WinLoss_And_Comm()
    {
        // Arrange
        var model = new WinLossByBetTypeModel
        {
            WinLoss = 200,
            Comm = 30
        };

        // Act
        var result = model.Total;

        // Assert
        Assert.Equal(230, result);
    }

    [Fact]
    public void Company_Should_Return_Negative_Sum_Of_All_Totals()
    {
        // Arrange
        var model = new WinLossByBetTypeModel
        {
            MemberWinLoss = 100,
            MemberComm = 50,
            DownlineTotal = 200,
            WinLoss = 300,
            Comm = 50
        };

        // Act
        var result = model.Company;

        // Assert
        Assert.Equal(-700, result);
    }

    [Fact]
    public void Addition_Operator_Should_Add_All_Numeric_Properties()
    {
        // Arrange
        var model1 = new WinLossByBetTypeModel
        {
            NumberOfTickets = 10,
            Turnover = 1000,
            GrossComm = 100,
            MemberWinLoss = 200,
            MemberComm = 50,
            DownlineTotal = 300,
            WinLoss = 400,
            Comm = 80
        };

        var model2 = new WinLossByBetTypeModel
        {
            NumberOfTickets = 5,
            Turnover = 500,
            GrossComm = 50,
            MemberWinLoss = 100,
            MemberComm = 25,
            DownlineTotal = 150,
            WinLoss = 200,
            Comm = 40
        };

        // Act
        var result = model1 + model2;

        // Assert
        Assert.Equal(15, result.NumberOfTickets);
        Assert.Equal(1500m, result.Turnover);
        Assert.Equal(150m, result.GrossComm);
        Assert.Equal(300m, result.MemberWinLoss);
        Assert.Equal(75m, result.MemberComm);
        Assert.Equal(375m, result.MemberTotal);
        Assert.Equal(450m, result.DownlineTotal);
        Assert.Equal(600m, result.WinLoss);
        Assert.Equal(120m, result.Comm);
        Assert.Equal(720m, result.Total);
        Assert.Equal(-1545m, result.Company);
    }

    [Fact]
    public void Addition_Operator_Should_Return_First_Model_With_Updated_Values()
    {
        // Arrange
        var model1 = new WinLossByBetTypeModel
        {
            BetType = 1,
            BetTypeName = "Type1",
            ProductId = 100,
            SportType = 10,
            SportTypeName = "Sport1",
            LiveIndicator = true,
            IsMMR = false
        };

        var model2 = new WinLossByBetTypeModel
        {
            BetType = 2,
            BetTypeName = "Type2",
            ProductId = 200,
            SportType = 20,
            SportTypeName = "Sport2",
            LiveIndicator = false,
            IsMMR = true,
            NumberOfTickets = 5,
            Turnover = 500
        };

        // Act
        var result = model1 + model2;

        // Assert
        // Non-numerical properties should remain from model1
        Assert.Equal(1, result.BetType);
        Assert.Equal("Type1", result.BetTypeName);
        Assert.Equal(100, result.ProductId);
        Assert.Equal(10, result.SportType);
        Assert.Equal("Sport1", result.SportTypeName);
        Assert.True(result.LiveIndicator);
        Assert.False(result.IsMMR);

        // Numerical properties should be added
        Assert.Equal(5, result.NumberOfTickets);
        Assert.Equal(500m, result.Turnover);
    }
}