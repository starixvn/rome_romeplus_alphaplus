using AlphaPlus.Settings.MemberInfo.Domain.Models;

namespace AlphaPlus.Settings.MemberInfo.Domain.UnitTests.Models;

public class CustomerTransferStatusTests
{
    [Fact]
    public void CustomerTransferStatus_Default_Constructor_Should_Initialize_With_Default_Values()
    {
        // Act
        var status = new CustomerTransferStatus();

        // Assert
        Assert.Equal(0, status.CustId);
        Assert.False(status.IsAllDayBalTransfer);
    }

    [Fact]
    public void CustomerTransferStatus_Parameterized_Constructor_Should_Set_Properties()
    {
        // Arrange
        var custId = 123;
        var isAllDayBalTransfer = true;

        // Act
        var status = new CustomerTransferStatus(custId, isAllDayBalTransfer);

        // Assert
        Assert.Equal(custId, status.CustId);
        Assert.Equal(isAllDayBalTransfer, status.IsAllDayBalTransfer);
    }

    [Fact]
    public void CustomerTransferStatus_Should_Allow_Property_Modification()
    {
        // Arrange
        var status = new CustomerTransferStatus();

        // Act
        status.CustId = 456;
        status.IsAllDayBalTransfer = true;

        // Assert
        Assert.Equal(456, status.CustId);
        Assert.True(status.IsAllDayBalTransfer);
    }

    [Theory]
    [InlineData(0, false)]
    [InlineData(1, true)]
    [InlineData(999, false)]
    [InlineData(-1, true)]
    public void CustomerTransferStatus_Should_Handle_Various_Values(int custId, bool isAllDayBalTransfer)
    {
        // Act
        var status = new CustomerTransferStatus(custId, isAllDayBalTransfer);

        // Assert
        Assert.Equal(custId, status.CustId);
        Assert.Equal(isAllDayBalTransfer, status.IsAllDayBalTransfer);
    }
}
