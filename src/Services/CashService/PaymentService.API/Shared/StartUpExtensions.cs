using System.Diagnostics.CodeAnalysis;
using System.Net;
using AlphaPlus.PaymentService.API.Shared;
using AlphaPlus.PaymentService.API.U2Pay;
using AlphaPlus.PaymentService.API.DirePay;
using Fanex.Data.Repository;
using AlphaPlus.PaymentService.API.Payments.Services;
using Fanex.Data.SqlServer;

namespace AlphaPlus.PaymentService.API.Shared;

public static class StartUpExtensions
{
    public static IServiceCollection AddPaymentServices(this IServiceCollection services, IConfiguration configuration)
    {
        services
            .AddKeyedScoped<IPaymentService, U2PayPaymentService>("U2Pay")
            .AddHttpClient<IPaymentService, U2PayPaymentService>("http://localhost:5000/api/u2pay/");
        services
            .AddKeyedScoped<IPaymentService, DirePayPaymentService>("Dire")
            .AddHttpClient<U2PayPaymentService>(client =>
            {
                client.BaseAddress = new Uri("http://localhost:5000/api/dire/");
            });

        services
            .AddFanexSql(configuration)
            .AddSingleton<IDynamicRepository, DynamicRepository>()
            .AddSingleton<ISiteDBService, SiteDBService>()
            .AddKeyedSingleton<IWithdrawalService, WithdrawalService>("U2Pay")
            .AddKeyedSingleton<IWithdrawalService, WithdrawalService>("DirePay")
            .AddSingleton<IPaymentProviderService, U2PayProviderService>()
            .AddSingleton<IPaymentProviderService, DirePayProviderService>();

        return services;
    }

    private static IServiceCollection AddHttpClient<TClient, [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] TImplementation>(
        this IServiceCollection services,
        string baseUrl,
        int pooledConnectionLifetime = 15,
        WebProxy webProxy = null)
            where TClient : class
            where TImplementation : class, TClient
    {
        services
            .AddHttpClient<TClient, TImplementation>(httpClient => httpClient.BaseAddress = new Uri(baseUrl))
            .ConfigurePrimaryHttpMessageHandler(_ => new HttpClientHandler
            {
                AutomaticDecompression = DecompressionMethods.Deflate | DecompressionMethods.GZip
            })
            .ConfigurePrimaryHttpMessageHandler(_ => new SocketsHttpHandler
            {
                PooledConnectionLifetime = TimeSpan.FromMinutes(pooledConnectionLifetime)
            })
            .ConfigurePrimaryHttpMessageHandler(() => new HttpClientHandler
            {
                UseProxy = webProxy != null,
                Proxy = webProxy
            });

        return services;
    }

    public static void RegisterPaymentsEndpoints(this WebApplication app)
    {
        app
            .RegisterU2PayEndpoints()
            .RegisterDirePayEndpoints();
    }
}