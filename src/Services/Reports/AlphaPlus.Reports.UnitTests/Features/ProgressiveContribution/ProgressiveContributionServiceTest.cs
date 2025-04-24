using AlphaPlus.PartialUM.Services;
using AlphaPlus.Reports.Features.ProgressiveContribution;
using AlphaPlus.Reports.Features.Shared;
using AlphaPlus.Reports.Features.Shared.Models;
using Fanex.Data.Repository;

namespace AlphaPlus.Reports.UnitTests.Features.ProgressiveContribution;

[Trait("Category", "ProgressiveContribution")]
public class ProgressiveContributionServiceTest
{
    private readonly Mock<IDynamicRepository> mockRepository;
    private readonly Mock<IPartialUMService> mockPartialUMService;
    private readonly Mock<ICacheRefService> mockCacheRefService;
    private readonly Mock<IReportDateService> mockReportDateService;
    private readonly ProgressiveContributionService service;
    private readonly ProgressiveContributionParam defaultParam;

    public ProgressiveContributionServiceTest()
    {
        mockRepository = new Mock<IDynamicRepository>();
        mockPartialUMService = new Mock<IPartialUMService>();
        mockCacheRefService = new Mock<ICacheRefService>();
        mockReportDateService = new Mock<IReportDateService>();

        service = new ProgressiveContributionService(
            mockRepository.Object,
            mockPartialUMService.Object,
            mockCacheRefService.Object,
            mockReportDateService.Object);

        defaultParam = new ProgressiveContributionParam(
            CustId: 123,
            FromDate: DateTime.Today.AddDays(-7),
            ToDate: DateTime.Today,
            ProductIds: "1,2",
            ViewerLevelId: 1,
            SiteName: "TestSite");
    }

    [Fact]
    public async Task GetProgressiveContributions_WhenNoData_ShouldReturnEmptyListAndEmptyTotal()
    {
        // Arrange
        SetupMocks(new List<ProgressiveContributionEntity>());

        // Act
        var (models, grandTotal) = await service.GetProgressiveContributions(defaultParam);

        // Assert
        Assert.Empty(models);
        Assert.Equal(0m, grandTotal.Turnover);
        Assert.Equal(0m, grandTotal.Contribution);
        Assert.Equal(0m, grandTotal.ContributionAgent);
        Assert.Equal(0m, grandTotal.ContributionMaster);
        Assert.Equal(0m, grandTotal.ContributionSuper);
    }

    [Fact]
    public async Task GetProgressiveContributions_WithData_ShouldGroupByProductAndCalculateTotals()
    {
        // Arrange
        var data = new List<ProgressiveContributionEntity>
            {
                new()
                {
                    ProductId = 1,
                    BetType = 10,
                    Turnover = 100m,
                    Contribution = 50m,
                    ContributionAgent = 20m,
                    ContributionMaster = 10m,
                    ContributionSuper = 5m
                },
                new()
                {
                    ProductId = 1,
                    BetType = 20,
                    Turnover = 200m,
                    Contribution = 60m,
                    ContributionAgent = 25m,
                    ContributionMaster = 15m,
                    ContributionSuper = 8m
                },
                new()
                {
                    ProductId = 2,
                    BetType = 30,
                    Turnover = 300m,
                    Contribution = 70m,
                    ContributionAgent = 30m,
                    ContributionMaster = 20m,
                    ContributionSuper = 10m
                }
            };

        mockCacheRefService.Setup(s => s.GetBetTypeName(It.IsAny<int>())).Returns("Bet Type Name");
        mockCacheRefService.Setup(s => s.GetProductName(1)).Returns("Product 1");
        mockCacheRefService.Setup(s => s.GetProductName(2)).Returns("Product 2");

        SetupMocks(data);

        // Act
        var (models, grandTotal) = await service.GetProgressiveContributions(defaultParam);

        // Assert
        Assert.Equal(2, models.Count());

        // Check product 1 totals
        var product1Model = models.First(m => m.ProductId == 1);
        Assert.Equal("Product 1", product1Model.ProductName);
        Assert.Equal(300m, product1Model.Total.Turnover);
        Assert.Equal(110m, product1Model.Total.Contribution);
        Assert.Equal(45m, product1Model.Total.ContributionAgent);
        Assert.Equal(25m, product1Model.Total.ContributionMaster);
        Assert.Equal(13m, product1Model.Total.ContributionSuper);
        Assert.Equal(-83m, product1Model.Total.CompanyContribution);
        Assert.Equal(2, product1Model.ProgressiveContributions.Count());

        // Check product 2 totals
        var product2Model = models.First(m => m.ProductId == 2);
        Assert.Equal("Product 2", product2Model.ProductName);
        Assert.Equal(300m, product2Model.Total.Turnover);
        Assert.Equal(70m, product2Model.Total.Contribution);
        Assert.Equal(30m, product2Model.Total.ContributionAgent);
        Assert.Equal(20m, product2Model.Total.ContributionMaster);
        Assert.Equal(10m, product2Model.Total.ContributionSuper);
        Assert.Equal(-60m, product2Model.Total.CompanyContribution);
        Assert.Single(product2Model.ProgressiveContributions);

        // Check grand totals
        Assert.Equal(600m, grandTotal.Turnover);
        Assert.Equal(180m, grandTotal.Contribution);
        Assert.Equal(75m, grandTotal.ContributionAgent);
        Assert.Equal(45m, grandTotal.ContributionMaster);
        Assert.Equal(23m, grandTotal.ContributionSuper);
        Assert.Equal(-143m, grandTotal.CompanyContribution);
    }

    [Fact]
    public async Task GetProgressiveContributions_ShouldVerifyPartialUMCheck()
    {
        // Arrange
        SetupMocks(new List<ProgressiveContributionEntity>());

        // Act
        await service.GetProgressiveContributions(defaultParam);

        // Assert
        mockPartialUMService.Verify(
            s => s.ThrowIfUM(
                It.IsAny<string>(),
                It.Is<string>(siteName => siteName == defaultParam.SiteName)),
            Times.Once);
    }

    private void SetupMocks(List<ProgressiveContributionEntity> data)
    {
        mockReportDateService
            .Setup(s => s.GetBalanceUpDayAndMoveBetsDay())
            .ReturnsAsync(new BalanceUpDayAndMoveBetsDay { BalanceUpDay = DateTime.Today, MoveBetsDay = DateTime.Today });

        mockRepository
            .Setup(r => r.FetchAsync<ProgressiveContributionEntity>(It.IsAny<ProgressiveContributionCriteria>(), string.Empty))
            .ReturnsAsync(data);

        mockPartialUMService
            .Setup(s => s.ThrowIfUM(It.IsAny<string>(), It.IsAny<string>()))
            .Returns(Task.CompletedTask);
    }
}