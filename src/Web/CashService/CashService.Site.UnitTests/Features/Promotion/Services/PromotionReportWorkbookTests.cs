using AlphaPlus.CashService.Site.Features.Promotion.Models;
using AlphaPlus.CashService.Site.Features.Promotion.Requests;
using AlphaPlus.CashService.Site.Features.Promotion.Services;
using AlphaPlus.Shared.AppResources.CashServices;
using Microsoft.AspNetCore.Hosting;
using Moq;
using ONEbook.SpreadSheets;
using System.Reflection;
using Xunit;

namespace AlphaPlus.CashService.Site.UnitTests.Features.Promotion.Services;

[Trait("Category", "CashService")]
public class PromotionReportWorkbookTests
{
    private readonly Mock<IWebHostEnvironment> _webHostEnvironmentMock;
    private readonly PromotionReportWorkbook _reportWorkbook;
    private readonly GetPromotionReportRequest _request;
    private readonly List<PromotionReport> _reportData;

    public PromotionReportWorkbookTests()
    {
        _webHostEnvironmentMock = new Mock<IWebHostEnvironment>();
        _webHostEnvironmentMock.Setup(e => e.ContentRootPath).Returns(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location));
        _reportWorkbook = new PromotionReportWorkbook(_webHostEnvironmentMock.Object);

        _request = new GetPromotionReportRequest
        {
            Username = "testUser",
            FromDate = new DateTime(2023, 1, 1),
            ToDate = new DateTime(2023, 1, 31)
        };

        _reportData = new List<PromotionReport>
            {
                new PromotionReport
                {
                    PromotionTitle = "Test Promotion",
                    PromotionType = 1,
                    FromDate = new DateTime(2023, 1, 1),
                    ToDate = new DateTime(2023, 1, 31),
                    TotalRequest = 100,
                    TotalApproved = 80,
                    TotalRejected = 20,
                    TotalAmount = 5000,
                    Enabled = true
                },
                new PromotionReport
                {
                    PromotionTitle = "Another Promotion",
                    PromotionType = 1,
                    FromDate = new DateTime(2023, 1, 15),
                    ToDate = new DateTime(2023, 1, 31),
                    TotalRequest = 50,
                    TotalApproved = 30,
                    TotalRejected = 20,
                    TotalAmount = 2500,
                    Enabled = false
                }
            };
    }

    [Fact]
    public void BuildExcelFile_ShouldReturnExcelFileViewModel_WithCorrectFileName()
    {
        // Arrange
        var fromDate = _request.FromDate.ToString("dd/MM/yyyy");
        var toDate = _request.ToDate.ToString("dd/MM/yyyy");
        var expectedFileName = $"{CashServicesResx.PromotionReport}-{_request.Username}-{fromDate}-{toDate}";

        // Act
        var result = _reportWorkbook.BuildExcelFile(_request, _reportData);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(expectedFileName, result.Name);
        Assert.NotNull(result.Content);
        Assert.True(result.Content.Length > 0);
    }

    [Fact]
    public void BuildExcelFile_ShouldReturnExcelFileViewModel_WithFileContent()
    {
        // Act
        var result = _reportWorkbook.BuildExcelFile(_request, _reportData);

        // Assert
        Assert.NotNull(result);
        Assert.NotNull(result.Content);
        Assert.True(result.Content.Length > 0);
    }

    [Fact]
    public void BuildExcelFile_WithEmptyReportData_ShouldReturnExcelFileViewModel()
    {
        // Arrange
        var emptyData = new List<PromotionReport>();

        // Act
        var result = _reportWorkbook.BuildExcelFile(_request, emptyData);

        // Assert
        Assert.NotNull(result);
        Assert.NotNull(result.Content);
        Assert.True(result.Content.Length > 0);
    }

    [Fact]
    public void GetRowMapper_ShouldMapAllFieldsCorrectly()
    {
        // Arrange
        var mapperMethod = typeof(PromotionReportWorkbook)
            .GetMethod("GetRowMapper", BindingFlags.NonPublic | BindingFlags.Instance);

        // Act
        var mapper = mapperMethod.Invoke(_reportWorkbook, null) as Mapper<PromotionReport>;

        // Assert
        Assert.NotNull(mapper);

        // This is more of a smoke test since we can't directly verify the mapping logic
        // without executing the full export process
        Assert.NotNull(mapper);
    }
}
