using AlphaPlus.Reports.Features.Products;
using AlphaPlus.Reports.Features.WinLoss;
using AlphaPlus.Reports.Features.WinLossAnalysis;
using AlphaPlus.Reports.Features.WinLossDetail;
using AlphaPlus.Reports.Site.Features.Shared.Extensions;
using AlphaPlus.Reports.Site.Features.WinLoss;
using AlphaPlus.Reports.Site.Features.WinLossByProduct;
using AlphaPlus.Reports.Site.Features.WinLossDetail;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Reports.Site.UnitTests.Features.Shared.Extensions;

public class ServiceCollectionExtensionsTests
{
    private readonly IServiceCollection _services;
    private readonly IConfiguration _configuration;

    public ServiceCollectionExtensionsTests()
    {
        _services = new ServiceCollection();
        _configuration = new ConfigurationBuilder().Build();
    }

    [Fact]
    public void AddApplicationServices_ShouldRegisterRequiredServices()
    {
        // Act
        _services.AddApplicationServices(_configuration);
        var serviceProvider = _services.BuildServiceProvider();

        // Assert - verify that key services are registered
        var productService = serviceProvider.GetService<IProductService>();
        Assert.NotNull(productService);

        var winLossService = serviceProvider.GetService<IWinLossService>();
        Assert.NotNull(winLossService);

        var winLossAnalysisService = serviceProvider.GetService<IWinLossAnalysisService>();
        Assert.NotNull(winLossAnalysisService);

        var winLossDetailService = serviceProvider.GetService<IWinLossDetailService>();
        Assert.NotNull(winLossDetailService);
    }

    [Fact]
    public void AddApplicationServices_ShouldRegisterWorkbooks()
    {
        // Act
        _services.AddApplicationServices(_configuration);
        var serviceProvider = _services.BuildServiceProvider();

        // Assert - verify that workbook implementations are registered
        var winLossWorkbook = serviceProvider.GetService<IWinLossWorkbook>();
        Assert.NotNull(winLossWorkbook);

        var winLossByProductWorkbook = serviceProvider.GetService<IWinLossByProductWorkbook>();
        Assert.NotNull(winLossByProductWorkbook);

        var winLossDetailWorkbook = serviceProvider.GetService<IWinLossDetailWorkbook>();
        Assert.NotNull(winLossDetailWorkbook);
    }
}