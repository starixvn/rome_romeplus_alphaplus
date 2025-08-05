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
}
