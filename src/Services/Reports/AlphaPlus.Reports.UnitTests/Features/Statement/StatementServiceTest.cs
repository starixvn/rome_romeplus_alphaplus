using AlphaPlus.PartialUM.Services;
using AlphaPlus.Reports.Features.Shared;
using AlphaPlus.Reports.Features.Shared.Models;
using AlphaPlus.Reports.Features.Statement;
using Fanex.Data.Repository;

namespace AlphaPlus.Reports.UnitTests.Features.Statement;

[Trait("Category", "Statement")]
public class StatementServiceTest
{
    private readonly Mock<IReportDateService> reportDateServiceMock;
    private readonly Mock<IDynamicRepository> repositoryMock;
    private readonly Mock<IPartialUMService> partialUMServiceMock;
    private readonly StatementService service;
    private readonly string siteName = "TestSite";

    public StatementServiceTest()
    {
        reportDateServiceMock = new Mock<IReportDateService>();
        repositoryMock = new Mock<IDynamicRepository>();
        partialUMServiceMock = new Mock<IPartialUMService>();

        service = new StatementService(
            reportDateServiceMock.Object,
            repositoryMock.Object,
            partialUMServiceMock.Object);
    }

    [Fact]
    public async Task GetStatements_ShouldReturnCombinedResults_FromMultipleTimeRanges()
    {
        // Arrange
        var custId = 123;
        var fromDate = new DateTime(2023, 1, 1);
        var toDate = new DateTime(2023, 1, 10);
        var balanceUpDay = new DateTime(2023, 1, 5);

        var param = new GetStatementParam(
            CustId: custId,
            FromDate: fromDate,
            ToDate: toDate,
            SiteName: siteName);

        var timeRanges = new List<TimeRange>
        {
            new TimeRange(new DateTime(2023, 1, 1), new DateTime(2023, 1, 5)),
            new TimeRange(new DateTime(2023, 1, 6), new DateTime(2023, 1, 10))
        };

        reportDateServiceMock
            .Setup(x => x.GetTimeRangesToGetReportDataBetweenTables(fromDate, toDate))
            .ReturnsAsync(timeRanges);

        reportDateServiceMock
            .Setup(x => x.GetBalanceUpDay())
            .ReturnsAsync(balanceUpDay);

        reportDateServiceMock
            .Setup(x => x.GetStoreMappingSuffix(timeRanges[0].FromDate))
            .ReturnsAsync("_Warehouse");

        reportDateServiceMock
            .Setup(x => x.GetStoreMappingSuffix(timeRanges[1].FromDate))
            .ReturnsAsync("_BetTrans14");

        var statements1 = new List<StatementModel>
        {
            new()
            {
                WinLossDate = DateTime.Now.AddDays(-1),
                CustId = 1001,
                OpeningBalance = 5000.00m,
                Turnover = 12500.50m,
                WinLoss = -750.25m,
                Commission = 125.50m,
                TransferAmount = 1000.00m,
                ClosingBalance = 5375.25m,
                Outstanding = 0.00m,
                VoidAmount = 50.00m,
                GrandPrize = null
            },
            new()
            {
                WinLossDate = DateTime.Now,
                CustId = 1002,
                OpeningBalance = 2500.00m,
                Turnover = 7800.75m,
                WinLoss = 1250.50m,
                Commission = 78.00m,
                TransferAmount = -500.00m,
                ClosingBalance = 3172.50m,
                Outstanding = 125.00m,
                VoidAmount = 0.00m,
                GrandPrize = 500.00m
            }
        };

        var statements2 = new List<StatementModel>
        {
            new()
            {
                WinLossDate = DateTime.Now.AddDays(-5),
                CustId = 1045,
                OpeningBalance = 3200.50m,
                Turnover = 9750.25m,
                WinLoss = -450.75m,
                Commission = 97.50m,
                TransferAmount = 1500.00m,
                ClosingBalance = 4152.25m,
                Outstanding = 75.00m,
                VoidAmount = 25.00m,
                GrandPrize = null
            },
            new()
            {
                WinLossDate = DateTime.Now.AddDays(-2),
                CustId = 1089,
                OpeningBalance = 7500.00m,
                Turnover = 15300.50m,
                WinLoss = 2250.75m,
                Commission = 153.00m,
                TransferAmount = -1000.00m,
                ClosingBalance = 8597.75m,
                Outstanding = 0.00m,
                VoidAmount = 0.00m,
                GrandPrize = 750.00m
            }
        };

        repositoryMock
            .Setup(x => x.FetchAsync<StatementModel>(It.IsAny<GetStatementCriteria>(), string.Empty))
            .ReturnsAsync(statements1);

        repositoryMock
            .Setup(x => x.FetchAsync<StatementModel>(It.IsAny<GetStatementBetTrans14Criteria>(), string.Empty))
            .ReturnsAsync(statements2);

        // Act
        var result = await service.GetStatements(param);

        // Assert
        Assert.Equal(4, result.Count());
    }

