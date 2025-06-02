using AlphaPlus.PartialUM.Services;
using AlphaPlus.Reports.Features.Shared;
using AlphaPlus.Reports.Features.Shared.Models;
using AlphaPlus.Reports.Features.WinLoss;
using Fanex.Data.Repository;

namespace AlphaPlus.Reports.UnitTests.Features.WinLoss;

public class WinLossServiceTests
{
    private readonly Mock<IDynamicRepository> repositoryMock;
    private readonly Mock<IReportDateService> reportDateServiceMock;
    private readonly Mock<IPartialUMService> partialUMServiceMock;
    private readonly WinLossService service;

    public WinLossServiceTests()
    {
        repositoryMock = new Mock<IDynamicRepository>();
        reportDateServiceMock = new Mock<IReportDateService>();
        partialUMServiceMock = new Mock<IPartialUMService>();
        service = new WinLossService(repositoryMock.Object, reportDateServiceMock.Object, partialUMServiceMock.Object);
    }

    [Fact]
    public async Task GetWinLoss_WhenAgentLevel_ShouldUseWinLossMemberCriteria()
    {
        // Arrange
        var param = new WinLossParam(
            CustId: 123,
            CustLevelId: RoleIdConstants.Agent,
            FromDate: DateTime.Now.AddDays(-7),
            ToDate: DateTime.Now,
            ProductIds: "1,2,3",
            ProductDetailsXml: "<products></products>",
            LevelType: 0,
            SiteName: "TestSite"
        );

        var reportDate = new BalanceUpDayAndMoveBetsDay { BalanceUpDay = DateTime.Now, MoveBetsDay = DateTime.Now };
        reportDateServiceMock.Setup(x => x.GetBalanceUpDayAndMoveBetsDay()).ReturnsAsync(reportDate);

        var winLossData = new List<WinLossModel> { new WinLossModel { CustId = 123 } };
        repositoryMock.Setup(x => x.FetchAsync<WinLossModel>(It.IsAny<ICriteria>(), string.Empty)).ReturnsAsync(winLossData);

        // Act
        var result = await service.GetWinLoss(param);

        // Assert
        repositoryMock.Verify(x => x.FetchAsync<WinLossModel>(It.Is<ICriteria>(c => c is WinLossMemberCriteria), string.Empty), Times.Once);
        Assert.Single(result.DataSource);
        Assert.Equal(123, result.DataSource.First().CustId);
    }

    [Fact]
    public async Task GetWinLoss_WhenMasterLevel_ShouldUseWinLossAgentCriteria()
    {
        // Arrange
        var param = new WinLossParam(
            CustId: 456,
            CustLevelId: RoleIdConstants.Master,
            FromDate: DateTime.Now.AddDays(-7),
            ToDate: DateTime.Now,
            ProductIds: "1,2,3",
            ProductDetailsXml: "<products></products>",
            LevelType: 1,
            SiteName: "TestSite"
        );

        var reportDate = new BalanceUpDayAndMoveBetsDay { BalanceUpDay = DateTime.Now, MoveBetsDay = DateTime.Now };
        reportDateServiceMock.Setup(x => x.GetBalanceUpDayAndMoveBetsDay()).ReturnsAsync(reportDate);

        var winLossData = new List<WinLossModel> { new WinLossModel { CustId = 456 } };
        repositoryMock.Setup(x => x.FetchAsync<WinLossModel>(It.IsAny<ICriteria>(), string.Empty)).ReturnsAsync(winLossData);

        // Act
        var result = await service.GetWinLoss(param);

        // Assert
        repositoryMock.Verify(x => x.FetchAsync<WinLossModel>(It.Is<ICriteria>(c => c is WinLossAgentCriteria), string.Empty), Times.Once);
        Assert.Single(result.DataSource);
        Assert.Equal(456, result.DataSource.First().CustId);
    }

    [Fact]
    public async Task GetWinLoss_WhenSuperOrHigherLevel_ShouldUseWinLossMasterCriteria()
    {
        // Arrange
        var param = new WinLossParam(
            CustId: 789,
            CustLevelId: RoleIdConstants.Super,
            FromDate: DateTime.Now.AddDays(-7),
            ToDate: DateTime.Now,
            ProductIds: "1,2,3",
            ProductDetailsXml: "<products></products>",
            LevelType: 2,
            SiteName: "TestSite"
        );

        var reportDate = new BalanceUpDayAndMoveBetsDay { BalanceUpDay = DateTime.Now, MoveBetsDay = DateTime.Now };
        reportDateServiceMock.Setup(x => x.GetBalanceUpDayAndMoveBetsDay()).ReturnsAsync(reportDate);

        var winLossData = new List<WinLossModel> { new WinLossModel { CustId = 789 } };
        repositoryMock.Setup(x => x.FetchAsync<WinLossModel>(It.IsAny<ICriteria>(), string.Empty)).ReturnsAsync(winLossData);

        // Act
        var result = await service.GetWinLoss(param);

        // Assert
        repositoryMock.Verify(x => x.FetchAsync<WinLossModel>(It.Is<ICriteria>(c => c is WinLossMasterCriteria), string.Empty), Times.Once);
        Assert.Single(result.DataSource);
        Assert.Equal(789, result.DataSource.First().CustId);
    }

