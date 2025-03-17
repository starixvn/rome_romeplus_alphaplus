using AlphaPlus.Reports.Features.BetChoiceMatching;

namespace AlphaPlus.Reports.UnitTests.Features.BetChoiceMatching;

[Trait("Category", "BetChoiceMatching")]
public class BetChoiceMatchingUserModelTest
{
    [Fact]
    public void Constructor_ShouldInitializePropertiesCorrectly()
    {
        // Arrange
        var tickets = new List<BetChoiceMatchingTicketModel>
        {
            new BetChoiceMatchingTicketModel
            {
                CustId = 1,
                Username = "user1",
                Stake = 100,
                MemberWinLoss = 50,
                SuperWinLoss = 20,
                Company = 30,
                IsRunning = false
            },
            new BetChoiceMatchingTicketModel
            {
                CustId = 1,
                Username = "user1",
                Stake = 200,
                MemberWinLoss = 100,
                SuperWinLoss = 40,
                Company = 60,
                IsRunning = false
            }
        };

        // Act
        var userModel = new BetChoiceMatchingUserModel(tickets);

        // Assert
        Assert.Equal(1, userModel.CustId);
        Assert.Equal("user1", userModel.Username);
        Assert.Equal(300, userModel.Stake);
        Assert.Equal(150, userModel.MemberWinLoss);
        Assert.Equal(60, userModel.SuperWinLoss);
        Assert.Equal(90, userModel.Company);
        Assert.False(userModel.IsRunning);
        Assert.Equal(tickets, userModel.Tickets);
    }

    [Fact]
    public void Constructor_ShouldRoundDownMemberWinLoss()
    {
        // Arrange
        var tickets = new List<BetChoiceMatchingTicketModel>
        {
            new BetChoiceMatchingTicketModel
            {
                CustId = 1,
                Username = "user1",
                Stake = 100,
                MemberWinLoss = 50.5678m,
                SuperWinLoss = 20,
                Company = 30,
                IsRunning = false
            }
        };

        // Act
        var userModel = new BetChoiceMatchingUserModel(tickets);

        // Assert
        Assert.Equal(50.56m, userModel.MemberWinLoss);
    }
}