    [Fact]
    public async Task GetStatements_ShouldUseCorrectCriteria_BasedOnStoreMappingSuffix()
    {
        // Arrange
        var custId = 123;
        var fromDate = new DateTime(2023, 1, 1);
        var toDate = new DateTime(2023, 1, 5);
        var balanceUpDay = new DateTime(2023, 1, 10);

        var param = new GetStatementParam(
            CustId: custId,
            FromDate: fromDate,
            ToDate: toDate,
            SiteName: siteName);

        var timeRanges = new List<TimeRange>
        {
            new TimeRange(fromDate, toDate)
        };

        reportDateServiceMock
            .Setup(x => x.GetTimeRangesToGetReportDataBetweenTables(fromDate, toDate))
            .ReturnsAsync(timeRanges);

        reportDateServiceMock
            .Setup(x => x.GetBalanceUpDay())
            .ReturnsAsync(balanceUpDay);

        reportDateServiceMock
            .Setup(x => x.GetStoreMappingSuffix(fromDate))
            .ReturnsAsync("_14");

        var statements = new List<StatementModel>();

        repositoryMock
            .Setup(x => x.FetchAsync<StatementModel>(It.IsAny<GetStatementBetTrans14Criteria>(), string.Empty))
            .ReturnsAsync(statements)
            .Verifiable();

        // Act
        var result = await service.GetStatements(param);

        // Assert
        repositoryMock.Verify(x => x.FetchAsync<StatementModel>(It.IsAny<GetStatementBetTrans14Criteria>(), string.Empty), Times.Once);
    }

    [Fact]
    public async Task GetStatements_ShouldCallPartialUMService_ForEachTimeRange()
    {
        // Arrange
        var custId = 123;
        var fromDate = new DateTime(2023, 1, 1);
        var toDate = new DateTime(2023, 1, 10);
        var balanceUpDay = new DateTime(2023, 1, 5);

        var param = new GetStatementParam(
            CustId: custId,
            FromDate: fromDate,
            ToDate: toDate,
            SiteName: siteName);

        var timeRanges = new List<TimeRange>
        {
            new TimeRange(new DateTime(2023, 1, 1), new DateTime(2023, 1, 5)),
            new TimeRange(new DateTime(2023, 1, 6), new DateTime(2023, 1, 10))
        };

        reportDateServiceMock
            .Setup(x => x.GetTimeRangesToGetReportDataBetweenTables(fromDate, toDate))
            .ReturnsAsync(timeRanges);

        reportDateServiceMock
            .Setup(x => x.GetBalanceUpDay())
            .ReturnsAsync(balanceUpDay);

        reportDateServiceMock
            .Setup(x => x.GetStoreMappingSuffix(It.IsAny<DateTime>()))
            .ReturnsAsync("_Warehouse");

        var statements = new List<StatementModel>();
        repositoryMock
            .Setup(x => x.FetchAsync<StatementModel>(It.IsAny<GetStatementCriteria>(), string.Empty))
            .ReturnsAsync(statements);

        // Act
        await service.GetStatements(param);

        // Assert
        partialUMServiceMock.Verify(x => x.ThrowIfUM(It.IsAny<string>(), siteName), Times.Exactly(2));
    }

