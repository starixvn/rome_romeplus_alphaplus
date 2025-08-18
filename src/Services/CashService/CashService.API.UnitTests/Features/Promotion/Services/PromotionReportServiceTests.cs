using AlphaPlus.CashService.API.Features.Promotion.Criteria;
using AlphaPlus.CashService.API.Features.Promotion.Models;
using AlphaPlus.CashService.API.Features.Promotion.Requests;
using AlphaPlus.CashService.API.Features.Promotion.Services;
using Fanex.Data.Repository;
using Moq;
using Xunit;

namespace AlphaPlus.CashService.API.UnitTests.Features.Promotion.Services;

public class PromotionReportServiceTests
{
    private readonly Mock<IDynamicRepository> repositoryMock;
    private readonly IPromotionReportService service;

    public PromotionReportServiceTests()
    {
        repositoryMock = new Mock<IDynamicRepository>();
        service = new PromotionReportService(repositoryMock.Object);
    }

    [Fact]
    public async Task GetMainReport_ShouldCreateCorrectCriteria_AndReturnRepositoryResult()
    {
        // Arrange
        var request = new GetPromotionReportRequest
        {
            CustId = 123,
            FromDate = DateTime.Parse("2023-01-01"),
            ToDate = DateTime.Parse("2023-01-31"),
            PromotionType = 1,
            Status = null
        };

        var expectedReports = new List<PromotionReport>
            {
                new PromotionReport(),
                new PromotionReport()
            };

        GetPromotionReportCriteria capturedCriteria = null;
        repositoryMock.Setup(r => r.FetchAsync<PromotionReport>(It.IsAny<ICriteria>(), string.Empty, default))
            .Callback<ICriteria, string, CancellationToken>((c, s, t) => capturedCriteria = (GetPromotionReportCriteria)c)
            .ReturnsAsync(expectedReports);

        // Act
        var result = await service.GetMainReport(request);

        // Assert
        Assert.Equal(expectedReports, result);
        Assert.NotNull(capturedCriteria);
        Assert.Equal(request.CustId, capturedCriteria.CustId);
        Assert.Equal(request.FromDate, capturedCriteria.FromDate);
        Assert.Equal(request.ToDate, capturedCriteria.ToDate);
        Assert.Equal(request.PromotionType, capturedCriteria.PromotionType);
        Assert.Equal(request.Status, capturedCriteria.StatusId);
    }

    [Fact]
    public async Task GetDetailsReport_ShouldCreateCorrectCriteria_AndReturnPromotionDetailsReport()
    {
        // Arrange
        int promotionId = 42;
        var request = new GetPromotionDetailsReportRequest
        {
            CustId = 123,
            FromDate = DateTime.Parse("2023-01-01"),
            ToDate = DateTime.Parse("2023-01-31"),
            Status = 2
        };

        var details = new List<PromotionDetails>
            {
                new PromotionDetails(),
                new PromotionDetails()
            };

        var summary = new List<PromotionDetailsSummary>
            {
                new PromotionDetailsSummary()
            };

        GetPromotionDetailsReportCriteria capturedCriteria = null;
        repositoryMock.Setup(r => r.FetchMultipleAsync<PromotionDetails, PromotionDetailsSummary>(It.IsAny<ICriteria>(), string.Empty, default))
            .Callback<ICriteria, string, CancellationToken>((c, s, t) =>
            {
                capturedCriteria = (GetPromotionDetailsReportCriteria)c;
            })
            .ReturnsAsync((details, summary));

        // Act
        var result = await service.GetDetailsReport(promotionId, request);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(summary.FirstOrDefault(), result.Summary);
        Assert.Equal(details, result.PromotionDetails);
        Assert.NotNull(capturedCriteria);
        Assert.Equal(request.CustId, capturedCriteria.CustId);
        Assert.Equal(promotionId, capturedCriteria.PromotionId);
        Assert.Equal(request.FromDate, capturedCriteria.FromDate);
        Assert.Equal(request.ToDate, capturedCriteria.ToDate);
        Assert.Equal(request.Status, capturedCriteria.StatusId);
    }

    [Fact]
    public async Task GetDetailsReport_WithEmptySummaryResult_ShouldReturnReportWithNullSummary()
    {
        // Arrange
        int promotionId = 42;
        var request = new GetPromotionDetailsReportRequest { CustId = 123 };

        var details = new List<PromotionDetails> { new PromotionDetails() };
        var summary = new List<PromotionDetailsSummary>();

        repositoryMock.Setup(r => r.FetchMultipleAsync<PromotionDetails, PromotionDetailsSummary>(It.IsAny<ICriteria>(), string.Empty, default))
            .ReturnsAsync((details, summary));

        // Act
        var result = await service.GetDetailsReport(promotionId, request);

        // Assert
        Assert.NotNull(result);
        Assert.Null(result.Summary);
        Assert.Equal(details, result.PromotionDetails);
    }

    [Fact]
    public async Task GetManualBonus_Should_Call_Repository_With_Correct_Criteria()
    {
        // Arrange
        var custId = 123;
        var fromDate = new DateTime(2023, 1, 1);
        var toDate = new DateTime(2023, 1, 31);
        var request = new GetManualBonusRequest
        {
            CustId = custId,
            FromDate = fromDate,
            ToDate = toDate
        };

        var expectedCriteria = new GetManualBonusCriteria(custId, fromDate, toDate);
        var expectedResult = new List<ManualBonus> { new ManualBonus() };

        repositoryMock
            .Setup(r => r.FetchAsync<ManualBonus>(It.IsAny<ICriteria>(), string.Empty, default))
            .ReturnsAsync(expectedResult)
            .Callback<ICriteria, string, CancellationToken>((criteria, _, _) =>
            {
                var c = criteria as GetManualBonusCriteria;
                Assert.Equal(custId, c.CustId);
                Assert.Equal(fromDate, c.FromDate);
                Assert.Equal(toDate, c.ToDate);
            });

        // Act
        var result = await service.GetManualBonus(request);

        // Assert
        repositoryMock.Verify(r => r.FetchAsync<ManualBonus>(It.IsAny<GetManualBonusCriteria>(), string.Empty, default), Times.Once);
        Assert.Same(expectedResult, result);
    }

    [Fact]
    public async Task GetManualBonus_Should_Return_Empty_Collection_When_Repository_Returns_Null()
    {
        // Arrange
        var request = new GetManualBonusRequest
        {
            CustId = 123,
            FromDate = DateTime.Now,
            ToDate = DateTime.Now
        };

        repositoryMock
            .Setup(r => r.FetchAsync<ManualBonus>(It.IsAny<GetManualBonusCriteria>(), string.Empty, default))
            .ReturnsAsync((IEnumerable<ManualBonus>)null);

        // Act
        var result = await service.GetManualBonus(request);

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public async Task GetManualBonus_Should_Pass_Through_Exception_From_Repository()
    {
        // Arrange
        var request = new GetManualBonusRequest
        {
            CustId = 123,
            FromDate = DateTime.Now,
            ToDate = DateTime.Now
        };

        var expectedException = new Exception("Repository error");
        repositoryMock
            .Setup(r => r.FetchAsync<ManualBonus>(It.IsAny<GetManualBonusCriteria>(), string.Empty, default))
            .ThrowsAsync(expectedException);

        // Act & Assert
        var exception = await Assert.ThrowsAsync<Exception>(() => service.GetManualBonus(request));
        Assert.Same(expectedException, exception);
    }
}
