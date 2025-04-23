using AlphaPlus.Reports.Features.ProgressiveContribution;

namespace AlphaPlus.Reports.UnitTests.Features.ProgressiveContribution;

[Trait("Category", "ProgressiveContribution")]
public class ProgressiveContributionEntityTest
{
    [Fact]
    public void CompanyContribution_ShouldCalculateCorrectly()
    {
        // Arrange
        var entity = new ProgressiveContributionEntity
        {
            ContributionSuper = 100m,
            ContributionMaster = 200m,
            ContributionAgent = 300m
        };

        // Act
        var result = entity.CompanyContribution;

        // Assert
        Assert.Equal(-600m, result);
    }

    [Fact]
    public void CompanyContribution_ShouldIgnoreSetValue()
    {
        // Arrange
        var entity = new ProgressiveContributionEntity
        {
            ContributionSuper = 100m,
            ContributionMaster = 200m,
            ContributionAgent = 300m
        };

        // Act
        entity.CompanyContribution = 999m;

        // Assert
        Assert.Equal(-600m, entity.CompanyContribution);
    }

    [Fact]
    public void OperatorPlus_ShouldAddTwoEntitiesProperly()
    {
        // Arrange
        var entity1 = new ProgressiveContributionEntity
        {
            ProductId = 1,
            BetType = 10,
            BetTypeName = "Test",
            Turnover = 100m,
            Contribution = 50m,
            ContributionAgent = 20m,
            ContributionMaster = 10m,
            ContributionSuper = 5m
        };

        var entity2 = new ProgressiveContributionEntity
        {
            ProductId = 1,
            BetType = 10,
            BetTypeName = "Test",
            Turnover = 200m,
            Contribution = 100m,
            ContributionAgent = 40m,
            ContributionMaster = 20m,
            ContributionSuper = 10m
        };

        // Act
        var result = entity1 + entity2;

        // Assert
        Assert.Equal(300m, result.Turnover);
        Assert.Equal(150m, result.Contribution);
        Assert.Equal(60m, result.ContributionAgent);
        Assert.Equal(30m, result.ContributionMaster);
        Assert.Equal(15m, result.ContributionSuper);
        Assert.Equal(-105m, result.CompanyContribution);
    }

    [Fact]
    public void OperatorPlus_ShouldReturnFirstEntity()
    {
        // Arrange
        var entity1 = new ProgressiveContributionEntity
        {
            ProductId = 1,
            BetType = 10,
            BetTypeName = "Test"
        };

        var entity2 = new ProgressiveContributionEntity
        {
            ProductId = 2,
            BetType = 20,
            BetTypeName = "Different"
        };

        // Act
        var result = entity1 + entity2;

        // Assert
        Assert.Equal(1, result.ProductId);
        Assert.Equal(10, result.BetType);
        Assert.Equal("Test", result.BetTypeName);
    }

    [Fact]
    public void OperatorPlus_ShouldHandleZeroValues()
    {
        // Arrange
        var entity1 = new ProgressiveContributionEntity();
        var entity2 = new ProgressiveContributionEntity();

        // Act
        var result = entity1 + entity2;

        // Assert
        Assert.Equal(0m, result.Turnover);
        Assert.Equal(0m, result.Contribution);
        Assert.Equal(0m, result.ContributionAgent);
        Assert.Equal(0m, result.ContributionMaster);
        Assert.Equal(0m, result.ContributionSuper);
        Assert.Equal(0m, result.CompanyContribution);
    }
}