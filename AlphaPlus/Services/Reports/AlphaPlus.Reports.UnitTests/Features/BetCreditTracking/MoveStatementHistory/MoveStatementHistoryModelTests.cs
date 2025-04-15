using AlphaPlus.Reports.Features.BetCreditTracking.MoveStatementHistory;

namespace AlphaPlus.Reports.UnitTests.Features.BetCreditTracking.MoveStatementHistory;

[Trait("Category", "MoveStatementHistory")]
public class MoveStatementHistoryModelTests
{
    [Fact]
    public void Constructor_ShouldInitializePropertiesCorrectly()
    {
        // Arrange
        var errorCode = 1;
        var errorMessage = "Error message";
        var reportData = new List<MoveStatementHistoryByMember>
        {
            new MoveStatementHistoryByMember(
                "testuser",
                new MoveStatementHistoryByRefNo
                {
                    Username = "testuser",
                    MemberWinLoss = 100.50m,
                    MemberComm = 10.75m,
                    MemberTotal = 111.25m,
                    IsDirectMember = true
                },
                new List<MoveStatementHistoryByRefNo>
                {
                    new MoveStatementHistoryByRefNo
                    {
                        Username = "refuser1",
                        MemberWinLoss = 50.25m,
                        MemberComm = 5.50m,
                        MemberTotal = 55.75m,
                        IsDirectMember = false
                    }
                })
        };
        var summary = new MoveStatementHistorySummary
        {
            TotalMembers = 10,
            MemberWinLoss = 150.75m,
            MemberComm = 16.25m,
            MemberTotal = 167.00m,
            Variance = 0.5M
        };
        var totalRows = 10;

        // Act
        var model = new MoveStatementHistoryModel(errorCode, errorMessage, reportData, summary, totalRows);

        // Assert
        Assert.Equal(errorCode, model.ErrorCode);
        Assert.Equal(errorMessage, model.ErrorMessage);
        Assert.Equal(reportData, model.ReportData);
        Assert.Equal(summary, model.Summary);
        Assert.Equal(totalRows, model.TotalRows);
    }
}
