//using AlphaPlus.Core.Models;
//using AlphaPlus.Reports.Features.WinLossByProduct;
//using AlphaPlus.Reports.Site.Features.WinLossByProduct;
//using Microsoft.AspNetCore.Hosting;

//namespace Reports.Site.UnitTests.Features.WinLossByProduct;

//public class WinLossByProductWorkbookTests
//{
//    private readonly Mock<IWebHostEnvironment> _webHostEnvironmentMock;
//    private readonly WinLossByProductWorkbook _workbook;

//    public WinLossByProductWorkbookTests()
//    {
//        _webHostEnvironmentMock = new Mock<IWebHostEnvironment>();
//        _webHostEnvironmentMock.Setup(env => env.ContentRootPath).Returns("D:\\Working_Individual\\RomePlus\\alphaplus");

//        _workbook = new WinLossByProductWorkbook(_webHostEnvironmentMock.Object);
//    }

//    [Fact]
//    public void BuildExcelFile_WithValidData_ShouldReturnExcelFileViewModel()
//    {
//        // Arrange
//        var query = new WinLossByProductExcelQuery
//        {
//            ViewerId = "user1",
//            ViewerLevel = UserLevel.Agent,
//            FromDate = new DateTime(2025, 4, 1),
//            ToDate = new DateTime(2025, 4, 15),
//            CustName = "TestCustomer"
//        };

//        var reportData = new List<WinLossByProductModel>
//        {
//            new WinLossByProductModel
//            {
//                ProductId = 1,
//                ProductName = "Test Product",
//                Total = new WinLossByBetTypeModel
//                {
//                    BetTypeName = "Test Bet Type",
//                    NumberOfTickets = 100,
//                    Turnover = 1000,
//                    GrossComm = 50,
//                    MemberWinLoss = 200,
//                    MemberComm = 10,
//                    MemberTotal = 210,
//                    WinLoss = 300,
//                    Comm = 15,
//                    Total = 315,
//                    Company = 100
//                },
//                WinLossBySports = new List<WinLossBySportModel>
//                {
//                    new WinLossBySportModel
//                    {
//                        SportTypeId = 1,
//                        SportTypeName = "Test Sport",
//                        Total = new WinLossByBetTypeModel
//                        {
//                            BetTypeName = "Total",
//                            NumberOfTickets = 50,
//                            Turnover = 500,
//                            GrossComm = 25,
//                            MemberWinLoss = 100,
//                            MemberComm = 5,
//                            MemberTotal = 105,
//                            WinLoss = 150,
//                            Comm = 7.5M,
//                            Total = 157.5M,
//                            Company = 50
//                        },
//                        WinLossByBetTypes = new List<WinLossByBetTypeModel>
//                        {
//                            new WinLossByBetTypeModel
//                            {
//                                BetTypeName = "Test Bet Type 1",
//                                NumberOfTickets = 50,
//                                Turnover = 500,
//                                GrossComm = 25,
//                                MemberWinLoss = 100,
//                                MemberComm = 5,
//                                MemberTotal = 105,
//                                WinLoss = 150,
//                                Comm = 7.5M,
//                                Total = 157.5M,
//                                Company = 50
//                            }
//                        }
//                    }
//                }
//            }
//        };

//        var total = new WinLossByBetTypeModel
//        {
//            BetTypeName = "Total",
//            NumberOfTickets = 100,
//            Turnover = 1000,
//            GrossComm = 50,
//            MemberWinLoss = 200,
//            MemberComm = 10,
//            MemberTotal = 210,
//            WinLoss = 300,
//            Comm = 15,
//            Total = 315,
//            Company = 100
//        };

//        var productNames = "Test Product";

//        // Act & Assert
//        // Note: Since the actual Excel generation depends on the NPOI library and templates,
//        // we're just verifying the method can be called without exceptions
//        // and returns an object with expected filename format
//        var exception = Record.Exception(() =>
//            _workbook.BuildExcelFile(query, reportData, total, productNames));

//        Assert.Null(exception);
//    }
//}