using AlphaPlus.CashService.API.Features.Promotion.Criteria;
using AlphaPlus.CashService.API.Features.Promotion.Models;
using AlphaPlus.CashService.API.Features.Promotion.Requests;
using AlphaPlus.CashService.API.Features.Promotion.Services;
using Fanex.Data.Repository;
using Moq;
using Xunit;

namespace AlphaPlus.CashService.API.UnitTests.Features.Promotion.Services;

public class PromotionReportServiceTest
{
    private readonly Mock<IDynamicRepository> repositoryMock;
    private readonly PromotionReportService service;

    public PromotionReportServiceTest()
    {
        repositoryMock = new Mock<IDynamicRepository>();
        service = new PromotionReportService(repositoryMock.Object);
    }

    [Fact]
    public async Task GetMainReport_ShouldCreateCorrectCriteriaAndReturnRepositoryResult()
    {
        // Arrange
        var request = new GetPromotionReportRequest
        {
            CustId = 123,
            FromDate = new DateTime(2023, 1, 1),
            ToDate = new DateTime(2023, 12, 31),
            PromotionType = 2,
            Status = true
        };

        var expectedReports = new List<PromotionReport>
        {
            new() { PromotionId = 1, PromotionTitle = "Promotion 1" },
            new() { PromotionId = 2, PromotionTitle = "Promotion 2" }
        };

        repositoryMock.Setup(r => r.FetchAsync<PromotionReport>(It.IsAny<ICriteria>(), string.Empty, default))
            .ReturnsAsync(expectedReports)
            .Callback<ICriteria, string, CancellationToken>((criteria, s, t) =>
            {
                var c = criteria as GetPromotionReportCriteria;
                Assert.Equal(request.CustId, c.CustId);
                Assert.Equal(request.FromDate, c.FromDate);
                Assert.Equal(request.ToDate, c.ToDate);
                Assert.Equal(request.PromotionType, c.PromotionType);
                Assert.Equal(request.Status, c.StatusId);
            });

        // Act
        var result = await service.GetMainReport(request);

        // Assert
        Assert.Same(expectedReports, result);
        repositoryMock.Verify(r => r.FetchAsync<PromotionReport>(It.IsAny<GetPromotionReportCriteria>(), string.Empty, default), Times.Once);
    }

    [Fact]
    public async Task GetDetailsReport_ShouldCreateCorrectCriteriaAndReturnFormattedReport()
    {
        // Arrange
        int promotionId = 42;
        var request = new GetPromotionDetailsReportRequest
        {
            CustId = 123,
            FromDate = new DateTime(2023, 1, 1),
            ToDate = new DateTime(2023, 12, 31),
            Status = 1
        };

        var promotionDetails = new List<PromotionDetails>
        {
            new() { StatusId = 4, Amount = 100 },  // Approved
            new() { StatusId = 4, Amount = 150 },  // Approved
            new() { StatusId = 5, Amount = 50 },   // Rejected
            new() { StatusId = 1, Amount = 75 }    // Other status
        };

        repositoryMock.Setup(r => r.FetchAsync<PromotionDetails>(It.IsAny<ICriteria>(), string.Empty, default))
            .ReturnsAsync(promotionDetails)
            .Callback<ICriteria, string, CancellationToken>((criteria, s, t) =>
            {
                var c = criteria as GetPromotionDetailsReportCriteria;
                Assert.Equal(request.CustId, c.CustId);
                Assert.Equal(promotionId, c.PromotionId);
                Assert.Equal(request.FromDate, c.FromDate);
                Assert.Equal(request.ToDate, c.ToDate);
                Assert.Equal(request.Status, c.StatusId);
            });

        // Act
        var result = await service.GetDetailsReport(promotionId, request);

        // Assert
        Assert.NotNull(result);
        Assert.NotNull(result.Summary);
        Assert.NotNull(result.PromotionDetails);
        Assert.Equal(250m, result.Summary.TotalBonus);
        Assert.Equal(2, result.Summary.TotalApproved);
        Assert.Equal(1, result.Summary.TotalRejected);
        Assert.Same(promotionDetails, result.PromotionDetails);
        repositoryMock.Verify(r => r.FetchAsync<PromotionDetails>(It.IsAny<GetPromotionDetailsReportCriteria>(), string.Empty, default), Times.Once);
    }

    [Fact]
    public async Task GetDetailsReport_ShouldReturnZeroSummaryValues_WhenNoDetailsFound()
    {
        // Arrange
        int promotionId = 42;
        var request = new GetPromotionDetailsReportRequest
        {
            CustId = 123,
            FromDate = new DateTime(2023, 1, 1),
            ToDate = new DateTime(2023, 12, 31),
            Status = 1
        };

        var emptyDetails = new List<PromotionDetails>();

        repositoryMock.Setup(r => r.FetchAsync<PromotionDetails>(It.IsAny<GetPromotionDetailsReportCriteria>(), string.Empty, default))
            .ReturnsAsync(emptyDetails);

        // Act
        var result = await service.GetDetailsReport(promotionId, request);

        // Assert
        Assert.NotNull(result);
        Assert.NotNull(result.Summary);
        Assert.Equal(0m, result.Summary.TotalBonus);
        Assert.Equal(0, result.Summary.TotalApproved);
        Assert.Equal(0, result.Summary.TotalRejected);
        Assert.Same(emptyDetails, result.PromotionDetails);
    }
}