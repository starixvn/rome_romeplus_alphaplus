using AlphaPlus.PaymentService.API.Shared;
using AlphaPlus.PaymentService.API.Payments.Services;
using AlphaPlus.PaymentService.API.Payments.Requests;
namespace AlphaPlus.PaymentService.API.DirePay;

public static class DirePayEndpoints
{
    public static WebApplication RegisterDirePayEndpoints(this WebApplication app)
    {
        var group = app.MapGroup("/api/payments/direpay").WithTags("DirePay");
        group.MapPost("/deposit", ManualDeposit);
        group.MapPost("/deposit-callback", ManualDeposit);
        //.Produces<IEnumerable<int>>();

        group.MapPost("/payout", ([FromKeyedServices("DirePay")] IPaymentWithProviderService paymentWithProviderService, CreateManualWithdrawalRequest request) => Payout(paymentWithProviderService, request));

        return app;
    }

    static IResult ManualDeposit([FromKeyedServices("DirePay")] IPaymentService paymentService)
    {
        return Results.Ok(paymentService.Deposit());
    }

    static IResult Payout([FromKeyedServices("DirePay")] IPaymentWithProviderService paymentWithProviderService, CreateManualWithdrawalRequest request)
    {
        return Results.Ok(paymentWithProviderService.ProcessPayout(request));
    }
}