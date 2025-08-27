using AlphaPlus.Settings.MemberInfo.Domain.Models.CreditBalance;
using AlphaPlus.Shared.AppResources.Settings.MemberInfo;

namespace AlphaPlus.Settings.MemberInfo.Domain.UnitTests.Models.CreditBalance;

public class CreditBalanceItemTests
{
    [Fact]
    public void CreditBalanceItem_Default_Constructor_Should_Initialize_With_Default_Values()
    {
        // Act
        var item = new CreditBalanceItem();

        // Assert
        Assert.Equal(0, item.RowNumber);
        Assert.False(item.Enabled);
        Assert.False(item.Closed);
        Assert.Equal(0, item.ClosedReason);
        Assert.False(item.Suspended);
        Assert.Equal(0, item.Locked);
        Assert.Equal(0, item.CustId);
        Assert.Null(item.UserName);
        Assert.Null(item.FirstName);
        Assert.Null(item.LastName);
        Assert.Equal(0, item.Credit);
        Assert.Equal(0, item.BetCredit);
        Assert.Equal(0, item.Balance);
        Assert.Equal(0, item.Yesterday_Balance);
        Assert.Equal(0, item.Outstanding);
        Assert.Equal(0, item.PendingTransfer3rd);
        Assert.Null(item.LastLogged);
        Assert.Null(item.CreatedDate);
        Assert.Null(item.IPLogin);
        Assert.False(item.IsDirectMember);
    }

    [Fact]
    public void CreditBalanceItem_Parameterized_Constructor_Should_Set_All_Properties()
    {
        // Arrange
        var rowNumber = 1;
        var enabled = true;
        var closed = false;
        var suspended = false;
        var custId = 123;
        var userName = "testuser";
        var firstName = "John";
        var lastName = "Doe";
        var credit = 1000.50m;
        var betCredit = 500.25m;
        var balance = 250.00m;
        var yesterdayBalance = 300.00m;
        var outstanding = 150.75m;
        var pendingTransfer3rd = 50.00m;
        var createdDate = new DateTime(2024, 1, 15);
        var lastLogged = new DateTime(2024, 3, 15, 10, 30, 0);
        var ipLogin = "192.168.1.100";
        var isDirectMember = true;

        // Act
        var item = new CreditBalanceItem(
            rowNumber,
            enabled,
            closed,
            suspended,
            custId,
            userName,
            firstName,
            lastName,
            credit,
            betCredit,
            balance,
            yesterdayBalance,
            outstanding,
            pendingTransfer3rd,
            createdDate,
            lastLogged,
            ipLogin,
            isDirectMember);

        // Assert
        Assert.Equal(rowNumber, item.RowNumber);
        Assert.Equal(enabled, item.Enabled);
        Assert.Equal(closed, item.Closed);
        Assert.Equal(suspended, item.Suspended);
        Assert.Equal(custId, item.CustId);
        Assert.Equal(userName, item.UserName);
        Assert.Equal(firstName, item.FirstName);
        Assert.Equal(lastName, item.LastName);
        Assert.Equal(credit, item.Credit);
        Assert.Equal(betCredit, item.BetCredit);
        Assert.Equal(balance, item.Balance);
        Assert.Equal(yesterdayBalance, item.Yesterday_Balance);
        Assert.Equal(outstanding, item.Outstanding);
        Assert.Equal(pendingTransfer3rd, item.PendingTransfer3rd);
        Assert.Equal(createdDate, item.CreatedDate);
        Assert.Equal(lastLogged, item.LastLogged);
        Assert.Equal(ipLogin, item.IPLogin);
        Assert.Equal(isDirectMember, item.IsDirectMember);
    }

    [Theory]
    [InlineData(true, false, false, "Open")]
    [InlineData(false, false, false, "Disabled")]
    [InlineData(true, true, false, "Closed")]
    [InlineData(true, false, true, "Suspended")]
    [InlineData(true, true, true, "Closed")] // Closed takes precedence over Suspended
    public void Status_Should_Return_Correct_Status_Based_On_Flags(bool enabled, bool closed, bool suspended, string expectedStatus)
    {
        // Arrange
        var item = new CreditBalanceItem
        {
            Enabled = enabled,
            Closed = closed,
            Suspended = suspended
        };

        // Act
        var status = item.Status;

        // Assert
        Assert.Equal(expectedStatus, status);
    }

    [Fact]
    public void Status_Should_Return_Open_When_All_Flags_Are_False()
    {
        // Arrange
        var item = new CreditBalanceItem
        {
            Enabled = true,
            Closed = false,
            Suspended = false
        };

        // Act
        var status = item.Status;

        // Assert
        Assert.Equal(Resources.Open, status);
    }

    [Fact]
    public void Status_Should_Return_Disabled_When_Enabled_Is_False()
    {
        // Arrange
        var item = new CreditBalanceItem
        {
            Enabled = false,
            Closed = true,
            Suspended = true
        };

        // Act
        var status = item.Status;

        // Assert
        Assert.Equal(Resources.Disabled, status);
    }

    [Fact]
    public void Status_Should_Return_Closed_When_Closed_Is_True_And_Enabled_Is_True()
    {
        // Arrange
        var item = new CreditBalanceItem
        {
            Enabled = true,
            Closed = true,
            Suspended = true
        };

        // Act
        var status = item.Status;

        // Assert
        Assert.Equal(Resources.Closed, status);
    }

    [Fact]
    public void Status_Should_Return_Suspended_When_Suspended_Is_True_And_Closed_Is_False()
    {
        // Arrange
        var item = new CreditBalanceItem
        {
            Enabled = true,
            Closed = false,
            Suspended = true
        };

        // Act
        var status = item.Status;

        // Assert
        Assert.Equal(Resources.Suspended, status);
    }
}

// Mock Resources class for testing
internal static class Resources
{
    public const string Disabled = "Disabled";
    public const string Closed = "Closed";
    public const string Suspended = "Suspended";
    public const string Open = "Open";
}
