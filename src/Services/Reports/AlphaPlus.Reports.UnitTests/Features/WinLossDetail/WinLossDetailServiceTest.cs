using AlphaPlus.PartialUM.Services;
using AlphaPlus.Reports.Features.Shared;
using AlphaPlus.Reports.Features.Shared.Models;
using AlphaPlus.Reports.Features.WinLossDetail;
using Fanex.Data.Repository;

namespace AlphaPlus.Reports.UnitTests.Features.WinLossDetail;

using Reports.Features.WinLossDetail.Criteria;

[Trait("Category", "WinLossDetail")]
public class WinLossDetailServiceTest
{
    private readonly Mock<IDynamicRepository> repositoryMock;
    private readonly Mock<IReportDateService> reportDateServiceMock;
    private readonly Mock<IPartialUMService> partialUMServiceMock;
    private readonly WinLossDetailService service;

    public WinLossDetailServiceTest()
    {
        repositoryMock = new Mock<IDynamicRepository>();
        reportDateServiceMock = new Mock<IReportDateService>();
        partialUMServiceMock = new Mock<IPartialUMService>();

        service = new WinLossDetailService(
            repositoryMock.Object,
            reportDateServiceMock.Object,
            partialUMServiceMock.Object,
            new AllowedSitesToShareLoad(new Dictionary<string, string> { }));
    }

    [Fact]
    public async Task GetWeekData_Should_Return_Expected_Data()
    {
        // Arrange
        var param = new WinLossDetailWeekParam(
            ViewerId: 123,
            FromDate: DateTime.Now.AddDays(-7),
            ToDate: DateTime.Now,
            ProductIds: "1,2,3",
            ProductDetailsXml: "<products></products>",
            Username: "testuser",
            IsReward: false,
            LevelType: 1,
            SiteName: "TestSite"
        );

        var reportDate = new BalanceUpDayAndMoveBetsDay
        {
            BalanceUpDay = DateTime.Now.AddDays(-1),
            MoveBetsDay = DateTime.Now.AddDays(-2)
        };

        var expectedData = new List<WinLossDetailWeekModel>
        {
            new() { /* Properties initialization */ }
        };

        reportDateServiceMock
            .Setup(s => s.GetBalanceUpDayAndMoveBetsDay())
            .ReturnsAsync(reportDate);

        repositoryMock
            .Setup(r => r.FetchAsync<WinLossDetailWeekModel>(It.IsAny<WinLossDetailWeekDataCriteria>(), It.IsAny<string>()))
            .ReturnsAsync(expectedData);

        // Act
        var result = await service.GetWeekData(param);

        // Assert
        Assert.Equal(expectedData, result);
        reportDateServiceMock.Verify(s => s.GetBalanceUpDayAndMoveBetsDay(), Times.Once);
        repositoryMock.Verify(
            r => r.FetchAsync<WinLossDetailWeekModel>(
                It.Is<WinLossDetailWeekDataCriteria>(c =>
                    c.CustId == param.ViewerId &&
                    c.FromDate == param.FromDate &&
                    c.ToDate == param.ToDate &&
                    c.ProductIds == param.ProductIds),
                It.IsAny<string>()),
            Times.Once);
    }

    [Fact]
    public async Task GetWinLossDetail_Should_Return_Expected_Data()
    {
        // Arrange
        var param = new WinLossDetailParam(
            CustId: 123,
            CustLevelId: RoleIdConstants.Agent,
            FromDate: DateTime.Now.AddDays(-7),
            ToDate: DateTime.Now,
            ProductIds: "1,2,3",
            ProductDetailsXml: "<products></products>",
            Username: "testuser",
            IsReward: false,
            LevelType: 1,
            SiteName: "TestSite"
        );

        var reportDate = new BalanceUpDayAndMoveBetsDay
        {
            BalanceUpDay = DateTime.Now.AddDays(-1),
            MoveBetsDay = DateTime.Now.AddDays(-2)
        };

        var rawData = new List<WinLossDetailModel>
        {
            new() { CustId = 1, NetTurnover = 100 },
            new() { CustId = 2, NetTurnover = 200 }
        };

        var totals = new List<WinLossDetailModel>
        {
            new() { NetTurnover = 300, GrossComm = 30, NumberOfTickets = 10 }
        };

        reportDateServiceMock
            .Setup(s => s.GetBalanceUpDayAndMoveBetsDay())
            .ReturnsAsync(reportDate);

        repositoryMock
            .Setup(r => r.FetchMultipleAsync<WinLossDetailModel, WinLossDetailModel>(
                It.IsAny<WinLossDetailMemberCriteria>(), It.IsAny<string>()))
            .ReturnsAsync((rawData, totals));

        // Act
        var result = await service.GetWinLossDetail(param);

        // Assert
        Assert.Equal(rawData, result.DataSource);
        Assert.Equal(totals.First(), result.Total);
        reportDateServiceMock.Verify(s => s.GetBalanceUpDayAndMoveBetsDay(), Times.Once);
        partialUMServiceMock.Verify(s => s.ThrowIfUM(It.IsAny<string>(), param.SiteName), Times.Once);
    }

