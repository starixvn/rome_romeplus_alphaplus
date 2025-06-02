using AlphaPlus.BetLists.Shared.Models;
using AlphaPlus.Shared.AppResources.Betlist.Services;

namespace AlphaPlus.BetLists.UnitTests.Shared.Models;

[Trait("Category", "BetLists")]
public class TicketDtoTest
{
    [Fact]
    public void MemberWin_WhenExtraInfoHasAttributeInXmlTagAndIsRunningWaiting_ReturnsCorrectCalculation()
    {
        // Arrange
        var ticket = new TicketDto
        {
            ExtraInfo = "<p1 pa=\"200\" /><other>some data</other>",
            ActualStake = 100
        };

        // Mock IsRunningWaiting to return true by setting appropriate status property
        // Since we can't directly mock the extension method, we need to ensure the object state
        // will cause it to return true when called
        ticket.Status = "running";

        // Act
        var result = ticket.MemberWin;

        // Assert
        Assert.Equal(100m, result); // 200 (payout) - 100 (stake) = 100
    }

    [Fact]
    public void MemberWin_WhenExtraInfoDoesntHaveRequiredAttribute_ReturnsNull()
    {
        // Arrange
        var ticket = new TicketDto
        {
            ExtraInfo = "<other>some data</other>",
            ActualStake = 100
        };
        ticket.Status = "running";

        // Act
        var result = ticket.MemberWin;

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public void MemberWin_WhenExtraInfoIsNull_ReturnsNull()
    {
        // Arrange
        var ticket = new TicketDto
        {
            ExtraInfo = null,
            ActualStake = 100
        };
        ticket.Status = "running";

        // Act
        var result = ticket.MemberWin;

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public void MemberWin_WhenExtraInfoIsEmptyString_ReturnsNull()
    {
        // Arrange
        var ticket = new TicketDto
        {
            ExtraInfo = string.Empty,
            ActualStake = 100
        };
        ticket.Status = "running";

        // Act
        var result = ticket.MemberWin;

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public void MemberWin_WhenIsRunningWaitingReturnsFalse_ReturnsNull()
    {
        // Arrange
        var ticket = new TicketDto
        {
            ExtraInfo = "<p1 pa=\"200\" /><other>some data</other>",
            ActualStake = 100
        };
        ticket.Status = "void";

        // Act
        var result = ticket.MemberWin;

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public void MemberWinText_WhenMemberWinHasValue_ReturnsFormattedWinloss()
    {
        // Arrange
        var ticket = new TicketDto
        {
            ExtraInfo = "<p1 pa=\"200\" /><other>some data</other>",
            ActualStake = 100
        };
        ticket.Status = "running"; // Ensure IsRunningWaiting returns true

        // Act
        var result = ticket.MemberWinText;

        // Assert
        // The formatted result depends on ConvertByBetType.Winloss implementation
        // This assumes ConvertByBetType.Winloss(100) formats it as expected
        // We're using the same value as the previous test to ensure consistency
        Assert.Equal(Fanex.BetList.Core.Utils.ConvertByBetType.Winloss(100), result);
    }

    [Fact]
    public void MemberWinText_WhenMemberWinIsNull_ReturnsNA()
    {
        // Arrange
        var ticket = new TicketDto
        {
            ExtraInfo = null,
            ActualStake = 100
        };
        ticket.Status = "winlost"; // Ensure MemberWin returns null

        // Act
        var result = ticket.MemberWinText;

        // Assert
        Assert.Equal(Resources.Platform_NA, result);
    }
}