    [Fact]
    public async Task GetWinLoss_ShouldCalculateTotal()
    {
        // Arrange
        var param = new WinLossParam(
            CustId: 123,
            CustLevelId: RoleIdConstants.Agent,
            FromDate: DateTime.Now.AddDays(-7),
            ToDate: DateTime.Now,
            ProductIds: "1,2,3",
            ProductDetailsXml: "<products></products>",
            LevelType: 0,
            SiteName: string.Empty
        );

        var reportDate = new BalanceUpDayAndMoveBetsDay { BalanceUpDay = DateTime.Now, MoveBetsDay = DateTime.Now };
        reportDateServiceMock.Setup(x => x.GetBalanceUpDayAndMoveBetsDay()).ReturnsAsync(reportDate);

        var winLossData = new List<WinLossModel>
        {
            new WinLossModel { CustId = 123, Turnover = 100, GrossComm = 10, MemberWinLoss = 50, MemberComm = 5 },
            new WinLossModel { CustId = 456, Turnover = 200, GrossComm = 20, MemberWinLoss = 100, MemberComm = 10 }
        };
        repositoryMock.Setup(x => x.FetchAsync<WinLossModel>(It.IsAny<ICriteria>(), string.Empty)).ReturnsAsync(winLossData);

        // Act
        var result = await service.GetWinLoss(param);

        // Assert
        Assert.Equal(2, result.TotalRecords);
        Assert.Equal(300, result.Total.Turnover);
        Assert.Equal(30, result.Total.GrossComm);
        Assert.Equal(150, result.Total.MemberWinLoss);
        Assert.Equal(15, result.Total.MemberComm);
    }

    [Fact]
    public async Task GetWinLoss_ShouldCheckPartialUM()
    {
        // Arrange
        var param = new WinLossParam(
            CustId: 123,
            CustLevelId: RoleIdConstants.Agent,
            FromDate: DateTime.Now.AddDays(-7),
            ToDate: DateTime.Now,
            ProductIds: "1,2,3",
            ProductDetailsXml: "<products></products>",
            LevelType: 0,
            SiteName: "TestSite"
        );

        var reportDate = new BalanceUpDayAndMoveBetsDay { BalanceUpDay = DateTime.Now, MoveBetsDay = DateTime.Now };
        reportDateServiceMock.Setup(x => x.GetBalanceUpDayAndMoveBetsDay()).ReturnsAsync(reportDate);

        var winLossData = new List<WinLossModel> { new WinLossModel { CustId = 123 } };
        repositoryMock.Setup(x => x.FetchAsync<WinLossModel>(It.IsAny<ICriteria>(), string.Empty)).ReturnsAsync(winLossData);

        // Act
        await service.GetWinLoss(param);

        // Assert
        partialUMServiceMock.Verify(x => x.ThrowIfUM(It.IsAny<string>(), "TestSite"), Times.Once);
    }

    [Fact]
    public async Task GetWinLoss_WhenNoData_ShouldReturnEmptyTotalModel()
    {
        // Arrange
        var param = new WinLossParam(
            CustId: 123,
            CustLevelId: RoleIdConstants.Agent,
            FromDate: DateTime.Now.AddDays(-7),
            ToDate: DateTime.Now,
            ProductIds: "1,2,3",
            ProductDetailsXml: "<products></products>",
            LevelType: 0,
            SiteName: string.Empty
        );

        var reportDate = new BalanceUpDayAndMoveBetsDay { BalanceUpDay = DateTime.Now, MoveBetsDay = DateTime.Now };
        reportDateServiceMock.Setup(x => x.GetBalanceUpDayAndMoveBetsDay()).ReturnsAsync(reportDate);

        var winLossData = new List<WinLossModel>();
        repositoryMock.Setup(x => x.FetchAsync<WinLossModel>(It.IsAny<ICriteria>(), string.Empty)).ReturnsAsync(winLossData);

        // Act
        var result = await service.GetWinLoss(param);

        // Assert
        Assert.Empty(result.DataSource);
        Assert.Equal(0, result.TotalRecords);
        Assert.False(result.HasData);
        Assert.False(result.MoreThanOne);
    }
}