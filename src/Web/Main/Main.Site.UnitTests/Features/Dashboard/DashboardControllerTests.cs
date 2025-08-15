using AlphaPlus.Core;
using AlphaPlus.Core.Configurations;
using AlphaPlus.Core.Models;
using AlphaPlus.Main.Site.Features.Dashboard;
using AlphaPlus.Main.Site.Features.Dashboard.BalanceInfo;
using AlphaPlus.Main.Site.Features.Dashboard.Banner;
using AlphaPlus.Main.Site.Features.Dashboard.CashService;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Moq;

namespace AlphaPlus.Main.Site.UnitTests.Features.Dashboard
{
    public class DashboardControllerTests
    {
        private readonly Mock<IConfiguration> _configurationMock;
        private readonly Mock<IMediator> _mediatorMock;
        private readonly Mock<IUserSession> _userSessionMock;
        private readonly Mock<IOptions<AuthorizationOptions>> _optionsMock;
        private readonly DashboardController _controller;
        private readonly AuthorizationOptions _authorizationOptions;

        public DashboardControllerTests()
        {
            _configurationMock = new Mock<IConfiguration>();
            _mediatorMock = new Mock<IMediator>();
            _userSessionMock = new Mock<IUserSession>();
            _optionsMock = new Mock<IOptions<AuthorizationOptions>>();
            _authorizationOptions = new AuthorizationOptions();

            // Setup default user session properties
            _userSessionMock.Setup(x => x.RoleId).Returns(4); // Super level
            _userSessionMock.Setup(x => x.CustId).Returns(123);
            _userSessionMock.Setup(x => x.Site).Returns("test-site");
            _userSessionMock.Setup(x => x.OnUserId).Returns(123);
            _userSessionMock.Setup(x => x.Currency).Returns("USD");
            _userSessionMock.Setup(x => x.Language).Returns("en");

            // Setup SubPageAccess with real instance
            var subPageAccess = new AlphaPlus.Core.Models.PageAccess();
            subPageAccess.SetFullPermission();
            _userSessionMock.Setup(x => x.SubPageAccess).Returns(subPageAccess);

            _optionsMock.Setup(x => x.Value).Returns(_authorizationOptions);

            _controller = new DashboardController(
                _configurationMock.Object,
                _mediatorMock.Object,
                _userSessionMock.Object,
                _optionsMock.Object);

            // Setup default HttpContext
            var httpContext = new DefaultHttpContext();
            _controller.ControllerContext = new ControllerContext
            {
                HttpContext = httpContext
            };
        }

        [Fact]
        public void Constructor_ShouldCreateControllerWithDependencies()
        {
            // Assert
            Assert.NotNull(_controller);
        }

        [Fact]
        public void Index_WhenUserIsInProtectedMode_ShouldReturnProtectedModeView()
        {
            // Arrange
            _userSessionMock.Setup(x => x.IsProtectedMode).Returns(true);

            // Act
            var result = _controller.Index();

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            Assert.Equal("~/Views/Dashboard/IndexProtectedMode.cshtml", viewResult.ViewName);
        }

        [Fact]
        public void Index_WhenUserIsNotInProtectedMode_ShouldCallIndex2()
        {
            // Arrange
            _userSessionMock.Setup(x => x.IsProtectedMode).Returns(false);

            // Act
            var result = _controller.Index();

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            Assert.IsType<DashboardViewModel>(viewResult.Model);
        }

        [Fact]
        public void Index2_ShouldReturnViewWithDashboardViewModel()
        {
            // Arrange
            const byte expectedRoleId = 4;
            _userSessionMock.Setup(x => x.RoleId).Returns(expectedRoleId);

            // Act
            var result = _controller.Index2();

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            var viewModel = Assert.IsType<DashboardViewModel>(viewResult.Model);
            Assert.Equal(Enumeration.FromValue<UserLevel>(expectedRoleId).DisplayName.ToLower(), viewModel.UserLevelName);
        }

