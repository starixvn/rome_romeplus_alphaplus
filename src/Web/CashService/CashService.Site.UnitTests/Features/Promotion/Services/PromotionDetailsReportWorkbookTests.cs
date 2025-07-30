using AlphaPlus.CashService.Site.Features.Promotion.Models;
using AlphaPlus.CashService.Site.Features.Promotion.Requests;
using AlphaPlus.CashService.Site.Features.Promotion.Services;
using AlphaPlus.CashService.Site.Features.Transactions.Enum;
using AlphaPlus.Shared.AppResources.CashServices;
using Microsoft.AspNetCore.Hosting;
using Moq;
using Xunit;

namespace AlphaPlus.CashService.Site.UnitTests.Features.Promotion.Services;

[Trait("Category", "CashService")]
public class PromotionDetailsReportWorkbookTest
{
    private readonly Mock<IWebHostEnvironment> _webHostEnvironmentMock;
    private readonly PromotionDetailsReportWorkbook _workbook;

    public PromotionDetailsReportWorkbookTest()
    {
        _webHostEnvironmentMock = new Mock<IWebHostEnvironment>();
        _workbook = new PromotionDetailsReportWorkbook(_webHostEnvironmentMock.Object);
    }

    [Fact]
    public void BuildExcelFile_Should_Return_Valid_ExcelFileViewModel()
    {
        // Arrange
        var request = new GetPromotionDetailsReportRequest
        {
            PromotionTitle = "Test Promotion",
            Username = "testuser",
            FromDate = new DateTime(2023, 1, 1),
            ToDate = new DateTime(2023, 1, 31)
        };

        var reportData = new PromotionDetailsReport
        (
            new PromotionDetailsSummary
            {
                TotalAmount = 1000,
                TotalApproved = 10,
                TotalRejected = 2
            },
            new List<PromotionDetails>
            {
                new PromotionDetails
                {
                    Username = "user1",
                    Nickname = "nick1",
                    FirstName = "First",
                    LastName = "Last",
                    FromDate = new DateTime(2023, 1, 1),
                    ToDate = new DateTime(2023, 1, 31),
                    Amount = 100,
                    OperatedBy = "admin",
                    SubAccName = "",
                    OperatedTime = DateTime.Now,
                    StatusId = (byte)TransactionStatus.Completed
                },
                new PromotionDetails
                {
                    Username = "user2",
                    Nickname = "",
                    FirstName = "",
                    LastName = "Last",
                    FromDate = new DateTime(2023, 1, 1),
                    ToDate = new DateTime(2023, 1, 31),
                    Amount = 200,
                    OperatedBy = "admin",
                    SubAccName = "subadmin",
                    OperatedTime = DateTime.Now,
                    StatusId = (byte)TransactionStatus.Pending
                }
            }
        );

        // Mock the template handling
        var templatePath = "path/to/template";
        _webHostEnvironmentMock.Setup(w => w.ContentRootPath)
                .Returns("root/path");

        // Use reflection to test private method BuildTextBreakline
        var buildTextBreaklineMethod = typeof(PromotionDetailsReportWorkbook).GetMethod(
            "BuildTextBreakline",
            System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Static);

        // Act
        var result = _workbook.BuildExcelFile(request, reportData);

        // Assert
        Assert.NotNull(result);
        Assert.Equal($"{CashServicesResx.PromotionReport}-testuser-01/01/2023-31/01/2023", result.Name);
        Assert.NotNull(result.Content);
    }

    [Theory]
    [InlineData("user1", "nick1", "user1\nnick1")]
    [InlineData("user1", "", "user1")]
    [InlineData("", "nick1", "nick1")]
    [InlineData("", "", "")]
    public void BuildTextBreakline_Should_Format_Text_Correctly(string text1, string text2, string expected)
    {
        // Arrange
        var methodInfo = typeof(PromotionDetailsReportWorkbook).GetMethod(
            "BuildTextBreakline",
            System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Static);

        // Act
        var result = methodInfo.Invoke(null, new object[] { text1, text2 }) as string;

        // Assert
        Assert.Equal(expected, result);
    }

    // [Fact]
    // public void GetRowMapper_Should_Map_Status_Correctly()
    // {
    //     // Arrange
    //     var mapper = typeof(PromotionDetailsReportWorkbook)
    //         .GetMethod("GetRowMapper", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
    //         .Invoke(_workbook, Array.Empty<object>()) as Mapper<PromotionDetails>;

    //     var completedDetails = new PromotionDetails { StatusId = (byte)TransactionStatus.Completed };
    //     var pendingDetails = new PromotionDetails { StatusId = (byte)TransactionStatus.Pending };
    //     var rejectedDetails = new PromotionDetails { StatusId = (byte)TransactionStatus.Rejected };
    //     var unknownDetails = new PromotionDetails { StatusId = 100 };

    //     // Act & Assert
    //     Assert.Equal(CashServicesResx.Approved, mapper.GetCellValue(Column.H, completedDetails));
    //     Assert.Equal(Resources.Pending, mapper.GetCellValue(Column.H, pendingDetails));
    //     Assert.Equal(Resources.Rejected, mapper.GetCellValue(Column.H, rejectedDetails));
    //     Assert.Equal(string.Empty, mapper.GetCellValue(Column.H, unknownDetails));
    // }

    // [Fact]
    // public void GetRowMapper_Should_Map_OperatedBy_And_SubAccName_Correctly()
    // {
    //     // Arrange
    //     var mapper = typeof(PromotionDetailsReportWorkbook)
    //         .GetMethod("GetRowMapper", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
    //         .Invoke(_workbook, Array.Empty<object>()) as Mapper<PromotionDetails>;

    //     var detailsWithSubAcc = new PromotionDetails { OperatedBy = "admin", SubAccName = "subadmin" };
    //     var detailsWithoutSubAcc = new PromotionDetails { OperatedBy = "admin", SubAccName = "" };

    //     // Act & Assert
    //     Assert.Equal("subadmin", mapper.GetCellValue(Column.F, detailsWithSubAcc));
    //     Assert.Equal("admin", mapper.GetCellValue(Column.F, detailsWithoutSubAcc));
    // }
}