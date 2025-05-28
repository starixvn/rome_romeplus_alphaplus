using System.Diagnostics.CodeAnalysis;
using System.Net;
using AlphaPlus.PaymentService.API.Shared;
using AlphaPlus.PaymentService.API.U2Pay;
using AlphaPlus.PaymentService.API.DirePay;
using AlphaPlus.PaymentService.API.Payments.Services;

namespace AlphaPlus.PaymentService.API.Shared;

public static class StartUpExtensions
{
    public static IServiceCollection AddPaymentServices(this IServiceCollection services, IConfiguration configuration)
    {
        services
            .AddKeyedSingleton<IPaymentWithProviderService, PaymentWithProviderService>("U2Pay")
            .AddKeyedSingleton<IPaymentWithProviderService, PaymentWithProviderService>("DirePay")
            .AddHttpClient<IPaymentProviderService, U2PayProviderService>("http://localhost:5000/api/u2pay/") // TODO: Change to configuration
            .AddHttpClient<IPaymentProviderService, DirePayProviderService>("http://localhost:5000/api/direpay/"); // TODO: Change to configuration

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