//using AlphaPlus.Reports.Features.WinLossDetail;
//using AlphaPlus.Reports.Site.Features.WinLossDetail;
//using Microsoft.AspNetCore.Hosting;

//namespace Reports.Site.UnitTests.Features.WinLossDetail;

//public class WinLossDetailWorkbookTests
//{
//    private readonly Mock<IWebHostEnvironment> _webHostEnvironmentMock;
//    private readonly WinLossDetailWorkbook _workbook;

//    public WinLossDetailWorkbookTests()
//    {
//        _webHostEnvironmentMock = new Mock<IWebHostEnvironment>();
//        _webHostEnvironmentMock.Setup(env => env.ContentRootPath).Returns("D:\\Working_Individual\\RomePlus\\alphaplus");

//        _workbook = new WinLossDetailWorkbook(_webHostEnvironmentMock.Object);
//    }

//    [Fact]
//    public void BuildExcelFile_WithValidData_ShouldNotThrowException()
//    {
//        // Arrange
//        var query = new WinLossDetailQuery
//        {
//            ViewerId = "user1",
//            ViewerLevel = 1,
//            FromDate = new DateTime(2025, 4, 1),
//            ToDate = new DateTime(2025, 4, 15),
//            CustomerId = "customer1",
//            CustomerName = "Customer One"
//        };

//        var reportData = new List<WinLossDetailModel>
//        {
//            new WinLossDetailModel
//            {
//                TicketId = "ticket1",
//                PlacedDate = DateTime.Now,
//                SettledDate = DateTime.Now.AddHours(2),
//                CustId = "customer1",
//                CustomerName = "Customer One",
//                Stake = 100,
//                WinLoss = 50,
//                ValidTurnover = 100,
//                Status = 1
//            },
//            new WinLossDetailModel
//            {
//                TicketId = "ticket2",
//                PlacedDate = DateTime.Now.AddDays(-1),
//                SettledDate = DateTime.Now.AddDays(-1).AddHours(3),
//                CustId = "customer1",
//                CustomerName = "Customer One",
//                Stake = 200,
//                WinLoss = -50,
//                ValidTurnover = 200,
//                Status = 1
//            }
//        };

//        // Act & Assert
//        // Since the actual Excel generation depends on NPOI and templates,
//        // we're just checking that the method doesn't throw an exception
//        var exception = Record.Exception(() =>
//            _workbook.BuildExcelFile(query, reportData));

//        Assert.Null(exception);
//    }

//    [Fact]
//    public void BuildExcelFile_WithNoData_ShouldNotThrowException()
//    {
//        // Arrange
//        var query = new WinLossDetailQuery
//        {
//            ViewerId = "user1",
//            ViewerLevel = 1,
//            FromDate = new DateTime(2025, 4, 1),
//            ToDate = new DateTime(2025, 4, 15),
//            CustomerId = "customer1",
//            CustomerName = "Customer One"
//        };

//        var reportData = new List<WinLossDetailModel>();

//        // Act & Assert
//        // Since the actual Excel generation depends on NPOI and templates,
//        // we're just checking that the method doesn't throw an exception
//        var exception = Record.Exception(() =>
//            _workbook.BuildExcelFile(query, reportData));

//        Assert.Null(exception);
//    }
//}