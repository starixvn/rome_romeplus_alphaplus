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

        group.MapPost("/payout", (IPaymentWithProviderService withdrawalService, CreateManualWithdrawalRequest request) => Payout(withdrawalService, request));

        return app;
    }

    static IResult ManualDeposit([FromKeyedServices("DirePay")] IPaymentService paymentService)
    {
        return Results.Ok(paymentService.Deposit());
    }

    static IResult Payout(IPaymentWithProviderService withdrawalService, CreateManualWithdrawalRequest request)
    {
        return Results.Ok(withdrawalService.ProcessPayout(request));
    }
}