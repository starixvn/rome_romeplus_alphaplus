using AgencyCash.API.Payments.Requests;
using AgencyCash.API.Payments.Services;
using AgencyCash.API.Shared.Helpers;
using AgencyCash.API.Shared.Models;

namespace AgencyCash.API.Payments;

public static class PaymentsEndpoints
{
    public static void MapPaymentsEndpoints(this WebApplication app)
    {
        var deposit = app.MapGroup("/api/deposit");
        deposit
            .MapPost("/manual", ManualDeposit)
            .AddEndpointFilter<ModelValidationFilter<ManualDepositRequest>>()
            .Produces<Result>()
            .WithTags("Deposit");
        deposit
            .MapPost("/auto", AutoDeposit)
            .AddEndpointFilter<ModelValidationFilter<AutoDepositRequest>>()
            .Produces<Result>()
            .WithTags("Deposit");
    }

    private static async Task<IResult> ManualDeposit(ManualDepositRequest request, IDepositService depositService)
        => Results.Ok(await depositService.Manual(request));

    private static Task<IResult> AutoDeposit(AutoDepositRequest request, IDepositService depositService)
    {
        return Task.FromResult(Results.Ok(new Result(0, "")));
    }
}