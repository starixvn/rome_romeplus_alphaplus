using AgencyCash.API.Payments.Models;
using AgencyCash.API.Payments.Requests;
using AgencyCash.API.Payments.Services;
using AgencyCash.API.Shared.Helpers;

namespace AgencyCash.API.Payments;

public static class PaymentsEndpoints
{
    public static void MapPaymentsEndpoints(this WebApplication app)
    {
        var deposit = app.MapGroup("/api/deposit");
        deposit
            .MapPost("/manual", ManualDeposit)
            .AddEndpointFilter<ModelValidationFilter<ManualDepositRequest>>()
            .Produces<PaymentResult>()
            .WithTags("Deposit");
        deposit
            .MapPost("/auto", AutoDeposit)
            .AddEndpointFilter<ModelValidationFilter<AutoDepositRequest>>()
            .Produces<PaymentResult>()
            .WithTags("Deposit");
    }

    private static async Task<IResult> ManualDeposit(ManualDepositRequest request, IDepositService depositService)
        => TypedResults.Ok(await depositService.Manual(request));

    private static async Task<IResult> AutoDeposit(AutoDepositRequest request, IDepositService depositService)
    {
        return await Task.FromResult(TypedResults.Ok(new PaymentResult(0, "", 123456789)));
    }
}