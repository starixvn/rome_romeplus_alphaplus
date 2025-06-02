using AlphaPlus.Reports.Features.CommissionByBetType;

namespace AlphaPlus.Reports.UnitTests.Features.CommissionByBetType;

[Trait("Category", "CommissionByBetType")]
public class CommissionByBetTypeModelTest
{
    [Fact]
    public void Properties_ShouldSetAndGetCorrectly()
    {
        // Arrange
        var model = new CommissionByBetTypeModel
        {
            CustId = 123,
            CustName = "Test Customer",
            Comm1 = 10.5m,
            Comm2 = 20.75m,
            Comm3 = 30.25m,
            CasinoComm = 40.5m,
            IsDirectMember = true
        };

        // Assert
        Assert.Equal(123, model.CustId);
        Assert.Equal("Test Customer", model.CustName);
        Assert.Equal(10.5m, model.Comm1);
        Assert.Equal(20.75m, model.Comm2);
        Assert.Equal(30.25m, model.Comm3);
        Assert.Equal(40.5m, model.CasinoComm);
        Assert.True(model.IsDirectMember);
    }

    [Fact]
    public void AdditionOperator_ShouldAddCommissionValues()
    {
        // Arrange
        var model1 = new CommissionByBetTypeModel
        {
            CustId = 123,
            CustName = "Test Customer 1",
            Comm1 = 10.5m,
            Comm2 = 20.75m,
            Comm3 = 30.25m,
            CasinoComm = 40.5m,
            IsDirectMember = true
        };

        var model2 = new CommissionByBetTypeModel
        {
            CustId = 456,
            CustName = "Test Customer 2",
            Comm1 = 5.25m,
            Comm2 = 15.5m,
            Comm3 = 25.75m,
            CasinoComm = 35.25m,
            IsDirectMember = false
        };

        // Act
        var result = model1 + model2;

        // Assert
        Assert.Equal(123, result.CustId); // Keeps model1's CustId
        Assert.Equal("Test Customer 1", result.CustName); // Keeps model1's CustName
        Assert.Equal(15.75m, result.Comm1); // 10.5 + 5.25
        Assert.Equal(36.25m, result.Comm2); // 20.75 + 15.5
        Assert.Equal(56.0m, result.Comm3); // 30.25 + 25.75
        Assert.Equal(75.75m, result.CasinoComm); // 40.5 + 35.25
        Assert.True(result.IsDirectMember); // Keeps model1's IsDirectMember
    }

    [Fact]
    public void AdditionOperator_ShouldWorkWithZeroValues()
    {
        // Arrange
        var model1 = new CommissionByBetTypeModel
        {
            CustId = 123,
            CustName = "Test Customer 1",
            Comm1 = 10.5m,
            Comm2 = 20.75m,
            Comm3 = 30.25m,
            CasinoComm = 40.5m
        };

        var model2 = new CommissionByBetTypeModel
        {
            CustId = 456,
            CustName = "Test Customer 2",
            Comm1 = 0m,
            Comm2 = 0m,
            Comm3 = 0m,
            CasinoComm = 0m
        };

        // Act
        var result = model1 + model2;

        // Assert
        Assert.Equal(10.5m, result.Comm1);
        Assert.Equal(20.75m, result.Comm2);
        Assert.Equal(30.25m, result.Comm3);
        Assert.Equal(40.5m, result.CasinoComm);
    }

    [Fact]
    public void AdditionOperator_ShouldHandleNegativeValues()
    {
        // Arrange
        var model1 = new CommissionByBetTypeModel
        {
            CustId = 123,
            CustName = "Test Customer 1",
            Comm1 = 10.5m,
            Comm2 = 20.75m,
            Comm3 = -30.25m,
            CasinoComm = 40.5m
        };

        var model2 = new CommissionByBetTypeModel
        {
            CustId = 456,
            CustName = "Test Customer 2",
            Comm1 = -5.25m,
            Comm2 = 15.5m,
            Comm3 = 25.75m,
            CasinoComm = -35.25m
        };

        // Act
        var result = model1 + model2;

        // Assert
        Assert.Equal(5.25m, result.Comm1); // 10.5 + (-5.25)
        Assert.Equal(36.25m, result.Comm2); // 20.75 + 15.5
        Assert.Equal(-4.5m, result.Comm3); // -30.25 + 25.75
        Assert.Equal(5.25m, result.CasinoComm); // 40.5 + (-35.25)
    }
}