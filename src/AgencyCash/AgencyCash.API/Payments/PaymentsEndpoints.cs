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

        var withdrawal = app.MapGroup("/api/withdrawal");
        withdrawal
            .MapPost("/manual", ManualWithdrawal)
            .Produces<Result>()
            .WithTags("Payments")
            .WithName("Manual Withdrawal");

        withdrawal.MapPost("/auto", AutoWithdrawal)
            .Produces<Result>()
            .WithTags("Payments")
            .WithName("Automatic Withdrawal");
    }

    private static async Task<IResult> ManualDeposit(ManualDepositRequest request, IDepositService depositService)
        => Results.Ok(await depositService.Manual(request));

    private static Task<IResult> AutoDeposit(AutoDepositRequest request, IDepositService depositService)
    {
        return Task.FromResult(Results.Ok(new PaymentResult(0, "", 123456789)));
    }

    private static async Task<IResult> ManualWithdrawal(CreateManualWithdrawalRequest request, IWithdrawalService withdrawalService)
        => Results.Ok(await withdrawalService.CreateManualWithdrawal(request));

    private static async Task<IResult> AutoWithdrawal(CreateAutoWithdrawalRequest request, IWithdrawalService withdrawalService)
        => Results.Ok(await withdrawalService.CreateAutoWithdrawal(request));
}