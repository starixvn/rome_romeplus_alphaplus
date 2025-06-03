using AlphaPlus.PartialUM.Services;
using AlphaPlus.Reports.Features.Shared;
using AlphaPlus.Reports.Features.Shared.Models;
using AlphaPlus.Reports.Features.WinLossAnalysisChart;
using Fanex.Data.Repository;

namespace AlphaPlus.Reports.UnitTests.Features.WinLossAnalysisChart;

[Trait("Category", "WinLossAnalysisChart")]
public class WinLossAnalysisChartServiceTest
{
    private readonly Mock<IReportDateService> reportDateServiceMock = new();
    private readonly Mock<IDynamicRepository> repositoryMock = new();
    private readonly Mock<IPartialUMService> partialUMServiceMock = new();
    private readonly WinLossAnalysisChartService service;

    private readonly WinLossAnalysisChartParam testParam = new(
        CustId: 123,
        CustLevelId: RoleIdConstants.Agent,
        SiteName: "TestSite",
        CurrencyId: 20,
        FromDate: new DateTime(2023, 1, 1),
        ToDate: new DateTime(2023, 3, 31),
        ProductIds: "1,2,3"
    );

    private readonly BalanceUpDayAndMoveBetsDay testReportDate = new()
    {
        BalanceUpDay = new DateTime(2023, 3, 31),
        MoveBetsDay = new DateTime(2023, 3, 31)
    };

    public WinLossAnalysisChartServiceTest()
    {
        service = new WinLossAnalysisChartService(
            reportDateServiceMock.Object,
            repositoryMock.Object,
            partialUMServiceMock.Object);
    }

    [Fact]
    public async Task GetWinLossAnalysisChart_Should_Call_PartialUMService_With_CorrectSettingKey()
    {
        // Arrange
        SetupMocks();

        // Act
        await service.GetWinLossAnalysisChart(testParam);

        // Assert
        partialUMServiceMock.Verify(
            s => s.ThrowIfUM("Age_Chart_WinlossAnalysis_Member", testParam.SiteName),
            Times.Once);
    }

    [Fact]
    public async Task GetWinLossAnalysisChart_Should_Call_Repository_With_CorrectCriteria()
    {
        // Arrange
        SetupMocks();

        // Act
        await service.GetWinLossAnalysisChart(testParam);

        // Assert
        repositoryMock.Verify(
            r => r.FetchAsync<WinLossAnalysisChartModel>(
                It.Is<WinLossAnalysisChartCriteria>(c =>
                    c.CustId == testParam.CustId &&
                    c.FromDate == testParam.FromDate &&
                    c.ToDate == testParam.ToDate &&
                    c.ProductIds == testParam.ProductIds &&
                    c.BalanceUpDay == testReportDate.BalanceUpDay &&
                    c.MoveBetsDay == testReportDate.MoveBetsDay), string.Empty),
            Times.Once);
    }

    [Fact]
    public async Task GetWinLossAnalysisChart_Should_Return_Data_With_EmptyMonths_Added()
    {
        // Arrange
        var existingData = new List<WinLossAnalysisChartModel>
        {
            new() { WinLossDate = "2023-01", Turnover = 1000, WinLoss = 100 },
            new() { WinLossDate = "2023-03", Turnover = 3000, WinLoss = 300 }
        };

        SetupMocks(existingData);

        // Act
        var result = (await service.GetWinLossAnalysisChart(testParam)).ToList();

        // Assert
        Assert.Equal(3, result.Count);

        Assert.Equal("2023-02", result[0].WinLossDate);
        Assert.Equal(0, result[0].Turnover);
        Assert.Equal(0, result[0].WinLoss);

        Assert.Equal("2023-01", result[1].WinLossDate);
        Assert.Equal(1000, result[1].Turnover);
        Assert.Equal(100, result[1].WinLoss);

        Assert.Equal("2023-03", result[2].WinLossDate);
        Assert.Equal(3000, result[2].Turnover);
        Assert.Equal(300, result[2].WinLoss);
    }

    [Fact]
    public async Task GetWinLossAnalysisChart_Should_Return_AllEmptyData_When_NoExistingData()
    {
        // Arrange
        var emptyData = new List<WinLossAnalysisChartModel>();

        SetupMocks(emptyData);

        // Act
        var result = (await service.GetWinLossAnalysisChart(testParam)).ToList();

        // Assert
        Assert.Equal(3, result.Count);

        Assert.Equal("2023-01", result[0].WinLossDate);
        Assert.Equal(0, result[0].Turnover);
        Assert.Equal(0, result[0].WinLoss);

        Assert.Equal("2023-02", result[1].WinLossDate);
        Assert.Equal(0, result[1].Turnover);
        Assert.Equal(0, result[1].WinLoss);

        Assert.Equal("2023-03", result[2].WinLossDate);
        Assert.Equal(0, result[2].Turnover);
        Assert.Equal(0, result[2].WinLoss);
    }

    private void SetupMocks(List<WinLossAnalysisChartModel> resultData = null)
    {
        reportDateServiceMock
            .Setup(s => s.GetBalanceUpDayAndMoveBetsDay())
            .ReturnsAsync(testReportDate);

        partialUMServiceMock
            .Setup(s => s.ThrowIfUM(It.IsAny<string>(), It.IsAny<string>()))
            .Returns(Task.CompletedTask);

        repositoryMock
            .Setup(r => r.FetchAsync<WinLossAnalysisChartModel>(It.IsAny<WinLossAnalysisChartCriteria>(), string.Empty))
            .ReturnsAsync(resultData ?? new List<WinLossAnalysisChartModel>());
    }
}