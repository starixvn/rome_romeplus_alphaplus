using AlphaPlus.Core.Models;
using AlphaPlus.Reports.Features.Shared.Models;
using AlphaPlus.Reports.Site.Features.Shared.Requests;
using AlphaPlus.Reports.Site.Features.Shared.ViewModels;
using AlphaPlus.Reports.Site.Features.Shared.ViewModels.Components;
using AlphaPlus.Reports.Site.UnitTests;

namespace AlphaPlus.Reports.Site.Features.Shared.Tests.ViewModels;

[Trait("Category", "Shared")]
public class DrillDownableViewModelTest
{
    private class TestDrillDownableViewModel : DrillDownableViewModel<object>
    {
        public TestDrillDownableViewModel(
            BaseQuery message,
            byte levelType,
            PageHeaderViewModel pageHeader,
            ReportData<object> reportData,
            string sortPropertyName,
            byte descendingSort,
            string betListPath = null)
            : base(message, levelType, pageHeader, reportData, sortPropertyName, descendingSort, betListPath)
        {
        }

        public BreadcrumbViewModel TestBuildBreadcrumbViewModel(BaseQuery message)
        {
            return BuildBreadcrumbViewModel(message);
        }

        protected override IDictionary<string, string> AddMoreExcelParams(IDictionary<string, string> queryParams)
        {
            queryParams["TestParam"] = "TestValue";
            return queryParams;
        }

        protected override IDictionary<string, string> AddMoreBetListParams(IDictionary<string, string> queryParams)
        {
            queryParams["TestParam"] = "TestValue";
            return queryParams;
        }
    }

    [Fact]
    public void Constructor_Should_Initialize_Properties_Correctly()
    {
        // Arrange
        var userSessionMock = GlobalMockUserSession.CreateMockUserSessionDefault();
        var message = new BaseQuery(userSessionMock.Object);
        var levelType = (byte)1;
        var pageHeader = new PageHeaderViewModel("Test");
        var reportData = new ReportData<object>(new List<object>(), 0);
        var sortPropertyName = "TestProperty";
        byte descendingSort = 1;
        var betListPath = "test/path";

        // Act
        var viewModel = new TestDrillDownableViewModel(message, levelType, pageHeader, reportData, sortPropertyName, descendingSort, betListPath);

        // Assert
        Assert.Equal(message.CustId, viewModel.CustId);
        Assert.Equal(message.CustName, viewModel.CustName);
        Assert.Equal(message.CustLevel, viewModel.CustLevel);
        Assert.Equal((byte)(message.CustLevel.Value - 1), viewModel.DownlineLevelId);
        Assert.Equal(levelType, viewModel.LevelType);
        Assert.True(viewModel.ShowLevelType);
        Assert.Equal(pageHeader, viewModel.PageHeader);
        Assert.Equal(reportData, viewModel.ReportData);
        Assert.Equal(sortPropertyName, viewModel.SortPropertyName);
        Assert.Equal(descendingSort, viewModel.DescendingSort);
    }

    [Fact]
    public void BuildExcelParams_Should_Return_Correct_Parameters()
    {
        // Arrange
        var userSessionMock = GlobalMockUserSession.CreateMockUserSessionDefault();
        var message = new BaseQuery(userSessionMock.Object);
        var viewModel = new TestDrillDownableViewModel(
            message,
            1,
            new PageHeaderViewModel("Test"),
            new ReportData<object>(new List<object>(), 0),
            "TestProperty",
            1);

        // Act
        var excelParams = viewModel.BuildExcelParams();

        // Assert
        Assert.Equal(5, excelParams.Count);
        Assert.Equal("123", excelParams["CustId"]);
        Assert.Equal("TestSuper", excelParams["CustName"]);
        Assert.Equal(message.CustLevel.Value.ToString(), excelParams["CustLevelId"]);
        Assert.Equal("1", excelParams["LevelType"]);
        Assert.Equal("TestValue", excelParams["TestParam"]);
    }

    [Fact]
    public void BuildBetListUrl_Should_Return_Correct_Url()
    {
        // Arrange
        var userSessionMock = GlobalMockUserSession.CreateMockUserSessionDefault();
        var message = new BaseQuery(userSessionMock.Object);
        var betListPath = "test/path";
        var viewModel = new TestDrillDownableViewModel(
            message,
            1,
            new PageHeaderViewModel("Test"),
            new ReportData<object>(new List<object>(), 0),
            "TestProperty",
            1,
            betListPath);

        // Act
        var url = viewModel.BuildBetListUrl(456, "DownlineCustomer");

        // Assert
        Assert.Contains("test/path", url);
        Assert.Contains("custId=456", url);
        Assert.Contains("custName=DownlineCustomer", url);
        Assert.Contains("TestParam=TestValue", url);
    }

    [Fact]
    public void BuildBreadcrumbViewModel_Should_Return_Valid_BreadcrumbViewModel()
    {
        // Arrange
        var userSessionMock = GlobalMockUserSession.CreateMockUserSessionDefault();
        var message = new BaseQuery(userSessionMock.Object);
        var viewModel = new TestDrillDownableViewModel(
            message,
            1,
            new PageHeaderViewModel("Test"),
            new ReportData<object>(new List<object>(), 0),
            "TestProperty",
            1);

        // Act
        var breadcrumb = viewModel.TestBuildBreadcrumbViewModel(message);

        // Assert
        Assert.NotNull(breadcrumb);
        Assert.NotNull(breadcrumb.BreadcrumbsJson);
    }
}