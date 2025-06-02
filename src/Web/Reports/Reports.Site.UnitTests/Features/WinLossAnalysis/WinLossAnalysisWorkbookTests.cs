//using AlphaPlus.Core.Models;
//using AlphaPlus.Reports.Features.WinLossAnalysis;
//using AlphaPlus.Reports.Site.Features.WinLossAnalysis;
//using Microsoft.AspNetCore.Hosting;

//namespace Reports.Site.UnitTests.Features.WinLossAnalysis;

//public class WinLossAnalysisWorkbookTests
//{
//    private readonly Mock<IWebHostEnvironment> _webHostEnvironmentMock;
//    private readonly WinLossAnalysisWorkbook _workbook;

//    public WinLossAnalysisWorkbookTests()
//    {
//        _webHostEnvironmentMock = new Mock<IWebHostEnvironment>();
//        _webHostEnvironmentMock.Setup(env => env.ContentRootPath).Returns("D:\\Working_Individual\\RomePlus\\alphaplus");

//        _workbook = new WinLossAnalysisWorkbook(_webHostEnvironmentMock.Object);
//    }

//    [Fact]
//    public void BuildExcelFile_WithValidData_ShouldNotThrowException()
//    {
//        // Arrange
//        var query = new WinLossAnalysisExcelQuery
//        {
//            ViewerId = "user1",
//            ViewerLevel = UserLevel.Agent,
//            FromDate = new DateTime(2025, 4, 1),
//            ToDate = new DateTime(2025, 4, 15),
//            CustName = "TestCustomer",
//            TopN = 10
//        };

//        var reportData = new List<WinLossAnalysisModel>
//        {
//            new WinLossAnalysisModel
//            {
//                CustomerId = "cust1",
//                CustomerName = "Customer 1",
//                MemberCount = 5,
//                Turnover = 1000,
//                GrossComm = 50,
//                ActualComm = 25,
//                FinancialComm = 30,
//                MemberWL = 200,
//                WinLoss = 300,
//                NetWL = 275,
//                CompanyWL = 175
//            },
//            new WinLossAnalysisModel
//            {
//                CustomerId = "cust2",
//                CustomerName = "Customer 2",
//                MemberCount = 3,
//                Turnover = 800,
//                GrossComm = 40,
//                ActualComm = 20,
//                FinancialComm = 25,
//                MemberWL = 150,
//                WinLoss = 200,
//                NetWL = 180,
//                CompanyWL = 120
//            }
//        };

//        var summary = new WinLossSummaryModel
//        {
//            MemberCount = 8,
//            Turnover = 1800,
//            GrossComm = 90,
//            ActualComm = 45,
//            FinancialComm = 55,
//            MemberWL = 350,
//            WinLoss = 500,
//            NetWL = 455,
//            CompanyWL = 295
//        };

//        var productNames = "All Products";

//        // Act & Assert
//        // Since the actual Excel generation depends on NPOI and templates,
//        // we're just checking that the method doesn't throw an exception
//        var exception = Record.Exception(() =>
//            _workbook.BuildExcelFile(query, reportData, summary, productNames));

//        Assert.Null(exception);
//    }
//}