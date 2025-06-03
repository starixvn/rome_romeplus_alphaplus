using System.Net;
using AlphaPlus.Core.Models;
using AlphaPlus.Reports.Site.Features.Shared.Requests;
using AlphaPlus.Reports.Site.UnitTests;
using AlphaPlus.Shared.AppResources.Report;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace AlphaPlus.Reports.Site.Features.Shared.Tests
{
    [Trait("Category", "Shared")]
    public class ReportBaseControllerTest
    {
        private readonly Mock<IMediator> _mediator;
        private readonly TestReportController _controller;

        public ReportBaseControllerTest()
        {
            _mediator = new Mock<IMediator>();
            _controller = new TestReportController(_mediator.Object);
        }

        [Fact]
        public async Task View_ValidQuery_ReturnsViewWithResults()
        {
            // Arrange
            var userSessionMock = GlobalMockUserSession.CreateMockUserSessionDefault();
            var query = new TestValidQuery(userSessionMock.Object);
            var expectedResult = "TestResult";
            _mediator.Setup(m => m.Send(query, CancellationToken.None)).ReturnsAsync(expectedResult);

            // Act
            var result = await _controller.TestView(query) as ViewResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal(expectedResult, result.Model);
            Assert.Null(result.ViewName);
        }

        [Fact]
        public async Task View_InvalidQuery_ReturnsInvalidReportView()
        {
            // Arrange
            var mockUserSession = new Mock<IUserSession>();
            mockUserSession.Setup(x => x.CustId).Returns(-1);
            mockUserSession.Setup(x => x.RoleId).Returns(4);

            var query = new TestInvalidQuery(mockUserSession.Object);

            // Act
            var result = await _controller.TestView(query) as ViewResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal("InvalidReport", result.ViewName);
        }

        [Fact]
        public async Task View_WithCustomViewName_ReturnsCustomView()
        {
            // Arrange
            var userSessionMock = GlobalMockUserSession.CreateMockUserSessionDefault();
            var query = new TestValidQuery(userSessionMock.Object);
            var expectedResult = "TestResult";
            _mediator.Setup(m => m.Send(query, CancellationToken.None)).ReturnsAsync(expectedResult);
            var customViewName = "CustomView";

            // Act
            var result = await _controller.TestViewWithCustomName(query, customViewName) as ViewResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal(customViewName, result.ViewName);
            Assert.Equal(expectedResult, result.Model);
        }

        [Fact]
        public async Task Json_ValidQuery_ReturnsJsonResult()
        {
            // Arrange
            var userSessionMock = GlobalMockUserSession.CreateMockUserSessionDefault();
            var query = new TestValidQuery(userSessionMock.Object);
            var expectedResult = "TestResult";
            _mediator.Setup(m => m.Send(query, CancellationToken.None)).ReturnsAsync(expectedResult);

            // Act
            var result = await _controller.TestJson(query) as JsonResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal(expectedResult, result.Value);
        }

        [Fact]
        public async Task Json_InvalidQuery_ReturnsForbiddenResult()
        {
            // Arrange
            var mockUserSession = new Mock<IUserSession>();
            mockUserSession.Setup(x => x.CustId).Returns(-1);
            mockUserSession.Setup(x => x.RoleId).Returns(4);

            var query = new TestInvalidQuery(mockUserSession.Object);

            // Act
            var result = await _controller.TestJson(query) as JsonResult;
            var forbiddenResult = result.Value as Reports.Features.Shared.Models.Result;

            // Assert
            Assert.NotNull(result);
            Assert.NotNull(forbiddenResult);
            Assert.Equal((int)HttpStatusCode.Forbidden, forbiddenResult.Code);
            Assert.Equal(Resources.NotAuthorized, forbiddenResult.Message);
        }

        [Fact]
        public async Task ExcelView_ValidQuery_ReturnsFileContentResult()
        {
            // Arrange
            var userSessionMock = GlobalMockUserSession.CreateMockUserSessionDefault();
            var query = new TestValidExcelQuery(userSessionMock.Object);
            var excelContent = new byte[] { 1, 2, 3 };
            var fileName = "test";
            var contentType = "application/vnd.ms-excel";
            var excelViewModel = new ExcelFileViewModel(fileName, excelContent);

            _mediator.Setup(m => m.Send(query, CancellationToken.None)).ReturnsAsync(excelViewModel);

            // Act
            var result = await _controller.TestExcel(query) as FileContentResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal(excelContent, result.FileContents);
            Assert.Equal(contentType, result.ContentType);
            Assert.Equal($"{fileName}.xls", result.FileDownloadName);
        }

        [Fact]
        public async Task ExcelView_InvalidQuery_ReturnsEmptyExcelFile()
        {
            // Arrange
            var mockUserSession = new Mock<IUserSession>();
            mockUserSession.Setup(x => x.CustId).Returns(-1);
            mockUserSession.Setup(x => x.RoleId).Returns(4);

            var query = new TestInvalidExcelQuery(mockUserSession.Object);

            // Act
            var result = await _controller.TestExcel(query) as FileContentResult;

            // Assert
            Assert.NotNull(result);
            Assert.Empty(result.FileContents);
            Assert.Equal("InvalidReport.xls", result.FileDownloadName);
        }

        // Test helper classes
        private class TestReportController : ReportBaseController
        {
            public TestReportController(IMediator mediator) : base(mediator) { }

            public async Task<IActionResult> TestView(IRequest<string> query)
            {
                return await View(query);
            }

            public async Task<IActionResult> TestViewWithCustomName(IRequest<string> query, string viewName)
            {
                return await View(query, viewName);
            }

            public async Task<IActionResult> TestJson(IRequest<string> query)
            {
                return await Json(query);
            }

            public async Task<IActionResult> TestExcel(IRequest<ExcelFileViewModel> query)
            {
                return await ExcelView(query);
            }
        }

        private class TestValidQuery : BaseQuery, IRequest<string>
        {
            public TestValidQuery(IUserSession viewer) : base(viewer) { }
        }

        private class TestInvalidQuery : BaseQuery, IRequest<string>
        {
            public TestInvalidQuery(IUserSession viewer) : base(viewer) { }
        }

        private class TestValidExcelQuery : BaseQuery, IRequest<ExcelFileViewModel>
        {
            public TestValidExcelQuery(IUserSession viewer) : base(viewer) { }
        }

        private class TestInvalidExcelQuery : BaseQuery, IRequest<ExcelFileViewModel>
        {
            public TestInvalidExcelQuery(IUserSession viewer) : base(viewer) { }
        }
    }
}