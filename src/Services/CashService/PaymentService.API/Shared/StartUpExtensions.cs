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
            .AddSingleton<Fanex.Logging.ILogger, Fanex.Logging.Logger>()
            .AddSingleton(paymentProviderConfig)
            .AddKeyedSingleton<IPaymentWithProviderService, PaymentWithProviderService>("U2Pay")
            .AddKeyedSingleton<IPaymentWithProviderService, PaymentWithProviderService>("DirePay");

        services.AddHttpClient<IPaymentProviderService, U2PayProviderService>(client => client.BaseAddress = new Uri(paymentProviderConfig.U2Pay.BaseUrl));
        services.AddHttpClient<IPaymentProviderService, DirePayProviderService>(client => client.BaseAddress = new Uri(paymentProviderConfig.DirePay.BaseUrl));

        return services;
    }

    public static void RegisterPaymentsEndpoints(this WebApplication app)
    {
        app
            .RegisterU2PayEndpoints()
            .RegisterDirePayEndpoints();
    }
}