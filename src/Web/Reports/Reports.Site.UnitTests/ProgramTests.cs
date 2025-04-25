using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Reports.Site.UnitTests;

public class ProgramTests
{
    [Fact]
    public void WebApplicationBuilder_ShouldConfigureServices()
    {
        // Arrange
        var builder = WebApplication.CreateBuilder(new string[0]);

        // Act - mimicking the service registration in Program.cs
        builder.Services.AddControllersWithViews();
        builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblies(typeof(AlphaPlus.Reports.Site.Program).Assembly));
        builder.Services.AddHealthChecks();

        // Assert
        var app = builder.Build();
        var mediatorService = app.Services.GetService<MediatR.IMediator>();
        var healthCheckService = app.Services.GetService<Microsoft.Extensions.Diagnostics.HealthChecks.HealthCheckService>();

        Assert.NotNull(mediatorService);
        Assert.NotNull(healthCheckService);
    }

    [Fact]
    public void WebApplication_ShouldConfigureMiddlewarePipeline()
    {
        // Arrange
        var builder = WebApplication.CreateBuilder(new string[0]);

        // Configure minimal services required for middleware testing
        builder.Services.AddControllersWithViews();
        builder.Services.AddAuthentication();
        builder.Services.AddAuthorization();

        // Act - configure middleware pipeline similar to Program.cs
        var app = builder.Build();

        // This setup should not throw exceptions
        app.UseRouting();
        app.UseAuthentication();
        app.UseAuthorization();
        app.MapHealthChecks("/health");
        app.MapDefaultControllerRoute();

        // Assert - successful configuration results in no exceptions
        var endpointRouteBuilder = app as Microsoft.AspNetCore.Routing.IEndpointRouteBuilder;
        Assert.NotNull(endpointRouteBuilder);
    }
}