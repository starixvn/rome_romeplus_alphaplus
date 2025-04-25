using AlphaPlus.Reports.Features.BetCreditTracking.MoveStatementHistory;

namespace AlphaPlus.Reports.UnitTests.Features.BetCreditTracking.MoveStatementHistory;

[Trait("Category", "MoveStatementHistory")]
public class MoveStatementHistoryByMemberTest
{
    [Fact]
    public void Constructor_ShouldInitializePropertiesCorrectly()
    {
        // Arrange
        var username = "testuser";
        var memberRow = new MoveStatementHistoryByRefNo
        {
            Username = username,
            MemberWinLoss = 100.50m,
            MemberComm = 10.75m,
            MemberTotal = 111.25m,
            IsDirectMember = true
        };
        var refNoRow = new List<MoveStatementHistoryByRefNo>
        {
            new MoveStatementHistoryByRefNo
            {
                Username = "refuser1",
                MemberWinLoss = 50.25m,
                MemberComm = 5.50m,
                MemberTotal = 55.75m,
                IsDirectMember = false
            },
            new MoveStatementHistoryByRefNo
            {
                Username = "refuser2",
                MemberWinLoss = 75.00m,
                MemberComm = 7.50m,
                MemberTotal = 82.50m,
                IsDirectMember = false
            }
        };

        // Act
        var historyByMember = new MoveStatementHistoryByMember(username, memberRow, refNoRow);

        // Assert
        Assert.Equal(username, historyByMember.MemberRow.Username);
        Assert.Equal(memberRow.MemberWinLoss, historyByMember.MemberRow.MemberWinLoss);
        Assert.Equal(memberRow.MemberComm, historyByMember.MemberRow.MemberComm);
        Assert.Equal(memberRow.MemberTotal, historyByMember.MemberRow.MemberTotal);
        Assert.Equal(memberRow.IsDirectMember, historyByMember.MemberRow.IsDirectMember);
        Assert.Equal(refNoRow, historyByMember.RefNoRow);
    }
}
