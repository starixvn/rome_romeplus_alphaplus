using AlphaPlus.Settings.MemberInfo.Domain.Features.Commission;

namespace AlphaPlus.Settings.MemberInfo.Domain.UnitTests.Features.Commission;

public class CommissionEntityTests
{
    [Fact]
    public void CommissionEntity_Should_Initialize_With_Default_Values()
    {
        // Act
        var entity = new CommissionEntity();

        // Assert
        Assert.False(entity.Closed);
        Assert.False(entity.Enabled);
        Assert.False(entity.Suspended);
        Assert.Null(entity.Username);
        Assert.Equal(0, entity.CustId);
        Assert.Null(entity.UGroup);
        Assert.Null(entity.FirstName);
        Assert.Null(entity.LastName);
        Assert.Equal(0, entity.Credit);
        Assert.False(entity.DoubleCommLosing);
        Assert.Equal(0, entity.ADiscount);
        Assert.Equal(0, entity.Discount);
        Assert.Equal(0, entity.ADiscount1X2);
        Assert.Equal(0, entity.Discount1X2);
        Assert.Equal(0, entity.PlayerDiscount1X2);
        Assert.Equal(0, entity.ADiscountCS);
        Assert.Equal(0, entity.DiscountCS);
        Assert.Equal(0, entity.PlayerDiscountCS);
        Assert.Equal(0, entity.SportType);
        Assert.Equal(0, entity.MasterDiscount);
        Assert.Equal(0, entity.AgentDiscount);
        Assert.Equal(0, entity.PlayerDiscount);
        Assert.Equal(0, entity.Locked);
        Assert.Equal(0, entity.ClosedReason);
        Assert.Equal(0, entity.ServiceType);
        Assert.False(entity.IsCashMember);
    }

    [Theory]
    [InlineData(0, false)]
    [InlineData(1, true)]
    [InlineData(2, false)]
    [InlineData(-1, false)]
    public void IsCashMember_Should_Return_True_Only_When_ServiceType_Is_1(int serviceType, bool expected)
    {
        // Act
        var entity = new CommissionEntity { ServiceType = serviceType };

        // Assert
        Assert.Equal(expected, entity.IsCashMember);
    }

    [Fact]
    public void CommissionEntity_Should_Set_All_Init_Properties()
    {
        // Arrange
        var entity = new CommissionEntity
        {
            Closed = true,
            Enabled = true,
            Suspended = false,
            Username = "testuser",
            CustId = 123,
            UGroup = "TestGroup",
            FirstName = "John",
            LastName = "Doe",
            Credit = 1000.50m,
            DoubleCommLosing = true,
            ADiscount = 5.5m,
            Discount = 10.0m,
            ADiscount1X2 = 2.5m,
            Discount1X2 = 5.0m,
            PlayerDiscount1X2 = 1.0m,
            ADiscountCS = 3.0m,
            DiscountCS = 6.0m,
            PlayerDiscountCS = 2.0m,
            SportType = 1,
            MasterDiscount = 8.0m,
            AgentDiscount = 4.0m,
            PlayerDiscount = 2.0m,
            Locked = 1,
            ClosedReason = 2,
            ServiceType = 1
        };

        // Assert
        Assert.True(entity.Closed);
        Assert.True(entity.Enabled);
        Assert.False(entity.Suspended);
        Assert.Equal("testuser", entity.Username);
        Assert.Equal(123, entity.CustId);
        Assert.Equal("TestGroup", entity.UGroup);
        Assert.Equal("John", entity.FirstName);
        Assert.Equal("Doe", entity.LastName);
        Assert.Equal(1000.50m, entity.Credit);
        Assert.True(entity.DoubleCommLosing);
        Assert.Equal(5.5m, entity.ADiscount);
        Assert.Equal(10.0m, entity.Discount);
        Assert.Equal(2.5m, entity.ADiscount1X2);
        Assert.Equal(5.0m, entity.Discount1X2);
        Assert.Equal(1.0m, entity.PlayerDiscount1X2);
        Assert.Equal(3.0m, entity.ADiscountCS);
        Assert.Equal(6.0m, entity.DiscountCS);
        Assert.Equal(2.0m, entity.PlayerDiscountCS);
        Assert.Equal(1, entity.SportType);
        Assert.Equal(8.0m, entity.MasterDiscount);
        Assert.Equal(4.0m, entity.AgentDiscount);
        Assert.Equal(2.0m, entity.PlayerDiscount);
        Assert.Equal(1, entity.Locked);
        Assert.Equal(2, entity.ClosedReason);
        Assert.Equal(1, entity.ServiceType);
        Assert.True(entity.IsCashMember);
    }
}
