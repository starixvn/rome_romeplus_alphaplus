using AlphaPlus.Settings.MemberInfo.Domain.Features.DormantAccountList.Models;

namespace AlphaPlus.Settings.MemberInfo.Domain.UnitTests.Features.DormantAccountList.Models;

public class YesterdayBalanceEntityTests
{
    [Fact]
    public void Default_Constructor_Should_Initialize_With_Default_Values()
    {
        // Act
        var entity = new YesterdayBalanceEntity();

        // Assert
        Assert.Equal(0, entity.CustId);
        Assert.Equal(0, entity.YesterdayTotalBalance);
    }

    [Fact]
    public void Parameterized_Constructor_Should_Set_Properties_Correctly()
    {
        // Arrange
        var custId = 123;
        var yesterdayTotalBalance = 456.78m;

        // Act
        var entity = new YesterdayBalanceEntity(custId, yesterdayTotalBalance);

        // Assert
        Assert.Equal(custId, entity.CustId);
        Assert.Equal(yesterdayTotalBalance, entity.YesterdayTotalBalance);
    }

    [Fact]
    public void CustId_Property_Should_Be_Settable()
    {
        // Arrange
        var entity = new YesterdayBalanceEntity();
        var custId = 789;

        // Act
        entity.CustId = custId;

        // Assert
        Assert.Equal(custId, entity.CustId);
    }

    [Fact]
    public void YesterdayTotalBalance_Property_Should_Be_Settable()
    {
        // Arrange
        var entity = new YesterdayBalanceEntity();
        var balance = 999.99m;

        // Act
        entity.YesterdayTotalBalance = balance;

        // Assert
        Assert.Equal(balance, entity.YesterdayTotalBalance);
    }

    [Theory]
    [InlineData(0, 0)]
    [InlineData(1, 100.50)]
    [InlineData(-1, -200.75)]
    [InlineData(int.MaxValue, 999999.99)]
    [InlineData(int.MinValue, -999999.99)]
    public void Constructor_Should_Handle_Various_Values(int custId, decimal yesterdayTotalBalance)
    {
        // Act
        var entity = new YesterdayBalanceEntity(custId, yesterdayTotalBalance);

        // Assert
        Assert.Equal(custId, entity.CustId);
        Assert.Equal(yesterdayTotalBalance, entity.YesterdayTotalBalance);
    }

    [Fact]
    public void Properties_Should_Be_Independent()
    {
        // Arrange
        var entity = new YesterdayBalanceEntity();

        // Act
        entity.CustId = 123;
        entity.YesterdayTotalBalance = 456.78m;

        // Assert
        Assert.Equal(123, entity.CustId);
        Assert.Equal(456.78m, entity.YesterdayTotalBalance);
    }

    [Fact]
    public void Constructor_Should_Not_Modify_Input_Values()
    {
        // Arrange
        var originalCustId = 123;
        var originalBalance = 456.78m;
        var custId = originalCustId;
        var balance = originalBalance;

        // Act
        var entity = new YesterdayBalanceEntity(custId, balance);

        // Assert
        Assert.Equal(originalCustId, custId);
        Assert.Equal(originalBalance, balance);
        Assert.Equal(originalCustId, entity.CustId);
        Assert.Equal(originalBalance, entity.YesterdayTotalBalance);
    }

    [Fact]
    public void Multiple_Instances_Should_Be_Independent()
    {
        // Arrange & Act
        var entity1 = new YesterdayBalanceEntity(1, 100m);
        var entity2 = new YesterdayBalanceEntity(2, 200m);

        entity1.CustId = 10;
        entity1.YesterdayTotalBalance = 1000m;

        // Assert
        Assert.Equal(10, entity1.CustId);
        Assert.Equal(1000m, entity1.YesterdayTotalBalance);
        Assert.Equal(2, entity2.CustId);
        Assert.Equal(200m, entity2.YesterdayTotalBalance);
    }

    [Fact]
    public void Properties_Should_Have_Getters_And_Setters()
    {
        // Arrange
        var type = typeof(YesterdayBalanceEntity);

        // Act & Assert
        var custIdProperty = type.GetProperty(nameof(YesterdayBalanceEntity.CustId));
        Assert.NotNull(custIdProperty);
        Assert.True(custIdProperty.CanRead);
        Assert.True(custIdProperty.CanWrite);
        Assert.Equal(typeof(int), custIdProperty.PropertyType);

        var balanceProperty = type.GetProperty(nameof(YesterdayBalanceEntity.YesterdayTotalBalance));
        Assert.NotNull(balanceProperty);
        Assert.True(balanceProperty.CanRead);
        Assert.True(balanceProperty.CanWrite);
        Assert.Equal(typeof(decimal), balanceProperty.PropertyType);
    }

    [Fact]
    public void Constructor_With_Zero_Values_Should_Work()
    {
        // Act
        var entity = new YesterdayBalanceEntity(0, 0m);

        // Assert
        Assert.Equal(0, entity.CustId);
        Assert.Equal(0m, entity.YesterdayTotalBalance);
    }

    [Fact]
    public void Constructor_With_Negative_CustId_Should_Work()
    {
        // Act
        var entity = new YesterdayBalanceEntity(-1, 100m);

        // Assert
        Assert.Equal(-1, entity.CustId);
        Assert.Equal(100m, entity.YesterdayTotalBalance);
    }

    [Fact]
    public void Constructor_With_Negative_Balance_Should_Work()
    {
        // Act
        var entity = new YesterdayBalanceEntity(123, -500.50m);

        // Assert
        Assert.Equal(123, entity.CustId);
        Assert.Equal(-500.50m, entity.YesterdayTotalBalance);
    }

    [Fact]
    public void Properties_Should_Handle_Large_Values()
    {
        // Arrange
        var entity = new YesterdayBalanceEntity();

        // Act
        entity.CustId = int.MaxValue;
        entity.YesterdayTotalBalance = decimal.MaxValue;

        // Assert
        Assert.Equal(int.MaxValue, entity.CustId);
        Assert.Equal(decimal.MaxValue, entity.YesterdayTotalBalance);
    }

    [Fact]
    public void Properties_Should_Handle_Minimum_Values()
    {
        // Arrange
        var entity = new YesterdayBalanceEntity();

        // Act
        entity.CustId = int.MinValue;
        entity.YesterdayTotalBalance = decimal.MinValue;

        // Assert
        Assert.Equal(int.MinValue, entity.CustId);
        Assert.Equal(decimal.MinValue, entity.YesterdayTotalBalance);
    }
}
