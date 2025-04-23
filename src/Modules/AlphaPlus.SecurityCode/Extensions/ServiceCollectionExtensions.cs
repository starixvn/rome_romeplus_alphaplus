using AlphaPlus.SecurityCode.Configurations;
using AlphaPlus.SecurityCode.Services;
using AlphaPlus.SecurityCode.Web;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace AlphaPlus.SecurityCode.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddSecurityCodeServices(
        this IServiceCollection services,
        IConfiguration configuration)
        => services
        .Configure<SecurityCodeOptions>(configuration.GetSection(SecurityCodeOptions.SecurityCode))
        .AddSingleton<ISecurityCodeHttpContext, SecurityCodeHttpContext>()
        .AddSingleton<ISecurityCodeChecker, SecurityCodeChecker>()
        .AddSingleton<ISecurityCodePathBuilder, SecurityCodePathBuilder>();
}