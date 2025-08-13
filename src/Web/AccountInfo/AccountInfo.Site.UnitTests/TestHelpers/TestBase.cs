using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Moq;

namespace AlphaPlus.AccountInfo.Site.UnitTests.TestHelpers;

public abstract class TestBase
{
    protected readonly Fixture Fixture;
    protected readonly Mock<IConfiguration> ConfigurationMock;
    protected readonly Mock<IServiceCollection> ServicesMock;

    protected TestBase()
    {
        Fixture = new Fixture();
        ConfigurationMock = new Mock<IConfiguration>();
        ServicesMock = new Mock<IServiceCollection>();
    }

    protected Mock<HttpContext> CreateMockHttpContext()
    {
        var httpContextMock = new Mock<HttpContext>();
        var httpRequestMock = new Mock<HttpRequest>();
        var httpResponseMock = new Mock<HttpResponse>();
        var serviceProviderMock = new Mock<IServiceProvider>();

        httpContextMock.Setup(x => x.Request).Returns(httpRequestMock.Object);
        httpContextMock.Setup(x => x.Response).Returns(httpResponseMock.Object);
        httpContextMock.Setup(x => x.RequestServices).Returns(serviceProviderMock.Object);

        return httpContextMock;
    }

    protected ControllerContext CreateControllerContext(Mock<HttpContext> httpContextMock)
    {
        return new ControllerContext
        {
            HttpContext = httpContextMock.Object
        };
    }

    protected IServiceCollection CreateServiceCollection()
    {
        return new ServiceCollection();
    }

    protected IServiceProvider CreateServiceProvider(IServiceCollection services)
    {
        return services.BuildServiceProvider();
    }

    protected Mock<T> CreateMock<T>() where T : class
    {
        return new Mock<T>();
    }

    protected T CreateInstance<T>() where T : class
    {
        return Fixture.Create<T>();
    }
}
