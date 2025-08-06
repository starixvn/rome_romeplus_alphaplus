using AlphaPlus.PaymentService.API.Payments.Requests;
using AlphaPlus.PaymentService.API.Payments.Services;
using AlphaPlus.PaymentService.API.Shared;
namespace AlphaPlus.PaymentService.API.DirePay;

public static class DirePayEndpoints
{
    public static WebApplication RegisterDirePayEndpoints(this WebApplication app)
    {
        var group = app.MapGroup("/api/payments/direpay").WithTags("DirePay");
        group.MapPost("/deposit", ManualDeposit);
        group.MapPost("/deposit-callback", ManualDeposit);

        group.MapPost("/payout", async ([FromKeyedServices("DirePay")] IPaymentWithProviderService paymentWithProviderService, CreateManualWithdrawalRequest request) =>
        {
            var result = await paymentWithProviderService.ProcessPayout(request);
            return Results.Ok(result);
        });

        return app;
    }

    static IResult ManualDeposit([FromKeyedServices("DirePay")] IPaymentService paymentService)
    {
        return Results.Ok(paymentService.Deposit());
    }
}