        [Fact]
        public async Task BalanceInfo_ShouldReturnPartialViewWithBalanceInfoViewModel()
        {
            // Arrange
            var expectedBalanceInfoViewModel = new BalanceInfoViewModel(
                new BalanceInfo(),
                UserLevel.Super,
                "USD",
                123,
                true,
                _ => true);
            var expectedCashServiceInfoViewModel = new CashServiceInfoViewModel();

            _mediatorMock.Setup(x => x.Send(
                It.IsAny<BalanceInfoQuery>(),
                It.IsAny<CancellationToken>()))
                .ReturnsAsync(expectedBalanceInfoViewModel);

            _mediatorMock.Setup(x => x.Send(
                It.IsAny<GetCashServiceStatusQuery>(),
                It.IsAny<CancellationToken>()))
                .ReturnsAsync(expectedCashServiceInfoViewModel);

            // Act
            var result = await _controller.BalanceInfo();

            // Assert
            var partialViewResult = Assert.IsType<PartialViewResult>(result);
            var viewModel = Assert.IsType<BalanceInfoViewModel>(partialViewResult.Model);
            Assert.Equal(expectedCashServiceInfoViewModel, viewModel.CashServiceInfoViewModel);

            _mediatorMock.Verify(x => x.Send(
                It.Is<BalanceInfoQuery>(q => q.CustId == _userSessionMock.Object.CustId),
                It.IsAny<CancellationToken>()), Times.Once);

            _mediatorMock.Verify(x => x.Send(
                It.Is<GetCashServiceStatusQuery>(q => q.UserSession == _userSessionMock.Object),
                It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public void GetServerTime_WhenDuplicationLoginCheckEnabledAndOnUserIdMismatch_ShouldReturnSignOutJson()
        {
            // Arrange
            _authorizationOptions.DisabledCheckDuplicationLogin = false;
            _userSessionMock.Setup(x => x.OnUserId).Returns(123);
            const int onUserId = 456;

            // Act
            var result = _controller.GetServerTime(onUserId);

            // Assert
            var jsonResult = Assert.IsType<JsonResult>(result);
            dynamic resultValue = jsonResult.Value;
            Assert.True(resultValue.signOut);
        }

        [Fact]
        public void GetServerTime_WhenDuplicationLoginCheckEnabledAndOnUserIdIsZero_ShouldReturnSignOutJson()
        {
            // Arrange
            _authorizationOptions.DisabledCheckDuplicationLogin = false;
            _userSessionMock.Setup(x => x.OnUserId).Returns(0);

            // Act
            var result = _controller.GetServerTime();

            // Assert
            var jsonResult = Assert.IsType<JsonResult>(result);
            dynamic resultValue = jsonResult.Value;
            Assert.True(resultValue.signOut);
        }

        [Fact]
        public void GetServerTime_WhenDuplicationLoginCheckDisabled_ShouldReturnCurrentTimeJson()
        {
            // Arrange
            _authorizationOptions.DisabledCheckDuplicationLogin = true;
            _userSessionMock.Setup(x => x.OnUserId).Returns(123);

            // Act
            var result = _controller.GetServerTime(123);

            // Assert
            var jsonResult = Assert.IsType<JsonResult>(result);
            dynamic resultValue = jsonResult.Value;

            var now = DateTime.Now;
            Assert.Equal(now.Year, resultValue.Year);
            Assert.Equal(now.Month, resultValue.Month);
            Assert.Equal(now.Day, resultValue.Day);
            Assert.Equal(now.Hour, resultValue.Hour);
            Assert.Equal(now.Minute, resultValue.Minute);
            Assert.Equal(now.Second, resultValue.Second);
        }

        [Fact]
        public void GetServerTime_WhenDuplicationLoginCheckEnabledAndOnUserIdMatches_ShouldReturnCurrentTimeJson()
        {
            // Arrange
            _authorizationOptions.DisabledCheckDuplicationLogin = false;
            const int onUserId = 123;
            _userSessionMock.Setup(x => x.OnUserId).Returns(onUserId);

            // Act
            var result = _controller.GetServerTime(onUserId);

            // Assert
            var jsonResult = Assert.IsType<JsonResult>(result);
            dynamic resultValue = jsonResult.Value;

            var now = DateTime.Now;
            Assert.Equal(now.Year, resultValue.Year);
            Assert.Equal(now.Month, resultValue.Month);
            Assert.Equal(now.Day, resultValue.Day);
            Assert.Equal(now.Hour, resultValue.Hour);
            Assert.Equal(now.Minute, resultValue.Minute);
            Assert.Equal(now.Second, resultValue.Second);
        }

        [Fact]
        public async Task UpdateCashServiceStatus_ShouldSendCommandAndReturnResult()
        {
            // Arrange
            const bool isEnableCash = true;
            var expectedActionResult = new OkResult();

            _mediatorMock.Setup(x => x.Send(
                It.IsAny<UpdateCashServiceStatusCommand>(),
                It.IsAny<CancellationToken>()))
                .ReturnsAsync(expectedActionResult);

            // Act
            var result = await _controller.UpdateCashServiceStatus(isEnableCash);

            // Assert
            Assert.Equal(expectedActionResult, result);

            _mediatorMock.Verify(x => x.Send(
                It.Is<UpdateCashServiceStatusCommand>(cmd =>
                    cmd.IsEnableCash == isEnableCash &&
                    cmd.UserSession == _userSessionMock.Object),
                It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task BannerClickLog_ShouldSendRequestAndReturnJsonResult()
        {
            // Arrange
            const string bannerName = "TestBanner";
            const bool expectedResult = true;
            const byte expectedRoleId = 4;

            _userSessionMock.Setup(x => x.RoleId).Returns(expectedRoleId);
            _mediatorMock.Setup(x => x.Send(
                It.IsAny<BannerClickLogRequest>(),
                It.IsAny<CancellationToken>()))
                .ReturnsAsync(expectedResult);

            // Act
            var result = await _controller.BannerClickLog(bannerName);

            // Assert
            var jsonResult = Assert.IsType<JsonResult>(result);
            Assert.Equal(expectedResult, jsonResult.Value);

            _mediatorMock.Verify(x => x.Send(
                It.Is<BannerClickLogRequest>(req =>
                    req.BannerName == bannerName &&
                    req.RoleId == expectedRoleId),
                It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public void GetServerTime_WhenOnUserIdIsNegativeOne_ShouldReturnCurrentTimeJson()
        {
            // Arrange
            _authorizationOptions.DisabledCheckDuplicationLogin = false;
            _userSessionMock.Setup(x => x.OnUserId).Returns(123);
            const int onUserId = -1;

            // Act
            var result = _controller.GetServerTime(onUserId);

            // Assert
            var jsonResult = Assert.IsType<JsonResult>(result);
            dynamic resultValue = jsonResult.Value;

            var now = DateTime.Now;
            Assert.Equal(now.Year, resultValue.Year);
            Assert.Equal(now.Month, resultValue.Month);
            Assert.Equal(now.Day, resultValue.Day);
            Assert.Equal(now.Hour, resultValue.Hour);
            Assert.Equal(now.Minute, resultValue.Minute);
            Assert.Equal(now.Second, resultValue.Second);
        }
    }
}