    [Fact]
    public async Task GetWinLossDetail_Should_Use_MasterCriteria_When_CustLevelId_Is_SuperAdmin()
    {
        // Arrange
        var param = new WinLossDetailParam(
            CustId: 123,
            CustLevelId: 9,
            FromDate: DateTime.Now.AddDays(-7),
            ToDate: DateTime.Now,
            ProductIds: "1,2,3",
            ProductDetailsXml: "<products></products>",
            Username: "testuser",
            IsReward: false,
            LevelType: 1,
            SiteName: "TestSite"
        );

        var reportDate = new BalanceUpDayAndMoveBetsDay();

        reportDateServiceMock
            .Setup(s => s.GetBalanceUpDayAndMoveBetsDay())
            .ReturnsAsync(reportDate);

        repositoryMock
            .Setup(r => r.FetchMultipleAsync<WinLossDetailModel, WinLossDetailModel>(
                It.IsAny<WinLossDetailMasterCriteria>(), It.IsAny<string>()))
            .ReturnsAsync((new List<WinLossDetailModel>(), new List<WinLossDetailModel> { new() }));

        // Act
        await service.GetWinLossDetail(param);

        // Assert
        repositoryMock.Verify(
            r => r.FetchMultipleAsync<WinLossDetailModel, WinLossDetailModel>(
                It.Is<WinLossDetailMasterCriteria>(c => c.CustId == param.CustId),
                It.IsAny<string>()),
            Times.Once);
    }

    [Fact]
    public async Task GetWinLossDetail_Should_Use_AgentCriteria_When_CustLevelId_Is_Master()
    {
        // Arrange
        var param = new WinLossDetailParam(
            CustId: 123,
            CustLevelId: RoleIdConstants.Master,
            FromDate: DateTime.Now.AddDays(-7),
            ToDate: DateTime.Now,
            ProductIds: "1,2,3",
            ProductDetailsXml: "<products></products>",
            Username: "testuser",
            IsReward: false,
            LevelType: 1,
            SiteName: "TestSite"
        );

        var reportDate = new BalanceUpDayAndMoveBetsDay();

        reportDateServiceMock
            .Setup(s => s.GetBalanceUpDayAndMoveBetsDay())
            .ReturnsAsync(reportDate);

        repositoryMock
            .Setup(r => r.FetchMultipleAsync<WinLossDetailModel, WinLossDetailModel>(
                It.IsAny<WinLossDetailAgentCriteria>(), It.IsAny<string>()))
            .ReturnsAsync((new List<WinLossDetailModel>(), new List<WinLossDetailModel> { new() }));

        // Act
        await service.GetWinLossDetail(param);

        // Assert
        repositoryMock.Verify(
            r => r.FetchMultipleAsync<WinLossDetailModel, WinLossDetailModel>(
                It.Is<WinLossDetailAgentCriteria>(c => c.CustId == param.CustId),
                It.IsAny<string>()),
            Times.Once);
    }

    [Fact]
    public async Task GetWinLossDetail_Should_Use_MemberCriteria_When_CustLevelId_Is_Agent()
    {
        // Arrange
        var param = new WinLossDetailParam(
            CustId: 123,
            CustLevelId: RoleIdConstants.Agent,
            FromDate: DateTime.Now.AddDays(-7),
            ToDate: DateTime.Now,
            ProductIds: "1,2,3",
            ProductDetailsXml: "<products></products>",
            Username: "testuser",
            IsReward: false,
            LevelType: 1,
            SiteName: "TestSite"
        );

        var reportDate = new BalanceUpDayAndMoveBetsDay();

        reportDateServiceMock
            .Setup(s => s.GetBalanceUpDayAndMoveBetsDay())
            .ReturnsAsync(reportDate);

        repositoryMock
            .Setup(r => r.FetchMultipleAsync<WinLossDetailModel, WinLossDetailModel>(
                It.IsAny<WinLossDetailMemberCriteria>(), It.IsAny<string>()))
            .ReturnsAsync((new List<WinLossDetailModel>(), new List<WinLossDetailModel> { new() }));

        // Act
        await service.GetWinLossDetail(param);

        // Assert
        repositoryMock.Verify(
            r => r.FetchMultipleAsync<WinLossDetailModel, WinLossDetailModel>(
                It.Is<WinLossDetailMemberCriteria>(c =>
                    c.CustId == param.CustId &&
                    !(c is WinLossDetailAgentCriteria) &&
                    !(c is WinLossDetailMasterCriteria)),
                It.IsAny<string>()),
            Times.Once);
    }
}