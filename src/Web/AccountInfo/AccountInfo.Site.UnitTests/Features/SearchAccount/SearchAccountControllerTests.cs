using AlphaPlus.AccountInfo.Site.Features.SearchAccount;
using AlphaPlus.AccountInfo.Site.Features.SearchAccount.GetAccount;
using AlphaPlus.AccountInfo.Site.Features.SearchAccount.GetAccount.CustomerStatus;
using AlphaPlus.AccountInfo.Site.Features.SearchAccount.GetAccount.DonwlineList;
using AlphaPlus.AccountInfo.Site.Features.SearchAccount.GetAccount.GivenCredit.Get;
using AlphaPlus.AccountInfo.Site.Features.SearchAccount.GetAccount.GivenCredit.Update;
using AlphaPlus.AccountInfo.Site.Features.SearchAccount.GetAccount.OnlineList;
using AlphaPlus.AccountInfo.Site.Features.SearchAccount.GetAccount.Transfer.Requests;
using AlphaPlus.AccountInfo.Site.Features.SearchAccount.Transfer.Requests;
using AlphaPlus.Shared.AppResources.Common;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace AlphaPlus.AccountInfo.Site.UnitTests.Features.SearchAccount;

public class SearchAccountControllerTests
{
    private readonly Mock<IMediator> _mediatorMock;
    private readonly Mock<HttpContext> _httpContextMock;
    private readonly Mock<HttpRequest> _httpRequestMock;
    private readonly Mock<HttpResponse> _httpResponseMock;
    private readonly Mock<IServiceProvider> _serviceProviderMock;
    private readonly SearchAccountController _controller;
    private readonly Fixture _fixture;

    public SearchAccountControllerTests()
    {
        _mediatorMock = new Mock<IMediator>();
        _httpContextMock = new Mock<HttpContext>();
        _httpRequestMock = new Mock<HttpRequest>();
        _httpResponseMock = new Mock<HttpResponse>();
        _serviceProviderMock = new Mock<IServiceProvider>();
        _fixture = new Fixture();

        _httpContextMock.Setup(x => x.Request).Returns(_httpRequestMock.Object);
        _httpContextMock.Setup(x => x.Response).Returns(_httpResponseMock.Object);
        _httpContextMock.Setup(x => x.RequestServices).Returns(_serviceProviderMock.Object);

        _controller = new SearchAccountController(_mediatorMock.Object)
        {
            ControllerContext = new ControllerContext
            {
                HttpContext = _httpContextMock.Object
            }
        };
    }

    [Fact]
    public void Constructor_ShouldSetMediator()
    {
        // Act
        var controller = new SearchAccountController(_mediatorMock.Object);

        // Assert
        Assert.NotNull(controller);
        Assert.NotNull(_mediatorMock.Object);
    }

    [Fact]
    public void Controller_ShouldInheritFromSiteBaseController()
    {
        // Act
        var controller = new SearchAccountController(_mediatorMock.Object);

        // Assert
        Assert.IsAssignableFrom<AlphaPlus.AccountInfo.Site.Features.Shared.SiteBaseController>(controller);
    }
}
