using AlphaPlus.Reports.Features.WinLossDetail;
using AlphaPlus.Reports.Site.Features.Shared;
using AlphaPlus.Reports.Site.Features.WinLossDetail;
using Microsoft.AspNetCore.Hosting;

namespace Reports.Site.UnitTests.Features.WinLossDetail;

public class WinLossDetailHandlerTests
{
    private readonly Mock<IWinLossDetailService> _winLossDetailServiceMock;
    private readonly Mock<IWinLossDetailWorkbook> _workbookMock;
    private readonly WinLossDetailHandler _handler;

    public WinLossDetailHandlerTests()
    {
        _winLossDetailServiceMock = new Mock<IWinLossDetailService>();
        _workbookMock = new Mock<IWinLossDetailWorkbook>();

        _handler = new WinLossDetailHandler(
            _winLossDetailServiceMock.Object,
            _workbookMock.Object);
    }

    [Fact]
    public async Task Handle_WinLossDetailQuery_ShouldReturnExcelFile()
    {
        // Arrange
        var query = new WinLossDetailQuery
        {
            ViewerId = "user1",
            ViewerLevel = 1,
            FromDate = DateTime.Now.AddDays(-7),
            ToDate = DateTime.Now,
            CustomerId = "customer1",
            CustomerName = "Customer One"
        };

        var reportData = new List<WinLossDetailModel>
        {
            new WinLossDetailModel
            {
                TicketId = "ticket1",
                PlacedDate = DateTime.Now,
                // Additional properties would be set here
            }
        };

        var excelFileContent = new byte[] { 1, 2, 3 };

        _winLossDetailServiceMock.Setup(s => s.GetWinLossDetail(It.IsAny<WinLossDetailParam>()))
            .ReturnsAsync(reportData);

        _workbookMock.Setup(w => w.BuildExcelFile(
            It.IsAny<WinLossDetailQuery>(),
            It.IsAny<IEnumerable<WinLossDetailModel>>()))
            .Returns(new ExcelFileViewModel("filename", excelFileContent));

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(excelFileContent, result.FileContents);

        _winLossDetailServiceMock.Verify(
            s => s.GetWinLossDetail(It.Is<WinLossDetailParam>(p =>
                p.ViewerId == query.ViewerId &&
                p.ViewerLevel == query.ViewerLevel &&
                p.CustomerId == query.CustomerId)),
            Times.Once);

        _workbookMock.Verify(
            w => w.BuildExcelFile(
                query,
                It.IsAny<IEnumerable<WinLossDetailModel>>()),
            Times.Once);
    }

    [Fact]
    public async Task Handle_WhenServiceReturnsNoData_ShouldStillReturnExcelFile()
    {
        // Arrange
        var query = new WinLossDetailQuery
        {
            ViewerId = "user1",
            ViewerLevel = 1,
            FromDate = DateTime.Now.AddDays(-7),
            ToDate = DateTime.Now,
            CustomerId = "customer1",
            CustomerName = "Customer One"
        };

        var emptyReportData = new List<WinLossDetailModel>();
        var excelFileContent = new byte[] { 1, 2, 3 };

        _winLossDetailServiceMock.Setup(s => s.GetWinLossDetail(It.IsAny<WinLossDetailParam>()))
            .ReturnsAsync(emptyReportData);

        _workbookMock.Setup(w => w.BuildExcelFile(
            It.IsAny<WinLossDetailQuery>(),
            It.IsAny<IEnumerable<WinLossDetailModel>>()))
            .Returns(new ExcelFileViewModel("filename", excelFileContent));

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(excelFileContent, result.FileContents);
    }
}