using AlphaPlus.Reports.Features.Shared.Models;

namespace AlphaPlus.Reports.UnitTests.Features.SuperWinLossDetailByPT;

[Trait("Category", "SuperWinLossDetailByPT")]
public class SuperWinLossDetailByPTEntityTest
{
    [Fact]
    public void SuperWinLossDetailByPTEntity_Should_Initialize_Properties_Correctly()
    {
        // Arrange & Act
        var entity = new Reports.Features.SuperWinLossDetailByPT.SuperWinLossDetailByPTEntity
        {
            SuperPT = 10.5m,
            ProductId = 123,
            ProductName = "TestProduct",
            NetTurnover = 1000m,
            GrossComm = 100m
        };

        // Assert
        Assert.Equal(10.5m, entity.SuperPT);
        Assert.Equal(123, entity.ProductId);
        Assert.Equal("TestProduct", entity.ProductName);
        Assert.Equal(1000m, entity.NetTurnover);
        Assert.Equal(100m, entity.GrossComm);
    }

    [Fact]
    public void Addition_Operator_Should_Combine_Two_Entities_Correctly()
    {
        // Arrange
        var entity1 = new Reports.Features.SuperWinLossDetailByPT.SuperWinLossDetailByPTEntity
        {
            SuperPT = 5m,
            ProductId = 123,
            ProductName = "TestProduct1",
            NetTurnover = 1000m,
            GrossComm = 100m,
            SAgWinLoss = 500m,
            SAgComm = 200m
        };

        var entity2 = new Reports.Features.SuperWinLossDetailByPT.SuperWinLossDetailByPTEntity
        {
            SuperPT = 7m,
            ProductId = 456,
            ProductName = "TestProduct2",
            NetTurnover = 2000m,
            GrossComm = 200m,
            SAgWinLoss = 800m,
            SAgComm = 300m
        };

        // Act
        var result = entity1 + entity2;

        // Assert
        Assert.Equal(5m, result.SuperPT); // Should keep first entity's SuperPT
        Assert.Equal(123, result.ProductId); // Should keep first entity's ProductId
        Assert.Equal("TestProduct1", result.ProductName); // Should keep first entity's ProductName
        Assert.Equal(3000m, result.NetTurnover); // 1000 + 2000
        Assert.Equal(300m, result.GrossComm); // 100 + 200
        Assert.Equal(1300m, result.SAgWinLoss); // 500 + 800
        Assert.Equal(500m, result.SAgComm); // 200 + 300
    }

    [Fact]
    public void SuperWinLossDetailByPTEntity_Should_Inherit_From_NetWinLoss()
    {
        // Arrange & Act
        var entity = new Reports.Features.SuperWinLossDetailByPT.SuperWinLossDetailByPTEntity();

        // Assert
        Assert.IsAssignableFrom<NetWinLoss>(entity);
    }

    [Fact]
    public void Addition_Operator_Should_Properly_Call_Base_Add_Method()
    {
        // Arrange
        var entity1 = new Reports.Features.SuperWinLossDetailByPT.SuperWinLossDetailByPTEntity
        {
            NetTurnover = 100m,
            GrossComm = 50m,
            Turnover = 10m,
            AgWinLoss = 5m
        };

        var entity2 = new Reports.Features.SuperWinLossDetailByPT.SuperWinLossDetailByPTEntity
        {
            NetTurnover = 200m,
            GrossComm = 100m,
            Turnover = 20m,
            AgWinLoss = 10m
        };

        // Act
        var result = entity1 + entity2;

        // Assert
        Assert.Equal(300m, result.NetTurnover);
        Assert.Equal(150m, result.GrossComm);
        Assert.Equal(30m, result.Turnover);
        Assert.Equal(15m, result.AgWinLoss);
    }
}