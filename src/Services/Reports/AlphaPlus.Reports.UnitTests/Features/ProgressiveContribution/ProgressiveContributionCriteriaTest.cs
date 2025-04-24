using AlphaPlus.Reports.Features.ProgressiveContribution;

namespace AlphaPlus.Reports.UnitTests.Features.ProgressiveContribution;

[Trait("Category", "ProgressiveContribution")]
public class ProgressiveContributionCriteriaTest
{
    [Fact]
    public void GetSettingKey_WithAgentLevelId_ShouldReturnMemberSettingKey()
    {
        // Arrange
        byte agentLevelId = RoleIdConstants.Agent;
        var criteria = new ProgressiveContributionCriteria(
            custId: 1,
            fromDate: DateTime.Today.AddDays(-7),
            toDate: DateTime.Today,
            balanceUpDay: DateTime.Today,
            moveBetsDay: DateTime.Today,
            productIds: "1,2,3",
            viewerLevelId: agentLevelId);

        // Act
        var result = criteria.GetSettingKey();

        // Assert
        Assert.Equal("Age_Rpt_ProgressiveGamesContribution_Member", result);
    }

    [Fact]
    public void GetSettingKey_WithMasterLevelId_ShouldReturnAgentSettingKey()
    {
        // Arrange
        byte masterLevelId = RoleIdConstants.Master;
        var criteria = new ProgressiveContributionCriteria(
            custId: 1,
            fromDate: DateTime.Today.AddDays(-7),
            toDate: DateTime.Today,
            balanceUpDay: DateTime.Today,
            moveBetsDay: DateTime.Today,
            productIds: "1,2,3",
            viewerLevelId: masterLevelId);

        // Act
        var result = criteria.GetSettingKey();

        // Assert
        Assert.Equal("Age_Rpt_ProgressiveGamesContribution_Agent", result);
    }

    [Fact]
    public void GetSettingKey_WithOtherLevelId_ShouldReturnMasterSettingKey()
    {
        // Arrange
        byte otherLevelId = 0; // Non-agent, non-master level ID
        var criteria = new ProgressiveContributionCriteria(
            custId: 1,
            fromDate: DateTime.Today.AddDays(-7),
            toDate: DateTime.Today,
            balanceUpDay: DateTime.Today,
            moveBetsDay: DateTime.Today,
            productIds: "1,2,3",
            viewerLevelId: otherLevelId);

        // Act
        var result = criteria.GetSettingKey();

        // Assert
        Assert.Equal("Age_Rpt_ProgressiveGamesContribution_Master", result);
    }

    [Fact]
    public void Constructor_ShouldInitializeProperties()
    {
        // Arrange
        int custId = 1;
        DateTime fromDate = DateTime.Today.AddDays(-7);
        DateTime toDate = DateTime.Today;
        DateTime balanceUpDay = DateTime.Today.AddDays(-3);
        DateTime moveBetsDay = DateTime.Today.AddDays(-2);
        string productIds = "1,2,3";
        byte viewerLevelId = RoleIdConstants.Agent;

        // Act
        var criteria = new ProgressiveContributionCriteria(
            custId, fromDate, toDate, balanceUpDay, moveBetsDay, productIds, viewerLevelId);

        // Assert
        Assert.Equal(custId, criteria.CustId);
        Assert.Equal(fromDate, criteria.FromDate);
        Assert.Equal(toDate, criteria.ToDate);
        Assert.Equal(balanceUpDay, criteria.BalanceUpDay);
        Assert.Equal(moveBetsDay, criteria.MoveBetsDay);
        Assert.Equal(productIds, criteria.ProductIds);
    }
}