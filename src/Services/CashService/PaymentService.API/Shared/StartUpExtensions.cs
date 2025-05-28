using System.Diagnostics.CodeAnalysis;
using System.Net;
using AlphaPlus.PaymentService.API.Shared;
using AlphaPlus.PaymentService.API.U2Pay;
using AlphaPlus.PaymentService.API.DirePay;
using AlphaPlus.PaymentService.API.Payments.Services;
using AlphaPlus.PaymentService.API.Payments.Models;

namespace AlphaPlus.PaymentService.API.Shared;

public static class StartUpExtensions
{
    public static IServiceCollection AddPaymentServices(this IServiceCollection services, IConfiguration configuration)
    {
        var paymentProviderConfig = configuration.GetSection("PaymentProviderConfig").Get<PaymentProviderConfig>();
        services
            .AddSingleton(paymentProviderConfig)
            .AddKeyedSingleton<IPaymentWithProviderService, PaymentWithProviderService>("U2Pay")
            .AddKeyedSingleton<IPaymentWithProviderService, PaymentWithProviderService>("DirePay")
            .AddHttpClient<IPaymentProviderService, U2PayProviderService>(paymentProviderConfig.BaseUrl)
            .AddHttpClient<IPaymentProviderService, DirePayProviderService>(paymentProviderConfig.BaseUrl);

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