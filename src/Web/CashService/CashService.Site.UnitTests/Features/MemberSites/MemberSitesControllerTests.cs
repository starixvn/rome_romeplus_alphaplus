using System.Net;
using System.Net.Http.Json;
using AlphaPlus.CashService.Site.Features.MemberSites;
using AlphaPlus.CashService.Site.Features.MemberSites.Models;
using AlphaPlus.CashService.Site.Features.MemberSites.ViewModels;
using AlphaPlus.CashService.Site.Features.Shared.Models;
using AlphaPlus.Core.Models;
using Fanex.Customer.Client.Services;
using Microsoft.AspNetCore.Antiforgery;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Moq;
using Xunit;

namespace AlphaPlus.CashService.Site.UnitTests.Features.MemberSites
{
    public class MemberSitesControllerTests
    {
        private readonly Mock<IConfiguration> _mockConfig = new();
        private readonly Mock<IUserSession> _mockUserSession = new();
        private readonly Mock<IAntiforgery> _mockAntiforgery = new();
        private readonly Mock<ICustomerService> _mockCustomerService = new();
        private readonly MemberSitesController _controller;

        public MemberSitesControllerTests()
        {
            _mockConfig.Setup(c => c["CashService:APIUrl"]).Returns("http://api");
            _mockUserSession.SetupGet(u => u.DarkModeName).Returns("dark");
            _mockUserSession.SetupGet(u => u.CustId).Returns(1);
            _mockUserSession.SetupGet(u => u.Site).Returns("site");
            _mockUserSession.SetupGet(u => u.IsInternal).Returns(false);
            _mockUserSession.SetupGet(u => u.UserId).Returns(2);
            _mockUserSession.SetupGet(u => u.SubAccId).Returns(3);
            _mockUserSession.SetupGet(u => u.UserLoginIp).Returns("127.0.0.1");
            _mockUserSession.SetupGet(u => u.LangKey).Returns("en");
            _mockUserSession.SetupGet(u => u.CustDBGroupId).Returns(0);

            _controller = new MemberSitesController(
                _mockConfig.Object,
                _mockUserSession.Object,
                new HttpClient(new MockHttpMessageHandler()),
                _mockAntiforgery.Object,
                _mockCustomerService.Object
            );
        }

        [Fact]
        public void Index_ReturnsViewWithModel()
        {
            _mockCustomerService.Setup(s => s.GetCashServiceStatus(1)).ReturnsAsync(1);
            var result = _controller.Index();
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<MemberSiteBaseViewModel>(viewResult.Model);
            Assert.Equal("dark", model.DarkModeName);
            Assert.Equal(1, model.IsCashServiceEnabled);
        }

        [Fact]
        public async Task MemberSiteManagement_Get_ReturnsOkWithResponse()
        {
            var model = new MemberSiteManagementModel();
            var controller = new MemberSitesController(
                _mockConfig.Object,
                _mockUserSession.Object,
                new HttpClient(new MockHttpMessageHandler(model)),
                _mockAntiforgery.Object,
                _mockCustomerService.Object
            );

            var result = await controller.MemberSiteManagement(CancellationToken.None);
            var okResult = Assert.IsType<OkObjectResult>(result);
            var response = Assert.IsType<MemberSiteManagementModel>(okResult.Value);
        }

        [Fact]
        public async Task MemberSiteManagement_Put_InternalUser_ReturnsUnauthorized()
        {
            _mockUserSession.SetupGet(u => u.IsInternal).Returns(true);
            var model = new UpdateMemberSiteManagementModel();
            var result = await _controller.MemberSiteManagement(model, CancellationToken.None);
            Assert.IsType<UnauthorizedResult>(result);
        }

        [Fact]
        public async Task MemberSiteManagement_Put_CashServiceDisabled_Returns401Json()
        {
            _mockCustomerService.Setup(s => s.GetCashServiceStatus(1)).ReturnsAsync(0);
            var model = new UpdateMemberSiteManagementModel();
            var result = await _controller.MemberSiteManagement(model, CancellationToken.None);
            var jsonResult = Assert.IsType<JsonResult>(result);
            var resultModel = Assert.IsType<ResultModel>(jsonResult.Value);
            Assert.Equal(401, resultModel.Code);
        }

        [Fact]
        public async Task MemberSiteManagement_Put_InvalidModel_ReturnsBadRequest()
        {
            _mockCustomerService.Setup(s => s.GetCashServiceStatus(1)).ReturnsAsync(1);
            _controller.ModelState.AddModelError("Test", "Error");
            var model = new UpdateMemberSiteManagementModel();
            var result = await _controller.MemberSiteManagement(model, CancellationToken.None);
            Assert.IsType<BadRequestObjectResult>(result);
        }

        [Fact]
        public async Task MemberSiteManagement_Put_ValidModel_ReturnsOkWithResult()
        {
            _mockCustomerService.Setup(s => s.GetCashServiceStatus(1)).ReturnsAsync(1);
            var handler = new MockHttpMessageHandler(new Result { Code = 200 });
            var controller = new MemberSitesController(
                _mockConfig.Object,
                _mockUserSession.Object,
                new HttpClient(handler),
                _mockAntiforgery.Object,
                _mockCustomerService.Object
            );
            var model = new UpdateMemberSiteManagementModel();
            var result = await controller.MemberSiteManagement(model, CancellationToken.None);
            var okResult = Assert.IsType<OkObjectResult>(result);
            var resultValue = Assert.IsType<Result>(okResult.Value);
            Assert.Equal(200, resultValue.Code);
        }

        // Helper mock handler for HttpClient
        private class MockHttpMessageHandler : HttpMessageHandler
        {
            private readonly object _response;
            public MockHttpMessageHandler(object response = null) => _response = response;
            protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
            {
                var msg = new HttpResponseMessage(HttpStatusCode.OK);
                if (_response != null)
                {
                    msg.Content = JsonContent.Create(_response);
                }
                return Task.FromResult(msg);
            }
        }
    }
}