    [Fact]
    public async Task StatementDetails_ShouldUseWarehouseSuffix_WhenTransferDateBeforeBalanceUpDay()
    {
        // Arrange
        var custId = 123;
        var transferDate = new DateTime(2023, 1, 1);
        var balanceUpDay = new DateTime(2023, 1, 5);

        reportDateServiceMock
            .Setup(x => x.GetBalanceUpDay())
            .ReturnsAsync(balanceUpDay);

        var statements = new List<StatementDetailModel>
        {
            new StatementDetailModel { Amount = 100 },
            new StatementDetailModel { Amount = 200 }
        };

        repositoryMock
            .Setup(x => x.FetchAsync<StatementDetailModel>(It.Is<TransferDetailsCriteria>(
                c => c.StoreMappingSuffix == "_WH"), string.Empty))
            .ReturnsAsync(statements)
            .Verifiable();

        // Act
        var result = await service.StatementDetails(custId, transferDate);

        // Assert
        repositoryMock.Verify();
        Assert.Equal(300, result.Total.Amount);
        Assert.Equal(2, result.TotalRecords);
    }

    [Fact]
    public async Task StatementDetails_ShouldUseEmptySuffix_WhenTransferDateAfterBalanceUpDay()
    {
        // Arrange
        var custId = 123;
        var transferDate = new DateTime(2023, 1, 10);
        var balanceUpDay = new DateTime(2023, 1, 5);

        reportDateServiceMock
            .Setup(x => x.GetBalanceUpDay())
            .ReturnsAsync(balanceUpDay);

        var statements = new List<StatementDetailModel>
        {
            new StatementDetailModel { Amount = 300 },
            new StatementDetailModel { Amount = 400 }
        };

        repositoryMock
            .Setup(x => x.FetchAsync<StatementDetailModel>(It.Is<TransferDetailsCriteria>(
                c => c.StoreMappingSuffix == string.Empty), string.Empty))
            .ReturnsAsync(statements)
            .Verifiable();

        // Act
        var result = await service.StatementDetails(custId, transferDate);

        // Assert
        repositoryMock.Verify();
        Assert.Equal(700, result.Total.Amount);
        Assert.Equal(2, result.DataSource.Count());
    }

    [Fact]
    public async Task StatementDetails_ShouldCalculateTotalAmount_FromStatementDetails()
    {
        // Arrange
        var custId = 123;
        var transferDate = new DateTime(2023, 1, 1);
        var balanceUpDay = new DateTime(2023, 1, 5);

        reportDateServiceMock
            .Setup(x => x.GetBalanceUpDay())
            .ReturnsAsync(balanceUpDay);

        var statements = new List<StatementDetailModel>
        {
            new StatementDetailModel { Amount = 100 },
            new StatementDetailModel { Amount = 200 },
            new StatementDetailModel { Amount = -50 }
        };

        repositoryMock
            .Setup(x => x.FetchAsync<StatementDetailModel>(It.IsAny<TransferDetailsCriteria>(), string.Empty))
            .ReturnsAsync(statements);

        // Act
        var result = await service.StatementDetails(custId, transferDate);

        // Assert
        Assert.Equal(250, result.Total.Amount);
    }

    [Fact]
    public async Task StatementDetails_ShouldHandleNullAmounts_WhenCalculatingTotal()
    {
        // Arrange
        var custId = 123;
        var transferDate = new DateTime(2023, 1, 1);
        var balanceUpDay = new DateTime(2023, 1, 5);

        reportDateServiceMock
            .Setup(x => x.GetBalanceUpDay())
            .ReturnsAsync(balanceUpDay);

        var statements = new List<StatementDetailModel>
        {
            new StatementDetailModel { Amount = 100 },
            new StatementDetailModel { Amount = null },
            new StatementDetailModel { Amount = 200 }
        };

        repositoryMock
            .Setup(x => x.FetchAsync<StatementDetailModel>(It.IsAny<TransferDetailsCriteria>(), string.Empty))
            .ReturnsAsync(statements);

        // Act
        var result = await service.StatementDetails(custId, transferDate);

        // Assert
        Assert.Equal(300, result.Total.Amount);
    }
}