using AlphaPlus.PaymentService.API.Shared;
using AlphaPlus.PaymentService.API.Payments.Services;
using AlphaPlus.PaymentService.API.Payments.Requests;
using Microsoft.AspNetCore.Mvc;

namespace AlphaPlus.PaymentService.API.U2Pay;

public static class U2PayEndpoints
{
    public static WebApplication RegisterU2PayEndpoints(this WebApplication app)
    {
        var group = app.MapGroup("/api/payments/u2pay").WithTags("U2Pay");
        group.MapPost("/deposit", ManualDeposit);
        group.MapPost("/deposit-callback", ManualDeposit);
        //.Produces<IEnumerable<int>>();

        group.MapPost("/payout", async ([FromKeyedServices("U2Pay")] IPaymentWithProviderService paymentWithProviderService, CreateManualWithdrawalRequest request) => 
        {
            var result = await paymentWithProviderService.ProcessPayout(request);
            return Results.Ok(result);
        });

        return app;
    }

    static IResult ManualDeposit([FromKeyedServices("U2Pay")] IPaymentService paymentService)
    {
        return Results.Ok(paymentService.Deposit());
    }
}