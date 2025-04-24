using AlphaPlus.PartialUM.Services;
using AlphaPlus.Reports.Features.Shared;
using AlphaPlus.Reports.Features.Shared.Models;
using AlphaPlus.Reports.Features.WinLossAnalysis;
using Fanex.Data.Repository;

namespace AlphaPlus.Reports.UnitTests.Features.WinLossAnalysis;

[Trait("Category", "WinLossAnalysis")]
public class WinLossAnalysisServiceTest
{
    private readonly Mock<IReportDateService> mockReportDateService;
    private readonly Mock<IDynamicRepository> mockRepository;
    private readonly Mock<ICacheRefService> mockCacheRefService;
    private readonly Mock<IPartialUMService> mockPartialUMService;
    private readonly WinLossAnalysisService service;

    public WinLossAnalysisServiceTest()
    {
        mockReportDateService = new Mock<IReportDateService>();
        mockRepository = new Mock<IDynamicRepository>();
        mockCacheRefService = new Mock<ICacheRefService>();
        mockPartialUMService = new Mock<IPartialUMService>();

        service = new WinLossAnalysisService(
            mockReportDateService.Object,
            mockRepository.Object,
            mockCacheRefService.Object,
            mockPartialUMService.Object);
    }

    [Fact]
    public async Task GetWinLossAnalysis_Should_Return_Correct_Data_For_Product_Type()
    {
        // Arrange
        var param = new WinLossAnalysisParam(
            ViewerId: 1,
            ViewerLevelId: 3,
            SiteName: "TestSite",
            ReportType: WinLossAnalysisConstants.Product,
            FromDate: new DateTime(2023, 1, 1),
            ToDate: new DateTime(2023, 1, 31),
            ProductIds: "1,2"
        );

        var reportDate = new BalanceUpDayAndMoveBetsDay
        {
            BalanceUpDay = new DateTime(2023, 1, 31),
            MoveBetsDay = new DateTime(2023, 1, 31)
        };

        var rawData = new List<WinLossAnalysisModel>
        {
            new() { RollupKey = "1", BetCount = 10, Turnover = 1000, MemberWinLoss = -200 },
            new() { RollupKey = "2", BetCount = 20, Turnover = 2000, MemberWinLoss = 300 }
        };

        mockReportDateService.Setup(s => s.GetBalanceUpDayAndMoveBetsDay()).ReturnsAsync(reportDate);
        mockRepository.Setup(r => r.FetchAsync<WinLossAnalysisModel>(It.IsAny<WinLossAnalysisCriteria>(), string.Empty))
            .ReturnsAsync(rawData);
        mockCacheRefService.Setup(c => c.GetProductName(1)).Returns("Product1");
        mockCacheRefService.Setup(c => c.GetProductName(2)).Returns("Product2");

        // Act
        var result = await service.GetWinLossAnalysis(param);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(2, result.DataSource.Count());
        Assert.Equal(30, result.Total.BetCount);
        Assert.Equal(3000, result.Total.Turnover);
        Assert.Equal(100, result.Total.MemberWinLoss);

        // Verify products are ordered by ID
        Assert.Equal(1, result.DataSource.ElementAt(0).RollupKeyId);
        Assert.Equal("Product1", result.DataSource.ElementAt(0).RollupKey);
        Assert.Equal(2, result.DataSource.ElementAt(1).RollupKeyId);
        Assert.Equal("Product2", result.DataSource.ElementAt(1).RollupKey);

        // Verify PartialUM check was called
        mockPartialUMService.Verify(p => p.ThrowIfUM(It.IsAny<string>(), "TestSite"), Times.Once);
    }

    [Fact]
    public async Task GetWinLossAnalysis_Should_Return_Correct_Data_For_Non_Product_Type()
    {
        // Arrange
        var param = new WinLossAnalysisParam(
            ViewerId: 1,
            ViewerLevelId: 3,
            SiteName: "TestSite",
            ReportType: WinLossAnalysisConstants.Daily,
            FromDate: new DateTime(2023, 1, 1),
            ToDate: new DateTime(2023, 1, 31),
            ProductIds: "1,2"
        );

        var reportDate = new BalanceUpDayAndMoveBetsDay
        {
            BalanceUpDay = new DateTime(2023, 1, 31),
            MoveBetsDay = new DateTime(2023, 1, 31)
        };

        var rawData = new List<WinLossAnalysisModel>
        {
            new() { RollupKey = "Member1", MAgWinLoss = 10, MAgComm = 1000, SAgWinLoss = -200 },
            new() { RollupKey = "Member2", MAgWinLoss = 20, MAgComm = 2000, SAgWinLoss = 300 }
        };

        mockReportDateService.Setup(s => s.GetBalanceUpDayAndMoveBetsDay()).ReturnsAsync(reportDate);
        mockRepository.Setup(r => r.FetchAsync<WinLossAnalysisModel>(It.IsAny<WinLossAnalysisCriteria>(), string.Empty))
            .ReturnsAsync(rawData);

        // Act
        var result = await service.GetWinLossAnalysis(param);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(2, result.DataSource.Count());
        Assert.Equal(30, result.Total.MAgWinLoss);
        Assert.Equal(3000, result.Total.MAgComm);
        Assert.Equal(100, result.Total.SAgWinLoss);

        // Verify original keys are preserved for non-product type
        Assert.Equal("Member1", result.DataSource.ElementAt(0).RollupKey);
        Assert.Equal("Member2", result.DataSource.ElementAt(1).RollupKey);

        // Verify product lookup was not called
        mockCacheRefService.Verify(c => c.GetProductName(It.IsAny<int>()), Times.Never);
    }

    [Fact]
    public async Task GetWinLossAnalysis_Should_Check_PartialUM()
    {
        // Arrange
        var param = new WinLossAnalysisParam(
            ViewerId: 1,
            ViewerLevelId: 3,
            SiteName: "TestSite",
            ReportType: WinLossAnalysisConstants.Monthly,
            FromDate: new DateTime(2023, 1, 1),
            ToDate: new DateTime(2023, 1, 31),
            ProductIds: "1,2"
        );

        var reportDate = new BalanceUpDayAndMoveBetsDay
        {
            BalanceUpDay = new DateTime(2023, 1, 31),
            MoveBetsDay = new DateTime(2023, 1, 31)
        };

        mockReportDateService.Setup(s => s.GetBalanceUpDayAndMoveBetsDay()).ReturnsAsync(reportDate);
        mockRepository.Setup(r => r.FetchAsync<WinLossAnalysisModel>(It.IsAny<WinLossAnalysisCriteria>(), string.Empty))
            .ReturnsAsync(new List<WinLossAnalysisModel>());

        // Setup PartialUM to throw an exception
        mockPartialUMService.Setup(p => p.ThrowIfUM(It.IsAny<string>(), "TestSite"))
            .ThrowsAsync(new UnauthorizedAccessException("UM Check failed"));

        // Act & Assert
        await Assert.ThrowsAsync<UnauthorizedAccessException>(() => service.GetWinLossAnalysis(param));
    }
}
